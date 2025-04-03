using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ikloud_Aflatoon.Controllers
{
    public class OutwardSingleDomainDashBoardController : Controller
    {

        AflatoonEntities db = new AflatoonEntities();
        //
        // GET: /OutwardSingleDomainDashBoard/

        public ActionResult Index()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }

            Session["CustomerID"] = 6;//get custId from application

            int custId = Convert.ToInt16(Session["CustomerID"]);
            int domainId = 1;//get domainId from application

            var dshbrd = db.OutwardSingleDomainDashBoard
                .Where(m => m.DomainId == domainId);


            ViewBag.domainlist = new SelectList(db.DomainMaster.Where(m => m.CustomerId == custId), "Id", "Name").ToList();

            return View(dshbrd);
        }

        [HttpPost]
        public ActionResult Index(string refresh, string domainlist)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            Session["processdate"] = DateTime.ParseExact("2017-01-19", "yyyy-MM-dd", null);//get processdate from application
            DateTime dt = Convert.ToDateTime(Session["processdate"].ToString());

            Session["CustomerID"] = 6;//get custId from application
            int custId = Convert.ToInt16(Session["CustomerID"]);
            int domainId = 1;//get domainId form application

            if (domainlist == "")
            {
                ViewBag.domainlist = new SelectList(db.DomainMaster.Where(m => m.CustomerId == custId), "Id", "Name").ToList();
                return View();
            }


            try
            {
                SqlConnection con = new SqlConnection();

                con = db.Database.Connection as SqlConnection;

                SqlDataAdapter adp = new SqlDataAdapter("GetOutwardSingleDomainDashBoardData", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@ProcessDate", SqlDbType.Date).Value = dt;
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.Int).Value = domainlist;

                DataSet ds = new DataSet();
                adp.Fill(ds);

                ViewBag.domainlist = new SelectList(db.DomainMaster.Where(m => m.CustomerId == custId), "Id", "Name").ToList();

                int domainid = Convert.ToInt32(domainlist);

                var dshbrd = db.OutwardSingleDomainDashBoard
                    .Where(m => m.DomainId == domainid);

                return View(dshbrd);
            }
            catch (Exception e)
            {
                //Server.MapPath(strMappath);
                // ErrorDisplay er = new ErrorDisplay();
                //er.ErrorMessage = e.Message.ToString();
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
                //return View("Error", er);
            }
        }

    }
}
