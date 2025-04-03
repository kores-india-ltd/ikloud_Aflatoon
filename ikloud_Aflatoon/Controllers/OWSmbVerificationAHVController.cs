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
    //[OutputCache(Duration = 0)]
    public class OWSmbVerificationAHVController : Controller
    {
        //
        // GET: /OWL2/
        private static Logger logger = LogManager.GetCurrentClassLogger();
        AflatoonEntities af = new AflatoonEntities();
        //OWProcDataContext OWpro = new OWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);

        public ActionResult SelectionForBranchCode(int id = 0)
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

            SelectionForBranchCode selectionForBranchCode = new SelectionForBranchCode();
            try
            {
                if (Session["uid"] != null)
                {
                    //int uid = (int)Session["uid"];
                    int custid = Convert.ToInt16(Session["CustomerID"]);
                    Session["VerificationId"] = id;
                    var owVeriEnableBranch = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "OWVerificationEnableBranchCodeSelection")?.SettingValue;

                    if (owVeriEnableBranch == null || owVeriEnableBranch == "")
                    {
                        ViewBag.OWVeriEnableBranch = "N";
                        return RedirectToAction("Index", "OWSmbVerificationAHV", new { id = id });
                    }
                    else
                    {
                        if(owVeriEnableBranch == "Y")
                        {
                            ViewBag.OWVeriEnableBranch = "Y";
                            if (Session["ProType"].ToString() == "Outward")
                            {
                                string customerId = Session["CustomerID"].ToString();
                                string domainId = Session["DomainselectID"].ToString();
                                string domainName = Session["SelectdDomainName"].ToString();
                                string domainId1 = Session["domainid"].ToString();
                                string domainName1 = Session["domainname"].ToString();
                                string processingDate = Session["processdate"].ToString();
                                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                                string procDate = processingDate1.ToString("yyyy-MM-dd");

                                
                            }
                        }
                        else
                        {
                            ViewBag.OWVeriEnableBranch = "N";
                            return RedirectToAction("Index", "OWSmbVerificationAHV", new { id = id });
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
                logerror(e.Message, e.InnerException.ToString());

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL2 Selection for branch code - " + innerExcp });
            }
            return View(selectionForBranchCode);
        }

        public JsonResult SelectBranchCodes()
        {
            try
            {
                int uid = (int)Session["uid"];
                int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());

                int customerId = Convert.ToInt32(Session["CustomerID"].ToString());
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);
                int verificationId = Convert.ToInt16(Session["VerificationId"].ToString());

                if(verificationId == 5)
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
                logerror(e.Message, e.InnerException.ToString());

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
            int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());

            int customerId = Convert.ToInt32(Session["CustomerID"].ToString());
            DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
            string procDate = processingDate1.ToString("yyyy-MM-dd");
            var xyz = Convert.ToDateTime(procDate);

            SqlDataAdapter adp = new SqlDataAdapter("OW_Verification_SelectBranchCodeList_AHV", con);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
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

            int verificationId = Convert.ToInt16(Session["VerificationId"].ToString());
            if (Request.Form["BranchSelect"] != null)
                Session["BranchID"] = Request.Form["BranchSelect"].ToString();
            try
            {
                var branch = Session["BranchID"].ToString();
                return RedirectToAction("Index", "OWSmbVerificationAHV", new { id = verificationId, branchId = branch });
            }
            catch(Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "Selection for branch code post - " + innerExcp });
            }
        }

        public ActionResult Index(int id = 0, string branchId = null)
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

                SqlDataAdapter adp = new SqlDataAdapter("[OWSelectSMBL2_AHV]", con);
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
                        if(id == 5)
                        {
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
                                ChqDate = "",
                                ChqAmt = "",
                                ChqAcno = "",
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
                        }
                        else
                        {
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

                            //logerror("OWSMBVerificationCOntroller", "executed SMBVerificationView");

                            

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
                        }


                        ViewBag.FrontGrey = ds.Tables[0].Rows[0]["FrontGreyImagePath"].ToString() == "" ? "" : ds.Tables[0].Rows[0]["FrontGreyImagePath"].ToString();
                        ViewBag.FrontTiff = ds.Tables[0].Rows[0]["FrontTiffImagePath"].ToString() == "" ? "" : ds.Tables[0].Rows[0]["FrontTiffImagePath"].ToString();
                        ViewBag.BackGrey = ds.Tables[0].Rows[0]["BackGreyImagePath"].ToString() == "" ? "" : ds.Tables[0].Rows[0]["BackGreyImagePath"].ToString();
                        ViewBag.BackTiff = ds.Tables[0].Rows[0]["BackTiffImagePath"].ToString() == "" ? "" : ds.Tables[0].Rows[0]["BackTiffImagePath"].ToString();
                        ViewBag.FrontUV = ds.Tables[0].Rows[0]["FrontUVImage"].ToString() == "" ? "" : ds.Tables[0].Rows[0]["FrontUVImage"].ToString();

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
                            if(id == 5)
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
                logerror(e.Message, e.InnerException.ToString());

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL2 HttpGet Index- " + innerExcp });
            }

            return RedirectToAction("IWIndex", "Home", new { id = 4 });
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
            int uid = (int)Session["uid"];//That will be Session value.
            int ttcnt = 0;
            try
            {
                if (btnSubmit == "Close")
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
                double camt = Convert.ToDouble(Request.Form["ChqAmt"]);

                string cdate = Convert.ToString(Request.Form["ChqDate"]);
                string cdatenew = "";
                if (Session["OWIsDataEntryAllowedForDate"].ToString().ToUpper() == "Y")
                {
                    cdatenew = "20" + cdate.Substring(4, 2) + "-" +
                    cdate.Substring(2, 2) + "-" +
                    cdate.Substring(0, 2);
                }
                else
                {

                }
                    

                //DateTime dt = DateTime.ParseExact(cdatenew, "yyyy-MM-dd", null);

                string cacno = Convert.ToString(Request.Form["ChqAcno"]);
                string cpayee = Convert.ToString(Request.Form["ChqPayeeName"]);
                string cfinalchqno = Convert.ToString(Request.Form["FinalChqNo"]);
                string cfianlsortcode = Convert.ToString(Request.Form["FinalSortcode"]);
                string cfinalsan = Convert.ToString(Request.Form["FinalSAN"]);
                string cfialtrcd = Convert.ToString(Request.Form["FinalTransCode"]);
                //string cDraweeName = Convert.ToString(Request.Form["DraweeName"]);

                int cNRESourceOfFundId = 0, cNROSourceOfFundId = 0;

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

                if (Decision.ToUpper() == "A")
                {
                    if(Modified == true)
                    {
                        modaction = "M";
                    }
                    else
                    {
                        modaction = "A";
                    }
                }
                else if(Decision.ToUpper() == "R")
                {
                    modaction = "R";
                }
                else if(Decision.ToUpper() == "H")
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
                if(Session["BranchID"] != null)
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
                        //        "",//userNarration, 
                        //        rejectreasondescrpsn,//rejectreasondescrpsn, 
                        //       Convert.ToString(Session["CtsSessionType"]),
                        //        "",//@CBSAccountInformation
                        //        "",//@CBSJointAccountInformation 
                        //        ignoreiqa,//@IgnoreIQA, 
                        //        doctype,//@DocType 
                        //        modified,//@Modified, 
                        //        null,
                        //        //,
                        //        "",
                        //        cNRESourceOfFundId,
                        //        cNROSourceOfFundId);//@strHoldReason

                        SqlCommand cmd = new SqlCommand("UpdateSMBVerificationL1_AHV", con);
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
                        cmd.Parameters.AddWithValue("@UserNarration", "");
                        cmd.Parameters.AddWithValue("@RejectReasonDescription", rejectreasondescrpsn);
                        cmd.Parameters.AddWithValue("@CTSNONCTS", Convert.ToString(Session["CtsSessionType"]));
                        cmd.Parameters.AddWithValue("@CBSAccountInformation", "");
                        cmd.Parameters.AddWithValue("@CBSJointAccountInformation", "");
                        cmd.Parameters.AddWithValue("@IgnoreIQA", ignoreiqa);
                        cmd.Parameters.AddWithValue("@DocType", doctype);
                        cmd.Parameters.AddWithValue("@Modified", modified);
                        cmd.Parameters.AddWithValue("@strHoldReason", "");
                        cmd.Parameters.AddWithValue("@DraweeName", "");
                        cmd.Parameters.AddWithValue("@NRESourceOfFundId", cNRESourceOfFundId);
                        cmd.Parameters.AddWithValue("@NROSourceOfFundId", cNROSourceOfFundId);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                        //logerror("Finished Update L1 query", "Finished Update L1 query" + " - > IN Ok L1 verification value Finished Update L1 query");
                        //return RedirectToAction("Index", "OWSmbVerification", new { id = Session["VFTypeID"] });
                        return RedirectToAction("Index", "OWSmbVerificationAHV", new { id = verificationId, branchId = branch });
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
                        if(L2Status == "8" || L2Status == "9")
                        {
                            if(modaction == "A" || modaction == "M")
                            {
                                modifiedAction = "2";
                            }
                            else if(modaction == "R")
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
                                else if(modaction == "R")
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
                        //        //,
                        //        "",
                        //        cNRESourceOfFundId,
                        //        cNROSourceOfFundId);//@strHoldReason

                        SqlCommand cmd = new SqlCommand("UpdateSMBVerification_AHV", con);
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
                        cmd.Parameters.AddWithValue("@ActionTaken", modifiedAction);
                        cmd.Parameters.AddWithValue("@LName", @Session["LoginID"].ToString());
                        cmd.Parameters.AddWithValue("@ProcessingDate", Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@CustomerId", smbver.CustomerId);
                        cmd.Parameters.AddWithValue("@DomainId", smbver.DomainId);
                        cmd.Parameters.AddWithValue("@ScanningNodeId", smbver.ScanningNodeId);
                        cmd.Parameters.AddWithValue("@ChequeAmtotal", 0);
                        cmd.Parameters.AddWithValue("@SlipAmount", 0);
                        cmd.Parameters.AddWithValue("@ChequeTotal", 0);
                        cmd.Parameters.AddWithValue("@UserNarration", "");
                        cmd.Parameters.AddWithValue("@RejectReasonDescription", rejectreasondescrpsn);
                        cmd.Parameters.AddWithValue("@CTSNONCTS", Convert.ToString(Session["CtsSessionType"]));
                        cmd.Parameters.AddWithValue("@CBSAccountInformation", "");
                        cmd.Parameters.AddWithValue("@CBSJointAccountInformation", "");
                        cmd.Parameters.AddWithValue("@IgnoreIQA", ignoreiqa);
                        cmd.Parameters.AddWithValue("@DocType", doctype);
                        cmd.Parameters.AddWithValue("@Modified", modified);
                        cmd.Parameters.AddWithValue("@strHoldReason", "");
                        cmd.Parameters.AddWithValue("@DraweeName", "");
                        cmd.Parameters.AddWithValue("@NRESourceOfFundId", cNRESourceOfFundId);
                        cmd.Parameters.AddWithValue("@NROSourceOfFundId", cNROSourceOfFundId);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();


                        //logerror("Finished Update L2 query", "Finished Update L2 query" + " - > IN Ok L2 verification value Finished Update L2 query");
                        //return RedirectToAction("Index", "OWSmbVerification", new { id = Session["VFTypeID"] });
                        return RedirectToAction("Index", "OWSmbVerificationAHV", new { id = verificationId, branchId = branch });
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

                logerror(e.Message, e.InnerException.ToString());

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
    }
}
