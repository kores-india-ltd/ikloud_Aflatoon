﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ikloud_Aflatoon.Infrastructure;
using NLog;

namespace ikloud_Aflatoon.Controllers
{
    public class OW_QuerySearchNewController : Controller
    {
        //
        // GET: /OW_QuerySearchNew/
        AflatoonEntities af = new AflatoonEntities();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);

        public class ErrorDisplay
        {
            public int ErrorNo { get; set; }
            public string ErrorMessage { get; set; }
        }

        public class RejectReason
        {
            public string ReasonCodeS { get; set; }
            public int ReasonCode { get; set; }
            public int mtrid { get; set; }
            public string Description { get; set; }
            public bool IsChecked { get; set; }
            public string CBSCode { get; set; }

        }
        private void logerror(string errormsg, string errordesc)
        {
            ErrorDisplay er = new ErrorDisplay();
            string ServerPath = "";
            string filename = "";
            string fileNameWithPath = "";
            //FormsAuthentication.SignOut();


            //ViewBag.Error = e.InnerException;

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
            return View();
        }

        [HttpPost]
        public ActionResult QuerySearch(string ProcessingDate = null, string ToProcessingDate = null, string XMLSerialNo = null, string XMLAmount = null,
            string AccountNo = null, string XMLPayorBankRoutNo = null, string XMLTrns = null, string BranchCode = null, string P2f = null)
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
                logerror("In SearchQuery function", "In SearchQuery function" + " - >");
                string strFromDate = null, strToDate = null, strCheqNo = null, strSortCode = null, strTransCode = null, strAccountno = null, strBranchCode = null, strP2f = null;
                decimal amount = 0;
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
                if (BranchCode != null && BranchCode != "")
                    strBranchCode = BranchCode.ToString();
                int custid = Convert.ToInt16(Session["CustomerID"]);

                logerror("In SearchQuery function ", "strFromDate - " + strFromDate.ToString());

                if (strFromDate != null)
                {
                    DateTime Totempdate = new DateTime();
                    string tempDateFrom = strFromDate.Substring(6, 4) + "-" + strFromDate.Substring(3, 2) + "-" + strFromDate.Substring(0, 2);
                    string tempDateTo = "";
                    DateTime tempdate = Convert.ToDateTime(tempDateFrom);
                    if (strToDate != null)
                    {
                        tempDateTo = strToDate.Substring(6, 4) + "-" + strToDate.Substring(3, 2) + "-" + strToDate.Substring(0, 2);
                        Totempdate = Convert.ToDateTime(tempDateTo);
                    }

                    logerror("In SearchQuery function ", "FromProcessingDate - " + tempDateFrom.ToString());
                    logerror("In SearchQuery function ", "ToProcessingDate - " + tempDateTo.ToString());
                    logerror("In SearchQuery function ", "CustomerID - " + Convert.ToInt16(Session["CustomerID"]).ToString());

                    //========= Creating SP on 06/01/2023 ===============================
                    strP2f = P2f == "true" ? "C" : "B";

                    logerror("In SearchQuery function ", "Executing OWGetCheques_ByQuerySearch - started ");
                    SqlDataAdapter adp = new SqlDataAdapter("OWGetCheques_ByQuerySearch", con);
                    adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                    adp.SelectCommand.Parameters.Add("@FromProcessingDate", SqlDbType.NVarChar).Value = tempDateFrom;
                    adp.SelectCommand.Parameters.Add("@ToProcessingDate", SqlDbType.NVarChar).Value = tempDateTo;
                    adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                    adp.SelectCommand.Parameters.Add("@ChequeNoFinal", SqlDbType.NVarChar).Value = strCheqNo;
                    adp.SelectCommand.Parameters.Add("@FinalAmount", SqlDbType.Decimal).Value = amount;
                    adp.SelectCommand.Parameters.Add("@FinalAccountNo", SqlDbType.NVarChar).Value = strAccountno;
                    adp.SelectCommand.Parameters.Add("@SortCodeFinal", SqlDbType.NVarChar).Value = strSortCode;
                    adp.SelectCommand.Parameters.Add("@TransCodeFinal", SqlDbType.NVarChar).Value = strTransCode;
                    adp.SelectCommand.Parameters.Add("@DocType", SqlDbType.NVarChar).Value = strP2f;
                    adp.SelectCommand.Parameters.Add("@BranchCode", SqlDbType.NVarChar).Value = strBranchCode;
                    //adp.SelectCommand.Parameters.Add("@EOD", SqlDbType.NVarChar).Value = EOD;

                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    var objectlst = new List<IWSearch>();
                    IWSearch def;
                    logerror("In SearchQuery function ", "Executing OWGetCheques_ByQuerySearch - finished ");
                    logerror("In SearchQuery function ", "DataSet Rows count - " + ds.Tables[0].Rows.Count);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            def = new IWSearch
                            {
                                ID = Convert.ToInt64(ds.Tables[0].Rows[i]["Id"]),
                                XMLSerialNo = ds.Tables[0].Rows[i]["ChequeNoFinal"] == null ? "" : ds.Tables[0].Rows[i]["ChequeNoFinal"].ToString(),
                                XMLAmount = Convert.ToDecimal(ds.Tables[0].Rows[i]["FinalAmount"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["FinalAmount"]),
                                XMLPayeeName = ds.Tables[0].Rows[i]["PayeeName"] == null ? "" : ds.Tables[0].Rows[i]["PayeeName"].ToString(),
                                XMLPayorBankRoutNo = ds.Tables[0].Rows[i]["SortCodeFinal"] == null ? "" : ds.Tables[0].Rows[i]["SortCodeFinal"].ToString(),
                                XMLSAN = ds.Tables[0].Rows[i]["SANFinal"] == null ? "" : ds.Tables[0].Rows[i]["SANFinal"].ToString(),
                                XMLTrns = ds.Tables[0].Rows[i]["TransCodeFinal"] == null ? "" : ds.Tables[0].Rows[i]["TransCodeFinal"].ToString(),
                                FrontGreyImagePath = ds.Tables[0].Rows[i]["FrontGreyImagePath"] == null ? "" : ds.Tables[0].Rows[i]["FrontGreyImagePath"].ToString(),
                                chiStatus = ds.Tables[0].Rows[i]["CHIStatus"] == null ? "" : ds.Tables[0].Rows[i]["CHIStatus"].ToString(),
                                P2F = ds.Tables[0].Rows[i]["DocType"] == null ? "" : ds.Tables[0].Rows[i]["DocType"].ToString(),
                                chequedate = ds.Tables[0].Rows[i]["FinalDate"] == null ? "" : ds.Tables[0].Rows[i]["FinalDate"].ToString(),
                                CreditAccountNo = ds.Tables[0].Rows[i]["CreditAccountNo"] == null ? "" : ds.Tables[0].Rows[i]["CreditAccountNo"].ToString(),
                            };
                            objectlst.Add(def);
                        }

                    }

                    logerror("In SearchQuery function ", "Binding the IWSearch object properties - finished ");
                    return PartialView("_QuerySearchCheques", objectlst);
                }
                else
                {
                    return PartialView("_QuerySearchCheques");
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
        public PartialViewResult CheqDetls(Int64 id)
        {
            try
            {

                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter("OWGetChequeNew", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@id", SqlDbType.BigInt).Value = id;
                //adp.SelectCommand.Parameters.Add("@EOD", SqlDbType.NVarChar).Value = EOD;

                adp.Fill(ds);
                IWSearch Owsr = null;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        Owsr = new IWSearch
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
                            chiStatus = ds.Tables[0].Rows[index].ItemArray[14].ToString(),
                            L1VerificationName = ds.Tables[0].Rows[index].ItemArray[15].ToString(),
                            L2VerificationName = ds.Tables[0].Rows[index].ItemArray[16].ToString(),
                            L3VerificationName = ds.Tables[0].Rows[index].ItemArray[17].ToString(),
                            L1VerificationAction = ds.Tables[0].Rows[index].ItemArray[18].ToString(),
                            L2VerificationAction = ds.Tables[0].Rows[index].ItemArray[19].ToString(),
                            L3VerificationAction = ds.Tables[0].Rows[index].ItemArray[20].ToString(),
                            L1RejectReason = ds.Tables[0].Rows[index].ItemArray[21].ToString(),
                            L2RejectReason = ds.Tables[0].Rows[index].ItemArray[22].ToString(),
                            L3RejectReason = ds.Tables[0].Rows[index].ItemArray[23].ToString(),
                            chequedate = ds.Tables[0].Rows[index].ItemArray[24].ToString(),
                            CreditAccountNo = ds.Tables[0].Rows[index].ItemArray[25].ToString(),
                            CustomerID = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[26].ToString()),
                            ScanningID = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[27].ToString()),
                            BranchCode = ds.Tables[0].Rows[index].ItemArray[28].ToString(),
                            BatchNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[29].ToString()),
                            SlipNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[30].ToString()),
                            RejectReasonDescription = ds.Tables[0].Rows[index].ItemArray[31] != null ? ds.Tables[0].Rows[index].ItemArray[31].ToString() : "",
                            ItemSeqNo = ds.Tables[0].Rows[index].ItemArray[32] != null ? ds.Tables[0].Rows[index].ItemArray[32].ToString() : "",
                            BranchName = ds.Tables[0].Rows[index].ItemArray[33] != null ? ds.Tables[0].Rows[index].ItemArray[33].ToString() : "",
                            ReturnReason = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[34] == DBNull.Value ? 0 : ds.Tables[0].Rows[index].ItemArray[34]),
                            ReturnReasonDescription = ds.Tables[0].Rows[index].ItemArray[35] != null ? ds.Tables[0].Rows[index].ItemArray[35].ToString() : "",

                        };
                        count = count - 1;
                        index = index + 1;
                    }
                    if (Owsr.PresentmentDate != null && Owsr.PresentmentDate != "")
                        Owsr.PresentmentDate = Convert.ToDateTime(Owsr.PresentmentDate).Date.ToString("dd-MM-yyyy");
                    if (Owsr.chequedate != null && Owsr.chequedate != "")
                        Owsr.chequedate = Convert.ToDateTime(Owsr.chequedate).Date.ToString("dd-MM-yyyy");

                    //-----------------------L1action---------------------------
                    if (Owsr.L1VerificationAction != null && Owsr.L1VerificationAction != "")
                    {
                        if (Owsr.L1VerificationAction == "2")
                            Owsr.L1VerificationAction = "Y";
                        else if (Owsr.L1VerificationAction == "3")
                            Owsr.L1VerificationAction = "R";
                        else if (Owsr.L1VerificationAction == "0")
                            Owsr.L1VerificationAction = "N";

                    }
                    //-----------------------L2action---------------------------
                    if (Owsr.L2VerificationAction != null && Owsr.L2VerificationAction != "")
                    {
                        if (Owsr.L2VerificationAction == "2")
                            Owsr.L2VerificationAction = "Y";
                        else if (Owsr.L2VerificationAction == "3")
                            Owsr.L2VerificationAction = "R";
                        else if (Owsr.L2VerificationAction == "8")
                            Owsr.L2VerificationAction = "M";
                        else if (Owsr.L2VerificationAction == "0")
                            Owsr.L2VerificationAction = "N";

                    }
                    //-----------------------L3action---------------------------
                    if (Owsr.L3VerificationAction != null && Owsr.L3VerificationAction != "")
                    {
                        if (Owsr.L3VerificationAction == "2")
                            Owsr.L3VerificationAction = "Y";
                        else if (Owsr.L3VerificationAction == "3")
                            Owsr.L3VerificationAction = "R";
                        else if (Owsr.L3VerificationAction == "6")
                            Owsr.L3VerificationAction = "M";
                        else if (Owsr.L3VerificationAction == "0")
                            Owsr.L3VerificationAction = "N";

                    }
                    //-----------------------L1User Name---------------------------
                    if (Owsr.L1VerificationName != null && Owsr.L1VerificationName != "" && Owsr.L1VerificationName != "0")
                    {
                        int L1id = Convert.ToInt16(Owsr.L1VerificationName);
                        var l1result = af.UserMasters.Where(m => m.ID == L1id).FirstOrDefault();
                        Owsr.L1VerificationName = l1result.LoginID;
                    }
                    //-----------------------L2User Name---------------------------
                    if (Owsr.L2VerificationName != null && Owsr.L2VerificationName != "" && Owsr.L2VerificationName != "0")
                    {
                        int L2id = Convert.ToInt16(Owsr.L2VerificationName);
                        var l2result = af.UserMasters.Where(m => m.ID == L2id).FirstOrDefault();
                        Owsr.L2VerificationName = l2result.LoginID;
                    }
                    //-----------------------L3User Name---------------------------
                    if (Owsr.L3VerificationName != null && Owsr.L3VerificationName != "" && Owsr.L3VerificationName != "0")
                    {
                        int L3id = Convert.ToInt16(Owsr.L3VerificationName);
                        var l3result = af.UserMasters.Where(m => m.ID == L3id).FirstOrDefault();
                        Owsr.L3VerificationName = l3result.LoginID;
                    }
                    //-------------------------Reject reason L1----------------------//
                    if (Owsr.L1RejectReason != null && Owsr.L1RejectReason != "" && Owsr.L1RejectReason != "0")
                    {
                        string l1reason = Owsr.L1RejectReason.PadLeft(2, '0');

                        var L1Retrn = af.ItemReturnReasons.Where(m => m.RETURN_REASON_CODE == l1reason).FirstOrDefault();
                        Owsr.L1RejectReason = L1Retrn.DESCRIPTION;
                    }
                    //-------------------------Reject reason L2----------------------//
                    if (Owsr.L2RejectReason != null && Owsr.L2RejectReason != "" && Owsr.L2RejectReason != "0")
                    {
                        string l2reason = Owsr.L2RejectReason.PadLeft(2, '0');
                        if (l2reason == "88")
                        {
                            Owsr.L2RejectReason = Owsr.RejectReasonDescription;
                        }
                        else
                        {
                            var L2Retrn = af.ItemReturnReasons.Where(m => m.RETURN_REASON_CODE == l2reason).FirstOrDefault();
                            Owsr.L2RejectReason = L2Retrn.DESCRIPTION;
                        }

                    }
                    //-------------------------Reject reason L3----------------------//
                    if (Owsr.L3RejectReason != null && Owsr.L3RejectReason != "" && Owsr.L3RejectReason != "0")
                    {
                        string l3reason = Owsr.L3RejectReason.PadLeft(2, '0');

                        var L3Retrn = af.ItemReturnReasons.Where(m => m.RETURN_REASON_CODE == l3reason).FirstOrDefault();
                        Owsr.L3RejectReason = L3Retrn.DESCRIPTION;
                    }
                    //------------------------- Return Reason Description ----------------------//
                    //if (Owsr.ReturnReason != 0 && Owsr.ReturnReasonDescription != "")
                    //{
                    //    string l2reason = Owsr.L2RejectReason.PadLeft(2, '0');
                    //    if (l2reason == "88")
                    //    {
                    //        Owsr.L2RejectReason = Owsr.RejectReasonDescription;
                    //    }
                    //    else
                    //    {
                    //        var L2Retrn = af.ItemReturnReasons.Where(m => m.RETURN_REASON_CODE == l2reason).FirstOrDefault();
                    //        Owsr.L2RejectReason = L2Retrn.DESCRIPTION;
                    //    }

                    //}

                    return PartialView("_OwCheqDetails", Owsr);
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

        public PartialViewResult RejectReason1()
        {
            // string[] code = { "13", "30", "31", "32", "34", "35", "66" };
            //var rjrs = (from r in af.ItemReturnReasons
            //            select new RejectReason
            //            {
            //                Description = r.DESCRIPTION,
            //                ReasonCodeS = r.RETURN_REASON_CODE
            //            });


            SqlDataAdapter adpRej = new SqlDataAdapter("SP_OwItemReturnReasons", con);
            adpRej.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataSet dsRej = new DataSet();
            adpRej.Fill(dsRej);
            var objectlstRej = new List<RejectReason>();
            var objectr = new RejectReason();
            RejectReason rr;

            int countRej = dsRej.Tables[0].Rows.Count;
            int indexRej = 0;
            while (countRej > 0)
            {
                rr = new RejectReason
                {
                    ReasonCodeS = Convert.ToString((dsRej.Tables[0].Rows[indexRej].ItemArray[0])),
                    Description = Convert.ToString((dsRej.Tables[0].Rows[indexRej].ItemArray[1])),
                };
                objectlstRej.Add(rr);
                
                countRej = countRej - 1;
                indexRej = indexRej + 1;
            }

            return PartialView("_OwRejectDetails", objectlstRej);
        }

        public ActionResult getTiffImage(string httpwebimgpath = null)
        {
            try
            {

                int custid = Convert.ToInt16(Session["CustomerID"]);
                //var destpath = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "DestinationDownloadFolder");
                var destpath = af.CustomerMasters.FirstOrDefault((p) => p.Id == custid);

                //Owsr.L1VerificationName = l1result.LoginID;
                //string destroot = destpath.SettingValue;
                string destroot = destpath.PhysicalPath;

                const char delimiter = '\\';
                string[] destrootarr = destroot.Split(delimiter);

                string foldrname = destrootarr[destrootarr.Length - 1];

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

    }
}
