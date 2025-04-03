using ikloud_Aflatoon.Infrastructure;
using ikloud_Aflatoon.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ikloud_Aflatoon.Controllers
{
    public class IW_QuerySearch_ArchivalController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        AflatoonEntities af = new AflatoonEntities();
        IWProcDataContext iwpro = new IWProcDataContext();
        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["IkloudPro_ImageArchival"].ConnectionString);

        private void logerror(string errormsg, string errordesc)
        {
            ErrorDisplay er = new ErrorDisplay();
            string ServerPath = "";
            string filename = "";
            string fileNameWithPath = "";

            //-------------------------------- ServerPath = Server.MapPath("~/Logs/");----
            ServerPath = Server.MapPath("~/Logs/");
            if (System.IO.Directory.Exists(ServerPath) == false)
            {
                System.IO.Directory.CreateDirectory(ServerPath);
            }
            filename = DateTime.Now.ToString("ddMMyyyy") + "Logs.txt";
            fileNameWithPath = ServerPath + filename;
            System.IO.StreamWriter str = new System.IO.StreamWriter(fileNameWithPath, true, System.Text.Encoding.Default);
            //  str.WriteLine("hello");
            str.WriteLine(DateTime.Now.ToShortTimeString() + ": Exception: " + errormsg);
            str.WriteLine(DateTime.Now.ToShortTimeString() + ": StackTrace: " + errordesc);
            str.Close();
        }

        //
        // GET: /IW_QuerySearch_Archival/

        public ActionResult Index()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["Query"] == false)
            {

                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            Session["glob"] = null;
            return View();
        }

        [HttpPost]
        public PartialViewResult QuerySearch(string ProcessingDate = null, string ToProcessingDate = null, string XMLSerialNo = null, string XMLAmount = null,
            string AccountNo = null, string XMLPayorBankRoutNo = null, string XMLTrns = null, string clrtype = null, string P2f = null, string EOD = null, string DbYear = null)
        {
            try
            {
                logerror("In SearchIndex function", "In SearchIndex function" + " - >");
                int custID = Convert.ToInt16(Session["CustomerID"]);
                string strFromDate = null, strToDate = null, strCheqNo = null, strSortCode = null, strTransCode = null, strAccountno = null;
                decimal amount = 0; string cleraringtype = null;

                if (ProcessingDate != null && ProcessingDate != "")
                    strFromDate = ProcessingDate.ToString();
                if (ToProcessingDate != null && ToProcessingDate != "")
                    strToDate = ToProcessingDate.ToString();
                if (XMLSerialNo != null && XMLSerialNo != "")
                    strCheqNo = XMLSerialNo.ToString();
                if (XMLAmount != null && XMLAmount != "")
                    amount = Convert.ToDecimal(XMLAmount.ToString());
                else
                    amount = 0;
                if (AccountNo != null && AccountNo != "")
                    strAccountno = AccountNo.ToString();
                if (XMLPayorBankRoutNo != null && XMLPayorBankRoutNo != "")
                    strSortCode = XMLPayorBankRoutNo.ToString();
                if (XMLTrns != null && XMLTrns != "")
                    strTransCode = XMLTrns.ToString();
                if (clrtype != null && clrtype != "")
                {
                    cleraringtype = clrtype.ToString();
                    if (cleraringtype == "CTS")
                        cleraringtype = "01";
                    else if (cleraringtype == "Non-CTS")
                        cleraringtype = "11";
                    else if (cleraringtype == "SPECIAL")
                        cleraringtype = "99";
                    else
                        cleraringtype = null;
                }

                if (strFromDate != null)
                {
                    //DateTime Totempdate = new DateTime();
                    //DateTime tempdate = Convert.ToDateTime(strFromDate);
                    //if (strToDate != null)
                    //{
                    //    Totempdate = Convert.ToDateTime(strToDate);
                    //}

                    DateTime Totempdate = new DateTime();
                    string tempDateFrom = strFromDate.Substring(6, 4) + "-" + strFromDate.Substring(3, 2) + "-" + strFromDate.Substring(0, 2);
                    string tempDateTo = "";
                    DateTime tempdate = Convert.ToDateTime(tempDateFrom);
                    if (strToDate != null)
                    {
                        tempDateTo = strToDate.Substring(6, 4) + "-" + strToDate.Substring(3, 2) + "-" + strToDate.Substring(0, 2);
                        Totempdate = Convert.ToDateTime(tempDateTo);
                    }

                    if (EOD == "true")
                    {
                        logerror("In SearchIndex EOD function", "In SearchIndex EOD function" + " - >");

                        //var iwsr = (from r in iwpro.EOD_IWFinalMainTransactions
                        //            where (r.ProcessingDate >= tempdate) && (r.ProcessingDate <= Totempdate || Totempdate == null) && (r.CustomerId == custID) && (r.XMLSerialNo == strCheqNo || strCheqNo == null) &&
                        //            (r.XMLAmount == amount || amount == 0) && (r.DbtAccNo == strAccountno || strAccountno == null) && (r.XMLPayorBankRoutNo == strSortCode || strSortCode == null)
                        //            && (r.XMLTransCode == strTransCode || strTransCode == null) && (P2f == "true" ? r.DocType == "C" : r.DocType == r.DocType)
                        //            && (r.ClearingType == cleraringtype || cleraringtype == null)
                        //            select new IWSearch
                        //            {
                        //                ID = r.ID,
                        //                XMLSerialNo = r.XMLSerialNo,
                        //                XMLAmount = r.XMLAmount,
                        //                XMLPayeeName = r.XMLPayeeName,
                        //                XMLPayorBankRoutNo = r.XMLPayorBankRoutNo,
                        //                XMLSAN = r.XMLSAN,
                        //                XMLTrns = r.XMLTransCode,
                        //                FrontGreyImagePath = r.FrontGreyImagePath.ToString(),
                        //                chiStatus = r.CHIStatus,
                        //                P2F = r.DocType,
                        //                AccountNo = r.DbtAccNo,
                        //                MiscStatus = r.MiscStatus

                        //            }).ToList();

                        var ObjList = new List<IWSearch>();
                        string connectionString = GetConnectionString(DbYear);
                        using (SqlConnection con = new SqlConnection(connectionString))
                        {
                            SqlDataAdapter adp = new SqlDataAdapter("SP_IwSearch_EOD", con);
                            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                            adp.SelectCommand.Parameters.Add("@FromDate", SqlDbType.VarChar).Value = tempDateFrom;
                            adp.SelectCommand.Parameters.Add("@ToDate", SqlDbType.VarChar).Value = tempDateTo;
                            adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = custID;
                            adp.SelectCommand.Parameters.Add("@xmlSerialNo", SqlDbType.VarChar).Value = strCheqNo;
                            //if (amount != 0)
                            adp.SelectCommand.Parameters.Add("@xmlAmount", SqlDbType.Decimal).Value = amount;
                            adp.SelectCommand.Parameters.Add("@DbtAccNo", SqlDbType.VarChar).Value = strAccountno;
                            adp.SelectCommand.Parameters.Add("@XMLPayorBankRoutNo", SqlDbType.VarChar).Value = strSortCode;
                            adp.SelectCommand.Parameters.Add("@XMLTransCode", SqlDbType.VarChar).Value = strTransCode;

                            if (P2f == "true")
                            {
                                adp.SelectCommand.Parameters.Add("@DocType", SqlDbType.VarChar).Value = "C";
                            }
                            adp.SelectCommand.Parameters.Add("@ClearingType", SqlDbType.VarChar).Value = cleraringtype;

                            DataSet ds = new DataSet();
                            adp.Fill(ds);

                            IWSearch def;

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                int count = ds.Tables[0].Rows.Count;
                                for (int index = 0; index < count; index++)
                                {
                                    def = new IWSearch
                                    {
                                        ID = Convert.ToInt64(ds.Tables[0].Rows[index]["ID"]),
                                        XMLSerialNo = Convert.ToString(ds.Tables[0].Rows[index]["XMLSerialNo"]),
                                        XMLAmount = Convert.ToDecimal(ds.Tables[0].Rows[index]["XMLAmount"]),
                                        XMLPayeeName = Convert.ToString(ds.Tables[0].Rows[index]["XMLPayeeName"] == null ? "" : ds.Tables[0].Rows[index]["XMLPayeeName"]),
                                        XMLPayorBankRoutNo = Convert.ToString(ds.Tables[0].Rows[index]["XMLPayorBankRoutNo"]),
                                        XMLSAN = Convert.ToString(ds.Tables[0].Rows[index]["XMLSAN"]),
                                        XMLTrns = Convert.ToString(ds.Tables[0].Rows[index]["XMLTransCode"]),
                                        FrontGreyImagePath = Convert.ToString(ds.Tables[0].Rows[index]["FrontGreyImagePath"]),
                                        chiStatus = Convert.ToString(ds.Tables[0].Rows[index]["CHIStatus"] == null ? "" : ds.Tables[0].Rows[index]["CHIStatus"]),
                                        P2F = Convert.ToString(ds.Tables[0].Rows[index]["DocType"] == null ? "" : ds.Tables[0].Rows[index]["DocType"]),
                                        AccountNo = Convert.ToString(ds.Tables[0].Rows[index]["DbtAccNo"] == null ? "" : ds.Tables[0].Rows[index]["DbtAccNo"]),
                                        MiscStatus = Convert.ToInt32(ds.Tables[0].Rows[index]["MiscStatus"]),

                                    };
                                    //logerror("Loop", index.ToString() + "loop");

                                    ObjList.Add(def);
                                }
                            }
                        }

                        return PartialView("_SearchCheques", ObjList);
                    }
                    else
                    {
                        logerror("In SearchIndex EOD else function", "In SearchIndex EOD else function" + " - >");
                        logerror(tempdate.ToString(), tempdate.ToString() + " TempDate - >");
                        logerror(Totempdate.ToString(), Totempdate.ToString() + " ToTempDate - >");
                        //var iwsr = (from r in af.IWFinalMainTransactions
                        //            where (r.ProcessingDate >= tempdate) && (r.ProcessingDate <= Totempdate || Totempdate == null) && (r.CustomerId == custID) && (r.XMLSerialNo == strCheqNo || strCheqNo == null) &&
                        //            (r.XMLAmount == amount || amount == 0) && (r.DbtAccNo == strAccountno || strAccountno == null) && (r.XMLPayorBankRoutNo == strSortCode || strSortCode == null)
                        //            && (r.XMLTransCode == strTransCode || strTransCode == null) && (P2f == "true" ? r.DocType == "C" : r.DocType == r.DocType)
                        //            && (r.ClearingType == cleraringtype || cleraringtype == null)
                        //            select new IWSearch
                        //            {
                        //                ID = r.ID,
                        //                XMLSerialNo = r.XMLSerialNo,
                        //                XMLAmount = r.XMLAmount,
                        //                XMLPayeeName = r.XMLPayeeName,
                        //                XMLPayorBankRoutNo = r.XMLPayorBankRoutNo,
                        //                XMLSAN = r.XMLSAN,
                        //                XMLTrns = r.XMLTransCode,
                        //                FrontGreyImagePath = r.FrontGreyImagePath.ToString(),
                        //                chiStatus = r.CHIStatus,
                        //                P2F = r.DocType,
                        //                AccountNo = r.DbtAccNo,
                        //                MiscStatus = r.MiscStatus

                        //            }).ToList();
                        //logerror(iwsr.Count.ToString(), iwsr.Count.ToString() + " iwsr count - >");

                        var ObjList = new List<IWSearch>();
                        string connectionString = GetConnectionString(DbYear);
                        using (SqlConnection con = new SqlConnection(connectionString))
                        {
                            SqlDataAdapter adp = new SqlDataAdapter("SP_IwSearch", con);
                            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                            adp.SelectCommand.Parameters.Add("@FromDate", SqlDbType.VarChar).Value = tempDateFrom;
                            adp.SelectCommand.Parameters.Add("@ToDate", SqlDbType.VarChar).Value = tempDateTo;
                            adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = custID;
                            adp.SelectCommand.Parameters.Add("@xmlSerialNo", SqlDbType.VarChar).Value = strCheqNo;
                            //if (amount != 0)
                            adp.SelectCommand.Parameters.Add("@xmlAmount", SqlDbType.Decimal).Value = amount;
                            adp.SelectCommand.Parameters.Add("@DbtAccNo", SqlDbType.VarChar).Value = strAccountno;
                            adp.SelectCommand.Parameters.Add("@XMLPayorBankRoutNo", SqlDbType.VarChar).Value = strSortCode;
                            adp.SelectCommand.Parameters.Add("@XMLTransCode", SqlDbType.VarChar).Value = strTransCode;

                            if (P2f == "true")
                            {
                                adp.SelectCommand.Parameters.Add("@DocType", SqlDbType.VarChar).Value = "C";
                            }
                            adp.SelectCommand.Parameters.Add("@ClearingType", SqlDbType.VarChar).Value = cleraringtype;

                            DataSet ds = new DataSet();
                            adp.Fill(ds);
                            
                            IWSearch def;

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                int count = ds.Tables[0].Rows.Count;
                                for (int index = 0; index < count; index++)
                                {
                                    def = new IWSearch
                                    {
                                        ID = Convert.ToInt64(ds.Tables[0].Rows[index]["ID"]),
                                        XMLSerialNo = Convert.ToString(ds.Tables[0].Rows[index]["XMLSerialNo"]),
                                        XMLAmount = Convert.ToDecimal(ds.Tables[0].Rows[index]["XMLAmount"]),
                                        XMLPayeeName = Convert.ToString(ds.Tables[0].Rows[index]["XMLPayeeName"] == null ? "" : ds.Tables[0].Rows[index]["XMLPayeeName"]),
                                        XMLPayorBankRoutNo = Convert.ToString(ds.Tables[0].Rows[index]["XMLPayorBankRoutNo"]),
                                        XMLSAN = Convert.ToString(ds.Tables[0].Rows[index]["XMLSAN"]),
                                        XMLTrns = Convert.ToString(ds.Tables[0].Rows[index]["XMLTransCode"]),
                                        FrontGreyImagePath = Convert.ToString(ds.Tables[0].Rows[index]["FrontGreyImagePath"]),
                                        chiStatus = Convert.ToString(ds.Tables[0].Rows[index]["CHIStatus"] == null ? "" : ds.Tables[0].Rows[index]["CHIStatus"]),
                                        P2F = Convert.ToString(ds.Tables[0].Rows[index]["DocType"] == null ? "" : ds.Tables[0].Rows[index]["DocType"]),
                                        AccountNo = Convert.ToString(ds.Tables[0].Rows[index]["DbtAccNo"] == null ? "" : ds.Tables[0].Rows[index]["DbtAccNo"]),
                                        MiscStatus = Convert.ToInt32(ds.Tables[0].Rows[index]["MiscStatus"]),

                                    };
                                    //logerror("Loop", index.ToString() + "loop");

                                    ObjList.Add(def);
                                }
                            }
                        }
                            
                        return PartialView("_SearchCheques", ObjList);
                    }
                }
                else
                {
                    return PartialView("_SearchCheques");
                }
            }
            catch (Exception e)
            {
                ErrorDisplay er = new ErrorDisplay();
                er.ErrorMessage = e.Message.ToString();
                return PartialView("Error", er);
            }
        }

        [HttpPost]
        public ActionResult MarkRtn(Int64 id, string actn = null, string npcirtncd = null, string rtnrjctdescrn = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            try
            {
                if ((bool)Session["Query"] == false)
                {
                    int uid1 = (int)Session["uid"];
                    UserMaster usrm = af.UserMasters.Find(uid1);
                    usrm.Active = false;
                    af.SaveChanges();
                    return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
                }
                int? updateFlg = 0;
                if (actn == "M" && npcirtncd.Length < 2)
                    npcirtncd = npcirtncd.PadLeft(2, '0');
                iwpro.IWQueryRntMark(id, npcirtncd, (int)Session["uid"], actn, ref updateFlg, rtnrjctdescrn);
                return Json(true);
            }
            catch (Exception e)
            {
                //Server.MapPath(strMappath);
                // ErrorDisplay er = new ErrorDisplay();
                //er.ErrorMessage = e.Message.ToString();
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
                //return View("Error", er);
            }
        }

        [HttpPost]
        public PartialViewResult RejectReason()
        {
            var rjrs = (from r in af.ItemReturnReasons
                        select new RejectReason
                        {
                            Description = r.DESCRIPTION,
                            ReasonCodeS = r.RETURN_REASON_CODE
                        });
            return PartialView("_RejectDetails", rjrs);
        }

        [HttpPost]
        public PartialViewResult ShowData(Int64 id, string EOD = null, string DbYear = null)
        {
            try
            {
                string connectionString = GetConnectionString(DbYear);
                SqlConnection con = new SqlConnection(connectionString);

                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter("GetCheque", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@id", SqlDbType.BigInt).Value = id;
                adp.SelectCommand.Parameters.Add("@EOD", SqlDbType.NVarChar).Value = EOD;
                ////--------------------Customer Selection---------------------
                //adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);


                //// adp.SelectCommand.Parameters.Add("@ChqNo", SqlDbType.NVarChar).Value = iwsrch.XMLSerialNo;
                //adp.SelectCommand.Parameters.Add("@Amount", SqlDbType.Float).Value = iwsrch.XMLAmount;
                adp.Fill(ds);
                IWSearch iwsr = null;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        iwsr = new IWSearch
                        {
                            ID = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[0]),
                            XMLSerialNo = ds.Tables[0].Rows[index].ItemArray[1].ToString(),
                            XMLAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[2].ToString()),
                            XMLPayeeName = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                            XMLPayorBankRoutNo = ds.Tables[0].Rows[index].ItemArray[4].ToString(),
                            XMLSAN = ds.Tables[0].Rows[index].ItemArray[5].ToString(),
                            XMLTrns = ds.Tables[0].Rows[index].ItemArray[6].ToString(),
                            DocType = ds.Tables[0].Rows[index].ItemArray[7].ToString(),
                            PresentmentDate = ds.Tables[0].Rows[index].ItemArray[8].ToString(),
                            BOFDRoutNo = ds.Tables[0].Rows[index].ItemArray[9].ToString(),
                            MICRRepairFlags = ds.Tables[0].Rows[index].ItemArray[10].ToString(),
                            FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[11].ToString(),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[12].ToString(),
                            BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[13].ToString(),
                            //FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[11].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                            //FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[12].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                            //BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[13].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                            chiStatus = ds.Tables[0].Rows[index].ItemArray[14].ToString(),
                            AccountNo = ds.Tables[0].Rows[index].ItemArray[15].ToString(),
                            L1VerificationName = ds.Tables[0].Rows[index].ItemArray[16].ToString(),
                            L2VerificationName = ds.Tables[0].Rows[index].ItemArray[17].ToString(),
                            L3VerificationName = ds.Tables[0].Rows[index].ItemArray[18].ToString(),

                            L1VerificationAction = ds.Tables[0].Rows[index].ItemArray[19].ToString(),
                            L2VerificationAction = ds.Tables[0].Rows[index].ItemArray[20].ToString(),
                            L3VerificationAction = ds.Tables[0].Rows[index].ItemArray[21].ToString(),
                            L1RejectReason = ds.Tables[0].Rows[index].ItemArray[22].ToString(),
                            L2RejectReason = ds.Tables[0].Rows[index].ItemArray[23].ToString(),
                            L3RejectReason = ds.Tables[0].Rows[index].ItemArray[24].ToString(),
                            chequedate = ds.Tables[0].Rows[index].ItemArray[25].ToString(),
                            MiscStatus = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[26].ToString()),
                            ItemSeqNo = ds.Tables[0].Rows[index].ItemArray[27] != null ? ds.Tables[0].Rows[index].ItemArray[27].ToString() : "",
                            ReturnReason = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[28] == DBNull.Value ? 0 : ds.Tables[0].Rows[index].ItemArray[28]),
                            ReturnReasonDescription = ds.Tables[0].Rows[index].ItemArray[29] != null ? ds.Tables[0].Rows[index].ItemArray[29].ToString() : "",
                            ReturnMarkedByName = ds.Tables[0].Rows[index].ItemArray[30] != null ? ds.Tables[0].Rows[index].ItemArray[30].ToString() : "",

                        };
                        count = count - 1;
                        index = index + 1;
                    }
                    if (iwsr.PresentmentDate != null && iwsr.PresentmentDate != "")
                        iwsr.PresentmentDate = Convert.ToDateTime(iwsr.PresentmentDate).Date.ToString("dd-MM-yyyy");
                    if (iwsr.chequedate != null && iwsr.chequedate != "")
                    {
                        if (iwsr.chequedate.Length <= 10)
                            iwsr.chequedate = iwsr.chequedate.ToString().Substring(0, 2) + "-" + iwsr.chequedate.ToString().Substring(2, 2) + "-" + "20" + iwsr.chequedate.ToString().Substring(4, 2);
                        else
                            iwsr.chequedate = Convert.ToDateTime(iwsr.chequedate).Date.ToString("dd-MM-yyyy");
                    }
                    //-----------------------L1action---------------------------
                    if (iwsr.L1VerificationAction != null && iwsr.L1VerificationAction != "")
                    {
                        if (iwsr.L1VerificationAction == "1")
                            iwsr.L1VerificationAction = "Y";
                        else if (iwsr.L1VerificationAction == "2")
                            iwsr.L1VerificationAction = "R";
                        else if (iwsr.L1VerificationAction == "0")
                            iwsr.L1VerificationAction = "N";

                    }
                    //-----------------------L2action---------------------------
                    if (iwsr.L2VerificationAction != null && iwsr.L2VerificationAction != "")
                    {
                        if (iwsr.L2VerificationAction == "1" || iwsr.L2VerificationAction == "2" || iwsr.L2VerificationAction == "3" || iwsr.L2VerificationAction == "6" || iwsr.L2VerificationAction == "8")
                            iwsr.L2VerificationAction = "Y";
                        else if (iwsr.L2VerificationAction == "4" || iwsr.L2VerificationAction == "7" || iwsr.L2VerificationAction == "9")
                            iwsr.L2VerificationAction = "R";
                        else if (iwsr.L2VerificationAction == "10" || iwsr.L2VerificationAction == "11")
                            iwsr.L2VerificationAction = "M";
                        //else if (iwsr.L2VerificationAction == "8")
                        //    iwsr.L2VerificationAction = "M";
                        //else if (iwsr.L2VerificationAction == "0")
                        //    iwsr.L2VerificationAction = "N";

                    }
                    //-----------------------L3action---------------------------
                    if (iwsr.L3VerificationAction != null && iwsr.L3VerificationAction != "")
                    {
                        if (iwsr.L3VerificationAction == "1" || iwsr.L3VerificationAction == "3")
                            iwsr.L3VerificationAction = "Y";
                        else if (iwsr.L3VerificationAction == "2")
                            iwsr.L3VerificationAction = "R";
                        else if (iwsr.L3VerificationAction == "4")
                            iwsr.L3VerificationAction = "M";
                        else if (iwsr.L3VerificationAction == "0")
                            iwsr.L3VerificationAction = "N";

                    }
                    //-----------------------L1User Name---------------------------
                    if (iwsr.L1VerificationName != null && iwsr.L1VerificationName != "" && iwsr.L1VerificationName != "0")
                    {
                        int L1id = Convert.ToInt16(iwsr.L1VerificationName);
                        var l1result = af.UserMasters.Where(m => m.ID == L1id).FirstOrDefault();
                        iwsr.L1VerificationName = l1result.FirstName;
                    }
                    //-----------------------L2User Name---------------------------
                    if (iwsr.L2VerificationName != null && iwsr.L2VerificationName != "" && iwsr.L2VerificationName != "0")
                    {
                        int L2id = Convert.ToInt16(iwsr.L2VerificationName);
                        var l2result = af.UserMasters.Where(m => m.ID == L2id).FirstOrDefault();
                        iwsr.L2VerificationName = l2result.FirstName;
                    }
                    //-----------------------L3User Name---------------------------
                    if (iwsr.L3VerificationName != null && iwsr.L3VerificationName != "" && iwsr.L3VerificationName != "0")
                    {
                        int L3id = Convert.ToInt16(iwsr.L3VerificationName);
                        var l3result = af.UserMasters.Where(m => m.ID == L3id).FirstOrDefault();
                        iwsr.L3VerificationName = l3result.FirstName;
                    }
                    //-------------------------Reject reason L1----------------------//
                    if (iwsr.L1RejectReason != null && iwsr.L1RejectReason != "" && iwsr.L1RejectReason != "0")
                    {
                        string l1reason = iwsr.L1RejectReason.PadLeft(2, '0');

                        var L1Retrn = af.ItemReturnReasons.Where(m => m.RETURN_REASON_CODE == l1reason).FirstOrDefault();
                        iwsr.L1RejectReason = L1Retrn.DESCRIPTION;
                    }
                    //-------------------------Reject reason L2----------------------//
                    if (iwsr.L2RejectReason != null && iwsr.L2RejectReason != "" && iwsr.L2RejectReason != "0")
                    {
                        string l2reason = iwsr.L2RejectReason.PadLeft(2, '0');
                        var L2Retrn = af.ItemReturnReasons.Where(m => m.RETURN_REASON_CODE == l2reason).FirstOrDefault();
                        iwsr.L2RejectReason = L2Retrn.DESCRIPTION;
                    }
                    //-------------------------Reject reason L3----------------------//
                    if (iwsr.L3RejectReason != null && iwsr.L3RejectReason != "" && iwsr.L3RejectReason != "0")
                    {
                        string l3reason = iwsr.L3RejectReason.PadLeft(2, '0');
                        var L3Retrn = af.ItemReturnReasons.Where(m => m.RETURN_REASON_CODE == l3reason).FirstOrDefault();
                        iwsr.L3RejectReason = L3Retrn.DESCRIPTION;
                    }
                    return PartialView("_CheqDetls", iwsr);
                }
                else
                {
                    return PartialView(false);
                }

            }
            catch (Exception e)
            {
                ErrorDisplay er = new ErrorDisplay();
                er.ErrorMessage = e.Message.ToString();
                return PartialView("Error", er);
            }
        }

        [HttpPost]
        public ActionResult CheqDetls(Int64 id, string EOD = null, string DbYear = null)
        {
            try
            {
                string connectionString = GetConnectionString(DbYear);
                SqlConnection con = new SqlConnection(connectionString);
                
                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter("GetCheque", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@id", SqlDbType.BigInt).Value = id;
                adp.SelectCommand.Parameters.Add("@EOD", SqlDbType.NVarChar).Value = EOD;
                ////--------------------Customer Selection---------------------
                //adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);


                //// adp.SelectCommand.Parameters.Add("@ChqNo", SqlDbType.NVarChar).Value = iwsrch.XMLSerialNo;
                //adp.SelectCommand.Parameters.Add("@Amount", SqlDbType.Float).Value = iwsrch.XMLAmount;
                adp.Fill(ds);
                IWSearch iwsr = null;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        int status = 0;
                        status = Convert.ToInt32(ds.Tables[0].Rows[index]["status"] == DBNull.Value ? 0 : ds.Tables[0].Rows[index]["status"]);

                        string frontGreyImagePath = "";
                        string frontTiffImagePath = "";
                        string backTiffImagePath = "";

                        if (status == 2 || status == 4 || status == 12 || status == 14 || status == 22 || status == 24)
                        {
                            string connectionString1 = GetConnectionString(DbYear);
                            using (SqlConnection con1 = new SqlConnection(connectionString1))
                            {
                                DataSet ds111 = new DataSet();
                                SqlDataAdapter adp111 = new SqlDataAdapter("IWGetChequeImageEncrypted", con1);
                                adp111.SelectCommand.CommandType = CommandType.StoredProcedure;
                                adp111.SelectCommand.Parameters.Add("@id", SqlDbType.BigInt).Value = Convert.ToInt32(ds.Tables[0].Rows[index]["ID"].ToString());

                                adp111.Fill(ds111);

                                if (ds111.Tables[0].Rows.Count > 0)
                                {

                                    byte[] imageBytesFrontGray = (byte[])(ds111.Tables[0].Rows[0]["FrontGray"]);
                                    string imageDataURLFrontGray = CreateImageDataURL(imageBytesFrontGray);

                                    byte[] imageBytesFrontTiff = (byte[])(ds111.Tables[0].Rows[0]["FrontTiff"]);
                                    string imageDataURLFrontTiff = CreateImageDataURL(imageBytesFrontTiff);

                                    byte[] imageBytesBackTiff = (byte[])(ds111.Tables[0].Rows[0]["BackTiff"]);
                                    string imageDataURLBackTiff = CreateImageDataURL(imageBytesBackTiff);

                                    frontGreyImagePath = imageDataURLFrontGray;
                                    frontTiffImagePath = imageDataURLFrontTiff;
                                    backTiffImagePath = imageDataURLBackTiff;
                                }
                            }
                                
                        }
                        else
                        {
                            frontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[11].ToString();
                            frontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[12].ToString();
                            backTiffImagePath = ds.Tables[0].Rows[index].ItemArray[13].ToString();
                        }

                        iwsr = new IWSearch
                        {
                            ID = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[0]),
                            XMLSerialNo = ds.Tables[0].Rows[index].ItemArray[1].ToString(),
                            XMLAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[2].ToString()),
                            XMLPayeeName = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                            XMLPayorBankRoutNo = ds.Tables[0].Rows[index].ItemArray[4].ToString(),
                            XMLSAN = ds.Tables[0].Rows[index].ItemArray[5].ToString(),
                            XMLTrns = ds.Tables[0].Rows[index].ItemArray[6].ToString(),
                            DocType = ds.Tables[0].Rows[index].ItemArray[7].ToString(),
                            PresentmentDate = ds.Tables[0].Rows[index].ItemArray[8].ToString(),
                            BOFDRoutNo = ds.Tables[0].Rows[index].ItemArray[9].ToString(),
                            MICRRepairFlags = ds.Tables[0].Rows[index].ItemArray[10].ToString(),
                            FrontGreyImagePath = frontGreyImagePath,
                            FrontTiffImagePath = frontTiffImagePath,
                            BackTiffImagePath = backTiffImagePath,

                            //FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[11].ToString(),
                            //FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[12].ToString(),
                            //BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[13].ToString(),
                            //FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[11].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                            //FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[12].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                            //BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[13].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                            chiStatus = ds.Tables[0].Rows[index].ItemArray[14].ToString(),
                            AccountNo = ds.Tables[0].Rows[index].ItemArray[15].ToString(),
                            L1VerificationName = ds.Tables[0].Rows[index].ItemArray[16].ToString(),
                            L2VerificationName = ds.Tables[0].Rows[index].ItemArray[17].ToString(),
                            L3VerificationName = ds.Tables[0].Rows[index].ItemArray[18].ToString(),

                            L1VerificationAction = ds.Tables[0].Rows[index].ItemArray[19].ToString(),
                            L2VerificationAction = ds.Tables[0].Rows[index].ItemArray[20].ToString(),
                            L3VerificationAction = ds.Tables[0].Rows[index].ItemArray[21].ToString(),
                            L1RejectReason = ds.Tables[0].Rows[index].ItemArray[22].ToString(),
                            L2RejectReason = ds.Tables[0].Rows[index].ItemArray[23].ToString(),
                            L3RejectReason = ds.Tables[0].Rows[index].ItemArray[24].ToString(),
                            chequedate = ds.Tables[0].Rows[index].ItemArray[25].ToString(),
                            MiscStatus = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[26].ToString()),
                            ItemSeqNo = ds.Tables[0].Rows[index].ItemArray[27] != null ? ds.Tables[0].Rows[index].ItemArray[27].ToString() : "",
                            ReturnReason = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[28] == DBNull.Value ? 0 : ds.Tables[0].Rows[index].ItemArray[28]),
                            ReturnReasonDescription = ds.Tables[0].Rows[index].ItemArray[29] != null ? ds.Tables[0].Rows[index].ItemArray[29].ToString() : "",
                            ReturnMarkedByName = ds.Tables[0].Rows[index].ItemArray[30] != null ? ds.Tables[0].Rows[index].ItemArray[30].ToString() : "",
                            Status = Convert.ToInt32(ds.Tables[0].Rows[index]["status"] == DBNull.Value ? 0 : ds.Tables[0].Rows[index]["status"])

                        };
                        count = count - 1;
                        index = index + 1;
                    }
                    if (iwsr.PresentmentDate != null && iwsr.PresentmentDate != "")
                        iwsr.PresentmentDate = Convert.ToDateTime(iwsr.PresentmentDate).Date.ToString("dd-MM-yyyy");
                    if (iwsr.chequedate != null && iwsr.chequedate != "")
                    {
                        if (iwsr.chequedate.Length <= 10)
                            iwsr.chequedate = iwsr.chequedate.ToString().Substring(0, 2) + "-" + iwsr.chequedate.ToString().Substring(2, 2) + "-" + "20" + iwsr.chequedate.ToString().Substring(4, 2);
                        else
                            iwsr.chequedate = Convert.ToDateTime(iwsr.chequedate).Date.ToString("dd-MM-yyyy");
                    }
                    //-----------------------L1action---------------------------
                    if (iwsr.L1VerificationAction != null && iwsr.L1VerificationAction != "")
                    {
                        if (iwsr.L1VerificationAction == "1")
                            iwsr.L1VerificationAction = "Y";
                        else if (iwsr.L1VerificationAction == "2")
                            iwsr.L1VerificationAction = "R";
                        else if (iwsr.L1VerificationAction == "0")
                            iwsr.L1VerificationAction = "N";

                    }
                    //-----------------------L2action---------------------------
                    if (iwsr.L2VerificationAction != null && iwsr.L2VerificationAction != "")
                    {
                        if (iwsr.L2VerificationAction == "1" || iwsr.L2VerificationAction == "2" || iwsr.L2VerificationAction == "3" || iwsr.L2VerificationAction == "6" || iwsr.L2VerificationAction == "8")
                            iwsr.L2VerificationAction = "Y";
                        else if (iwsr.L2VerificationAction == "4" || iwsr.L2VerificationAction == "7" || iwsr.L2VerificationAction == "9")
                            iwsr.L2VerificationAction = "R";
                        else if (iwsr.L2VerificationAction == "10" || iwsr.L2VerificationAction == "11")
                            iwsr.L2VerificationAction = "M";
                        //else if (iwsr.L2VerificationAction == "8")
                        //    iwsr.L2VerificationAction = "M";
                        //else if (iwsr.L2VerificationAction == "0")
                        //    iwsr.L2VerificationAction = "N";

                    }
                    //-----------------------L3action---------------------------
                    if (iwsr.L3VerificationAction != null && iwsr.L3VerificationAction != "")
                    {
                        if (iwsr.L3VerificationAction == "1" || iwsr.L3VerificationAction == "3")
                            iwsr.L3VerificationAction = "Y";
                        else if (iwsr.L3VerificationAction == "2")
                            iwsr.L3VerificationAction = "R";
                        else if (iwsr.L3VerificationAction == "4")
                            iwsr.L3VerificationAction = "M";
                        else if (iwsr.L3VerificationAction == "0")
                            iwsr.L3VerificationAction = "N";

                    }
                    //-----------------------L1User Name---------------------------
                    if (iwsr.L1VerificationName != null && iwsr.L1VerificationName != "" && iwsr.L1VerificationName != "0")
                    {
                        int L1id = Convert.ToInt16(iwsr.L1VerificationName);
                        var l1result = af.UserMasters.Where(m => m.ID == L1id).FirstOrDefault();
                        iwsr.L1VerificationName = l1result.FirstName;
                    }
                    //-----------------------L2User Name---------------------------
                    if (iwsr.L2VerificationName != null && iwsr.L2VerificationName != "" && iwsr.L2VerificationName != "0")
                    {
                        int L2id = Convert.ToInt16(iwsr.L2VerificationName);
                        var l2result = af.UserMasters.Where(m => m.ID == L2id).FirstOrDefault();
                        iwsr.L2VerificationName = l2result.FirstName;
                    }
                    //-----------------------L3User Name---------------------------
                    if (iwsr.L3VerificationName != null && iwsr.L3VerificationName != "" && iwsr.L3VerificationName != "0")
                    {
                        int L3id = Convert.ToInt16(iwsr.L3VerificationName);
                        var l3result = af.UserMasters.Where(m => m.ID == L3id).FirstOrDefault();
                        iwsr.L3VerificationName = l3result.FirstName;
                    }
                    //-------------------------Reject reason L1----------------------//
                    if (iwsr.L1RejectReason != null && iwsr.L1RejectReason != "" && iwsr.L1RejectReason != "0")
                    {
                        string l1reason = iwsr.L1RejectReason.PadLeft(2, '0');

                        var L1Retrn = af.ItemReturnReasons.Where(m => m.RETURN_REASON_CODE == l1reason).FirstOrDefault();
                        iwsr.L1RejectReason = L1Retrn.DESCRIPTION;
                    }
                    //-------------------------Reject reason L2----------------------//
                    if (iwsr.L2RejectReason != null && iwsr.L2RejectReason != "" && iwsr.L2RejectReason != "0")
                    {
                        string l2reason = iwsr.L2RejectReason.PadLeft(2, '0');
                        var L2Retrn = af.ItemReturnReasons.Where(m => m.RETURN_REASON_CODE == l2reason).FirstOrDefault();
                        iwsr.L2RejectReason = L2Retrn.DESCRIPTION;
                    }
                    //-------------------------Reject reason L3----------------------//
                    if (iwsr.L3RejectReason != null && iwsr.L3RejectReason != "" && iwsr.L3RejectReason != "0")
                    {
                        string l3reason = iwsr.L3RejectReason.PadLeft(2, '0');
                        var L3Retrn = af.ItemReturnReasons.Where(m => m.RETURN_REASON_CODE == l3reason).FirstOrDefault();
                        iwsr.L3RejectReason = L3Retrn.DESCRIPTION;
                    }
                    return PartialView("_CheqDetls", iwsr);
                }
                else
                {
                    return PartialView(false);
                }

            }
            catch (Exception e)
            {
                ErrorDisplay er = new ErrorDisplay();
                er.ErrorMessage = e.Message.ToString();
                return PartialView("Error", er);
            }
        }

        public ActionResult getTiffImage(string httpwebimgpath = null)
        {
            try
            {
                logerror(httpwebimgpath, httpwebimgpath.ToString() + " - >");
                int custid = Convert.ToInt16(Session["CustomerID"]);
                var destpath = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "PhysicalPath");

                //Owsr.L1VerificationName = l1result.LoginID;
                string destroot = destpath.SettingValue;
                logerror(destroot, destroot.ToString() + " - > destroot");

                const char delimiter = '\\';
                string[] destrootarr = destroot.Split(delimiter);

                string foldrname = destrootarr[destrootarr.Length - 1];
                logerror(foldrname, foldrname.ToString() + " - > Folder Name");

                httpwebimgpath = httpwebimgpath.Replace("\\", "/");
                string[] arrpath = httpwebimgpath.Split('/');
                string actualpath = "";

                int flgmtch = 0;
                foreach (var item in arrpath)
                {
                    if (item != "")
                    {
                        if (flgmtch == 1)
                        {

                            actualpath = actualpath + "\\" + item;
                        }

                        if (item == foldrname)
                        {
                            flgmtch = 1;
                        }
                    }

                }

                actualpath = destroot + "\\" + actualpath;
                actualpath = actualpath.Replace("\\\\", "\\");
                //System.IO.File.AppendAllText("C:\\temp\\log1.txt", "actualpathOWSearch:" + actualpath);
                logerror(actualpath, actualpath.ToString());

                System.Drawing.Bitmap bmp = new Bitmap(actualpath);
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                stream.Position = 0;

                byte[] data = new byte[stream.Length];
                int lngth = (int)stream.Length;
                stream.Read(data, 0, lngth);
                stream.Close();

                string imageBase64Data = Convert.ToBase64String(data);
                Array.Clear(data, 0, data.Length);


                string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                ViewBag.ImageData = imageDataURL;

                // imageDataURL = "";
            }
            catch (Exception ex)
            {

                System.IO.File.AppendAllText("C:\\temp\\log1.txt", "gettiffiame error :" + ex.Message.ToString());
            }


            return PartialView("_getTiffImage");
            //return Json(imageDataURL, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getTiffImageNew()
        {
            return PartialView("_getTiffImage");
        }

        public string CreateImageDataURL(byte[] imageBytes)
        {
            //byte[] imageBytes = Encoding.ASCII.GetBytes(ds111.Tables[0].Rows[index]["FrontGray"].ToString());
            //byte[] imageBytes = (byte[])(dataString);
            //string imageBase64Data = Convert.ToBase64String(imageBytes);
            //Array.Clear(imageBytes, 0, imageBytes.Length);

            Stream streamactual = new MemoryStream(imageBytes);
            System.Drawing.Bitmap bmp = new Bitmap(streamactual);
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            stream.Position = 0;

            byte[] data = new byte[stream.Length];
            int lngth = (int)stream.Length;
            stream.Read(data, 0, lngth);
            stream.Close();

            string imageBase64Data = Convert.ToBase64String(data);
            Array.Clear(imageBytes, 0, imageBytes.Length);

            string imageDataURL = string.Format("data:image/jpeg;base64,{0}", imageBase64Data);
            //ViewBag.ImageDataFrontGray = imageDataURL;

            return imageDataURL;
        }

        public string GetConnectionString(string dbyear = null)
        {
            string dataSource = "", intialCatalog = "", userId = "", password = "", connectionString = "";

            dataSource = System.Configuration.ConfigurationManager.AppSettings["DataSource"].ToString();
            intialCatalog = System.Configuration.ConfigurationManager.AppSettings["InitialCatalog"].ToString();
            userId = System.Configuration.ConfigurationManager.AppSettings["UserID"].ToString();
            password = System.Configuration.ConfigurationManager.AppSettings["Password"].ToString();

            connectionString = "Data Source=" + dataSource + ";Initial Catalog=" + intialCatalog + "_" + dbyear + ";User ID=" + userId + ";Password=" + password;

            return connectionString;
        }
    }
}
