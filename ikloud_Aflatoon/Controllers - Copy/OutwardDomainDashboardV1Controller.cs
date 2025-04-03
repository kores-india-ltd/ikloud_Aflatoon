using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;


namespace ikloud_Aflatoon.Controllers
{

    public class OutwardDomainDashboardV1Controller : Controller
    {
        //
        // GET: /OutwardDomainDashboardV1/

        //OWProcDataContext db = new OWProcDataContext();
        AflatoonEntities conn = new AflatoonEntities();

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
                UserMaster usrm = conn.UserMasters.Find(uid1);
                usrm.Active = false;
                conn.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            try
            {

            

                int uid = (int)Session["uid"];
                int custid = Convert.ToInt16(Session["CustomerID"]);
                int domainid = Convert.ToInt32(Session["DomainselectID"]);
                DateTime procdate = Convert.ToDateTime(Session["processdate"].ToString());
                string ClrType = Session["CtsSessionType"].ToString();

                //db.ProcessMaster.Where(p => (p.Domain.DomainName == Cdomain) && (p.Branch.BranchCode == brcode || brcode == null) && (p.ProcessDate == TxtFromDate)).Select(t => t.ID).ToList();
                List<int?> domainlist = new List<int?>();
                domainlist = conn.DomainUserMapMasters.Where(d => d.UserId == uid).Select(d => d.DomainId).ToList();
                //var dshbrd = db.OutwardDomainDashBoard_V1s.Where(m => m.ProcessDate == procdate).ToList();

                var dshbrd = conn.OutwardDomainDashBoard_V2.Where(m => m.ProcessDate == procdate && m.CustomerId == custid && m.ClearingType == ClrType && domainlist.Contains(m.DomainId)).ToList().OrderBy(m => m.DomainId);
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

        //public ActionResult Index()
        //{

        //    //Session["processdate"] = DateTime.ParseExact("2017-07-31", "yyyy-MM-dd", null);
        //    //Session["CustomerID"] = 6;

        //    //DateTime procdate = Convert.ToDateTime(Session["processdate"].ToString());

        //    //var dshbrd = db.OutwardDomainDashBoard_V1s.Where(m => m.ProcessDate == procdate).ToList();
        //    //return View(dshbrd);

        //}

        //[HttpPost]
        //public ActionResult Index(string refresh)
        //{
        //    Session["processdate"] = DateTime.ParseExact("2017-07-31", "yyyy-MM-dd", null);
        //    DateTime dt = Convert.ToDateTime(Session["processdate"].ToString());

        //    SqlConnection con = new SqlConnection();

        //    con = conn.Database.Connection as SqlConnection;

        //    SqlDataAdapter adp = new SqlDataAdapter("GetOutwardDomainDashBoardData_V1", con);
        //    adp.SelectCommand.CommandType = CommandType.StoredProcedure;
        //    adp.SelectCommand.Parameters.Add("@ProcessDate", SqlDbType.Date).Value = dt;
        //    adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = Convert.ToInt16(Session["CustomerID"]);

        //    DataSet ds = new DataSet();
        //    adp.Fill(ds);

        //    var dshbrd = db.OutwardDomainDashBoard_V1s.ToList();
        //    return View(dshbrd);
        //}
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
                UserMaster usrm = conn.UserMasters.Find(uid1);
                usrm.Active = false;
                conn.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            DateTime dt = Convert.ToDateTime(Session["processdate"].ToString());

            try
            {
                int uid = (int)Session["uid"];
                SqlConnection con = new SqlConnection();

                con = conn.Database.Connection as SqlConnection;

                SqlDataAdapter adp = new SqlDataAdapter("GetOutwardDomainDashBoardData_V2", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@ProcessDate", SqlDbType.Date).Value = dt;
                adp.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = Convert.ToInt16(Session["CustomerID"]);

                DataSet ds = new DataSet();
                adp.Fill(ds);

                int custid = Convert.ToInt16(Session["CustomerID"]);
                int domainud = Convert.ToInt32(Session["DomainselectID"]);
                List<int?> domainlist = new List<int?>();
                domainlist = conn.DomainUserMapMasters.Where(d => d.UserId == uid).Select(d => d.DomainId).ToList();
                string ClrType = Session["CtsSessionType"].ToString();

                var dshbrd = conn.OutwardDomainDashBoard_V2.Where(m => m.ProcessDate == dt && m.CustomerId == custid && m.ClearingType == ClrType && domainlist.Contains(m.DomainId)).ToList();
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
