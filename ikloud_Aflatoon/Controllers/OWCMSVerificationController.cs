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
    public class OWCMSVerificationController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        AflatoonEntities af = new AflatoonEntities();
        //OWProcDataContext OWpro = new OWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
        //
        // GET: /OWCMSVerification/

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

        public ActionResult SelectionForBranchCode(int id = 0)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }

            if ((bool)Session["VF"] == false)
            {
                int uid = (int)Session["uid"];
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
                    int uid = (int)Session["uid"];
                    int custid = Convert.ToInt16(Session["CustomerID"]);

                    var owVeriEnableBranch = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "OWVerificationEnableBranchCodeSelection")?.SettingValue;

                    if (owVeriEnableBranch == null || owVeriEnableBranch == "")
                    {
                        ViewBag.OWVeriEnableBranch = "N";
                        return RedirectToAction("Index", "OWCMSVerification", new { id = id });
                    }
                    else
                    {
                        if (owVeriEnableBranch == "Y")
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
                            return RedirectToAction("Index", "OWCMSVerification", new { id = id });
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
                int verificationId = 4;

                if (verificationId == 4)
                {
                    var result = FetchBranchCodeListData(verificationId);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

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

            //logerror("FetchBranchCodeListData", "parameter ProcDate - " + procDate);
            //logerror("FetchBranchCodeListData", "parameter CustomerId - " + customerId);
            //logerror("FetchBranchCodeListData", "parameter DomainID - " + domainId);
            //logerror("FetchBranchCodeListData", "parameter VerificationID - " + verificationId);


            SqlDataAdapter adp = new SqlDataAdapter("OW_Verification_SelectBranchCodeList", con);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.Add("@DomainID", SqlDbType.NVarChar).Value = domainId;
            adp.SelectCommand.Parameters.Add("@ProcDate", SqlDbType.NVarChar).Value = procDate;
            adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.NVarChar).Value = customerId;
            adp.SelectCommand.Parameters.Add("@VerificationID", SqlDbType.NVarChar).Value = verificationId;
            DataSet ds = new DataSet();
            adp.Fill(ds);
            var objectlst = new List<BranchCodeList>();
            BranchCodeList def;
            //logerror("FetchBranchCodeListData", "count - " + ds.Tables[0].Rows.Count);
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
            //logerror("FetchBranchCodeListData", "objectlst - " + objectlst);
            return objectlst;
        }

        [HttpPost]
        public ActionResult SelectionForBranchCode(SelectionForBranchCode selectionForBranchCode)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }

            if ((bool)Session["VF"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            int verificationId = 0;
            if (Request.Form["BranchSelect"] != null)
                Session["BranchID"] = Request.Form["BranchSelect"].ToString();
            try
            {
                var branch = Session["BranchID"].ToString();
                //logerror("SelectionForBranchCode", "branch Id - " + branch);
                return RedirectToAction("Index", "OWCMSVerification", new { id = verificationId, branchId = branch });
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

        public ActionResult Index(int id = 0, string branchId = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            if ((bool)Session["VF"] == false)
            {
                //int uid = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            

            try
            {
                string VFType = "";
                VFType = "CMS";
                Session["VFType"] = VFType;
                Session["BranchCode"] = branchId;

                //logerror("OWCMSVerification Index", "parameter uid - " + uid);
                //logerror("OWCMSVerification Index", "parameter ProcessingDate - " + Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"));
                //logerror("OWCMSVerification Index", "parameter CustomerID - " + Convert.ToInt16(Session["CustomerID"]));
                //logerror("OWCMSVerification Index", "parameter DomainId - " + Convert.ToInt32(Session["DomainselectID"]));
                //logerror("OWCMSVerification Index", "parameter CtsSessionType - " + Session["CtsSessionType"]);
                //logerror("OWCMSVerification Index", "parameter VFtype - " + VFType);
                //logerror("OWCMSVerification Index", "parameter BranchCodeId - " + branchId);

                SqlDataAdapter adp = new SqlDataAdapter("OWSelectCMSL2", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();
                //-------------Added on 17-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                //-------------Added on 18-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);
                //-------------Added on 12-09-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = Session["CtsSessionType"];
                //---------------------------------------Added on 02-02-2018-----------------------------
                adp.SelectCommand.Parameters.Add("@VFtype", SqlDbType.NVarChar).Value = VFType;

                adp.SelectCommand.Parameters.Add("@BranchCodeId", SqlDbType.NVarChar).Value = branchId;

                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<CMS_L2verificationModel>();
                CMS_L2verificationModel def;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    def = new CMS_L2verificationModel
                    {
                        Id = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]),
                        BatchNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[1]),
                        BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[2]),
                        InstrumentType = ds.Tables[0].Rows[0].ItemArray[3].ToString(),
                        ClearingType = ds.Tables[0].Rows[0].ItemArray[4].ToString(),
                        SlipNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[5].ToString()),
                        SlipChequeCount = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[6].ToString()),
                        SlipAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[7]),
                        ChequeAmountTotal = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[8]),
                        Status = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[9]),
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[10].ToString(),
                        FrontGreyImagePath = ds.Tables[0].Rows[0].ItemArray[11].ToString(),
                        BackTiffImagePath = ds.Tables[0].Rows[0].ItemArray[12].ToString(),
                        ClientCode = ds.Tables[0].Rows[0].ItemArray[13].ToString(),
                        SlipRefNo = ds.Tables[0].Rows[0].ItemArray[14].ToString(),
                        CreditAccountNo = ds.Tables[0].Rows[0].ItemArray[15].ToString(),
                        BranchCode = ds.Tables[0].Rows[0].ItemArray[16].ToString(),
                        ProcessingDate = Convert.ToDateTime(ds.Tables[0].Rows[0].ItemArray[17].ToString()),
                        ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[18].ToString()),
                        RawDataId = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[19].ToString()),
                        DomainId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[21].ToString()),
                        CustomerId = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[22].ToString()),
                        ScanningType = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[23].ToString()),

                        PayeeName = ds.Tables[0].Rows[0].ItemArray[27].ToString(),
                        SlipDate = ds.Tables[0].Rows[0].ItemArray[26].ToString(),
                        PickupLocationId = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[28].ToString()),
                        PickupLocationCode = ds.Tables[0].Rows[0].ItemArray[29].ToString(),
                        NoOfInstrument = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[30].ToString()),

                        callby = "Slip",
                    };
                    objectlst.Add(def);

                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        def = new CMS_L2verificationModel
                        {
                            Id = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),
                            BatchNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[1]),
                            BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[2]),
                            InstrumentType = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                            ClearingType = ds.Tables[0].Rows[index].ItemArray[4].ToString(),
                            SlipNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[5].ToString()),
                            SlipChequeCount = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[6].ToString()),
                            SlipAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[7]),
                            ChequeAmountTotal = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[8]),
                            Status = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[9]),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[10].ToString(),
                            FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[11].ToString(),
                            BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[12].ToString(),
                            ClientCode = ds.Tables[0].Rows[index].ItemArray[13].ToString(),
                            SlipRefNo = ds.Tables[0].Rows[index].ItemArray[14].ToString(),
                            CreditAccountNo = ds.Tables[0].Rows[index].ItemArray[15].ToString(),
                            BranchCode = ds.Tables[0].Rows[index].ItemArray[16].ToString(),
                            ProcessingDate = Convert.ToDateTime(ds.Tables[0].Rows[index].ItemArray[17].ToString()),
                            ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[18].ToString()),
                            RawDataId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[19].ToString()),
                            DomainId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[21].ToString()),
                            CustomerId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[22].ToString()),
                            ScanningType = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[23].ToString()),
                            callby = "Slip",

                            PayeeName = ds.Tables[0].Rows[index].ItemArray[27].ToString(),
                            SlipDate = ds.Tables[0].Rows[index].ItemArray[26].ToString(),
                            PickupLocationId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[28].ToString()),
                            PickupLocationCode = ds.Tables[0].Rows[index].ItemArray[29].ToString(),
                            NoOfInstrument = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[30].ToString()),

                        };
                        //ViewBag.cnt = true;
                        objectlst.Add(def);
                        count = count - 1;
                        index = index + 1;
                    }

                    var rtnlist = (from i in af.ItemReturnReasons select i).ToList();// iwafl.ItemReturnReasons.Select(m).ToList();
                    ViewBag.rtnlist = rtnlist.Select(m => m.RETURN_REASON_CODE).ToList();
                    ViewBag.rtnlistDescrp = rtnlist.Select(m => m.DESCRIPTION).ToList();

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

                logger.Log(LogLevel.Error, "OWCMSL2 HttpGet Index|" + message + "INNEREXP| " + innerExcp, "Login Index-Msg");
                // logger.Log(LogLevel.Error, e.InnerException.StackTrace, "Login Index-Exception");

                return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });
            }

        }

        [HttpPost]
        public ActionResult OWCMSVerification(List<string> lst, bool snd, string img = null, string btnClose = "default", List<Int64> idlst = null, double ChequeAmountTotal = 0)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            if ((bool)Session["VF"] == false)
            {
                //int uid = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            try
            {
                int ttcnt = 0;

                if (lst != null)
                    ttcnt = lst.Count() / 34;

                byte rejct = 0;
                string rejectreasondescrpsn = "";
                string Clearingtype = "";
                string payeename = "Not Found";
                string draweename = "";
                DataSet ds = new DataSet();
                ObjectClass os = new ObjectClass();
                string instrumenttype = "";
                Int64 SlipID = 0;
                Int64 SlipRawaDataID = 0;
                string Modified = "";
                string LICNO = "";
                string Hirarchylink = "";
                string divisionlink = "";
                byte ScanningType = 0;
                string modaction = "";
                string actionTaken = "";
                //L1Verification def;
                var objectlst = new List<CMS_L2verificationModel>();
                Int64 id = 0;
                if (btnClose == "Close" && lst != null)
                    ttcnt = ttcnt + 1;
                else if (btnClose == "Close" && lst == null)
                    goto Close;
                instrumenttype = lst[7].ToString();

                if (lst[7].ToString() == "S")
                {
                    string tempclientcd = "";
                    string slipDate = "";
                    string finaldate = "";
                    string userNarration = "";
                    string slipRefNo = "";
                    decimal slpAmt = 0;
                    int noOfInstrument = 0;
                    string accNo = "";
                    string pickupLocCode = "";
                    Int64 pickupLocId = 0;

                    if (ttcnt == 1)
                    {
                        if (lst[11] != null && lst[11].ToString() != "")
                            tempclientcd = lst[11].ToString();

                        if (lst[12] != null)
                            pickupLocCode = lst[12].ToString();

                        if (lst[13] != null && lst[13].ToString() != "")
                            pickupLocId = Convert.ToInt64(lst[13]);

                        if (lst[14] != null)
                            slipRefNo = lst[14].ToString();

                        if (lst[15] != null)
                            accNo = lst[15].ToString();

                        if (lst[16] != null && lst[16].ToString() != "")
                            slpAmt = Convert.ToDecimal(lst[16].ToString());

                        if (lst[27] != null)
                            userNarration = lst[27].ToString();

                        if (lst[24] != null && lst[24].ToString() != "")
                            noOfInstrument = Convert.ToInt16(lst[24]);

                        if (lst[25] != null)
                            payeename = lst[25].ToString();

                        if (lst[26] != null)
                            draweename = lst[26].ToString();

                        if (lst[28] != null)
                            SlipID = Convert.ToInt64(lst[28]);

                        if (lst[8] != null)
                            ScanningType = Convert.ToByte(lst[8]);


                        if (lst[17] != null && lst[17].ToString() != "")
                        {
                            if (lst[17].ToString().Length != 10)
                                slipDate = "20" + lst[17].ToString().Substring(4, 2) + "-" + lst[17].ToString().Substring(2, 2) + "-" + lst[17].ToString().Substring(0, 2);
                            else
                                slipDate = lst[17].ToString();
                        }

                        if (lst[31] != null && lst[31].ToString() != "")
                            actionTaken = lst[31].ToString();

                        if (lst[32] != null && lst[32].ToString() != "")
                            rejct = Convert.ToByte(lst[32].ToString());

                        if (rejct == 88)
                        {
                            if (lst[33] != null)
                                rejectreasondescrpsn = lst[33].ToString();
                            else
                                rejectreasondescrpsn = "Other Reason";

                        }

                        SqlCommand com = new SqlCommand("CMS_Update_SlipAndChequeDataL2", con);

                        com.CommandType = CommandType.StoredProcedure;

                        //com.Parameters.Add("@SlipID", SqlDbType.BigInt).Value = Convert.ToInt64(lst[0]);
                        //com.Parameters.Add("@SlipRawDataID", SqlDbType.BigInt).Value = Convert.ToInt64(lst[10]);
                        //com.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                        //com.Parameters.Add("@CustomerId", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]); 
                        //com.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);
                        //com.Parameters.Add("@BranchCode", SqlDbType.NVarChar).Value = lst[3].ToString();
                        //com.Parameters.Add("@ScanningNodeId", SqlDbType.NVarChar).Value = lst[4].ToString();
                        //com.Parameters.Add("@ScanningType", SqlDbType.NVarChar).Value = lst[8].ToString();
                        //com.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = lst[5].ToString();
                        //com.Parameters.Add("@SlipNo", SqlDbType.NVarChar).Value = lst[9].ToString();
                        //com.Parameters.Add("@SlipRefNo", SqlDbType.NVarChar).Value = lst[14].ToString();
                        //com.Parameters.Add("@SlipDate", SqlDbType.NVarChar).Value = lst[17].ToString();
                        //com.Parameters.Add("@SlipAmount", SqlDbType.Decimal).Value = lst[16].ToString();
                        //com.Parameters.Add("@ClientCode", SqlDbType.NVarChar).Value = lst[11].ToString();
                        //com.Parameters.Add("@PayeeName", SqlDbType.NVarChar).Value = lst[25].ToString();
                        //com.Parameters.Add("@PickupLocationCode", SqlDbType.NVarChar).Value = lst[12].ToString();
                        //com.Parameters.Add("@PickupLocationId", SqlDbType.BigInt).Value = lst[13].ToString();
                        //com.Parameters.Add("@NoOfInstrument", SqlDbType.Int).Value = lst[24].ToString();
                        //com.Parameters.Add("@UserId", SqlDbType.Int).Value = uid;
                        //com.Parameters.Add("@AccountNo", SqlDbType.NVarChar).Value = lst[15].ToString();

                        com.Parameters.AddWithValue("@SlipID", Convert.ToInt64(lst[0]));
                        com.Parameters.AddWithValue("@SlipRawDataID", Convert.ToInt64(lst[10]));
                        com.Parameters.AddWithValue("@ProcessingDate", Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"));
                        com.Parameters.AddWithValue("@CustomerId", Convert.ToInt16(Session["CustomerID"]));
                        com.Parameters.AddWithValue("@DomainId", Convert.ToInt32(lst[2]));
                        com.Parameters.AddWithValue("@BranchCode", lst[3].ToString());
                        com.Parameters.AddWithValue("@ScanningNodeId", lst[4].ToString());
                        com.Parameters.AddWithValue("@ScanningType", lst[8].ToString());
                        com.Parameters.AddWithValue("@BatchNo", lst[5].ToString());
                        com.Parameters.AddWithValue("@SlipNo", lst[9].ToString());
                        com.Parameters.AddWithValue("@SlipRefNo", slipRefNo.ToString());
                        com.Parameters.AddWithValue("@SlipDate", slipDate.ToString());
                        com.Parameters.AddWithValue("@SlipAmount", slpAmt);
                        com.Parameters.AddWithValue("@ClientCode", tempclientcd.ToString());
                        com.Parameters.AddWithValue("@PayeeName", payeename.ToString());
                        com.Parameters.AddWithValue("@PickupLocationCode", pickupLocCode.ToString());
                        com.Parameters.AddWithValue("@PickupLocationId", pickupLocId);
                        com.Parameters.AddWithValue("@NoOfInstrument", noOfInstrument);
                        com.Parameters.AddWithValue("@UserId", uid);
                        com.Parameters.AddWithValue("@AccountNo", accNo.ToString());
                        com.Parameters.Add("@ActionTaken", SqlDbType.NVarChar).Value = actionTaken;
                        com.Parameters.Add("@RejectReason", SqlDbType.TinyInt).Value = rejct;
                        com.Parameters.Add("@RejectReasonDescription", SqlDbType.NVarChar).Value = rejectreasondescrpsn;
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();


                        if (lst[11] != null)
                            tempclientcd = lst[11].ToString();

                        objectlst = os.selectCMSL2Cheques(con, uid, Session["LoginID"].ToString(), lst,
                            Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), img, "Slip", tempclientcd,
                            Convert.ToInt16(Session["CustomerID"]), Convert.ToInt32(Session["DomainselectID"]), false,
                            Session["CtsSessionType"].ToString(), Session["VFType"].ToString(), Session["BranchCode"].ToString());
                    }
                }
                else if (lst[7].ToString() == "C")
                {
                    string finaldate = "";
                    string tempclientcd = "";
                    string userNarration = "";
                    string creditcardno = "";

                    if (ttcnt == 1)
                    {
                        for (int i = 0; i < ttcnt; i++)
                        {
                            finaldate = "";
                            tempclientcd = "";
                            userNarration = "";
                            creditcardno = "";
                            string slipRefNo = "";
                            decimal finalAmt = 0;

                            id = Convert.ToInt64(lst[0]);

                            if (lst[19] != null && lst[19].ToString() != "")
                            {
                                if (lst[19].ToString().Length != 10)
                                    finaldate = "20" + lst[19].ToString().Substring(4, 2) + "-" + lst[19].ToString().Substring(2, 2) + "-" + lst[19].ToString().Substring(0, 2);
                                else
                                    finaldate = lst[19].ToString();
                            }
                            if (lst[11] != null && lst[11].ToString() != "")
                                tempclientcd = lst[11].ToString();

                            if (lst[14] != null)
                                slipRefNo = lst[14].ToString();

                            if (lst[18] != null && lst[18].ToString() != "")
                                finalAmt = Convert.ToDecimal(lst[18].ToString());

                            if (lst[27] != null)
                                userNarration = lst[27].ToString();

                            if (lst[25] != null)
                                payeename = lst[25].ToString();

                            if (lst[26] != null)
                                draweename = lst[26].ToString();

                            if (lst[28] != null)
                                SlipID = Convert.ToInt64(lst[28]);

                            if (lst[8] != null)
                                ScanningType = Convert.ToByte(lst[8]);

                            if (lst[31] != null && lst[31].ToString() != "")
                                actionTaken = lst[31].ToString();

                            if (lst[32] != null && lst[32].ToString() != "")
                                rejct = Convert.ToByte(lst[32].ToString());

                            if (rejct == 88)
                            {
                                if (lst[33] != null)
                                    rejectreasondescrpsn = lst[33].ToString();
                                else
                                    rejectreasondescrpsn = "Other Reason";

                            }

                            //====== update cheque start ==============

                            SqlCommand com = new SqlCommand("CMS_Update_ChequeDataL2", con);

                            com.CommandType = CommandType.StoredProcedure;
                            com.Parameters.Add("@ID", SqlDbType.BigInt).Value = Convert.ToInt64(lst[0]);
                            com.Parameters.Add("@RawDataID", SqlDbType.BigInt).Value = Convert.ToInt64(lst[10]);
                            com.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                            com.Parameters.Add("@CustomerId", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                            com.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);
                            com.Parameters.Add("@BranchCode", SqlDbType.NVarChar).Value = lst[3].ToString();
                            com.Parameters.Add("@ScanningNodeId", SqlDbType.NVarChar).Value = lst[4].ToString();
                            com.Parameters.Add("@ScanningType", SqlDbType.NVarChar).Value = lst[8].ToString();
                            com.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = lst[5].ToString();
                            com.Parameters.Add("@SlipNo", SqlDbType.NVarChar).Value = lst[9].ToString();
                            com.Parameters.Add("@SlipRefNo", SqlDbType.NVarChar).Value = slipRefNo.ToString();
                            com.Parameters.Add("@FinalDate", SqlDbType.NVarChar).Value = finaldate.ToString();
                            com.Parameters.Add("@FinalAmount", SqlDbType.Decimal).Value = finalAmt;
                            com.Parameters.Add("@ClientCode", SqlDbType.NVarChar).Value = tempclientcd.ToString();
                            com.Parameters.Add("@PayeeName", SqlDbType.NVarChar).Value = payeename.ToString();
                            com.Parameters.Add("@DraweeName", SqlDbType.NVarChar).Value = draweename.ToString();
                            com.Parameters.Add("@UserNarration", SqlDbType.NVarChar).Value = userNarration.ToString();
                            //com.Parameters.Add("@NoOfInstrument", SqlDbType.Int).Value = lst[24].ToString();
                            com.Parameters.Add("@UserId", SqlDbType.Int).Value = uid;
                            com.Parameters.Add("@ActionTaken", SqlDbType.NVarChar).Value = actionTaken;
                            com.Parameters.Add("@RejectReason", SqlDbType.TinyInt).Value = rejct;
                            com.Parameters.Add("@RejectReasonDescription", SqlDbType.NVarChar).Value = rejectreasondescrpsn;
                            //com.Parameters.Add("@AccountNo", SqlDbType.NVarChar).Value = lst[15].ToString();
                            con.Open();
                            com.ExecuteNonQuery();
                            con.Close();
                            //con.Dispose();

                            //====== update cheque end ==============

                            for (int k = 0; k < idlst.Count; k++)
                            {
                                if (idlst[k] == id)
                                    idlst.RemoveAt(k);
                            }
                        }

                        if (btnClose == "Close")
                            goto Close;
                    }
                    else
                    {
                        for (int i = 0; i < ttcnt - 1; i++)
                        {
                            finaldate = "";
                            tempclientcd = "";
                            userNarration = "";
                            creditcardno = "";
                            string slipRefNo = "";
                            decimal finalAmt = 0;


                            id = Convert.ToInt64(lst[0]);

                            if (lst[19] != null && lst[19].ToString() != "")
                            {
                                if (lst[19].ToString().Length != 10)
                                    finaldate = "20" + lst[19].ToString().Substring(4, 2) + "-" + lst[19].ToString().Substring(2, 2) + "-" + lst[19].ToString().Substring(0, 2);
                                else
                                    finaldate = lst[19].ToString();
                            }
                            if (lst[11] != null && lst[11].ToString() != "")
                                tempclientcd = lst[11].ToString();

                            if (lst[14] != null)
                                slipRefNo = lst[14].ToString();

                            if (lst[18] != null && lst[18].ToString() != "")
                                finalAmt = Convert.ToDecimal(lst[18].ToString());

                            if (lst[27] != null)
                                userNarration = lst[27].ToString();

                            if (lst[25] != null)
                                payeename = lst[25].ToString();

                            if (lst[26] != null)
                                draweename = lst[26].ToString();

                            if (lst[28] != null)
                                SlipID = Convert.ToInt64(lst[28]);

                            if (lst[8] != null)
                                ScanningType = Convert.ToByte(lst[8]);

                            if (lst[31] != null && lst[31].ToString() != "")
                                actionTaken = lst[31].ToString();

                            if (lst[32] != null && lst[32].ToString() != "")
                                rejct = Convert.ToByte(lst[32].ToString());

                            if (rejct == 88)
                            {
                                if (lst[33] != null)
                                    rejectreasondescrpsn = lst[33].ToString();
                                else
                                    rejectreasondescrpsn = "Other Reason";

                            }

                            //====== update cheque start ==============

                            SqlCommand com = new SqlCommand("CMS_Update_ChequeDataL2", con);

                            com.CommandType = CommandType.StoredProcedure;
                            com.Parameters.Add("@ID", SqlDbType.BigInt).Value = Convert.ToInt64(lst[0]);
                            com.Parameters.Add("@RawDataID", SqlDbType.BigInt).Value = Convert.ToInt64(lst[10]);
                            com.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                            com.Parameters.Add("@CustomerId", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                            com.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);
                            com.Parameters.Add("@BranchCode", SqlDbType.NVarChar).Value = lst[3].ToString();
                            com.Parameters.Add("@ScanningNodeId", SqlDbType.NVarChar).Value = lst[4].ToString();
                            com.Parameters.Add("@ScanningType", SqlDbType.NVarChar).Value = lst[8].ToString();
                            com.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = lst[5].ToString();
                            com.Parameters.Add("@SlipNo", SqlDbType.NVarChar).Value = lst[9].ToString();
                            com.Parameters.Add("@SlipRefNo", SqlDbType.NVarChar).Value = slipRefNo.ToString();
                            com.Parameters.Add("@FinalDate", SqlDbType.NVarChar).Value = finaldate.ToString();
                            com.Parameters.Add("@FinalAmount", SqlDbType.Decimal).Value = finalAmt;
                            com.Parameters.Add("@ClientCode", SqlDbType.NVarChar).Value = tempclientcd.ToString();
                            com.Parameters.Add("@PayeeName", SqlDbType.NVarChar).Value = payeename.ToString();
                            com.Parameters.Add("@DraweeName", SqlDbType.NVarChar).Value = draweename.ToString();
                            com.Parameters.Add("@UserNarration", SqlDbType.NVarChar).Value = userNarration.ToString();
                            //com.Parameters.Add("@NoOfInstrument", SqlDbType.Int).Value = lst[24].ToString();
                            com.Parameters.Add("@UserId", SqlDbType.Int).Value = uid;
                            com.Parameters.Add("@ActionTaken", SqlDbType.NVarChar).Value = actionTaken;
                            com.Parameters.Add("@RejectReason", SqlDbType.TinyInt).Value = rejct;
                            com.Parameters.Add("@RejectReasonDescription", SqlDbType.NVarChar).Value = rejectreasondescrpsn;
                            //com.Parameters.Add("@AccountNo", SqlDbType.NVarChar).Value = lst[15].ToString();
                            con.Open();
                            com.ExecuteNonQuery();
                            con.Close();
                            //con.Dispose();

                            //====== update cheque end ==============

                            for (int k = 0; k < idlst.Count; k++)
                            {
                                if (idlst[k] == id)
                                    idlst.RemoveAt(k);
                            }
                            lst.RemoveRange(0, 34);
                        }
                        if (btnClose == "Close")
                            goto Close;
                    }

                    objectlst = os.selectCMSL2Cheques(con, uid, Session["LoginID"].ToString(), lst,
                            Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), img, "Cheq", tempclientcd,
                            Convert.ToInt32(Session["CustomerID"]), Convert.ToInt32(Session["DomainselectID"]), false,
                            Session["CtsSessionType"].ToString(), Session["VFType"].ToString(), Session["BranchCode"].ToString());

                }

            //---------------------------IF Close button called--------------------------------//
            Close:
                if (btnClose == "Close")
                {

                    /// Int64 SlipRawaDataID = 0;
                    Session["glob"] = true;
                    for (int p = 0; p < idlst.Count; p++)
                    {
                        //OWpro.OWUnlockRecords(idlst[p], "L2CMS");

                        SqlCommand com = new SqlCommand("OWUnlockRecords", con);
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@id", idlst[p]);
                        com.Parameters.AddWithValue("@module", "L2CMS");

                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                    }
                    //if (lst[34] != null)
                    //    SlipRawaDataID = Convert.ToInt64(lst[34]);
                    if (instrumenttype == "C")
                    {
                        //OWpro.OWUnlockRecords(SlipID, "L2CMS");

                        SqlCommand com = new SqlCommand("OWUnlockRecords", con);
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@id", SlipID);
                        com.Parameters.AddWithValue("@module", "L2CMS");

                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                    }
                        


                    return Json(false);
                }

                //-------------Calling next Records---------------

                if (objectlst.Count != 0 || objectlst != null)
                {
                    ViewBag.cnt = true;
                    return Json(objectlst);
                }
            exitblock:
                Session["glob"] = true;
                return Json(false);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                logger.Log(LogLevel.Error, "OWCMSDataEntry HttpPost OWCMSDataEntry|" + message + "INNEREXP| " + innerExcp, "OWCMSDataEntry Index-Msg");
                //logger.Log(LogLevel.Error, e.InnerException.StackTrace, "Login Index-Exception");

                return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });
            }
            finally
            {
                con.Close();
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

    }
}
