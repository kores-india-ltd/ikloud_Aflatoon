using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iKloud_Aflatoon.Models;
using System.Web.Security;
using iKloud.Controllers;
using ikloud_Aflatoon;

namespace iKloud_Aflatoon.Controllers
{
    //[Authorize]
    public class DomainSelectionController : Controller
    {
        //
        // GET: /DomainSelection/
        private AflatoonEntities db = new AflatoonEntities();
        public ActionResult Index()
        {
            try
            {
                int uid = (int)Session["uid"];
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            return View();
        }

        public ActionResult SelectDomain()
        {
            int uid;
            try
            {
                uid = (int)Session["uid"];
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            bool flag = true;

            if (TempData["flg"] != null)
                flag = Convert.ToBoolean(TempData["flg"]);

            if (flag == false)
            {
                ViewBag.userlogin = false;
            }
            else
            {
                ViewBag.userlogin = true;
            }
            string proctype = Session["ProType"].ToString();

            var model = from dm in db.UserDomainMappings
                        from dt in db.IwSoD
                        where dm.User_ID == uid &&
                        dt.SoDStatus == 1
                        select new DomainsDates
                        {
                            DomainID = (int)dm.Domain_ID,
                            DomainName = dm.Domain.DomainName,
                            ProcessDates = dt.ProcessingDate,
                            ClearingType = "IW",
                            PostDate = dt.PostDated,
                            StaleDate = dt.StaleDated,
                            CustomerID = dt.CustomerId

                        };


            return View(model);

        }

        public ActionResult SelectedDomain(int id, string name, DateTime dt, string type, DateTime PostDate, DateTime StaleDate, int CustomerID)
        {
            // iKloud.Controllers.LoginController lnc = new iKloud.Controllers.LoginController();
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            var DEData = db.AppSettings.Where(a => a.Domain.ID == id && a.ClearingType == type).FirstOrDefault();
            Session["blnChqAmt"] = DEData.CaptureChqAmount;
            Session["blnCrdAccNo"] = DEData.CaptureChqCrdAccount;
            Session["blnDate"] = DEData.CaptureChqDate;
            Session["blnDrName"] = DEData.CaptureChqDraweeName;
            Session["blnPyName"] = DEData.CaptureChqPayeeName;
            Session["blnSlipAmt"] = DEData.CaptureSlipAmount;
            Session["blnSlipAC"] = DEData.CaptureSlipAccount;
            Session["blnDeBySnippet"] = DEData.DEBySnippet;
            Session["AutoCodelineDecode"] = DEData.AutoCodelineDecode;
            Session["SANCompulsory"] = DEData.SANCompulsory;
            Session["blnDbtAccNo"] = DEData.CaptureChqDbtAccount;
            //Session["Reverification"] = DEData.Reverification;
            //------------- Domain wise a/c length -----------------------
            Session["acfrm"] = DEData.AccLenthFrom;
            Session["acto"] = DEData.AccLenthTo;
            Session["domainid"] = id;
            Session["domainname"] = name;
            Session["processdate"] = dt.ToShortDateString();//dt.ToString("dd/MM/yyyy");
            Session["clearingtype"] = type;
            Session["QCEnabled"] = DEData.QCEnabled;
            Session["glob"] = true;
            //-----------------------------Get Postdated And Stale Cheques--------------
            Session["PostDate"] = PostDate;
            Session["StaleDate"] = StaleDate;
            Session["CustomerID"] = CustomerID;

            string[] ddt = new string[0];
            string dtemp = Session["processdate"].ToString();
            ddt = dtemp.Split('/');
            Session["SnipDate"] = dt.ToString("dd/MM/yyyy");//"04.06.2016";//ddt[1]+"."+ ddt[0]+"." + ddt[2];

            ///---------------Added on 13-02-2017-------------------------------
            var GetAccountDetailsV = db.ApplicationSettings.Where(a => a.CustomerId == CustomerID && a.SettingName == "GetAccountDetails").FirstOrDefault();
            Session["GetAccountDetails "] = GetAccountDetailsV.SettingValue;
            /////-----------------------------------------------------------------

            var custid = db.Domains.Find(id).Customer_ID.ToString();
            Session["CustomerIDTemp"] = custid;
            var CommonSet = db.CommonSettings.Where(a => a.AppName == "CTSCONFIG 1" && a.SettingName == "DEFAULTDS").FirstOrDefault();
            if (CommonSet != null)
                Session["DefaultDS"] = CommonSet.SettingValue;

            var CommonSet1 = db.CommonSettings.Where(a => a.AppName == "BULKCOUNT" && a.SettingName == custid).FirstOrDefault();
            if (CommonSet1 != null)
                Session["bulkcount"] = CommonSet1.SettingValue;
            var CommonSetHigVl = db.CommonSettings.Where(a => a.AppName == "HIGHAMT" && a.SettingName == custid).FirstOrDefault();
            if (CommonSetHigVl != null)
                Session["HIGHAMT"] = CommonSetHigVl.SettingValue;
            var RVERFNHIGHAMT = db.CommonSettings.Where(a => a.AppName == "RVERFNHIGHAMT" && a.SettingName == custid).FirstOrDefault();
            if (RVERFNHIGHAMT != null)
                Session["RVERFNHIGHAMT"] = RVERFNHIGHAMT.SettingValue;

            if (type == "IW")
            {
                var accmodel = db.CommonSettings.Where(s => s.SettingName == custid && s.AppName == "sign").Select(s => s.SettingValue).SingleOrDefault();
                if (accmodel != null)
                    Session["Sign"] = accmodel.ToString();

                Session["blnDbtAccNo"] = DEData.CaptureChqDbtAccount;
                Session["blnDate"] = DEData.CaptureChqDate;
                Session["blnPyName"] = DEData.CaptureChqPayeeName;
                Session["reverification"] = DEData.Reverification;

                var CommonSets = db.CommonSettings.Where(a => a.AppName == "CTSCONFIGIW" && a.SettingName == "WebPath").Select(s => s.SettingValue).SingleOrDefault();
                Session["webpath"] = CommonSets.ToString();

                var IWfilehdr = db.IWFileHDRs.Where(f => f.ProcessingDate == dt.Date).FirstOrDefault();
                if (IWfilehdr != null)
                {
                    Session["Settelmentdate"] = IWfilehdr.SettlementDate.Substring(4, 4) + "/" + IWfilehdr.SettlementDate.Substring(2, 2) + "/" + IWfilehdr.SettlementDate.Substring(0, 2);
                    Session["SessionDate"] = IWfilehdr.SessionDate.Substring(4, 4) + "/" + IWfilehdr.SessionDate.Substring(2, 2) + "/" + IWfilehdr.SessionDate.Substring(0, 2); ;
                }


            }


            // checkLockRecord(model.ID);
            //lnc.checkLockRecord(Convert.ToDateTime(Session["processdate"]), (int)Session["uid"], (int)Session["domainid"], Session["clearingtype"].ToString());
            //FormsAuthentication.Authenticate();
            //FormsAuthentication.SetAuthCookie(Session["uid"].ToString(), false);

            return RedirectToAction("IWIndex", "Home");

        }
    }
}
