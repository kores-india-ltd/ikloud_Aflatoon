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
    public class IwSuspenseQueueController : Controller
    {
        //
        // GET: /IwSuspenseQueue/
        private static Logger logger = LogManager.GetCurrentClassLogger();
        AflatoonEntities af = new AflatoonEntities();
        //OWProcDataContext OWpro = new OWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);

        public ActionResult SelectionForBranchCode(int id = 0)
        {
            SelectionForBranchCode selectionForBranchCode = new SelectionForBranchCode();
            try
            {
                if (Session["uid"] != null)
                {
                    int uid = (int)Session["uid"];
                    int custid = Convert.ToInt16(Session["CustomerID"]);
                    //Session["VerificationId"] = id;
                    //var owVeriEnableBranch = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "OWVerificationEnableBranchCodeSelection")?.SettingValue;

                    //if (owVeriEnableBranch == null || owVeriEnableBranch == "")
                    //{
                    //    ViewBag.OWVeriEnableBranch = "N";
                    //    return RedirectToAction("Index", "OWSmbVerification", new { id = id });
                    //}
                    //else
                    //{
                    //    if (owVeriEnableBranch == "Y")
                    //    {
                    //        ViewBag.OWVeriEnableBranch = "Y";
                    //        if (Session["ProType"].ToString() == "Outward")
                    //        {
                                string customerId = Session["CustomerID"].ToString();
                                //string domainId = Session["DomainselectID"].ToString();
                                //string domainId = "4";
                                //string domainName = Session["SelectdDomainName"].ToString();
                                //string domainId1 = Session["domainid"].ToString();
                                //string domainName1 = Session["domainname"].ToString();
                                string processingDate = Session["processdate"].ToString();
                                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                                string procDate = processingDate1.ToString("yyyy-MM-dd");


                    //        }
                    //    }
                    //    else
                    //    {
                    //        ViewBag.OWVeriEnableBranch = "N";
                    //        return RedirectToAction("Index", "OWSmbVerification", new { id = id });
                    //    }
                    //}
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
                //int domainId = Convert.ToInt32(4);

                int customerId = Convert.ToInt32(Session["CustomerID"].ToString());
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);
                //int verificationId = Convert.ToInt16(Session["VerificationId"].ToString());

                //if (verificationId == 5)
                //{
                //    var result = FetchBranchCodeListData(verificationId);
                //    return Json(result, JsonRequestBehavior.AllowGet);
                //}
                //else if (verificationId == 1)
                //{
                //    var result = FetchBranchCodeListData(verificationId);
                //    return Json(result, JsonRequestBehavior.AllowGet);
                //}
                //else if (verificationId == 11)
                //{
                //    var result = FetchBranchCodeListData(verificationId);
                //    return Json(result, JsonRequestBehavior.AllowGet);
                //}
                //else if (verificationId == 12)
                //{
                //    var result = FetchBranchCodeListData(verificationId);
                //    return Json(result, JsonRequestBehavior.AllowGet);
                //}


                //return Json("false", JsonRequestBehavior.AllowGet);

                var result = FetchBranchCodeListData();
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

        public List<BranchCodeList> FetchBranchCodeListData()
        {
            int uid = (int)Session["uid"];
            //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());

            int customerId = Convert.ToInt32(Session["CustomerID"].ToString());
            DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
            string procDate = processingDate1.ToString("yyyy-MM-dd");
            var xyz = Convert.ToDateTime(procDate);

            SqlDataAdapter adp = new SqlDataAdapter("IW_SuspenseQueue_SelectBranchCodeList", con);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            //adp.SelectCommand.Parameters.Add("@DomainID", SqlDbType.NVarChar).Value = 4;
            //adp.SelectCommand.Parameters.Add("@DomainID", SqlDbType.NVarChar).Value = Session["domainid"];
            adp.SelectCommand.Parameters.Add("@DomainID", SqlDbType.NVarChar).Value = 0;
            adp.SelectCommand.Parameters.Add("@ProcDate", SqlDbType.NVarChar).Value = procDate;
            adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.NVarChar).Value = customerId;
            //adp.SelectCommand.Parameters.Add("@VerificationID", SqlDbType.NVarChar).Value = verificationId;
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
            //int verificationId = Convert.ToInt16(Session["VerificationId"].ToString());
            if (Request.Form["BranchSelect"] != null)
                Session["BranchID"] = Request.Form["BranchSelect"].ToString();
            try
            {
                var branch = Session["BranchID"].ToString();
                return RedirectToAction("Index", "IwSuspenseQueue", new { branchId = branch });
            }
            catch (Exception e)
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

        public ActionResult Index(string branchId = null)
        {
            int uid = (int)Session["uid"];
            int custid = Convert.ToInt16(Session["CustomerID"]);
            //var varMinAclen = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "ACFrom").SettingValue;
            //var varMaxAclen = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "ACTo").SettingValue;
            //var varMaxPayeelen = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "PayeeNameMaxLength").SettingValue;

            //int intMinAclen = Convert.ToInt32(varMinAclen);
            //int intMaxAclen = Convert.ToInt32(varMaxAclen);
            //int intMaxPayeelen = Convert.ToInt32(varMaxPayeelen);

            //ViewBag.MinAclen = intMinAclen;
            //ViewBag.MaxAclen = intMaxAclen;
            //ViewBag.MaxPayeelen = intMaxPayeelen;
            ViewBag.BankCode = Session["BankCode"].ToString();

            try
            {
                SqlDataAdapter adp = new SqlDataAdapter("[OWSelectSuspenseQueue]", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                //adp.SelectCommand.Parameters.Add("@VFtype", SqlDbType.NVarChar).Value = VFType;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                //adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(4);
                //adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = Session["CtsSessionType"];
                adp.SelectCommand.Parameters.Add("@BranchCode", SqlDbType.NVarChar).Value = branchId;


                DataSet ds = new DataSet();
                adp.Fill(ds);
                IwSuspenseQueueView def;
                var objectlst = new List<IwSuspenseQueueView>();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        def = new IwSuspenseQueueView
                        {
                            Id = Convert.ToInt64(ds.Tables[0].Rows[i]["Id"]),
                            BranchCode = ds.Tables[0].Rows[i]["BranchCode"].ToString(),
                            AccountNumber = ds.Tables[0].Rows[i]["AccountNumber"].ToString(),
                            AccountName = ds.Tables[0].Rows[i]["AccountName"].ToString(),
                            AvailabeAmount = Convert.ToDecimal((ds.Tables[0].Rows[i]["AvailabeAmount"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["AvailabeAmount"])),
                            ShadowBalance = Convert.ToDecimal((ds.Tables[0].Rows[i]["ShadowBalance"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["ShadowBalance"])),
                            ChequeNumber = ds.Tables[0].Rows[i]["ChequeNumber"].ToString(),
                            Amount = Convert.ToDecimal((ds.Tables[0].Rows[i]["Amount"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["Amount"])),
                            LienFlag = ds.Tables[0].Rows[i]["LienFlag"].ToString(),
                            PPSFlag = ds.Tables[0].Rows[i]["PPSFlag"].ToString(),
                            PONumber = ds.Tables[0].Rows[i]["PONumber"].ToString(),
                            POStatus = ds.Tables[0].Rows[i]["POStatus"].ToString(),
                            Status = Convert.ToByte(ds.Tables[0].Rows[i]["Status"]),
                            WaiveCharges = ds.Tables[0].Rows[i]["WaiveCharges"].ToString(),
                            Decision = ds.Tables[0].Rows[i]["Decision"].ToString(),
                            Remarks = ds.Tables[0].Rows[i]["Remarks"].ToString()
                        };
                        objectlst.Add(def);
                    }
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

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "Selection for branch code post - " + innerExcp });
            }

            
        }

        public ActionResult ShowIwSuspenseQueueDetails(Int64 Id = 0)
        {
            try
            {
                SqlDataAdapter adp = new SqlDataAdapter("Get_IW_SuspenseQueue_Details", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@Id", SqlDbType.NVarChar).Value = Id;
                
                DataSet ds = new DataSet();
                adp.Fill(ds);
                
                IwSuspenseQueueView def = new IwSuspenseQueueView();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    def = new IwSuspenseQueueView
                    {
                        Id = Convert.ToInt64(ds.Tables[0].Rows[0]["Id"]),
                        BranchCode = ds.Tables[0].Rows[0]["BranchCode"].ToString(),
                        AccountNumber = ds.Tables[0].Rows[0]["AccountNumber"].ToString(),
                        AccountName = ds.Tables[0].Rows[0]["AccountName"].ToString(),
                        AvailabeAmount = Convert.ToDecimal((ds.Tables[0].Rows[0]["AvailabeAmount"] == DBNull.Value ? 0 : ds.Tables[0].Rows[0]["AvailabeAmount"])),
                        ShadowBalance = Convert.ToDecimal((ds.Tables[0].Rows[0]["ShadowBalance"] == DBNull.Value ? 0 : ds.Tables[0].Rows[0]["ShadowBalance"])),
                        ChequeNumber = ds.Tables[0].Rows[0]["ChequeNumber"].ToString(),
                        Amount = Convert.ToDecimal((ds.Tables[0].Rows[0]["Amount"] == DBNull.Value ? 0 : ds.Tables[0].Rows[0]["Amount"])),
                        LienFlag = ds.Tables[0].Rows[0]["LienFlag"].ToString(),
                        PPSFlag = ds.Tables[0].Rows[0]["PPSFlag"].ToString(),
                        PONumber = ds.Tables[0].Rows[0]["PONumber"].ToString(),
                        POStatus = ds.Tables[0].Rows[0]["POStatus"].ToString(),
                        Status = Convert.ToByte(ds.Tables[0].Rows[0]["Status"]),
                        WaiveCharges = ds.Tables[0].Rows[0]["WaiveCharges"].ToString(),
                        Decision = ds.Tables[0].Rows[0]["Decision"].ToString(),
                        Remarks = ds.Tables[0].Rows[0]["Remarks"].ToString(),
                        UDK = ds.Tables[0].Rows[0]["UDK"] == null ? "" :  ds.Tables[0].Rows[0]["UDK"].ToString()

                    };

                    ViewBag.DecisionTaken = Convert.ToByte(ds.Tables[0].Rows[0]["Status"]);
                    ViewBag.RejectCode = ds.Tables[0].Rows[0]["RejectReason"].ToString();
                }

                //var result = FetchBranchCodeListData();
                //return Json(def, JsonRequestBehavior.AllowGet);

                //var rtnlist = (from i in af.ItemReturnReasons select i).ToList();// iwafl.ItemReturnReasons.Select(m).ToList();
                //ViewBag.rtnlist = rtnlist.Select(m => m.RETURN_REASON_CODE).ToList();
                //ViewBag.rtnlistDescrp = rtnlist.Select(m => m.DESCRIPTION).ToList();

                SqlDataAdapter adp1 = new SqlDataAdapter("IW_SuspenseQueue_GetRejectReason", con);
                adp1.SelectCommand.CommandType = CommandType.StoredProcedure;

                DataSet ds1 = new DataSet();
                adp1.Fill(ds1);

                var objectlst1 = new List<RejectReason>();
                RejectReason def1;

                if (ds1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        def1 = new RejectReason
                        {
                            ReasonCodeS = ds1.Tables[0].Rows[i].ItemArray[0].ToString(),
                            Description = ds1.Tables[0].Rows[i].ItemArray[1].ToString()
                        };
                        objectlst1.Add(def1);
                    }
                }
                ViewBag.rtnlist = objectlst1.Select(m => m.ReasonCodeS).ToList();
                ViewBag.rtnlistDescrp = objectlst1.Select(m => m.Description).ToList();

                return View(def);
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

                //return Json(message, JsonRequestBehavior.AllowGet);
                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "Selection for branch code post - " + innerExcp });
            }
        }

        public PartialViewResult RejectReason(int id = 0)
        {

            SqlDataAdapter adp = new SqlDataAdapter("IW_SuspenseQueue_GetRejectReason", con);
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

        //[HttpPost]
        public ActionResult Update_SuspenseQueue_Data(Int64 Id = 0, string Decision = null, int RejectCode = 0, string IsWaiveCharges = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
                //return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            try
            {
                int uid = (int)Session["uid"];

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "IW_SuspenseQueue_Update_Details";
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = uid;
                cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = Id;
                cmd.Parameters.Add("@Decision", SqlDbType.NVarChar).Value = Decision == "A" ? "2" : "3";
                cmd.Parameters.Add("@RejectCode", SqlDbType.NVarChar).Value = RejectCode;
                cmd.Parameters.Add("@IsWaiveCharges", SqlDbType.NVarChar).Value = IsWaiveCharges == "Yes" ? "Y" : "N";
                
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

        //[HttpPost]
        public JsonResult Get_SuspenseQueue_ImageData(string BranchCode = null, decimal ChequeAmount = 0, string ChequeNumber = null, string udkItemSeqNo = null,
                                                        string udkPresentmentDate = null, string udkChequeNo = null, string udkSortCode = null, string udkTransCode = null)
        {
            int uid = (int)Session["uid"];
            int custid = Convert.ToInt16(Session["CustomerID"]);
            
            ViewBag.BankCode = Session["BankCode"].ToString();

            try
            {
                var finaldate = "";
                if(udkPresentmentDate != null || udkPresentmentDate != "")
                {
                    finaldate = "20" + udkPresentmentDate.ToString().Substring(6, 2) + "-" + udkPresentmentDate.ToString().Substring(2, 2) + "-" + udkPresentmentDate.ToString().Substring(0, 2);

                }
                //logerror("Get_SuspenseQueue_ImageData", "parameter ProcessingDate - " + Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"));
                //logerror("Get_SuspenseQueue_ImageData", "parameter CustomerId - " + Convert.ToInt16(Session["CustomerID"]));
                //logerror("Get_SuspenseQueue_ImageData", "parameter BranchCode - " + BranchCode);
                //logerror("Get_SuspenseQueue_ImageData", "parameter ChequeNo - " + ChequeNumber);
                //logerror("Get_SuspenseQueue_ImageData", "parameter Amount - " + ChequeAmount);
                //logerror("Get_SuspenseQueue_ImageData", "parameter udkItemSeqNo - " + udkItemSeqNo);
                //logerror("Get_SuspenseQueue_ImageData", "parameter udkPresentmentDate - " + finaldate);
                //logerror("Get_SuspenseQueue_ImageData", "parameter udkChequeNo - " + udkChequeNo);
                //logerror("Get_SuspenseQueue_ImageData", "parameter udkSortCode - " + udkSortCode);
                //logerror("Get_SuspenseQueue_ImageData", "parameter udkTransCode - " + udkTransCode);


                SqlDataAdapter adp = new SqlDataAdapter("[Get_IW_SuspenseQueue_Images]", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();
                adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                //adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(4);
                //adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = Session["CtsSessionType"];
                adp.SelectCommand.Parameters.Add("@BranchCode", SqlDbType.NVarChar).Value = BranchCode;
                adp.SelectCommand.Parameters.Add("@ChequeNo", SqlDbType.NVarChar).Value = ChequeNumber;
                adp.SelectCommand.Parameters.Add("@Amount", SqlDbType.Decimal).Value = ChequeAmount;

                adp.SelectCommand.Parameters.Add("@udkItemSeqNo", SqlDbType.NVarChar).Value = udkItemSeqNo;
                adp.SelectCommand.Parameters.Add("@udkPresentmentDate", SqlDbType.NVarChar).Value = finaldate;
                adp.SelectCommand.Parameters.Add("@udkChequeNo", SqlDbType.NVarChar).Value = udkChequeNo;
                adp.SelectCommand.Parameters.Add("@udkSortCode", SqlDbType.NVarChar).Value = udkSortCode;
                adp.SelectCommand.Parameters.Add("@udkTransCode", SqlDbType.NVarChar).Value = udkTransCode;


                DataSet ds = new DataSet();
                adp.Fill(ds);
                Get_IW_SuspenseQueueImageData def;
                var objectlst = new List<Get_IW_SuspenseQueueImageData>();
                //logerror("Get_SuspenseQueue_ImageData", "count - " + ds.Tables[0].Rows.Count);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        def = new Get_IW_SuspenseQueueImageData
                        {
                            ChequeNo = ds.Tables[0].Rows[i]["ChequeNo"].ToString(),
                            SortCode = ds.Tables[0].Rows[i]["SortCode"].ToString(),
                            SAN = ds.Tables[0].Rows[i]["SAN"].ToString(),
                            TransCode = ds.Tables[0].Rows[i]["TransCode"].ToString(),
                            ChequeAmount = Convert.ToDecimal((ds.Tables[0].Rows[i]["ChequeAmount"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["ChequeAmount"])),
                            FrontGreyImagePath = ds.Tables[0].Rows[i]["FrontGreyImagePath"].ToString(),
                        };
                        objectlst.Add(def);
                    }
                }
                //logerror("Get_SuspenseQueue_ImageData", "objectlst - " + objectlst);
                //return JsonResult(objectlst, JsonRequestBehavior.AllowGet);
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
                //return RedirectToAction("Error", "Error", new { msg = message, popmsg = "Selection for branch code post - " + innerExcp });
            }


        }

        public class Get_IW_SuspenseQueueImageData
        {
            public string ChequeNo { get; set; }
            public string SortCode { get; set; }
            public string SAN { get; set; }
            public string TransCode { get; set; }
            public decimal  ChequeAmount { get; set; }
            public string FrontGreyImagePath { get; set; }

        }
    
    }
}
