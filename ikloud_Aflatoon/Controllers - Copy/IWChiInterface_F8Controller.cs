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
    public class IWChiInterface_F8Controller : Controller
    {
        //
        // GET: /IWChiInterface_F8/
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
            public string IS_Create_RRF_Status { get; set; }
            public string IS_Upload_RRF_Status { get; set; }
            public string IS_Load_Res_Ocr_RRF_Status { get; set; }

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

        //------------ PXF
        public JsonResult LoadPxf(int Id = 0)
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
                Session["IS_Load_PXF_RecordID"] = res.Result3;
                Session["IS_Load_PXF_Clicked"] = "Y";
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

        public JsonResult ExtractDataPxf(int Id = 0)
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
                Session["IS_Extract_Data_PXF_RecordID"] = res.Result3;
                Session["IS_Extract_Data_PXF_Clicked"] = "Y";
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

        public JsonResult GenerateRmi(int Id = 0)
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
                Session["IS_Generate_RMI_RecordID"] = res.Result3;
                Session["IS_Generate_RMI_Clicked"] = "Y";
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

    }
}
