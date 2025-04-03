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
    public class ScanningCutOffTimeController : Controller
    {
        //
        // GET: /ScanningCutOffTime/
        AflatoonEntities af = new AflatoonEntities();
        //IWProcDataContext iwpro = new IWProcDataContext();
        //OWProcDataContext OWpro = new OWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);

        public ActionResult Index()
        {
            logerror("in Index", "");
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }

            try
            {
                return View();
            }
            catch (Exception e)
            {
                logerror("in Index catch", e.Message);
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }


        }

        public JsonResult GetDomainList()
        {
            try
            {
                logerror("in GetDomainList", "");
                int uid = (int)Session["uid"];
                int customerId = Convert.ToInt16(Session["CustomerID"]);

                var resultdomain = (from du in af.DomainUserMapMasters
                                    from d in af.DomainMaster

                                    where du.DomainId == d.Id && du.CustomerID == customerId && du.UserId == uid
                                    select new
                                    {
                                        d.Id,
                                        d.Name
                                    }).ToList().Distinct().OrderByDescending(m => m.Id);

                logerror("in GetDomainList", "LN60");

                return Json(resultdomain, JsonRequestBehavior.AllowGet);
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
                //logerror(e.Message, e.InnerException.ToString());
                logerror("in GetDomainList catch==>", message + "InnerExp==>" + innerExcp);
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        public class ScanningCutOffTime_Data
        {
            public string CutOfTime { get; set; }
            public string IsDisabled { get; set; }
            public string TempExtensionTime { get; set; }
            public string AllowedInstrumentCount { get; set; }

            public string InstrumentsScannedAfterCutOff { get; set; }

            public string ScanAlertTime { get; set; }
        }

        public JsonResult GetScanningCutOffTimeForDomain(int DomainId = 0)
        {
            try
            {
                int uid = (int)Session["uid"];
                int customerId = Convert.ToInt16(Session["CustomerID"]);

                SqlDataAdapter adp = new SqlDataAdapter("Get_Scanning_CutOffTime_For_Domain", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.NVarChar).Value = customerId;
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = DomainId;

                DataSet ds = new DataSet();
                adp.Fill(ds);
                ScanningCutOffTime_Data def = new ScanningCutOffTime_Data();

                if (ds.Tables.Count > 0)
                {
                    def = new ScanningCutOffTime_Data
                    {
                        CutOfTime = ds.Tables[0].Rows[0]["CutOfTime"] == null ? "" : ds.Tables[0].Rows[0]["CutOfTime"].ToString(),
                        IsDisabled = ds.Tables[0].Rows[0]["IsCutOffDisabled"] == null ? "" : ds.Tables[0].Rows[0]["IsCutOffDisabled"].ToString(),
                        TempExtensionTime = ds.Tables[0].Rows[0]["TempExtensionTime"] == null ? "" : ds.Tables[0].Rows[0]["TempExtensionTime"].ToString(),
                        AllowedInstrumentCount = ds.Tables[0].Rows[0]["AllowedInstrumentCount"] == null ? "" : ds.Tables[0].Rows[0]["AllowedInstrumentCount"].ToString(),
                        InstrumentsScannedAfterCutOff = ds.Tables[0].Rows[0]["InstrumentsScannedAfterCutOff"] == null ? "" : ds.Tables[0].Rows[0]["InstrumentsScannedAfterCutOff"].ToString(),
                        ScanAlertTime = ds.Tables[0].Rows[0]["ScanAlertTime"] == null ? "" : ds.Tables[0].Rows[0]["ScanAlertTime"].ToString(),
                    };
                }

                return Json(def.CutOfTime + "," + def.IsDisabled + "," + def.TempExtensionTime + "," + def.AllowedInstrumentCount + ","
                    + def.InstrumentsScannedAfterCutOff + "," + def.ScanAlertTime, JsonRequestBehavior.AllowGet);
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
                //logerror(e.Message, e.InnerException.ToString());
                logerror("in GetScanningCutOffTimeForDomain catch==>", message + "InnerExp==>" + innerExcp);
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Update_ScanningCutOffTime(int DomainId = 0, string IsEnabled = null, string ScanningCutOfTime = null,
            string TempExtensionTime = null, string AllowedInstrumentCount = null, string ScanAlertTime = null)
        {
            try
            {
                int custId = Convert.ToInt16(Session["CustomerID"]);
                int uid = (int)Session["uid"];

                SqlCommand cmd = new SqlCommand("Update_Scanning_CutOffTime_For_Domain", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerId", custId);
                cmd.Parameters.AddWithValue("@DomainId", DomainId);
                cmd.Parameters.AddWithValue("@IsEnable", IsEnabled);
                cmd.Parameters.AddWithValue("@ScanningCutOfTime", ScanningCutOfTime);
                cmd.Parameters.AddWithValue("@TempExtensionTime", TempExtensionTime);
                cmd.Parameters.AddWithValue("@AllowedInstrumentCount", AllowedInstrumentCount);
                cmd.Parameters.AddWithValue("@Uid", uid);
                cmd.Parameters.AddWithValue("@ScanAlertTime", ScanAlertTime);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                return RedirectToAction("Index", "ScanningCutOffTime");
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


    }
}
