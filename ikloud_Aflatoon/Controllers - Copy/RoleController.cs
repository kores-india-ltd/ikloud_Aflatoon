using ikloud_Aflatoon.Infrastructure;
using ikloud_Aflatoon.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ikloud_Aflatoon.Controllers
{
    public class RoleController : Controller
    {
        //
        AflatoonEntities af = new AflatoonEntities();
        //IWProcDataContext iwpro = new IWProcDataContext();
        //OWProcDataContext OWpro = new OWProcDataContext();
        UserAflatoonDbContext adc = new UserAflatoonDbContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);

        public ActionResult IndexChecker(int id = 0)
        {
            try
            {
                if (Session["uid"] == null)
                {
                    return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
                }
                if ((bool)Session["RoleMasterChecker"] == false)
                {
                    int uid = (int)Session["uid"];
                    UserMaster usrm = af.UserMasters.Find(uid);
                    usrm.Active = false;
                    af.SaveChanges();
                    return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
                }
                if (id == 0)
                {
                    var res = af.RoleMaster.Where(a => a.IsActive == 0).ToList();
                    return View(res);
                }
                else
                {
                    if (id == 2)
                        ViewBag.mesg = "Role successfully approved !";

                    var res = af.RoleMaster.Where(a => a.IsActive == 0).ToList();
                    return View(res);
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
                return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });
            }
        }

        public ActionResult Index(string value = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["role"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            try
            {
                if (value != null)
                {
                    ViewBag.Updated = "Y";
                }
                else
                {
                    ViewBag.Updated = "N";
                }
                var res = af.RoleMaster.ToList();

                return View(res);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;
                return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });
            }
        }

        public ActionResult CreateRole(string value = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["role"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            try
            {
                if (value != null)
                {
                    ViewBag.Success = "Y";
                }
                else
                {
                    ViewBag.Success = "N";
                }
                UserRoles ur = new UserRoles();
                return View(ur);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;
                return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRole(UserRoles userRole)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["role"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            //if ((bool)Session["UserManagment"] == false)
            //{
            //    int uid1 = (int)Session["uid"];
            //    UserMaster usrm = adc.UserMasters.Find(uid1);
            //    usrm.Active = false;
            //    adc.SaveChanges();
            //    return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled", id = 1 });
            //}

            try
            {
                if (ModelState.IsValid)
                {
                    int id = (int)Session["uid"];
                    decimal l1frm = 0;
                    decimal l1to = 0;
                    decimal l2frm = 0;
                    decimal l2to = 0;
                    decimal l3frm = 0;
                    decimal l3to = 0;
                    decimal l4frm = 0;
                    decimal l4to = 0;

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
                    userRole.L1StartLimit = l1frm;
                    userRole.L1StopLimit = l1to;
                    userRole.L2StartLimit = l2frm;
                    userRole.L2StopLimit = l2to;
                    userRole.L3StartLimit = l3frm;
                    userRole.L3StopLimit = l3to;
                    userRole.L4StartLimit = l4frm;
                    userRole.L4StopLimit = l4to;

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
                    string RVF = Request.Form["rvf"];
                    string RVF4 = Request.Form["rvf4"];
                    string CCPH = Request.Form["CCPH"];
                    string um = Request.Form["um"];
                    string sod = Request.Form["sod"];
                    string master = Request.Form["master"];
                    string settg = Request.Form["settg"];
                    string archv = Request.Form["archv"];
                    string mesgbrd = Request.Form["mesgbrd"];
                    string chirjct = Request.Form["chirjct"];
                    string role = Request.Form["role"];
                    string umChecker = Request.Form["umc"];
                    string roleChecker = Request.Form["roleChecker"];

                    //----------- Admin Rights ----------------
                    //string mainAdmin = Request.Form["chkadmin"];

                    if (rc == "on")
                    {
                        userRole.IsRejectRepaire = true;
                    }
                    if (de == "on")
                    {
                        userRole.IsDataEntry = true;
                    }
                    if (qc == "on")
                    {
                        userRole.IsQC = true;
                    }
                    if (vf == "on")
                    {
                        userRole.IsVerification = true;
                    }
                    if (rpt == "on")
                    {
                        userRole.IsReport = true;
                    }
                    if (fildwnd == "on")
                    {
                        userRole.IsFileDownload = true;
                    }
                    if (ds == "on")
                    {
                        userRole.IsDashboard = true;
                    }
                    if (Query == "on")
                    {
                        userRole.IsQuery = true;
                    }
                    if (Querymd == "on")
                    {
                        userRole.IsQueryWithModification = true;
                    }
                    if (RVF == "on")
                    {
                        userRole.IsReVerification = true;
                    }
                    if (RVF4 == "on")
                    {
                        userRole.IsL4Verification = true;
                    }
                    if (CCPH == "on")
                    {
                        userRole.IsCCPH_Verification = true;
                    }
                    if (um == "on")
                    {
                        userRole.IsUserManagement = true;
                    }
                    if (sod == "on")
                    {
                        userRole.IsSOD = true;
                    }
                    if (master == "on")
                    {
                        userRole.IsMasters = true;
                    }
                    if (settg == "on")
                    {
                        userRole.IsSettings = true;
                    }
                    if (archv == "on")
                    {
                        userRole.IsArchieve = true;
                    }
                    if (mesgbrd == "on")
                    {
                        userRole.IsMessageBroadcasting = true;
                    }
                    if (chirjct == "on")
                    {
                        userRole.IsChiReject = true;
                    }
                    if (role == "on")
                    {
                        userRole.IsRoleMaster = true;
                    }
                    if (umChecker == "on")
                    {
                        userRole.IsUserManagementChecker = true;
                    }
                    if (roleChecker == "on")
                    {
                        userRole.IsRoleMasterChecker = true;
                    }

                    //OWpro.CreateUserRole(userRole.RoleName, userRole.IsRejectRepaire, userRole.IsDataEntry, userRole.IsQC, userRole.IsVerification,
                    //    userRole.IsReport, userRole.IsFileDownload, userRole.IsDashboard, userRole.IsQuery, userRole.IsQueryWithModification,
                    //    userRole.IsReVerification, userRole.IsL4Verification, userRole.IsCCPH_Verification, userRole.IsUserManagement, userRole.IsSOD,
                    //    userRole.IsMasters, userRole.IsSettings, userRole.IsArchieve, userRole.IsMessageBroadcasting, userRole.L1StartLimit,
                    //    userRole.L1StopLimit, userRole.L2StartLimit, userRole.L2StopLimit, userRole.L3StartLimit, userRole.L3StopLimit,
                    //    userRole.L4StartLimit, userRole.L4StopLimit, userRole.IsChiReject, id, userRole.IsRoleMaster, userRole.IsUserManagementChecker,
                    //    userRole.IsRoleMasterChecker,0);

                    SqlCommand cmd1 = new SqlCommand("CreateUserRole", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@RoleName", userRole.RoleName);
                    cmd1.Parameters.AddWithValue("@IsRejectRepaire", userRole.IsRejectRepaire);
                    cmd1.Parameters.AddWithValue("@IsDataEntry", userRole.IsDataEntry);
                    cmd1.Parameters.AddWithValue("@IsQC", userRole.IsQC);
                    cmd1.Parameters.AddWithValue("@IsVerification", userRole.IsVerification);
                    cmd1.Parameters.AddWithValue("@IsReport", userRole.IsReport);
                    cmd1.Parameters.AddWithValue("@IsFileDownload", userRole.IsFileDownload);
                    cmd1.Parameters.AddWithValue("@IsDashboard", userRole.IsDashboard);
                    cmd1.Parameters.AddWithValue("@IsQuery", userRole.IsQuery);
                    cmd1.Parameters.AddWithValue("@IsQueryWithModification", userRole.IsQueryWithModification);
                    cmd1.Parameters.AddWithValue("@IsReVerification", userRole.IsReVerification);
                    cmd1.Parameters.AddWithValue("@IsL4Verification", userRole.IsL4Verification);
                    cmd1.Parameters.AddWithValue("@IsCCPH_Verification", userRole.IsCCPH_Verification);
                    cmd1.Parameters.AddWithValue("@IsUserManagement", userRole.IsUserManagement);
                    cmd1.Parameters.AddWithValue("@IsSOD", userRole.IsSOD);
                    cmd1.Parameters.AddWithValue("@IsMasters", userRole.IsMasters);
                    cmd1.Parameters.AddWithValue("@IsSettings", userRole.IsSettings);

                    cmd1.Parameters.AddWithValue("@IsArchieve", userRole.IsArchieve);
                    cmd1.Parameters.AddWithValue("@IsMessageBroadcasting", userRole.IsMessageBroadcasting);
                    cmd1.Parameters.AddWithValue("@L1StartLimit", userRole.L1StartLimit);
                    cmd1.Parameters.AddWithValue("@L1StopLimit", userRole.L1StopLimit);
                    cmd1.Parameters.AddWithValue("@L2StartLimit", userRole.L2StartLimit);
                    cmd1.Parameters.AddWithValue("@L2StopLimit", userRole.L2StopLimit);
                    cmd1.Parameters.AddWithValue("@L3StartLimit", userRole.L3StartLimit);
                    cmd1.Parameters.AddWithValue("@L3StopLimit", userRole.L3StopLimit);
                    cmd1.Parameters.AddWithValue("@L4StartLimit", userRole.L4StartLimit);
                    cmd1.Parameters.AddWithValue("@L4StopLimit", userRole.L4StopLimit);
                    cmd1.Parameters.AddWithValue("@IsChiReject", userRole.IsChiReject);
                    cmd1.Parameters.AddWithValue("@UserId", id);
                    cmd1.Parameters.AddWithValue("@IsRoleMaster", userRole.IsRoleMaster);
                    cmd1.Parameters.AddWithValue("@IsUserManagementChecker", userRole.IsUserManagementChecker);
                    cmd1.Parameters.AddWithValue("@IsRoleMasterChecker", userRole.IsRoleMasterChecker);
                    cmd1.Parameters.AddWithValue("@IsActive", 0);


                    con.Open();

                    cmd1.ExecuteNonQuery();
                    con.Close();

                    ViewBag.Success = "Y";
                    return RedirectToAction("CreateRole", new { value = ViewBag.Success });
                }
                ViewBag.Success = "N";
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;
                return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });
            }
            ViewBag.Success = "N";
            return View(userRole);
        }

        public ActionResult Edit(int id = 0)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["role"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            try
            {
                var result = (from p in af.RoleMaster
                              where p.ID == id
                              select p).SingleOrDefault();
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
                return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });
            }
        }

        [HttpPost]
        public ActionResult Edit(RoleMaster userRole)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["role"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            //if ((bool)Session["UserManagment"] == false)
            //{
            //    int uid1 = (int)Session["uid"];
            //    UserMaster usrm = adc.UserMasters.Find(uid1);
            //    usrm.Active = false;
            //    adc.SaveChanges();
            //    return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled", id = 1 });
            //}

            try
            {
                if (ModelState.IsValid)
                {
                    int id = (int)Session["uid"];
                    decimal l1frm = 0;
                    decimal l1to = 0;
                    decimal l2frm = 0;
                    decimal l2to = 0;
                    decimal l3frm = 0;
                    decimal l3to = 0;
                    decimal l4frm = 0;
                    decimal l4to = 0;

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
                    userRole.L1StartLimit = l1frm;
                    userRole.L1StopLimit = l1to;
                    userRole.L2StartLimit = l2frm;
                    userRole.L2StopLimit = l2to;
                    userRole.L3StartLimit = l3frm;
                    userRole.L3StopLimit = l3to;
                    userRole.L4StartLimit = l4frm;
                    userRole.L4StopLimit = l4to;

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
                    string RVF = Request.Form["rvf"];
                    string RVF4 = Request.Form["rvf4"];
                    string CCPH = Request.Form["CCPH"];
                    string um = Request.Form["um"];
                    string sod = Request.Form["sod"];
                    string master = Request.Form["masters"];
                    string settg = Request.Form["settg"];
                    string archv = Request.Form["archv"];
                    string mesgbrd = Request.Form["mesgbrd"];
                    string chirjct = Request.Form["chirjct"];
                    string role = Request.Form["role"];
                    string umChecker = Request.Form["umc"];
                    string roleChecker = Request.Form["roleChecker"];

                    //----------- Admin Rights ----------------
                    //string mainAdmin = Request.Form["chkadmin"];

                    if (rc == "on")
                    {
                        userRole.IsRejectRepaire = true;
                    }
                    if (de == "on")
                    {
                        userRole.IsDataEntry = true;
                    }
                    if (qc == "on")
                    {
                        userRole.IsQC = true;
                    }
                    if (vf == "on")
                    {
                        userRole.IsVerification = true;
                    }
                    if (rpt == "on")
                    {
                        userRole.IsReport = true;
                    }
                    if (fildwnd == "on")
                    {
                        userRole.IsFileDownload = true;
                    }
                    if (ds == "on")
                    {
                        userRole.IsDashboard = true;
                    }
                    if (Query == "on")
                    {
                        userRole.IsQuery = true;
                    }
                    if (Querymd == "on")
                    {
                        userRole.IsQueryWithModification = true;
                    }
                    if (RVF == "on")
                    {
                        userRole.IsReVerification = true;
                    }
                    if (RVF4 == "on")
                    {
                        userRole.IsL4Verification = true;
                    }
                    if (CCPH == "on")
                    {
                        userRole.IsCCPH_Verification = true;
                    }
                    if (um == "on")
                    {
                        userRole.IsUserManagement = true;
                    }
                    if (sod == "on")
                    {
                        userRole.IsSOD = true;
                    }
                    if (master == "on")
                    {
                        userRole.IsMasters = true;
                    }
                    if (settg == "on")
                    {
                        userRole.IsSettings = true;
                    }
                    if (archv == "on")
                    {
                        userRole.IsArchieve = true;
                    }
                    if (mesgbrd == "on")
                    {
                        userRole.IsMessageBroadcasting = true;
                    }
                    if (chirjct == "on")
                    {
                        userRole.IsChiReject = true;
                    }
                    if (role == "on")
                    {
                        userRole.IsRoleMaster = true;
                    }
                    if (umChecker == "on")
                    {
                        userRole.IsUserManagementChecker = true;
                    }
                    if (roleChecker == "on")
                    {
                        userRole.IsRoleMasterChecker = true;
                    }

                    //OWpro.UpdateUserRole(userRole.ID, userRole.RoleName, userRole.IsRejectRepaire, userRole.IsDataEntry, userRole.IsQC, userRole.IsVerification,
                    //    userRole.IsReport, userRole.IsFileDownload, userRole.IsDashboard, userRole.IsQuery, userRole.IsQueryWithModification,
                    //    userRole.IsReVerification, userRole.IsL4Verification, userRole.IsCCPH_Verification, userRole.IsUserManagement, userRole.IsSOD,
                    //    userRole.IsMasters, userRole.IsSettings, userRole.IsArchieve, userRole.IsMessageBroadcasting, userRole.L1StartLimit,
                    //    userRole.L1StopLimit, userRole.L2StartLimit, userRole.L2StopLimit, userRole.L3StartLimit, userRole.L3StopLimit,
                    //    userRole.L4StartLimit, userRole.L4StopLimit, userRole.IsChiReject, id, userRole.IsRoleMaster, userRole.IsUserManagementChecker,
                    //    userRole.IsRoleMasterChecker,0);

                    SqlCommand cmd1 = new SqlCommand("UpdateUserRole", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@ID", userRole.ID);
                    cmd1.Parameters.AddWithValue("@RoleName", userRole.RoleName);
                    cmd1.Parameters.AddWithValue("@IsRejectRepaire", userRole.IsRejectRepaire);
                    cmd1.Parameters.AddWithValue("@IsDataEntry", userRole.IsDataEntry);
                    cmd1.Parameters.AddWithValue("@IsQC", userRole.IsQC);
                    cmd1.Parameters.AddWithValue("@IsVerification", userRole.IsVerification);
                    cmd1.Parameters.AddWithValue("@IsReport", userRole.IsReport);
                    cmd1.Parameters.AddWithValue("@IsFileDownload", userRole.IsFileDownload);
                    cmd1.Parameters.AddWithValue("@IsDashboard", userRole.IsDashboard);
                    cmd1.Parameters.AddWithValue("@IsQuery", userRole.IsQuery);
                    cmd1.Parameters.AddWithValue("@IsQueryWithModification", userRole.IsQueryWithModification);
                    cmd1.Parameters.AddWithValue("@IsReVerification", userRole.IsReVerification);
                    cmd1.Parameters.AddWithValue("@IsL4Verification", userRole.IsL4Verification);
                    cmd1.Parameters.AddWithValue("@IsCCPH_Verification", userRole.IsCCPH_Verification);
                    cmd1.Parameters.AddWithValue("@IsUserManagement", userRole.IsUserManagement);
                    cmd1.Parameters.AddWithValue("@IsSOD", userRole.IsSOD);
                    cmd1.Parameters.AddWithValue("@IsMasters", userRole.IsMasters);
                    cmd1.Parameters.AddWithValue("@IsSettings", userRole.IsSettings);

                    cmd1.Parameters.AddWithValue("@IsArchieve", userRole.IsArchieve);
                    cmd1.Parameters.AddWithValue("@IsMessageBroadcasting", userRole.IsMessageBroadcasting);
                    cmd1.Parameters.AddWithValue("@L1StartLimit", userRole.L1StartLimit);
                    cmd1.Parameters.AddWithValue("@L1StopLimit", userRole.L1StopLimit);
                    cmd1.Parameters.AddWithValue("@L2StartLimit", userRole.L2StartLimit);
                    cmd1.Parameters.AddWithValue("@L2StopLimit", userRole.L2StopLimit);
                    cmd1.Parameters.AddWithValue("@L3StartLimit", userRole.L3StartLimit);
                    cmd1.Parameters.AddWithValue("@L3StopLimit", userRole.L3StopLimit);
                    cmd1.Parameters.AddWithValue("@L4StartLimit", userRole.L4StartLimit);
                    cmd1.Parameters.AddWithValue("@L4StopLimit", userRole.L4StopLimit);
                    cmd1.Parameters.AddWithValue("@IsChiReject", userRole.IsChiReject);
                    cmd1.Parameters.AddWithValue("@UserId", id);
                    cmd1.Parameters.AddWithValue("@IsRoleMaster", userRole.IsRoleMaster);
                    cmd1.Parameters.AddWithValue("@IsUserManagementChecker", userRole.IsUserManagementChecker);
                    cmd1.Parameters.AddWithValue("@IsRoleMasterChecker", userRole.IsRoleMasterChecker);
                    cmd1.Parameters.AddWithValue("@IsActive", 0);


                    con.Open();

                    cmd1.ExecuteNonQuery();
                    con.Close();

                    ViewBag.Updated = "Y";
                    return RedirectToAction("Index", new { value = ViewBag.Updated });
                }
                ViewBag.Updated = "N";
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;
                return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });
            }
            ViewBag.Updated = "N";
            return View(userRole);
        }

        [HttpPost]
        public ActionResult ApproveRole(Int64 id, string btn = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            
            if ((bool)Session["RoleMasterChecker"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                adc.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            try
            {
                int seeionuid = (int)Session["uid"];
                RoleMaster rolemaster = af.RoleMaster.Find(id);
                if (rolemaster == null)
                {
                    return HttpNotFound();
                }
                if (btn == "Approve")
                {
                    //EnableRole(rolemaster);
                    rolemaster.IsActive = 1;
                    af.SaveChanges();
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
                return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });
            }
            //return View(usermaster);
            return RedirectToAction("IndexChecker", new { id = 3 });
        }

        //[HttpPost]
        //public ActionResult EnableRole(RoleMaster rolemaster)
        //{

        //}
    }
}
