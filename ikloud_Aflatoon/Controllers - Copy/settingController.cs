using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ikloud_Aflatoon.Controllers
{
    public class settingController : Controller
    {
        AflatoonEntities db = new AflatoonEntities();

        //
        // GET: /setting/

        public ActionResult Index()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }

            if ((bool)Session["settg"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            var result = (from a in db.ApplicationSettings
                          from c in db.CustomerMasters
                          where a.CustomerId==c.Id
                          select new applicationSettingsView() 
                          {
                          Id=a.Id,  
                          SettingName=a.SettingName,
                          SettingValue=a.SettingValue,
                          SettingLevel=a.SettingLevel,
                          CustomerName=c.Name
                          }).ToList();


            return View(result);
        }

        //
        // GET: /setting/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }

            if ((bool)Session["settg"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            ApplicationSetting applicationsetting = db.ApplicationSettings.Find(id);
            if (applicationsetting == null)
            {
                return HttpNotFound();
            }

            applicationSettingsView av = new applicationSettingsView();

            av.Id = applicationsetting.Id;
            av.CustomerName = db.CustomerMasters.Where(m => m.Id == applicationsetting.CustomerId).Select(m => m.Name).SingleOrDefault();
            av.SettingName = applicationsetting.SettingName;
            av.SettingLevel = applicationsetting.SettingLevel;
            av.SettingValue = applicationsetting.SettingValue;
            av.CustomerId = applicationsetting.CustomerId;

            return View(av);
        }

        //
        // GET: /setting/Create

        public ActionResult Create()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            
            if ((bool)Session["settg"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            applicationSettingsView app = new applicationSettingsView();
            //.Distinct().ToList().Select(m => m.SettingName)

           
            app.SettingNameLst = new SelectList(db.SettingValueMasters, "Id", "SettingName").Distinct(); 
            app.CustomerLst = new SelectList(db.CustomerMasters, "Id", "Name").Distinct();

            List<SelectListItem> settinglevels=new List<SelectListItem>()
            {
                new SelectListItem() {Text="Customer",Value="1",Selected=false},
                new SelectListItem() {Text="Domain" ,Value="2",Selected=false}
            };

            app.SettingLevelLst = settinglevels;

            return View(app);
        }

        //
        // POST: /setting/Create

        [HttpPost]
        public ActionResult Create(applicationSettingsView applicationsetting)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }

            if ((bool)Session["settg"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            if (ModelState.IsValid)
            {
                //db.ApplicationSettings.Add(applicationsetting);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            //applicationSettingsView app = new applicationSettingsView();
            //app.SettingNameLst = new SelectList(db.ApplicationSettings, "Id", "SettingName").Distinct();
            //app.CustomerLst = new SelectList(db.CustomerMasters, "Id", "Name").Distinct();

            applicationsetting.CustomerLst = new SelectList(db.CustomerMasters, "Id", "Name").Distinct();
            applicationsetting.SettingNameLst = new SelectList(db.CustomerMasters, "Id", "Name").Distinct();

            List<SelectListItem> settinglevels = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Customer",Value="1",Selected=false},
                new SelectListItem() {Text="Domain" ,Value="2",Selected=false}
            };

            applicationsetting.SettingLevelLst = settinglevels;

            return View(applicationsetting);
        }

        //
        // GET: /setting/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }

            if ((bool)Session["settg"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            ApplicationSetting applicationsetting = db.ApplicationSettings.Find(id);
            if (applicationsetting == null)
            {
                return HttpNotFound();
            }

            applicationSettingsView av = new applicationSettingsView();

            av.Id = applicationsetting.Id;
            av.CustomerId = applicationsetting.CustomerId;
            av.CustomerName = db.CustomerMasters.Where(m => m.Id == applicationsetting.CustomerId).Select(m => m.Name).SingleOrDefault();
            av.SettingName = applicationsetting.SettingName;
            av.SettingLevel = applicationsetting.SettingLevel;
            av.SettingValue = applicationsetting.SettingValue;


            av.CustomerLst = new SelectList(db.CustomerMasters, "Id", "Name").Distinct();
            av.SettingNameLst = new SelectList(db.ApplicationSettings, "Id", "SettingName").Distinct();

            List<SelectListItem> settinglevels = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Customer",Value="1",Selected=false},
                new SelectListItem() {Text="Domain" ,Value="2",Selected=false}
            };

            av.SettingLevelLst = settinglevels;


            return View(av);
        }

        //
        // POST: /setting/Edit/5

        [HttpPost]
        public ActionResult Edit(ApplicationSetting applicationsetting)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }

            if ((bool)Session["settg"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            if (ModelState.IsValid)
            {
                db.Entry(applicationsetting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationsetting);
        }

        //
        // GET: /setting/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }

            if ((bool)Session["settg"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            ApplicationSetting applicationsetting = db.ApplicationSettings.Find(id);
            if (applicationsetting == null)
            {
                return HttpNotFound();
            }
            return View(applicationsetting);
        }

        //
        // POST: /setting/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }

            if ((bool)Session["settg"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            ApplicationSetting applicationsetting = db.ApplicationSettings.Find(id);
            db.ApplicationSettings.Remove(applicationsetting);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}