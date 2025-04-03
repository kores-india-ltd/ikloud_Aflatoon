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
    public class domainController : Controller
    {
        AflatoonEntities db = new AflatoonEntities();

        string userid = "";
        //
        // GET: /domain/

        public ActionResult Index()
        {
            try
            {
                int custid = Convert.ToInt16(Session["CustomerID"]);

                var result = (from d in db.DomainMaster
                              from c in db.CustomerMasters
                              where d.CustomerId == c.Id && d.CustomerId == custid
                              select new domainView()
                              {
                                  ID = d.Id,
                                  custID = c.Id,
                                  DomainName = d.Name,
                                  DomainCode = d.Code,
                                  CityCode = d.CityCode,
                                  BranchCode=d.BranchCode
                              }).ToList();


                return View(result);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "Domain HttpGet Index-" + innerExcp });
            }
            //Session["CustomerID"] = 6;
           
        }

        //
        // GET: /domain/Details/5

        public ActionResult Details(int id = 0)
        {
            DomainMaster domainmaster = db.DomainMaster.Find(id);
            if (domainmaster == null)
            {
                return HttpNotFound();
            }
            return View(domainmaster);
        }

        //
        // GET: /domain/Create

        public ActionResult Create()
        {

            //Session["CustomerID"] = 6;
            

            try
            {
                int custid = Convert.ToInt16(Session["CustomerID"]);

                var rslt = (from d in db.ApplicationSettings
                            where d.CustomerId == custid && d.SettingLevel == "Domain"
                            select new domainLevelSettingView()
                            {
                                appSettingID = d.Id,
                                SettingName = d.SettingName,
                                SettingValue = d.SettingValue

                            }).ToList();

                domainView dv = new domainView();

                dv.lstDomainLevelSettings = rslt;
                dv.custID = custid;

                return View(dv);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "Domain HttpGet Create-" + innerExcp });
            }
        
        }

        //
        // POST: /domain/Create

        [HttpPost]
        public ActionResult Create(domainView dv)
        {

            try
            {
                int domainId = 0;
                //Session["CustomerID"] = 6;
                int custid = Convert.ToInt16(Session["CustomerID"]);
                Session["userid"] = 1;
                int userid = Convert.ToInt32(Session["userid"]);

                if (ModelState.IsValid)
                {

                    DomainMaster dm1 = new DomainMaster();
                    if (true)
                    {
                        dm1 = db.DomainMaster.Where(m => m.Name == dv.DomainName).FirstOrDefault();

                        if (dm1 != null)
                        {
                            
                                ModelState.AddModelError("", "Domain already exists!!");
                                var rslt = (from d in db.ApplicationSettings
                                            where d.CustomerId == custid && d.SettingLevel == "Domain"
                                            select new domainLevelSettingView()
                                            {
                                                appSettingID = d.Id,
                                                SettingName = d.SettingName,
                                                SettingValue = d.SettingValue

                                            }).ToList();

                                // domainView dv = new domainView();

                                dv.lstDomainLevelSettings = rslt;
                                dv.custID = custid;
                                return View(dv);
                            
                        }

                      }

                    DomainMaster dm = new DomainMaster();

                    dm.CustomerId = dv.custID;
                    dm.CityCode = dv.CityCode;
                    dm.Name = dv.DomainName;
                    dm.Code = dv.DomainCode;
                    dm.BranchCode = dv.BranchCode;

                    dm.CreatedBy = userid;
                    dm.CreationDateTime = DateTime.Now;

                    dm = db.DomainMaster.Add(dm);
                    db.SaveChanges();

                    domainId = dm.Id;
                    //return RedirectToAction("Index");

                    int cnt1 = Convert.ToInt32(Request["domCount"]);


                    for (int i = 0; i < cnt1; i++)
                    {
                        DomainLevelSetting dls = new DomainLevelSetting();

                        string strIDkey = "appsettingID " + i.ToString();
                        string strSttngValkey = "dom " + i.ToString();

                        dls.ApplicationSettingId = Convert.ToInt32(Request[strIDkey]);
                        dls.DomainId = domainId;
                        dls.SettingValue = Request[strSttngValkey].ToString();
                        dls.CreatedBy = userid;
                        dls.CreatedOn = DateTime.Now;
                        db.DomainLevelSettings.Add(dls);
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }
                else
                {


                    var rslt = (from d in db.ApplicationSettings
                                where d.CustomerId == custid && d.SettingLevel == "Domain"
                                select new domainLevelSettingView()
                                {
                                    appSettingID = d.Id,
                                    SettingName = d.SettingName,
                                    SettingValue = d.SettingValue

                                }).ToList();

                    dv.lstDomainLevelSettings = rslt;

                    return View(dv);
                }
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "Domain HttpPost Create-" + innerExcp });
            }


        }

        //
        // GET: /domain/Edit/5

        public ActionResult Edit(int id = 0)
        {

            try
            {
                DomainMaster dm = db.DomainMaster.Find(id);

                if (dm == null)
                {
                    return HttpNotFound();
                }

                var rslt = (from d in db.DomainLevelSettings
                            from a in db.ApplicationSettings
                            where d.ApplicationSettingId == a.Id && d.DomainId == id
                            select new domainLevelSettingView
                            {
                                id = d.Id,
                                SettingName = a.SettingName,
                                SettingValue = d.SettingValue
                            }).ToList();


                domainView dv = new domainView();
                dv.custID = dm.CustomerId;
                dv.CityCode = dm.CityCode;
                dv.DomainName = dm.Name;
                dv.DomainCode = dm.Code;
                dv.BranchCode = dm.BranchCode;

                dv.lstDomainLevelSettings = rslt;

                return View(dv);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "Domain HttpGet Edit-" + innerExcp });
            }

            
        }

        //
        // POST: /domain/Edit/5

        [HttpPost]
        public ActionResult Edit(domainView dv)
        {

            try
            {
                int domainId = 0;

                if (ModelState.IsValid)
                {
                    DomainMaster dm = new DomainMaster();

                    domainId = dv.ID;

                    dm.Id = dv.ID;
                    dm.CustomerId = dv.custID;
                    dm.CityCode = dv.CityCode;
                    dm.Name = dv.DomainName;
                    dm.Code = dv.DomainCode;
                    dm.BranchCode = dv.BranchCode;

                    db.Entry(dm).State = EntityState.Modified;
                    db.SaveChanges();

                    //return RedirectToAction("Index");

                    int cnt1 = Convert.ToInt32(Request["domCount"]);


                    for (int i = 0; i < cnt1; i++)
                    {
                        DomainLevelSetting dls = new DomainLevelSetting();

                        string dlsRowId = "rowid" + i;
                        string strSttngValkey = "dom" + i.ToString();

                        int rowid = Convert.ToInt32(Request[dlsRowId].ToString());

                        dls = db.DomainLevelSettings.Find(rowid);

                        if (dls != null)
                        {
                            dls.SettingValue = Request[strSttngValkey].ToString();

                            db.Entry(dls).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }

                    return RedirectToAction("Index");

                }

                return View(dv);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "Domain HttpPost Edit-" + innerExcp });
            }

            
        }

        //
        // GET: /domain/Delete/5

        public ActionResult Delete(int id = 0)
        {
            DomainMaster domainmaster = db.DomainMaster.Find(id);
            if (domainmaster == null)
            {
                return HttpNotFound();
            }
            return View(domainmaster);
        }

        //
        // POST: /domain/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            DomainMaster domainmaster = db.DomainMaster.Find(id);
            db.DomainMaster.Remove(domainmaster);
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