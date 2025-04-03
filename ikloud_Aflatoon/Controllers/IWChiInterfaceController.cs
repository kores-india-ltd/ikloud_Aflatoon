using ikloud_Aflatoon.Infrastructure;
using ikloud_Aflatoon.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ikloud_Aflatoon.Controllers
{
    public class IWChiInterfaceController : Controller
    {
        //
        // GET: /IWChiInterface/
        AflatoonEntities af = new AflatoonEntities();
        IWProcDataContext iwpro = new IWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);

        public class ResponseResult
        {
            public string Result1 { get; set; }
            public string Result2 { get; set; }
            public string Result3 { get; set; }

            public string Result4 { get; set; }
            public string Result5 { get; set; }
        }
        public class ResponseResultForDEM
        {
            public string IS_Create_RRF_Status { get; set; }
            public string IS_Upload_RRF_Status { get; set; }
            public string IS_Load_Res_Ocr_RRF_Status { get; set; }

            public string IS_Load_CHM_Status { get; set; }

            public string IS_Load_PXF_Status { get; set; }
            public string IS_Extract_Data_PXF_Status { get; set; }
            public string IS_Generate_RMI_Status { get; set; }

            public string IS_Generate_PXF_Status { get; set; }
            public string IS_Read_RRF_Status { get; set; }

        }

        public JsonResult CheckDEM_Session()
        {
            try
            {
                ResponseResultForDEM res = new ResponseResultForDEM();
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                if (Session["IS_Create_RRF_Clicked"] != null)
                {
                    if (Session["IS_Create_RRF_Clicked"].ToString() == "Y")
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("CheckStatusForDemInwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RecordID", Convert.ToInt64(Session["IS_Create_RRF_RecordID"].ToString()));
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            res.IS_Create_RRF_Status = reader["Status"].ToString();
                        }

                        con.Close();
                        Session["IS_Create_RRF_Status"] = res.IS_Create_RRF_Status;

                        if (res.IS_Create_RRF_Status == "2" || res.IS_Create_RRF_Status == "4")
                        {
                            Session["IS_Create_RRF_Clicked"] = "N";
                        }

                    }

                }
                if (Session["IS_Upload_RRF_Clicked"] != null)
                {
                    if (Session["IS_Upload_RRF_Clicked"].ToString() == "Y")
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("CheckStatusForDemInwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RecordID", Convert.ToInt64(Session["IS_Upload_RRF_RecordID"].ToString()));
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            res.IS_Upload_RRF_Status = reader["Status"].ToString();
                        }

                        con.Close();
                        Session["IS_Upload_RRF_Status"] = res.IS_Upload_RRF_Status;

                        if (res.IS_Upload_RRF_Status == "2" || res.IS_Upload_RRF_Status == "4")
                        {
                            Session["IS_Upload_RRF_Clicked"] = "N";
                        }

                    }

                }
                if (Session["IS_Load_Res_Ocr_RRF_Clicked"] != null)
                {
                    if (Session["IS_Load_Res_Ocr_RRF_Clicked"].ToString() == "Y")
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("CheckStatusForDemInwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RecordID", Convert.ToInt64(Session["IS_Load_Res_Ocr_RRF_RecordID"].ToString()));
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            res.IS_Load_Res_Ocr_RRF_Status = reader["Status"].ToString();
                        }

                        con.Close();
                        Session["IS_Load_Res_Ocr_RRF_Status"] = res.IS_Load_Res_Ocr_RRF_Status;

                        if (res.IS_Load_Res_Ocr_RRF_Status == "2" || res.IS_Load_Res_Ocr_RRF_Status == "4")
                        {
                            Session["IS_Load_Res_Ocr_RRF_Clicked"] = "N";
                        }

                    }

                }
                if (Session["IS_Load_CHM_Clicked"] != null)
                {
                    if (Session["IS_Load_CHM_Clicked"].ToString() == "Y")
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("CheckStatusForDemInwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RecordID", Convert.ToInt64(Session["IS_Load_CHM_RecordID"].ToString()));
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            res.IS_Load_CHM_Status = reader["Status"].ToString();
                        }

                        con.Close();
                        Session["IS_Load_CHM_Status"] = res.IS_Load_CHM_Status;

                        if (res.IS_Load_CHM_Status == "2" || res.IS_Load_CHM_Status == "4")
                        {
                            Session["IS_Load_CHM_Clicked"] = "N";
                        }

                    }

                }
                if (Session["IS_Load_PXF_Clicked"] != null)
                {
                    if (Session["IS_Load_PXF_Clicked"].ToString() == "Y")
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("CheckStatusForDemInwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RecordID", Convert.ToInt64(Session["IS_Load_PXF_RecordID"].ToString()));
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            res.IS_Load_PXF_Status = reader["Status"].ToString();
                        }

                        con.Close();
                        Session["IS_Load_PXF_Status"] = res.IS_Load_PXF_Status;

                        if (res.IS_Load_PXF_Status == "2" || res.IS_Load_PXF_Status == "4")
                        {
                            Session["IS_Load_PXF_Clicked"] = "N";
                        }

                    }

                }
                if (Session["IS_Extract_Data_PXF_Clicked"] != null)
                {
                    if (Session["IS_Extract_Data_PXF_Clicked"].ToString() == "Y")
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("CheckStatusForDemInwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RecordID", Convert.ToInt64(Session["IS_Extract_Data_PXF_RecordID"].ToString()));
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            res.IS_Extract_Data_PXF_Status = reader["Status"].ToString();
                        }

                        con.Close();
                        Session["IS_Extract_Data_PXF_Status"] = res.IS_Extract_Data_PXF_Status;

                        if (res.IS_Extract_Data_PXF_Status == "2" || res.IS_Extract_Data_PXF_Status == "4")
                        {
                            Session["IS_Extract_Data_PXF_Clicked"] = "N";
                        }

                    }

                }
                if (Session["IS_Generate_RMI_Clicked"] != null)
                {
                    if (Session["IS_Generate_RMI_Clicked"].ToString() == "Y")
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("CheckStatusForDemInwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RecordID", Convert.ToInt64(Session["IS_Generate_RMI_RecordID"].ToString()));
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            res.IS_Generate_RMI_Status = reader["Status"].ToString();
                        }

                        con.Close();
                        Session["IS_Generate_RMI_Status"] = res.IS_Generate_RMI_Status;

                        if (res.IS_Generate_RMI_Status == "2" || res.IS_Generate_RMI_Status == "4")
                        {
                            Session["IS_Generate_RMI_Clicked"] = "N";
                        }

                    }

                }
                if (Session["IS_Generate_PXF_Clicked"] != null)
                {
                    if (Session["IS_Generate_PXF_Clicked"].ToString() == "Y")
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("CheckStatusForDemInwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RecordID", Convert.ToInt64(Session["IS_Generate_PXF_RecordID"].ToString()));
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            res.IS_Generate_PXF_Status = reader["Status"].ToString();
                        }

                        con.Close();
                        Session["IS_Generate_PXF_Status"] = res.IS_Generate_PXF_Status;

                        if (res.IS_Generate_PXF_Status == "2" || res.IS_Generate_PXF_Status == "4")
                        {
                            Session["IS_Generate_PXF_Clicked"] = "N";
                        }

                    }

                }
                if (Session["IS_Read_RRF_Clicked"] != null)
                {
                    if (Session["IS_Read_RRF_Clicked"].ToString() == "Y")
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("CheckStatusForDemInwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RecordID", Convert.ToInt64(Session["IS_Read_RRF_RecordID"].ToString()));
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            res.IS_Read_RRF_Status = reader["Status"].ToString();
                        }

                        con.Close();
                        Session["IS_Read_RRF_Status"] = res.IS_Read_RRF_Status;

                        if (res.IS_Read_RRF_Status == "2" || res.IS_Read_RRF_Status == "4")
                        {
                            Session["IS_Read_RRF_Clicked"] = "N";
                        }

                    }

                }

                return Json(res, JsonRequestBehavior.AllowGet);
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
                //return RedirectToAction("Error", "Error", new { msg = message, popmsg = "CreateRrf HttpGet - " + innerExcp });
            }
        }

        public JsonResult TerminateRequest()
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);
                Int64 RecordID = Convert.ToInt64(Session["RecordID"]);

                con.Open();
                SqlCommand cmd = new SqlCommand("TerminateInwardRequest", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@RequestId", RecordID);
                cmd.Parameters.AddWithValue("@RequestedBy", uid);
                int reader = cmd.ExecuteNonQuery();
                ResponseResult res = new ResponseResult();

                if (reader > 0)
                {
                    res.Result1 = "Request Terminated Successfully!!!";

                }
                Session["IS_Create_RRF_Clicked"] = "N";
                Session["IS_Upload_RRF_Clicked"] = "N";
                Session["IS_Load_Res_Ocr_RRF_Clicked"] = "N";
                Session["IS_Load_PXF_Clicked"] = "N";
                Session["IS_Extract_Data_PXF_Clicked"] = "N";
                Session["IS_Generate_RMI_Clicked"] = "N";
                Session["IS_Generate_PXF_Clicked"] = "N";
                Session["IS_Read_RRF_Clicked"] = "N";

                con.Close();

                return Json(res, JsonRequestBehavior.AllowGet);
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
                //return RedirectToAction("Error", "Error", new { msg = message, popmsg = "CreateRrf HttpGet - " + innerExcp });
            }

        }

        public ActionResult Index(string msg1 = null, string msg2 = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }

            if ((bool)Session["Mesgbrd"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            if (msg1 != null)
            {
                ViewBag.Message1 = msg1;
            }
            if (msg2 != null)
            {
                ViewBag.Message2 = msg2;
            }
            @Session["glob"] = null;

            ViewBag.ExpiryTime = Session["SessionExpiryTime_dropDown"];

            //reject reason
            var rjrs = (from r in af.ItemReturnReasons
                        select new RejectReason
                        {
                            Description = r.RETURN_REASON_CODE + " - " + r.DESCRIPTION,

                            ReasonCodeS = r.RETURN_REASON_CODE + " - " + r.DESCRIPTION
                        }).ToList();

            ViewBag.returnreason = ViewBag.ReturnReasons = new SelectList(rjrs, "ReasonCodeS", "Description");
            return View();
        }

        //------------ RRF
        public JsonResult CreateRrf(int Id = 0)
        {
            try
            {
                int uid = (int)Session["uid"];
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);


                con.Open();
                SqlCommand cmd = new SqlCommand("MakeInwardRequest", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CustomerId", custid);
                cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                cmd.Parameters.AddWithValue("@RequestType", Id);
                cmd.Parameters.AddWithValue("@RequestedBy", uid);
                //cmd.ExecuteNonQuery();
                SqlDataReader reader = cmd.ExecuteReader();
                ResponseResult res = new ResponseResult();

                while (reader.Read())
                {
                    res.Result1 = reader["Column1"].ToString();
                    res.Result2 = reader["Column2"].ToString();
                    res.Result3 = reader["Column3"].ToString();
                }

                con.Close();
                Session["RecordID"] = res.Result3;
                Session["IS_Create_RRF_RecordID"] = res.Result3;
                Session["IS_Create_RRF_Clicked"] = "Y";
                return Json(res, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("Index", res.Result1, res.Result2);
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
                //return RedirectToAction("Error", "Error", new { msg = message, popmsg = "CreateRrf HttpGet - " + innerExcp });
            }
        }

        public JsonResult UploadRrf(int Id = 0)
        {
            try
            {
                int uid = (int)Session["uid"];
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);


                con.Open();
                SqlCommand cmd = new SqlCommand("MakeInwardRequest", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CustomerId", custid);
                cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                cmd.Parameters.AddWithValue("@RequestType", Id);
                cmd.Parameters.AddWithValue("@RequestedBy", uid);
                SqlDataReader reader = cmd.ExecuteReader();
                ResponseResult res = new ResponseResult();

                while (reader.Read())
                {
                    res.Result1 = reader["Column1"].ToString();
                    res.Result2 = reader["Column2"].ToString();
                    res.Result3 = reader["Column3"].ToString();
                }

                con.Close();
                Session["RecordID"] = res.Result3;
                Session["IS_Upload_RRF_RecordID"] = res.Result3;
                Session["IS_Upload_RRF_Clicked"] = "Y";
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;
                return Json(message, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("Error", "Error", new { msg = message, popmsg = "UploadRrf HttpGet - " + innerExcp });
            }
            
        }

        public JsonResult LoadResOcrRrf(int Id = 0)
        {
            try
            {
                int uid = (int)Session["uid"];
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);


                con.Open();
                SqlCommand cmd = new SqlCommand("MakeInwardRequest", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CustomerId", custid);
                cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                cmd.Parameters.AddWithValue("@RequestType", Id);
                cmd.Parameters.AddWithValue("@RequestedBy", uid);
                SqlDataReader reader = cmd.ExecuteReader();
                ResponseResult res = new ResponseResult();

                while (reader.Read())
                {
                    res.Result1 = reader["Column1"].ToString();
                    res.Result2 = reader["Column2"].ToString();
                    res.Result3 = reader["Column3"].ToString();
                }

                con.Close();
                Session["RecordID"] = res.Result3;
                Session["IS_Load_Res_Ocr_RRF_RecordID"] = res.Result3;
                Session["IS_Load_Res_Ocr_RRF_Clicked"] = "Y";
                return Json(res, JsonRequestBehavior.AllowGet);
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
                //return RedirectToAction("Error", "Error", new { msg = message, popmsg = "UploadRrf HttpGet - " + innerExcp });
            }
        }

        public JsonResult LoadCHM(int Id = 0)
        {
            try
            {
                int uid = (int)Session["uid"];
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);


                con.Open();
                SqlCommand cmd = new SqlCommand("MakeInwardRequest", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CustomerId", custid);
                cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                cmd.Parameters.AddWithValue("@RequestType", Id);
                cmd.Parameters.AddWithValue("@RequestedBy", uid);
                SqlDataReader reader = cmd.ExecuteReader();
                ResponseResult res = new ResponseResult();

                while (reader.Read())
                {
                    res.Result1 = reader["Column1"].ToString();
                    res.Result2 = reader["Column2"].ToString();
                    res.Result3 = reader["Column3"].ToString();
                }

                con.Close();
                Session["RecordID"] = res.Result3;
                Session["IS_Load_CHM_RecordID"] = res.Result3;
                Session["IS_Load_CHM_Clicked"] = "Y";
                return Json(res, JsonRequestBehavior.AllowGet);
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
                //return RedirectToAction("Error", "Error", new { msg = message, popmsg = "UploadRrf HttpGet - " + innerExcp });
            }
        }

        //------------ PXF
        public JsonResult LoadPxf(int Id = 0, string UserConfirmed = null)
        {
            try
            {
                int uid = (int)Session["uid"];
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);
                ResponseResult res = new ResponseResult();

                if (UserConfirmed == "Y")
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("MakeInwardRequest", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CustomerId", custid);
                    cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                    cmd.Parameters.AddWithValue("@RequestType", Id);
                    cmd.Parameters.AddWithValue("@RequestedBy", uid);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        res.Result1 = reader["Column1"].ToString();
                        res.Result2 = reader["Column2"].ToString();
                        res.Result3 = reader["Column3"].ToString();
                    }

                    con.Close();
                    Session["RecordID"] = res.Result3;
                    Session["IS_Load_PXF_RecordID"] = res.Result3;
                    Session["IS_Load_PXF_Clicked"] = "Y";
                }
                else
                {
                    SodValidation result = Is_Iw_Sod_Validation();

                    if (result.StatusResult == "S")
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("MakeInwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RequestType", Id);
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            res.Result1 = reader["Column1"].ToString();
                            res.Result2 = reader["Column2"].ToString();
                            res.Result3 = reader["Column3"].ToString();
                        }

                        con.Close();
                        Session["RecordID"] = res.Result3;
                        Session["IS_Load_PXF_RecordID"] = res.Result3;
                        Session["IS_Load_PXF_Clicked"] = "Y";
                    }
                    else
                    {
                        res.Result4 = "F";
                        res.Result5 = result.Message;
                    }
                }

                return Json(res, JsonRequestBehavior.AllowGet);
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
                //return RedirectToAction("Error", "Error", new { msg = message, popmsg = "UploadRrf HttpGet - " + innerExcp });
            }
        }

        public JsonResult ExtractDataPxf(int Id = 0, string UserConfirmed = null)
        {
            try
            {
                int uid = (int)Session["uid"];
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);
                ResponseResult res = new ResponseResult();

                if (UserConfirmed == "Y")
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("MakeInwardRequest", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CustomerId", custid);
                    cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                    cmd.Parameters.AddWithValue("@RequestType", Id);
                    cmd.Parameters.AddWithValue("@RequestedBy", uid);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        res.Result1 = reader["Column1"].ToString();
                        res.Result2 = reader["Column2"].ToString();
                        res.Result3 = reader["Column3"].ToString();
                    }

                    con.Close();
                    Session["RecordID"] = res.Result3;
                    Session["IS_Extract_Data_PXF_RecordID"] = res.Result3;
                    Session["IS_Extract_Data_PXF_Clicked"] = "Y";
                }
                else
                {
                    SodValidation result = Is_Iw_Sod_Validation();

                    if (result.StatusResult == "S")
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("MakeInwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RequestType", Id);
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            res.Result1 = reader["Column1"].ToString();
                            res.Result2 = reader["Column2"].ToString();
                            res.Result3 = reader["Column3"].ToString();
                        }

                        con.Close();
                        Session["RecordID"] = res.Result3;
                        Session["IS_Extract_Data_PXF_RecordID"] = res.Result3;
                        Session["IS_Extract_Data_PXF_Clicked"] = "Y";
                    }
                    else
                    {
                        res.Result4 = "F";
                        res.Result5 = result.Message;
                    }
                }
                   
                return Json(res, JsonRequestBehavior.AllowGet);
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
                //return RedirectToAction("Error", "Error", new { msg = message, popmsg = "UploadRrf HttpGet - " + innerExcp });
            }
        }

        public class SodValidation
        {
            public string StatusResult { get; set; }
            public string Message { get; set; }
        }

        public JsonResult GenerateRmi(int Id = 0, string UserConfirmed = null)
        {
            try
            {
                int uid = (int)Session["uid"];
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);
                ResponseResult res = new ResponseResult();

                if(UserConfirmed == "Y")
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("MakeInwardRequest", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@CustomerId", custid);
                    cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                    cmd.Parameters.AddWithValue("@RequestType", Id);
                    cmd.Parameters.AddWithValue("@RequestedBy", uid);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        res.Result1 = reader["Column1"].ToString();
                        res.Result2 = reader["Column2"].ToString();
                        res.Result3 = reader["Column3"].ToString();
                    }

                    con.Close();
                    Session["RecordID"] = res.Result3;
                    Session["IS_Generate_RMI_RecordID"] = res.Result3;
                    Session["IS_Generate_RMI_Clicked"] = "Y";
                }
                else
                {
                    SodValidation result = Is_Iw_Sod_Validation();

                    if (result.StatusResult == "S")
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("MakeInwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RequestType", Id);
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            res.Result1 = reader["Column1"].ToString();
                            res.Result2 = reader["Column2"].ToString();
                            res.Result3 = reader["Column3"].ToString();
                        }

                        con.Close();
                        Session["RecordID"] = res.Result3;
                        Session["IS_Generate_RMI_RecordID"] = res.Result3;
                        Session["IS_Generate_RMI_Clicked"] = "Y";
                    }
                    else
                    {
                        res.Result4 = "F";
                        res.Result5 = result.Message;
                    }
                }
                
                return Json(res, JsonRequestBehavior.AllowGet);
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
                //return RedirectToAction("Error", "Error", new { msg = message, popmsg = "GenerateRmi HttpGet - " + innerExcp });
            }
        }

        public SodValidation Is_Iw_Sod_Validation()
        {
            SodValidation sodValidation = new SodValidation();
            try
            {
                int uid = (int)Session["uid"];
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                
                
                DateTime currentDate = DateTime.Now;
                DateTime maxSodDate = DateTime.Now;

                //========== Step 1 validation start ===============================
                con.Open();
                SqlDataAdapter adp = new SqlDataAdapter("Get_IW_Sod_Date", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = custid;

                DataSet ds = new DataSet();
                adp.Fill(ds);
                con.Close();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    maxSodDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["Max_SOD"]);
                    currentDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["System_CurrentDate"]);
                }

                string iwSodSetting = ConfigurationManager.AppSettings["IwSodValidationSetting"].ToString();

                if (iwSodSetting == "G")
                {
                    //bool isSameDate = processingDate1.Date > currentDate.Date;
                    bool isSameDate = maxSodDate.Date > currentDate.Date;
                    if (isSameDate)
                    {
                        bool isValid = processingDate1.Date == maxSodDate.Date;
                        if (isValid)
                        {
                            sodValidation.StatusResult = "S";
                            sodValidation.Message = "Processing Date is matching with Max SOD Date.";
                        }
                        else
                        {
                            sodValidation.StatusResult = "F";
                            sodValidation.Message = "Processing Date is not matching with Max SOD Date.";
                        }
                    }
                    else
                    {
                        sodValidation.StatusResult = "F";
                        //sodValidation.Message = "Processing Date is less than the System Date.";
                        sodValidation.Message = "SOD not found.";
                    }
                }
                else if (iwSodSetting == "L")
                {
                    bool isSameDate = maxSodDate.Date == currentDate.Date;
                    if (isSameDate)
                    {
                        bool isValid = processingDate1.Date == maxSodDate.Date;
                        if (isValid)
                        {
                            sodValidation.StatusResult = "S";
                            sodValidation.Message = "Processing Date is matching with Max SOD Date.";
                        }
                        else
                        {
                            sodValidation.StatusResult = "F";
                            sodValidation.Message = "Processing Date is not matching with Max SOD Date.";
                        }
                    }
                    else
                    {
                        sodValidation.StatusResult = "F";
                        sodValidation.Message = "SOD not found.";
                    }
                }
                else
                {
                    //bool isSameDate = processingDate1.Date == currentDate.Date;
                    //if (isSameDate)
                    //{
                    //    sodValidation.StatusResult = "S";
                    //    sodValidation.Message = "Processing Date is matching with System Date.";
                    //}
                    //else
                    //{
                    //    sodValidation.StatusResult = "F";
                    //    sodValidation.Message = "Processing Date is not match with the System Date.";
                    //}
                    sodValidation.StatusResult = "S";
                }
                return sodValidation;
            }
            catch(Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;
                sodValidation.StatusResult = "F";
                sodValidation.Message = e.InnerException.Message;
                return sodValidation;
            }
        }

        //------------ LVB
        public JsonResult GeneratePxf(int Id = 0)
        {
            try
            {
                int uid = (int)Session["uid"];
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);


                con.Open();
                SqlCommand cmd = new SqlCommand("MakeInwardRequest", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CustomerId", custid);
                cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                cmd.Parameters.AddWithValue("@RequestType", Id);
                cmd.Parameters.AddWithValue("@RequestedBy", uid);
                SqlDataReader reader = cmd.ExecuteReader();
                ResponseResult res = new ResponseResult();

                while (reader.Read())
                {
                    res.Result1 = reader["Column1"].ToString();
                    res.Result2 = reader["Column2"].ToString();
                    res.Result3 = reader["Column3"].ToString();
                }

                con.Close();
                Session["RecordID"] = res.Result3;
                Session["IS_Generate_PXF_RecordID"] = res.Result3;
                Session["IS_Generate_PXF_Clicked"] = "Y";
                return Json(res, JsonRequestBehavior.AllowGet);
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
                //return RedirectToAction("Error", "Error", new { msg = message, popmsg = "GeneratePxf HttpGet - " + innerExcp });
            }
        }

        public JsonResult ReadRrf(int Id = 0)
        {
            try
            {
                int uid = (int)Session["uid"];
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);


                con.Open();
                SqlCommand cmd = new SqlCommand("MakeInwardRequest", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CustomerId", custid);
                cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                cmd.Parameters.AddWithValue("@RequestType", Id);
                cmd.Parameters.AddWithValue("@RequestedBy", uid);
                SqlDataReader reader = cmd.ExecuteReader();
                ResponseResult res = new ResponseResult();

                while (reader.Read())
                {
                    res.Result1 = reader["Column1"].ToString();
                    res.Result2 = reader["Column2"].ToString();
                    res.Result3 = reader["Column3"].ToString();
                }

                con.Close();
                Session["RecordID"] = res.Result3;
                Session["IS_Read_RRF_RecordID"] = res.Result3;
                Session["IS_Read_RRF_Clicked"] = "Y";
                return Json(res, JsonRequestBehavior.AllowGet);
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
                //return RedirectToAction("Error", "Error", new { msg = message, popmsg = "ReadRrf HttpGet - " + innerExcp });
            }
        }

        //=================== Show Files ==============================

        public PartialViewResult Show_Files_RRF()
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                SqlDataAdapter adp = new SqlDataAdapter("GetRrfFileStatus", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = custid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = procDate;

                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<Exe_Status>();
                Exe_Status def;


                if (ds.Tables[0].Rows.Count > 0)
                {
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;

                    for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        def = new Exe_Status
                        {
                            FileId = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[0].ToString()),
                            FileName = ds.Tables[0].Rows[i].ItemArray[1].ToString(),
                            Status = ds.Tables[0].Rows[i].ItemArray[2].ToString(),
                            ClearingType = ds.Tables[0].Rows[i].ItemArray[3].ToString(),
                            ItemCount = Convert.ToInt16(ds.Tables[0].Rows[i].ItemArray[4].ToString()),
                            AcceptCount = Convert.ToInt16(ds.Tables[0].Rows[i].ItemArray[5].ToString()),
                            RejectCount = Convert.ToInt16(ds.Tables[0].Rows[i].ItemArray[6].ToString()),
                            CreationTimeStamp = ds.Tables[0].Rows[i].ItemArray[7].ToString(),
                            UploadTimeStamp = ds.Tables[0].Rows[i].ItemArray[8].ToString(),
                            RESTimeStamp = ds.Tables[0].Rows[i].ItemArray[9].ToString(),
                            OACKTimeStamp = ds.Tables[0].Rows[i].ItemArray[10].ToString()
                        };
                        objectlst.Add(def);
                    }
                    
                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                }
                ViewBag.Heading = "RRF File Status";

                return PartialView("_Exe_Status", objectlst);
            }
            catch (Exception e)
            {
                ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                er.ErrorMessage = e.InnerException.Message;

                return PartialView("Error", er);
                // return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }

        public PartialViewResult Show_Files_PXF()
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                SqlDataAdapter adp = new SqlDataAdapter("GetPxfFileStatus", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = custid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = procDate;

                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<Exe_Status>();
                Exe_Status def;


                if (ds.Tables[0].Rows.Count > 0)
                {
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;

                    for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        def = new Exe_Status
                        {
                            //FileId = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[0].ToString()),
                            FileName = ds.Tables[0].Rows[i].ItemArray[0].ToString(),
                            Status = ds.Tables[0].Rows[i].ItemArray[1].ToString(),
                            //ClearingType = ds.Tables[0].Rows[i].ItemArray[3].ToString(),
                            ItemCount = Convert.ToInt16(ds.Tables[0].Rows[i].ItemArray[2].ToString())
                            //AcceptCount = Convert.ToInt16(ds.Tables[0].Rows[i].ItemArray[5].ToString()),
                            //RejectCount = Convert.ToInt16(ds.Tables[0].Rows[i].ItemArray[6].ToString()),
                            //CreationTimeStamp = ds.Tables[0].Rows[i].ItemArray[7].ToString(),
                            //UploadTimeStamp = ds.Tables[0].Rows[i].ItemArray[8].ToString(),
                            //RESTimeStamp = ds.Tables[0].Rows[i].ItemArray[9].ToString(),
                            //OACKTimeStamp = ds.Tables[0].Rows[i].ItemArray[10].ToString()
                        };
                        objectlst.Add(def);
                    }
                    
                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                }
                ViewBag.Heading = "PXF File Status";

                return PartialView("_Exe_Status", objectlst);
            }
            catch (Exception e)
            {
                ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                er.ErrorMessage = e.InnerException.Message;

                return PartialView("Error", er);
                // return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }

        public PartialViewResult Show_Files_LVB_PXF()
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                SqlDataAdapter adp = new SqlDataAdapter("GetLVBPxfFileStatus", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = custid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = procDate;

                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<Exe_Status>();
                Exe_Status def;


                if (ds.Tables[0].Rows.Count > 0)
                {
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;

                    for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        def = new Exe_Status
                        {
                            FileId = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[0].ToString()),
                            FileName = ds.Tables[0].Rows[i].ItemArray[1].ToString(),
                            Status = ds.Tables[0].Rows[i].ItemArray[2].ToString(),
                            ClearingType = ds.Tables[0].Rows[i].ItemArray[4].ToString(),
                            ItemCount = Convert.ToInt16(ds.Tables[0].Rows[i].ItemArray[3].ToString()),
                            //AcceptCount = Convert.ToInt16(ds.Tables[0].Rows[i].ItemArray[5].ToString()),
                            //RejectCount = Convert.ToInt16(ds.Tables[0].Rows[i].ItemArray[6].ToString()),
                            CreationTimeStamp = ds.Tables[0].Rows[i].ItemArray[5].ToString()
                            //UploadTimeStamp = ds.Tables[0].Rows[i].ItemArray[8].ToString(),
                            //RESTimeStamp = ds.Tables[0].Rows[i].ItemArray[9].ToString(),
                            //OACKTimeStamp = ds.Tables[0].Rows[i].ItemArray[10].ToString()
                        };
                        objectlst.Add(def);
                    }
                    
                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                }
                ViewBag.Heading = "LVB PXF File Status";

                return PartialView("_Exe_Status", objectlst);
            }
            catch (Exception e)
            {
                ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                er.ErrorMessage = e.InnerException.Message;

                return PartialView("Error", er);
                // return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }

        public PartialViewResult Show_Files_LVB_RRF()
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                SqlDataAdapter adp = new SqlDataAdapter("GetLVBRrfFileStatus", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = custid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = procDate;

                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<Exe_Status>();
                Exe_Status def;


                if (ds.Tables[0].Rows.Count > 0)
                {
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;

                    for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        def = new Exe_Status
                        {
                            FileId = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[0].ToString()),
                            FileName = ds.Tables[0].Rows[i].ItemArray[1].ToString(),
                            Status = ds.Tables[0].Rows[i].ItemArray[2].ToString(),
                            ClearingType = ds.Tables[0].Rows[i].ItemArray[3].ToString(),
                            ItemCount = Convert.ToInt16(ds.Tables[0].Rows[i].ItemArray[4].ToString()),
                            AcceptCount = Convert.ToInt16(ds.Tables[0].Rows[i].ItemArray[5].ToString()),
                            RejectCount = Convert.ToInt16(ds.Tables[0].Rows[i].ItemArray[6].ToString()),
                            CreationTimeStamp = ds.Tables[0].Rows[i].ItemArray[7].ToString(),
                            UploadTimeStamp = ds.Tables[0].Rows[i].ItemArray[8].ToString(),
                            RESTimeStamp = ds.Tables[0].Rows[i].ItemArray[9].ToString(),
                            OACKTimeStamp = ds.Tables[0].Rows[i].ItemArray[10].ToString()
                        };
                        objectlst.Add(def);
                    }
                    
                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                }
                ViewBag.Heading = "LVB RRF File Status";

                return PartialView("_Exe_Status", objectlst);
            }
            catch (Exception e)
            {
                ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                er.ErrorMessage = e.InnerException.Message;

                return PartialView("Error", er);
                // return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }

        //======================= Show Records Counts ==================================

        public PartialViewResult Show_Records_RRF()
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                SqlDataAdapter adp = new SqlDataAdapter("GetRRFAvaiableCount", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = custid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = procDate;

                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<Exe_Count_RRF>();
                Exe_Count_RRF def;


                if (ds.Tables[0].Rows.Count > 0)
                {
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;

                    for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        def = new Exe_Count_RRF
                        {
                            ClearingType = ds.Tables[0].Rows[i].ItemArray[0].ToString(),
                            TotalCount = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[1].ToString()),
                            Available = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[2].ToString()),
                            Presented = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[3].ToString()),
                        };
                        objectlst.Add(def);
                    }
                    
                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                }
                ViewBag.Heading = "RRF Records Count";

                return PartialView("_Exe_Count_RRF", objectlst);
            }
            catch (Exception e)
            {
                ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                er.ErrorMessage = e.InnerException.Message;

                return PartialView("Error", er);
                // return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }

        public PartialViewResult Show_Records_PXF()
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                SqlDataAdapter adp = new SqlDataAdapter("GetPXFAvaiableCount", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = custid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = procDate;

                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<Exe_Count_PXF>();
                Exe_Count_PXF def;


                if (ds.Tables[0].Rows.Count > 0)
                {
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;

                    for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        def = new Exe_Count_PXF
                        {
                            ClearingType = ds.Tables[0].Rows[i].ItemArray[0].ToString(),
                            DocType = ds.Tables[0].Rows[i].ItemArray[1].ToString(),
                            TotalCount = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[2].ToString()),
                        };
                        objectlst.Add(def);
                    }
                    
                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                }
                ViewBag.Heading = "PXF Records Count";

                return PartialView("_Exe_Count_PXF", objectlst);
            }
            catch (Exception e)
            {
                ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                er.ErrorMessage = e.InnerException.Message;

                return PartialView("Error", er);
                // return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }

        public PartialViewResult Show_Records_LVB_PXF()
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                SqlDataAdapter adp = new SqlDataAdapter("GetLvbPXFAvaiableCount", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = custid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = procDate;

                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<Exe_Count_PXF>();
                Exe_Count_PXF def;


                if (ds.Tables[0].Rows.Count > 0)
                {
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;

                    for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        def = new Exe_Count_PXF
                        {
                            ClearingType = ds.Tables[0].Rows[i].ItemArray[0].ToString(),
                            DocType = ds.Tables[0].Rows[i].ItemArray[1].ToString(),
                            TotalCount = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[2].ToString()),
                        };
                        objectlst.Add(def);
                    }
                    
                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                }
                ViewBag.Heading = "LVB PXF Records Count";

                return PartialView("_Exe_Count_PXF", objectlst);
            }
            catch (Exception e)
            {
                ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                er.ErrorMessage = e.InnerException.Message;

                return PartialView("Error", er);
                // return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }

        public PartialViewResult Show_Records_LVB_RRF()
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                SqlDataAdapter adp = new SqlDataAdapter("GetLvbRRFAvaiableCount", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = custid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = procDate;

                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<Exe_Count_RRF>();
                Exe_Count_RRF def;


                if (ds.Tables[0].Rows.Count > 0)
                {
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;

                    for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        def = new Exe_Count_RRF
                        {
                            ClearingType = ds.Tables[0].Rows[i].ItemArray[0].ToString(),
                            TotalCount = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[1].ToString()),
                            Available = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[2].ToString()),
                            Presented = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[3].ToString()),
                        };
                        objectlst.Add(def);
                    }
                    
                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                }
                ViewBag.Heading = "LVB RRF Records Count";

                return PartialView("_Exe_Count_RRF", objectlst);
            }
            catch (Exception e)
            {
                ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                er.ErrorMessage = e.InnerException.Message;

                return PartialView("Error", er);
                // return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }

        //mark all accept/return 01-10-24

        public JsonResult MarkAllAcceptUpdate(string TimeStamp)
        {
            try
            {
                int uid = (int)Session["uid"];
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                con.Open();
                SqlCommand cmd = new SqlCommand("IW_MarkAllAcceptUpdate", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                cmd.Parameters.AddWithValue("@CustomerId", custid);


                TimeSpan itemExpiryTimeStamp = TimeSpan.Parse(TimeStamp);

                cmd.Parameters.AddWithValue("@ItemExpiryTime", itemExpiryTimeStamp);
                SqlParameter outputParam = new SqlParameter("@RowsAffected", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);


                cmd.ExecuteNonQuery();


                int rowsAffected = (int)outputParam.Value;
                if (rowsAffected > 0)
                {
                    return Json(new { message = "Successfully marked All pending cheques as accept." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { message = "No records were updated." }, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception e)
            {

                string message = e.Message;
                string innerExcp = e.InnerException != null ? e.InnerException.Message : null;
                return Json(new { error = message, innerError = innerExcp }, JsonRequestBehavior.AllowGet);
            }



        }

        public PartialViewResult GetShowCountAccept(string TimeStamp)
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                SqlDataAdapter adp = new SqlDataAdapter("IW_ShowCount_AcceptAll", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = custid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = procDate;
                adp.SelectCommand.Parameters.Add("@ItemExpiryTimeStamp", SqlDbType.NVarChar).Value = TimeStamp; //"00:00:00";

                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<GetShowCountAccept>();
                GetShowCountAccept def;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    int count = ds.Tables[0].Rows.Count;
                    for (var i = 0; i < count; i++)
                    {
                        def = new GetShowCountAccept
                        {
                            TotalCheques = ds.Tables[0].Rows[i].ItemArray[0] != DBNull.Value
                               ? ds.Tables[0].Rows[i].ItemArray[0].ToString()
                               : "0", // or another default value

                            TotalAmount = ds.Tables[0].Rows[i].ItemArray[1] != DBNull.Value
                               ? ds.Tables[0].Rows[i].ItemArray[1].ToString()
                               : "0.0m" // or another default value
                        };

                        objectlst.Add(def);
                    }

                    ViewBag.cnt = true;
                    Session["glob"] = null;
                }

                ViewBag.Heading = "Pending Cheques Count";

                return PartialView("_GetShowCountAccept", objectlst);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                er.ErrorMessage = innerExcp;
                return PartialView("Error", er);
            }
        }

        public PartialViewResult GetShowAcceptFile(string TimeStamp)
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                SqlDataAdapter adp = new SqlDataAdapter("IW_ShowRecord_AcceptAll", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = procDate;
                adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = custid;
                adp.SelectCommand.Parameters.Add("@ItemExpiryTimeStamp", SqlDbType.NVarChar).Value = TimeStamp; //"00:00:00";

                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<GetShowRecordAccept>();
                GetShowRecordAccept def;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int count = ds.Tables[0].Rows.Count;
                    for (var i = 0; i < count; i++)
                    {
                        def = new GetShowRecordAccept
                        {
                            id = Convert.ToInt64(ds.Tables[0].Rows[i][0]), // ID
                            ChqNo = ds.Tables[0].Rows[i][1].ToString(), // ChqNo
                            SortCode = ds.Tables[0].Rows[i][2].ToString(), // SortCode
                            Transcode = ds.Tables[0].Rows[i][3].ToString(), // Transcode
                            SANCode = ds.Tables[0].Rows[i][4].ToString(), // SANCode
                            Amount = ds.Tables[0].Rows[i][5].ToString(), // Amount
                            Payeename = ds.Tables[0].Rows[i][6].ToString() // Payeename
                        };
                        objectlst.Add(def);
                    }
                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                }
                ViewBag.Heading = "Records For Cheques";

                return PartialView("_GetShowRecordAccept", objectlst);




            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                er.ErrorMessage = innerExcp;
                return PartialView("Error", er);
            }
        }


        //mark all return 
        public JsonResult MarkAllRejectUpdate(string ItemExpiryTime, string returnCode, string description)
        {
            try
            {
                int uid = (int)Session["uid"];
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                TimeSpan itemExpiryTimeSpan = TimeSpan.Parse(ItemExpiryTime);

                con.Open();
                SqlCommand cmd = new SqlCommand("IW_MarkAllRejectUpdate", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                cmd.Parameters.AddWithValue("@CustomerId", custid);
                cmd.Parameters.AddWithValue("@ReturnCode", returnCode);
                cmd.Parameters.AddWithValue("@ReturnReasonDescription", description);

                cmd.Parameters.AddWithValue("@ItemExpiryTimeStamp", itemExpiryTimeSpan);
                SqlParameter outputParam = new SqlParameter("@RowsAffected", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);
                cmd.Parameters.AddWithValue("@uid", uid);
                cmd.Parameters.AddWithValue("@ReturnFlag", "IWDEM");

                cmd.ExecuteNonQuery();


                int rowsAffected = (int)outputParam.Value;
                if (rowsAffected > 0)
                {
                    return Json(new { message = "Successfully marked All pending cheques as return." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { message = "No records were updated." }, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception e)
            {

                string message = e.Message;
                string innerExcp = e.InnerException != null ? e.InnerException.Message : null;
                logerror("in MarkAllRejectUpdate catch==> message==>" + message, "InnerExp==>" + innerExcp);
                return Json(new { error = message, innerError = innerExcp }, JsonRequestBehavior.AllowGet);
            }
        }


        private void logerror(string errormsg, string errordesc)
        {
            //ErrorDisplay er = new ErrorDisplay();
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

        //reset api 
        public JsonResult ResetFailAPI(int ApiType = 0)
        {
            var msg = "";

            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                con.Open();
                SqlCommand cmd = new SqlCommand("IwResetAPIFailedCases", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // cmd.Parameters.AddWithValue("@CustomerId", custid);
                cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                cmd.Parameters.AddWithValue("@RecordType", ApiType);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    msg = "Success"; 
                }
                else
                {
                    msg = "no record update";
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
                logerror("in IWResetAPI CATCH==>msg==>" + message, " InnerExp==>" + innerExcp);
                return Json(message, JsonRequestBehavior.AllowGet);
            }


            return Json(msg, JsonRequestBehavior.AllowGet);
        }

    }
}
