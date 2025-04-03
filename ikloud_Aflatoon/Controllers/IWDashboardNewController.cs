using ikloud_Aflatoon.Infrastructure;
using ikloud_Aflatoon.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using NPOI.SS.Formula.Functions;

namespace ikloud_Aflatoon.Controllers
{
    public class IWDashboardNewController : Controller
    {
        //
        // GET: /IWDashboard/
        IWProcDataContext dc = new IWProcDataContext();
        AflatoonEntities af = new AflatoonEntities();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);

        public ActionResult Index()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            //----------------------------------Checking user rights--------------------//

            if ((bool)Session["Ds"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            try
            {
                // List<IwSessionDashBoardData> chequeDetails = new List<IwSessionDashBoardData>();//GetDataDetails(); 
                ViewBag.SessionExpiryTime = Session["SessionExpiryTime_dropDown"];
                string defaultsession = "";
                List<IwSessionDashBoardData> chequeDetails= GetDataDetails(defaultsession);
                return View(chequeDetails);
               // return View();
            }
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;
                logerror("In IWDashboardNew Index Get Catch ===>>" + message, "InnerExp==>" + innerExcp);
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
               
            }


           
        }
        [HttpPost]
        public ActionResult Index(string btn = null, string sesntp = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            //----------------------------------Checking user rights--------------------//

            if ((bool)Session["Ds"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            if (sesntp == "01")
                Session["Clrtype"] = "01";
            else
                Session["Clrtype"] = "11";
            //ViewBag.clrtype = false;
            return View();
        }


        private List<IwSessionDashBoardData> GetDataDetails(string selectedsession=null)
        {
            List<IwSessionDashBoardData> data = new List<IwSessionDashBoardData>();

            var procdate= Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
            var CustId = Convert.ToInt16(Session["CustomerID"]);

            var headersession = selectedsession;
            if (headersession == "") { headersession = "All"; }

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("GetIwSessionDashboardData_New", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Processingdate", procdate);
                    cmd.Parameters.AddWithValue("@CustomerId", CustId);
                    cmd.Parameters.AddWithValue("@ExpiryTime", selectedsession);
                    //cmd.Parameters.AddWithValue("@Processingdate", procdate);

                    SqlDataAdapter sda= new SqlDataAdapter(cmd);
                    DataSet ds= new DataSet();
                    sda.Fill(ds);

                    if (ds.Tables.Count > 0)
                    {
                        DataTable dt = new DataTable();
                        dt=ds.Tables[0];

                        foreach (DataRow dr in dt.Rows) 
                        {
                            IwSessionDashBoardData dashboardData = new IwSessionDashBoardData
                            {
                                //SettlementPeriod = dr["SettlementPeriod"].ToString(),
                                //TotalChequesReceived = Convert.ToInt32(dr["TotalChequesReceived"]),
                                //PendingForSubmission = Convert.ToInt32(dr["PendingForSubmission"]),
                                //ResponseSubmitted = Convert.ToInt32(dr["ResponseSubmitted"]),
                                //SuccessPosted = Convert.ToInt32(dr["SuccessPosted"]),
                                //ReturnPosted = Convert.ToInt32(dr["ReturnPosted"]),
                                //FailedPostings = Convert.ToInt32(dr["FailedPostings"]),
                                //PendingForPosting = Convert.ToInt32(dr["PendingForPosting"]),
                                //L1Pending = Convert.ToInt32(dr["L1Pending"]),
                                //L2Pending = Convert.ToInt32(dr["L2Pending"]),
                                //L3Pending = Convert.ToInt32(dr["L3Pending"]),
                                //Locked = Convert.ToInt32(dr["Locked"]),
                                //KoresSQPending = Convert.ToInt32(dr["KoresSQPending"])


                                Id = Convert.ToInt32(dr["Id"]),
                                StartTime = dr["StartTime"].ToString(),
                                EndTime = dr["EndTime"].ToString(),
                                ExpiryTime = dr["ExpiryTime"].ToString(),
                                TotalInstruments = Convert.ToInt32(dr["TotalInstruments"]),
                                PendingforNPCI = Convert.ToInt32(dr["PendingforNPCI"]),
                                SubmittedToNPCI = Convert.ToInt32(dr["SubmittedToNPCI"]),
                                SuccessPosted = Convert.ToInt32(dr["SuccessPosted"]),
                                ReturnPosted = Convert.ToInt32(dr["ReturnPosted"]),
                                PendingforCBSPosting = Convert.ToInt32(dr["PendingforCBSPosting"]),
                                FailedCBSPosted = Convert.ToInt32(dr["FailedCBSPosted"]),
                                SucessCBSPosted = Convert.ToInt32(dr["SucessCBSPosted"]),
                                PendingforL1 = Convert.ToInt32(dr["PendingforL1"]),
                                PendingforL2 = Convert.ToInt32(dr["PendingforL2"]),
                                PendingforL3 = Convert.ToInt32(dr["PendingforL3"]),
                                PendingSQ = Convert.ToInt32(dr["PendingSQ"]),
                                L1LockCount = Convert.ToInt32(dr["L1LockCount"]),
                                L2LockCount = Convert.ToInt32(dr["L2LockCount"]),
                                L3LockCount = Convert.ToInt32(dr["L3LockCount"]),
                                HeaderExpiryTime= headersession




                            };
                            data.Add(dashboardData);



                        }
                    }

                    
                }
            }
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                string trace = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                {
                    innerExcp = e.InnerException.Message;
                    trace = e.InnerException.StackTrace;
                }
                logerror("In GetDataDetails Catch==>>" + message, "InnerExp===>>" + innerExcp);
            }

          

            return data;
        }

        public PartialViewResult GetSessionDashBoardData(string SelectedValue=null)
        {
            List<IwSessionDashBoardData> datalst = new List<IwSessionDashBoardData>();
            try
            {
                
               
                datalst = GetDataDetails(SelectedValue);
                



            }
            catch (Exception e)
            {


                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;
                logerror("In GetSessionDashBoardData  Catch ===>>" + message, "InnerExp==>" + innerExcp);
               
            }

            return PartialView("_SessionDashboard", datalst);
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


    }
}
