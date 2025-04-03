using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ikloud_Aflatoon.Models;
using ikloud_Aflatoon.Infrastructure;
using System.DirectoryServices.AccountManagement;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace ikloud_Aflatoon.Controllers
{
    //[Authorize]
    //[OutputCache(Duration = 0)]
    public class SODController : Controller
    {
        private SodusrDbCon db = new SodusrDbCon();
        private AflatoonEntities udb = new AflatoonEntities();
        //OWProcDataContext OWpro = new OWProcDataContext();
        string cs = ConfigurationManager.ConnectionStrings["iKloudProConnectionString2"].ConnectionString;
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);

        //
        // GET: /SOD/
        //  [ValidateAntiForgeryToken]
        public ActionResult SOD()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            //----------------------------------Checking user rights--------------------//

            if ((bool)Session["SOD"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid1);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            int uid = (int)Session["uid"];
            //----------------------------------OW/IW-----------------
            //ClearingTypes clr = new ClearingTypes();
            //ViewBag.ClearingType = new SelectList(clr.ClrType);
            ////--------------CTS/NonCTS-------------------
            ////List<String> ProcessTypes = new List<string>();

            //ViewBag.ProcessTypes = new SelectList(clr.ProType);
            //if (Session["ProType"].ToString()=="OW")
            //{
            //   
            //    return View("OwSods", owsod);
            //}
            //else
            //{
            //    IwSoD Iwsod = new IwSoD();
            //    return View("IWSods", Iwsod);
            //}
            try
            {
                var custoomer = from c in udb.CustomerMasters
                                select new { c.Id, c.Name };

                ViewBag.customer = new SelectList(custoomer.AsEnumerable(), "Id", "Name");
                OwSoD owsod = new OwSoD();

                return View("OwSods", owsod);
            }
            catch (Exception e)
            {
                //Server.MapPath(strMappath);
                // ErrorDisplay er = new ErrorDisplay();
                //er.ErrorMessage = e.Message.ToString();
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
                //return View("Error", er);
            }

            // return View(sod);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SOD(OwSoD sod)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            //----------------------------------Checking user rights--------------------//

            if ((bool)Session["SOD"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid1);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            try
            {

                int cust = Convert.ToInt16(Request.Form["CustomerId"].ToString());
                string[] slplitprocdate = Request.Form["ProcessingDate"].ToString().Split('/');
                string[] slplitPostDated = Request.Form["PostDated"].ToString().Split('/');
                string[] slplitStaleDated = Request.Form["StaleDated"].ToString().Split('/');
                DateTime ProcessingDate = Convert.ToDateTime(slplitprocdate[2] + "-" + slplitprocdate[1] + "-" + slplitprocdate[0]);
                DateTime PostDated = Convert.ToDateTime(slplitPostDated[2] + "-" + slplitPostDated[1] + "-" + slplitPostDated[0]);
                DateTime StaleDated = Convert.ToDateTime(slplitStaleDated[2] + "-" + slplitStaleDated[1] + "-" + slplitStaleDated[0]);

                if (Session["ProType"].ToString() == "Outward")
                {
                    if (db.OwSODs.Count(s => (s.ProcessingDate == ProcessingDate) && s.CustomerId == sod.CustomerId) == 0)
                    {
                        sod.ProcessingDate = ProcessingDate;
                        sod.SoDBy = (int)Session["uid"];// db.UserMasters.Find((int)Session["uid"]).ID;
                        sod.SoDOn = DateTime.Now;
                        sod.SoDStatus = 1;
                        sod.CustomerId = sod.CustomerId;
                        sod.PostDated = PostDated;
                        sod.StaleDated = StaleDated;
                        db.OwSODs.Add(sod);
                        db.SaveChanges();
                        //Session["glob"] = null;
                        var custoomer = from c in udb.CustomerMasters
                                        select new { c.Id, c.Name };

                        ViewBag.customer = new SelectList(custoomer.AsEnumerable(), "Id", "Name");
                        OwSoD owsod = new OwSoD();
                        ViewBag.done = "SOD Created successfully";
                        // ModelState.AddModelError("", "SOD Created successfully");
                        return View("OwSods", owsod);
                    }
                    else
                    {
                        int uid = (int)Session["uid"];
                        var custoomer = from c in udb.CustomerMasters
                                        select new { c.Id, c.Name };

                        ViewBag.customer = new SelectList(custoomer.AsEnumerable(), "Id", "Name");

                        ModelState.AddModelError("", "SOD is Already Created ");

                        return View("OwSods", sod);
                    }
                }
                if (Session["ProType"].ToString() == "Inward")
                {
                    DateTime finaldate = Convert.ToDateTime(sod.ProcessingDate).Date;


                    if (db.IwSODs.Count(s => (s.ProcessingDate == ProcessingDate) && s.CustomerId == sod.CustomerId) == 0)
                    {
                        IwSoD iwsod = new IwSoD();

                        iwsod.ProcessingDate = ProcessingDate;
                        iwsod.SoDBy = (int)Session["uid"];// db.UserMasters.Find((int)Session["uid"]).ID;
                        iwsod.SoDOn = DateTime.Now;
                        iwsod.SoDStatus = 1;
                        iwsod.CustomerId = sod.CustomerId;
                        iwsod.PostDated = PostDated;
                        iwsod.StaleDated = StaleDated;

                        db.IwSODs.Add(iwsod);
                        db.SaveChanges();
                        // Session["glob"] = null;

                        var custoomer = from c in udb.CustomerMasters
                                        select new { c.Id, c.Name };

                        ViewBag.customer = new SelectList(custoomer.AsEnumerable(), "Id", "Name");
                        OwSoD owsod = new OwSoD();
                        ViewBag.done = "SOD Created successfully";
                        return View("OwSods", owsod);
                    }
                    else
                    {
                        int uid = (int)Session["uid"];
                        //var model = from dm in db.UserDomainMapping
                        //            from d in db.Domains
                        //            where dm.User.ID == uid && dm.Domain.ID == d.ID
                        //            select d;
                        //ViewBag.DomainID_ID = new SelectList(model, "ID", "DomainName");

                        var custoomer = from c in udb.CustomerMasters
                                        select new { c.Id, c.Name };

                        ViewBag.customer = new SelectList(custoomer.AsEnumerable(), "Id", "Name");

                        ModelState.AddModelError("", "SOD is Already Created ");
                        return View("OwSods", sod);
                    }

                }
                return View();

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
        public JsonResult EOD(string procdate = null, string custid = null)
        {
            int intcustid = Convert.ToInt16(custid);
            string[] slplitprocdate;
            DateTime ProcessingDate;

            if (procdate != null && procdate != "")
            {
                slplitprocdate = procdate.Split('/');
                procdate = slplitprocdate[2] + "-" + slplitprocdate[1] + "-" + slplitprocdate[0];
                ProcessingDate = Convert.ToDateTime(procdate);
            }
            else
                ProcessingDate = DateTime.Now;


            //OWpro.SP_doEOD(procdate, Session["ProType"].ToString(), (int)Session["uid"], intcustid);

            SqlCommand com = new SqlCommand("SP_doEOD", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@ProcDate", procdate);
            com.Parameters.AddWithValue("@Clearing", Session["ProType"].ToString());
            com.Parameters.AddWithValue("@uid", (int)Session["uid"]);
            com.Parameters.AddWithValue("@Customerid", intcustid);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();

            //----------------
            ActivityLog act = new ActivityLog();
            act.LogLevel = "SOD/EOD";

            act.ActionTaken = "EOD";
            act.TimeStamp = DateTime.Now;
            act.LoginId = Session["LoginID"].ToString();
            act.ProcessingDate = ProcessingDate;
            act.CustomerId = intcustid;
            db.ActivityLogs.Add(act);
            db.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult login(string uname = null, string upass = null, string procdate = null, string custid = null, string loglevel = null)
        {
            CommonFunction cmf = new CommonFunction();
            
            bool flg = false;
            string[] slplitprocdate;
            DateTime ProcessingDate;

            var model = db.UserMasters.Where(m => m.LoginID == uname).SingleOrDefault();

            if (model != null)
            {
                if(model.UsertType.ToLower() == "bank_user" || model.UsertType.ToLower() == "scanning_user"){
                    string dname = null;
                    var DomiaName = udb.CommonSettings.Where(m => m.AppName == "CTSCONFIG1" && m.SettingName == "DomainName").SingleOrDefault();
                    if (DomiaName != null)
                        dname = DomiaName.SettingValue;
                    string adauth = null;
                    var CommAdAuth = udb.CommonSettings.Where(m => m.AppName == "CTSCONFIG1" && m.SettingName == "ADAuthentication").SingleOrDefault();
                    if (CommAdAuth != null)
                        adauth = CommAdAuth.SettingValue;

                    if (adauth == "Y" && (model.UsertType.ToLower() == "bank_user" || model.UsertType.ToLower() == "scanning_user"))
                    {
                        using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, dname))
                        {
                            bool isValid = pc.ValidateCredentials(uname, upass);
                            
                            if (isValid == false)
                            {
                                flg = false;
                            }
                            else
                            {
                                flg = true;

                                if (procdate != null && procdate != "")
                                {
                                    slplitprocdate = procdate.Split('/');
                                    ProcessingDate = Convert.ToDateTime(slplitprocdate[2] + "-" + slplitprocdate[1] + "-" + slplitprocdate[0]);
                                }
                                else
                                    ProcessingDate = DateTime.Now;
                                if (custid == null)
                                    custid = "0";

                                ActivityLog act = new ActivityLog();
                                if (loglevel != null)
                                    act.LogLevel = loglevel;
                                else
                                    act.LogLevel = "SOD";

                                act.ActionTaken = "Modified";
                                act.TimeStamp = DateTime.Now;
                                act.LoginId = uname;
                                act.ProcessingDate = ProcessingDate;
                                act.CustomerId = Convert.ToInt16(custid);
                                db.ActivityLogs.Add(act);
                                db.SaveChanges();
                            }
                        }
                    }
                }
                else
                {
                    string finpass = cmf.EncryptPassword(upass);
                    var model1 = db.UserMasters.Where(m => m.LoginID == uname && m.Password == finpass).SingleOrDefault();

                    if (model1 != null)
                    {
                        flg = true;
                        if (procdate != null && procdate != "")
                        {
                            slplitprocdate = procdate.Split('/');
                            ProcessingDate = Convert.ToDateTime(slplitprocdate[2] + "-" + slplitprocdate[1] + "-" + slplitprocdate[0]);
                        }
                        else
                            ProcessingDate = DateTime.Now;
                        if (custid == null)
                            custid = "0";


                        ActivityLog act = new ActivityLog();
                        if (loglevel != null)
                            act.LogLevel = loglevel;
                        else
                            act.LogLevel = "SOD";

                        act.ActionTaken = "Modified";
                        act.TimeStamp = DateTime.Now;
                        act.LoginId = uname;
                        act.ProcessingDate = ProcessingDate;
                        act.CustomerId = Convert.ToInt16(custid);
                        db.ActivityLogs.Add(act);
                        db.SaveChanges();
                    }
                }
            }

            return Json(flg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExistinHolidayMaster(string procdate, string customername)
        {
            string flg = "";
            //IFormatProvider culture = new CultureInfo("en-US", true);

            //DateTime dateVal = DateTime.ParseExact(procdate, "yyyy-MM-dd", culture);
            //string dateVal = procdate;
            //string dateVal = procdate.ToString("yyyy-dd-MM");

            DateTime processingDate1 = Convert.ToDateTime(procdate.ToString());
            string dateVal = processingDate1.ToString("yyyy-MM-dd");
            var xyz = Convert.ToDateTime(dateVal);

            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("ExistinHolidayMasterSOD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@customername", SqlDbType.VarChar).Value = customername;
                    cmd.Parameters.Add("@ProcessingDate", SqlDbType.VarChar).Value = dateVal;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        flg = reader["HolidayMasterExists"].ToString();
                    }
                }
            }

            return Json(flg, JsonRequestBehavior.AllowGet);

        }
    }
}
