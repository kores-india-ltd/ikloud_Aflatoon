using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using ikloud_Aflatoon.Models;
using System.Drawing;
using System.Data;

namespace ikloud_Aflatoon.Controllers
{
    public class OWCDKCheckerVerificationController : Controller
    {
        //
        private static Logger logger = LogManager.GetCurrentClassLogger();
        AflatoonEntities af = new AflatoonEntities();
        //OWProcDataContext OWpro = new OWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
        // GET: /OWCDKCheckerVerification/

        public ActionResult Index()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            if ((bool)Session["VF"] == false)
            {
                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            return View();
        }

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

        public ActionResult SelectionForBatch(int id = 0)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            if ((bool)Session["VF"] == false)
            {
                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            try
            {
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                var result = (from a in af.BatchMaster

                              where a.CustomerId == custid
                              && a.ScanningType == 11
                              && a.ProcessDate == xyz
                              && (a.Status == 4)
                              select new
                              {
                                  a.Id,
                                  a.BatchNo,
                                  a.ScanningNodeId,
                                  a.BranchCode
                                  
                              }).ToList();
                var objectlst = new List<BatchMaster>();
                BatchMaster crd;

                if (result.Count > 0)
                {
                    int index = 0;
                    int count = result.Count;

                    for (var i = 0; i < result.Count; i++)
                    {
                        crd = new BatchMaster
                        {
                            Id = result[i].Id,
                            BatchNo = result[i].BatchNo,
                            ScanningNodeId = result[i].ScanningNodeId,
                            BranchCode = result[i].BranchCode
                        };
                        objectlst.Add(crd);
                    }
                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                }
                else
                {
                    return RedirectToAction("IWIndex", "Home", new { id = 1 });// return RedirectToAction("DeSelection", new { id = 1 });
                }

                return View(objectlst);
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
            //return View();
        }

        public ActionResult Details(int id = 0)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            if ((bool)Session["VF"] == false)
            {
                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            try
            {
                //int uid = (int)Session["uid"];
                int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);
                Session["BatchID"] = id;

                var result = (from a in af.BatchMaster

                              where a.Id == id
                              select new
                              {
                                  a.Id,
                                  a.BatchNo,
                                  a.ScanningNodeId,
                                  a.ScanningType,
                                  a.BranchCode,
                                  
                              }).FirstOrDefault();

                var resultNew = (from a in af.L2Verification
                                  where a.ProcessingDate == xyz && a.CustomerId == custid && a.DomainId == domainId && a.BranchCode == result.BranchCode && 
                                  a.BatchNo == result.BatchNo && a.ScanningType == result.ScanningType && a.ScanningNodeId == result.ScanningNodeId
                                  orderby a.BatchNo,a.BatchSeqNo ascending

                                select new
                                {
                                    a.Id,
                                    a.RawDataId,
                                    a.ScanningType,
                                    a.ScanningNodeId,
                                    a.BatchNo,
                                    a.BatchSeqNo,
                                    a.InstrumentType,
                                    a.ChequeNoMICR,
                                    a.SortCodeMICR,
                                    a.SANMICR,
                                    a.TransCodeMICR,
                                    a.PayeeName,
                                    a.Status,

                                }).ToList();
                var objectlst = new List<L2Verification>();
                L2Verification crd;

                if (resultNew.Count > 0)
                {
                    int index = 0;
                    int count = resultNew.Count;

                    for (var i = 0; i < resultNew.Count; i++)
                    {
                        crd = new L2Verification
                        {
                            Id = resultNew[i].Id,
                            RawDataId = resultNew[i].RawDataId,
                            ScanningType = resultNew[i].ScanningType,
                            ScanningNodeId = resultNew[i].ScanningNodeId,
                            BatchNo = resultNew[i].BatchNo,
                            BatchSeqNo = resultNew[i].BatchSeqNo,
                            InstrumentType = resultNew[i].InstrumentType,
                            ChequeNoMICR = resultNew[i].ChequeNoMICR,
                            SortCodeMICR = resultNew[i].SortCodeMICR,
                            SANMICR = resultNew[i].SANMICR,
                            TransCodeMICR = resultNew[i].TransCodeMICR,
                            PayeeName = resultNew[i].PayeeName,
                            Status = resultNew[i].Status
                        };
                        objectlst.Add(crd);
                    }
                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                }
                else
                {
                    return RedirectToAction("IWIndex", "Home", new { id = 1 });// return RedirectToAction("DeSelection", new { id = 1 });
                }

                return View(objectlst);
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

        public ActionResult Verify(int id = 0)
        {
            try
            {
                if (Session["uid"] == null)
                {
                    return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
                }
                int uid = (int)Session["uid"];
                if ((bool)Session["VF"] == false)
                {
                    UserMaster usrm = af.UserMasters.Find(uid);
                    usrm.Active = false;
                    af.SaveChanges();
                    return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
                }

                //int uid = (int)Session["uid"];
                int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                var varMinAclen = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "ACFrom").SettingValue;
                var varMaxAclen = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "ACTo").SettingValue;
                var varMaxPayeelen = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "PayeeNameMaxLength").SettingValue;

                int intMinAclen = Convert.ToInt32(varMinAclen);
                int intMaxAclen = Convert.ToInt32(varMaxAclen);
                int intMaxPayeelen = Convert.ToInt32(varMaxPayeelen);


                ViewBag.MinAclen = intMinAclen;
                ViewBag.MaxAclen = intMaxAclen;
                ViewBag.MaxPayeelen = intMaxPayeelen;

                ViewBag.BankCode = Session["BankCode"].ToString();

                var OWIsDataEntryAllowedForAccountNo = "Y";
                var OWIsDataEntryAllowedForPayeeName = "Y";
                var OWIsDataEntryAllowedForDate = "Y";
                var OWIsDataEntryAllowedForAmount = "Y";

                ViewBag.OWIsDataEntryAllowedForAccountNo = OWIsDataEntryAllowedForAccountNo;
                ViewBag.OWIsDataEntryAllowedForPayeeName = OWIsDataEntryAllowedForPayeeName;
                ViewBag.OWIsDataEntryAllowedForDate = OWIsDataEntryAllowedForDate;
                ViewBag.OWIsDataEntryAllowedForAmount = OWIsDataEntryAllowedForAmount;

                //============= For AccountNo textbox ==================================
                OWIsDataEntryAllowedForAccountNo = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "OWIsDataEntryAllowedForAccountNo")?.SettingValue;
                if (OWIsDataEntryAllowedForAccountNo == null || OWIsDataEntryAllowedForAccountNo == "")
                {
                    ViewBag.OWIsDataEntryAllowedForAccountNo = "Y";
                    //ViewBag.OWVerDisbAccNo = "N";
                }
                else
                {
                    if (OWIsDataEntryAllowedForAccountNo == "Y")
                    {
                        ViewBag.OWIsDataEntryAllowedForAccountNo = "Y";
                        //ViewBag.OWVerDisbAccNo = "Y";
                    }
                    else
                    {
                        ViewBag.OWIsDataEntryAllowedForAccountNo = "N";
                        //ViewBag.OWVerDisbAccNo = "N";
                    }
                }

                //============= For PayeeName textbox ==================================
                OWIsDataEntryAllowedForPayeeName = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "OWIsDataEntryAllowedForPayeeName")?.SettingValue;
                if (OWIsDataEntryAllowedForPayeeName == null || OWIsDataEntryAllowedForPayeeName == "")
                {
                    ViewBag.OWIsDataEntryAllowedForPayeeName = "Y";
                    //ViewBag.OWVerDisbAccNo = "N";
                }
                else
                {
                    if (OWIsDataEntryAllowedForPayeeName == "Y")
                    {
                        ViewBag.OWIsDataEntryAllowedForPayeeName = "Y";
                        //ViewBag.OWVerDisbAccNo = "Y";
                    }
                    else
                    {
                        ViewBag.OWIsDataEntryAllowedForPayeeName = "N";
                        //ViewBag.OWVerDisbAccNo = "N";
                    }
                }

                //============= For Date textbox ==================================
                OWIsDataEntryAllowedForDate = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "OWIsDataEntryAllowedForDate")?.SettingValue;
                if (OWIsDataEntryAllowedForDate == null || OWIsDataEntryAllowedForDate == "")
                {
                    ViewBag.OWIsDataEntryAllowedForDate = "Y";
                }
                else
                {
                    if (OWIsDataEntryAllowedForDate == "Y")
                    {
                        ViewBag.OWIsDataEntryAllowedForDate = "Y";
                    }
                    else
                    {
                        ViewBag.OWIsDataEntryAllowedForDate = "N";
                    }
                }

                //============= For Amount textbox ==================================
                OWIsDataEntryAllowedForAmount = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "OWIsDataEntryAllowedForAmount")?.SettingValue;
                if (OWIsDataEntryAllowedForAmount == null || OWIsDataEntryAllowedForAmount == "")
                {
                    ViewBag.OWIsDataEntryAllowedForAmount = "Y";
                }
                else
                {
                    if (OWIsDataEntryAllowedForAmount == "Y")
                    {
                        ViewBag.OWIsDataEntryAllowedForAmount = "Y";
                    }
                    else
                    {
                        ViewBag.OWIsDataEntryAllowedForAmount = "N";
                    }
                }

                //============== Setting Session variable for all text box =================
                Session["OWIsDataEntryAllowedForAccountNo"] = ViewBag.OWIsDataEntryAllowedForAccountNo;
                Session["OWIsDataEntryAllowedForPayeeName"] = ViewBag.OWIsDataEntryAllowedForPayeeName;
                Session["OWIsDataEntryAllowedForDate"] = ViewBag.OWIsDataEntryAllowedForDate;
                Session["OWIsDataEntryAllowedForAmount"] = ViewBag.OWIsDataEntryAllowedForAmount;
                //==========================================================================================

                ViewBag.OWVerDisbAccNo = "N";
                ViewBag.OWCdkDisbAccNo = "N";
                ViewBag.OWCdkDisbPayeeName = "N";

                var owVerificationDisableAccountNo = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "OWVerificationDisableAccountNo")?.SettingValue;
                var owCdkDisableAccountNo = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "OWCDKDisableAccountNo")?.SettingValue;
                var owCdkDisablePayeeName = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "OWCDKDisablePayeeName")?.SettingValue;

                if (owVerificationDisableAccountNo == null || owVerificationDisableAccountNo == "")
                {
                    ViewBag.OWVerDisbAccNo = "Y";
                }
                else
                {
                    if (owVerificationDisableAccountNo == "N")
                    {
                        ViewBag.OWVerDisbAccNo = "N";
                    }
                    else
                    {
                        ViewBag.OWVerDisbAccNo = "Y";
                    }
                }

                if (owCdkDisableAccountNo == null || owCdkDisableAccountNo == "")
                {
                    ViewBag.OWCdkDisbAccNo = "Y";
                }
                else
                {
                    if (owCdkDisableAccountNo == "N")
                    {
                        ViewBag.OWCdkDisbAccNo = "N";
                    }
                    else
                    {
                        ViewBag.OWCdkDisbAccNo = "Y";
                    }
                }

                if (owCdkDisablePayeeName == null || owCdkDisablePayeeName == "")
                {
                    ViewBag.OWCdkDisbPayeeName = "Y";
                }
                else
                {
                    if (owCdkDisablePayeeName == "N")
                    {
                        ViewBag.OWCdkDisbPayeeName = "N";
                    }
                    else
                    {
                        ViewBag.OWCdkDisbPayeeName = "Y";
                    }
                }

                string VFType = "";
                VFType = "CDKL2";
                Session["VFType"] = VFType;
                Session["VFTypeID"] = 12;
                ViewBag.ScanningType = 12;

                var result = (from a in af.L2Verification

                              where a.Id == id
                              select new
                              {
                                  a.Id,
                                  a.RawDataId,
                                  a.BatchNo,
                                  a.BatchSeqNo,
                                  a.ScanningNodeId,
                                  a.ScanningType,
                                  a.BranchCode,
                                  a.InstrumentType,
                                  a.ClearingType,
                                  a.Status,
                                  a.FrontGreyImagePath,
                                  a.FrontTiffImagePath,
                                  a.BackGreyImagePath,
                                  a.BackTiffImagePath,
                                  a.CreditAccountNo,
                                  a.ProcessingDate,
                                  a.DomainId,
                                  a.CustomerId,
                                  a.RejectReason,
                                  a.L1UserId,
                                  a.L1RejectReason,
                                  a.PayeeName,
                                  a.CBSAccountInformation,
                                  a.CBSJointAccountInformation,
                                  a.L1VerificationStatus,
                                  a.RejectReasonDescription,
                                  a.L1Modified,
                                  a.FinalAmount,
                                  a.FinalDate,
                                  a.FinalAccountNo,
                                  a.ChequeNoFinal,
                                  a.SortCodeFinal,
                                  a.SANFinal,
                                  a.TransCodeFinal,
                                  a.Imported_SMBUdk,
                                  a.Imported_SMBChqDate,
                                  a.Imported_SMBPayeeName,
                                  a.Imported_SMBChqAmount,
                                  a.Imported_SMBChqAccNo,
                                  a.L2Modified,
                                  a.FrontUVImage,

                              }).FirstOrDefault();

                SMBVerificationView def;
                if(result != null)
                {
                    var cbs = "";
                    var cbsJoint = "";
                    if(result.CBSAccountInformation != null)
                    {
                        cbs = result.CBSAccountInformation.ToString();
                    }
                    if (result.CBSJointAccountInformation != null)
                    {
                        cbsJoint = result.CBSJointAccountInformation.ToString();
                    }


                    def = new SMBVerificationView
                    {
                        L2Id = Convert.ToInt32(result.Id),
                        BatchNo = Convert.ToInt32(result.BatchNo),
                        BatchSeqNo = Convert.ToInt32(result.BatchSeqNo),
                        captureRawId = Convert.ToInt64(result.RawDataId),
                        InstrumentType = result.InstrumentType.ToString(),
                        BranchCode = result.BranchCode.ToString(),
                        CustomerId = Convert.ToInt32(result.CustomerId),
                        DomainId = Convert.ToInt32(result.DomainId),
                        ScanningNodeId = Convert.ToInt32(result.ScanningNodeId),
                        FrontTiffImage = result.FrontTiffImagePath.ToString(),
                        FrontGreyImage = result.FrontGreyImagePath.ToString(),
                        BackTiffImage = result.BackTiffImagePath.ToString(),
                        BackGreyImage = result.BackGreyImagePath.ToString(),
                        ChqDate = result.Imported_SMBChqDate.ToString(),
                        ChqAmt = result.Imported_SMBChqAmount.ToString(),
                        ChqAcno = result.Imported_SMBChqAccNo.ToString(),
                        ChqPayeeName = result.Imported_SMBPayeeName.ToString(),
                        CBSAccountInformation = cbs,
                        CBSJointAccountInformation = cbsJoint,
                        FinalChqNo = result.ChequeNoFinal.ToString(),
                        FinalSortcode = result.SortCodeFinal.ToString(),
                        FinalSAN = result.SANFinal.ToString(),
                        FinalTransCode = result.TransCodeFinal.ToString(),
                        ScanningType = Convert.ToInt32(result.ScanningType),
                        FrontUVImage = result.FrontUVImage != null ? result.FrontUVImage.ToString() : "",
                    };

                    string ChqDateold = result.Imported_SMBChqDate.ToString();

                    if (ChqDateold.Length == 6)
                    {
                        ChqDateold = "20" + ChqDateold.Substring(4, 2) + ChqDateold.Substring(2, 2) + ChqDateold.Substring(0, 2);
                        ChqDateold = ChqDateold.Substring(6, 2) + ChqDateold.Substring(4, 2) + ChqDateold.Substring(2, 2);
                    }
                    else
                    {
                        ChqDateold = ChqDateold.Substring(8, 2) + ChqDateold.Substring(5, 2) + ChqDateold.Substring(2, 2);
                    }

                    def.ChqDate = ChqDateold;

                    //----------------------------------------Account field formatting-----------------------------------
                    string SMBChqAccNo = result.Imported_SMBChqAccNo.ToString();

                    if (SMBChqAccNo.Length > intMaxAclen)
                    {
                        SMBChqAccNo = SMBChqAccNo.Substring(1, intMaxAclen);
                    }
                    else if (SMBChqAccNo.Length < intMinAclen)
                    {
                        SMBChqAccNo = SMBChqAccNo.PadLeft(intMinAclen, '0');
                    }
                    def.ChqAcno = SMBChqAccNo;

                    //----------------------------------------Payeename field formatting-----------------------------------
                    string SMBChqPayeeName = result.Imported_SMBPayeeName.ToString().Trim();
                    if (SMBChqPayeeName.Length > intMaxPayeelen)
                    {
                        SMBChqPayeeName = SMBChqPayeeName.Substring(1, intMaxPayeelen);
                    }

                    def.ChqPayeeName = SMBChqPayeeName;
                    //-----------------------------------------------------------------------------------------------------

                    ViewBag.ChequeAmount = result.Imported_SMBChqAmount.ToString();
                    ViewBag.FrontGrey = result.FrontGreyImagePath == null ? "" : result.FrontGreyImagePath.ToString();
                    ViewBag.FrontTiff = result.FrontTiffImagePath == null ? "" : result.FrontTiffImagePath.ToString();
                    ViewBag.BackGrey = result.BackGreyImagePath == null ? "" : result.BackGreyImagePath.ToString();
                    ViewBag.BackTiff = result.BackTiffImagePath == null ? "" : result.BackTiffImagePath.ToString();
                    ViewBag.FrontUV = result.FrontUVImage == null ? "" : result.FrontUVImage.ToString();

                    Session["ScanningTypeId"] = Convert.ToInt32(result.ScanningType);

                    var rtnlist = (from i in af.ItemReturnReasons select i).ToList();// iwafl.ItemReturnReasons.Select(m).ToList();
                    ViewBag.rtnlist = rtnlist.Select(m => m.RETURN_REASON_CODE).ToList();
                    ViewBag.rtnlistDescrp = rtnlist.Select(m => m.DESCRIPTION).ToList();
                    //-------------------------------For Narration Accounts---------------------
                    ViewBag.narration = (from n in af.NarrationAccount where n.status == 1 select n.AccountNumber).ToList();
                    //-------------------------------For SchemCode---------------------
                    ViewBag.SchemCode = (from n in af.NreNroAccountList where n.IsActive == true select n.SchemCode).ToList();

                    @Session["glob"] = null;
                    ViewBag.cnt = true;

                    var L1RejectCode = result.L1RejectReason.ToString();
                    //var L1RejectDescp = (from i in af.ItemReturnReasons where i.RETURN_REASON_CODE == L1RejectCode select i.DESCRIPTION);
                    var L1RejectDescp = af.ItemReturnReasons.FirstOrDefault((p) => p.RETURN_REASON_CODE == L1RejectCode)?.DESCRIPTION;
                    if (L1RejectDescp == null || L1RejectDescp == "")
                    {
                        ViewBag.L1RejectDesc = "";
                    }
                    else
                    {
                        ViewBag.L1RejectDesc = L1RejectDescp.ToString();
                    }

                    var L2RejectCode = result.RejectReason.ToString();
                    //var L1RejectDescp = (from i in af.ItemReturnReasons where i.RETURN_REASON_CODE == L1RejectCode select i.DESCRIPTION);
                    var L2RejectDescp = af.ItemReturnReasons.FirstOrDefault((p) => p.RETURN_REASON_CODE == L2RejectCode)?.DESCRIPTION;
                    if (L2RejectDescp == null || L2RejectDescp == "")
                    {
                        ViewBag.L2RejectDesc = "";
                    }
                    else
                    {
                        ViewBag.L2RejectDesc = L2RejectDescp.ToString();
                    }

                    ViewBag.L1Modified = result.L1Modified.ToString();
                    ViewBag.L2Modified = result.L2Modified.ToString();
                    ViewBag.L1Status = Convert.ToInt16(result.L1VerificationStatus);
                    ViewBag.L2Status = Convert.ToInt16(result.Status);
                    Session["L1Status"] = Convert.ToInt16(result.L1VerificationStatus);
                    Session["L2Status"] = Convert.ToInt16(result.Status);

                    return View(def);
                }
                else
                    return RedirectToAction("IWIndex", "Home", new { id = 4 });
            }
            catch(Exception e)
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
        public ActionResult OWCDKL2(SMBVerificationView smbver, string btnSubmit, string msg, string Decision, string MarkP2f, string IWRemark, string rejectreasondescrpsn, string realModified, string modified)
        {

            //if (Session["uid"] == null)
            //{
            //    return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            //}
            //if ((bool)Session["VF"] == false)
            //{
            //    int uid1 = (int)Session["uid"];
            //    UserMaster usrm = af.UserMasters.Find(uid1);
            //    usrm.Active = false;
            //    af.SaveChanges();
            //    return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            //}

            //IWAmountTmpProcess jt;

            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            if ((bool)Session["VF"] == false)
            {
                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            //int uid = (int)Session["uid"];//That will be Session value.
            int ttcnt = 0;
            try
            {
                if (btnSubmit == "Close")
                {
                    string verification = Session["VFType"].ToString();
                    //OWpro.UnlockRecordsForOutwardVerification(smbver.L2Id, verification);
                    /// Int64 SlipRawaDataID = 0;
                    /// 
                    SqlCommand com = new SqlCommand("UnlockRecordsForOutwardVerification", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@id", smbver.L2Id);
                    com.Parameters.AddWithValue("@module", verification);

                    con.Open();
                    int i = com.ExecuteNonQuery();
                    con.Close();

                    Session["glob"] = true;
                    return RedirectToAction("IWIndex", "Home", new { id = 3 });
                }
                double camt = Convert.ToDouble(Request.Form["ChqAmt"]);

                string cdate = Convert.ToString(Request.Form["ChqDate"]);
                string cdatenew = "";
                if (Session["OWIsDataEntryAllowedForDate"].ToString().ToUpper() == "Y")
                {
                    cdatenew = "20" + cdate.Substring(4, 2) + "-" +
                    cdate.Substring(2, 2) + "-" +
                    cdate.Substring(0, 2);
                }
                else
                {

                }


                //DateTime dt = DateTime.ParseExact(cdatenew, "yyyy-MM-dd", null);

                string cacno = Convert.ToString(Request.Form["ChqAcno"]);
                string cpayee = Convert.ToString(Request.Form["ChqPayeeName"]);
                string cfinalchqno = Convert.ToString(Request.Form["FinalChqNo"]);
                string cfianlsortcode = Convert.ToString(Request.Form["FinalSortcode"]);
                string cfinalsan = Convert.ToString(Request.Form["FinalSAN"]);
                string cfialtrcd = Convert.ToString(Request.Form["FinalTransCode"]);
                //string cDraweeName = Convert.ToString(Request.Form["DraweeName"]);

                int cNRESourceOfFundId = 0, cNROSourceOfFundId = 0;

                if (IWRemark == "")
                {
                    IWRemark = "0";
                }

                bool p2f = Convert.ToBoolean(MarkP2f);
                bool ignoreiqa;
                string doctype = "";

                if (p2f)
                {
                    ignoreiqa = true;
                    doctype = "C";
                }
                else
                {
                    ignoreiqa = false;
                    doctype = "B";
                }

                string modaction = "";
                string modactionL1 = "";
                bool Modified = false;
                string verification1 = Session["VFType"].ToString();
                if (verification1 == "RNormalL1")
                {
                    Modified = false;
                    modified = "00000000";
                }
                else
                {
                    Modified = Convert.ToBoolean(realModified);
                    //modified = modified;
                }

                //logerror(smbver.captureRawId.ToString(), smbver.captureRawId.ToString() + " - > Actual values from RawDataId");
                //logerror(Decision, Decision.ToString() + " - > Actual values from texbox Decision");
                //logerror(modified, modified.ToString() + " - > Actual values from texbox modified");

                if (Decision.ToUpper() == "A")
                {
                    if (Modified == true)
                    {
                        modaction = "M";
                    }
                    else
                    {
                        modaction = "A";
                    }
                }
                else if (Decision.ToUpper() == "R")
                {
                    modaction = "R";
                }
                else if (Decision.ToUpper() == "H")
                {
                    modaction = "H";
                }

                //logerror(modaction, modaction.ToString() + " - > value modaction");

                string L1Status = Session["L1Status"].ToString();
                string L2Status = Session["L2Status"].ToString();

                //logerror(L1Status, L1Status.ToString() + " - > value L1Status from session");
                //logerror(L2Status, L2Status.ToString() + " - > value L2Status from session");

                if (L1Status == "2" || L1Status == "8")
                {
                    modactionL1 = "A";
                }
                else
                {
                    modactionL1 = "R";
                }

                //logerror(modactionL1, modactionL1.ToString() + " - > value modactionL1");

                //int verificationId = Convert.ToInt16(Session["VerificationId"].ToString());

                var branch = "";
                if (Session["BranchID"] != null)
                {
                    branch = Session["BranchID"].ToString();
                }
                //var branch = Session["BranchID"].ToString();
                var scanningTypeId = Session["ScanningTypeId"].ToString();

                if (btnSubmit == "Ok")
                {
                    string verification = Session["VFType"].ToString();
                    //logerror(verification, verification.ToString() + " - > IN Ok value verification");

                    
                    string modifiedAction = "";
                    if (L2Status == "8" || L2Status == "9")
                    {
                        if (modaction == "A" || modaction == "M")
                        {
                            modifiedAction = "2";
                        }
                        else if (modaction == "R")
                        {
                            modifiedAction = "3";
                        }

                        //logerror(modifiedAction, modifiedAction.ToString() + " - > IN Ok L2 verification In L2Staus 8 or 9 if clause value modifiedAction");
                    }
                    else
                    {
                        //logerror(scanningTypeId, scanningTypeId.ToString() + " - > IN Ok L2 verification In L2Staus 8 or 9 else clause value scanningTypeId");
                        if (scanningTypeId == "2" || scanningTypeId == "15")
                        {
                            if (modaction == "A" || modaction == "M")
                            {
                                modifiedAction = "2";
                            }
                            else if (modaction == "R")
                            {
                                modifiedAction = "3";
                            }

                            //logerror(modifiedAction, modifiedAction.ToString() + " - > IN Ok L2 verification In L2Staus 8 or 9 else clause and scanningType 2 or 15 if clause value modifiedAction");
                        }
                        else
                        {
                            //logerror(modifiedAction, modifiedAction.ToString() + " - > IN Ok L2 verification In L2Staus 8 or 9 else clause and scanningType 2 or 15 else clause value modifiedAction");
                            //logerror(modaction, modaction.ToString() + " - > IN Ok L2 verification In L2Staus 8 or 9 else clause and scanningType 2 or 15 else clause value modaction");
                            //logerror(modactionL1, modactionL1.ToString() + " - > IN Ok L2 verification In L2Staus 8 or 9 else clause and scanningType 2 or 15 else clause value modactionL1");

                            if (modaction == "A" && modactionL1 == "A")
                            {
                                modifiedAction = "2";
                            }
                            else if (modaction == "A" && modactionL1 == "R")
                            {
                                modifiedAction = "8";
                            }
                            else if (modaction == "M" && modactionL1 == "A")
                            {
                                modifiedAction = "8";
                            }
                            else if (modaction == "M" && modactionL1 == "R")
                            {
                                modifiedAction = "8";
                            }
                            else if (modaction == "R" && modactionL1 == "A")
                            {
                                modifiedAction = "9";
                            }
                            else if (modaction == "R" && modactionL1 == "R")
                            {
                                modifiedAction = "3";
                            }

                            //logerror(modifiedAction, modifiedAction.ToString() + " - > IN Ok L2 verification In L2Staus 8 or 9 else clause and scanningType 2 or 15 else clause value modifiedAction");
                        }

                    }

                    //logerror(smbver.L2Id.ToString(), smbver.L2Id.ToString() + " - > IN Ok L2 verification value Id");
                    //logerror(modifiedAction.ToString(), modifiedAction.ToString() + " - > IN Ok L2 verification value modifiedAction");
                    //logerror(modified.ToString(), modified.ToString() + " - > IN Ok L2 verification value modified");

                    //OWpro.UpdateSMBVerification(smbver.L2Id,
                    //        smbver.captureRawId,
                    //        uid,
                    //        smbver.InstrumentType,
                    //        camt,
                    //        //Convert.ToDateTime(cdatenew).ToString("yyyy-MM-dd"),
                    //        (cdatenew == "" ? DateTime.Now.ToString("yyyy-MM-dd") : cdatenew),
                    //        cfinalchqno,
                    //        cfianlsortcode,
                    //        cfinalsan,
                    //        cfialtrcd,
                    //        cacno,
                    //        cpayee,
                    //        2,//status 
                    //        Convert.ToByte(IWRemark), //reject reason
                    //        modifiedAction, //actiontaken
                    //        @Session["LoginID"].ToString(),
                    //        Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"),
                    //        smbver.CustomerId,
                    //        smbver.DomainId,
                    //        smbver.ScanningNodeId,
                    //        0,//chqamttotal 
                    //        0,//slipamttotal
                    //        0, //ChequeTotal
                    //        "",//userNarration, 
                    //        rejectreasondescrpsn,//rejectreasondescrpsn, 
                    //        Convert.ToString(Session["CtsSessionType"]),
                    //        "",//@CBSAccountInformation
                    //        "",//@CBSJointAccountInformation 
                    //        ignoreiqa,//@IgnoreIQA, 
                    //        doctype,//@DocType 
                    //        modified,//@Modified, 
                    //        null,
                    //        //,
                    //        "",
                    //        0,
                    //        0);//@strHoldReason

                    SqlCommand cmd = new SqlCommand("UpdateSMBVerification", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", smbver.L2Id);
                    cmd.Parameters.AddWithValue("@RawDataId", smbver.captureRawId);
                    cmd.Parameters.AddWithValue("@Uid", uid);
                    cmd.Parameters.AddWithValue("@InstrumentType", smbver.InstrumentType);
                    cmd.Parameters.AddWithValue("@FinalAmount", camt);
                    cmd.Parameters.AddWithValue("@FinalDate", (cdatenew == "" ? DateTime.Now.ToString("yyyy-MM-dd") : cdatenew));
                    cmd.Parameters.AddWithValue("@FinalChqNo", cfinalchqno);
                    cmd.Parameters.AddWithValue("@FinalSortcode", cfianlsortcode);
                    cmd.Parameters.AddWithValue("@FinalSAN", cfinalsan);
                    cmd.Parameters.AddWithValue("@FinalTransCode", cfialtrcd);
                    cmd.Parameters.AddWithValue("@CreditAccountNo", cacno);
                    cmd.Parameters.AddWithValue("@PayeName", cpayee);
                    cmd.Parameters.AddWithValue("@status", 2);
                    cmd.Parameters.AddWithValue("@RejectReason", Convert.ToByte(IWRemark));
                    cmd.Parameters.AddWithValue("@ActionTaken", modifiedAction);
                    cmd.Parameters.AddWithValue("@LName", @Session["LoginID"].ToString());
                    cmd.Parameters.AddWithValue("@ProcessingDate", Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@CustomerId", smbver.CustomerId);
                    cmd.Parameters.AddWithValue("@DomainId", smbver.DomainId);
                    cmd.Parameters.AddWithValue("@ScanningNodeId", smbver.ScanningNodeId);
                    cmd.Parameters.AddWithValue("@ChequeAmtotal", 0);
                    cmd.Parameters.AddWithValue("@SlipAmount", 0);
                    cmd.Parameters.AddWithValue("@ChequeTotal", 0);
                    cmd.Parameters.AddWithValue("@UserNarration", "");
                    cmd.Parameters.AddWithValue("@RejectReasonDescription", rejectreasondescrpsn);
                    cmd.Parameters.AddWithValue("@CTSNONCTS", Convert.ToString(Session["CtsSessionType"]));
                    cmd.Parameters.AddWithValue("@CBSAccountInformation", "");
                    cmd.Parameters.AddWithValue("@CBSJointAccountInformation", "");
                    cmd.Parameters.AddWithValue("@IgnoreIQA", ignoreiqa);
                    cmd.Parameters.AddWithValue("@DocType", doctype);
                    cmd.Parameters.AddWithValue("@Modified", modified);
                    cmd.Parameters.AddWithValue("@strHoldReason", "");
                    cmd.Parameters.AddWithValue("@DraweeName", "");
                    cmd.Parameters.AddWithValue("@NRESourceOfFundId", 0);
                    cmd.Parameters.AddWithValue("@NROSourceOfFundId", 0);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    //logerror("Finished Update L2 query", "Finished Update L2 query" + " - > IN Ok L2 verification value Finished Update L2 query");
                    //return RedirectToAction("Index", "OWSmbVerification", new { id = Session["VFTypeID"] });
                    return RedirectToAction("Details", "OWCDKCheckerVerification", new { id = Convert.ToInt16(Session["BatchID"]) });
                }

                else if (btnSubmit == "Close")
                {
                    string verification = Session["VFType"].ToString();
                    //OWpro.UnlockRecordsForOutwardVerification(smbver.L2Id, verification);
                    /// Int64 SlipRawaDataID = 0;
                    /// 
                    SqlCommand com = new SqlCommand("UnlockRecordsForOutwardVerification", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@id", smbver.L2Id);
                    com.Parameters.AddWithValue("@module", verification);

                    con.Open();
                    int i = com.ExecuteNonQuery();
                    con.Close();

                    Session["glob"] = true;
                    return RedirectToAction("IWIndex", "Home", new { id = 3 });
                }


                @Session["glob"] = true;
                return RedirectToAction("IWIndex", "Home", new { id = 1 });
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                Session.Abandon();

                logerror(e.Message, e.InnerException.ToString());

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL2 HttpPost Index- " + innerExcp });
            }
            finally
            {
                con.Close();
            }

        }
        public ActionResult getTiffImage(string httpwebimgpath = null)
        {
            try
            {

                int custid = Convert.ToInt16(Session["CustomerID"]);
                var destpath = af.CustomerMasters.FirstOrDefault((p) => p.Id == custid);

                //Owsr.L1VerificationName = l1result.LoginID;
                string destroot = destpath.PhysicalPath;

                //logerror(httpwebimgpath, httpwebimgpath.ToString() + " - >");

                const char delimiter = '\\';
                string[] destrootarr = destroot.Split(delimiter);

                string foldrname = destrootarr[destrootarr.Length - 1];

                //logerror(foldrname, foldrname.ToString() + " - > Folder Name"); 

                string[] arrpath = httpwebimgpath.Split('/');
                string actualpath = "";

                int flgmtch = 0;
                foreach (var item in arrpath)
                {
                    if (item != "")
                    {
                        if (flgmtch == 1)
                        {

                            actualpath = actualpath + "\\" + item;
                        }

                        if (item == foldrname)
                        {
                            flgmtch = 1;
                        }
                    }

                }
                //logerror(destroot, destroot.ToString() + "-> Root Path");
                actualpath = destroot + "\\" + actualpath;
                // System.IO.File.AppendAllText("C:\\temp\\log1.txt", "actualpathOWL1:" + actualpath);
                //logerror(actualpath, actualpath.ToString());
                System.Drawing.Bitmap bmp = new Bitmap(actualpath);
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                stream.Position = 0;

                byte[] data = new byte[stream.Length];
                int lngth = (int)stream.Length;
                stream.Read(data, 0, lngth);
                stream.Close();

                string imageBase64Data = Convert.ToBase64String(data);
                Array.Clear(data, 0, data.Length);


                string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                ViewBag.ImageData = imageDataURL;
                //logerror(imageDataURL, imageDataURL.ToString());
                return PartialView("_getTiffImage");
            }
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "SMB L2 Get Tiff Image Index- " + innerExcp });
            }

        }

        public ActionResult CloseBatch(string Decision = null)
        {
            try
            {
                if (Session["uid"] == null)
                {
                    return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
                }
                int uid = (int)Session["uid"];
                if ((bool)Session["VF"] == false)
                {
                    UserMaster usrm = af.UserMasters.Find(uid);
                    usrm.Active = false;
                    af.SaveChanges();
                    return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
                }

                Int64 batchID = Convert.ToInt64(Session["BatchID"]);
                //int uid = (int)Session["uid"];
                int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
                int custid = Convert.ToInt16(Session["CustomerID"]);
                DateTime processingDate1 = Convert.ToDateTime(Session["processdate"].ToString());
                string procDate = processingDate1.ToString("yyyy-MM-dd");
                var xyz = Convert.ToDateTime(procDate);

                byte status = 0;
                byte L2Status = 0;
                if(Decision == "A")
                {
                    status = 50;
                    L2Status = 2;
                }
                else if(Decision == "R")
                {
                    status = 51;
                    L2Status = 3;
                }

                var result = (from a in af.BatchMaster

                              where a.Id == batchID
                              select new
                              {
                                  a.Id,
                                  a.BatchNo,
                                  a.ScanningNodeId,
                                  a.ScanningType,
                                  a.BranchCode,

                              }).FirstOrDefault();

                var resultNew = (from a in af.L2Verification
                                 where a.ProcessingDate == xyz && a.CustomerId == custid && a.DomainId == domainId && a.BranchCode == result.BranchCode &&
                                 a.BatchNo == result.BatchNo && a.ScanningType == result.ScanningType && a.ScanningNodeId == result.ScanningNodeId
                                 orderby a.BatchNo, a.BatchSeqNo ascending

                                 select new
                                 {
                                     a.Id,
                                     a.RawDataId,
                                     a.Status,

                                 }).ToList();

                if (resultNew.Count > 0)
                {
                    int count = resultNew.Count;

                    for (var i = 0; i < resultNew.Count; i++)
                    {
                        Int64 L2Id = resultNew[i].Id;
                        int l2Staus = Convert.ToInt16(resultNew[i].Status);
                        //================== Change L2 Status ===============================
                        if(l2Staus == 0)
                        {
                            var result111 = (from p in af.L2Verification
                                             where p.Id == L2Id
                                             select p).SingleOrDefault();

                            result111.Status = L2Status;
                            af.SaveChanges();
                        }
                        
                    }
                }


                //================== Change Batch Status ===============================
                var result1 = (from p in af.BatchMaster
                            where p.Id == batchID
                            select p).SingleOrDefault();
                
                result1.Status = status;
                af.SaveChanges();

                return RedirectToAction("SelectionForBatch", "OWCDKCheckerVerification", new { id = 0 });
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

    }
}
