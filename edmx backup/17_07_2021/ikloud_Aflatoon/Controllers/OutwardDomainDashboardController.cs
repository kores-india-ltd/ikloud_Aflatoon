using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ikloud_Aflatoon.Controllers
{
    public class OutwardDomainDashboardController : Controller
    {
        AflatoonEntities db = new AflatoonEntities();

        //
        // GET: /OutwardDomainDashboard/

        public ActionResult Index()
        {

            //Session["processdate"] = DateTime.ParseExact("2017-01-19", "yyyy-MM-dd", null);
            //Session["CustomerID"] = 6;
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
                int uid = (int)Session["uid"];
                int custid = Convert.ToInt16(Session["CustomerID"]);
                int domainid = Convert.ToInt32(Session["DomainselectID"]);
                DateTime procdate = Convert.ToDateTime(Session["processdate"].ToString());

                //db.ProcessMaster.Where(p => (p.Domain.DomainName == Cdomain) && (p.Branch.BranchCode == brcode || brcode == null) && (p.ProcessDate == TxtFromDate)).Select(t => t.ID).ToList();
                List<int ?> domainlist = new List<int ?>();
                domainlist = db.DomainUserMapMasters.Where(d => d.UserId == uid).Select(d => d.DomainId).ToList();

                //var dshbrd = db.OutwardDomainDashBoard.Where(m => m.ProcessDate == procdate && m.CustomerId == custid && domainlist.Contains(m.DomainId)).ToList();
                var dshbrd = db.OutwardDomainDashBoard.Where(m => m.ProcessDate == procdate && m.CustomerId == custid ).ToList();
                return View(dshbrd);
            }
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWDomainDashboard HttpGet Index- " + innerExcp });
            }



        }

        [HttpPost]
        public ActionResult Index(string refresh)
        {
            //Session["processdate"] = DateTime.ParseExact("2017-01-19", "yyyy-MM-dd", null);
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
            DateTime dt = Convert.ToDateTime(Session["processdate"].ToString());

            try
            {
                int uid = (int)Session["uid"];
                SqlConnection con = new SqlConnection();

                con = db.Database.Connection as SqlConnection;

                SqlDataAdapter adp = new SqlDataAdapter("GetOutwardDomainDashBoardData", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@ProcessDate", SqlDbType.Date).Value = dt;
                adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = Convert.ToInt16(Session["CustomerID"]);

                DataSet ds = new DataSet();
                adp.Fill(ds);

                int custid = Convert.ToInt16(Session["CustomerID"]);
                int domainud = Convert.ToInt32(Session["DomainselectID"]);
                List<int?> domainlist = new List<int?>();
                domainlist = db.DomainUserMapMasters.Where(d => d.UserId == uid).Select(d => d.DomainId).ToList();

                //var dshbrd = db.OutwardDomainDashBoard.Where(m => m.ProcessDate == dt && m.CustomerId == custid && domainlist.Contains(m.DomainId)).ToList();
                var dshbrd = db.OutwardDomainDashBoard.Where(m => m.ProcessDate == dt && m.CustomerId == custid ).ToList();
                return View(dshbrd);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWDomainDashboard HttpPost Index- " + innerExcp });
            }
        }

    }
}
