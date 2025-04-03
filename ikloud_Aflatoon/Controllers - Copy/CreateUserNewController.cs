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
using NPOI.HPSF;
using NLog;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

namespace ikloud_Aflatoon.Controllers
{
    public class CreateUserNewController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        AflatoonEntities af = new AflatoonEntities();
        //IWProcDataContext iwpro = new IWProcDataContext();
        //OWProcDataContext OWpro = new OWProcDataContext();
        UserAflatoonDbContext adc = new UserAflatoonDbContext();
        UserAflatoonDbContext dbTemp;
        UserLogDbContext ulc = new UserLogDbContext();
        CommonFunction cmf = new CommonFunction();
        DatabaseQuery dbq = new DatabaseQuery();

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
        string[] rollname = new string[] { "UserManagment", "RejectRepair", "DE", "QC", "VF", "Report", "fildwnd", "Ds", "Query", "QueryMod", "sod", "master", "settg", "archv", "mesgbrd", "chirjct", "RVF", "CCPH", "RVF4" };
        string allroles = "";
        string ORGNIzations = "";
        string Customers = "";
        string Domains = "";
        //
        //
        // GET: /CreateUser/

        public ActionResult IndexChecker(int id = 0, string password = null)
        {   
            try
            {
                UserMaster usrm;
                if (Session["uid"] == null)
                {
                    return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
                }
                int uid = (int)Session["uid"];
                if ((bool)Session["UserManagementChecker"] == false)
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
                                  where a.UserDeleted == false && (a.IsActive == 0 || a.IsActive == 2 || a.IsActive == 4) //&& a.UsertType == usrm.UsertType
                                  select a).ToList();
                    }
                    else
                    {
                        usmstr = (from a in adc.UserMasters
                                  where a.UserDeleted == false && a.UsertType == usrm.UsertType && (a.IsActive == 0 || a.IsActive == 2 || a.IsActive == 4)
                                  select a).ToList();
                    }
                    return View(usmstr);
                }
                else
                {
                    ViewBag.disable = true;

                    if (id == 2)
                        ViewBag.mesg = "User successfully approved !";
                    else if (id == 4)
                        ViewBag.mesg = "User successfully unlocked !";
                    else if (id == 5)
                        ViewBag.mesg = "User successfully updated !";
                    else if (id == 6)
                        ViewBag.mesg = "You can not approved yourself !";
                    else if (id == 7)
                        ViewBag.mesg = "User has been marked delete  !";

                    List<UserMaster> usmstr = new List<UserMaster>();

                    if (usrm.UsertType == "Developer_User")
                    {
                        usmstr = (from a in adc.UserMasters
                                  where a.UserDeleted == false && (a.IsActive == 0 || a.IsActive == 2) //&& a.UsertType == usrm.UsertType
                                  select a).ToList();
                    }
                    else
                    {
                        usmstr = (from a in adc.UserMasters
                                  where a.UserDeleted == false && a.UsertType == usrm.UsertType && (a.IsActive == 0 || a.IsActive == 2)
                                  select a).ToList();
                    }

                    //var query = from a in adc.UserMasters
                    //            where a.UserDeleted == false
                    //            select a;
                    return View(usmstr);
                }
                //else
                //{
                //    List<UserMaster> usmstr = new List<UserMaster>();

                //    if (usrm.UsertType == "Developer_User")
                //    {
                //        usmstr = (from a in adc.UserMasters
                //                  where a.UserDeleted == false && a.IsActive == 0 //&& a.UsertType == usrm.UsertType
                //                  select a).ToList();
                //    }
                //    else
                //    {
                //        usmstr = (from a in adc.UserMasters
                //                  where a.UserDeleted == false && a.UsertType == usrm.UsertType && a.IsActive == 0
                //                  select a).ToList();
                //    }
                //    //var query = from a in adc.UserMasters
                //    //            where a.UserDeleted == false //&& a.UsertType==
                //    //            select a;
                //    ViewBag.modf = true;

                //    return View(usmstr);
                //}

                
            }
            catch(Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                logger.Log(LogLevel.Error, "ChangePassword|" + message + "INNEREXP| " + innerExcp, "Login Index-Msg");
                // logger.Log(LogLevel.Error, e.InnerException.StackTrace, "Login Index-Exception");

                return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });
            }
        }

        public ActionResult Index(int id = 0, string password = null)
        {
            //string ih = Session["asds"].ToString();
            //Session["uid"] = 1;
            try
            {


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
                    {
                        ViewBag.mesg = "User has been reset successfully !";
                        ViewBag.pass = password;
                    }

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
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                logger.Log(LogLevel.Error, "ChangePassword|" + message + "INNEREXP| " + innerExcp, "Login Index-Msg");
                // logger.Log(LogLevel.Error, e.InnerException.StackTrace, "Login Index-Exception");

                return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });
            }

        }

        //
        // GET: /CreateUser/Details/5

        public ActionResult Details(int id = 0)
        {
            //if (Session["domainid"] == null)
            //{
            //    return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            //}
            UserMaster usermaster = adc.UserMasters.Find(id);
            if (usermaster == null)
            {
                return HttpNotFound();
            }
            return View(usermaster);
        }

        //
        // GET: /CreateUser/Create

        public JsonResult SelectUserRole()
        {
            List<string> customerlist = new List<string>();
            try
            {
                int uid = (int)Session["uid"];
                //int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());

                //---------------------------------------Role Master------------------------
                //var roleMasters = af.RoleMaster.ToList();
                var roleMasters = (from a in af.RoleMaster
                                   where a.IsActive == 1
                                   orderby a.RoleName
                                   select new
                                   {
                                       a.ID,
                                       a.RoleName
                                   }).ToList();

                ViewBag.RoleMasters = roleMasters;
                //---------------------------------------Vendor Master------------------------END--

                //if (domainId == 0)
                //{
                //    var result1 = (from a in udb.ScanningNodeMasters

                //                   select new
                //                   {
                //                       a.Id,
                //                       a.MACIP
                //                       //BranchCodeName = a.BranchCode + " (" + a.BranchName + ")"
                //                   }).ToList();
                //    return Json(result1, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                //    var result = (from a in udb.ScanningNodeMasters

                //                  where a.DomainId == domainId
                //                  select new
                //                  {
                //                      a.Id,
                //                      a.MACIP
                //                      //BranchCodeName = a.BranchCode + " (" + a.BranchName + ")"
                //                  }).ToList();

                //    return Json(result, JsonRequestBehavior.AllowGet);
                //}

                return Json(roleMasters, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Create(int id = 0, string pass = null)
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
                ViewBag.temppassword = pass;
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
                    if (usrm.UsertType == "Developer_User" || usrm.UsertType == "Admin_user")
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
                    string message = "";
                    string innerExcp = "";
                    if (e.Message != null)
                        message = e.Message.ToString();
                    if (e.InnerException != null)
                        innerExcp = e.InnerException.Message;

                    logger.Log(LogLevel.Error, "ChangePassword|" + message + "INNEREXP| " + innerExcp, "Login Index-Msg");
                    // logger.Log(LogLevel.Error, e.InnerException.StackTrace, "Login Index-Exception");

                    return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });
                    //return View("Error", er);
                }
            }
            //return View();
        }

        public class VendorCodeList
        {
            public string VendorCode { get; set; }
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

            //var city = from c in af.Cities
            //           orderby c.CityName
            //           select new { c.CityName };
            //============ Changes done on 06/05/2023 for showing domain name in location dll =======================

            var city = from c in af.DomainMaster
                       orderby c.Name
                       select new { c.Name };

            ViewBag.city = city.Select(c => c.Name);
            //-------------User Type------------
            if (usrm.UsertType == "Developer_User" || usrm.UsertType == "Admin_user")
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
            //---------------------------------------Vendor Master------------------------
            //var Vendormaster = (from b in OWpro.VendorMasters
            //                        //where a.UserType == usrm.UsertType
            //                    orderby b.VendorCode
            //                    select new { b.VendorCode });

            SqlDataAdapter adp = new SqlDataAdapter("FetchVendorCodeList", con);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            //adp.SelectCommand.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = uid;

            DataSet ds = new DataSet();
            adp.Fill(ds);
            var objectlst = new List<VendorCodeList>();
            VendorCodeList def;

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    def = new VendorCodeList
                    {
                        VendorCode = ds.Tables[0].Rows[i].ItemArray[0].ToString(),
                    };
                    objectlst.Add(def);
                }

            }

            ViewBag.VendorCode = objectlst.Select(a => a.VendorCode);
            //---------------------------------------Vendor Master------------------------END--



            return PartialView("_EntryField", um);
        }
        public PartialViewResult _SelectDomain()
        {
            int uid = (int)Session["uid"];
            int customerId = Convert.ToInt32(Session["CustomerID"].ToString());
            //var resut = (from a in adc.DomainUserMapMasters
            //             from d in adc.Domains
            //             orderby d.ID
            //             where a.DomainId == d.ID && a.UserId == uid
            //             select new SelectDomain
            //             {
            //                 domainName = d.DomainName
            //             }).ToList();
            //var resut = (from a in adc.DomainUserMapMasters
            //             from d in adc.DomainMaster
            //             orderby a.ID
            //             where a.CustomerID == customerId && a.UserId == uid && a.DomainId == d.Id
            //             select new SelectDomain
            //             {
            //                 domainName = d.Name
            //             }).ToList();
            var resut = (from d in adc.DomainMaster
                         orderby d.Id
                         where d.CustomerId == customerId
                         select new SelectDomain
                         {
                             domainName = d.Name
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
            int customerId = Convert.ToInt32(Session["CustomerID"].ToString());
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
                var resut = (from d in af.DomainMaster
                             from c in af.CustomerMasters
                             orderby d.Id
                             where d.CustomerId == c.Id
                             select new
                             {
                                 domainName = d.Name + "-" + c.Name,
                                 Id = d.Id
                             }).ToList();

                //var resut = (from a in af.DomainUserMapMasters
                //             from d in af.DomainMaster
                //             from c in af.CustomerMasters
                //             orderby d.Id
                //             where a.DomainId == d.Id && d.CustomerId == c.Id && custids.Contains(d.CustomerId) && a.UserId == uid
                //             select new
                //             {
                //                 domainName = d.Name + "-" + c.Name,
                //                 Id = d.Id
                //             }).ToList();
                ViewBag.DOM = new SelectList(resut.AsEnumerable(), "Id", "domainName");
            }
            else
            {
                //var resut = (from a in af.DomainUserMapMasters
                //             from d in af.DomainMaster
                //             from c in af.CustomerMasters
                //             orderby d.Id
                //             where a.DomainId == d.Id && a.CustomerID == d.CustomerId && c.Id == a.CustomerID && a.UserId == uid
                //             select new
                //             {
                //                 domainName = d.Name + "-" + c.Name,
                //                 Id = d.Id
                //             }).ToList();

                var resut = (from d in af.DomainMaster
                             from c in af.CustomerMasters
                             orderby d.Id
                             where d.CustomerId == c.Id
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
                        string format = "yyyy-MM-dd hh:mm:ss";
                        string dob = Request.Form["datepicker"];
                        string title = Request.Form["title"];
                        string location = "";
                        string Accesslevel = "";

                        string vendorCode = "";
                        string selectRole = "";

                        //----------------Added By Abid 30-12-2019----------------For Vendor----
                        if (Request.Form["VendorCode"] != "")
                        {
                            vendorCode = Request.Form["VendorCode"].ToString();
                        }

                        if (Request.Form["SelectRoles"] != "")
                        {
                            selectRole = Request.Form["SelectRoles"].ToString();
                            usermaster.RoleId = Convert.ToInt16(selectRole);
                        }

                        usermaster.IsActive = 0;
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
                        //if (Request.Form["qc"] == "on")
                        //{
                        //    l1frm = Convert.ToDecimal(Request.Form["l1frm"]);
                        //    l1to = Convert.ToDecimal(Request.Form["l1to"]);
                        //}
                        //if (Request.Form["vf"] == "on")
                        //{
                        //    l2frm = Convert.ToDecimal(Request.Form["l2frm"]);
                        //    l2to = Convert.ToDecimal(Request.Form["l2to"]);

                        //}
                        //if (Request.Form["rvf"] == "on")
                        //{
                        //    l3frm = Convert.ToDecimal(Request.Form["l3frm"]);
                        //    l3to = Convert.ToDecimal(Request.Form["l3to"]);
                        //}
                        //if (Request.Form["rvf4"] == "on")
                        //{
                        //    l4frm = Convert.ToDecimal(Request.Form["l4frm"]);
                        //    l4to = Convert.ToDecimal(Request.Form["l4to"]);
                        //}
                        //usermaster.L1StartLimit = l1frm;
                        //usermaster.L1StopLimit = l1to;
                        //usermaster.L2StartLimit = l2frm;
                        //usermaster.L2StopLimit = l2to;
                        //usermaster.L3StartLimit = l3frm;
                        //usermaster.L3StopLimit = l3to;
                        //usermaster.L4StartLimit = l4frm;
                        //usermaster.L4StopLimit = l4to;

                        //-------------Added On 19-12-2018----------For Axis Requirement-----------
                        //string password = RandomPwd(usermaster.LoginID, usermaster.FirstName, usermaster.LastName);
                        //---------------------End------------------------
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
                        usermaster.LastLogin = Convert.ToDateTime(DateTime.Now.ToString(format));
                        usermaster.InvalidPasswordAttempts = 0;

                        adc.UserMasters.Add(usermaster);
                        try
                        {
                            adc.SaveChanges();
                            //------------------------------------30-12-2019---Abid for Vendor name--------------                            
                            //OWpro.updateVendorCode(usermaster.ID, vendorCode);

                            SqlCommand com = new SqlCommand("updateVendorCode", con);
                            com.CommandType = CommandType.StoredProcedure;
                            com.Parameters.AddWithValue("@uid", usermaster.ID);
                            com.Parameters.AddWithValue("@VendorCode", vendorCode);

                            con.Open();
                            com.ExecuteNonQuery();
                            con.Close();

                            //-----------------------------------    
                        }
                        catch (Exception e)
                        {
                            string message = "";
                            string innerExcp = "";
                            if (e.Message != null)
                                message = e.Message.ToString();
                            if (e.InnerException != null)
                                innerExcp = e.InnerException.Message;

                            logger.Log(LogLevel.Error, "Post-Create|" + message + "INNEREXP| " + innerExcp, "Create User");

                            return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });


                            //return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
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
                            //  adc.SaveChanges();
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
                                usrOrgMap.UserId = tempuserid;
                                usrOrgMap.CreatedBy = id;
                                usrOrgMap.CreatedOn = DateTime.Now;
                                adc.UserOrganizationMappings.Add(usrOrgMap);
                                orgids.Add(Convert.ToInt16(orgnizationlist[i]));
                                adc.SaveChanges();
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
                                adc.SaveChanges();
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
                            // adc.SaveChanges();
                            //-----------------------DomainMaping---------

                            DataSet dsdomin = new DataSet();
                            SqlDataAdapter adp = new SqlDataAdapter("SelectUserDomains", con);
                            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                            adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.BigInt).Value = 0;
                            adp.SelectCommand.Parameters.Add("@DomainID", SqlDbType.BigInt).Value = 0;
                            adp.SelectCommand.Parameters.Add("@UserID", SqlDbType.BigInt).Value = id;//usermaster.ID;

                            adp.Fill(dsdomin);
                            //  IWSearch Owsr = null;
                            if (dsdomin.Tables[0].Rows.Count > 0)
                            {
                                int index = 0;
                                int count = dsdomin.Tables[0].Rows.Count;
                                while (count > 0)
                                {
                                    //OWpro.InsertDomainUserMapMaster(Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[1]), Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[0]), (int)usermaster.ID);

                                    SqlCommand com = new SqlCommand("InsertDomainUserMapMaster", con);
                                    com.CommandType = CommandType.StoredProcedure;
                                    com.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[1]));
                                    com.Parameters.AddWithValue("@DomainID", Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[0]));
                                    com.Parameters.AddWithValue("@UserID", (int)usermaster.ID);

                                    con.Open();
                                    com.ExecuteNonQuery();
                                    con.Close();

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
                                adc.SaveChanges();
                            }

                            uma = new UserMasterActivity();
                            uma.Action = "User Created";
                            uma.ActionBy = id;
                            uma.Actiondate = DateTime.Now;
                            uma.UserId = tempuserid;
                            uma.comments = "Cust" + " " + Customers;
                            ulc.UserMasterActivities.Add(uma);
                            ulc.SaveChanges();
                            //  adc.SaveChanges();
                            //-----------------------DomainMaping---------


                            //------------------Select From Proc--------------------
                            DataSet dsdomin = new DataSet();
                            SqlDataAdapter adp = new SqlDataAdapter("SelectUserDomains", con);
                            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                            adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.BigInt).Value = 0;
                            adp.SelectCommand.Parameters.Add("@DomainID", SqlDbType.BigInt).Value = 0;
                            adp.SelectCommand.Parameters.Add("@UserID", SqlDbType.BigInt).Value = id;//usermaster.ID;

                            adp.Fill(dsdomin);
                            //  IWSearch Owsr = null;
                            if (dsdomin.Tables[0].Rows.Count > 0)
                            {
                                int index = 0;
                                int count = dsdomin.Tables[0].Rows.Count;
                                while (count > 0)
                                {
                                    //OWpro.InsertDomainUserMapMaster(Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[1]), Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[0]), (int)usermaster.ID);

                                    SqlCommand com = new SqlCommand("InsertDomainUserMapMaster", con);
                                    com.CommandType = CommandType.StoredProcedure;
                                    com.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[1]));
                                    com.Parameters.AddWithValue("@DomainID", Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[0]));
                                    com.Parameters.AddWithValue("@UserID", (int)usermaster.ID);

                                    con.Open();
                                    com.ExecuteNonQuery();
                                    con.Close();

                                    index = index + 1;
                                    count = count - 1;
                                }
                            }
                            //-------------------------
                            uma = new UserMasterActivity();
                            uma.Action = "User Created";
                            uma.ActionBy = id;
                            uma.Actiondate = DateTime.Now;
                            uma.UserId = tempuserid;
                            uma.comments = "Dom" + " " + Domains;
                            ulc.UserMasterActivities.Add(uma);

                            ulc.SaveChanges();


                        }
                        else if (Accesslevel == "DOM")
                        {

                            List<int> domns = new List<int>();
                            domainsids = Request.Form["Dom"].ToString();
                            domainlists = domainsids.Split(',');
                            for (int i = 0; i < domainlists.Length; i++)
                            {
                                //domns.Add(Convert.ToInt16(domainlists[i]));

                                DataSet dsdomin = new DataSet();
                                SqlDataAdapter adp = new SqlDataAdapter("SelectUserDomains", con);
                                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.BigInt).Value = 0;
                                adp.SelectCommand.Parameters.Add("@DomainID", SqlDbType.BigInt).Value = Convert.ToInt16(domainlists[i]);
                                adp.SelectCommand.Parameters.Add("@UserID", SqlDbType.BigInt).Value = id;

                                adp.Fill(dsdomin);
                                //  IWSearch Owsr = null;
                                if (dsdomin.Tables[0].Rows.Count > 0)
                                {
                                    int index = 0;
                                    int count = dsdomin.Tables[0].Rows.Count;
                                    while (count > 0)
                                    {
                                        //OWpro.InsertDomainUserMapMaster(Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[1]), Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[0]), (int)usermaster.ID);

                                        SqlCommand com = new SqlCommand("InsertDomainUserMapMaster", con);
                                        com.CommandType = CommandType.StoredProcedure;
                                        com.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[1]));
                                        com.Parameters.AddWithValue("@DomainID", Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[0]));
                                        com.Parameters.AddWithValue("@UserID", (int)usermaster.ID);

                                        con.Open();
                                        com.ExecuteNonQuery();
                                        con.Close();

                                        index = index + 1;
                                        count = count - 1;
                                    }
                                }
                            }
                            //-----------------------DomainMaping---------

                            //--------------------Proc----------------

                            uma = new UserMasterActivity();
                            uma.Action = "User Created";
                            uma.ActionBy = id;
                            uma.Actiondate = DateTime.Now;
                            uma.UserId = tempuserid;
                            uma.comments = "Dom" + " " + Domains;
                            ulc.UserMasterActivities.Add(uma);

                            ulc.SaveChanges();

                        }


                        //----------------- User ActivitiesLogs------

                        ViewBag.result = false;
                        // ViewBag.temppassword = "Password is:" + password;
                        return RedirectToAction("Create", "CreateUserNew", new { id = 1, pass = password });
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
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                logger.Log(LogLevel.Error, "Create-Post|" + message + "INNEREXP| " + innerExcp, "Login Index-Msg");
                // logger.Log(LogLevel.Error, e.InnerException.StackTrace, "Login Index-Exception");

                FormsAuthentication.SignOut();

                return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });
                // return RedirectToAction("Error", "Error", new { msg = message, popmsg = "CMS HttpPost GetProfundData- " + innerExcp });
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

            //------------------select Vendore Code-----------
            SqlDataReader Vreader;
            Vreader = dbq.GetSQLReader("select  VendorCode from UserMasters with(nolock) where id=" + id, con);
            if (Vreader.Read())
                usermaster.VendorCode = Convert.ToString(Vreader.GetValue(0));
            //------------------------------------------------

            //------------------select Role-----------

            //------------------------------------------------

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

            //var city = from c in af.Cities
            //           orderby c.CityName
            //           select new { c.CityName };
            //============ Changes done on 06/05/2023 for showing domain name in location dll =======================

            var city = from c in af.DomainMaster
                       orderby c.Name
                       select new { c.Name };

            ViewBag.city = city.Select(c => c.Name);

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
            //---------------------------------------Vendor Master------------------------
            //var Vendormaster = (from a in OWpro.VendorMasters
            //                        //where a.UserType == usrm.UsertType
            //                    orderby a.VendorCode
            //                    select new { a.VendorCode });

            SqlDataAdapter adp = new SqlDataAdapter("FetchVendorCodeList", con);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            //adp.SelectCommand.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = uid;

            DataSet ds = new DataSet();
            adp.Fill(ds);
            var objectlst = new List<VendorCodeList>();
            VendorCodeList def;

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    def = new VendorCodeList
                    {
                        VendorCode = ds.Tables[0].Rows[i].ItemArray[0].ToString(),
                    };
                    objectlst.Add(def);
                }

            }

            ViewBag.VendorCode = objectlst.Select(a => a.VendorCode);
            //---------------------------------------Vendor Master------------------------END--------

            ViewBag.SelectedAccesslevel = lst;

            //-----------------------user role -------------------
            //var Roles = from c in af.RoleMaster
            //            orderby c.RoleName
            //           select new { c.ID, c.RoleName };
            var Roles = af.RoleMaster.Where(a => a.IsActive == 1).ToList();

            List<SelectListItem> lstRoles = new List<SelectListItem>();
            for (int i = 0; i < Roles.Count(); i++)
            {
                lstRoles.Add(new SelectListItem() { Text = Roles[i].RoleName, Value = Roles[i].ID.ToString() });
            }
            ViewBag.SelectRoles1 = lstRoles;
            //ViewBag.SelectRoles = Roles.Select(c => c.RoleName);

            var UserRole = af.RoleMaster.Where(a => a.ID == usermaster.RoleId).SingleOrDefault();

            if(UserRole == null)
            {
                ViewBag.UserRoleName = "";
                ViewBag.UserRoleId = 0;
            }
            else
            {
                ViewBag.UserRoleName = UserRole.RoleName;
                ViewBag.UserRoleId = UserRole.ID;
            }

            

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
            //if (Session["domainid"] == null)
            //{
            //    return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            //}
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
                    decimal l4frm = 0;
                    decimal l4to = 0;
                    int uid = (int)Session["uid"];
                    string dob = Request.Form["datepicker"];
                    string title = Request.Form["title"];
                    string policyname = Request.Form["policynm"];
                    var pol = adc.AppSecPolicies.Where(m => m.Name == policyname).SingleOrDefault();

                    usermaster.DOB = DateTime.Now;

                    if (Request.Form["usertype"] != "")
                    {
                        verlevl = Request.Form["usertype"].ToString();
                    }
                    if (Request.Form["city"] != "")
                    {
                        location = Request.Form["city"].ToString();
                    }

                    //---------------Added on 13-04-2020-----------
                    string VendorCode = Request.Form["VendorCode"];

                    RoleMapping rol = new RoleMapping();

                    ////----------------- User ActivitiesLogs------
                    UserMasterActivity uma = new UserMasterActivity();
                    uma.Action = "User Modified";
                    uma.ActionBy = uid;
                    uma.Actiondate = DateTime.Now;
                    uma.UserId = usermaster.ID;
                    uma.comments = "Roles[" + allroles + "]";
                    ulc.UserMasterActivities.Add(uma);
                    // ----------------------------------END-------

                    string Accesslevel = "";
                    if (Request.Form["accesschange"] == "1")
                    {
                        if (Request.Form["SelectedAccesslevel"] != "")
                        {
                            Accesslevel = Request.Form["SelectedAccesslevel"].ToString();

                            if (usermaster.AccessLevel == "ORG")
                            {
                                //OWpro.SpRemoveUserAccess(usermaster.ID, uid, usermaster.AccessLevel, Accesslevel);

                                SqlCommand com1 = new SqlCommand("SpRemoveUserAccess", con);
                                com1.CommandType = CommandType.StoredProcedure;
                                com1.Parameters.AddWithValue("@Uid", usermaster.ID);
                                com1.Parameters.AddWithValue("@RemoveBY", uid);
                                com1.Parameters.AddWithValue("@Usertype", usermaster.AccessLevel);
                                com1.Parameters.AddWithValue("@NewAccess", Accesslevel);

                                con.Open();
                                com1.ExecuteNonQuery();
                                con.Close();
                            }
                                
                            else if (usermaster.AccessLevel == "CUST")
                            {
                                //OWpro.SpRemoveUserAccess(usermaster.ID, uid, usermaster.AccessLevel, Accesslevel);

                                SqlCommand com2 = new SqlCommand("SpRemoveUserAccess", con);
                                com2.CommandType = CommandType.StoredProcedure;
                                com2.Parameters.AddWithValue("@Uid", usermaster.ID);
                                com2.Parameters.AddWithValue("@RemoveBY", uid);
                                com2.Parameters.AddWithValue("@Usertype", usermaster.AccessLevel);
                                com2.Parameters.AddWithValue("@NewAccess", Accesslevel);

                                con.Open();
                                com2.ExecuteNonQuery();
                                con.Close();
                            }
                                
                            else if (usermaster.AccessLevel == "DOM")
                            {
                                //OWpro.SpRemoveUserAccess(usermaster.ID, uid, usermaster.AccessLevel, Accesslevel);

                                SqlCommand com3 = new SqlCommand("SpRemoveUserAccess", con);
                                com3.CommandType = CommandType.StoredProcedure;
                                com3.Parameters.AddWithValue("@Uid", usermaster.ID);
                                com3.Parameters.AddWithValue("@RemoveBY", uid);
                                com3.Parameters.AddWithValue("@Usertype", usermaster.AccessLevel);
                                com3.Parameters.AddWithValue("@NewAccess", Accesslevel);

                                con.Open();
                                com3.ExecuteNonQuery();
                                con.Close();
                            }
                                

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
                                    adc.SaveChanges();
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
                                    adc.SaveChanges();
                                }

                                uma = new UserMasterActivity();
                                uma.Action = "User Modified";
                                uma.ActionBy = uid;
                                uma.Actiondate = DateTime.Now;
                                uma.UserId = usermaster.ID;
                                uma.comments = "Cust" + " " + Customers;
                                ulc.UserMasterActivities.Add(uma);
                                ulc.SaveChanges();
                                //   adc.SaveChanges();
                                //-----------------------DomainMaping---------
                                //DomainUserMapMaster domainUsrMap;

                                //var domlist = (from d in af.DomainMaster
                                //               from c in af.UserCustomerMappings
                                //               //    from ud in af.DomainUserMapMaster
                                //               where d.CustomerId == c.CustomerId && c.UserId == usermaster.ID// && d.Id == ud.DomainId && ud.UserId == uid
                                //               select new { d.Id, d.CustomerId });

                                //foreach (var item in domlist)
                                //{
                                //    domainUsrMap = new DomainUserMapMaster();
                                //    domainUsrMap.CustomerID = item.CustomerId;// Convert.ToInt16(orgnizationlist[i]);
                                //    domainUsrMap.DomainId = item.Id;
                                //    domainUsrMap.UserId = (int)usermaster.ID;
                                //    adc.DomainUserMapMasters.Add(domainUsrMap);
                                //    Domains = Domains + "," + domainUsrMap.DomainId;
                                //    adc.SaveChanges();
                                //}

                                //--------------------Select pro---------------
                                DataSet dsdomin = new DataSet();
                                SqlDataAdapter adp = new SqlDataAdapter("SelectUserDomains", con);
                                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.BigInt).Value = 0;
                                adp.SelectCommand.Parameters.Add("@DomainID", SqlDbType.BigInt).Value = 0;
                                adp.SelectCommand.Parameters.Add("@UserID", SqlDbType.BigInt).Value = uid; //usermaster.ID;

                                adp.Fill(dsdomin);
                                //  IWSearch Owsr = null;
                                if (dsdomin.Tables[0].Rows.Count > 0)
                                {
                                    int index = 0;
                                    int count = dsdomin.Tables[0].Rows.Count;
                                    while (count > 0)
                                    {
                                        //OWpro.InsertDomainUserMapMaster(Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[1]), Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[0]), (int)usermaster.ID);

                                        SqlCommand com4 = new SqlCommand("InsertDomainUserMapMaster", con);
                                        com4.CommandType = CommandType.StoredProcedure;
                                        com4.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[1]));
                                        com4.Parameters.AddWithValue("@DomainID", Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[0]));
                                        com4.Parameters.AddWithValue("@UserID", (int)usermaster.ID);

                                        con.Open();
                                        com4.ExecuteNonQuery();
                                        con.Close();

                                        index = index + 1;
                                        count = count - 1;
                                    }
                                }

                                uma = new UserMasterActivity();
                                uma.Action = "User Modified";
                                uma.ActionBy = uid;
                                uma.Actiondate = DateTime.Now;
                                uma.UserId = usermaster.ID;
                                uma.comments = "Dom" + " " + Domains;
                                ulc.UserMasterActivities.Add(uma);
                                ulc.SaveChanges();


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
                                    adc.SaveChanges();
                                }

                                uma = new UserMasterActivity();
                                uma.Action = "User Modified";
                                uma.ActionBy = uid;
                                uma.Actiondate = DateTime.Now;
                                uma.UserId = usermaster.ID;
                                uma.comments = "Cust" + " " + Customers;
                                ulc.UserMasterActivities.Add(uma);
                                ulc.SaveChanges();
                                // adc.SaveChanges();
                                //-----------------------DomainMaping---------
                                //DomainUserMapMaster domainUsrMap;

                                //var domlist = (from d in af.DomainMaster
                                //               from c in af.UserCustomerMappings
                                //               // from ud in af.DomainUserMapMaster
                                //               where d.CustomerId == c.CustomerId && c.UserId == usermaster.ID //&& d.Id == ud.DomainId && ud.UserId == uid
                                //               select new { d.Id, d.CustomerId });

                                //foreach (var item in domlist)
                                //{
                                //    domainUsrMap = new DomainUserMapMaster();
                                //    domainUsrMap.CustomerID = item.CustomerId;// Convert.ToInt16(orgnizationlist[i]);
                                //    domainUsrMap.DomainId = item.Id;
                                //    domainUsrMap.UserId = usermaster.ID;
                                //    adc.DomainUserMapMasters.Add(domainUsrMap);
                                //    Domains = Domains + "," + domainUsrMap.DomainId;
                                //    adc.SaveChanges();
                                //}
                                //--------------------Select pro---------------

                                for (int i = 0; i < customerlist.Length; i++)
                                {
                                    // usrCustMap = new UserCustomerMapping();
                                    // usrCustMap.CustomerId = Convert.ToInt16(customerlist[i]);

                                    //---------------------
                                    DataSet dsdomin = new DataSet();
                                    SqlDataAdapter adp = new SqlDataAdapter("SelectUserDomains", con);
                                    adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                                    adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.BigInt).Value = Convert.ToInt16(customerlist[i]);
                                    adp.SelectCommand.Parameters.Add("@DomainID", SqlDbType.BigInt).Value = 0;
                                    adp.SelectCommand.Parameters.Add("@UserID", SqlDbType.BigInt).Value = uid;//usermaster.ID;

                                    adp.Fill(dsdomin);
                                    //  IWSearch Owsr = null;
                                    if (dsdomin.Tables[0].Rows.Count > 0)
                                    {
                                        int index = 0;
                                        int count = dsdomin.Tables[0].Rows.Count;
                                        while (count > 0)
                                        {
                                            //OWpro.InsertDomainUserMapMaster(Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[1]), Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[0]), (int)usermaster.ID);

                                            SqlCommand com5 = new SqlCommand("InsertDomainUserMapMaster", con);
                                            com5.CommandType = CommandType.StoredProcedure;
                                            com5.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[1]));
                                            com5.Parameters.AddWithValue("@DomainID", Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[0]));
                                            com5.Parameters.AddWithValue("@UserID", (int)usermaster.ID);

                                            con.Open();
                                            com5.ExecuteNonQuery();
                                            con.Close();

                                            index = index + 1;
                                            count = count - 1;
                                        }

                                        uma = new UserMasterActivity();
                                        uma.Action = "User Modified";
                                        uma.ActionBy = uid;
                                        uma.Actiondate = DateTime.Now;
                                        uma.UserId = usermaster.ID;
                                        uma.comments = "Dom" + " " + Domains;
                                        ulc.UserMasterActivities.Add(uma);
                                        ulc.SaveChanges();
                                    }
                                }


                            }
                            else if (Accesslevel == "DOM")
                            {

                                List<int> domns = new List<int>();
                                domainsids = Request.Form["Dom"].ToString();
                                domainlists = domainsids.Split(',');
                                for (int i = 0; i < domainlists.Length; i++)
                                {
                                    // domns.Add(Convert.ToInt16(domainlists[i]));

                                    DataSet dsdomin = new DataSet();
                                    SqlDataAdapter adp = new SqlDataAdapter("SelectUserDomains", con);
                                    adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                                    adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.BigInt).Value = 0;
                                    adp.SelectCommand.Parameters.Add("@DomainID", SqlDbType.BigInt).Value = Convert.ToInt16(domainlists[i]);
                                    adp.SelectCommand.Parameters.Add("@UserID", SqlDbType.BigInt).Value = uid;

                                    adp.Fill(dsdomin);
                                    //  IWSearch Owsr = null;
                                    if (dsdomin.Tables[0].Rows.Count > 0)
                                    {
                                        int index = 0;
                                        int count = dsdomin.Tables[0].Rows.Count;
                                        while (count > 0)
                                        {
                                            //OWpro.InsertDomainUserMapMaster(Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[1]), Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[0]), (int)usermaster.ID);

                                            SqlCommand com6 = new SqlCommand("InsertDomainUserMapMaster", con);
                                            com6.CommandType = CommandType.StoredProcedure;
                                            com6.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[1]));
                                            com6.Parameters.AddWithValue("@DomainID", Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[0]));
                                            com6.Parameters.AddWithValue("@UserID", (int)usermaster.ID);

                                            con.Open();
                                            com6.ExecuteNonQuery();
                                            con.Close();

                                            index = index + 1;
                                            count = count - 1;
                                        }
                                    }
                                }
                                //-----------------------DomainMaping---------
                                //DomainUserMapMaster domainUsrMap;

                                //var domlist = (from d in af.DomainMaster
                                //               where domns.Contains(d.Id)
                                //               select new { d.Id, d.CustomerId });

                                //foreach (var item in domlist)
                                //{
                                //    domainUsrMap = new DomainUserMapMaster();
                                //    domainUsrMap.CustomerID = item.CustomerId;// Convert.ToInt16(orgnizationlist[i]);
                                //    domainUsrMap.DomainId = item.Id;
                                //    domainUsrMap.UserId = usermaster.ID;
                                //    adc.DomainUserMapMasters.Add(domainUsrMap);
                                //    Domains = Domains + "," + domainUsrMap.DomainId;
                                //    adc.SaveChanges();
                                //}
                                //--------------------Select pro---------------


                                uma = new UserMasterActivity();
                                uma.Action = "User Modified";
                                uma.ActionBy = uid;
                                uma.Actiondate = DateTime.Now;
                                uma.UserId = usermaster.ID;
                                uma.comments = "Dom" + " " + Domains;
                                ulc.UserMasterActivities.Add(uma);

                                ulc.SaveChanges();

                            }

                            usermaster.AccessLevel = Accesslevel;
                        }

                    }
                    //--------------------userManagment-------
                    //usermaster.L1StartLimit = l1frm;
                    //usermaster.L1StopLimit = l1to;
                    //usermaster.L2StartLimit = l2frm;
                    //usermaster.L2StopLimit = l2to;
                    //usermaster.L3StartLimit = l3frm;
                    //usermaster.L3StopLimit = l3to;
                    //usermaster.L4StartLimit = l4frm;
                    //usermaster.L4StopLimit = l4to;
                    usermaster.Title = title;
                    usermaster.AppSecPolicieID = pol.ID;
                    usermaster.City = location;
                    usermaster.UsertType = verlevl;

                    string selectRole = "";
                    if (Request.Form["SelectRoles1"] != "")
                    {
                        selectRole = Request.Form["SelectRoles1"].ToString();
                        usermaster.RoleId = Convert.ToInt16(selectRole);
                    }
                    usermaster.IsActive = 0;
                    usermaster.ModifedBy = Convert.ToInt16(Session["uid"]);
                    //
                    //------------------------------------30-12-2019---Abid for Vendor name--------------                    
                    //OWpro.updateVendorCode(usermaster.ID, VendorCode);

                    SqlCommand com = new SqlCommand("updateVendorCode", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@uid", usermaster.ID);
                    com.Parameters.AddWithValue("@VendorCode", VendorCode);

                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();

                    //-----------------------------------    

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

                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                logger.Log(LogLevel.Error, "Edit-Post|" + message + "INNEREXP| " + innerExcp, "Login Index-Msg");
                //logger.Log(LogLevel.Error, e.InnerException.StackTrace, "Login Index-Exception");

                return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });

                // return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }

        //
        // GET: /CreateUser/Delete/5
        [HttpPost]
        public ActionResult Delete(UserMaster usermaster)
        {
            //if (Session["domainid"] == null)
            //{
            //    return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            //}
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
                    //query.UserDeleted = true;
                    query.IsActive = 4;
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
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = "Delete" });
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
            //if (Session["domainid"] == null)
            //{
            //    return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            //}
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
            //if (Session["domainid"] == null)
            //{
            //    return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            //}
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

                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                logger.Log(LogLevel.Error, "Change-Password|" + message + "INNEREXP| " + innerExcp, "Login Index-Msg");
                // logger.Log(LogLevel.Error, e.InnerException.StackTrace, "Login Index-Exception");

                return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });
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

                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                logger.Log(LogLevel.Error, "ChangePass|" + message + "INNEREXP| " + innerExcp, "Login Index-Msg");
                // logger.Log(LogLevel.Error, e.InnerException.StackTrace, "Login Index-Exception");

                return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });
                //return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }
        [HttpPost]
        public ActionResult Disable(UserMaster usermaster)
        {
            //if (Session["domainid"] == null)
            //{
            //    return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            //}
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
                    query.IsActive = 0;
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

                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                logger.Log(LogLevel.Error, "Disable|" + message + "INNEREXP| " + innerExcp, "Login Index-Msg");
                //logger.Log(LogLevel.Error, e.InnerException.StackTrace, "Login Index-Exception");

                return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });
            }
            return RedirectToAction("Index", new { id = 2 });
        }
        [HttpPost]
        public ActionResult PasswordReset(UserMaster usermast, string btn = null)
        {
            //if (Session["domainid"] == null)
            //{
            //    return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            //}
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
                        string password = "newuser@123";
                        //password = RandomPwd(usermaster.LoginID, usermaster.FirstName, usermaster.LastName);
                        usermaster.Password = cmf.EncryptPassword(password);
                        //  usermaster.Password = cmf.EncryptPassword("newuser@123");
                        //
                        usermaster.IsActive = 0;
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
                        return RedirectToAction("Index", new { id = 3, password = password });
                    }
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

                logger.Log(LogLevel.Error, "PasswordReset|" + message + "INNEREXP| " + innerExcp, "Login Index-Msg");
                //logger.Log(LogLevel.Error, e.InnerException.StackTrace, "Login Index-Exception");

                //return RedirectToAction("Error", "Error", new { msg = message, popmsg = innerExcp });
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
            //if (Session["domainid"] == null)
            //{
            //    return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            //}
            //UserMaster usermaster = db.Users.Find(id);
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
                query.IsActive = 0;
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
                adc.SaveChanges();
            }
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                logger.Log(LogLevel.Error, "Unlock|" + message + "INNEREXP| " + innerExcp, "Login Index-Msg");
                // logger.Log(LogLevel.Error, e.InnerException.StackTrace, "Login Index-Exception");

                return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });
            }
            return RedirectToAction("Index", new { id = 4 });
        }
        public ActionResult MyAccount()
        {
            //if (Session["domainid"] == null)
            //{
            //    return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            //}
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
            //if (Session["domainid"] == null)
            //{
            //    return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            //}
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
        public string RandomPwd(string loginID = null, string firstName = null, string lastName = null)
        {
            var sloginID = loginID.ToString().Trim();
            var sFstNm = firstName.Substring(0, 4).ToString().Trim();
            var sLstNm = lastName.Substring(0, 2).ToString().Trim();

            var sfullString = sloginID + sFstNm + sLstNm;
            var stringChars = new Char[10];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = sfullString[random.Next(sfullString.Length)];
            }

            string FinalString = new string(stringChars);
            //OWpro.sp_PwdForNewUser_InsertRndPwd(loginID, firstName, lastName, FinalString);

            SqlCommand com = new SqlCommand("sp_PwdForNewUser_InsertRndPwd", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@LoginID", loginID);
            com.Parameters.AddWithValue("@FirstName", firstName);
            com.Parameters.AddWithValue("@LastName", lastName);
            com.Parameters.AddWithValue("@RandomPassword", FinalString);

            con.Open();
            com.ExecuteNonQuery();
            con.Close();

            //TempData["Message"] = FinalString;
            return FinalString;
        }
        public PartialViewResult _getcustdomains(string userlevel = null, int uid = 0)
        {
            //  getcustdomname getname = new getcustdomname();
            if (userlevel == "CUST" || userlevel == "ORG")
            {
                var custdomname = (from u in af.UserCustomerMappings
                                   from c in af.CustomerMasters
                                   where u.UserId == uid && u.CustomerId == c.Id
                                   select new getcustdomname
                                   {
                                       Name = c.Name
                                   }).ToList();

                ViewBag.LabelName = "Customers Name";
                return PartialView(custdomname);
            }
            else
            {
                var custdomname = (from u in af.DomainUserMapMasters
                                   from c in af.DomainMaster
                                   where u.UserId == uid && u.DomainId == c.Id
                                   select new getcustdomname { Name = c.Name }).ToList();

                ViewBag.LabelName = "Domains Name";
                return PartialView(custdomname);
            }

        }
        public DataSet getselecteddat()
        {
            DataSet ds = new DataSet();


            return ds;
        }


        protected override void Dispose(bool disposing)
        {
            adc.Dispose();
            ulc.Dispose();
            base.Dispose(disposing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create11(UserMaster usermaster)
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
                        string format = "yyyy-MM-dd hh:mm:ss";
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
                        decimal l4frm = 0;
                        decimal l4to = 0;
                        string vendorCode = "";

                        //----------------Added By Abid 30-12-2019----------------For Vendor----
                        if (Request.Form["VendorCode"] != "")
                        {
                            vendorCode = Request.Form["VendorCode"].ToString();
                        }


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
                        if (Request.Form["rvf4"] == "on")
                        {
                            l4frm = Convert.ToDecimal(Request.Form["l4frm"]);
                            l4to = Convert.ToDecimal(Request.Form["l4to"]);
                        }
                        usermaster.L1StartLimit = l1frm;
                        usermaster.L1StopLimit = l1to;
                        usermaster.L2StartLimit = l2frm;
                        usermaster.L2StopLimit = l2to;
                        usermaster.L3StartLimit = l3frm;
                        usermaster.L3StopLimit = l3to;
                        usermaster.L4StartLimit = l4frm;
                        usermaster.L4StopLimit = l4to;

                        //-------------Added On 19-12-2018----------For Axis Requirement-----------
                        //string password = RandomPwd(usermaster.LoginID, usermaster.FirstName, usermaster.LastName);
                        //---------------------End------------------------
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
                        usermaster.LastLogin = Convert.ToDateTime(DateTime.Now.ToString(format));
                        usermaster.InvalidPasswordAttempts = 0;

                        adc.UserMasters.Add(usermaster);
                        try
                        {
                            adc.SaveChanges();
                            //------------------------------------30-12-2019---Abid for Vendor name--------------                            
                            //OWpro.updateVendorCode(usermaster.ID, vendorCode);

                            SqlCommand com = new SqlCommand("updateVendorCode", con);
                            com.CommandType = CommandType.StoredProcedure;
                            com.Parameters.AddWithValue("@uid", usermaster.ID);
                            com.Parameters.AddWithValue("@VendorCode", vendorCode);

                            con.Open();
                            com.ExecuteNonQuery();
                            con.Close();

                            //-----------------------------------    
                        }
                        catch (Exception e)
                        {
                            string message = "";
                            string innerExcp = "";
                            if (e.Message != null)
                                message = e.Message.ToString();
                            if (e.InnerException != null)
                                innerExcp = e.InnerException.Message;

                            logger.Log(LogLevel.Error, "Post-Create|" + message + "INNEREXP| " + innerExcp, "Create User");

                            return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });


                            //return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
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
                        //---------------Added On 08-09-2017--------------------------
                        string CCPH = Request.Form["CCPH"];
                        //---------------Added On 27-07-2018--------------------------
                        string RVF4 = Request.Form["rvf4"];
                        //------------------Added  On 27-07-2018----
                        if (RVF4 == "on")
                        {
                            allroles = "RVF4";
                            rol.Process = rollname[18];
                            rol.Active = true;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        else
                        {
                            rol.Process = rollname[18];
                            rol.Active = false;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        //--------------------------------------------

                        if (CCPH == "on")
                        {
                            allroles = "CCPH";
                            rol.Process = rollname[17];
                            rol.Active = true;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                        else
                        {
                            rol.Process = rollname[17];
                            rol.Active = false;
                            rol.UserID = tempuserid;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }

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
                            //  adc.SaveChanges();
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
                                adc.SaveChanges();
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
                                adc.SaveChanges();
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
                            // adc.SaveChanges();
                            //-----------------------DomainMaping---------
                            //DomainUserMapMaster domainUsrMap;

                            //var domlist = (from d in af.DomainMaster
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
                            adp.SelectCommand.Parameters.Add("@UserID", SqlDbType.BigInt).Value = id;//usermaster.ID;

                            adp.Fill(dsdomin);
                            //  IWSearch Owsr = null;
                            if (dsdomin.Tables[0].Rows.Count > 0)
                            {
                                int index = 0;
                                int count = dsdomin.Tables[0].Rows.Count;
                                while (count > 0)
                                {
                                    //OWpro.InsertDomainUserMapMaster(Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[1]), Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[0]), (int)usermaster.ID);

                                    SqlCommand com = new SqlCommand("InsertDomainUserMapMaster", con);
                                    com.CommandType = CommandType.StoredProcedure;
                                    com.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[1]));
                                    com.Parameters.AddWithValue("@DomainID", Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[0]));
                                    com.Parameters.AddWithValue("@UserID", (int)usermaster.ID);

                                    con.Open();
                                    com.ExecuteNonQuery();
                                    con.Close();

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
                                adc.SaveChanges();
                            }

                            uma = new UserMasterActivity();
                            uma.Action = "User Created";
                            uma.ActionBy = id;
                            uma.Actiondate = DateTime.Now;
                            uma.UserId = tempuserid;
                            uma.comments = "Cust" + " " + Customers;
                            ulc.UserMasterActivities.Add(uma);
                            ulc.SaveChanges();
                            //  adc.SaveChanges();
                            //-----------------------DomainMaping---------
                            //DomainUserMapMaster domainUsrMap;

                            //var domlist = (from d in af.DomainMaster
                            //               from c in af.UserCustomerMappings
                            //               // from ud in af.DomainUserMapMaster
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

                            //    adc.SaveChanges();
                            //}

                            //------------------Select From Proc--------------------
                            DataSet dsdomin = new DataSet();
                            SqlDataAdapter adp = new SqlDataAdapter("SelectUserDomains", con);
                            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                            adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.BigInt).Value = 0;
                            adp.SelectCommand.Parameters.Add("@DomainID", SqlDbType.BigInt).Value = 0;
                            adp.SelectCommand.Parameters.Add("@UserID", SqlDbType.BigInt).Value = id;//usermaster.ID;

                            adp.Fill(dsdomin);
                            //  IWSearch Owsr = null;
                            if (dsdomin.Tables[0].Rows.Count > 0)
                            {
                                int index = 0;
                                int count = dsdomin.Tables[0].Rows.Count;
                                while (count > 0)
                                {
                                    //OWpro.InsertDomainUserMapMaster(Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[1]), Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[0]), (int)usermaster.ID);

                                    SqlCommand com = new SqlCommand("InsertDomainUserMapMaster", con);
                                    com.CommandType = CommandType.StoredProcedure;
                                    com.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[1]));
                                    com.Parameters.AddWithValue("@DomainID", Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[0]));
                                    com.Parameters.AddWithValue("@UserID", (int)usermaster.ID);

                                    con.Open();
                                    com.ExecuteNonQuery();
                                    con.Close();

                                    index = index + 1;
                                    count = count - 1;
                                }
                            }
                            //-------------------------
                            uma = new UserMasterActivity();
                            uma.Action = "User Created";
                            uma.ActionBy = id;
                            uma.Actiondate = DateTime.Now;
                            uma.UserId = tempuserid;
                            uma.comments = "Dom" + " " + Domains;
                            ulc.UserMasterActivities.Add(uma);

                            ulc.SaveChanges();


                        }
                        else if (Accesslevel == "DOM")
                        {

                            List<int> domns = new List<int>();
                            domainsids = Request.Form["Dom"].ToString();
                            domainlists = domainsids.Split(',');
                            for (int i = 0; i < domainlists.Length; i++)
                            {
                                //domns.Add(Convert.ToInt16(domainlists[i]));

                                DataSet dsdomin = new DataSet();
                                SqlDataAdapter adp = new SqlDataAdapter("SelectUserDomains", con);
                                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.BigInt).Value = 0;
                                adp.SelectCommand.Parameters.Add("@DomainID", SqlDbType.BigInt).Value = Convert.ToInt16(domainlists[i]);
                                adp.SelectCommand.Parameters.Add("@UserID", SqlDbType.BigInt).Value = id;

                                adp.Fill(dsdomin);
                                //  IWSearch Owsr = null;
                                if (dsdomin.Tables[0].Rows.Count > 0)
                                {
                                    int index = 0;
                                    int count = dsdomin.Tables[0].Rows.Count;
                                    while (count > 0)
                                    {
                                        //OWpro.InsertDomainUserMapMaster(Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[1]), Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[0]), (int)usermaster.ID);

                                        SqlCommand com = new SqlCommand("InsertDomainUserMapMaster", con);
                                        com.CommandType = CommandType.StoredProcedure;
                                        com.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[1]));
                                        com.Parameters.AddWithValue("@DomainID", Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[0]));
                                        com.Parameters.AddWithValue("@UserID", (int)usermaster.ID);

                                        con.Open();
                                        com.ExecuteNonQuery();
                                        con.Close();

                                        index = index + 1;
                                        count = count - 1;
                                    }
                                }
                            }
                            //-----------------------DomainMaping---------
                            //DomainUserMapMaster domainUsrMap;

                            //var domlist = (from d in af.DomainMaster
                            //               where domns.Contains(d.Id)
                            //               select new { d.Id, d.CustomerId });

                            //foreach (var item in domlist)
                            //{
                            //    domainUsrMap = new DomainUserMapMaster();
                            //    domainUsrMap.CustomerID = item.CustomerId;// Convert.ToInt16(orgnizationlist[i]);
                            //    domainUsrMap.DomainId = item.Id;
                            //    domainUsrMap.UserId = (int)tempuserid;
                            //    adc.DomainUserMapMasters.Add(domainUsrMap);
                            //    Domains = Domains + "," + domainUsrMap.DomainId;
                            //    adc.SaveChanges();
                            //}
                            //--------------------Proc----------------

                            uma = new UserMasterActivity();
                            uma.Action = "User Created";
                            uma.ActionBy = id;
                            uma.Actiondate = DateTime.Now;
                            uma.UserId = tempuserid;
                            uma.comments = "Dom" + " " + Domains;
                            ulc.UserMasterActivities.Add(uma);

                            ulc.SaveChanges();

                        }


                        //----------------- User ActivitiesLogs------
                        //UserMasterActivity uma = new UserMasterActivity();

                        ////----------------------------------END-------
                        //try
                        //{
                        //    adc.SaveChanges();
                        //}
                        //catch (Exception e1)
                        //{
                        //    //ErrorDisplay er = new ErrorDisplay();
                        //    //ViewBag.Error = e1.InnerException;
                        //    //er.ErrorMessage = e1.InnerException.Message;
                        //    //return View("Error", er);
                        //    return RedirectToAction("Error", "Error", new { msg = e1.Message.ToString(), popmsg = e1.StackTrace.ToString() });
                        //}

                        ViewBag.result = false;
                        // ViewBag.temppassword = "Password is:" + password;
                        return RedirectToAction("Create", "CreateUserNew", new { id = 1, pass = password });
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
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                logger.Log(LogLevel.Error, "Create-Post|" + message + "INNEREXP| " + innerExcp, "Login Index-Msg");
                // logger.Log(LogLevel.Error, e.InnerException.StackTrace, "Login Index-Exception");

                FormsAuthentication.SignOut();

                return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });
                // return RedirectToAction("Error", "Error", new { msg = message, popmsg = "CMS HttpPost GetProfundData- " + innerExcp });
                //return View("Error", er);
            }
        }

        [HttpPost]
        public ActionResult Edit11(UserMaster usermaster)
        {
            //if (Session["domainid"] == null)
            //{
            //    return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            //}
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
                    decimal l4frm = 0;
                    decimal l4to = 0;
                    int uid = (int)Session["uid"];
                    string dob = Request.Form["datepicker"];
                    string title = Request.Form["title"];
                    string policyname = Request.Form["policynm"];
                    var pol = adc.AppSecPolicies.Where(m => m.Name == policyname).SingleOrDefault();
                    ////----------------Comented On--------05-03-2018---------
                    //if (dob != null || dob != "")
                    //{
                    //    string[] dobb = dob.Split('/');
                    //    if (dobb.Length > 2)
                    //    {
                    //        dob = dobb[2].Substring(0, 4) + '-' + dobb[0] + '-' + dobb[1];
                    //        usermaster.DOB = Convert.ToDateTime(dob);
                    //    }
                    //    else
                    //        usermaster.DOB = DateTime.Now;
                    //}
                    //else
                    //    usermaster.DOB = DateTime.Now;
                    ////----------------Comented On--------05-03-2018---------
                    usermaster.DOB = DateTime.Now;

                    //string[] dobb = dob.Split('/');
                    //if (dobb.Length > 2)
                    //{
                    //    dob = dobb[2].Substring(0, 4) + '-' + dobb[0] + '-' + dobb[1];
                    //    usermaster.DOB = Convert.ToDateTime(dob);
                    //}

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
                    if (Request.Form["rvf4"] == "on")
                    {
                        l4frm = Convert.ToDecimal(Request.Form["l4frm"]);
                        l4to = Convert.ToDecimal(Request.Form["l4to"]);
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
                    //---------------Added On 08-09-2017--------------------------
                    string CCPH = Request.Form["CCPH"];
                    //---------------Added On 30-03-2015--------------------------
                    string RVF4 = Request.Form["rvf4"];
                    //---------------Added on 13-04-2020-----------
                    string VendorCode = Request.Form["VendorCode"];

                    RoleMapping rol = new RoleMapping();

                    if (RVF4 == "on")
                    {
                        RoleMapping rl = adc.RoleMappings.Where(m => m.UserID == usermaster.ID && m.Process == "RVF4").SingleOrDefault();
                        if (rl == null)
                        {
                            allroles = "RVF4";
                            rol.Process = rollname[18];
                            rol.Active = true;
                            rol.UserID = usermaster.ID;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                    }

                    if (CCPH == "on")
                    {
                        RoleMapping rl = adc.RoleMappings.Where(m => m.UserID == usermaster.ID && m.Process == "CCPH").SingleOrDefault();
                        if (rl == null)
                        {
                            allroles = "CCPH";
                            rol.Process = rollname[17];
                            rol.Active = true;
                            rol.UserID = usermaster.ID;
                            adc.RoleMappings.Add(rol);
                            adc.SaveChanges();
                        }
                    }

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
                        if (item.Process == "RVF4")
                        {
                            rolmp = dbTemp.RoleMappings.Single(i => i.ID == item.ID);
                            if (RVF4 == "on")
                            {
                                rolmp.Active = true;
                                allroles = "RVF4";
                            }
                            else
                                rolmp.Active = false;
                        }
                        //---------------------------------------
                        if (item.Process == "CCPH")
                        {
                            rolmp = dbTemp.RoleMappings.Single(i => i.ID == item.ID);
                            if (CCPH == "on")
                            {
                                rolmp.Active = true;
                                allroles = "CCPH";
                            }
                            else
                                rolmp.Active = false;
                        }

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
                            {
                                //OWpro.SpRemoveUserAccess(usermaster.ID, uid, usermaster.AccessLevel, Accesslevel);

                                SqlCommand com7 = new SqlCommand("SpRemoveUserAccess", con);
                                com7.CommandType = CommandType.StoredProcedure;
                                com7.Parameters.AddWithValue("@Uid", usermaster.ID);
                                com7.Parameters.AddWithValue("@RemoveBY", uid);
                                com7.Parameters.AddWithValue("@Usertype", usermaster.AccessLevel);
                                com7.Parameters.AddWithValue("@NewAccess", Accesslevel);

                                con.Open();
                                com7.ExecuteNonQuery();
                                con.Close();
                            }
                                
                            else if (usermaster.AccessLevel == "CUST")
                            {
                                //OWpro.SpRemoveUserAccess(usermaster.ID, uid, usermaster.AccessLevel, Accesslevel);

                                SqlCommand com8 = new SqlCommand("SpRemoveUserAccess", con);
                                com8.CommandType = CommandType.StoredProcedure;
                                com8.Parameters.AddWithValue("@Uid", usermaster.ID);
                                com8.Parameters.AddWithValue("@RemoveBY", uid);
                                com8.Parameters.AddWithValue("@Usertype", usermaster.AccessLevel);
                                com8.Parameters.AddWithValue("@NewAccess", Accesslevel);

                                con.Open();
                                com8.ExecuteNonQuery();
                                con.Close();
                            }
                                
                            else if (usermaster.AccessLevel == "DOM")
                            {
                                //OWpro.SpRemoveUserAccess(usermaster.ID, uid, usermaster.AccessLevel, Accesslevel);

                                SqlCommand com9 = new SqlCommand("SpRemoveUserAccess", con);
                                com9.CommandType = CommandType.StoredProcedure;
                                com9.Parameters.AddWithValue("@Uid", usermaster.ID);
                                com9.Parameters.AddWithValue("@RemoveBY", uid);
                                com9.Parameters.AddWithValue("@Usertype", usermaster.AccessLevel);
                                com9.Parameters.AddWithValue("@NewAccess", Accesslevel);

                                con.Open();
                                com9.ExecuteNonQuery();
                                con.Close();
                            }
                                

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
                                    adc.SaveChanges();
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
                                    adc.SaveChanges();
                                }

                                uma = new UserMasterActivity();
                                uma.Action = "User Modified";
                                uma.ActionBy = uid;
                                uma.Actiondate = DateTime.Now;
                                uma.UserId = usermaster.ID;
                                uma.comments = "Cust" + " " + Customers;
                                ulc.UserMasterActivities.Add(uma);
                                ulc.SaveChanges();
                                //   adc.SaveChanges();
                                //-----------------------DomainMaping---------
                                //DomainUserMapMaster domainUsrMap;

                                //var domlist = (from d in af.DomainMaster
                                //               from c in af.UserCustomerMappings
                                //               //    from ud in af.DomainUserMapMaster
                                //               where d.CustomerId == c.CustomerId && c.UserId == usermaster.ID// && d.Id == ud.DomainId && ud.UserId == uid
                                //               select new { d.Id, d.CustomerId });

                                //foreach (var item in domlist)
                                //{
                                //    domainUsrMap = new DomainUserMapMaster();
                                //    domainUsrMap.CustomerID = item.CustomerId;// Convert.ToInt16(orgnizationlist[i]);
                                //    domainUsrMap.DomainId = item.Id;
                                //    domainUsrMap.UserId = (int)usermaster.ID;
                                //    adc.DomainUserMapMasters.Add(domainUsrMap);
                                //    Domains = Domains + "," + domainUsrMap.DomainId;
                                //    adc.SaveChanges();
                                //}

                                //--------------------Select pro---------------
                                DataSet dsdomin = new DataSet();
                                SqlDataAdapter adp = new SqlDataAdapter("SelectUserDomains", con);
                                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.BigInt).Value = 0;
                                adp.SelectCommand.Parameters.Add("@DomainID", SqlDbType.BigInt).Value = 0;
                                adp.SelectCommand.Parameters.Add("@UserID", SqlDbType.BigInt).Value = uid; //usermaster.ID;

                                adp.Fill(dsdomin);
                                //  IWSearch Owsr = null;
                                if (dsdomin.Tables[0].Rows.Count > 0)
                                {
                                    int index = 0;
                                    int count = dsdomin.Tables[0].Rows.Count;
                                    while (count > 0)
                                    {
                                        //OWpro.InsertDomainUserMapMaster(Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[1]), Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[0]), (int)usermaster.ID);

                                        SqlCommand com10 = new SqlCommand("InsertDomainUserMapMaster", con);
                                        com10.CommandType = CommandType.StoredProcedure;
                                        com10.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[1]));
                                        com10.Parameters.AddWithValue("@DomainID", Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[0]));
                                        com10.Parameters.AddWithValue("@UserID", (int)usermaster.ID);

                                        con.Open();
                                        com10.ExecuteNonQuery();
                                        con.Close();

                                        index = index + 1;
                                        count = count - 1;
                                    }
                                }

                                uma = new UserMasterActivity();
                                uma.Action = "User Modified";
                                uma.ActionBy = uid;
                                uma.Actiondate = DateTime.Now;
                                uma.UserId = usermaster.ID;
                                uma.comments = "Dom" + " " + Domains;
                                ulc.UserMasterActivities.Add(uma);
                                ulc.SaveChanges();


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
                                    adc.SaveChanges();
                                }

                                uma = new UserMasterActivity();
                                uma.Action = "User Modified";
                                uma.ActionBy = uid;
                                uma.Actiondate = DateTime.Now;
                                uma.UserId = usermaster.ID;
                                uma.comments = "Cust" + " " + Customers;
                                ulc.UserMasterActivities.Add(uma);
                                ulc.SaveChanges();
                                // adc.SaveChanges();
                                //-----------------------DomainMaping---------
                                //DomainUserMapMaster domainUsrMap;

                                //var domlist = (from d in af.DomainMaster
                                //               from c in af.UserCustomerMappings
                                //               // from ud in af.DomainUserMapMaster
                                //               where d.CustomerId == c.CustomerId && c.UserId == usermaster.ID //&& d.Id == ud.DomainId && ud.UserId == uid
                                //               select new { d.Id, d.CustomerId });

                                //foreach (var item in domlist)
                                //{
                                //    domainUsrMap = new DomainUserMapMaster();
                                //    domainUsrMap.CustomerID = item.CustomerId;// Convert.ToInt16(orgnizationlist[i]);
                                //    domainUsrMap.DomainId = item.Id;
                                //    domainUsrMap.UserId = usermaster.ID;
                                //    adc.DomainUserMapMasters.Add(domainUsrMap);
                                //    Domains = Domains + "," + domainUsrMap.DomainId;
                                //    adc.SaveChanges();
                                //}
                                //--------------------Select pro---------------

                                for (int i = 0; i < customerlist.Length; i++)
                                {
                                    // usrCustMap = new UserCustomerMapping();
                                    // usrCustMap.CustomerId = Convert.ToInt16(customerlist[i]);

                                    //---------------------
                                    DataSet dsdomin = new DataSet();
                                    SqlDataAdapter adp = new SqlDataAdapter("SelectUserDomains", con);
                                    adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                                    adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.BigInt).Value = Convert.ToInt16(customerlist[i]);
                                    adp.SelectCommand.Parameters.Add("@DomainID", SqlDbType.BigInt).Value = 0;
                                    adp.SelectCommand.Parameters.Add("@UserID", SqlDbType.BigInt).Value = uid;//usermaster.ID;

                                    adp.Fill(dsdomin);
                                    //  IWSearch Owsr = null;
                                    if (dsdomin.Tables[0].Rows.Count > 0)
                                    {
                                        int index = 0;
                                        int count = dsdomin.Tables[0].Rows.Count;
                                        while (count > 0)
                                        {
                                            //OWpro.InsertDomainUserMapMaster(Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[1]), Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[0]), (int)usermaster.ID);

                                            SqlCommand com11 = new SqlCommand("InsertDomainUserMapMaster", con);
                                            com11.CommandType = CommandType.StoredProcedure;
                                            com11.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[1]));
                                            com11.Parameters.AddWithValue("@DomainID", Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[0]));
                                            com11.Parameters.AddWithValue("@UserID", (int)usermaster.ID);

                                            con.Open();
                                            com11.ExecuteNonQuery();
                                            con.Close();

                                            index = index + 1;
                                            count = count - 1;
                                        }

                                        uma = new UserMasterActivity();
                                        uma.Action = "User Modified";
                                        uma.ActionBy = uid;
                                        uma.Actiondate = DateTime.Now;
                                        uma.UserId = usermaster.ID;
                                        uma.comments = "Dom" + " " + Domains;
                                        ulc.UserMasterActivities.Add(uma);
                                        ulc.SaveChanges();
                                    }
                                }


                            }
                            else if (Accesslevel == "DOM")
                            {

                                List<int> domns = new List<int>();
                                domainsids = Request.Form["Dom"].ToString();
                                domainlists = domainsids.Split(',');
                                for (int i = 0; i < domainlists.Length; i++)
                                {
                                    // domns.Add(Convert.ToInt16(domainlists[i]));

                                    DataSet dsdomin = new DataSet();
                                    SqlDataAdapter adp = new SqlDataAdapter("SelectUserDomains", con);
                                    adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                                    adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.BigInt).Value = 0;
                                    adp.SelectCommand.Parameters.Add("@DomainID", SqlDbType.BigInt).Value = Convert.ToInt16(domainlists[i]);
                                    adp.SelectCommand.Parameters.Add("@UserID", SqlDbType.BigInt).Value = uid;

                                    adp.Fill(dsdomin);
                                    //  IWSearch Owsr = null;
                                    if (dsdomin.Tables[0].Rows.Count > 0)
                                    {
                                        int index = 0;
                                        int count = dsdomin.Tables[0].Rows.Count;
                                        while (count > 0)
                                        {
                                            //OWpro.InsertDomainUserMapMaster(Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[1]), Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[0]), (int)usermaster.ID);

                                            SqlCommand com12 = new SqlCommand("InsertDomainUserMapMaster", con);
                                            com12.CommandType = CommandType.StoredProcedure;
                                            com12.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[1]));
                                            com12.Parameters.AddWithValue("@DomainID", Convert.ToInt32(dsdomin.Tables[0].Rows[index].ItemArray[0]));
                                            com12.Parameters.AddWithValue("@UserID", (int)usermaster.ID);

                                            con.Open();
                                            com12.ExecuteNonQuery();
                                            con.Close();

                                            index = index + 1;
                                            count = count - 1;
                                        }
                                    }
                                }
                                //-----------------------DomainMaping---------
                                //DomainUserMapMaster domainUsrMap;

                                //var domlist = (from d in af.DomainMaster
                                //               where domns.Contains(d.Id)
                                //               select new { d.Id, d.CustomerId });

                                //foreach (var item in domlist)
                                //{
                                //    domainUsrMap = new DomainUserMapMaster();
                                //    domainUsrMap.CustomerID = item.CustomerId;// Convert.ToInt16(orgnizationlist[i]);
                                //    domainUsrMap.DomainId = item.Id;
                                //    domainUsrMap.UserId = usermaster.ID;
                                //    adc.DomainUserMapMasters.Add(domainUsrMap);
                                //    Domains = Domains + "," + domainUsrMap.DomainId;
                                //    adc.SaveChanges();
                                //}
                                //--------------------Select pro---------------


                                uma = new UserMasterActivity();
                                uma.Action = "User Modified";
                                uma.ActionBy = uid;
                                uma.Actiondate = DateTime.Now;
                                uma.UserId = usermaster.ID;
                                uma.comments = "Dom" + " " + Domains;
                                ulc.UserMasterActivities.Add(uma);

                                ulc.SaveChanges();

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
                    usermaster.L4StartLimit = l4frm;
                    usermaster.L4StopLimit = l4to;
                    usermaster.Title = title;
                    usermaster.AppSecPolicieID = pol.ID;
                    usermaster.City = location;
                    usermaster.UsertType = verlevl;


                    usermaster.ModifedBy = Convert.ToInt16(Session["uid"]);
                    //
                    //------------------------------------30-12-2019---Abid for Vendor name--------------                    
                    //OWpro.updateVendorCode(usermaster.ID, VendorCode);

                    SqlCommand com = new SqlCommand("updateVendorCode", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@uid", usermaster.ID);
                    com.Parameters.AddWithValue("@VendorCode", VendorCode);

                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();

                    //-----------------------------------    

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
                //-------------User Type-------------
                var qry = from a in af.UsertTpeMasters
                          orderby a.UserType

                          select new { a.UserType };
                ViewBag.Vlevel = qry.Select(a => a.UserType);
                return View(usermaster);
            }
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                logger.Log(LogLevel.Error, "Edit-Post|" + message + "INNEREXP| " + innerExcp, "Login Index-Msg");
                //logger.Log(LogLevel.Error, e.InnerException.StackTrace, "Login Index-Exception");

                return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });

                // return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }

        public ActionResult Report()
        {
            try
            {
                return View();
            }
            catch(Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                logger.Log(LogLevel.Error, "Report|" + message + "INNEREXP| " + innerExcp, "Create User New");
                // logger.Log(LogLevel.Error, e.InnerException.StackTrace, "Login Index-Exception");

                return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });
            }
        }

        public ActionResult OWActionReport(string fromdate = null, string todate = null, string reporttype = null, string filedwnldtype = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            string tempFromDate = fromdate.Substring(6, 4) + "-" + fromdate.Substring(3, 2) + "-" + fromdate.Substring(0, 2);
            string tempToDate = todate.Substring(6, 4) + "-" + todate.Substring(3, 2) + "-" + todate.Substring(0, 2);

            ReportDocument rDocument = new ReportDocument();

            try
            {
                SqlConnectionStringBuilder SConn = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
                string thisConnectionString = SConn.ConnectionString;
                SqlConnection thisConnection = new SqlConnection(thisConnectionString);
                thisConnection.Open();
                SqlCommand sqlCommand = null;
                string reportPath = "";
                DataTable dTable = new DataTable("DataTable");

                if (reporttype == "Login Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.LoginReport", thisConnection);
                else if (reporttype == "User Management Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.UserActivity", thisConnection);
                else if (reporttype == "Role Management Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RoleReport", thisConnection);
                else if (reporttype == "UnAuthorized User Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.UnAuthorizedUserReport", thisConnection);
                else if (reporttype == "UnAuthentication User Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.UnAuthenticationUserReport", thisConnection);

                if (reporttype == "Login Report" || reporttype == "User Management Report" || reporttype == "Role Management Report" ||
                     reporttype == "UnAuthorized User Report" || reporttype == "UnAuthentication User Report")
                {
                    string fromDate = tempFromDate + " 00:00:00.000";
                    string toDate = tempToDate + " 23:59:59.999";
                    DateTime newToDate = Convert.ToDateTime(tempToDate).AddDays(1);

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add("@FromDate", SqlDbType.Date).Value = Convert.ToDateTime(tempFromDate);
                    sqlCommand.Parameters.Add("@ToDate ", SqlDbType.Date).Value = newToDate;
                }
                else
                {

                }

                if (reporttype == "Login Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/LoginReport.rpt");
                else if (reporttype == "User Management Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/UserManagementReport.rpt");
                else if (reporttype == "Role Management Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RoleManagementReport.rpt");
                else if (reporttype == "UnAuthorized User Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/UnAuthorizedUserReport.rpt");
                else if (reporttype == "UnAuthentication User Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/UnAuthenticationUserReport.rpt");

                rDocument.Load(reportPath);

                using (SqlDataAdapter da = new SqlDataAdapter(sqlCommand))
                {
                    da.Fill(dTable);
                    rDocument.SetDataSource(dTable);
                }

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                if (filedwnldtype == "EXCEL")
                {
                    //-------------------------EXCEl------------------------
                    Stream str = rDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.ExcelWorkbook);
                    str.Seek(0, SeekOrigin.Begin);
                    //--------------------Disposing Objects
                    rDocument.Close();
                    rDocument.Clone();
                    rDocument.Dispose();
                    rDocument = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    return File(str, "application/excel", reporttype + tempFromDate + tempToDate + ".xls");
                }
                else if (filedwnldtype == "CSV")
                {
                    //--------CSV-----------------
                    Stream str = rDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.CharacterSeparatedValues);
                    str.Seek(0, SeekOrigin.Begin);
                    //--------------------Disposing Objects
                    rDocument.Close();
                    rDocument.Clone();
                    rDocument.Dispose();
                    rDocument = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    return File(str, "application/csv", reporttype + ".csv");
                }
                else
                {
                    Stream str = rDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    str.Seek(0, SeekOrigin.Begin);
                    //--------------------Disposing Objects
                    rDocument.Close();
                    rDocument.Clone();
                    rDocument.Dispose();
                    rDocument = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    return File(str, "application/pdf");//, "maintransaction.pdf");                }
                }

            }
            catch (Exception e)
            {
                rDocument.Close();
                rDocument.Clone();
                rDocument.Dispose();
                rDocument = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();

                return RedirectToAction("ReportError", "Error", new { msg = e.Message.ToString(), popmsg = "OW UserManagement Report" });
            }
        }

        [HttpPost]
        public ActionResult ActiveInactiveUser(Int64 id, string btn = null)
        {
            //if (Session["domainid"] == null)
            //{
            //    return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            //}
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            
            if ((bool)Session["UserManagementChecker"] == false)
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
                UserMaster usermaster = adc.UserMasters.Find(id);
                if (usermaster == null)
                {
                    return HttpNotFound();
                }
                if (btn == "Approve")
                {
                    EnableUser(usermaster);
                    return RedirectToAction("IndexChecker", new { id = 2 });
                }
                else if(btn == "Reject")
                {
                    RejectUser(usermaster);
                    return RedirectToAction("IndexChecker", new { id = 2 });
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

                logger.Log(LogLevel.Error, "PasswordReset|" + message + "INNEREXP| " + innerExcp, "Login Index-Msg");
            }
            //return View(usermaster);
            return RedirectToAction("IndexChecker", new { id = 3 });
        }

        [HttpPost]
        public ActionResult EnableUser(UserMaster usermaster)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            //if ((bool)Session["UserManagment"] == false)
            //{
            //    //int uid = (int)Session["uid"];
            //    UserMaster usrm = adc.UserMasters.Find(uid);
            //    usrm.Active = false;
            //    adc.SaveChanges();
            //    return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            //}
            
            if (uid == usermaster.ID)//------If user login and disable user is same
                return RedirectToAction("IndexChecker", new { id = 6 });

            if (usermaster == null)
            {
                return HttpNotFound();
            }
            //var query = (from a in adc.UserMasters
            //             where a.ID == usermaster.ID
            //             select a).SingleOrDefault();
            try
            {
                SqlDataAdapter adp = new SqlDataAdapter("Get_User_Master_Detail", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@Id", SqlDbType.Int).Value = usermaster.ID;

                DataSet ds = new DataSet();
                adp.Fill(ds);
                UserMaster query = new UserMaster();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    query = new UserMaster
                    {
                        ID = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]),
                        IsActive = Convert.ToInt32(ds.Tables[0].Rows[0]["IsActive"]),

                    };
                    
                }

                //----------------- User ActivitiesLogs------
                UserMasterActivity uma = new UserMasterActivity();
                string userAction = "";
                bool userDeleted = false;
                int isActive = query.IsActive;

                if (query != null)
                {
                    if (query.IsActive == 4)
                    {
                        //query.UserDeleted = true;
                        //uma.Action = "User Approved";
                        userAction = "User Approved";
                        userDeleted = true;
                    }
                    else
                    {
                        //query.IsActive = 1;
                        //uma.Action = "User Rejected";
                        userAction = "User Rejected";
                        userDeleted = false;
                        isActive = 1;
                    }

                }

                //uma.ActionBy = sid;
                //uma.Actiondate = DateTime.Now;
                //uma.UserId = usermaster.ID;
                //ulc.UserMasterActivities.Add(uma);

                //adc.SaveChanges();
                //ulc.SaveChanges();

                //----------------- User ActivitiesLogs start ------
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Insert_User_Master_Activities_For_EnableUser";
                cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = usermaster.ID;
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = userAction;
                cmd.Parameters.Add("@ActionBy", SqlDbType.Int).Value = uid;
                cmd.Parameters.Add("@Actiondate", SqlDbType.DateTime).Value = DateTime.Now;

                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                //----------------- User ActivitiesLogs end ------

                //----------------- User Master Update start ------
                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "Update_User_Details_For_EnableUser";
                cmd1.Parameters.Add("@Id", SqlDbType.Int).Value = query.ID;
                cmd1.Parameters.Add("@UserDeleted", SqlDbType.Bit).Value = userDeleted;
                cmd1.Parameters.Add("@IsActive", SqlDbType.Int).Value = isActive;

                cmd1.Connection = con;
                con.Open();
                cmd1.ExecuteNonQuery();
                con.Close();
                con.Dispose();
                //----------------- User Master Update end ------
            }
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                logger.Log(LogLevel.Error, "Disable|" + message + "INNEREXP| " + innerExcp, "Login Index-Msg");
                return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });
            }
            return RedirectToAction("IndexChecker", new { id = 2 });
        }

        [HttpPost]
        public ActionResult RejectUser(UserMaster usermaster)
        {
            //int uid = (int)Session["uid"];
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            //if ((bool)Session["UserManagment"] == false)
            //{
            //    //int uid = (int)Session["uid"];
            //    UserMaster usrm = adc.UserMasters.Find(uid);
            //    usrm.Active = false;
            //    adc.SaveChanges();
            //    return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            //}

            if (uid == usermaster.ID)//------If user login and disable user is same
                return RedirectToAction("IndexChecker", new { id = 6 });

            if (usermaster == null)
            {
                return HttpNotFound();
            }
            //var query = (from a in adc.UserMasters
            //             where a.ID == usermaster.ID
            //             select a).SingleOrDefault();

            try
            {
                SqlDataAdapter adp = new SqlDataAdapter("Get_User_Master_Detail", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@Id", SqlDbType.Int).Value = usermaster.ID;

                DataSet ds = new DataSet();
                adp.Fill(ds);
                UserMaster query = new UserMaster();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    query = new UserMaster
                    {
                        ID = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]),
                        IsActive = Convert.ToInt32(ds.Tables[0].Rows[0]["IsActive"]),

                    };
                }

                //----------------- User ActivitiesLogs------
                UserMasterActivity uma = new UserMasterActivity();
                string userAction = "";
                bool userDeleted = false;
                int isActive = query.IsActive;

                if (query != null)
                {
                    if (query.IsActive == 4)
                    {
                        //query.UserDeleted = false;
                        //query.IsActive = 1;
                        //uma.Action = "User Rejected";
                        userAction = "User Rejected";
                        userDeleted = false;
                        isActive = 1;

                        //----------------- User ActivitiesLogs start ------
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Insert_User_Master_Activities_For_EnableUser";
                        cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = usermaster.ID;
                        cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = userAction;
                        cmd.Parameters.Add("@ActionBy", SqlDbType.Int).Value = uid;
                        cmd.Parameters.Add("@Actiondate", SqlDbType.DateTime).Value = DateTime.Now;

                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        //----------------- User ActivitiesLogs end ------

                        //----------------- User Master Update start ------
                        SqlCommand cmd1 = new SqlCommand();
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.CommandText = "Update_User_Details_For_EnableUser";
                        cmd1.Parameters.Add("@Id", SqlDbType.Int).Value = query.ID;
                        cmd1.Parameters.Add("@UserDeleted", SqlDbType.Bit).Value = userDeleted;
                        cmd1.Parameters.Add("@IsActive", SqlDbType.Int).Value = isActive;

                        cmd1.Connection = con;
                        con.Open();
                        cmd1.ExecuteNonQuery();
                        con.Close();
                        con.Dispose();
                        //----------------- User Master Update end ------
                    }
                    else
                    {
                        //query.IsActive = 3;
                        isActive = 3;

                        //----------------- User Master Update start ------
                        SqlCommand cmd1 = new SqlCommand();
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.CommandText = "Update_User_Details_For_RejectUser";
                        cmd1.Parameters.Add("@Id", SqlDbType.Int).Value = query.ID;
                        //cmd1.Parameters.Add("@UserDeleted", SqlDbType.Bit).Value = userDeleted;
                        cmd1.Parameters.Add("@IsActive", SqlDbType.Int).Value = isActive;

                        cmd1.Connection = con;
                        con.Open();
                        cmd1.ExecuteNonQuery();
                        con.Close();
                        con.Dispose();
                        //----------------- User Master Update end ------
                    }

                }

                //uma.ActionBy = sid;
                //uma.Actiondate = DateTime.Now;
                //uma.UserId = usermaster.ID;
                //ulc.UserMasterActivities.Add(uma);
                //adc.SaveChanges();
                //ulc.SaveChanges();

            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                logger.Log(LogLevel.Error, "Disable|" + message + "INNEREXP| " + innerExcp, "Login Index-Msg");
                return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });
            }
            return RedirectToAction("IndexChecker", new { id = 2 });
        }

        //[HttpPost]
        //public ActionResult EnableUser(UserMaster usermaster)
        //{
        //    int sid = (int)Session["uid"];
        //    if (sid == usermaster.ID)//------If user login and disable user is same
        //        return RedirectToAction("IndexChecker", new { id = 6 });

        //    if (usermaster == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    var query = (from a in adc.UserMasters
        //                 where a.ID == usermaster.ID
        //                 select a).SingleOrDefault();

        //    try
        //    {
        //        //----------------- User ActivitiesLogs------
        //        UserMasterActivity uma = new UserMasterActivity();
        //        if (query != null)
        //        {
        //            if(query.IsActive == 4)
        //            {
        //                query.UserDeleted = true;
        //                uma.Action = "User Approved";
        //            }
        //            else
        //            {
        //                query.IsActive = 1;
        //                uma.Action = "User Rejected";
        //            }
                    
        //        }
                
        //        uma.ActionBy = sid;
        //        uma.Actiondate = DateTime.Now;
        //        uma.UserId = usermaster.ID;
        //        // uma.comments = "Roles[" + allroles + "]";
        //        ulc.UserMasterActivities.Add(uma);
        //        //----------------------------------END-------
        //        adc.SaveChanges();
        //        ulc.SaveChanges();
        //    }
        //    catch (Exception e)
        //    {

        //        string message = "";
        //        string innerExcp = "";
        //        if (e.Message != null)
        //            message = e.Message.ToString();
        //        if (e.InnerException != null)
        //            innerExcp = e.InnerException.Message;

        //        logger.Log(LogLevel.Error, "Disable|" + message + "INNEREXP| " + innerExcp, "Login Index-Msg");
        //        return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });
        //    }
        //    return RedirectToAction("IndexChecker", new { id = 2 });
        //}

        //[HttpPost]
        //public ActionResult RejectUser(UserMaster usermaster)
        //{
        //    int sid = (int)Session["uid"];
        //    if (sid == usermaster.ID)//------If user login and disable user is same
        //        return RedirectToAction("IndexChecker", new { id = 6 });

        //    if (usermaster == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    var query = (from a in adc.UserMasters
        //                 where a.ID == usermaster.ID
        //                 select a).SingleOrDefault();

        //    try
        //    {
        //        //----------------- User ActivitiesLogs------
        //        UserMasterActivity uma = new UserMasterActivity();
        //        if (query != null)
        //        {
        //            if (query.IsActive == 4)
        //            {
        //                query.UserDeleted = false;
        //                query.IsActive = 1;
        //                uma.Action = "User Rejected";
        //            }
        //            else
        //            {
        //                query.IsActive = 3;
        //            }
                    
        //        }
                
        //        uma.ActionBy = sid;
        //        uma.Actiondate = DateTime.Now;
        //        uma.UserId = usermaster.ID;
        //        // uma.comments = "Roles[" + allroles + "]";
        //        ulc.UserMasterActivities.Add(uma);
        //        //----------------------------------END-------
        //        adc.SaveChanges();
        //        ulc.SaveChanges();
        //    }
        //    catch (Exception e)
        //    {
        //        string message = "";
        //        string innerExcp = "";
        //        if (e.Message != null)
        //            message = e.Message.ToString();
        //        if (e.InnerException != null)
        //            innerExcp = e.InnerException.Message;

        //        logger.Log(LogLevel.Error, "Disable|" + message + "INNEREXP| " + innerExcp, "Login Index-Msg");
        //        return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });
        //    }
        //    return RedirectToAction("IndexChecker", new { id = 2 });
        //}
    }
}
