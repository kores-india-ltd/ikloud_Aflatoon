using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using iKloud.Models;
using System.Configuration;
using System.Data.SqlClient;
using iKloud.Controllers;
using PagedList;
using iTextSharp.text;
namespace iKloudNewUI.Controllers
{
    // [Authorize]
    public class SignVerificationController : Controller
    {
        private iKloudDBContext db = new iKloudDBContext();

        //--------- DeSelection -------------
        public ActionResult IWVFSelection(int id = 0, string msg = null,string vftype=null,string reVf=null)
        {
            if (Session["domainid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            //----------- ----------- -------- Checking user rights ---------- --------- ----------///

            if ((bool)Session[vftype] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = db.Users.Find(uid1);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            if (id == 3)
            {
                ViewBag.novf = true;
                ViewBag.meg = msg;
            }
            List<string> lst = new List<string>();
            lst.Add("01");
            lst.Add("11");
            //if (vftype=="QC")
            //lst.Add("LIC");

            ViewBag.Dname = lst;
            ViewBag.ActionType = vftype;
            ViewBag.ReVf = reVf;
            Session["actiontype"] = vftype;
            Session["Vftype"] = reVf;
            return View();

        }
        [HttpPost]
        public ActionResult IWVFSelection(string DEtype)
        {
            string actiontype = Session["actiontype"].ToString();
            string reVf = Session["Vftype"].ToString();
           
            DEtype = Request.Form["degrp"];
            Session["clrngtype"]=DEtype;
            //if (DEtype=="LIC" && actiontype == "QC" )
            //    return RedirectToAction("LICWVerificAbid","QCFORLIC");
             if (actiontype == "QC")
                 return RedirectToAction("LICWVerificAbid", "QCFORLIC");
               //return RedirectToAction("L1IWVerificAbid");
            else if (actiontype == "VF" && reVf == "Yes")
                return RedirectToAction("L3IWVerificAbid");
            else if (actiontype == "VF")
                return RedirectToAction("IWVerificAbid");
           
            return View();
        }
        //------------QC For Mismatch Cheques-------------------------------//
        public ActionResult L1IWVerificAbid(int maintid = 0, string reason = null, int pendingcnt = 0)
        {
            IW_VerificationEntery Vobj = new IW_VerificationEntery();

            if (Session["domainid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            //----------------------------------Checking user rights--------------------//

            if ((bool)Session["QC"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = db.Users.Find(uid1);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            ViewBag.rtnmodule = "L1IWVerificAbid";
            int domainid = (int)Session["domainid"];
            int uid = (int)Session["uid"];
            DateTime processdt = Convert.ToDateTime(Session["processdate"].ToString());

            bool blnDbtAccNo, blnDate, blnPyName, blnQCEnabled;

            blnDbtAccNo = (bool)Session["blnDbtAccNo"];
            blnDate = (bool)Session["blnDate"];
            blnPyName = (bool)Session["blnPyName"];
            blnQCEnabled = (bool)Session["QCEnabled"];
            ViewBag.DbtAccNo = blnDbtAccNo;
            ViewBag.ChqDate = blnDate;
            ViewBag.PyNme = blnPyName;
            ViewBag.L2 = false;
            if (blnQCEnabled == false)
            {
                return RedirectToAction("Index", "Home", new { id = 3 });
            }
            
            if (maintid != 0)
            {
                IWMainTransaction iwm = db.IWMainTransactions.Find(maintid);
                var model = (from m in db.IWMainTransactions
                             from im in db.IWImageProcessings
                             where m.Process.ID == im.Process.ID && m.File.ID == im.File.ID && m.FileSeqNo == m.FileSeqNo && m.ID == iwm.ID
                             && im.Process.ID == iwm.Process.ID && im.File.ID == iwm.File.ID && im.FileSeqNo == iwm.FileSeqNo

                             select new IW_VerificationEntery
                             {
                                 ID = im.ID,
                                 MID = m.ID,
                                 ProcessID = m.Process.ID,
                                 FrontTiffImagePath = im.FrontTiffImagePath,
                                 BackTiffImagePath = im.BackTiffImagePath,
                                 FrontGreyImagePath = im.FrontGreyImagePath,
                                 BackGreyImagePath = im.BackGreyImagePath,
                                 Amount = m.Amount,
                                 ChqDate = m.ChqDate,
                                 DbtAccNo = m.DbtAccNo,
                                 PayeeName = m.PayeeName,
                                 ChqNo = m.SerialNo,//Chq No
                                 SortCode = m.PayorBankRoutNo, //SortCode
                                 SAN = m.AccountNo, //SAN
                                 TrCode = m.TransCode,
                                 File_ID = m.File.ID,
                                 FileSeqNo = m.FileSeqNo,
                                 BatchNo = m.BatchNo,
                                 BranchName = m.Process.Branch.BranchCode + "-" + m.Process.Branch.BranchName,
                                 JointHolders = m.CBSJointHoldersName,
                                 ClientAccountDtls = m.CBSClientAccountDtls,
                                 DOCType = m.DocType,
                                 PresentingBank = m.PresentingBankRoutNo
                             }).FirstOrDefault(); ;
                ViewBag.pendingcount = pendingcnt;

                if (model.ClientAccountDtls != null)
                {
                    model.modf = true;
                    if (model.ClientAccountDtls.Split('|').ElementAt(1) == "S")
                    {
                        if (model.ClientAccountDtls.Split('|').ElementAt(5).Trim() != "")
                        {
                            string MOP = db.MOPCodeMasters.Find(model.ClientAccountDtls.Split('|').ElementAt(5)).Description;
                            model.MOP = MOP != null ? MOP : "";
                        }
                        else
                        {
                            model.MOP = "";
                        }
                        if (model.ClientAccountDtls.Split('|').ElementAt(6).Trim() != "")
                        {
                            string AccountStatus = db.AccStatusCodeMasters.Find(model.ClientAccountDtls.Split('|').ElementAt(6)).Description;
                            model.AccountStatus = AccountStatus != null ? AccountStatus : "";
                        }
                        else
                        {
                            model.AccountStatus = "";
                        }

                        if (model.ClientAccountDtls.Split('|').ElementAt(12).Trim() != "")
                        {
                            string AccountOwnership = db.AccOwnershipCodeMasters.Find(model.ClientAccountDtls.Split('|').ElementAt(12).ToString()).Description;
                            model.AccountOwnership = AccountOwnership != null ? AccountOwnership : "";
                        }
                        else
                        {
                            model.AccountOwnership = "";
                        }
                    }
                }
                if (reason != null)
                    model.IWRemark = reason.ToString();
                model.IWDecision = "R";
                if (model.DOCType == "C")
                    model.DOCType = "Y";
                else
                    model.DOCType = "N";
                var accmodel = db.CommonSetting.Where(s => s.SettingName == "sign").Select(s => s.SettingValue).SingleOrDefault();
                if (accmodel != null)
                {
                    model.sign = accmodel.ToString();
                }
                return View("IWVerificnabid", model);
            }
            else
            {
                IWSPDataContext IWSP = new IWSPDataContext();
     
                int? ProcessID = null;
                int? FileID = null;
                int? FIleSeqno = null;
                int? imgID = null;
                int? mainID = null;
                double? amount = null;
                string chqdate = null;
                string dbtacno = null;
                string payeename = null;
                string chqno = null;
                string sortcode = null;
                string SAN = null;
                string trncode = null;
                string Branchname = null;
                string JointHolders = null;
                string ClientAccountDtls = null;
                string DOCType = null;
                string PresentingBank = null;
                string frontGreyImagePath = "";
                string frontTiffImagePath = "";
                string backTiffImagePath = "";
                int? batchno = null;

                IWSP.TempSelectL1IWVF(processdt, domainid, uid, Session["clrngtype"].ToString(), ref ProcessID, ref FileID, ref FIleSeqno, ref imgID, ref mainID, ref amount, ref chqdate, ref dbtacno, ref payeename, ref chqno
                    , ref sortcode, ref SAN, ref trncode, ref frontGreyImagePath, ref frontTiffImagePath, ref backTiffImagePath, ref JointHolders, ref ClientAccountDtls, ref DOCType, ref PresentingBank, ref batchno);

                IW_VerificationEntery model1 = new IW_VerificationEntery();
                if (ProcessID != null)
                {
                    model1.ProcessID = (int)ProcessID; model1.File_ID = (int)FileID; model1.FileSeqNo = (int)FIleSeqno; model1.ID = (int)imgID; model1.MID = (int)mainID;
                    model1.Amount = (decimal)amount; model1.ChqDate = chqdate; model1.DbtAccNo = dbtacno; model1.PayeeName = payeename; model1.ChqNo = chqno;
                    model1.SortCode = sortcode; model1.SAN = SAN; model1.TrCode = trncode; model1.FrontGreyImagePath = frontGreyImagePath; model1.FrontTiffImagePath = frontTiffImagePath;
                    model1.BackTiffImagePath = backTiffImagePath; model1.JointHolders = JointHolders; model1.ClientAccountDtls = ClientAccountDtls; model1.DOCType = DOCType;
                    model1.PresentingBank = PresentingBank; model1.BatchNo = (int)batchno;
                    IWSP.SelectBranchName(ProcessID, ref Branchname);
                    model1.BranchName = Branchname;

                    

                    model1.modf = true;
                    if (model1.DOCType == "C")
                        model1.DOCType = "Y";
                    else
                        model1.DOCType = "N";

                    if (model1.ClientAccountDtls != null)
                    {
                        if (model1.ClientAccountDtls.Split('|').ElementAt(1) == "S")
                        {
                            if (model1.ClientAccountDtls.Split('|').ElementAt(5).Trim() != "")
                            {
                                string MOP = db.MOPCodeMasters.Find(model1.ClientAccountDtls.Split('|').ElementAt(5)).Description;
                                model1.MOP = MOP != null ? MOP : "";
                            }
                            else
                            {
                                model1.MOP = "";
                            }
                            if (model1.ClientAccountDtls.Split('|').ElementAt(6).Trim() != "")
                            {
                                string AccountStatus = db.AccStatusCodeMasters.Find(model1.ClientAccountDtls.Split('|').ElementAt(6)).Description;
                                model1.AccountStatus = AccountStatus != null ? AccountStatus : "";
                            }
                            else
                            {
                                model1.AccountStatus = "";
                            }

                            if (model1.ClientAccountDtls.Split('|').ElementAt(12).Trim() != "")
                            {
                                string AccountOwnership = db.AccOwnershipCodeMasters.Find(model1.ClientAccountDtls.Split('|').ElementAt(12).ToString()).Description;
                                model1.AccountOwnership = AccountOwnership != null ? AccountOwnership : "";
                            }
                            else
                            {
                                model1.AccountOwnership = "";
                            }
                        }
                    }
                    IWSP.Dispose();
                    return View("IWVerificnabid", model1);
                }
                else
                {
                    IWSP.Dispose();
                    return RedirectToAction("IWVFSelection", new { id = 3, msg = "No Data Found!", vftype = Session["actiontype"].ToString(), reVf = Session["Vftype"].ToString() });
                }
            }

        }
        //-------------------------------------------------------------------//
        public ActionResult L1NewIWVerificAbid(int maintid = 0, string reason = null, int pendingcnt = 0)
        {
            IW_VerificationEntery Vobj = new IW_VerificationEntery();

            if (Session["domainid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            //------------------------------------------------------Checking user rights------------------------------------------------------////////////////////

            if ((bool)Session["QC"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = db.Users.Find(uid1);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            ViewBag.rtnmodule = "L1IWVerificAbid";
            int domainid = (int)Session["domainid"];
            int uid = (int)Session["uid"];
            DateTime processdt = Convert.ToDateTime(Session["processdate"].ToString());
            //var custid = db.Domains.Find(domainid).Customer.ID.ToString();
            //var SettingData = db.Appsettings.Where(a => ((a.Domain.ID == domainid) && (a.ClearingType == "IW")));

            bool blnDbtAccNo, blnDate, blnPyName, blnQCEnabled;

            blnDbtAccNo = (bool)Session["blnDbtAccNo"];
            blnDate = (bool)Session["blnDate"];
            blnPyName = (bool)Session["blnPyName"];
            blnQCEnabled = (bool)Session["QCEnabled"];
            ViewBag.DbtAccNo = blnDbtAccNo;
            ViewBag.ChqDate = blnDate;
            ViewBag.PyNme = blnPyName;

            if (blnQCEnabled == false)
            {
                return RedirectToAction("Index", "Home", new { id = 3 });
            }
            // var model1 = db.IWImageProcessings.Where(i => (i.VerificationStatus == "S")).FirstOrDefault();
            if (maintid != 0)
            {
                IWMainTransaction iwm = db.IWMainTransactions.Find(maintid);
                var model = (from m in db.IWMainTransactions
                             from im in db.IWImageProcessings
                             where m.Process.ID == im.Process.ID && m.File.ID == im.File.ID && m.FileSeqNo == m.FileSeqNo && m.ID == iwm.ID
                             && im.Process.ID == iwm.Process.ID && im.File.ID == iwm.File.ID && im.FileSeqNo == iwm.FileSeqNo

                             select new IW_VerificationEntery
                             {
                                 ID = im.ID,
                                 MID = m.ID,
                                 ProcessID = m.Process.ID,
                                 FrontTiffImagePath = im.FrontTiffImagePath,
                                 BackTiffImagePath = im.BackTiffImagePath,
                                 FrontGreyImagePath = im.FrontGreyImagePath,
                                 BackGreyImagePath = im.BackGreyImagePath,
                                 Amount = m.Amount,
                                 ChqDate = m.ChqDate,
                                 DbtAccNo = m.DbtAccNo,
                                 PayeeName = m.PayeeName,
                                 ChqNo = m.SerialNo,//Chq No
                                 SortCode = m.PayorBankRoutNo, //SortCode
                                 SAN = m.AccountNo, //SAN
                                 TrCode = m.TransCode,
                                 File_ID = m.File.ID,
                                 FileSeqNo = m.FileSeqNo,
                                 BatchNo = m.BatchNo,
                                 BranchName = m.Process.Branch.BranchCode + "-" + m.Process.Branch.BranchName,
                                 JointHolders = m.CBSJointHoldersName,
                                 ClientAccountDtls = m.CBSClientAccountDtls,
                                 DOCType = m.DocType,
                                 PresentingBank = m.PresentingBankRoutNo
                             }).FirstOrDefault(); ;
                ViewBag.pendingcount = pendingcnt;

                // var model = model1.FirstOrDefault();
                // model.ReasonCode = Convert.ToString(reason);
                if (model.ClientAccountDtls != null)
                {
                    model.modf = true;
                    if (model.ClientAccountDtls.Split('|').ElementAt(1) == "S")
                    {
                        if (model.ClientAccountDtls.Split('|').ElementAt(5).Trim() != "")
                        {
                            string MOP = db.MOPCodeMasters.Find(model.ClientAccountDtls.Split('|').ElementAt(5)).Description;
                            model.MOP = MOP != null ? MOP : "";
                        }
                        else
                        {
                            model.MOP = "";
                        }
                        if (model.ClientAccountDtls.Split('|').ElementAt(6).Trim() != "")
                        {
                            string AccountStatus = db.AccStatusCodeMasters.Find(model.ClientAccountDtls.Split('|').ElementAt(6)).Description;
                            model.AccountStatus = AccountStatus != null ? AccountStatus : "";
                        }
                        else
                        {
                            model.AccountStatus = "";
                        }

                        if (model.ClientAccountDtls.Split('|').ElementAt(12).Trim() != "")
                        {
                            string AccountOwnership = db.AccOwnershipCodeMasters.Find(model.ClientAccountDtls.Split('|').ElementAt(12).ToString()).Description;
                            model.AccountOwnership = AccountOwnership != null ? AccountOwnership : "";
                        }
                        else
                        {
                            model.AccountOwnership = "";
                        }
                    }
                }
                if (reason != null)
                    model.IWRemark = reason.ToString();
                model.IWDecision = "R";
                if (model.DOCType == "C")
                    model.DOCType = "Y";
                else
                    model.DOCType = "N";
                var accmodel = db.CommonSetting.Where(s => s.SettingName == "sign").Select(s => s.SettingValue).SingleOrDefault();
                if (accmodel != null)
                {
                    model.sign = accmodel.ToString();
                }
                return View("IWVerificnabid", model);
            }
            else
            {
                IWSPDataContext IWSP = new IWSPDataContext();
                //var model1 = (from im in db.IWImageProcessings
                //              from m in db.IWMainTransactions
                //              where
                //             (im.Process.Domain.ID == domainid && im.Process.ProcessDate == processdt
                //             && im.Process.ID == m.Process.ID && im.File.ID == m.File.ID && im.FileSeqNo == m.FileSeqNo &&
                //             (blnDbtAccNo ? im.DbtAccNoDEStatus == "Y" || im.DbtAccNoDEStatus == "E" : im.DbtAccNoDEStatus == "N" || im.DbtAccNoDEStatus == "R") &&
                //             (blnPyName ? im.PayeeNameDEStatus == "Y" || im.PayeeNameDEStatus == "E" : im.PayeeNameDEStatus == "N" || im.PayeeNameDEStatus == "R") &&
                //             (blnDate ? im.DateDEStatus == "Y" || im.DateDEStatus == "E" : im.DateDEStatus == "N" || im.DateDEStatus == "R")
                //              && m.CBSClientAccountDtls != null &&
                //                  //(blnQCEnabled ? im.VerificationStatus == "Y" || im.VerificationStatus == "R" : im.VerificationStatus == "N") &&
                //             (im.VerificationStatus == "N" || (im.VerificationStatus == "L" && im.VerificationBy == uid)))
                //              select new IW_VerificationEntery
                //              {
                //                  ID = im.ID,
                //                  MID = m.ID,
                //                  ProcessID = m.Process.ID,
                //                  FrontTiffImagePath = im.FrontTiffImagePath,
                //                  BackTiffImagePath = im.BackTiffImagePath,
                //                  FrontGreyImagePath = im.FrontGreyImagePath,
                //                  BackGreyImagePath = im.BackGreyImagePath,
                //                  Amount = m.Amount,
                //                  ChqDate = m.ChqDate,
                //                  DbtAccNo = m.DbtAccNo,
                //                  PayeeName = m.PayeeName,
                //                  ChqNo = m.SerialNo,//Chq No
                //                  SortCode = m.PayorBankRoutNo, //SortCode
                //                  SAN = m.AccountNo, //SAN
                //                  TrCode = m.TransCode,
                //                  File_ID = m.File.ID,
                //                  FileSeqNo = m.FileSeqNo,
                //                  BatchNo = m.BatchNo,
                //                  BranchName = m.Process.Branch.BranchCode + "-" + m.Process.Branch.BranchName,
                //                  JointHolders = m.CBSJointHoldersName,
                //                  ClientAccountDtls = m.CBSClientAccountDtls,
                //                  DOCType = m.DocType,
                //                  PresentingBank = m.PresentingBankRoutNo

                //              }).FirstOrDefault();




                //var model1 = model2.FirstOrDefault();
                int? ProcessID = null;
                int? FileID = null;
                int? FIleSeqno = null;
                int? imgID = null;
                int? mainID = null;
                double? amount = null;
                string chqdate = null;
                string dbtacno = null;
                string payeename = null;
                string chqno = null;
                string sortcode = null;
                string SAN = null;
                string trncode = null;
                string Branchname = null;
                string JointHolders = null;
                string ClientAccountDtls = null;
                string DOCType = null;
                string PresentingBank = null;
                string frontGreyImagePath = "";
                string frontTiffImagePath = "";
                string backTiffImagePath = "";
                int? batchno = null;

                //IWSP.SelectIWL1VF(processdt, domainid, uid, ref ProcessID, ref FileID, ref FIleSeqno, ref imgID, ref mainID, ref amount, ref chqdate, ref dbtacno, ref payeename, ref chqno
                //    , ref sortcode, ref SAN, ref trncode, ref frontGreyImagePath, ref frontTiffImagePath, ref backTiffImagePath, ref JointHolders, ref ClientAccountDtls, ref DOCType, ref PresentingBank, ref batchno);

                IWSP.TempSelectL1IWVF(processdt, domainid, uid, Session["clrngtype"].ToString(), ref ProcessID, ref FileID, ref FIleSeqno, ref imgID, ref mainID, ref amount, ref chqdate, ref dbtacno, ref payeename, ref chqno
                    , ref sortcode, ref SAN, ref trncode, ref frontGreyImagePath, ref frontTiffImagePath, ref backTiffImagePath, ref JointHolders, ref ClientAccountDtls, ref DOCType, ref PresentingBank, ref batchno);

                IW_VerificationEntery model1 = new IW_VerificationEntery();
                if (ProcessID != null)
                {
                    model1.ProcessID = (int)ProcessID; model1.File_ID = (int)FileID; model1.FileSeqNo = (int)FIleSeqno; model1.ID = (int)imgID; model1.MID = (int)mainID;
                    model1.Amount = (decimal)amount; model1.ChqDate = chqdate; model1.DbtAccNo = dbtacno; model1.PayeeName = payeename; model1.ChqNo = chqno;
                    model1.SortCode = sortcode; model1.SAN = SAN; model1.TrCode = trncode; model1.FrontGreyImagePath = frontGreyImagePath; model1.FrontTiffImagePath = frontTiffImagePath;
                    model1.BackTiffImagePath = backTiffImagePath; model1.JointHolders = JointHolders; model1.ClientAccountDtls = ClientAccountDtls; model1.DOCType = DOCType;
                    model1.PresentingBank = PresentingBank; model1.BatchNo = (int)batchno;
                    IWSP.SelectBranchName(ProcessID, ref Branchname);
                    model1.BranchName = Branchname;
                    // model1.RejectDescription = (model1.IWRemark != null ? db.ItemReturnReasons.Find(model1.IWRemark).DESCRIPTION : "");
                    //ViewBag.btnlock = true;
                    model1.modf = true;
                    if (model1.DOCType == "C")
                        model1.DOCType = "Y";
                    else
                        model1.DOCType = "N";

                    if (model1.ClientAccountDtls != null)
                    {
                        if (model1.ClientAccountDtls.Split('|').ElementAt(1) == "S")
                        {
                            if (model1.ClientAccountDtls.Split('|').ElementAt(5).Trim() != "")
                            {
                                string MOP = db.MOPCodeMasters.Find(model1.ClientAccountDtls.Split('|').ElementAt(5)).Description;
                                model1.MOP = MOP != null ? MOP : "";
                            }
                            else
                            {
                                model1.MOP = "";
                            }
                            if (model1.ClientAccountDtls.Split('|').ElementAt(6).Trim() != "")
                            {
                                string AccountStatus = db.AccStatusCodeMasters.Find(model1.ClientAccountDtls.Split('|').ElementAt(6)).Description;
                                model1.AccountStatus = AccountStatus != null ? AccountStatus : "";
                            }
                            else
                            {
                                model1.AccountStatus = "";
                            }

                            if (model1.ClientAccountDtls.Split('|').ElementAt(12).Trim() != "")
                            {
                                string AccountOwnership = db.AccOwnershipCodeMasters.Find(model1.ClientAccountDtls.Split('|').ElementAt(12).ToString()).Description;
                                model1.AccountOwnership = AccountOwnership != null ? AccountOwnership : "";
                            }
                            else
                            {
                                model1.AccountOwnership = "";
                            }
                        }
                    }

                    IWSP.Dispose();
                    return View("IWVerificnabid", model1);
                }
                else
                {
                    IWSP.Dispose();
                    return RedirectToAction("IWVFSelection", new { id = 3, msg = "No Data Found!", vftype = Session["actiontype"].ToString(),reVf=Session["Vftype"].ToString() });
                }
            }

        }
        //--------update ---------------
        [HttpPost]
        public ActionResult L1IWVerificAbid(IW_VerificationEntery vEnt)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            //----------------------------------Checking user rights--------------------//

            if ((bool)Session["QC"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = db.Users.Find(uid1);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            //if (QCE.InstrumentTyep == "C")
            //{
            int uid = (int)Session["uid"];

            IWSPDataContext IWSP = new IWSPDataContext();


            if (vEnt.IWDecision.ToUpper() == "R")
            {
                var Reject = db.ItemReturnReasons.Where(r => r.RETURN_REASON_CODE == vEnt.IWRemark).SingleOrDefault();
                if (Reject == null)
                {
                    vEnt.notfound = true;
                    vEnt.IWRemark = vEnt.IWRemark;
                    vEnt.IWDecision = "R";
                    IWSP.Dispose();
                    return View("IWVerificnabid", vEnt);
                }

            }

            else if (vEnt.IWDecision.ToUpper() == "A")
            {

                IWMainTransaction main = db.IWMainTransactions.Find(vEnt.MID);
                // ----------------------LOG--------------------------------

                if (vEnt.ChqNo != main.SerialNo)
                {
                    IWSP.IWTranscnLog(vEnt.ID, "MICR", "CHQNO", main.SerialNo, vEnt.ChqNo, uid);
                }
                if (vEnt.SortCode != main.PayorBankRoutNo)
                {
                    IWSP.IWTranscnLog(vEnt.ID, "MICR", "SORTCODE", main.PayorBankRoutNo, vEnt.SortCode, uid);
                }
                if (vEnt.SAN != main.AccountNo)
                {
                    IWSP.IWTranscnLog(vEnt.ID, "MICR", "SAN", main.AccountNo, vEnt.SAN, uid);
                }
                if (vEnt.TrCode != main.TransCode)
                {
                    IWSP.IWTranscnLog(vEnt.ID, "MICR", "TRANSCOD", main.TransCode, vEnt.SAN, uid);
                }
                if (vEnt.PayeeName != main.PayeeName)
                {
                    IWSP.IWTranscnLog(vEnt.ID, "Payeename", "Payeename", main.PayeeName, vEnt.SAN, uid);
                }
            }
            IWSP.IWActivityLog((int)vEnt.ID, (int)vEnt.MID, "L1 VERIFICATION", vEnt.IWDecision.ToUpper(), Session["LgnName"].ToString(), vEnt.IWRemark, null);
            //IWSP.UpdateIWL1VF(uid, vEnt.IWDecision, vEnt.IWRemark, vEnt.ID, vEnt.MID, "L1", vEnt.ChqNo, vEnt.SortCode, vEnt.SAN, vEnt.TrCode, (vEnt.PayeeName == null ? "N" : vEnt.PayeeName.Trim()), vEnt.Comment);
            IWSP.TempUpdateIWL1VF(uid, vEnt.IWDecision, vEnt.IWRemark, vEnt.ID, vEnt.MID, "L1", vEnt.ChqNo, vEnt.ChqDate, vEnt.SAN, vEnt.TrCode, (vEnt.PayeeName == null ? "N" : vEnt.PayeeName.Trim()), vEnt.Comment);
           
            IWSP.Dispose();
            return RedirectToAction("L1IWVerificAbid");
        }

        public ActionResult IWVerificAbid(int maintid = 0, string reason = null, int pendingcnt = 0)
        {
            IW_VerificationEntery Vobj = new IW_VerificationEntery();

            if (Session["domainid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }

            //----------------------------------Checking user rights--------------------//

            if ((bool)Session["VF"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = db.Users.Find(uid1);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            ViewBag.rtnmodule = "IWVerificAbid";
            int domainid = (int)Session["domainid"];
            int uid = (int)Session["uid"];
            DateTime processdt = Convert.ToDateTime(Session["processdate"].ToString());
            // var custid = db.Domains.Find(domainid).Customer.ID.ToString();
            //var SettingData = db.Appsettings.Where(a => ((a.Domain.ID == domainid) && (a.ClearingType == "IW")));
            bool blnDbtAccNo, blnDate, blnPyName, blnQCEnabled;
            blnDbtAccNo = (bool)Session["blnDbtAccNo"];
            blnDate = (bool)Session["blnDate"];
            blnPyName = (bool)Session["blnPyName"];
            blnQCEnabled = (bool)Session["QCEnabled"];

            ViewBag.DbtAccNo = blnDbtAccNo;
            ViewBag.ChqDate = blnDate;
            ViewBag.PyNme = blnPyName;
            ViewBag.backtomodule = "IWVerificAbid";
            ViewBag.L2 = true;
            if (maintid != 0)
            {
                IWMainTransaction iwm = db.IWMainTransactions.Find(maintid);
                var model = (from m in db.IWMainTransactions
                             from im in db.IWImageProcessings
                             where m.Process.ID == im.Process.ID && m.File.ID == im.File.ID && m.FileSeqNo == m.FileSeqNo && m.ID == iwm.ID
                             && im.Process.ID == iwm.Process.ID && im.File.ID == iwm.File.ID && im.FileSeqNo == iwm.FileSeqNo

                             select new IW_VerificationEntery
                                 {
                                     ID = im.ID,
                                     MID = m.ID,
                                     ProcessID = m.Process.ID,
                                     FrontTiffImagePath = im.FrontTiffImagePath,
                                     BackTiffImagePath = im.BackTiffImagePath,
                                     FrontGreyImagePath = im.FrontGreyImagePath,
                                     BackGreyImagePath = im.BackGreyImagePath,
                                     Amount = m.Amount,
                                     ChqDate = m.ChqDate,
                                     DbtAccNo = m.DbtAccNo,
                                     PayeeName = m.PayeeName,
                                     ChqNo = m.SerialNo,//Chq No
                                     SortCode = m.PayorBankRoutNo, //SortCode
                                     SAN = m.AccountNo, //SAN
                                     TrCode = m.TransCode,
                                     File_ID = m.File.ID,
                                     FileSeqNo = m.FileSeqNo,
                                     BatchNo = m.BatchNo,
                                     BranchName = m.Process.Branch.BranchCode + "-" + m.Process.Branch.BranchName,
                                     JointHolders = m.CBSJointHoldersName,
                                     ClientAccountDtls = m.CBSClientAccountDtls,
                                     L1Descision = im.VerificationStatus,
                                     IWRemark = m.ReturnCode,
                                     DOCType = m.DocType,
                                     PresentingBank = m.PresentingBankRoutNo
                                 }).FirstOrDefault();

                ViewBag.pendingcount = pendingcnt;
                if (model.DOCType == "C")
                    model.DOCType = "Y";
                else
                    model.DOCType = "N";
                model.RejectDescription = (model.IWRemark != null ? db.ItemReturnReasons.Find(model.IWRemark).DESCRIPTION : "");
                //var model = model1.FirstOrDefault();
                // model.ReasonCode = Convert.ToString(reason);
                if (model.ClientAccountDtls != null)
                {
                    if (model.ClientAccountDtls.Split('|').ElementAt(1) == "S")
                    {
                        if (model.ClientAccountDtls.Split('|').ElementAt(5).Trim() != "")
                        {
                            string MOP = db.MOPCodeMasters.Find(model.ClientAccountDtls.Split('|').ElementAt(5)).Description;
                            model.MOP = MOP != null ? MOP : "";
                        }
                        else
                        {
                            model.MOP = "";
                        }
                        if (model.ClientAccountDtls.Split('|').ElementAt(6).Trim() != "")
                        {
                            string AccountStatus = db.AccStatusCodeMasters.Find(model.ClientAccountDtls.Split('|').ElementAt(6)).Description;
                            model.AccountStatus = AccountStatus != null ? AccountStatus : "";
                        }
                        else
                        {
                            model.AccountStatus = "";
                        }

                        if (model.ClientAccountDtls.Split('|').ElementAt(12).Trim() != "")
                        {
                            string AccountOwnership = db.AccOwnershipCodeMasters.Find(model.ClientAccountDtls.Split('|').ElementAt(12).ToString()).Description;
                            model.AccountOwnership = AccountOwnership != null ? AccountOwnership : "";
                        }
                        else
                        {
                            model.AccountOwnership = "";
                        }
                    }
                }
                if (reason != null)
                    model.IWRemark = reason.ToString();
                model.IWDecision = "R";
                //var accmodel = db.CommonSetting.Where(s => s.SettingName == "sign").Select(s => s.SettingValue).SingleOrDefault();
                //if (accmodel != null)
                //{
                model.sign = Session["Sign"].ToString();
                //}
                return View("IWVerificnabid", model);
            }
            else
            {
                IWSPDataContext IWSP = new IWSPDataContext();

                //var model1 = (from im in db.IWImageProcessings
                //              from m in db.IWMainTransactions
                //              where
                //             (im.Process.ID==ProcessID && im.File.ID==FileID && im.FileSeqNo==FIleSeqno &&
                //              im.Process.ID == m.Process.ID && im.File.ID == m.File.ID && im.FileSeqNo == m.FileSeqNo )

                //              select new IW_VerificationEntery
                //              {
                //                  ID = im.ID,
                //                  MID = m.ID,
                //                  ProcessID = m.Process.ID,
                //                  FrontTiffImagePath = im.FrontTiffImagePath,
                //                  BackTiffImagePath = im.BackTiffImagePath,
                //                  FrontGreyImagePath = im.FrontGreyImagePath,
                //                  BackGreyImagePath = im.BackGreyImagePath,
                //                  Amount = m.Amount,
                //                  ChqDate = m.ChqDate,
                //                  DbtAccNo = m.DbtAccNo,
                //                  PayeeName = m.PayeeName,
                //                  ChqNo = m.SerialNo,//Chq No
                //                  SortCode = m.PayorBankRoutNo, //SortCode
                //                  SAN = m.AccountNo, //SAN
                //                  TrCode = m.TransCode,
                //                  File_ID = m.File.ID,
                //                  FileSeqNo = m.FileSeqNo,
                //                  BatchNo = m.BatchNo,
                //                  BranchName = m.Process.Branch.BranchCode + "-" + m.Process.Branch.BranchName,
                //                  JointHolders = m.CBSJointHoldersName,
                //                  ClientAccountDtls = m.CBSClientAccountDtls,
                //                  L1Descision = im.VerificationStatus,
                //                  IWRemark = m.ReturnCode,
                //                  DOCType = m.DocType,
                //                  PresentingBank = m.PresentingBankRoutNo

                //              }).FirstOrDefault();



                //var model1 = model2.FirstOrDefault();
                // 
                int? ProcessID = null;
                int? FileID = null;
                int? FIleSeqno = null;
                int? imgID = null;
                int? mainID = null;
                double? amount = null;
                string chqdate = null;
                string dbtacno = null;
                string payeename = null;
                string chqno = null;
                string sortcode = null;
                string SAN = null;
                string trncode = null;
                string Branchname = null;
                string JointHolders = null;
                string ClientAccountDtls = null;
                string DOCType = null;
                string L1Decision = null;
                string IWRemark = null;
                string PresentingBank = null;
                string frontGreyImagePath = "";
                string frontTiffImagePath = "";
                string backTiffImagePath = "";
                int? batchno = null;
                //int? rcount = null;

                //IWSP.SelectIWVF(processdt, domainid, uid, ref ProcessID, ref FileID, ref FIleSeqno, ref imgID, ref mainID, ref amount, ref chqdate, ref dbtacno, ref payeename, ref chqno
                //    , ref sortcode, ref SAN, ref trncode, ref frontGreyImagePath, ref frontTiffImagePath, ref backTiffImagePath, ref JointHolders, ref ClientAccountDtls, ref IWRemark, ref L1Decision, ref DOCType, ref PresentingBank, ref batchno);
               
                IWSP.TempSelectL2IWVF(processdt, domainid, uid,Session["clrngtype"].ToString(), ref ProcessID, ref FileID, ref FIleSeqno, ref imgID, ref mainID, ref amount, ref chqdate, ref dbtacno, ref payeename, ref chqno
                   , ref sortcode, ref SAN, ref trncode, ref frontGreyImagePath, ref frontTiffImagePath, ref backTiffImagePath, ref JointHolders, ref ClientAccountDtls, ref IWRemark, ref L1Decision, ref DOCType, ref PresentingBank, ref batchno);

                IW_VerificationEntery model1 = new IW_VerificationEntery();
                if (ProcessID != null)
                {
                    model1.ProcessID = (int)ProcessID; model1.File_ID = (int)FileID; model1.FileSeqNo = (int)FIleSeqno; model1.ID = (int)imgID; model1.MID = (int)mainID;
                    model1.Amount = (decimal)amount; model1.ChqDate = chqdate; model1.DbtAccNo = dbtacno; model1.PayeeName = payeename; model1.ChqNo = chqno;
                    model1.SortCode = sortcode; model1.SAN = SAN; model1.TrCode = trncode; model1.FrontGreyImagePath = frontGreyImagePath; model1.FrontTiffImagePath = frontTiffImagePath;
                    model1.BackTiffImagePath = backTiffImagePath; model1.JointHolders = JointHolders; model1.ClientAccountDtls = ClientAccountDtls; model1.DOCType = DOCType;
                    model1.L1Descision = L1Decision; model1.PresentingBank = PresentingBank; model1.Remark = IWRemark; model1.BatchNo = (int)batchno;
                    IWSP.SelectBranchName(ProcessID, ref Branchname);
                    model1.BranchName = Branchname;
                    model1.RejectDescription = (model1.IWRemark != null ? db.ItemReturnReasons.Find(model1.IWRemark).DESCRIPTION : "");

                    // ViewBag.btnlock = true;

                    if (model1.DOCType == "C")
                        model1.DOCType = "Y";
                    else
                        model1.DOCType = "N";
                    if (model1.ClientAccountDtls != null)
                    {
                        if (model1.ClientAccountDtls.Split('|').ElementAt(1) == "S")
                        {
                            if (model1.ClientAccountDtls.Split('|').ElementAt(5).Trim() != "")
                            {
                                string MOP = db.MOPCodeMasters.Find(model1.ClientAccountDtls.Split('|').ElementAt(5)).Description;
                                model1.MOP = MOP != null ? MOP : "";
                            }
                            else
                            {
                                model1.MOP = "";
                            }
                            if (model1.ClientAccountDtls.Split('|').ElementAt(6).Trim() != "")
                            {
                                string AccountStatus = db.AccStatusCodeMasters.Find(model1.ClientAccountDtls.Split('|').ElementAt(6)).Description;
                                model1.AccountStatus = AccountStatus != null ? AccountStatus : "";
                            }
                            else
                            {
                                model1.AccountStatus = "";
                            }

                            if (model1.ClientAccountDtls.Split('|').ElementAt(12).Trim() != "")
                            {
                                string AccountOwnership = db.AccOwnershipCodeMasters.Find(model1.ClientAccountDtls.Split('|').ElementAt(12).ToString()).Description;
                                model1.AccountOwnership = AccountOwnership != null ? AccountOwnership : "";
                            }
                            else
                            {
                                model1.AccountOwnership = "";
                            }
                        }
                    }

                    //var accmodel = db.CommonSetting.Where(s => s.SettingName == custid && s.AppName == "sign").Select(s => s.SettingValue).SingleOrDefault();
                    //if (accmodel != null)
                    //{
                    model1.sign = Session["Sign"].ToString();
                    //}



                    //ViewBag.pendingcount = (from im in db.IWImageProcessings
                    //                        where
                    //                        im.Process.Domain.ID == domainid && im.Process.ProcessDate == processdt &&
                    //                        (blnDbtAccNo ? im.DbtAccNoDEStatus == "Y" || im.DbtAccNoDEStatus == "E" : im.DbtAccNoDEStatus == "N" || im.DbtAccNoDEStatus == "R") &&
                    //                        (blnPyName ? im.PayeeNameDEStatus == "Y" || im.PayeeNameDEStatus == "E" : im.PayeeNameDEStatus == "N" || im.PayeeNameDEStatus == "R") &&
                    //                        (blnDate ? im.DateDEStatus == "Y" || im.DateDEStatus == "E" : im.DateDEStatus == "N" || im.DateDEStatus == "R") &&
                    //                        (blnQCEnabled ? im.VerificationStatus == "Y" || im.VerificationStatus == "R" : im.VerificationStatus == "N") &&
                    //                        (im.SignVerificationStatus == "N" || (im.SignVerificationStatus == "L" && im.SignverificationBy == uid)) && im.VerificationBy != uid
                    //                        select im.ID).Count();
                    IWSP.Dispose();
                    return View("IWVerificnabid", model1);
                }
                else
                {
                    IWSP.Dispose();
                    //return RedirectToAction("Index", "Home", new { id = 3 });
                    return RedirectToAction("IWVFSelection", new { id = 3, msg = "No Data Found!", vftype = Session["actiontype"].ToString(), reVf = Session["Vftype"].ToString() });
                }
            }

        }
        //--------update ---------------
        [HttpPost]
        public ActionResult IWVerificAbid(IW_VerificationEntery vEnt)
        {
            //if (QCE.InstrumentTyep == "C")
            //{
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            //----------------------------------Checking user rights--------------------//

            if ((bool)Session["VF"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = db.Users.Find(uid1);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            int uid = (int)Session["uid"];
            //IWImageProcessing im = db.IWImageProcessings.Find(vEnt.ID);
            //im.SignVerificationStatus = vEnt.IWDecision.ToUpper();
            //IWMainTransaction main = db.IWMainTransactions.Find(vEnt.MID);
            //main.SignVerificationStatus = vEnt.IWDecision.ToUpper();
            //main.SignverificationBy = db.Users.Find((int)Session["uid"]);
            //im.SignverificationBy = uid;

            //IWActivityLog log = new IWActivityLog();
            //log.IWMainTrID = main.ID;
            //log.IWImgTrID = im.ID;
            //log.LoginID = main.SignverificationBy.LoginID;
            //log.Timestamp = DateTime.Now;
            //log.LogLevel = "L2 VERIFICATION";
            // db.SaveChanges();
            /// }
            IWSPDataContext IWSP = new IWSPDataContext();
            if (vEnt.IWDecision.ToUpper() == "R")
            {
                var Reject = db.ItemReturnReasons.Where(r => r.RETURN_REASON_CODE == vEnt.IWRemark).SingleOrDefault();
                if (Reject == null)
                {
                    vEnt.notfound = true;
                    vEnt.IWRemark = vEnt.IWRemark;
                    vEnt.IWDecision = "R";
                    IWSP.Dispose();
                    return View("IWVerificnabid", vEnt);
                }


                //db.SaveChanges();
                //return RedirectToAction("QualityCheck");
            }
            //else if (vEnt.IWDecision.ToUpper() == "C")
            //{
            //IWImageProcessing im = db.IWImageProcessings.Find(vEnt.ID);
            //im.VerificationStatus = "N";
            //main.VerificationStatus = "N";
            //im.Remark = vEnt.IWRemark.ToString().ToUpper();
            //im.Comments = (vEnt.Comment != null ? vEnt.Comment.ToString().ToUpper() : null);

                //log.Activity = "CORRECTION";
            //log.RejectDesc = im.Remark;
            //log.Comments = im.Comments;

                ////im.VerificationBy = (int)Session["uid"];
            //for (int i = 0; i < im.Remark.Length; i++)
            //{
            //    if (im.Remark.Substring(i, 1) == "A")
            //    {
            //        im.DbtAccNoDEStatus = "C";
            //        main.CBSClientAccountDtls = null;
            //        main.CBSJointHoldersName = null;
            //    }

                //    if (im.Remark.Substring(i, 1) == "P")
            //    {
            //        im.PayeeNameDEStatus = "C";
            //    }
            //    if (im.Remark.Substring(i, 1) == "D")
            //    {
            //        im.DateDEStatus = "C";
            //    }
            //}
            // db.Entry(im).State = EntityState.Modified;

                //return RedirectToAction("QualityCheck");
            //}
            else if (vEnt.IWDecision.ToUpper() == "A")
            {

                IWMainTransaction main = db.IWMainTransactions.Find(vEnt.MID);
                // ----------------------LOG--------------------------------
                //bool flg = false;
                //IWTransactionLog IWTR = new IWTransactionLog();
                if (vEnt.ChqNo != main.SerialNo)
                {
                    IWSP.IWTranscnLog(vEnt.ID, "MICR", "CHQNO", main.SerialNo, vEnt.ChqNo, uid);
                    //flg = true;
                    //IWTR.ImageProcessing = im;
                    //IWTR.LogLevel = "MICR";
                    //IWTR.LogField = "Chqno";
                    //IWTR.PreviousValue = main.SerialNo;
                    //IWTR.NewValue = vEnt.ChqNo;
                    //db.IWTransactionLogs.Add(IWTR);
                }
                if (vEnt.SortCode != main.PayorBankRoutNo)
                {
                    IWSP.IWTranscnLog(vEnt.ID, "MICR", "SORTCODE", main.PayorBankRoutNo, vEnt.SortCode, uid);
                    //IWTR.ImageProcessing = im;
                    //IWTR.LogLevel = "MICR";
                    //IWTR.LogField = "Sortcode";
                    //IWTR.PreviousValue = main.PayorBankRoutNo;
                    //IWTR.NewValue = vEnt.SortCode;
                    //db.IWTransactionLogs.Add(IWTR);
                }
                if (vEnt.SAN != main.AccountNo)
                {
                    IWSP.IWTranscnLog(vEnt.ID, "MICR", "SAN", main.AccountNo, vEnt.SAN, uid);
                    //IWTR.ImageProcessing = im;
                    //IWTR.LogLevel = "MICR";
                    //IWTR.LogField = "SAN";
                    //IWTR.PreviousValue = main.AccountNo;
                    //IWTR.NewValue = vEnt.SAN;
                    //db.IWTransactionLogs.Add(IWTR);
                }
                if (vEnt.TrCode != main.TransCode)
                {
                    IWSP.IWTranscnLog(vEnt.ID, "MICR", "TRANSCOD", main.TransCode, vEnt.SAN, uid);
                    //IWTR.ImageProcessing = im;
                    //IWTR.LogLevel = "MICR";
                    //IWTR.LogField = "SAN";
                    //IWTR.PreviousValue = main.TransCode;
                    //IWTR.NewValue = vEnt.TrCode;
                    //db.IWTransactionLogs.Add(IWTR);
                }
                if (vEnt.PayeeName != main.PayeeName)
                {
                    IWSP.IWTranscnLog(vEnt.ID, "Payeename", "Payeename", main.PayeeName, vEnt.SAN, uid);
                    //IWTR.ImageProcessing = im;
                    //IWTR.LogLevel = "Payeename";
                    //IWTR.LogField = "Payeename";
                    //IWTR.PreviousValue = main.PayeeName;
                    //IWTR.NewValue = vEnt.PayeeName;
                    //db.IWTransactionLogs.Add(IWTR);
                }

                //-------------------------------------MICR Correction Update--------------------------------
                //if (flg == true)
                //{
                //    main.SerialNo = vEnt.ChqNo;
                //    main.PayorBankRoutNo = vEnt.SortCode;
                //    main.AccountNo = vEnt.SAN;
                //    main.TransCode = vEnt.TrCode;
                //    main.PayeeName = vEnt.PayeeName;
                //}
            }
           // IWSP.IWActivityLog((int)vEnt.ID, (int)vEnt.MID, "L2 VERIFICATION", vEnt.IWDecision.ToUpper(), Session["LgnName"].ToString(), vEnt.IWRemark, null);
            //IWSP.UpdateIWL1VF(uid, vEnt.IWDecision, vEnt.IWRemark, vEnt.ID, vEnt.MID, "L2", vEnt.ChqNo.Trim(), vEnt.SortCode.Trim(), (vEnt.SAN == null ? "000000" : vEnt.SAN.Trim()), vEnt.TrCode.Trim(), (vEnt.PayeeName == null ? "N" : vEnt.PayeeName.Trim()), vEnt.Comment);
            IWSP.TempUpdateIWL2VF(uid, vEnt.IWDecision, vEnt.IWRemark, vEnt.ID, vEnt.MID, "L2", vEnt.ChqNo.Trim(), vEnt.SortCode.Trim(), (vEnt.SAN == null ? "000000" : vEnt.SAN.Trim()), vEnt.TrCode.Trim(), (vEnt.PayeeName == null ? "N" : vEnt.PayeeName.Trim()), vEnt.Comment, Session["LgnName"].ToString());
            IWSP.Dispose();
            return RedirectToAction("IWVerificAbid");
        }

        public ActionResult L3IWVerificAbid(int maintid = 0, string reason = null, int pendingcnt = 0)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            //----------------------------------Checking user rights--------------------//

            if ((bool)Session["VF"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = db.Users.Find(uid1);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            IW_VerificationEntery Vobj = new IW_VerificationEntery();

            if (Session["domainid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            ViewBag.rtnmodule = "L3IWVerificAbid";

            int domainid = (int)Session["domainid"];
            int uid = (int)Session["uid"];
            DateTime processdt = Convert.ToDateTime(Session["processdate"].ToString());
            //var custid = db.Domains.Find(domainid).Customer.ID.ToString();

            //var SettingData = db.Appsettings.Where(a => ((a.Domain.ID == domainid) && (a.ClearingType == "IW")));
            if ((bool)Session["reverification"] == false)
                return RedirectToAction("Index", "Home", new { id = 3 });


            bool blnDbtAccNo, blnDate, blnPyName, blnQCEnabled;
            blnDbtAccNo = (bool)Session["blnDbtAccNo"];
            blnDate = (bool)Session["blnDate"];
            blnPyName = (bool)Session["blnPyName"];
            blnQCEnabled = (bool)Session["QCEnabled"];
            ViewBag.DbtAccNo = blnDbtAccNo;
            ViewBag.ChqDate = blnDate;
            ViewBag.PyNme = blnPyName;
            ViewBag.backtomodule = "L3IWVerificAbid";
            ViewBag.L2 = true;
            if (maintid != 0)
            {
                IWMainTransaction iwm = db.IWMainTransactions.Find(maintid);
                var model = (from m in db.IWMainTransactions
                             from im in db.IWImageProcessings
                             where m.Process.ID == im.Process.ID && m.File.ID == im.File.ID && m.FileSeqNo == m.FileSeqNo && m.ID == iwm.ID
                             && im.Process.ID == iwm.Process.ID && im.File.ID == iwm.File.ID && im.FileSeqNo == iwm.FileSeqNo

                             select new IW_VerificationEntery
                             {
                                 ID = im.ID,
                                 MID = m.ID,
                                 ProcessID = m.Process.ID,
                                 FrontTiffImagePath = im.FrontTiffImagePath,
                                 BackTiffImagePath = im.BackTiffImagePath,
                                 FrontGreyImagePath = im.FrontGreyImagePath,
                                 BackGreyImagePath = im.BackGreyImagePath,
                                 Amount = m.Amount,
                                 ChqDate = m.ChqDate,
                                 DbtAccNo = m.DbtAccNo,
                                 PayeeName = m.PayeeName,
                                 ChqNo = m.SerialNo,//Chq No
                                 SortCode = m.PayorBankRoutNo, //SortCode
                                 SAN = m.AccountNo, //SAN
                                 TrCode = m.TransCode,
                                 File_ID = m.File.ID,
                                 FileSeqNo = m.FileSeqNo,
                                 BatchNo = m.BatchNo,
                                 BranchName = m.Process.Branch.BranchCode + "-" + m.Process.Branch.BranchName,
                                 JointHolders = m.CBSJointHoldersName,
                                 ClientAccountDtls = m.CBSClientAccountDtls,
                                 L1Descision = im.VerificationStatus,
                                 L2Descision = im.SignVerificationStatus,
                                 IWRemark = m.ReturnCode,
                                 DOCType = m.DocType,
                                 PresentingBank = m.PresentingBankRoutNo
                             }).FirstOrDefault();
                ViewBag.pendingcount = pendingcnt;
                model.RejectDescription = (db.IWActivityLogs.Where(l => l.IWMainTrID == model.MID && l.LogLevel == "L1 VERIFICATION").FirstOrDefault() != null ? db.IWActivityLogs.Where(l => l.IWMainTrID == model.MID && l.LogLevel == "L1 VERIFICATION").FirstOrDefault().RejectDesc : "");
                model.L2RejectDescription = (model.IWRemark != null ? db.ItemReturnReasons.Find(model.IWRemark).DESCRIPTION : "");
                if (model.DOCType == "C")
                    model.DOCType = "Y";
                else
                    model.DOCType = "N";
                // model.ReasonCode = Convert.ToString(reason);
                if (model.ClientAccountDtls != null)
                {
                    if (model.ClientAccountDtls.Split('|').ElementAt(1) == "S")
                    {
                        if (model.ClientAccountDtls.Split('|').ElementAt(5).Trim() != "")
                        {
                            string MOP = db.MOPCodeMasters.Find(model.ClientAccountDtls.Split('|').ElementAt(5)).Description;
                            model.MOP = MOP != null ? MOP : "";
                        }
                        else
                        {
                            model.MOP = "";
                        }
                        if (model.ClientAccountDtls.Split('|').ElementAt(6).Trim() != "")
                        {
                            string AccountStatus = db.AccStatusCodeMasters.Find(model.ClientAccountDtls.Split('|').ElementAt(6)).Description;
                            model.AccountStatus = AccountStatus != null ? AccountStatus : "";
                        }
                        else
                        {
                            model.AccountStatus = "";
                        }

                        if (model.ClientAccountDtls.Split('|').ElementAt(12).Trim() != "")
                        {
                            string AccountOwnership = db.AccOwnershipCodeMasters.Find(model.ClientAccountDtls.Split('|').ElementAt(12).ToString()).Description;
                            model.AccountOwnership = AccountOwnership != null ? AccountOwnership : "";
                        }
                        else
                        {
                            model.AccountOwnership = "";
                        }
                    }
                }
                if (reason != null)
                    model.IWRemark = reason.ToString();
                model.IWDecision = "R";
                //var accmodel = db.CommonSetting.Where(s => s.AppName == "sign" && s.SettingName == custid).Select(s => s.SettingValue).SingleOrDefault();
                //if (accmodel != null)
                //{
                //    model.sign = accmodel.ToString();
                //}
                return View("IWVerificnabid", model);
            }
            else
            {
                IWSPDataContext IWSP = new IWSPDataContext();
                //var model2 = (from im in db.IWImageProcessings
                //              from m in db.IWMainTransactions
                //              where
                //             (im.Process.Domain.ID == domainid && im.Process.ProcessDate == processdt
                //             && im.Process.ID == m.Process.ID && im.File.ID == m.File.ID && im.FileSeqNo == m.FileSeqNo &&
                //             (blnDbtAccNo ? im.DbtAccNoDEStatus == "Y" || im.DbtAccNoDEStatus == "E" : im.DbtAccNoDEStatus == "N" || im.DbtAccNoDEStatus == "R") &&
                //             (blnPyName ? im.PayeeNameDEStatus == "Y" || im.PayeeNameDEStatus == "E" : im.PayeeNameDEStatus == "N" || im.PayeeNameDEStatus == "R") &&
                //             (blnDate ? im.DateDEStatus == "Y" || im.DateDEStatus == "E" : im.DateDEStatus == "N" || im.DateDEStatus == "R") &&
                //             ((im.VerificationStatus == "R" || im.VerificationStatus == "Y") && (im.SignVerificationStatus == "R" || im.SignVerificationStatus == "Y") ? im.SignVerificationStatus != im.VerificationStatus || im.SignVerificationStatus == "R" : im.SignVerificationStatus == "R") &&
                //             (im.ReverificationStatus == "N" || (im.ReverificationStatus == "L" && im.ReverificationBy == uid)) &&
                //             im.VerificationBy != uid && im.SignverificationBy != uid) //&& m.ReturnCode != "00" 
                //             && m.CBSClientAccountDtls != null 
                //              select new IW_VerificationEntery
                //              {
                //                  ID = im.ID,
                //                  MID = m.ID,
                //                  ProcessID = m.Process.ID,
                //                  FrontTiffImagePath = im.FrontTiffImagePath,
                //                  BackTiffImagePath = im.BackTiffImagePath,
                //                  FrontGreyImagePath = im.FrontGreyImagePath,
                //                  BackGreyImagePath = im.BackGreyImagePath,
                //                  Amount = m.Amount,
                //                  ChqDate = m.ChqDate,
                //                  DbtAccNo = m.DbtAccNo,
                //                  PayeeName = m.PayeeName,
                //                  ChqNo = m.SerialNo,//Chq No
                //                  SortCode = m.PayorBankRoutNo, //SortCode
                //                  SAN = m.AccountNo, //SAN
                //                  TrCode = m.TransCode,
                //                  File_ID = m.File.ID,
                //                  FileSeqNo = m.FileSeqNo,
                //                  BatchNo = m.BatchNo,
                //                  BranchName = m.Process.Branch.BranchCode + "-" + m.Process.Branch.BranchName,
                //                  JointHolders = m.CBSJointHoldersName,
                //                  ClientAccountDtls = m.CBSClientAccountDtls,
                //                  L1Descision = im.VerificationStatus,
                //                  L2Descision = im.SignVerificationStatus,
                //                  IWRemark = m.ReturnCode,
                //                  DOCType = m.DocType,
                //                  PresentingBank = m.PresentingBankRoutNo
                //              });


                int? ProcessID = null;
                int? FileID = null;
                int? FIleSeqno = null;
                int? imgID = null;
                int? mainID = null;
                double? amount = null;
                string chqdate = null;
                string dbtacno = null;
                string payeename = null;
                string chqno = null;
                string sortcode = null;
                string SAN = null;
                string trncode = null;
                string Branchname = null;
                string JointHolders = null;
                string ClientAccountDtls = null;
                string DOCType = null;
                string L1Decision = null;
                string L2Decision = null;
                string IWRemark = null;
                string PresentingBank = null;
                string frontGreyImagePath = "";
                string frontTiffImagePath = "";
                string backTiffImagePath = "";
                int? batchno = null;

                //IWSP.SelectL3IWVF(processdt, domainid, uid, ref ProcessID, ref FileID, ref FIleSeqno, ref imgID, ref mainID, ref amount, ref chqdate, ref dbtacno, ref payeename, ref chqno
                //    , ref sortcode, ref SAN, ref trncode, ref frontGreyImagePath, ref frontTiffImagePath, ref backTiffImagePath, ref JointHolders, ref ClientAccountDtls, ref IWRemark, ref L1Decision, ref DOCType, ref PresentingBank, ref batchno, ref L2Decision);
                IWSP.TempSelectL3IWVF(processdt, domainid, uid,Session["clrngtype"].ToString(), ref ProcessID, ref FileID, ref FIleSeqno, ref imgID, ref mainID, ref amount, ref chqdate, ref dbtacno, ref payeename, ref chqno
                   , ref sortcode, ref SAN, ref trncode, ref frontGreyImagePath, ref frontTiffImagePath, ref backTiffImagePath, ref JointHolders, ref ClientAccountDtls, ref IWRemark, ref L1Decision, ref DOCType, ref PresentingBank, ref batchno, ref L2Decision);


                IW_VerificationEntery model1 = new IW_VerificationEntery();
                if (ProcessID != null)
                {
                    model1.ProcessID = (int)ProcessID; model1.File_ID = (int)FileID; model1.FileSeqNo = (int)FIleSeqno; model1.ID = (int)imgID; model1.MID = (int)mainID;
                    model1.Amount = (decimal)amount; model1.ChqDate = chqdate; model1.DbtAccNo = dbtacno; model1.PayeeName = payeename; model1.ChqNo = chqno;
                    model1.SortCode = sortcode; model1.SAN = SAN; model1.TrCode = trncode; model1.FrontGreyImagePath = frontGreyImagePath; model1.FrontTiffImagePath = frontTiffImagePath;
                    model1.BackTiffImagePath = backTiffImagePath; model1.JointHolders = JointHolders; model1.ClientAccountDtls = ClientAccountDtls; model1.DOCType = DOCType;
                    model1.L1Descision = L1Decision; model1.PresentingBank = PresentingBank; model1.Remark = IWRemark; model1.BatchNo = (int)batchno; model1.L2Descision = L2Decision;
                    IWSP.SelectBranchName(ProcessID, ref Branchname);
                    model1.BranchName = Branchname;
                    if (model1.L1Descision == "R")
                    {
                        var IWact = db.IWActivityLogs.Where(l => l.IWMainTrID == model1.MID && l.LogLevel == "L1 VERIFICATION").FirstOrDefault();
                        if (IWact != null)
                            model1.RejectDescription = IWact.RejectDesc;
                        else
                            model1.RejectDescription = "";
                        //db.IWActivityLogs.Where(l => l.IWMainTrID == model1.MID && l.LogLevel == "L1 VERIFICATION").FirstOrDefault().RejectDesc : "");
                    }
                    //model1.RejectDescription = (model1.Remark != null ? db.ItemReturnReasons.Find(model1.Remark).DESCRIPTION : "");

                    if (model1.DOCType == "C")
                        model1.DOCType = "Y";
                    else
                        model1.DOCType = "N";

                    //
                    model1.L2RejectDescription = (model1.Remark != null ? db.ItemReturnReasons.Find(model1.Remark).DESCRIPTION : "");
                    if (model1.ClientAccountDtls != null)
                    {
                        if (model1.ClientAccountDtls.Split('|').ElementAt(1) == "S")
                        {
                            if (model1.ClientAccountDtls.Split('|').ElementAt(5).Trim() != "")
                            {
                                string MOP = db.MOPCodeMasters.Find(model1.ClientAccountDtls.Split('|').ElementAt(5)).Description;
                                model1.MOP = MOP != null ? MOP : "";
                            }
                            else
                            {
                                model1.MOP = "";
                            }
                            if (model1.ClientAccountDtls.Split('|').ElementAt(6).Trim() != "")
                            {
                                string AccountStatus = db.AccStatusCodeMasters.Find(model1.ClientAccountDtls.Split('|').ElementAt(6)).Description;
                                model1.AccountStatus = AccountStatus != null ? AccountStatus : "";
                            }
                            else
                            {
                                model1.AccountStatus = "";
                            }

                            if (model1.ClientAccountDtls.Split('|').ElementAt(12).Trim() != "")
                            {
                                string AccountOwnership = db.AccOwnershipCodeMasters.Find(model1.ClientAccountDtls.Split('|').ElementAt(12).ToString()).Description;
                                model1.AccountOwnership = AccountOwnership != null ? AccountOwnership : "";
                            }
                            else
                            {
                                model1.AccountOwnership = "";
                            }
                        }
                    }

                    //var accmodel = db.CommonSetting.Where(s => s.SettingName == custid && s.AppName == "sign").Select(s => s.SettingValue).SingleOrDefault();
                    //if (accmodel != null)
                    //{
                    model1.sign = Session["Sign"].ToString();
                    //}

                    //ViewBag.pendingcount = (from im in db.IWImageProcessings
                    //                        where
                    //                        im.Process.Domain.ID == domainid && im.Process.ProcessDate == processdt &&
                    //                        (blnDbtAccNo ? im.DbtAccNoDEStatus == "Y" || im.DbtAccNoDEStatus == "E" : im.DbtAccNoDEStatus == "N" || im.DbtAccNoDEStatus == "R") &&
                    //                         (blnPyName ? im.PayeeNameDEStatus == "Y" || im.PayeeNameDEStatus == "E" : im.PayeeNameDEStatus == "N" || im.PayeeNameDEStatus == "R") &&
                    //                         (blnDate ? im.DateDEStatus == "Y" || im.DateDEStatus == "E" : im.DateDEStatus == "N" || im.DateDEStatus == "R") &&
                    //                         ((im.VerificationStatus == "R" || im.VerificationStatus == "Y") && (im.SignVerificationStatus == "R" || im.SignVerificationStatus == "Y") ? im.SignVerificationStatus != im.VerificationStatus || im.SignVerificationStatus == "R" : im.SignVerificationStatus == "R") &&
                    //                         (im.ReverificationStatus == "N" || (im.ReverificationStatus == "L" && im.ReverificationBy == uid)) &&
                    //                         im.VerificationBy != uid && im.SignverificationBy != uid
                    //                        select im.ID).Count();

                    IWSP.Dispose();
                    return View("IWVerificnabid", model1);
                }
                else
                {
                    IWSP.Dispose();
                    //return RedirectToAction("Index", "Home", new { id = 3 });
                    return RedirectToAction("IWVFSelection", new { id = 3, msg = "No Data Found!", vftype = Session["actiontype"].ToString(), reVf = Session["Vftype"].ToString() });
                }
            }

        }
        [HttpPost]
        public ActionResult L3IWVerificAbid(IW_VerificationEntery vEnt)
        {
            //if (QCE.InstrumentTyep == "C")
            //{
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            //----------------------------------Checking user rights--------------------//

            if ((bool)Session["VF"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = db.Users.Find(uid1);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            int uid = (int)Session["uid"];
            //IWImageProcessing im = db.IWImageProcessings.Find(vEnt.ID);
            //im.SignVerificationStatus = vEnt.IWDecision.ToUpper();
            //IWMainTransaction main = db.IWMainTransactions.Find(vEnt.MID);
            //main.SignVerificationStatus = vEnt.IWDecision.ToUpper();
            //main.SignverificationBy = db.Users.Find((int)Session["uid"]);
            //im.SignverificationBy = uid;

            //IWActivityLog log = new IWActivityLog();
            //log.IWMainTrID = main.ID;
            //log.IWImgTrID = im.ID;
            //log.LoginID = main.SignverificationBy.LoginID;
            //log.Timestamp = DateTime.Now;
            //log.LogLevel = "L2 VERIFICATION";
            // db.SaveChanges();
            /// }
            IWSPDataContext IWSP = new IWSPDataContext();
            if (vEnt.IWDecision.ToUpper() == "R")
            {
                var Reject = db.ItemReturnReasons.Where(r => r.RETURN_REASON_CODE == vEnt.IWRemark).SingleOrDefault();
                if (Reject == null)
                {
                    vEnt.notfound = true;
                    vEnt.IWRemark = vEnt.IWRemark;
                    vEnt.IWDecision = "R";
                    IWSP.Dispose();
                    return View("IWVerificnabid", vEnt);
                }


                //db.SaveChanges();
                //return RedirectToAction("QualityCheck");
            }
            //else if (vEnt.IWDecision.ToUpper() == "C")
            //{
            //IWImageProcessing im = db.IWImageProcessings.Find(vEnt.ID);
            //im.VerificationStatus = "N";
            //main.VerificationStatus = "N";
            //im.Remark = vEnt.IWRemark.ToString().ToUpper();
            //im.Comments = (vEnt.Comment != null ? vEnt.Comment.ToString().ToUpper() : null);

                //log.Activity = "CORRECTION";
            //log.RejectDesc = im.Remark;
            //log.Comments = im.Comments;

                ////im.VerificationBy = (int)Session["uid"];
            //for (int i = 0; i < im.Remark.Length; i++)
            //{
            //    if (im.Remark.Substring(i, 1) == "A")
            //    {
            //        im.DbtAccNoDEStatus = "C";
            //        main.CBSClientAccountDtls = null;
            //        main.CBSJointHoldersName = null;
            //    }

                //    if (im.Remark.Substring(i, 1) == "P")
            //    {
            //        im.PayeeNameDEStatus = "C";
            //    }
            //    if (im.Remark.Substring(i, 1) == "D")
            //    {
            //        im.DateDEStatus = "C";
            //    }
            //}
            // db.Entry(im).State = EntityState.Modified;

                //return RedirectToAction("QualityCheck");
            //}
            else if (vEnt.IWDecision.ToUpper() == "A")
            {

                IWMainTransaction main = db.IWMainTransactions.Find(vEnt.MID);
                // ----------------------LOG--------------------------------
                //bool flg = false;
                //IWTransactionLog IWTR = new IWTransactionLog();
                if (vEnt.ChqNo != main.SerialNo)
                {
                    IWSP.IWTranscnLog(vEnt.ID, "MICR", "CHQNO", main.SerialNo, vEnt.ChqNo, uid);
                    //flg = true;
                    //IWTR.ImageProcessing = im;
                    //IWTR.LogLevel = "MICR";
                    //IWTR.LogField = "Chqno";
                    //IWTR.PreviousValue = main.SerialNo;
                    //IWTR.NewValue = vEnt.ChqNo;
                    //db.IWTransactionLogs.Add(IWTR);
                }
                if (vEnt.SortCode != main.PayorBankRoutNo)
                {
                    IWSP.IWTranscnLog(vEnt.ID, "MICR", "SORTCODE", main.PayorBankRoutNo, vEnt.SortCode, uid);
                    //IWTR.ImageProcessing = im;
                    //IWTR.LogLevel = "MICR";
                    //IWTR.LogField = "Sortcode";
                    //IWTR.PreviousValue = main.PayorBankRoutNo;
                    //IWTR.NewValue = vEnt.SortCode;
                    //db.IWTransactionLogs.Add(IWTR);
                }
                if (vEnt.SAN != main.AccountNo)
                {
                    IWSP.IWTranscnLog(vEnt.ID, "MICR", "SAN", main.AccountNo, vEnt.SAN, uid);
                    //IWTR.ImageProcessing = im;
                    //IWTR.LogLevel = "MICR";
                    //IWTR.LogField = "SAN";
                    //IWTR.PreviousValue = main.AccountNo;
                    //IWTR.NewValue = vEnt.SAN;
                    //db.IWTransactionLogs.Add(IWTR);
                }
                if (vEnt.TrCode != main.TransCode)
                {
                    IWSP.IWTranscnLog(vEnt.ID, "MICR", "TRANSCOD", main.TransCode, vEnt.SAN, uid);
                    //IWTR.ImageProcessing = im;
                    //IWTR.LogLevel = "MICR";
                    //IWTR.LogField = "SAN";
                    //IWTR.PreviousValue = main.TransCode;
                    //IWTR.NewValue = vEnt.TrCode;
                    //db.IWTransactionLogs.Add(IWTR);
                }
                if (vEnt.PayeeName != main.PayeeName)
                {
                    IWSP.IWTranscnLog(vEnt.ID, "Payeename", "Payeename", main.PayeeName, vEnt.SAN, uid);
                    //IWTR.ImageProcessing = im;
                    //IWTR.LogLevel = "Payeename";
                    //IWTR.LogField = "Payeename";
                    //IWTR.PreviousValue = main.PayeeName;
                    //IWTR.NewValue = vEnt.PayeeName;
                    //db.IWTransactionLogs.Add(IWTR);
                }

                //-------------------------------------MICR Correction Update--------------------------------
                //if (flg == true)
                //{
                //    main.SerialNo = vEnt.ChqNo;
                //    main.PayorBankRoutNo = vEnt.SortCode;
                //    main.AccountNo = vEnt.SAN;
                //    main.TransCode = vEnt.TrCode;
                //    main.PayeeName = vEnt.PayeeName;
                //}
            }
            IWSP.IWActivityLog((int)vEnt.ID, (int)vEnt.MID, "L3 VERIFICATION", vEnt.IWDecision.ToUpper(), Session["LgnName"].ToString(), vEnt.IWRemark, null);
            IWSP.TempUpdateIWL3VF(uid, vEnt.IWDecision, vEnt.IWRemark, vEnt.ID, vEnt.MID, "L3", vEnt.ChqNo, vEnt.SortCode, vEnt.SAN, vEnt.TrCode, (vEnt.PayeeName == null ? "N" : vEnt.PayeeName.Trim()), vEnt.Comment);
            IWSP.Dispose();
            return RedirectToAction("L3IWVerificAbid");
        }

        public ActionResult RejectReason(int? page, int mid = 0, string rtnmodule = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            ViewBag.rtnmodule = rtnmodule;
            int pageSize = 100;
            int pageNumber = (page ?? 1);
            var rjrs = (from r in db.ItemReturnReasons
                        select new RejectReason
                        {
                            Description = r.DESCRIPTION,
                            ReasonCodeS = r.RETURN_REASON_CODE
                        });
            ViewBag.mid = mid;

            return View(rjrs.ToList().ToPagedList(pageNumber, pageSize));
        }

        public ActionResult SelectReason(int main, string reasn, string rtnmodule)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            // IWMainTransaction iwm = db.IWMainTransactions.Find(main);

            return RedirectToAction(rtnmodule, "SignVerification", new { maintid = main, reason = reasn });
        }
        //--------------------------


        ////--------------------------------------------- Account Verf-------------------------------------
        public ActionResult DebtAcntIWVerificAbid(int maintid = 0, string reason = null, int pendingcnt = 0)
        {
            IW_VerificationEntery Vobj = new IW_VerificationEntery();

            if (Session["domainid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            //----------------------------------Checking user rights--------------------//

            if ((bool)Session["VF"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = db.Users.Find(uid1);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            ViewBag.rtnmodule = "L1IWVerificAbid";
            int domainid = (int)Session["domainid"];
            int uid = (int)Session["uid"];
            DateTime processdt = Convert.ToDateTime(Session["processdate"].ToString());
            var custid = db.Domains.Find(domainid).Customer.ID.ToString();
            var SettingData = db.Appsettings.Where(a => ((a.Domain.ID == domainid) && (a.ClearingType == "IW")));

            bool blnDbtAccNo, blnDate, blnPyName, blnQCEnabled;
            blnDbtAccNo = SettingData.First().CaptureChqDbtAccount;
            blnDate = SettingData.First().CaptureChqDate;
            blnPyName = SettingData.First().CaptureChqPayeeName;
            blnQCEnabled = SettingData.First().QCEnabled;
            ViewBag.DbtAccNo = blnDbtAccNo;
            ViewBag.ChqDate = blnDate;
            ViewBag.PyNme = blnPyName;

            if (blnQCEnabled == false)
            {
                return RedirectToAction("Index", "Home", new { id = 3 });
            }
            // var model1 = db.IWImageProcessings.Where(i => (i.VerificationStatus == "S")).FirstOrDefault();
            if (maintid != 0)
            {
                IWMainTransaction iwm = db.IWMainTransactions.Find(maintid);
                var model1 = (from m in db.IWMainTransactions
                              from im in db.IWImageProcessings
                              where m.Process.ID == im.Process.ID && m.File.ID == im.File.ID && m.FileSeqNo == m.FileSeqNo && m.ID == iwm.ID
                              && im.Process.ID == iwm.Process.ID && im.File.ID == iwm.File.ID && im.FileSeqNo == iwm.FileSeqNo

                              select new IW_VerificationEntery
                              {
                                  ID = im.ID,
                                  MID = m.ID,
                                  ProcessID = m.Process.ID,
                                  FrontTiffImagePath = im.FrontTiffImagePath,
                                  BackTiffImagePath = im.BackTiffImagePath,
                                  FrontGreyImagePath = im.FrontGreyImagePath,
                                  BackGreyImagePath = im.BackGreyImagePath,
                                  Amount = m.Amount,
                                  ChqDate = m.ChqDate,
                                  DbtAccNo = m.DbtAccNo,
                                  PayeeName = m.PayeeName,
                                  ChqNo = m.SerialNo,//Chq No
                                  SortCode = m.PayorBankRoutNo, //SortCode
                                  SAN = m.AccountNo, //SAN
                                  TrCode = m.TransCode,
                                  File_ID = m.File.ID,
                                  FileSeqNo = m.FileSeqNo,
                                  BatchNo = m.BatchNo,
                                  BranchName = m.Process.Branch.BranchCode + "-" + m.Process.Branch.BranchName,
                                  JointHolders = m.CBSJointHoldersName,
                                  ClientAccountDtls = m.CBSClientAccountDtls,
                                  DOCType = m.DocType,
                                  PresentingBank = m.PresentingBankRoutNo

                              });
                ViewBag.pendingcount = pendingcnt;

                var model = model1.FirstOrDefault();
                // model.ReasonCode = Convert.ToString(reason);
                if (model.ClientAccountDtls != null)
                {
                    if (model.ClientAccountDtls.Split('|').ElementAt(1) == "S")
                    {
                        if (model.ClientAccountDtls.Split('|').ElementAt(5).Trim() != "")
                        {
                            string MOP = db.MOPCodeMasters.Find(model.ClientAccountDtls.Split('|').ElementAt(5)).Description;
                            model.MOP = MOP != null ? MOP : "";
                        }
                        else
                        {
                            model.MOP = "";
                        }
                        if (model.ClientAccountDtls.Split('|').ElementAt(6).Trim() != "")
                        {
                            string AccountStatus = db.AccStatusCodeMasters.Find(model.ClientAccountDtls.Split('|').ElementAt(6)).Description;
                            model.AccountStatus = AccountStatus != null ? AccountStatus : "";
                        }
                        else
                        {
                            model.AccountStatus = "";
                        }

                        if (model.ClientAccountDtls.Split('|').ElementAt(12).Trim() != "")
                        {
                            string AccountOwnership = db.AccOwnershipCodeMasters.Find(model.ClientAccountDtls.Split('|').ElementAt(12).ToString()).Description;
                            model.AccountOwnership = AccountOwnership != null ? AccountOwnership : "";
                        }
                        else
                        {
                            model.AccountOwnership = "";
                        }
                    }
                }
                if (reason != null)
                    model.IWRemark = reason.ToString();
                model.IWDecision = "R";
                var accmodel = db.CommonSetting.Where(s => s.SettingName == "sign").Select(s => s.SettingValue).SingleOrDefault();
                if (accmodel != null)
                {
                    model.sign = accmodel.ToString();
                }
                return View("IWVerificnabid", model);
            }
            else
            {
                var model1 = (from im in db.IWImageProcessings
                              from m in db.IWMainTransactions
                              where
                             (im.Process.Domain.ID == domainid && im.Process.ProcessDate == processdt
                             && im.Process.ID == m.Process.ID && im.File.ID == m.File.ID && im.FileSeqNo == m.FileSeqNo &&
                                  (blnQCEnabled ? im.VerificationStatus == "R" && m.ReturnCode == "00" : im.VerificationStatus == im.VerificationStatus) &&
                              m.CBSClientAccountDtls != null &&
                                  (im.SignVerificationStatus == "N" || (im.SignVerificationStatus == "L" && im.SignverificationBy == uid)))
                              select new IW_VerificationEntery
                              {
                                  ID = im.ID,
                                  MID = m.ID,
                                  ProcessID = m.Process.ID,
                                  FrontTiffImagePath = im.FrontTiffImagePath,
                                  BackTiffImagePath = im.BackTiffImagePath,
                                  FrontGreyImagePath = im.FrontGreyImagePath,
                                  BackGreyImagePath = im.BackGreyImagePath,
                                  Amount = m.Amount,
                                  ChqDate = m.ChqDate,
                                  DbtAccNo = m.DbtAccNo,
                                  PayeeName = m.PayeeName,
                                  ChqNo = m.SerialNo,//Chq No
                                  SortCode = m.PayorBankRoutNo, //SortCode
                                  SAN = m.AccountNo, //SAN
                                  TrCode = m.TransCode,
                                  File_ID = m.File.ID,
                                  FileSeqNo = m.FileSeqNo,
                                  BatchNo = m.BatchNo,
                                  BranchName = m.Process.Branch.BranchCode + "-" + m.Process.Branch.BranchName,
                                  JointHolders = m.CBSJointHoldersName,
                                  ClientAccountDtls = m.CBSClientAccountDtls,
                                  DOCType = m.DocType,
                                  PresentingBank = m.PresentingBankRoutNo

                              }).FirstOrDefault();


                ViewBag.pendingcount = (from im in db.IWImageProcessings
                                        where
                                        im.Process.Domain.ID == domainid && im.Process.ProcessDate == processdt &&
                                        (blnDbtAccNo ? im.DbtAccNoDEStatus == "Y" || im.DbtAccNoDEStatus == "E" : im.DbtAccNoDEStatus == "N" || im.DbtAccNoDEStatus == "R") &&
                                        (blnPyName ? im.PayeeNameDEStatus == "Y" || im.PayeeNameDEStatus == "E" : im.PayeeNameDEStatus == "N" || im.PayeeNameDEStatus == "R") &&
                                        (blnDate ? im.DateDEStatus == "Y" || im.DateDEStatus == "E" : im.DateDEStatus == "N" || im.DateDEStatus == "R") &&
                                        (im.VerificationStatus == "N" || (im.VerificationStatus == "L" && im.VerificationBy == uid))
                                        select im.ID).Count();

                //var model1 = model2.FirstOrDefault();

                if (model1 != null)
                {
                    if (model1.DOCType == "C")
                        model1.DOCType = "Y";
                    else
                        model1.DOCType = "N";

                    if (model1.ClientAccountDtls != null)
                    {
                        if (model1.ClientAccountDtls.Split('|').ElementAt(1) == "S")
                        {
                            if (model1.ClientAccountDtls.Split('|').ElementAt(5).Trim() != "")
                            {
                                string MOP = db.MOPCodeMasters.Find(model1.ClientAccountDtls.Split('|').ElementAt(5)).Description;
                                model1.MOP = MOP != null ? MOP : "";
                            }
                            else
                            {
                                model1.MOP = "";
                            }
                            if (model1.ClientAccountDtls.Split('|').ElementAt(6).Trim() != "")
                            {
                                string AccountStatus = db.AccStatusCodeMasters.Find(model1.ClientAccountDtls.Split('|').ElementAt(6)).Description;
                                model1.AccountStatus = AccountStatus != null ? AccountStatus : "";
                            }
                            else
                            {
                                model1.AccountStatus = "";
                            }

                            if (model1.ClientAccountDtls.Split('|').ElementAt(12).Trim() != "")
                            {
                                string AccountOwnership = db.AccOwnershipCodeMasters.Find(model1.ClientAccountDtls.Split('|').ElementAt(12).ToString()).Description;
                                model1.AccountOwnership = AccountOwnership != null ? AccountOwnership : "";
                            }
                            else
                            {
                                model1.AccountOwnership = "";
                            }
                        }
                    }

                    //var accmodel = db.CommonSetting.Where(s => s.SettingName == custid && s.AppName == "sign").Select(s => s.SettingValue).SingleOrDefault();
                    //if (accmodel != null)
                    //{
                    //    model1.sign = accmodel.ToString();
                    //}

                    var result = db.IWImageProcessings.Find(model1.ID);
                    //model1.Decision = "C";
                    //model1.Remark = "A";
                    ViewBag.Acct = true;
                    result.SignVerificationStatus = "L";
                    result.SignverificationBy = uid;
                    db.Entry(result).State = EntityState.Modified;
                    db.SaveChanges();


                    return View("IWVerificnabid", model1);
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { id = 3 });
                }
            }

        }
        //--------update ---------------
        [HttpPost]
        public ActionResult DebtAcntIWVerificAbid(IW_VerificationEntery vEnt)
        {
            //if (QCE.InstrumentTyep == "C")
            //{
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            //----------------------------------Checking user rights--------------------//

            if ((bool)Session["VF"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = db.Users.Find(uid1);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            int uid = (int)Session["uid"];
            IWImageProcessing im = db.IWImageProcessings.Find(vEnt.ID);

            IWMainTransaction main = db.IWMainTransactions.Find(vEnt.MID);
            //main.VerificationStatus = vEnt.IWDecision.ToUpper();
            //main.VerificationBy = db.Users.Find((int)Session["uid"]);
            im.VerificationBy = uid;

            // db.SaveChanges();
            /// }

            if (vEnt.IWDecision.ToUpper() == "R")
            {
                var Reject = db.ItemReturnReasons.Where(r => r.RETURN_REASON_CODE == vEnt.IWRemark).SingleOrDefault();
                if (Reject != null)
                {
                    im.SignVerificationStatus = vEnt.IWDecision.ToUpper();
                    main.Return = true;
                    main.ReturnCode = vEnt.IWRemark;
                }
                else
                {
                    vEnt.notfound = true;
                    vEnt.IWRemark = vEnt.IWRemark;
                    vEnt.IWDecision = "R";
                    return View("DebtAcntIWVerificAbid", vEnt);
                }

                //db.SaveChanges();
                //return RedirectToAction("QualityCheck");
            }
            else if (vEnt.IWDecision.ToUpper() == "C")
            {
                //IWImageProcessing im = db.IWImageProcessings.Find(vEnt.ID);
                im.SignVerificationStatus = "N";
                main.SignVerificationStatus = "N";
                //im.DbtAccNoDEStatus = "C";
                im.Remark = vEnt.IWRemark.ToString().ToUpper();
                im.Comments = (vEnt.Comment != null ? vEnt.Comment.ToString().ToUpper() : null);
                //im.VerificationBy = (int)Session["uid"];
                for (int i = 0; i < im.Remark.Length; i++)
                {
                    if (im.Remark.Substring(i, 1) == "A")
                    {
                        im.DbtAccNoDEStatus = "C";
                        main.CBSClientAccountDtls = null;
                        main.CBSJointHoldersName = null;
                        main.Return = false;
                        main.ReturnCode = null;
                    }

                    //if (im.Remark.Substring(i, 1) == "P")
                    //{
                    //    im.PayeeNameDEStatus = "C";
                    //}
                    //if (im.Remark.Substring(i, 1) == "D")
                    //{
                    //    im.DateDEStatus = "C";
                    //}
                }
                // db.Entry(im).State = EntityState.Modified;

                //return RedirectToAction("QualityCheck");
            }
            else if (vEnt.IWDecision.ToUpper() == "A")
            {
                im.VerificationStatus = "Y";
                main.VerificationStatus = "Y";
                main.Return = false;
                main.ReturnCode = null;
            }
            db.Entry(im).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("DebtAcntIWVerificAbid");
        }
        //-----------------------------------------------------------------------------------------------




        [HttpPost]
        public bool InsertLog(IWTransactionLog logobj)
        {
            try
            {
                db.IWTransactionLogs.Add(logobj);

                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }

        }


        [HttpPost]
        public ActionResult RejectCollection()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            UserMaster usr = db.Users.Find(uid);

            ViewBag.rFlg = false;
            var rCdecollect = Request.Form.Get("hdval");
            var rDisCollect = Request.Form.Get("hdDisp");

            var id = Request.Form.Get("id");
            int IWmainTrId = Convert.ToInt32(id);

            var imgTblid = Request.Form.Get("imgid");
            int IWimgTrId = Convert.ToInt32(imgTblid);

            string[] rCdeArry;
            string[] DiscArry;


            if (rCdecollect != "")
            {
                rCdecollect = rCdecollect.Substring(0, rCdecollect.Length - 1);
                rDisCollect = rDisCollect.Substring(0, rDisCollect.Length - 1);

                rCdeArry = rCdecollect.Split(',');
                DiscArry = rDisCollect.Split(',');


                for (int j = 0; j < rCdeArry.Length; j++)
                {

                    IWRejectionLog rlog = new IWRejectionLog();

                    rlog.Description = DiscArry[j];
                    rlog.LogLevel = "SIGNVERIFICATION";
                    rlog.MainTransaction = db.IWMainTransactions.Find(IWmainTrId);
                    rlog.ReasonCode = db.ItemReturnReasons.Find(rCdeArry[j]);

                    Insert_Rlog(rlog);

                }

                var rModel = db.IWImageProcessings.Find(Convert.ToInt32(imgTblid));
                var mrModel = db.IWMainTransactions.Find(Convert.ToInt32(IWmainTrId));
                mrModel.SignVerificationStatus = "R";
                rModel.SignVerificationStatus = "R";
                mrModel.SignverificationBy = usr;
                db.SaveChanges();

                TempData["alertMessage"] = "Cheque is Rejected !!";
            }
            else
            {
                var rModel = db.IWImageProcessings.Find(Convert.ToInt32(imgTblid));
                rModel.SignVerificationStatus = "N";
                db.SaveChanges();
                TempData["alertMessage"] = "Cheque is Not Rejected  !!";
            }
            return RedirectToAction("IWVerification_Result");
        }

        [HttpPost]
        public bool Insert_Rlog(IWRejectionLog rlogobj)
        {
            try
            {
                db.IWRejectionLogs.Add(rlogobj);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }

        }

        public ActionResult ShowActivityLogs(int id, string module)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            ViewBag.backtomodule = module;
            var model = db.IWActivityLogs.Where(l => l.IWMainTrID == id).OrderBy(l => l.Timestamp).ToList();
            return View(model);
        }

    }

}

