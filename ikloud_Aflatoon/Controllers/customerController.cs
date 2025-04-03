using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ikloud_Aflatoon.Controllers
{
    public class customerController : Controller
    {
        AflatoonEntities db = new AflatoonEntities();

        //
        // GET: /customer/

        public ActionResult Index()
        {
            ViewData["domainId"] = new SelectList(db.DomainMaster, "Id", "Name");

            var result = (from c in db.CustomerMasters
                          from g in db.GridMasters
                          from o in db.OrganizationMasters
                          where c.GridId == g.Id && o.Id == c.OrganizationId
                          select new customerView()
                          {
                              Id = c.Id,
                              OrganizationName = o.Name,
                              GridName = g.Name,
                              CustomerName = c.Name,
                              CustomerCode = c.Code,
                              PresentingBankRouteNo = c.PresentingBankRouteNo
                          }).ToList();

            return View(result);
        }

        //
        // GET: /customer/Details/5

        public ActionResult Details(int id = 0)
        {
            CustomerMaster customermaster = db.CustomerMasters.Find(id);
            if (customermaster == null)
            {
                return HttpNotFound();
            }

            customerView cv = new customerView();
            cv.Id = customermaster.Id;
            cv.OrganizationName = customermaster.Name;
            cv.GridName = db.GridMasters.Where(m => m.Id == customermaster.GridId).Select(m => m.Name).SingleOrDefault();
            cv.OrganizationName = db.OrganizationMasters.Where(m => m.Id == customermaster.OrganizationId).Select(m => m.Name).SingleOrDefault();
            cv.CustomerCode = customermaster.Code;
            cv.PresentingBankRouteNo = customermaster.PresentingBankRouteNo;


            return View(cv);
        }

        //
        // GET: /customer/Create

        public ActionResult Create()
        {

            customerView cv = new customerView();

            cv.Grid = new SelectList(db.GridMasters, "Id", "Name");
            cv.Organization = new SelectList(db.OrganizationMasters, "Id", "Name");

            return View(cv);
        }

        //
        // POST: /customer/Create

        [HttpPost]
        public ActionResult Create(customerView cv)
        {

            try
            {
                if (ModelState.IsValid)
                {

                    CustomerMaster cm = new CustomerMaster();

                    cm.Code = cv.CustomerCode;
                    cm.Name = cv.CustomerName;
                    cm.PresentingBankRouteNo = cv.PresentingBankRouteNo;
                    cm.GridId = cv.GridId;
                    cm.OrganizationId = cv.OrganizationId;
                    cm.CreationDateTime = DateTime.Now;
                    cm.CreatedBy = null;

                    db.CustomerMasters.Add(cm);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }


                cv.Grid = new SelectList(db.GridMasters, "Id", "Name");
                cv.Organization = new SelectList(db.OrganizationMasters, "Id", "Name");

                return View(cv);
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
        // GET: /customer/Edit/5

        public ActionResult Edit(int id = 0)
        {

            CustomerMaster customermaster = db.CustomerMasters.Find(id);
            if (customermaster == null)
            {
                return HttpNotFound();
            }

            return View(customermaster);
        }

        //
        // POST: /customer/Edit/5

        [HttpPost]
        public ActionResult Edit(CustomerMaster customermaster)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(customermaster).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(customermaster);
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
        // GET: /customer/Delete/5

        public ActionResult Delete(int id = 0)
        {
            CustomerMaster customermaster = db.CustomerMasters.Find(id);
            if (customermaster == null)
            {
                return HttpNotFound();
            }
            return View(customermaster);
        }

        //
        // POST: /customer/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerMaster customermaster = db.CustomerMasters.Find(id);
            db.CustomerMasters.Remove(customermaster);
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