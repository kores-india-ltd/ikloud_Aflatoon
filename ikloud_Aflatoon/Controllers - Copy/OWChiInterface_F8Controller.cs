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

namespace ikloud_Aflatoon.Controllers
{
    public class OWChiInterface_F8Controller : Controller
    {
        //
        // GET: /OWChiInterface_F8/
        AflatoonEntities af = new AflatoonEntities();
        IWProcDataContext iwpro = new IWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);

        public class ResponseResult
        {
            public string Result1 { get; set; }
            public string Result2 { get; set; }
            public string Result3 { get; set; }
        }
        public class ResponseResultForDEM
        {
            public string IS_Create_CXF_Status { get; set; }
            public string IS_Upload_CXF_Status { get; set; }
            public string IS_Download_Res_Oack_Status { get; set; }

            public string IS_Read_PPS_File_Status { get; set; }
            public string IS_PPS_Create_CIIF_Status { get; set; }
            public string IS_PPS_Upload_CIIF_Status { get; set; }
            public string IS_PPS_Download_Res_Status { get; set; }

            public string IS_Read_BRF_Status { get; set; }
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

                if (Session["IS_Create_CXF_Clicked"] != null)
                {
                    if (Session["IS_Create_CXF_Clicked"].ToString() == "Y")
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("CheckStatusForDemOutwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RecordID", Convert.ToInt64(Session["IS_Create_CXF_RecordID"].ToString()));
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            res.IS_Create_CXF_Status = reader["Status"].ToString();
                        }

                        con.Close();
                        Session["IS_Create_CXF_Status"] = res.IS_Create_CXF_Status;

                        if (res.IS_Create_CXF_Status == "2" || res.IS_Create_CXF_Status == "4")
                        {
                            Session["IS_Create_CXF_Clicked"] = "N";
                        }

                    }

                }
                if (Session["IS_Upload_CXF_Clicked"] != null)
                {
                    if (Session["IS_Upload_CXF_Clicked"].ToString() == "Y")
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("CheckStatusForDemOutwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RecordID", Convert.ToInt64(Session["IS_Upload_CXF_RecordID"].ToString()));
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            res.IS_Upload_CXF_Status = reader["Status"].ToString();
                        }

                        con.Close();
                        Session["IS_Upload_CXF_Status"] = res.IS_Upload_CXF_Status;

                        if (res.IS_Upload_CXF_Status == "2" || res.IS_Upload_CXF_Status == "4")
                        {
                            Session["IS_Upload_CXF_Clicked"] = "N";
                        }

                    }
                }
                if (Session["IS_Download_Res_Oack_Clicked"] != null)
                {
                    if (Session["IS_Download_Res_Oack_Clicked"].ToString() == "Y")
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("CheckStatusForDemOutwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RecordID", Convert.ToInt64(Session["IS_Download_Res_Oack_RecordID"].ToString()));
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            res.IS_Download_Res_Oack_Status = reader["Status"].ToString();
                        }

                        con.Close();
                        Session["IS_Download_Res_Oack_Status"] = res.IS_Download_Res_Oack_Status;

                        if (res.IS_Download_Res_Oack_Status == "2" || res.IS_Download_Res_Oack_Status == "4")
                        {
                            Session["IS_Download_Res_Oack_Clicked"] = "N";
                        }

                    }
                }
                if (Session["IS_Read_PPS_File_Clicked"] != null)
                {
                    if (Session["IS_Read_PPS_File_Clicked"].ToString() == "Y")
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("CheckStatusForDemOutwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RecordID", Convert.ToInt64(Session["IS_Read_PPS_File_RecordID"].ToString()));
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            res.IS_Read_PPS_File_Status = reader["Status"].ToString();
                        }

                        con.Close();
                        Session["IS_Read_PPS_File_Status"] = res.IS_Read_PPS_File_Status;

                        if (res.IS_Read_PPS_File_Status == "2" || res.IS_Read_PPS_File_Status == "4")
                        {
                            Session["IS_Read_PPS_File_Clicked"] = "N";
                        }

                    }
                }
                if (Session["IS_PPS_Create_CIIF_Clicked"] != null)
                {
                    if (Session["IS_PPS_Create_CIIF_Clicked"].ToString() == "Y")
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("CheckStatusForDemOutwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RecordID", Convert.ToInt64(Session["IS_PPS_Create_CIIF_RecordID"].ToString()));
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            res.IS_PPS_Create_CIIF_Status = reader["Status"].ToString();
                        }

                        con.Close();
                        Session["IS_PPS_Create_CIIF_Status"] = res.IS_PPS_Create_CIIF_Status;

                        if (res.IS_PPS_Create_CIIF_Status == "2" || res.IS_PPS_Create_CIIF_Status == "4")
                        {
                            Session["IS_PPS_Create_CIIF_Clicked"] = "N";
                        }

                    }
                }
                if (Session["IS_PPS_Upload_CIIF_Clicked"] != null)
                {
                    if (Session["IS_PPS_Upload_CIIF_Clicked"].ToString() == "Y")
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("CheckStatusForDemOutwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RecordID", Convert.ToInt64(Session["IS_PPS_Upload_CIIF_RecordID"].ToString()));
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            res.IS_PPS_Upload_CIIF_Status = reader["Status"].ToString();
                        }

                        con.Close();
                        Session["IS_PPS_Upload_CIIF_Status"] = res.IS_PPS_Upload_CIIF_Status;

                        if (res.IS_PPS_Upload_CIIF_Status == "2" || res.IS_PPS_Upload_CIIF_Status == "4")
                        {
                            Session["IS_PPS_Upload_CIIF_Clicked"] = "N";
                        }

                    }
                }
                if (Session["IS_PPS_Download_Res_Clicked"] != null)
                {
                    if (Session["IS_PPS_Download_Res_Clicked"].ToString() == "Y")
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("CheckStatusForDemOutwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RecordID", Convert.ToInt64(Session["IS_PPS_Download_Res_RecordID"].ToString()));
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            res.IS_PPS_Download_Res_Status = reader["Status"].ToString();
                        }

                        con.Close();
                        Session["IS_PPS_Download_Res_Status"] = res.IS_PPS_Download_Res_Status;

                        if (res.IS_PPS_Download_Res_Status == "2" || res.IS_PPS_Download_Res_Status == "4")
                        {
                            Session["IS_PPS_Download_Res_Clicked"] = "N";
                        }

                    }
                }
                if (Session["IS_Read_BRF_Clicked"] != null)
                {
                    if (Session["IS_Read_BRF_Clicked"].ToString() == "Y")
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("CheckStatusForDemOutwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RecordID", Convert.ToInt64(Session["IS_Read_BRF_RecordID"].ToString()));
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            res.IS_Read_BRF_Status = reader["Status"].ToString();
                        }

                        con.Close();
                        Session["IS_Read_BRF_Status"] = res.IS_Read_BRF_Status;

                        if (res.IS_Read_BRF_Status == "2" || res.IS_Read_BRF_Status == "4")
                        {
                            Session["IS_Read_BRF_Clicked"] = "N";
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
                SqlCommand cmd = new SqlCommand("TerminateOutwardRequest", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@RequestId", RecordID);
                cmd.Parameters.AddWithValue("@RequestedBy", uid);
                int reader = cmd.ExecuteNonQuery();
                ResponseResult res = new ResponseResult();

                if (reader > 0)
                {
                    res.Result1 = "Request Terminated Successfully!!!";

                }
                Session["IS_Create_CXF_Clicked"] = "N";
                Session["IS_Upload_CXF_Clicked"] = "N";
                Session["IS_Download_Res_Oack_Clicked"] = "N";
                Session["IS_Read_PPS_File_Clicked"] = "N";
                Session["IS_PPS_Create_CIIF_Clicked"] = "N";
                Session["IS_PPS_Upload_CIIF_Clicked"] = "N";
                Session["IS_PPS_Download_Res_Clicked"] = "N";
                Session["IS_Read_BRF_Clicked"] = "N";

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
            if (msg1 != null)
            {
                ViewBag.Message1 = msg1;
            }
            if (msg2 != null)
            {
                ViewBag.Message2 = msg2;
            }
            @Session["glob"] = null;
            return View();
        }

        //------------ CXF
        public JsonResult CreateCXF(int Id = 0)
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);
                Session["Create_CXF_ID"] = Id;

                con.Open();
                SqlCommand cmd = new SqlCommand("MakeOutwardRequest", con);
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
                Session["IS_Create_CXF_RecordID"] = res.Result3;
                Session["IS_Create_CXF_Clicked"] = "Y";
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

        public JsonResult UploadCXF(int Id = 0)
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);


                con.Open();
                SqlCommand cmd = new SqlCommand("MakeOutwardRequest", con);
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
                Session["IS_Upload_CXF_RecordID"] = res.Result3;
                Session["IS_Upload_CXF_Clicked"] = "Y";
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

        public JsonResult DownloadRES_OACK_CXF(int Id = 0)
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);


                con.Open();
                SqlCommand cmd = new SqlCommand("MakeOutwardRequest", con);
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
                Session["IS_Download_Res_Oack_RecordID"] = res.Result3;
                Session["IS_Download_Res_Oack_Clicked"] = "Y";
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

        //------------ PPS File
        public JsonResult Read_PPS_File(int Id = 0)
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);


                con.Open();
                SqlCommand cmd = new SqlCommand("MakeOutwardRequest", con);
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
                Session["IS_Read_PPS_File_RecordID"] = res.Result3;
                Session["IS_Read_PPS_File_Clicked"] = "Y";
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

        //------------ CIIF
        public JsonResult CreateCIIF(int Id = 0)
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);


                con.Open();
                SqlCommand cmd = new SqlCommand("MakeOutwardRequest", con);
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
                Session["IS_PPS_Create_CIIF_RecordID"] = res.Result3;
                Session["IS_PPS_Create_CIIF_Clicked"] = "Y";
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

        public JsonResult UploadCIIF(int Id = 0)
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);


                con.Open();
                SqlCommand cmd = new SqlCommand("MakeOutwardRequest", con);
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
                Session["IS_PPS_Upload_CIIF_RecordID"] = res.Result3;
                Session["IS_PPS_Upload_CIIF_Clicked"] = "Y";
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

        public JsonResult DownloadRES(int Id = 0)
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);


                con.Open();
                SqlCommand cmd = new SqlCommand("MakeOutwardRequest", con);
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
                Session["IS_PPS_Download_Res_RecordID"] = res.Result3;
                Session["IS_PPS_Download_Res_Clicked"] = "Y";
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

        //----------------- BRF
        public JsonResult ReadBRF(int Id = 0)
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);


                con.Open();
                SqlCommand cmd = new SqlCommand("MakeOutwardRequest", con);
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
                Session["IS_Read_BRF_RecordID"] = res.Result3;
                Session["IS_Read_BRF_Clicked"] = "Y";
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

        //=================== Show Files ==============================

        public PartialViewResult Show_Files_CXF()
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                SqlDataAdapter adp = new SqlDataAdapter("GetCXFFileStatus", con);
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
                            FileId = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0].ToString()),
                            FileName = ds.Tables[0].Rows[0].ItemArray[1].ToString(),
                            Status = ds.Tables[0].Rows[0].ItemArray[2].ToString(),
                            ClearingType = ds.Tables[0].Rows[0].ItemArray[3].ToString(),
                            ItemCount = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[4].ToString()),
                            AcceptCount = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[5].ToString()),
                            RejectCount = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[6].ToString()),
                            CreationTimeStamp = ds.Tables[0].Rows[0].ItemArray[7].ToString(),
                            UploadTimeStamp = ds.Tables[0].Rows[0].ItemArray[8].ToString(),
                            RESTimeStamp = ds.Tables[0].Rows[0].ItemArray[9].ToString(),
                            OACKTimeStamp = ds.Tables[0].Rows[0].ItemArray[10].ToString()
                        };
                        objectlst.Add(def);
                    }

                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                }
                ViewBag.Heading = "CXF File Status";

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

        public PartialViewResult Show_Files_CIIF()
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                SqlDataAdapter adp = new SqlDataAdapter("GetCiiFFileStatus", con);
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
                            FileId = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0].ToString()),
                            FileName = ds.Tables[0].Rows[0].ItemArray[1].ToString(),
                            Status = ds.Tables[0].Rows[0].ItemArray[2].ToString(),
                            ClearingType = ds.Tables[0].Rows[0].ItemArray[3].ToString(),
                            ItemCount = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[4].ToString()),
                            AcceptCount = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[5].ToString()),
                            RejectCount = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[6].ToString()),
                            CreationTimeStamp = ds.Tables[0].Rows[0].ItemArray[7].ToString(),
                            UploadTimeStamp = ds.Tables[0].Rows[0].ItemArray[8].ToString(),
                            RESTimeStamp = ds.Tables[0].Rows[0].ItemArray[9].ToString(),
                            OACKTimeStamp = ds.Tables[0].Rows[0].ItemArray[10].ToString()
                        };
                        objectlst.Add(def);
                    }

                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                }
                ViewBag.Heading = "CIIF File Status";

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

        public PartialViewResult Show_Files_BRF()
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                SqlDataAdapter adp = new SqlDataAdapter("GetBRFFileStatus", con);
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
                            FileId = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0].ToString()),
                            FileName = ds.Tables[0].Rows[0].ItemArray[1].ToString(),
                            Status = ds.Tables[0].Rows[0].ItemArray[2].ToString(),
                            ClearingType = ds.Tables[0].Rows[0].ItemArray[3].ToString(),
                            ItemCount = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[4].ToString()),
                            AcceptCount = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[5].ToString()),
                            RejectCount = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[6].ToString()),
                            CreationTimeStamp = ds.Tables[0].Rows[0].ItemArray[7].ToString(),
                            UploadTimeStamp = ds.Tables[0].Rows[0].ItemArray[8].ToString(),
                            RESTimeStamp = ds.Tables[0].Rows[0].ItemArray[9].ToString(),
                            OACKTimeStamp = ds.Tables[0].Rows[0].ItemArray[10].ToString()
                        };
                        objectlst.Add(def);
                    }

                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                }
                ViewBag.Heading = "BRF File Status";

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

        //=================== Show Record Count ==============================


        public PartialViewResult Show_Records_CXF()
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                SqlDataAdapter adp = new SqlDataAdapter("GetCXFAvaiableCount", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = custid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = procDate;

                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<Exe_Count_CXF>();
                Exe_Count_CXF def;


                if (ds.Tables[0].Rows.Count > 0)
                {
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;

                    for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        def = new Exe_Count_CXF
                        {

                            ClearingType = ds.Tables[0].Rows[0].ItemArray[0].ToString(),
                            TotalCount = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[1].ToString()),
                            Available = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[2].ToString()),
                            Duplicate = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[3].ToString()),
                            HomeClearing = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[4].ToString()),
                        };
                        objectlst.Add(def);
                    }

                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                }
                ViewBag.Heading = "CXF Records Count";

                return PartialView("_Exe_Count_CXF", objectlst);
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

        public PartialViewResult Show_Records_BRF()
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                SqlDataAdapter adp = new SqlDataAdapter("GetBRFCount", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = custid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = procDate;

                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<Exe_Count_BRF>();
                Exe_Count_BRF def;


                if (ds.Tables[0].Rows.Count > 0)
                {
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;

                    for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        def = new Exe_Count_BRF
                        {
                            ClearingType = ds.Tables[0].Rows[0].ItemArray[0].ToString(),
                            TotalCount = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[1].ToString()),
                            Presented = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[2].ToString()),
                            Returned = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[3].ToString()),
                            Extention = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[4].ToString()),
                        };
                        objectlst.Add(def);
                    }

                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                }
                ViewBag.Heading = "BRF Records Count";

                return PartialView("_Exe_Count_BRF", objectlst);
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

        public PartialViewResult Show_Records_PPS()
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                SqlDataAdapter adp = new SqlDataAdapter("GetPPSAvaiableCount", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = custid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = procDate;

                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<Exe_Count_PPS>();
                Exe_Count_PPS def;


                if (ds.Tables[0].Rows.Count > 0)
                {
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;

                    for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        def = new Exe_Count_PPS
                        {
                            TotalCount = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0].ToString()),
                            Available = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[1].ToString()),
                            Presented = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[2].ToString()),
                            Duplicate = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[3].ToString()),
                        };
                        objectlst.Add(def);
                    }

                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                }
                ViewBag.Heading = "PPS Records Count";

                return PartialView("_Exe_Count_PPS", objectlst);
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

    }
}
