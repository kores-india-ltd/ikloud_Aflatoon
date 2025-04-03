using ikloud_Aflatoon.Infrastructure;
using ikloud_Aflatoon.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ikloud_Aflatoon.Controllers
{
    public class IW_QuerySearchController : Controller
    {
        //
        // GET: /IW_QuerySearch/
        private static Logger logger = LogManager.GetCurrentClassLogger();
        AflatoonEntities af = new AflatoonEntities();
        IWProcDataContext iwpro = new IWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);

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
        public string QuerySearch11()
        {
            return "Success";
        }

        [HttpPost]
        public PartialViewResult QuerySearch111(string ProcessingDate = null, string ToProcessingDate = null, string XMLSerialNo = null)
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
                

                if (strFromDate != null)
                {
                    DateTime Totempdate = new DateTime();
                    DateTime tempdate = Convert.ToDateTime(strFromDate);
                    if (strToDate != null)
                    {
                        Totempdate = Convert.ToDateTime(strToDate);
                    }

                    //if (EOD == "true")
                    //{
                    //    logerror("In SearchIndex EOD function", "In SearchIndex EOD function" + " - >");
                    //    var iwsr = (from r in iwpro.EOD_IWFinalMainTransactions
                    //                where (r.ProcessingDate >= tempdate) && (r.ProcessingDate <= Totempdate || Totempdate == null) && (r.CustomerId == custID) && (r.XMLSerialNo == strCheqNo || strCheqNo == null) &&
                    //                (r.XMLAmount == amount || amount == 0) && (r.DbtAccNo == strAccountno || strAccountno == null) && (r.XMLPayorBankRoutNo == strSortCode || strSortCode == null)
                    //                && (r.XMLTransCode == strTransCode || strTransCode == null) && (P2f == "true" ? r.DocType == "C" : r.DocType == r.DocType)
                    //                && (r.ClearingType == cleraringtype || cleraringtype == null)
                    //                select new IWSearch
                    //                {
                    //                    ID = r.ID,
                    //                    XMLSerialNo = r.XMLSerialNo,
                    //                    XMLAmount = r.XMLAmount,
                    //                    XMLPayeeName = r.XMLPayeeName,
                    //                    XMLPayorBankRoutNo = r.XMLPayorBankRoutNo,
                    //                    XMLSAN = r.XMLSAN,
                    //                    XMLTrns = r.XMLTransCode,
                    //                    FrontGreyImagePath = r.FrontGreyImagePath.ToString(),
                    //                    chiStatus = r.CHIStatus,
                    //                    P2F = r.DocType,
                    //                    AccountNo = r.DbtAccNo,
                    //                    MiscStatus = r.MiscStatus

                    //                }).ToList();
                    //    return PartialView("_SearchCheques", iwsr);
                    //}
                    //else
                    //{
                        logerror("In SearchIndex EOD else function", "In SearchIndex EOD else function" + " - >");
                        var iwsr = (from r in af.IWFinalMainTransactions
                                    where (r.ProcessingDate >= tempdate) && (r.ProcessingDate <= Totempdate || Totempdate == null) && (r.CustomerId == custID) && (r.XMLSerialNo == strCheqNo || strCheqNo == null) 
                                    select new IWSearch
                                    {
                                        ID = r.ID,
                                        XMLSerialNo = r.XMLSerialNo,
                                        XMLAmount = r.XMLAmount,
                                        XMLPayeeName = r.XMLPayeeName,
                                        XMLPayorBankRoutNo = r.XMLPayorBankRoutNo,
                                        XMLSAN = r.XMLSAN,
                                        XMLTrns = r.XMLTransCode,
                                        FrontGreyImagePath = r.FrontGreyImagePath.ToString(),
                                        chiStatus = r.CHIStatus,
                                        P2F = r.DocType,
                                        AccountNo = r.DbtAccNo,
                                        MiscStatus = r.MiscStatus

                                    }).ToList();
                        return PartialView("_SearchCheques", iwsr);
                    //}
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
        public PartialViewResult QuerySearch(string ProcessingDate = null, string ToProcessingDate = null, string XMLSerialNo = null, string XMLAmount = null,
            string AccountNo = null, string XMLPayorBankRoutNo = null, string XMLTrns = null, string clrtype = null, string P2f = null, string EOD = null)
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
                        
                        var iwsr = (from r in iwpro.EOD_IWFinalMainTransactions
                                    where (r.ProcessingDate >= tempdate) && (r.ProcessingDate <= Totempdate || Totempdate == null) && (r.CustomerId == custID) && (r.XMLSerialNo == strCheqNo || strCheqNo == null) &&
                                    (r.XMLAmount == amount || amount == 0) && (r.DbtAccNo == strAccountno || strAccountno == null) && (r.XMLPayorBankRoutNo == strSortCode || strSortCode == null)
                                    && (r.XMLTransCode == strTransCode || strTransCode == null) && (P2f == "true" ? r.DocType == "C" : r.DocType == r.DocType)
                                    && (r.ClearingType == cleraringtype || cleraringtype == null)
                                    select new IWSearch
                                    {
                                        ID = r.ID,
                                        XMLSerialNo = r.XMLSerialNo,
                                        XMLAmount = r.XMLAmount,
                                        XMLPayeeName = r.XMLPayeeName,
                                        XMLPayorBankRoutNo = r.XMLPayorBankRoutNo,
                                        XMLSAN = r.XMLSAN,
                                        XMLTrns = r.XMLTransCode,
                                        FrontGreyImagePath = r.FrontGreyImagePath.ToString(),
                                        chiStatus = r.CHIStatus,
                                        P2F = r.DocType,
                                        AccountNo = r.DbtAccNo,
                                        MiscStatus = r.MiscStatus

                                    }).ToList();
                        return PartialView("_SearchCheques", iwsr);
                    }
                    else
                    {
                        logerror("In SearchIndex EOD else function", "In SearchIndex EOD else function" + " - >");
                        logerror(tempdate.ToString(), tempdate.ToString() + " TempDate - >");
                        logerror(Totempdate.ToString(), Totempdate.ToString() + " ToTempDate - >");
                        var iwsr = (from r in af.IWFinalMainTransactions
                                    where (r.ProcessingDate >= tempdate) && (r.ProcessingDate <= Totempdate || Totempdate == null) && (r.CustomerId == custID) && (r.XMLSerialNo == strCheqNo || strCheqNo == null) &&
                                    (r.XMLAmount == amount || amount == 0) && (r.DbtAccNo == strAccountno || strAccountno == null) && (r.XMLPayorBankRoutNo == strSortCode || strSortCode == null)
                                    && (r.XMLTransCode == strTransCode || strTransCode == null) && (P2f == "true" ? r.DocType == "C" : r.DocType == r.DocType)
                                    && (r.ClearingType == cleraringtype || cleraringtype == null)
                                    select new IWSearch
                                    {
                                        ID = r.ID,
                                        XMLSerialNo = r.XMLSerialNo,
                                        XMLAmount = r.XMLAmount,
                                        XMLPayeeName = r.XMLPayeeName,
                                        XMLPayorBankRoutNo = r.XMLPayorBankRoutNo,
                                        XMLSAN = r.XMLSAN,
                                        XMLTrns = r.XMLTransCode,
                                        FrontGreyImagePath = r.FrontGreyImagePath.ToString(),
                                        chiStatus = r.CHIStatus,
                                        P2F = r.DocType,
                                        AccountNo = r.DbtAccNo,
                                        MiscStatus = r.MiscStatus

                                    }).ToList();
                        logerror(iwsr.Count.ToString(), iwsr.Count.ToString() + " iwsr count - >");
                        return PartialView("_SearchCheques", iwsr);
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
        public PartialViewResult ShowData(Int64 id, string EOD = null)
        {
            try
            {
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
        public ActionResult CheqDetls(Int64 id, string EOD = null)
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

            try
            {
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


                            SQMakerId = ds.Tables[0].Rows[index].ItemArray[31] != null ? ds.Tables[0].Rows[index].ItemArray[31].ToString() : "",
                            SQMakerDecision = ds.Tables[0].Rows[index].ItemArray[32] != null ? ds.Tables[0].Rows[index].ItemArray[32].ToString() : "",
                            SQMakerReturnReason = ds.Tables[0].Rows[index].ItemArray[33] != null ? ds.Tables[0].Rows[index].ItemArray[33].ToString() : "",
                            SQMakerReturnReasonDiscription = ds.Tables[0].Rows[index].ItemArray[34] != null ? ds.Tables[0].Rows[index].ItemArray[34].ToString() : "",

                            SQCheckerId = ds.Tables[0].Rows[index].ItemArray[35] != null ? ds.Tables[0].Rows[index].ItemArray[35].ToString() : "",
                            SQCheckerDecision = ds.Tables[0].Rows[index].ItemArray[36] != null ? ds.Tables[0].Rows[index].ItemArray[36].ToString() : "",
                            ExceptionRejectReason = ds.Tables[0].Rows[index].ItemArray[37] != null ? ds.Tables[0].Rows[index].ItemArray[37].ToString() : "",
                            ExceptionRejectDescription = ds.Tables[0].Rows[index].ItemArray[38] != null ? ds.Tables[0].Rows[index].ItemArray[38].ToString() : "",

                            // SQCheckerReturnReason= ds.Tables[0].Rows[index].ItemArray[39] != null ? ds.Tables[0].Rows[index].ItemArray[39].ToString() : "",
                            // SQCheckerReturnReasonDiscription = ds.Tables[0].Rows[index].ItemArray[40] != null ? ds.Tables[0].Rows[index].ItemArray[40].ToString() : "",
                            IsByPassedFromL0= ds.Tables[0].Rows[index].ItemArray[39] != null ? ds.Tables[0].Rows[index].ItemArray[39].ToString() : "",

                        };
                        count = count - 1;
                        index = index + 1;
                    }
                    if (iwsr.PresentmentDate != null && iwsr.PresentmentDate != "")
                        iwsr.PresentmentDate = Convert.ToDateTime(iwsr.PresentmentDate).Date.ToString("dd-MM-yyyy");
                    if (iwsr.chequedate != null && iwsr.chequedate != "")
                    {
                        if (iwsr.chequedate.Length < 10)
                            iwsr.chequedate = iwsr.chequedate.ToString().Substring(0, 2) + "-" + iwsr.chequedate.ToString().Substring(2, 2) + "-" + "20" + iwsr.chequedate.ToString().Substring(4, 2);
                        else
                            iwsr.chequedate = Convert.ToDateTime(iwsr.chequedate).Date.ToString("dd-MM-yyyy");
                    }
                    //-----------------------L1action---------------------------
                    if (iwsr.L1VerificationAction != null && iwsr.L1VerificationAction != "")
                    {
                        if (iwsr.L1VerificationAction == "2")
                            iwsr.L1VerificationAction = "A";
                        else if (iwsr.L1VerificationAction == "3")
                            iwsr.L1VerificationAction = "R";
                        else if (iwsr.L1VerificationAction == "4")
                            iwsr.L1VerificationAction = "E";

                    }
                    //-----------------------L2action---------------------------
                    if (iwsr.L2VerificationAction != null && iwsr.L2VerificationAction != "")
                    {
                        //if (iwsr.L2VerificationAction == "1" || iwsr.L2VerificationAction == "2" || iwsr.L2VerificationAction == "3" || iwsr.L2VerificationAction == "6" || iwsr.L2VerificationAction == "8")
                        //    iwsr.L2VerificationAction = "Y";
                        //else if (iwsr.L2VerificationAction == "4" || iwsr.L2VerificationAction == "7" || iwsr.L2VerificationAction == "9")
                        //    iwsr.L2VerificationAction = "R";
                        //else if (iwsr.L2VerificationAction == "10" || iwsr.L2VerificationAction == "11")
                        //    iwsr.L2VerificationAction = "M";


                        if (iwsr.L2VerificationAction == "2")
                            iwsr.L2VerificationAction = "A";
                        else if (iwsr.L2VerificationAction == "3")
                            iwsr.L2VerificationAction = "R";
                        else if (iwsr.L2VerificationAction == "4")
                            iwsr.L2VerificationAction = "E";





                        //else if (iwsr.L2VerificationAction == "8")
                        //    iwsr.L2VerificationAction = "M";
                        //else if (iwsr.L2VerificationAction == "0")
                        //    iwsr.L2VerificationAction = "N";

                    }
                    //-----------------------L3action---------------------------
                    if (iwsr.L3VerificationAction != null && iwsr.L3VerificationAction != "")
                    {
                        //if (iwsr.L3VerificationAction == "1" || iwsr.L3VerificationAction == "3")
                        //    iwsr.L3VerificationAction = "Y";
                        //else if (iwsr.L3VerificationAction == "2")
                        //    iwsr.L3VerificationAction = "R";
                        //else if (iwsr.L3VerificationAction == "4")
                        //    iwsr.L3VerificationAction = "M";
                        //else if (iwsr.L3VerificationAction == "0")
                        //    iwsr.L3VerificationAction = "N";

                        if (iwsr.L3VerificationAction == "2")
                            iwsr.L3VerificationAction = "A";
                        else if (iwsr.L3VerificationAction == "3")
                            iwsr.L3VerificationAction = "R";
                        else if (iwsr.L3VerificationAction == "4")
                            iwsr.L3VerificationAction = "E";




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
                        if (iwsr.L1VerificationAction == "E")
                        {
                            string l1exreason= iwsr.L1RejectReason.PadLeft(2, '0');
                            var l1exreturn = GetExceptionReturnReason();//list of 
                           
                            var result = l1exreturn.Where(m => m.ReasonCodeS.Trim().Equals(l1exreason, StringComparison.OrdinalIgnoreCase))
                                            .Select(m => m.Description)
                                            .FirstOrDefault();

                            iwsr.L1RejectReason = result;
                        }
                        else
                        {
                            string l1reason = iwsr.L1RejectReason.PadLeft(2, '0');

                            var L1Retrn = af.ItemReturnReasons.Where(m => m.RETURN_REASON_CODE == l1reason).FirstOrDefault();
                            iwsr.L1RejectReason = L1Retrn.DESCRIPTION;
                        }
                       
                    }
                    //-------------------------Reject reason L2----------------------//
                    if (iwsr.L2RejectReason != null && iwsr.L2RejectReason != "" && iwsr.L2RejectReason != "0")
                    {

                        if(iwsr.L2VerificationAction == "E")
                        {
                            string l1exreason = iwsr.L2RejectReason.PadLeft(2, '0');
                            var l1exreturn = GetExceptionReturnReason();//list of 

                            var result = l1exreturn.Where(m => m.ReasonCodeS.Trim().Equals(l1exreason, StringComparison.OrdinalIgnoreCase))
                                            .Select(m => m.Description)
                                            .FirstOrDefault();

                            iwsr.L2RejectReason = result;
                        }
                        else
                        {
                            string l2reason = iwsr.L2RejectReason.PadLeft(2, '0');
                            var L2Retrn = af.ItemReturnReasons.Where(m => m.RETURN_REASON_CODE == l2reason).FirstOrDefault();
                            iwsr.L2RejectReason = L2Retrn.DESCRIPTION;
                        }

                       
                    }
                    //-------------------------Reject reason L3----------------------//
                    if (iwsr.L3RejectReason != null && iwsr.L3RejectReason != "" && iwsr.L3RejectReason != "0")
                    {
                        if(iwsr.L3VerificationAction == "E")
                        {
                            string l1exreason = iwsr.L3RejectReason.PadLeft(2, '0');
                            var l1exreturn = GetExceptionReturnReason();//list of 

                            var result = l1exreturn.Where(m => m.ReasonCodeS.Trim().Equals(l1exreason, StringComparison.OrdinalIgnoreCase))
                                            .Select(m => m.Description)
                                            .FirstOrDefault();

                            iwsr.L3RejectReason = result;
                        }
                        else
                        {
                            string l3reason = iwsr.L3RejectReason.PadLeft(2, '0');
                            var L3Retrn = af.ItemReturnReasons.Where(m => m.RETURN_REASON_CODE == l3reason).FirstOrDefault();
                            iwsr.L3RejectReason = L3Retrn.DESCRIPTION;
                        }

                        
                    }

                   // -----------------------Suspence Q--------------------------// 
                    if (iwsr.SQMakerReturnReason != null && iwsr.SQMakerReturnReason != "")
                    {
                        string sqreson = iwsr.SQMakerReturnReason.PadLeft(2, '0');
                        var sqreturn = af.ItemReturnReasons.Where(m => m.RETURN_REASON_CODE == sqreson).FirstOrDefault();
                        iwsr.SQMakerReturnReason = sqreturn.DESCRIPTION;
                    }



                    //for IsByPassedFromL0
                    if (iwsr.IsByPassedFromL0 == "True")
                    {
                        iwsr.L1VerificationName= "System";
                        iwsr.L2VerificationName = "System";
                        iwsr.L3VerificationName = "System";
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

                httpwebimgpath = httpwebimgpath.Replace("\\","/");
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


        //get activity log
        public PartialViewResult GetActivityLog(string mainTransId = null, string chequeno = null)
        {
            var Get_OWL1ActivityLog = new List<Get_OWL1ActivityLog>();
            var Get_OWL2ActivityLog = new List<Get_OWL2ActivityLog>();
            var Get_OWL3ActivityLog = new List<Get_OWL3ActivityLog>();
            var Get_IWSQActivityLog = new List<Get_IWSQActivityLog>();

            try
            {
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString))
                {
                    con.Open();

                    // Fetch Table1 data
                    using (var cmd = new SqlCommand("Get_IWL1ActivityLog", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@mainTransId", Convert.ToInt64(mainTransId));
                        cmd.Parameters.AddWithValue("@ProcessingDate", Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@customerid", Convert.ToInt16(Session["CustomerID"]));
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Get_OWL1ActivityLog.Add(new Get_OWL1ActivityLog
                                {
                                    //Column1 = reader["Column1"].ToString(),
                                    //Column2 = reader["Column2"].ToString(),

                                    modified = reader["modified"].ToString(),
                                    LogLevel = reader["LogLevel"].ToString(),

                                    L1_ChequeNo = reader["L1_ChequeNo"].ToString(),
                                    L1_AL_ChequeNo = reader["L1_AL_ChequeNo"].ToString(),
                                    L1_ChequeNo_Comparison = reader["L1_ChequeNo_Comparison"].ToString(),
                                    L1_SortCode = reader["L1_SortCode"].ToString(),

                                    L1_AL_SortCode = reader["L1_AL_SortCode"].ToString(),
                                    L1_SortCode_Comparison = reader["L1_SortCode_Comparison"].ToString(),
                                    L1_SAN = reader["L1_SAN"].ToString(),
                                    L1_AL_SAN = reader["L1_AL_SAN"].ToString(),
                                    L1_SAN_Comparison = reader["L1_SAN_Comparison"].ToString(),
                                    L1_TC = reader["L1_TC"].ToString(),

                                    L1_AL_TC = reader["L1_AL_TC"].ToString(),
                                    L1_TC_Comparison = reader["L1_TC_Comparison"].ToString(),

                                    L1_CreditAccNo = reader["L1_CreditAccNo"].ToString(),
                                    L1_AL_CreditAccNo = reader["L1_AL_CreditAccNo"].ToString(),
                                    L1_CreditAccNo_Comparison = reader["L1_CreditAccNo_Comparison"].ToString(),

                                    L1RawDataId = reader["L1RawDataId"].ToString(),
                                    AlL1RawDataId = reader["AlL1RawDataId"].ToString(),
                                    L1_RawDataId_Comparison = reader["L1_RawDataId_Comparison"].ToString(),
                                    L1_status = reader["L1_status"].ToString(),
                                    AL_L1_LoginId = reader["AL_L1_LoginId"].ToString(),
                                    L1timestamp = Convert.ToDateTime(reader["L1timestamp"].ToString()),


                                });
                            }
                        }
                    }

                    // Fetch Table2 data
                    using (var cmd = new SqlCommand("Get_IWL2ActivityLog", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@mainTransId", Convert.ToInt64(mainTransId));
                        cmd.Parameters.AddWithValue("@ProcessingDate", Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@customerid", Convert.ToInt16(Session["CustomerID"]));
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Get_OWL2ActivityLog.Add(new Get_OWL2ActivityLog
                                {
                                    modified = reader["modified"].ToString(),
                                    LogLevel = reader["LogLevel"].ToString(),

                                    L2_ChequeNo = reader["L2_ChequeNo"].ToString(),
                                    L2_AL_ChequeNo = reader["L2_AL_ChequeNo"].ToString(),
                                    L2_ChequeNo_Comparison = reader["L2_ChequeNo_Comparison"].ToString(),
                                    L2_SortCode = reader["L2_SortCode"].ToString(),

                                    L2_AL_SortCode = reader["L2_AL_SortCode"].ToString(),
                                    L2_SortCode_Comparison = reader["L2_SortCode_Comparison"].ToString(),
                                    L2_SAN = reader["L2_SAN"].ToString(),
                                    L2_AL_SAN = reader["L2_AL_SAN"].ToString(),
                                    L2_SAN_Comparison = reader["L2_SAN_Comparison"].ToString(),
                                    L2_TC = reader["L2_TC"].ToString(),

                                    L2_AL_TC = reader["L2_AL_TC"].ToString(),
                                    L2_TC_Comparison = reader["L2_TC_Comparison"].ToString(),

                                    L2_CreditAccNo = reader["L2_CreditAccNo"].ToString(),
                                    L2_AL_CreditAccNo = reader["L2_AL_CreditAccNo"].ToString(),
                                    L2_CreditAccNo_Comparison = reader["L2_CreditAccNo_Comparison"].ToString(),

                                    L2RawDataId = reader["L2RawDataId"].ToString(),
                                    AlL2RawDataId = reader["AlL2RawDataId"].ToString(),
                                    L2_RawDataId_Comparison = reader["L2_RawDataId_Comparison"].ToString(),
                                    L2_status = reader["L2_status"].ToString(),
                                    AL_L2_LoginId = reader["AL_L2_LoginId"].ToString(),
                                    L2timestamp = Convert.ToDateTime(reader["L2timestamp"].ToString()),
                                    // Map other columns
                                });
                            }
                        }
                    }


                    // Fetch Table3 data
                    using (var cmd = new SqlCommand("Get_IWL3ActivityLog", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@mainTransId", Convert.ToInt64(mainTransId));
                        cmd.Parameters.AddWithValue("@ProcessingDate", Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@customerid", Convert.ToInt16(Session["CustomerID"]));
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Get_OWL3ActivityLog.Add(new Get_OWL3ActivityLog
                                {
                                    modified = reader["modified"].ToString(),
                                    LogLevel = reader["LogLevel"].ToString(),

                                    L3_ChequeNo = reader["L3_ChequeNo"].ToString(),
                                    L3_AL_ChequeNo = reader["L3_AL_ChequeNo"].ToString(),
                                    L3_ChequeNo_Comparison = reader["L3_ChequeNo_Comparison"].ToString(),
                                    L3_SortCode = reader["L3_SortCode"].ToString(),

                                    L3_AL_SortCode = reader["L3_AL_SortCode"].ToString(),
                                    L3_SortCode_Comparison = reader["L3_SortCode_Comparison"].ToString(),
                                    L3_SAN = reader["L3_SAN"].ToString(),
                                    L3_AL_SAN = reader["L3_AL_SAN"].ToString(),
                                    L3_SAN_Comparison = reader["L3_SAN_Comparison"].ToString(),
                                    L3_TC = reader["L3_TC"].ToString(),

                                    L3_AL_TC = reader["L3_AL_TC"].ToString(),
                                    L3_TC_Comparison = reader["L3_TC_Comparison"].ToString(),

                                    L3_CreditAccNo = reader["L3_CreditAccNo"].ToString(),
                                    L3_AL_CreditAccNo = reader["L3_AL_CreditAccNo"].ToString(),
                                    L3_CreditAccNo_Comparison = reader["L3_CreditAccNo_Comparison"].ToString(),

                                    L3RawDataId = reader["L3RawDataId"].ToString(),
                                    AlL3RawDataId = reader["AlL3RawDataId"].ToString(),
                                    L3_RawDataId_Comparison = reader["L3_RawDataId_Comparison"].ToString(),
                                    L3_status = reader["L3_status"].ToString(),
                                    AL_L3_LoginId = reader["AL_L3_LoginId"].ToString(),
                                    L3timestamp = Convert.ToDateTime(reader["L3timestamp"].ToString()),
                                    // Map other columns
                                });
                            }
                        }
                    }



                    //fetch sq 
                    using (var cmd = new SqlCommand("Get_IWSQActivityLog", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@mainTransId", Convert.ToInt64(mainTransId));
                        cmd.Parameters.AddWithValue("@ProcessingDate", Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@customerid", Convert.ToInt16(Session["CustomerID"]));
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Get_IWSQActivityLog.Add(new Get_IWSQActivityLog
                                {
                                    modified = reader["modified"].ToString(),
                                    LogLevel = reader["LogLevel"].ToString(),

                                    SQ_ChequeNo = reader["SQ_ChequeNo"].ToString(),
                                    SQ_AL_ChequeNo = reader["SQ_AL_ChequeNo"].ToString(),
                                    SQ_ChequeNo_Comparison = reader["SQ_ChequeNo_Comparison"].ToString(),
                                    SQ_SortCode = reader["SQ_SortCode"].ToString(),

                                    SQ_AL_SortCode = reader["SQ_AL_SortCode"].ToString(),
                                    SQ_SortCode_Comparison = reader["SQ_SortCode_Comparison"].ToString(),
                                    SQ_SAN = reader["SQ_SAN"].ToString(),
                                    SQ_AL_SAN = reader["SQ_AL_SAN"].ToString(),
                                    SQ_SAN_Comparison = reader["SQ_SAN_Comparison"].ToString(),
                                    SQ_TC = reader["SQ_TC"].ToString(),

                                    SQ_AL_TC = reader["SQ_AL_TC"].ToString(),
                                    SQ_TC_Comparison = reader["SQ_TC_Comparison"].ToString(),

                                    SQ_CreditAccNo = reader["SQ_CreditAccNo"].ToString(),
                                    SQ_AL_CreditAccNo = reader["SQ_AL_CreditAccNo"].ToString(),
                                    SQ_CreditAccNo_Comparison = reader["SQ_CreditAccNo_Comparison"].ToString(),

                                    SQRawDataId = reader["SQRawDataId"].ToString(),
                                    AlSQRawDataId = reader["AlSQRawDataId"].ToString(),
                                    SQ_RawDataId_Comparison = reader["SQ_RawDataId_Comparison"].ToString(),
                                    SQ_Maker_status= reader["SQ_Maker_status"].ToString(),
                                    SQ_Checker_status= reader["SQ_Checker_status"].ToString(),
                                    SQ_MakerID= reader["SQ_MakerID"].ToString(),
                                    SQ_CheckerID= reader["SQ_CheckerID"].ToString(),

                                   SQ_MakerTime= reader["SQ_MakerTime"].ToString(),
                                   SQ_CheckerTime = reader["SQ_CheckerTime"].ToString(),
                                    AL_SQ_LoginId = reader["AL_SQ_LoginId"].ToString(),
                                    SQtimestamp = Convert.ToDateTime(reader["SQtimestamp"].ToString()),
                                    // Map other columns
                                });
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {

                ErrorDisplay er = new ErrorDisplay();
                er.ErrorMessage = e.Message.ToString();

                string message = "";
                string innerExcp = "";
                string trace = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                {
                    innerExcp = e.InnerException.Message;
                    trace = e.InnerException.StackTrace;
                }

                logerror("in Catch Iw QuerySearch ActivityLog===>message==>" + message, "InnerExp==>" + innerExcp);

                return PartialView("Error", er);
            }



            var viewModel = new IWActivityLogViewModal
            {
                Get_OWL1ActivityLog = Get_OWL1ActivityLog,
                Get_OWL2ActivityLog = Get_OWL2ActivityLog,
                Get_OWL3ActivityLog = Get_OWL3ActivityLog,
                Get_IWSQActivityLog= Get_IWSQActivityLog,

            };

            return PartialView("_GetActivityLog", viewModel);
        }


        public List<RejectReason> GetExceptionReturnReason()
        {

            var rjrs = new List<RejectReason>();
           
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("GetExtensionRejectReasons", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var rejectReason = new RejectReason
                            {
                                Description = reader["DESCRIPTION"].ToString(),
                                ReasonCodeS = reader["RETURN_REASON_CODE"].ToString()
                            };
                            rjrs.Add(rejectReason);
                        }
                    }
                }
            }
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                string trace = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                {
                    innerExcp = e.InnerException.Message;
                    trace = e.InnerException.StackTrace;
                }
                logerror("In iwquery GetExceptionReturnReason Catch==>>" + message, "InnerExp===>>" + innerExcp);
            }
           

            return rjrs;
        }

       


        //not in use
        public bool IWQueryRntMark(Int64 id, string npcirtncd, int uid, string actn, out int updateFlg, string rtnrjctdescrn)
        {
            updateFlg = 0;
            try
            {
                using (SqlCommand cmd = new SqlCommand("IWQueryRntMark", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add input parameters
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@NPCICode", npcirtncd);
                    cmd.Parameters.AddWithValue("@uid", uid);
                    cmd.Parameters.AddWithValue("@Action", actn);
                    cmd.Parameters.AddWithValue("@ReturnReasonDescription", rtnrjctdescrn);
                    cmd.Parameters.AddWithValue("@ReturnFlag", "OWQuery");

                    // Add output parameter
                    SqlParameter outputParam = new SqlParameter("@updateFlg", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputParam);

                    // Open the connection if not already open
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    // Execute the stored procedure
                    cmd.ExecuteNonQuery();

                    // Retrieve the value of the output parameter
                    updateFlg = Convert.ToInt32(outputParam.Value);

                    return true; // Indicates successful execution
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error executing stored procedure: {ex.Message}");
                return false; // Indicates failure
            }
        }
    }
}
