using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ikloud_Aflatoon.Infrastructure;
using ikloud_Aflatoon.Models;
using NLog;

namespace ikloud_Aflatoon.Controllers
{
    public class OWBranchLevelDataEntryController : Controller
    {
        //
        // GET: /OWBranchLevelDataEntry/
        private static Logger logger = LogManager.GetCurrentClassLogger();
        AflatoonEntities af = new AflatoonEntities();
        private AflatoonEntities udb = new AflatoonEntities();
        //OWProcDataContext OWpro = new OWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SelectionForBranchDataEntry()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }

            if ((bool)Session["DE"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            SelectionForBranchDataEntry selectionForBranchDataEntry = new SelectionForBranchDataEntry();
            try
            {
                if (Session["uid"] != null)
                {
                    int uid = (int)Session["uid"];
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
            }
            catch(Exception e)
            {

            }
            return View(selectionForBranchDataEntry);
        }

        public JsonResult UpdateBDE_StatusForBatch()
        {
            try
            {
                var batch = Session["BatchID"].ToString();
                Int64 batchID = Convert.ToInt64(batch);
                int uid = (int)Session["uid"];

                //var result = (from p in udb.BatchMaster
                //              where p.Id == batchID
                //              select p).SingleOrDefault();
                //result.Branch_DE_Status = "C";
                //result.Lock_UserId = uid;
                //result.Status = 20;
                //udb.SaveChanges();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "OW_BDE_UpdateBDE_StatusForBatch";
                cmd.Parameters.Add("@BatchID", SqlDbType.BigInt).Value = batchID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = uid;
                
                cmd.Connection = con;
                con.Open();

                cmd.ExecuteNonQuery();
                con.Close();
                con.Dispose();
                return Json("Suceess", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateWithAddListFileData(Int64 id = 0, string Account = null, decimal Amount = 0, string ChequeNo = null, string PresentmentDate = null, string PresentingMICR = null, string PayeeName = null, string TransCode = null, string SAN = null)
        {
            try
            {
                int uid = (int)Session["uid"];

                //var result = (from p in udb.CaptureRawData
                //              where p.Id == id
                //              select p).SingleOrDefault();
                //result.BranchAccount = branchAccount;
                //result.BranchAmount = branchAmount;
                //result.BranchDE_Status = "C";
                //result.Lock_UserId = uid;
                //udb.SaveChanges();
                string cdatenew = "";
                string newDate = "";
                if (Session["OWIsDataEntryAllowedForDate"].ToString().ToUpper() == "Y")
                {
                    newDate = PresentmentDate.Substring(4, 4) + "-" +
                    PresentmentDate.Substring(2, 2) + "-" +
                    PresentmentDate.Substring(0, 2);
                }
                else
                {
                    newDate = DateTime.Now.ToString("yyyy-MM-dd");
                }
                    

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "OW_AddList_UpdateWithAddListFileData";
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = uid;
                cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = id;
                cmd.Parameters.Add("@Account", SqlDbType.NVarChar).Value = Account;
                cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Amount;
                cmd.Parameters.Add("@ChequeNo", SqlDbType.NVarChar).Value = ChequeNo;
                cmd.Parameters.Add("@PresentmentDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(newDate).ToString("yyyy-MM-dd");
                cmd.Parameters.Add("@PresentingMICR", SqlDbType.NVarChar).Value = PresentingMICR;
                cmd.Parameters.Add("@PayeeName", SqlDbType.NVarChar).Value = PayeeName;
                cmd.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                cmd.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);
                cmd.Parameters.Add("@ScanningNodeID", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["ScanningNodeID"]);
                cmd.Parameters.Add("@LoginID", SqlDbType.NVarChar).Value = Session["LoginID"].ToString();
                cmd.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = Session["CtsSessionType"];
                cmd.Parameters.Add("@TransCode", SqlDbType.NVarChar).Value = TransCode;
                cmd.Parameters.Add("@SAN", SqlDbType.NVarChar).Value = SAN;
                cmd.Connection = con;
                con.Open();

                cmd.ExecuteNonQuery();
                con.Close();
                con.Dispose();
                return Json("Suceess", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Update_RejectCode_StatusForCaptureRawData(Int64 id = 0, string branchAccount = null, decimal branchAmount = 0, string rejectCode = null, string otherRejctReason = null, string PayeeName = null)
        {
            try
            {
                int uid = (int)Session["uid"];

                //var result = (from p in udb.CaptureRawData
                //              where p.Id == id
                //              select p).SingleOrDefault();

                //result.BranchAccount = branchAccount;
                //result.BranchAmount = branchAmount;
                //result.Lock_UserId = uid;
                //result.ItemRejectCode = rejectCode;
                //result.OtherRejctReason = otherRejctReason;
                //result.BranchDE_Status = "R";
                //udb.SaveChanges();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "OW_BDE_Update_RejectCode_StatusForCaptureRawData";
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = uid;
                cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = id;
                cmd.Parameters.Add("@BranchAccount", SqlDbType.NVarChar).Value = branchAccount;
                cmd.Parameters.Add("@BranchAmount", SqlDbType.Decimal).Value = branchAmount;
                cmd.Parameters.Add("@ItemRejectCode", SqlDbType.NVarChar).Value = rejectCode;
                cmd.Parameters.Add("@OtherRejctReason", SqlDbType.NVarChar).Value = otherRejctReason;
                cmd.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                cmd.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                cmd.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);
                cmd.Parameters.Add("@ScanningNodeID", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["ScanningNodeID"]);
                cmd.Parameters.Add("@LoginID", SqlDbType.NVarChar).Value = Session["LoginID"].ToString();
                cmd.Parameters.Add("@PayeeName", SqlDbType.NVarChar).Value = PayeeName;
                cmd.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = Session["CtsSessionType"];
                cmd.Connection = con;
                con.Open();

                cmd.ExecuteNonQuery();
                con.Close();
                con.Dispose();
                return Json("Suceess", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Update_Close_StatusForBatch()
        {
            try
            {
                //var batch = Session["BatchID"].ToString();
                //Int64 batchID = Convert.ToInt64(batch);
                int uid = (int)Session["uid"];
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");

                //var result = (from p in udb.BatchMaster
                //              where p.Id == batchID
                //              select p).SingleOrDefault();
                //result.Branch_DE_Status = "A";
                //result.Lock_UserId = uid;
                //udb.SaveChanges();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "OW_BDE_Update_Close_StatusForBatch";
                cmd.Parameters.Add("@ProcDate", SqlDbType.NVarChar).Value = procDate;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = uid;

                cmd.Connection = con;
                con.Open();

                cmd.ExecuteNonQuery();
                con.Close();
                con.Dispose();
                return Json("Suceess", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SelectRejectReasonCodes()
        {
            try
            {
                var result = (from a in udb.ItemReturnReasons

                                select new
                                {
                                    a.RETURN_REASON_CODE,
                                    a.DESCRIPTION
                                    //BranchCodeName = a.BranchCode + " (" + a.BranchName + ")"
                                }).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SelectScanningNode()
        {
            List<string> customerlist = new List<string>();
            try
            {
                int uid = (int)Session["uid"];
                int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);
                int custid = Convert.ToInt16(Session["CustomerID"]);


                //if (domainId == 0)
                //{
                //    var result1 = (from a in udb.ScanningNodeMasters

                //                   select new
                //                   {
                //                       a.Id,
                //                       a.MACIP
                //                       //BranchCodeName = a.BranchCode + " (" + a.BranchName + ")"
                //                   }).ToList();
                //    return Json(result1, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                //    var result = (from a in udb.ScanningNodeMasters

                //                  where a.DomainId == domainId
                //                  select new
                //                  {
                //                      a.Id,
                //                      a.MACIP
                //                      //BranchCodeName = a.BranchCode + " (" + a.BranchName + ")"
                //                  }).ToList();

                //    return Json(result, JsonRequestBehavior.AllowGet);
                //}

                SqlDataAdapter adp = new SqlDataAdapter("OW_BDE_SelectScanningNodeList", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@ProcDate", SqlDbType.NVarChar).Value = procDate;
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = custid;
                adp.SelectCommand.Parameters.Add("@DomainID", SqlDbType.NVarChar).Value = domainId;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<ScanningNodeMaster>();
                ScanningNodeMaster def;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for(int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        def = new ScanningNodeMaster
                        {
                            Id = Convert.ToInt16(ds.Tables[0].Rows[i].ItemArray[0]),
                            MACIP = ds.Tables[0].Rows[i].ItemArray[1].ToString()
                        };
                        objectlst.Add(def);
                    }

                }
                return Json(objectlst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SelectBranchCodes()
        {
            List<string> customerlist = new List<string>();
            try
            {
                int uid = (int)Session["uid"];
                int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());

                //if (domainId == 0)
                //{
                //    var result1 = (from a in udb.BranchMasters

                //                   select new
                //                   {
                //                       a.BranchCode,
                //                       BranchCodeName = a.BranchCode + " (" + a.BranchName + ")"
                //                   }).ToList();
                //    return Json(result1, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                //    var result = (from a in udb.BranchMasters

                //                  where a.OwDomainId == domainId
                //                  select new
                //                  {
                //                      a.BranchCode,
                //                      BranchCodeName = a.BranchCode + " (" + a.BranchName + ")"
                //                  }).ToList();

                //    return Json(result, JsonRequestBehavior.AllowGet);
                //}

                SqlDataAdapter adp = new SqlDataAdapter("OW_BDE_SelectBranchCodesList", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@DomainID", SqlDbType.NVarChar).Value = domainId;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<BranchMaster>();
                BranchMaster def;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        def = new BranchMaster
                        {
                            BranchCode = ds.Tables[0].Rows[i].ItemArray[0].ToString(),
                            BranchName = ds.Tables[0].Rows[i].ItemArray[1].ToString()
                        };
                        objectlst.Add(def);
                    }

                }
                return Json(objectlst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        public class GroupData
        {
            public string id { get; set; }
            public string scanningType { get; set; }
        }

        //[HttpPost]
        public JsonResult SelectBatchNos(string id = null, int scanningTypeId = 0, int scanningNodeId = 0)
        {
            try
            {

                int uid = (int)Session["uid"];
                int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                //DateTime processingDate = Convert.ToDateTime(Session["processdate"].ToString());
                //string procDate = processingDate.ToString("yyyy-MM-dd");
                string processingDate2 = Session["processdate"].ToString();
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                //DateTime processingDate = DateTime.ParseExact(Session["processdate"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var xyz = Convert.ToDateTime(procDate);
                //int scanningTypeId = Convert.ToInt32(Session["ScanningTypeID"].ToString());
                //int scanningTypeId = Convert.ToInt32(dat.scanningType.ToString());

                //var result = (from a in udb.BatchMaster

                //              where a.BranchCode == id && a.ProcessDate == xyz && a.DomainId == domainId && a.ScanningType == scanningTypeId
                //              && a.ScanningNodeId == scanningNodeId 
                //              && a.Branch_DE_Status == "A" 
                //              && (a.Status == 16 || a.Status == 17)
                //              select new
                //              {
                //                  a.Id,
                //                  a.BatchNo
                //              }).ToList();


                //return Json(result, JsonRequestBehavior.AllowGet);

                SqlDataAdapter adp = new SqlDataAdapter("OW_BDE_SelectBatchNosList", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@ProcDate", SqlDbType.NVarChar).Value = procDate;
                adp.SelectCommand.Parameters.Add("@BranchCode", SqlDbType.NVarChar).Value = id;
                adp.SelectCommand.Parameters.Add("@ScanningNodeID", SqlDbType.NVarChar).Value = scanningNodeId;
                adp.SelectCommand.Parameters.Add("@ScanningTypeID", SqlDbType.NVarChar).Value = scanningTypeId;
                adp.SelectCommand.Parameters.Add("@DomainID", SqlDbType.NVarChar).Value = domainId;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<BatchMaster>();
                BatchMaster def;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        def = new BatchMaster
                        {
                            Id = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]),
                            BatchNo = Convert.ToInt16(ds.Tables[0].Rows[i].ItemArray[1])
                        };
                        objectlst.Add(def);
                    }

                }
                return Json(objectlst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SelectScanningTypes(string id = null, int scanningNodeId = 0)
        {
            try
            {
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                //var result = (from b in udb.BatchMaster
                //              join s in udb.ScanningType on b.ScanningType equals s.Code
                //              where s.KeepActive == true && b.ProcessDate == xyz && b.BranchCode == id && b.ScanningNodeId == scanningNodeId && 
                //              b.Branch_DE_Status == "A" && (b.Status == 16 || b.Status == 17)

                //              select new
                //              {
                //                  s.ID,
                //                  s.Description
                //              }).Distinct().ToList();

                //return Json(result, JsonRequestBehavior.AllowGet);
                Session["ScanningNodeID"] = scanningNodeId;
                SqlDataAdapter adp = new SqlDataAdapter("OW_BDE_SelectScanningTypesList", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                
                adp.SelectCommand.Parameters.Add("@ProcDate", SqlDbType.NVarChar).Value = procDate;
                adp.SelectCommand.Parameters.Add("@BranchCode", SqlDbType.NVarChar).Value = id;
                adp.SelectCommand.Parameters.Add("@ScanningNodeID", SqlDbType.NVarChar).Value = scanningNodeId;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<SelectScanningTypesList>();
                SelectScanningTypesList def;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        def = new SelectScanningTypesList
                        {
                            ID = Convert.ToInt16(ds.Tables[0].Rows[i].ItemArray[0]),
                            Description = ds.Tables[0].Rows[i].ItemArray[1].ToString()
                        };
                        objectlst.Add(def);
                    }

                }
                return Json(objectlst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpPost]
        public ActionResult BranchDataEntry()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            if ((bool)Session["DE"] == false)
            {
                //int uid = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            
            int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
            DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
            string procDate = processingDate1.ToString("yyyy-MM-dd");
            var xyz = Convert.ToDateTime(procDate);
            var branchCode = Session["BranchID"].ToString();
            Int64 batchNo = Convert.ToInt64(Session["BatchID"].ToString());
            int scanningTypeId = Convert.ToInt32(Session["ScanningTypeID"].ToString());
            int custid = Convert.ToInt16(Session["CustomerID"]);

            try
            {
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

                
                var PresentingMICR_StartIndex = udb.AddList_FILE_UPLOAD_Format_BranchDataEntry.FirstOrDefault((p) => p.CustomerId == custid && p.File_Format_Name == "Add List File")?.PresentingMICR_StartIndex;
                var PresentingMICR_Length = udb.AddList_FILE_UPLOAD_Format_BranchDataEntry.FirstOrDefault((p) => p.CustomerId == custid && p.File_Format_Name == "Add List File")?.PresentingMICR_Length;
                var DraweeBank_StartIndex = udb.AddList_FILE_UPLOAD_Format_BranchDataEntry.FirstOrDefault((p) => p.CustomerId == custid && p.File_Format_Name == "Add List File")?.DraweeBank_StartIndex;
                var DraweeBank_Length = udb.AddList_FILE_UPLOAD_Format_BranchDataEntry.FirstOrDefault((p) => p.CustomerId == custid && p.File_Format_Name == "Add List File")?.DraweeBank_Length;
                var PresentmentDate_StartIndex = udb.AddList_FILE_UPLOAD_Format_BranchDataEntry.FirstOrDefault((p) => p.CustomerId == custid && p.File_Format_Name == "Add List File")?.PresentmentDate_StartIndex;
                var PresentmentDate_Length = udb.AddList_FILE_UPLOAD_Format_BranchDataEntry.FirstOrDefault((p) => p.CustomerId == custid && p.File_Format_Name == "Add List File")?.PresentmentDate_Length;
                var Amount_StartIndex = udb.AddList_FILE_UPLOAD_Format_BranchDataEntry.FirstOrDefault((p) => p.CustomerId == custid && p.File_Format_Name == "Add List File")?.Amount_StartIndex;
                var Amount_Length = udb.AddList_FILE_UPLOAD_Format_BranchDataEntry.FirstOrDefault((p) => p.CustomerId == custid && p.File_Format_Name == "Add List File")?.Amount_Length;
                var ChequeNo_StartIndex = udb.AddList_FILE_UPLOAD_Format_BranchDataEntry.FirstOrDefault((p) => p.CustomerId == custid && p.File_Format_Name == "Add List File")?.ChequeNo_StartIndex;
                var ChequeNo_Length = udb.AddList_FILE_UPLOAD_Format_BranchDataEntry.FirstOrDefault((p) => p.CustomerId == custid && p.File_Format_Name == "Add List File")?.ChequeNo_Length;
                var AccountNo_StartIndex = udb.AddList_FILE_UPLOAD_Format_BranchDataEntry.FirstOrDefault((p) => p.CustomerId == custid && p.File_Format_Name == "Add List File")?.AccountNo_StartIndex;
                var AccountNo_Length = udb.AddList_FILE_UPLOAD_Format_BranchDataEntry.FirstOrDefault((p) => p.CustomerId == custid && p.File_Format_Name == "Add List File")?.AccountNo_Length;
                var PayeeName_StartIndex = udb.AddList_FILE_UPLOAD_Format_BranchDataEntry.FirstOrDefault((p) => p.CustomerId == custid && p.File_Format_Name == "Add List File")?.PayeeName_StartIndex;
                var PayeeName_Length = udb.AddList_FILE_UPLOAD_Format_BranchDataEntry.FirstOrDefault((p) => p.CustomerId == custid && p.File_Format_Name == "Add List File")?.PayeeName_Length;


                ViewBag.PresentingMICR_StartIndex = Convert.ToInt32(PresentingMICR_StartIndex);
                ViewBag.PresentingMICR_Length = Convert.ToInt32(PresentingMICR_Length);
                ViewBag.DraweeBank_StartIndex = Convert.ToInt32(DraweeBank_StartIndex);
                ViewBag.DraweeBank_Length = Convert.ToInt32(DraweeBank_Length);
                ViewBag.PresentmentDate_StartIndex = Convert.ToInt32(PresentmentDate_StartIndex);
                ViewBag.PresentmentDate_Length = Convert.ToInt32(PresentmentDate_Length);
                ViewBag.Amount_StartIndex = Convert.ToInt32(Amount_StartIndex);
                ViewBag.Amount_Length = Convert.ToInt32(Amount_Length);
                ViewBag.ChequeNo_StartIndex = Convert.ToInt32(ChequeNo_StartIndex);
                ViewBag.ChequeNo_Length = Convert.ToInt32(ChequeNo_Length);
                ViewBag.AccountNo_StartIndex = Convert.ToInt32(AccountNo_StartIndex);
                ViewBag.AccountNo_Length = Convert.ToInt32(AccountNo_Length);
                ViewBag.PayeeName_StartIndex = Convert.ToInt32(PayeeName_StartIndex);
                ViewBag.PayeeName_Length = Convert.ToInt32(PayeeName_Length);

                var result = (from a in udb.BatchMaster

                              where a.Id == batchNo
                              select new
                              {
                                  a.Id,
                                  a.BatchNo,
                                  a.ScanningNodeId,
                                  a.ScanningType,
                                  a.BranchCode,
                                  a.ChequeCount,
                                  a.TotalChequeAmount,

                              }).FirstOrDefault();

                ViewBag.TotalChequeAmount = (result.TotalChequeAmount == null) ? 0 : result.TotalChequeAmount;

                //SqlDataAdapter adp = new SqlDataAdapter("OW_BDE_GetBatchMasterDetails", con);
                //adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                //adp.SelectCommand.Parameters.Add("@BatchID", SqlDbType.NVarChar).Value = batchNo;

                //DataSet ds = new DataSet();
                //adp.Fill(ds);

                //Int64 Batch_ID;
                //int Batch_No,Batch_ScanningType, Batch_ChequeCount;
                //Int16 Batch_ScanningNodeID;
                //string Batch_BranchCode = "";
                var objectlst = new List<CaptureRawData>();
                CaptureRawData def;

                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    //Batch_ID = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]);
                //    //Batch_No = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[1]);
                //    //Batch_ScanningNodeID = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[2]);
                //    //Batch_ScanningType = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[3]);
                //    //Batch_BranchCode = ds.Tables[0].Rows[0].ItemArray[4].ToString();
                //    //Batch_ChequeCount = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[5]);

                //    SqlDataAdapter adp1 = new SqlDataAdapter("OW_BDE_GetCaptureRawDataList", con);
                //    adp1.SelectCommand.CommandType = CommandType.StoredProcedure;
                //    adp1.SelectCommand.Parameters.Add("@ProcDate", SqlDbType.NVarChar).Value = xyz;
                //    adp1.SelectCommand.Parameters.Add("@DomainID", SqlDbType.NVarChar).Value = domainId;
                //    adp1.SelectCommand.Parameters.Add("@BranchCode", SqlDbType.NVarChar).Value = branchCode;
                //    adp1.SelectCommand.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[1]);
                //    adp1.SelectCommand.Parameters.Add("@ScanningTypeID", SqlDbType.NVarChar).Value = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[3]);
                //    adp1.SelectCommand.Parameters.Add("@ScanningNodeID", SqlDbType.NVarChar).Value = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[2]);


                //    DataSet ds1 = new DataSet();
                //    adp1.Fill(ds1);


                //    if (ds1.Tables[0].Rows.Count > 0)
                //    {
                //        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                //        {
                //            def = new CaptureRawData
                //            {
                //                Id = Convert.ToInt64(ds1.Tables[0].Rows[i].ItemArray[0]),
                //                ScanningType = Convert.ToByte(ds1.Tables[0].Rows[i].ItemArray[1]),
                //                ScanningNodeId = Convert.ToInt32(ds1.Tables[0].Rows[i].ItemArray[2]),
                //                BatchNo = Convert.ToInt32(ds1.Tables[0].Rows[i].ItemArray[3]),
                //                BatchSeqNo = Convert.ToInt32(ds1.Tables[0].Rows[i].ItemArray[4]),
                //                InstrumentType = ds1.Tables[0].Rows[i].ItemArray[5].ToString(),
                //                SlipNo = Convert.ToInt32(ds1.Tables[0].Rows[i].ItemArray[6]),
                //                SlipChequeCount = Convert.ToInt32(ds1.Tables[0].Rows[i].ItemArray[7]),
                //                FrontGreyImage = ds1.Tables[0].Rows[i].ItemArray[8].ToString(),
                //                FrontTiffImage = ds1.Tables[0].Rows[i].ItemArray[9].ToString(),
                //                BackGreyImage = ds1.Tables[0].Rows[i].ItemArray[10].ToString(),
                //                BackTiffImage = ds1.Tables[0].Rows[i].ItemArray[11].ToString(),
                //                BranchAccount = ds1.Tables[0].Rows[i].ItemArray[12].ToString(),
                //                BranchAmount = Convert.ToDecimal((ds1.Tables[0].Rows[i].ItemArray[13] == DBNull.Value ? 0 : ds1.Tables[0].Rows[i].ItemArray[13])),
                //                CreditAccountNo = ds1.Tables[0].Rows[i].ItemArray[14].ToString(),
                //                BranchDE_Status = ds1.Tables[0].Rows[i].ItemArray[15].ToString(),
                //                ItemRejectCode = ds1.Tables[0].Rows[i].ItemArray[16].ToString(),
                //                OtherRejctReason = ds1.Tables[0].Rows[i].ItemArray[17].ToString(),
                //                ChequeNoMICR = ds1.Tables[0].Rows[i].ItemArray[18].ToString(),
                //                SortCodeMICR = ds1.Tables[0].Rows[i].ItemArray[19].ToString(),
                //            };
                //            objectlst.Add(def);
                //        }
                //        ViewBag.cnt = true;
                //        @Session["glob"] = null;
                //    }
                //}
                //else
                //{
                //    return RedirectToAction("IWIndex", "Home", new { id = 1 });// return RedirectToAction("DeSelection", new { id = 1 });
                //}
                //return View(objectlst);
                var resultNew = (from a in udb.CaptureRawData

                                 where a.ProcessingDate == xyz && a.DomainId == domainId && a.BranchCode == branchCode && a.BatchNo == result.BatchNo &&
                                 a.ScanningType == result.ScanningType && a.ScanningNodeId == result.ScanningNodeId && a.BranchDE_Status == "A"
                                 orderby a.BatchSeqNo ascending
                                 select new
                                 {
                                     a.Id,
                                     a.ScanningType,
                                     a.ScanningNodeId,
                                     a.BatchNo,
                                     a.BatchSeqNo,
                                     a.InstrumentType,
                                     a.SlipNo,
                                     a.SlipChequeCount,
                                     a.FrontGreyImage,
                                     a.FrontTiffImage,
                                     a.BackGreyImage,
                                     a.BackTiffImage,
                                     a.BranchAccount,
                                     a.BranchAmount,
                                     a.CreditAccountNo,
                                     a.BranchDE_Status,
                                     a.ItemRejectCode,
                                     a.OtherRejctReason,
                                     a.ChequeNoMICR,
                                     a.SortCodeMICR,
                                     a.PayeeName,
                                     a.SANMICR,
                                     a.TransCodeMICR,
                                     a.FrontUVImage

                                 }).ToList();

                CaptureRawData crd;
                //List<CaptureRawData> objectlst = new List<CaptureRawData>();
                //List<CaptureRawData> objectlst1 = new List<CaptureRawData>();
                //List<CaptureRawData> objectlst2 = new List<CaptureRawData>();
                //List<CaptureRawData> objectlst3 = new List<CaptureRawData>();



                if (resultNew.Count > 0)
                {
                    int index = 0;
                    int count = resultNew.Count;

                    for (var i = 0; i < resultNew.Count; i++)
                    {
                        crd = new CaptureRawData
                        {
                            Id = resultNew[i].Id,
                            ScanningType = resultNew[i].ScanningType,
                            ScanningNodeId = resultNew[i].ScanningNodeId,
                            BatchNo = resultNew[i].BatchNo,
                            BatchSeqNo = resultNew[i].BatchSeqNo,
                            InstrumentType = resultNew[i].InstrumentType,
                            SlipNo = resultNew[i].SlipNo,
                            SlipChequeCount = resultNew[i].SlipChequeCount,
                            FrontGreyImage = resultNew[i].FrontGreyImage,
                            FrontTiffImage = resultNew[i].FrontTiffImage,
                            BackGreyImage = resultNew[i].BackGreyImage,
                            BackTiffImage = resultNew[i].BackTiffImage,
                            BranchAccount = resultNew[i].BranchAccount,
                            BranchAmount = resultNew[i].BranchAmount,
                            CreditAccountNo = resultNew[i].CreditAccountNo,
                            BranchDE_Status = resultNew[i].BranchDE_Status,
                            ChequeNoMICR = resultNew[i].ChequeNoMICR,
                            SortCodeMICR = resultNew[i].SortCodeMICR,
                            PayeeName = resultNew[i].PayeeName,
                            SANMICR = resultNew[i].SANMICR,
                            TransCodeMICR = resultNew[i].TransCodeMICR,
                            FrontUVImage = resultNew[i].FrontUVImage
                        };
                        objectlst.Add(crd);
                    }




                    //    //objectlst1 = captureLists1();
                    //    //objectlst2 = captureLists2();
                    //    //objectlst3 = captureLists3();
                    ViewBag.cnt = true;
                    @Session["glob"] = null;

                }
                else
                {
                    return RedirectToAction("IWIndex", "Home", new { id = 1 });// return RedirectToAction("DeSelection", new { id = 1 });
                }
                //return View(objectlst1);
                //return View(objectlst2);
                //return View(objectlst3);
                return View(objectlst);


            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                //return Json(message, JsonRequestBehavior.AllowGet);
                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWBranchLevelDataEntry HttpGet - " + innerExcp });
            }
        }

        public ActionResult LoadCaptureRawData()
        {
            int uid = (int)Session["uid"];
            int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
            DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
            string procDate = processingDate1.ToString("yyyy-MM-dd");
            var xyz = Convert.ToDateTime(procDate);
            var branchCode = Session["BranchID"].ToString();
            Int64 batchNo = Convert.ToInt64(Session["BatchID"].ToString());
            int scanningTypeId = Convert.ToInt32(Session["ScanningTypeID"].ToString());

            try
            {
                var result = (from a in udb.BatchMaster

                              where a.Id == batchNo
                              select new
                              {
                                  a.Id,
                                  a.BatchNo,
                                  a.ScanningNodeId,
                                  a.ScanningType,
                                  a.BranchCode,
                                  a.ChequeCount,

                              }).FirstOrDefault();

                var objectlst = new List<CaptureRawData>();
                CaptureRawData def;
                var resultNew = (from a in udb.CaptureRawData

                                 where a.ProcessingDate == xyz && a.DomainId == domainId && a.BranchCode == branchCode && a.BatchNo == result.BatchNo &&
                                 a.ScanningType == result.ScanningType && a.ScanningNodeId == result.ScanningNodeId
                                 orderby a.BatchSeqNo ascending
                                 select new
                                 {
                                     a.Id,
                                     a.ScanningType,
                                     a.ScanningNodeId,
                                     a.BatchNo,
                                     a.BatchSeqNo,
                                     a.InstrumentType,
                                     a.SlipNo,
                                     a.SlipChequeCount,
                                     a.FrontGreyImage,
                                     a.FrontTiffImage,
                                     a.BackGreyImage,
                                     a.BackTiffImage,
                                     a.BranchAccount,
                                     a.BranchAmount,
                                     a.CreditAccountNo,
                                     a.BranchDE_Status,
                                     a.ItemRejectCode,
                                     a.OtherRejctReason,
                                     a.ChequeNoMICR,
                                     a.SortCodeMICR,
                                     a.PayeeName

                                 }).ToList();

                CaptureRawData crd;

                if (resultNew.Count > 0)
                {
                    int index = 0;
                    int count = resultNew.Count;

                    for (var i = 0; i < resultNew.Count; i++)
                    {
                        crd = new CaptureRawData
                        {
                            Id = resultNew[i].Id,
                            ScanningType = resultNew[i].ScanningType,
                            ScanningNodeId = resultNew[i].ScanningNodeId,
                            BatchNo = resultNew[i].BatchNo,
                            BatchSeqNo = resultNew[i].BatchSeqNo,
                            InstrumentType = resultNew[i].InstrumentType,
                            SlipNo = resultNew[i].SlipNo,
                            SlipChequeCount = resultNew[i].SlipChequeCount,
                            FrontGreyImage = resultNew[i].FrontGreyImage,
                            FrontTiffImage = resultNew[i].FrontTiffImage,
                            BackGreyImage = resultNew[i].BackGreyImage,
                            BackTiffImage = resultNew[i].BackTiffImage,
                            BranchAccount = resultNew[i].BranchAccount,
                            BranchAmount = resultNew[i].BranchAmount,
                            CreditAccountNo = resultNew[i].CreditAccountNo,
                            BranchDE_Status = resultNew[i].BranchDE_Status,
                            ChequeNoMICR = resultNew[i].ChequeNoMICR,
                            SortCodeMICR = resultNew[i].SortCodeMICR,
                            PayeeName = resultNew[i].PayeeName
                        };
                        objectlst.Add(crd);
                    }

                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                }
                else
                {
                    return RedirectToAction("IWIndex", "Home", new { id = 1 });// return RedirectToAction("DeSelection", new { id = 1 });
                }
                
                return View(objectlst);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                //return Json(message, JsonRequestBehavior.AllowGet);
                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWBranchLevelDataEntry HttpGet - " + innerExcp });
            }
        }

        [HttpPost]
        public ActionResult SelectionForBranchDataEntry(SelectionForBranchDataEntry selectionForBranchDataEntry)
        {
            if (Request.Form["BranchSelect"] != null)
                Session["BranchID"] = Request.Form["BranchSelect"].ToString();

            if (Request.Form["BatchSelect"] != null)
                Session["BatchID"] = Request.Form["BatchSelect"].ToString();

            if (Request.Form["SelectScanningType"] != null)
                Session["ScanningTypeID"] = Request.Form["SelectScanningType"].ToString();
            try
            {
                var branch = Session["BranchID"].ToString();
                var batch = Session["BatchID"].ToString();
                var scanningType = Session["ScanningTypeID"].ToString();
                int uid = (int)Session["uid"];
                Int64 batchID = Convert.ToInt64(batch);
                int scanningTypeId = Convert.ToInt16(scanningType);

                //var res = (from a in udb.ScanningType
                //           where a.ID == scanningTypeId
                //           select a).SingleOrDefault();
                //Session["ScanningType"] = res.Description;

                SqlDataAdapter adp = new SqlDataAdapter("OW_BDE_GetScanningTypeName", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@ScanningTypeID", SqlDbType.NVarChar).Value = scanningTypeId;
                
                DataSet ds = new DataSet();
                adp.Fill(ds);
                //var objectlst = new List<BatchMaster>();
                //ScanningType def;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session["ScanningType"] = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                    //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    //{
                    //    def = new ScanningType
                    //    {
                    //        Description = ds.Tables[0].Rows[i].ItemArray[0].ToString(),
                    //    };
                    //    //objectlst.Add(def);
                        
                    //}

                }
                

                //var result = (from p in udb.BatchMaster
                //              where p.Id == batchID
                //              select p).SingleOrDefault();
                //result.Branch_DE_Status = "L";
                //result.Lock_UserId = uid;
                //udb.SaveChanges();

                //SqlDataAdapter adp1 = new SqlDataAdapter("OW_BDE_UpdateBatchMasterDE_Status", con);
                //adp1.UpdateCommand.CommandType = CommandType.StoredProcedure;
                //adp1.SelectCommand.Parameters.Add("@BatchID", SqlDbType.NVarChar).Value = batchID;

                //DataSet ds = new DataSet();
                //adp.Fill(ds);

                //OWpro.OW_BDE_UpdateBatchMasterDE_Status()

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "OW_BDE_UpdateBatchMasterDE_Status";
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = uid;
                cmd.Parameters.Add("@BatchID", SqlDbType.BigInt).Value = batchID;
                cmd.Connection = con;
                con.Open();

                cmd.ExecuteNonQuery();
                con.Close();
                con.Dispose();

                return RedirectToAction("BranchDataEntry", "OWBranchLevelDataEntry");
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWBranchLevelDataEntry HttpGet - " + innerExcp });
            }
        }

        public PartialViewResult RejectReason(int id = 0)
        {

            //var rjrs = (from r in udb.ItemReturnReasonsForBranchDataEntry
            //            select new RejectReason
            //            {
            //                Description = r.DESCRIPTION,
            //                ReasonCodeS = r.RETURN_REASON_CODE
            //            });


            SqlDataAdapter adp = new SqlDataAdapter("OW_BDE_GetRejectReason", con);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            
            DataSet ds = new DataSet();
            adp.Fill(ds);

            var objectlst = new List<RejectReason>();
            RejectReason def;

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    def = new RejectReason
                    {
                        ReasonCodeS = ds.Tables[0].Rows[i].ItemArray[0].ToString(),
                        Description = ds.Tables[0].Rows[i].ItemArray[1].ToString()
                    };
                    objectlst.Add(def);
                }
            }

            return PartialView("_RejectReason", objectlst);

        }

        public JsonResult GetReasonRejectData(string id = null)
        {
            try
            {
                int uid = (int)Session["uid"];
                //var result = (from p in udb.ItemReturnReasonsForBranchDataEntry
                //              where p.RETURN_REASON_CODE == id
                //              select p).SingleOrDefault();

                SqlDataAdapter adp = new SqlDataAdapter("OW_BDE_GetReasonRejectData", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@RejectCode", SqlDbType.NVarChar).Value = id;

                DataSet ds = new DataSet();
                adp.Fill(ds);

                var objectlst = new List<RejectReason>();
                RejectReason def = new RejectReason();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        //def
                        //{
                        def.ReasonCodeS = ds.Tables[0].Rows[i].ItemArray[0].ToString();
                        def.Description = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                        //};

                        //objectlst.Add(def);
                    };
                    
                }

                return Json(def, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetReasonRejectDescription(string id = null)
        {
            try
            {
                int uid = (int)Session["uid"];
                //var result = (from p in udb.ItemReturnReasonsForBranchDataEntry
                //              where p.RETURN_REASON_CODE == id
                //              select p).SingleOrDefault();

                SqlDataAdapter adp = new SqlDataAdapter("OW_BDE_GetReasonRejectData", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@RejectCode", SqlDbType.NVarChar).Value = id;

                DataSet ds = new DataSet();
                adp.Fill(ds);

                //var objectlst = new List<RejectReason>();
                RejectReason def = new RejectReason();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        def.ReasonCodeS = ds.Tables[0].Rows[i].ItemArray[0].ToString();
                        def.Description = ds.Tables[0].Rows[i].ItemArray[1].ToString();

                    };
                }

                return Json(def, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DisplayFrontBackTiffImage(string httpwebimgpath = null)
        {
            try
            {
                string someUrl = httpwebimgpath;
                var webClient = new WebClient();

                byte[] imageBytes = webClient.DownloadData(someUrl);

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


                string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                ViewBag.ImageData = imageDataURL;
                return Json(imageDataURL, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                //logger.Log(LogLevel.Error, "OWL2Chq getTiffImg|" + message + "INNEREXP| " + innerExcp, "Login Index-Msg");
                //logger.Log(LogLevel.Error, e.InnerException.StackTrace, "Login Index-Exception");

                //return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });
                //return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWBranchLevelDataEntry HttpGet - " + innerExcp });
                return Json(message, JsonRequestBehavior.AllowGet);
                // return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL2 getTiffImage - " + innerExcp });
            }

            //return PartialView("_getTiffImage");
            
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
                //logerror(actualpath, actualpath.ToString() + "-> actualpath Path");
                actualpath = destroot + "\\" + actualpath;
                // System.IO.File.AppendAllText("C:\\temp\\log1.txt", "actualpathOWL1:" + actualpath);
                //logerror(actualpath, actualpath.ToString() + "-> After actualpath Path");
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

        public class SelectScanningTypesList
        {
            public Int16 ID { get; set; }
            public string Description { get; set; }
            //public IEnumerable<SelectListItem> BranchCodeList { get; set; }
            //public IEnumerable<SelectListItem> BatchNoList { get; set; }
        }
    }
}
