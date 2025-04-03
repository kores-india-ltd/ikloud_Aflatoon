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
            ViewBag.ClearingType = new SelectList(iwafl.ClearingType, "Code", "Name").ToList();

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
        public ActionResult Index(string clrtype = null)
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

                SqlDataAdapter adp = new SqlDataAdapter("IWL1Verification", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@procDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                adp.SelectCommand.Parameters.Add("@ClrType", SqlDbType.NVarChar).Value = clrtype;
                //--------------------Customer Selection---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);


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
                        ActualAmount = 0,
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
                        FrontGreyImagePath = ds.Tables[0].Rows[0].ItemArray[13].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[14].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                        CBSClientAccountDtls = ds.Tables[0].Rows[0].ItemArray[15].ToString(),
                        CBSJointHoldersName = ds.Tables[0].Rows[0].ItemArray[16].ToString(),
                        BackTiffImagePath = ds.Tables[0].Rows[0].ItemArray[17].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
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
                            ActualAmount = 0,
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
                            FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[13].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[14].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                            CBSClientAccountDtls = ds.Tables[0].Rows[index].ItemArray[15].ToString(),
                            CBSJointHoldersName = ds.Tables[0].Rows[index].ItemArray[16].ToString(),
                            BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[17].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                        };
                        //ViewBag.cnt = true;
                        objectlst.Add(def);
                        count = count - 1;
                        index = index + 1;
                    }
                    //var rtnlist = iwafl.ItemReturnReasons.Select(m => m.RETURN_REASON_CODE).ToList();
                    //ViewBag.rtnlist = rtnlist;

                    var rtnlist = (from i in iwafl.ItemReturnReasons select i).ToList();// iwafl.ItemReturnReasons.Select(m).ToList();
                    ViewBag.rtnlist = rtnlist.Select(m => m.RETURN_REASON_CODE).ToList();
                    ViewBag.rtnlistDescrp = rtnlist.Select(m => m.DESCRIPTION).ToList();
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
                    ttcnt = lst.Count() / 17;

                int stu;
                string resncode = "0";
                string rejctdecrptn = null;
                string cbdclnts = "";
                string cbdJointdtls = "";

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

                    Int64 id = Convert.ToInt64(lst[0]);
                    iwpro.UpdateIWL1Verification(id, uid, lst[4].ToString(), lst[5].ToString(), lst[6].ToString(), lst[7].ToString(), lst[1].ToString(), Convert.ToDouble(lst[2]), lst[3].ToString(), stu, resncode, rejctdecrptn, cbdclnts, cbdJointdtls, @Session["LoginID"].ToString());
                    for (int k = 0; k < idlst.Count; k++)
                    {
                        if (idlst[k] == id)
                            idlst.RemoveAt(k);
                    }
                    lst.RemoveRange(0, 17);
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
                        iwpro.UnlockRecords(idlst[p], "L1", 0, null, null, 0);
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
                adp.SelectCommand.Parameters.Add("@ClrType", SqlDbType.NVarChar).Value = Session["clrtype"];
                //--------------------Customer Selection---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);


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

                            Int64 id = Convert.ToInt64(lst[0]);
                            iwpro.UpdateIWL1Verification(id, uid, lst[4].ToString(), lst[5].ToString(), lst[6].ToString(), lst[7].ToString(), lst[1].ToString(), Convert.ToDouble(lst[2]), lst[3].ToString(), stu, resncode, rejctdecrptn, cbdclnts, cbdJointdtls, @Session["LoginID"].ToString());
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
                        CBSJointHoldersName = cbdJointdtls
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
                                ActualAmount = 0,
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
                                FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[13].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                                FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[14].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                                CBSClientAccountDtls = ds.Tables[0].Rows[index].ItemArray[15].ToString(),
                                CBSJointHoldersName = ds.Tables[0].Rows[index].ItemArray[16].ToString(),
                                BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[17].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
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

            var rjrs = (from r in iwafl.ItemReturnReasons
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
                        if (ac.Length == 6)
                        {
                            //model = (from c in iwafl.ACDetails
                            //         where c.Cbsdtls.Contains(ac)
                            //         select new cbstetails
                            //         {
                            //             cbsdls = c.Cbsdtls,
                            //             JoinHldrs = c.JoinHldrs,
                            //             Account = c.Ac
                            //         }
                            //).SingleOrDefault();

                            try
                            {
                                var model1 = (from c in iwafl.ACDetails
                                              where c.Cbsdtls.Contains(ac)
                                              select c
                                              ).ToList();

                                if (model1.Count > 1)                                
                                    model = null;
                               
                                else
                                {
                                    model = (from c in iwafl.ACDetails
                                             where c.Cbsdtls.Contains(ac)
                                             select new cbstetails
                                             {
                                                 cbsdls = c.Cbsdtls,
                                                 JoinHldrs = c.JoinHldrs,
                                                 Account = c.Ac
                                             }
                                           ).FirstOrDefault();
                                }

                            }
                            catch (Exception ex)
                            {
                                model = null;

                            }





                        }
                        else
                        {
                            //model = (from c in iwafl.ACDetails
                            //         where c.Ac == ac
                            //         select new cbstetails
                            //         {
                            //             cbsdls = c.Cbsdtls,
                            //             JoinHldrs = c.JoinHldrs,
                            //         }
                            //).SingleOrDefault();

                            try
                            {
                                model = (from c in iwafl.ACDetails
                                         where c.Ac == ac
                                         select new cbstetails
                                         {
                                             cbsdls = c.Cbsdtls,
                                             JoinHldrs = c.JoinHldrs,
                                         }
                                                         ).First();
                            }
                            catch (Exception ex)
                            {
                                model = null;

                            }


                        }

                    }
                    else if (Session["GetAccountDetails "].ToString().ToUpper() == "C")
                    {
                        //---------For CBS Bank----------------

                        iwpro.GetOnlyIWUpdateCBSAccInfo(ac, 1, ref strcbsdls, ref strJoinHldrs);
                        model.cbsdls = strcbsdls;
                        model.JoinHldrs = strJoinHldrs;
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
                er.ErrorMessage = e.InnerException.Message;
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
        //public ActionResult RejectReason(int? page, int mid = 0, string rtnmodule = null)
        //{
        //    if (Session["uid"] == null)
        //    {
        //        return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
        //    }
        //    ViewBag.rtnmodule = rtnmodule;
        //    int pageSize = 100;
        //    int pageNumber = (page ?? 1);
        //    var rjrs = (from r in iwafl.ItemReturnReasons
        //                select new RejectReason
        //                {
        //                    Description = r.DESCRIPTION,
        //                    ReasonCodeS = r.RETURN_REASON_CODE
        //                });
        //    ViewBag.mid = mid;

        //    return PartialView("_RejectReason",rjrs.ToList().ToPagedList(pageNumber, pageSize));
        //}

    }
}
