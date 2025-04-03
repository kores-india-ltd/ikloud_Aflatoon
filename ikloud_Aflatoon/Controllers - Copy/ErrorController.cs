using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ikloud_Aflatoon.Models;
using System.IO;

namespace iKloud_Aflatoon.Controllers
{
    public class ErrorController : Controller
    {

        //
        // GET: /Error/

        public ActionResult Error(string msg = null, string popmsg = null, int id = 0)
        {
            if (id == 1)
            {
                ViewBag.popmsg = popmsg;
                ViewBag.extra = true;
            }
            string ServerPath = "";
            string filename = "";
            string fileNameWithPath = "";
            FormsAuthentication.SignOut();
            Session.Abandon();
            ErrorDisplay err = new ErrorDisplay();

            if (msg != null && msg != "Session Expired")
                err.ErrorMessage = "An error occurred while procesing your request. Service Unavailable  !!!...";
            else
                err.ErrorMessage = msg;
            if (msg != "Session Expired")
            {
                ServerPath = Server.MapPath("~/Logs/");
                if (System.IO.Directory.Exists(ServerPath) == false)
                {
                    System.IO.Directory.CreateDirectory(ServerPath);
                }
                filename = DateTime.Now.ToString("ddMMyyyy") + "Logs.txt";
                fileNameWithPath = ServerPath + filename;
                StreamWriter str = new StreamWriter(fileNameWithPath, true, System.Text.Encoding.Default);
                //  str.WriteLine("hello");
                str.WriteLine(DateTime.Now.ToShortTimeString() + ": Exception: " + msg);
                str.WriteLine(DateTime.Now.ToShortTimeString() + ": StackTrace: " + popmsg);
                str.Close();
            }

            //FormsAuthentication.SignOut();
            //Session.Abandon();

            return View("NotFound",err);
        }
        public ActionResult NotFound(string msg = null, int id = 0, string popmsg = null)
        {
            if (id == 1)
            {
                ViewBag.popmsg = popmsg;
                ViewBag.extra = true;
            }
            FormsAuthentication.SignOut();
            Session.Abandon();
            ErrorDisplay err = new ErrorDisplay();
            if (msg == null)
                err.ErrorMessage = "Page Not Found!!!...";
            else
                err.ErrorMessage = msg;

            return View(err);
        }
        public ActionResult GlobalError(string msg = null, int id = 0, string popmsg = null)
        {
            if (id == 1)
            {
                ViewBag.popmsg = popmsg;
                ViewBag.extra = true;
            }
            FormsAuthentication.SignOut();
            Session.Abandon();
            ErrorDisplay err = new ErrorDisplay();
            if (msg == null)
                err.ErrorMessage = "Service unavailable!!!...";
            else
                err.ErrorMessage = msg;

            return View(err);
        }

    }
}
