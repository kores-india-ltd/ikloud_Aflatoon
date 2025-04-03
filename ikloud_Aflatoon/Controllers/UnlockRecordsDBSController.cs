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
    public class UnlockRecordsDBSController : Controller
    {
        //
        // GET: /UnlockRecordsDBS/
        AflatoonEntities af = new AflatoonEntities();
        UserAflatoonDbContext adc = new UserAflatoonDbContext();
        //OWProcDataContext OWpro = new OWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);

        public ActionResult Index()
        {
            var inow = Session["ProType"].ToString();
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["Settg"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            Session["glob"] = null;
            if (inow == "Inward")
            {
                return View("IWIndex");
            }
            else
            {
                return View();
            }

            
        }

        [HttpPost]
        public ActionResult unlockData(string modename = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["Settg"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            int uid = (int)Session["uid"];
            string unlockfield = "";
            Int64 rawdataid = 0;
            Int64 id = 0;
            unlockRecords unlk = new unlockRecords();

            try
            {
                unlockfield = modename;
                ViewBag.unlockfield = unlockfield;

                SqlDataAdapter adp = new SqlDataAdapter("UnlockRecordsDataDBS", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@ModuleName", SqlDbType.NVarChar).Value = unlockfield;
                adp.SelectCommand.Parameters.Add("@ClearingType", SqlDbType.NVarChar).Value = Session["ProType"].ToString();
                adp.SelectCommand.Parameters.Add("@processingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();

                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Session["DomainselectID"] == null ? 0 : Convert.ToInt32(Session["DomainselectID"]);

                DataSet ds = new DataSet();
                adp.Fill(ds);
                DataTable temptable = new DataTable();

                return View("_ShowData", ds);
            }
            catch(Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "unlockData HttpPost- " + innerExcp });
            }
        }

        public ActionResult Username(int id = 0)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["Settg"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            ViewBag.user = adc.UserMasters.Where(m => m.ID == id).Select(m => m.FirstName).FirstOrDefault();
            return View("_Username");
        }

        [HttpPost]
        public ActionResult clearLocks(string fieldname = null, Int64 Id = 0, Int64 RawDataId = 0, int Status = 0, string UserId = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["Settg"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            try
            {
                Int64 ID = Convert.ToInt64(Id);
                //OWpro.Clearlocks(ID, fieldname, Session["ProType"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"),
                //Convert.ToInt16(Session["CustomerID"]), 0, Session["LoginID"].ToString());

                SqlCommand com = new SqlCommand("Clearlocks", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@id", ID);
                com.Parameters.AddWithValue("@module", fieldname);
                com.Parameters.AddWithValue("@ClearingType", Session["ProType"].ToString());
                com.Parameters.AddWithValue("@processingDate", Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"));
                com.Parameters.AddWithValue("@CustomerID", Convert.ToInt16(Session["CustomerID"]));
                com.Parameters.AddWithValue("@DomainId", 0);
                com.Parameters.AddWithValue("@LoginID", Session["LoginID"].ToString());
                

                con.Open();
                com.ExecuteNonQuery();
                con.Close();

            }
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "clearLocks HttpPost- " + innerExcp });
            }
            finally
            {
                con.Close();
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}
