using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace ikloud_Aflatoon.Controllers
{
    public class OutwardOrganizationDashboardController : Controller
    {
        //
        // GET: /OutwardOrganizationDashboard/

        AflatoonEntities db = new AflatoonEntities();

        public ActionResult Index()
        {
            //Session["processdate"] = DateTime.ParseExact("2017-01-19", "yyyy-MM-dd", null);//get processdate from application
            //Session["CustomerID"] = 6;//get custId from application
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["Ds"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid1);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            try
            {
                int custid = Convert.ToInt16(Session["CustomerID"]);

                DateTime procdate = Convert.ToDateTime(Session["processdate"].ToString());

                var dshboard = db.OutwardOrganizationDashBoard.Where(m => m.ProcessDate == procdate && m.CustomerId == custid).ToList();
                return View(dshboard);
            }
            catch (Exception e)
            {
                
              string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OrgnizationDashboard HttpGet Index- " + innerExcp });
            }
           
        }

        [HttpPost]
        public ActionResult Index(string refresh)
        {

            //Session["processdate"] = DateTime.ParseExact("2017-01-19", "yyyy-MM-dd", null);//get processdate from application
            //Session["CustomerID"] = 6;//get custId from application
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["Ds"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid1);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            int custId = Convert.ToInt16(Session["CustomerID"]);

            DateTime procdate = Convert.ToDateTime(Session["processdate"].ToString());

            var orgId = db.CustomerMasters.Where(m => m.Id == custId).Select(m => m.OrganizationId).SingleOrDefault();
            int intOrgId = Convert.ToInt16(orgId);
            try
            {

                SqlConnection con = new SqlConnection();

                con = db.Database.Connection as SqlConnection;

                SqlDataAdapter adp = new SqlDataAdapter("GetOutwardOrganizationDashBoardData", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@ProcessDate", SqlDbType.Date).Value = procdate;
                adp.SelectCommand.Parameters.Add("@OrganizationId", SqlDbType.Int).Value = intOrgId;

                DataSet ds = new DataSet();
                adp.Fill(ds);

                var dshboard = db.OutwardOrganizationDashBoard.Where(m => m.ProcessDate == procdate && m.CustomerId == custId).ToList();
                return View(dshboard);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OrgnizationDashboard HttpPost Index- " + innerExcp });
            }
        }

    }
}
