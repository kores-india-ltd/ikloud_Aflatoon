using ikloud_Aflatoon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ikloud_Aflatoon.Controllers
{
    public class organizationController : Controller
    {
        AflatoonEntities db = new AflatoonEntities();

        //
        // GET: /organization/

        public ActionResult Index()
        {
            return View(db.OrganizationMasters.ToList());
        }

        //
        // GET: /organization/Details/5

        public ActionResult Details(int id = 0)
        {
            OrganizationMaster organizationmaster = db.OrganizationMasters.Find(id);
            if (organizationmaster == null)
            {
                return HttpNotFound();
            }
            return View(organizationmaster);
        }

        //
        // GET: /organization/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /organization/Create

        [HttpPost]
        public ActionResult Create(OrganizationMaster organizationmaster)
        {
            if (ModelState.IsValid)
            {
                db.OrganizationMasters.Add(organizationmaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(organizationmaster);
        }

        //
        // GET: /organization/Edit/5

        public ActionResult Edit(int id = 0)
        {
            OrganizationMaster organizationmaster = db.OrganizationMasters.Find(id);
            if (organizationmaster == null)
            {
                return HttpNotFound();
            }
            return View(organizationmaster);
        }

        //
        // POST: /organization/Edit/5

        [HttpPost]
        public ActionResult Edit(OrganizationMaster organizationmaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(organizationmaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(organizationmaster);
        }

        //
        // GET: /organization/Delete/5

        public ActionResult Delete(int id = 0)
        {
            OrganizationMaster organizationmaster = db.OrganizationMasters.Find(id);
            if (organizationmaster == null)
            {
                return HttpNotFound();
            }
            return View(organizationmaster);
        }

        //
        // POST: /organization/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            OrganizationMaster organizationmaster = db.OrganizationMasters.Find(id);
            db.OrganizationMasters.Remove(organizationmaster);
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