using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ikloud_Aflatoon.Controllers
{
    public class gridController : Controller
    {
        AflatoonEntities db = new AflatoonEntities();

        //
        // GET: /grid/

        public ActionResult Index()
        {
            return View(db.GridMasters.ToList());
        }

        //
        // GET: /grid/Details/5

        public ActionResult Details(int id = 0)
        {
            GridMaster gridmaster = db.GridMasters.Find(id);
            if (gridmaster == null)
            {
                return HttpNotFound();
            }
            return View(gridmaster);
        }

        //
        // GET: /grid/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /grid/Create

        [HttpPost]
        public ActionResult Create(GridMaster gridmaster)
        {
            if (ModelState.IsValid)
            {
                db.GridMasters.Add(gridmaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gridmaster);
        }

        //
        // GET: /grid/Edit/5

        public ActionResult Edit(int id = 0)
        {
            GridMaster gridmaster = db.GridMasters.Find(id);
            if (gridmaster == null)
            {
                return HttpNotFound();
            }
            return View(gridmaster);
        }

        //
        // POST: /grid/Edit/5

        [HttpPost]
        public ActionResult Edit(GridMaster gridmaster)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(gridmaster).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(gridmaster);
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
        // GET: /grid/Delete/5

        public ActionResult Delete(int id = 0)
        {
            GridMaster gridmaster = db.GridMasters.Find(id);
            if (gridmaster == null)
            {
                return HttpNotFound();
            }
            return View(gridmaster);
        }

        //
        // POST: /grid/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            GridMaster gridmaster = db.GridMasters.Find(id);
            db.GridMasters.Remove(gridmaster);
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