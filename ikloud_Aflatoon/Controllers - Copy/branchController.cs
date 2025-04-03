using ikloud_Aflatoon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ikloud_Aflatoon.Models;


namespace ikloud_Aflatoon.Controllers
{
    public class branchController : Controller
    {

        AflatoonEntities db = new AflatoonEntities();

        //
        // GET: /branch/

        public ActionResult Index()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            
            if ((bool)Session["master"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            var result = (from b in db.BranchMaster
                          from d in db.DomainMaster
                          where (b.OwDomainId == d.Id)
                          select new branchView()
                          {
                              Id = b.Id,
                              BranchCode = b.BranchCode,
                              BranchName = b.BranchName,
                              Address1 = b.Address1,
                              Address2 = b.Address2,
                              IFSCode = b.IFSCode,
                              MICRCode = b.MICRCode,
                              OutwardDomain = d.Name,
                              InwardDomain = d.Name
                          }).ToList();

            return View(result);
        }

        //
        // GET: /branch/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            
            if ((bool)Session["master"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            BranchMaster branchmaster = db.BranchMaster.Find(id);
            if (branchmaster == null)
            {
                return HttpNotFound();
            }

            branchView bv = new branchView();
            bv.Id = branchmaster.Id;
            bv.BranchCode = branchmaster.BranchCode;
            bv.BranchName = branchmaster.BranchName;
            bv.Address1 = branchmaster.Address1;
            bv.Address2 = branchmaster.Address2;
            bv.IFSCode = branchmaster.IFSCode;
            bv.MICRCode = branchmaster.MICRCode;
            bv.OutwardDomain = db.BranchMaster.Where(m => m.Id == branchmaster.OwDomainId).Select(m => m.BranchName).SingleOrDefault();
            bv.InwardDomain = db.BranchMaster.Where(m => m.Id == branchmaster.IwDomainId).Select(m => m.BranchName).SingleOrDefault();

            return View(bv);
        }

        //
        // GET: /branch/Create

        public ActionResult Create()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            
            if ((bool)Session["master"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            int custid = Convert.ToInt16(Session["CustomerID"]);
            branchView bv = new branchView();

            bv.OwDomainLst = new SelectList(db.DomainMaster.Where(m => m.CustomerId == custid), "Id", "Name");
            bv.IwDomainLst = new SelectList(db.DomainMaster.Where(m => m.CustomerId == custid), "Id", "Name");

            return View(bv);
        }

        //
        // POST: /branch/Create

        [HttpPost]
        public ActionResult Create(branchView bv)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            
            if ((bool)Session["master"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            try
            {

                if (ModelState.IsValid)
                {

                    BranchMaster bm = new BranchMaster();

                    bm.Id = bv.Id;
                    bm.BranchCode = bv.BranchCode;
                    bm.BranchName = bv.BranchName;
                    bm.Address1 = bv.Address1;
                    bm.Address2 = bv.Address2;
                    bm.IFSCode = bv.IFSCode;
                    bm.MICRCode = bv.MICRCode;
                    bm.OwDomainId = bv.OwDomainId;
                    bm.IwDomainId = bv.IwDomainId;
                    bm.CreationDateTime = DateTime.Now;
                    bm.CreatedBy = null;

                    db.BranchMaster.Add(bm);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                int custid = Convert.ToInt16(Session["CustomerID"]);
                bv.OwDomainLst = new SelectList(db.DomainMaster.Where(m => m.CustomerId == custid), "Id", "Name");
                bv.IwDomainLst = new SelectList(db.DomainMaster.Where(m => m.CustomerId == custid), "Id", "Name");

                return View(bv);
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

        //
        // GET: /branch/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            
            if ((bool)Session["master"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            BranchMaster branchmaster = db.BranchMaster.Find(id);
            if (branchmaster == null)
            {
                return HttpNotFound();
            }

            branchView bv = new branchView();


            bv.Id = branchmaster.Id;
            bv.BranchCode = branchmaster.BranchCode;
            bv.BranchName = branchmaster.BranchName;
            bv.Address1 = branchmaster.Address1;
            bv.Address2 = branchmaster.Address2;
            bv.IFSCode = branchmaster.IFSCode;
            bv.MICRCode = branchmaster.MICRCode;
            //bv.OutwardDomain = db.BranchMasters.Where(m => m.Id == branchmaster.OwDomainId).Select(m => m.BranchName).SingleOrDefault();
            //bv.InwardDomain = db.BranchMasters.Where(m => m.Id == branchmaster.IwDomainId).Select(m => m.BranchName).SingleOrDefault();

            int custid = Convert.ToInt16(Session["CustomerID"]);

            bv.OwDomainLst = new SelectList(db.DomainMaster.Where(m => m.CustomerId == custid), "Id", "Name");
            bv.IwDomainLst = new SelectList(db.DomainMaster.Where(m => m.CustomerId == custid), "Id", "Name");

            return View(bv);
        }

        //
        // POST: /branch/Edit/5

        [HttpPost]
        public ActionResult Edit(branchView bv)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            
            if ((bool)Session["master"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            try
            {

                int custid = Convert.ToInt16(Session["CustomerID"]);

                if (ModelState.IsValid)
                {


                    BranchMaster bm = new BranchMaster();

                    bm.Id = bv.Id;
                    bm.BranchCode = bv.BranchCode;
                    bm.BranchName = bv.BranchName;
                    bm.Address1 = bv.Address1;
                    bm.Address2 = bv.Address2;
                    bm.IFSCode = bv.IFSCode;
                    bm.MICRCode = bv.MICRCode;
                    bm.OwDomainId = bv.OwDomainId;
                    bm.IwDomainId = bv.IwDomainId;
                    bm.CreationDateTime = DateTime.Now;
                    bm.CreatedBy = null;

                    //db.BranchMaster.Add(bm);
                    db.Entry(bm).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                bv.OwDomainLst = new SelectList(db.DomainMaster.Where(m => m.CustomerId == custid), "Id", "Name");
                bv.IwDomainLst = new SelectList(db.DomainMaster.Where(m => m.CustomerId == custid), "Id", "Name");

                return View(bv);

                //}
                //catch (DbEntityValidationException e)
                //{

                //    foreach (var eve in e.EntityValidationErrors)
                //    {
                //        //Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                //        //    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //        foreach (var ve in eve.ValidationErrors)
                //        {
                //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                //                ve.PropertyName, ve.ErrorMessage);
                //        }
                //    }
                //    throw;
                //}
            }
            catch (Exception e)
            {
                //Server.MapPath(strMappath);
                // ErrorDisplay er = new ErrorDisplay();
                //er.ErrorMessage = e.Message.ToString();
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
                //return View("Error", er);
            }

            return View(bv);
        }

        //
        // GET: /branch/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            
            if ((bool)Session["master"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            BranchMaster branchmaster = db.BranchMaster.Find(id);
            if (branchmaster == null)
            {
                return HttpNotFound();
            }
            return View(branchmaster);
        }

        //
        // POST: /branch/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            
            if ((bool)Session["master"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            BranchMaster branchmaster = db.BranchMaster.Find(id);
            db.BranchMaster.Remove(branchmaster);
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