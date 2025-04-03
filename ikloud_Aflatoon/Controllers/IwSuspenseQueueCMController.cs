using ikloud_Aflatoon.Infrastructure;
using NPOI.OpenXmlFormats.Dml;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

namespace ikloud_Aflatoon.Controllers
{
    public class IwSuspenseQueueCMController : Controller
    {
        //
        // GET: /IwSuspenseQueueCM/
        AflatoonEntities af = new AflatoonEntities();
        //OWProcDataContext OWpro = new OWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
        List<string> lAccNames = new List<string>();

        public ActionResult Index(string branchId = null,string ExceptionId="")
        {
            logerror("In IwSuspenseQueue index== ", "");
            int uid = (int)Session["uid"];
            Session["CheckerOrMaker_New"] = Session["CheckerOrMaker"].ToString();
            int custid = Convert.ToInt16(Session["CustomerID"]);
            ViewBag.BankCode = Session["BankCode"].ToString();
            string bal = "0.0";
            string initialamt = "0.0";
            var IsAPI = ConfigurationManager.AppSettings["ISCMSQAPI"].ToString();
            suspenseQcbsdata obj=new suspenseQcbsdata();
            obj.CurrentAccBal = "0.0";
            logerror("In IwSuspenseQueue index==IsAPI==> ", IsAPI.ToString());
            logerror("In IwSuspenseQueue index==>obj.CurrentAccBal==> ", obj.CurrentAccBal);

            try
            {
                SqlDataAdapter adp = new SqlDataAdapter("IwSelectSuspenseQueue_CM", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                //adp.SelectCommand.Parameters.Add("@VFtype", SqlDbType.NVarChar).Value = VFType;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                //adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(4);
                //adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = Session["CtsSessionType"];
                adp.SelectCommand.Parameters.Add("@BranchCode", SqlDbType.NVarChar).Value = branchId;
                adp.SelectCommand.Parameters.Add("@ExceptionId", SqlDbType.NVarChar).Value = ExceptionId;

                DataSet ds = new DataSet();
                adp.Fill(ds);
                IwSuspenseQueueViewCM def;
                var objectlst = new List<IwSuspenseQueueViewCM>();

               


                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        logerror("In IwSuspenseQueue index for loop====> ","");
                        if (IsAPI == "Y")
                        {
                            logerror("In IwSuspenseQueue index for loop====> ", "");
                            var acc = ds.Tables[0].Rows[i]["DbtAccNo"].ToString();
                            logerror("In IwSuspenseQueue index for loop====> ", acc.ToString());
                            obj = GetCurrentaccountBalancesFromCasaApi(acc);
                            logerror("In IwSuspenseQueue index for loop====>obj.CurrentAccBal==>obj.SOLID===>obj.PayeeName==> ", obj.CurrentAccBal.ToString()+" "+obj.SOLID.ToString()+" "+obj.PayeeName.Count.ToString());
                        }
                        string cbsdata = ds.Tables[0].Rows[i]["CBSClientAccountDtls"].ToString();
                        if (!string.IsNullOrEmpty(cbsdata)) 
                        {
                            string[] amtparts = cbsdata.Split('|');
                            if(amtparts.Length > 5)
                            {
                                initialamt = amtparts[5];
                                obj.SOLID = amtparts[11];
                                //obj.CurrentAccBal = amtparts[5];
                            }
                        }
                       
                       


                        def = new IwSuspenseQueueViewCM
                        {
                            Id = Convert.ToInt64(ds.Tables[0].Rows[i]["Id"]),
                            AccountNumber= ds.Tables[0].Rows[i]["DbtAccNo"].ToString(),
                            AccName =obj.PayeeName, //ds.Tables[0].Rows[i]["AccountName"].ToString(),
                            SOLID=obj.SOLID,
                            InitialAvailableBalance= initialamt,
                            CurrentAvailableBalance= obj.CurrentAccBal,
                            ChequeNumber = ds.Tables[0].Rows[i]["EntrySerialNo"].ToString(),
                            Amount = Convert.ToDecimal((ds.Tables[0].Rows[i]["XMLAmount"] == DBNull.Value ? 0 : ds.Tables[0].Rows[i]["XMLAmount"])),
                            RBISequenceNo= ds.Tables[0].Rows[i]["ItemSeqNo"].ToString(),
                            CPPS_Flag= ds.Tables[0].Rows[i]["CPPS_FLAG"].ToString(),
                            ExceptionReson = ds.Tables[0].Rows[i]["ExceptionRejectDescription"].ToString(),
                            ExceptionID= ds.Tables[0].Rows[i]["ExceptionById"].ToString(),
                            MakerDecision= ds.Tables[0].Rows[i]["SQMakerDecision"].ToString(),
                            MakerId= ds.Tables[0].Rows[i]["MakerId"].ToString(),
                            CheckerDecision= ds.Tables[0].Rows[i]["SQCheckerDecision"].ToString(),
                            CheckerId= ds.Tables[0].Rows[i]["CheckerId"].ToString(),

                            SQMakerId= ds.Tables[0].Rows[i]["SQMakerId"].ToString(),
                            SQCheckerId= ds.Tables[0].Rows[i]["SQCheckerId"].ToString(), 

                            ExpiryTime = Convert.ToDateTime(ds.Tables[0].Rows[i]["ItemExpiryTimeStamp"].ToString()),
                        };
                        objectlst.Add(def);
                        initialamt = "0.0";
                        bal = "0.0";
                        obj.CurrentAccBal = "0.0";
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
                logerror("in Index get catch==>" + message, "innerexp==>" + innerExcp);
                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "Selection for branch code post - " + innerExcp });
            }



           
        }


        public class BranchCodeList
        {
            public string BranchCode { get; set; }
            public string BranchCodeName { get; set; }
        }
        public ActionResult SelectionForBranchCode(int id = 0,string flag=null)
        {
            SelectionForBranchCode selectionForBranchCode = new SelectionForBranchCode();
            try
            {
                if (Session["uid"] != null)
                {
                    int uid = (int)Session["uid"];
                    int custid = Convert.ToInt16(Session["CustomerID"]);
                   
                    string customerId = Session["CustomerID"].ToString();

                    if (flag == "maker") { Session["CheckerOrMaker"] = "Maker"; }else if (flag == "checker") { Session["CheckerOrMaker"] = "Checker"; }
                   
                    string processingDate = Session["processdate"].ToString();
                    DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                    string procDate = processingDate1.ToString("yyyy-MM-dd");
                    GetExceptionFilter();
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
        public List<BranchCodeList> FetchBranchCodeListData()
        {
            var objectlst = new List<BranchCodeList>();
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());

                int customerId = Convert.ToInt32(Session["CustomerID"].ToString());
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                SqlDataAdapter adp = new SqlDataAdapter("IW_SuspenseQueue_SelectBranchCodeListCM", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                //adp.SelectCommand.Parameters.Add("@DomainID", SqlDbType.NVarChar).Value = 4;
                //adp.SelectCommand.Parameters.Add("@DomainID", SqlDbType.NVarChar).Value = Session["domainid"];
                adp.SelectCommand.Parameters.Add("@DomainID", SqlDbType.NVarChar).Value = 0;
                adp.SelectCommand.Parameters.Add("@ProcDate", SqlDbType.NVarChar).Value = procDate;
                adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.NVarChar).Value = customerId;
                //adp.SelectCommand.Parameters.Add("@VerificationID", SqlDbType.NVarChar).Value = verificationId;
                DataSet ds = new DataSet();
                adp.Fill(ds);

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
            }
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;
               
                logerror("in FetchBranchCodeListData catch==>" + message, "innerexp==>"+innerExcp);
            }

            
            return objectlst;
        }

        [HttpPost]
        public ActionResult SelectionForBranchCode(SelectionForBranchCode selectionForBranchCode)
        {
            //int verificationId = Convert.ToInt16(Session["VerificationId"].ToString()); 
            if (Request.Form["BranchSelect"] != null)
                Session["BranchID"] = Request.Form["BranchSelect"].ToString();

            if(Request.Form["ExceptionFilter"] != null)
                Session["ExceptionFilterId"]= Request.Form["ExceptionFilter"].ToString() ;

            if (Request.Form["role"]!=null)
                Session["CheckerOrMaker"]= Request.Form["role"].ToString();



            try
            {
                var branch = Session["BranchID"].ToString();
                var strExceptionId = Session["ExceptionFilterId"].ToString();
                return RedirectToAction("Index", "IwSuspenseQueueCM", new { branchId = branch , ExceptionId= strExceptionId });
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;
                logerror("in SelectionForBranchCode Post catch ==> " + message, " innerexp==>" + innerExcp);

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "Selection for branch code post - " + innerExcp });
            }
        }


        public ActionResult ShowIwSuspenseQueueDetailsCM(Int64 Id = 0)
        {
            try
            {
                string bal = "0.0";
                string initialamt = "0.0";
                var IsAPI = ConfigurationManager.AppSettings["ISCMSQAPI"].ToString();
                suspenseQcbsdata obj = new suspenseQcbsdata();
                obj.CurrentAccBal = "0.0";

                Session["RecordId"] = Id;
                SqlDataAdapter adp = new SqlDataAdapter("GetIwSuspenceDetails", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@Id", SqlDbType.NVarChar).Value = Id;

                DataSet ds = new DataSet();
                adp.Fill(ds);

                IwSuspenseQueueViewCM def = new IwSuspenseQueueViewCM();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (IsAPI == "Y")
                    {
                        var acc = ds.Tables[0].Rows[0]["DbtAccNo"].ToString();
                        obj = GetCurrentaccountBalancesFromCasaApi(acc);
                    }
                    string cbsdata = ds.Tables[0].Rows[0]["CBSClientAccountDtls"].ToString();
                    if (!string.IsNullOrEmpty(cbsdata))
                    {
                        string[] amtparts = cbsdata.Split('|');
                        if (amtparts.Length > 5)
                        {
                            initialamt = amtparts[5];
                        }
                    }




                    def = new IwSuspenseQueueViewCM
                    {
                        Id = Convert.ToInt64(ds.Tables[0].Rows[0]["Id"]),
                        AccountNumber= ds.Tables[0].Rows[0]["DbtAccNo"].ToString(),
                        AccountName= ds.Tables[0].Rows[0]["EntryPayeeName"].ToString(),
                        SOLID = "",
                        InitialAvailableBalance = initialamt,
                        CurrentAvailableBalance = obj.CurrentAccBal,
                        ChequeNumber = ds.Tables[0].Rows[0]["EntrySerialNo"].ToString(),
                        Amount = Convert.ToDecimal((ds.Tables[0].Rows[0]["XMLAmount"] == DBNull.Value ? 0 : ds.Tables[0].Rows[0]["XMLAmount"])),
                        RBISequenceNo = ds.Tables[0].Rows[0]["ItemSeqNo"].ToString(),//ExceptionRejectDescription
                        ExceptionReson = ds.Tables[0].Rows[0]["ExceptionRejectDescription"].ToString(),//ExceptionById
                        ExceptionID = ds.Tables[0].Rows[0]["ExceptionById"].ToString(),
                        FrontGrayImg= ds.Tables[0].Rows[0]["FrontGreyImagePath"].ToString(),
                        FrontTiffImg= ds.Tables[0].Rows[0]["FrontTiffImagePath"].ToString(),
                        BackGrayImg= ds.Tables[0].Rows[0]["BackGreyImagePath"].ToString(),
                        BackTiffImg= ds.Tables[0].Rows[0]["BackTiffImagePath"].ToString(),

                        XmlSerialNo = ds.Tables[0].Rows[0]["XMLSerialNo"].ToString(),
                        XmlSan= ds.Tables[0].Rows[0]["XMLSAN"].ToString(),
                        XmlTc= ds.Tables[0].Rows[0]["XMLTransCode"].ToString(),
                        XmlSortCode= ds.Tables[0].Rows[0]["XMLPayorBankRoutNo"].ToString(),

                        EntrySerialNo= ds.Tables[0].Rows[0]["EntrySerialNo"].ToString(),
                        EntrySortCode= ds.Tables[0].Rows[0]["EntryPayorBankRoutNo"].ToString(),
                        EntrySan= ds.Tables[0].Rows[0]["EntrySAN"].ToString(),
                        EntryTc= ds.Tables[0].Rows[0]["EntryTransCode"].ToString(),


                        MakerReturnReason = ds.Tables[0].Rows[0]["SQMakerReturnReason"].ToString(),
                        MakerReturnReasonDiscription= ds.Tables[0].Rows[0]["SQMakerReturnReasonDiscription"].ToString(),
                        CheckerSendBackToMakerDiscription= ds.Tables[0].Rows[0]["SQSendBacktoMakerDiscription"].ToString(),

                        MakerDecision= ds.Tables[0].Rows[0]["SQMakerDecision"].ToString(),
                        SQAccountName= ds.Tables[0].Rows[0]["SQAccountName"].ToString(),
                        SQAccountNumber= ds.Tables[0].Rows[0]["SQAccountNumber"].ToString(),

                        SQEntrySerialNo = ds.Tables[0].Rows[0]["SQEntrySerialNo"].ToString(),
                        SQEntryPayorBankRoutNo = ds.Tables[0].Rows[0]["SQEntryPayorBankRoutNo"].ToString(), //sortcode
                        SQEntryTransCode = ds.Tables[0].Rows[0]["SQEntryTransCode"].ToString(),
                        SQEntrySAN = ds.Tables[0].Rows[0]["SQEntrySAN"].ToString(),
                        API_Data=obj.API_Data,





                    };

                    //ViewBag.DecisionTaken = Convert.ToByte(ds.Tables[0].Rows[0]["Status"]);
                    //ViewBag.RejectCode = ds.Tables[0].Rows[0]["RejectReason"].ToString();
                }

                

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

                ExtensionRejectReason();

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


        public JsonResult UpdateSQData(Int64 id=0, string makerchecker=null,string branchId=null,string ExceptionId=null,string ChqNo=null,string XmlSortCode=null,string XmlSan=null,string XmlTc=null,string decision=null,string rejectReason=null,
                                       string rejectReasonDiscription=null,string Accnumber=null,string AccountName=null,string APiData=null)
        {
           
                    string msg = "";
                    int uid = (int)Session["uid"];
                 using (SqlCommand cmd = new SqlCommand("UpdateCMSuspenceQueueData", con))
                 {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@makerchecker", makerchecker);
                    cmd.Parameters.AddWithValue("@branchId", branchId);
                    cmd.Parameters.AddWithValue("@ExceptionId", ExceptionId);
                    cmd.Parameters.AddWithValue("@ChqNo", ChqNo);
                    cmd.Parameters.AddWithValue("@XmlSortCode", XmlSortCode);
                    cmd.Parameters.AddWithValue("@XmlSan", XmlSan);
                    cmd.Parameters.AddWithValue("@XmlTc", XmlTc);
                   
                    cmd.Parameters.AddWithValue("@decision", decision.ToUpper());
                    cmd.Parameters.AddWithValue("@rejectReason", rejectReason);
                    cmd.Parameters.AddWithValue("@rejectReasonDiscription", rejectReasonDiscription);
                    cmd.Parameters.AddWithValue("@UserId", uid);
                    cmd.Parameters.AddWithValue("@AccNumber", Accnumber);
                    cmd.Parameters.AddWithValue("@AccountName", AccountName);
                    cmd.Parameters.AddWithValue("@APiData", APiData);


                try
                {

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Data updated successfully.");
                    msg = "Update successfully";
                }
                catch (Exception e)
                {

                    string message = "";
                    string innerExcp = "";
                    if (e.Message != null)
                        message = e.Message.ToString();
                    if (e.InnerException != null)
                        innerExcp = e.InnerException.Message;
                    logerror("In UpdateSQData Catch ===>>" + message, "InnerExp==>" + innerExcp);
                    return Json(msg, JsonRequestBehavior.AllowGet);

                }
            }


            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateSendBackToMaker(Int64 id=0,string SendBckToMakerRemark = null)
        {
            string msg = "";
            int uid = (int)Session["uid"];
            using (SqlCommand cmd = new SqlCommand("UpdateCMSendBackToMaker", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@SendBckToMakerRemark", SendBckToMakerRemark);
                cmd.Parameters.AddWithValue("@UserId", uid);

                try
                {

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Data updated successfully.");
                    msg = "Remark Send successfully";
                }
                catch (Exception e)
                {

                    string message = "";
                    string innerExcp = "";
                    if (e.Message != null)
                        message = e.Message.ToString();
                    if (e.InnerException != null)
                        innerExcp = e.InnerException.Message;
                    logerror("In UpdateSendBackToMaker Catch ===>>" + message, "InnerExp==>" + innerExcp);
                    return Json(message, JsonRequestBehavior.AllowGet);

                }




            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }



        public JsonResult UpdateSQData_Checker(Int64 id=0,string makerchecker=null,string decision=null,string ReturnCode=null, string ReturnDescription=null)
        {
            string msg = "";
            int uid = (int)Session["uid"];
            using (SqlCommand cmd = new SqlCommand("UpdateCheckerAcceptStatus", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@UserId", uid);
                cmd.Parameters.AddWithValue("@makerchecker", makerchecker);
                cmd.Parameters.AddWithValue("@decision", decision);

                cmd.Parameters.AddWithValue("@ReturnCode", ReturnCode);
                cmd.Parameters.AddWithValue("@ReturnDescription", ReturnDescription);

               

                try
                {

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Data updated successfully.");
                    msg = "Save successfully";
                }
                catch (Exception e)
                {

                    string message = "";
                    string innerExcp = "";
                    if (e.Message != null)
                        message = e.Message.ToString();
                    if (e.InnerException != null)
                        innerExcp = e.InnerException.Message;
                    logerror("In UpdateSQData_Checker Catch ===>>" + message, "InnerExp==>" + innerExcp);
                    return Json(message, JsonRequestBehavior.AllowGet);

                }




            }
            return Json(msg, JsonRequestBehavior.AllowGet);
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



        public PartialViewResult ExtensionRejectReason(int id = 0)
        {
            var rjrs = new List<RejectReason>();


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
            ViewBag.rtnlistex = rjrs.Select(m => m.ReasonCodeS).ToList();
            ViewBag.rtnlistDescrpex = rjrs.Select(m => m.Description).ToList();

            return PartialView("_ExRejectReason", rjrs);
        }


        public void GetExceptionFilter()
        {
            try
            {
                SqlDataAdapter adp = new SqlDataAdapter("IW_GetExceptionFilter", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;

                DataSet ds = new DataSet();
                adp.Fill(ds);

                var objectlst = new List<ExceptionFilter>();
                ExceptionFilter def;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        def = new ExceptionFilter
                        {
                            ExReturnReasonCode = ds.Tables[0].Rows[i].ItemArray[0].ToString(),
                            ExReturnExceptionDiscription = ds.Tables[0].Rows[i].ItemArray[1].ToString()
                        };
                        objectlst.Add(def);
                    }
                }

                ViewBag.ExceptionFilter = new SelectList(objectlst, "ExReturnReasonCode", "ExReturnExceptionDiscription").ToList();
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
                logerror("in catch GetExceptionFilter==>" + e.Message, "InnerExp==>"+e.InnerException.ToString());
            }

           
        }




        //casa api for getting account balance 

        public suspenseQcbsdata GetCurrentaccountBalancesFromCasaApi(string ac=null)
        {
            string str = "0.0";
            Int64 openDate = 0;
            suspenseQcbsdata obj = new suspenseQcbsdata();
            //CASA variables
            var CasaClientId = ConfigurationManager.AppSettings["CasaClientId"].ToString();
            var CasaCorellationId = ConfigurationManager.AppSettings["CasaCorellationId"].ToString();
            var CasaServiceURL = ConfigurationManager.AppSettings["CasaServiceURL"].ToString();

            //Token variables
            var TokenClientId = ConfigurationManager.AppSettings["TokenClientId"].ToString();
            var TokenSecreteKey = ConfigurationManager.AppSettings["TokenSecreteKey"].ToString();
            var TokenServiceURL = ConfigurationManager.AppSettings["TokenServiceURL"].ToString();


            //CMPC variables
            var CMCPCountry = ConfigurationManager.AppSettings["CMCPCountry"].ToString();
            var CMCPReqUID = ConfigurationManager.AppSettings["CMCPReqUID"].ToString();
            var CMCPReqClientId = ConfigurationManager.AppSettings["CMCPReqClientId"].ToString();
            var CMCPServiceURL = ConfigurationManager.AppSettings["CMCPServiceURL"].ToString();

            try
            {
                string sEtoken = GetToken();
                obj.AccStatus = "";
                var respoce = getAccountDetailsDBSRequest(CasaServiceURL, CasaClientId, CasaCorellationId, ac.ToUpper(), sEtoken);

                var jObject = Newtonsoft.Json.Linq.JObject.Parse(respoce);
                if (jObject["error"] != null)
                {
                    obj.AccStatus = "Invalid Account"; 
                }
                else if (jObject["accountClosedFlag"].ToString().Trim().ToUpper() != "Y")
                {
                    if (jObject["accountStatus"] != null)
                    {
                        obj.AccStatus = jObject["accountStatus"].ToString();

                        if (jObject["branchCode"] != null)
                        {
                            obj.SOLID = jObject["branchCode"].ToString().Trim();
                        }
                        else
                        {
                            obj.SOLID = "";
                        }

                        if (jObject["openedDate"] != null)
                        {
                            openDate = Convert.ToInt64(jObject["openedDate"].ToString().Trim());
                        }
                        else
                        {
                            openDate = 0;
                        }


                        if (openDate != 0)
                        {
                            DateTimeOffset dateTimeFromUnix = DateTimeOffset.FromUnixTimeMilliseconds(openDate);
                            DateTimeOffset currentDateTime = DateTimeOffset.Now;
                            int differenceInMonths = CalculateDifferenceInMonths(currentDateTime, dateTimeFromUnix);

                            if (differenceInMonths > 6)
                            {
                                //Console.WriteLine("The difference is greater than six months.");
                               
                                obj.acOpenDateOld = "Y";
                            }
                            else
                            {
                                //Console.WriteLine("The difference is not greater than six months.");
                               
                                obj.acOpenDateOld = "N";
                            }
                        }

                        if (jObject["modeOfOperation"] != null)
                        {
                            obj.MOP = jObject["modeOfOperation"].ToString().Trim();
                        }
                        if (jObject["staffIndicator"] != null)
                        {
                            obj.StaffAcc = (bool)jObject["staffIndicator"] ? "Y" : "N";
                        }
                       
                        if (jObject["freezeStatusCode"] != null)
                        {
                            obj.sFreezeStatusCode = jObject["freezeStatusCode"].ToString();
                        }
                        if (jObject["productCode"] != null)
                        {
                            obj.productCode = jObject["productCode"].ToString().Trim();
                        }
                        if (jObject["productType"] != null)
                        {
                            obj.productType = jObject["productType"].ToString().Trim();
                        }
                        if (jObject["accountCurrencyCode"] != null)
                        {
                            obj.accountCurrencyCode = jObject["accountCurrencyCode"].ToString().Trim();
                        }

                        if (jObject["accountBalances"] != null)
                        {
                           
                            obj.ACBALAmount = jObject["accountBalances"]["effectiveAvailableAmount"] == null ? "0" :
                                              jObject["accountBalances"]["effectiveAvailableAmount"].ToString().Trim();
                        }
                        else
                        {
                            
                            obj.ACBALAmount = "0.0";
                        }


                        if (jObject["sourceCustomerId"] != null)
                        {
                            obj.SourceCustomerId = jObject["sourceCustomerId"].ToString().Trim();
                        }

                       

                        if (jObject["productType"] != null)
                        {
                            obj.OffAcc = jObject["productType"].ToString() == "OAB" ? "Y" : "N";
                        }

                        //             sourceCustomerId|accountCurrencyCode|isopenedDateOld|productCode|productType|accountBalances|modeOfOperation|accountStatus|staffIndicator|freezeStatusCode|openDate|SOLID|offAcc
                      
                        obj.API_Data = obj.SourceCustomerId + "|" + obj.accountCurrencyCode + "|" + obj.acOpenDateOld + "|" + obj.productCode + "|" + obj.productType + "|" + obj.ACBALAmount + "|" + obj.MOP + "|" + obj.AccStatus + "|" + obj.StaffAcc + "|" + obj.sFreezeStatusCode + "|" + openDate + "|" + obj.SOLID + "|" + obj.OffAcc;



                        if (jObject["accountBalances"] != null)
                        {
                            obj.CurrentAccBal = jObject["accountBalances"]["effectiveAvailableAmount"] == null ? "0.0" :
                                  jObject["accountBalances"]["effectiveAvailableAmount"].ToString().Trim();

                            
                        }
                        if(jObject["accountName"] != null)
                        {
                            if (((Newtonsoft.Json.Linq.JContainer)jObject["relatedCustomerInfo"]).Count > 0)
                            {
                                int iIndex = 0;
                                while (iIndex < jObject["relatedCustomerInfo"].Count())
                                {
                                    string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, sEtoken, jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                    string sCustomerName = GetCMCPCustomerName(sCMPCResponse);

                                    if (sCustomerName != "")
                                    {
                                        lAccNames.Add(sCustomerName);
                                    }

                                    logerror("Joint Account Name : ", sCustomerName.ToString());
                                    iIndex++;
                                }
                                logerror("lAccNames Count After while ====>LN876:==>", lAccNames.Count.ToString());
                                obj.PayeeName = lAccNames;
                            }
                            else
                            {
                                lAccNames.Add(jObject["accountName"].ToString().Trim());
                                obj.PayeeName = lAccNames;

                            }

                            if (lAccNames.Count == 0)
                            {
                                lAccNames.Add(jObject["accountName"].ToString().Trim());


                                if (lAccNames.Count > 0)
                                {
                                    obj.PayeeName = lAccNames;
                                }


                            }
                        }

                       
                    }
                }
            }
            catch (Exception Ex)
            {
                string message = "";
                string innerExcp = "";
                string trace = "";
                if (Ex.Message != null)
                    message = Ex.Message.ToString();
                if (Ex.InnerException != null)
                {
                    innerExcp = Ex.InnerException.Message;
                    trace = Ex.InnerException.StackTrace;
                }

                logerror("In GetCasaApiData catch===>>", message.ToString() + "InnerExp==>" + innerExcp);

            }


           

            return obj;

        }

        [HttpPost]
        public JsonResult GetCurrentAPIValue(string ac = null)
        {
            suspenseQcbsdata ob = new suspenseQcbsdata();

            try
            {
                ob = GetCurrentaccountBalancesFromCasaApi(ac);
            }
            catch (Exception Ex)
            {


                string message = "";
                string innerExcp = "";
                string trace = "";
                if (Ex.Message != null)
                    message = Ex.Message.ToString();
                if (Ex.InnerException != null)
                {
                    innerExcp = Ex.InnerException.Message;
                    trace = Ex.InnerException.StackTrace;
                }

                logerror("In GetCurrentAPIValue catch===>>", message.ToString() + "InnerExp==>" + innerExcp);
            }

            return Json(ob,JsonRequestBehavior.AllowGet);

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
                // logerror("In GetToken method inside the Count : ", "");
                sEtoken = dt.Rows[0]["SettingValue"].ToString().Trim();
                // sEtoken = Session["sToken"].ToString().Trim();
                // logerror("In GetToken method inside the Count sEtoken value : ", sEtoken);
            }
            else
            {
                // logerror("In GetToken method inside the else : ", "");
                // logerror("In GetToken method calling sendCMCPTokenRequest method start : ", "");
                sTokenResponse = sendCMCPTokenRequest(TokenServiceURL, TokenClientId, TokenSecreteKey);
                //  logerror("In GetToken method calling sendCMCPTokenRequest method end : ", "");
                //  logerror("In GetToken method calling sTokenResponse value : ", sTokenResponse);

                // logerror("In GetToken method calling getCMCPToken method start : ", "");
                sEtoken = getCMCPToken(sTokenResponse);
                // logerror("In GetToken method calling getCMCPToken method end : ", "");
                // logerror("In GetToken method calling getCMCPToken method end sEtoken value : ", sEtoken);

                //Save new Token
                if (con.State == ConnectionState.Closed)
                    con.Open();

                logerror("In Suspence Q GetToken method calling UpdateToken SP start : ", "");
                SqlCommand cmd = new SqlCommand("UpdateToken", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@SettingValue", sEtoken);
                int iExec = cmd.ExecuteNonQuery();
                logerror("In Suspence Q GetToken method calling UpdateToken SP method end : ", "");
            }

            return sEtoken;
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
            }
            return sResposne;
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
                logerror("in GetCMCPCustomerName", "1108");
                logerror("in GetCMCPCustomerName===>sResposne==>", sResposne);

                var jObject = Newtonsoft.Json.Linq.JObject.Parse(sResposne);
                logerror("in GetCMCPCustomerName==jObject===>", jObject.ToString());
                if (jObject["data"]["profileInfo"]["registeredName"] != null)
                {
                    logerror("in GetCMCPCustomerName===>in If block==LN1115==>", "");
                    logerror("in GetCMCPCustomerName===>in If block==LN1116==>", jObject["data"]["profileInfo"]["registeredName"].ToString());
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
                string message = "";
                string innerExcp = "";
                string trace = "";
                if (Ex.Message != null)
                    message = Ex.Message.ToString();
                if (Ex.InnerException != null)
                {
                    innerExcp = Ex.InnerException.Message;
                    trace = Ex.InnerException.StackTrace;
                }
                logerror("Exception in GetCMCPCustomerName  message==>"+ message.ToString(),"InnerExp===>"+ innerExcp);
                return sCustomerName;
            }
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

        private string getAccountDetailsDBSRequest(string sServiceUrl, string sClientId, string sCoreRelationId, string sAccountNo, string sEToken)
        {
           var sResposne = "";

            try
            {

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;

               var sInputString = "";

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
                if (Ex.Message == "The remote server returned an error: (422) Unprocessable Entity.")
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

                string message = "";
                string innerExcp = "";
                string trace = "";
                if (Ex.Message != null)
                    message = Ex.Message.ToString();
                if (Ex.InnerException != null)
                {
                    innerExcp = Ex.InnerException.Message;
                    trace = Ex.InnerException.StackTrace;
                }

                logerror("In Suspence Q getAccountDetailsDBSRequest catch===>>", message.ToString() + "InnerExp==>" + innerExcp);
            }


            return sResposne;
        }

        private void logerror(string errormsg, string errordesc)
        {
           // ErrorDisplay er = new ErrorDisplay();
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

        static int CalculateDifferenceInMonths(DateTimeOffset laterDate, DateTimeOffset earlierDate)
        {
            int monthsApart = 12 * (laterDate.Year - earlierDate.Year) + laterDate.Month - earlierDate.Month;

            if (laterDate.Day < earlierDate.Day)
            {
                monthsApart--;
            }

            return monthsApart;
        }
    }
}
