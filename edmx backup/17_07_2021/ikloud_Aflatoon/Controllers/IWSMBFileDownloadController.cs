using ikloud_Aflatoon.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ikloud_Aflatoon.Controllers       // Developed by Sachin Date: 2019-03-29
{
    public class IWSMBFileDownloadController : Controller
    {
        //UserAflatoonDbContext af = new UserAflatoonDbContext();
        AflatoonEntities af = new AflatoonEntities();
        IWProcDataContext IWpro = new IWProcDataContext();
        
        // GET: /IWCBSFileDownload/
        public ActionResult IWSMBFileDwnld()
        {
            if (Session["uid"] == null)
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });

            if ((bool)Session["fildwnd"] == false)      //----------------------------------Checking user rights--------------------//
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            string date = Convert.ToDateTime(Session["processdate"]).ToString("yyyy-MM-dd");
            int custID = Convert.ToInt16(Session["CustomerID"].ToString());
            var getData = IWpro.SMB_GetSMBFileDownloadLinkDetails(date, custID).ToList(); //---- getting record from DB

            return View(getData);
        }

        //[HttpPost]
        public ActionResult ZipDownload(int? id=0)
        {
            try
            {
                string DownloadURL = "", ReportPath = "", PhysicalFilePath = "";
                string date = Convert.ToDateTime(Session["processdate"]).ToString("yyyy-MM-dd");
                int custID = Convert.ToInt16(Session["CustomerID"].ToString());
                IWpro.SMB_GetSMBFileDownloadLink(date, custID, id, ref DownloadURL, ref PhysicalFilePath);   //----getting Zipfile name for download from DB

                if (DownloadURL != null || DownloadURL != "")
                {
                    if (DownloadURL.Substring((DownloadURL.Length - 4), 4) == ".rar")//ikloudProSMB\IkloudProSMB
                        ReportPath = Server.MapPath("~/IkloudProSMB/FileDownload/" + Convert.ToDateTime(Session["processdate"]).ToString("dd.MM.yyyy") + "/" + DownloadURL);
                    else
                        ReportPath = PhysicalFilePath + "/" + DownloadURL;

                    return File(ReportPath, "application/zip", DownloadURL);
                }
                else
                    return File(ReportPath, "application/zip");
            }
            catch(Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "ZipDownload- " + innerExcp });  
            }
        }
    }
}
