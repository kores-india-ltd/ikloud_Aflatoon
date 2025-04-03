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
using System.DirectoryServices.AccountManagement;
using NLog;
//using System.Net.Http;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Data.SqlClient;
using System.Data;
//using System.Net;
//using KoresAESSH256;

namespace ikloud_Aflatoon.Controllers
{
    //[OutputCache(Duration = 0)]
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //private AES_Algorithm aes = new KoresAESSH256.AES_Algorithm();
        private UserAflatoonDbContext db = new UserAflatoonDbContext();
        private AflatoonEntities udb = new AflatoonEntities();

        private UserLogDbContext usrlg = new UserLogDbContext();
        CommonFunction cmf = new CommonFunction();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);

        public async System.Threading.Tasks.Task<ActionResult> Index(string code, string iss, string client_id)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                if (code != null)
                {
                    string sResposne = "";
                    try
                    {
                        try
                        {
                            var data = new[]
                            {
                                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                                new KeyValuePair<string, string>("code", code),
                                new KeyValuePair<string, string>("client_id", System.Configuration.ConfigurationManager.AppSettings["ClientId"].ToString()),
                                new KeyValuePair<string, string>("client_secret", System.Configuration.ConfigurationManager.AppSettings["ClientSecret"].ToString()),
                                new KeyValuePair<string, string>("redirect_uri", System.Configuration.ConfigurationManager.AppSettings["SenderUrl"].ToString()),
                            };

                            //logerror("Calling first api AccessTokenUrl ", "Calling first api AccessTokenUrl " + "->  In line no - 61 ");
                            using (var client = new HttpClient())
                            {
                                //logerror("Calling first api AccessTokenUrl Read AccessTokenUrl ", "->  In line no - 69 " + System.Configuration.ConfigurationManager.AppSettings["AccessTokenUrl"].ToString());
                                //logerror("Calling first api AccessTokenUrl Read code ", "->  In line no - 70 " + code);
                                //logerror("Calling first api AccessTokenUrl Read client_id ", "->  In line no - 71 " + System.Configuration.ConfigurationManager.AppSettings["ClientId"].ToString());
                                //logerror("Calling first api AccessTokenUrl Read client_secret ", "->  In line no - 72 " + System.Configuration.ConfigurationManager.AppSettings["ClientSecret"].ToString());
                                //logerror("Calling first api AccessTokenUrl Read redirect_uri ", "->  In line no - 73 " + System.Configuration.ConfigurationManager.AppSettings["SenderUrl"].ToString());
                                //logerror("Calling first api AccessTokenUrl Read data ", "->  In line no - 74 " + data);

                                var response = await client.PostAsync(System.Configuration.ConfigurationManager.AppSettings["AccessTokenUrl"].ToString(), new FormUrlEncodedContent(data));
                                var sResponse = await response.Content.ReadAsStringAsync();
                                {
                                    //logerror("Calling first api AccessTokenUrl and reading response ", "Calling first api AccessTokenUrl and reading response " + "->  In line no - 79 ");
                                    using (StreamReader sReader = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(sResponse))))
                                    {
                                        var result = sReader.ReadToEnd();
                                        sResposne = result;
                                    }
                                }
                            }
                            //logerror("Finish Calling first api AccessTokenUrl ", "Calling first api AccessTokenUrl " + "->  In line no - 87 ");
                        }
                        catch (Exception Ex)
                        {
                            string message = "";
                            string innerExcp = "";
                            if (Ex.Message != null)
                                message = Ex.Message.ToString();
                            if (Ex.InnerException != null)
                                innerExcp = Ex.InnerException.Message;
                            ViewBag.msg = innerExcp;
                            logerror("Finish Calling first api AccessTokenUrl In Catch Message ", "->  In line no - 98 - " + message);
                            logerror("Finish Calling first api AccessTokenUrl In Catch InnerException ", "->  In line no - 99 - " + innerExcp);
                            sResposne = "{        " +
                                        "    \"error\":\"Runtime Error While Sending the Request\"," +
                                        "    \"error_description\":\"" + Ex.Message +
                                        "\"}";
                        }

                        //logerror("Reading response from first api AccessTokenUrl ",  "->  In line no - 106 " + sResposne);

                        var jObject = Newtonsoft.Json.Linq.JObject.Parse(sResposne);
                        //logerror("Reading access_token from first api AccessTokenUrl ", "->  In line no - 109 " + jObject["id_token"]);
                        if (jObject["id_token"] != null)
                        {
                            string sAccessToken = jObject["id_token"].ToString();

                            try
                            {
                                //logerror("Calling second api id token ", "Calling first api id token " + "->  In line no - 116 ");
                                using (var client = new HttpClient())
                                {
                                    var data = new[]{
                                            new KeyValuePair<string, string>("client_id", System.Configuration.ConfigurationManager.AppSettings["ClientId"].ToString()),
                                            new KeyValuePair<string, string>("client_secret", System.Configuration.ConfigurationManager.AppSettings["ClientSecret"].ToString()),
                                            new KeyValuePair<string, string>("id_token", sAccessToken),
                                        };

                                    //logerror("Calling second api id token reading TokenIdInfoUrl ", "->  In line no - 125 " + System.Configuration.ConfigurationManager.AppSettings["TokenIdInfoUrl"].ToString());
                                    //logerror("Calling second api id token reading client_id ", "->  In line no - 126 " + System.Configuration.ConfigurationManager.AppSettings["ClientId"].ToString());
                                    //logerror("Calling second api id token reading client_secret ", "->  In line no - 127 " + System.Configuration.ConfigurationManager.AppSettings["ClientSecret"].ToString());
                                    //logerror("Calling second api id token reading id_token ", "->  In line no - 128 " + sAccessToken);
                                    //logerror("Calling second api id token reading data ", "->  In line no - 129 " + data);
                                    var response = await client.PostAsync(System.Configuration.ConfigurationManager.AppSettings["TokenIdInfoUrl"].ToString(), new FormUrlEncodedContent(data));

                                    var sResponse = await response.Content.ReadAsStringAsync();
                                    {
                                        //logerror("Calling second api id token reading ", "Calling first api id token reading " + "->  In line no - 134 ");

                                        using (StreamReader sReader = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(sResponse))))
                                        {
                                            var result = sReader.ReadToEnd();
                                            sResposne = result;
                                        }
                                    }

                                }
                                //logerror("Finish Calling second api id token ", "Calling first api id token " + "->  In line no - 144 ");
                            }
                            catch (Exception Ex)
                            {
                                string message = "";
                                string innerExcp = "";
                                if (Ex.Message != null)
                                    message = Ex.Message.ToString();
                                if (Ex.InnerException != null)
                                    innerExcp = Ex.InnerException.Message;
                                ViewBag.msg = innerExcp;
                                logerror("Finish Calling second api id token In Catch Message ", "->  In line no - 155 - " + message);
                                logerror("Finish Calling second api id token In Catch InnerException ", "->  In line no - 156 - " + innerExcp);
                                
                                sResposne = "{        " +
                                            "    \"error\":\"Runtime Error While Sending the Request\"," +
                                            "    \"error_description\":\"" + Ex.Message +
                                            "\"}";
                            }

                            //logerror("Reading response from second api id token ", "->  In line no - 164 " + sResposne);

                            jObject = Newtonsoft.Json.Linq.JObject.Parse(sResposne);
                            //logerror("Reading userId from second api id token ", "->  In line no - 167 - " + jObject["userId"]);
                            if (jObject["userId"] != null)
                            {

                                //return Redirect(sso_url);

                                // LoginController

                                // txtLoginId.Text = jObject["userId"].ToString();

                                // MessageBox.Show(jObject["givenName"].ToString(), jObject["userId"].ToString());

                                //====== Login controller code start by amol ============================
                                
                                try
                                {
                                    string UserId = jObject["userId"].ToString();
                                    //string UserId = "raees";
                                    //logerror("UserId - " + UserId,  "->  In line no - 93 ");
                                    LoginLogoutAudit linlout = new LoginLogoutAudit();

                                    string form = "dd-MM-yyyy hh:mm:ss";
                                    string format = "yyyy-MM-dd hh:mm:ss";
                                    string commdate = DateTime.Now.ToString(format);
                                    int invlidatpass, invldpasspar;

                                    var CustomerBankCode = "";
                                    ViewBag.CustomerBankCode = CustomerBankCode;
                                    CustomerBankCode = udb.CommonSettings.FirstOrDefault((p) => p.AppName == "CTSCONFIG1" && p.SettingName == "BankCode")?.SettingValue;
                                    if (CustomerBankCode != null || CustomerBankCode != "")
                                    {
                                        if (CustomerBankCode == "641")
                                        {
                                            var userModel = db.UserMasters.Where(m => m.LoginID == UserId).SingleOrDefault();
                                            if (userModel != null)
                                            {
                                                if (userModel.IsActive != 1)
                                                {
                                                    //ModelState.AddModelError("", "Oops! Your id has not approved, Please contact to administrator !");
                                                    ViewBag.msg = "Oops! Your id has not approved, Please contact to administrator !";
                                                    return View("Index");
                                                }
                                            }
                                        }
                                    }
                                    //logerror("After Customer Bank Code ", "After Customer Bank Code " + "->  After Customer Bank Code ");
                                    //Session["clearingtype"] = Request.Form["clearing"].ToString();

                                    var model = db.UserMasters.Where(m => m.LoginID == UserId).SingleOrDefault();
                                    if (model == null)
                                    {
                                        UserMasterActivity uma = new UserMasterActivity();
                                        uma.Action = "UserId Wrong";
                                        uma.ActionBy = null;
                                        uma.Actiondate = DateTime.Now;
                                        uma.UserId = null;
                                        uma.comments = UserId;
                                        usrlg.UserMasterActivities.Add(uma);
                                        usrlg.SaveChanges();
                                        //logerror("In UserMasterActivity model null ", "In UserMasterActivity model null " + "->  In UserMasterActivity model null ");
                                        //ModelState.AddModelError("", "Oops! userid/password is wrong");
                                        ViewBag.msg = "Oops! userid/password is wrong";
                                        return View("Index");
                                    }
                                    //logerror("In UserMasterActivity model not null ", "In UserMasterActivity model not null " + "->  In UserMasterActivity model not null ");
                                    //if (model != null)
                                    //{
                                    //    if (model.Active == false)
                                    //    {
                                    //        //logerror("In model not null acitve false ", "In model not null acitve false " + "->  In model not null acitve false ");
                                    //        ModelState.AddModelError("", "Oops! Your id has been disable, Please contact to administrator !");
                                    //        return View("Index");
                                    //    }
                                    //    else
                                    //    {

                                    //    }
                                    //}

                                    var usmtr1 = db.UserMasters.Where(u => u.LoginID == UserId).SingleOrDefault();
                                    if (usmtr1 != null)
                                    {
                                        //logerror("After AD usermaster not null ", "After AD usermaster not null" + "->  After AD usermaster not null");
                                        if (usmtr1.Active == false)
                                        {
                                            //logerror("After AD usermaster not null active false ", "After AD usermaster not null active false" + "->  After AD usermaster not null active false");
                                            //ModelState.AddModelError("", "Oops! Your id has been disable, Please contact to administrator !");
                                            ViewBag.msg = "Oops! Your id has been disable, Please contact to administrator !";
                                            return View("Index");
                                        }
                                        //------------B
                                        var loginout = usrlg.LoginLogoutAudits.Where(m => m.User_ID == model.ID).OrderByDescending(m => m.ID).FirstOrDefault();
                                        if (loginout != null)
                                            Session["lastlogin"] = loginout.LoginDateTime.ToString(form);
                                        //logerror("After AD after loginlogout ", "After AD after loginlogout" + "->  After AD after loginlogout");
                                        linlout.User_ID = model.ID;
                                        linlout.LoginDateTime = Convert.ToDateTime(commdate);
                                        linlout.LogoutDateTime = DateTime.Now;

                                        Session["logintime"] = commdate;

                                        usrlg.LoginLogoutAudits.Add(linlout);
                                        // string sid = Session.SessionID;
                                        usrlg.SaveChanges();
                                        //logerror("After AD after loginlogout add userlog ", "After AD after loginlogout add userlog" + "->  After AD after loginlogout add userlog");

                                        Session["LoginID"] = model.LoginID;
                                        Session["uid"] = model.ID;
                                        Session["title"] = model.Title;
                                        Session["fname"] = model.FirstName;
                                        Session["LgnName"] = model.LoginID;
                                        Session.Timeout = 300;

                                        //=================== Check User Management Setting By Amol====================================

                                        var UserManagementVersion2 = "N";
                                        ViewBag.UserManagementVersion2 = UserManagementVersion2;
                                        UserManagementVersion2 = udb.CommonSettings.FirstOrDefault((p) => p.AppName == "CTSCONFIG1" && p.SettingName == "UserManagement_v2")?.SettingValue;
                                        if (UserManagementVersion2 == null || UserManagementVersion2 == "")
                                        {
                                            //logerror("UserManagement version null ", "UserManagement version null" + "->  UserManagement version null");
                                            ViewBag.UserManagementVersion2 = "N";
                                        }
                                        else
                                        {
                                            //logerror("UserManagement version not null ", "UserManagement version not null" + "->  UserManagement version not null");
                                            if (UserManagementVersion2 == "Y")
                                            {
                                                ViewBag.UserManagementVersion2 = "Y";
                                            }
                                            else
                                            {
                                                ViewBag.UserManagementVersion2 = "N";
                                            }
                                        }
                                        Session["UserManagementVersion2"] = ViewBag.UserManagementVersion2;
                                        //=================== Check User Management Setting End ====================================
                                        //logerror(Session["UserManagementVersion2"].ToString(), Session["UserManagementVersion2"].ToString() + "->  UserManagement version value");
                                        if (Session["UserManagementVersion2"].ToString() == "Y")
                                        {
                                            //logerror("UserManagement version Y ", "UserManagement version Y" + "->  UserManagement version Y");
                                            //logerror("UserManagement version Y model.RoleId ", "UserManagement version Y" + "->  " + model.RoleId);
                                            //logerror("UserManagement version Y usmtr1.RoleId ", "UserManagement version Y" + "->  " + usmtr1.RoleId);
                                            var userRole = udb.RoleMaster.Where(a => a.ID == model.RoleId).SingleOrDefault();
                                            var roleName = userRole.RoleName;
                                            //logerror("UserManagement version Y roleName ", "UserManagement version Y roleName" + "->  " + roleName);
                                            Session["RoleName"] = roleName;
                                            Session["DE"] = userRole.IsDataEntry;
                                            Session["Ds"] = userRole.IsDashboard;
                                            Session["fildwnd"] = userRole.IsFileDownload;
                                            Session["QC"] = userRole.IsQC;
                                            Session["QueryMod"] = userRole.IsQueryWithModification;
                                            Session["RejectRepair"] = userRole.IsRejectRepaire;
                                            Session["Report"] = userRole.IsReport;
                                            Session["UserManagment"] = userRole.IsUserManagement;
                                            Session["VF"] = userRole.IsVerification;
                                            Session["Archv"] = userRole.IsArchieve;
                                            Session["Master"] = userRole.IsMasters;
                                            Session["Mesgbrd"] = userRole.IsMessageBroadcasting;
                                            Session["Query"] = userRole.IsQuery;
                                            Session["Settg"] = userRole.IsSettings;
                                            Session["SOD"] = userRole.IsSOD;
                                            Session["chirjct"] = userRole.IsChiReject;
                                            Session["role"] = userRole.IsRoleMaster;
                                            Session["RVF"] = userRole.IsReVerification;
                                            Session["CCPH"] = userRole.IsCCPH_Verification;
                                            Session["UserManagementChecker"] = userRole.IsUserManagementChecker;
                                            Session["RoleMasterChecker"] = userRole.IsRoleMasterChecker;
                                            //Session["RVF4"] = chkroles1.RVF4;
                                            //Session["ProType"] = Request.Form["clearing"];
                                            //-------------------Added On 25/01/2017------------For Inward L2AmountWise------------
                                            Session["L2StartLimit"] = userRole.L2StartLimit;
                                            Session["L2StopLimit"] = userRole.L2StopLimit;
                                            //logerror("UserManagement version Y end ", "UserManagement version Y end" + "->  UserManagement version Y end");
                                        }
                                        else
                                        {
                                            Session["RoleName"] = "";
                                            //logerror("UserManagement version N", "UserManagement version N" + "->  UserManagement version N");
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
                                            //Session["RVF4"] = chkroles1.RVF4;
                                            //Session["ProType"] = Request.Form["clearing"];
                                            //-------------------Added On 25/01/2017------------For Inward L2AmountWise------------
                                            Session["L2StartLimit"] = model.L2StartLimit;
                                            Session["L2StopLimit"] = model.L2StopLimit;
                                            //logerror("UserManagement version N end", "UserManagement version N end" + "->  UserManagement version N end");
                                        }

                                        //FormsAuthentication.SetAuthCookie(model.LoginID, false);

                                        //----------------------------Uncomment---------------For deployment
                                        //foreach (string s in Response.Cookies.AllKeys)
                                        //{
                                        //    if (s == FormsAuthentication.FormsCookieName || s.ToLower() == "asp.net_sessionid")
                                        //    {
                                        //        Response.Cookies[s].Secure = true;
                                        //    }
                                        //}
                                        //logerror("After form Authentication", "After form Authentication" + "->  After form Authentication");
                                        //------------Checking previous Login-------------
                                        if (Session["uid"] == null)
                                            ViewBag.userlogin = "false";

                                        bool userlogin = true;
                                        int uid = (int)Session["uid"];
                                        //-----------End
                                        //---C
                                        if (model.sessionid != null)
                                        {
                                            //logerror("In session id not null", "In session id not null" + "->  In session id not null");
                                            if (model.sessionid != Session.SessionID)
                                            {
                                                userlogin = false;
                                                usmtr1.LastLogin = Convert.ToDateTime(DateTime.Now.ToString(format));
                                            }
                                            else
                                            {
                                                //logerror("In session id not null in else", "In session id not null in else" + "->  In session id not null in else");
                                                userlogin = true;
                                                usmtr1.sessionid = Session.SessionID;
                                                usmtr1.loginFlg = 1;
                                                usmtr1.InvalidPasswordAttempts = 0;
                                                usmtr1.LastLogin = Convert.ToDateTime(DateTime.Now.ToString(format));
                                                Session["afterlogin"] = true;
                                                
                                            }
                                            db.SaveChanges();
                                        }
                                        else
                                        {
                                            //logerror("In session id null", "In session id null" + "->  In session id  null");
                                            userlogin = true;
                                            usmtr1.sessionid = Session.SessionID;
                                            usmtr1.loginFlg = 1;
                                            usmtr1.InvalidPasswordAttempts = 0;
                                            usmtr1.LastLogin = Convert.ToDateTime(DateTime.Now.ToString(format));
                                            Session["afterlogin"] = true;
                                            db.SaveChanges();
                                        }
                                        Session["Accesslevel"] = usmtr1.AccessLevel;

                                        //====================== Account No Validation setting by Amol =========================================
                                        var CommonSet = udb.CommonSettings.Where(a => a.AppName == "CTSCONFIG1" && a.SettingName == "AccValidation").FirstOrDefault();
                                        if (CommonSet != null)
                                            Session["AccValidation"] = CommonSet.SettingValue;
                                        //logerror("In common setting acc validation", "In common setting acc validation" + "->  In common setting acc validation");
                                        if (Session["UserManagementVersion2"].ToString() == "Y")
                                        {
                                            //logerror("In User management version Y", "In User management version Y" + "->  In User management version Y");
                                            if (Session["RoleName"].ToString() == "IDM MAKER" || Session["RoleName"].ToString() == "IDM CHECKER")
                                            {
                                                //logerror("In User management version Y maker checker", "In User management version Y maker checker" + "->  In User management version Y maker checker");
                                                @Session["glob"] = true;
                                                Session["processdate"] = DateTime.Now.ToString();
                                                return RedirectToAction("IWIndex", "Home");
                                            }
                                        }
                                        //logerror("In Login end", "In Login end" + "->  In Login end");
                                        //return RedirectToAction("CustDomDateSelection", "Login", new { Accesslevel = usmtr1.AccessLevel, userlogin = userlogin });
                                        //---END
                                        return RedirectToAction("LoginIndex", "Login", new { Accesslevel = usmtr1.AccessLevel, userlogin = userlogin });

                                    }
                                    else
                                    {
                                        UserMasterActivity uma = new UserMasterActivity();
                                        uma.Action = "UserId Wrong";
                                        uma.ActionBy = null;
                                        uma.Actiondate = DateTime.Now;
                                        uma.UserId = null;
                                        uma.comments = UserId;
                                        usrlg.UserMasterActivities.Add(uma);
                                        usrlg.SaveChanges();
                                        //logerror("In UserMasterActivity model null ", "In UserMasterActivity model null " + "->  In UserMasterActivity model null ");
                                        //ModelState.AddModelError("", "Oops! userid/password is wrong");
                                        ViewBag.msg = "Oops! userid/password is wrong";
                                        return View("Index");
                                    }


                                }
                                catch(Exception e)
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
                            else if (jObject["error"] != null || jObject["error_description"] != null)
                            {
                                //MessageBox.Show(jObject["error_description"].ToString(), jObject["error"].ToString());
                                ViewBag.msg = jObject["error_description"].ToString() + " " + jObject["error"].ToString();
                                return View("Index");
                            }
                            else
                            {
                                //Invalid Reponse from API
                                ViewBag.msg = "Oops! Invalid Reponse from API";
                                return View("Index");
                            }
                        }
                        else if (jObject["error"] != null || jObject["error_description"] != null)
                        {
                            //MessageBox.Show(jObject["error_description"].ToString(), jObject["error"].ToString());
                            ViewBag.msg = jObject["error_description"].ToString() + " " + jObject["error"].ToString();
                            return View("Index");
                        }
                        else
                        {
                            //Invalid Reponse from API
                            ViewBag.msg = "Oops! Invalid Reponse from API";
                            return View("Index");
                        }
                    }
                    catch (Exception Ex)
                    {
                        //MessageBox.Show(Ex.Message, "Runtime Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    return View("Index");
                }
                else
                {
                    //logerror("In Login else", "In Login else" + "->  In Login else");
                    var MFA_Login = udb.CommonSettings.FirstOrDefault((p) => p.AppName == "CTSCONFIG1" && p.SettingName == "MFA_Login")?.SettingValue;
                    //logerror("In Login check MFA setting ", "MFA Setting => " + MFA_Login);
                    if (MFA_Login == null || MFA_Login == "")
                    {
                        return View("index_old");
                    }
                    else
                    {
                        if(MFA_Login == "Y")
                        {
                            string sso_url = System.Configuration.ConfigurationManager.AppSettings["MFAURL"].ToString() + "authorize?client_id=";
                            sso_url += System.Configuration.ConfigurationManager.AppSettings["ClientId"].ToString() + "&response_type=code&scope=openid%20profile&redirect_uri=" + System.Configuration.ConfigurationManager.AppSettings["SenderUrl"].ToString();
                            //string sso_url = System.Configuration.ConfigurationManager.AppSettings["SSO"].ToString();


                            return Redirect(sso_url);
                        }
                        else
                        {
                            //return View("Index_Old");
                            return RedirectToAction("Index_Old", new { id = 0 });
                        }
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

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "Login HttpGet Index-" + innerExcp });
            }
        }

        public ActionResult LoginIndex(string Accesslevel = null, bool userlogin = true)
        {
            try
            {
                //logerror("In LoginIndex start",  "->  In Login start and AccessLevel - " + Accesslevel + " and userlogin - " + userlogin);
                ViewBag.AccessLevel = Accesslevel;
                ViewBag.UserLogin = userlogin;
                return View("LoginIndex");
            }
            catch(Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "Login HttpGet LoginIndex- " + innerExcp });
            }
        }

        public ActionResult Index2(string code, string iss, string client_id)
        {
            
            try
            {
                //ViewBag.Url_code = code;
                //ViewBag.Url_iss = iss;
                //ViewBag.Url_client_id = client_id;


                try
                {
                    string sso_url = System.Configuration.ConfigurationManager.AppSettings["MFAURL"].ToString() + "access_token?grant_type=authorization_code&code=" + code +"&client_id=";
                    sso_url += System.Configuration.ConfigurationManager.AppSettings["ClientId"].ToString() + "&client_secret="; 
                    sso_url += System.Configuration.ConfigurationManager.AppSettings["ClientSecret"].ToString()+ "&redirect_uri=" + Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.IndexOf("Index2")) + "Index3/";


                    return Redirect(sso_url);
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

                //return View();
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

        public ActionResult Index3(string code)
        {
            ViewBag.Url_Json = code;
            return View();
        }

        public ActionResult Index4(string access_token, string scope, string id_token, string token_type, string expires_in, string nonce)
        {

            try
            {
                //ViewBag.Url_code = code;
                //ViewBag.Url_iss = iss;
                //ViewBag.Url_client_id = client_id;


                try
                {
                    string sso_url = System.Configuration.ConfigurationManager.AppSettings["MFAURL"].ToString() + "idtokeninfo?client_id=";
                    sso_url += System.Configuration.ConfigurationManager.AppSettings["ClientId"].ToString() + "&client_secret=";
                    sso_url += System.Configuration.ConfigurationManager.AppSettings["ClientSecret"].ToString() + "&id_token=" + id_token;


                    return Redirect(sso_url);
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

                //return View();
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


        private void logerror(string errormsg, string errordesc)
        {
            ErrorDisplay er = new ErrorDisplay();
            string ServerPath = "";
            string filename = "";
            string fileNameWithPath = "";
            //FormsAuthentication.SignOut();


            //ViewBag.Error = e.InnerException;

            //-------------------------------- ServerPath = Server.MapPath("~/Logs/");----
            ServerPath = Server.MapPath("~/Logs/");
            if (System.IO.Directory.Exists(ServerPath) == false)
            {
                System.IO.Directory.CreateDirectory(ServerPath);
            }
            filename = DateTime.Now.ToString("ddMMyyyy") + "Logs.txt";
            fileNameWithPath = ServerPath + filename;
            System.IO.StreamWriter str = new System.IO.StreamWriter(fileNameWithPath, true, System.Text.Encoding.Default);
            //  str.WriteLine("hello");
            str.WriteLine(DateTime.Now.ToShortTimeString() + ": Exception: " + errormsg);
            str.WriteLine(DateTime.Now.ToShortTimeString() + ": StackTrace: " + errordesc);
            str.Close();
        }

        public ActionResult Index_Old(int id = 0)
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

                if (id == 1)
                {
                    ViewBag.disable = true;
                    ViewBag.meg = "UserId is disabled !";
                }
                if (id == 2)
                {
                    ViewBag.disable = true;
                    ViewBag.meg = "Oops! Userid/Password is wrong !";
                }
                if (id == 3)
                {
                    ViewBag.disable = true;
                    ViewBag.meg = "Password has been change successfully !";
                }
                if (id == 4)
                {
                    ViewBag.disable = true;
                    ViewBag.meg = "Oops! Your id has not approved, Please contact to administrator !";
                }
                if (id == 5)
                {
                    ViewBag.disable = true;
                    ViewBag.meg = "Oops! Failed to fetch IsActive column value!";
                }
                if (id == 6)
                {
                    ViewBag.disable = true;
                    ViewBag.meg = "Oops! Your id has been disable, Please contact to administrator !";
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

                var SessionValue = AESEncrytDecry.RandomString(16);

                Session["sessionVal"] = SessionValue.ToString();

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

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Index_Old(AppStResult AppStr = null, string OW = null)
        {
            //logerror("Index_Old method start ", "Index_Old method start " + "->  Index_Old method start ");
            //logerror("Start Login ", "Start Login " + "->  Start Login ");
            try
            {
                var uName = Request.Form["valN"];
                var uPass = Request.Form["valY"];
                var key = Request.Form["sess"];

                var decryptedUserName = AESEncrytDecry.DecryptStringAES(uName, key);
                AppStr.name = decryptedUserName.ToString();

                var decryptedPassword = AESEncrytDecry.DecryptStringAES(uPass, key);
                AppStr.pass = decryptedPassword.ToString();

                string form = "dd-MM-yyyy hh:mm:ss";
                //if (ModelState.IsValid)
                if (AppStr.name != "" && AppStr.pass != "")
                {
                    //logerror("In Model is valid ", "In Model is valid " + "->  In Model is valid ");
                    var CustomerBankCode = "";
                    ViewBag.CustomerBankCode = CustomerBankCode;
                    CustomerBankCode = udb.CommonSettings.FirstOrDefault((p) => p.AppName == "CTSCONFIG1" && p.SettingName == "BankCode")?.SettingValue;
                    if (CustomerBankCode != null || CustomerBankCode != "")
                    {
                        if(CustomerBankCode == "641")
                        {
                            var userModel = db.UserMasters.Where(m => m.LoginID == AppStr.name).SingleOrDefault();
                            if (userModel != null)
                            {
                                if (userModel.IsActive != 1)
                                {
                                    ModelState.AddModelError("", "Oops! Your id has not approved, Please contact to administrator !");
                                    return View("Index_Old");
                                }
                            }
                        }
                    }
                    //logerror("After Customer Bank Code ", "After Customer Bank Code " + "->  After Customer Bank Code ");
                    Session["clearingtype"] = Request.Form["clearing"].ToString();
                    LoginLogoutAudit linlout = new LoginLogoutAudit();

                    string format = "yyyy-MM-dd hh:mm:ss";
                    string commdate = DateTime.Now.ToString(format);
                    int invlidatpass, invldpasspar;

                    //////-------------------------Chnages on 06/05/2017------------
                    //string midelpass = "";
                    //string midelname = "";
                    ////----------------------------Pass---------------For Axis Bank---Client side encryption
                    //for (int i = 0; i < AppStr.pass.Length; i++)
                    //{
                    //    //midelpass = midelpass + Convert.ToChar((int)AppStr.pass[i] + 13);
                    //    //if (AppStr.pass[i].ToString() == "I")
                    //    //    midelpass = midelpass + "I";
                    //    //else
                    //    midelpass = midelpass + Convert.ToChar((int)AppStr.pass[i] + 13);
                    //}
                    //AppStr.pass = midelpass;
                    //logerror("After password length ", "After password length " + "->  After password length ");
                    ////----------------------------Name--------------- //----------------------------Uncomment---------------
                    //for (int i = 0; i < AppStr.name.Length; i++)
                    //{
                    //    midelname = midelname + Convert.ToChar((int)AppStr.name[i] + 13);
                    //}
                    // -------------------------

                    //AppStr.name = midelname;
                    //----------------------------Uncomment---------------
                    string finpass = null;

                    //-------------------Added On 22-03-2017--------------
                    var model = db.UserMasters.Where(m => m.LoginID == AppStr.name).SingleOrDefault();
                    if (model == null)
                    {
                        UserMasterActivity uma = new UserMasterActivity();
                        uma.Action = "UserId Wrong";
                        uma.ActionBy = null;
                        uma.Actiondate = DateTime.Now;
                        uma.UserId = null;
                        uma.comments = AppStr.name;
                        usrlg.UserMasterActivities.Add(uma);
                        usrlg.SaveChanges();
                        //logerror("In UserMasterActivity model null ", "In UserMasterActivity model null " + "->  In UserMasterActivity model null ");
                        //ModelState.AddModelError("", "Oops! userid/password is wrong");
                        //return View("Index");

                        return RedirectToAction("Index_Old", new { id = 2 });
                    }
                    //logerror("In UserMasterActivity model not null ", "In UserMasterActivity model not null " + "->  In UserMasterActivity model not null ");
                    if (model != null)
                    {
                        if (model.Active == false)
                        {
                            //logerror("In model not null acitve false ", "In model not null acitve false " + "->  In model not null acitve false ");
                            //ModelState.AddModelError("", "Oops! Your id has been disable, Please contact to administrator !");
                            //return View("Index");

                            return RedirectToAction("Index_Old", new { id = 6 });
                        }
                    }
                    //logerror("In model not null acitve not false ", "In model not null acitve not false " + "->  In model not null acitve not false ");
                    //----------------------------Added on 07-08-2019------------------BY Abid------------------------
                    string dname = null;
                    var DomiaName = udb.CommonSettings.Where(m => m.AppName == "CTSCONFIG1" && m.SettingName == "DomainName").SingleOrDefault();
                    if (DomiaName != null)
                        dname = DomiaName.SettingValue;
                    string adauth = null;
                    var CommAdAuth = udb.CommonSettings.Where(m => m.AppName == "CTSCONFIG1" && m.SettingName == "ADAuthentication").SingleOrDefault();
                    if (CommAdAuth != null)
                        adauth = CommAdAuth.SettingValue;
                    //logerror(dname, dname.ToString() + "->  DomainName");
                    //logerror(adauth, adauth.ToString() + "->  AD Authentication setting");
                    //logerror(AppStr.name, AppStr.name.ToString() + "->  UserName");
                    //logerror(model.UsertType, model.UsertType.ToString() + "->  User type and going to AD IF function");
                    Session["UserTypeForSQ"] = model.UsertType.ToLower();
                    if (adauth == "Y" && (model.UsertType.ToLower() == "bank_user" || model.UsertType.ToLower() == "scanning_user"))
                    {
                        try
                        {
                            //logerror(dname, dname.ToString() + "-> In AD -- DomainName");
                            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, dname))
                            {
                                //logerror(AppStr.name, AppStr.name.ToString() + "-> In AD Function -- UserName");
                                //logerror(AppStr.pass, AppStr.pass.ToString() + "-> In AD -- Password");
                                bool isValid = pc.ValidateCredentials(AppStr.name, AppStr.pass);
                                //logerror(isValid.ToString(), isValid.ToString() + "-> In AD Function -- validation result");
                                if (isValid == false)
                                {
                                    UserMasterActivity uma = new UserMasterActivity();
                                    uma.Action = "Password Wrong";
                                    uma.ActionBy = null;
                                    uma.Actiondate = DateTime.Now;
                                    uma.UserId = null;
                                    uma.comments = "LoginId is - " + AppStr.name + " and Invalid Password Entered";
                                    usrlg.UserMasterActivities.Add(uma);
                                    usrlg.SaveChanges();
                                    //logerror("In AD isValid false ", "In AD isValid false " + "->  In AD isValid false ");
                                    //ModelState.AddModelError("", "Oops! User Id or Password Wrong!");
                                    //return View("Index");

                                    return RedirectToAction("Index_Old", new { id = 2 });
                                }
                            }
                        }
                        catch(Exception e)
                        {
                            logerror(e.Message.ToString(), e.Message.ToString() + "-> In AD catch Function -- e.Message");
                            logerror(e.InnerException.ToString(), e.InnerException.ToString() + "-> In AD catch Function -- e.InnerException");
                        }
                        
                    }
                    else
                    {
                        //logerror("In AD Else", "In AD Else" + "->  In AD Else function");
                        finpass = cmf.EncryptPassword(AppStr.pass);
                        //finpass = aes.EncryptDBPassword(AppStr.pass);
                        //finpass = aes.EncryptDBPassword("Pass@123");

                        model = db.UserMasters.Where(m => m.LoginID == AppStr.name && m.Password == finpass).SingleOrDefault();
                        if (model != null)
                        {
                            ViewBag.name = model.Title + " " + model.FirstName;
                            ViewBag.wrong = " ";
                            //----A----
                            var usmtr = db.UserMasters.Where(u => u.LoginID == AppStr.name).SingleOrDefault();
                            if (usmtr != null)
                            {
                                if (usmtr.Active == false)
                                {
                                    //ModelState.AddModelError("", "Oops! Your id has been disable, Please contact to administrator !");
                                    //return View("Index");

                                    return RedirectToAction("Index_Old", new { id = 6 });
                                }
                                var polcy = db.AppSecPolicies.Where(m => m.ID == model.AppSecPolicieID).SingleOrDefault();
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
                                            return RedirectToAction("Index_Old", new { id = 6 });
                                            //ModelState.AddModelError("", "Oops! Your id has been disable, Please contact to administrator !");
                                            //return View("Index");
                                        }
                                    }
                                }

                                if (usmtr.FirstLogin == true)
                                {
                                    linlout.User_ID = model.ID;
                                    linlout.LoginDateTime = Convert.ToDateTime(commdate);
                                    linlout.LogoutDateTime = DateTime.Now;

                                    Session["logintime"] = commdate;

                                    usrlg.LoginLogoutAudits.Add(linlout);
                                    Session["uid"] = model.ID;
                                    Session["LoginID"] = model.LoginID;

                                    usmtr.InvalidPasswordAttempts = 0;
                                    usmtr.LastLogin = Convert.ToDateTime(DateTime.Now.ToString(format));
                                    db.SaveChanges();
                                    return RedirectToAction("ChangePassword");
                                }

                                var lastpaswrd = (from ph in usrlg.PasswordHistories
                                                  orderby ph.ID descending
                                                  where ph.User_ID == model.ID
                                                  select ph).FirstOrDefault();

                                if (lastpaswrd != null)
                                {
                                    DateTime demidate = Convert.ToDateTime(lastpaswrd.PassworChangeDate);
                                    TimeSpan ts = DateTime.Now.Subtract(demidate);
                                    int intBounceentryCnt = ts.Days;
                                    if (intBounceentryCnt > polcy.PwdExpiryDays)
                                    {
                                        Session["uid"] = model.ID;
                                        Session["LoginID"] = model.LoginID;
                                        usmtr.InvalidPasswordAttempts = 0;
                                        usmtr.LastLogin = Convert.ToDateTime(DateTime.Now.ToString(format));
                                        db.SaveChanges();
                                        return RedirectToAction("ChangePassword");
                                    }
                                }
                                //---------Commented On 23/10/2018-----------------
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
                                            return RedirectToAction("Index_Old", new { id = 6 });
                                            //ModelState.AddModelError("", "Oops! Your id has been disable, Please contact to administrator !");
                                            //return View("Index");
                                        }
                                    }
                                }
                            }
                            //---B
                            //---C                       

                        }
                        else
                        {

                            var usmtr = db.UserMasters.Where(u => u.LoginID == AppStr.name).SingleOrDefault();

                            if (usmtr != null)
                            {
                                var polcy1 = db.AppSecPolicies.Where(m => m.ID == usmtr.AppSecPolicieID).SingleOrDefault();
                                invlidatpass = usmtr.InvalidPasswordAttempts;
                                invldpasspar = polcy1.InvalidAttemptsAllowed;
                                if (invlidatpass >= invldpasspar)
                                {
                                    usmtr.Active = false;
                                    db.SaveChanges();
                                    return RedirectToAction("Index_Old", new { id = 6 });
                                    //ModelState.AddModelError("", "Oops! User Id has been disable, Please contact to administrator !!");
                                    //return View("Index");
                                }
                                else
                                {
                                    usmtr.InvalidPasswordAttempts = usmtr.InvalidPasswordAttempts + 1;
                                    db.SaveChanges();
                                }
                            }

                            UserMasterActivity uma = new UserMasterActivity();
                            uma.Action = "Password Wrong";
                            uma.ActionBy = null;
                            uma.Actiondate = DateTime.Now;
                            uma.UserId = null;
                            uma.comments = "LoginId is - " + AppStr.name + " and Invalid Password Entered";
                            usrlg.UserMasterActivities.Add(uma);
                            usrlg.SaveChanges();

                            //ModelState.AddModelError("", "Oops! User Id or Password Wrong!");
                            //return View("Index");

                            return RedirectToAction("Index_Old", new { id = 2 });
                        }

                    }
                    //-------------------------------AD Auth----END---------------------
                    //A-------------
                    var usmtr1 = db.UserMasters.Where(u => u.LoginID == AppStr.name).SingleOrDefault();
                    if (usmtr1 != null)
                    {
                        //logerror("After AD usermaster not null ", "After AD usermaster not null" + "->  After AD usermaster not null");
                        if (usmtr1.Active == false)
                        {
                            //logerror("After AD usermaster not null active false ", "After AD usermaster not null active false" + "->  After AD usermaster not null active false");
                            //ModelState.AddModelError("", "Oops! Your id has been disable, Please contact to administrator !");
                            //return View("Index");

                            return RedirectToAction("Index_Old", new { id = 6 });
                        }
                        //------------B
                        var loginout = usrlg.LoginLogoutAudits.Where(m => m.User_ID == model.ID).OrderByDescending(m => m.ID).FirstOrDefault();
                        if (loginout != null)
                            Session["lastlogin"] = loginout.LoginDateTime.ToString(form);
                        //logerror("After AD after loginlogout ", "After AD after loginlogout" + "->  After AD after loginlogout");
                        linlout.User_ID = model.ID;
                        linlout.LoginDateTime = Convert.ToDateTime(commdate);
                        linlout.LogoutDateTime = DateTime.Now;

                        Session["logintime"] = commdate;

                        usrlg.LoginLogoutAudits.Add(linlout);
                        // string sid = Session.SessionID;
                        usrlg.SaveChanges();
                        //logerror("After AD after loginlogout add userlog ", "After AD after loginlogout add userlog" + "->  After AD after loginlogout add userlog");

                        Session["LoginID"] = model.LoginID;
                        Session["uid"] = model.ID;
                        Session["title"] = model.Title;
                        Session["fname"] = model.FirstName;
                        Session["LgnName"] = model.LoginID;
                        Session.Timeout = 300;

                        //=================== Check User Management Setting By Amol====================================

                        var UserManagementVersion2 = "N";
                        ViewBag.UserManagementVersion2 = UserManagementVersion2;
                        UserManagementVersion2 = udb.CommonSettings.FirstOrDefault((p) => p.AppName == "CTSCONFIG1" && p.SettingName == "UserManagement_v2")?.SettingValue;
                        if (UserManagementVersion2 == null || UserManagementVersion2 == "")
                        {
                            //logerror("UserManagement version null ", "UserManagement version null" + "->  UserManagement version null");
                            ViewBag.UserManagementVersion2 = "N";
                        }
                        else
                        {
                            //logerror("UserManagement version not null ", "UserManagement version not null" + "->  UserManagement version not null");
                            if (UserManagementVersion2 == "Y")
                            {
                                ViewBag.UserManagementVersion2 = "Y";
                            }
                            else
                            {
                                ViewBag.UserManagementVersion2 = "N";
                            }
                        }
                        Session["UserManagementVersion2"] = ViewBag.UserManagementVersion2;
                        //=================== Check User Management Setting End ====================================
                        //logerror(Session["UserManagementVersion2"].ToString(), Session["UserManagementVersion2"].ToString() + "->  UserManagement version value");
                        if (Session["UserManagementVersion2"].ToString() == "Y")
                        {
                            //logerror("UserManagement version Y ", "UserManagement version Y" + "->  UserManagement version Y");
                            var userRole = udb.RoleMaster.Where(a => a.ID == model.RoleId).SingleOrDefault();
                            var roleName = userRole.RoleName;
                            Session["RoleName"] = roleName;
                            Session["DE"] = userRole.IsDataEntry;
                            Session["Ds"] = userRole.IsDashboard;
                            Session["fildwnd"] = userRole.IsFileDownload;
                            Session["QC"] = userRole.IsQC;
                            Session["QueryMod"] = userRole.IsQueryWithModification;
                            Session["RejectRepair"] = userRole.IsRejectRepaire;
                            Session["Report"] = userRole.IsReport;
                            Session["UserManagment"] = userRole.IsUserManagement;
                            Session["VF"] = userRole.IsVerification;
                            Session["Archv"] = userRole.IsArchieve;
                            Session["Master"] = userRole.IsMasters;
                            Session["Mesgbrd"] = userRole.IsMessageBroadcasting;
                            Session["Query"] = userRole.IsQuery;
                            Session["Settg"] = userRole.IsSettings;
                            Session["SOD"] = userRole.IsSOD;
                            Session["chirjct"] = userRole.IsChiReject;
                            Session["role"] = userRole.IsRoleMaster;
                            Session["RVF"] = userRole.IsReVerification;
                            Session["CCPH"] = userRole.IsCCPH_Verification;
                            Session["UserManagementChecker"] = userRole.IsUserManagementChecker;
                            Session["RoleMasterChecker"] = userRole.IsRoleMasterChecker;
                            //Session["RVF4"] = chkroles1.RVF4;
                            Session["ProType"] = Request.Form["clearing"];
                            //-------------------Added On 25/01/2017------------For Inward L2AmountWise------------
                            Session["L2StartLimit"] = userRole.L2StartLimit;
                            Session["L2StopLimit"] = userRole.L2StopLimit;
                            //logerror("UserManagement version Y end ", "UserManagement version Y end" + "->  UserManagement version Y end");
                        }
                        else
                        {
                            //logerror("UserManagement version N", "UserManagement version N" + "->  UserManagement version N");
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
                            //Session["RVF4"] = chkroles1.RVF4;
                            Session["ProType"] = Request.Form["clearing"];
                            //-------------------Added On 25/01/2017------------For Inward L2AmountWise------------
                            Session["L2StartLimit"] = model.L2StartLimit;
                            Session["L2StopLimit"] = model.L2StopLimit;
                            //logerror("UserManagement version N end", "UserManagement version N end" + "->  UserManagement version N end");
                        }
                        
                        FormsAuthentication.SetAuthCookie(model.LoginID, false);

                        //----------------------------Uncomment---------------For deployment
                        foreach (string s in Response.Cookies.AllKeys)
                        {
                            if (s == FormsAuthentication.FormsCookieName || s.ToLower() == "asp.net_sessionid")
                            {
                                Response.Cookies[s].Secure = true;
                            }
                        }
                        //logerror("After form Authentication", "After form Authentication" + "->  After form Authentication");
                        //------------Checking previous Login-------------
                        if (Session["uid"] == null)
                            ViewBag.userlogin = "false";

                        bool userlogin = true;
                        int uid = (int)Session["uid"];
                        //-----------End
                        //---C
                        if (model.sessionid != null)
                        {
                            //logerror("In session id not null", "In session id not null" + "->  In session id not null");
                            if (model.sessionid != Session.SessionID)
                            {
                                userlogin = false;
                                usmtr1.LastLogin = Convert.ToDateTime(DateTime.Now.ToString(format));
                            }
                            else
                            {
                                //logerror("In session id not null in else", "In session id not null in else" + "->  In session id not null in else");
                                userlogin = true;
                                usmtr1.sessionid = Session.SessionID;
                                usmtr1.loginFlg = 1;
                                usmtr1.InvalidPasswordAttempts = 0;
                                usmtr1.LastLogin = Convert.ToDateTime(DateTime.Now.ToString(format));
                                Session["afterlogin"] = true;
                                
                            }
                            db.SaveChanges();
                        }
                        else
                        {
                            //logerror("In session id null", "In session id null" + "->  In session id  null");
                            userlogin = true;
                            usmtr1.sessionid = Session.SessionID;
                            usmtr1.loginFlg = 1;
                            usmtr1.InvalidPasswordAttempts = 0;
                            usmtr1.LastLogin = Convert.ToDateTime(DateTime.Now.ToString(format));
                            Session["afterlogin"] = true;
                            db.SaveChanges();
                        }
                        Session["Accesslevel"] = usmtr1.AccessLevel;

                        //====================== Account No Validation setting by Amol =========================================
                        var CommonSet = udb.CommonSettings.Where(a => a.AppName == "CTSCONFIG1" && a.SettingName == "AccValidation").FirstOrDefault();
                        if (CommonSet != null)
                            Session["AccValidation"] = CommonSet.SettingValue;
                        //logerror("In common setting acc validation", "In common setting acc validation" + "->  In common setting acc validation");
                        if (Session["UserManagementVersion2"].ToString() == "Y")
                        {
                            //logerror("In User management version Y", "In User management version Y" + "->  In User management version Y");
                            if (Session["RoleName"].ToString() == "IDM MAKER" || Session["RoleName"].ToString() == "IDM CHECKER")
                            {
                                //logerror("In User management version Y maker checker", "In User management version Y maker checker" + "->  In User management version Y maker checker");
                                @Session["glob"] = true;
                                Session["processdate"] = DateTime.Now.ToString();
                                return RedirectToAction("IWIndex", "Home");
                            }
                        }
                        //logerror("In Login end", "In Login end" + "->  In Login end");
                        return RedirectToAction("CustDomDateSelection", "Login", new { Accesslevel = usmtr1.AccessLevel, userlogin = userlogin });
                        //---END


                    }//-------A

                }
                else
                {
                    return RedirectToAction("Index_Old", new { id = 2 });
                    //ModelState.AddModelError("", "Oops! User Id or Password Wrong!");
                    //return View("Index");
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
            //return View("index");
            return RedirectToAction("Index_Old", new { id = 0 });
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
        public ActionResult CustDomDateSelection(string Accesslevel = null, bool userlogin = true, string clearingType = null)
        {
            selectcustprocDate selectcusdate = new selectcustprocDate();
            selectcusdate.userlogin = userlogin;
            try
            {
                //logerror("In CustDomDateSelection start", "->  In CustDomDateSelection start and AccessLevel - " + Accesslevel + " and userlogin - " + userlogin + " and clearingType - " + clearingType);
                if (clearingType != null)
                {
                    Session["clearingtype"] = clearingType;
                    Session["ProType"] = clearingType;
                }
                //logerror("In CustDomDateSelection start ", "->  Session['uid'] - " + Session["uid"].ToString());
                if (Session["uid"] != null)
                {
                    //logerror("In CustDomDateSelection start under uid ", "->  Session['uid'] - ");
                    List<int> orgnisids = new List<int>();
                    int uid = (int)Session["uid"];
                    selectcusdate.Accesslevel = Accesslevel;

                    if (Accesslevel == "ORG")
                    {
                        //logerror("In CustDomDateSelection under Accesslevel ORG ", "->  Accesslevel - " + Accesslevel);
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
                        //logerror("In CustDomDateSelection under Accesslevel ORG ", "->  selectcusdate - " + selectcusdate);
                        if (resut.Count > 1)
                        {
                            //logerror("In CustDomDateSelection under Accesslevel ORG under resut if ", "->  resut.Count - " + resut.Count);
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
                            //logerror("In CustDomDateSelection under Accesslevel ORG under resut else ", "->  resut.Count - " + resut.Count);
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
                        //logerror("In CustDomDateSelection under Accesslevel ORG end ", "->  Accesslevel - " + Accesslevel);

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
                    //logerror("In CustDomDateSelection start under uid end ", "->  ViewBag.ClearingType - " + ViewBag.ClearingType);
                }
                //logerror("In CustDomDateSelection  end ", "-> selectcusdate - " + selectcusdate);
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

                    //                    DateTime procdate = DateTime.ParseExact(Request.Form["procdateval"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime procdate = Convert.ToDateTime(Request.Form["procdateval"].ToString().Substring(6, 4) + "-" + Request.Form["procdateval"].ToString().Substring(3, 2) + "-" + Request.Form["procdateval"].ToString().Substring(0, 2));

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
                    var CommonSetHigVl3 = udb.ApplicationSettings.Where(a => a.CustomerId == domainseting.CustomerID && a.SettingName == "L3AmountThreshold").FirstOrDefault();

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

                    //======= Added by Amol on 17/02/2024 for High value L3 ============
                    if (CommonSetHigVl3 != null)
                        Session["HIGHAMTL3"] = CommonSetHigVl3.SettingValue;
                    else
                        Session["HIGHAMTL3"] = 0;

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

                    //====================== Bank Code setting by Amol =========================================
                    int custid = Convert.ToInt16(Session["CustomerID"]);
                    var presentingBankRoutNo = (from a in udb.CustomerMasters
                                                where a.Id == custid
                                                select
                                                    a.PresentingBankRouteNo
                                    ).FirstOrDefault().ToString();

                    var BankCode = presentingBankRoutNo.Substring(3, 3);
                    Session["BankCode"] = BankCode;

                    //04-10-24=====added by sambhaji dem cxf file time setting start=====
                    //cxf create
                    var DEMCXF_Start = udb.ApplicationSettings.Where(a => a.CustomerId == selectedcustid && a.SettingName == "DEM_CXF_C_Start").FirstOrDefault();
                    var DEMCXF_End = udb.ApplicationSettings.Where(a => a.CustomerId == selectedcustid && a.SettingName == "DEM_CXF_C_End").FirstOrDefault();

                    if (DEMCXF_Start != null)
                    {
                        Session["DEMCXF_C_Start"] = DEMCXF_Start.SettingValue;
                    }
                    else
                    {
                        Session["DEMCXF_C_Start"] = "10:00";
                    }

                    if (DEMCXF_End != null)
                    {
                        Session["DEMCXF_C_End"] = DEMCXF_End.SettingValue;
                    }
                    else
                    {
                        Session["DEMCXF_C_End"] = "15:45";
                    }

                    //cxf upload
                    var DEMCXF_U_Start = udb.ApplicationSettings.Where(a => a.CustomerId == selectedcustid && a.SettingName == "DEM_CXF_U_Start").FirstOrDefault();
                    var DEMCXF_U_End = udb.ApplicationSettings.Where(a => a.CustomerId == selectedcustid && a.SettingName == "DEM_CXF_U_End").FirstOrDefault();
                    if (DEMCXF_U_Start != null)
                    {
                        Session["DEMCXF_U_Start"] = DEMCXF_U_Start.SettingValue;
                    }
                    else
                    {
                        Session["DEMCXF_U_Start"] = "10:00";
                    }
                    if (DEMCXF_U_End != null)
                    {
                        Session["DEMCXF_U_End"] = DEMCXF_U_End.SettingValue;
                    }
                    else
                    {
                        Session["DEMCXF_U_End"] = "15:45";
                    }


                    //04-10-24=====added by sambhaji dem cxf file time setting end=====




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
                    //DateTime procdate = DateTime.ParseExact(Request.Form["procdateval"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime procdate = Convert.ToDateTime(Request.Form["procdateval"].ToString().Substring(6, 4) + "-" + Request.Form["procdateval"].ToString().Substring(3, 2) + "-" + Request.Form["procdateval"].ToString().Substring(0, 2));

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

                    //====================== Bank Code setting by Amol =========================================
                    int custid = Convert.ToInt16(Session["CustomerID"]);
                    var presentingBankRoutNo = (from a in udb.CustomerMasters
                                                where a.Id == custid
                                                select
                                                    a.PresentingBankRouteNo
                                    ).FirstOrDefault().ToString();

                    var BankCode = presentingBankRoutNo.Substring(3, 3);
                    Session["BankCode"] = BankCode;

                    //sambhaji 30-09-24
                    var ExpTimeList = GetExpiryTime();
                    var selectList = ExpTimeList.Select((x, index) => new
                    {
                        x.ExpiryTime,
                        DisplayText = $"Cycle {index + 1} [{x.StartTime} - {x.EndTime} -> Expiry {DateTime.Parse(x.ExpiryTime).ToString("h:mm tt")}]"
                    }).ToList();

                    // Create a SelectListItem list from the expiry times
                    var expiryTimeSelectList = selectList.Select(x => new SelectListItem
                    {
                        Value = x.ExpiryTime.ToString(),
                        Text = x.DisplayText
                    }).ToList();

                    // Add the hardcoded "All" value at the end
                    // expiryTimeSelectList.Add(new SelectListItem { Value = "All", Text = "All" });

                    ViewBag.SessionExpiryTime = expiryTimeSelectList;
                    Session["SessionExpiryTime_dropDown"] = expiryTimeSelectList;

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

        //sambhaji
        public class SessionExpiryTime
        {
            public int Id { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }
            public string ExpiryTime { get; set; }
        }
        public List<SessionExpiryTime> GetExpiryTime()
        {
            List<SessionExpiryTime> list = new List<SessionExpiryTime>();

            try
            {
                using (SqlCommand cmd = new SqlCommand("GetSessionExpiryTime", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            SessionExpiryTime lstObj = new SessionExpiryTime
                            {
                                Id = Convert.ToInt32(rdr["Id"]),
                                StartTime = rdr["StartTime"].ToString(),
                                EndTime = rdr["EndTime"].ToString(),
                                ExpiryTime = rdr["ExpiryTime"].ToString()
                            };
                            list.Add(lstObj);
                        }
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

                logerror("In GetExpiryTime Catch==>" + message, "InnerExp===>" + innerExcp);
            }

            return list;
        }

    }
}
