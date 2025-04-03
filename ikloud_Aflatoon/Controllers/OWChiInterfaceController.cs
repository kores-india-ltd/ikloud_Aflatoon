using ikloud_Aflatoon.Infrastructure;
using ikloud_Aflatoon.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace ikloud_Aflatoon.Controllers
{
    public class OWChiInterfaceController : Controller
    {
        //
        // GET: /OWChiInterface/
        AflatoonEntities af = new AflatoonEntities();
        IWProcDataContext iwpro = new IWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);

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
            public string IS_Load_Vendor_File_Status { get; set; }

            public string IS_Read_LVB_CXF_Status { get; set; }
            public string IS_Read_LVB_BRF_Status { get; set; }
            public string IS_Read_LVB_PPS_Status { get; set; }

            public string IS_Create_IB_CXF_Status { get; set; }
            public string IS_Read_IB_RRF_Status { get; set; }
            
            public string IS_Outward_Extract_Status { get; set; }
            public string IS_Return_Extract_Status { get; set; }
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

                //logerror("In CheckDEM ", "IS_Read_LVB_BRF_Clicked -> " + Session["IS_Read_LVB_BRF_Clicked"].ToString());
                //logerror("In CheckDEM ", "IS_Read_LVB_CXF_Clicked -> " + Session["IS_Read_LVB_CXF_Clicked"].ToString());
                //logerror("In CheckDEM ", "IS_Read_LVB_PPS_Clicked -> " + Session["IS_Read_LVB_PPS_Clicked"].ToString());

                if (Session["IS_Create_CXF_Clicked"] != null)
                {
                    if (Session["IS_Create_CXF_Clicked"].ToString() == "Y")
                    {
                        //logerror("In CheckDEM In Session Clicked ", "IS_Create_CXF_Clicked -> " + Session["IS_Create_CXF_Clicked"].ToString());
                        con.Open();
                        SqlCommand cmd = new SqlCommand("CheckStatusForDemOutwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        //logerror("In CheckDEM In Session Clicked ", "CustomerId -> " + custid);
                        //logerror("In CheckDEM In Session Clicked ", "ProcessingDate -> " + procDate);
                        //logerror("In CheckDEM In Session Clicked ", "RecordID -> " + Convert.ToInt64(Session["IS_Create_CXF_RecordID"].ToString()));
                        //logerror("In CheckDEM In Session Clicked ", "RequestedBy -> " + uid);

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RecordID", Convert.ToInt64(Session["IS_Create_CXF_RecordID"].ToString()));
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        //logerror("In CheckDEM In Session Clicked ", "IS_Create_CXF_Clicked Sql Reader has rows -> " + reader.HasRows);

                        while (reader.Read())
                        {
                            res.IS_Create_CXF_Status = reader["Status"].ToString();
                        }

                        con.Close();
                        Session["IS_Create_CXF_Status"] = res.IS_Create_CXF_Status;

                        if(res.IS_Create_CXF_Status == "2" || res.IS_Create_CXF_Status == "4")
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
                if (Session["IS_Read_LVB_BRF_Clicked"] != null)
                {
                    if (Session["IS_Read_LVB_BRF_Clicked"].ToString() == "Y")
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("CheckStatusForDemOutwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RecordID", Convert.ToInt64(Session["IS_Read_LVB_BRF_RecordID"].ToString()));
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            res.IS_Read_LVB_BRF_Status = reader["Status"].ToString();
                        }

                        con.Close();
                        Session["IS_Read_LVB_BRF_Status"] = res.IS_Read_LVB_BRF_Status;

                        if (res.IS_Read_LVB_BRF_Status == "2" || res.IS_Read_LVB_BRF_Status == "4")
                        {
                            Session["IS_Read_LVB_BRF_Clicked"] = "N";
                        }

                    }
                }
                if (Session["IS_Read_LVB_CXF_Clicked"] != null)
                {
                    if (Session["IS_Read_LVB_CXF_Clicked"].ToString() == "Y")
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("CheckStatusForDemOutwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RecordID", Convert.ToInt64(Session["IS_Read_LVB_CXF_RecordID"].ToString()));
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            res.IS_Read_LVB_CXF_Status = reader["Status"].ToString();
                        }

                        con.Close();
                        Session["IS_Read_LVB_CXF_Status"] = res.IS_Read_LVB_CXF_Status;

                        if (res.IS_Read_LVB_CXF_Status == "2" || res.IS_Read_LVB_CXF_Status == "4")
                        {
                            Session["IS_Read_LVB_CXF_Clicked"] = "N";
                        }

                    }
                }
                if (Session["IS_Read_LVB_PPS_Clicked"] != null)
                {
                    if (Session["IS_Read_LVB_PPS_Clicked"].ToString() == "Y")
                    {
                        //logerror("In CheckDEM In Session Clicked ", "IS_Read_LVB_PPS_Clicked -> " + Session["IS_Read_LVB_PPS_Clicked"].ToString());
                        con.Open();
                        SqlCommand cmd = new SqlCommand("CheckStatusForDemOutwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        //logerror("In CheckDEM In Session Clicked ", "CustomerId -> " + custid);
                        //logerror("In CheckDEM In Session Clicked ", "ProcessingDate -> " + procDate);
                        //logerror("In CheckDEM In Session Clicked ", "RecordID -> " + Convert.ToInt64(Session["IS_Read_LVB_PPS_RecordID"].ToString()));
                        //logerror("In CheckDEM In Session Clicked ", "RequestedBy -> " + uid);

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RecordID", Convert.ToInt64(Session["IS_Read_LVB_PPS_RecordID"].ToString()));
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        //logerror("In CheckDEM In Session Clicked ", "IS_Read_LVB_PPS_Clicked Sql Reader has rows -> " + reader.HasRows);
                        
                        while (reader.Read())
                        {
                            //logerror("In CheckDEM In Session Clicked In reader ", "Reader_Status -> " + reader["Status"].ToString());
                            res.IS_Read_LVB_PPS_Status = reader["Status"].ToString();
                            //logerror("In CheckDEM In Session Clicked In reader ", "IS_Read_LVB_PPS_Status -> " + res.IS_Read_LVB_PPS_Status);
                        }

                        con.Close();
                        Session["IS_Read_LVB_PPS_Status"] = res.IS_Read_LVB_PPS_Status;

                        if (res.IS_Read_LVB_PPS_Status == "2" || res.IS_Read_LVB_PPS_Status == "4")
                        {
                            Session["IS_Read_LVB_PPS_Clicked"] = "N";
                        }

                    }
                }
                if (Session["IS_Create_IB_CXF_Clicked"] != null)
                {
                    if (Session["IS_Create_IB_CXF_Clicked"].ToString() == "Y")
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("CheckStatusForDemOutwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RecordID", Convert.ToInt64(Session["IS_Create_IB_CXF_RecordID"].ToString()));
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            res.IS_Create_IB_CXF_Status = reader["Status"].ToString();
                        }

                        con.Close();
                        Session["IS_Create_IB_CXF_Status"] = res.IS_Create_IB_CXF_Status;

                        if (res.IS_Create_IB_CXF_Status == "2" || res.IS_Create_IB_CXF_Status == "4")
                        {
                            Session["IS_Create_IB_CXF_Clicked"] = "N";
                        }

                    }
                }
                if (Session["IS_Read_IB_RRF_Clicked"] != null)
                {
                    if (Session["IS_Read_IB_RRF_Clicked"].ToString() == "Y")
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("CheckStatusForDemOutwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RecordID", Convert.ToInt64(Session["IS_Read_IB_RRF_RecordID"].ToString()));
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            res.IS_Read_IB_RRF_Status = reader["Status"].ToString();
                        }

                        con.Close();
                        Session["IS_Read_IB_RRF_Status"] = res.IS_Read_IB_RRF_Status;

                        if (res.IS_Read_IB_RRF_Status == "2" || res.IS_Read_IB_RRF_Status == "4")
                        {
                            Session["IS_Read_IB_RRF_Clicked"] = "N";
                        }

                    }
                }
                if (Session["IS_Load_Vendor_File_Clicked"] != null)
                {
                    if (Session["IS_Load_Vendor_File_Clicked"].ToString() == "Y")
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("CheckStatusForDemOutwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RecordID", Convert.ToInt64(Session["IS_Load_Vendor_File_RecordID"].ToString()));
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            res.IS_Load_Vendor_File_Status = reader["Status"].ToString();
                        }

                        con.Close();
                        Session["IS_Load_Vendor_File_Status"] = res.IS_Load_Vendor_File_Status;

                        if (res.IS_Load_Vendor_File_Status == "2" || res.IS_Load_Vendor_File_Status == "4")
                        {
                            Session["IS_Load_Vendor_File_Clicked"] = "N";
                        }

                    }
                }
                if (Session["IS_Outward_Extract_Clicked"] != null)
                {
                    if (Session["IS_Outward_Extract_Clicked"].ToString() == "Y")
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("CheckStatusForDemOutwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RecordID", Convert.ToInt64(Session["IS_Outward_Extract_RecordID"].ToString()));
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            res.IS_Outward_Extract_Status = reader["Status"].ToString();
                        }

                        con.Close();
                        Session["IS_Outward_Extract_Status"] = res.IS_Outward_Extract_Status;

                        if (res.IS_Outward_Extract_Status == "2" || res.IS_Outward_Extract_Status == "4")
                        {
                            Session["IS_Outward_Extract_Clicked"] = "N";
                        }

                    }
                }
                if (Session["IS_Return_Extract_Clicked"] != null)
                {
                    if (Session["IS_Return_Extract_Clicked"].ToString() == "Y")
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("CheckStatusForDemOutwardRequest", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                        cmd.Parameters.AddWithValue("@RecordID", Convert.ToInt64(Session["IS_Return_Extract_RecordID"].ToString()));
                        cmd.Parameters.AddWithValue("@RequestedBy", uid);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            res.IS_Return_Extract_Status = reader["Status"].ToString();
                        }

                        con.Close();
                        Session["IS_Return_Extract_Status"] = res.IS_Return_Extract_Status;

                        if (res.IS_Return_Extract_Status == "2" || res.IS_Return_Extract_Status == "4")
                        {
                            Session["IS_Return_Extract_Clicked"] = "N";
                        }

                    }
                }

                //logerror("In CheckDEM In Session Clicked In  ", "res -> " + res);
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

                if(reader > 0)
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
                Session["IS_Read_LVB_BRF_Clicked"] = "N";
                Session["IS_Read_LVB_CXF_Clicked"] = "N";
                Session["IS_Read_LVB_PPS_Clicked"] = "N";
                Session["IS_Create_IB_CXF_Clicked"] = "N";
                Session["IS_Read_IB_RRF_Clicked"] = "N";
                Session["IS_Load_Vendor_File_Clicked"] = "N";

                Session["IS_Outward_Extract_Clicked"] = "N";
                Session["IS_Return_Extract_Clicked"] = "N";

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

            //CheckDEM_Session();
            if (msg1 != null)
            {
                ViewBag.Message1 = msg1;
            }
            if (msg2 != null)
            {
                ViewBag.Message2 = msg2;
            }
            @Session["glob"] = null;
            Session["CXF_C_S"] = Session["DEMCXF_C_Start"].ToString();
            Session["CXF_C_E"] = Session["DEMCXF_C_End"].ToString();

            Session["CXF_U_S"] = Session["DEMCXF_U_Start"].ToString();
            Session["CXF_U_E"] = Session["DEMCXF_U_End"].ToString();


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

        //----------------- LVB 
        public JsonResult Read_LVB_BRF(int Id = 0)
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
                Session["IS_Read_LVB_BRF_RecordID"] = res.Result3; 
                Session["IS_Read_LVB_BRF_Clicked"] = "Y";
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

        public JsonResult Read_LVB_CXF(int Id = 0)
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
                Session["IS_Read_LVB_CXF_RecordID"] = res.Result3; 
                Session["IS_Read_LVB_CXF_Clicked"] = "Y";
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

        public JsonResult Read_LVB_PPS(int Id = 0)
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
                Session["IS_Read_LVB_PPS_RecordID"] = res.Result3;
                Session["IS_Read_LVB_PPS_Clicked"] = "Y";
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

        public JsonResult Create_IB_CXF(int Id = 0)
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
                Session["IS_Create_IB_CXF_RecordID"] = res.Result3; 
                Session["IS_Create_IB_CXF_Clicked"] = "Y";
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

        public JsonResult Read_IB_RRF(int Id = 0)
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
                Session["IS_Read_IB_RRF_RecordID"] = res.Result3; 
                Session["IS_Read_IB_RRF_Clicked"] = "Y";
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

        //----------------- VendorFile
        public JsonResult ReadVendorFile(int Id = 0)
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
                Session["IS_Load_Vendor_File_RecordID"] = res.Result3; 
                Session["IS_Load_Vendor_File_Clicked"] = "Y";
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

        //----------------- CBS Extract Outward
        public JsonResult OutwardExtract(int Id = 0)
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
                Session["IS_Outward_Extract_RecordID"] = res.Result3;
                Session["IS_Outward_Extract_Clicked"] = "Y";
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

        //----------------- CBS Extract Return
        public JsonResult ReturnExtract(int Id = 0)
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
                Session["IS_Return_Extract_RecordID"] = res.Result3;
                Session["IS_Return_Extract_Clicked"] = "Y";
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
                            FileId = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[0].ToString()),
                            FileName = ds.Tables[0].Rows[i].ItemArray[1].ToString(),
                            Status = ds.Tables[0].Rows[i].ItemArray[2].ToString(),
                            //ClearingType = ds.Tables[0].Rows[i].ItemArray[3].ToString(),
                            ItemCount = Convert.ToInt16(ds.Tables[0].Rows[i].ItemArray[3].ToString()),
                            AcceptCount = ds.Tables[0].Rows[i].ItemArray[2].ToString() == "RES Received" ? (Convert.ToInt16(ds.Tables[0].Rows[i].ItemArray[3].ToString()) - Convert.ToInt16(ds.Tables[0].Rows[i].ItemArray[4].ToString())) : 0,
                            RejectCount = Convert.ToInt16(ds.Tables[0].Rows[i].ItemArray[4].ToString()),
                            CreationTimeStamp = ds.Tables[0].Rows[i].ItemArray[5].ToString(),
                            UploadTimeStamp = ds.Tables[0].Rows[i].ItemArray[6].ToString(),
                            RESTimeStamp = ds.Tables[0].Rows[i].ItemArray[7].ToString(),
                            //OACKTimeStamp = ds.Tables[0].Rows[i].ItemArray[10].ToString()
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

        public PartialViewResult Show_Files_VendorFile()
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                SqlDataAdapter adp = new SqlDataAdapter("GetVendorFileStatus", con);
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
                ViewBag.Heading = "Vendor File Status";

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

        public PartialViewResult Show_Files_LVB_BRF()
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                SqlDataAdapter adp = new SqlDataAdapter("GetLVBBrfFileStatus", con);
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
                ViewBag.Heading = "LVB BRF File Status";

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

        public PartialViewResult Show_Files_LVB_CXF()
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                SqlDataAdapter adp = new SqlDataAdapter("GetLVBCxfFileStatus", con);
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
                ViewBag.Heading = "LVB CXF File Status";

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

                            ClearingType = ds.Tables[0].Rows[i].ItemArray[0].ToString(),
                            TotalCount = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[1].ToString()),
                            Available = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[2].ToString()),
                            Duplicate = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[3].ToString()),
                            HomeClearing = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[4].ToString()),
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

        public PartialViewResult Show_Records_VendorFile()
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                SqlDataAdapter adp = new SqlDataAdapter("GetVendorAvaiableCount", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = custid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = procDate;

                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<Exe_Count_Vendor>();
                Exe_Count_Vendor def;


                if (ds.Tables[0].Rows.Count > 0)
                {
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;

                    for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        def = new Exe_Count_Vendor
                        {
                            LoadedCount = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[0].ToString()),
                            ProcessedCount = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[1].ToString()),
                            AcceptedCount = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[2].ToString()),
                            RejecedCount = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[3].ToString()),
                            PresentToDEM = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[4].ToString()),
                        };
                        objectlst.Add(def);
                    }
                    
                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                }
                ViewBag.Heading = "Vendor File Records Count";

                return PartialView("_Exe_Count_Vendor", objectlst);
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
                            ClearingType = ds.Tables[0].Rows[i].ItemArray[0].ToString(),
                            TotalCount = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[1].ToString()),
                            Presented = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[2].ToString()),
                            Returned = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[3].ToString()),
                            Extention = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[4].ToString()),
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
                            TotalCount = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[0].ToString()),
                            Available = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[1].ToString()),
                            Presented = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[2].ToString()),
                            Duplicate = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[3].ToString()),
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

        public PartialViewResult Show_Records_LVB_BRF()
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                SqlDataAdapter adp = new SqlDataAdapter("GetLvbBRFCount", con);
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
                            ClearingType = ds.Tables[0].Rows[i].ItemArray[0].ToString(),
                            TotalCount = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[1].ToString()),
                            Presented = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[2].ToString()),
                            Returned = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[3].ToString()),
                            Extention = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[4].ToString()),
                        };
                        objectlst.Add(def);
                    }
                    
                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                }
                ViewBag.Heading = "LVB BRF Records Count";

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

        public PartialViewResult Show_Records_LVB_CXF()
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                SqlDataAdapter adp = new SqlDataAdapter("GetLvbCXFAvaiableCount", con);
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

                            ClearingType = ds.Tables[0].Rows[i].ItemArray[0].ToString(),
                            TotalCount = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[1].ToString()),
                            Available = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[2].ToString()),
                            Duplicate = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[3].ToString()),
                            HomeClearing = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[4].ToString()),
                        };
                        objectlst.Add(def);
                    }
                    
                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                }
                ViewBag.Heading = "LVB CXF Records Count";

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



        //sambhhaji 27-09-24 reset all 
        public JsonResult ResetAllInstrument()
        {
            string msg = "";
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);


                con.Open();
                SqlCommand cmd = new SqlCommand("Ow_ResetlatePresentedInstrument", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CustomerId", custid);
                cmd.Parameters.AddWithValue("@ProcessingDate", procDate);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    msg = "Records Updated Successfully..";
                }
                else
                {
                    msg = "no record update";
                }

                return Json(msg, JsonRequestBehavior.AllowGet); ;
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


        public PartialViewResult Show_AllCount_Reset()
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                SqlDataAdapter adp = new SqlDataAdapter("GetLatePresentAvaiableCount", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = custid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = procDate;

                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<Show_AllCount_Reset>();
                Show_AllCount_Reset def;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;

                    for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        def = new Show_AllCount_Reset
                        {
                            ClearingType = ds.Tables[0].Rows[i].ItemArray[0].ToString(),
                            TotalCount = ds.Tables[0].Rows[i].ItemArray[1].ToString(),
                        };
                        objectlst.Add(def);
                    }
                    ViewBag.cnt = true;
                    @Session["glob"] = null;

                }
                ViewBag.Heading = "Show Count ";

                return PartialView("_Show_AllCount_Reset", objectlst);
            }
            catch (Exception e)
            {

                ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                er.ErrorMessage = innerExcp;

                return PartialView("Error", er);
            }
        }


        public PartialViewResult Show_AllDetails_Reset()
        {
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                SqlDataAdapter adp = new SqlDataAdapter("GetLatePresentAvaiableDetails", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = custid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = procDate;

                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<Show_AllDetails_Reset>();
                Show_AllDetails_Reset def;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;

                    for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        def = new Show_AllDetails_Reset
                        {
                            ClearingType = ds.Tables[0].Rows[i].ItemArray[0].ToString(),
                            Processingdate = ds.Tables[0].Rows[i].ItemArray[1].ToString(),
                            CustomerId = ds.Tables[0].Rows[i].ItemArray[2].ToString(),
                            ChequeNo = ds.Tables[0].Rows[i].ItemArray[3].ToString(),
                            SortCode = ds.Tables[0].Rows[i].ItemArray[4].ToString(),
                            San = ds.Tables[0].Rows[i].ItemArray[5].ToString(),
                            Tc = ds.Tables[0].Rows[i].ItemArray[6].ToString(),
                            Amount = ds.Tables[0].Rows[i].ItemArray[7].ToString(),
                        };
                        objectlst.Add(def);
                    }
                    ViewBag.cnt = true;
                    @Session["glob"] = null;

                }
                ViewBag.Heading = "Show Details";

                return PartialView("_Show_AllDetails_Reset", objectlst);
            }
            catch (Exception e)
            {

                ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                er.ErrorMessage = innerExcp;

                return PartialView("Error", er);
            }
        }


        //get cxf create time
        

        public bool IsCurrentTimeWithinRange(string flag)
        {
            bool isWithinRange = false;
            int custid = Convert.ToInt16(Session["CustomerID"]);
            var sp_name = "";
            if (flag == "C") { sp_name = "GetCXF_C_TimeSettings"; } else if (flag == "U") { sp_name = "GetCXF_U_TimeSettings"; }

                 try
                 {
                    using (SqlCommand cmd = new SqlCommand(sp_name, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CustomerId", custid);
                        con.Open();
                        var result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            isWithinRange = Convert.ToBoolean(result);  
                        }
                    }
                }
                catch (Exception ex)
                {
                    
                }
            

            return isWithinRange;
        }

        [HttpPost]
        public JsonResult GetCXF_C_TimeFlag()
        {
            int custid = Convert.ToInt16(Session["CustomerID"]);
            bool isWithinRange = IsCurrentTimeWithinRange("C");

            return Json(new { isWithinRange = isWithinRange });
        }

        //get cxf upload time

        
       [HttpPost]
        public JsonResult GetCXF_U_TimeFlag()
        {
            int custid = Convert.ToInt16(Session["CustomerID"]);
            bool isWithinRange = IsCurrentTimeWithinRange("U");

            return Json(new { isWithinRange = isWithinRange });
        }

        //reset api 
       // [HttpPost]
        public JsonResult ResetFailAPI(int ApiType=0)
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
                SqlCommand cmd = new SqlCommand("OwResetAPIFailedCases", con);
                cmd.CommandType = CommandType.StoredProcedure;

               // cmd.Parameters.AddWithValue("@CustomerId", custid);
                cmd.Parameters.AddWithValue("@ProcessingDate", procDate);
                cmd.Parameters.AddWithValue("@RecordType", ApiType);

                int rowsAffected = cmd.ExecuteNonQuery();

                if(rowsAffected > 0)
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
                logerror("in OWResetAPI CATCH==>msg==>" + message, " InnerExp==>" + innerExcp);
                return Json(message, JsonRequestBehavior.AllowGet);
            }


            return Json(msg, JsonRequestBehavior.AllowGet);
        }

    }
}
