using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ikloud_Aflatoon.Infrastructure;
using PagedList;
using System.Collections;
using ikloud_Aflatoon.Models;
using System.Net;
using System.IO;
using System.Drawing;


namespace ikloud_Aflatoon.Controllers
{
    public class IWL1Controller : Controller
    {
        //
        // GET: /IWL1/
        AflatoonEntities iwafl = new AflatoonEntities();
        
        IWProcDataContext iwpro = new IWProcDataContext();

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);

        public ActionResult Selection()
        {
            ViewBag.ClearingType = new SelectList(iwafl.ClearingTypes, "Code", "Name").ToList();

            return View();
        }
        [HttpPost]
        public ActionResult Selection(L2Helper lHelpr)
        {

            string clrtype = Request.Form["ClearingType"];
            Session["clrtype"] = clrtype;
            lHelpr.Clrtype = clrtype;


            return RedirectToAction("Index", new { clrtype = lHelpr.Clrtype });
        }
        public ActionResult Index(string VFTYPE = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            if ((bool)Session["QC"] == false)
            {

                UserMaster usrm = iwafl.UserMasters.Find(uid);
                usrm.Active = false;
                iwafl.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            try
            {
                Session["VFTYPE"] = VFTYPE;

                SqlDataAdapter adp = new SqlDataAdapter("IWL1Verification", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@procDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                adp.SelectCommand.Parameters.Add("@ClrType", SqlDbType.NVarChar).Value = Session["CtsSessionType"];
                //--------------------Customer Selection---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                adp.SelectCommand.Parameters.Add("@VFTYPE", SqlDbType.NVarChar).Value = VFTYPE;



                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<IWTempL1VerificationModel>();
                IWTempL1VerificationModel def;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    def = new IWTempL1VerificationModel
                    {
                        ID = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]),
                        XMLAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[1]),
                        ActualAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[2]),
                        DbtAccountNo = ds.Tables[0].Rows[0].ItemArray[3].ToString(),
                        Date = ds.Tables[0].Rows[0].ItemArray[4].ToString(),
                        XMLSerialNo = ds.Tables[0].Rows[0].ItemArray[5].ToString(),
                        XMLPayorBankRoutNo = ds.Tables[0].Rows[0].ItemArray[6].ToString(),
                        XMLSAN = ds.Tables[0].Rows[0].ItemArray[7].ToString(),
                        XMLTransCode = ds.Tables[0].Rows[0].ItemArray[8].ToString(),
                        EntrySerialNo = ds.Tables[0].Rows[0].ItemArray[9].ToString(),
                        EntryPayorBankRoutNo = ds.Tables[0].Rows[0].ItemArray[10].ToString(),
                        EntrySAN = ds.Tables[0].Rows[0].ItemArray[11].ToString(),
                        EntryTransCode = ds.Tables[0].Rows[0].ItemArray[12].ToString(),
                        FrontGreyImagePath = replaceImgpath(ds.Tables[0].Rows[0].ItemArray[13].ToString()),
                        FrontTiffImagePath = replaceImgpath(ds.Tables[0].Rows[0].ItemArray[14].ToString().Replace("tif", "jpg")),
                        CBSClientAccountDtls = ds.Tables[0].Rows[0].ItemArray[15].ToString(),
                        CBSJointHoldersName = ds.Tables[0].Rows[0].ItemArray[16].ToString(),
                        BackTiffImagePath = replaceImgpath(ds.Tables[0].Rows[0].ItemArray[17].ToString().Replace("tif", "jpg")),
                        EntryPayeeName = ds.Tables[0].Rows[0].ItemArray[18].ToString(),
                        XMLMICRRepairFlags = ds.Tables[0].Rows[0].ItemArray[19].ToString(),
                    };
                    objectlst.Add(def);
                    //------------------------END------------------------//
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        def = new IWTempL1VerificationModel
                        {
                            ID = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),
                            XMLAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[1]),
                            ActualAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[2]),
                            DbtAccountNo = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                            Date = ds.Tables[0].Rows[index].ItemArray[4].ToString(),
                            XMLSerialNo = ds.Tables[0].Rows[index].ItemArray[5].ToString(),
                            XMLPayorBankRoutNo = ds.Tables[0].Rows[index].ItemArray[6].ToString(),
                            XMLSAN = ds.Tables[0].Rows[index].ItemArray[7].ToString(),
                            XMLTransCode = ds.Tables[0].Rows[index].ItemArray[8].ToString(),
                            EntrySerialNo = ds.Tables[0].Rows[index].ItemArray[9].ToString(),
                            EntryPayorBankRoutNo = ds.Tables[0].Rows[index].ItemArray[10].ToString(),
                            EntrySAN = ds.Tables[0].Rows[index].ItemArray[11].ToString(),
                            EntryTransCode = ds.Tables[0].Rows[index].ItemArray[12].ToString(),
                            FrontGreyImagePath = replaceImgpath(ds.Tables[0].Rows[index].ItemArray[13].ToString()),
                            FrontTiffImagePath = replaceImgpath(ds.Tables[0].Rows[index].ItemArray[14].ToString().Replace("tif", "jpg")),
                            CBSClientAccountDtls = ds.Tables[0].Rows[index].ItemArray[15].ToString(),
                            CBSJointHoldersName = ds.Tables[0].Rows[index].ItemArray[16].ToString(),
                            BackTiffImagePath = replaceImgpath(ds.Tables[0].Rows[index].ItemArray[17].ToString().Replace("tif", "jpg")),
                            EntryPayeeName = ds.Tables[0].Rows[index].ItemArray[18].ToString(),
                            XMLMICRRepairFlags = ds.Tables[0].Rows[index].ItemArray[19].ToString(),
                        };
                        //ViewBag.cnt = true;
                        objectlst.Add(def);
                        count = count - 1;
                        index = index + 1;
                    }
                    //var rtnlist = iwafl.ItemReturnReasons.Select(m => m.RETURN_REASON_CODE).ToList();
                    //ViewBag.rtnlist = rtnlist;
                    //-------------Added on 17-09-2020------By Abid----------
                    string[] code = { "05", "10", "11", "12", "13", "14", "15", "16", "17", "30", "31", "32", "33","34","35","36","37","38","39","40","50","52","53","54"
                                    ,"55","60","61","62","63","64","65","66","67","68","71","75","76","85","86","87","88"};
                    var rjrs = (from r in iwafl.ItemReturnReasons
                                where code.Contains(r.RETURN_REASON_CODE)
                                select new RejectReason
                                {
                                    Description = r.DESCRIPTION,
                                    ReasonCodeS = r.RETURN_REASON_CODE
                                });
                    //-------------------------END---------------------------

                    //    var rtnlist = (from i in iwafl.ItemReturnReasons select i).ToList();// iwafl.ItemReturnReasons.Select(m).ToList();
                    ViewBag.rtnlist = rjrs.Select(m => m.ReasonCodeS).ToList();
                    ViewBag.rtnlistDescrp = rjrs.Select(m => m.Description).ToList();
                    Session["glob"] = null;
                    ViewBag.cnt = true;
                    return View(objectlst);
                }
                else
                    return RedirectToAction("IWIndex", "Home", new { id = 1 });
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
        public ActionResult IWl1(List<string> lst, bool snd, string img = null, string btnClose = "default", List<Int64> idlst = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["QC"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = iwafl.UserMasters.Find(uid1);
                usrm.Active = false;
                iwafl.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            //IWAmountTmpProcess jt;
            int uid = (int)Session["uid"];//That will be Session value.
            int ttcnt = 0;
            try
            {
                if (lst != null)
                    ttcnt = lst.Count() / 20;

                int stu;
                string resncode = "0";
                string rejctdecrptn = null;
                string cbdclnts = "";
                string cbdJointdtls = "";
                string payeename = "";

                if (btnClose == "Close" && lst != null)
                    ttcnt = ttcnt + 1;
                // string IWDicision = Request.Form["IWDecision"].ToUpper();
                //if (snd == false)
                //{
                //if (ttcnt == 1)
                //{
                //    for (int i = 0; i < ttcnt; i++)
                //    {
                //        if (Convert.ToBoolean(lst[8]) == true)
                //            stu = 1;
                //        else
                //        {
                //            stu = 0;
                //        }
                //        if (lst[14] != null)
                //            if (lst[14].ToString() != "")
                //                resncode = lst[14].ToString();
                //            else
                //                resncode = "0";

                //        if (lst[15] != null)//---------------------CBS Details
                //            if (lst[15].ToString() != "")
                //                cbdclnts = lst[15].ToString();
                //        if (lst[16] != null)//---------------------joint Details
                //            if (lst[16].ToString() != "")
                //                cbdJointdtls = lst[16].ToString();

                //        Int64 id = Convert.ToInt64(lst[0]);
                //        iwpro.UpdateIWL1Verification(id, uid, lst[4].ToString(), lst[5].ToString(), lst[6].ToString(), lst[7].ToString(), lst[1].ToString(), Convert.ToDouble(lst[2]), lst[3].ToString(), stu, resncode, rejctdecrptn, cbdclnts, cbdJointdtls, @Session["LoginID"].ToString());

                //        for (int k = 0; k < idlst.Count; k++)
                //        {
                //            if (idlst[k] == id)
                //                idlst.RemoveAt(k);
                //        }

                //        // lst.RemoveRange(0, 17);
                //    }
                //}
                //else
                //{
                for (int i = 0; i < ttcnt - 1; i++)
                {
                    // jt = new IWAmountTmpProcess();
                    if (Convert.ToBoolean(lst[8]) == true)
                        stu = 1;
                    else
                    {
                        stu = 0;
                    }
                    if (lst[14] != null)
                        if (lst[14].ToString() != "")
                            resncode = lst[14].ToString();
                        else
                            resncode = "0";

                    if (lst[15] != null)//---------------------CBS Details
                        if (lst[15].ToString() != "")
                            cbdclnts = lst[15].ToString();
                    if (lst[16] != null)//---------------------joint Details
                        if (lst[16].ToString() != "")
                            cbdJointdtls = lst[16].ToString();

                    if (lst[17] != null)//---------------------Payee Name
                        if (lst[17].ToString() != "")
                            payeename = lst[17].ToString();

                    Int64 id = Convert.ToInt64(lst[0]);
                    iwpro.UpdateIWL1Verification(id, uid, lst[4].ToString(), lst[5].ToString(), lst[6].ToString(), lst[7].ToString(), lst[1].ToString(), Convert.ToDouble(lst[2]), lst[3].ToString(), stu, resncode, rejctdecrptn, cbdclnts, cbdJointdtls, @Session["LoginID"].ToString(), Convert.ToInt16(Session["CustomerID"]), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), payeename, lst[19].ToString());
                    for (int k = 0; k < idlst.Count; k++)
                    {
                        if (idlst[k] == id)
                            idlst.RemoveAt(k);
                    }
                    lst.RemoveRange(0, 20);
                }
                //}
                //}
                //else
                //{
                //    for (int i = 0; i < ttcnt - 1; i++)
                //    {
                //        // jt = new IWAmountTmpProcess();
                //        if (Convert.ToBoolean(lst[8]) == true)
                //            stu = 15;
                //        else
                //            stu = 16;

                //        if (lst[14] != null)
                //            if (lst[14].ToString() != "")
                //                resncode = Convert.ToInt32(lst[14]);

                //        if (lst[15] != null)//---------------------CBS Details
                //            if (lst[15].ToString() != "")
                //                cbdclnts = lst[15].ToString();
                //        if (lst[16] != null)//---------------------CBS Details
                //            if (lst[16].ToString() != "")
                //                cbdJointdtls = lst[16].ToString();

                //        Int64 id = Convert.ToInt64(lst[0]);
                //        iwpro.UpdateIWL1Verification(id, uid, lst[4].ToString(), lst[5].ToString(), lst[6].ToString(), lst[7].ToString(), lst[1].ToString(), Convert.ToDouble(lst[2]), lst[3].ToString(), stu, resncode, rejctdecrptn, cbdclnts, cbdJointdtls, @Session["LoginID"].ToString());
                //        lst.RemoveRange(0, 17);
                //    }
                //}

                //---------------------------IF Close button called--------------------------------//
                if (btnClose == "Close")
                {
                    @Session["glob"] = true;
                    for (int p = 0; p < idlst.Count; p++)
                    {
                        iwpro.UnlockRecords(idlst[p], "L1");
                    }
                    return Json(false);
                }
                //------------------Select next Pending Record------------------
                //------------------------------Calling Proc for selecting data-----------------------

                // Procommand = new System.Data.Sql.("{ call SelectForDwld('"+ date.ToString("yyyy-MM-dd")+"')}",con);

                SqlDataAdapter adp = new SqlDataAdapter("IWL1Verification", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@procDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                adp.SelectCommand.Parameters.Add("@ClrType", SqlDbType.NVarChar).Value = Session["CtsSessionType"];
                //--------------------Customer Selection---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                adp.SelectCommand.Parameters.Add("@VFTYPE", SqlDbType.NVarChar).Value = Session["VFTYPE"];
                //Session["VFTYPE"]


                ArrayList ids = new ArrayList();
                bool checkid = false;

                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<IWTempL1VerificationModel>();
                IWTempL1VerificationModel def;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //--------------Checking Last record and list record are same------
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        if (Convert.ToInt64(lst[0]) == Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]))
                        {
                            if (Convert.ToBoolean(lst[8]) == true)
                                stu = 1;
                            else
                                stu = 0;
                            if (lst[14] != null)
                                if (lst[14].ToString() != "")
                                    resncode = lst[14].ToString();
                                else
                                    resncode = "0";

                            if (lst[15] != null)//---------------------CBS Details
                                if (lst[15].ToString() != "")
                                    cbdclnts = lst[15].ToString();
                            if (lst[16] != null)//---------------------CBS Details
                                if (lst[16].ToString() != "")
                                    cbdJointdtls = lst[16].ToString();
                            if (lst[17] != null)//---------------------Payee Name
                                if (lst[17].ToString() != "")
                                    payeename = lst[17].ToString();

                            Int64 id = Convert.ToInt64(lst[0]);
                            iwpro.UpdateIWL1Verification(id, uid, lst[4].ToString(), lst[5].ToString(), lst[6].ToString(), lst[7].ToString(), lst[1].ToString(), Convert.ToDouble(lst[2]), lst[3].ToString(), stu, resncode, rejctdecrptn, cbdclnts, cbdJointdtls, @Session["LoginID"].ToString(), Convert.ToInt16(Session["CustomerID"]), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), payeename, lst[17].ToString());
                            goto Notfnd;
                        }

                    }
                    //---------------Adding last record of list-----------------//
                    if (lst[14] != null)

                        if (lst[14] != null)
                            if (lst[14].ToString() != "")
                                resncode = lst[14].ToString();
                            else
                                resncode = "0";

                    if (lst[15] != null)//---------------------CBS Details
                        if (lst[15].ToString() != "")
                            cbdclnts = lst[15].ToString();
                    if (lst[16] != null)//---------------------CBS Details
                        if (lst[16].ToString() != "")
                            cbdJointdtls = lst[16].ToString();

                    def = new IWTempL1VerificationModel
                    {
                        ID = Convert.ToInt64(lst[0]),
                        FrontGreyImagePath = img,
                        DbtAccountNo = lst[1].ToString(),
                        ActualAmount = Convert.ToDecimal(lst[2]),
                        Date = lst[3].ToString(),
                        sttsdtqc = Convert.ToBoolean(lst[8]),
                        EntrySerialNo = lst[4].ToString(),
                        EntryPayorBankRoutNo = lst[5].ToString(),
                        EntrySAN = lst[6].ToString(),
                        EntryTransCode = lst[7].ToString(),
                        XMLAmount = Convert.ToDecimal(lst[9]),
                        XMLSerialNo = lst[10].ToString(),
                        XMLPayorBankRoutNo = lst[11].ToString(),
                        XMLSAN = lst[12].ToString(),
                        XMLTransCode = lst[13].ToString(),
                        RejectReason = resncode,
                        CBSClientAccountDtls = cbdclnts,
                        CBSJointHoldersName = cbdJointdtls,
                        EntryPayeeName = lst[17].ToString(),
                        XMLMICRRepairFlags = lst[18].ToString(),
                        strModified = lst[19].ToString(),
                        //-------------------------------//------------------//
                    };
                    objectlst.Add(def);
                    ids.Add(def.ID);

                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        //for (int i = 0; i < ids.Count; i++)
                        //{
                        // tempId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]);
                        if (Convert.ToInt64(ids[0]) == Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]))
                        {
                            checkid = true;
                            // break;
                        }
                        //}
                        if (checkid == false)
                        {
                            def = new IWTempL1VerificationModel
                            {
                                ID = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),
                                XMLAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[1]),
                                ActualAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[2]),
                                DbtAccountNo = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                                Date = ds.Tables[0].Rows[index].ItemArray[4].ToString(),
                                XMLSerialNo = ds.Tables[0].Rows[index].ItemArray[5].ToString(),
                                XMLPayorBankRoutNo = ds.Tables[0].Rows[index].ItemArray[6].ToString(),
                                XMLSAN = ds.Tables[0].Rows[index].ItemArray[7].ToString(),
                                XMLTransCode = ds.Tables[0].Rows[index].ItemArray[8].ToString(),
                                EntrySerialNo = ds.Tables[0].Rows[index].ItemArray[9].ToString(),
                                EntryPayorBankRoutNo = ds.Tables[0].Rows[index].ItemArray[10].ToString(),
                                EntrySAN = ds.Tables[0].Rows[index].ItemArray[11].ToString(),
                                EntryTransCode = ds.Tables[0].Rows[index].ItemArray[12].ToString(),
                                FrontGreyImagePath = replaceImgpath(ds.Tables[0].Rows[index].ItemArray[13].ToString()),
                                FrontTiffImagePath = replaceImgpath(ds.Tables[0].Rows[index].ItemArray[14].ToString().Replace("tif", "jpg")),
                                CBSClientAccountDtls = ds.Tables[0].Rows[index].ItemArray[15].ToString(),
                                CBSJointHoldersName = ds.Tables[0].Rows[index].ItemArray[16].ToString(),
                                BackTiffImagePath = replaceImgpath(ds.Tables[0].Rows[index].ItemArray[17].ToString().Replace("tif", "jpg")),
                                EntryPayeeName = ds.Tables[0].Rows[index].ItemArray[18].ToString(),
                                XMLMICRRepairFlags = ds.Tables[0].Rows[index].ItemArray[19].ToString(),
                            };
                            ids.Add(def.ID);
                            objectlst.Add(def);
                        }
                        checkid = false;

                        count = count - 1;
                        index = index + 1;
                    }
                    ViewBag.cnt = true;
                    return Json(objectlst);
                }
                else
                {
                    ViewBag.cnt = false;
                    goto Notfnd;
                }
            Notfnd:
                ViewBag.cnt = false;
                @Session["glob"] = true;
                return Json(false);
            }
            catch (Exception e)
            {

                //ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                //er.ErrorMessage = e.InnerException.Message;
                //return View("Error", er);
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.InnerException.Message });
            }
        }

        public PartialViewResult RejectReason(int id = 0)
        {

            //-------------Added on 17-09-2020------By Abid----------
            string[] code = { "05", "10", "11", "12", "13", "14", "15", "16", "17", "30", "31", "32", "33","34","35","36","37","38","39","40","50","52","53","54"
                                    ,"55","60","61","62","63","64","65","66","67","68","71","75","76","85","86","87","88"};
            //-------------------------END---------------------------

            var rjrs = (from r in iwafl.ItemReturnReasons
                        where code.Contains(r.RETURN_REASON_CODE)
                        select new RejectReason
                        {
                            Description = r.DESCRIPTION,
                            ReasonCodeS = r.RETURN_REASON_CODE
                        });
            return PartialView("_RejectReason", rjrs);

            //return PartialView("_RejectReason", rjrs.ToList().ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public PartialViewResult GetCBSDtls(string ac = null, string strcbsdls = null, string strJoinHldrs = null)
        {
            cbstetails model = new cbstetails();
            try
            {
                if (ac != null)
                {
                    if (Session["GetAccountDetails "].ToString().ToUpper() == "L")
                    {
                        //var model
                        model = (from c in iwafl.ACDetails
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

                        //iwpro.GetOnlyIWUpdateCBSAccInfo(ac, 1, ref strcbsdls, ref strJoinHldrs);
                        //model.cbsdls = strcbsdls;
                        //model.JoinHldrs = strJoinHldrs;

                        //======= Adding try catch block for handling finacle down error view on 01/03/2023 by amol ==========
                        try
                        {
                            //---------For CBS Bank----------------

                            iwpro.GetOnlyIWUpdateCBSAccInfo(ac, 1, ref strcbsdls, ref strJoinHldrs);
                            model.cbsdls = strcbsdls;
                            model.JoinHldrs = strJoinHldrs;
                        }
                        catch (Exception ex)
                        {
                            model.CustomError = "Finacle down. Service unavailable!";
                            return PartialView("_GetCBSDtls", model);
                        }

                    }

                    if (model != null && model.cbsdls != null)
                    {
                        if (model.cbsdls.Split('|').ElementAt(1) == "S")
                        {
                            if (model.cbsdls.Split('|').ElementAt(5).Trim() != "")
                            {
                                string MOP = iwafl.MOPCodeMasters.Find(model.cbsdls.Split('|').ElementAt(5)).Description;
                                model.MOP = MOP != null ? MOP : "";
                            }
                            else
                            {
                                model.MOP = "";
                            }
                            if (model.cbsdls.Split('|').ElementAt(6).Trim() != "")
                            {
                                string AccountStatus = iwafl.AccStatusCodeMasters.Find(model.cbsdls.Split('|').ElementAt(6)).Description;
                                model.AccountStatus = AccountStatus != null ? AccountStatus : "";
                            }
                            else
                            {
                                model.AccountStatus = "";
                            }

                            if (model.cbsdls.Split('|').ElementAt(12).Trim() != "")
                            {
                                string AccountOwnership = iwafl.AccOwnershipCodeMasters.Find(model.cbsdls.Split('|').ElementAt(12).ToString()).Description;
                                model.AccountOwnership = AccountOwnership != null ? AccountOwnership : "";
                            }
                            else
                            {
                                model.AccountOwnership = "";
                            }

                            //-------------------------------Added BY Abid on 28-08-2019------------------
                            SqlDataAdapter adp = new SqlDataAdapter("GetValidationForSchemeCode", con);
                            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                            adp.SelectCommand.Parameters.Add("@SchemeCode", SqlDbType.NVarChar).Value = model.cbsdls.Split('|').ElementAt(3).ToString();
                            adp.SelectCommand.Parameters.Add("@Varificationlvl", SqlDbType.NVarChar).Value = "L1";
                            adp.SelectCommand.Parameters.Add("@OutInward", SqlDbType.NVarChar).Value = "IW";
                            //  adp.SelectCommand.Parameters.Add("@InstrumentType", SqlDbType.NVarChar).Value = instrntType;
                            DataSet ds = new DataSet();
                            adp.Fill(ds);

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                model.SchemeCode = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                                model.Message = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                                model.AllowToBlock = ds.Tables[0].Rows[0].ItemArray[2].ToString();
                                model.Amount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[3]);
                                model.popRqd = ds.Tables[0].Rows[0].ItemArray[4].ToString();
                                model.Color = ds.Tables[0].Rows[0].ItemArray[5].ToString();

                            }
                        }
                    }
                    else
                    {
                        cbstetails Tempcbdtls = new cbstetails();
                        Tempcbdtls.cbsdls = "|F|Account does not exist";
                        model = Tempcbdtls;
                    }
                    return PartialView("_GetCBSDtls", model);
                }
                else
                {
                    // cbstetails model = new cbstetails();
                    model.cbsdls = strcbsdls;
                    model.JoinHldrs = strJoinHldrs;
                    if (model.cbsdls != null)
                    {
                        if (model.cbsdls.Split('|').ElementAt(1) == "S")
                        {
                            if (model.cbsdls.Split('|').ElementAt(5).Trim() != "")
                            {
                                string MOP = iwafl.MOPCodeMasters.Find(model.cbsdls.Split('|').ElementAt(5)).Description;
                                model.MOP = MOP != null ? MOP : "";
                            }
                            else
                            {
                                model.MOP = "";
                            }
                            if (model.cbsdls.Split('|').ElementAt(6).Trim() != "")
                            {
                                string AccountStatus = iwafl.AccStatusCodeMasters.Find(model.cbsdls.Split('|').ElementAt(6)).Description;
                                model.AccountStatus = AccountStatus != null ? AccountStatus : "";
                            }
                            else
                            {
                                model.AccountStatus = "";
                            }

                            if (model.cbsdls.Split('|').ElementAt(12).Trim() != "")
                            {
                                string AccountOwnership = iwafl.AccOwnershipCodeMasters.Find(model.cbsdls.Split('|').ElementAt(12).ToString()).Description;
                                model.AccountOwnership = AccountOwnership != null ? AccountOwnership : "";
                            }
                            else
                            {
                                model.AccountOwnership = "";
                            }
                        }
                        else
                        {
                            cbstetails Tempcbdtls = new cbstetails();
                            Tempcbdtls.cbsdls = "|F|Account does not exist";
                            model = Tempcbdtls;
                        }
                    }
                    //else
                    //{
                    //    cbstetails cbdtls = new cbstetails();
                    //    cbdtls.cbsdls = "|F|Account does not exist";
                    //    model = cbdtls;
                    //}
                    return PartialView("_GetCBSDtls", model);
                }
            }
            catch (Exception e)
            {

                ErrorDisplay er = new ErrorDisplay();
                string ServerPath = "";
                string filename = "";
                string fileNameWithPath = "";
                //FormsAuthentication.SignOut();
                Session.Abandon();

                //ViewBag.Error = e.InnerException;
                //er.ErrorMessage = e.InnerException.Message;
                //-------------------------------- ServerPath = Server.MapPath("~/Logs/");----
                if (System.IO.Directory.Exists(ServerPath) == false)
                {
                    System.IO.Directory.CreateDirectory(ServerPath);
                }
                filename = DateTime.Now.ToString("ddMMyyyy") + "Logs.txt";
                fileNameWithPath = ServerPath + filename;
                System.IO.StreamWriter str = new System.IO.StreamWriter(fileNameWithPath, true, System.Text.Encoding.Default);
                //  str.WriteLine("hello");
                str.WriteLine(DateTime.Now.ToShortTimeString() + ": Exception: " + e.Message);
                str.WriteLine(DateTime.Now.ToShortTimeString() + ": StackTrace: " + er.ErrorMessage);
                str.Close();
                return PartialView("Error", er);
                // return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
                //return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }
        [HttpPost]
        public PartialViewResult GetCBSDtls_New(string ac = null, string strcbsdls = null, string strJoinHldrs = null, string callby = null, string payeename = null)
        {
            cbstetails model = new cbstetails();
            try
            {
                if (strcbsdls != null && strcbsdls != "")
                {

                    model.cbsdls = strcbsdls;
                    model.JoinHldrs = strJoinHldrs;
                    if (model.cbsdls != null && model.cbsdls != "")
                    {

                        //-------------------------For Creditcard-----------------------
                        //if (ac.Length == 16 && ac != "9999999999999999")
                        //{
                        //    if (Session["CreditCardValidationReq"].ToString() == "1")
                        //    {

                        //        if (model.cbsdls.Split('|').ElementAt(1) == "S")
                        //        {
                        //            if (model.cbsdls.Length < 5)
                        //            {
                        //                model.cbsdls = "|S|CREDIT CARD-BRANCH COLLECTION A/C|CRC1| || |0|N|N|N|OAB|O";
                        //                model.JoinHldrs = "|S|CREDIT CARD-BRANCH COLLECTION A/C|CRC1|VALID CARD || |0|N|N|N|OAB|O";
                        //            }
                        //        }
                        //        else
                        //        {
                        //            if (model.cbsdls.Length < 5)
                        //            {
                        //                model.cbsdls = "|S|CREDIT CARD-BRANCH COLLECTION A/C|CRC1| || |0|N|N|N|OAB|O";
                        //                model.JoinHldrs = "|F|CREDIT CARD-BRANCH COLLECTION A/C|CRC1|INVALID CARD || |0|N|N|N|OAB|O";
                        //            }
                        //        }

                        //    }
                        //}
                        //else if (ac.Length == 16 && ac == "9999999999999999")
                        //{
                        //    model.cbsdls = "|F|Account does not exist";
                        //    model.JoinHldrs = "|F|Account does not exist";
                        //}
                        //-------------------------For Creditcard-------END----------------
                        if (model.cbsdls.Split('|').ElementAt(1) == "S")
                        {


                            if (model.cbsdls.Split('|').ElementAt(6).Trim() != "")
                            {
                                string MOP = iwafl.MOPCodeMasters.Find(model.cbsdls.Split('|').ElementAt(6)).Description;
                                model.MOP = MOP != null ? MOP : "";
                            }
                            else
                            {
                                model.MOP = "";
                            }
                            if (model.cbsdls.Split('|').ElementAt(7).Trim() != "")
                            {
                                string AccountStatus = iwafl.AccStatusCodeMasters.Find(model.cbsdls.Split('|').ElementAt(7)).Description;
                                model.AccountStatus = AccountStatus != null ? AccountStatus : "";
                            }
                            else
                            {
                                model.AccountStatus = "";
                            }

                            if (model.cbsdls.Split('|').ElementAt(13).Trim() != "")
                            {
                                string AccountOwnership = iwafl.AccOwnershipCodeMasters.Find(model.cbsdls.Split('|').ElementAt(13).ToString()).Description;
                                model.AccountOwnership = AccountOwnership != null ? AccountOwnership : "";
                            }
                            else
                            {
                                model.AccountOwnership = "";
                            }
                            List<string> ar = new List<string>();
                            ar.Add(model.cbsdls.Split('|').ElementAt(3).ToString());

                            if (model.cbsdls.Split('|').ElementAt(14).Trim() != "") //model.JoinHldrs.Split('|').Count() - 1 remove on 05/05/2017
                            {
                                for (int i = 15; i < model.cbsdls.Split('|').Count(); i++)
                                {
                                    if (model.cbsdls.Split('|').ElementAt(i).ToString() != "")
                                        ar.Add(model.cbsdls.Split('|').ElementAt(i).ToString().Replace("PRIMARY-", " ").Replace("JOINT-", " "));

                                }
                            }
                            model.PayeeName = ar;
                        }
                        else
                        {
                            if (model.cbsdls.Split('|').ElementAt(1) == "F")
                            {
                                model.cbsdls = model.cbsdls;
                                model.JoinHldrs = model.JoinHldrs;
                            }
                            else
                            {
                                cbstetails Tempcbdtls = new cbstetails();
                                Tempcbdtls.cbsdls = "|F|Account does not exist";
                                model = Tempcbdtls;
                            }

                        }
                    }
                    // model.callby = callby;
                }
                else if (ac != null)
                {


                    if (Session["GetAccountDetails "].ToString().ToUpper() == "L")
                    {
                        model = (from c in iwafl.ACDetails
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
                        //======= Adding try catch block for handling finacle down error view on 01/03/2023 by amol ==========
                        try
                        {
                            //---------For CBS Bank----------------

                            iwpro.OWGetCBSAccInfoWithOutUpdate_New(ac, ref strcbsdls, ref strJoinHldrs);
                            model.cbsdls = strcbsdls;
                            model.JoinHldrs = strJoinHldrs;
                            //---------------------------------
                        }
                        catch (Exception ex)
                        {
                            model.CustomError = "Finacle down. Service unavailable!";
                            return PartialView("_GetCBSDtls", model);
                        }
                        
                    }

                    if (model != null && model.cbsdls != null)
                    {
                        //-------------------------For Creditcard-----------------------
                        //if (ac.Length == 16 && ac != "9999999999999999")
                        //{
                        //    if (Session["CreditCardValidationReq"].ToString() == "1")
                        //    {
                        //        if (model.cbsdls.Split('|').ElementAt(1) == "S")
                        //        {
                        //            if (model.cbsdls.Length < 5)
                        //            {
                        //                model.cbsdls = "|S|CREDIT CARD-BRANCH COLLECTION A/C|CRC1| || |0|N|N|N|OAB|O";
                        //                model.JoinHldrs = "|S|CREDIT CARD-BRANCH COLLECTION A/C|CRC1|VALID CARD || |0|N|N|N|OAB|O";
                        //            }
                        //        }
                        //        else
                        //        {
                        //            if (model.cbsdls.Length < 5)
                        //            {
                        //                model.cbsdls = "|S|CREDIT CARD-BRANCH COLLECTION A/C|CRC1| || |0|N|N|N|OAB|O";
                        //                model.JoinHldrs = "|F|CREDIT CARD-BRANCH COLLECTION A/C|CRC1|INVALID CARD || |0|N|N|N|OAB|O";
                        //            }
                        //        }

                        //    }
                        //}
                        //else if (ac.Length == 16 && ac == "9999999999999999")
                        //{
                        //    model.cbsdls = "|F|Account does not exist";
                        //    model.JoinHldrs = "|F|Account does not exist";
                        //}
                        //-------------------------For Creditcard-------END----------------
                        if (model.cbsdls.Split('|').ElementAt(1) == "S")
                        {
                            if (model.cbsdls.Split('|').ElementAt(6).Trim() != "")
                            {
                                string MOP = iwafl.MOPCodeMasters.Find(model.cbsdls.Split('|').ElementAt(6)).Description;
                                model.MOP = MOP != null ? MOP : "";
                            }
                            else
                            {
                                model.MOP = "";
                            }
                            if (model.cbsdls.Split('|').ElementAt(7).Trim() != "")
                            {
                                string AccountStatus = iwafl.AccStatusCodeMasters.Find(model.cbsdls.Split('|').ElementAt(7)).Description;
                                model.AccountStatus = AccountStatus != null ? AccountStatus : "";
                            }
                            else
                            {
                                model.AccountStatus = "";
                            }

                            if (model.cbsdls.Split('|').ElementAt(13).Trim() != "")
                            {
                                string AccountOwnership = iwafl.AccOwnershipCodeMasters.Find(model.cbsdls.Split('|').ElementAt(13).ToString()).Description;
                                model.AccountOwnership = AccountOwnership != null ? AccountOwnership : "";
                            }
                            else
                            {
                                model.AccountOwnership = "";
                            }

                            List<string> ar = new List<string>();
                            ar.Add(model.cbsdls.Split('|').ElementAt(3).ToString());
                            if (model.cbsdls.Split('|').ElementAt(14).Trim() != "")//model.JoinHldrs.Split('|').Count() - 1 remove on 05/05/2017
                            {
                                for (int i = 15; i < model.cbsdls.Split('|').Count(); i++)
                                {
                                    if (model.cbsdls.Split('|').ElementAt(i).ToString() != "")
                                        ar.Add(model.cbsdls.Split('|').ElementAt(i).ToString().Replace("PRIMARY-", " ").Replace("JOINT-", " "));

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
                //-------------------------------Added BY Abid on 28-08-2019------------------CR---For Pop----------------
                if (model.cbsdls.Split('|').ElementAt(1) == "S")
                {
                    SqlDataAdapter adp = new SqlDataAdapter("GetValidationForSchemeCode", con);
                    adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                    adp.SelectCommand.Parameters.Add("@SchemeCode", SqlDbType.NVarChar).Value = model.cbsdls.Split('|').ElementAt(4).ToString();
                    adp.SelectCommand.Parameters.Add("@Varificationlvl", SqlDbType.NVarChar).Value = "L1";
                    adp.SelectCommand.Parameters.Add("@OutInward", SqlDbType.NVarChar).Value = "IW";
                    //  adp.SelectCommand.Parameters.Add("@InstrumentType", SqlDbType.NVarChar).Value = instrntType;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        model.SchemeCode = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                        model.Message = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                        model.AllowToBlock = ds.Tables[0].Rows[0].ItemArray[2].ToString();
                        model.Amount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[3]);
                        model.popRqd = ds.Tables[0].Rows[0].ItemArray[4].ToString();
                        model.Color = ds.Tables[0].Rows[0].ItemArray[5].ToString();

                    }
                    //-------------------------------------------------------------END------------
                }

                return PartialView("_GetCBSDtls", model);
            }
            catch (Exception e)
            {
                ErrorDisplay er = new ErrorDisplay();
               // ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                er.ErrorMessage = e.InnerException.Message;

                return PartialView("Error", er);
                //return PartialView("Error", "Error");
            }
        }
        //---------------------------PositivePay Data------------------------//
        public ActionResult GetPositiveData(string AccountNo = null, string chequeNo = null)
        {
            //-------------------------------Added BY Abid on 07-11-2020------------------
            string OutputData = null;
            string date = null;
            string amount = null;
            string payee = null;
            string Finaldata = null;

            iwpro.GetPositivePayInfo(AccountNo, chequeNo, ref OutputData);
            if (OutputData != null && OutputData != "")
            {
                if (OutputData.Split('|').ElementAt(0) != "F")
                {
                    date = OutputData.Split('|').ElementAt(0).ToString();
                    amount = OutputData.Split('|').ElementAt(1).ToString();
                    payee = OutputData.Split('|').ElementAt(2).ToString();
                    date = Convert.ToDateTime(date).ToString("yyyy-MM-dd");

                    date = date.Split('-').ElementAt(2) + date.Split('-').ElementAt(1) + date.Split('-').ElementAt(0).Substring(2, 2);
                    Finaldata = OutputData.Split('|').ElementAt(1).ToString() + "|" + date + "|" + payee;
                }
                else
                    Finaldata = OutputData.ToString();
            }

            return Json(Finaldata, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getTiffImage(string httpwebimgpath = null)
        {
            try
            {


                string someUrl = httpwebimgpath;
                var webClient = new WebClient();

                byte[] imageBytes = webClient.DownloadData(someUrl);

                Stream streamactual = new MemoryStream(imageBytes);
                System.Drawing.Bitmap bmp = new Bitmap(streamactual);
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                stream.Position = 0;

                byte[] data = new byte[stream.Length];
                int lngth = (int)stream.Length;
                stream.Read(data, 0, lngth);
                stream.Close();

                string imageBase64Data = Convert.ToBase64String(data);
                Array.Clear(imageBytes, 0, imageBytes.Length);


                string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                ViewBag.ImageData = imageDataURL;
            }
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                //  logger.Log(LogLevel.Error, "HttpPost ChequeAccount|" + message + "INNEREXP| " + innerExcp, "Login Index-Msg");
                //logger.Log(LogLevel.Error, e.InnerException.StackTrace, "Login Index-Exception");
                //return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "IW L1 getTiffImage - " + innerExcp });
                // return Json("Error", JsonRequestBehavior.AllowGet);//PartialView("_getTiffImage");
            }

            return PartialView("_getTiffImage");
            //return Json(imageDataURL, JsonRequestBehavior.AllowGet);

        }
        public string replaceImgpath(string imgpath = null)
        {
            return imgpath = imgpath.Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]).Replace((string)Session["SrcWebIP1"], (string)Session["DestWepIP1"]).Replace((string)Session["SrcWebName1"], (string)Session["DestWebName1"]);
        }

        public JsonResult getAIDecision(Int64 ID = 0)
        {
            SqlDataAdapter SQLDA = new SqlDataAdapter("GetAIFinalDecision", con);
            SQLDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            SQLDA.SelectCommand.Parameters.AddWithValue("@ID", ID);
            SQLDA.SelectCommand.Parameters.AddWithValue("@PROCESSINGDATE", Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"));

            DataSet ds = new DataSet();
            SQLDA.Fill(ds);
            string AIDecision = null;
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0].ItemArray[0] != null && ds.Tables[0].Rows[0].ItemArray[0] != "")
                {
                    AIDecision = Convert.ToString(ds.Tables[0].Rows[0].ItemArray[0]);
                }

            }
            return Json(AIDecision, JsonRequestBehavior.AllowGet);
            // return PartialView("_IWActivitylogs", model);

        }
        public JsonResult getAIRejectDecrp(Int64 ID = 0)
        {
            SqlDataAdapter SQLDA = new SqlDataAdapter("GetAIRejectDescrp", con);
            SQLDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            SQLDA.SelectCommand.Parameters.AddWithValue("@ID", ID);
            SQLDA.SelectCommand.Parameters.AddWithValue("@PROCESSINGDATE", Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"));

            DataSet ds = new DataSet();
            SQLDA.Fill(ds);
            string AIRejectDesp = null;
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0].ItemArray[0] != null && ds.Tables[0].Rows[0].ItemArray[0] != "")
                    AIRejectDesp = Convert.ToString(ds.Tables[0].Rows[0].ItemArray[0]);
                if (ds.Tables[0].Rows[0].ItemArray[1] != null && ds.Tables[0].Rows[0].ItemArray[1] != "")
                    AIRejectDesp = AIRejectDesp + " " + Convert.ToString(ds.Tables[0].Rows[0].ItemArray[1]);
                if (ds.Tables[0].Rows[0].ItemArray[2] != null && ds.Tables[0].Rows[0].ItemArray[2] != "")
                    AIRejectDesp = AIRejectDesp + " " + Convert.ToString(ds.Tables[0].Rows[0].ItemArray[2]);
                if (ds.Tables[0].Rows[0].ItemArray[3] != null && ds.Tables[0].Rows[0].ItemArray[3] != "")
                    AIRejectDesp = AIRejectDesp + " " + Convert.ToString(ds.Tables[0].Rows[0].ItemArray[3]);
                if (ds.Tables[0].Rows[0].ItemArray[4] != null && ds.Tables[0].Rows[0].ItemArray[4] != "")
                    AIRejectDesp = AIRejectDesp + " " + Convert.ToString(ds.Tables[0].Rows[0].ItemArray[4]);

            }
            return Json(AIRejectDesp, JsonRequestBehavior.AllowGet);
            // return Json();
        }

       
    }
}
