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
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace ikloud_Aflatoon.Controllers
{
    //[OutputCache(Duration = 0)]
    public class OWSmbVerificationController : Controller
    {
        //
        // GET: /OWL2/
        private static Logger logger = LogManager.GetCurrentClassLogger();
        AflatoonEntities af = new AflatoonEntities();
        //OWProcDataContext OWpro = new OWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
        List<string> lAccNames = new List<string>();
        List<string> lSrcFnds = new List<string>();
        string sInputString = ""; string sResposne = ""; string sgetAccountDetailsDBS = "";

        string sCasaClientId = "";
        string sCasaCorellationId = "";
        string sCasaServiceURL = "";
        string sAccountNo = "";
        public ActionResult SelectionForBranchCode(int id = 0)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            if ((bool)Session["DE"] == false)
            {

                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            //logerror("OWSMBVerificationCOntroller", "executing SelectionForBranchCode");
            //logerror("OWSMBVerificationCOntroller", "Id - " + id);
            SelectionForBranchCode selectionForBranchCode = new SelectionForBranchCode();
            try
            {

                if (id == 5)
                {
                    Session["header"] = "Data Entry";
                    Session["Title"] = "Data Entry";
                    ViewBag.header = "Data Entry";
                    ViewBag.Title = "Data Entry";
                }
                else if (id == 99)
                {
                    Session["header"] = "Cluster Verification";
                    Session["Title"] = "Cluster Verification";
                    ViewBag.header = "Cluster Verification";
                    ViewBag.Title = "Cluster Verification";
                }



                if (Session["uid"] != null)
                {
                    //int uid = (int)Session["uid"];
                    int custid = Convert.ToInt16(Session["CustomerID"]);
                    Session["VerificationId"] = id;
                    var owVeriEnableBranch = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "OWVerificationEnableBranchCodeSelection")?.SettingValue;
                    //logerror("OWSMBVerificationCOntroller", "owVeriEnableBranch - " + owVeriEnableBranch);
                    if (owVeriEnableBranch == null || owVeriEnableBranch == "")
                    {
                        ViewBag.OWVeriEnableBranch = "N";
                        return RedirectToAction("Index", "OWSmbVerification", new { id = id });
                    }
                    else
                    {
                        if (owVeriEnableBranch == "Y")
                        {
                            ViewBag.OWVeriEnableBranch = "Y";
                            //logerror("OWSMBVerificationCOntroller", "session ProType value - " + Session["ProType"]);
                            if (Session["ProType"].ToString() == "Outward")
                            {
                                string domainId = "", domainName = "", domainId1 = "", domainName1 = "";
                                //logerror("OWSMBVerificationCOntroller", "In outward - ");
                                string customerId = Session["CustomerID"].ToString();

                                if (Session["DomainselectID"] != null)
                                    domainId = Session["DomainselectID"].ToString().Trim();

                                if (Session["SelectdDomainName"] != null && Session["SelectdDomainName"].ToString().Trim() != "")
                                    domainName = Session["SelectdDomainName"].ToString();

                                if (Session["domainid"] != null)
                                    domainId1 = Session["domainid"].ToString();

                                if (Session["domainname"] != null)
                                    domainName1 = Session["domainname"].ToString();

                                string processingDate = Session["processdate"].ToString();
                                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                                string procDate = processingDate1.ToString("yyyy-MM-dd");

                                //logerror("OWSMBVerificationCOntroller", "In outward end - ");
                            }
                        }
                        else
                        {
                            ViewBag.OWVeriEnableBranch = "N";
                            return RedirectToAction("Index", "OWSmbVerification", new { id = id });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;
                Session.Abandon();
                logerrorInCatch(e.Message, e.InnerException.ToString());

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL2 Selection for branch code - " + innerExcp });
            }
            //logerror("OWSMBVerificationCOntroller", "In outward end before rendering view - ");
            return View(selectionForBranchCode);
        }

        public JsonResult SelectBranchCodes()
        {
            try
            {
                int uid = (int)Session["uid"];

                int domainId = 0;

                if (Session["DomainselectID"] != null)
                    domainId = Convert.ToInt32(Session["DomainselectID"].ToString().Trim());
                else
                    domainId = Convert.ToInt16(Session["sDomainId"].ToString().Trim());

                int customerId = Convert.ToInt32(Session["CustomerID"].ToString());
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);
                int verificationId = Convert.ToInt16(Session["VerificationId"].ToString());

                if (verificationId == 5)
                {
                    var result = FetchBranchCodeListData(verificationId);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else if (verificationId == 1)
                {
                    var result = FetchBranchCodeListData(verificationId);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else if (verificationId == 11)
                {
                    var result = FetchBranchCodeListData(verificationId);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else if (verificationId == 12)
                {
                    var result = FetchBranchCodeListData(verificationId);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                //if (domainId == 0)
                //{
                //    //var result = (from b in udb.BatchMaster
                //    //              join s in udb.ScanningType on b.ScanningType equals s.Code
                //    //              where s.KeepActive == true && b.ProcessDate == xyz && b.BranchCode == id && b.ScanningNodeId == scanningNodeId && 
                //    //              b.Branch_DE_Status == "A" && (b.Status == 16 || b.Status == 17)

                //    //              select new
                //    //              {
                //    //                  s.ID,
                //    //                  s.Description
                //    //              }).Distinct().ToList();
                //    //var result1;

                //    if (Convert.ToInt16(Session["VerificationId"].ToString()) == 5)
                //    {
                //        var result1 = (from a in af.BranchMaster
                //                       join l in af.L1Verification on a.BranchCode equals l.BranchCode
                //                       where l.ProcessingDate == xyz && (l.Status == 0 || l.Status == 8 || l.Status == 9) && l.ScanningType == 2 && l.CustomerId == customerId
                //                       select new
                //                       {
                //                           a.BranchCode,
                //                           BranchCodeName = a.BranchCode + " (" + a.BranchName + ")"
                //                       }).Distinct().ToList();
                //        return Json(result1, JsonRequestBehavior.AllowGet);
                //    }
                //    else if (Convert.ToInt16(Session["VerificationId"].ToString()) == 1)
                //    {
                //        var result1 = (from a in af.BranchMaster
                //                       join l in af.L2Verification on a.BranchCode equals l.BranchCode
                //                       where l.ProcessingDate == xyz && (l.Status == 0 || l.Status == 8 || l.Status == 9) && (l.ScanningType == 2 || l.ScanningType == 15) && l.CustomerId == customerId
                //                       select new
                //                       {
                //                           a.BranchCode,
                //                           BranchCodeName = a.BranchCode + " (" + a.BranchName + ")"
                //                       }).Distinct().ToList();
                //        return Json(result1, JsonRequestBehavior.AllowGet);
                //    }
                //    else if (Convert.ToInt16(Session["VerificationId"].ToString()) == 11)
                //    {
                //        var result1 = (from a in af.BranchMaster
                //                       join l in af.L1Verification on a.BranchCode equals l.BranchCode
                //                       where l.ProcessingDate == xyz && (l.Status == 0 || l.Status == 8 || l.Status == 9) && l.ScanningType == 11 && l.CustomerId == customerId
                //                       select new
                //                       {
                //                           a.BranchCode,
                //                           BranchCodeName = a.BranchCode + " (" + a.BranchName + ")"
                //                       }).Distinct().ToList();
                //        return Json(result1, JsonRequestBehavior.AllowGet);
                //    }
                //    else if (Convert.ToInt16(Session["VerificationId"].ToString()) == 12)
                //    {
                //        var result1 = (from a in af.BranchMaster 
                //                       join l in af.L2Verification on a.BranchCode equals l.BranchCode
                //                       where l.ProcessingDate == xyz && (l.Status == 0 || l.Status == 8 || l.Status == 9) && l.ScanningType == 11 && l.CustomerId == customerId
                //                       select new
                //                       {
                //                           a.BranchCode,
                //                           BranchCodeName = a.BranchCode + " (" + a.BranchName + ")"
                //                       }).Distinct().ToList();
                //        return Json(result1, JsonRequestBehavior.AllowGet);
                //    }

                //}
                //else
                //{
                //    if (Convert.ToInt16(Session["VerificationId"].ToString()) == 5)
                //    {
                //        var result = (from a in af.BranchMaster
                //                      join l in af.L1Verification on a.BranchCode equals l.BranchCode
                //                      where a.OwDomainId == domainId && l.ProcessingDate == xyz && (l.Status == 0 || l.Status == 8 || l.Status == 9) && l.ScanningType == 2 && l.CustomerId == customerId
                //                      select new
                //                      {
                //                          a.BranchCode,
                //                          BranchCodeName = a.BranchCode + " (" + a.BranchName + ")"
                //                      }).Distinct().ToList();

                //        return Json(result, JsonRequestBehavior.AllowGet);
                //    }
                //    else if (Convert.ToInt16(Session["VerificationId"].ToString()) == 1)
                //    {
                //        var result = (from a in af.BranchMaster
                //                      join l in af.L2Verification on a.BranchCode equals l.BranchCode
                //                      where a.OwDomainId == domainId && l.ProcessingDate == xyz && (l.Status == 0 || l.Status == 8 || l.Status == 9) && (l.ScanningType == 2 || l.ScanningType == 15) && l.CustomerId == customerId
                //                      select new
                //                      {
                //                          a.BranchCode,
                //                          BranchCodeName = a.BranchCode + " (" + a.BranchName + ")"
                //                      }).Distinct().ToList();

                //        return Json(result, JsonRequestBehavior.AllowGet);
                //    }
                //    else if (Convert.ToInt16(Session["VerificationId"].ToString()) == 11)
                //    {
                //        var result = (from a in af.BranchMaster
                //                      join l in af.L1Verification on a.BranchCode equals l.BranchCode
                //                      where a.OwDomainId == domainId && l.ProcessingDate == xyz && (l.Status == 0 || l.Status == 8 || l.Status == 9) && l.ScanningType == 11 && l.CustomerId == customerId
                //                      select new
                //                      {
                //                          a.BranchCode,
                //                          BranchCodeName = a.BranchCode + " (" + a.BranchName + ")"
                //                      }).Distinct().ToList();

                //        return Json(result, JsonRequestBehavior.AllowGet);
                //    }
                //    else if (Convert.ToInt16(Session["VerificationId"].ToString()) == 12)
                //    {
                //        var result = (from a in af.BranchMaster
                //                      join l in af.L2Verification on a.BranchCode equals l.BranchCode
                //                      where a.OwDomainId == domainId && l.ProcessingDate == xyz && (l.Status == 0 || l.Status == 8 || l.Status == 9) && l.ScanningType == 11 && l.CustomerId == customerId
                //                      select new
                //                      {
                //                          a.BranchCode,
                //                          BranchCodeName = a.BranchCode + " (" + a.BranchName + ")"
                //                      }).Distinct().ToList();

                //        return Json(result, JsonRequestBehavior.AllowGet);
                //    }

                //}
                return Json("false", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;
                Session.Abandon();
                logerrorInCatch(e.Message, e.InnerException.ToString());

                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        public class BranchCodeList
        {
            public string BranchCode { get; set; }
            public string BranchCodeName { get; set; }
        }

        public List<BranchCodeList> FetchBranchCodeListData(int verificationId = 0)
        {
            int uid = (int)Session["uid"];

            int domainId = 0;
            if (Session["DomainselectID"] != null)
                domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
            else
                domainId = Convert.ToInt16(Session["sDomainId"].ToString().Trim());

            int customerId = Convert.ToInt32(Session["CustomerID"].ToString());
            DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
            string procDate = processingDate1.ToString("yyyy-MM-dd");
            var xyz = Convert.ToDateTime(procDate);

            SqlDataAdapter adp = new SqlDataAdapter();

            if (Session["AccessLevel"].ToString().Trim() == "BRN")
            {
                adp = new SqlDataAdapter("OW_Verification_SelectBranchCodeList_BLU", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = uid;
            }
            else
            {

                adp = new SqlDataAdapter("OW_Verification_SelectBranchCodeList", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            }


            adp.SelectCommand.Parameters.Add("@DomainID", SqlDbType.NVarChar).Value = domainId;
            adp.SelectCommand.Parameters.Add("@ProcDate", SqlDbType.NVarChar).Value = procDate;
            adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.NVarChar).Value = customerId;
            adp.SelectCommand.Parameters.Add("@VerificationID", SqlDbType.NVarChar).Value = verificationId;
            DataSet ds = new DataSet();
            adp.Fill(ds);
            var objectlst = new List<BranchCodeList>();
            BranchCodeList def;

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    def = new BranchCodeList
                    {
                        BranchCode = ds.Tables[0].Rows[i].ItemArray[0].ToString(),
                        BranchCodeName = ds.Tables[0].Rows[i].ItemArray[1].ToString()
                    };
                    objectlst.Add(def);
                }

            }
            return objectlst;
        }

        [HttpPost]
        public ActionResult SelectionForBranchCode(SelectionForBranchCode selectionForBranchCode)
        {
            int verificationId = Convert.ToInt16(Session["VerificationId"].ToString());
            if (Request.Form["BranchSelect"] != null)
                Session["BranchID"] = Request.Form["BranchSelect"].ToString();
            try
            {
                var branch = Session["BranchID"].ToString();
                return RedirectToAction("Index", "OWSmbVerification", new { id = verificationId, branchId = branch });
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;
                //logerrorInCatch(e.Message, e.InnerException.ToString());

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "Selection for branch code post - " + innerExcp });
            }
        }

        public ActionResult Index(int id = 0, string branchId = null)
        {
            //vikram forr web API
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            //vikram forr web API

            //var openDate = jObject["openedDate"].ToString().Trim();

            //var dateTime = DateTimeOffset.FromUnixTimeSeconds(1550962800);
            //Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
            //var pDate = DateTimeOffset.FromUnixTimeSeconds(1550962800);

            

            //Get token no. for API
            string NewApiCall = null;
            var OwApi = af.CommonSettings.FirstOrDefault(a => a.AppName == "CTSCONFIG1" && a.SettingName == "OWVerAPI")?.SettingValue;
            if (OwApi != null && OwApi != "")
            {
                NewApiCall = OwApi.ToString().ToUpper();
            }
            else
                NewApiCall = "N";

            ViewBag.NewApiCall = NewApiCall;
            // 1 uncomment when deployed on bank start
            if (NewApiCall == "Y")
                Session["sToken"] = CreateToken();
            // 1 uncomment when deployed on bank end

            //Get token no. for API

            // module name
            ViewBag.header = Session["header"];
            ViewBag.Title = Session["Title"];


            //logerror("OWSMBVerificationCOntroller", "executing OWSelectSMBL2");
            //questionArray.Where(x=>x.QuestionId == 2).Select(x=>x.Answer);

            int custid = Convert.ToInt16(Session["CustomerID"]);
            var varMinAclen = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "ACFrom").SettingValue;
            var varMaxAclen = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "ACTo").SettingValue;
            var varMaxPayeelen = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "PayeeNameMaxLength").SettingValue;

            int intMinAclen = Convert.ToInt32(varMinAclen);
            int intMaxAclen = Convert.ToInt32(varMaxAclen);
            int intMaxPayeelen = Convert.ToInt32(varMaxPayeelen);


            ViewBag.MinAclen = intMinAclen;
            ViewBag.MaxAclen = intMaxAclen;
            ViewBag.MaxPayeelen = intMaxPayeelen;

            //var presentingBankRoutNo = (from a in af.CustomerMasters
            //                            where a.Id == custid
            //                            select
            //                                a.PresentingBankRouteNo
            //                            ).FirstOrDefault().ToString();

            //var BankCode = presentingBankRoutNo.Substring(3, 3);
            ViewBag.BankCode = Session["BankCode"].ToString();
            //Session["BankCode"] = BankCode;

            var OWIsDataEntryAllowedForAccountNo = "Y";
            var OWIsDataEntryAllowedForPayeeName = "Y";
            var OWIsDataEntryAllowedForDate = "Y";
            var OWIsDataEntryAllowedForAmount = "Y";

            ViewBag.OWIsDataEntryAllowedForAccountNo = OWIsDataEntryAllowedForAccountNo;
            ViewBag.OWIsDataEntryAllowedForPayeeName = OWIsDataEntryAllowedForPayeeName;
            ViewBag.OWIsDataEntryAllowedForDate = OWIsDataEntryAllowedForDate;
            ViewBag.OWIsDataEntryAllowedForAmount = OWIsDataEntryAllowedForAmount;

            //============= For AccountNo textbox ==================================
            var CommonSet = af.CommonSettings.Where(a => a.AppName == "CTSCONFIG1" && a.SettingName == "AccValidation").FirstOrDefault();
            if (CommonSet != null)
                Session["AccValidation"] = CommonSet.SettingValue;

            OWIsDataEntryAllowedForAccountNo = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "OWIsDataEntryAllowedForAccountNo")?.SettingValue;
            if (OWIsDataEntryAllowedForAccountNo == null || OWIsDataEntryAllowedForAccountNo == "")
            {
                ViewBag.OWIsDataEntryAllowedForAccountNo = "Y";
                //ViewBag.OWVerDisbAccNo = "N";
            }
            else
            {
                if (OWIsDataEntryAllowedForAccountNo == "Y")
                {
                    ViewBag.OWIsDataEntryAllowedForAccountNo = "Y";
                    //ViewBag.OWVerDisbAccNo = "Y";
                }
                else
                {
                    ViewBag.OWIsDataEntryAllowedForAccountNo = "N";
                    //ViewBag.OWVerDisbAccNo = "N";
                }
            }

            //============= For PayeeName textbox ==================================
            OWIsDataEntryAllowedForPayeeName = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "OWIsDataEntryAllowedForPayeeName")?.SettingValue;
            if (OWIsDataEntryAllowedForPayeeName == null || OWIsDataEntryAllowedForPayeeName == "")
            {
                ViewBag.OWIsDataEntryAllowedForPayeeName = "Y";
                //ViewBag.OWVerDisbAccNo = "N";
            }
            else
            {
                if (OWIsDataEntryAllowedForPayeeName == "Y")
                {
                    ViewBag.OWIsDataEntryAllowedForPayeeName = "Y";
                    //ViewBag.OWVerDisbAccNo = "Y";
                }
                else
                {
                    ViewBag.OWIsDataEntryAllowedForPayeeName = "N";
                    //ViewBag.OWVerDisbAccNo = "N";
                }
            }

            //============= For Date textbox ==================================
            OWIsDataEntryAllowedForDate = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "OWIsDataEntryAllowedForDate")?.SettingValue;
            if (OWIsDataEntryAllowedForDate == null || OWIsDataEntryAllowedForDate == "")
            {
                ViewBag.OWIsDataEntryAllowedForDate = "Y";
            }
            else
            {
                if (OWIsDataEntryAllowedForDate == "Y")
                {
                    ViewBag.OWIsDataEntryAllowedForDate = "Y";
                }
                else
                {
                    ViewBag.OWIsDataEntryAllowedForDate = "N";
                }
            }

            //============= For Amount textbox ==================================
            OWIsDataEntryAllowedForAmount = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "OWIsDataEntryAllowedForAmount")?.SettingValue;
            if (OWIsDataEntryAllowedForAmount == null || OWIsDataEntryAllowedForAmount == "")
            {
                ViewBag.OWIsDataEntryAllowedForAmount = "Y";
            }
            else
            {
                if (OWIsDataEntryAllowedForAmount == "Y")
                {
                    ViewBag.OWIsDataEntryAllowedForAmount = "Y";
                }
                else
                {
                    ViewBag.OWIsDataEntryAllowedForAmount = "N";
                }
            }

            //============== Setting Session variable for all text box =================
            Session["OWIsDataEntryAllowedForAccountNo"] = ViewBag.OWIsDataEntryAllowedForAccountNo;
            Session["OWIsDataEntryAllowedForPayeeName"] = ViewBag.OWIsDataEntryAllowedForPayeeName;
            Session["OWIsDataEntryAllowedForDate"] = ViewBag.OWIsDataEntryAllowedForDate;
            Session["OWIsDataEntryAllowedForAmount"] = ViewBag.OWIsDataEntryAllowedForAmount;

            if (id == 5)
            {
                ViewBag.OWVerDisbAccNo = "N";
                ViewBag.OWCdkDisbAccNo = "N";
                ViewBag.OWCdkDisbPayeeName = "N";
            }
            else
            {
                //ViewBag.OWIsDataEntryAllowedForAccountNo = OWIsDataEntryAllowedForAccountNo;
                //ViewBag.OWIsDataEntryAllowedForPayeeName = OWIsDataEntryAllowedForPayeeName;
                //ViewBag.OWIsDataEntryAllowedForDate = OWIsDataEntryAllowedForDate;
                //ViewBag.OWIsDataEntryAllowedForAmount = OWIsDataEntryAllowedForAmount;
                ////============= For AccountNo textbox ==================================
                //OWIsDataEntryAllowedForAccountNo = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "OWIsDataEntryAllowedForAccountNo")?.SettingValue;
                //if (OWIsDataEntryAllowedForAccountNo == null || OWIsDataEntryAllowedForAccountNo == "")
                //{
                //    ViewBag.OWIsDataEntryAllowedForAccountNo = "N";
                //    //ViewBag.OWVerDisbAccNo = "N";
                //}
                //else
                //{
                //    if (OWIsDataEntryAllowedForAccountNo == "Y")
                //    {
                //        ViewBag.OWIsDataEntryAllowedForAccountNo = "Y";
                //        //ViewBag.OWVerDisbAccNo = "Y";
                //    }
                //    else
                //    {
                //        ViewBag.OWIsDataEntryAllowedForAccountNo = "N";
                //        //ViewBag.OWVerDisbAccNo = "N";
                //    }
                //}

                ////============= For PayeeName textbox ==================================
                //OWIsDataEntryAllowedForPayeeName = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "OWIsDataEntryAllowedForPayeeName")?.SettingValue;
                //if (OWIsDataEntryAllowedForPayeeName == null || OWIsDataEntryAllowedForPayeeName == "")
                //{
                //    ViewBag.OWIsDataEntryAllowedForPayeeName = "N";
                //    //ViewBag.OWVerDisbAccNo = "N";
                //}
                //else
                //{
                //    if (OWIsDataEntryAllowedForPayeeName == "Y")
                //    {
                //        ViewBag.OWIsDataEntryAllowedForPayeeName = "Y";
                //        //ViewBag.OWVerDisbAccNo = "Y";
                //    }
                //    else
                //    {
                //        ViewBag.OWIsDataEntryAllowedForPayeeName = "N";
                //        //ViewBag.OWVerDisbAccNo = "N";
                //    }
                //}

                ////============= For Date textbox ==================================
                //OWIsDataEntryAllowedForDate = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "OWIsDataEntryAllowedForDate")?.SettingValue;
                //if (OWIsDataEntryAllowedForDate == null || OWIsDataEntryAllowedForDate == "")
                //{
                //    ViewBag.OWIsDataEntryAllowedForDate = "N";
                //}
                //else
                //{
                //    if (OWIsDataEntryAllowedForDate == "Y")
                //    {
                //        ViewBag.OWIsDataEntryAllowedForDate = "Y";
                //    }
                //    else
                //    {
                //        ViewBag.OWIsDataEntryAllowedForDate = "N";
                //    }
                //}

                ////============= For Amount textbox ==================================
                //OWIsDataEntryAllowedForAmount = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "OWIsDataEntryAllowedForAmount")?.SettingValue;
                //if (OWIsDataEntryAllowedForAmount == null || OWIsDataEntryAllowedForAmount == "")
                //{
                //    ViewBag.OWIsDataEntryAllowedForAmount = "N";
                //}
                //else
                //{
                //    if (OWIsDataEntryAllowedForAmount == "Y")
                //    {
                //        ViewBag.OWIsDataEntryAllowedForAmount = "Y";
                //    }
                //    else
                //    {
                //        ViewBag.OWIsDataEntryAllowedForAmount = "N";
                //    }
                //}

                ViewBag.OWVerDisbAccNo = "N";
                ViewBag.OWCdkDisbAccNo = "N";
                ViewBag.OWCdkDisbPayeeName = "N";

                var owVerificationDisableAccountNo = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "OWVerificationDisableAccountNo")?.SettingValue;
                var owCdkDisableAccountNo = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "OWCDKDisableAccountNo")?.SettingValue;
                var owCdkDisablePayeeName = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "OWCDKDisablePayeeName")?.SettingValue;

                if (owVerificationDisableAccountNo == null || owVerificationDisableAccountNo == "")
                {
                    ViewBag.OWVerDisbAccNo = "Y";
                }
                else
                {
                    if (owVerificationDisableAccountNo == "N")
                    {
                        ViewBag.OWVerDisbAccNo = "N";
                    }
                    else
                    {
                        ViewBag.OWVerDisbAccNo = "Y";
                    }
                }

                if (owCdkDisableAccountNo == null || owCdkDisableAccountNo == "")
                {
                    ViewBag.OWCdkDisbAccNo = "Y";
                }
                else
                {
                    if (owCdkDisableAccountNo == "N")
                    {
                        ViewBag.OWCdkDisbAccNo = "N";
                    }
                    else
                    {
                        ViewBag.OWCdkDisbAccNo = "Y";
                    }
                }

                if (id == 1)
                {
                    ViewBag.OWCdkDisbPayeeName = "N";
                }
                else
                {
                    if (owCdkDisablePayeeName == null || owCdkDisablePayeeName == "")
                    {
                        ViewBag.OWCdkDisbPayeeName = "Y";
                    }
                    else
                    {
                        if (owCdkDisablePayeeName == "N")
                        {
                            ViewBag.OWCdkDisbPayeeName = "N";
                        }
                        else
                        {
                            ViewBag.OWCdkDisbPayeeName = "Y";
                        }
                    }
                }
            }


            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            if ((bool)Session["DE"] == false)
            {
                
                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            
            //int uid = (int)Session["uid"];

            //if ((bool)Session["VF"] == false)
            //{

            //    UserMaster usrm = af.UserMasters.Find(uid);
            //    usrm.Active = false;
            //    af.SaveChanges();
            //    return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            //}

            //logerror("OWSMBVerificationCOntroller", "executing OWSelectSMBL2");
            //int id = 1;
            try
            {
                string VFType = "";
                if (id == 1)
                    VFType = "RNormal";
                else if (id == 2)
                    VFType = "RHold";
                else if (id == 3)
                    VFType = "BNormal";
                else if (id == 4)
                    VFType = "BHold";
                if (id == 5)
                    VFType = "RNormalL1";
                else if (id == 11)
                    VFType = "CDKL1";
                else if (id == 12)
                    VFType = "CDKL2";

                Session["VFType"] = VFType;
                Session["VFTypeID"] = id;
                ViewBag.ScanningType = id;

                //logerror("OWSMBVerificationCOntroller", "executing OWSelectSMBL2");

                SqlDataAdapter adp = new SqlDataAdapter("[OWSelectSMBL2]", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@VFtype", SqlDbType.NVarChar).Value = VFType;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();
                //-------------Added on 17-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                //-------------Added on 18-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);

                //-------------Added on 12-09-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = Session["CtsSessionType"];
                adp.SelectCommand.Parameters.Add("@BranchCode", SqlDbType.NVarChar).Value = branchId;


                DataSet ds = new DataSet();
                adp.Fill(ds);

                SMBVerificationView def;

                //logerror("OWSMBVerificationCOntroller", "OWSelectSMBL2 executed count found.." + ds.Tables[0].Rows.Count);

                if (ds.Tables.Count > 0)
                {

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (id == 5)
                        {
                            //logerror("OWSMBVerificationCOntroller", "OWSelectSMBL2 executed line no - 768");
                            //post date stale date
                            Session["PostDate1"] = ds.Tables[0].Rows[0]["PostDate"].ToString();
                            Session["StaleDate1"] = ds.Tables[0].Rows[0]["StaleDate"].ToString();

                            ViewBag.vbFrontTiffImage = ds.Tables[0].Rows[0]["FrontTiffImagePath"].ToString();
                            ViewBag.vbFrontGreyImage = ds.Tables[0].Rows[0]["FrontGreyImagePath"].ToString();
                            ViewBag.vbBackTiffImage = ds.Tables[0].Rows[0]["BackTiffImagePath"].ToString();
                            ViewBag.vbBackGreyImage = ds.Tables[0].Rows[0]["BackGreyImagePath"].ToString();

                            //For ICR
                            string sChqDate, sChqAmt, sChqAcno;

                            if (ds.Tables[0].Rows[0]["FinalDate"] != null && ds.Tables[0].Rows[0]["FinalDate"].ToString().Trim() != "")
                                sChqDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["FinalDate"]).ToString("ddMMyy").Trim();
                            else
                                sChqDate = "";

                            if (ds.Tables[0].Rows[0]["FinalAmount"] != null && ds.Tables[0].Rows[0]["FinalAmount"].ToString().Trim() != "")
                                sChqAmt = ds.Tables[0].Rows[0]["FinalAmount"].ToString().Trim();
                            else
                                sChqAmt = "";

                            if (ds.Tables[0].Rows[0]["FinalAccountNo"] != null && ds.Tables[0].Rows[0]["FinalAccountNo"].ToString().Trim() != "")
                                sChqAcno = ds.Tables[0].Rows[0]["FinalAccountNo"].ToString().Trim();
                            else
                                sChqAcno = "";


                            // sChqDate = ""; sChqAmt = ""; sChqAcno = "";

                            def = new SMBVerificationView
                            {
                                L2Id = Convert.ToInt32(ds.Tables[0].Rows[0]["Id"].ToString()),
                                BatchNo = Convert.ToInt32(ds.Tables[0].Rows[0]["BatchNo"].ToString()),
                                BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[0]["BatchSeqNo"].ToString()),
                                captureRawId = Convert.ToInt64(ds.Tables[0].Rows[0]["RawDataId"].ToString()),
                                InstrumentType = ds.Tables[0].Rows[0]["InstrumentType"].ToString(),
                                BranchCode = ds.Tables[0].Rows[0]["BranchCode"].ToString(),
                                CustomerId = Convert.ToInt32(ds.Tables[0].Rows[0]["CustomerId"].ToString()),
                                DomainId = Convert.ToInt32(ds.Tables[0].Rows[0]["DomainId"].ToString()),
                                ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[0]["ScanningNodeId"].ToString()),
                                FrontTiffImage = ds.Tables[0].Rows[0]["FrontTiffImagePath"].ToString(),
                                FrontGreyImage = ds.Tables[0].Rows[0]["FrontGreyImagePath"].ToString(),
                                BackTiffImage = ds.Tables[0].Rows[0]["BackTiffImagePath"].ToString(),
                                BackGreyImage = ds.Tables[0].Rows[0]["BackGreyImagePath"].ToString(),
                                ChqDate = sChqDate,
                                ChqAmt = sChqAmt,
                                ChqAcno = sChqAcno,
                                ChqPayeeName = "",
                                CBSAccountInformation = ds.Tables[0].Rows[0]["CBSAccountInformation"].ToString(),
                                CBSJointAccountInformation = ds.Tables[0].Rows[0]["CBSJointAccountInformation"].ToString(),
                                FinalChqNo = ds.Tables[0].Rows[0]["ChequeNoFinal"].ToString(),
                                FinalSortcode = ds.Tables[0].Rows[0]["SortCodeFinal"].ToString(),
                                FinalSAN = ds.Tables[0].Rows[0]["SANFinal"].ToString(),
                                FinalTransCode = ds.Tables[0].Rows[0]["TransCodeFinal"].ToString(),
                                ScanningType = Convert.ToInt32(ds.Tables[0].Rows[0]["ScanningType"].ToString()),
                                FrontUVImage = ds.Tables[0].Rows[0]["FrontUVImage"].ToString(),
                            };

                            ViewBag.ChequeAmount = "";
                            //logerror("OWSMBVerificationCOntroller", "OWSelectSMBL2 executed line no - 829");
                        }
                        else
                        {
                            //logerror("OWSMBVerificationCOntroller", "OWSelectSMBL2 executed line no - 833");
                            def = new SMBVerificationView
                            {
                                L2Id = Convert.ToInt32(ds.Tables[0].Rows[0]["Id"].ToString()),
                                BatchNo = Convert.ToInt32(ds.Tables[0].Rows[0]["BatchNo"].ToString()),
                                BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[0]["BatchSeqNo"].ToString()),
                                captureRawId = Convert.ToInt64(ds.Tables[0].Rows[0]["RawDataId"].ToString()),
                                InstrumentType = ds.Tables[0].Rows[0]["InstrumentType"].ToString(),
                                BranchCode = ds.Tables[0].Rows[0]["BranchCode"].ToString(),
                                CustomerId = Convert.ToInt32(ds.Tables[0].Rows[0]["CustomerId"].ToString()),
                                DomainId = Convert.ToInt32(ds.Tables[0].Rows[0]["DomainId"].ToString()),
                                ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[0]["ScanningNodeId"].ToString()),
                                FrontTiffImage = ds.Tables[0].Rows[0]["FrontTiffImagePath"].ToString(),
                                FrontGreyImage = ds.Tables[0].Rows[0]["FrontGreyImagePath"].ToString(),
                                BackTiffImage = ds.Tables[0].Rows[0]["BackTiffImagePath"].ToString(),
                                BackGreyImage = ds.Tables[0].Rows[0]["BackGreyImagePath"].ToString(),
                                ChqDate = ds.Tables[0].Rows[0]["Imported_SMBChqDate"].ToString(),
                                ChqAmt = ds.Tables[0].Rows[0]["Imported_SMBChqAmount"].ToString(),
                                ChqAcno = ds.Tables[0].Rows[0]["Imported_SMBChqAccNo"].ToString(),
                                ChqPayeeName = ds.Tables[0].Rows[0]["Imported_SMBPayeeName"].ToString(),
                                CBSAccountInformation = ds.Tables[0].Rows[0]["CBSAccountInformation"].ToString(),
                                CBSJointAccountInformation = ds.Tables[0].Rows[0]["CBSJointAccountInformation"].ToString(),
                                FinalChqNo = ds.Tables[0].Rows[0]["ChequeNoFinal"].ToString(),
                                FinalSortcode = ds.Tables[0].Rows[0]["SortCodeFinal"].ToString(),
                                FinalSAN = ds.Tables[0].Rows[0]["SANFinal"].ToString(),
                                FinalTransCode = ds.Tables[0].Rows[0]["TransCodeFinal"].ToString(),
                                ScanningType = Convert.ToInt32(ds.Tables[0].Rows[0]["ScanningType"].ToString()),
                                FrontUVImage = ds.Tables[0].Rows[0]["FrontUVImage"].ToString(),
                            };


                            //logerror("OWSMBVerificationCOntroller", "OWSelectSMBL2 executed line no - 864");

                            //------------------------END------------------------//
                            int index = 0;


                            //----------------------------------------Cheque field formatting-----------------------------------
                            //ddmmyy 2019-01-14
                            //140119

                            string ChqDateold = ds.Tables[0].Rows[0]["Imported_SMBChqDate"].ToString();

                            if (ChqDateold.Length == 6)
                            {
                                ChqDateold = "20" + ChqDateold.Substring(4, 2) + ChqDateold.Substring(2, 2) + ChqDateold.Substring(0, 2);
                                ChqDateold = ChqDateold.Substring(6, 2) + ChqDateold.Substring(4, 2) + ChqDateold.Substring(2, 2);
                            }
                            else
                            {
                                ChqDateold = ChqDateold.Substring(8, 2) + ChqDateold.Substring(5, 2) + ChqDateold.Substring(2, 2);
                            }

                            def.ChqDate = ChqDateold;

                            //----------------------------------------Account field formatting-----------------------------------
                            string SMBChqAccNo = ds.Tables[0].Rows[0]["Imported_SMBChqAccNo"].ToString();

                            if (SMBChqAccNo.Length > intMaxAclen)
                            {
                                SMBChqAccNo = SMBChqAccNo.Substring(1, intMaxAclen);
                            }
                            else if (SMBChqAccNo.Length < intMinAclen)
                            {
                                SMBChqAccNo = SMBChqAccNo.PadLeft(intMinAclen, '0');
                            }
                            def.ChqAcno = SMBChqAccNo;

                            //----------------------------------------Payeename field formatting-----------------------------------
                            string SMBChqPayeeName = ds.Tables[0].Rows[0]["Imported_SMBPayeeName"].ToString().Trim();
                            if (SMBChqPayeeName.Length > intMaxPayeelen)
                            {
                                SMBChqPayeeName = SMBChqPayeeName.Substring(1, intMaxPayeelen);
                            }

                            def.ChqPayeeName = SMBChqPayeeName;
                            //-----------------------------------------------------------------------------------------------------
                            ViewBag.ChequeAmount = ds.Tables[0].Rows[0]["Imported_SMBChqAmount"].ToString();
                            //logerror("OWSMBVerificationCOntroller", "OWSelectSMBL2 executed line no - 911");
                        }

                        //logerror("OWSMBVerificationCOntroller", "OWSelectSMBL2 executed line no - 914");

                        ViewBag.FrontGrey = ds.Tables[0].Rows[0]["FrontGreyImagePath"].ToString() == "" ? "" : ds.Tables[0].Rows[0]["FrontGreyImagePath"].ToString();
                        ViewBag.FrontTiff = ds.Tables[0].Rows[0]["FrontTiffImagePath"].ToString() == "" ? "" : ds.Tables[0].Rows[0]["FrontTiffImagePath"].ToString();
                        ViewBag.BackGrey = ds.Tables[0].Rows[0]["BackGreyImagePath"].ToString() == "" ? "" : ds.Tables[0].Rows[0]["BackGreyImagePath"].ToString();
                        ViewBag.BackTiff = ds.Tables[0].Rows[0]["BackTiffImagePath"].ToString() == "" ? "" : ds.Tables[0].Rows[0]["BackTiffImagePath"].ToString();
                        ViewBag.FrontUV = ds.Tables[0].Rows[0]["FrontUVImage"].ToString() == "" ? "" : ds.Tables[0].Rows[0]["FrontUVImage"].ToString();


                        //For UV image above 200000
                        string sFinalAmount = ds.Tables[0].Rows[0]["FinalAmount"].ToString().Trim();

                        if (Convert.ToInt32(sFinalAmount.Substring(0, sFinalAmount.Length - 3)) >= 200000)
                            ViewBag.DefaultImage = ViewBag.FrontUV;
                        else
                            ViewBag.DefaultImage = ViewBag.FrontGrey;
                        //For UV image above 200000

                        Session["ScanningTypeId"] = Convert.ToInt32(ds.Tables[0].Rows[0]["ScanningType"].ToString());

                        var rtnlist = (from i in af.ItemReturnReasons select i).ToList();// iwafl.ItemReturnReasons.Select(m).ToList();
                        ViewBag.rtnlist = rtnlist.Select(m => m.RETURN_REASON_CODE).ToList();
                        ViewBag.rtnlistDescrp = rtnlist.Select(m => m.DESCRIPTION).ToList();
                        //-------------------------------For Narration Accounts---------------------
                        ViewBag.narration = (from n in af.NarrationAccount where n.status == 1 select n.AccountNumber).ToList();
                        //-------------------------------For SchemCode---------------------
                        ViewBag.SchemCode = (from n in af.NreNroAccountList where n.IsActive == true select n.SchemCode).ToList();

                        @Session["glob"] = null;
                        ViewBag.cnt = true;

                        //logerror("OWSMBVerificationCOntroller", "OWSelectSMBL2 executed line no - 945");

                        if (id == 12 || id == 2)
                        {
                            var L1RejectCode = ds.Tables[0].Rows[0]["L1RejectReason"].ToString();
                            //var L1RejectDescp = (from i in af.ItemReturnReasons where i.RETURN_REASON_CODE == L1RejectCode select i.DESCRIPTION);
                            var L1RejectDescp = af.ItemReturnReasons.FirstOrDefault((p) => p.RETURN_REASON_CODE == L1RejectCode)?.DESCRIPTION;
                            if (L1RejectDescp == null || L1RejectDescp == "")
                            {
                                ViewBag.L1RejectDesc = "";
                            }
                            else
                            {
                                ViewBag.L1RejectDesc = L1RejectDescp.ToString();
                            }

                            var L2RejectCode = ds.Tables[0].Rows[0]["RejectReason"].ToString();
                            //var L1RejectDescp = (from i in af.ItemReturnReasons where i.RETURN_REASON_CODE == L1RejectCode select i.DESCRIPTION);
                            var L2RejectDescp = af.ItemReturnReasons.FirstOrDefault((p) => p.RETURN_REASON_CODE == L2RejectCode)?.DESCRIPTION;
                            if (L2RejectDescp == null || L2RejectDescp == "")
                            {
                                ViewBag.L2RejectDesc = "";
                            }
                            else
                            {
                                ViewBag.L2RejectDesc = L2RejectDescp.ToString();
                            }
                        }


                        if (id == 12 || id == 2)
                        {
                            ViewBag.L1Modified = ds.Tables[0].Rows[0]["L1Modified"].ToString();
                            ViewBag.L2Modified = ds.Tables[0].Rows[0]["L2Modified"].ToString();
                            ViewBag.L1Status = Convert.ToInt16(ds.Tables[0].Rows[0]["L1VerificationStatus"].ToString());
                            ViewBag.L2Status = Convert.ToInt16(ds.Tables[0].Rows[0]["Status"].ToString());
                            Session["L1Status"] = Convert.ToInt16(ds.Tables[0].Rows[0]["L1VerificationStatus"].ToString());
                            Session["L2Status"] = Convert.ToInt16(ds.Tables[0].Rows[0]["Status"].ToString());

                        }
                        else
                        {
                            //if(id == 2)
                            //{
                            //    ViewBag.L2Modified = ds.Tables[0].Rows[0]["L2Modified"].ToString();
                            //}
                            //else
                            //{
                            //    ViewBag.L2Modified = "00000000";
                            //}
                            //ViewBag.L1Status = 2;
                            //ViewBag.L2Status = Convert.ToInt16(ds.Tables[0].Rows[0]["status"].ToString());
                            //Session["L1Status"] = 2;
                            //Session["L2Status"] = Convert.ToInt16(ds.Tables[0].Rows[0]["status"].ToString());
                            if (id == 5)
                            {
                                ViewBag.L1Status = Convert.ToInt16(ds.Tables[0].Rows[0]["Status"].ToString());
                                Session["L1Status"] = Convert.ToInt16(ds.Tables[0].Rows[0]["Status"].ToString());
                                ViewBag.L2Status = 0;
                                Session["L2Status"] = 0;
                            }
                            else
                            {
                                ViewBag.L1Status = 2;
                                ViewBag.L2Status = 0;
                                Session["L1Status"] = 2;
                                Session["L2Status"] = 0;
                            }
                            ViewBag.L1Modified = "00000000";
                            ViewBag.L2Modified = "00000000";
                        }
                        ViewBag.block = 0;
                        ViewBag.Currency = "";
                        //logerror("OWSMBVerificationCOntroller", "OWSelectSMBL2 executed line no - 1017");
                        return View(def);
                    }

                }
                else
                    return RedirectToAction("IWIndex", "Home", new { id = 4 });
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                Session.Abandon();
                //return PartialView("Error", er);
                logerrorInCatch(e.Message, e.InnerException.ToString());

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL2 HttpGet Index- " + innerExcp });
            }

            return RedirectToAction("IWIndex", "Home", new { id = 4 });
        }

        private void logerror(string errormsg, string errordesc)
        {
            var writeLog = ConfigurationManager.AppSettings["WriteLog"].ToString().ToUpper();

            if (writeLog == "Y")
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
                
        }

        private void logerrorInCatch(string errormsg, string errordesc)
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

        [HttpPost]
        public ActionResult OWSMBL2(SMBVerificationView smbver, string btnSubmit, string msg, string Decision, string MarkP2f, string IWRemark, string rejectreasondescrpsn, string realModified, string modified)
        {

            //if (Session["uid"] == null)
            //{
            //    return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            //}
            //if ((bool)Session["VF"] == false)
            //{
            //    int uid1 = (int)Session["uid"];
            //    UserMaster usrm = af.UserMasters.Find(uid1);
            //    usrm.Active = false;
            //    af.SaveChanges();
            //    return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            //}

            //IWAmountTmpProcess jt;
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            if ((bool)Session["DE"] == false)
            {
                
                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            //int uid = (int)Session["uid"];//That will be Session value.
            int ttcnt = 0;
            try
            {

                if (btnSubmit == "Close")
                {
                    string verification = Session["VFType"].ToString();
                    //OWpro.UnlockRecordsForOutwardVerification(smbver.L2Id, verification);
                    /// Int64 SlipRawaDataID = 0;

                    
                    SqlCommand com = new SqlCommand("UnlockRecordsForOutwardVerification", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@id", smbver.L2Id);
                    com.Parameters.AddWithValue("@module", verification);

                    con.Open();
                    int i = com.ExecuteNonQuery();
                    con.Close();


                    Session["glob"] = true;
                    return RedirectToAction("IWIndex", "Home", new { id = 3 });
                }
                double camt = Convert.ToDouble(Request.Form["ChqAmt"]);

                string cdate = Convert.ToString(Request.Form["ChqDate"]);
                string cdatenew = "";

                if (Decision.ToUpper() == "A")
                {
                    if (Session["OWIsDataEntryAllowedForDate"].ToString().ToUpper() == "Y")
                    {
                        cdatenew = "20" + cdate.Substring(4, 2) + "-" +
                        cdate.Substring(2, 2) + "-" +
                        cdate.Substring(0, 2);
                    }
                    else
                    {
                        cdatenew = "20" + cdate.Substring(4, 2) + "-" +
                        cdate.Substring(2, 2) + "-" +
                        cdate.Substring(0, 2);
                    }
                }
                    
                //DateTime dt = DateTime.ParseExact(cdatenew, "yyyy-MM-dd", null);

                string cacno = Convert.ToString(Request.Form["ChqAcno"]);
                string cpayee = Convert.ToString(Request.Form["ChqPayeeName"]);
                string cfinalchqno = Convert.ToString(Request.Form["FinalChqNo"]);
                string cfianlsortcode = Convert.ToString(Request.Form["FinalSortcode"]);
                string cfinalsan = Convert.ToString(Request.Form["FinalSAN"]);
                string cfialtrcd = Convert.ToString(Request.Form["FinalTransCode"]);
                string cnartext = Convert.ToString(Request.Form["nartext"]);
                string cDraweeName = Convert.ToString(Request.Form["DraweeName"]);

                string NREtxtSrcFnds = "";// Convert.ToString(Request.Form["txtSrcFnds"]);
                string NROtxtSrcFnds = "";

                int cNRESourceOfFundId = 0, cNROSourceOfFundId = 0;

                if (Decision.ToUpper() == "A")
                {
                    if (Session["sNR"] != null)
                    {
                        if (Session["sNR"].ToString().Trim() == "NRE")
                        {
                            cNRESourceOfFundId = Convert.ToInt32(Request.Form["ddSrcFnds"]);
                            cNROSourceOfFundId = 0;
                            NREtxtSrcFnds= Convert.ToString(Request.Form["txtSrcFnds"]);
                            NROtxtSrcFnds = "";
                        }
                        else
                        {
                            cNRESourceOfFundId = 0;
                            cNROSourceOfFundId = Convert.ToInt32(Request.Form["ddSrcFnds"]);
                            NREtxtSrcFnds = "";
                            NROtxtSrcFnds = Convert.ToString(Request.Form["txtSrcFnds"]);
                        }
                    }
                    else
                    {
                        cNRESourceOfFundId = 0;
                        cNROSourceOfFundId = Convert.ToInt32(Request.Form["ddSrcFnds"]);
                        NREtxtSrcFnds= "";
                        NROtxtSrcFnds = Convert.ToString(Request.Form["txtSrcFnds"]);
                    }
                }
                    

                //========= Amol changes on 27/02/2024 for capturing api details start ============
                string api_data = "";
                string sourceCustomerId = "";
                string currency = "";
                string isOpenedDateOld = "";

                //sambhaji added 30-11-24
                string ProductCode = "";
                string ProductType = "";
                string accountBal = "";
                string accountStatus = "";
                string FreezstatusCode = "";
                string solid = "";
                string staffAcc = "";
                string offAcc = "";
                string opendate = "";
                string MOP = "";


                //mop
                if (Session["Mop"] != null)
                {
                    MOP = Session["Mop"].ToString().Trim();
                }
                else
                {
                    MOP = "";
                }
                //solid
                if (Session["SolID"] != null)
                {
                    solid = Session["SolID"].ToString().Trim();
                }
                else
                {
                    solid = "";
                }

                //staffac
                if(Session["StaffAcc"] != null)
                {
                    staffAcc = Session["StaffAcc"].ToString().Trim();
                }
                else
                {
                    staffAcc = "";
                }

                //offacc
                if (Session["OffAcc"] != null)
                {
                    offAcc = Session["OffAcc"].ToString().Trim();
                }
                else
                {
                    offAcc = "";
                }


                if(Session["Opendate"] != null)
                {
                    opendate = Session["Opendate"].ToString().Trim();
                }
                else
                {
                    opendate = "";
                }

                //product code
                if (Session["productCode"] != null) 
                {
                    ProductCode = Session["productCode"].ToString().Trim();
                }
                else
                {
                    ProductCode = "";
                }

                //product type
                if (Session["productType"] != null)
                {
                    ProductType = Session["productType"].ToString().Trim();
                }
                else
                {
                    ProductType = "";
                }
                //accountBal
                if (Session["accountBalances"] != null)
                {
                    accountBal = Session["accountBalances"].ToString().Trim();
                }
                else
                {
                    accountBal = "";
                }

                //accountStatus
                if (Session["AccountStatus"]!=null)
                {
                    accountStatus = Session["AccountStatus"].ToString().Trim();
                }
                else
                {
                    accountStatus = "";
                }

                if (Session["FreezeStatusCode"] != null)
                {
                    FreezstatusCode = Session["FreezeStatusCode"].ToString().Trim();
                }
                else
                {
                    FreezstatusCode = "";
                }

                //30-11-24

                if (Session["SourceCustomerId"] != null)
                {
                    sourceCustomerId = Session["SourceCustomerId"].ToString().Trim();
                }
                else
                {
                    sourceCustomerId = "";
                }

                if (Session["AccountCurrency"] != null)
                {
                    currency = Session["AccountCurrency"].ToString().Trim();
                }
                else
                {
                    currency = "";
                }

                if (Session["IsOpenedDateOld"] != null)
                {
                    isOpenedDateOld = Session["IsOpenedDateOld"].ToString().Trim();
                }
                else
                {
                    isOpenedDateOld = "";
                }
                //========= Amol changes on 27/02/2024 for capturing api details end ============

                if (IWRemark == "")
                {
                    IWRemark = "0";
                }

                bool p2f = Convert.ToBoolean(MarkP2f);
                bool ignoreiqa;
                string doctype = "";

                if (p2f)
                {
                    ignoreiqa = true;
                    doctype = "C";
                }
                else
                {
                    ignoreiqa = false;
                    doctype = "B";
                }

                string modaction = "";
                string modactionL1 = "";
                bool Modified = false;
                string verification1 = Session["VFType"].ToString();
                if (verification1 == "RNormalL1")
                {
                    Modified = false;
                    modified = "00000000";
                }
                else
                {
                    Modified = Convert.ToBoolean(realModified);
                    //modified = modified;
                }

                //logerror(smbver.captureRawId.ToString(), smbver.captureRawId.ToString() + " - > Actual values from RawDataId");
                //logerror(Decision, Decision.ToString() + " - > Actual values from texbox Decision");
                //logerror(modified, modified.ToString() + " - > Actual values from texbox modified");
                //Decision = "A";
                if (Decision.ToUpper() == "A")
                {
                    if (Modified == true)
                    {
                        modaction = "M";
                    }
                    else
                    {
                        modaction = "A";
                    }
                }
                else if (Decision.ToUpper() == "R")
                {
                    modaction = "R";
                }
                else if (Decision.ToUpper() == "H")
                {
                    modaction = "H";
                }

                //logerror(modaction, modaction.ToString() + " - > value modaction");

                string L1Status = Session["L1Status"].ToString();
                string L2Status = Session["L2Status"].ToString();

                //logerror(L1Status, L1Status.ToString() + " - > value L1Status from session");
                //logerror(L2Status, L2Status.ToString() + " - > value L2Status from session");

                if (L1Status == "2" || L1Status == "8")
                {
                    modactionL1 = "A";
                }
                else
                {
                    modactionL1 = "R";
                }

                //logerror(modactionL1, modactionL1.ToString() + " - > value modactionL1");

                int verificationId = Convert.ToInt16(Session["VerificationId"].ToString());

                var branch = "";
                if (Session["BranchID"] != null)
                {
                    branch = Session["BranchID"].ToString();
                }
                //var branch = Session["BranchID"].ToString();
                var scanningTypeId = Session["ScanningTypeId"].ToString();

                if (btnSubmit == "Ok")
                {
                    string verification = Session["VFType"].ToString();
                    //logerror(verification, verification.ToString() + " - > IN Ok value verification");

                    if (verification == "CDKL1" || verification == "RNormalL1")
                    {
                        //logerror("Start Update L1 query", "Start Update L1 query" + " - > IN Ok L1 verification value Start Update L1 query");
                        //logerror(smbver.L2Id.ToString(), smbver.L2Id.ToString() + " - > IN Ok L1 verification value Id");
                        //logerror(modaction.ToString(), modaction.ToString() + " - > IN Ok L1 verification value modaction");
                        //logerror(modified.ToString(), modified.ToString() + " - > IN Ok L1 verification value modified");

                        //OWpro.UpdateSMBVerificationL1(smbver.L2Id,
                        //        smbver.captureRawId,
                        //        uid,
                        //        smbver.InstrumentType,
                        //        camt,
                        //        //Convert.ToDateTime(cdatenew).ToString("yyyy-MM-dd"),
                        //        (cdatenew == "" ? DateTime.Now.ToString("yyyy-MM-dd") : cdatenew),
                        //        cfinalchqno,
                        //        cfianlsortcode,
                        //        cfinalsan,
                        //        cfialtrcd,
                        //        cacno,
                        //        cpayee,
                        //        2,//status 
                        //        Convert.ToByte(IWRemark), //reject reason
                        //        modaction, //actiontaken
                        //        @Session["LoginID"].ToString(),
                        //        Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"),
                        //        smbver.CustomerId,
                        //        smbver.DomainId,
                        //        smbver.ScanningNodeId,
                        //        0,//chqamttotal 
                        //        0,//slipamttotal
                        //        0, //ChequeTotal
                        //        cnartext,//userNarration, 
                        //        rejectreasondescrpsn,//rejectreasondescrpsn, 
                        //       Convert.ToString(Session["CtsSessionType"]),
                        //        "",//@CBSAccountInformation
                        //        "",//@CBSJointAccountInformation 
                        //        ignoreiqa,//@IgnoreIQA, 
                        //        doctype,//@DocType 
                        //        modified,//@Modified, 
                        //        null,
                        //        cDraweeName,
                        //        cNRESourceOfFundId,
                        //        cNROSourceOfFundId);//@strHoldReason

                        // api_data = sourceCustomerId + "|" + currency ;  old 


                       //           sourceCustomerId | accountCurrencyCode | openedDate | productCode | productType | accountBalances | modeOfOperation | accountStatus | staffIndicator | freezeStatusCode | openDate | SOLID | offAcc
                        api_data = sourceCustomerId + "|" + currency + "|" + isOpenedDateOld + "|" + ProductCode + "|" + ProductType + "|" + accountBal + "|" + MOP + "|" + accountStatus + "|" + staffAcc + "|" + FreezstatusCode + "|" + opendate + "|" + solid + "|" + offAcc;

                        SqlCommand cmd = new SqlCommand("UpdateSMBVerificationL1", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", smbver.L2Id);
                        cmd.Parameters.AddWithValue("@RawDataId", smbver.captureRawId);
                        cmd.Parameters.AddWithValue("@Uid", uid);
                        cmd.Parameters.AddWithValue("@InstrumentType", smbver.InstrumentType);
                        cmd.Parameters.AddWithValue("@FinalAmount", camt);
                        cmd.Parameters.AddWithValue("@FinalDate", (cdatenew == "" ? DateTime.Now.ToString("yyyy-MM-dd") : cdatenew));
                        cmd.Parameters.AddWithValue("@FinalChqNo", cfinalchqno);
                        cmd.Parameters.AddWithValue("@FinalSortcode", cfianlsortcode);
                        cmd.Parameters.AddWithValue("@FinalSAN", cfinalsan);
                        cmd.Parameters.AddWithValue("@FinalTransCode", cfialtrcd);
                        cmd.Parameters.AddWithValue("@CreditAccountNo", cacno);
                        cmd.Parameters.AddWithValue("@PayeName", cpayee);
                        cmd.Parameters.AddWithValue("@status", 2);
                        cmd.Parameters.AddWithValue("@RejectReason", Convert.ToByte(IWRemark));
                        cmd.Parameters.AddWithValue("@ActionTaken", modaction);
                        cmd.Parameters.AddWithValue("@LName", @Session["LoginID"].ToString());
                        cmd.Parameters.AddWithValue("@ProcessingDate", Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@CustomerId", smbver.CustomerId);
                        cmd.Parameters.AddWithValue("@DomainId", smbver.DomainId);
                        cmd.Parameters.AddWithValue("@ScanningNodeId", smbver.ScanningNodeId);
                        cmd.Parameters.AddWithValue("@ChequeAmtotal", 0);
                        cmd.Parameters.AddWithValue("@SlipAmount", 0);
                        cmd.Parameters.AddWithValue("@ChequeTotal", 0);
                        cmd.Parameters.AddWithValue("@UserNarration", (cnartext == null ? "" : cnartext));
                        cmd.Parameters.AddWithValue("@RejectReasonDescription", rejectreasondescrpsn);
                        cmd.Parameters.AddWithValue("@CTSNONCTS", Convert.ToString(Session["CtsSessionType"]));
                        cmd.Parameters.AddWithValue("@CBSAccountInformation", "");
                        cmd.Parameters.AddWithValue("@CBSJointAccountInformation", "");
                        cmd.Parameters.AddWithValue("@IgnoreIQA", ignoreiqa);
                        cmd.Parameters.AddWithValue("@DocType", doctype);
                        cmd.Parameters.AddWithValue("@Modified", modified);
                        cmd.Parameters.AddWithValue("@strHoldReason", "");
                        cmd.Parameters.AddWithValue("@DraweeName", cDraweeName);
                        cmd.Parameters.AddWithValue("@NRESourceOfFundId", cNRESourceOfFundId);
                        cmd.Parameters.AddWithValue("@NROSourceOfFundId", cNROSourceOfFundId);

                        //=========== Added by Amol on 17/02/2024 for High Value L3 =======================
                        cmd.Parameters.AddWithValue("@HighValueAmountL3", Convert.ToDouble(Session["HIGHAMTL3"]));

                        //=========== Added by Amol on 27/02/2024 for API data =======================
                        cmd.Parameters.AddWithValue("@API_Data", api_data);

                        //=========== Added by Amol on 20/03/2024 for openedDate =======================
                        cmd.Parameters.AddWithValue("@IsOpenedDateOld", isOpenedDateOld);

                        cmd.Parameters.AddWithValue("@SrcFndsDescription", NREtxtSrcFnds);
                        cmd.Parameters.AddWithValue("@NROSrcFndsDescription", NROtxtSrcFnds);

                        if (con.State == ConnectionState.Closed)
                            con.Open();

                        int result = cmd.ExecuteNonQuery();
                        con.Close();

                        //logerror("Finished Update L1 query", "Finished Update L1 query" + " - > IN Ok L1 verification value Finished Update L1 query");
                        //return RedirectToAction("Index", "OWSmbVerification", new { id = Session["VFTypeID"] });
                        return RedirectToAction("Index", "OWSmbVerification", new { id = verificationId, branchId = branch });
                    }
                    //else if(verification == "RNormalL1")
                    //{

                    //}
                    else
                    {
                        //logerror("Start Update L2 query", "Start Update L2 query" + " - > IN Ok L2 verification value Start Update L2 query");
                        //logerror(L2Status, L2Status.ToString() + " - > IN Ok L2 verification value L2Status");
                        //logerror(modaction, modaction.ToString() + " - > IN Ok L2 verification value modaction");


                        string modifiedAction = "";
                        if (L2Status == "8" || L2Status == "9")
                        {
                            if (modaction == "A" || modaction == "M")
                            {
                                modifiedAction = "2";
                            }
                            else if (modaction == "R")
                            {
                                modifiedAction = "3";
                            }

                            //logerror(modifiedAction, modifiedAction.ToString() + " - > IN Ok L2 verification In L2Staus 8 or 9 if clause value modifiedAction");
                        }
                        else
                        {
                            //logerror(scanningTypeId, scanningTypeId.ToString() + " - > IN Ok L2 verification In L2Staus 8 or 9 else clause value scanningTypeId");
                            if (scanningTypeId == "2" || scanningTypeId == "15")
                            {
                                if (modaction == "A" || modaction == "M")
                                {
                                    modifiedAction = "2";
                                }
                                else if (modaction == "R")
                                {
                                    modifiedAction = "3";
                                }

                                //logerror(modifiedAction, modifiedAction.ToString() + " - > IN Ok L2 verification In L2Staus 8 or 9 else clause and scanningType 2 or 15 if clause value modifiedAction");
                            }
                            else
                            {
                                //logerror(modifiedAction, modifiedAction.ToString() + " - > IN Ok L2 verification In L2Staus 8 or 9 else clause and scanningType 2 or 15 else clause value modifiedAction");
                                //logerror(modaction, modaction.ToString() + " - > IN Ok L2 verification In L2Staus 8 or 9 else clause and scanningType 2 or 15 else clause value modaction");
                                //logerror(modactionL1, modactionL1.ToString() + " - > IN Ok L2 verification In L2Staus 8 or 9 else clause and scanningType 2 or 15 else clause value modactionL1");

                                if (modaction == "A" && modactionL1 == "A")
                                {
                                    modifiedAction = "2";
                                }
                                else if (modaction == "A" && modactionL1 == "R")
                                {
                                    modifiedAction = "8";
                                }
                                else if (modaction == "M" && modactionL1 == "A")
                                {
                                    modifiedAction = "8";
                                }
                                else if (modaction == "M" && modactionL1 == "R")
                                {
                                    modifiedAction = "8";
                                }
                                else if (modaction == "R" && modactionL1 == "A")
                                {
                                    modifiedAction = "9";
                                }
                                else if (modaction == "R" && modactionL1 == "R")
                                {
                                    modifiedAction = "3";
                                }

                                //logerror(modifiedAction, modifiedAction.ToString() + " - > IN Ok L2 verification In L2Staus 8 or 9 else clause and scanningType 2 or 15 else clause value modifiedAction");
                            }

                        }

                        //logerror(smbver.L2Id.ToString(), smbver.L2Id.ToString() + " - > IN Ok L2 verification value Id");
                        //logerror(modifiedAction.ToString(), modifiedAction.ToString() + " - > IN Ok L2 verification value modifiedAction");
                        //logerror(modified.ToString(), modified.ToString() + " - > IN Ok L2 verification value modified");

                        //OWpro.UpdateSMBVerification(smbver.L2Id,
                        //        smbver.captureRawId,
                        //        uid,
                        //        smbver.InstrumentType,
                        //        camt,
                        //        //Convert.ToDateTime(cdatenew).ToString("yyyy-MM-dd"),
                        //        (cdatenew == "" ? DateTime.Now.ToString("yyyy-MM-dd") : cdatenew),
                        //        cfinalchqno,
                        //        cfianlsortcode,
                        //        cfinalsan,
                        //        cfialtrcd,
                        //        cacno,
                        //        cpayee,
                        //        2,//status 
                        //        Convert.ToByte(IWRemark), //reject reason
                        //        modifiedAction, //actiontaken
                        //        @Session["LoginID"].ToString(),
                        //        Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"),
                        //        smbver.CustomerId,
                        //        smbver.DomainId,
                        //        smbver.ScanningNodeId,
                        //        0,//chqamttotal 
                        //        0,//slipamttotal
                        //        0, //ChequeTotal
                        //        "",//userNarration, 
                        //        rejectreasondescrpsn,//rejectreasondescrpsn, 
                        //       Convert.ToString(Session["CtsSessionType"]),
                        //        "",//@CBSAccountInformation
                        //        "",//@CBSJointAccountInformation 
                        //        ignoreiqa,//@IgnoreIQA, 
                        //        doctype,//@DocType 
                        //        modified,//@Modified, 
                        //        null,
                        //        cDraweeName,
                        //        cNRESourceOfFundId,
                        //        cNROSourceOfFundId);//@strHoldReason

                        //logerror("Finished Update L2 query", "Finished Update L2 query" + " - > IN Ok L2 verification value Finished Update L2 query");
                        //return RedirectToAction("Index", "OWSmbVerification", new { id = Session["VFTypeID"] });
                        return RedirectToAction("Index", "OWSmbVerification", new { id = verificationId, branchId = branch });
                    }
                }
                else if (btnSubmit == "Close")
                {
                    string verification = Session["VFType"].ToString();
                    //OWpro.UnlockRecordsForOutwardVerification(smbver.L2Id, verification);
                    /// Int64 SlipRawaDataID = 0;
                    /// 
                    SqlCommand com = new SqlCommand("UnlockRecordsForOutwardVerification", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@id", smbver.L2Id);
                    com.Parameters.AddWithValue("@module", verification);

                    con.Open();
                    int i = com.ExecuteNonQuery();
                    con.Close();

                    Session["glob"] = true;
                    return RedirectToAction("IWIndex", "Home", new { id = 3 });
                }


                @Session["glob"] = true;
                return RedirectToAction("IWIndex", "Home", new { id = 1 });
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                Session.Abandon();

                logerrorInCatch(e.Message, e.InnerException.ToString());

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL2 HttpPost Index- " + innerExcp });
            }
            finally
            {
                con.Close();
            }

        }
        public ActionResult getTiffImage(string httpwebimgpath = null)
        {
            try
            {

                int custid = Convert.ToInt16(Session["CustomerID"]);
                var destpath = af.CustomerMasters.FirstOrDefault((p) => p.Id == custid);

                //Owsr.L1VerificationName = l1result.LoginID;
                string destroot = destpath.PhysicalPath;

                //logerror(httpwebimgpath, httpwebimgpath.ToString() + " - >");

                const char delimiter = '\\';
                string[] destrootarr = destroot.Split(delimiter);

                string foldrname = destrootarr[destrootarr.Length - 1];

                //logerror(foldrname, foldrname.ToString() + " - > Folder Name"); 

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
                //logerror(destroot, destroot.ToString() + "-> Root Path");
                actualpath = destroot + "\\" + actualpath;
                actualpath = actualpath.Replace("\\\\", "\\");
                // System.IO.File.AppendAllText("C:\\temp\\log1.txt", "actualpathOWL1:" + actualpath);
                //logerror(actualpath, actualpath.ToString());
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
                //logerror(imageDataURL, imageDataURL.ToString());
                return PartialView("_getTiffImage");
            }
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "SMB L2 Get Tiff Image Index- " + innerExcp });
            }

        }

        public ActionResult getTiffImageNew(string httpwebimgpath = null)
        {
            try
            {
                int custid = Convert.ToInt16(Session["CustomerID"]);
                var destpath = af.CustomerMasters.FirstOrDefault((p) => p.Id == custid);

                //Owsr.L1VerificationName = l1result.LoginID;
                string destroot = destpath.PhysicalPath;

                //logerror(httpwebimgpath, httpwebimgpath.ToString() + " - >");

                const char delimiter = '\\';
                string[] destrootarr = destroot.Split(delimiter);

                string foldrname = destrootarr[destrootarr.Length - 1];

                //logerror(foldrname, foldrname.ToString() + " - > Folder Name");

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
                //logerror(destroot, destroot.ToString() + "-> Root Path");
                actualpath = destroot + actualpath;
                // System.IO.File.AppendAllText("C:\\temp\\log1.txt", "actualpathOWL1:" + actualpath);
                //logerror(actualpath, actualpath.ToString());
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
                //logerror(imageDataURL, imageDataURL.ToString());
                return PartialView("_getTiffImage");
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;
                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "SMB L2 Get Tiff Image Index- " + innerExcp });
            }

        }
        public ActionResult GetCMSAccount(string ac = null)
        {
            bool flg = false;
            var rest = (from c in af.CMS_BranchAccountMappings
                        where c.AccountNo == ac
                        select c).ToList();

            if (rest.Count != 0)
                flg = true;
            else
                flg = false;

            return Json(flg, JsonRequestBehavior.AllowGet);
        }

        //Ac. No. API Function
        public string getAccountDetailsSIB(string sServiceUrl = null, string sClientId = null, string sClientSecretKey = null, string sSenderCode = null, string sSenderName = null, string sAccountNo = null)
        //public ActionResult getAccountDetailsSIB(string sAccountNo)
        {
            string sResposne = "";
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            try
            {
                string sInputString = "";

                sInputString = "{";
                sInputString += "\"Request\": {";
                sInputString += "               \"Header\": {";
                sInputString += "                            \"Timestamp\": \"" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "\",";
                sInputString += "                            \"ChannelDetails\": {";
                sInputString += "                                                  \"ChannelID\": \"CRM\",";
                sInputString += "                                                  \"ChannelType\": \"WEB\",";
                sInputString += "                                                  \"ChannelSubClass\": \"Retail\",";
                sInputString += "                                                  \"BranchCode\": \"\",";
                sInputString += "                                                  \"ChannelCusHdr\": {}";
                sInputString += "                                                 }, ";
                sInputString += "                            \"DeviceDetails\": {";
                sInputString += "                                                \"DeviceID\": \"Device1\",";
                sInputString += "                                                \"IMEINumber\": \"\",";
                sInputString += "                                                \"ClientIP\": \"\",";
                sInputString += "                                                \"OS\": \"\",";
                sInputString += "                                                \"BrowserType\": \"\",";
                sInputString += "                                                \"MobileNumber\": \"\",";
                sInputString += "                                                \"GeoLocation\": {";
                sInputString += "                                                                  \"Latitude\": \"13.072090\",";
                sInputString += "                                                                  \"Longitude\": \"80.201859\"";
                sInputString += "                                                                  }";
                sInputString += "                                                }";
                sInputString += "                              },";
                sInputString += "";
                sInputString += "              \"Body\": {";
                sInputString += "                        \"UUID\": \"K" + sAccountNo + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "\","; //"FEBA_1512106219450\",";
                                                                                                                                           //sInputString += "			             \"merchantCode\": \"DMmwxBIZxzgqZAQHtLjyQmWgRLlMKzOTuZb\",";
                                                                                                                                           //sInputString += "                        \"sender_code\": \"DMmwxBIZxzgqZAQHtLjyQmWgRLlMKzOTuZb\",";
                                                                                                                                           //sInputString += "                        \"sender_name\": \"KORES_CLG_CENTRALIZATION\",";

                sInputString += "                        \"sender_code\": \"" + sSenderCode + "\",";
                sInputString += "                        \"sender_name\": \"" + sSenderName + "\",";
                sInputString += "                        \"channel_id\": \"CRM\",";
                sInputString += "                        \"acct_num\": \"" + sAccountNo + "\"";
                sInputString += "                         }";
                sInputString += "            }";
                sInputString += "      }";


                var httpWebRequest = (HttpWebRequest)WebRequest.Create(sServiceUrl);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("SIB-Client-Secret", sClientSecretKey); //"Q5sG7qB4sP2kI3cB4bB2dC5bI3hW0gK5cB8iV3kV6gY5eU8oV7");
                httpWebRequest.Headers.Add("SIB-Client-Id", sClientId);//"97aafbe1-0a88-4c49-986b-0e21025b4983");
                httpWebRequest.Headers.Add("GlobalTranID", "KOR" + DateTime.Now.ToString("yyyyMMddHHmmssfff"));

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(sInputString);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    sResposne = result;
                }

            }
            catch (Exception Ex)
            {
                sResposne = "{\"Request\":{\"Error\":\"Runtime Error While Sending the Request\"," +
                                          "\"Description\":\"" + Ex.Message
                                     + "\"}" +
                            "}";
            }


            return sResposne;


        }

        public string ClosedAccount()
        {
            string sResposne = "";

            string sInputString = "";

            sInputString = sInputString + "{";
            sInputString = sInputString + "\"Response\": {";
            sInputString = sInputString + "\"Header\": {";
            sInputString = sInputString + "\"Timestamp\":\"20220412153826344\",";
            sInputString = sInputString + "\"APIName\":\"casa-validator-api\",";
            sInputString = sInputString + "\"APIVersion\":\"1.1.0\",";
            sInputString = sInputString + "\"Interface\":\"CASA_Validator\"";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"Status\": {";
            sInputString = sInputString + "\"Code\":\"406\",";
            sInputString = sInputString + "\"Desc\":\"Failure\"";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"Body\": {";
            sInputString = sInputString + "\"Description\":\"Account is closed\"";
            sInputString = sInputString + "}";
            sInputString = sInputString + "}";
            sInputString = sInputString + "}";

            sResposne = sInputString.Trim();

            return sResposne;

        }
        public string JointAccount()
        {
            string sResposne = "";

            string sInputString = "";

            sInputString = sInputString + "{";
            sInputString = sInputString + "\"Response\": {";
            sInputString = sInputString + "\"Header\": {";
            sInputString = sInputString + "\"Timestamp\":\"20220412153826344\",";
            sInputString = sInputString + "\"APIName\":\"casa-validator-api\",";
            sInputString = sInputString + "\"APIVersion\":\"1.1.0\",";
            sInputString = sInputString + "\"Interface\":\"CASA_Validator\"";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"Status\": {";
            sInputString = sInputString + "\"Code\":\"200\",";
            sInputString = sInputString + "\"Desc\":\"Success\"";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"Body\": {";
            sInputString = sInputString + "\"UUID\":\"kores20220412153225\",";
            sInputString = sInputString + "\"solID\":\"0012\",";
            sInputString = sInputString + "\"AcctId\":\"0012053000004770\",";
            sInputString = sInputString + "\"AcctType\": {";
            sInputString = sInputString + "\"SchmCode\":\"SBGEN\",";
            sInputString = sInputString + "\"SchmType\":\"SBA\"";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"AcctCurr\":\"INR\",";
            sInputString = sInputString + "\"AcctOpnDt\":\"16-02-1987\",";
            sInputString = sInputString + "\"ModeOfOper\":\"003\",";
            sInputString = sInputString + "\"BankAcctStatusCode\":\"\",";
            sInputString = sInputString + "\"CustId\": {";
            sInputString = sInputString + "\"CustId\":\"A00823501\",";
            sInputString = sInputString + "\"PersonName\": {";
            sInputString = sInputString + "\"LastName\":\"\",";
            sInputString = sInputString + "\"FirstName\":\"\",";
            sInputString = sInputString + "\"MiddleName\":\"\",";
            sInputString = sInputString + "\"Name\":\"PAUL P K\",";
            sInputString = sInputString + "\"TitlePrefix\":\"MR\"";
            sInputString = sInputString + "}";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"AcctName\":\"PAUL P K\",";
            sInputString = sInputString + "\"AcctShortName\":\"PAUL\",";
            sInputString = sInputString + "\"AcctStmtMode\":\"B\",";
            sInputString = sInputString + "\"AcctStmtFreq\": {";
            sInputString = sInputString + "\"Cal\":\"\",";
            sInputString = sInputString + "\"Type\":\"M\",";
            sInputString = sInputString + "\"StartDt\":\"1\",";
            sInputString = sInputString + "\"WeekDay\":\"0\",";
            sInputString = sInputString + "\"WeekNum\":\" \",";
            sInputString = sInputString + "\"HolStat\":\"N\"";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"AcctStmtNxtPrintDt\":\"01-06-2015\",";
            sInputString = sInputString + "\"DespatchMode\":\"E\",";
            sInputString = sInputString + "\"AcctBalCrDrInd\":\"C\",";
            sInputString = sInputString + "\"AcctBalAmt\": {";
            sInputString = sInputString + "\"amountValue\":\"36131.96\",";
            sInputString = sInputString + "\"currencyCode\":\"INR\"";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"FreezeStatusCode\":\" \",";
            sInputString = sInputString + "\"FreezeReasonCode\":\"\",";
            sInputString = sInputString + "\"AccrIntDrCrInd\":\"C\",";
            sInputString = sInputString + "\"AccrIntRate\": {";
            sInputString = sInputString + "\"value\":\"2.35\"";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"IntCalcFreq\": {";
            sInputString = sInputString + "\"Cal\":\"\",";
            sInputString = sInputString + "\"Type\":\" \",";
            sInputString = sInputString + "\"StartDt\":\"0\",";
            sInputString = sInputString + "\"WeekDay\":\"0\",";
            sInputString = sInputString + "\"WeekNum\":\" \",";
            sInputString = sInputString + "\"HolStat\":\" \"";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"IntRateCode\":\"SBGEN\",";
            sInputString = sInputString + "\"NetIntDrCrInd\":\"C\",";
            sInputString = sInputString + "\"NetIntRate\": {";
            sInputString = sInputString + "\"value\":\"2.35\"";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"RelPartyRec\": [";
            sInputString = sInputString + "{";
            sInputString = sInputString + "\"RelPartyType\":\"M\",";
            sInputString = sInputString + "\"RelPartyTypeDesc\":\"MAIN HOLDER OF ACCOUNT\",";
            sInputString = sInputString + "\"RelPartyCode\":\"\",";
            sInputString = sInputString + "\"RelPartyCodeDesc\":\"\",";
            sInputString = sInputString + "\"CustId\": {";
            sInputString = sInputString + "\"CustId\":\"A00823501\",";
            sInputString = sInputString + "\"PersonName\": {";
            sInputString = sInputString + "\"LastName\":\"\",";
            sInputString = sInputString + "\"FirstName\":\"\",";
            sInputString = sInputString + "\"MiddleName\":\"\",";
            sInputString = sInputString + "\"Name\":\"PAUL P K\",";
            sInputString = sInputString + "\"TitlePrefix\":\"MR\"";
            sInputString = sInputString + "}";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"RelPartyContactInfo\": {";
            sInputString = sInputString + "\"PhoneNum\": {";
            sInputString = sInputString + "\"TelephoneNum\":\"+912702387\",";
            sInputString = sInputString + "\"FaxNum\":\"\",";
            sInputString = sInputString + "\"TelexNum\":\"\"";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"EmailAddr\":\"\",";
            sInputString = sInputString + "\"PostAddr\": {";
            sInputString = sInputString + "\"Addr1\":\"PEREPPADAN HOUSE\",";
            sInputString = sInputString + "\"Addr2\":\"SANTHIPURAM,MELOOR PO\",";
            sInputString = sInputString + "\"Addr3\":\"\",";
            sInputString = sInputString + "\"City\":\"KL972\",";
            sInputString = sInputString + "\"StateProv\":\"KL\",";
            sInputString = sInputString + "\"PostalCode\":\"680311\",";
            sInputString = sInputString + "\"Country\":\"IN\",";
            sInputString = sInputString + "\"AddrType\":\"\"";
            sInputString = sInputString + "}";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"RecDelFlg\":\"N\"";
            sInputString = sInputString + "},";
            sInputString = sInputString + "{";
            sInputString = sInputString + "\"RelPartyType\":\"J\",";
            sInputString = sInputString + "\"RelPartyTypeDesc\":\"JOINT HOLDER OF ACCOUNT\",";
            sInputString = sInputString + "\"RelPartyCode\":\"\",";
            sInputString = sInputString + "\"RelPartyCodeDesc\":\"WIFE\",";
            sInputString = sInputString + "\"CustId\": {";
            sInputString = sInputString + "\"CustId\":\"A03194062\",";
            sInputString = sInputString + "\"PersonName\": {";
            sInputString = sInputString + "\"LastName\":\"\",";
            sInputString = sInputString + "\"FirstName\":\"\",";
            sInputString = sInputString + "\"MiddleName\":\"\",";
            sInputString = sInputString + "\"Name\":\"TREESA PAUL\",";
            sInputString = sInputString + "\"TitlePrefix\":\"MRS\"";
            sInputString = sInputString + "}";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"RelPartyContactInfo\": {";
            sInputString = sInputString + "\"PhoneNum\": {";
            sInputString = sInputString + "\"TelephoneNum\":\"2739575\",";
            sInputString = sInputString + "\"FaxNum\":\"\",";
            sInputString = sInputString + "\"TelexNum\":\"\"";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"EmailAddr\":\"\",";
            sInputString = sInputString + "\"PostAddr\": {";
            sInputString = sInputString + "\"Addr1\":\"W/O P K PAUL,PEREPADAN HOUSE\",";
            sInputString = sInputString + "\"Addr2\":\"SANTHIPURAM,MELOOR PO\",";
            sInputString = sInputString + "\"Addr3\":\"\",";
            sInputString = sInputString + "\"City\":\"KL972\",";
            sInputString = sInputString + "\"StateProv\":\"KL\",";
            sInputString = sInputString + "\"PostalCode\":\"680311\",";
            sInputString = sInputString + "\"Country\":\"IN\",";
            sInputString = sInputString + "\"AddrType\":\"\"";
            sInputString = sInputString + "}";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"RecDelFlg\":\"N\"";
            sInputString = sInputString + "}";
            sInputString = sInputString + "],";
            sInputString = sInputString + "\"acct_status\":\"A\",";
            sInputString = sInputString + "\"customer_band\":\"\",";
            sInputString = sInputString + "\"customerisMinor\":\"N\",";
            sInputString = sInputString + "\"customerisNRE\":\"N\"";
            sInputString = sInputString + "}";
            sInputString = sInputString + "}";
            sInputString = sInputString + "}";

            sResposne = sInputString.Trim();

            return sResposne;

        }


        public string ValidAcNo()
        {
            string sResposne = "";



            try
            {
                string sInputString = "";

                sInputString = sInputString + "{";
                sInputString = sInputString + "\"Response\": {";
                sInputString = sInputString + "\"Header\": {";
                sInputString = sInputString + "\"Timestamp\":\"20200330160257641\",";
                sInputString = sInputString + "\"APIName\":\"casa-validator-api\",";
                sInputString = sInputString + "\"APIVersion\":\"1.1.0\",";
                sInputString = sInputString + "\"Interface\":\"CASA_Validator\"";
                sInputString = sInputString + "},";
                sInputString = sInputString + "\"Status\": {";
                sInputString = sInputString + "\"Code\":\"200\",";
                sInputString = sInputString + "\"Desc\":\"Success\"";
                sInputString = sInputString + "},";
                sInputString = sInputString + "\"Body\": {";
                sInputString = sInputString + "\"UUID\":\"1234A1A1aa1S\",";
                sInputString = sInputString + "\"AcctId\":\"\",";
                sInputString = sInputString + "\"AcctType\": {";
                sInputString = sInputString + "\"SchmCode\":\"\",";
                sInputString = sInputString + "\"SchmType\":\"\"";
                sInputString = sInputString + "},";
                sInputString = sInputString + "\"AcctCurr\":\"\",";
                sInputString = sInputString + "\"AcctOpnDt\":\"19-10-2017\",";
                sInputString = sInputString + "\"ModeOfOper\":\"001\",";
                sInputString = sInputString + "\"BankAcctStatusCode\":\"\",";
                sInputString = sInputString + "\"CustId\": {";
                sInputString = sInputString + "\"CustId\":\"A47413298\",";
                sInputString = sInputString + "\"PersonName\": {";
                sInputString = sInputString + "\"LastName\":\"\",";
                sInputString = sInputString + "\"FirstName\":\"\",";
                sInputString = sInputString + "\"MiddleName\":\"\",";
                sInputString = sInputString + "\"Name\":\"SHIVSHANKAR KRISHNAMURTHI\",";
                sInputString = sInputString + "\"TitlePrefix\":\"MR\"";
                sInputString = sInputString + "}";
                sInputString = sInputString + "},";
                sInputString = sInputString + "\"AcctName\":\"SHIVSHANKAR KRISHNAMURTHI\",";
                sInputString = sInputString + "\"AcctShortName\":\"SHIVSHANKA\",";
                sInputString = sInputString + "\"AcctStmtMode\":\"B\",";
                sInputString = sInputString + "\"AcctStmtFreq\": {";
                sInputString = sInputString + "\"Cal\":\"\",";
                sInputString = sInputString + "\"Type\":\"M\",";
                sInputString = sInputString + "\"StartDt\":\"1\",";
                sInputString = sInputString + "\"WeekDay\":\"0\",";
                sInputString = sInputString + "\"WeekNum\":\" \",";
                sInputString = sInputString + "\"HolStat\":\"N\"";
                sInputString = sInputString + "},";
                sInputString = sInputString + "\"AcctStmtNxtPrintDt\":\"21-11-2020\",";
                sInputString = sInputString + "\"DespatchMode\":\"P\",";
                sInputString = sInputString + "\"AcctBalCrDrInd\":\"C\",";
                sInputString = sInputString + "\"AcctBalAmt\": {";
                sInputString = sInputString + "\"amountValue\":\"3733.77\",";
                sInputString = sInputString + "\"currencyCode\":\"INR\"";
                sInputString = sInputString + "},";
                sInputString = sInputString + "\"FreezeStatusCode\":\" \",";
                sInputString = sInputString + "\"FreezeReasonCode\":\"\",";
                sInputString = sInputString + "\"AccrIntDrCrInd\":\"\",";
                sInputString = sInputString + "\"AccrIntRate\": {";
                sInputString = sInputString + "\"value\":\"0.00\"";
                sInputString = sInputString + "},";
                sInputString = sInputString + "\"IntCalcFreq\": {";
                sInputString = sInputString + "\"Cal\":\"\",";
                sInputString = sInputString + "\"Type\":\" \",";
                sInputString = sInputString + "\"StartDt\":\"0\",";
                sInputString = sInputString + "\"WeekDay\":\"0\",";
                sInputString = sInputString + "\"WeekNum\":\" \",";
                sInputString = sInputString + "\"HolStat\":\" \"";
                sInputString = sInputString + "},";
                sInputString = sInputString + "\"IntRateCode\":\"C0825\",";
                sInputString = sInputString + "\"NetIntDrCrInd\":\"\",";
                sInputString = sInputString + "\"NetIntRate\": {";
                sInputString = sInputString + "\"value\":\"0.00\"";
                sInputString = sInputString + "},";
                sInputString = sInputString + "\"NomineeInfoRec\": {";
                sInputString = sInputString + "\"RegNum\":\"\",";
                sInputString = sInputString + "\"NomineeName\":\"\",";
                sInputString = sInputString + "\"RelType\":\"\",";
                sInputString = sInputString + "\"NomineeContactInfo\": {";
                sInputString = sInputString + "\"PhoneNum\": {";
                sInputString = sInputString + "\"TelephoneNum\":\"\",";
                sInputString = sInputString + "\"FaxNum\":\"\",";
                sInputString = sInputString + "\"TelexNum\":\"\"";
                sInputString = sInputString + "},";
                sInputString = sInputString + "\"EmailAddr\":\"\",";
                sInputString = sInputString + "\"PostAddr\": {";
                sInputString = sInputString + "\"Addr1\":\"\",";
                sInputString = sInputString + "\"Addr2\":\"\",";
                sInputString = sInputString + "\"Addr3\":\"\",";
                sInputString = sInputString + "\"City\":\"\",";
                sInputString = sInputString + "\"StateProv\":\"\",";
                sInputString = sInputString + "\"PostalCode\":\"\",";
                sInputString = sInputString + "\"Country\":\"\",";
                sInputString = sInputString + "\"AddrType\":\"\"";
                sInputString = sInputString + "}";
                sInputString = sInputString + "},";
                sInputString = sInputString + "\"NomineeMinorFlg\":\"\",";
                sInputString = sInputString + "\"NomineePercent\": {";
                sInputString = sInputString + "\"value\":\"\"";
                sInputString = sInputString + "},";
                sInputString = sInputString + "\"GuardianInfo\": {";
                sInputString = sInputString + "\"GuardianCode\":\"\",";
                sInputString = sInputString + "\"GuardianName\":\"\",";
                sInputString = sInputString + "\"GuardianContactInfo\": {";
                sInputString = sInputString + "\"PhoneNum\": {";
                sInputString = sInputString + "\"TelephoneNum\":\"\",";
                sInputString = sInputString + "\"FaxNum\":\"\",";
                sInputString = sInputString + "\"TelexNum\":\"\"";
                sInputString = sInputString + "},";
                sInputString = sInputString + "\"EmailAddr\":\"\",";
                sInputString = sInputString + "\"PostAddr\": {";
                sInputString = sInputString + "\"Addr1\":\"\",";
                sInputString = sInputString + "\"Addr2\":\"\",";
                sInputString = sInputString + "\"Addr3\":\"\",";
                sInputString = sInputString + "\"City\":\"\",";
                sInputString = sInputString + "\"StateProv\":\"\",";
                sInputString = sInputString + "\"PostalCode\":\"\",";
                sInputString = sInputString + "\"Country\":\"\",";
                sInputString = sInputString + "\"AddrType\":\"\"";
                sInputString = sInputString + "}";
                sInputString = sInputString + "}";
                sInputString = sInputString + "},";
                sInputString = sInputString + "\"RecDelFlg\":\"\"";
                sInputString = sInputString + "},";
                sInputString = sInputString + "\"RelPartyRec\": {";
                sInputString = sInputString + "\"RelPartyType\":\"M\",";
                sInputString = sInputString + "\"RelPartyTypeDesc\":\"MAIN HOLDER OF ACCOUNT\",";
                sInputString = sInputString + "\"RelPartyCode\":\"\",";
                sInputString = sInputString + "\"RelPartyCodeDesc\":\"\",";
                sInputString = sInputString + "\"CustId\": {";
                sInputString = sInputString + "\"CustId\":\"A47413298\",";
                sInputString = sInputString + "\"PersonName\": {";
                sInputString = sInputString + "\"LastName\":\"\",";
                sInputString = sInputString + "\"FirstName\":\"\",";
                sInputString = sInputString + "\"MiddleName\":\"\",";
                sInputString = sInputString + "\"Name\":\"SHIVSHANKAR KRISHNAMURTHI\",";
                sInputString = sInputString + "\"TitlePrefix\":\"MR\"";
                sInputString = sInputString + "}";
                sInputString = sInputString + "},";
                sInputString = sInputString + "\"RelPartyContactInfo\": {";
                sInputString = sInputString + "\"PhoneNum\": {";
                sInputString = sInputString + "\"TelephoneNum\":\"+9104422481637\",";
                sInputString = sInputString + "\"FaxNum\":\"\",";
                sInputString = sInputString + "\"TelexNum\":\"\"";
                sInputString = sInputString + "},";
                sInputString = sInputString + "\"EmailAddr\":\"singershiv@gmail.com\",";
                sInputString = sInputString + "\"PostAddr\": {";
                sInputString = sInputString + "\"Addr1\":\"6, GANAPATHY STREET, LIC COLONY EXTENSION\",";
                sInputString = sInputString + "\"Addr2\":\"PAMMAL, CHENNAI\",";
                sInputString = sInputString + "\"Addr3\":\"\",";
                sInputString = sInputString + "\"City\":\"TN900\",";
                sInputString = sInputString + "\"StateProv\":\"TN\",";
                sInputString = sInputString + "\"PostalCode\":\"600075\",";
                sInputString = sInputString + "\"Country\":\"IN\",";
                sInputString = sInputString + "\"AddrType\":\"\"";
                sInputString = sInputString + "}";
                sInputString = sInputString + "},";
                sInputString = sInputString + "\"RecDelFlg\":\"N\"";
                sInputString = sInputString + "},";
                sInputString = sInputString + "\"acct_status\":\"\",";
                sInputString = sInputString + "\"customer_band\":\"\"";
                sInputString = sInputString + "}";
                sInputString = sInputString + "}";
                sInputString = sInputString + "}";

                sResposne = sInputString.ToString().Trim();
            }
            catch (Exception Ex)
            {
                sResposne = "{\"Request\":{\"Error\":\"Runtime Error While Sending the Request\"," +
                                              "\"Description\":\"" + Ex.Message
                                         + "\"}" +
                                "}";
            }

            return sResposne;
        }

        public string InValidAcNo()
        {
            string sResposne = "";

            try
            {
                string sInputString = "";

                sInputString = sInputString + "{";
                sInputString = sInputString + "\"Response\": {";
                sInputString = sInputString + "\"Header\": {";
                sInputString = sInputString + "\"Timestamp\":\"20200330160257641\",";
                sInputString = sInputString + "\"APIName\":\"casa-validator-api\",";
                sInputString = sInputString + "\"APIVersion\":\"1.1.0\",";
                sInputString = sInputString + "\"Interface\":\"CASA_Validator\"";
                sInputString = sInputString + "},";
                sInputString = sInputString + "\"Status\": {";
                sInputString = sInputString + "\"Code\":\"406\",";
                sInputString = sInputString + "\"Desc\":\"Failure\"";
                sInputString = sInputString + "},";
                sInputString = sInputString + "\"Body\": {";
                sInputString = sInputString + "\"Description\":\"Invalid Account No.\"";

                sInputString = sInputString + "}";
                sInputString = sInputString + "}";
                sInputString = sInputString + "}";

                sResposne = sInputString.ToString().Trim();
            }
            catch (Exception Ex)
            {
                sResposne = "{\"Request\":{\"Error\":\"Runtime Error While Sending the Request\"," +
                                              "\"Description\":\"" + Ex.Message
                                         + "\"}" +
                                "}";
            }

            return sResposne;
        }



        public ActionResult GetAccNames(string ac = null)
        {
            cbstetails vAccNames = new cbstetails();

            if (ac != null && ac != "")
            {
                //Verification Ac no. API Calling
                string NewApiCall = null;
                var OwApi = af.CommonSettings.FirstOrDefault(a => a.AppName == "CTSCONFIG1" && a.SettingName == "OWVerAPI")?.SettingValue;
                if (OwApi != null && OwApi != "")
                    NewApiCall = OwApi.ToString().ToUpper();
                else
                    NewApiCall = "N";

                if (NewApiCall == "Y")
                {
                    try
                    {
                        string sAcctName = "";

                        var sServiceUrl = ConfigurationManager.AppSettings["sServiceUrl"].ToString();
                        var sClientId = ConfigurationManager.AppSettings["sClientId"].ToString();
                        var sClientSecretKey = ConfigurationManager.AppSettings["sClientSecretKey"].ToString();
                        var sSenderCode = ConfigurationManager.AppSettings["sSenderCode"].ToString();
                        var sSenderName = ConfigurationManager.AppSettings["sSenderName"].ToString();

                        //string sgetAccountDetailsSIB = getAccountDetailsSIB(sServiceUrl, sClientId, sClientSecretKey, sSenderCode, sSenderName, ac);
                        //string sgetAccountDetailsSIB = ValidAcNo();
                        //string sgetAccountDetailsSIB = InValidAcNo();

                        string sgetAccountDetailsSIB = JointAccount();


                        var jObject = Newtonsoft.Json.Linq.JObject.Parse(sgetAccountDetailsSIB);





                        if (jObject["Response"]["Body"]["RelPartyRec"].Count() > 0 && (ac == "123451234512345" || ac == "999999999999999"))
                        {
                            int iIndex = 0;
                            //while (iIndex < jObject["Response"]["Body"]["RelPartyRec"].Count())
                            //{
                            //    lAccNames.Add(jObject["Response"]["Body"]["RelPartyRec"][iIndex]["CustId"]["PersonName"]["Name"].ToString().Trim());
                            //    iIndex++;
                            //}
                            //new SelectListItem { Text = "Action", Value = "0" }
                            vAccNames.status = "SUCCESS";


                            //SelectList AccNames = new SelectList(lAccNames);
                            //ViewData["AccNames"] = AccNames;
                            //var vAccNames = lAccNames;
                            // vAccNames.PayeeName = lAccNames;
                            //ViewBag.AccNames = AccNames;



                            //ViewBag.AccNames = new SelectList(vAccNames.AsEnumerable(),"Id", "name");
                            //return PartialView("_AccountNames", ViewBag.AccNames);
                            return PartialView("_JointAcNms");
                            //return PartialView("_AccountNames", sAcctName);


                        }
                        else
                        {
                            //ViewBag.AccNames = null; ;
                            //return Json(new { data = sAcctName }, JsonRequestBehavior.AllowGet);
                            vAccNames.status = "FAILURE";
                            return PartialView("_AccountNames", vAccNames);
                        }


                    }
                    catch (Exception e)
                    {
                        ErrorDisplay er = new ErrorDisplay();
                        string message = "";
                        string innerExcp = "";
                        if (e.Message != null)
                            message = e.Message.ToString();
                        if (e.InnerException != null)
                            innerExcp = e.InnerException.Message;
                        er.ErrorMessage = message;

                        return PartialView("Error", er);
                    }
                }//NewApiCall == "Y"
                else
                {
                    vAccNames.status = "FAILURE";
                    return PartialView("_AccountNames", vAccNames);
                }

            } //ac!=null
            else
            {
                vAccNames.status = "FAILURE";
                return PartialView("_AccountNames", vAccNames);
            }
        }//main

        public PartialViewResult GetJointAcNms(string ac = null)
        {

            try
            {
                cbstetails cbsdtls = new cbstetails();
                if (ac != null && ac != "")
                {
                    string NewApiCall = null;
                    var OwApi = af.CommonSettings.FirstOrDefault(a => a.AppName == "CTSCONFIG1" && a.SettingName == "OWVerAPI")?.SettingValue;
                    if (OwApi != null && OwApi != "")
                    {
                        NewApiCall = OwApi.ToString().ToUpper();
                    }
                    else
                        NewApiCall = "N";

                    if (NewApiCall == "Y")
                    {

                        string sBankNm = "DBS";
                        if (sBankNm == "DBS")
                        {
                            //CASA variables
                            var CasaClientId = ConfigurationManager.AppSettings["CasaClientId"].ToString();
                            var CasaCorellationId = ConfigurationManager.AppSettings["CasaCorellationId"].ToString();
                            var CasaServiceURL = ConfigurationManager.AppSettings["CasaServiceURL"].ToString();

                            sCasaClientId = CasaClientId;
                            sCasaCorellationId = CasaCorellationId;
                            sCasaServiceURL = CasaServiceURL;
                            sAccountNo = ac;

                            //Token variables
                            var TokenClientId = ConfigurationManager.AppSettings["TokenClientId"].ToString();
                            var TokenSecreteKey = ConfigurationManager.AppSettings["TokenSecreteKey"].ToString();
                            var TokenServiceURL = ConfigurationManager.AppSettings["TokenServiceURL"].ToString();

                            //CMPC variables
                            var CMCPCountry = ConfigurationManager.AppSettings["CMCPCountry"].ToString();
                            var CMCPReqUID = ConfigurationManager.AppSettings["CMCPReqUID"].ToString();
                            var CMCPReqClientId = ConfigurationManager.AppSettings["CMCPReqClientId"].ToString();
                            var CMCPServiceURL = ConfigurationManager.AppSettings["CMCPServiceURL"].ToString();

                            // 2 uncomment when deployed on bank start
                            //Get Token 
                            string sEtoken = GetToken();  // uncomment when deployed on bank
                            // 2 uncomment when deployed on bank end

                            ViewBag.Currency = "";
                            ViewBag.sCAPA = "";
                            ViewBag.vbNRE = "";
                            Session["sNR"] = "";
                            Session["SourceCustomerId"] = "";
                            Session["AccountCurrency"] = "";
                            Session["IsOpenedDateOld"] = "";
                            Session["productCode"] = "";
                            Session["productType"] = "";
                            Session["accountBalances"] = "0";
                            Session["AccountStatus"] = "";
                            Session["FreezeStatusCode"] = "";

                            long openDate = 0;

                            if (ac == "999999999999")
                            {
                                // ViewBag.vberror = "Account is inactive";
                                ViewBag.vbstatus = "SUCCESS";
                                ViewBag.vbcbsdls = null;
                                ViewBag.vbAcctName = null;
                                ViewBag.block = 0;
                            }
                            else
                            {
                                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;


                                //sgetAccountDetailsDBS = getAccountDetailsDBSInvalid(CasaServiceURL, CasaClientId, CasaCorellationId, ac);

                                //====== 3 uncomment when deployed on bank start =====
                                //string sgetAccountDetailsDBS = getAccountDetailsDBSResponse(CasaServiceURL, CasaClientId, CasaCorellationId, ac); //local testing
                                sgetAccountDetailsDBS = getAccountDetailsDBSRequest(CasaServiceURL, CasaClientId, CasaCorellationId, ac, sEtoken); // DBS testing
                                //sgetAccountDetailsDBS = testCMCP_Response();
                                //====== 3 uncomment when deployed on bank end =====
                                //logerror("In getAccountDetailsDBSRequest method ", "sgetAccountDetailsDBS - " + sgetAccountDetailsDBS);
                                var newResponse = sgetAccountDetailsDBS.Replace(", Please", " Please");

                                var jObject = Newtonsoft.Json.Linq.JObject.Parse(newResponse);

                                logerror("In GetJointAcNms method ", "after getting parse jObject successfully - ");

                                if (jObject["error"] != null)
                                {
                                    if(jObject["errorDescription"] != null)
                                    {
                                        ViewBag.vberror = jObject["errorDescription"].ToString();
                                        ViewBag.vbstatus = "SUCCESS";
                                        ViewBag.vbcbsdls = null;
                                        ViewBag.block = 1;
                                    }
                                    else
                                    {
                                        ViewBag.vberror = "Invalid Account";
                                        ViewBag.vbstatus = "SUCCESS";
                                        ViewBag.vbcbsdls = null;
                                        ViewBag.block = 1;
                                    }

                                }
                                else
                                {
                                    if (jObject["accountClosedFlag"] != null)
                                    {
                                        if (jObject["accountClosedFlag"].ToString().Trim().ToUpper() != "Y")
                                        {
                                            logerror("In GetJointAcNms method sourceCustomerId - ", jObject["sourceCustomerId"].ToString().Trim());
                                            logerror("In GetJointAcNms method accountCurrency - ", jObject["accountCurrency"].ToString().Trim());
                                            //=========== Amol changes on 27/02/2024 for capturing SourceCustomerId start ======
                                            if (jObject["sourceCustomerId"] != null)
                                            {
                                                Session["SourceCustomerId"] = jObject["sourceCustomerId"].ToString().Trim();
                                                //cbsdtls.SourceCustomerId = jObject["sourceCustomerId"].ToString().Trim();
                                            }
                                            else
                                            {
                                                Session["SourceCustomerId"] = "";
                                            }

                                            if (jObject["accountCurrencyCode"] != null)
                                            {
                                                Session["AccountCurrency"] = jObject["accountCurrencyCode"].ToString().Trim();
                                                ViewBag.Currency = jObject["accountCurrencyCode"].ToString().Trim();
                                                //cbsdtls.Currency = jObject["accountCurrency"].ToString().Trim();
                                            }
                                            else
                                            {
                                                Session["AccountCurrency"] = "";
                                                ViewBag.Currency = "";
                                            }

                                            if (jObject["openedDate"] != null)
                                            {
                                                openDate = Convert.ToInt64(jObject["openedDate"].ToString().Trim());
                                                Session["Opendate"]=openDate;
                                                //var dateTime = DateTimeOffset.FromUnixTimeSeconds(1550962800);
                                            }
                                            else
                                            {
                                                openDate = 0;
                                                Session["Opendate"] = "";
                                            }

                                            if (openDate != 0)
                                            {
                                                // DateTimeOffset from Unix timestamp
                                                DateTimeOffset dateTimeFromUnix = DateTimeOffset.FromUnixTimeMilliseconds(openDate);
                                                // Current DateTimeOffset
                                                DateTimeOffset currentDateTime = DateTimeOffset.Now;
                                                // Calculate the difference
                                                //TimeSpan difference = currentDateTime - dateTimeFromUnix;

                                                int differenceInMonths = CalculateDifferenceInMonths(currentDateTime, dateTimeFromUnix);

                                                // Check if the difference is greater than 6 months
                                                if (differenceInMonths > 6)
                                                {
                                                    //Console.WriteLine("The difference is greater than six months.");
                                                    Session["IsOpenedDateOld"] = "Y";
                                                }
                                                else
                                                {
                                                    //Console.WriteLine("The difference is not greater than six months.");
                                                    Session["IsOpenedDateOld"] = "N";
                                                }
                                            }
                                            else
                                            {
                                                Session["IsOpenedDateOld"] = "";
                                            }

                                            if (jObject["productCode"] != null)
                                            {
                                                Session["productCode"] = jObject["productCode"].ToString().Trim();
                                            }
                                            else
                                            {
                                                Session["productCode"] = "";
                                            }

                                            if (jObject["productType"] != null)
                                            {
                                                Session["productType"] = jObject["productType"].ToString().Trim();
                                            }
                                            else
                                            {
                                                Session["productType"] = "";
                                            }

                                            if (jObject["accountBalances"] != null)
                                            {
                                                Session["accountBalances"] = jObject["accountBalances"]["effectiveAvailableAmount"] == null ? "0" :
                                                                            jObject["accountBalances"]["effectiveAvailableAmount"].ToString().Trim();
                                            }
                                            else
                                            {
                                                Session["accountBalances"] = "0";
                                            }

                                            logerror("In GetJointAcNms method sourceCustomerId session - ", Session["SourceCustomerId"].ToString().Trim());
                                            logerror("In GetJointAcNms method accountCurrency session - ", Session["AccountCurrency"].ToString().Trim());

                                            if (jObject["accountStatus"] != null)
                                            {
                                                Session["AccountStatus"] = jObject["accountStatus"].ToString().Trim();
                                            }
                                            else
                                            {
                                                Session["AccountStatus"] = "";
                                            }

                                            if (jObject["freezeStatusCode"] != null)
                                            {
                                                Session["FreezeStatusCode"] = jObject["freezeStatusCode"].ToString().Trim();
                                            }
                                            else
                                            {
                                                Session["FreezeStatusCode"] = "";
                                            }


                                            if(jObject["branchCode"] != null)
                                            {
                                                Session["SolID"] = jObject["branchCode"].ToString().Trim();
                                            }
                                            else
                                            {
                                                Session["SolID"] = "";
                                            }

                                            if (jObject["modeOfOperation"] != null)
                                            {
                                                Session["Mop"] = jObject["modeOfOperation"].ToString().Trim();
                                            }
                                            else
                                            {
                                                Session["Mop"] = "";
                                            }

                                            if (jObject["staffIndicator"] != null)
                                            {
                                                Session["StaffAcc"] = (bool)jObject["staffIndicator"] ? "Y" : "N";
                                            }
                                            else
                                            {
                                                Session["StaffAcc"] = "";
                                            }

                                            if (jObject["productType"] != null)
                                            {
                                                Session["OffAcc"] = jObject["productType"].ToString() == "OAB" ? "Y" : "N";
                                            }
                                            else
                                            {
                                                Session["OffAcc"] = "";
                                            }



                                            //=========== Amol changes on 27/02/2024 for capturing SourceCustomerId end ======

                                            if (jObject["accountStatus"] != null)
                                                {
                                                if (jObject["accountStatus"].ToString().Trim() == "Active")
                                                {
                                                    if (jObject["accountName"] != null)
                                                    {
                                                        if (((Newtonsoft.Json.Linq.JContainer)jObject["relatedCustomerInfo"]).Count > 0)
                                                        {
                                                            logerror("In GetJointAcNms method relatedCustomerInfo count - ", ((Newtonsoft.Json.Linq.JContainer)jObject["relatedCustomerInfo"]).Count.ToString());
                                                            int iIndex = 0;

                                                            while (iIndex < jObject["relatedCustomerInfo"].Count())
                                                            {
                                                                //Call for account holders
                                                                logerror("In GetJointAcNms method related party customerId - ", jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                                //====== 4 uncomment when deployed on bank start ===================
                                                                string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, sEtoken, jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                                //string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, "", jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                                //string sCMPCResponse = testCMCP_Response();
                                                                //====== 4 uncomment when deployed on bank end ===================
                                                                logerror("In Join Ac", "Active and sCMPCResponse - " + sCMPCResponse);
                                                                //Get account holders
                                                                string sCustomerName = GetCMCPCustomerName(sCMPCResponse);
                                                                //string sCustomerName = getCustomerName(sCMPCResponse);
                                                                logerror("In Join Ac", "Active and GetCMCPCustomerName - " + sCustomerName);
                                                                //var jObject1 = Newtonsoft.Json.Linq.JObject.Parse(sCustomerName);
                                                                //string sname = jObject1["data"]["profileInfo"]["registeredName"].ToString().Trim();

                                                                if (sCustomerName != "")
                                                                {
                                                                    lAccNames.Add(sCustomerName);
                                                                }


                                                                logerror("Joint Account Name : ", sCustomerName.ToString());
                                                                iIndex++;
                                                            }

                                                            ViewBag.vbstatus = "SUCCESS";
                                                            ViewBag.vbcbsdls = null;

                                                            if (lAccNames.Count > 0)
                                                                ViewBag.vbAcctName = lAccNames;
                                                        }
                                                        else
                                                        {
                                                            lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                            ViewBag.vbstatus = "SUCCESS";
                                                            ViewBag.vbcbsdls = null;

                                                            if (lAccNames.Count > 0)
                                                                ViewBag.vbAcctName = lAccNames;
                                                        }

                                                        if (lAccNames.Count == 0)
                                                        {
                                                            lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                            ViewBag.vbstatus = "SUCCESS";
                                                            ViewBag.vbcbsdls = null;

                                                            if (lAccNames.Count > 0)
                                                                ViewBag.vbAcctName = lAccNames;
                                                        }
                                                        //======================================================================= comment by amol for CMCP account name on 07/06/2023 start ==================
                                                        //lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                        //ViewBag.vbstatus = "SUCCESS";
                                                        //ViewBag.vbcbsdls = null;

                                                        //if (lAccNames.Count > 0)
                                                        //    ViewBag.vbAcctName = lAccNames;
                                                        //======================================================================= comment by amol for CMCP account name on 07/06/2023 end ==================

                                                        if (jObject["productCode"].ToString().Trim() == "NRESA" || jObject["productCode"].ToString().Trim() == "NROSA"
                                                            || jObject["productCode"].ToString().Trim() == "NRESP" || jObject["productCode"].ToString().Trim() == "NROSP"
                                                            || jObject["productCode"].ToString().Trim() == "NRETR" || jObject["productCode"].ToString().Trim() == "NROTR"
                                                            || jObject["productCode"].ToString().Trim() == "NRET1" || jObject["productCode"].ToString().Trim() == "NROT1"
                                                            || jObject["productCode"].ToString().Trim() == "NRET3" || jObject["productCode"].ToString().Trim() == "NEPIS"
                                                            || jObject["productCode"].ToString().Trim() == "NOPIS" || jObject["productCode"].ToString().Trim() == "NREWL"
                                                            || jObject["productCode"].ToString().Trim() == "NROWL" || jObject["productCode"].ToString().Trim() == "NRSAV"
                                                            || jObject["productCode"].ToString().Trim() == "SFNRE" || jObject["productCode"].ToString().Trim() == "SFNRO"
                                                            || jObject["productCode"].ToString().Trim() == "NROT3")
                                                        {
                                                            ViewBag.sCAPA = "NRE Account";
                                                            ViewBag.vbNRE = "NRE Account";

                                                            if (jObject["productCode"].ToString().Trim() == "NRESA" || jObject["productCode"].ToString().Trim() == "NRESP"
                                                                || jObject["productCode"].ToString().Trim() == "NRETR" || jObject["productCode"].ToString().Trim() == "NRET1"
                                                                || jObject["productCode"].ToString().Trim() == "NRET3" || jObject["productCode"].ToString().Trim() == "NEPIS"
                                                                || jObject["productCode"].ToString().Trim() == "NREWL" || jObject["productCode"].ToString().Trim() == "NRSAV"
                                                                || jObject["productCode"].ToString().Trim() == "SFNRE")
                                                                Session["sNR"] = "NRE";
                                                            else if (jObject["productCode"].ToString().Trim() == "NROSA" || jObject["productCode"].ToString().Trim() == "NROSP"
                                                                || jObject["productCode"].ToString().Trim() == "NROTR" || jObject["productCode"].ToString().Trim() == "NROT1"
                                                                || jObject["productCode"].ToString().Trim() == "NOPIS" || jObject["productCode"].ToString().Trim() == "NROWL"
                                                                || jObject["productCode"].ToString().Trim() == "SFNRO" || jObject["productCode"].ToString().Trim() == "NROT3")
                                                                Session["sNR"] = "NRO";

                                                        }
                                                    }
                                                    else
                                                    {
                                                        ViewBag.vberror = "Invalid Account";
                                                        ViewBag.vbstatus = "SUCCESS";
                                                        ViewBag.vbcbsdls = null;
                                                        ViewBag.block = 1;
                                                    }

                                                    if (jObject["freezeStatusCode"].ToString().Trim() == "T")
                                                    {
                                                        ViewBag.sCAPA = "Total freeze";
                                                        ViewBag.vberror = "Account is Total freeze";
                                                        ViewBag.block = 1;
                                                    }
                                                    else if (jObject["freezeStatusCode"].ToString().Trim() == "C")
                                                    {
                                                        ViewBag.sCAPA = "Credit freeze";
                                                        ViewBag.vberror = "Account is Credit freeze";
                                                        ViewBag.block = 1;
                                                    }
                                                    else if (jObject["freezeStatusCode"].ToString().Trim() == "D")
                                                    {
                                                        ViewBag.sCAPA = "Debit freeze";
                                                        ViewBag.vberror = "Account is Debit freeze";
                                                        ViewBag.block = 1;
                                                    }

                                                }
                                                else if (jObject["accountStatus"].ToString().Trim() == "Dormant")
                                                {
                                                    if (jObject["accountName"] != null)
                                                    {
                                                        if (((Newtonsoft.Json.Linq.JContainer)jObject["relatedCustomerInfo"]).Count > 0)
                                                        {
                                                            int iIndex = 0;

                                                            while (iIndex < jObject["relatedCustomerInfo"].Count())
                                                            {
                                                                //Call for account holders

                                                                //====== 5 uncomment when deployed on bank start ===================
                                                                string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, sEtoken, jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                                //string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, "", jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                                //====== 5 uncomment when deployed on bank end ===================

                                                                //Get account holders
                                                                string sCustomerName = GetCMCPCustomerName(sCMPCResponse);
                                                                //string sCustomerName = getCustomerName(sCMPCResponse);

                                                                //var jObject1 = Newtonsoft.Json.Linq.JObject.Parse(sCustomerName);
                                                                //string sname = jObject1["data"]["profileInfo"]["registeredName"].ToString().Trim();

                                                                if (sCustomerName != "")
                                                                {
                                                                    lAccNames.Add(sCustomerName);
                                                                }


                                                                logerror("Joint Account Name : ", sCustomerName.ToString());
                                                                iIndex++;
                                                            }

                                                            ViewBag.vbstatus = "SUCCESS";
                                                            ViewBag.vbcbsdls = null;

                                                            if (lAccNames.Count > 0)
                                                                ViewBag.vbAcctName = lAccNames;
                                                        }
                                                        else
                                                        {
                                                            lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                            ViewBag.vbstatus = "SUCCESS";
                                                            ViewBag.vbcbsdls = null;

                                                            if (lAccNames.Count > 0)
                                                                ViewBag.vbAcctName = lAccNames;
                                                        }

                                                        if (lAccNames.Count == 0)
                                                        {
                                                            lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                            ViewBag.vbstatus = "SUCCESS";
                                                            ViewBag.vbcbsdls = null;

                                                            if (lAccNames.Count > 0)
                                                                ViewBag.vbAcctName = lAccNames;
                                                        }

                                                        //======================================================================= comment by amol for CMCP account name on 07/06/2023 start ==================
                                                        //lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                        //ViewBag.vbstatus = "SUCCESS";
                                                        //ViewBag.vbcbsdls = null;

                                                        //if (lAccNames.Count > 0)
                                                        //    ViewBag.vbAcctName = lAccNames;
                                                        //======================================================================= comment by amol for CMCP account name on 07/06/2023 end ==================

                                                    }

                                                    ViewBag.vberror = "Dormant Account";
                                                    ViewBag.vbstatus = "SUCCESS";
                                                    ViewBag.vbcbsdls = null;
                                                    ViewBag.block = 1;

                                                }
                                                else if (jObject["accountStatus"].ToString().Trim() == "Inactive")
                                                {
                                                    if (jObject["accountName"] != null)
                                                    {
                                                        if (((Newtonsoft.Json.Linq.JContainer)jObject["relatedCustomerInfo"]).Count > 0)
                                                        {
                                                            int iIndex = 0;

                                                            while (iIndex < jObject["relatedCustomerInfo"].Count())
                                                            {
                                                                //Call for account holders

                                                                //====== 6 uncomment when deployed on bank start ===================
                                                                string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, sEtoken, jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                                //string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, "", jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                                //====== 6 uncomment when deployed on bank end ===================

                                                                //Get account holders
                                                                string sCustomerName = GetCMCPCustomerName(sCMPCResponse);
                                                                //string sCustomerName = getCustomerName(sCMPCResponse);

                                                                //var jObject1 = Newtonsoft.Json.Linq.JObject.Parse(sCustomerName);
                                                                //string sname = jObject1["data"]["profileInfo"]["registeredName"].ToString().Trim();

                                                                if (sCustomerName != "")
                                                                {
                                                                    lAccNames.Add(sCustomerName);
                                                                }


                                                                logerror("Joint Account Name : ", sCustomerName.ToString());
                                                                iIndex++;
                                                            }

                                                            ViewBag.vbstatus = "SUCCESS";
                                                            ViewBag.vbcbsdls = null;

                                                            if (lAccNames.Count > 0)
                                                                ViewBag.vbAcctName = lAccNames;
                                                        }
                                                        else
                                                        {
                                                            lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                            ViewBag.vbstatus = "SUCCESS";
                                                            ViewBag.vbcbsdls = null;

                                                            if (lAccNames.Count > 0)
                                                                ViewBag.vbAcctName = lAccNames;
                                                        }

                                                        if (lAccNames.Count == 0)
                                                        {
                                                            lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                            ViewBag.vbstatus = "SUCCESS";
                                                            ViewBag.vbcbsdls = null;

                                                            if (lAccNames.Count > 0)
                                                                ViewBag.vbAcctName = lAccNames;
                                                        }

                                                        //======================================================================= comment by amol for CMCP account name on 07/06/2023 start ==================
                                                        //lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                        //ViewBag.vbstatus = "SUCCESS";
                                                        //ViewBag.vbcbsdls = null;

                                                        //if (lAccNames.Count > 0)
                                                        //    ViewBag.vbAcctName = lAccNames;
                                                        //======================================================================= comment by amol for CMCP account name on 07/06/2023 end ==================
                                                        if (jObject["productCode"].ToString().Trim() == "NRESA" || jObject["productCode"].ToString().Trim() == "NROSA"
                                                           || jObject["productCode"].ToString().Trim() == "NRESP" || jObject["productCode"].ToString().Trim() == "NROSP"
                                                           || jObject["productCode"].ToString().Trim() == "NRETR" || jObject["productCode"].ToString().Trim() == "NROTR"
                                                           || jObject["productCode"].ToString().Trim() == "NRET1" || jObject["productCode"].ToString().Trim() == "NROT1"
                                                           || jObject["productCode"].ToString().Trim() == "NRET3" || jObject["productCode"].ToString().Trim() == "NEPIS"
                                                           || jObject["productCode"].ToString().Trim() == "NOPIS" || jObject["productCode"].ToString().Trim() == "NREWL"
                                                           || jObject["productCode"].ToString().Trim() == "NROWL" || jObject["productCode"].ToString().Trim() == "NRSAV"
                                                           || jObject["productCode"].ToString().Trim() == "SFNRE" || jObject["productCode"].ToString().Trim() == "SFNRO"
                                                           || jObject["productCode"].ToString().Trim() == "NROT3")
                                                        {
                                                            ViewBag.sCAPA = "NRE Account";
                                                            ViewBag.vbNRE = "NRE Account";

                                                            if (jObject["productCode"].ToString().Trim() == "NRESA" || jObject["productCode"].ToString().Trim() == "NRESP"
                                                                || jObject["productCode"].ToString().Trim() == "NRETR" || jObject["productCode"].ToString().Trim() == "NRET1"
                                                                || jObject["productCode"].ToString().Trim() == "NRET3" || jObject["productCode"].ToString().Trim() == "NEPIS"
                                                                || jObject["productCode"].ToString().Trim() == "NREWL" || jObject["productCode"].ToString().Trim() == "NRSAV"
                                                                || jObject["productCode"].ToString().Trim() == "SFNRE")
                                                                Session["sNR"] = "NRE";
                                                            else if (jObject["productCode"].ToString().Trim() == "NROSA" || jObject["productCode"].ToString().Trim() == "NROSP"
                                                                || jObject["productCode"].ToString().Trim() == "NROTR" || jObject["productCode"].ToString().Trim() == "NROT1"
                                                                || jObject["productCode"].ToString().Trim() == "NOPIS" || jObject["productCode"].ToString().Trim() == "NROWL"
                                                                || jObject["productCode"].ToString().Trim() == "SFNRO" || jObject["productCode"].ToString().Trim() == "NROT3")
                                                                Session["sNR"] = "NRO";

                                                        }







                                                    }

                                                    ViewBag.sCAPA = "Account is inactive";
                                                    ViewBag.vberror = "Account is inactive";
                                                    ViewBag.vbstatus = "SUCCESS";
                                                    ViewBag.vbcbsdls = null;
                                                    ViewBag.block = 1;

                                                }


                                            }
                                            else
                                            {
                                                ViewBag.vberror = "Invalid Account";
                                                ViewBag.vbstatus = "SUCCESS";
                                                ViewBag.vbcbsdls = null;
                                                ViewBag.block = 1;

                                            }
                                        }
                                        else
                                        {
                                            //=========== Amol changes on 27/02/2024 for capturing SourceCustomerId start ======
                                            if (jObject["sourceCustomerId"] != null)
                                            {
                                                Session["SourceCustomerId"] = jObject["sourceCustomerId"].ToString().Trim();
                                            }
                                            else
                                            {
                                                Session["SourceCustomerId"] = "";
                                            }

                                            if (jObject["accountCurrencyCode"] != null)
                                            {
                                                Session["AccountCurrency"] = jObject["accountCurrencyCode"].ToString().Trim();
                                                ViewBag.Currency = jObject["accountCurrencyCode"].ToString().Trim();
                                            }
                                            else
                                            {
                                                Session["AccountCurrency"] = "";
                                                ViewBag.Currency = "";
                                            }

                                            if (jObject["openedDate"] != null)
                                            {
                                                openDate = Convert.ToInt64(jObject["openedDate"].ToString().Trim());

                                                //var dateTime = DateTimeOffset.FromUnixTimeSeconds(1550962800);
                                            }
                                            else
                                            {
                                                openDate = 0;
                                            }

                                            if (openDate != 0)
                                            {
                                                // DateTimeOffset from Unix timestamp
                                                DateTimeOffset dateTimeFromUnix = DateTimeOffset.FromUnixTimeMilliseconds(openDate);
                                                // Current DateTimeOffset
                                                DateTimeOffset currentDateTime = DateTimeOffset.Now;
                                                // Calculate the difference
                                                //TimeSpan difference = currentDateTime - dateTimeFromUnix;

                                                int differenceInMonths = CalculateDifferenceInMonths(currentDateTime, dateTimeFromUnix);

                                                // Check if the difference is greater than 6 months
                                                if (differenceInMonths > 6)
                                                {
                                                    //Console.WriteLine("The difference is greater than six months.");
                                                    Session["IsOpenedDateOld"] = "Y";
                                                }
                                                else
                                                {
                                                    //Console.WriteLine("The difference is not greater than six months.");
                                                    Session["IsOpenedDateOld"] = "N";
                                                }
                                            }
                                            else
                                            {
                                                Session["IsOpenedDateOld"] = "";
                                            }

                                            if (jObject["productCode"] != null)
                                            {
                                                Session["productCode"] = jObject["productCode"].ToString().Trim();
                                            }
                                            else
                                            {
                                                Session["productCode"] = "";
                                            }

                                            if (jObject["productType"] != null)
                                            {
                                                Session["productType"] = jObject["productType"].ToString().Trim();
                                            }
                                            else
                                            {
                                                Session["productType"] = "";
                                            }

                                            if (jObject["accountBalances"] != null)
                                            {
                                                Session["accountBalances"] = jObject["accountBalances"]["effectiveAvailableAmount"] == null ? "0" :
                                                                            jObject["accountBalances"]["effectiveAvailableAmount"].ToString().Trim();
                                            }
                                            else
                                            {
                                                Session["accountBalances"] = "0";
                                            }

                                            if (jObject["accountStatus"] != null)
                                            {
                                                Session["AccountStatus"] = jObject["accountStatus"].ToString().Trim();
                                            }
                                            else
                                            {
                                                Session["AccountStatus"] = "";
                                            }

                                            if (jObject["freezeStatusCode"] != null)
                                            {
                                                Session["FreezeStatusCode"] = jObject["freezeStatusCode"].ToString().Trim();
                                            }
                                            else
                                            {
                                                Session["FreezeStatusCode"] = "";
                                            }


                                            if (jObject["branchCode"] != null)
                                            {
                                                Session["SolID"] = jObject["branchCode"].ToString().Trim();
                                            }
                                            else
                                            {
                                                Session["SolID"] = "";
                                            }

                                            if (jObject["modeOfOperation"] != null)
                                            {
                                                Session["Mop"] = jObject["modeOfOperation"].ToString().Trim();
                                            }
                                            else
                                            {
                                                Session["Mop"] = "";
                                            }

                                            if (jObject["staffIndicator"] != null)
                                            {
                                                Session["StaffAcc"] = (bool)jObject["staffIndicator"] ? "Y" : "N";
                                            }
                                            else
                                            {
                                                Session["StaffAcc"] = "";
                                            }

                                            if (jObject["productType"] != null)
                                            {
                                                Session["OffAcc"] = jObject["productType"].ToString() == "OAB" ? "Y" : "N";
                                            }
                                            else
                                            {
                                                Session["OffAcc"] = "";
                                            }



                                            //=========== Amol changes on 27/02/2024 for capturing SourceCustomerId end ======

                                            if (jObject["accountName"] != null)
                                            {
                                                if (((Newtonsoft.Json.Linq.JContainer)jObject["relatedCustomerInfo"]).Count > 0)
                                                {
                                                    int iIndex = 0;

                                                    while (iIndex < jObject["relatedCustomerInfo"].Count())
                                                    {
                                                        //Call for account holders
                                                        //====== 7 uncomment when deployed on bank start ===================
                                                        string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, sEtoken, jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                        //string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, "", jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                        //====== 7 uncomment when deployed on bank start ===================
                                                        //Get account holders
                                                        string sCustomerName = GetCMCPCustomerName(sCMPCResponse);
                                                        //string sCustomerName = getCustomerName(sCMPCResponse);

                                                        //var jObject1 = Newtonsoft.Json.Linq.JObject.Parse(sCustomerName);
                                                        //string sname = jObject1["data"]["profileInfo"]["registeredName"].ToString().Trim();

                                                        if (sCustomerName != "")
                                                        {
                                                            lAccNames.Add(sCustomerName);
                                                        }

                                                        //lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                        //logerror("Joint Account Name : ", sCustomerName.ToString());
                                                        iIndex++;
                                                    }

                                                    ViewBag.vbstatus = "SUCCESS";
                                                    ViewBag.vbcbsdls = null;

                                                    if (lAccNames.Count > 0)
                                                        ViewBag.vbAcctName = lAccNames;
                                                }
                                                else
                                                {
                                                    lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                    ViewBag.vbstatus = "SUCCESS";
                                                    ViewBag.vbcbsdls = null;

                                                    if (lAccNames.Count > 0)
                                                        ViewBag.vbAcctName = lAccNames;
                                                }

                                                if (lAccNames.Count == 0)
                                                {
                                                    lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                    ViewBag.vbstatus = "SUCCESS";
                                                    ViewBag.vbcbsdls = null;

                                                    if (lAccNames.Count > 0)
                                                        ViewBag.vbAcctName = lAccNames;
                                                }

                                                //======================================================================= comment by amol for CMCP account name on 07/06/2023 start ==================
                                                //lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                //ViewBag.vbstatus = "SUCCESS";
                                                //ViewBag.vbcbsdls = null;

                                                //if (lAccNames.Count > 0)
                                                //    ViewBag.vbAcctName = lAccNames;
                                                //======================================================================= comment by amol for CMCP account name on 07/06/2023 start ==================
                                            }

                                            ViewBag.sCAPA = "Account is closed";
                                            ViewBag.vberror = "Account is closed";
                                            ViewBag.vbstatus = "SUCCESS";
                                            ViewBag.vbcbsdls = null;
                                            ViewBag.block = 1;

                                        }
                                    }
                                    else
                                    {
                                        ViewBag.vberror = "API Data Not Fetched";
                                        ViewBag.vbstatus = "FAILED";
                                        ViewBag.vbcbsdls = null;
                                        ViewBag.block = 1;
                                    }
                                    
                                }

                            }

                        }
                        else
                        {
                            string sgetAccountDetailsSIB = JointAccount();
                            //string sgetAccountDetailsSIB = ClosedAccount();


                            //string sgetAccountDetailsSIB = getAccountDetailsSIB(sServiceUrl, sClientId, sClientSecretKey, sSenderCode, sSenderName, ac);
                            var jObject = Newtonsoft.Json.Linq.JObject.Parse(sgetAccountDetailsSIB);



                            if (jObject["Response"] != null)
                            {
                                if (jObject["Response"]["Body"]["RelPartyRec"] != null)
                                {
                                    if (jObject["Response"]["Body"]["RelPartyRec"].Count() > 0)
                                    {
                                        lAccNames.Add(jObject["Response"]["Body"]["AcctName"].ToString().Trim());
                                        int iIndex = 0;
                                        while (iIndex < jObject["Response"]["Body"]["RelPartyRec"].Count())
                                        {
                                            if (jObject["Response"]["Body"]["AcctName"].ToString().Trim() != jObject["Response"]["Body"]["RelPartyRec"][iIndex]["CustId"]["PersonName"]["Name"].ToString().Trim())
                                                lAccNames.Add(jObject["Response"]["Body"]["RelPartyRec"][iIndex]["CustId"]["PersonName"]["Name"].ToString().Trim());

                                            iIndex++;
                                        }
                                    }

                                    else
                                    {
                                        lAccNames.Add(jObject["Response"]["Body"]["AcctName"].ToString().Trim());
                                        ViewBag.vbstatus = "No";
                                        ViewBag.vbcbsdls = null;
                                    }
                                }
                                else if (jObject["Response"]["Body"]["Description"] != null)
                                {
                                    ViewBag.sClosedAccount = jObject["Response"]["Body"]["Description"].ToString().Trim();
                                    ViewBag.sClosedAccount = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToUpper(ViewBag.sClosedAccount.ToString().Trim());

                                    if (ViewBag.sClosedAccount == "ACCOUNT IS CLOSED")
                                    {
                                        ViewBag.vbstatus = "No";
                                        ViewBag.vbcbsdls = null;
                                    }
                                }

                                ViewBag.vbstatus = "SUCCESS";
                                ViewBag.vbcbsdls = null;

                                if (lAccNames.Count > 0)
                                    ViewBag.vbAcctName = lAccNames;

                            }

                            else if (jObject["ErrorResponse"] != null)//
                            {
                                ViewBag.vberror = jObject["ErrorResponse"]["Status"]["Desc"].ToString();
                                ViewBag.vbstatus = "No";
                                ViewBag.vbcbsdls = null;
                            }
                            else if (jObject["Request"] != null)
                            {
                                ViewBag.vberror = jObject["Request"]["Description"].ToString();
                                ViewBag.vbstatus = "No";
                                ViewBag.vbcbsdls = null;
                            }
                            else
                            {
                                ViewBag.vberror = "CBS API Service unavailable!";
                                ViewBag.vbstatus = "No";
                                ViewBag.vbcbsdls = null;
                            }

                        }

                    }//NewApiCall
                    else// for testing purpose block
                    {
                        ViewBag.Currency = "";
                        ViewBag.sCAPA = "";
                        ViewBag.vbNRE = "";
                        Session["sNR"] = "";
                        Session["SourceCustomerId"] = "";
                        Session["AccountCurrency"] = "";
                        Session["IsOpenedDateOld"] = "";
                        Session["productCode"] = "";
                        Session["productType"] = "";
                        Session["accountBalances"] = "0";

                        long openDate = 0;

                        //sgetAccountDetailsDBS = testCMCP_Response();
                        sgetAccountDetailsDBS = getAccountDetailsDBSResponseTest();
                        var newResponse = sgetAccountDetailsDBS.Replace(", Please", " Please");
                        var jObject = Newtonsoft.Json.Linq.JObject.Parse(newResponse);

                        if (jObject["error"] != null)
                        {
                            if (jObject["errorDescription"] != null)
                            {
                                ViewBag.vberror = jObject["errorDescription"].ToString();
                                ViewBag.vbstatus = "SUCCESS";
                                ViewBag.vbcbsdls = null;
                                ViewBag.block = 1;
                            }
                            else
                            {
                                ViewBag.vberror = "Invalid Account";
                                ViewBag.vbstatus = "SUCCESS";
                                ViewBag.vbcbsdls = null;
                                ViewBag.block = 1;
                            }

                        }
                        else
                        {
                            if (jObject["accountClosedFlag"].ToString().Trim().ToUpper() != "Y")
                            {
                                logerror("In GetJointAcNms method sourceCustomerId - ", jObject["sourceCustomerId"].ToString().Trim());
                                logerror("In GetJointAcNms method accountCurrency - ", jObject["accountCurrency"].ToString().Trim());
                                //=========== Amol changes on 27/02/2024 for capturing SourceCustomerId start ======
                                if (jObject["sourceCustomerId"] != null)
                                {
                                    Session["SourceCustomerId"] = jObject["sourceCustomerId"].ToString().Trim();
                                    //cbsdtls.SourceCustomerId = jObject["sourceCustomerId"].ToString().Trim();
                                }
                                else
                                {
                                    Session["SourceCustomerId"] = "";
                                }

                                if (jObject["accountCurrencyCode"] != null)
                                {
                                    Session["AccountCurrency"] = jObject["accountCurrencyCode"].ToString().Trim();
                                    ViewBag.Currency = jObject["accountCurrencyCode"].ToString().Trim();
                                    //cbsdtls.Currency = jObject["accountCurrency"].ToString().Trim();
                                }
                                else
                                {
                                    Session["AccountCurrency"] = "";
                                    ViewBag.Currency = "";
                                }

                                if (jObject["openedDate"] != null)
                                {
                                    openDate = Convert.ToInt64(jObject["openedDate"].ToString().Trim());

                                    //var dateTime = DateTimeOffset.FromUnixTimeSeconds(1550962800);
                                }
                                else
                                {
                                    openDate = 0;
                                }

                                if (openDate != 0)
                                {
                                    // DateTimeOffset from Unix timestamp
                                    DateTimeOffset dateTimeFromUnix = DateTimeOffset.FromUnixTimeMilliseconds(openDate);
                                    // Current DateTimeOffset
                                    DateTimeOffset currentDateTime = DateTimeOffset.Now;
                                    // Calculate the difference
                                    //TimeSpan difference = currentDateTime - dateTimeFromUnix;

                                    int differenceInMonths = CalculateDifferenceInMonths(currentDateTime, dateTimeFromUnix);

                                    // Check if the difference is greater than 6 months
                                    if (differenceInMonths > 6)
                                    {
                                        //Console.WriteLine("The difference is greater than six months.");
                                        Session["IsOpenedDateOld"] = "Y";
                                    }
                                    else
                                    {
                                        //Console.WriteLine("The difference is not greater than six months.");
                                        Session["IsOpenedDateOld"] = "N";
                                    }
                                }
                                else
                                {
                                    Session["IsOpenedDateOld"] = "";
                                }

                                if (jObject["productCode"] != null)
                                {
                                    Session["productCode"] = jObject["productCode"].ToString().Trim();
                                }
                                else
                                {
                                    Session["productCode"] = "";
                                }

                                if (jObject["productType"] != null)
                                {
                                    Session["productType"] = jObject["productType"].ToString().Trim();
                                }
                                else
                                {
                                    Session["productType"] = "";
                                }

                                if (jObject["accountBalances"] != null)
                                {
                                    Session["accountBalances"] = jObject["accountBalances"]["effectiveAvailableAmount"] == null ? "0" :
                                                                jObject["accountBalances"]["effectiveAvailableAmount"].ToString().Trim();
                                }
                                else
                                {
                                    Session["accountBalances"] = "0";
                                }

                                logerror("In GetJointAcNms method sourceCustomerId session - ", Session["SourceCustomerId"].ToString().Trim());
                                logerror("In GetJointAcNms method accountCurrency session - ", Session["AccountCurrency"].ToString().Trim());
                                //=========== Amol changes on 27/02/2024 for capturing SourceCustomerId end ======

                                if (jObject["accountStatus"] != null)
                                {
                                    if (jObject["accountStatus"].ToString().Trim() == "Active")
                                    {
                                        if (jObject["accountName"] != null)
                                        {
                                            if (((Newtonsoft.Json.Linq.JContainer)jObject["relatedCustomerInfo"]).Count > 0)
                                            {
                                                logerror("In GetJointAcNms method relatedCustomerInfo count - ", ((Newtonsoft.Json.Linq.JContainer)jObject["relatedCustomerInfo"]).Count.ToString());
                                                int iIndex = 0;

                                                while (iIndex < jObject["relatedCustomerInfo"].Count())
                                                {
                                                    //Call for account holders
                                                    logerror("In GetJointAcNms method related party customerId - ", jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                    //====== 4 uncomment when deployed on bank start ===================
                                                    //string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, sEtoken, jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                    //string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, "", jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                    //string sCMPCResponse = testCMCP_Response();
                                                    //====== 4 uncomment when deployed on bank end ===================
                                                    //logerror("In Join Ac", "Active and sCMPCResponse - " + sCMPCResponse);
                                                    //Get account holders
                                                    string sCustomerName = GetCMCPCustomerNameTest();
                                                    //string sCustomerName = getCustomerName(sCMPCResponse);
                                                    logerror("In Join Ac", "Active and GetCMCPCustomerName - " + sCustomerName);
                                                    //var jObject1 = Newtonsoft.Json.Linq.JObject.Parse(sCustomerName);
                                                    //string sname = jObject1["data"]["profileInfo"]["registeredName"].ToString().Trim();

                                                    if (sCustomerName != "")
                                                    {
                                                        lAccNames.Add(sCustomerName);
                                                    }


                                                    logerror("Joint Account Name : ", sCustomerName.ToString());
                                                    iIndex++;
                                                }

                                                ViewBag.vbstatus = "SUCCESS";
                                                ViewBag.vbcbsdls = null;

                                                if (lAccNames.Count > 0)
                                                    ViewBag.vbAcctName = lAccNames;
                                            }
                                            else
                                            {
                                                lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                ViewBag.vbstatus = "SUCCESS";
                                                ViewBag.vbcbsdls = null;

                                                if (lAccNames.Count > 0)
                                                    ViewBag.vbAcctName = lAccNames;
                                            }

                                            if (lAccNames.Count == 0)
                                            {
                                                lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                ViewBag.vbstatus = "SUCCESS";
                                                ViewBag.vbcbsdls = null;

                                                if (lAccNames.Count > 0)
                                                    ViewBag.vbAcctName = lAccNames;
                                            }
                                            //======================================================================= comment by amol for CMCP account name on 07/06/2023 start ==================
                                            //lAccNames.Add(jObject["accountName"].ToString().Trim());
                                            //ViewBag.vbstatus = "SUCCESS";
                                            //ViewBag.vbcbsdls = null;

                                            //if (lAccNames.Count > 0)
                                            //    ViewBag.vbAcctName = lAccNames;
                                            //======================================================================= comment by amol for CMCP account name on 07/06/2023 end ==================

                                            if (jObject["productCode"].ToString().Trim() == "NRESA" || jObject["productCode"].ToString().Trim() == "NROSA"
                                                            || jObject["productCode"].ToString().Trim() == "NRESP" || jObject["productCode"].ToString().Trim() == "NROSP"
                                                            || jObject["productCode"].ToString().Trim() == "NRETR" || jObject["productCode"].ToString().Trim() == "NROTR"
                                                            || jObject["productCode"].ToString().Trim() == "NRET1" || jObject["productCode"].ToString().Trim() == "NROT1"
                                                            || jObject["productCode"].ToString().Trim() == "NRET3" || jObject["productCode"].ToString().Trim() == "NEPIS"
                                                            || jObject["productCode"].ToString().Trim() == "NOPIS" || jObject["productCode"].ToString().Trim() == "NREWL"
                                                            || jObject["productCode"].ToString().Trim() == "NROWL" || jObject["productCode"].ToString().Trim() == "NRSAV"
                                                            || jObject["productCode"].ToString().Trim() == "SFNRE" || jObject["productCode"].ToString().Trim() == "SFNRO"
                                                            || jObject["productCode"].ToString().Trim() == "NROT3")
                                            {
                                                ViewBag.sCAPA = "NRE Account";
                                                ViewBag.vbNRE = "NRE Account";

                                                if (jObject["productCode"].ToString().Trim() == "NRESA" || jObject["productCode"].ToString().Trim() == "NRESP"
                                                    || jObject["productCode"].ToString().Trim() == "NRETR" || jObject["productCode"].ToString().Trim() == "NRET1"
                                                    || jObject["productCode"].ToString().Trim() == "NRET3" || jObject["productCode"].ToString().Trim() == "NEPIS"
                                                    || jObject["productCode"].ToString().Trim() == "NREWL" || jObject["productCode"].ToString().Trim() == "NRSAV"
                                                    || jObject["productCode"].ToString().Trim() == "SFNRE")
                                                    Session["sNR"] = "NRE";
                                                else if (jObject["productCode"].ToString().Trim() == "NROSA" || jObject["productCode"].ToString().Trim() == "NROSP"
                                                    || jObject["productCode"].ToString().Trim() == "NROTR" || jObject["productCode"].ToString().Trim() == "NROT1"
                                                    || jObject["productCode"].ToString().Trim() == "NOPIS" || jObject["productCode"].ToString().Trim() == "NROWL"
                                                    || jObject["productCode"].ToString().Trim() == "SFNRO" || jObject["productCode"].ToString().Trim() == "NROT3")
                                                    Session["sNR"] = "NRO";

                                            }
                                        }
                                        else
                                        {
                                            ViewBag.vberror = "Invalid Account";
                                            ViewBag.vbstatus = "SUCCESS";
                                            ViewBag.vbcbsdls = null;
                                            ViewBag.block = 1;
                                        }

                                        if (jObject["freezeStatusCode"].ToString().Trim() == "T")
                                        {
                                            ViewBag.sCAPA = "Total freeze";
                                            ViewBag.vberror = "Account is Total freeze";
                                            ViewBag.block = 1;
                                        }
                                        else if (jObject["freezeStatusCode"].ToString().Trim() == "C")
                                        {
                                            ViewBag.sCAPA = "Credit freeze";
                                            ViewBag.vberror = "Account is Credit freeze";
                                            ViewBag.block = 1;
                                        }
                                        else if (jObject["freezeStatusCode"].ToString().Trim() == "D")
                                        {
                                            ViewBag.sCAPA = "Debit freeze";
                                            ViewBag.vberror = "Account is Debit freeze";
                                            ViewBag.block = 1;
                                        }

                                    }
                                    else if (jObject["accountStatus"].ToString().Trim() == "Dormant")
                                    {
                                        if (jObject["accountName"] != null)
                                        {
                                            if (((Newtonsoft.Json.Linq.JContainer)jObject["relatedCustomerInfo"]).Count > 0)
                                            {
                                                int iIndex = 0;

                                                while (iIndex < jObject["relatedCustomerInfo"].Count())
                                                {
                                                    //Call for account holders

                                                    //====== 5 uncomment when deployed on bank start ===================
                                                    //string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, sEtoken, jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                    //string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, "", jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                    //====== 5 uncomment when deployed on bank end ===================

                                                    //Get account holders
                                                    string sCustomerName = GetCMCPCustomerNameTest();
                                                    //string sCustomerName = getCustomerName(sCMPCResponse);

                                                    //var jObject1 = Newtonsoft.Json.Linq.JObject.Parse(sCustomerName);
                                                    //string sname = jObject1["data"]["profileInfo"]["registeredName"].ToString().Trim();

                                                    if (sCustomerName != "")
                                                    {
                                                        lAccNames.Add(sCustomerName);
                                                    }


                                                    logerror("Joint Account Name : ", sCustomerName.ToString());
                                                    iIndex++;
                                                }

                                                ViewBag.vbstatus = "SUCCESS";
                                                ViewBag.vbcbsdls = null;

                                                if (lAccNames.Count > 0)
                                                    ViewBag.vbAcctName = lAccNames;
                                            }
                                            else
                                            {
                                                lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                ViewBag.vbstatus = "SUCCESS";
                                                ViewBag.vbcbsdls = null;

                                                if (lAccNames.Count > 0)
                                                    ViewBag.vbAcctName = lAccNames;
                                            }

                                            if (lAccNames.Count == 0)
                                            {
                                                lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                ViewBag.vbstatus = "SUCCESS";
                                                ViewBag.vbcbsdls = null;

                                                if (lAccNames.Count > 0)
                                                    ViewBag.vbAcctName = lAccNames;
                                            }

                                            //======================================================================= comment by amol for CMCP account name on 07/06/2023 start ==================
                                            //lAccNames.Add(jObject["accountName"].ToString().Trim());
                                            //ViewBag.vbstatus = "SUCCESS";
                                            //ViewBag.vbcbsdls = null;

                                            //if (lAccNames.Count > 0)
                                            //    ViewBag.vbAcctName = lAccNames;
                                            //======================================================================= comment by amol for CMCP account name on 07/06/2023 end ==================

                                        }

                                        ViewBag.vberror = "Dormant Account";
                                        ViewBag.vbstatus = "SUCCESS";
                                        ViewBag.vbcbsdls = null;
                                        ViewBag.block = 1;

                                    }
                                    else if (jObject["accountStatus"].ToString().Trim() == "Inactive")
                                    {
                                        if (jObject["accountName"] != null)
                                        {
                                            if (((Newtonsoft.Json.Linq.JContainer)jObject["relatedCustomerInfo"]).Count > 0)
                                            {
                                                int iIndex = 0;

                                                while (iIndex < jObject["relatedCustomerInfo"].Count())
                                                {
                                                    //Call for account holders

                                                    //====== 6 uncomment when deployed on bank start ===================
                                                    //string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, sEtoken, jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                    //string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, "", jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                    //====== 6 uncomment when deployed on bank end ===================

                                                    //Get account holders
                                                    string sCustomerName = GetCMCPCustomerNameTest();
                                                    //string sCustomerName = getCustomerName(sCMPCResponse);

                                                    //var jObject1 = Newtonsoft.Json.Linq.JObject.Parse(sCustomerName);
                                                    //string sname = jObject1["data"]["profileInfo"]["registeredName"].ToString().Trim();

                                                    if (sCustomerName != "")
                                                    {
                                                        lAccNames.Add(sCustomerName);
                                                    }


                                                    logerror("Joint Account Name : ", sCustomerName.ToString());
                                                    iIndex++;
                                                }

                                                ViewBag.vbstatus = "SUCCESS";
                                                ViewBag.vbcbsdls = null;

                                                if (lAccNames.Count > 0)
                                                    ViewBag.vbAcctName = lAccNames;
                                            }
                                            else
                                            {
                                                lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                ViewBag.vbstatus = "SUCCESS";
                                                ViewBag.vbcbsdls = null;

                                                if (lAccNames.Count > 0)
                                                    ViewBag.vbAcctName = lAccNames;
                                            }

                                            if (lAccNames.Count == 0)
                                            {
                                                lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                ViewBag.vbstatus = "SUCCESS";
                                                ViewBag.vbcbsdls = null;

                                                if (lAccNames.Count > 0)
                                                    ViewBag.vbAcctName = lAccNames;
                                            }

                                            //======================================================================= comment by amol for CMCP account name on 07/06/2023 start ==================
                                            //lAccNames.Add(jObject["accountName"].ToString().Trim());
                                            //ViewBag.vbstatus = "SUCCESS";
                                            //ViewBag.vbcbsdls = null;

                                            //if (lAccNames.Count > 0)
                                            //    ViewBag.vbAcctName = lAccNames;
                                            //======================================================================= comment by amol for CMCP account name on 07/06/2023 end ==================


                                        }

                                        ViewBag.sCAPA = "Account is inactive";
                                        ViewBag.vberror = "Account is inactive";
                                        ViewBag.vbstatus = "SUCCESS";
                                        ViewBag.vbcbsdls = null;
                                        ViewBag.block = 1;

                                    }


                                }
                                else
                                {
                                    ViewBag.vberror = "Invalid Account";
                                    ViewBag.vbstatus = "SUCCESS";
                                    ViewBag.vbcbsdls = null;
                                    ViewBag.block = 1;

                                }
                            }
                            else
                            {
                                //=========== Amol changes on 27/02/2024 for capturing SourceCustomerId start ======
                                if (jObject["sourceCustomerId"] != null)
                                {
                                    Session["SourceCustomerId"] = jObject["sourceCustomerId"].ToString().Trim();
                                }
                                else
                                {
                                    Session["SourceCustomerId"] = "";
                                }

                                if (jObject["accountCurrencyCode"] != null)
                                {
                                    Session["AccountCurrency"] = jObject["accountCurrencyCode"].ToString().Trim();
                                    ViewBag.Currency = jObject["accountCurrencyCode"].ToString().Trim();
                                }
                                else
                                {
                                    Session["AccountCurrency"] = "";
                                    ViewBag.Currency = "";
                                }

                                if (jObject["openedDate"] != null)
                                {
                                    openDate = Convert.ToInt64(jObject["openedDate"].ToString().Trim());

                                    //var dateTime = DateTimeOffset.FromUnixTimeSeconds(1550962800);
                                }
                                else
                                {
                                    openDate = 0;
                                }

                                if (openDate != 0)
                                {
                                    // DateTimeOffset from Unix timestamp
                                    DateTimeOffset dateTimeFromUnix = DateTimeOffset.FromUnixTimeMilliseconds(openDate);
                                    // Current DateTimeOffset
                                    DateTimeOffset currentDateTime = DateTimeOffset.Now;
                                    // Calculate the difference
                                    //TimeSpan difference = currentDateTime - dateTimeFromUnix;

                                    int differenceInMonths = CalculateDifferenceInMonths(currentDateTime, dateTimeFromUnix);

                                    // Check if the difference is greater than 6 months
                                    if (differenceInMonths > 6)
                                    {
                                        //Console.WriteLine("The difference is greater than six months.");
                                        Session["IsOpenedDateOld"] = "Y";
                                    }
                                    else
                                    {
                                        //Console.WriteLine("The difference is not greater than six months.");
                                        Session["IsOpenedDateOld"] = "N";
                                    }
                                }
                                else
                                {
                                    Session["IsOpenedDateOld"] = "";
                                }

                                if (jObject["productCode"] != null)
                                {
                                    Session["productCode"] = jObject["productCode"].ToString().Trim();
                                }
                                else
                                {
                                    Session["productCode"] = "";
                                }

                                if (jObject["productType"] != null)
                                {
                                    Session["productType"] = jObject["productType"].ToString().Trim();
                                }
                                else
                                {
                                    Session["productType"] = "";
                                }

                                if (jObject["accountBalances"] != null)
                                {
                                    Session["accountBalances"] = jObject["accountBalances"]["effectiveAvailableAmount"] == null ? "0" :
                                                                jObject["accountBalances"]["effectiveAvailableAmount"].ToString().Trim();
                                }
                                else
                                {
                                    Session["accountBalances"] = "0";
                                }
                                //=========== Amol changes on 27/02/2024 for capturing SourceCustomerId end ======

                                if (jObject["accountName"] != null)
                                {
                                    if (((Newtonsoft.Json.Linq.JContainer)jObject["relatedCustomerInfo"]).Count > 0)
                                    {
                                        int iIndex = 0;

                                        while (iIndex < jObject["relatedCustomerInfo"].Count())
                                        {
                                            //Call for account holders
                                            //====== 7 uncomment when deployed on bank start ===================
                                            //string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, sEtoken, jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                            //string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, "", jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                            //====== 7 uncomment when deployed on bank start ===================
                                            //Get account holders
                                            string sCustomerName = GetCMCPCustomerNameTest();
                                            //string sCustomerName = getCustomerName(sCMPCResponse);

                                            //var jObject1 = Newtonsoft.Json.Linq.JObject.Parse(sCustomerName);
                                            //string sname = jObject1["data"]["profileInfo"]["registeredName"].ToString().Trim();

                                            if (sCustomerName != "")
                                            {
                                                lAccNames.Add(sCustomerName);
                                            }

                                            //lAccNames.Add(jObject["accountName"].ToString().Trim());
                                            //logerror("Joint Account Name : ", sCustomerName.ToString());
                                            iIndex++;
                                        }

                                        ViewBag.vbstatus = "SUCCESS";
                                        ViewBag.vbcbsdls = null;

                                        if (lAccNames.Count > 0)
                                            ViewBag.vbAcctName = lAccNames;
                                    }
                                    else
                                    {
                                        lAccNames.Add(jObject["accountName"].ToString().Trim());
                                        ViewBag.vbstatus = "SUCCESS";
                                        ViewBag.vbcbsdls = null;

                                        if (lAccNames.Count > 0)
                                            ViewBag.vbAcctName = lAccNames;
                                    }

                                    if (lAccNames.Count == 0)
                                    {
                                        lAccNames.Add(jObject["accountName"].ToString().Trim());
                                        ViewBag.vbstatus = "SUCCESS";
                                        ViewBag.vbcbsdls = null;

                                        if (lAccNames.Count > 0)
                                            ViewBag.vbAcctName = lAccNames;
                                    }

                                    //======================================================================= comment by amol for CMCP account name on 07/06/2023 start ==================
                                    //lAccNames.Add(jObject["accountName"].ToString().Trim());
                                    //ViewBag.vbstatus = "SUCCESS";
                                    //ViewBag.vbcbsdls = null;

                                    //if (lAccNames.Count > 0)
                                    //    ViewBag.vbAcctName = lAccNames;
                                    //======================================================================= comment by amol for CMCP account name on 07/06/2023 start ==================
                                }

                                ViewBag.sCAPA = "Account is closed";
                                ViewBag.vberror = "Account is closed";
                                ViewBag.vbstatus = "SUCCESS";
                                ViewBag.vbcbsdls = null;
                                ViewBag.block = 1;

                            }
                        }
                    }
                }//ac

                return PartialView("_AccountNames");
            }
            catch (Exception e)
            {
                //LogError();
                //if (e.Message.ToString().Trim().IndexOf("error", 1) > 0)
                //{
                //    ViewBag.vberror = "Invalid Account";
                //    ViewBag.vbstatus = "SUCCESS";
                //    ViewBag.vbcbsdls = null;

                //    return PartialView("_AccountNames");
                //}
                //else
                //{
                ErrorDisplay er = new ErrorDisplay();
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;
                er.ErrorMessage = message;
                //==================================================================================
                //error log
                logerrorInCatch(e.Message.ToString(), message);
                //====================================================================================

                return PartialView("Error", er);
                //return PartialView("_AccountNames");
                //}
            }

        }

        public string GetToken()
        {


            ServicePointManager.Expect100Continue = true;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
               | SecurityProtocolType.Tls11
               | SecurityProtocolType.Tls12
               | SecurityProtocolType.Ssl3;

            //logic for fetching joint holders names from CMCP API

            //Token variables
            var TokenClientId = ConfigurationManager.AppSettings["TokenClientId"].ToString();
            var TokenSecreteKey = ConfigurationManager.AppSettings["TokenSecreteKey"].ToString();
            var TokenServiceURL = ConfigurationManager.AppSettings["TokenServiceURL"].ToString();

            //logerror("TokenClientId : ", TokenClientId);
            //logerror("TokenSecreteKey : ", TokenSecreteKey);
            //logerror("TokenServiceURL : ", TokenServiceURL);

            //checking for date and 8 hours to get new token
            SqlDataAdapter adp = new SqlDataAdapter("GetToken", con);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataTable dt = new DataTable();
            adp.Fill(dt);

            //Call for token
            string sTokenResponse = "", sEtoken = "";

            //Get token
            if (dt.Rows.Count > 0)
            {
                //sEtoken = dt.Rows[0]["SettingValue"].ToString().Trim();
                sEtoken = Session["sToken"].ToString().Trim();
            }
            else
            {
                sTokenResponse = sendCMCPTokenRequest(TokenServiceURL, TokenClientId, TokenSecreteKey);
                //logerror("sTokenResponse : ", sTokenResponse);

                sEtoken = getCMCPToken(sTokenResponse);

                //Save new Token
                if (con.State == ConnectionState.Closed)
                    con.Open();

                SqlCommand cmd = new SqlCommand("UpdateToken", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@SettingValue", sEtoken);
                int iExec = cmd.ExecuteNonQuery();
            }


            //logerror("sEtoken : ", sEtoken);

            return sEtoken;



        }
        public string CreateToken()
        {


            ServicePointManager.Expect100Continue = true;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
               | SecurityProtocolType.Tls11
               | SecurityProtocolType.Tls12
               | SecurityProtocolType.Ssl3;

            //logic for fetching joint holders names from CMCP API

            //Token variables
            var TokenClientId = ConfigurationManager.AppSettings["TokenClientId"].ToString();
            var TokenSecreteKey = ConfigurationManager.AppSettings["TokenSecreteKey"].ToString();
            var TokenServiceURL = ConfigurationManager.AppSettings["TokenServiceURL"].ToString();

            //logerror("TokenClientId : ", TokenClientId);
            //logerror("TokenSecreteKey : ", TokenSecreteKey);
            //logerror("TokenServiceURL : ", TokenServiceURL);

            //checking for date and 8 hours to get new token
            SqlDataAdapter adp = new SqlDataAdapter("GetToken", con);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataTable dt = new DataTable();
            adp.Fill(dt);

            //Call for token
            string sTokenResponse = "", sEtoken = "";

            //Get token
            if (dt.Rows.Count > 0)
            {
                sEtoken = dt.Rows[0]["SettingValue"].ToString().Trim();
            }
            else
            {
                sTokenResponse = sendCMCPTokenRequest(TokenServiceURL, TokenClientId, TokenSecreteKey);
                //logerror("sTokenResponse : ", sTokenResponse);

                sEtoken = getCMCPToken(sTokenResponse);

                //Save new Token
                if (con.State == ConnectionState.Closed)
                    con.Open();

                SqlCommand cmd = new SqlCommand("UpdateToken", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@SettingValue", sEtoken);
                int iExec = cmd.ExecuteNonQuery();
            }


            //logerror("sEtoken : ", sEtoken);

            return sEtoken;



        }
        public ActionResult GetSMBCBSDetails(string ac = null)
        {
            cbstetails model = new cbstetails();


            try
            {
                int acstatusId = 0;
                int freezeid = 0;
                //if (Session["GetAccountDetails "].ToString().ToUpper() == "L")
                //{



                if (ac != null)
                {
                    //Verification Ac no. API Calling
                    string NewApiCall = null;
                    var OwApi = af.CommonSettings.FirstOrDefault(a => a.AppName == "CTSCONFIG1" && a.SettingName == "OWVerAPI")?.SettingValue;
                    if (OwApi != null && OwApi != "")
                        NewApiCall = OwApi.ToString().ToUpper();
                    else
                        NewApiCall = "N";

                    if (NewApiCall == "Y")
                    {
                        try
                        {
                            string sAcctName = "";

                            var sServiceUrl = ConfigurationManager.AppSettings["sServiceUrl"].ToString();
                            var sClientId = ConfigurationManager.AppSettings["sClientId"].ToString();
                            var sClientSecretKey = ConfigurationManager.AppSettings["sClientSecretKey"].ToString();
                            var sSenderCode = ConfigurationManager.AppSettings["sSenderCode"].ToString();
                            var sSenderName = ConfigurationManager.AppSettings["sSenderName"].ToString();

                            //string sgetAccountDetailsSIB = getAccountDetailsSIB(sServiceUrl, sClientId, sClientSecretKey, sSenderCode, sSenderName, ac);
                            //string sgetAccountDetailsSIB = ValidAcNo();
                            //string sgetAccountDetailsSIB = InValidAcNo();

                            string sgetAccountDetailsSIB = JointAccount();


                            var jObject = Newtonsoft.Json.Linq.JObject.Parse(sgetAccountDetailsSIB);

                            if (jObject["Response"]["Body"]["RelPartyRec"].Count() > 0 && (ac == "123451234512345" || ac == "999999999999999"))
                            {
                                int iIndex = 0;
                                //while (iIndex < jObject["Response"]["Body"]["RelPartyRec"].Count())
                                //{
                                //    lAccNames.Add(jObject["Response"]["Body"]["RelPartyRec"][iIndex]["CustId"]["PersonName"]["Name"].ToString().Trim());
                                //    iIndex++;
                                //}
                                model.status = "SUCCESS";


                                //model.PayeeName = lAccNames;

                                model.AccountStatus = "Live";
                                model.MOP = "MOP";
                                model.payeenameselected = sAcctName;


                                return PartialView("_AccountNames");
                                //return Json(new { data = sAcctName },JsonRequestBehavior.AllowGet);
                                //return Json(new { model.AccountStatus, model.MOP, model.payeenameselected }, JsonRequestBehavior.AllowGet);
                                //return Json(null);
                            }
                            else
                            {
                                return PartialView("_AccountNames", model);
                            }
                        }
                        catch (Exception e)
                        {
                            ErrorDisplay er = new ErrorDisplay();
                            string message = "";
                            string innerExcp = "";
                            if (e.Message != null)
                                message = e.Message.ToString();
                            if (e.InnerException != null)
                                innerExcp = e.InnerException.Message;
                            er.ErrorMessage = message;

                            return PartialView("Error", er);
                        }
                    }//NewApiCall

                    //Verification Ac no. API Calling



                }
                else
                    return PartialView("_AccountNames", model);

            }
            catch (Exception e)
            {

                ErrorDisplay er = new ErrorDisplay();
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;
                er.ErrorMessage = message;

                return PartialView("Error", er);
            }//, model.AccountStatus, model.Allow, model.MOP, model.FreezAllow
            return Json(JsonRequestBehavior.AllowGet);
            //return PartialView("_GetSMBCBSDetails");
        }
        public PartialViewResult GetCBSDtls(string ac = null, string strcbsdls = null, string strJoinHldrs = null, string callby = null, string payeename = null)
        {
            cbstetails model = new cbstetails();
            try
            {
                if (strcbsdls != null)
                {

                    model.cbsdls = strcbsdls;
                    model.JoinHldrs = strJoinHldrs;
                    if (model.cbsdls != null)
                    {
                        //-------------------------For Creditcard-----------------------
                        //-------------------------For Creditcard-----------------------
                        if (ac.Length == 16 && ac != "9999999999999999")
                        {
                            if (Session["CreditCardValidationReq"].ToString() == "1")
                            {

                                if (model.cbsdls.Split('|').ElementAt(1) == "S")
                                {
                                    if (model.cbsdls.Length < 5)
                                    {
                                        model.cbsdls = "|S|CREDIT CARD-BRANCH COLLECTION A/C|CRC1| || |0|N|N|N|OAB|O";
                                        model.JoinHldrs = "|S|CREDIT CARD-BRANCH COLLECTION A/C|CRC1|VALID CARD || |0|N|N|N|OAB|O";
                                    }
                                }
                                else
                                {
                                    if (model.cbsdls.Length < 5)
                                    {
                                        model.cbsdls = "|S|CREDIT CARD-BRANCH COLLECTION A/C|CRC1| || |0|N|N|N|OAB|O";
                                        model.JoinHldrs = "|F|CREDIT CARD-BRANCH COLLECTION A/C|CRC1|INVALID CARD || |0|N|N|N|OAB|O";
                                    }
                                }

                            }
                        }
                        else if (ac.Length == 16 && ac == "9999999999999999")
                        {
                            model.cbsdls = "|F|Account does not exist";
                            model.JoinHldrs = "|F|Account does not exist";
                        }
                        //-------------------------For Creditcard-------END----------------
                        if (model.cbsdls.Split('|').ElementAt(1) == "S")
                        {
                            if (model.cbsdls.Split('|').ElementAt(5).Trim() != "")
                            {
                                string MOP = af.MOPCodeMasters.Find(model.cbsdls.Split('|').ElementAt(5)).Description;
                                model.MOP = MOP != null ? MOP : "";
                            }
                            else
                            {
                                model.MOP = "";
                            }
                            if (model.cbsdls.Split('|').ElementAt(6).Trim() != "")
                            {
                                string AccountStatus = af.AccStatusCodeMasters.Find(model.cbsdls.Split('|').ElementAt(6)).Description;
                                model.AccountStatus = AccountStatus != null ? AccountStatus : "";
                            }
                            else
                            {
                                model.AccountStatus = "";
                            }

                            if (model.cbsdls.Split('|').ElementAt(12).Trim() != "")
                            {
                                string AccountOwnership = af.AccOwnershipCodeMasters.Find(model.cbsdls.Split('|').ElementAt(12).ToString()).Description;
                                model.AccountOwnership = AccountOwnership != null ? AccountOwnership : "";
                            }
                            else
                            {
                                model.AccountOwnership = "";
                            }
                            List<string> ar = new List<string>();
                            ar.Add(model.cbsdls.Split('|').ElementAt(2).ToString());

                            if (model.JoinHldrs != null)//model.JoinHldrs.Split('|').Count() - 1 remove on 05/05/2017
                            {
                                for (int i = 3; i < model.JoinHldrs.Split('|').Count(); i++)
                                {
                                    if (model.JoinHldrs.Split('|').ElementAt(i).ToString() != "")
                                        ar.Add(model.JoinHldrs.Split('|').ElementAt(i).ToString());

                                }
                            }
                            model.PayeeName = ar;
                        }
                        else
                        {
                            cbstetails Tempcbdtls = new cbstetails();
                            Tempcbdtls.cbsdls = "|F|Account does not exist";
                            model = Tempcbdtls;
                        }
                    }
                    // model.callby = callby;
                }
                else if (ac != null)
                {
                    if (Session["GetAccountDetails "].ToString().ToUpper() == "L")
                    {
                        model = (from c in af.ACDetails
                                 where c.Ac == ac
                                 select new cbstetails
                                 {
                                     cbsdls = c.Cbsdtls,
                                     JoinHldrs = c.JoinHldrs
                                 }
                            ).SingleOrDefault();

                    }
                    else if (Session["GetAccountDetails "].ToString().ToUpper() == "C")
                    {
                        //---------For CBS Bank----------------

                        //OWpro.OWGetCBSAccInfoWithOutUpdate(ac, ref strcbsdls, ref strJoinHldrs);
                        model.cbsdls = strcbsdls;
                        model.JoinHldrs = strJoinHldrs;
                        //---------------------------------
                    }



                    if (model != null && model.cbsdls != null)
                    {
                        //-------------------------For Creditcard-----------------------
                        //-------------------------For Creditcard-----------------------
                        if (ac.Length == 16 && ac != "9999999999999999")
                        {
                            if (Session["CreditCardValidationReq"].ToString() == "1")
                            {
                                if (model.cbsdls.Split('|').ElementAt(1) == "S")
                                {
                                    if (model.cbsdls.Length < 5)
                                    {
                                        model.cbsdls = "|S|CREDIT CARD-BRANCH COLLECTION A/C|CRC1| || |0|N|N|N|OAB|O";
                                        model.JoinHldrs = "|S|CREDIT CARD-BRANCH COLLECTION A/C|CRC1|VALID CARD || |0|N|N|N|OAB|O";
                                    }
                                }
                                else
                                {
                                    if (model.cbsdls.Length < 5)
                                    {
                                        model.cbsdls = "|S|CREDIT CARD-BRANCH COLLECTION A/C|CRC1| || |0|N|N|N|OAB|O";
                                        model.JoinHldrs = "|F|CREDIT CARD-BRANCH COLLECTION A/C|CRC1|INVALID CARD || |0|N|N|N|OAB|O";
                                    }
                                }

                            }
                        }
                        else if (ac.Length == 16 && ac == "9999999999999999")
                        {
                            model.cbsdls = "|F|Account does not exist";
                            model.JoinHldrs = "|F|Account does not exist";
                        }
                        //-------------------------For Creditcard-------END----------------

                        if (model.cbsdls.Split('|').ElementAt(1) == "S")
                        {
                            if (model.cbsdls.Split('|').ElementAt(5).Trim() != "")
                            {
                                string MOP = af.MOPCodeMasters.Find(model.cbsdls.Split('|').ElementAt(5)).Description;
                                model.MOP = MOP != null ? MOP : "";
                            }
                            else
                            {
                                model.MOP = "";
                            }
                            if (model.cbsdls.Split('|').ElementAt(6).Trim() != "")
                            {
                                string AccountStatus = af.AccStatusCodeMasters.Find(model.cbsdls.Split('|').ElementAt(6)).Description;
                                model.AccountStatus = AccountStatus != null ? AccountStatus : "";
                            }
                            else
                            {
                                model.AccountStatus = "";
                            }

                            if (model.cbsdls.Split('|').ElementAt(12).Trim() != "")
                            {
                                string AccountOwnership = af.AccOwnershipCodeMasters.Find(model.cbsdls.Split('|').ElementAt(12).ToString()).Description;
                                model.AccountOwnership = AccountOwnership != null ? AccountOwnership : "";
                            }
                            else
                            {
                                model.AccountOwnership = "";
                            }

                            List<string> ar = new List<string>();
                            ar.Add(model.cbsdls.Split('|').ElementAt(2).ToString());

                            if (model.JoinHldrs != null)//model.JoinHldrs.Split('|').Count() - 1 remove on 05/05/2017
                            {
                                for (int i = 3; i < model.JoinHldrs.Split('|').Count(); i++)
                                {
                                    if (model.JoinHldrs.Split('|').ElementAt(i).ToString() != "")
                                        ar.Add(model.JoinHldrs.Split('|').ElementAt(i).ToString());

                                }
                            }
                            model.PayeeName = ar;
                        }
                    }
                    else
                    {
                        cbstetails Tempcbdtls = new cbstetails();
                        Tempcbdtls.cbsdls = "|F|Account does not exist";
                        model = Tempcbdtls;
                    }

                }

                model.callby = callby;
                model.payeenameselected = payeename;
                return PartialView("_GetCBSDtls", model);
            }
            catch (Exception e)
            {
                //Server.MapPath(strMappath);
                ErrorDisplay er = new ErrorDisplay();
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;
                er.ErrorMessage = message;
                // return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL2 HttpPost Index- " + innerExcp });
                return PartialView("Error", er);
            }
        }
        public PartialViewResult RejectReason(int id = 0)
        {

            var rjrs = (from r in af.ItemReturnReasons
                        select new RejectReason
                        {
                            Description = r.DESCRIPTION,
                            ReasonCodeS = r.RETURN_REASON_CODE
                        });
            return PartialView("_RejectReason", rjrs);

        }
        public PartialViewResult GetBankName(string bankcode = null)
        {
            //if (bankcode != null && bankcode != "")
            //{
            //string tempbankcode = bankcode.Substring(3, 3);
            //    int bnkCust = Convert.ToInt16(Session["CustomerID"]);
            //    var Banks = (from c in af.BankBranches
            //                 from ct in af.CustomerMasters
            //                 where c.GridID == ct.GridId && ct.Id == bnkCust && c.Bank_BankCode == bankcode
            //                 select new { c.BankName }).SingleOrDefault();
            //    if (Banks != null)
            //        ViewBag.BankName = Banks.BankName;
            //    else
            //        ViewBag.BankName = null;
            //}
            //else
            //    ViewBag.BankName = null;

            if (bankcode != null && bankcode != "")
            {
                string tempbankcode = bankcode.Substring(3, 3);
                var Banks = (from c in af.Banks
                             where c.BankCode.Substring(3, 3) == tempbankcode
                             select new { c.BankName }).SingleOrDefault();
                if (Banks != null)
                    ViewBag.BankName = Banks.BankName;
                else
                    ViewBag.BankName = null;
            }
            else
                ViewBag.BankName = null;


            return PartialView("_Bankname");
        }
        public PartialViewResult GetClientDlts(string ac = null)
        {
            var customer = (from c in af.CMS_CustomerMaster
                            where c.Customer_Code == ac
                            select new { c.Customer_Name }).SingleOrDefault();
            if (customer != null)
                ViewBag.customer = customer.Customer_Name;
            else
                ViewBag.customer = null;

            return PartialView("GetClientDlts");
        }
        public PartialViewResult getOWlogs(int id)
        {
            try
            {
                var model = af.ActivityLogs.Where(l => l.RawDataId == id).OrderBy(l => l.TimeStamp).ToList();
                return PartialView("_OWActivitylogs", model);
            }
            catch (Exception e)
            {

                ErrorDisplay er = new ErrorDisplay();
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;
                er.ErrorMessage = message;
                // return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL2 HttpPost Index- " + innerExcp });
                return PartialView("Error", er);
            }

        }
        //-------------------------Added On 24/07/2017-----------------------For Slip Image-----------
        public JsonResult slipImage(Int64 SlipId = 0)
        {
            var owL2 = af.L2Verification.Where(m => m.Id == SlipId).FirstOrDefault().FrontGreyImagePath;


            return Json(owL2, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OWL2Chq(int id = 0)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            if ((bool)Session["VF"] == false)
            {

                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            try
            {
                string VFType = "";
                if (id == 1)
                    VFType = "RNormal";
                else if (id == 2)
                    VFType = "RHold";
                else if (id == 3)
                    VFType = "BNormal";
                else if (id == 4)
                    VFType = "BHold";

                Session["VFType"] = VFType;

                SqlDataAdapter adp = new SqlDataAdapter("OWSelectCHQL2", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@VFtype", SqlDbType.NVarChar).Value = VFType;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();
                //-------------Added on 17-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                //-------------Added on 18-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);

                //-------------Added on 12-09-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = Session["CtsSessionType"];


                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<L2verificationModel>();
                L2verificationModel def;

                if (ds.Tables[0].Rows.Count > 0)
                {

                    def = new L2verificationModel
                    {
                        Id = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]),
                        BatchNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[1]),
                        BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[2]),
                        InstrumentType = ds.Tables[0].Rows[0].ItemArray[3].ToString(),
                        ClearingType = ds.Tables[0].Rows[0].ItemArray[4].ToString(),
                        //SlipNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[5].ToString()),
                        //SlipChequeCount = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[6].ToString()),
                        //SlipAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[7]),
                        //ChequeAmountTotal = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[8]),
                        Status = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[5]),
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[6].ToString(),
                        FrontGreyImagePath = ds.Tables[0].Rows[0].ItemArray[7].ToString(),
                        BackTiffImagePath = ds.Tables[0].Rows[0].ItemArray[8].ToString(),
                        ClientCode = ds.Tables[0].Rows[0].ItemArray[9].ToString(),
                        SlipRefNo = ds.Tables[0].Rows[0].ItemArray[10].ToString(),
                        CreditAccountNo = ds.Tables[0].Rows[0].ItemArray[11].ToString(),
                        BranchCode = ds.Tables[0].Rows[0].ItemArray[12].ToString(),
                        ProcessingDate = Convert.ToDateTime(ds.Tables[0].Rows[0].ItemArray[13].ToString()),
                        ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[14].ToString()),
                        RawDataId = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[15].ToString()),
                        DomainId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[16].ToString()),
                        CustomerId = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[18].ToString()),
                        ScanningType = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[19].ToString()),
                        L1VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[20].ToString()),
                        L1UserId = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[21].ToString()),
                        L1RejectReason = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[22].ToString()),
                        PayeeName = ds.Tables[0].Rows[0].ItemArray[23].ToString(),
                        CBSAccountInformation = ds.Tables[0].Rows[0].ItemArray[24].ToString(),
                        CBSJointAccountInformation = ds.Tables[0].Rows[0].ItemArray[25].ToString(),
                        UserNarration = ds.Tables[0].Rows[0].ItemArray[26].ToString(),
                        SlipUserNarration = ds.Tables[0].Rows[0].ItemArray[26].ToString(),
                        RejectReasonDescription = ds.Tables[0].Rows[0].ItemArray[27].ToString(),
                        FinalAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[28]),
                        FinalDate = ds.Tables[0].Rows[0].ItemArray[29].ToString(),
                        ChequeNoFinal = ds.Tables[0].Rows[0].ItemArray[30].ToString(),
                        SortCodeFinal = ds.Tables[0].Rows[0].ItemArray[31].ToString(),
                        SANFinal = ds.Tables[0].Rows[0].ItemArray[32].ToString(),
                        TransCodeFinal = ds.Tables[0].Rows[0].ItemArray[33].ToString(),
                        DocType = ds.Tables[0].Rows[0].ItemArray[34].ToString(),
                        Modified1 = ds.Tables[0].Rows[0].ItemArray[35].ToString(),
                        callby = "Cheq",
                    };
                    objectlst.Add(def);
                    //------------------------END------------------------//
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        def = new L2verificationModel
                        {
                            Id = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),
                            BatchNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[1]),
                            BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[2]),
                            InstrumentType = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                            ClearingType = ds.Tables[0].Rows[index].ItemArray[4].ToString(),
                            //SlipNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[5].ToString()),
                            //SlipChequeCount = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[6].ToString()),
                            //SlipAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[7]),
                            //ChequeAmountTotal = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[8]),
                            Status = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[5]),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[6].ToString(),
                            FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[7].ToString(),
                            BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[8].ToString(),
                            ClientCode = ds.Tables[0].Rows[index].ItemArray[9].ToString(),
                            SlipRefNo = ds.Tables[0].Rows[index].ItemArray[10].ToString(),
                            CreditAccountNo = ds.Tables[0].Rows[index].ItemArray[11].ToString(),
                            BranchCode = ds.Tables[0].Rows[index].ItemArray[12].ToString(),
                            ProcessingDate = Convert.ToDateTime(ds.Tables[0].Rows[index].ItemArray[13].ToString()),
                            ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[14].ToString()),
                            RawDataId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[15].ToString()),
                            DomainId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[16].ToString()),
                            CustomerId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[18].ToString()),
                            ScanningType = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[19].ToString()),
                            L1VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[20].ToString()),
                            L1UserId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[21].ToString()),
                            L1RejectReason = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[22].ToString()),
                            PayeeName = ds.Tables[0].Rows[index].ItemArray[23].ToString(),
                            CBSAccountInformation = ds.Tables[0].Rows[index].ItemArray[24].ToString(),
                            CBSJointAccountInformation = ds.Tables[0].Rows[index].ItemArray[25].ToString(),
                            UserNarration = ds.Tables[0].Rows[index].ItemArray[26].ToString(),
                            SlipUserNarration = ds.Tables[0].Rows[index].ItemArray[26].ToString(),
                            RejectReasonDescription = ds.Tables[0].Rows[index].ItemArray[27].ToString(),
                            FinalAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[28]),
                            FinalDate = ds.Tables[0].Rows[index].ItemArray[29].ToString(),
                            ChequeNoFinal = ds.Tables[0].Rows[index].ItemArray[30].ToString(),
                            SortCodeFinal = ds.Tables[0].Rows[index].ItemArray[31].ToString(),
                            SANFinal = ds.Tables[0].Rows[index].ItemArray[32].ToString(),
                            TransCodeFinal = ds.Tables[0].Rows[index].ItemArray[33].ToString(),
                            DocType = ds.Tables[0].Rows[index].ItemArray[34].ToString(),
                            Modified1 = ds.Tables[0].Rows[index].ItemArray[35].ToString(),
                            callby = "Cheq",
                        };
                        //ViewBag.cnt = true;
                        objectlst.Add(def);
                        count = count - 1;
                        index = index + 1;
                    }
                    //var rtnlist = iwafl.ItemReturnReasons.Select(m => m.RETURN_REASON_CODE).ToList();
                    //ViewBag.rtnlist = rtnlist;

                    var rtnlist = (from i in af.ItemReturnReasons select i).ToList();// iwafl.ItemReturnReasons.Select(m).ToList();
                    ViewBag.rtnlist = rtnlist.Select(m => m.RETURN_REASON_CODE).ToList();
                    ViewBag.rtnlistDescrp = rtnlist.Select(m => m.DESCRIPTION).ToList();
                    //-------------------------------For Narration Accounts---------------------
                    ViewBag.narration = (from n in af.NarrationAccount where n.status == 1 select n.AccountNumber).ToList();
                    //-------------------------------For SchemCode---------------------
                    ViewBag.SchemCode = (from n in af.NreNroAccountList where n.IsActive == true select n.SchemCode).ToList();

                    @Session["glob"] = null;
                    ViewBag.cnt = true;
                    return View(objectlst);
                }
                else
                    return RedirectToAction("IWIndex", "Home", new { id = 1 });
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL2 HttpGet Index- " + innerExcp });
            }

        }

        public ActionResult OWChqL1(string ChqType = null)
        {
            Session["ChqType"] = ChqType;

            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];

            int custid = Convert.ToInt16(Session["CustomerID"]);
            var varMinAclen = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "ACFrom").SettingValue;
            var varMaxAclen = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "ACTo").SettingValue;
            var varMaxPayeelen = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "PayeeNameMaxLength").SettingValue;

            int intMinAclen = Convert.ToInt32(varMinAclen);
            int intMaxAclen = Convert.ToInt32(varMaxAclen);
            int intMaxPayeelen = Convert.ToInt32(varMaxPayeelen);

            ViewBag.MinAclen = intMinAclen;
            ViewBag.MaxAclen = intMaxAclen;
            ViewBag.MaxPayeelen = intMaxPayeelen;

            try
            {
                SqlDataAdapter adp = new SqlDataAdapter("OWSelectCHQL1", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();
                //-------------Added on 17-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                //-------------Added on 18-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);
                //-------------Added on 12-09-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = Session["CtsSessionType"];
                //-------------Added on 20-05-2019-----------------------------
                adp.SelectCommand.Parameters.Add("@ChqType", SqlDbType.NVarChar).Value = ChqType;

                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<L1verificationModel>();
                L1verificationModel def;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    def = new L1verificationModel
                    {
                        Id = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]),
                        BatchNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[1]),
                        BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[2]),
                        InstrumentType = ds.Tables[0].Rows[0].ItemArray[3].ToString(),
                        ClearingType = ds.Tables[0].Rows[0].ItemArray[4].ToString(),
                        Status = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[5]),
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[6].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]).Replace((string)Session["SrcWebIP1"], (string)Session["DestWepIP1"]).Replace((string)Session["SrcWebName1"], (string)Session["DestWebName1"]),
                        FrontGreyImagePath = ds.Tables[0].Rows[0].ItemArray[7].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]).Replace((string)Session["SrcWebIP1"], (string)Session["DestWepIP1"]).Replace((string)Session["SrcWebName1"], (string)Session["DestWebName1"]),
                        BackTiffImagePath = ds.Tables[0].Rows[0].ItemArray[8].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]).Replace((string)Session["SrcWebIP1"], (string)Session["DestWepIP1"]).Replace((string)Session["SrcWebName1"], (string)Session["DestWebName1"]),
                        CreditAccountNo = ds.Tables[0].Rows[0].ItemArray[11].ToString(),
                        BranchCode = ds.Tables[0].Rows[0].ItemArray[12].ToString(),
                        ProcessingDate = Convert.ToDateTime(ds.Tables[0].Rows[0].ItemArray[13].ToString()),
                        ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[14].ToString()),
                        RawDataId = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[15].ToString()),
                        DomainId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[16].ToString()),
                        CustomerId = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[17].ToString()),
                        ScanningType = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[18].ToString()),
                        FinalAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[19].ToString()),
                        FinalDate = ds.Tables[0].Rows[0].ItemArray[20].ToString(),
                        ChequeNoFinal = ds.Tables[0].Rows[0].ItemArray[21].ToString(),
                        SortCodeFinal = ds.Tables[0].Rows[0].ItemArray[22].ToString(),
                        SANFinal = ds.Tables[0].Rows[0].ItemArray[23].ToString(),
                        TransCodeFinal = ds.Tables[0].Rows[0].ItemArray[24].ToString(),


                        callby = "Chq",
                    };


                    objectlst.Add(def);
                    //------------------------------------END---------------------------------------//
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        def = new L1verificationModel
                        {
                            Id = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),
                            BatchNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[1]),
                            BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[2]),
                            InstrumentType = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                            ClearingType = ds.Tables[0].Rows[index].ItemArray[4].ToString(),
                            Status = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[5]),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[6].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]).Replace((string)Session["SrcWebIP1"], (string)Session["DestWepIP1"]).Replace((string)Session["SrcWebName1"], (string)Session["DestWebName1"]),
                            FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[7].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]).Replace((string)Session["SrcWebIP1"], (string)Session["DestWepIP1"]).Replace((string)Session["SrcWebName1"], (string)Session["DestWebName1"]),
                            BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[8].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]).Replace((string)Session["SrcWebIP1"], (string)Session["DestWepIP1"]).Replace((string)Session["SrcWebName1"], (string)Session["DestWebName1"]),
                            CreditAccountNo = ds.Tables[0].Rows[index].ItemArray[11].ToString(),
                            BranchCode = ds.Tables[0].Rows[index].ItemArray[12].ToString(),
                            ProcessingDate = Convert.ToDateTime(ds.Tables[0].Rows[index].ItemArray[13].ToString()),
                            ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[14].ToString()),
                            RawDataId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[15].ToString()),
                            DomainId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[16].ToString()),
                            CustomerId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[17].ToString()),
                            ScanningType = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[18].ToString()),
                            FinalAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[19].ToString()),
                            FinalDate = ds.Tables[0].Rows[index].ItemArray[20].ToString(),
                            ChequeNoFinal = ds.Tables[0].Rows[index].ItemArray[21].ToString(),
                            SortCodeFinal = ds.Tables[0].Rows[index].ItemArray[22].ToString(),
                            SANFinal = ds.Tables[0].Rows[index].ItemArray[23].ToString(),
                            TransCodeFinal = ds.Tables[0].Rows[index].ItemArray[24].ToString(),

                            callby = "Chq",
                        };

                        objectlst.Add(def);
                        count = count - 1;
                        index = index + 1;
                    }

                    var rtnlist = (from i in af.ItemReturnReasons select i).ToList();// iwafl.ItemReturnReasons.Select(m).ToList();
                    ViewBag.rtnlist = rtnlist.Select(m => m.RETURN_REASON_CODE).ToList();
                    ViewBag.rtnlistDescrp = rtnlist.Select(m => m.DESCRIPTION).ToList();
                    //-------------------------------For Narration Accounts---------------------
                    ViewBag.narration = (from n in af.NarrationAccount where n.status == 1 select n.AccountNumber).ToList();
                    //-------------------------------For SchemCode---------------------
                    ViewBag.SchemCode = (from n in af.NreNroAccountList where n.IsActive == true select n.SchemCode).ToList();

                    Session["glob"] = null;
                    ViewBag.cnt = true;
                    return View(objectlst);
                }
                else
                    return RedirectToAction("IWIndex", "Home", new { id = 1 });
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                logger.Log(LogLevel.Error, "OWChqL1 HttpGet|" + message + "INNEREXP| " + innerExcp, "Login Index-Msg");
                return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });

            }
        }


        private string getAccountDetailsDBSRequest(string sServiceUrl, string sClientId, string sCoreRelationId, string sAccountNo, string sEToken)
        {
            sResposne = "";

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;

                sInputString = "";

                sInputString = " {";
                sInputString += "   \"sourceAccountNumber\": \"" + sAccountNo + "\"";
                sInputString += "}";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(sServiceUrl);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("x-Correlation-Id", sCoreRelationId);
                httpWebRequest.Headers.Add("x-sourcetimestamp", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                httpWebRequest.Headers.Add("x-sourceclientid", sClientId);
                httpWebRequest.Headers.Add("Authorization", "Bearer " + sEToken);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(sInputString);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    sResposne = result;
                }


            }
            catch (Exception Ex)
            {
                var exceptionMessage = "";
                if(Ex.Message == "The remote server returned an error: (422) Unprocessable Entity.")
                {
                    exceptionMessage = "Invalid Account";
                }
                else
                {
                    exceptionMessage = Ex.Message;
                }

                sResposne = "{" +
                             "\"error\":\"Runtime Error While Sending the Request\"," +
                             "\"errorDescription\":\"" + exceptionMessage +
                            "\"}";

                //logerror("sServiceUrl ", sServiceUrl);
                //logerror("sCoreRelationId ", sCoreRelationId);
                //logerror("sClientId ", sClientId);
                //logerror("sEToken ", sEToken);
                //logerror("sResposne ", sResposne);
                //logerror("CASA: ", Ex.Message);
            }


            return sResposne;
        }
        private string getAccountDetailsDBSResponse(string sServiceUrl, string sClientId, string sCoreRelationId, string sAccountNo)
        {
            string sResposne = "";

            if (sAccountNo == "222222222222")
            {
                sResposne =
            "{ " +
                "\"sourceAccountNumber\": \"881028754855\"," +
                "\"ibanAccountNumber\": \"\"," +
                "\"sourceSystemId\": \"\"," +
                "\"sourceCustomerId\": \"22945768\"," +
                "\"productType\": \"SBA\"," +
                "\"productTypeDescription\": []," +
                "\"subProductType\": \"\"," +
                "\"productCode\": \"DBSBA\"," +
                "\"productCodeDescription\": [ { \"productCodeDescription\": \"DIGISAVINGS\"," +
                    "                              \"languageCode\": \"INFENG\"         } ]," +
                //"\"productName\": \"NA\"," +
                "\"productName\": \"SARSA\"," +
                "\"nativeProductName\": \"DBSBA\"," +
                "\"accountCurrency\": \"INR\"," +
                "\"openedDate\": 1546041600000," +
                "\"modeOfOperation\": \"0003\"," +
                "\"serviceChargeExemption\": 0," +
                "\"staffIndicator\": false," +
                "\"officerId\": \"\"," +
                "\"officerUnit\": \"\"," +
                "\"firstExcessDate\": 0," +
                "\"lastUpdatedEvent\": \"\"," +
                "\"returnChequeDetailsInfo\": []," +
                "\"accountStatus\": \"Inactive\"," +
                "\"accountStatusCode\": \"I\"," +
                "\"accountSignatoryType\": \"01\"," +
                "\"accountSignal\": \"\"," +
                "\"loanServicingIndicator\": false," +
                "\"accountFrozenIndicator\": true," +
                "\"noDebitIndicator\": false," +
                "\"debitReferralIndicator\": false," +
                "\"irregularSignalIndicator\": false," +
                "\"lineOfferedIndicator\": false," +
                "\"closureNoticeIndicator\": false," +
                "\"multipleAccountIndicator\": false," +
                "\"recallPassbookIndicator\": false," +
                "\"updateRequiredIndicator\": false," +
                "\"productIndicator\": \"\"," +
                "\"brandIndicator\": \"\"," +
                "\"ibanIndicator\": \"N\"," +
                "\"virtualAccountIndicator\": \"N\"," +
                "\"spclCustomerType\": \"\"," +
                "\"accountType\": \"\"," +
                "\"accountTypeDescription\": \"\"," +
                "\"currencyDecimal\": 2," +
                "\"generalLedgerSubHeadCode\": \"21201\"," +
                "\"accountCurrencyCode\": \"INR\"," +
                "\"odLimitType\": \"\"," +
                "\"odInterestAmount\": 0," +
                "\"accountName\": \"A XAXX GXXXSX\"," +
                "\"accountShortName\": \"A XAXX GXX\"," +
                "\"virtualAccountName\": \"\"," +
                "\"accountStatement\": { \"statementMode\": \"S\"," +
                                          "  \"statementCalendar\": \"00\"," +
                                          "  \"frequency\": \"M\"," +
                                          "  \"frequencyStartDate\": 31," +
                                          "  \"frequencyDay\": 0," +
                                          "  \"frequencyWeekNumber\": 0," +
                                          "  \"frequencyHolidayStatus\": \"N\"," +
                                          "  \"nextPrintDate\": 1546214400000," +
                                          "  \"despatchMode\": \"N\" " +
                                          "  }" +
                                          " ," +
                "\"balanceDebitCreditIndicator\": \"C\"," +
                "\"freezeCode\": \"C\"," +
                "\"freezeStatusCode\": \"C\"," +
                "\"freezeReasonCode\": \"0009\"," +
                "\"freezeReasonCode1\": \"\"," +
                "\"additionalFreezeReasonCodes\": []," +
                "\"additionalFreezeRemarks\": []," +
                "\"freezeReasonCodeDescriptionList\": []," +
                "\"freezeRemarks\": \"\"," +
                "\"freezeRemarks1\": \"\"," +
                "\"accountInterest\": { \"interestRate\": 3.25," +
                    "                     \"interestCalInterest\": 0," +
                    "                     \"interestFrequencyType\": \"M\"," +
                    "                     \"interestFrequencyStartDate\": 31," +
                    "                     \"interestFrequencyDay\": 0," +
                    "                     \"interestFrequencyWeekNum\": 0," +
                    "                     \"accountInterestFrequencyHolidayStatus\": \"P\"," +
                    "                     \"interestRateCode\": \"DBSB1\"," +
                    "                     \"netInterestRate\": 0," +
                    "                     \"netInterestDebitCreditIndicator\": \"C\"," +
                    "                     \"accruedInterestDebitCreditIndicator\": \"C\"," +
                    "                     \"unpaidInterestFee\": 0," +
                    "                     \"bookedamount\": 0," +
                    "                     \"interestAmount\": 0," +
                    "                     \"preferentialInterest\": 0" +
                    "                   }" +
                    "," +
                "\"taxCategory\": \"A\"," +
                "\"taxFloorLimit\": 0," +
                "\"taxFloorLimitCurrencyCode\": \"INR\"," +
                "\"withholdingTaxPercent\": 0," +
                "\"gstin\": \"\"," +
                "\"gstExemptionFlag\": \"\"," +
                "\"nickName\": \"\"," +
                "\"productShortName\": \"DBSBA\"," +
                "\"preferredLanguageProductShortName\": \"DBSBA\"," +
                "\"sourceMultiCurrencyAccountNumber\": \"\"," +
                "\"multiCurrencyAccountFlag\": false," +
                "\"branchCode\": \"811\"," +
                "\"branchCodeDescription\": \"\"," +
                "\"bankCode\": \"DBSIN\"," +
                //"\"accountClosedFlag\": \"N\"," +
                "\"accountClosedFlag\": \"Y\"," +
                "\"accountClosedReasonCode\": \"\"," +
                "\"accountClosedRemarks\": \"\"," +
                "\"accountClosedDate\": 0," +
                "\"lastBalanceUpdateDateTime\": 0," +
                "\"earmarkUpdateDateTime\": 0," +
                "\"holdBalanceUpdateDateTime\": 0," +
                "\"sanctionLimitUpdateDateTime\": 0," +
                "\"staticDataUpdateDateTime\": 1656516121000," +
                "\"halfDayHoldBalanceExpiryDate\": 0," +
                "\"childAccounts\": []," +
                "\"accountBalances\": 	{" +
                        "				   \"availableBalance\": -374," +
                        "				   \"availableBalanceCurrencyCode\": \"INR\"," +
                        "				   \"accountBalance\": 126," +
                        "				   \"accountBalanceCurrencyCode\": \"INR\"," +
                        "				   \"sanctionLimit\": 0," +
                        "				   \"sanctionLimitCurrencyCode\": \"INR\"," +
                        "				   \"ledgerBalance\": 126," +
                        "				   \"ledgerBalanceCurrencyCode\": \"INR\"," +
                        "				   \"halfDayHoldBalance\": 0," +
                        "				   \"oneDayHoldBalance\": 0," +
                        "				   \"twoDayHoldBalance\": 0," +
                        "				   \"earmarkDebitAmount\": 0," +
                        "				   \"earmarkCreditAmount\": 500," +
                        "				   \"floatAmount\": 0," +
                        "				   \"earmarkAmount\": 500," +
                        "				   \"effectiveAvailableAmount\": -374," +
                        "				   \"drawingPower\": 0," +
                        "				   \"overDueLiableAmount\": 0," +
                        "				   \"openingBalanceAmount\": 0," +
                        "				   \"closingBalanceAmount\": 0," +
                        "				   \"fundsClearingAmount\": 0," +
                        "				   \"cumulativeCreditAmount\": 35170," +
                        "				   \"cumulativeDebitAmount\": 35044," +
                        "				   \"utilizedAmount\": 0," +
                        "				   \"systemReservedAmount\": 0," +
                        "				   \"overdueFutureAmount\": 0," +
                        "				   \"utilizedFutureAmount\": 0," +
                        "				   \"effectiveFutureAvailableAmount\": 0," +
                        "				   \"availableAmountLineOfCredit\": 0," +
                        "				   \"unclearDrawingAmount\": 0," +
                        "				   \"ffdAvailableAmount\": 0," +
                        "				   \"sweepsEffectiveAvailableAmount\": 0," +
                        "				   \"hcAvailableAmount\": 0," +
                        "				   \"futureAmount\": 0," +
                        "				   \"futureCreditAmount\": 0," +
                        "				   \"futureClearBalanceAmount\": 0," +
                        "				   \"futureUnclearBalanceAmount\": 0," +
                        "				   \"daccLimit\": 0," +
                        "				   \"dafaLimit\": 0 " +
                        "					}," +
                        "\"relatedCustomerInfo\": [" +
                        "                            { " +
                        "                            \"relatedPartyCode\": \"\"," +
                        "                            \"relatedPartyCodeDescription\": \"\"," +
                        "                            \"relatedPartyCustomerId\": \"22945768\"," +
                        "                            \"relatedPartySourceCustomerId\": \"\"," +
                        "                            \"relatedPartyType\": \"M\"," +
                        "                            \"relatedPartyTypeDescription\": \"Main Holder\"," +
                        "                            \"relatedPartyDeleteFlag\": \"N\"," +
                        "                            \"relatedPartyAddressType\": \"Mailing\"" +
                        "                            }," +
                        "                            { " +
                        "                            \"relatedPartyCode\": \"\"," +
                        "                            \"relatedPartyCodeDescription\": \"\"," +
                        "                            \"relatedPartyCustomerId\": \"22945769\"," +
                        "                            \"relatedPartySourceCustomerId\": \"\"," +
                        "                            \"relatedPartyType\": \"M\"," +
                        "                            \"relatedPartyTypeDescription\": \"2nd Holder\"," +
                        "                            \"relatedPartyDeleteFlag\": \"N\"," +
                        "                            \"relatedPartyAddressType\": \"Mailing\"" +
                        "                            }" +
                        "                      ]," +
                        "\"promoCode\": []," +
                        "\"mobileMoneyIdentifier\": \"9641689\"," +
                        "\"mobileNumbers\": [ \"7358386665\" ]," +
                        "\"reference1\": \"\"," +
                        "\"reference2\": \"\"," +
                        "\"ifscCode\": \"DBSS0IN0811\"," +
                        "\"channelId\": \"SOI\"," +
                        "\"accountStatusDate\": 1623456000000," +
                        "\"expressAccountExpiryDate\": 0," +
                        "\"schemeConversionDate\": 0," +
                        "\"virtualAccountType\": \"\"," +
                        "\"faxIndeminity\": \"\"," +
                        "\"nomineeAvailableFlag\": \"N\"," +
                        "\"nomineeGuardianInfo\": []" +
                "}   ";

            }
            else if (sAccountNo == "333333333333")
            {
                sResposne =
            "{ " +
                "\"sourceAccountNumber\": \"881028754855\"," +
                "\"ibanAccountNumber\": \"\"," +
                "\"sourceSystemId\": \"\"," +
                "\"sourceCustomerId\": \"22945768\"," +
                "\"productType\": \"SBA\"," +
                "\"productTypeDescription\": []," +
                "\"subProductType\": \"\"," +
                "\"productCode\": \"DBSBA\"," +
                "\"productCodeDescription\": [ { \"productCodeDescription\": \"DIGISAVINGS\"," +
                    "                              \"languageCode\": \"INFENG\"         } ]," +
                //"\"productName\": \"NA\"," +
                "\"productName\": \"NRESA\"," +
                "\"nativeProductName\": \"DBSBA\"," +
                "\"accountCurrency\": \"INR\"," +
                "\"openedDate\": 1546041600000," +
                "\"modeOfOperation\": \"0003\"," +
                "\"serviceChargeExemption\": 0," +
                "\"staffIndicator\": false," +
                "\"officerId\": \"\"," +
                "\"officerUnit\": \"\"," +
                "\"firstExcessDate\": 0," +
                "\"lastUpdatedEvent\": \"\"," +
                "\"returnChequeDetailsInfo\": []," +
                //"\"accountStatus\": \"Inactive\"," +
                "\"accountStatus\": \"Active\"," +
                "\"accountStatusCode\": \"I\"," +
                "\"accountSignatoryType\": \"01\"," +
                "\"accountSignal\": \"\"," +
                "\"loanServicingIndicator\": false," +
                "\"accountFrozenIndicator\": true," +
                "\"noDebitIndicator\": false," +
                "\"debitReferralIndicator\": false," +
                "\"irregularSignalIndicator\": false," +
                "\"lineOfferedIndicator\": false," +
                "\"closureNoticeIndicator\": false," +
                "\"multipleAccountIndicator\": false," +
                "\"recallPassbookIndicator\": false," +
                "\"updateRequiredIndicator\": false," +
                "\"productIndicator\": \"\"," +
                "\"brandIndicator\": \"\"," +
                "\"ibanIndicator\": \"N\"," +
                "\"virtualAccountIndicator\": \"N\"," +
                "\"spclCustomerType\": \"\"," +
                "\"accountType\": \"\"," +
                "\"accountTypeDescription\": \"\"," +
                "\"currencyDecimal\": 2," +
                "\"generalLedgerSubHeadCode\": \"21201\"," +
                "\"accountCurrencyCode\": \"INR\"," +
                "\"odLimitType\": \"\"," +
                "\"odInterestAmount\": 0," +
                "\"accountName\": \"A XAXX GXXXSX\"," +
                "\"accountShortName\": \"A XAXX GXX\"," +
                "\"virtualAccountName\": \"\"," +
                "\"accountStatement\": { \"statementMode\": \"S\"," +
                                          "  \"statementCalendar\": \"00\"," +
                                          "  \"frequency\": \"M\"," +
                                          "  \"frequencyStartDate\": 31," +
                                          "  \"frequencyDay\": 0," +
                                          "  \"frequencyWeekNumber\": 0," +
                                          "  \"frequencyHolidayStatus\": \"N\"," +
                                          "  \"nextPrintDate\": 1546214400000," +
                                          "  \"despatchMode\": \"N\" " +
                                          "  }" +
                                          " ," +
                "\"balanceDebitCreditIndicator\": \"C\"," +
                "\"freezeCode\": \"C\"," +
                "\"freezeStatusCode\": \"C\"," +
                "\"freezeReasonCode\": \"0009\"," +
                "\"freezeReasonCode1\": \"\"," +
                "\"additionalFreezeReasonCodes\": []," +
                "\"additionalFreezeRemarks\": []," +
                "\"freezeReasonCodeDescriptionList\": []," +
                "\"freezeRemarks\": \"\"," +
                "\"freezeRemarks1\": \"\"," +
                "\"accountInterest\": { \"interestRate\": 3.25," +
                    "                     \"interestCalInterest\": 0," +
                    "                     \"interestFrequencyType\": \"M\"," +
                    "                     \"interestFrequencyStartDate\": 31," +
                    "                     \"interestFrequencyDay\": 0," +
                    "                     \"interestFrequencyWeekNum\": 0," +
                    "                     \"accountInterestFrequencyHolidayStatus\": \"P\"," +
                    "                     \"interestRateCode\": \"DBSB1\"," +
                    "                     \"netInterestRate\": 0," +
                    "                     \"netInterestDebitCreditIndicator\": \"C\"," +
                    "                     \"accruedInterestDebitCreditIndicator\": \"C\"," +
                    "                     \"unpaidInterestFee\": 0," +
                    "                     \"bookedamount\": 0," +
                    "                     \"interestAmount\": 0," +
                    "                     \"preferentialInterest\": 0" +
                    "                   }" +
                    "," +
                "\"taxCategory\": \"A\"," +
                "\"taxFloorLimit\": 0," +
                "\"taxFloorLimitCurrencyCode\": \"INR\"," +
                "\"withholdingTaxPercent\": 0," +
                "\"gstin\": \"\"," +
                "\"gstExemptionFlag\": \"\"," +
                "\"nickName\": \"\"," +
                "\"productShortName\": \"DBSBA\"," +
                "\"preferredLanguageProductShortName\": \"DBSBA\"," +
                "\"sourceMultiCurrencyAccountNumber\": \"\"," +
                "\"multiCurrencyAccountFlag\": false," +
                "\"branchCode\": \"811\"," +
                "\"branchCodeDescription\": \"\"," +
                "\"bankCode\": \"DBSIN\"," +
                "\"accountClosedFlag\": \"N\"," +
                //"\"accountClosedFlag\": \"Y\"," +
                "\"accountClosedReasonCode\": \"\"," +
                "\"accountClosedRemarks\": \"\"," +
                "\"accountClosedDate\": 0," +
                "\"lastBalanceUpdateDateTime\": 0," +
                "\"earmarkUpdateDateTime\": 0," +
                "\"holdBalanceUpdateDateTime\": 0," +
                "\"sanctionLimitUpdateDateTime\": 0," +
                "\"staticDataUpdateDateTime\": 1656516121000," +
                "\"halfDayHoldBalanceExpiryDate\": 0," +
                "\"childAccounts\": []," +
                "\"accountBalances\": 	{" +
                        "				   \"availableBalance\": -374," +
                        "				   \"availableBalanceCurrencyCode\": \"INR\"," +
                        "				   \"accountBalance\": 126," +
                        "				   \"accountBalanceCurrencyCode\": \"INR\"," +
                        "				   \"sanctionLimit\": 0," +
                        "				   \"sanctionLimitCurrencyCode\": \"INR\"," +
                        "				   \"ledgerBalance\": 126," +
                        "				   \"ledgerBalanceCurrencyCode\": \"INR\"," +
                        "				   \"halfDayHoldBalance\": 0," +
                        "				   \"oneDayHoldBalance\": 0," +
                        "				   \"twoDayHoldBalance\": 0," +
                        "				   \"earmarkDebitAmount\": 0," +
                        "				   \"earmarkCreditAmount\": 500," +
                        "				   \"floatAmount\": 0," +
                        "				   \"earmarkAmount\": 500," +
                        "				   \"effectiveAvailableAmount\": -374," +
                        "				   \"drawingPower\": 0," +
                        "				   \"overDueLiableAmount\": 0," +
                        "				   \"openingBalanceAmount\": 0," +
                        "				   \"closingBalanceAmount\": 0," +
                        "				   \"fundsClearingAmount\": 0," +
                        "				   \"cumulativeCreditAmount\": 35170," +
                        "				   \"cumulativeDebitAmount\": 35044," +
                        "				   \"utilizedAmount\": 0," +
                        "				   \"systemReservedAmount\": 0," +
                        "				   \"overdueFutureAmount\": 0," +
                        "				   \"utilizedFutureAmount\": 0," +
                        "				   \"effectiveFutureAvailableAmount\": 0," +
                        "				   \"availableAmountLineOfCredit\": 0," +
                        "				   \"unclearDrawingAmount\": 0," +
                        "				   \"ffdAvailableAmount\": 0," +
                        "				   \"sweepsEffectiveAvailableAmount\": 0," +
                        "				   \"hcAvailableAmount\": 0," +
                        "				   \"futureAmount\": 0," +
                        "				   \"futureCreditAmount\": 0," +
                        "				   \"futureClearBalanceAmount\": 0," +
                        "				   \"futureUnclearBalanceAmount\": 0," +
                        "				   \"daccLimit\": 0," +
                        "				   \"dafaLimit\": 0 " +
                        "					}," +
                        "\"relatedCustomerInfo\": [" +
                        "                            { " +
                        "                            \"relatedPartyCode\": \"\"," +
                        "                            \"relatedPartyCodeDescription\": \"\"," +
                        "                            \"relatedPartyCustomerId\": \"22945768\"," +
                        "                            \"relatedPartySourceCustomerId\": \"\"," +
                        "                            \"relatedPartyType\": \"M\"," +
                        "                            \"relatedPartyTypeDescription\": \"Main Holder\"," +
                        "                            \"relatedPartyDeleteFlag\": \"N\"," +
                        "                            \"relatedPartyAddressType\": \"Mailing\"" +
                        "                            }," +
                        "                            { " +
                        "                            \"relatedPartyCode\": \"\"," +
                        "                            \"relatedPartyCodeDescription\": \"\"," +
                        "                            \"relatedPartyCustomerId\": \"22945769\"," +
                        "                            \"relatedPartySourceCustomerId\": \"\"," +
                        "                            \"relatedPartyType\": \"M\"," +
                        "                            \"relatedPartyTypeDescription\": \"2nd Holder\"," +
                        "                            \"relatedPartyDeleteFlag\": \"N\"," +
                        "                            \"relatedPartyAddressType\": \"Mailing\"" +
                        "                            }" +
                        "                      ]," +
                        "\"promoCode\": []," +
                        "\"mobileMoneyIdentifier\": \"9641689\"," +
                        "\"mobileNumbers\": [ \"7358386665\" ]," +
                        "\"reference1\": \"\"," +
                        "\"reference2\": \"\"," +
                        "\"ifscCode\": \"DBSS0IN0811\"," +
                        "\"channelId\": \"SOI\"," +
                        "\"accountStatusDate\": 1623456000000," +
                        "\"expressAccountExpiryDate\": 0," +
                        "\"schemeConversionDate\": 0," +
                        "\"virtualAccountType\": \"\"," +
                        "\"faxIndeminity\": \"\"," +
                        "\"nomineeAvailableFlag\": \"N\"," +
                        "\"nomineeGuardianInfo\": []" +
                "}   ";

            }
            else if (sAccountNo == "444444444444")
            {
                sResposne =
            "{ " +
                "\"sourceAccountNumber\": \"881028754855\"," +
                "\"ibanAccountNumber\": \"\"," +
                "\"sourceSystemId\": \"\"," +
                "\"sourceCustomerId\": \"22945768\"," +
                "\"productType\": \"SBA\"," +
                "\"productTypeDescription\": []," +
                "\"subProductType\": \"\"," +
                "\"productCode\": \"DBSBA\"," +
                "\"productCodeDescription\": [ { \"productCodeDescription\": \"DIGISAVINGS\"," +
                    "                              \"languageCode\": \"INFENG\"         } ]," +
                //"\"productName\": \"NA\"," +
                "\"productName\": \"CAGOS\"," +
                "\"nativeProductName\": \"DBSBA\"," +
                "\"accountCurrency\": \"INR\"," +
                "\"openedDate\": 1546041600000," +
                "\"modeOfOperation\": \"0003\"," +
                "\"serviceChargeExemption\": 0," +
                "\"staffIndicator\": false," +
                "\"officerId\": \"\"," +
                "\"officerUnit\": \"\"," +
                "\"firstExcessDate\": 0," +
                "\"lastUpdatedEvent\": \"\"," +
                "\"returnChequeDetailsInfo\": []," +
                "\"accountStatus\": \"Inactive\"," +
                "\"accountStatusCode\": \"I\"," +
                "\"accountSignatoryType\": \"01\"," +
                "\"accountSignal\": \"\"," +
                "\"loanServicingIndicator\": false," +
                "\"accountFrozenIndicator\": true," +
                "\"noDebitIndicator\": false," +
                "\"debitReferralIndicator\": false," +
                "\"irregularSignalIndicator\": false," +
                "\"lineOfferedIndicator\": false," +
                "\"closureNoticeIndicator\": false," +
                "\"multipleAccountIndicator\": false," +
                "\"recallPassbookIndicator\": false," +
                "\"updateRequiredIndicator\": false," +
                "\"productIndicator\": \"\"," +
                "\"brandIndicator\": \"\"," +
                "\"ibanIndicator\": \"N\"," +
                "\"virtualAccountIndicator\": \"N\"," +
                "\"spclCustomerType\": \"\"," +
                "\"accountType\": \"\"," +
                "\"accountTypeDescription\": \"\"," +
                "\"currencyDecimal\": 2," +
                "\"generalLedgerSubHeadCode\": \"21201\"," +
                "\"accountCurrencyCode\": \"INR\"," +
                "\"odLimitType\": \"\"," +
                "\"odInterestAmount\": 0," +
                "\"accountName\": \"A XAXX GXXXSX\"," +
                "\"accountShortName\": \"A XAXX GXX\"," +
                "\"virtualAccountName\": \"\"," +
                "\"accountStatement\": { \"statementMode\": \"S\"," +
                                          "  \"statementCalendar\": \"00\"," +
                                          "  \"frequency\": \"M\"," +
                                          "  \"frequencyStartDate\": 31," +
                                          "  \"frequencyDay\": 0," +
                                          "  \"frequencyWeekNumber\": 0," +
                                          "  \"frequencyHolidayStatus\": \"N\"," +
                                          "  \"nextPrintDate\": 1546214400000," +
                                          "  \"despatchMode\": \"N\" " +
                                          "  }" +
                                          " ," +
                "\"balanceDebitCreditIndicator\": \"C\"," +
                "\"freezeCode\": \"C\"," +
                "\"freezeStatusCode\": \"C\"," +
                "\"freezeReasonCode\": \"0009\"," +
                "\"freezeReasonCode1\": \"\"," +
                "\"additionalFreezeReasonCodes\": []," +
                "\"additionalFreezeRemarks\": []," +
                "\"freezeReasonCodeDescriptionList\": []," +
                "\"freezeRemarks\": \"\"," +
                "\"freezeRemarks1\": \"\"," +
                "\"accountInterest\": { \"interestRate\": 3.25," +
                    "                     \"interestCalInterest\": 0," +
                    "                     \"interestFrequencyType\": \"M\"," +
                    "                     \"interestFrequencyStartDate\": 31," +
                    "                     \"interestFrequencyDay\": 0," +
                    "                     \"interestFrequencyWeekNum\": 0," +
                    "                     \"accountInterestFrequencyHolidayStatus\": \"P\"," +
                    "                     \"interestRateCode\": \"DBSB1\"," +
                    "                     \"netInterestRate\": 0," +
                    "                     \"netInterestDebitCreditIndicator\": \"C\"," +
                    "                     \"accruedInterestDebitCreditIndicator\": \"C\"," +
                    "                     \"unpaidInterestFee\": 0," +
                    "                     \"bookedamount\": 0," +
                    "                     \"interestAmount\": 0," +
                    "                     \"preferentialInterest\": 0" +
                    "                   }" +
                    "," +
                "\"taxCategory\": \"A\"," +
                "\"taxFloorLimit\": 0," +
                "\"taxFloorLimitCurrencyCode\": \"INR\"," +
                "\"withholdingTaxPercent\": 0," +
                "\"gstin\": \"\"," +
                "\"gstExemptionFlag\": \"\"," +
                "\"nickName\": \"\"," +
                "\"productShortName\": \"DBSBA\"," +
                "\"preferredLanguageProductShortName\": \"DBSBA\"," +
                "\"sourceMultiCurrencyAccountNumber\": \"\"," +
                "\"multiCurrencyAccountFlag\": false," +
                "\"branchCode\": \"811\"," +
                "\"branchCodeDescription\": \"\"," +
                "\"bankCode\": \"DBSIN\"," +
                //"\"accountClosedFlag\": \"N\"," +
                "\"accountClosedFlag\": \"Y\"," +
                "\"accountClosedReasonCode\": \"\"," +
                "\"accountClosedRemarks\": \"\"," +
                "\"accountClosedDate\": 0," +
                "\"lastBalanceUpdateDateTime\": 0," +
                "\"earmarkUpdateDateTime\": 0," +
                "\"holdBalanceUpdateDateTime\": 0," +
                "\"sanctionLimitUpdateDateTime\": 0," +
                "\"staticDataUpdateDateTime\": 1656516121000," +
                "\"halfDayHoldBalanceExpiryDate\": 0," +
                "\"childAccounts\": []," +
                "\"accountBalances\": 	{" +
                        "				   \"availableBalance\": -374," +
                        "				   \"availableBalanceCurrencyCode\": \"INR\"," +
                        "				   \"accountBalance\": 126," +
                        "				   \"accountBalanceCurrencyCode\": \"INR\"," +
                        "				   \"sanctionLimit\": 0," +
                        "				   \"sanctionLimitCurrencyCode\": \"INR\"," +
                        "				   \"ledgerBalance\": 126," +
                        "				   \"ledgerBalanceCurrencyCode\": \"INR\"," +
                        "				   \"halfDayHoldBalance\": 0," +
                        "				   \"oneDayHoldBalance\": 0," +
                        "				   \"twoDayHoldBalance\": 0," +
                        "				   \"earmarkDebitAmount\": 0," +
                        "				   \"earmarkCreditAmount\": 500," +
                        "				   \"floatAmount\": 0," +
                        "				   \"earmarkAmount\": 500," +
                        "				   \"effectiveAvailableAmount\": -374," +
                        "				   \"drawingPower\": 0," +
                        "				   \"overDueLiableAmount\": 0," +
                        "				   \"openingBalanceAmount\": 0," +
                        "				   \"closingBalanceAmount\": 0," +
                        "				   \"fundsClearingAmount\": 0," +
                        "				   \"cumulativeCreditAmount\": 35170," +
                        "				   \"cumulativeDebitAmount\": 35044," +
                        "				   \"utilizedAmount\": 0," +
                        "				   \"systemReservedAmount\": 0," +
                        "				   \"overdueFutureAmount\": 0," +
                        "				   \"utilizedFutureAmount\": 0," +
                        "				   \"effectiveFutureAvailableAmount\": 0," +
                        "				   \"availableAmountLineOfCredit\": 0," +
                        "				   \"unclearDrawingAmount\": 0," +
                        "				   \"ffdAvailableAmount\": 0," +
                        "				   \"sweepsEffectiveAvailableAmount\": 0," +
                        "				   \"hcAvailableAmount\": 0," +
                        "				   \"futureAmount\": 0," +
                        "				   \"futureCreditAmount\": 0," +
                        "				   \"futureClearBalanceAmount\": 0," +
                        "				   \"futureUnclearBalanceAmount\": 0," +
                        "				   \"daccLimit\": 0," +
                        "				   \"dafaLimit\": 0 " +
                        "					}," +
                        "\"relatedCustomerInfo\": [" +
                        "                            { " +
                        "                            \"relatedPartyCode\": \"\"," +
                        "                            \"relatedPartyCodeDescription\": \"\"," +
                        "                            \"relatedPartyCustomerId\": \"22945768\"," +
                        "                            \"relatedPartySourceCustomerId\": \"\"," +
                        "                            \"relatedPartyType\": \"M\"," +
                        "                            \"relatedPartyTypeDescription\": \"Main Holder\"," +
                        "                            \"relatedPartyDeleteFlag\": \"N\"," +
                        "                            \"relatedPartyAddressType\": \"Mailing\"" +
                        "                            }," +
                        "                            { " +
                        "                            \"relatedPartyCode\": \"\"," +
                        "                            \"relatedPartyCodeDescription\": \"\"," +
                        "                            \"relatedPartyCustomerId\": \"22945769\"," +
                        "                            \"relatedPartySourceCustomerId\": \"\"," +
                        "                            \"relatedPartyType\": \"M\"," +
                        "                            \"relatedPartyTypeDescription\": \"2nd Holder\"," +
                        "                            \"relatedPartyDeleteFlag\": \"N\"," +
                        "                            \"relatedPartyAddressType\": \"Mailing\"" +
                        "                            }" +
                        "                      ]," +
                        "\"promoCode\": []," +
                        "\"mobileMoneyIdentifier\": \"9641689\"," +
                        "\"mobileNumbers\": [ \"7358386665\" ]," +
                        "\"reference1\": \"\"," +
                        "\"reference2\": \"\"," +
                        "\"ifscCode\": \"DBSS0IN0811\"," +
                        "\"channelId\": \"SOI\"," +
                        "\"accountStatusDate\": 1623456000000," +
                        "\"expressAccountExpiryDate\": 0," +
                        "\"schemeConversionDate\": 0," +
                        "\"virtualAccountType\": \"\"," +
                        "\"faxIndeminity\": \"\"," +
                        "\"nomineeAvailableFlag\": \"N\"," +
                        "\"nomineeGuardianInfo\": []" +
                "}   ";

            }
            else if (sAccountNo == "555555555555")
            {
                sResposne =
                "{ " +
                "\"error\":{ " +
                "\"timestamp\": \"1668148199647\"," +
                "\"status\": \"INTERNAL_SERVER_ERROR\"," +
                "\"errorType\": \"BE\"," +
                "\"errorCode\": \"MSTD-CD-5008\"," +
                "\"errorMessage\": \"Finacle Business Error\"," +
                "\"methodName\": \"\"," +
                "\"restError\":[ " +
                     "  {" +
                     "\"errorCode\": \"W025\"," +
                     "\"errorMessage\": \"Invalid Account Details \"" +
                     "  }" +
                     "  ]" +
                    "  }," +
                "\"timestamp\": \"1668148199647\"," +
                "\"status\": \"INTERNAL_SERVER_ERROR\"," +
                "\"errorType\": \"BE\"," +
                "\"errorCode\": \"MSTD-CD-5008\"," +
                "\"errorDescription\": \"Finacle Business Error\"," +
                "\"methodName\": \"\"," +
                "\"restError\": " +
                "  [" +
                "  {" +
                "\"errorCode\": \"W025\"," +
                "\"errorDescription\": \"Invalid Account Details \"" +
                 "  }" +
                  "  ]" +
                "  }";
            }
            //string sResposne =
            //"{ " +
            //    "\"sourceAccountNumber\": \"881028754855\"," +
            //    "\"ibanAccountNumber\": \"\"," +
            //    "\"sourceSystemId\": \"\"," +
            //    "\"sourceCustomerId\": \"22945768\"," +
            //    "\"productType\": \"SBA\"," +
            //    "\"productTypeDescription\": []," +
            //    "\"subProductType\": \"\"," +
            //    "\"productCode\": \"DBSBA\"," +
            //    "\"productCodeDescription\": [ { \"productCodeDescription\": \"DIGISAVINGS\"," +
            //        "                              \"languageCode\": \"INFENG\"         } ]," +
            //    //"\"productName\": \"NA\"," +
            //    "\"productName\": \"NRESA\"," +
            //    "\"nativeProductName\": \"DBSBA\"," +
            //    "\"accountCurrency\": \"INR\"," +
            //    "\"openedDate\": 1546041600000," +
            //    "\"modeOfOperation\": \"0003\"," +
            //    "\"serviceChargeExemption\": 0," +
            //    "\"staffIndicator\": false," +
            //    "\"officerId\": \"\"," +
            //    "\"officerUnit\": \"\"," +
            //    "\"firstExcessDate\": 0," +
            //    "\"lastUpdatedEvent\": \"\"," +
            //    "\"returnChequeDetailsInfo\": []," +
            //    "\"accountStatus\": \"Inactive\"," +
            //    "\"accountStatusCode\": \"I\"," +
            //    "\"accountSignatoryType\": \"01\"," +
            //    "\"accountSignal\": \"\"," +
            //    "\"loanServicingIndicator\": false," +
            //    "\"accountFrozenIndicator\": true," +
            //    "\"noDebitIndicator\": false," +
            //    "\"debitReferralIndicator\": false," +
            //    "\"irregularSignalIndicator\": false," +
            //    "\"lineOfferedIndicator\": false," +
            //    "\"closureNoticeIndicator\": false," +
            //    "\"multipleAccountIndicator\": false," +
            //    "\"recallPassbookIndicator\": false," +
            //    "\"updateRequiredIndicator\": false," +
            //    "\"productIndicator\": \"\"," +
            //    "\"brandIndicator\": \"\"," +
            //    "\"ibanIndicator\": \"N\"," +
            //    "\"virtualAccountIndicator\": \"N\"," +
            //    "\"spclCustomerType\": \"\"," +
            //    "\"accountType\": \"\"," +
            //    "\"accountTypeDescription\": \"\"," +
            //    "\"currencyDecimal\": 2," +
            //    "\"generalLedgerSubHeadCode\": \"21201\"," +
            //    "\"accountCurrencyCode\": \"INR\"," +
            //    "\"odLimitType\": \"\"," +
            //    "\"odInterestAmount\": 0," +
            //    "\"accountName\": \"A XAXX GXXXSX\"," +
            //    "\"accountShortName\": \"A XAXX GXX\"," +
            //    "\"virtualAccountName\": \"\"," +
            //    "\"accountStatement\": { \"statementMode\": \"S\"," +
            //                              "  \"statementCalendar\": \"00\"," +
            //                              "  \"frequency\": \"M\"," +
            //                              "  \"frequencyStartDate\": 31," +
            //                              "  \"frequencyDay\": 0," +
            //                              "  \"frequencyWeekNumber\": 0," +
            //                              "  \"frequencyHolidayStatus\": \"N\"," +
            //                              "  \"nextPrintDate\": 1546214400000," +
            //                              "  \"despatchMode\": \"N\" " +
            //                              "  }" +
            //                              " ," +
            //    "\"balanceDebitCreditIndicator\": \"C\"," +
            //    "\"freezeCode\": \"C\"," +
            //    "\"freezeStatusCode\": \"C\"," +
            //    "\"freezeReasonCode\": \"0009\"," +
            //    "\"freezeReasonCode1\": \"\"," +
            //    "\"additionalFreezeReasonCodes\": []," +
            //    "\"additionalFreezeRemarks\": []," +
            //    "\"freezeReasonCodeDescriptionList\": []," +
            //    "\"freezeRemarks\": \"\"," +
            //    "\"freezeRemarks1\": \"\"," +
            //    "\"accountInterest\": { \"interestRate\": 3.25," +
            //        "                     \"interestCalInterest\": 0," +
            //        "                     \"interestFrequencyType\": \"M\"," +
            //        "                     \"interestFrequencyStartDate\": 31," +
            //        "                     \"interestFrequencyDay\": 0," +
            //        "                     \"interestFrequencyWeekNum\": 0," +
            //        "                     \"accountInterestFrequencyHolidayStatus\": \"P\"," +
            //        "                     \"interestRateCode\": \"DBSB1\"," +
            //        "                     \"netInterestRate\": 0," +
            //        "                     \"netInterestDebitCreditIndicator\": \"C\"," +
            //        "                     \"accruedInterestDebitCreditIndicator\": \"C\"," +
            //        "                     \"unpaidInterestFee\": 0," +
            //        "                     \"bookedamount\": 0," +
            //        "                     \"interestAmount\": 0," +
            //        "                     \"preferentialInterest\": 0" +
            //        "                   }" +
            //        "," +
            //    "\"taxCategory\": \"A\"," +
            //    "\"taxFloorLimit\": 0," +
            //    "\"taxFloorLimitCurrencyCode\": \"INR\"," +
            //    "\"withholdingTaxPercent\": 0," +
            //    "\"gstin\": \"\"," +
            //    "\"gstExemptionFlag\": \"\"," +
            //    "\"nickName\": \"\"," +
            //    "\"productShortName\": \"DBSBA\"," +
            //    "\"preferredLanguageProductShortName\": \"DBSBA\"," +
            //    "\"sourceMultiCurrencyAccountNumber\": \"\"," +
            //    "\"multiCurrencyAccountFlag\": false," +
            //    "\"branchCode\": \"811\"," +
            //    "\"branchCodeDescription\": \"\"," +
            //    "\"bankCode\": \"DBSIN\"," +
            //    //"\"accountClosedFlag\": \"N\"," +
            //    "\"accountClosedFlag\": \"Y\"," +
            //    "\"accountClosedReasonCode\": \"\"," +
            //    "\"accountClosedRemarks\": \"\"," +
            //    "\"accountClosedDate\": 0," +
            //    "\"lastBalanceUpdateDateTime\": 0," +
            //    "\"earmarkUpdateDateTime\": 0," +
            //    "\"holdBalanceUpdateDateTime\": 0," +
            //    "\"sanctionLimitUpdateDateTime\": 0," +
            //    "\"staticDataUpdateDateTime\": 1656516121000," +
            //    "\"halfDayHoldBalanceExpiryDate\": 0," +
            //    "\"childAccounts\": []," +
            //    "\"accountBalances\": 	{" +
            //            "				   \"availableBalance\": -374," +
            //            "				   \"availableBalanceCurrencyCode\": \"INR\"," +
            //            "				   \"accountBalance\": 126," +
            //            "				   \"accountBalanceCurrencyCode\": \"INR\"," +
            //            "				   \"sanctionLimit\": 0," +
            //            "				   \"sanctionLimitCurrencyCode\": \"INR\"," +
            //            "				   \"ledgerBalance\": 126," +
            //            "				   \"ledgerBalanceCurrencyCode\": \"INR\"," +
            //            "				   \"halfDayHoldBalance\": 0," +
            //            "				   \"oneDayHoldBalance\": 0," +
            //            "				   \"twoDayHoldBalance\": 0," +
            //            "				   \"earmarkDebitAmount\": 0," +
            //            "				   \"earmarkCreditAmount\": 500," +
            //            "				   \"floatAmount\": 0," +
            //            "				   \"earmarkAmount\": 500," +
            //            "				   \"effectiveAvailableAmount\": -374," +
            //            "				   \"drawingPower\": 0," +
            //            "				   \"overDueLiableAmount\": 0," +
            //            "				   \"openingBalanceAmount\": 0," +
            //            "				   \"closingBalanceAmount\": 0," +
            //            "				   \"fundsClearingAmount\": 0," +
            //            "				   \"cumulativeCreditAmount\": 35170," +
            //            "				   \"cumulativeDebitAmount\": 35044," +
            //            "				   \"utilizedAmount\": 0," +
            //            "				   \"systemReservedAmount\": 0," +
            //            "				   \"overdueFutureAmount\": 0," +
            //            "				   \"utilizedFutureAmount\": 0," +
            //            "				   \"effectiveFutureAvailableAmount\": 0," +
            //            "				   \"availableAmountLineOfCredit\": 0," +
            //            "				   \"unclearDrawingAmount\": 0," +
            //            "				   \"ffdAvailableAmount\": 0," +
            //            "				   \"sweepsEffectiveAvailableAmount\": 0," +
            //            "				   \"hcAvailableAmount\": 0," +
            //            "				   \"futureAmount\": 0," +
            //            "				   \"futureCreditAmount\": 0," +
            //            "				   \"futureClearBalanceAmount\": 0," +
            //            "				   \"futureUnclearBalanceAmount\": 0," +
            //            "				   \"daccLimit\": 0," +
            //            "				   \"dafaLimit\": 0 " +
            //            "					}," +
            //            "\"relatedCustomerInfo\": [" +
            //            "                            { " +
            //            "                            \"relatedPartyCode\": \"\"," +
            //            "                            \"relatedPartyCodeDescription\": \"\"," +
            //            "                            \"relatedPartyCustomerId\": \"22945768\"," +
            //            "                            \"relatedPartySourceCustomerId\": \"\"," +
            //            "                            \"relatedPartyType\": \"M\"," +
            //            "                            \"relatedPartyTypeDescription\": \"Main Holder\"," +
            //            "                            \"relatedPartyDeleteFlag\": \"N\"," +
            //            "                            \"relatedPartyAddressType\": \"Mailing\"" +
            //            "                            }," +
            //            "                            { " +
            //            "                            \"relatedPartyCode\": \"\"," +
            //            "                            \"relatedPartyCodeDescription\": \"\"," +
            //            "                            \"relatedPartyCustomerId\": \"22945769\"," +
            //            "                            \"relatedPartySourceCustomerId\": \"\"," +
            //            "                            \"relatedPartyType\": \"M\"," +
            //            "                            \"relatedPartyTypeDescription\": \"2nd Holder\"," +
            //            "                            \"relatedPartyDeleteFlag\": \"N\"," +
            //            "                            \"relatedPartyAddressType\": \"Mailing\"" +
            //            "                            }" +
            //            "                      ]," +
            //            "\"promoCode\": []," +
            //            "\"mobileMoneyIdentifier\": \"9641689\"," +
            //            "\"mobileNumbers\": [ \"7358386665\" ]," +
            //            "\"reference1\": \"\"," +
            //            "\"reference2\": \"\"," +
            //            "\"ifscCode\": \"DBSS0IN0811\"," +
            //            "\"channelId\": \"SOI\"," +
            //            "\"accountStatusDate\": 1623456000000," +
            //            "\"expressAccountExpiryDate\": 0," +
            //            "\"schemeConversionDate\": 0," +
            //            "\"virtualAccountType\": \"\"," +
            //            "\"faxIndeminity\": \"\"," +
            //            "\"nomineeAvailableFlag\": \"N\"," +
            //            "\"nomineeGuardianInfo\": []" +
            //    "}   ";




            return sResposne;
        }

        private string getAccountDetailsDBSInvalid(string sServiceUrl, string sClientId, string sCoreRelationId, string sAccountNo)
        {
            string sResposne =
            "{ " +
                "\"error\":{ " +
                "\"timestamp\": \"1658131418469\"," +
                "\"status\": \"INTERNAL_SERVER_ERROR\"," +
                "\"errorType\": \"BE\"," +
                "\"errorCode\": \"MSTD-CD-5008\"," +
                "\"errorMessage\": \"Finacle Business Error\" }" +
                    //"\"methodName\": \"\"" +
                    //"\"restError\":[ " +
                    // "  {" +
                    // "\"errorCode\": \"W025\"," +
                    // "\"errorMessage\": \"Invalid Entity Details\"" +
                    // "  }" +
                    // "  ]" +
                    //"  }";
                    //"\"timestamp\": \"1658131418469\"," +
                    //"\"status\": \"INTERNAL_SERVER_ERROR\"," +
                    //"\"errorType\": \"BE\"," +
                    //"\"errorCode\": \"MSTD-CD-5008\"," +
                    //"\"errorDescription\": \"Finacle Business Error\"," +
                    //"\"methodName\": \"\"," +
                    //"\"restError\": " +
                    //"  [" +
                    //"  {" +
                    //"\"errorCode\": \"W025\"," +
                    //"\"errorDescription\": \"Invalid Entity Details " +
                    // "  }" +
                    //  "  ]" +
                    "  }";



            return sResposne;
        }
        private string sendCMCPTokenRequest(string TokenServiceURL, string TokenClientId, string TokenSecreteKey)
        {
            string sResposne = "";
            try
            {
                string sBase64String = TokenClientId + ":" + TokenSecreteKey;
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(sBase64String);
                sBase64String = System.Convert.ToBase64String(plainTextBytes);
                var stringContent = new StringContent(string.Empty);

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(TokenServiceURL);
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", "basic " + sBase64String);
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(stringContent);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    sResposne = result;
                }
            }
            catch (Exception Ex)
            {
                sResposne = "{" +
                            "\"errorCode\":\"Runtime Error While Sending the Request\"," +
                            "\"errorDescription\":\"" + Ex.Message +
                           "}";

                logerrorInCatch("sendCMCPTokenRequest: ", Ex.Message);
            }
            return sResposne;
        }


        string getCMCPToken(string sResposne)
        {
            string sEtoken = "";

            var jObject = Newtonsoft.Json.Linq.JObject.Parse(sResposne);
            if (jObject["access_token"] != null)
                sEtoken = jObject["access_token"].ToString();
            else
            {
                //WriteState code to log error

                //jObject["errorDescription"].ToString()
            }

            return sEtoken;
        }

        string sendCMPCPRequest(string CMCPServiceURL, string CMCPCountry, string CMCPReqUID, string CMCPReqClientId, string sEtoken, string sAccountNo)
        {
            string sResponse = "";
            try
            {
                string sInputString = "";

                //sInputString = " {";
                //sInputString += "   \"cmcpId\": \"" + sAccountNo + "\"";
                //sInputString += "}";

                sInputString = " {";
                sInputString += "   \"cin\": \"" + sAccountNo + "\",";
                sInputString += "   \"cinSuffix\": \"00\"";
                sInputString += "}";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(CMCPServiceURL);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("X-DBS-Country", CMCPCountry);
                httpWebRequest.Headers.Add("X-DBS-ReqUID", CMCPReqUID);
                httpWebRequest.Headers.Add("X-DBS-ReqClientId", CMCPReqClientId);
                httpWebRequest.Headers.Add("Authorization", "Bearer " + sEtoken);

                //
                //txtRequest.Text = sInputString; ;
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(sInputString);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    sResponse = result;
                }
            }
            catch (Exception Ex)
            {
                sResponse = "{" +
                            "\"errorCode\":\"Runtime Error While Sending the Request\"," +
                            "\"errorDescription\":\"" + Ex.Message +
                           "}";
            }

            return sResponse;
        }


        string GetCMCPCustomerName(string sResposne)
        {
            string sCustomerName = "";
            try
            {
                
                var jObject = Newtonsoft.Json.Linq.JObject.Parse(sResposne);
                if (jObject["data"]["profileInfo"]["registeredName"] != null)
                {
                    sCustomerName = jObject["data"]["profileInfo"]["registeredName"].ToString().Trim();
                }
                else
                {
                    //WriteState code to log error
                    logerror(jObject["errorDescription"].ToString(), "");
                }
                return sCustomerName;
            }
            catch (Exception Ex)
            {
                logerrorInCatch("Exception in GetCMCPCustomerName ", Ex.Message.ToString());
                return sCustomerName;
            }
            
        }


        void LogError()
        {
            //error log
            string sServerPath = Server.MapPath("~/Logs/");
            if (System.IO.Directory.Exists(sServerPath) == false)
                System.IO.Directory.CreateDirectory(sServerPath);

            string sfilename = DateTime.Now.ToString("ddMMyyyy") + "Logs.txt";
            string sfileNameWithPath = sServerPath + sfilename;
            System.IO.StreamWriter str1 = new System.IO.StreamWriter(sfileNameWithPath, true, System.Text.Encoding.Default);

            str1.WriteLine("\n");
            str1.WriteLine(DateTime.Now.ToShortTimeString() + ": Request: " + sInputString);
            str1.WriteLine("\n");

            str1.WriteLine(DateTime.Now.ToShortTimeString() + ": sCoreRelationId: " + sCasaCorellationId);
            str1.WriteLine("\n");
            str1.WriteLine(DateTime.Now.ToShortTimeString() + ": Timestamp: " + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            str1.WriteLine("\n");
            str1.WriteLine(DateTime.Now.ToShortTimeString() + ": sClientId: " + sCasaClientId);
            str1.WriteLine("\n");
            str1.WriteLine(DateTime.Now.ToShortTimeString() + ": sAccountNo: " + sAccountNo);
            str1.WriteLine("\n");
            str1.WriteLine(DateTime.Now.ToShortTimeString() + ": sServiceUrl: " + sCasaServiceURL);
            str1.WriteLine("\n");

            str1.WriteLine(DateTime.Now.ToShortTimeString() + ": Return API: " + sgetAccountDetailsDBS);
            str1.WriteLine("\n");
            str1.WriteLine("Response: " + sResposne);

            str1.Close();
        }

        public PartialViewResult GetSourceofFunds()
        {

            try
            {
                List<SelectListItem> items = new List<SelectListItem>();
                SqlDataAdapter adp = new SqlDataAdapter();

                if (Session["sNR"] == null)
                    Session["sNR"] = "";

                if (Session["sNR"].ToString().Trim() != "")
                {
                    if (Session["sNR"].ToString().Trim() == "NRE")
                        adp = new SqlDataAdapter("SP_NRESourceOfFund", con);
                    else if (Session["sNR"].ToString().Trim() == "NRO")
                        adp = new SqlDataAdapter("SP_NROSourceOfFund", con);

                    adp.SelectCommand.CommandType = CommandType.StoredProcedure;

                    DataTable dt = new DataTable();
                    adp.Fill(dt);

                    //int i = 0;
                    //while (i < dt.Rows.Count)
                    //{
                    //    lSrcFnds.Add(dt.Rows[i][1].ToString().Trim());
                    //    i = i + 1;
                    //}

                    ViewBag.vbSrcFnds = dt.DefaultView;
                }
                else
                    ViewBag.vbSrcFnds = "";

                return PartialView("_SourceFunds");
            }
            catch (Exception e)
            {
                ErrorDisplay er = new ErrorDisplay();
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;
                er.ErrorMessage = message;

                return PartialView("Error", er);

            }
        }

        static int CalculateDifferenceInMonths(DateTimeOffset laterDate, DateTimeOffset earlierDate)
        {
            int monthsApart = 12 * (laterDate.Year - earlierDate.Year) + laterDate.Month - earlierDate.Month;

            if (laterDate.Day < earlierDate.Day)
            {
                monthsApart--;
            }

            return monthsApart;
        }

        public string testCMCP_Response()
        {
            var data = "";
            return data = "{\"data\":{\"cmcpId\":\"IN1I2200Z52FL\",\"cin\":\"23997348\",\"cinSuffix\":\"00\",\"customerType\":1,\"profileInfo\":{\"dbsIndustryCode\":{\"codeValueId\":583,\"codeValueCd\":\"99950\",\"referenceCodeValueCd\":\"99950\",\"codeValueDisplay\":\"PRIVATEINDIVIDUALS\"},\"countryOfResidenceCode\":{\"codeValueId\":1959,\"codeValueCd\":\"IN\",\"referenceCodeValueCd\":\"IN\",\"codeValueDisplay\":\"INDIA\"},\"registeredName\":\"5MvnqWFkVzt9bnJjndaGIndNkzombY0RyqBlI4csfPsnmlWzE9H9c7MEPpPLWm97ENV4vLjcWE0SMAZSUgOfqBfsG0moX0BMrIX\",\"nameLine1\":\"j873YaKrblaMd1Pjgx51UyTs1TUlEdvCfuXidNhdUT3yUOxgRg3EOMK3FxaXiN4CsCqe7ztcrotZjp7v\",\"nameLine2\":\"8YdAHBeNIK5VIJGk0j5ke5A9h2N7TGVaiubsL9CDXeUvCJB8b5cMCxGaYqYZLra4V8y3z88XzpoDYcFh\",\"customerSubTypeCode\":{\"codeValueId\":694,\"codeValueCd\":\"R\",\"referenceCodeValueCd\":\"R\",\"codeValueDisplay\":\"RETAIL\"},\"relationshipStartDate\":\"2021-03-20\",\"nameInNativeLanguage\":\"P5ljQEZ7U9KQYHkkzWZCXZ3jc2TKldNyjGD5VJg5fT25IyPH236u22Su41VR8utPgoMrfQXanJxzUXPB\",\"preferredLanguageCode\":{\"codeValueId\":12014,\"codeValueCd\":\"001\",\"referenceCodeValueCd\":\"001\",\"codeValueDisplay\":\"ENGLISH\"},\"remarks\":\"\",\"taxDeductionAtSourceTableCode\":{\"codeValueId\":57956,\"codeValueCd\":\"TDS01\",\"referenceCodeValueCd\":\"TDS01\",\"codeValueDisplay\":\"TDS01\"},\"sectorCode\":{\"codeValueId\":57264,\"codeValueCd\":\"31005\",\"referenceCodeValueCd\":\"31005\",\"codeValueDisplay\":\"BANKING/FINANCE\"},\"subSectorCode\":{\"codeValueId\":57698,\"codeValueCd\":\"30310\",\"referenceCodeValueCd\":\"30310\",\"codeValueDisplay\":\"BANKS-REPRESENTATIVEOFFICES\"},\"forexTierGroupCode\":{\"codeValueId\":57191,\"codeValueCd\":\"10\",\"referenceCodeValueCd\":\"10\",\"codeValueDisplay\":\"FXGroup10\"},\"regionCode\":{\"codeValueId\":57245,\"codeValueCd\":\"02\",\"referenceCodeValueCd\":\"02\",\"codeValueDisplay\":\"West\"},\"taxStatus\":{\"codeValueId\":57971,\"codeValueCd\":\"800001\",\"referenceCodeValueCd\":\"800001\",\"codeValueDisplay\":\"ResidentTax\"},\"constitutionCode\":{\"codeValueId\":1856,\"codeValueCd\":\"15\",\"referenceCodeValueCd\":\"15\",\"codeValueDisplay\":\"INDIVIDUAL\"},\"customerNetWorth\":\"4999999\",\"profitCenterCode\":{\"codeValueId\":57240,\"codeValueCd\":\"06\",\"referenceCodeValueCd\":\"06\",\"codeValueDisplay\":\"Retail\"},\"previousName\":\"TYiCedQ9mhPPVL9euoniv0F3SDg5Ae1kdDAGZUgahODYgK2fyOpcSarD1cBQmDhbaM1nKzELAU86znYkbrHVJKL0FRB8fD6DzMOFz2N4Q0OL1VTUb0hLPPfieNIQYeOo39iMRdLDG9x49EhJsrH3PbgYMdipSgOi\",\"primaryBranchCode\":{\"codeValueId\":58744,\"codeValueCd\":\"854\",\"referenceCodeValueCd\":\"854\",\"codeValueDisplay\":\"VIKHROLI\"},\"alias\":\"IVMZ5NbKfJlipTnpCBfVqW1Ms132PUOmqqAWBYhhDTMOTiBKsOFRPdX2Pf0f\",\"salutationCode\":{\"codeValueId\":16,\"codeValueCd\":\"MR\",\"referenceCodeValueCd\":\"MR\",\"codeValueDisplay\":\"MR\"},\"sexCode\":{\"codeValueId\":25,\"codeValueCd\":\"M\",\"referenceCodeValueCd\":\"M\",\"codeValueDisplay\":\"MALE\"},\"maritalStatusCode\":{\"codeValueId\":28,\"codeValueCd\":\"2\",\"referenceCodeValueCd\":\"2\",\"codeValueDisplay\":\"MARRIED\"},\"nativeLanguageCode\":{\"codeValueId\":13662,\"codeValueCd\":\"INFENG\",\"referenceCodeValueCd\":\"INFENG\",\"codeValueDisplay\":\"ENGLISH\"},\"countryOfBirthCode\":{\"codeValueId\":1959,\"codeValueCd\":\"IN\",\"referenceCodeValueCd\":\"IN\",\"codeValueDisplay\":\"INDIA\"},\"maidenName\":\"xIFPJmGkrc2q3gbaRjSo94EBUYx8duqkSekcuMPQ3e9ojuhMCJ0f3W9CUxXK3Au8zk6BZeUS3QfCFMnqkCk5IdvXT8Vjr5phesN\",\"dateOfBirth\":\"1980-11-15\",\"motherMaidenName\":\"Vhc2GyrQqbb5mlhQLRGB2i2987EPsdNqBvj6C5UsfGfEhW81bYWQmSe1r4U4l3W48f64q5uUabLIcDPc5LZ3EPMu6eL1KfIv9r3\",\"fatherName\":\"oLai5QCdDDK1m7tYqs05T3GXp6j09Q6N9c8TSmcHjJZTdfHKMxo4am822mWIHn7zPHI1gVv3NRNEJ2c7lVX1NWBTRExiVaLTBKe\",\"spouseName\":\"PAOFN4MCilVWqsXvbeWmpVh3Jf3VK5GMxe6mmKtDhsm1MH8TWOgAgp8RRujlSfPhmj8J8ujCZbkBY6vQ0MO1N9gA8MHgOqi5buC\",\"staffFlag\":true,\"staffId\":\"ViniaHeng\",\"industryType2Code\":{\"codeValueId\":56936,\"codeValueCd\":\"410\",\"referenceCodeValueCd\":\"410\",\"codeValueDisplay\":\"4.1Individuals(includingHUF)\"},\"preferredName\":\"zOA05eVKpFvRNDVadOLz5rAhya1folMFFYuDbC8abf3hheNVFW5JzMEuVsuRAanT6jerDxORB7hSCTBkQ0e3UgE7zOeTN96XJYo\",\"version\":1,\"createdBy\":\"FIVUSR\",\"createdByChannel\":\"MIGRATION\",\"createdTimeStamp\":\"2021-03-20T13:11:58+08:00\",\"updatedBy\":\"SEJALAGRAWAL\",\"updatedByChannel\":\"MIGRATION\",\"updatedTimeStamp\":\"2022-02-10T19:56:58+08:00\"},\"owners\":[{\"id\":831384,\"ownerCode\":{\"codeValueId\":1835,\"codeValueCd\":\"0001\",\"referenceCodeValueCd\":\"0001\",\"codeValueDisplay\":\"MassMarket\"},\"ownerOrder\":1,\"customerOwnerTPCIndicator\":\"04\",\"version\":1,\"createdBy\":\"FIVUSR\",\"createdByChannel\":\"MIGRATION\",\"createdByDepartment\":\"MIGRATION\",\"createdTimeStamp\":\"2021-03-20T13:11:58+08:00\",\"updatedBy\":\"SEJALAGRAWAL\",\"updatedByChannel\":\"MIGRATION\",\"updatedByDepartment\":\"MIGRATION\",\"updatedTimeStamp\":\"2022-02-10T19:56:58+08:00\"}],\"attributes\":[{\"id\":6072502,\"customerAttributeCode\":{\"codeValueId\":2466,\"codeValueCd\":\"01\",\"referenceCodeValueCd\":\"01\",\"codeValueDisplay\":\"ACTIVE\",\"attributeValue\":\"3\"},\"statusSource\":\"bmxhUy3nBg86VLGkd46XuprF4JAWgX0jVyPbqTs4lTE9y8kzYJAZyUSDVb5x9XoT5k9Gq8NlNJJs4DXYMOpFs60ZIxg3Z9nHUUbVg6p6j0cYyYINojkK7H9LCX1L8pdntdECAEfhGd9PPFPSOQxuDHRB7n3hIYtehhDCS4VdyeZfiDjXRyXS1S3RXUIftNBbchNmU3WiCy6hAdM05XCEyJpL7nrBerIJ9mQ0VxjiHgcOvsJAbN63PO4atNg3kyi310b79yfF0tIIl2RY8ZOCVmqOsI2O8yFrkNNmMTyHf8O3\",\"version\":1,\"createdBy\":\"FIVUSR\",\"createdByChannel\":\"MIGRATION\",\"createdTimeStamp\":\"2021-03-20T13:11:58+08:00\",\"updatedBy\":\"SEJALAGRAWAL\",\"updatedByChannel\":\"MIGRATION\",\"updatedTimeStamp\":\"2022-02-10T19:56:58+08:00\"}],\"segments\":[{\"id\":2130415,\"segmentCode\":{\"codeValueId\":57990,\"codeValueCd\":\"IN000015\",\"referenceCodeValueCd\":\"IN000015\",\"codeValueDisplay\":\"MASSMKTMUMBAI\"},\"segmentTypeCode\":{\"codeValueId\":2530,\"codeValueCd\":\"01\",\"referenceCodeValueCd\":\"01\",\"codeValueDisplay\":\"SEGMENTLEVEL1\"},\"segmentSubTypeCode\":{\"codeValueId\":2533,\"codeValueCd\":\"1\",\"referenceCodeValueCd\":\"0001\",\"codeValueDisplay\":\"PRIMARY\"},\"tpcIndicator\":\"04\",\"version\":1,\"createdBy\":\"FIVUSR\",\"createdByChannel\":\"MIGRATION\",\"createdTimeStamp\":\"2021-03-20T13:11:58+08:00\",\"updatedBy\":\"SEJALAGRAWAL\",\"updatedByChannel\":\"MIGRATION\",\"updatedTimeStamp\":\"2022-02-10T19:56:58+08:00\"}],\"addresses\":[{\"id\":1097352,\"startDate\":\"2021-03-20\",\"endDate\":\"2099-12-31\",\"addressLabel\":\"ZByfmmxZxKbov7HAvJdaDUxFlSq16JHVZzddscvnFXRgq6CrXV2TifE7J5ozTDpDOSNvvXR5sULFx1Jx3daRLs2YtXdoEKrndPa\",\"addressTypeCode\":{\"codeValueId\":22771,\"codeValueCd\":\"Mailing3\",\"referenceCodeValueCd\":\"Mailing3\",\"codeValueDisplay\":\"Mailing3\"},\"levelNumber\":\"AVH\",\"unitNumber\":\"nfgGZvE\",\"blockNumber\":\"SpbMK\",\"streetName1\":\"F4O0OjnHVJsKcpg1iKZISeRRzMdvHraY6XrJ3vKbNm2Ho3oQ0g1qVTJFkXpzCdlWveh9KtCtyU6FzXayW5xz9WC7AOOEbeYbktD\",\"streetName2\":\"ujIIeZFbf2p2m7ILcBrWgMiUnl83m1nU2E5uJEtU5bBJ0PkyHjBpVFcfjULHA5ipkK1i6cLd3Z8ZIQbaGND79nNAk8BpL6NYcW5\",\"streetName3\":\"EURxm8XAUtAxrzgzpamPHqqxuOFZnfR7kyYbJSvim8Yy8BqAVCagDGYSSRJ5GqcRVjIS2HsnGvkihSQaBLKmxZepaByZpj9Asa0\",\"streetName4\":\"YMTWJHNuQdSJmPrdxPJ6EQAV7bMoaLXvWgJ9Jq5gAU2Q7DDqDvL66dC5hryYloYfPopQ9MKCai3K48pGI6ZnWsxGa8o4847BSqI\",\"streetName5\":\"kRQbqfgW7p7H9SNBbodeSFob3hbPEg8RuIDFoSeLxs1WoYH5dAqqNqMae1LBGTr5AaF87z57GdWLfb7MOOzGNPqDafWGLTkfxfy\",\"streetName6\":\"VKtZoAQhUpOXgpG8a755efmiisZa8oVLhhylRIYp5SOGiat0t6SpEMn4M3GElczlbK6x3xSeI6vdPvrpEL5XTnR3C6J1I00hneX\",\"postalCode\":\"547333\",\"cityCode\":{\"codeValueId\":51305,\"codeValueCd\":\"30805\",\"referenceCodeValueCd\":\"30805\",\"codeValueDisplay\":\"KALYAN\"},\"stateCode\":{\"codeValueId\":57353,\"codeValueCd\":\"MH\",\"referenceCodeValueCd\":\"MH\",\"codeValueDisplay\":\"MAHARASHTRA\"},\"countryCode\":{\"codeValueId\":1959,\"codeValueCd\":\"IN\",\"referenceCodeValueCd\":\"IN\",\"codeValueDisplay\":\"INDIA\"},\"nativeStreetName1\":\"lX64uqsWYkaApbTDqyZPDsKFWBjxOnNjorc7gy4PsQLvPuMvYh6CZk2Ra7pxg0UHdIqsxJOSakxHSEKrOes44syCFs11KVhQBAR\",\"nativeStreetName2\":\"TgARkCahh1mg2ultSQrrS2mlnYDBU6Ndv7E42ECyrdWJLGbuSz3vMn8FnueLTRVzYugRTgsOnb9eUsZJ2GBYQcU30JM8JAEdmQJ\",\"nativeStreetName3\":\"2oqOLOeZpoMzXllzfLN0XryEecPtF3mSzDb9kN9eE5shj7XAoCcZKZPKt0IWd0fF6Se8BCaP5oUqBAZUBn5nxixbB64XBgIpEye\",\"nativeStreetName4\":\"EQA8d49m2gQq5YcH7s7TI8FlJ4q6IEudjILXHC65MIMFcYNTdkKblbUn8yBuCrFUyUuYNjcplMfEVdr4WpCcV5QYvgl987ML1An\",\"preferredFlag\":false,\"holdMailFlag\":false,\"holdMailReason\":\"klvGY0NGpZjxHCrtD9HZGUAmKnQzcGWJiD69Tj9XzpaC6O0vYfqj52UgzcHTNE5pxO2lkHmklYKiRToX0thTkbSS890KpO48mXZ\",\"formatAddress1\":\"BLKSpbMKF4O0OjnHVJsKcpg1iKZISeRRzMdvHr\",\"formatAddress2\":\"ujIIeZFbf2p2m7ILcBrWgMiUnl83m1nU2E5uJEtU5bBJ0PkyHjBpVFcfjULHA5ipkK1i6cLd3Z8ZIQbaGND79nNAk8BpL6NYcW5\",\"formatAddress3\":\"EURxm8XAUtAxrzgzpamPHqqxuOFZnfR7kyYbJSvim8Yy8BqAVCagDGYSSRJ5GqcRVjIS2HsnGvkihSQaBLKmxZepaByZpj9Asa0\",\"formatAddress4\":\"#AVH-nfgGZvESINGAPORE547333\",\"version\":1,\"createdBy\":\"MIGRATION\",\"createdByChannel\":\"MIGRATION\",\"createdByDepartment\":\"\",\"createdTimeStamp\":\"2022-09-17T00:00:00+08:00\",\"updatedBy\":\"MIGRATION\",\"updatedByChannel\":\"MIGRATION\",\"updatedByDepartment\":\"\",\"updatedTimeStamp\":\"2021-03-20T13:11:58+08:00\"},{\"id\":1097353,\"startDate\":\"2021-03-20\",\"endDate\":\"2099-12-31\",\"addressLabel\":\"E153hYLHclTIoYmFgjNeQopHZMBNJgPgxlLynlgr2UuWeOOvC7ivR3HpVymbdAX4nCcKddzlCmYLWpeXZe3HC7MArXIUQKqGZus\",\"addressTypeCode\":{\"codeValueId\":11003,\"codeValueCd\":\"Mailing\",\"referenceCodeValueCd\":\"Mailing\",\"codeValueDisplay\":\"Mailing\"},\"levelNumber\":\"WbV\",\"unitNumber\":\"6uWHOMg\",\"blockNumber\":\"XeO7u\",\"streetName1\":\"KVRhalVhSI00kNx1WylD6NQKKgFYyQCX3hGWoEdr59lm2XGbQJrIELPl74E7prYeIGONP2rcqti5iFrXvIDK7Pu5DxFYQ8Ll1nv\",\"streetName2\":\"BU2IRRjtneN8zUZuyR4mVmvqokCOsC2HNEtNm5NJqUQ90B0FT02bqPrXEMM0ZrVUvD5eZmDnccVofGcYnQDzC0yoBqrRkcvn3mr\",\"streetName3\":\"SSz2LTKpRdLrF5TnKDqk9SFE0biXqN8B4WIdZjLtEuGs49ye17E9IxQ4Zbuz1ZqtyxvX34mkQZyRD1TPmfkQ84LrdGobfoWcVhz\",\"streetName5\":\"ls7MJyXH8dMefc8rhyaR80erKa3YeDFkZXti43dEdXLSJKsMLy5ulM5o60PvizY51U57fXTTlyS4TXY0M9WgsQIzb7HkP5z895l\",\"streetName6\":\"FRlX3HRBHJCHpZPYl7hnmvt3rbNl6Vipe58GqGdDU2hJtr63tTRQv386gHgH78NP8IiSYeSbeNN9sUSy8sadeLEYPHMkpNq72MW\",\"postalCode\":\"902276\",\"cityCode\":{\"codeValueId\":51305,\"codeValueCd\":\"30805\",\"referenceCodeValueCd\":\"30805\",\"codeValueDisplay\":\"KALYAN\"},\"stateCode\":{\"codeValueId\":57353,\"codeValueCd\":\"MH\",\"referenceCodeValueCd\":\"MH\",\"codeValueDisplay\":\"MAHARASHTRA\"},\"countryCode\":{\"codeValueId\":1959,\"codeValueCd\":\"IN\",\"referenceCodeValueCd\":\"IN\",\"codeValueDisplay\":\"INDIA\"},\"nativeStreetName1\":\"I5aL7aCmrjcU1Jn9UM1gFnvYnHBnQOW160aGC4NNO5G11hRpvaU4vxLAYkETEaSQqq2ozttc8GG71vP6fBydoK2A1vyelZ58jrB\",\"nativeStreetName2\":\"dGACNZPlJzFpurukqb0mA4Jr7694Bg6yHl812JuvI2V0OMgz0QLLLD13hpajtIltRsl5ZjMYomm5Zh7ic9TUysfFOl9rsFTVnZo\",\"nativeStreetName3\":\"oiFXxdqVZW8WqX0dEnrtF9FHFnLyH4y6Pg1xU68t4pc0O8HCscd8oJIB8HdGMazN481KII1NNbtMZA3LpMX7E6tVu3DKlCWcEzm\",\"nativeStreetName4\":\"RuBCREzmbjqk0PLViUe1Nu4FdxXf6QKty1XBHzxvJk7i1oC3yBeZO0NxtGxYzdoCrWUKlF2hRujFmZVLaQLzcHx9Z0LdQrC1XLY\",\"preferredFlag\":true,\"holdMailFlag\":false,\"holdMailReason\":\"VhOYu8Mj6bmIhEzvPqKXRu8ntxoI7Wf8EF2iz810dpxR0qN6hN1F59ql9rOCdpjZ6l9yPu1IqQLq5smXUXVlgd50qUL1RArMTOu\",\"formatAddress1\":\"BLKXeO7uKVRhalVhSI00kNx1WylD6NQKKgFYyQ\",\"formatAddress2\":\"BU2IRRjtneN8zUZuyR4mVmvqokCOsC2HNEtNm5NJqUQ90B0FT02bqPrXEMM0ZrVUvD5eZmDnccVofGcYnQDzC0yoBqrRkcvn3mr\",\"formatAddress3\":\"SSz2LTKpRdLrF5TnKDqk9SFE0biXqN8B4WIdZjLtEuGs49ye17E9IxQ4Zbuz1ZqtyxvX34mkQZyRD1TPmfkQ84LrdGobfoWcVhz\",\"formatAddress4\":\"#WbV-6uWHOMgSINGAPORE902276\",\"version\":1,\"createdBy\":\"MIGRATION\",\"createdByChannel\":\"MIGRATION\",\"createdByDepartment\":\"\",\"createdTimeStamp\":\"2022-09-17T00:00:00+08:00\",\"updatedBy\":\"MIGRATION\",\"updatedByChannel\":\"MIGRATION\",\"updatedByDepartment\":\"\",\"updatedTimeStamp\":\"2021-03-30T15:05:39+08:00\"},{\"id\":1097355,\"startDate\":\"2021-03-20\",\"endDate\":\"2099-12-31\",\"addressLabel\":\"Ak5jiECxf10UVAk6CL6aI7LhJjB0dZuRyZeolmpSoxuFAiQjEUobVMb2PxGbeycltO4cneGfKSkgnb8PTkrssSi7GS3dHfLlNF4\",\"addressTypeCode\":{\"codeValueId\":22768,\"codeValueCd\":\"Home\",\"referenceCodeValueCd\":\"Home\",\"codeValueDisplay\":\"Home\"},\"levelNumber\":\"PQg\",\"unitNumber\":\"IWZq3I0\",\"blockNumber\":\"T9xyI\",\"streetName1\":\"1jRYzB6e9n3RSgTC7IyyvmqMYD6ICxFDQ1laMM5BBT1VlK1M7C8uY3hbK7bu84ElgXJzWk673BzDnH9njhfLZvGFROygj0Iq0G6\",\"streetName2\":\"Fba0XQXB9UuOpWIsraBlJCWT9KM1yLCKMgKelrmKPlGVuepE79JV6g5vTQfuig5QjEgYeLthmR2gIcqe25r03DlsWzhcQc6xD3V\",\"streetName3\":\"YiQqp6ton2f0BamZgZ9toJcLNmxqLnEURPIiHNvnX8aN8rUxcnK1vFMrlzj1vQakegvaopqIOfrM6thtP2XShtfNKSEOxzgtkmk\",\"streetName4\":\"uAov9Rd5hUY2W47A88VmmDD2psEjsv0hUJsgBJjpPKUk8pqXZDD6iLjtnjXQmksOOr9mrxBK5TyOkjhlamo315yCspcJsRJ9cx2\",\"streetName5\":\"iX9NUr4fDtvA0difyLgkrGRcBKuIMNe6PPKHBYeKlFEAJ60XEXWCvPstg5Kc7zFAq3eghdKWqVA1TROKaikziAZC03GRlI75Jkt\",\"streetName6\":\"VQsEpT12Yk8xy43KeHouol5qyVY0mX0Ft9UWEl5mQtYf9PsK4U7eqFTvHus0Dnq64uQFrHyos4d8jhnh0RqFvfyUPuNDJavFP1R\",\"postalCode\":\"142353\",\"cityCode\":{\"codeValueId\":51305,\"codeValueCd\":\"30805\",\"referenceCodeValueCd\":\"30805\",\"codeValueDisplay\":\"KALYAN\"},\"stateCode\":{\"codeValueId\":57353,\"codeValueCd\":\"MH\",\"referenceCodeValueCd\":\"MH\",\"codeValueDisplay\":\"MAHARASHTRA\"},\"countryCode\":{\"codeValueId\":1959,\"codeValueCd\":\"IN\",\"referenceCodeValueCd\":\"IN\",\"codeValueDisplay\":\"INDIA\"},\"nativeStreetName1\":\"X9mIGi0n9YYRbRHmBb1AF5hTAhE2sTQg1RRVKW4NUH7Sz9Ihr0GNttABcOolC4IseMJ2qrMsFxibW68bJMdTD61DyOHDhKMOYtf\",\"nativeStreetName2\":\"A0Up4AGRu5P6g5zG90N4M11LqAqiuWgnDzBOraolrrEs3URGp9AqptWJClxUUl1LgUEiNRqk7C0PqPclzk6RTUSsCWYiI3zubAu\",\"nativeStreetName3\":\"mZQQt8jqnltTQGqKmn62PTpidxdREr4lE2tz4rGFolldFNLE2HPh4LihXZzpW4Ept7jqME9DL3eQgPQ3uYu055ikmrQRMuNvCiy\",\"nativeStreetName4\":\"kW36MpncWgPg1OjPYAEeZnOa3yksFDrgQT1CaivF0OYfcWg59qpMRjfQM8vfrWBZycmkRNrYNGosNe8e1ZY3MCT3CrBDGm01GCc\",\"preferredFlag\":false,\"holdMailFlag\":false,\"holdMailReason\":\"Ae10ZQsrnVIfqXxUfFWaUmLvMXBlBMMJldpJGWrcPOyEUFCJ4zArbi3jgHaq4bPSPhFn1m05tpBZm9tzd5RB6Las0XqD2CMP2KX\",\"formatAddress1\":\"BLKT9xyI1jRYzB6e9n3RSgTC7IyyvmqMYD6ICx\",\"formatAddress2\":\"Fba0XQXB9UuOpWIsraBlJCWT9KM1yLCKMgKelrmKPlGVuepE79JV6g5vTQfuig5QjEgYeLthmR2gIcqe25r03DlsWzhcQc6xD3V\",\"formatAddress3\":\"YiQqp6ton2f0BamZgZ9toJcLNmxqLnEURPIiHNvnX8aN8rUxcnK1vFMrlzj1vQakegvaopqIOfrM6thtP2XShtfNKSEOxzgtkmk\",\"formatAddress4\":\"#PQg-IWZq3I0SINGAPORE142353\",\"version\":1,\"createdBy\":\"MIGRATION\",\"createdByChannel\":\"MIGRATION\",\"createdByDepartment\":\"\",\"createdTimeStamp\":\"2022-09-17T00:00:00+08:00\",\"updatedBy\":\"MIGRATION\",\"updatedByChannel\":\"MIGRATION\",\"updatedByDepartment\":\"\",\"updatedTimeStamp\":\"2021-03-20T13:11:58+08:00\"}],\"emails\":[{\"id\":1951478,\"emailAddress\":\"IN1I2200Z52FL@dbstest.com\",\"emailTypeCode\":{\"codeValueId\":23090,\"codeValueCd\":\"51\",\"referenceCodeValueCd\":\"51\",\"codeValueDisplay\":\"OFFICIAL\"},\"emailStatusCode\":{\"codeValueId\":902,\"codeValueCd\":\"V\",\"referenceCodeValueCd\":\"V\",\"codeValueDisplay\":\"VERIFIED\"},\"preferredFlag\":true,\"version\":1,\"createdBy\":\"MIGRATION\",\"createdByChannel\":\"MIGRATION\",\"createdTimeStamp\":\"2022-09-17T00:00:00+08:00\",\"updatedBy\":\"MIGRATION\",\"updatedByChannel\":\"MIGRATION\",\"updatedTimeStamp\":\"2021-03-30T15:05:40+08:00\"}],\"contacts\":[{\"id\":130347,\"contactTypeCode\":{\"codeValueId\":899,\"codeValueCd\":\"4\",\"referenceCodeValueCd\":\"4\",\"codeValueDisplay\":\"HANDPHONE\"},\"contactSuffix\":\"00\",\"contactPersonName\":\"mGsp6eu6bTayXRuQ4eF9IVpqrTl75fK5Rdb2SfZzHKZVJhVIm6BTXIavNNH5u0RfmYOePzl5mLyGCyzA6clfjI9UzCnIPcBSyCkVZPhZbGVHQVlRzjv3eykIg9ejfUAFIQGMlxZjJTt6FAUMOIT2yy6yK6jOLxZKNVNhIuYIn2u5hk3Eqjchn7DvWClXdKRN0oC7qcQH\",\"contactPhoneNumber\":\"378936910\",\"localNumber\":\"280718784\",\"countryCode\":{\"codeValueId\":1539,\"codeValueCd\":\"91\",\"referenceCodeValueCd\":\"91\",\"codeValueDisplay\":\"INDIA\"},\"preferredFlag\":true,\"version\":1,\"createdBy\":\"MIGRATION\",\"createdByChannel\":\"MIGRATION\",\"createdTimeStamp\":\"2022-09-17T00:00:00+08:00\",\"updatedBy\":\"MIGRATION\",\"updatedByChannel\":\"MIGRATION\",\"updatedTimeStamp\":\"2021-03-30T15:05:40+08:00\"}],\"nationalities\":[{\"id\":934479,\"nationalityCode\":{\"codeValueId\":57046,\"codeValueCd\":\"IN\",\"referenceCodeValueCd\":\"IN\",\"codeValueDisplay\":\"INDIA\"},\"version\":1,\"createdBy\":\"MIGRATION\",\"createdByChannel\":\"MIGRATION\",\"createdTimeStamp\":\"2021-03-30T15:05:40+08:00\",\"updatedBy\":\"MIGRATION\",\"updatedByChannel\":\"MIGRATION\",\"updatedTimeStamp\":\"2021-03-30T15:05:40+08:00\"}],\"educationDetails\":[],\"employmentDetails\":[{\"id\":3402614,\"employerName\":\"LY1vfUFhAubjUzVJyfb5ISnRdzTN3ZoG6x0TqUsgyiNEHt7hAXFn0skdGYinAe7ZEEat7rJOjdOKxWhT0FLVrED0JvPf2hbpbRKyBMi0Ir22ieenyfpeUKsvL3fq1i8Fz5lLhXQUggHmoAK4b01A5CAVrbxooboii\",\"employerNameInAlternateLanguage\":\"ecY2J3Oyg5cAQhNKqppyjAjWmIS3tZr0cZrDnXC8thAGnT0z1SUJHk5Blytz2CnidJeW2m9krgJupML3JNC2mXdmY2hj5pSXK2m1lQkLYWJJ49bDY2Em7PKsKGaGVtI2gC9YeyPPSY7gFNgIDxbhEd5JALSvSaNzqzDdI02HIgeiOnh51hgGdDaagPV5XRQf1mg6QljPIvvHansjz2U4THzvo06KZAZmbx4UkH0OSr7jE81Z5ytRQfeYSeXjR7V\",\"addressLine1\":\"AMKQgMD7Me3sRzO9UWqFsL7fU6gaJKIxKC1LGIKB2mK31tXGRzFHBVtExfmsbQtCfvAx2IYjTeSLtoTU9BCjk2pCKCVSJleEsJZCVYeRCjaG8pu8caWAmpuqNUEeK4hYz05e9CnpIKcDyOJfAYfMStWq2X9mvWmU3d4gSa8pn7ytkVTxe3N9Hu2kR5umXpfjdgAttgOHxQ6dN9RmdLv3uzg3V5qnOfVnXZh8E9728JnuAkhpTb1D12uRUcyoff2\",\"addressLine2\":\"zTWIjB7ETHYM8GiFYE20J382gNmTe8gMHoj0PihO3UMZvBCu0KAjdaDKvcLUZyFJaSoqGpl8KsfmG0N2hmOJoloZp39nIHNPFPvqKxXUOblCYsWriYHoiKGrgjnNU8GQ5SHjMFV4lkOdnAAYSCJnbmv4U6b5EeCSFa2bD2BSp20Jh3oP8mqBOO0yDR2vjcQqBxvT1d0brSCfTvvoVk16FEearxZzLtjM6N3JEriR0jq1d0TVFdjdUko7yXgYYQG\",\"postalCode\":\"GBygB43055\",\"cityCode\":{\"codeValueId\":51140,\"codeValueCd\":\".\",\"referenceCodeValueCd\":\".\",\"codeValueDisplay\":\".\"},\"stateCode\":{\"codeValueId\":57304,\"codeValueCd\":\".\",\"referenceCodeValueCd\":\".\",\"codeValueDisplay\":\".\"},\"phoneNumber\":\"lc5L4ccRuqpLdZypU3Me\",\"phoneAreaNumber\":\"G9ABYCAtIjzWNCLnoI28\",\"faxNumber\":\"VlfZhGeCfhzqob27NLemyOgAaPial3\",\"incomeRangeFrom\":\"1500000\",\"incomeRangeTo\":\"2999999\",\"employmentStatusCode\":{\"codeValueId\":881,\"codeValueCd\":\"02\",\"referenceCodeValueCd\":\"02\",\"codeValueDisplay\":\"PERMANENTEMPLOYED-FULLTIME\"},\"sourceOfIncomeCode\":{\"codeValueId\":23107,\"codeValueCd\":\"003\",\"referenceCodeValueCd\":\"003\",\"codeValueDisplay\":\"Inheritance\"},\"occupationalGroupCode\":{\"codeValueId\":14495,\"codeValueCd\":\"13\",\"referenceCodeValueCd\":\"13\",\"codeValueDisplay\":\"SERVICE–PRIVATESECTOR\"},\"version\":1,\"createdBy\":\"FIVUSR\",\"createdByChannel\":\"MIGRATION\",\"createdTimeStamp\":\"2021-03-20T13:11:58+08:00\",\"updatedBy\":\"SEJALAGRAWAL\",\"updatedByChannel\":\"MIGRATION\",\"updatedTimeStamp\":\"2021-03-30T15:05:40+08:00\"}],\"relationshipManagers\":[{\"id\":1463044,\"ownerId\":831384,\"officerIncharge\":\"Bj2uzCjkIRf7QI8F0P3\",\"order\":1,\"version\":1,\"createdBy\":\"FIVUSR\",\"createdByChannel\":\"MIGRATION\",\"createdTimeStamp\":\"2021-03-20T13:11:58+08:00\",\"updatedBy\":\"SEJALAGRAWAL\",\"updatedByChannel\":\"MIGRATION\",\"updatedTimeStamp\":\"2022-02-10T19:56:58+08:00\"}]},\"status\":{\"statusCode\":0},\"traceInfo\":{\"traceId\":\"83bb2940-1f37-11eb-adc1-0242ac120009\",\"spanId\":\"d69a1b3d67d757df\",\"timestamp\":\"2024-03-15T15:26:59.906126+08:00\"}}";
        }

        private string getAccountDetailsDBSResponseTest()
        {
            string sResposne = "";

            sResposne =
            "{ " +
                "\"sourceAccountNumber\": \"857010068475\"," +
                "\"ibanAccountNumber\": \"\"," +
                "\"sourceSystemId\": \"\"," +
                "\"sourceCustomerId\": \"24298597\"," +
                "\"productType\": \"SBA\"," +
                "\"productTypeDescription\": []," +
                "\"subProductType\": \"\"," +
                "\"productCode\": \"NROT3\"," +
                "\"productCodeDescription\": [ { \"productCodeDescription\": \"DIGISAVINGS\"," +
                    "                              \"languageCode\": \"INFENG\"         } ]," +
                //"\"productName\": \"NA\"," +
                //"\"productName\": \"ODACC\"," +
                "\"productName\": \"TREPB\"," +
                "\"nativeProductName\": \"DBSBA\"," +
                "\"accountCurrency\": \"INR\"," +
                "\"openedDate\": 1656374400000," +
                "\"modeOfOperation\": \"0003\"," +
                "\"serviceChargeExemption\": 0," +
                "\"staffIndicator\": false," +
                "\"officerId\": \"\"," +
                "\"officerUnit\": \"\"," +
                "\"firstExcessDate\": 0," +
                "\"lastUpdatedEvent\": \"\"," +
                "\"returnChequeDetailsInfo\": []," +
                "\"accountStatus\": \"Active\"," +
                "\"accountStatusCode\": \"A\"," +
                "\"accountSignatoryType\": \"01\"," +
                "\"accountSignal\": \"\"," +
                "\"loanServicingIndicator\": false," +
                "\"accountFrozenIndicator\": true," +
                "\"noDebitIndicator\": false," +
                "\"debitReferralIndicator\": false," +
                "\"irregularSignalIndicator\": false," +
                "\"lineOfferedIndicator\": false," +
                "\"closureNoticeIndicator\": false," +
                "\"multipleAccountIndicator\": false," +
                "\"recallPassbookIndicator\": false," +
                "\"updateRequiredIndicator\": false," +
                "\"productIndicator\": \"\"," +
                "\"brandIndicator\": \"\"," +
                "\"ibanIndicator\": \"N\"," +
                "\"virtualAccountIndicator\": \"N\"," +
                "\"spclCustomerType\": \"\"," +
                "\"accountType\": \"\"," +
                "\"accountTypeDescription\": \"\"," +
                "\"currencyDecimal\": 2," +
                "\"generalLedgerSubHeadCode\": \"21201\"," +
                "\"accountCurrencyCode\": \"INR\"," +
                "\"odLimitType\": \"\"," +
                "\"odInterestAmount\": 0," +
                "\"accountName\": \"XXX XXXXX XXXXXXX XXXXXXX\"," +
                "\"accountShortName\": \"XXX XXXXX\"," +
                "\"virtualAccountName\": \"\"," +
                "\"accountStatement\": { \"statementMode\": \"S\"," +
                                          "  \"statementCalendar\": \"00\"," +
                                          "  \"frequency\": \"M\"," +
                                          "  \"frequencyStartDate\": 31," +
                                          "  \"frequencyDay\": 0," +
                                          "  \"frequencyWeekNumber\": 0," +
                                          "  \"frequencyHolidayStatus\": \"N\"," +
                                          "  \"nextPrintDate\": 1546214400000," +
                                          "  \"despatchMode\": \"N\" " +
                                          "  }" +
                                          " ," +
                "\"balanceDebitCreditIndicator\": \"C\"," +
                "\"freezeCode\": \"\"," +
                "\"freezeStatusCode\": \"\"," +
                "\"freezeReasonCode\": \"\"," +
                "\"freezeReasonCode1\": \"\"," +
                "\"additionalFreezeReasonCodes\": []," +
                "\"additionalFreezeRemarks\": []," +
                "\"freezeReasonCodeDescriptionList\": []," +
                "\"freezeRemarks\": \"\"," +
                "\"freezeRemarks1\": \"\"," +
                "\"accountInterest\": { \"interestRate\": 3.25," +
                    "                     \"interestCalInterest\": 0," +
                    "                     \"interestFrequencyType\": \"M\"," +
                    "                     \"interestFrequencyStartDate\": 31," +
                    "                     \"interestFrequencyDay\": 0," +
                    "                     \"interestFrequencyWeekNum\": 0," +
                    "                     \"accountInterestFrequencyHolidayStatus\": \"P\"," +
                    "                     \"interestRateCode\": \"DBSB1\"," +
                    "                     \"netInterestRate\": 0," +
                    "                     \"netInterestDebitCreditIndicator\": \"C\"," +
                    "                     \"accruedInterestDebitCreditIndicator\": \"C\"," +
                    "                     \"unpaidInterestFee\": 0," +
                    "                     \"bookedamount\": 0," +
                    "                     \"interestAmount\": 0," +
                    "                     \"preferentialInterest\": 0" +
                    "                   }" +
                    "," +
                "\"taxCategory\": \"A\"," +
                "\"taxFloorLimit\": 0," +
                "\"taxFloorLimitCurrencyCode\": \"INR\"," +
                "\"withholdingTaxPercent\": 0," +
                "\"gstin\": \"\"," +
                "\"gstExemptionFlag\": \"\"," +
                "\"nickName\": \"\"," +
                "\"productShortName\": \"DBSBA\"," +
                "\"preferredLanguageProductShortName\": \"DBSBA\"," +
                "\"sourceMultiCurrencyAccountNumber\": \"\"," +
                "\"multiCurrencyAccountFlag\": false," +
                "\"branchCode\": \"811\"," +
                "\"branchCodeDescription\": \"\"," +
                "\"bankCode\": \"DBSIN\"," +
                //"\"accountClosedFlag\": \"N\"," +
                "\"accountClosedFlag\": \"N\"," +
                "\"accountClosedReasonCode\": \"\"," +
                "\"accountClosedRemarks\": \"\"," +
                "\"accountClosedDate\": 0," +
                "\"lastBalanceUpdateDateTime\": 0," +
                "\"earmarkUpdateDateTime\": 0," +
                "\"holdBalanceUpdateDateTime\": 0," +
                "\"sanctionLimitUpdateDateTime\": 0," +
                "\"staticDataUpdateDateTime\": 1656516121000," +
                "\"halfDayHoldBalanceExpiryDate\": 0," +
                "\"childAccounts\": []," +
                "\"accountBalances\": 	{" +
                        "				   \"availableBalance\": -374," +
                        "				   \"availableBalanceCurrencyCode\": \"INR\"," +
                        "				   \"accountBalance\": 126," +
                        "				   \"accountBalanceCurrencyCode\": \"INR\"," +
                        "				   \"sanctionLimit\": 0," +
                        "				   \"sanctionLimitCurrencyCode\": \"INR\"," +
                        "				   \"ledgerBalance\": 126," +
                        "				   \"ledgerBalanceCurrencyCode\": \"INR\"," +
                        "				   \"halfDayHoldBalance\": 0," +
                        "				   \"oneDayHoldBalance\": 0," +
                        "				   \"twoDayHoldBalance\": 0," +
                        "				   \"earmarkDebitAmount\": 0," +
                        "				   \"earmarkCreditAmount\": 500," +
                        "				   \"floatAmount\": 0," +
                        "				   \"earmarkAmount\": 500," +
                        "				   \"effectiveAvailableAmount\": 0," +
                        "				   \"drawingPower\": 0," +
                        "				   \"overDueLiableAmount\": 0," +
                        "				   \"openingBalanceAmount\": 0," +
                        "				   \"closingBalanceAmount\": 0," +
                        "				   \"fundsClearingAmount\": 0," +
                        "				   \"cumulativeCreditAmount\": 35170," +
                        "				   \"cumulativeDebitAmount\": 35044," +
                        "				   \"utilizedAmount\": 0," +
                        "				   \"systemReservedAmount\": 0," +
                        "				   \"overdueFutureAmount\": 0," +
                        "				   \"utilizedFutureAmount\": 0," +
                        "				   \"effectiveFutureAvailableAmount\": 0," +
                        "				   \"availableAmountLineOfCredit\": 0," +
                        "				   \"unclearDrawingAmount\": 0," +
                        "				   \"ffdAvailableAmount\": 0," +
                        "				   \"sweepsEffectiveAvailableAmount\": 0," +
                        "				   \"hcAvailableAmount\": 0," +
                        "				   \"futureAmount\": 0," +
                        "				   \"futureCreditAmount\": 0," +
                        "				   \"futureClearBalanceAmount\": 0," +
                        "				   \"futureUnclearBalanceAmount\": 0," +
                        "				   \"daccLimit\": 0," +
                        "				   \"dafaLimit\": 0 " +
                        "					}," +
                        "\"relatedCustomerInfo\": [" +
                        "                            { " +
                        "                            \"relatedPartyCode\": \"\"," +
                        "                            \"relatedPartyCodeDescription\": \"\"," +
                        "                            \"relatedPartyCustomerId\": \"24298597\"," +
                        "                            \"relatedPartySourceCustomerId\": \"\"," +
                        "                            \"relatedPartyType\": \"M\"," +
                        "                            \"relatedPartyTypeDescription\": \"Main Holder\"," +
                        "                            \"relatedPartyDeleteFlag\": \"N\"," +
                        "                            \"relatedPartyAddressType\": \"Mailing\"" +
                        "                            }" +
                        //"                            { " +
                        //"                            \"relatedPartyCode\": \"\"," +
                        //"                            \"relatedPartyCodeDescription\": \"\"," +
                        //"                            \"relatedPartyCustomerId\": \"22945769\"," +
                        //"                            \"relatedPartySourceCustomerId\": \"\"," +
                        //"                            \"relatedPartyType\": \"M\"," +
                        //"                            \"relatedPartyTypeDescription\": \"2nd Holder\"," +
                        //"                            \"relatedPartyDeleteFlag\": \"N\"," +
                        //"                            \"relatedPartyAddressType\": \"Mailing\"" +
                        //"                            }" +
                        "                      ]," +
                        "\"promoCode\": []," +
                        "\"mobileMoneyIdentifier\": \"9641689\"," +
                        "\"mobileNumbers\": [ \"7358386665\" ]," +
                        "\"reference1\": \"\"," +
                        "\"reference2\": \"\"," +
                        "\"ifscCode\": \"DBSS0IN0811\"," +
                        "\"channelId\": \"SOI\"," +
                        "\"accountStatusDate\": 1623456000000," +
                        "\"expressAccountExpiryDate\": 0," +
                        "\"schemeConversionDate\": 0," +
                        "\"virtualAccountType\": \"\"," +
                        "\"faxIndeminity\": \"\"," +
                        "\"nomineeAvailableFlag\": \"N\"," +
                        "\"nomineeGuardianInfo\": []" +
                "}   ";



            string jsonResponse = @"
                                    {
                                      ""error"": {
                                        ""timestamp"": 1733730385113,
                                        ""status"": ""INTERNAL_SERVER_ERROR"",
                                        ""errorType"": ""BE"",
                                        ""errorCode"": ""MSTD-CD-5008"",
                                        ""errorMessage"": ""Finacle Business Error"",
                                        ""methodName"": """",
                                        ""restError"": [
                                          {
                                            ""errorCode"": ""W025"",
                                            ""errorMessage"": ""Invalid Account Details ""
                                          }
                                        ]
                                      },
                                      ""timestamp"": 1733730385113,
                                      ""status"": ""INTERNAL_SERVER_ERROR"",
                                      ""errorType"": ""BE"",
                                      ""errorCode"": ""MSTD-CD-5008"",
                                      ""errorDescription"": ""Finacle Business Error"",
                                      ""methodName"": """",
                                      ""restError"": [
                                        {
                                          ""errorCode"": ""W025"",
                                          ""errorDescription"": ""Invalid Account Details ""
                                        }
                                      ]
                                    }";


            return sResposne;

        }

        string GetCMCPCustomerNameTest()
        {
            //string sCustomerName = "89SdOhEnyHC5xJjOiHua4ZpiC02WBGKJFG0OyzBJfZsUUlKtev3XZMyjS4SCmjMoRPDMsjRnVs0M9dET4gtyiobeTK45TyKyrPsSAQamW5OXsdCnJd93ypKLJRZDbKknhEB0vKEQHOfYB463sMfdLmhMyIuOvVL";
            string sCustomerName = "i6dzbff29ch5uDkqCO7JxxLVin6jW1tk1qBePUbVK35n2yiiI6If4Op7dxJpjAblBvMpARqqGB6fObMBRnFdDP16h60VVSZ9xWK";

            try
            {

                //var jObject = Newtonsoft.Json.Linq.JObject.Parse(sResposne);
                //if (jObject["data"]["profileInfo"]["registeredName"] != null)
                //{
                //    sCustomerName = jObject["data"]["profileInfo"]["registeredName"].ToString().Trim();
                //}
                //else
                //{
                //    //WriteState code to log error
                //    logerror(jObject["errorDescription"].ToString(), "");
                //}
                return sCustomerName;
            }
            catch (Exception Ex)
            {
                logerrorInCatch("Exception in GetCMCPCustomerName ", Ex.Message.ToString());
                return sCustomerName;
            }

        }

        //============ Amol changes for checking sortcode in BankBranches master on 01/06/2024 start =============

        [HttpPost]
        public ActionResult IsSortCodeExistInBankBranches(string sortcode = null)
        {
            bool OutputData = false;

            try
            {
                con.Open();
                SqlDataAdapter adp = new SqlDataAdapter("IsSortCodeExistInBankBranches", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@SortCode", SqlDbType.VarChar).Value = sortcode;

                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(ds.Tables[0].Rows[0]["SortCode"].ToString()) > 0)
                    {
                        OutputData = true;
                    }

                }

            }
            catch (Exception e)
            {
                logerrorInCatch(e.Message, e.Message.ToString() + " - > In Catch block message");
                logerrorInCatch(e.InnerException.ToString(), e.InnerException.ToString() + " - > In Catch block InnerException");
            }

            return Json(OutputData, JsonRequestBehavior.AllowGet);
        }
        //============ Amol changes for checking sortcode in BankBranches master on 01/06/2024 end =============
    }
}
