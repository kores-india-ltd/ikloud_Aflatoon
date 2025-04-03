using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ikloud_Aflatoon.Models;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using ikloud_Aflatoon.Infrastructure;

namespace ikloud_Aflatoon.Controllers
{
    //[OutputCache(Duration = 0)]
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        private AflatoonEntities af = new AflatoonEntities();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
        public ActionResult IWIndex(int id = 0)
        {

            //int num2 = 0;

            //int num1 = 10 / num2;

            bool flg = true;
            if (TempData["flg"] != null)
            {
                flg = Convert.ToBoolean(TempData["flg"]);
                TempData.Remove("flg");
            }

            if (flg == false)
                ViewBag.userlogin = false;

            else
                ViewBag.userlogin = true;


            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if (id == 1)
                ViewBag.msg = "No Records found for Data Entry.!!..";

            if (id == 2)
                ViewBag.msg = "Data Entry screen Closed!!..";

            if (id == 3)
                ViewBag.msg = "Verification screen Closed!!..";
            if (id == 4)
                ViewBag.msg = "No Records found for Verification.!!..";
            if (id == 5)
                ViewBag.msg = "No Record was found!!..";

            @Session["glob"] = true;
            //   id = 0;

            return View();
        }
        public void setSession()
        {
            int uid = Convert.ToInt16(Session["uid"]);
            var usmtr = af.UserMasters.Where(u => u.ID == uid).SingleOrDefault();
            if (usmtr != null)
            {
                //usmtr.sessionid = Session.SessionID;
                // usmtr.LastLogin = Convert.ToDateTime(DateTime.Now.ToString(format));
                //usmtr.loginFlg = 0;
                usmtr.sessionid = Session.SessionID;
                usmtr.loginFlg = 1;
                usmtr.InvalidPasswordAttempts = 0;
                af.Entry(usmtr).State = EntityState.Modified;
                af.SaveChanges();
                Session["afterlogin"] = true;
            }
        }
        public bool ValidateSession()
        {

            if (Session["uid"] == null)
                return false;

            int uid = (int)Session["uid"];
            var model = af.UserMasters.Find(uid);
            if (model.sessionid != Session.SessionID)
            {

                return false;
            }
            else
                return true;

        }

        
    }
}
