using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Web.Security;
using System.Text.RegularExpressions;
using ikloud_Aflatoon;
using ikloud_Aflatoon.Models;
using ikloud_Aflatoon.Infrastructure;
using System.Data.SqlClient;
using System.Configuration;


namespace iKloud.Controllers
{
    //[OutputCache(Duration = 0)]
    public class CreateUserController : Controller
    {
        AflatoonEntities af = new AflatoonEntities();
        //IWProcDataContext iwpro = new IWProcDataContext();
        OWProcDataContext OWpro = new OWProcDataContext();
        UserAflatoonDbContext adc = new UserAflatoonDbContext();
        UserAflatoonDbContext dbTemp;
        UserLogDbContext ulc = new UserLogDbContext();
        CommonFunction cmf = new CommonFunction();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
        string[] rollname = new string[] { "UserManagment", "RejectRepair", "DE", "QC", "VF", "Report", "fildwnd", "Ds", "Query", "QueryMod", "sod", "master", "settg", "archv", "mesgbrd", "chirjct", "RVF" };
        string allroles = "";
        string ORGNIzations = "";
        string Customers = "";
        string Domains = "";
        //
        // GET: /CreateUser/

        public ActionResult Index(int id = 0)
        {
            //string ih = Session["asds"].ToString();
            //Session["uid"] = 1;
            UserMaster usrm;
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            if ((bool)Session["UserManagment"] == false)
            {
                uid = (int)Session["uid"];
                usrm = adc.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            usrm = adc.UserMasters.Find(uid);

            if (id == 0)
            {
                List<UserMaster> usmstr = new List<UserMaster>();

                if (usrm.UsertType == "Developer_User")
                {
                    usmstr = (from a in adc.UserMasters
                              where a.UserDeleted == false //&& a.UsertType == usrm.UsertType
                              select a).ToList();
                }
                else
                {
                    usmstr = (from a in adc.UserMasters
                              where a.UserDeleted == false && a.UsertType == usrm.UsertType
                              select a).ToList();
                }

                return View(usmstr);
            }
            else if (id == 2 || id == 3 || id == 5 || id == 4 || id == 6 || id == 7)
            {
                ViewBag.disable = true;

                if (id == 3)
                    ViewBag.mesg = "User has been reset successfully !";
                else if (id == 2)
                    ViewBag.mesg = "User successfully enable/disable !";
                else if (id == 4)
                    ViewBag.mesg = "User successfully unlocked !";
                else if (id == 5)
                    ViewBag.mesg = "User successfully updated !";
                else if (id == 6)
                    ViewBag.mesg = "You can not disable yourself !";
                else if (id == 7)
                    ViewBag.mesg = "User has been marked delete  !";

                List<UserMaster> usmstr = new List<UserMaster>();

                if (usrm.UsertType == "Developer_User")
                {
                    usmstr = (from a in adc.UserMasters
                              where a.UserDeleted == false //&& a.UsertType == usrm.UsertType
                              select a).ToList();
                }
                else
                {
                    usmstr = (from a in adc.UserMasters
                              where a.UserDeleted == false && a.UsertType == usrm.UsertType
                              select a).ToList();
                }

                //var query = from a in adc.UserMasters
                //            where a.UserDeleted == false
                //            select a;
                return View(usmstr);
            }
            else
            {
                List<UserMaster> usmstr = new List<UserMaster>();

                if (usrm.UsertType == "Developer_User")
                {
                    usmstr = (from a in adc.UserMasters
                              where a.UserDeleted == false //&& a.UsertType == usrm.UsertType
                              select a).ToList();
                }
                else
                {
                    usmstr = (from a in adc.UserMasters
                              where a.UserDeleted == false && a.UsertType == usrm.UsertType
                              select a).ToList();
                }
                //var query = from a in adc.UserMasters
                //            where a.UserDeleted == false //&& a.UsertType==
                //            select a;
                ViewBag.modf = true;

                return View(usmstr);
            }

        }

        //
        // GET: /CreateUser/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Session["domainid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            UserMaster usermaster = adc.UserMasters.Find(id);
            if (usermaster == null)
            {
                return HttpNotFound();
            }
            return View(usermaster);
        }

        //
        // GET: /CreateUser/Create

        public ActionResult Create(int id = 0)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["UserManagment"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = adc.UserMasters.Find(uid);
                usrm.Active = false;
                adc.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            if (id == 1)
            {
                ViewBag.value = "1";
                int uid = (int)Session["uid"];
                UserMaster usrm = adc.UserMasters.Find(uid);
                ////var resut = from a in adc.DomainUserMapMasters
                ////                from d in adc.DomainMaster
                ////                orderby a.ID
                ////                where a.DomainId == d.Id && a.UserId == uid
                ////                select new { d.Id, d.Name };
                ////ViewBag.droplist = new SelectList(resut.AsEnumerable(), "ID", "DomainName");
                ////ViewBag.droplist = resut.Select(a => a.Name);
                ////ViewBag.dropID = resut.Select(a => a.Id);
                ////-------------User Type------------
                //var qry = from a in af.UsertTpeMasters
                //          orderby a.UserType

                //          select new { a.UserType };
                //ViewBag.Vlevel = qry.Select(a => a.UserType);



                return View(usrm);
            }
            else
            {
                try
                {
                    int uid = (int)Session["uid"];
                    UserMaster usrm = adc.UserMasters.Find(uid);
                    //var resut = from a in adc.UserDomainMappings
                    //            from d in adc.Domains
                    //            orderby d.ID
                    //            where a.Domain_ID == d.ID && a.User_ID == uid
                    //            select new { d.ID, d.DomainName };

                    //ViewBag.droplist = new SelectList(resut.AsEnumerable(), "ID", "DomainName");
                    //ViewBag.dropID = resut.Select(a => a.ID);

                    //-------------User Type------------
                    if (usrm.UsertType == "Developer_User")
                    {
                        var qry = (from a in af.UsertTpeMasters
                                   //   where a.UserType == usrm.UsertType
                                   orderby a.UserType
                                   select new { a.UserType });

                        ViewBag.Vlevel = qry.Select(a => a.UserType);
                    }
                    else
                    {
                        var qry = (from a in af.UsertTpeMasters
                                   where a.UserType == usrm.UsertType
                                   orderby a.UserType
                                   select new { a.UserType });

                        ViewBag.Vlevel = qry.Select(a => a.UserType);
                    }


                    return View(usrm);
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
            //return View();
        }
        public PartialViewResult _CreateEntryField()
        {
            int uid = (int)Session["uid"];
            UserMaster um = new UserMaster();
            UserMaster usrm = adc.UserMasters.Find(uid);
            um.Password = "************************";
            var qry = from a in adc.AppSecPolicies
                      orderby a.ID
                      select new { a.Name };

            ViewBag.policy = qry.Select(a => a.Name);
            //------------------City selection--------------

            var city = from c in af.Cities
                       orderby c.CityName
                       select new { c.CityName };

            ViewBag.city = city.Select(c => c.CityName);
            //-------------User Type------------
            if (usrm.UsertType == "Developer_User")
            {
                var usertype = (from a in af.UsertTpeMasters
                           //   where a.UserType == usrm.UsertType
                           orderby a.UserType
                           select new { a.UserType });

                ViewBag.Vlevel = usertype.Select(a => a.UserType);
            }
            else
            {
                var usertype = (from a in af.UsertTpeMasters
                           where a.UserType == usrm.UsertType
                           orderby a.UserType
                           select new { a.UserType });

                ViewBag.Vlevel = usertype.Select(a => a.UserType);
            }
            //-----------------------------------
            //var qry1 = from a in af.UsertTpeMasters
            //           orderby a.UserType

            //           select new { a.UserType };
            //ViewBag.Vlevel = qry1.Select(a => a.UserType);

            //-------------User AccessLevel--------------
            var Accesslevel = af.UserMasters.Find(uid).AccessLevel;
            List<SelectListItem> lst = new List<SelectListItem>();
            if (Accesslevel == "ORG")
            {
                lst.Add(new SelectListItem() { Text = "Orgnization Level", Value = "ORG" });
                lst.Add(new SelectListItem() { Text = "Customer Level", Value = "CUST" });
                lst.Add(new SelectListItem() { Text = "Domain Level", Value = "DOM" });
            }
            else if (Accesslevel == "CUST")
            {
                lst.Add(new SelectListItem() { Text = "Customer Level", Value = "CUST" });
                lst.Add(new SelectListItem() { Text = "Domain Level", Value = "DOM" });
            }
            else if (Accesslevel == "DOM")
            {
                lst.Add(new SelectListItem() { Text = "Domain Level", Value = "DOM" });
            }

            ViewBag.SelectedAccesslevel = lst;

            return PartialView("_EntryField", um);
        }
        public PartialViewResult _SelectDomain()
        {
            int uid = (int)Session["uid"];
            var resut = (from a in adc.DomainUserMapMasters
                         from d in adc.Domains
                         orderby d.ID
                         where a.DomainId == d.ID && a.UserId == uid
                         select new SelectDomain
                         {
                             domainName = d.DomainName
                         }).ToList();

            return PartialView("_SelectDomains", resut);
        }
        public PartialViewResult _SelectOrgnization()
        {
            int uid = (int)Session["uid"];

            ViewBag.Customer = new SelectList(af.CustomerMasters, "Id", "Name").ToList();
            var resut = (from a in af.UserOrganizationMappings
                         from d in af.OrganizationMasters
                         orderby d.Id
                         where a.OrganizationId == d.Id && a.UserId == uid
                         select new
                         {
                             OrgnizationName = d.Name,
                             OrgnizationID = d.Id
                         }).ToList();
            ViewBag.Org = new SelectList(resut.AsEnumerable(), "OrgnizationID", "OrgnizationName");
            return PartialView("_Selectorgnization");
        }
        //-_SelectCustomer
        //[HttpPost]
        public PartialViewResult _SelectCustomer(List<string> customerlist = null)
        {
            int uid = (int)Session["uid"];
            List<int> orgnisids = new List<int>();
            if (customerlist != null)
            {
                //string[] customersarry = customerlist.Split(',');
                if (customerlist.Count > 0)
                {
                    for (int i = 0; i < customerlist.Count; i++)
                    {
                        orgnisids.Add(Convert.ToInt16(customerlist[i]));
                    }
                }
            }
            else
            {

                orgnisids = (from u in af.UserCustomerMappings
                             from c in af.CustomerMasters
                             where u.CustomerId == c.Id && u.UserId == uid
                             select c.OrganizationId).ToList();

            }

            var resut = (from a in af.UserCustomerMappings
                         from d in af.CustomerMasters
                         from o in af.OrganizationMasters
                         orderby d.Id
                         where a.CustomerId == d.Id && d.OrganizationId == o.Id && orgnisids.Contains(d.OrganizationId) && a.UserId == uid
                         select new
                         {
                             Customer = d.Name,
                             CustomerID = d.Id

                         }).ToList();
            ViewBag.CustBag = new SelectList(resut.AsEnumerable(), "CustomerID", "Customer");
            return PartialView("_SelectCustomer");
        }
        public ActionResult _SelectDomains(List<string> customerIds = null)
        {
            int uid = (int)Session["uid"];
            List<int> custids = new List<int>();
            if (customerIds != null)
            {
                //string[] customersarry = customerlist.Split(',');
                if (customerIds.Count > 0)
                {
                    for (int i = 0; i < customerIds.Count; i++)
                    {
                        custids.Add(Convert.ToInt16(customerIds[i]));
                    }
                }
            }
            else
            {
                //custids.Add(Convert.ToInt16(Session["CustomerID"]));
                custids = (from u in af.UserCustomerMappings
                           from c in af.CustomerMasters
                           where u.CustomerId == c.Id && u.UserId == uid
                           select c.Id).ToList();
                // orgnisids.Add(uid);
            }
            if (custids.Count != 0)
            {
                var resut = (from a in af.DomainUserMapMasters
                             from d in af.DomainMasters
                             from c in af.CustomerMasters
                             orderby d.Id
                             where a.DomainId == d.Id && d.CustomerId == c.Id && custids.Contains(d.CustomerId) && a.UserId == uid
                             select new
                             {
                                 domainName = d.Name + "-" + c.Name,
                                 Id = d.Id
                             }).ToList();
                ViewBag.DOM = new SelectList(resut.AsEnumerable(), "Id", "domainName");
            }
            else
            {
                var resut = (from a in af.DomainUserMapMasters
                             from d in af.DomainMasters
                             from c in af.CustomerMasters
                             orderby d.Id
                             where a.DomainId == d.Id && a.CustomerID == d.CustomerId && c.Id == a.CustomerID && a.UserId == uid
                             select new
                             {
                                 domainName = d.Name + "-" + c.Name,
                                 Id = d.Id
                             }).ToList();
                ViewBag.DOM = new SelectList(resut.AsEnumerable(), "Id", "domainName");
            }

            return PartialView("_SelectDomainsDrop");
        }
        public PartialViewResult _SelectedUsrDomains(int id = 0)
        {
            var resut = (from a in adc.DomainUserMapMasters
                         from d in adc.Domains
                         orderby d.ID
                         where a.DomainId == d.ID && a.UserId == id
                         select new SelectedUsrDomain
                         {
                             domainName = d.DomainName
                         }).ToList();
            return PartialView("_SelectedUsrDomain", resut);
        }

        //public PartialViewResult selectBranches()
        //{
        //    int id = (int)Session["uid"];
        //    //---------- Select Branches-------------
        //    var selectmodel = (from b in af.Branches
        //                       from d in db.Domains
        //                       from u in db.UserDomainMapping
        //                       orderby b.ID
        //                       where b.Domain.ID == d.ID && u.Domain.ID == d.ID && u.User.ID == id
        //                       select new selectbranches
        //                       {
        //                           domainID = b.Domain.ID,
        //                           domainname = d.DomainName,
        //                           branchname = b.BranchName,
        //                           brid = b.ID
        //                       });
        //    return PartialView("_CheckBranches", selectmodel);

        //}

        //
        // POST: /CreateUser/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserMaster usermaster)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["UserManagment"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = adc.UserMasters.Find(uid1);
                usrm.Active = false;
                adc.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled", id = 1 });
            }
            try
            {
                if (ModelState.IsValid)
                {
                    ViewBag.error = true;
                    var result = from a in adc.UserMasters
                                 where a.LoginID == usermaster.LoginID
                                 select a;
                    if (result.Count() != 0)
                    {
                        ViewBag.result = true;

                        ModelState.AddModelError("", "This user id already exists!");
                        return View();
                    }
                    else
                    {
                        int id = (int)Session["uid"];
                        string verlevl = "";

                        string dob = Request.Form["datepicker"];
                        string title = Request.Form["title"];
                        string location = "";
                        string Accesslevel = "";
                        decimal l1frm = 0;
                        decimal l1to = 0;
                        decimal l2frm = 0;
                        decimal l2to = 0;
                        decimal l3frm = 0;
                        decimal l3to = 0;
                        string policyname = Request.Form["policynm"];
                        var pol = adc.AppSecPolicies.Where(m => m.Name == policyname).SingleOrDefault();
                        if (dob != null || dob != "")
                        {
                            string[] dobb = dob.Split('/');
                            if (dobb.Length > 2)
                            {
                                dob = dobb[2].Substring(0, 4) + '-' + dobb[0] + '-' + dobb[1];
                                usermaster.DOB = Convert.ToDateTime(dob);
                            }
                            else
                                usermaster.DOB = DateTime.Now;
                        }
                        else
                            usermaster.DOB = DateTime.Now;

                        RoleMapping rol = new RoleMapping();
                        int tempuserid = 0;
                        //Inserting into UserMaster Venlvel
                        if (Request.Form["usertype"] != "")
                        {
                            verlevl = Request.Form["usertype"].ToString();
                        }
                        if (Request.Form["city"] != "")
                        {
                            location = Request.Form["city"].ToString();
                        }
                        if (Request.Form["SelectedAccesslevel"] != "")
                        {
                            Accesslevel = Request.Form["SelectedAccesslevel"].ToString();
                        }
                        //-----------Amount Limit------------
                        if (Request.Form["qc"] == "on")
                        {
                            l1frm = Convert.ToDecimal(Request.Form["l1frm"]);
                            l1to = Convert.ToDecimal(Request.Form["l1to"]);
                        }
                        if (Request.Form["vf"] == "on")
                        {
                            l2frm = Convert.ToDecimal(Request.Form["l2frm"]);
                            l2to = Convert.ToDecimal(Request.Form["l2to"]);

                        }
                        if (Request.Form["rvf"] == "on")
                        {
                            l3frm = Convert.ToDecimal(Request.Form["l3frm"]);
                            l3to = Convert.ToDecimal(Request.Form["l3to"]);
                        }
                        usermaster.L1StartLimit = l1frm;
                        usermaster.L1StopLimit = l1to;
                        usermaster.L2StartLimit = l2frm;
                        usermaster.L2StopLimit = l2to;
                        usermaster.L3StartLimit = l3frm;
                        usermaster.L3StopLimit = l3to;
                        //-----------------------------------
                        string password = "newuser@123";
                        usermaster.Password = cmf.EncryptPassword(password);
                        usermaster.Title = title;
                        usermaster.FirstLogin = true;
                        usermaster.AppSecPolicieID = pol.ID;
                        usermaster.createdby = id;
                        usermaster.creationdate = DateTime.Now;
                        usermaster.City = location;
                        usermaster.UsertType = verlevl;
                        usermaster.AccessLevel = Accesslevel;
                        //usermaster.createdby = Convert.ToInt16(Session["uid"]);
                        usermaster.InvalidPasswordAttempts = 0;
                        adc.UserMasters.Add(usermaster);
                        try
                        {
                            adc.SaveChanges();
                        }
                        catch (Exception e)
                        {

                            //ErrorDisplay er = new ErrorDisplay();
                            //ViewBag.Error = e.InnerException;
                            //er.ErrorMessage = e.InnerException.Message;
                            //return View("Error", er);
                            return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
                        }

                        var userid = from a in adc.UserMasters
                                     where a.LoginID == usermaster.LoginID
                                     select a;
                        if (userid.Count() != 0)
                        {
                            foreach (var x in userid)
                            {
                                tempuserid = x.ID;
                            }
                        }
                        // Inserting into RoleMappings
                        string rc = Request.Form["rc"];
                        string de = Request.Form["de"];
                        string qc = Request.Form["qc"];
                        string vf = Request.Form["vf"];
                        string rpt = Request.Form["rpt"];
                        string fildwnd = Request.Form["fildwnd"];
                        string ds = Request.Form["ds"];
                        string Query = Request.Form["qury"];
                        string Querymd = Request.Form["quryMd"];

                        //----------- Admin Rights ----------------
                        string mainAdmin = Request.Form["chkadmin"];
                        string um = Request.Form["um"];
                        string sod = Request.Form["sod"];
                        string master = Request.Form["master"];
                        string settg = Request.Form["settg"];
                        string archv = Request.Form["archv"];
                        string mesgbrd = Request.Form["mesgbrd"];
                        string chirjct = Request.Form["chirjct"];

                        //---------------Added On 30-03-2014--------------------------
                        string RVF = Request.Form["rvf"];

                        if (RVF == "on")
                        {
                            allroles = "RVF";
                            rol.Process = rollname[16];
                            rol.Active = true;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        else
                        {
                            rol.Process = rollname[16];
                            rol.Active = false;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }

                        //-----------Additional For CHI Reject ---------
                        if (chirjct == "on")
                        {
                            allroles = "CHR";
                            rol.Process = rollname[15];
                            rol.Active = true;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        else
                        {
                            rol.Process = rollname[15];
                            rol.Active = false;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        //---------------------------------------------

                        //string d= domain[id];

                        if (rc == "on")
                        {
                            allroles = "RR";
                            rol.Process = rollname[1];
                            rol.Active = true;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        else
                        {
                            rol.Process = rollname[1];
                            rol.Active = false;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        if (de == "on")
                        {
                            allroles = allroles + ",DE";
                            rol.Process = rollname[2];
                            rol.Active = true;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        else
                        {
                            rol.Process = rollname[2];
                            rol.Active = false;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        if (qc == "on")
                        {
                            allroles = allroles + ",QC";
                            rol.Process = rollname[3];
                            rol.Active = true;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        else
                        {
                            rol.Process = rollname[3];
                            rol.Active = false;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        if (vf == "on")
                        {
                            allroles = allroles + ",VF";
                            rol.Process = rollname[4];
                            rol.Active = true;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        else
                        {
                            rol.Process = rollname[4];
                            rol.Active = false;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        if (rpt == "on")
                        {
                            allroles = allroles + ",RP";
                            rol.Process = rollname[5];
                            rol.Active = true;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        else
                        {
                            rol.Process = rollname[5];
                            rol.Active = false;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        if (fildwnd == "on")
                        {
                            allroles = allroles + ",FD";
                            rol.Process = rollname[6];
                            rol.Active = true;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        else
                        {
                            rol.Process = rollname[6];
                            rol.Active = false;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        if (ds == "on")
                        {
                            allroles = allroles + ",DB";
                            rol.Process = rollname[7];
                            rol.Active = true;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        else
                        {
                            rol.Process = rollname[7];
                            rol.Active = false;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        if (Query == "on")
                        {
                            allroles = allroles + ",Query";
                            rol.Process = rollname[8];
                            rol.Active = true;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        else
                        {
                            //  allroles = allroles";
                            rol.Process = rollname[8];
                            rol.Active = false;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        if (Querymd == "on")
                        {
                            allroles = allroles + ",QM";
                            rol.Process = rollname[9];
                            rol.Active = true;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        else
                        {
                            //  allroles = allroles";
                            rol.Process = rollname[9];
                            rol.Active = false;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        //-----Adimn Rights---------------
                        //if (mainAdmin == "on")
                        //{
                        if (um == "on")
                        {
                            allroles = allroles + ",UM";
                            rol.Process = rollname[0];
                            rol.Active = true;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();

                        }
                        else
                        {
                            rol.Process = rollname[0];
                            rol.Active = false;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        if (sod == "on")
                        {
                            allroles = allroles + ",SOD";
                            rol.Process = rollname[10];
                            rol.Active = true;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();

                        }
                        else
                        {
                            rol.Process = rollname[10];
                            rol.Active = false;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }

                        if (master == "on")
                        {
                            allroles = allroles + ",MST";
                            rol.Process = rollname[11];
                            rol.Active = true;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();

                        }
                        else
                        {
                            rol.Process = rollname[11];
                            rol.Active = false;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        if (settg == "on")
                        {
                            allroles = allroles + ",ST";
                            rol.Process = rollname[12];
                            rol.Active = true;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();

                        }
                        else
                        {
                            rol.Process = rollname[12];
                            rol.Active = false;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        if (archv == "on")
                        {
                            allroles = allroles + ",AR";
                            rol.Process = rollname[13];
                            rol.Active = true;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        else
                        {
                            rol.Process = rollname[13];
                            rol.Active = false;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        if (mesgbrd == "on")
                        {
                            allroles = allroles + ",MB";
                            rol.Process = rollname[14];
                            rol.Active = true;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();

                        }
                        else
                        {
                            rol.Process = rollname[14];
                            rol.Active = false;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        //}


                        //-------Admin End----------------
                        //----------------- User ActivitiesLogs------
                        UserMasterActivity uma = new UserMasterActivity();
                        uma.Action = "User Created";
                        uma.ActionBy = id;
                        uma.Actiondate = DateTime.Now;
                        uma.UserId = tempuserid;
                        uma.comments = "Roles[" + allroles + "]";

                        ulc.UserMasterActivities.Add(uma);

                        //----------------------------------END-------
                        try
                        {
                            ulc.SaveChanges();
                            adc.SaveChanges();

                            uma.Action = "User Created";
                            uma.ActionBy = id;
                            uma.Actiondate = DateTime.Now;
                            uma.UserId = tempuserid;
                            uma.comments = "User Level " + Accesslevel;

                            ulc.UserMasterActivities.Add(uma);
                            ulc.SaveChanges();
                            adc.SaveChanges();
                        }
                        catch (Exception e1)
                        {
                            //ErrorDisplay er = new ErrorDisplay();
                            //ViewBag.Error = e1.InnerException;
                            //er.ErrorMessage = e1.InnerException.Message;
                            //return View("Error", er);
                            return RedirectToAction("Error", "Error", new { msg = e1.Message.ToString(), popmsg = e1.StackTrace.ToString() });
                        }
                        //----------------------------Added On 08-02-2017----------------
                        string orgnization = "";
                        string[] orgnizationlist;

                        string customer = "";
                        string[] customerlist;

                        string domainsids = "";
                        string[] domainlists;

                        if (Accesslevel == "ORG")
                        {
                            //---------------------Orgnization-Maping---------------
                            UserOrganizationMapping usrOrgMap;
                            List<int> orgids = new List<int>();
                            orgnization = Request.Form["Org"].ToString();
                            orgnizationlist = orgnization.Split(',');
                            for (int i = 0; i < orgnizationlist.Length; i++)
                            {
                                usrOrgMap = new UserOrganizationMapping();
                                usrOrgMap.OrganizationId = Convert.ToInt16(orgnizationlist[i]);
                                usrOrgMap.UserId = (int)tempuserid;
                                usrOrgMap.CreatedBy = id;
                                usrOrgMap.CreatedOn = DateTime.Now;
                                adc.UserOrganizationMappings.Add(usrOrgMap);
                                orgids.Add(Convert.ToInt16(orgnizationlist[i]));

                                ORGNIzations = ORGNIzations + "," + usrOrgMap.OrganizationId;

                            }

                            uma = new UserMasterActivity();
                            uma.Action = "User Created";
                            uma.ActionBy = id;
                            uma.Actiondate = DateTime.Now;
                            uma.UserId = tempuserid;
                            uma.comments = "ORG" + " " + ORGNIzations;
                            ulc.UserMasterActivities.Add(uma);
                            ulc.SaveChanges();
                            //-------------------Customer-Maping------------------------------
                            UserCustomerMapping usrCustMap;

                            var custlist = (from c in af.CustomerMasters
                                            // from uc in af.UserCustomerMappings
                                            where orgids.Contains(c.OrganizationId) //&& c.Id == uc.CustomerId && uc.UserId == id
                                            select c.Id);

                            foreach (var item in custlist)
                            {
                                usrCustMap = new UserCustomerMapping();
                                usrCustMap.CustomerId = item;// Convert.ToInt16(orgnizationlist[i]);
                                usrCustMap.UserId = (int)tempuserid;
                                usrCustMap.CreatedBy = id;
                                usrCustMap.CreatedOn = DateTime.Now;
                                adc.UserCustomerMappings.Add(usrCustMap);

                                Customers = Customers + "," + usrCustMap.CustomerId;
                            }

                            uma = new UserMasterActivity();
                            uma.Action = "User Created";
                            uma.ActionBy = id;
                            uma.Actiondate = DateTime.Now;
                            uma.UserId = tempuserid;
                            uma.comments = "Cust" + " " + Customers;
                            ulc.UserMasterActivities.Add(uma);
                            ulc.SaveChanges();
                            adc.SaveChanges();
                            //-----------------------DomainMaping---------
                            //DomainUserMapMaster domainUsrMap;

                            //var domlist = (from d in af.DomainMasters
                            //               from c in af.UserCustomerMappings
                            //               //  from ud in af.DomainUserMapMaster
                            //               where d.CustomerId == c.CustomerId && c.UserId == tempuserid //&& d.Id == ud.DomainId && ud.UserId == id
                            //               select new { d.Id, d.CustomerId });

                            //foreach (var item in domlist)
                            //{
                            //    domainUsrMap = new DomainUserMapMaster();
                            //    domainUsrMap.CustomerID = item.CustomerId;// Convert.ToInt16(orgnizationlist[i]);
                            //    domainUsrMap.DomainId = item.Id;
                            //    domainUsrMap.UserId = (int)tempuserid;
                            //    adc.DomainUserMapMasters.Add(domainUsrMap);
                            //    Domains = Domains + "," + domainUsrMap.DomainId;
                            //}
                            DataSet dsdomin = new DataSet();
                            SqlDataAdapter adp = new SqlDataAdapter("SelectUserDomains", con);
                            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                            adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.BigInt).Value = 0;
                            adp.SelectCommand.Parameters.Add("@DomainID", SqlDbType.BigInt).Value = 0;
                            adp.SelectCommand.Parameters.Add("@UserID", SqlDbType.BigInt).Value = usermaster.ID;

                            adp.Fill(dsdomin);
                            //  IWSearch Owsr = null;
                            if (dsdomin.Tables[0].Rows.Count > 0)
                            {
                                int index = 0;
                                int count = dsdomin.Tables[0].Rows.Count;
                                while (count > 0)
                                {
                                    OWpro.InsertDomainUserMapMaster(Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[1]), Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[0]), (int)usermaster.ID);
                                    index = index + 1;
                                    count = count - 1;
                                }
                            }

                            uma = new UserMasterActivity();
                            uma.Action = "User Created";
                            uma.ActionBy = id;
                            uma.Actiondate = DateTime.Now;
                            uma.UserId = tempuserid;
                            uma.comments = "Dom" + " " + Domains;
                            ulc.UserMasterActivities.Add(uma);
                            ulc.SaveChanges();
                            adc.SaveChanges();

                        }
                        else if (Accesslevel == "CUST")
                        {

                            List<int> custids = new List<int>();
                            customer = Request.Form["CustBag"].ToString();
                            customerlist = customer.Split(',');
                            //-------------------Customer-Maping------------------------------
                            UserCustomerMapping usrCustMap;
                            for (int i = 0; i < customerlist.Length; i++)
                            {
                                usrCustMap = new UserCustomerMapping();
                                usrCustMap.CustomerId = Convert.ToInt16(customerlist[i]);
                                usrCustMap.UserId = (int)tempuserid;
                                usrCustMap.CreatedBy = id;
                                usrCustMap.CreatedOn = DateTime.Now;
                                adc.UserCustomerMappings.Add(usrCustMap);
                                Customers = Customers + "," + usrCustMap.CustomerId;
                            }

                            uma = new UserMasterActivity();
                            uma.Action = "User Created";
                            uma.ActionBy = id;
                            uma.Actiondate = DateTime.Now;
                            uma.UserId = tempuserid;
                            uma.comments = "Cust" + " " + Customers;
                            ulc.UserMasterActivities.Add(uma);
                            ulc.SaveChanges();
                            adc.SaveChanges();
                            //-----------------------DomainMaping---------
                            DomainUserMapMaster domainUsrMap;

                            var domlist = (from d in af.DomainMasters
                                           from c in af.UserCustomerMappings
                                           // from ud in af.DomainUserMapMaster
                                           where d.CustomerId == c.CustomerId && c.UserId == tempuserid //&& d.Id == ud.DomainId && ud.UserId == id   
                                           select new { d.Id, d.CustomerId });

                            foreach (var item in domlist)
                            {
                                domainUsrMap = new DomainUserMapMaster();
                                domainUsrMap.CustomerID = item.CustomerId;// Convert.ToInt16(orgnizationlist[i]);
                                domainUsrMap.DomainId = item.Id;
                                domainUsrMap.UserId = (int)tempuserid;
                                adc.DomainUserMapMasters.Add(domainUsrMap);
                                Domains = Domains + "," + domainUsrMap.DomainId;
                            }
                            uma = new UserMasterActivity();
                            uma.Action = "User Created";
                            uma.ActionBy = id;
                            uma.Actiondate = DateTime.Now;
                            uma.UserId = tempuserid;
                            uma.comments = "Dom" + " " + Domains;
                            ulc.UserMasterActivities.Add(uma);

                            ulc.SaveChanges();
                            adc.SaveChanges();

                        }
                        else if (Accesslevel == "DOM")
                        {

                            List<int> domns = new List<int>();
                            domainsids = Request.Form["Dom"].ToString();
                            domainlists = domainsids.Split(',');
                            for (int i = 0; i < domainlists.Length; i++)
                            {
                                domns.Add(Convert.ToInt16(domainlists[i]));
                            }
                            //-----------------------DomainMaping---------
                            DomainUserMapMaster domainUsrMap;

                            var domlist = (from d in af.DomainMasters
                                           where domns.Contains(d.Id)
                                           select new { d.Id, d.CustomerId });

                            foreach (var item in domlist)
                            {
                                domainUsrMap = new DomainUserMapMaster();
                                domainUsrMap.CustomerID = item.CustomerId;// Convert.ToInt16(orgnizationlist[i]);
                                domainUsrMap.DomainId = item.Id;
                                domainUsrMap.UserId = (int)tempuserid;
                                adc.DomainUserMapMasters.Add(domainUsrMap);
                                Domains = Domains + "," + domainUsrMap.DomainId;
                            }
                            uma = new UserMasterActivity();
                            uma.Action = "User Created";
                            uma.ActionBy = id;
                            uma.Actiondate = DateTime.Now;
                            uma.UserId = tempuserid;
                            uma.comments = "Dom" + " " + Domains;
                            ulc.UserMasterActivities.Add(uma);

                            ulc.SaveChanges();
                            adc.SaveChanges();
                        }


                        //----------------- User ActivitiesLogs------
                        //UserMasterActivity uma = new UserMasterActivity();

                        ////----------------------------------END-------
                        try
                        {
                            adc.SaveChanges();
                        }
                        catch (Exception e1)
                        {
                            //ErrorDisplay er = new ErrorDisplay();
                            //ViewBag.Error = e1.InnerException;
                            //er.ErrorMessage = e1.InnerException.Message;
                            //return View("Error", er);
                            return RedirectToAction("Error", "Error", new { msg = e1.Message.ToString(), popmsg = e1.StackTrace.ToString() });
                        }

                        ViewBag.result = false;
                        return RedirectToAction("Create", "CreateUser", new { id = 1 });
                    }
                }
                //-------------User Type------------
                var qry = from a in af.UsertTpeMasters
                          orderby a.UserType

                          select new { a.UserType };
                ViewBag.Vlevel = qry.Select(a => a.UserType);

                return View(usermaster);
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
        // GET: /CreateUser/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            if ((bool)Session["UserManagment"] == false)
            {
                UserMaster usrm = adc.UserMasters.Find(uid);
                usrm.Active = false;
                adc.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            UserMaster usermaster = adc.UserMasters.Find(id);
            ViewBag.DGDetails = true;

            var qry = from a in adc.AppSecPolicies
                      orderby a.ID
                      select new { a.Name };

            ViewBag.policy = qry.Select(a => a.Name);
            var qrynam = (from a in adc.AppSecPolicies
                          where a.ID == usermaster.AppSecPolicieID
                          orderby a.ID
                          select a.Name).SingleOrDefault();

            ViewBag.policynamee = qrynam;
            //-------------User Type------------
            var qryy = from a in af.UsertTpeMasters
                       orderby a.UserType

                       select new { a.UserType };
            ViewBag.Vlevel = qryy.Select(a => a.UserType);
            //------------------City selection--------------

            var city = from c in af.Cities
                       orderby c.CityName
                       select new { c.CityName };

            ViewBag.city = city.Select(c => c.CityName);

            //-------------User AccessLevel--------------
            var Accesslevel = af.UserMasters.Find(uid).AccessLevel;
            List<SelectListItem> lst = new List<SelectListItem>();
            if (Accesslevel == "ORG")
            {
                lst.Add(new SelectListItem() { Text = "Orgnization Level", Value = "ORG" });
                lst.Add(new SelectListItem() { Text = "Customer Level", Value = "CUST" });
                lst.Add(new SelectListItem() { Text = "Domain Level", Value = "DOM" });
            }
            else if (Accesslevel == "CUST")
            {
                lst.Add(new SelectListItem() { Text = "Customer Level", Value = "CUST" });
                lst.Add(new SelectListItem() { Text = "Domain Level", Value = "DOM" });
            }
            else if (Accesslevel == "DOM")
            {
                lst.Add(new SelectListItem() { Text = "Domain Level", Value = "DOM" });
            }

            ViewBag.SelectedAccesslevel = lst;


            if (usermaster == null)
                return HttpNotFound();
            else
                return View(usermaster);
        }

        //
        // POST: /CreateUser/Edit/5

        [HttpPost]
        public ActionResult Edit(UserMaster usermaster)
        {
            if (Session["domainid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["UserManagment"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = adc.UserMasters.Find(uid);
                usrm.Active = false;
                adc.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            try
            {
                if (ModelState.IsValid)
                {
                    //-----------------------
                    string verlevl = "";
                    string location = "";
                    decimal l1frm = 0;
                    decimal l1to = 0;
                    decimal l2frm = 0;
                    decimal l2to = 0;
                    decimal l3frm = 0;
                    decimal l3to = 0;
                    int uid = (int)Session["uid"];
                    string dob = Request.Form["datepicker"];
                    string title = Request.Form["title"];
                    string policyname = Request.Form["policynm"];
                    var pol = adc.AppSecPolicies.Where(m => m.Name == policyname).SingleOrDefault();

                    string[] dobb = dob.Split('/');

                    if (dobb.Length > 2)
                    {
                        dob = dobb[2].Substring(0, 4) + '-' + dobb[1] + '-' + dobb[0];
                        usermaster.DOB = Convert.ToDateTime(dob);
                    }
                    //Inserting into UserMaster Venlvel
                    if (Request.Form["usertype"] != "")
                    {
                        verlevl = Request.Form["usertype"].ToString();
                    }
                    if (Request.Form["city"] != "")
                    {
                        location = Request.Form["city"].ToString();
                    }
                    //-----------Amount Limit------------
                    if (Request.Form["qc"] == "on")
                    {
                        l1frm = Convert.ToDecimal(Request.Form["l1frm"]);
                        l1to = Convert.ToDecimal(Request.Form["l1to"]);
                    }
                    if (Request.Form["vf"] == "on")
                    {
                        l2frm = Convert.ToDecimal(Request.Form["l2frm"]);
                        l2to = Convert.ToDecimal(Request.Form["l2to"]);

                    }
                    if (Request.Form["rvf"] == "on")
                    {
                        l3frm = Convert.ToDecimal(Request.Form["l3frm"]);
                        l3to = Convert.ToDecimal(Request.Form["l3to"]);
                    }
                    //usermaster.L1StartLimit = l1frm;
                    //usermaster.L1StopLimit = l1to;
                    //usermaster.L2StartLimit = l2frm;
                    //usermaster.L2StopLimit = l2to;
                    //usermaster.L3StartLimit = l3frm;
                    //usermaster.L3StopLimit = l3to;
                    //usermaster.Title = title;
                    //usermaster.AppSecPolicieID = pol.ID;
                    //usermaster.City = location;
                    //usermaster.UsertType = verlevl;
                    ////if (usermaster.ID == uid)
                    ////    usermaster.sessionid = Session.SessionID;

                    //usermaster.ModifedBy = Convert.ToInt16(Session["uid"]);

                    //adc.Entry(usermaster).State = EntityState.Modified;
                    //adc.SaveChanges();

                    // Inserting into RoleMappings
                    string rc = Request.Form["rc"];
                    string de = Request.Form["de"];
                    string qc = Request.Form["qc"];
                    string vf = Request.Form["vf"];
                    string rpt = Request.Form["rpt"];
                    string fildwnd = Request.Form["fildwnd"];
                    string ds = Request.Form["ds"];
                    string Query = Request.Form["qury"];
                    string Querymd = Request.Form["quryMd"];
                    //----------- Admin Rights ----------------
                    string um = Request.Form["um"];
                    string sod = Request.Form["sod"];
                    string master = Request.Form["master"];
                    string settg = Request.Form["settg"];
                    string archv = Request.Form["archv"];
                    string mesgbrd = Request.Form["mesgbrd"];
                    string chirjct = Request.Form["chirjct"];
                    //---------------Added On 30-03-2015--------------------------
                    string RVF = Request.Form["rvf"];
                    RoleMapping rol = new RoleMapping();
                    if (RVF == "on")
                    {
                        RoleMapping rl = adc.RoleMappings.Where(m => m.UserID == usermaster.ID && m.Process == "RVF").SingleOrDefault();
                        if (rl == null)
                        {
                            allroles = "RVF";
                            rol.Process = rollname[16];
                            rol.Active = true;
                            rol.UserID = usermaster.ID;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                    }

                    var userRol = (from r in adc.RoleMappings
                                   where r.UserID == usermaster.ID
                                   select r);

                    RoleMapping rolmp = null;
                    foreach (var item in userRol)
                    {
                        dbTemp = new UserAflatoonDbContext();
                        //---------------------------------------
                        if (item.Process == "RVF")
                        {
                            rolmp = dbTemp.RoleMappings.Single(i => i.ID == item.ID);
                            if (RVF == "on")
                            {
                                rolmp.Active = true;
                                allroles = "RVF";
                            }
                            else
                                rolmp.Active = false;
                        }
                        //---------------------------------------
                        //----------------Additional--------------------
                        //if (chirjct == "on")
                        //{
                        //    chirjct = null;

                        //    var chir = (from s in adc.RoleMappings
                        //                where s.UserID == usermaster.ID && s.Process == "chirjct"
                        //                select s).Count();
                        //    UserAflatoonDbContext dbTemp1 = new UserAflatoonDbContext();
                        //    RoleMapping rolmp1 = new RoleMapping();
                        //    if (chir == 0)
                        //    {

                        //        rolmp1.UserID = usermaster.ID;
                        //        rolmp1.Process = "chirjct";
                        //        rolmp1.Active = true;
                        //        dbTemp1.RoleMappings.Add(rolmp1);
                        //        dbTemp1.SaveChanges();
                        //    }
                        //    else
                        //    {
                        //        rolmp1 = dbTemp1.RoleMappings.Single(i => i.ID == item.ID);
                        //        if (chirjct == "on")
                        //        {
                        //            rolmp1.Active = true;
                        //            allroles = "CHR";
                        //        }
                        //        else
                        //            rolmp1.Active = false;
                        //        dbTemp1.SaveChanges();
                        //    }

                        //}
                        if (item.Process == "chirjct")
                        {
                            rolmp = dbTemp.RoleMappings.Single(i => i.ID == item.ID);
                            if (chirjct == "on")
                            {
                                rolmp.Active = true;
                                allroles = "CHR";
                            }
                            else
                                rolmp.Active = false;
                        }

                        //--------------------SOD-----------------
                        //if (sod == "on")
                        //{
                        //    // sod = null;
                        //    var sodd = (from s in af.RoleMappings
                        //                where s.UserID == usermaster.ID && s.Process == "sod"
                        //                select s).Count();
                        //    UserAflatoonDbContext dbTemp2 = new UserAflatoonDbContext();
                        //    RoleMapping rolmp2 = new RoleMapping();
                        //    if (sodd == 0)
                        //    {

                        //        rolmp2.UserID = usermaster.ID;
                        //        rolmp2.Process = "sod";
                        //        rolmp2.Active = true;
                        //        dbTemp2.RoleMappings.Add(rolmp2);
                        //        dbTemp2.SaveChanges();
                        //    }
                        //    else
                        //    {
                        //        rolmp2 = dbTemp2.RoleMappings.Single(i => i.ID == item.ID);
                        //        if (sod == "on")
                        //        {
                        //            rolmp2.Active = true;
                        //            allroles = "sod";
                        //        }
                        //        else
                        //            rolmp2.Active = false;

                        //        dbTemp2.SaveChanges();
                        //        dbTemp2.Dispose();
                        //        //af.Dispose();
                        //    }

                        //}
                        //-------------
                        //if (item.Process == "sod")
                        //{
                        //    rolmp = dbTemp.RoleMappings.Single(i => i.ID == item.ID);
                        //    if (sod == "on")
                        //    {
                        //        rolmp.Active = true;
                        //        allroles = "sod";
                        //    }
                        //    else
                        //        rolmp.Active = false;
                        //}
                        ////-------------------------------------
                        if (item.Process == "RejectRepair")
                        {
                            rolmp = dbTemp.RoleMappings.Single(i => i.ID == item.ID);
                            if (rc == "on")
                            {
                                rolmp.Active = true;
                                allroles = "RR";
                            }
                            else
                                rolmp.Active = false;
                        }
                        if (item.Process == "DE")
                        {
                            rolmp = dbTemp.RoleMappings.Single(i => i.ID == item.ID);
                            if (de == "on")
                            {
                                rolmp.Active = true;
                                allroles = allroles + ",DE";
                            }
                            else
                                rolmp.Active = false;
                        }
                        if (item.Process == "QC")
                        {
                            rolmp = dbTemp.RoleMappings.Single(i => i.ID == item.ID);
                            if (qc == "on")
                            {
                                rolmp.Active = true;
                                allroles = allroles + ",QC";
                            }
                            else
                                rolmp.Active = false;
                        }
                        if (item.Process == "VF")
                        {
                            rolmp = dbTemp.RoleMappings.Single(i => i.ID == item.ID);
                            if (vf == "on")
                            {
                                rolmp.Active = true;
                                allroles = allroles + ",VF";
                            }
                            else
                                rolmp.Active = false;
                        }
                        if (item.Process == "Report")
                        {
                            rolmp = dbTemp.RoleMappings.Single(i => i.ID == item.ID);
                            if (rpt == "on")
                            {
                                rolmp.Active = true;
                                allroles = allroles + ",RP";
                            }
                            else
                                rolmp.Active = false;
                        }

                        if (item.Process == "fildwnd")
                        {
                            rolmp = dbTemp.RoleMappings.Single(i => i.ID == item.ID);
                            if (fildwnd == "on")
                            {
                                rolmp.Active = true;
                                allroles = allroles + ",FD";
                            }
                            else
                                rolmp.Active = false;
                        }
                        if (item.Process == "Ds")
                        {
                            rolmp = dbTemp.RoleMappings.Single(i => i.ID == item.ID);
                            if (ds == "on")
                            {
                                rolmp.Active = true;
                                allroles = allroles + ",DB";
                            }
                            else
                                rolmp.Active = false;
                        }
                        if (item.Process == "Query")
                        {
                            rolmp = dbTemp.RoleMappings.Single(i => i.ID == item.ID);
                            if (Query == "on")
                            {
                                rolmp.Active = true;
                                rolmp.Process = "Query";
                                allroles = allroles + ",Q";
                            }
                            else
                            {
                                rolmp.Active = false;
                            }
                        }
                        if (item.Process == "QueryMod")
                        {
                            rolmp = dbTemp.RoleMappings.Single(i => i.ID == item.ID);
                            if (Querymd == "on")
                            {
                                rolmp.Active = true;
                                rolmp.Process = "QueryMod";
                                allroles = allroles + ",QM";
                            }
                            else
                            {
                                rolmp.Active = false;
                            }
                        }
                        ////----------- Admin Rights------------------
                        if (item.Process == "UserManagment")
                        {
                            rolmp = dbTemp.RoleMappings.Single(i => i.ID == item.ID);
                            if (um == "on")
                            {
                                rolmp.Active = true;
                                allroles = allroles + ",UM";
                            }
                            else
                                rolmp.Active = false;
                        }
                        if (item.Process == "sod")
                        {
                            rolmp = dbTemp.RoleMappings.Single(i => i.ID == item.ID);
                            if (sod == "on")
                            {
                                rolmp.Active = true;
                                allroles = allroles + ",SOD";
                            }
                            else
                                rolmp.Active = false;
                        }
                        if (item.Process == "master")
                        {
                            rolmp = dbTemp.RoleMappings.Single(i => i.ID == item.ID);
                            if (master == "on")
                            {
                                rolmp.Active = true;
                                allroles = allroles + ",MST";
                            }
                            else
                                rolmp.Active = false;
                        }
                        if (item.Process == "settg")
                        {
                            rolmp = dbTemp.RoleMappings.Single(i => i.ID == item.ID);
                            if (settg == "on")
                            {
                                rolmp.Active = true;
                                allroles = allroles + ",ST";
                            }
                            else
                                rolmp.Active = false;
                        }
                        if (item.Process == "archv")
                        {
                            rolmp = dbTemp.RoleMappings.Single(i => i.ID == item.ID);
                            if (archv == "on")
                            {
                                rolmp.Active = true;
                                allroles = allroles + ",AR";
                            }
                            else
                                rolmp.Active = false;
                        }
                        if (item.Process == "mesgbrd")
                        {
                            rolmp = dbTemp.RoleMappings.Single(i => i.ID == item.ID);
                            if (mesgbrd == "on")
                            {
                                rolmp.Active = true;
                                allroles = allroles + ",MB";
                            }
                            else
                                rolmp.Active = false;
                        }
                        //-----------------
                        dbTemp.Entry(rolmp).State = EntityState.Modified;
                        dbTemp.SaveChanges();//Saving rollmapings table changes;
                    }
                    ////----------------- User ActivitiesLogs------
                    UserMasterActivity uma = new UserMasterActivity();
                    uma.Action = "User Modified";
                    uma.ActionBy = uid;
                    uma.Actiondate = DateTime.Now;
                    uma.UserId = usermaster.ID;
                    uma.comments = "Roles[" + allroles + "]";
                    ulc.UserMasterActivities.Add(uma);
                    // ----------------------------------END-------

                    // --------- Deleting old domain maping----------
                    //UserAflatoonDbContext dbuserDomMap = new UserAflatoonDbContext();
                    //var userDomMap = (from d in adc.UserDomainMappings
                    //                  where d.User_ID == usermaster.ID
                    //                  select d);
                    //UserDomainMapping userDmnp = null;

                    //foreach (var it in userDomMap)
                    //{
                    //    userDmnp = dbuserDomMap.UserDomainMappings.Single(i => i.ID == it.ID);
                    //    dbuserDomMap.UserDomainMappings.Remove(userDmnp);
                    //    dbuserDomMap.SaveChanges();
                    //} SpRemoveUserAccess 



                    string Accesslevel = "";
                    if (Request.Form["accesschange"] == "1")
                    {
                        if (Request.Form["SelectedAccesslevel"] != "")
                        {
                            Accesslevel = Request.Form["SelectedAccesslevel"].ToString();

                            if (usermaster.AccessLevel == "ORG")
                                OWpro.SpRemoveUserAccess(usermaster.ID, uid, usermaster.AccessLevel, Accesslevel);

                            else if (usermaster.AccessLevel == "CUST")
                                OWpro.SpRemoveUserAccess(usermaster.ID, uid, usermaster.AccessLevel, Accesslevel);
                            else if (usermaster.AccessLevel == "DOM")
                                OWpro.SpRemoveUserAccess(usermaster.ID, uid, usermaster.AccessLevel, Accesslevel);

                            //-----------------------Insert access-----------------------

                            //----------------------------Added On 31-03-2017----------------
                            string orgnization = "";
                            string[] orgnizationlist;

                            string customer = "";
                            string[] customerlist;

                            string domainsids = "";
                            string[] domainlists;

                            if (Accesslevel == "ORG")
                            {
                                //---------------------Orgnization-Maping---------------
                                UserOrganizationMapping usrOrgMap;
                                List<int> orgids = new List<int>();
                                orgnization = Request.Form["Org"].ToString();
                                orgnizationlist = orgnization.Split(',');
                                for (int i = 0; i < orgnizationlist.Length; i++)
                                {
                                    usrOrgMap = new UserOrganizationMapping();
                                    usrOrgMap.OrganizationId = Convert.ToInt16(orgnizationlist[i]);
                                    usrOrgMap.UserId = usermaster.ID;
                                    usrOrgMap.CreatedBy = uid;
                                    usrOrgMap.CreatedOn = DateTime.Now;
                                    adc.UserOrganizationMappings.Add(usrOrgMap);
                                    orgids.Add(Convert.ToInt16(orgnizationlist[i]));

                                    ORGNIzations = ORGNIzations + "," + usrOrgMap.OrganizationId;

                                }

                                uma = new UserMasterActivity();
                                uma.Action = "User Modified";
                                uma.ActionBy = uid;
                                uma.Actiondate = DateTime.Now;
                                uma.UserId = usermaster.ID;
                                uma.comments = "ORG" + " " + ORGNIzations;
                                ulc.UserMasterActivities.Add(uma);
                                ulc.SaveChanges();
                                //-------------------Customer-Maping------------------------------
                                UserCustomerMapping usrCustMap;

                                var custlist = (from c in af.CustomerMasters
                                                // from uc in af.UserCustomerMappings
                                                where orgids.Contains(c.OrganizationId) //&& c.Id == uc.CustomerId && uc.UserId == uid
                                                select c.Id);

                                foreach (var item in custlist)
                                {
                                    usrCustMap = new UserCustomerMapping();
                                    usrCustMap.CustomerId = item;// Convert.ToInt16(orgnizationlist[i]);
                                    usrCustMap.UserId = usermaster.ID;
                                    usrCustMap.CreatedBy = uid;
                                    usrCustMap.CreatedOn = DateTime.Now;
                                    adc.UserCustomerMappings.Add(usrCustMap);

                                    Customers = Customers + "," + usrCustMap.CustomerId;
                                }

                                uma = new UserMasterActivity();
                                uma.Action = "User Modified";
                                uma.ActionBy = uid;
                                uma.Actiondate = DateTime.Now;
                                uma.UserId = usermaster.ID;
                                uma.comments = "Cust" + " " + Customers;
                                ulc.UserMasterActivities.Add(uma);
                                ulc.SaveChanges();
                                adc.SaveChanges();
                                //-----------------------DomainMaping---------
                                DomainUserMapMaster domainUsrMap;

                                var domlist = (from d in af.DomainMasters
                                               from c in af.UserCustomerMappings
                                               //    from ud in af.DomainUserMapMaster
                                               where d.CustomerId == c.CustomerId && c.UserId == usermaster.ID// && d.Id == ud.DomainId && ud.UserId == uid
                                               select new { d.Id, d.CustomerId });

                                foreach (var item in domlist)
                                {
                                    domainUsrMap = new DomainUserMapMaster();
                                    domainUsrMap.CustomerID = item.CustomerId;// Convert.ToInt16(orgnizationlist[i]);
                                    domainUsrMap.DomainId = item.Id;
                                    domainUsrMap.UserId = (int)usermaster.ID;
                                    adc.DomainUserMapMasters.Add(domainUsrMap);
                                    Domains = Domains + "," + domainUsrMap.DomainId;
                                }

                                uma = new UserMasterActivity();
                                uma.Action = "User Modified";
                                uma.ActionBy = uid;
                                uma.Actiondate = DateTime.Now;
                                uma.UserId = usermaster.ID;
                                uma.comments = "Dom" + " " + Domains;
                                ulc.UserMasterActivities.Add(uma);
                                ulc.SaveChanges();
                                adc.SaveChanges();

                            }
                            else if (Accesslevel == "CUST")
                            {

                                List<int> custids = new List<int>();
                                customer = Request.Form["CustBag"].ToString();
                                customerlist = customer.Split(',');
                                //-------------------Customer-Maping------------------------------
                                UserCustomerMapping usrCustMap;
                                for (int i = 0; i < customerlist.Length; i++)
                                {
                                    usrCustMap = new UserCustomerMapping();
                                    usrCustMap.CustomerId = Convert.ToInt16(customerlist[i]);
                                    usrCustMap.UserId = (int)usermaster.ID;
                                    usrCustMap.CreatedBy = uid;
                                    usrCustMap.CreatedOn = DateTime.Now;
                                    adc.UserCustomerMappings.Add(usrCustMap);
                                    Customers = Customers + "," + usrCustMap.CustomerId;
                                }

                                uma = new UserMasterActivity();
                                uma.Action = "User Modified";
                                uma.ActionBy = uid;
                                uma.Actiondate = DateTime.Now;
                                uma.UserId = usermaster.ID;
                                uma.comments = "Cust" + " " + Customers;
                                ulc.UserMasterActivities.Add(uma);
                                ulc.SaveChanges();
                                adc.SaveChanges();
                                //-----------------------DomainMaping---------
                                DomainUserMapMaster domainUsrMap;

                                var domlist = (from d in af.DomainMasters
                                               from c in af.UserCustomerMappings
                                               // from ud in af.DomainUserMapMaster
                                               where d.CustomerId == c.CustomerId && c.UserId == usermaster.ID //&& d.Id == ud.DomainId && ud.UserId == uid
                                               select new { d.Id, d.CustomerId });

                                foreach (var item in domlist)
                                {
                                    domainUsrMap = new DomainUserMapMaster();
                                    domainUsrMap.CustomerID = item.CustomerId;// Convert.ToInt16(orgnizationlist[i]);
                                    domainUsrMap.DomainId = item.Id;
                                    domainUsrMap.UserId = usermaster.ID;
                                    adc.DomainUserMapMasters.Add(domainUsrMap);
                                    Domains = Domains + "," + domainUsrMap.DomainId;
                                }
                                uma = new UserMasterActivity();
                                uma.Action = "User Modified";
                                uma.ActionBy = uid;
                                uma.Actiondate = DateTime.Now;
                                uma.UserId = usermaster.ID;
                                uma.comments = "Dom" + " " + Domains;
                                ulc.UserMasterActivities.Add(uma);
                                ulc.SaveChanges();
                                adc.SaveChanges();

                            }
                            else if (Accesslevel == "DOM")
                            {

                                List<int> domns = new List<int>();
                                domainsids = Request.Form["Dom"].ToString();
                                domainlists = domainsids.Split(',');
                                for (int i = 0; i < domainlists.Length; i++)
                                {
                                    domns.Add(Convert.ToInt16(domainlists[i]));
                                }
                                //-----------------------DomainMaping---------
                                DomainUserMapMaster domainUsrMap;

                                var domlist = (from d in af.DomainMasters
                                               where domns.Contains(d.Id)
                                               select new { d.Id, d.CustomerId });

                                foreach (var item in domlist)
                                {
                                    domainUsrMap = new DomainUserMapMaster();
                                    domainUsrMap.CustomerID = item.CustomerId;// Convert.ToInt16(orgnizationlist[i]);
                                    domainUsrMap.DomainId = item.Id;
                                    domainUsrMap.UserId = usermaster.ID;
                                    adc.DomainUserMapMasters.Add(domainUsrMap);
                                    Domains = Domains + "," + domainUsrMap.DomainId;
                                }
                                uma = new UserMasterActivity();
                                uma.Action = "User Modified";
                                uma.ActionBy = uid;
                                uma.Actiondate = DateTime.Now;
                                uma.UserId = usermaster.ID;
                                uma.comments = "Dom" + " " + Domains;
                                ulc.UserMasterActivities.Add(uma);

                                ulc.SaveChanges();
                                adc.SaveChanges();
                            }

                            usermaster.AccessLevel = Accesslevel;
                        }

                    }
                    //--------------------userManagment-------
                    usermaster.L1StartLimit = l1frm;
                    usermaster.L1StopLimit = l1to;
                    usermaster.L2StartLimit = l2frm;
                    usermaster.L2StopLimit = l2to;
                    usermaster.L3StartLimit = l3frm;
                    usermaster.L3StopLimit = l3to;
                    usermaster.Title = title;
                    usermaster.AppSecPolicieID = pol.ID;
                    usermaster.City = location;
                    usermaster.UsertType = verlevl;


                    usermaster.ModifedBy = Convert.ToInt16(Session["uid"]);

                    adc.Entry(usermaster).State = EntityState.Modified;
                    adc.SaveChanges();

                    //------------------------------------------------------------

                    //---------------

                    //---------- Inserting DomainMAping table --------
                    //UserDomainMapping udmp = new UserDomainMapping();
                    //string domainlist = Request.Form.Get("hddomain");
                    //string alldomain = Request.Form["vernc"];
                    //if (alldomain == "All")
                    //{
                    //    var query = (from a in adc.UserDomainMappings
                    //                 from d in adc.Domains
                    //                 orderby a.ID
                    //                 where a.Domain_ID == d.ID && a.User_ID == uid
                    //                 select new { d.ID }).ToList();
                    //    foreach (var m in query)
                    //    {
                    //        UserMaster usm = dbuserDomMap.UserMasters.Find(usermaster.ID);
                    //        Domain dom = dbuserDomMap.Domains.Find(m.ID);
                    //        udmp.Domain_ID = dom.ID;
                    //        udmp.User_ID = usm.ID;
                    //        dbuserDomMap.UserDomainMappings.Add(udmp);
                    //        dbuserDomMap.SaveChanges();
                    //    }
                    //}
                    //else
                    //{
                    //    if (domainlist != null)
                    //    {
                    //        string[] domains = domainlist.Split(',');
                    //        if (domains.Length > 1)
                    //        {
                    //            for (int j = 1; j <= domains.Length - 1; j++)
                    //            {
                    //                string tempdomain = domains[j];
                    //                Domain dom = dbuserDomMap.Domains.Single(a => a.DomainName == tempdomain);
                    //                UserMaster usm = dbuserDomMap.UserMasters.Find(usermaster.ID);
                    //                udmp.Domain_ID = dom.ID;
                    //                udmp.User_ID = usm.ID;
                    //                dbuserDomMap.UserDomainMappings.Add(udmp);
                    //                dbuserDomMap.SaveChanges();

                    //            }
                    //        }
                    //    }
                    //}
                    //----------------------
                    return RedirectToAction("Index", new { id = 5 });
                }
                //-------------User Type------------
                var qry = from a in af.UsertTpeMasters
                          orderby a.UserType

                          select new { a.UserType };
                ViewBag.Vlevel = qry.Select(a => a.UserType);
                return View(usermaster);
            }
            catch (Exception e)
            {

                //ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                //er.ErrorMessage = e.InnerException.Message;
                //return View("Error", er);
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }

        //
        // GET: /CreateUser/Delete/5
        [HttpPost]
        public ActionResult Delete(UserMaster usermaster)
        {
            if (Session["domainid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            //UserMaster usermaster = db.Users.Find(id);
            if (usermaster == null)
            {
                return HttpNotFound();
            }
            var query = (from a in adc.UserMasters
                         where a.ID == usermaster.ID
                         select a).SingleOrDefault();

            try
            {
                if (query != null)
                {
                    query.UserDeleted = true;
                }
                //----------------- User ActivitiesLogs------
                UserMasterActivity uma = new UserMasterActivity();
                uma.Action = "User Deleted";
                uma.ActionBy = uid;
                uma.Actiondate = DateTime.Now;
                uma.UserId = usermaster.ID;
                // uma.comments = "Roles[" + allroles + "]";
                ulc.UserMasterActivities.Add(uma);
                //----------------------------------END-------
                adc.SaveChanges();
                ulc.SaveChanges();
            }
            catch (Exception e)
            {

                //ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                //er.ErrorMessage = e.InnerException.Message;
                //return View("Error", er);
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
            //return View(usermaster);
            return RedirectToAction("Index", new { id = 7 });
        }

        //
        // POST: /CreateUser/Delete/5

        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    UserMaster usermaster = db.Users.Find(id);
        //    // db.Users.Remove(usermaster);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        public ActionResult selecteduser(int id = 0)
        {
            if (Session["domainid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["UserManagment"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = adc.UserMasters.Find(uid);
                usrm.Active = false;
                adc.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            UserMaster usermaster = adc.UserMasters.Find(id);
            if (usermaster == null)
            {
                return HttpNotFound();
            }
            return View(usermaster);

        }
        // Partial Vew----------------
        [HttpPost]
        public PartialViewResult InsertUser(string LoginID)
        {

            var result = from a in adc.UserMasters
                         where a.LoginID == LoginID
                         select a;
            if (result.Count() != 0)
            {
                ViewBag.result = true;
                ViewBag.message = "This user id already exists!";
                return PartialView("_CreateUser");
            }
            else
            {
                ViewBag.result = false;
                ViewBag.message = "User created successfully!";
                return PartialView("_CreateUser");
            }

        }
        public ActionResult SelectBranches()
        {
            if (Session["domainid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            return View();
        }
        public PartialViewResult _RolProcess(int id = 0)
        {
            var selectmodel = (from b in adc.RoleMappings

                               orderby b.ID
                               where b.UserID == id
                               select new commonUserCreate
                               {
                                   process = b.Process,//
                                   active = b.Active

                               }).ToList();
            if (selectmodel.Count() == 0)
                ViewBag.DGDetails = false;
            else
                ViewBag.DGDetails = true;
            return PartialView("_RoleProcess", selectmodel);
        }

        public ActionResult ChangePassword()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            //if (Session["uid"] == null)
            //{
            //    ErrorDisplay er = new ErrorDisplay();
            //    ViewBag.Error = "Oops!!";
            //    return View("Error", er);
            //}
            int uid = (int)Session["uid"];
            // FormsAuthentication.SetAuthCookie(Session["fname"].ToString(), false);
            try
            {
                changePassword chanpd = new changePassword();
                int tempid = (int)Session["uid"];
                chanpd = (from u in adc.UserMasters
                          where u.ID == tempid
                          select new changePassword
                          {
                              UserId = u.LoginID,
                              loginUsrid = u.ID,
                              policyid = u.AppSecPolicieID
                              //flg=u.loginFlg
                          }).SingleOrDefault();
                //userselect.firstlogin = 1;
                //if (id == 2)
                //    ModelState.AddModelError("", "Oops! Your password has been expired !");
                //else if(id==2)
                //     ModelState.AddModelError("", "Oops! Your password has been expired !");

                return View(chanpd);
            }
            catch (Exception e)
            {
                string format = "yyyy-MM-dd hh:mm:ss";
                LoginLogoutAudit linlout = new LoginLogoutAudit();
                DateTime sdat = Convert.ToDateTime(Session["logintime"]);

                int suid = Convert.ToInt32(Session["uid"]);
                var usmtr = adc.UserMasters.Where(u => u.ID == suid).SingleOrDefault();
                if (usmtr != null)
                {
                    //usmtr.sessionid = Session.SessionID;
                    usmtr.LastLogin = Convert.ToDateTime(DateTime.Now.ToString(format));
                    usmtr.loginFlg = 0;
                }

                linlout = ulc.LoginLogoutAudits.Where(l => ((l.LoginDateTime == sdat) && (l.User_ID == suid))).SingleOrDefault();
                if (linlout != null)
                    linlout.LogoutDateTime = Convert.ToDateTime(DateTime.Now.ToString(format));

                ulc.SaveChanges();//------------For User Logs
                adc.SaveChanges();//---------For User Tables
                FormsAuthentication.SignOut();

                if (Session.IsCookieless == false)
                {
                    Session.Clear();
                    Session.Abandon();
                    Session.RemoveAll();
                    Session.SessionID.Remove(0);
                }
                ErrorDisplay er = new ErrorDisplay();
                er.ErrorMessage = e.Message;
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }

        }

        [HttpPost]
        public ActionResult ChangePassword(changePassword cngpswd, Int16 tempfl, string btn)
        {
            //if (Session["uid"] == null)
            //{
            //    return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            //}
            // flg=3-> userid and password is same
            // flg=2-> last 5 password should not be same
            // flg=1-> successfully ypdated
            // flg=5-> successfully ypdated
            //if (Session["uid"] == null)
            //{
            //    ErrorDisplay er = new ErrorDisplay();
            //    ViewBag.Error = "Oops!!";
            //    return View("Error", er);
            //}
            //int uid = (int)Session["uid"];
            try
            {
                if (btn == "Cancel")
                    return RedirectToAction("IWIndex", "Home");

                CommonFunction cmf = new CommonFunction();
                PasswordHistory pswhtr = new PasswordHistory();
                UserMaster user = new UserMaster();
                //----------------------------------------  Check Password Validation -----------------------------//

                bool result = false;
                bool isDigit = false;
                bool isLetter = false;
                bool isLowerChar = false;
                bool isUpperChar = false;
                bool isNonAlpha = false;
                //--------------------- Check From AppSecPolicies --------------------//
                var policy = (from a in adc.AppSecPolicies
                              where a.ID == cngpswd.policyid
                              select new PolicyDetails
                              {
                                  policyname = a.Name,
                                  pwdexprydat = a.PwdExpiryDays,
                                  minpwdlength = a.MinPwdLength,
                                  maxpwdlength = a.MaxPwdLength,
                                  Aplhanumericmadate = a.AlphanumericMandatory,
                                  Specialcharmandate = a.SpecialCharMandatory,
                                  Invalidattamtallow = a.InvalidAttemptsAllowed,
                                  Deactivationdays = a.DeactivationDays
                              }).SingleOrDefault();
                //------------- Checking Length of password ------------------------//
                if (cngpswd.NewPassword.Length < policy.minpwdlength || cngpswd.NewPassword.Length > policy.maxpwdlength)
                    ModelState.AddModelError("", "Oops! Your minimum password length should be " + policy.minpwdlength + " and maximum password length should be " + policy.maxpwdlength + "!");

                foreach (char c in cngpswd.NewPassword)
                {
                    if (char.IsDigit(c))
                        isDigit = true;
                    if (char.IsLetter(c))
                    {
                        isLetter = true;
                        if (char.IsLower(c))
                            isLowerChar = true;
                        if (char.IsUpper(c))
                            isUpperChar = true;
                    }
                    Match m = Regex.Match(c.ToString(), @"\W|_");
                    if (m.Success)
                        isNonAlpha = true;
                }

                if (isDigit && isLetter && isLowerChar && isUpperChar && isNonAlpha)
                    result = true;

                //------------------- Old Password new password should not be same !--------------//
                if (result == false)
                {
                    ModelState.AddModelError("", "Oops! Password should contain alphanumeric and one special charater and one capital letter!");
                    return View(cngpswd);
                }

                //---------------------------------------- End ----------------------------------------------------//
                //--- If Old password incorrect---------
                string temppasswd = cmf.EncryptPassword(cngpswd.OldPassword);
                user = adc.UserMasters.Where(u => (u.ID == cngpswd.loginUsrid && u.Password == temppasswd)).SingleOrDefault();
                if (user == null)
                {
                    if (tempfl < 4)
                    {
                        cngpswd.flg = 4;
                        ModelState.AddModelError("", "Oops! Old Password is Wrong!");
                        return View(cngpswd);
                    }
                    if (tempfl == 4)
                    {
                        cngpswd.flg = 4 + 1;
                        ModelState.AddModelError("", "Oops! Old Password is Wrong!");
                        return View(cngpswd);
                    }
                    if (tempfl > 4)
                    {
                        string format = "yyyy-MM-dd hh:mm:ss";
                        LoginLogoutAudit linlout = new LoginLogoutAudit();
                        DateTime sdat = Convert.ToDateTime(Session["logintime"]);

                        // int suid = Convert.ToInt32(Session["uid"]);
                        int suid = cngpswd.loginUsrid;
                        var usmtr = adc.UserMasters.Where(u => u.ID == suid).SingleOrDefault();
                        if (usmtr != null)
                        {
                            //usmtr.sessionid = Session.SessionID;
                            usmtr.LastLogin = Convert.ToDateTime(DateTime.Now.ToString(format));
                            usmtr.loginFlg = 0;
                        }

                        linlout = ulc.LoginLogoutAudits.Where(l => ((l.LoginDateTime == sdat) && (l.User_ID == suid))).SingleOrDefault();
                        if (linlout != null)
                            linlout.LogoutDateTime = Convert.ToDateTime(DateTime.Now.ToString(format));

                        adc.SaveChanges();
                        ulc.SaveChanges();
                        FormsAuthentication.SignOut();

                        if (Session.IsCookieless == false)
                        {
                            Session.Clear();
                            Session.Abandon();
                            Session.RemoveAll();
                            Session.SessionID.Remove(0);
                        }
                        return RedirectToAction("Index", "Login");
                    }
                }

                //--If UserId And password is same--------------

                if (cngpswd.UserId == cngpswd.NewPassword)
                {
                    cngpswd.flg = 3;
                    ModelState.AddModelError("", "Oops! User id and Password should not be same!");
                    return View(cngpswd);
                }
                //-------Old Password And New Password Shld not be same---------------
                if (cngpswd.OldPassword == cngpswd.NewPassword)
                {
                    cngpswd.flg = 3;
                    ModelState.AddModelError("", "Oops! Old Password and New Password should not be same!");
                    return View(cngpswd);
                }

                var lastpaswrd = (from ph in ulc.PasswordHistories
                                  orderby ph.Password descending
                                  where ph.User_ID == cngpswd.loginUsrid
                                  select ph).ToList();
                if (lastpaswrd.Count != 0)
                {
                    int y = 1;
                    foreach (var p in lastpaswrd)
                    {
                        if (cmf.DecryptPassword(p.Password) == cngpswd.NewPassword)
                        {
                            cngpswd.flg = 2;
                            ModelState.AddModelError("", "Oops! Last five password should not be same!");
                            return View(cngpswd);
                        }
                        if (y == 5)
                            break;

                        y = y + 1;
                    }
                    pswhtr.Password = user.Password;
                    pswhtr.User_ID = user.ID;
                    pswhtr.PassworChangeDate = DateTime.Now;
                    pswhtr.PasswordChangedBy_ID = user.ID;

                    ulc.PasswordHistories.Add(pswhtr);

                    user.Password = cmf.EncryptPassword(cngpswd.NewPassword);
                    //if (cngpswd.firstlogin == 1)
                    user.FirstLogin = false;
                    string format1 = "yyyy-MM-dd hh:mm:ss";
                    user.LastLogin = Convert.ToDateTime(DateTime.Now.ToString(format1));
                    user.loginFlg = 0;
                    adc.SaveChanges();
                    ulc.SaveChanges();
                    FormsAuthentication.SignOut();
                    Session.Abandon();
                    return RedirectToAction("Index", "Login", new { id = 3 });
                }
                else
                {
                    pswhtr.Password = user.Password;
                    pswhtr.User_ID = user.ID;
                    pswhtr.PassworChangeDate = DateTime.Now;
                    pswhtr.PasswordChangedBy_ID = user.ID;

                    ulc.PasswordHistories.Add(pswhtr);

                    user.Password = cmf.EncryptPassword(cngpswd.NewPassword);
                    // if (cngpswd.firstlogin == 1)
                    user.FirstLogin = false;

                    string format = "yyyy-MM-dd hh:mm:ss";
                    LoginLogoutAudit linlout = new LoginLogoutAudit();
                    DateTime sdat = Convert.ToDateTime(Session["logintime"]);

                    // int suid = Convert.ToInt32(Session["uid"]);
                    int suid = cngpswd.loginUsrid;
                    var usmtr = adc.UserMasters.Where(u => u.ID == suid).SingleOrDefault();
                    if (usmtr != null)
                    {
                        //usmtr.sessionid = Session.SessionID;
                        usmtr.LastLogin = Convert.ToDateTime(DateTime.Now.ToString(format));
                        usmtr.loginFlg = 0;
                    }

                    linlout = ulc.LoginLogoutAudits.Where(l => ((l.LoginDateTime == sdat) && (l.User_ID == suid))).SingleOrDefault();
                    if (linlout != null)
                        linlout.LogoutDateTime = Convert.ToDateTime(DateTime.Now.ToString(format));

                    //db.SaveChanges();
                    FormsAuthentication.SignOut();
                    Session.Abandon();
                    if (Session.IsCookieless == false)
                    {
                        Session.Clear();
                        Session.Abandon();
                        Session.RemoveAll();
                        Session.SessionID.Remove(0);
                    }
                    //----------------- User ActivitiesLogs------
                    UserMasterActivity uma = new UserMasterActivity();
                    uma.Action = "User Password Change";
                    uma.ActionBy = cngpswd.loginUsrid;
                    uma.Actiondate = DateTime.Now;
                    uma.UserId = cngpswd.loginUsrid;
                    // uma.comments = "Roles[" + allroles + "]";
                    ulc.UserMasterActivities.Add(uma);
                    //----------------------------------END-------
                    ulc.SaveChanges();
                    adc.SaveChanges();
                    FormsAuthentication.SignOut();
                    Session.Abandon();
                    return RedirectToAction("Index", "Login", new { id = 3 });
                }
            }
            catch (Exception e)
            {

                //ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                //er.ErrorMessage = e.InnerException.Message;
                //return View("Error", er);
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }
        [HttpPost]
        public ActionResult Disable(UserMaster usermaster)
        {
            if (Session["domainid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            //if (Session["uid"]==null)
            //{
            //     ErrorDisplay er = new ErrorDisplay();
            //    er.ErrorMessage = "Services not available!";
            //    return View("Error", er); 
            //}
            int sid = (int)Session["uid"];
            if (sid == usermaster.ID)//------If user login and disable user is same
                return RedirectToAction("Index", new { id = 6 });

            //UserMaster usermaster = db.Users.Find(id);
            if (usermaster == null)
            {
                return HttpNotFound();
            }
            var query = (from a in adc.UserMasters
                         where a.ID == usermaster.ID
                         select a).SingleOrDefault();

            try
            {
                if (query != null)
                {
                    if (query.Active == true)
                        query.Active = false;
                    else
                        query.Active = true;
                    query.LastLogin = null;
                }
                //----------------- User ActivitiesLogs------
                UserMasterActivity uma = new UserMasterActivity();
                if (query.Active == true)
                    uma.Action = "User Enable";
                else
                    uma.Action = "User Disable";

                uma.ActionBy = sid;
                uma.Actiondate = DateTime.Now;
                uma.UserId = usermaster.ID;
                // uma.comments = "Roles[" + allroles + "]";
                ulc.UserMasterActivities.Add(uma);
                //----------------------------------END-------
                adc.SaveChanges();
                ulc.SaveChanges();
            }
            catch (Exception e)
            {

                //ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                //er.ErrorMessage = e.InnerException.Message;
                //return View("Error", er);
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
            return RedirectToAction("Index", new { id = 2 });
        }
        [HttpPost]
        public ActionResult PasswordReset(UserMaster usermast, string btn = null)
        {
            if (Session["domainid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["UserManagment"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = adc.UserMasters.Find(uid);
                usrm.Active = false;
                adc.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            try
            {
                int seeionuid = (int)Session["uid"];
                UserMaster usermaster = adc.UserMasters.Find(usermast.ID);
                if (usermaster == null)
                {
                    return HttpNotFound();
                }
                if (btn == "Unlock")
                {
                    Unlock(usermaster);
                    return RedirectToAction("Index", new { id = 4 });

                }
                else if (btn == "Disable" || btn == "Enable")
                {
                    Disable(usermaster);
                    return RedirectToAction("Index", new { id = 2 });
                }
                else if (btn == "Edit")
                {
                    // Edit(usermaster.ID);
                    return RedirectToAction("Edit", new { id = usermaster.ID });
                }
                else if (btn == "Delete")
                {
                    Delete(usermaster);
                    return RedirectToAction("Index", new { id = 7 });
                }
                else
                {
                    var query = (from a in adc.UserMasters
                                 where a.ID == usermaster.ID
                                 select a).SingleOrDefault();

                    if (query != null)
                    {
                        CommonFunction cmf = new CommonFunction();
                        PasswordHistory pswhtr = new PasswordHistory();
                        pswhtr.Password = usermaster.Password;
                        pswhtr.User_ID = usermaster.ID;
                        pswhtr.PassworChangeDate = DateTime.Now;
                        if (seeionuid == usermaster.ID)
                            pswhtr.PasswordChangedBy_ID = usermaster.ID;
                        else
                        {
                            //var sesionid = adc.UserMasters.Where(m => (m.ID == seeionuid)).SingleOrDefault();
                            pswhtr.PasswordChangedBy_ID = seeionuid;
                        }



                        usermaster.Password = cmf.EncryptPassword("newuser@123");
                        usermaster.FirstLogin = true;
                        usermaster.InvalidPasswordAttempts = 0;
                        usermaster.LastLogin = DateTime.Now;
                        ulc.PasswordHistories.Add(pswhtr);
                        //----------------- User ActivitiesLogs------
                        UserMasterActivity uma = new UserMasterActivity();

                        uma.Action = "User Password Reset";
                        uma.ActionBy = seeionuid;
                        uma.Actiondate = DateTime.Now;
                        uma.UserId = usermaster.ID;
                        // uma.comments = "Roles[" + allroles + "]";
                        ulc.UserMasterActivities.Add(uma);
                        //----------------------------------END-------
                        ulc.SaveChanges();
                        adc.SaveChanges();
                        return RedirectToAction("Index", new { id = 3 });
                    }
                }


            }
            catch (Exception e)
            {

                //ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                //er.ErrorMessage = e.InnerException.Message;
                //return View("Error", er);
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
            //return View(usermaster);
            return RedirectToAction("Index", new { id = 3 });
        }
        public PartialViewResult PolicyDetails(string name, int id = 0)
        {
            if (name == null)
            {
                //int sid=(int)Session["uid"];
                var pol = adc.AppSecPolicies.Where(m => m.ID == id).SingleOrDefault();
                name = pol.Name;
            }
            var policy = (from a in adc.AppSecPolicies
                          where a.Name == name
                          select new PolicyDetails
                          {
                              policyname = a.Name,
                              pwdexprydat = a.PwdExpiryDays,
                              minpwdlength = a.MinPwdLength,
                              maxpwdlength = a.MaxPwdLength,
                              Aplhanumericmadate = a.AlphanumericMandatory,
                              Specialcharmandate = a.SpecialCharMandatory,
                              Invalidattamtallow = a.InvalidAttemptsAllowed,
                              Deactivationdays = a.DeactivationDays
                          }).SingleOrDefault();


            return PartialView("_PolicyDetails", policy);
        }
        [HttpPost]

        public ActionResult Unlock(UserMaster usermaster)
        {
            if (Session["domainid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            //UserMaster usermaster = db.Users.Find(id);
            if (usermaster == null)
            {
                return HttpNotFound();
            }
            var query = (from a in adc.UserMasters
                         where a.ID == usermaster.ID
                         select a).SingleOrDefault();
            try
            {
                int seeionuid = (int)Session["uid"];
                if (query != null)
                    query.loginFlg = 0;
                //----------------- User ActivitiesLogs------
                UserMasterActivity uma = new UserMasterActivity();
                uma.Action = "User Unlock";
                uma.ActionBy = seeionuid;
                uma.Actiondate = DateTime.Now;
                uma.UserId = usermaster.ID;
                // uma.comments = "Roles[" + allroles + "]";
                ulc.UserMasterActivities.Add(uma);
                //----------------------------------END-------
                ulc.SaveChanges();
            }
            catch (Exception e)
            {

                //ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                //er.ErrorMessage = e.InnerException.Message;
                //return View("Error", er);
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
            return RedirectToAction("Index", new { id = 4 });
        }
        public ActionResult MyAccount()
        {
            if (Session["domainid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            try
            {
                int uid = (int)Session["uid"];
                var usr = adc.UserMasters.Where(m => m.ID == uid).SingleOrDefault();
                return View(usr);
            }
            catch (Exception)
            {
                ErrorDisplay er = new ErrorDisplay();
                er.ErrorMessage = "Services not available!";
                return View("Error", er);
            }
        }
        [HttpPost]
        public ActionResult MyAccount(UserMaster userm, string btn)
        {
            if (Session["domainid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            try
            {
                if (btn == "Cancel")
                    return RedirectToAction("Index", "Home");

                var upuser = adc.UserMasters.Where(m => m.ID == userm.ID).SingleOrDefault();
                upuser.FirstName = userm.FirstName;
                upuser.LastName = userm.LastName;
                upuser.Title = userm.Title;
                upuser.EmailID = userm.EmailID;
                upuser.DOB = userm.DOB;
                adc.SaveChanges();
            }
            catch (Exception)
            {
                ErrorDisplay er = new ErrorDisplay();
                er.ErrorMessage = "Services not available!";
                return View("Error", er);
            }
            return RedirectToAction("Index", new { id = 5 });
        }

        protected override void Dispose(bool disposing)
        {
            adc.Dispose();
            ulc.Dispose();
            base.Dispose(disposing);
        }
    }
}