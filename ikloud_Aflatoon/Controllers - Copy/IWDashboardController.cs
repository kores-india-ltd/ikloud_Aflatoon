using ikloud_Aflatoon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ikloud_Aflatoon.Controllers
{
    public class IWDashboardController : Controller
    {
        //
        // GET: /IWDashboard/
        IWProcDataContext dc = new IWProcDataContext();
        AflatoonEntities af = new AflatoonEntities();

        public ActionResult Index()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            //----------------------------------Checking user rights--------------------//

            if ((bool)Session["Ds"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            //ViewBag.clrtype = true;
            Session["Clrtype"] = "01";
            return View();
        }
        [HttpPost]
        public ActionResult Index(string btn = null, string sesntp = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            //----------------------------------Checking user rights--------------------//

            if ((bool)Session["Ds"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            if (sesntp == "01")
                Session["Clrtype"] = "01";
            else
                Session["Clrtype"] = "11";
            //ViewBag.clrtype = false;
            return View();
        }
        public ActionResult _IWSummary()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            //----------------------------------Checking user rights--------------------//

            if ((bool)Session["Ds"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            // int Did = (int)Session["domainid"];
            try
            {
                string clrtype = Session["Clrtype"].ToString();
                DateTime pDate = Convert.ToDateTime(Session["processdate"].ToString());
                int custID = Convert.ToInt16(Session["CustomerID"]);

                IWSummary iw = new IWSummary();
                DateTime? setlmntdate = null;
                int? returnCount = null;
                double? returnAmount = 0;

                dc.Sp_IWReturnSummary(pDate, clrtype,custID, ref setlmntdate, ref returnCount, ref returnAmount);
                if (returnCount != 0 && returnCount != null)
                {
                    iw.ProcDate = pDate;
                    iw.ReturnCount = (int)returnCount;
                    iw.SetlmntDate = setlmntdate.ToString();
                    iw.ReturnAmt = Convert.ToDecimal(returnAmount);
                }
                return PartialView("_IWSummary", iw);
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
        public ActionResult IWBatchsummary()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            //----------------------------------Checking user rights--------------------//

            if ((bool)Session["Ds"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            // int Did = (int)Session["domainid"];
            int custID = Convert.ToInt16(Session["CustomerID"]);
            try
            {
                string clrtype = Session["Clrtype"].ToString();
                DateTime pDate = Convert.ToDateTime(Session["processdate"].ToString());
                List<int> ProcId = new List<int>();

                var model = (from m in dc.IWBatchSummaries
                             where m.Processdate == pDate && m.CustomerID == custID && m.ClearingType == clrtype
                             select new IWBatchsummary
                             {
                                 ProcDate = (DateTime)m.Processdate,
                                 BatchNo = (int)m.Batchno,
                                 BatchDesc = (string)m.BatchDescrp,
                                 BatchCount = (int)m.BatchCount,
                                 BatchAmount = (string)m.BatchAmount
                             });

                return PartialView(model);
            }
            catch (Exception e)
            {
                //Server.MapPath(strMappath);
                // ErrorDisplay er = new ErrorDisplay();
                //er.ErrorMessage = e.Message.ToString();
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
                //return View("Error", er);
            }
            // dc.Dispose();
            //
        }
        public ActionResult _IWCommanDS()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            //----------------------------------Checking user rights--------------------//

            if ((bool)Session["Ds"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            //    int Did = (int)Session["domainid"];
            try
            {
                string clrtype = Session["Clrtype"].ToString();
                DateTime pDate = Convert.ToDateTime(Session["processdate"].ToString());
                List<int> ProcId = new List<int>();

                int custID = Convert.ToInt16(Session["CustomerID"]);


                var obj = (from d in dc.IWDashboardNews
                           where d.ProcessDate == pDate && d.CustomerID == custID && d.ClearingType == clrtype
                           select new IWDEStatus
                           {
                               DomainName = d.DomainName,
                               TotalIW = (int)d.TotalIW,

                               //----------For Reverification or L3
                               ATotalRVerf = (int)d.RVerified,
                               AAcceptedRVerf = (int)d.RAccepted,
                               ARjctRVerf = (int)d.RRejected,
                               APendingRVerf = (int)d.RPending,

                               //----------For verification or L2
                               ATotalsVerf = (int)d.Verified,
                               AAcceptedsVerf = (int)d.Accepted,
                               ARjctsVerf = (int)d.Rejected,
                               APendingsVerf = (int)d.VPending,

                               //----------For QC or L1
                               ATotalVerf = (int)d.QCDone,
                               AAcceptedVerf = (int)d.QCAccepted,
                               ARjctVerf = (int)d.QCRejected,
                               APendingVerf = (int)d.QCPending,

                               //----------For Payee
                               PCompletedDE = (int)d.PayeeNDEDone,
                               PPendingDE = (int)d.PayeeNDEPending,
                               PRejectedDE = (int)d.PayeeRejected,

                               //----------For Account
                               ACompletedDE = (int)d.AccDEDone,
                               ARejectedDE = (int)d.AccRejected,
                               APendingDE = (int)d.AccDEPending,
                               //----------For Amount
                               AmtCompletedDE = (int)d.AmountDEDone,
                               AmtRejectedDE = (int)d.AmountRejected,
                               AmtPendingDE = (int)d.AmountDEPending,

                               //----------For Date
                               DCompletedDE = (int)d.DateDEDone,
                               DRejectedDE = (int)d.DateRejected,
                               DPendingDE = (int)d.DateDEPending,

                               //----------For Date QC
                               DCompletedQC = (int)d.DateQCDEDone,
                               DRejectedQC = (int)d.DateQCRejected,
                               DPendingQC = (int)d.DateQCDEPending,

                               //----------For MICR---
                               MicrCompletedDE = (int)d.MICRDEDone,
                               MicrRejectedDE = (int)d.MICRRejected,
                               MicrPendingDE = (int)d.MICRDEPending,


                           }).SingleOrDefault();
                // dc.Dispose();
                return PartialView(obj);
                //dc.Dispose();
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

    }
}
