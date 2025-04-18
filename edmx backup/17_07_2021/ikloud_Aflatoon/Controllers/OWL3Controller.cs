﻿using ikloud_Aflatoon.Infrastructure;
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
    public class OWL3Controller : Controller
    {
        //
        // GET: /OWL3/
        AflatoonEntities af = new AflatoonEntities();
        OWProcDataContext OWpro = new OWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
        public ActionResult OWL3(int id = 0)
        {

            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            if ((bool)Session["RVF"] == false)
            {

                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            try
            {

                SqlDataAdapter adp = new SqlDataAdapter("OWSelectL3", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@VFtype", SqlDbType.NVarChar).Value = "Normal";
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();
                //-------------Added on 17-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                //-------------Added on 18-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);
                //-------------Added on 12-09-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = Session["CtsSessionType"];

                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<L3VerificationModel>();
                L3VerificationModel def;

                if (ds.Tables[0].Rows.Count > 0)
                {

                    def = new L3VerificationModel
                    {
                        Id = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]),
                        BatchNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[1]),
                        BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[2]),
                        InstrumentType = ds.Tables[0].Rows[0].ItemArray[3].ToString(),
                        ClearingType = ds.Tables[0].Rows[0].ItemArray[4].ToString(),
                        SlipNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[5].ToString()),
                        SlipChequeCount = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[6].ToString()),
                        SlipAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[7]),
                        ChequeAmountTotal = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[8]),
                        Status = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[9]),
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[10].ToString(),
                        FrontGreyImagePath = ds.Tables[0].Rows[0].ItemArray[11].ToString(),
                        BackTiffImagePath = ds.Tables[0].Rows[0].ItemArray[12].ToString(),
                        ClientCode = ds.Tables[0].Rows[0].ItemArray[13].ToString(),
                        SlipRefNo = ds.Tables[0].Rows[0].ItemArray[14].ToString(),
                        CreditAccountNo = ds.Tables[0].Rows[0].ItemArray[15].ToString(),
                        BranchCode = ds.Tables[0].Rows[0].ItemArray[16].ToString(),
                        ProcessingDate = Convert.ToDateTime(ds.Tables[0].Rows[0].ItemArray[17].ToString()),
                        ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[18].ToString()),
                        RawDataId = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[19].ToString()),
                        DomainId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[21].ToString()),
                        CustomerId = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[22].ToString()),
                        ScanningType = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[23].ToString()),
                        L1VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[24].ToString()),
                        L1UserId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[25].ToString()),
                        L1RejectReason = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[26].ToString()),
                        PayeeName = ds.Tables[0].Rows[0].ItemArray[27].ToString(),
                        L2VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[28].ToString()),
                        L2UserId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[29].ToString()),
                        L2RejectReason = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[30].ToString()),
                        CBSAccountInformation = ds.Tables[0].Rows[0].ItemArray[31].ToString(),
                        CBSJointAccountInformation = ds.Tables[0].Rows[0].ItemArray[32].ToString(),
                        UserNarration = ds.Tables[0].Rows[0].ItemArray[33].ToString(),
                        RejectReasonDescription = ds.Tables[0].Rows[0].ItemArray[34].ToString(),
                        Modified2 = ds.Tables[0].Rows[0].ItemArray[35].ToString(),
                        callby = "Slip",
                    };
                    objectlst.Add(def);
                    //------------------------END------------------------//
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        def = new L3VerificationModel
                        {
                            Id = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),
                            BatchNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[1]),
                            BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[2]),
                            InstrumentType = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                            ClearingType = ds.Tables[0].Rows[index].ItemArray[4].ToString(),
                            SlipNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[5].ToString()),
                            SlipChequeCount = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[6].ToString()),
                            SlipAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[7]),
                            ChequeAmountTotal = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[8]),
                            Status = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[9]),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[10].ToString(),
                            FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[11].ToString(),
                            BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[12].ToString(),
                            ClientCode = ds.Tables[0].Rows[index].ItemArray[13].ToString(),
                            SlipRefNo = ds.Tables[0].Rows[index].ItemArray[14].ToString(),
                            CreditAccountNo = ds.Tables[0].Rows[index].ItemArray[15].ToString(),
                            BranchCode = ds.Tables[0].Rows[index].ItemArray[16].ToString(),
                            ProcessingDate = Convert.ToDateTime(ds.Tables[0].Rows[index].ItemArray[17].ToString()),
                            ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[18].ToString()),
                            RawDataId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[19].ToString()),
                            DomainId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[21].ToString()),
                            CustomerId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[22].ToString()),
                            ScanningType = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[23].ToString()),
                            L1VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[24].ToString()),
                            L1UserId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[25].ToString()),
                            L1RejectReason = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[26].ToString()),
                            PayeeName = ds.Tables[0].Rows[index].ItemArray[27].ToString(),
                            L2VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[28].ToString()),
                            L2UserId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[29].ToString()),
                            L2RejectReason = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[30].ToString()),
                            CBSAccountInformation = ds.Tables[0].Rows[index].ItemArray[31].ToString(),
                            CBSJointAccountInformation = ds.Tables[0].Rows[index].ItemArray[32].ToString(),
                            UserNarration = ds.Tables[0].Rows[index].ItemArray[33].ToString(),
                            RejectReasonDescription = ds.Tables[0].Rows[index].ItemArray[34].ToString(),
                            Modified2 = ds.Tables[0].Rows[index].ItemArray[35].ToString(),
                            callby = "Slip",
                        };
                        //ViewBag.cnt = true;
                        objectlst.Add(def);
                        count = count - 1;
                        index = index + 1;
                    }
                    //var rtnlist = iwafl.ItemReturnReasons.Select(m => m.RETURN_REASON_CODE).ToList();
                    //ViewBag.rtnlist = rtnlist;

                    var rtnlist = (from i in af.ItemReturnReasons select i).ToList();// iwafl.ItemReturnReasons.Select(m).ToList();
                    ViewBag.rtnlist = rtnlist.Select(m => m.RETURN_REASON_CODE).ToList();
                    ViewBag.rtnlistDescrp = rtnlist.Select(m => m.DESCRIPTION).ToList();

                    //-------------------------------For Narration Accounts---------------------
                    ViewBag.narration = (from n in af.NarrationAccount where n.status == 1 select n.AccountNumber).ToList();
                    //-------------------------------For SchemCode---------------------
                    ViewBag.SchemCode = (from n in af.NreNroAccountList where n.IsActive == true select n.SchemCode).ToList();

                    @Session["glob"] = null;
                    ViewBag.cnt = true;
                    return View(objectlst);
                }
                else
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

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL3 HttpGet OWL3- " + innerExcp });
            }

        }
        [HttpPost]
        public ActionResult OWL3(List<string> lst, bool snd, string img = null, string btnClose = "default", List<Int64> idlst = null, double ChequeAmountTotal = 0)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["RVF"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            //IWAmountTmpProcess jt;
            int uid = (int)Session["uid"];//That will be Session value.
            int ttcnt = 0;
            try
            {
                if (lst != null)
                    ttcnt = lst.Count() / 42;

                int stu;
                int resncode = 0;
                string rejctdecrptn = null;
                string cbdclnts = "";
                string cbdJointdtls = "";
                byte rejct = 0;
                string modaction = "";
                string userNarration = "";
                string rejectreasondescrpsn = "";
                string Clearingtype = "";
                bool mark2pf = false;
                bool ignoreIQA = false;
                string DocType = "B";
                string instrumenttype = "";
                string finalModified = "";
                Int64 SlipID = 0;
                Int64 SlipRawaDataID = 0;
                DataSet ds = new DataSet();
                ObjectClass os = new ObjectClass();
                L3VerificationModel def;
                var objectlst = new List<L3VerificationModel>();
                Int64 id = 0;
                if (btnClose == "Close" && lst != null)
                    ttcnt = ttcnt + 1;
                else if (btnClose == "Close" && lst == null)
                    goto Close;
                // string IWDicision = Request.Form["IWDecision"].ToUpper();
                //if (snd == false)

                instrumenttype = lst[5].ToString();

                if (lst[5].ToString() == "S")
                {
                    if (ttcnt == 1)
                    {
                        //for (int i = 0; i < ttcnt; i++)
                        //{
                        if (lst[12].ToString() == "A")
                        {

                            objectlst = os.selectL3Cheques(con, uid, Session["LoginID"].ToString(), lst, Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), img, "Slip", false, Convert.ToInt16(Session["CustomerID"]), Convert.ToInt32(Session["DomainselectID"]), Session["CtsSessionType"].ToString());

                        }
                        else if (lst[12].ToString() == "F")
                        {

                            id = Convert.ToInt64(lst[0]);
                            OWpro.UpdateOWL3(id, Convert.ToInt64(lst[14]), uid, lst[5].ToString(), Convert.ToDouble(lst[2].ToString()), null, null, null, null, null, lst[1].ToString(),
                                lst[27].ToString(), Convert.ToInt16(lst[13]), null, "RF", @Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"),
                                Convert.ToInt16(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Convert.ToInt32(lst[9].ToString()), null, Convert.ToDouble(lst[2].ToString()), null, userNarration, null, null, null, null, null, null, null);

                            objectlst = os.selectL3Cheques(con, uid, Session["LoginID"].ToString(), lst, Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), img, "Slip", true, Convert.ToInt16(Session["CustomerID"]), Convert.ToInt32(Session["DomainselectID"]), Session["CtsSessionType"].ToString());

                        }
                        else if (lst[12].ToString() == "R")
                        {
                            if (lst[15] != null)
                                rejct = Convert.ToByte(lst[15].ToString());
                            if (rejct == 88)
                            {
                                if (lst[36] != null)
                                    rejectreasondescrpsn = lst[36].ToString();
                                else
                                    rejectreasondescrpsn = "Other Reason";
                            }
                            //---------------Added On 21/06/2017------------------
                            if (lst[39] != null)
                                SlipID = Convert.ToInt64(lst[39]);
                            if (lst[40] != null)
                                SlipRawaDataID = Convert.ToInt64(lst[40]);

                            if (lst[41] != null)
                                finalModified = lst[41].ToString();

                            OWpro.UpdateVerificationSlipOnly(Convert.ToInt64(lst[14]), uid, Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt32(lst[3].ToString()), Convert.ToInt32(lst[6].ToString()), Convert.ToInt32(lst[9].ToString()), lst[7].ToString(),
                                     Convert.ToDouble(lst[2].ToString()), lst[1].ToString(), lst[27].ToString(), lst[18].ToString(), lst[19].ToString(), "L3R", rejct, null, userNarration, rejectreasondescrpsn,
                                     Convert.ToInt32(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Session["LoginID"].ToString(), SlipID, SlipRawaDataID, null, 0, finalModified,null);
                            //----------------------------------------Commenetd On 17-01-2017------------
                            //id = Convert.ToInt64(lst[0]);
                            //OWpro.UpdateOWL3(id, Convert.ToInt64(lst[14]), uid, lst[5].ToString(), Convert.ToDouble(lst[2].ToString()), null, null, null, null, null, lst[1].ToString(),
                            //    null, Convert.ToInt16(lst[13]),rejct, lst[12].ToString(), @Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"),
                            //    Convert.ToInt16(lst[17].ToString()), Convert.ToInt16(lst[16].ToString()), Convert.ToInt16(lst[9].ToString()), null, Convert.ToDouble(lst[2].ToString()), null);
                            //----------------------------------------Commenetd On 17-01-2017------------END
                            objectlst = os.selectL3Cheques(con, uid, Session["LoginID"].ToString(), lst, Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), img, "Slip", true, Convert.ToInt16(Session["CustomerID"]), Convert.ToInt32(Session["DomainselectID"]), Session["CtsSessionType"].ToString());
                        }

                    }
                }
                else if (lst[5].ToString() == "C")
                {
                    string finaldate = "";
                    if (ttcnt == 1)
                    {
                        for (int i = 0; i < ttcnt; i++)
                        {
                            id = Convert.ToInt64(lst[0]);

                            if (lst[15] != null && lst[15].ToString() != "")
                                rejct = Convert.ToByte(lst[15].ToString());

                            if (lst[21] != null)
                            {
                                if (lst[21].ToString().Length != 10)
                                    finaldate = "20" + lst[21].ToString().Substring(4, 2) + "-" + lst[21].ToString().Substring(2, 2) + "-" + lst[21].ToString().Substring(0, 2);
                                else
                                    finaldate = lst[21].ToString();
                            }
                            //--------Modification Validation------------
                            if (lst[12].ToString() == "A")
                            {
                                if (Convert.ToBoolean(lst[30]) == true)
                                    modaction = "M";
                                else
                                    modaction = "A";
                            }
                            else if (lst[12].ToString() == "R")
                            {
                                modaction = "R";
                                if (rejct == 88)
                                {
                                    if (lst[36] != null)
                                        rejectreasondescrpsn = lst[36].ToString();
                                    else
                                        rejectreasondescrpsn = "Other Reason";
                                }
                            }

                            if (lst[35] != null)
                                userNarration = lst[35].ToString();

                            if (lst[37] != null)
                                Clearingtype = lst[37].ToString();

                            //------------------marking P2F--------------------//
                            if (lst[38] != null)
                            {
                                mark2pf = Convert.ToBoolean(lst[38]);
                                if (mark2pf == true)
                                {
                                    ignoreIQA = true;
                                    DocType = "C";
                                }
                                else
                                {
                                    ignoreIQA = false;
                                    DocType = "B";
                                }

                            }
                            else
                            {
                                ignoreIQA = false;
                                DocType = "B";
                            }
                            //---------------Added On 21/06/2017------------------
                            if (lst[39] != null)
                                SlipID = Convert.ToInt64(lst[39]);
                            if (lst[40] != null)
                                SlipRawaDataID = Convert.ToInt64(lst[40]);

                            if (lst[41] != null)
                                finalModified = lst[41].ToString();


                            OWpro.UpdateOWL3(id, Convert.ToInt64(lst[14]), uid, lst[5].ToString(), Convert.ToDouble(lst[20].ToString()), finaldate, lst[22].ToString(), lst[23].ToString(), lst[24].ToString(), lst[25].ToString(), lst[1].ToString(),
                                lst[27].ToString(), Convert.ToInt16(lst[13]), rejct, modaction, @Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"),
                                Convert.ToInt32(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Convert.ToInt32(lst[9].ToString()), null, Convert.ToDouble(lst[2].ToString()), null, userNarration,
                                rejectreasondescrpsn, Clearingtype, lst[18].ToString(), lst[19].ToString(), ignoreIQA, DocType, finalModified);

                            for (int k = 0; k < idlst.Count; k++)
                            {
                                if (idlst[k] == id)
                                    idlst.RemoveAt(k);
                            }
                        }
                        if (ChequeAmountTotal != Convert.ToDouble(lst[26]))
                        {
                            OWpro.UpdateChequeAmountTotalL1(Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt32(lst[3].ToString()), Convert.ToInt32(lst[6].ToString()), Convert.ToInt32(lst[9].ToString()), lst[7].ToString(), ChequeAmountTotal, "L3");
                        }
                        if (btnClose == "Close")
                            goto Close;
                    }
                    else
                    {
                        for (int i = 0; i < ttcnt - 1; i++)
                        {
                            id = Convert.ToInt64(lst[0]);

                            if (lst[15] != null && lst[15].ToString() != "")
                                rejct = Convert.ToByte(lst[15].ToString());

                            if (lst[21] != null)
                            {
                                if (lst[21].ToString().Length != 10)
                                    finaldate = "20" + lst[21].ToString().Substring(4, 2) + "-" + lst[21].ToString().Substring(2, 2) + "-" + lst[21].ToString().Substring(0, 2);
                                else
                                    finaldate = lst[21].ToString();
                            }
                            //--------Modification Validation------------
                            if (lst[12].ToString() == "A")
                            {
                                if (Convert.ToBoolean(lst[30]) == true)
                                    modaction = "M";
                                else
                                    modaction = "A";
                            }
                            else if (lst[12].ToString() == "R")
                            {
                                modaction = "R";
                                if (rejct == 88)
                                {
                                    if (lst[36] != null)
                                        rejectreasondescrpsn = lst[36].ToString();
                                    else
                                        rejectreasondescrpsn = "Other Reason";
                                }
                            }

                            if (lst[35] != null)
                                userNarration = lst[35].ToString();

                            if (lst[37] != null)
                                Clearingtype = lst[37].ToString();

                            //------------------marking P2F--------------------//
                            if (lst[38] != null)
                            {
                                mark2pf = Convert.ToBoolean(lst[38]);
                                if (mark2pf == true)
                                {
                                    ignoreIQA = true;
                                    DocType = "C";
                                }
                                else
                                {
                                    ignoreIQA = false;
                                    DocType = "B";
                                }

                            }
                            else
                            {
                                ignoreIQA = false;
                                DocType = "B";
                            }

                            if (lst[41] != null)
                                finalModified = lst[41].ToString();
                            //---------------Added On 21/06/2017------------------
                            if (lst[39] != null)
                                SlipID = Convert.ToInt64(lst[39]);
                            if (lst[40] != null)
                                SlipRawaDataID = Convert.ToInt64(lst[40]);

                            OWpro.UpdateOWL3(id, Convert.ToInt64(lst[14]), uid, lst[5].ToString(), Convert.ToDouble(lst[20].ToString()), finaldate, lst[22].ToString(), lst[23].ToString(), lst[24].ToString(), lst[25].ToString(), lst[1].ToString(),
                                lst[27].ToString(), Convert.ToInt16(lst[13]), rejct, modaction, @Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"),
                                Convert.ToInt32(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Convert.ToInt32(lst[9].ToString()), null, Convert.ToDouble(lst[2].ToString()), null, userNarration,
                                rejectreasondescrpsn, Clearingtype, lst[18].ToString(), lst[19].ToString(), ignoreIQA, DocType, finalModified);

                            for (int k = 0; k < idlst.Count; k++)
                            {
                                if (idlst[k] == id)
                                    idlst.RemoveAt(k);
                            }
                            lst.RemoveRange(0, 42);
                        }
                        if (btnClose == "Close")
                            goto Close;
                        if (ChequeAmountTotal != Convert.ToDouble(lst[26]))
                        {
                            OWpro.UpdateChequeAmountTotalL1(Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt32(lst[3].ToString()), Convert.ToInt32(lst[6].ToString()), Convert.ToInt32(lst[9].ToString()), lst[7].ToString(), ChequeAmountTotal, "L3");
                        }
                    }
                    objectlst = os.selectL3Cheques(con, uid, Session["LoginID"].ToString(), lst, Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), img, "Cheq", false, Convert.ToInt16(Session["CustomerID"]), Convert.ToInt32(Session["DomainselectID"]), Session["CtsSessionType"].ToString());
                }



            //---------------------------IF Close button called--------------------------------//
            Close:
                if (btnClose == "Close")
                {
                    @Session["glob"] = true;
                    for (int p = 0; p < idlst.Count; p++)
                    {
                        OWpro.OWUnlockRecords(idlst[p], "L3");
                    }

                    //if (instrumenttype == "C")
                    //    OWpro.OWUnlockRecords(SlipID, "L3");

                    return Json(false);
                }

                //-------------Calling next Records---------------

                if (objectlst != null || objectlst.Count != 0)
                {
                    ViewBag.cnt = true;
                    return Json(objectlst);
                }

                @Session["glob"] = true;
                return Json(false);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL3 HttpPost OWL3- " + innerExcp });
            }

        }
        public PartialViewResult getOWlogs(int id)
        {
            var model = af.ActivityLogs.Where(l => l.RawDataId == id).OrderBy(l => l.TimeStamp).ToList();
            return PartialView("_OWActivitylogs", model);
        }
        public JsonResult getL1logs(int id)
        {
            string decr = null;
            var IWact = af.ActivityLogs.Where(l => l.RawDataId == id && l.LogLevel == "L1 VERIFICATION" && l.ActionTaken.ToUpper() == "R").FirstOrDefault();
            if (IWact != null)
                decr = IWact.RejectDesc;
            else
                decr = "";
            return Json(decr, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult GetCBSDtls(string ac = null, string strcbsdls = null, string strJoinHldrs = null, string callby = null, string payeename = null)
        {
            cbstetails model = new cbstetails();
            try
            {
                if (strcbsdls != null)
                {

                    model.cbsdls = strcbsdls;
                    model.JoinHldrs = strJoinHldrs;
                    if (model.cbsdls != null)
                    {
                        //-------------------------For Creditcard-----------------------
                        //-------------------------For Creditcard-----------------------
                        if (ac.Length == 16 && ac != "9999999999999999")
                        {
                            if (Session["CreditCardValidationReq"].ToString() == "1")
                            {

                                if (model.cbsdls.Split('|').ElementAt(1) == "S")
                                {
                                    if (model.cbsdls.Length < 5)
                                    {
                                        model.cbsdls = "|S|CREDIT CARD-BRANCH COLLECTION A/C|CRC1| || |0|N|N|N|OAB|O";
                                        model.JoinHldrs = "|S|CREDIT CARD-BRANCH COLLECTION A/C|CRC1|VALID CARD || |0|N|N|N|OAB|O";
                                    }
                                }
                                else
                                {
                                    if (model.cbsdls.Length < 5)
                                    {
                                        model.cbsdls = "|S|CREDIT CARD-BRANCH COLLECTION A/C|CRC1| || |0|N|N|N|OAB|O";
                                        model.JoinHldrs = "|F|CREDIT CARD-BRANCH COLLECTION A/C|CRC1|INVALID CARD || |0|N|N|N|OAB|O";
                                    }
                                }

                            }
                        }
                        else if (ac.Length == 16 && ac == "9999999999999999")
                        {
                            model.cbsdls = "|F|Account does not exist";
                            model.JoinHldrs = "|F|Account does not exist";
                        }
                        //-------------------------For Creditcard-------END----------------
                        if (model.cbsdls.Split('|').ElementAt(1) == "S")
                        {
                            if (model.cbsdls.Split('|').ElementAt(5).Trim() != "")
                            {
                                string MOP = af.MOPCodeMasters.Find(model.cbsdls.Split('|').ElementAt(5)).Description;
                                model.MOP = MOP != null ? MOP : "";
                            }
                            else
                            {
                                model.MOP = "";
                            }
                            if (model.cbsdls.Split('|').ElementAt(6).Trim() != "")
                            {
                                string AccountStatus = af.AccStatusCodeMasters.Find(model.cbsdls.Split('|').ElementAt(6)).Description;
                                model.AccountStatus = AccountStatus != null ? AccountStatus : "";
                            }
                            else
                            {
                                model.AccountStatus = "";
                            }

                            if (model.cbsdls.Split('|').ElementAt(12).Trim() != "")
                            {
                                string AccountOwnership = af.AccOwnershipCodeMasters.Find(model.cbsdls.Split('|').ElementAt(12).ToString()).Description;
                                model.AccountOwnership = AccountOwnership != null ? AccountOwnership : "";
                            }
                            else
                            {
                                model.AccountOwnership = "";
                            }
                            List<string> ar = new List<string>();
                            ar.Add(model.cbsdls.Split('|').ElementAt(2).ToString());

                            if (model.JoinHldrs != null)//model.JoinHldrs.Split('|').Count() - 1 remove on 05/05/2017
                            {
                                for (int i = 3; i < model.JoinHldrs.Split('|').Count(); i++)
                                {
                                    if (model.JoinHldrs.Split('|').ElementAt(i).ToString() != "")
                                        ar.Add(model.JoinHldrs.Split('|').ElementAt(i).ToString());

                                }
                            }
                            model.PayeeName = ar;
                        }
                        else
                        {
                            cbstetails Tempcbdtls = new cbstetails();
                            Tempcbdtls.cbsdls = "|F|Account does not exist";
                            model = Tempcbdtls;
                        }
                    }
                    // model.callby = callby;
                }
                else if (ac != null)
                {

                    if (Session["GetAccountDetails "].ToString().ToUpper() == "L")
                    {
                        model = (from c in af.ACDetails
                                 where c.Ac == ac
                                 select new cbstetails
                                 {
                                     cbsdls = c.Cbsdtls,
                                     JoinHldrs = c.JoinHldrs
                                 }
                            ).SingleOrDefault();

                    }
                    else if (Session["GetAccountDetails "].ToString().ToUpper() == "C")
                    {
                        //---------For CBS Bank----------------

                        OWpro.OWGetCBSAccInfoWithOutUpdate(ac, ref strcbsdls, ref strJoinHldrs);
                        model.cbsdls = strcbsdls;
                        model.JoinHldrs = strJoinHldrs;
                        //---------------------------------
                    }

                    if (model != null && model.cbsdls != null)
                    {

                        //-------------------------For Creditcard-----------------------
                        //-------------------------For Creditcard-----------------------
                        if (ac.Length == 16 && ac != "9999999999999999")
                        {
                            if (Session["CreditCardValidationReq"].ToString() == "1")
                            {
                                if (model.cbsdls.Split('|').ElementAt(1) == "S")
                                {
                                    if (model.cbsdls.Length < 5)
                                    {
                                        model.cbsdls = "|S|CREDIT CARD-BRANCH COLLECTION A/C|CRC1| || |0|N|N|N|OAB|O";
                                        model.JoinHldrs = "|S|CREDIT CARD-BRANCH COLLECTION A/C|CRC1|VALID CARD || |0|N|N|N|OAB|O";
                                    }
                                }
                                else
                                {
                                    if (model.cbsdls.Length < 5)
                                    {
                                        model.cbsdls = "|S|CREDIT CARD-BRANCH COLLECTION A/C|CRC1| || |0|N|N|N|OAB|O";
                                        model.JoinHldrs = "|F|CREDIT CARD-BRANCH COLLECTION A/C|CRC1|INVALID CARD || |0|N|N|N|OAB|O";
                                    }
                                }

                            }
                        }
                        else if (ac.Length == 16 && ac == "9999999999999999")
                        {
                            model.cbsdls = "|F|Account does not exist";
                            model.JoinHldrs = "|F|Account does not exist";
                        }
                        //-------------------------For Creditcard-------END----------------
                        if (model.cbsdls.Split('|').ElementAt(1) == "S")
                        {
                            if (model.cbsdls.Split('|').ElementAt(5).Trim() != "")
                            {
                                string MOP = af.MOPCodeMasters.Find(model.cbsdls.Split('|').ElementAt(5)).Description;
                                model.MOP = MOP != null ? MOP : "";
                            }
                            else
                            {
                                model.MOP = "";
                            }
                            if (model.cbsdls.Split('|').ElementAt(6).Trim() != "")
                            {
                                string AccountStatus = af.AccStatusCodeMasters.Find(model.cbsdls.Split('|').ElementAt(6)).Description;
                                model.AccountStatus = AccountStatus != null ? AccountStatus : "";
                            }
                            else
                            {
                                model.AccountStatus = "";
                            }

                            if (model.cbsdls.Split('|').ElementAt(12).Trim() != "")
                            {
                                string AccountOwnership = af.AccOwnershipCodeMasters.Find(model.cbsdls.Split('|').ElementAt(12).ToString()).Description;
                                model.AccountOwnership = AccountOwnership != null ? AccountOwnership : "";
                            }
                            else
                            {
                                model.AccountOwnership = "";
                            }

                            List<string> ar = new List<string>();
                            ar.Add(model.cbsdls.Split('|').ElementAt(2).ToString());

                            if (model.JoinHldrs != null)//model.JoinHldrs.Split('|').Count() - 1 remove on 05/05/2017
                            {
                                for (int i = 3; i < model.JoinHldrs.Split('|').Count(); i++)
                                {
                                    if (model.JoinHldrs.Split('|').ElementAt(i).ToString() != "")
                                        ar.Add(model.JoinHldrs.Split('|').ElementAt(i).ToString());

                                }
                            }
                            model.PayeeName = ar;
                        }
                    }
                    else
                    {
                        cbstetails Tempcbdtls = new cbstetails();
                        Tempcbdtls.cbsdls = "|F|Account does not exist";
                        model = Tempcbdtls;
                    }

                }

                model.callby = callby;
                model.payeenameselected = payeename;
                return PartialView("_GetCBSDtls", model);
            }
            catch (Exception e)
            {
                //Server.MapPath(strMappath);
                ErrorDisplay er = new ErrorDisplay();
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;
                er.ErrorMessage = message;

                // return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL1 HttpPost OWL1- " + innerExcp });
                return PartialView("Error", er);
            }
        }
        public PartialViewResult RejectReason(int id = 0)
        {

            var rjrs = (from r in af.ItemReturnReasons
                        select new RejectReason
                        {
                            Description = r.DESCRIPTION,
                            ReasonCodeS = r.RETURN_REASON_CODE
                        });
            return PartialView("RejectReason", rjrs);

        }
        public PartialViewResult GetClientDlts(string ac = null)
        {
            var customer = (from c in af.CMS_CustomerMaster
                            where c.Customer_Code == ac
                            select new { c.Customer_Name }).SingleOrDefault();
            if (customer != null)
                ViewBag.customer = customer.Customer_Name;
            else
                ViewBag.customer = null;

            return PartialView("GetClientDlts");
        }
        public PartialViewResult GetBankName(string bankcode = null)
        {
            //if (bankcode != null && bankcode != "")
            //{
            //    //string tempbankcode = bankcode.Substring(3, 3);
            //    int bnkCust = Convert.ToInt16(Session["CustomerID"]);
            //    var Banks = (from c in af.BankBranches
            //                 from ct in af.CustomerMasters
            //                 where c.GridID == ct.GridId && ct.Id == bnkCust && c.Bank_BankCode == bankcode
            //                 select new { c.BankName }).SingleOrDefault();
            //    if (Banks != null)
            //        ViewBag.BankName = Banks.BankName;
            //    else
            //        ViewBag.BankName = null;
            //}
            //else
            //    ViewBag.BankName = null;
            if (bankcode != null && bankcode != "")
            {
                string tempbankcode = bankcode.Substring(3, 3);
                var Banks = (from c in af.Banks
                             where c.BankCode.Substring(3, 3) == tempbankcode
                             select new { c.BankName }).SingleOrDefault();
                if (Banks != null)
                    ViewBag.BankName = Banks.BankName;
                else
                    ViewBag.BankName = null;
            }
            else
                ViewBag.BankName = null;


            return PartialView("_Bankname");
        }
        //-------------------------Added On 24/07/2017-----------------------For Slip Image-----------
        public JsonResult slipImage(Int64 SlipId = 0)
        {
            var owL2 = af.L3Verification.Where(m => m.Id == SlipId).FirstOrDefault().FrontGreyImagePath;


            return Json(owL2, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OWL3Chq(int id = 0)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            if ((bool)Session["RVF"] == false)
            {

                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            try
            {
                Session["VFType"] = "Normal";

                SqlDataAdapter adp = new SqlDataAdapter("OWSelectCHQL3", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@VFtype", SqlDbType.NVarChar).Value = "Normal";
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();
                //-------------Added on 17-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                //-------------Added on 18-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);
                //-------------Added on 12-09-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = Session["CtsSessionType"];

                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<L3VerificationModel>();
                L3VerificationModel def;

                if (ds.Tables[0].Rows.Count > 0)
                {

                    def = new L3VerificationModel
                    {
                        Id = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]),
                        BatchNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[1]),
                        BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[2]),
                        InstrumentType = ds.Tables[0].Rows[0].ItemArray[3].ToString(),
                        ClearingType = ds.Tables[0].Rows[0].ItemArray[4].ToString(),
                        //SlipNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[5].ToString()),
                        //SlipChequeCount = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[6].ToString()),
                        //SlipAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[7]),
                        //ChequeAmountTotal = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[8]),
                        Status = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[5]),
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[6].ToString(),
                        FrontGreyImagePath = ds.Tables[0].Rows[0].ItemArray[7].ToString(),
                        BackTiffImagePath = ds.Tables[0].Rows[0].ItemArray[8].ToString(),
                        ClientCode = ds.Tables[0].Rows[0].ItemArray[9].ToString(),
                        SlipRefNo = ds.Tables[0].Rows[0].ItemArray[10].ToString(),
                        CreditAccountNo = ds.Tables[0].Rows[0].ItemArray[11].ToString(),
                        BranchCode = ds.Tables[0].Rows[0].ItemArray[12].ToString(),
                        ProcessingDate = Convert.ToDateTime(ds.Tables[0].Rows[0].ItemArray[13].ToString()),
                        ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[14].ToString()),
                        RawDataId = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[15].ToString()),
                        DomainId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[17].ToString()),
                        CustomerId = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[18].ToString()),
                        ScanningType = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[19].ToString()),
                        L1VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[20].ToString()),
                        L1UserId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[21].ToString()),
                        L1RejectReason = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[22].ToString()),
                        PayeeName = ds.Tables[0].Rows[0].ItemArray[23].ToString(),
                        L2VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[24].ToString()),
                        L2UserId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[25].ToString()),
                        L2RejectReason = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[26].ToString()),
                        CBSAccountInformation = ds.Tables[0].Rows[0].ItemArray[27].ToString(),
                        CBSJointAccountInformation = ds.Tables[0].Rows[0].ItemArray[28].ToString(),
                        UserNarration = ds.Tables[0].Rows[0].ItemArray[29].ToString(),
                        RejectReasonDescription = ds.Tables[0].Rows[0].ItemArray[30].ToString(),
                        FinalAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[31]),
                        FinalDate = ds.Tables[0].Rows[0].ItemArray[32].ToString(),
                        ChequeNoFinal = ds.Tables[0].Rows[0].ItemArray[33].ToString(),
                        SortCodeFinal = ds.Tables[0].Rows[0].ItemArray[34].ToString(),
                        SANFinal = ds.Tables[0].Rows[0].ItemArray[35].ToString(),
                        TransCodeFinal = ds.Tables[0].Rows[0].ItemArray[36].ToString(),
                        DocType = ds.Tables[0].Rows[0].ItemArray[37].ToString(),
                        Modified2 = ds.Tables[0].Rows[0].ItemArray[38].ToString(),
                        callby = "Cheq",
                    };
                    objectlst.Add(def);
                    //------------------------END------------------------//
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        def = new L3VerificationModel
                        {
                            Id = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),
                            BatchNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[1]),
                            BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[2]),
                            InstrumentType = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                            ClearingType = ds.Tables[0].Rows[index].ItemArray[4].ToString(),
                            //SlipNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[5].ToString()),
                            //SlipChequeCount = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[6].ToString()),
                            //SlipAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[7]),
                            //ChequeAmountTotal = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[8]),
                            Status = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[5]),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[6].ToString(),
                            FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[7].ToString(),
                            BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[8].ToString(),
                            ClientCode = ds.Tables[0].Rows[index].ItemArray[9].ToString(),
                            SlipRefNo = ds.Tables[0].Rows[index].ItemArray[10].ToString(),
                            CreditAccountNo = ds.Tables[0].Rows[index].ItemArray[11].ToString(),
                            BranchCode = ds.Tables[0].Rows[index].ItemArray[12].ToString(),
                            ProcessingDate = Convert.ToDateTime(ds.Tables[0].Rows[index].ItemArray[13].ToString()),
                            ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[14].ToString()),
                            RawDataId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[15].ToString()),
                            DomainId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[17].ToString()),
                            CustomerId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[18].ToString()),
                            ScanningType = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[19].ToString()),
                            L1VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[20].ToString()),
                            L1UserId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[21].ToString()),
                            L1RejectReason = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[22].ToString()),
                            PayeeName = ds.Tables[0].Rows[index].ItemArray[23].ToString(),
                            L2VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[24].ToString()),
                            L2UserId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[25].ToString()),
                            L2RejectReason = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[26].ToString()),
                            CBSAccountInformation = ds.Tables[0].Rows[index].ItemArray[27].ToString(),
                            CBSJointAccountInformation = ds.Tables[0].Rows[index].ItemArray[28].ToString(),
                            UserNarration = ds.Tables[0].Rows[index].ItemArray[29].ToString(),
                            RejectReasonDescription = ds.Tables[0].Rows[index].ItemArray[30].ToString(),
                            FinalAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[31]),
                            FinalDate = ds.Tables[0].Rows[index].ItemArray[32].ToString(),
                            ChequeNoFinal = ds.Tables[0].Rows[index].ItemArray[33].ToString(),
                            SortCodeFinal = ds.Tables[0].Rows[index].ItemArray[34].ToString(),
                            SANFinal = ds.Tables[0].Rows[index].ItemArray[35].ToString(),
                            TransCodeFinal = ds.Tables[0].Rows[index].ItemArray[36].ToString(),
                            DocType = ds.Tables[0].Rows[index].ItemArray[37].ToString(),
                            Modified2 = ds.Tables[0].Rows[index].ItemArray[38].ToString(),
                            callby = "Cheq",
                        };
                        //ViewBag.cnt = true;
                        objectlst.Add(def);
                        count = count - 1;
                        index = index + 1;
                    }
                    //var rtnlist = iwafl.ItemReturnReasons.Select(m => m.RETURN_REASON_CODE).ToList();
                    //ViewBag.rtnlist = rtnlist;

                    var rtnlist = (from i in af.ItemReturnReasons select i).ToList();// iwafl.ItemReturnReasons.Select(m).ToList();
                    ViewBag.rtnlist = rtnlist.Select(m => m.RETURN_REASON_CODE).ToList();
                    ViewBag.rtnlistDescrp = rtnlist.Select(m => m.DESCRIPTION).ToList();

                    //-------------------------------For Narration Accounts---------------------
                    ViewBag.narration = (from n in af.NarrationAccount where n.status == 1 select n.AccountNumber).ToList();
                    //-------------------------------For SchemCode---------------------
                    ViewBag.SchemCode = (from n in af.NreNroAccountList where n.IsActive == true select n.SchemCode).ToList();

                    @Session["glob"] = null;
                    ViewBag.cnt = true;
                    return View(objectlst);
                }
                else
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

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL3 HttpGet OWL3- " + innerExcp });
            }

        }

        [HttpPost]
        public ActionResult OWL3Chq(List<string> lst, bool snd, string img = null, string btnClose = "default", List<Int64> idlst = null, double ChequeAmountTotal = 0)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["RVF"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            //IWAmountTmpProcess jt;
            int uid = (int)Session["uid"];//That will be Session value.
            int ttcnt = 0;
            try
            {
                if (lst != null)
                    ttcnt = lst.Count() / 42;

                int stu;
                int resncode = 0;
                string rejctdecrptn = null;
                string cbdclnts = "";
                string cbdJointdtls = "";
                byte rejct = 0;
                string modaction = "";
                string userNarration = "";
                string rejectreasondescrpsn = "";
                string Clearingtype = "";
                bool mark2pf = false;
                bool ignoreIQA = false;
                string DocType = "B";
                string instrumenttype = "";
                string finalModified = "";
                Int64 SlipID = 0;
                Int64 SlipRawaDataID = 0;
                DataSet ds = new DataSet();
                ObjectClass os = new ObjectClass();
                L3VerificationModel def;
                var objectlst = new List<L3VerificationModel>();
                Int64 id = 0;
                if (btnClose == "Close" && lst != null)
                    ttcnt = ttcnt + 1;
                else if (btnClose == "Close" && lst == null)
                    goto Close;
                // string IWDicision = Request.Form["IWDecision"].ToUpper();
                //if (snd == false)

                instrumenttype = lst[5].ToString();

                if (lst[5].ToString() == "S")
                {
                    if (ttcnt == 1)
                    {
                        //for (int i = 0; i < ttcnt; i++)
                        //{
                        if (lst[12].ToString() == "A")
                        {

                            objectlst = os.selectL3Cheques(con, uid, Session["LoginID"].ToString(), lst, Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), img, "Slip", false, Convert.ToInt16(Session["CustomerID"]), Convert.ToInt32(Session["DomainselectID"]), Session["CtsSessionType"].ToString());

                        }
                        else if (lst[12].ToString() == "F")
                        {
                            if (lst[41] != null)
                                finalModified = lst[41].ToString();

                            id = Convert.ToInt64(lst[0]);
                            OWpro.UpdateOWL3(id, Convert.ToInt64(lst[14]), uid, lst[5].ToString(), Convert.ToDouble(lst[2].ToString()), null, null, null, null, null, lst[1].ToString(),
                                lst[27].ToString(), Convert.ToInt16(lst[13]), null, "RF", @Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"),
                                Convert.ToInt16(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Convert.ToInt32(lst[9].ToString()), null, Convert.ToDouble(lst[2].ToString()), null, userNarration, null, null, null, null, null, null, finalModified);

                            objectlst = os.selectL3Cheques(con, uid, Session["LoginID"].ToString(), lst, Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), img, "Slip", true, Convert.ToInt16(Session["CustomerID"]), Convert.ToInt32(Session["DomainselectID"]), Session["CtsSessionType"].ToString());

                        }
                        else if (lst[12].ToString() == "R")
                        {
                            if (lst[15] != null)
                                rejct = Convert.ToByte(lst[15].ToString());
                            if (rejct == 88)
                            {
                                if (lst[36] != null)
                                    rejectreasondescrpsn = lst[36].ToString();
                                else
                                    rejectreasondescrpsn = "Other Reason";
                            }
                            //---------------Added On 21/06/2017------------------
                            if (lst[39] != null)
                                SlipID = Convert.ToInt64(lst[39]);
                            if (lst[40] != null)
                                SlipRawaDataID = Convert.ToInt64(lst[40]);

                            if (lst[41] != null)
                                finalModified = lst[41].ToString();

                            OWpro.UpdateVerificationSlipOnly(Convert.ToInt64(lst[14]), uid, Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt32(lst[3].ToString()), Convert.ToInt32(lst[6].ToString()), Convert.ToInt32(lst[9].ToString()), lst[7].ToString(),
                                     Convert.ToDouble(lst[2].ToString()), lst[1].ToString(), lst[27].ToString(), lst[18].ToString(), lst[19].ToString(), "L3R", rejct, null, userNarration, rejectreasondescrpsn,
                                     Convert.ToInt32(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Session["LoginID"].ToString(), SlipID, SlipRawaDataID, null, 0, finalModified,null);
                            //----------------------------------------Commenetd On 17-01-2017------------
                            //id = Convert.ToInt64(lst[0]);
                            //OWpro.UpdateOWL3(id, Convert.ToInt64(lst[14]), uid, lst[5].ToString(), Convert.ToDouble(lst[2].ToString()), null, null, null, null, null, lst[1].ToString(),
                            //    null, Convert.ToInt16(lst[13]),rejct, lst[12].ToString(), @Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"),
                            //    Convert.ToInt16(lst[17].ToString()), Convert.ToInt16(lst[16].ToString()), Convert.ToInt16(lst[9].ToString()), null, Convert.ToDouble(lst[2].ToString()), null);
                            //----------------------------------------Commenetd On 17-01-2017------------END
                            objectlst = os.selectL3Cheques(con, uid, Session["LoginID"].ToString(), lst, Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), img, "Slip", true, Convert.ToInt16(Session["CustomerID"]), Convert.ToInt32(Session["DomainselectID"]), Session["CtsSessionType"].ToString());
                        }

                    }
                }
                else if (lst[5].ToString() == "C")
                {
                    string finaldate = "";
                    if (ttcnt == 1)
                    {
                        for (int i = 0; i < ttcnt; i++)
                        {
                            id = Convert.ToInt64(lst[0]);

                            if (lst[15] != null && lst[15].ToString() != "")
                                rejct = Convert.ToByte(lst[15].ToString());

                            if (lst[21] != null)
                            {
                                if (lst[21].ToString().Length != 10)
                                    finaldate = "20" + lst[21].ToString().Substring(4, 2) + "-" + lst[21].ToString().Substring(2, 2) + "-" + lst[21].ToString().Substring(0, 2);
                                else
                                    finaldate = lst[21].ToString();
                            }
                            //--------Modification Validation------------
                            if (lst[12].ToString() == "A")
                            {
                                if (Convert.ToBoolean(lst[30]) == true)
                                    modaction = "M";
                                else
                                    modaction = "A";
                            }
                            else if (lst[12].ToString() == "R")
                            {
                                modaction = "R";
                                if (rejct == 88)
                                {
                                    if (lst[36] != null)
                                        rejectreasondescrpsn = lst[36].ToString();
                                    else
                                        rejectreasondescrpsn = "Other Reason";
                                }
                            }

                            if (lst[35] != null)
                                userNarration = lst[35].ToString();

                            if (lst[37] != null)
                                Clearingtype = lst[37].ToString();

                            //------------------marking P2F--------------------//
                            if (lst[38] != null)
                            {
                                mark2pf = Convert.ToBoolean(lst[38]);
                                if (mark2pf == true)
                                {
                                    ignoreIQA = true;
                                    DocType = "C";
                                }
                                else
                                {
                                    ignoreIQA = false;
                                    DocType = "B";
                                }

                            }
                            else
                            {
                                ignoreIQA = false;
                                DocType = "B";
                            }
                            //---------------Added On 21/06/2017------------------
                            if (lst[39] != null)
                                SlipID = Convert.ToInt64(lst[39]);
                            if (lst[40] != null)
                                SlipRawaDataID = Convert.ToInt64(lst[40]);

                            if (lst[41] != null)
                                finalModified = lst[41].ToString();


                            OWpro.UpdateOWL3(id, Convert.ToInt64(lst[14]), uid, lst[5].ToString(), Convert.ToDouble(lst[20].ToString()), finaldate, lst[22].ToString(), lst[23].ToString(), lst[24].ToString(), lst[25].ToString(), lst[1].ToString(),
                                lst[27].ToString(), Convert.ToInt16(lst[13]), rejct, modaction, @Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"),
                                Convert.ToInt32(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Convert.ToInt32(lst[9].ToString()), null, 0, null, userNarration,
                                rejectreasondescrpsn, Clearingtype, lst[18].ToString(), lst[19].ToString(), ignoreIQA, DocType, finalModified);

                            for (int k = 0; k < idlst.Count; k++)
                            {
                                if (idlst[k] == id)
                                    idlst.RemoveAt(k);
                            }
                        }
                        //if (ChequeAmountTotal != Convert.ToDouble(lst[26]))
                        //{
                        //    OWpro.UpdateChequeAmountTotalL1(Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt32(lst[3].ToString()), Convert.ToInt32(lst[6].ToString()), Convert.ToInt32(lst[9].ToString()), lst[7].ToString(), ChequeAmountTotal, "L3");
                        //}
                        if (btnClose == "Close")
                            goto Close;
                    }
                    else
                    {
                        for (int i = 0; i < ttcnt - 1; i++)
                        {
                            id = Convert.ToInt64(lst[0]);

                            if (lst[15] != null && lst[15].ToString() != "")
                                rejct = Convert.ToByte(lst[15].ToString());

                            if (lst[21] != null)
                            {
                                if (lst[21].ToString().Length != 10)
                                    finaldate = "20" + lst[21].ToString().Substring(4, 2) + "-" + lst[21].ToString().Substring(2, 2) + "-" + lst[21].ToString().Substring(0, 2);
                                else
                                    finaldate = lst[21].ToString();
                            }
                            //--------Modification Validation------------
                            if (lst[12].ToString() == "A")
                            {
                                if (Convert.ToBoolean(lst[30]) == true)
                                    modaction = "M";
                                else
                                    modaction = "A";
                            }
                            else if (lst[12].ToString() == "R")
                            {
                                modaction = "R";
                                if (rejct == 88)
                                {
                                    if (lst[36] != null)
                                        rejectreasondescrpsn = lst[36].ToString();
                                    else
                                        rejectreasondescrpsn = "Other Reason";
                                }
                            }

                            if (lst[35] != null)
                                userNarration = lst[35].ToString();

                            if (lst[37] != null)
                                Clearingtype = lst[37].ToString();

                            //------------------marking P2F--------------------//
                            if (lst[38] != null)
                            {
                                mark2pf = Convert.ToBoolean(lst[38]);
                                if (mark2pf == true)
                                {
                                    ignoreIQA = true;
                                    DocType = "C";
                                }
                                else
                                {
                                    ignoreIQA = false;
                                    DocType = "B";
                                }

                            }
                            else
                            {
                                ignoreIQA = false;
                                DocType = "B";
                            }

                            //---------------Added On 21/06/2017------------------
                            if (lst[39] != null)
                                SlipID = Convert.ToInt64(lst[39]);
                            if (lst[40] != null)
                                SlipRawaDataID = Convert.ToInt64(lst[40]);

                            if (lst[41] != null)
                                finalModified = lst[41].ToString();

                            OWpro.UpdateOWL3(id, Convert.ToInt64(lst[14]), uid, lst[5].ToString(), Convert.ToDouble(lst[20].ToString()), finaldate, lst[22].ToString(), lst[23].ToString(), lst[24].ToString(), lst[25].ToString(), lst[1].ToString(),
                                lst[27].ToString(), Convert.ToInt16(lst[13]), rejct, modaction, @Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"),
                                Convert.ToInt32(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Convert.ToInt32(lst[9].ToString()), null, 0, null, userNarration,
                                rejectreasondescrpsn, Clearingtype, lst[18].ToString(), lst[19].ToString(), ignoreIQA, DocType, finalModified);

                            for (int k = 0; k < idlst.Count; k++)
                            {
                                if (idlst[k] == id)
                                    idlst.RemoveAt(k);
                            }
                            lst.RemoveRange(0, 42);
                        }
                        if (btnClose == "Close")
                            goto Close;
                        //if (ChequeAmountTotal != Convert.ToDouble(lst[26]))
                        //{
                        //    OWpro.UpdateChequeAmountTotalL1(Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt32(lst[3].ToString()), Convert.ToInt32(lst[6].ToString()), Convert.ToInt32(lst[9].ToString()), lst[7].ToString(), ChequeAmountTotal, "L3");
                        //}
                    }
                    objectlst = os.selectL3ChequesOnly(con, uid, Session["LoginID"].ToString(), lst, Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), img, "Cheq", false, Convert.ToInt16(Session["CustomerID"]), Convert.ToInt32(Session["DomainselectID"]), Session["CtsSessionType"].ToString(), Session["VFType"].ToString());
                }



            //---------------------------IF Close button called--------------------------------//
            Close:
                if (btnClose == "Close")
                {
                    @Session["glob"] = true;
                    for (int p = 0; p < idlst.Count; p++)
                    {
                        OWpro.OWUnlockRecords(idlst[p], "L3");
                    }

                    //if (instrumenttype == "C")
                    //    OWpro.OWUnlockRecords(SlipID, "L3");

                    return Json(false);
                }

                //-------------Calling next Records---------------

                if (objectlst != null || objectlst.Count != 0)
                {
                    ViewBag.cnt = true;
                    return Json(objectlst);
                }

                @Session["glob"] = true;
                return Json(false);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL3 HttpPost OWL3- " + innerExcp });
            }

        }
    }
}
