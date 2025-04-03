using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ikloud_Aflatoon.Models;
using System.Web.Security;
using System.Security.Cryptography;

using System.Text.RegularExpressions;

using ikloud_Aflatoon;
using ikloud_Aflatoon.Infrastructure;
using System.Configuration;
using System.Web.Configuration;
using System.Globalization;
using System.IO;
using System.Collections;

namespace ikloud_Aflatoon.Controllers
{
    //[OutputCache(Duration = 0)]
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        private UserAflatoonDbContext db = new UserAflatoonDbContext();
        private AflatoonEntities udb = new AflatoonEntities();

        private UserLogDbContext usrlg = new UserLogDbContext();
        CommonFunction cmf = new CommonFunction();

        public ActionResult Index(int id = 0)
        {
            //ProtectSection("connectionStrings", "RSAProtectedConfigurationProvider");
            //  decrpt("connectionStrings", "RSAProtectedConfigurationProvider");
            try
            {

                if (TempData["mesag"] != null)
                {
                    FormsAuthentication.SignOut();
                    Session.Clear();
                    Session.Abandon();
                    Session.RemoveAll();
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                    Response.Cache.SetNoStore();
                    FormsAuthentication.SignOut();

                    if (Session.IsCookieless == false)
                    {
                        Session.Clear();
                        Session.Abandon();
                        Session.RemoveAll();
                        Session.SessionID.Remove(0);
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                        Response.Cache.SetNoStore();
                        FormsAuthentication.SignOut();
                    }
                    ViewBag.sessionout = "Your session has been expired!!";
                }
                if (id == 10)
                    ViewBag.sessionout = "Your session has been expired!!";

                if (id == 3)
                {
                    ViewBag.disable = true;
                    ViewBag.meg = "Password has been change successfully !";
                }
                if (TempData["changemsg"] != null)
                {
                    ViewBag.disable = true;
                    ViewBag.meg = "Oops! Your id has been disable, Please contact to administrator !";
                    FormsAuthentication.SignOut();

                    if (Session.IsCookieless == false)
                    {
                        Session.Clear();
                        Session.Abandon();
                        Session.RemoveAll();
                        Session.SessionID.Remove(0);
                    }
                }

                if (Session.IsNewSession)
                {
                    FormsAuthentication.SignOut();
                    // Session.Clear();

                    Session.Remove("SessionID");
                    Session.RemoveAll();
                    Session.Abandon();
                    Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
                }


                return View();
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "Login HttpGet Index-" + innerExcp });
            }
        }
        private void ProtectSection(string sectionName, string provider)
        {
            Configuration config =
                WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
            ConfigurationSection section = config.GetSection(sectionName);
            try
            {
                if (section != null &&
                          !section.SectionInformation.IsProtected)
                {
                    config.ConnectionStrings.SectionInformation.ForceSave = true;
                    section.SectionInformation.ProtectSection(provider);
                    config.Save(ConfigurationSaveMode.Full);
                }
                //
                //if (_configuration.ConnectionStrings.ConnectionStrings[Application.ProductName] == null)
                //{
                //    _configuration.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings(Application.ProductName,
                //                                                                                        _connStringBuilder.ToString()));
                //}
                //else
                //{
                //    _configuration.ConnectionStrings.ConnectionStrings[Application.ProductName].ConnectionString = _connStringBuilder.ToString();
                //}

                //if (!_configuration.ConnectionStrings.SectionInformation.IsProtected)
                //{
                //    _configuration.ConnectionStrings.SectionInformation.ForceSave = true;
                //    _configuration.ConnectionStrings.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                //}

                //_configuration.Save(ConfigurationSaveMode.Minimal);
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.ToString() + "\n" + ex.StackTrace);
            }
            //
        }
        /// <summary>
        /// 
        public void decrpt(string sectionName, string provider)
        {
            try
            {
                //read exe config file
                Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);

                //read section that needs to be unprotected
                ConfigurationSection section = config.GetSection(sectionName);

                //check if section is protected already
                if (section.SectionInformation.IsProtected)
                {
                    //call UnprotectSection() method to decrypt section data
                    section.SectionInformation.UnprotectSection();


                    //force the associated configuration section to apear on configuration file
                    section.SectionInformation.ForceDeclaration(true);

                    //force to update the changes on disk
                    section.SectionInformation.ForceSave = true;

                    //save the change
                    config.Save(ConfigurationSaveMode.Full);

                }

            }
            catch (Exception ex)
            {

                string errMessage = ex.Message;

            }
        }
        /// 
        /// </summary>
        /// <param name="AppStr"></param>
        /// <returns></returns>


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Index(AppStResult AppStr = null, string OW = null)
        {

            try
            {
                string form = "dd-MM-yyyy hh:mm:ss";
                if (ModelState.IsValid)
                {
                    //System.IO.File.AppendAllText("C:\\temp\\log1.txt", "model state is valid");

                    Session["clearingtype"] = Request.Form["clearing"].ToString();
                    LoginLogoutAudit linlout = new LoginLogoutAudit();

                    string format = "yyyy-MM-dd hh:mm:ss";
                    string commdate = DateTime.Now.ToString(format);
                    int invlidatpass, invldpasspar;

                    //////-------------------------Chnages on 06/05/2017------------
                    string midelpass = "";
                    //string midelname = "";
                    //----------------------------Pass---------------
                    for (int i = 0; i < AppStr.pass.Length; i++)
                    {
                        midelpass = midelpass + Convert.ToChar((int)AppStr.pass[i] + 13);
                    }
                    //////----------------------------Name---------------
                    //for (int i = 0; i < AppStr.name.Length; i++)
                    //{
                    //    midelname = midelname + Convert.ToChar((int)AppStr.name[i] + 13);
                    //}
                    ////-------------------------
                    AppStr.pass = midelpass;
                    //AppStr.name = midelname;
                    //----------With Encryp------------------------------
                    string finpass = cmf.EncryptPassword(AppStr.pass);
                    //----------Without Encryp------------------------------
                    //string finpass = AppStr.pass;

                    //System.IO.File.AppendAllText("C:\\temp\\log1.txt", "password:" + finpass);

                    //-------------------Added On 22-03-2017--------------
                    var model = db.UserMasters.Where(m => m.LoginID == AppStr.name).SingleOrDefault();
                    if (model == null)
                    {
                        ModelState.AddModelError("", "Oops! userid/password is wrong");
                        return View("Index");
                    }
                    //model = db.UserMasters.Where(m => m.LoginID == AppStr.name).SingleOrDefault();
                    if (model != null)
                    {
                        if (model.Active == false)
                        {
                            ModelState.AddModelError("", "Oops! Your id has been disable, Please contact to administrator !");
                            return View("Index");
                        }
                    }
                    //
                    model = db.UserMasters.Where(m => m.LoginID == AppStr.name && m.Password == finpass).SingleOrDefault();
                    if (model != null)
                    {
                        //System.IO.File.AppendAllText("C:\\temp\\log1.txt", "after password match");

                        ViewBag.name = model.Title + " " + model.FirstName;
                        ViewBag.wrong = " ";
                        var usmtr = db.UserMasters.Where(u => u.LoginID == AppStr.name).SingleOrDefault();

                        if (usmtr != null)
                        {

                            //System.IO.File.AppendAllText("C:\\temp\\log1.txt", "user found");

                            //if (usmtr.loginFlg == 1)
                            //{                         
                            //    ModelState.AddModelError("", "Oops! Multiple logins are not allowed!");
                            //    return View("Index");
                            //}
                            //else
                            //{
                            //----------- If User login after long time------------
                            if (usmtr.Active == false)
                            {
                                ModelState.AddModelError("", "Oops! Your id has been disable, Please contact to administrator !");
                                return View("Index");
                            }
                            //------------------26-04-2017-----Added Session--------
                            // Hashtable sessions = (Hashtable)Application["WEB_SESSIONS_OBJECT"];

                            if (usmtr.FirstLogin == true)
                            {
                                linlout.User_ID = model.ID;
                                linlout.LoginDateTime = Convert.ToDateTime(commdate);
                                linlout.LogoutDateTime = DateTime.Now;

                                Session["logintime"] = commdate;

                                usrlg.LoginLogoutAudits.Add(linlout);
                                // string sid = Session.SessionID;
                                Session["uid"] = model.ID;
                                Session["LoginID"] = model.LoginID;
                                //    Session["lastlogin"] = loginout.LoginDateTime.ToString(form);
                                usmtr.InvalidPasswordAttempts = 0;
                                db.SaveChanges();
                                //FormsAuthentication.SetAuthCookie(model.LoginID, false);
                                //return RedirectToAction("Index", "Home",new{id=5});
                                return RedirectToAction("ChangePassword");
                            }

                            var lastpaswrd = (from ph in usrlg.PasswordHistories
                                              orderby ph.ID descending
                                              where ph.User_ID == model.ID
                                              select ph).FirstOrDefault();
                            var polcy = db.AppSecPolicies.Where(m => m.ID == model.AppSecPolicieID).SingleOrDefault();

                            if (lastpaswrd != null)
                            {
                                DateTime demidate = Convert.ToDateTime(lastpaswrd.PassworChangeDate);
                                TimeSpan ts = DateTime.Now.Subtract(demidate);
                                int intBounceentryCnt = ts.Days;
                                if (intBounceentryCnt > polcy.PwdExpiryDays)
                                {
                                    Session["uid"] = model.ID;
                                    Session["LoginID"] = model.LoginID;
                                    //   Session["lastlogin"] = loginout.LoginDateTime.ToString(form);
                                    //FormsAuthentication.SetAuthCookie(model.LoginID, false);
                                    usmtr.InvalidPasswordAttempts = 0;
                                    //Session["uid"] = model.ID;
                                    db.SaveChanges();
                                    return RedirectToAction("ChangePassword");
                                }
                            }
                            if (usmtr.LastLogin != null)
                            {
                                DateTime demidate = Convert.ToDateTime(usmtr.LastLogin);
                                TimeSpan ts = DateTime.Now.Subtract(demidate);
                                int intBounceentryCnt = ts.Days;
                                if (intBounceentryCnt > 0)
                                {
                                    if (intBounceentryCnt > polcy.DeactivationDays)
                                    {
                                        usmtr.Active = false;
                                        db.SaveChanges();
                                        ModelState.AddModelError("", "Oops! Your id has been disable, Please contact to administrator !");
                                        return View("Index");
                                    }
                                }
                            }
                            /////---Commented oN 22-02-2017----
                            //usmtr.sessionid = Session.SessionID;
                            //usmtr.loginFlg = 1;
                            //usmtr.InvalidPasswordAttempts = 0;
                            //usmtr.LastLogin = DateTime.Now;
                            //}
                        }
                        var loginout = usrlg.LoginLogoutAudits.Where(m => m.User_ID == model.ID).OrderByDescending(m => m.ID).FirstOrDefault();
                        if (loginout != null)
                        {

                            Session["lastlogin"] = loginout.LoginDateTime.ToString(form);

                        }

                        linlout.User_ID = model.ID;
                        linlout.LoginDateTime = Convert.ToDateTime(commdate);
                        linlout.LogoutDateTime = DateTime.Now;

                        Session["logintime"] = commdate;

                        usrlg.LoginLogoutAudits.Add(linlout);
                        // string sid = Session.SessionID;
                        usrlg.SaveChanges();
                        Session["LoginID"] = model.LoginID;
                        Session["uid"] = model.ID;
                        Session["title"] = model.Title;
                        Session["fname"] = model.FirstName;
                        Session["LgnName"] = model.LoginID;
                        Session.Timeout = 300;

                        CheckRole chkroles1 = new CheckRole(model.ID);
                        Session["DE"] = chkroles1.DE;
                        Session["Ds"] = chkroles1.Ds;
                        Session["fildwnd"] = chkroles1.fildwnd;
                        Session["QC"] = chkroles1.QC;
                        Session["QueryMod"] = chkroles1.QueryMod;
                        Session["RejectRepair"] = chkroles1.RejectRepair;
                        Session["Report"] = chkroles1.Report;
                        Session["UserManagment"] = chkroles1.UserManagment;
                        Session["VF"] = chkroles1.VF;
                        Session["Archv"] = chkroles1.Archv;
                        Session["Master"] = chkroles1.Master;
                        Session["Mesgbrd"] = chkroles1.Mesgbrd;
                        Session["Query"] = chkroles1.Query;
                        Session["Settg"] = chkroles1.Settg;
                        Session["SOD"] = chkroles1.SOD;
                        Session["chirjct"] = chkroles1.chirjct;
                        Session["RVF"] = chkroles1.RVF;
                        Session["CCPH"] = chkroles1.CCPH;
                        Session["ProType"] = Request.Form["clearing"];



                        //FormsAuthentication.SetAuthCookie(Session["uid"].ToString(), false);
                        FormsAuthentication.SetAuthCookie(model.LoginID, false);

                        //uncomment the foraeach in producation dll
                        //------------------------------------------------------------------------------------------------------------
                        foreach (string s in Response.Cookies.AllKeys)
                        {
                            if (s == FormsAuthentication.FormsCookieName || s.ToLower() == "asp.net_sessionid")
                            {
                                Response.Cookies[s].Secure = true;
                            }
                        }
                        //------------------------------------------------------------------------------------------------------------

                        //FormsAuthentication.Encrypt();

                        //checkLockRecord(model.ID);// Check Lock records -----------------------
                        //------------Checking previous Login-------------
                        if (Session["uid"] == null)
                            ViewBag.userlogin = "false";

                        bool userlogin = true;
                        int uid = (int)Session["uid"];
                        //var model1 = db.UserMasters.Find(uid);


                        if (model.sessionid != null)
                        {
                            if (model.sessionid != Session.SessionID)
                            {
                                // usmtr.sessionid = Session.SessionID;
                                userlogin = false;
                            }
                            else
                            {

                                userlogin = true;
                                usmtr.sessionid = Session.SessionID;
                                usmtr.loginFlg = 1;
                                usmtr.InvalidPasswordAttempts = 0;
                                Session["afterlogin"] = true;
                                db.SaveChanges();

                                //System.IO.File.AppendAllText("C:\\temp\\log1.txt", "session id found");
                            }
                        }
                        else
                        {

                            userlogin = true;
                            usmtr.sessionid = Session.SessionID;
                            usmtr.loginFlg = 1;
                            usmtr.InvalidPasswordAttempts = 0;
                            Session["afterlogin"] = true;
                            db.SaveChanges();
                        }

                        //if (Request.Form["clearing"].ToString() == "Outward")
                        //{
                        //    // DateTime procdate;// = new DateTime();
                        //    string[] datewithcust = Request.Form["ProcDate"].ToString().Split('_');
                        //    string tempdate = datewithcust[0].ToString();

                        //    int selectedcustid = Convert.ToInt16(datewithcust[2]);

                        //    // procdate =Convert.ToDateTime(tempdate);

                        //    DateTime procdate = DateTime.ParseExact(tempdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        //    var owsods = udb.OwSoDs.Where(m => m.ProcessingDate == procdate && m.CustomerId == selectedcustid).FirstOrDefault();
                        //    if (owsods != null)
                        //    {
                        //        Session["PostDate"] = owsods.PostDated;
                        //        Session["StaleDate"] = owsods.StaleDated;
                        //        Session["CustomerID"] = owsods.CustomerId;
                        //    }
                        //    var domainseting = (from d in udb.DomainUserMapMaster
                        //                        from dm in udb.DomainMaster
                        //                        where d.DomainId == dm.Id && d.UserId == model.ID
                        //                        select new { d.CustomerID, dm.Id, dm.Name }
                        //                            ).FirstOrDefault();
                        //    //udb.DomainUserMapMaster.Where(d => d.UserId == model.ID).SingleOrDefault();
                        //    var appsettingsACFrom = udb.ApplicationSettings.Where(m => m.CustomerId == domainseting.CustomerID && m.SettingName == "ACFrom").SingleOrDefault();
                        //    var appsettingsACTo = udb.ApplicationSettings.Where(m => m.CustomerId == domainseting.CustomerID && m.SettingName == "ACTo").SingleOrDefault();
                        //    var Sniptsetting = udb.ApplicationSettings.Where(m => m.CustomerId == domainseting.CustomerID && m.SettingName == "SnippetSettings").SingleOrDefault();
                        //    var CommonSetHigVl = udb.ApplicationSettings.Where(a => a.CustomerId == domainseting.CustomerID && a.SettingName == "HighValue").FirstOrDefault();
                        //    var DEBySnippet = udb.ApplicationSettings.Where(a => a.CustomerId == domainseting.CustomerID && a.SettingName == "SnippetSettings").FirstOrDefault();

                        //    var GetAccountDetailsV = udb.ApplicationSettings.Where(a => a.CustomerId == domainseting.CustomerID && a.SettingName == "GetAccountDetails").FirstOrDefault();
                        //    Session["GetAccountDetails "] = GetAccountDetailsV.SettingValue;

                        //    var NarrationV = udb.ApplicationSettings.Where(a => a.CustomerId == domainseting.CustomerID && a.SettingName == "Narration").FirstOrDefault();
                        //    Session["Narration"] = NarrationV.SettingValue;

                        //    var CreditCardValidationReq = udb.ApplicationSettings.Where(a => a.CustomerId == domainseting.CustomerID && a.SettingName == "CreditCardValidationRequired").FirstOrDefault();
                        //    Session["CreditCardValidationReq"] = CreditCardValidationReq.SettingValue;

                        //    var AllowACAlpha = udb.ApplicationSettings.Where(a => a.CustomerId == domainseting.CustomerID && a.SettingName == "ACAlphaAllow").FirstOrDefault();
                        //    if (AllowACAlpha != null)
                        //        Session["ACAlphaAllow"] = AllowACAlpha.SettingValue;

                        //    if (CreditCardValidationReq.SettingValue == "1")
                        //    {
                        //        var CreditCardValidAcNo = udb.ApplicationSettings.Where(a => a.CustomerId == domainseting.CustomerID && a.SettingName == "CreditCardValidAcNo").FirstOrDefault();
                        //        Session["CreditCardValidAcNo"] = CreditCardValidAcNo.SettingValue;
                        //        var CreditCardInValidAcNo = udb.ApplicationSettings.Where(a => a.CustomerId == domainseting.CustomerID && a.SettingName == "CreditCardInValidAcNo").FirstOrDefault();
                        //        Session["CreditCardInValidAcNo"] = CreditCardInValidAcNo.SettingValue;
                        //    }

                        //    if (DEBySnippet != null)
                        //        Session["OWDEBySnippet"] = DEBySnippet.SettingValue;

                        //    if (CommonSetHigVl != null)
                        //        Session["HIGHAMT"] = CommonSetHigVl.SettingValue;
                        //    else
                        //        Session["HIGHAMT"] = 0;

                        //    Session["blnDeBySnippet"] = Sniptsetting.SettingValue;
                        //    Session["acfrm"] = appsettingsACFrom.SettingValue;
                        //    Session["acto"] = appsettingsACTo.SettingValue;
                        //    Session["domainid"] = domainseting.Id;
                        //    Session["domainname"] = domainseting.Name;
                        //    Session["processdate"] = procdate;//dt.ToString("dd/MM/yyyy");

                        //    string[] ddt = new string[0];
                        //    string dtemp = Session["processdate"].ToString();
                        //    ddt = dtemp.Split('/');
                        //    Session["SnipDate"] = procdate.ToString("dd/MM/yyyy");//"04.06.2016";//ddt[1]+"."+ ddt[0]+"." + ddt[2];

                        //    @Session["glob"] = true;
                        //    db.SaveChanges();
                        //    if (userlogin == false)
                        //        TempData["flg"] = userlogin;

                        //    return RedirectToAction("IWIndex", "Home");
                        //}
                        //else if (Request.Form["clearing"].ToString() == "Inward")
                        //{
                        //    db.SaveChanges();
                        //    if (userlogin == false)
                        //        TempData["flg"] = userlogin;

                        //    return RedirectToAction("SelectDomain", "DomainSelection");
                        //}
                        Session["Accesslevel"] = usmtr.AccessLevel;
                        Session["CurrentDataEntryCount"] = 0;

                        var CommonSet = udb.CommonSettings.Where(a => a.AppName == "CTSCONFIG1" && a.SettingName == "AccValidation").FirstOrDefault();
                        if (CommonSet != null)
                            Session["AccValidation"] = CommonSet.SettingValue;

                        return RedirectToAction("CustDomDateSelection", "Login", new { Accesslevel = usmtr.AccessLevel, userlogin = userlogin });
                        //  return View();
                    }
                    else
                    {
                        var usmtr = db.UserMasters.Where(u => u.LoginID == AppStr.name).SingleOrDefault();

                        //System.IO.File.AppendAllText("C:\\temp\\log1.txt", "in else condition");

                        if (usmtr != null)
                        {
                            var polcy1 = db.AppSecPolicies.Where(m => m.ID == usmtr.AppSecPolicieID).SingleOrDefault();
                            invlidatpass = usmtr.InvalidPasswordAttempts;
                            invldpasspar = polcy1.InvalidAttemptsAllowed;
                            if (invlidatpass >= invldpasspar)
                            {
                                usmtr.Active = false;
                                db.SaveChanges();
                                ModelState.AddModelError("", "Oops! User Id has been disable, Please contact to administrator !!");
                                return View("Index");
                            }
                            else
                            {
                                usmtr.InvalidPasswordAttempts = usmtr.InvalidPasswordAttempts + 1;
                                db.SaveChanges();
                            }
                        }

                        ModelState.AddModelError("", "Oops! User Id or Password Wrong!");
                        //ViewBag.wrong = "wrong";
                        return View("Index");
                    }
                }
                else
                {
                    //System.IO.File.AppendAllText("C:\\temp\\log1.txt", "model state is invalid");

                    ModelState.AddModelError("", "Oops! User Id or Password Wrong!");
                    return View("Index");
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

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "Login HttpPost Index- " + innerExcp });
            }

        }

        // [HttpPost]
        public JsonResult _SelectedCustomers(int id = 0)
        {
            List<string> customerlist = new List<string>();
            try
            {

                int uid = (int)Session["uid"];
                var result = (from a in udb.CustomerMasters
                              from d in udb.DomainUserMapMasters
                              where a.Id == d.CustomerID && a.OrganizationId == id && d.UserId == uid
                              select new
                              {
                                  a.Name,
                                  a.Id
                              }).ToList().Distinct().OrderBy(m => m.Id);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string ServerPath = "";
                string filename = "";
                string fileNameWithPath = "";

                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                ErrorDisplay err = new ErrorDisplay();

                ServerPath = Server.MapPath("~/Logs/");
                if (System.IO.Directory.Exists(ServerPath) == false)
                {
                    System.IO.Directory.CreateDirectory(ServerPath);
                }
                filename = DateTime.Now.ToString("ddMMyyyy") + "Logs.txt";
                fileNameWithPath = ServerPath + filename;
                StreamWriter str = new StreamWriter(fileNameWithPath, true, System.Text.Encoding.Default);
                //  str.WriteLine("hello");
                str.WriteLine(DateTime.Now.ToShortTimeString() + ": Exception: " + message);
                str.WriteLine(DateTime.Now.ToShortTimeString() + ": StackTrace: " + innerExcp);
                str.Close();

                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult _GetBranches(int id = 0)
        {
            var result1 = (dynamic)null;

            if (Session["ProType"].ToString() == "Outward")
            {

                result1 = (from a in udb.BranchMaster
                           where a.OwDomainId == id
                           select new
                           {
                               a.BranchName,
                               a.Id
                           }).ToList().Distinct().OrderByDescending(m => m.BranchName);
            }

            //var Alldata = new { procdate = procllist, domlst = domainlistfinal };


            return Json(result1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult _SelectedProcDate(int id = 0)
        {
            List<string> datedrop = new List<string>();

            List<string> domainlist = new List<string>();
            int uid = (int)Session["uid"];

            if (Session["ProType"].ToString() == "Outward")
            {
                var result = (from a in udb.OwSoDs
                              where a.EoDStatus == 0 && a.CustomerId == id
                              select new
                              {
                                  a.ProcessingDate,
                                  a.CustomerId
                              }).ToList().Distinct().OrderByDescending(m => m.ProcessingDate);


                //---------------------------Domain-----------------
                var resultdomain = (from du in udb.DomainUserMapMasters
                                    from d in udb.DomainMaster
                                    //from c in udb.CustomerMasters

                                    where du.DomainId == d.Id && du.CustomerID == id && du.UserId == uid //a.EoDStatus == 0 && a.CustomerId == id
                                    select new
                                    {
                                        d.Id,
                                        d.Name
                                    }).ToList().Distinct().OrderByDescending(m => m.Id);
                ///----------------------------------------------------
                var procllist = result.AsEnumerable().Select(r => new
                {
                    ProcessingDate = r.ProcessingDate.ToString("dd/MM/yyyy"),
                    CustomerId = r.CustomerId,

                });

                var domainlistfinal = resultdomain.AsEnumerable().Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,

                });
                var Alldata = new { procdate = procllist, domlst = domainlistfinal };//

                return Json(Alldata, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = (from a in udb.IwSoD
                              where a.EoDStatus == 0 && a.CustomerId == id
                              select new
                              {
                                  a.ProcessingDate,
                                  a.CustomerId
                              }).ToList().Distinct().OrderByDescending(m => m.ProcessingDate);

                //---------------------------Domain-----------------
                //var resultdomain = (from du in udb.DomainUserMapMaster
                //                    from d in udb.DomainMaster
                //                    //from c in udb.CustomerMasters

                //                    where du.DomainId == d.Id && du.CustomerID == id && du.UserId == uid //a.EoDStatus == 0 && a.CustomerId == id
                //                    select new
                //                    {
                //                        d.Id,
                //                        d.Name
                //                    }).ToList().Distinct().OrderByDescending(m => m.Id);
                ///----------------------------------------------------
                //var procllist = result.AsEnumerable().Select(r => new
                //{
                //    ProcessingDate = r.ProcessingDate.ToString("dd/MM/yyyy"),
                //    CustomerId = r.CustomerId,

                //});

                //var domainlistfinal = resultdomain.AsEnumerable().Select(x => new
                //{
                //    Id = x.Id,
                //    Name = x.Name,

                //});
                //var Alldata = new { procdate = procllist, domlst = domainlistfinal };//


                //return Json(Alldata, JsonRequestBehavior.AllowGet);

                return Json(result.AsEnumerable().Select(r => new
                {
                    ProcessingDate = r.ProcessingDate.ToString("dd/MM/yyyy"),
                    CustomerId = r.CustomerId,

                }), JsonRequestBehavior.AllowGet);
            }


        }
        public ActionResult CustDomDateSelection(string Accesslevel = null, bool userlogin = true)
        {
            selectcustprocDate selectcusdate = new selectcustprocDate();
            selectcusdate.userlogin = userlogin;
            try
            {
                if (Session["uid"] != null)
                {
                    List<int> orgnisids = new List<int>();
                    int uid = (int)Session["uid"];
                    selectcusdate.Accesslevel = Accesslevel;

                    if (Accesslevel == "ORG")
                    {

                        var resut = (from a in udb.UserOrganizationMappings
                                     from d in udb.OrganizationMasters
                                     orderby d.Id
                                     where a.OrganizationId == d.Id && a.UserId == uid
                                     select new
                                     {
                                         OrgnizationName = d.Name,
                                         OrgnizationID = d.Id
                                     }).ToList();
                        //    ViewBag.Org = new SelectList(resut.AsEnumerable(), "OrgnizationID", "OrgnizationName");
                        selectcusdate.OrgnizationLst = new SelectList(resut, "OrgnizationID", "OrgnizationName");


                        selectcusdate.Orgdrop = 1;//------------Show Orgnization DropDown-----

                        if (resut.Count > 1)
                        {
                            var blank = (from a in udb.UserCustomerMappings
                                         from d in udb.CustomerMasters
                                         orderby d.Id
                                         where a.CustomerId == d.Id && a.UserId == 0
                                         select new
                                         {
                                             Customer = d.Name,
                                             CustomerID = d.Id

                                         }).ToList();
                            ViewBag.CustBag = new SelectList(blank.AsEnumerable(), "CustomerID", "Customer");
                        }
                        else
                        {
                            //orgnisids = (from u in udb.UserCustomerMappings
                            //             from c in udb.CustomerMasters
                            //             where u.CustomerId == c.Id && u.UserId == uid
                            //             select c.OrganizationId).ToList();

                            var Cust = (from a in udb.DomainUserMapMasters
                                        from d in udb.CustomerMasters
                                        //  from o in udb.OrganizationMasters
                                        orderby d.Id
                                        where a.CustomerID == d.Id && a.UserId == uid
                                        select new
                                        {
                                            Customer = d.Name,
                                            CustomerID = d.Id

                                        }).ToList().Distinct();

                            //var Cust = (from a in udb.UserCustomerMappings
                            //            from d in udb.CustomerMasters
                            //            from c in udb.DomainUserMapMaster
                            //            // from o in udb.OrganizationMasters
                            //            orderby d.Id
                            //            where a.CustomerId == d.Id && a.CustomerId == c.CustomerID && a.UserId == uid
                            //            select new
                            //            {
                            //                Customer = d.Name,
                            //                CustomerID = d.Id

                            //            }).ToList();

                            ViewBag.CustBag = new SelectList(Cust.AsEnumerable(), "CustomerID", "Customer");
                            //selectcusdate.OrgnizationLst = new SelectList("Select");
                            selectcusdate.custid = 1;
                        }


                    }
                    else if (Accesslevel == "CUST")
                    {

                        var Cust = (from a in udb.UserCustomerMappings
                                    from d in udb.CustomerMasters
                                    //  from o in udb.OrganizationMasters
                                    orderby d.Id
                                    where a.CustomerId == d.Id && a.UserId == uid
                                    select new
                                    {
                                        Customer = d.Name,
                                        CustomerID = d.Id

                                    }).ToList();
                        ViewBag.CustBag = new SelectList(Cust.AsEnumerable(), "CustomerID", "Customer");
                        selectcusdate.OrgnizationLst = new SelectList("Select");
                        selectcusdate.custid = 1;

                    }
                    else if (Accesslevel == "DOM")
                    {
                        var Cust = (from dom in udb.DomainUserMapMasters
                                    // from a in udb.UserCustomerMappings
                                    from d in udb.CustomerMasters
                                    orderby d.Id
                                    where dom.CustomerID == d.Id && dom.UserId == uid
                                    select new
                                    {
                                        Customer = d.Name,
                                        CustomerID = d.Id

                                    }).ToList().Distinct();
                        ViewBag.CustBag = new SelectList(Cust.AsEnumerable(), "CustomerID", "Customer");
                        selectcusdate.OrgnizationLst = new SelectList("Select");
                        selectcusdate.custid = 1;
                    }
                    else if (Accesslevel == "BRNCH")
                    {
                        //var Cust = (from brnch in udb.BranchMasters
                        //            orderby d.Id
                        //            where brnch.OwDomainId == d.Id && dom.UserId == uid
                        //            select new
                        //            {
                        //                Customer = d.Name,
                        //                CustomerID = d.Id

                        //            }).ToList().Distinct();
                        //ViewBag.CustBag = new SelectList(Cust.AsEnumerable(), "CustomerID", "Customer");
                        //selectcusdate.OrgnizationLst = new SelectList("Select");
                        //selectcusdate.custid = 1;
                    }
                    //----------------Clearingtype-------22-06-2017---------------
                    ViewBag.ClearingType = new SelectList(udb.ClearingType, "Code", "Name").ToList();
                }

                return View(selectcusdate);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "Login HttpGet CustDomDateSelection- " + innerExcp });
            }
        }
        [HttpPost]
        public ActionResult CustDomDateSelection(selectcustprocDate selectprocdate, string cancel = null)
        {

            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if (cancel == "Cancel")
            {
                return RedirectToAction("Logout", "Login", new { user = Session["LoginID"].ToString() });
            }

            int uid = (int)Session["uid"];
            if (Request.Form["CustName"] != null)
                Session["CustomerName"] = Request.Form["CustName"].ToString();
            //-----------------------------22-06-2017-----------------------
            if (Request.Form["ClearingType"] != null)
                Session["CtsSessionType"] = Request.Form["ClearingType"].ToString();



            try
            {
                if (Session["ProType"].ToString() == "Outward")
                {
                    if (Request.Form["Domainselect"] != null)
                        Session["DomainselectID"] = Request.Form["Domainselect"].ToString();

                    if (Request.Form["domval"] != null)
                        Session["SelectdDomainName"] = Request.Form["domval"].ToString();

                    DateTime procdate = DateTime.ParseExact(Request.Form["procdateval"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    int selectedcustid = Convert.ToInt16(Request.Form["CustBag"]);
                    if (selectedcustid == 4 || selectedcustid == 5 || selectedcustid == 6)
                    {
                        //------------Adding Web Config Imge Url setting------------------
                        Session["SrcWebIP"] = System.Configuration.ConfigurationManager.AppSettings["SrcWebIP"].ToString();
                        Session["DestWepIP"] = System.Configuration.ConfigurationManager.AppSettings["DestWepIP"].ToString();
                        Session["SrcWebName"] = System.Configuration.ConfigurationManager.AppSettings["SrcWebName"].ToString();
                        Session["DestWebName"] = System.Configuration.ConfigurationManager.AppSettings["DestWebName"].ToString();
                    }

                    var owsods = udb.OwSoDs.Where(m => m.ProcessingDate == procdate && m.CustomerId == selectedcustid).FirstOrDefault();
                    if (owsods != null)
                    {
                        Session["PostDate"] = owsods.PostDated;
                        Session["StaleDate"] = owsods.StaleDated;
                        Session["CustomerID"] = owsods.CustomerId;
                    }
                    var domainseting = (from d in udb.DomainUserMapMasters
                                        from dm in udb.DomainMaster
                                        where d.DomainId == dm.Id && d.UserId == uid && d.CustomerID == owsods.CustomerId
                                        select new { d.CustomerID, dm.Id, dm.Name }
                                            ).FirstOrDefault();
                    //udb.DomainUserMapMaster.Where(d => d.UserId == model.ID).SingleOrDefault();
                    var appsettingsACFrom = udb.ApplicationSettings.Where(m => m.CustomerId == domainseting.CustomerID && m.SettingName == "ACFrom").SingleOrDefault();
                    var appsettingsACTo = udb.ApplicationSettings.Where(m => m.CustomerId == domainseting.CustomerID && m.SettingName == "ACTo").SingleOrDefault();
                    var Sniptsetting = udb.ApplicationSettings.Where(m => m.CustomerId == domainseting.CustomerID && m.SettingName == "SnippetSettings").SingleOrDefault();
                    var CommonSetHigVl = udb.ApplicationSettings.Where(a => a.CustomerId == domainseting.CustomerID && a.SettingName == "HighValue").FirstOrDefault();
                    var DEBySnippet = udb.ApplicationSettings.Where(a => a.CustomerId == domainseting.CustomerID && a.SettingName == "SnippetSettings").FirstOrDefault();

                    var GetAccountDetailsV = udb.ApplicationSettings.Where(a => a.CustomerId == domainseting.CustomerID && a.SettingName == "GetAccountDetails").FirstOrDefault();
                    Session["GetAccountDetails "] = GetAccountDetailsV.SettingValue;

                    var NarrationV = udb.ApplicationSettings.Where(a => a.CustomerId == domainseting.CustomerID && a.SettingName == "Narration").FirstOrDefault();
                    Session["Narration"] = NarrationV.SettingValue;

                    var CreditCardValidationReq = udb.ApplicationSettings.Where(a => a.CustomerId == domainseting.CustomerID && a.SettingName == "CreditCardValidationRequired").FirstOrDefault();
                    Session["CreditCardValidationReq"] = CreditCardValidationReq.SettingValue;

                    var AllowACAlpha = udb.ApplicationSettings.Where(a => a.CustomerId == domainseting.CustomerID && a.SettingName == "ACAlphaAllow").FirstOrDefault();
                    if (AllowACAlpha != null)
                        Session["ACAlphaAllow"] = AllowACAlpha.SettingValue;

                    if (CreditCardValidationReq.SettingValue == "1")
                    {
                        var CreditCardValidAcNo = udb.ApplicationSettings.Where(a => a.CustomerId == domainseting.CustomerID && a.SettingName == "CreditCardValidAcNo").FirstOrDefault();
                        Session["CreditCardValidAcNo"] = CreditCardValidAcNo.SettingValue;
                        var CreditCardInValidAcNo = udb.ApplicationSettings.Where(a => a.CustomerId == domainseting.CustomerID && a.SettingName == "CreditCardInValidAcNo").FirstOrDefault();
                        Session["CreditCardInValidAcNo"] = CreditCardInValidAcNo.SettingValue;
                    }
                    //--------------Added On 20-06-2017-------------For SlipOnly VF------------
                    var SlipOnlyAccept = udb.ApplicationSettings.Where(a => a.CustomerId == domainseting.CustomerID && a.SettingName == "OnlySlipVF").FirstOrDefault();
                    if (SlipOnlyAccept != null)
                        Session["SlipOnlyAccept"] = SlipOnlyAccept.SettingValue;

                    var SlipOnlyAcceptAmtThreshold = udb.ApplicationSettings.Where(a => a.CustomerId == domainseting.CustomerID && a.SettingName == "OnlySlipAmtThreshold").FirstOrDefault();
                    if (SlipOnlyAcceptAmtThreshold != null)
                        Session["SlipOnlyAcceptAmtThreshold"] = SlipOnlyAcceptAmtThreshold.SettingValue;
                    //--------------------------------------------------------------------
                    /////-----------------------------------------------------------------
                    var RVERFNHIGHAMT = udb.ApplicationSettings.Where(a => a.CustomerId == selectedcustid && a.SettingName == "RVERFNHIGHAMT").FirstOrDefault();
                    if (RVERFNHIGHAMT != null)
                        Session["RVERFNHIGHAMT"] = RVERFNHIGHAMT.SettingValue;

                    if (DEBySnippet != null)
                        Session["OWDEBySnippet"] = DEBySnippet.SettingValue;

                    if (CommonSetHigVl != null)
                        Session["HIGHAMT"] = CommonSetHigVl.SettingValue;
                    else
                        Session["HIGHAMT"] = 0;

                    Session["blnDeBySnippet"] = Sniptsetting.SettingValue;
                    Session["acfrm"] = appsettingsACFrom.SettingValue;
                    Session["acto"] = appsettingsACTo.SettingValue;
                    Session["domainid"] = domainseting.Id;
                    Session["domainname"] = domainseting.Name;
                    Session["processdate"] = procdate;//dt.ToString("dd/MM/yyyy");

                    string[] ddt = new string[0];
                    string dtemp = Session["processdate"].ToString();
                    ddt = dtemp.Split('/');
                    Session["SnipDate"] = procdate.ToString("dd/MM/yyyy");//"04.06.2016";//ddt[1]+"."+ ddt[0]+"." + ddt[2];

                    @Session["glob"] = true;
                    // db.SaveChanges();
                    //if (selectprocdate.userlogin == false)
                    //    TempData["flg"] = selectprocdate.userlogin;

                    return RedirectToAction("IWIndex", "Home");
                }
                else if (Session["ProType"].ToString() == "Inward")
                {
                    // db.SaveChanges();
                    //if (selectprocdate.userlogin == false)
                    //    TempData["flg"] = selectprocdate.userlogin;

                    ////var DEData = udb.AppSettings.Where(a => a.Domain.ID == id && a.ClearingType == type).FirstOrDefault();
                    ////Session["blnChqAmt"] = DEData.CaptureChqAmount;
                    ////Session["blnCrdAccNo"] = DEData.CaptureChqCrdAccount;
                    ////Session["blnDate"] = DEData.CaptureChqDate;
                    ////Session["blnDrName"] = DEData.CaptureChqDraweeName;
                    ////Session["blnPyName"] = DEData.CaptureChqPayeeName;
                    ////Session["blnSlipAmt"] = DEData.CaptureSlipAmount;
                    ////Session["blnSlipAC"] = DEData.CaptureSlipAccount;
                    ////Session["blnDeBySnippet"] = DEData.DEBySnippet;
                    ////Session["AutoCodelineDecode"] = DEData.AutoCodelineDecode;
                    ////Session["SANCompulsory"] = DEData.SANCompulsory;
                    ////Session["blnDbtAccNo"] = DEData.CaptureChqDbtAccount;
                    //Session["Reverification"] = DEData.Reverification;

                    //------------Adding Web Config Imge Url setting------------------
                    Session["SrcWebIP"] = System.Configuration.ConfigurationManager.AppSettings["SrcWebIP"].ToString();
                    Session["DestWepIP"] = System.Configuration.ConfigurationManager.AppSettings["DestWepIP"].ToString();
                    Session["SrcWebName"] = System.Configuration.ConfigurationManager.AppSettings["SrcWebName"].ToString();
                    Session["DestWebName"] = System.Configuration.ConfigurationManager.AppSettings["DestWebName"].ToString();
                    //------------- Domain wise a/c length -----------------------
                    DateTime procdate = DateTime.ParseExact(Request.Form["procdateval"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    int selectedcustid = Convert.ToInt16(Request.Form["CustBag"]);

                    var iwsods = udb.IwSoD.Where(m => m.ProcessingDate == procdate && m.CustomerId == selectedcustid).FirstOrDefault();
                    if (iwsods != null)
                    {
                        Session["PostDate"] = iwsods.PostDated;
                        Session["Settelmentdate"] = iwsods.PostDated;
                        Session["StaleDate"] = iwsods.StaleDated;
                        Session["CustomerID"] = iwsods.CustomerId;
                    }
                    var domainseting = (from d in udb.DomainUserMapMasters
                                        from dm in udb.DomainMaster
                                        where d.DomainId == dm.Id && d.UserId == uid && d.CustomerID == selectedcustid
                                        select new { d.CustomerID, dm.Id, dm.Name }
                                            ).FirstOrDefault();

                    var appsettingsACFrom = udb.ApplicationSettings.Where(m => m.CustomerId == domainseting.CustomerID && m.SettingName == "ACFrom").SingleOrDefault();
                    var appsettingsACTo = udb.ApplicationSettings.Where(m => m.CustomerId == domainseting.CustomerID && m.SettingName == "ACTo").SingleOrDefault();
                    var Sniptsetting = udb.ApplicationSettings.Where(m => m.CustomerId == domainseting.CustomerID && m.SettingName == "SnippetSettings").SingleOrDefault();
                    var CommonSetHigVl = udb.ApplicationSettings.Where(a => a.CustomerId == domainseting.CustomerID && a.SettingName == "HighValue").FirstOrDefault();
                    //   var DEBySnippet = udb.ApplicationSettings.Where(a => a.CustomerId == domainseting.CustomerID && a.SettingName == "SnippetSettings").FirstOrDefault();
                    if (Sniptsetting.SettingValue == "1")
                        Session["blnDeBySnippet"] = "True";
                    else
                        Session["blnDeBySnippet"] = "False";

                    Session["acfrm"] = appsettingsACFrom.SettingValue;
                    Session["acto"] = appsettingsACTo.SettingValue;
                    Session["domainid"] = domainseting.Id;
                    Session["domainname"] = domainseting.Name;
                    Session["processdate"] = procdate;//dt.ToString("dd/MM/yyyy");
                    //------------------
                    //Session["acfrm"] = DEData.AccLenthFrom;
                    //Session["acto"] = DEData.AccLenthTo;
                    //Session["domainid"] = id;
                    //Session["domainname"] = name;
                    //Session["processdate"] = dt.ToShortDateString();//dt.ToString("dd/MM/yyyy");
                    // Session["clearingtype"] = type;
                    //    Session["QCEnabled"] = DEData.QCEnabled;
                    Session["glob"] = true;
                    //-----------------------------Get Postdated And Stale Cheques--------------
                    //Session["PostDate"] = PostDate;
                    //Session["StaleDate"] = StaleDate;
                    //Session["CustomerID"] = CustomerID;

                    //string[] ddt = new string[0];
                    //string dtemp = Session["processdate"].ToString();
                    //ddt = dtemp.Split('/');
                    //Session["SnipDate"] = dt.ToString("dd/MM/yyyy");//"04.06.2016";//ddt[1]+"."+ ddt[0]+"." + ddt[2];

                    string[] ddt = new string[0];
                    string dtemp = Session["processdate"].ToString();
                    ddt = dtemp.Split('/');
                    Session["SnipDate"] = procdate.ToString("dd/MM/yyyy");

                    ///---------------Added on 13-02-2017-------------------------------
                    var GetAccountDetailsV = udb.ApplicationSettings.Where(a => a.CustomerId == selectedcustid && a.SettingName == "GetAccountDetails").FirstOrDefault();
                    Session["GetAccountDetails "] = GetAccountDetailsV.SettingValue;
                    /////-----------------------------------------------------------------
                    var RVERFNHIGHAMT = udb.ApplicationSettings.Where(a => a.CustomerId == selectedcustid && a.SettingName == "RVERFNHIGHAMT").FirstOrDefault();
                    if (RVERFNHIGHAMT != null)
                        Session["RVERFNHIGHAMT"] = RVERFNHIGHAMT.SettingValue;

                    var accmodel = udb.ApplicationSettings.Where(a => a.CustomerId == iwsods.CustomerId && a.SettingName == "sign").FirstOrDefault();
                    if (accmodel != null)
                        Session["Sign"] = accmodel.SettingValue;


                    //var custid = db.Domains.Find(id).Customer_ID.ToString();
                    //Session["CustomerIDTemp"] = custid;
                    //var CommonSet = udb.CommonSettings.Where(a => a.AppName == "CTSCONFIG 1" && a.SettingName == "DEFAULTDS").FirstOrDefault();
                    //if (CommonSet != null)
                    //    Session["DefaultDS"] = CommonSet.SettingValue;

                    //var CommonSet1 = udb.CommonSettings.Where(a => a.AppName == "BULKCOUNT" && a.SettingName == custid).FirstOrDefault();
                    //if (CommonSet1 != null)
                    //    Session["bulkcount"] = CommonSet1.SettingValue;
                    //var CommonSetHigVl = udb.CommonSettings.Where(a => a.AppName == "HIGHAMT" && a.SettingName == custid).FirstOrDefault();
                    //if (CommonSetHigVl != null)
                    //    Session["HIGHAMT"] = CommonSetHigVl.SettingValue;


                    //if (type == "IW")
                    //{

                    // Session["blnDbtAccNo"] = DEData.CaptureChqDbtAccount;
                    // Session["blnDate"] = DEData.CaptureChqDate;
                    //Session["blnPyName"] = DEData.CaptureChqPayeeName;
                    // Session["reverification"] = DEData.Reverification;

                    //var CommonSets = db.CommonSettings.Where(a => a.AppName == "CTSCONFIGIW" && a.SettingName == "WebPath").Select(s => s.SettingValue).SingleOrDefault();
                    //Session["webpath"] = CommonSets.ToString();

                    //var IWfilehdr = udb.IWFileHDRs.Where(f => f.ProcessingDate == procdate).FirstOrDefault();
                    //if (IWfilehdr != null)
                    //{
                    //    Session["Settelmentdate"] = IWfilehdr.SettlementDate.Substring(4, 4) + "/" + IWfilehdr.SettlementDate.Substring(2, 2) + "/" + IWfilehdr.SettlementDate.Substring(0, 2);
                    //    Session["SessionDate"] = IWfilehdr.SessionDate.Substring(4, 4) + "/" + IWfilehdr.SessionDate.Substring(2, 2) + "/" + IWfilehdr.SessionDate.Substring(0, 2); ;
                    //}
                    // }

                    // return RedirectToAction("SelectDomain", "DomainSelection");
                    return RedirectToAction("IWIndex", "Home");
                }

                return View();
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "Login HttpPost CustDomDateSelection- " + innerExcp });
            }
        }

        public ActionResult ChangeDomain()
        {
            selectcustprocDate selectcusdate = new selectcustprocDate();

            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            int customerid = Convert.ToInt16(Session["CustomerID"]);
            //---------------------------Domain-----------------
            var resultdomain = (from du in udb.DomainUserMapMasters
                                from d in udb.DomainMaster
                                //from c in udb.CustomerMasters

                                where du.DomainId == d.Id && du.CustomerID == customerid && du.UserId == uid //a.EoDStatus == 0 && a.CustomerId == id
                                select new
                                {
                                    Id = d.Id,
                                    Name = d.Name
                                }).ToList().Distinct().OrderByDescending(m => m.Id);
            ///----------------------------------------------------
            selectcusdate.DomainLst = new SelectList(resultdomain, "Id", "Name");

            //ViewBag.Domainselect = resultdomain;
            return View("_ChangeDomain", selectcusdate);
        }
        [HttpPost]
        public ActionResult ChangeDomain(int id = 0)
        {
            if (Request.Form["Domainselect"] != null)
            {
                if (Request.Form["Domainselect"].ToString() != "")
                    Session["DomainselectID"] = Request.Form["Domainselect"].ToString();
                else
                    Session["DomainselectID"] = "0";
            }

            if (Request.Form["domval"] != null)
            {
                if (Request.Form["domval"].ToString() != "")
                    Session["SelectdDomainName"] = Request.Form["domval"].ToString();
                else
                    Session["SelectdDomainName"] = "ALL";
            }


            return RedirectToAction("IWIndex", "Home");
        }
        public PartialViewResult PolicyDetails(string name, int id = 0)
        {
            if (name == null)
            {
                //int sid=(int)Session["uid"];
                var pol = db.AppSecPolicies.Where(m => m.ID == id).SingleOrDefault();
                name = pol.Name;
            }
            var policy = (from a in db.AppSecPolicies
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
        public ActionResult Logdet(AppStResult AppStr)
        {
            if (ModelState.IsValid)
            {
                ViewBag.name = AppStr.name;

            }
            return View();
        }

        public ActionResult Logout(string user = null, bool rememberme = false, int id = 0)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    if (id == 10)
                    {
                        TempData["mesag"] = "Expired!";
                    }
                    else
                    {
                        if (FormsAuthentication.GetAuthCookie(user, rememberme) != null)
                        {
                            string format = "yyyy-MM-dd hh:mm:ss";
                            LoginLogoutAudit linlout = new LoginLogoutAudit();
                            DateTime sdat = Convert.ToDateTime(Session["logintime"]);

                            int suid = Convert.ToInt32(Session["uid"]);
                            var usmtr = usrlg.UserMasters.Where(u => u.ID == suid).SingleOrDefault();

                            if (usmtr != null)
                            {
                                linlout = usrlg.LoginLogoutAudits.Where(l => ((l.LoginDateTime == sdat) && (l.User_ID == suid))).FirstOrDefault();
                                if (linlout != null)
                                {
                                    linlout.LogoutDateTime = Convert.ToDateTime(DateTime.Now.ToString(format));
                                    linlout.sessionid = usmtr.sessionid;
                                }
                                //usmtr.sessionid = Session.SessionID;
                                usmtr.LastLogin = Convert.ToDateTime(DateTime.Now.ToString(format));
                                usmtr.loginFlg = 0;
                                usmtr.sessionid = null;
                            }


                            // checkLockRecord(suid);//--------Checking Lock records -----------------
                            usrlg.SaveChanges();
                            FormsAuthentication.SignOut();
                            Session.Clear();
                            Session.Abandon();
                            Session.RemoveAll();
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                            Response.Cache.SetNoStore();
                            // FormsAuthentication.SignOut();
                            Session.Remove("SessionID");
                            Session.RemoveAll();
                            Session.Abandon();
                            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));

                            if (Session.IsCookieless == false)
                            {
                                Session.Clear();
                                Session.Abandon();
                                Session.RemoveAll();
                                Session.SessionID.Remove(0);
                                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                                Response.Cache.SetNoStore();
                                FormsAuthentication.SignOut();
                                Session.Remove("SessionID");
                                Session.RemoveAll();
                                Session.Abandon();
                                Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
                            }
                        }
                    }

                }

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "Login HttpGet Logout- " + innerExcp });
            }
        }

        public void CheckPassword(string password)
        {
            string format = "yyyy-MM-dd hh:mm:ss";
            LoginLogoutAudit linlout = new LoginLogoutAudit();
            DateTime sdat = Convert.ToDateTime(Session["logintime"]);

            int suid = Convert.ToInt32(Session["uid"]);
            var usmtr = db.UserMasters.Where(u => u.ID == suid).SingleOrDefault();

            if (usmtr != null)
            {
                //usmtr.sessionid = Session.SessionID;
                usmtr.LastLogin = Convert.ToDateTime(DateTime.Now.ToString(format));
                usmtr.loginFlg = 0;
            }
            linlout = usrlg.LoginLogoutAudits.Where(l => ((l.LoginDateTime == sdat) && (l.User_ID == suid))).SingleOrDefault();
            if (linlout != null)
                linlout.LogoutDateTime = Convert.ToDateTime(DateTime.Now.ToString(format));

            db.SaveChanges();
            usrlg.SaveChanges();
            //checkLockRecord(suid);//----Check Lock Records ----And Make it Unlock----
            FormsAuthentication.SignOut();
            //= CookieProtection.All;
            if (Session.IsCookieless == false)
            {
                Session.Clear();
                Session.Abandon();
                Session.RemoveAll();
                Session.SessionID.Remove(0);
            }
            Session.Timeout = 1;
            //return RedirectToAction("Index", "Home");
        }

        public int checkLockRecord(DateTime? procdate, int uid = 0, int Did = 0, string clr = null)
        {

            //-------------Unlock BY PROC----------------------------
            // SPClassesDataContext spdb = new SPClassesDataContext();
            //spdb.UnlockALL(uid);
            //DateTime procdate = Convert.ToDateTime(Session["processdate"]);
            // int Did = (int)Session["domainid"];

            //int MINProcId = 0;
            //int MAXProcId = 0;
            //bool flg = false;
            //SPClassesDataContext SPD = new SPClassesDataContext();
            //-------------------- O/W--------------------------
            //MINProcId = db.ProcessMaster.Where(p => p.Domain.ID == Did && p.ProcessDate == procdate).Select(p1 => p1.ID).Min();
            //MAXProcId = db.ProcessMaster.Where(p => p.Domain.ID == Did && p.ProcessDate == procdate).Select(p1 => p1.ID).Max();
            //=-------------------------------- OW------------------------------
            if (clr == "OW")
            {
                // SPD.TempUnlockALL(uid, "OW");
                //----------------- MICR -------------------------------- && && m.ProcessID.ID>=MINProcId && m.ProcessID.ID<=MAXProcId

                //var Micr = db.ImageProcessings.Where(m => (m.MICRFalg == "L") && (m.MICRBy == uid) && m.ProcessID.ID > 3272).ToList();
                //if (Micr.Count != 0)
                //{
                //    flg = true;
                //    foreach (var mir in Micr)
                //    {
                //        if (mir.AmountQCStatus == "C" || mir.VerificationStatus == "C")
                //            mir.MICRFalg = "C";
                //        else mir.MICRFalg = "R";

                //    }
                //}
                ////----------------- Data Entry  Amount --------------------------------&& && m.ProcessID.ID >= MINProcId && m.ProcessID.ID <= MAXProcId
                //var DEAmt = db.ImageProcessings.Where(m => (m.AmountDEStatus == "L") && (m.AmountDEBy == uid) && m.ProcessID.ID > 3272).ToList();
                //if (DEAmt.Count != 0)
                //{
                //    flg = true;
                //    foreach (var den in DEAmt)
                //    {
                //        den.AmountDEStatus = "R";
                //    }
                //}
                ////----------------- Data Entry - Date-------------------------------&& m.ProcessID.ID >= MINProcId && m.ProcessID.ID <= MAXProcId
                //var DeDat = db.ImageProcessings.Where(m => (m.DateDEStatus == "L") && (m.DateDEBy == uid) && m.ProcessID.ID > 3272).ToList();
                //if (DeDat.Count != 0)
                //{
                //    flg = true;
                //    foreach (var Dedt in DeDat)
                //    {
                //        Dedt.DateDEStatus = "R";
                //    }
                //}
                ////----------------- Data Entry - A/C-------------------------------&& m.ProcessID.ID >= MINProcId && m.ProcessID.ID <= MAXProcId
                //var DeAc = db.ImageProcessings.Where(m => (m.CrdAccNoDEStatus == "L") && (m.CrdAccNoDEBy == uid) && m.ProcessID.ID > 3272).ToList();
                //if (DeAc.Count != 0)
                //{
                //    flg = true;
                //    foreach (var Deact in DeAc)
                //    {
                //        Deact.CrdAccNoDEStatus = "R";
                //    }
                //}

                ////----------------- QC ------------------------------- && m.ProcessID.ID >= MINProcId && m.ProcessID.ID <= MAXProcId
                //var Qc = db.ImageProcessings.Where(m => (m.AmountQCStatus == "L") && (m.AmountQCBy == uid) && m.ProcessID.ID > 3272).ToList();
                //if (Qc.Count != 0)
                //{
                //    flg = true;
                //    foreach (var qcchk in Qc)
                //    {
                //        qcchk.AmountQCStatus = "N";
                //    }
                //}
                ////----------------- Verification -------------------------------&& m.ProcessID.ID >= MINProcId && m.ProcessID.ID <= MAXProcId
                //var Vf = db.ImageProcessings.Where(m => (m.VerificationStatus == "L") && (m.VerificationBy == uid) && m.ProcessID.ID > 3272).ToList();
                //if (Vf.Count != 0)
                //{
                //    flg = true;
                //    foreach (var vfn in Vf)
                //    {
                //        vfn.VerificationStatus = "N";
                //    }
                //}
            } //------------------------ I/W -----------------------------
            else
            {
                //SPD.TempUnlockALL(uid, "IW");
                //------------------------ Data Entery -- DebtAc---------------------------&& m.Process.ID >= MINProcId && m.Process.ID <= MAXProcId
                //var debtAc = db.IWImageProcessings.Where(m => (m.DbtAccNoDEStatus == "L") && (m.DebitAccNoDEBy == uid) && m.Process.ID > 3272).ToList();
                //if (debtAc.Count != 0)
                //{
                //    flg = true;
                //    foreach (var mir in debtAc)
                //    {
                //        mir.DbtAccNoDEStatus = "R";
                //    }
                //}
                ////----------------- Data Entry  payename --------------------------------&& m.Process.ID >= MINProcId && m.Process.ID <= MAXProcId
                //var iwpayname = db.IWImageProcessings.Where(m => (m.PayeeNameDEStatus == "L" && m.PayeeNameDEBy == uid && m.Process.ID > 3272)).ToList();
                //if (iwpayname.Count != 0)
                //{
                //    flg = true;
                //    foreach (var den in iwpayname)
                //    {
                //        den.PayeeNameDEStatus = "R";
                //    }
                //}
                ////----------------- Data Entry - Date-------------------------------&& m.Process.ID >= MINProcId && m.Process.ID <= MAXProcId
                //var DeIwDat = db.IWImageProcessings.Where(m => (m.DateDEStatus == "L" && m.DateDEBy == uid && m.Process.ID > 3272)).ToList();
                //if (DeIwDat.Count != 0)
                //{
                //    flg = true;
                //    foreach (var Dedt in DeIwDat)
                //    {
                //        Dedt.DateDEStatus = "R";
                //    }
                //}
                ////----------------- QC -------------------------------&& (m.VerificationBy == uid) && m.Process.ID >= MINProcId && m.Process.ID <= MAXProcId
                //var IWQc = db.IWImageProcessings.Where(m => (m.VerificationStatus == "L" && m.VerificationBy == uid && m.Process.ID > 3272)).ToList();
                //if (IWQc.Count != 0)
                //{
                //    flg = true;
                //    foreach (var qcchk in IWQc)
                //    {
                //        qcchk.VerificationStatus = "N";
                //    }
                //}
                ////----------------- Verification -------------------------------&& (m.SignverificationBy == uid) && m.Process.ID >= MINProcId && m.Process.ID <= MAXProcId
                //var IwVf = db.IWImageProcessings.Where(m => (m.SignVerificationStatus == "L" && m.SignverificationBy == uid && m.Process.ID > 3272)).ToList();
                //if (IwVf.Count != 0)
                //{
                //    flg = true;
                //    foreach (var vfn in IwVf)
                //    {
                //        vfn.SignVerificationStatus = "N";
                //    }
                //}
                ////----------------- Reverification ----------L3---------------------&& ProcId.Contains(m.Process.ID)
                //var Revrn = db.IWImageProcessings.Where(m => (m.ReverificationStatus == "L") && (m.ReverificationBy == uid && m.Process.ID > 3272)).ToList();
                //if (Revrn.Count != 0)
                //{
                //    flg = true;
                //    foreach (var vfn in Revrn)
                //    {
                //        vfn.ReverificationStatus = "N";
                //    }
                //}     
            }

            // SPD.Dispose();
            //if (flg == true)
            //    db.SaveChanges();
            return (0);
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
            FormsAuthentication.SetAuthCookie(Session["LoginID"].ToString(), false);
            try
            {
                int tempid = (int)Session["uid"];
                var userselect = (from u in db.UserMasters
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
                userselect.flg = 1;
                return View(userselect);
            }
            catch (Exception e)
            {
                string format = "yyyy-MM-dd hh:mm:ss";
                LoginLogoutAudit linlout = new LoginLogoutAudit();
                DateTime sdat = Convert.ToDateTime(Session["logintime"]);

                int suid = Convert.ToInt32(Session["uid"]);
                var usmtr = db.UserMasters.Where(u => u.ID == suid).SingleOrDefault();
                if (usmtr != null)
                {
                    //usmtr.sessionid = Session.SessionID;
                    usmtr.LastLogin = Convert.ToDateTime(DateTime.Now.ToString(format));
                    usmtr.loginFlg = 0;
                    db.SaveChanges();
                }

                linlout = usrlg.LoginLogoutAudits.Where(l => ((l.LoginDateTime == sdat) && (l.User_ID == suid))).SingleOrDefault();
                if (linlout != null)
                    linlout.LogoutDateTime = Convert.ToDateTime(DateTime.Now.ToString(format));

                usrlg.SaveChanges();
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

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "Login HttpGet ChangePassword- " + innerExcp });
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
            int uid = (int)Session["uid"];
            try
            {
                if (btn == "Cancel")
                    return RedirectToAction("Logout", "Login", new { user = Session["LoginID"].ToString() });
                // return RedirectToAction("Index", "Login", new { id = 0 });
                // return RedirectToAction("IWIndex", "Home");

                CommonFunction cmf = new CommonFunction();
                PasswordHistory pswhtr = new PasswordHistory();
                //----------------------------------------  Check Password Validation -----------------------------//


                bool result = false;
                bool isDigit = false;
                bool isLetter = false;
                bool isLowerChar = false;
                bool isUpperChar = false;
                bool isNonAlpha = false;
                int invlidatpass, invldpasspar;
                //--------------------- Check From AppSecPolicies --------------------//
                var policy = (from a in db.AppSecPolicies
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
                //---------------Old Password----------- New Logic For Axis ----
                string midelpass = "";
                string NewPassword = "";
                string confirmpasswor = "";

                for (int i = 0; i < cngpswd.OldPassword.Length; i++)
                {
                    midelpass = midelpass + Convert.ToChar((int)cngpswd.OldPassword[i] + 13);
                }
                //-------------New Password------
                for (int i = 0; i < cngpswd.NewPassword.Length; i++)
                {
                    NewPassword = NewPassword + Convert.ToChar((int)cngpswd.NewPassword[i] + 13);
                }
                //-------------New Confirm------
                for (int i = 0; i < cngpswd.ConfrmPassword.Length; i++)
                {
                    confirmpasswor = confirmpasswor + Convert.ToChar((int)cngpswd.ConfrmPassword[i] + 13);
                }
                cngpswd.OldPassword = midelpass;
                cngpswd.NewPassword = NewPassword;
                cngpswd.ConfrmPassword = confirmpasswor;
                //--- If Old password incorrect---------
                string temppasswd = cmf.EncryptPassword(cngpswd.OldPassword);
                var user = db.UserMasters.Where(u => (u.ID == uid && u.Password == temppasswd)).SingleOrDefault();
                if (user == null)
                {
                    //**************************
                    var usmtr = db.UserMasters.Where(u => u.ID == uid).SingleOrDefault();

                    var polcy1 = db.AppSecPolicies.Where(m => m.ID == usmtr.AppSecPolicieID).SingleOrDefault();
                    invlidatpass = usmtr.InvalidPasswordAttempts;
                    invldpasspar = polcy1.InvalidAttemptsAllowed;

                    if (invlidatpass >= invldpasspar)
                    {
                        usmtr.Active = false;
                        usmtr.loginFlg = 0;
                        db.SaveChanges();
                        ModelState.AddModelError("", "Oops! User Id has been disable, Please contact to administrator !!");

                        TempData["changemsg"] = "wrongpass";
                        cngpswd.succ = 1;
                        return View(cngpswd);
                        // return RedirectToAction("Index", "Login");
                        //return View("Index");
                    }
                    else
                    {
                        usmtr.InvalidPasswordAttempts = usmtr.InvalidPasswordAttempts + 1;
                        db.SaveChanges();
                        ModelState.AddModelError("", "Oops! Old Password is Wrong!");
                        return View(cngpswd);
                    }

                    //*************************
                    /*
                    if (tempfl <= 3)
                    {
                        cngpswd.flg = tempfl + 1;
                        ModelState.AddModelError("", "Oops! Old Password is Wrong!");
                        return View(cngpswd);
                    }
                    //if (tempfl == 4)
                    //{
                    //    cngpswd.flg = 4 + 1;
                    //    ModelState.AddModelError("", "Oops! Old Password is Wrong!");
                    //    return View(cngpswd);
                    //}
                    if (tempfl >= 4)
                    {
                        string format = "yyyy-MM-dd hh:mm:ss";
                        LoginLogoutAudit linlout = new LoginLogoutAudit();
                        DateTime sdat = Convert.ToDateTime(Session["logintime"]);

                        int suid = Convert.ToInt32(Session["uid"]);
                        // int suid = cngpswd.loginUsrid;
                        var usmtr = db.UserMasters.Where(u => u.ID == suid).SingleOrDefault();
                        if (usmtr != null)
                        {
                            //usmtr.sessionid = Session.SessionID;
                            usmtr.LastLogin = Convert.ToDateTime(DateTime.Now.ToString(format));
                            usmtr.loginFlg = 0;
                            usmtr.Active = false;
                            // TempData["changemsg"] = "wrongpass";
                            //ModelState.AddModelError("", "Oops! Your id has been disable, Please contact to administrator !");
                        }

                        linlout = usrlg.LoginLogoutAudits.Where(l => ((l.LoginDateTime == sdat) && (l.User_ID == suid))).SingleOrDefault();
                        if (linlout != null)
                            linlout.LogoutDateTime = Convert.ToDateTime(DateTime.Now.ToString(format));

                        db.SaveChanges();
                        //FormsAuthentication.SignOut();

                        //if (Session.IsCookieless == false)
                        //{
                        //    Session.Clear();
                        //    Session.Abandon();
                        //    Session.RemoveAll();
                        //    Session.SessionID.Remove(0);
                        //}
                        TempData["changemsg"] = "wrongpass";
                        return RedirectToAction("Index", "Login");
                    }
                     */
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

                var lastpaswrd = (from ph in usrlg.PasswordHistories
                                  orderby ph.Password descending
                                  where ph.User_ID == uid
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

                    usrlg.PasswordHistories.Add(pswhtr);

                    user.Password = cmf.EncryptPassword(cngpswd.NewPassword);
                    //if (cngpswd.firstlogin == 1)
                    user.FirstLogin = false;
                    string format1 = "yyyy-MM-dd hh:mm:ss";
                    user.LastLogin = Convert.ToDateTime(DateTime.Now.ToString(format1));
                    user.loginFlg = 0;
                    db.SaveChanges();
                    usrlg.SaveChanges();
                    cngpswd.succ = 1;
                    ModelState.AddModelError("", "Password has been change successfully !!");

                    return View(cngpswd);
                    // return RedirectToAction("Index", "Login", new { id = 3 });
                }
                else
                {
                    pswhtr.Password = user.Password;
                    pswhtr.User_ID = user.ID;
                    pswhtr.PassworChangeDate = DateTime.Now;
                    pswhtr.PasswordChangedBy_ID = user.ID;

                    usrlg.PasswordHistories.Add(pswhtr);

                    user.Password = cmf.EncryptPassword(cngpswd.NewPassword);
                    // if (cngpswd.firstlogin == 1)
                    user.FirstLogin = false;

                    string format = "yyyy-MM-dd hh:mm:ss";
                    LoginLogoutAudit linlout = new LoginLogoutAudit();
                    DateTime sdat = Convert.ToDateTime(Session["logintime"]);

                    // int suid = Convert.ToInt32(Session["uid"]);
                    int suid = uid;
                    var usmtr = db.UserMasters.Where(u => u.ID == suid).SingleOrDefault();
                    if (usmtr != null)
                    {
                        //usmtr.sessionid = Session.SessionID;
                        usmtr.LastLogin = Convert.ToDateTime(DateTime.Now.ToString(format));
                        usmtr.loginFlg = 0;
                        db.SaveChanges();
                    }

                    linlout = usrlg.LoginLogoutAudits.Where(l => ((l.LoginDateTime == sdat) && (l.User_ID == suid))).SingleOrDefault();
                    if (linlout != null)
                        linlout.LogoutDateTime = Convert.ToDateTime(DateTime.Now.ToString(format));

                    //db.SaveChanges();
                    FormsAuthentication.SignOut();

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
                    uma.ActionBy = uid;
                    uma.Actiondate = DateTime.Now;
                    uma.UserId = uid;
                    // uma.comments = "Roles[" + allroles + "]";
                    usrlg.UserMasterActivities.Add(uma);
                    //----------------------------------END-------
                    usrlg.SaveChanges();
                    // return RedirectToAction("Index", "Login", new { id = 3 });
                    ModelState.AddModelError("", "Password has been change successfully !!");
                    //   ViewBag.succ = "1";
                    cngpswd.succ = 1;
                    return View(cngpswd);
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

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "Login HttpPost ChangePassword- " + innerExcp });
            }
        }


    }
}
