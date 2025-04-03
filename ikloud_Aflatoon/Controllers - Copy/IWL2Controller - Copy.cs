using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ikloud_Aflatoon.Infrastructure;
using System.Collections;
using ikloud_Aflatoon.Models;

namespace ikloud_Aflatoon.Controllers
{
    public class IWL2Controller : Controller
    {
        //
        // GET: /L2Verification/
        AflatoonEntities iwafl = new AflatoonEntities();
        IWProcDataContext iwpro = new IWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);

        public ActionResult Selection()
        {
            ViewBag.ClearingType = new SelectList(iwafl.ClearingType, "Code", "Name").ToList();
            @Session["glob"] = true;

            return View();
        }
        [HttpPost]
        public ActionResult Selection(L2Helper lHelpr)
        {

            string clrtype = Request.Form["ClearingType"];
            Session["clrtype"] = clrtype;
            lHelpr.Clrtype = clrtype;
            @Session["glob"] = true;

            return RedirectToAction("Index", new { clrtype = lHelpr.Clrtype });
        }

        public ActionResult Index(string clrtype = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            if ((bool)Session["VF"] == false)
            {
                UserMaster usrm = iwafl.UserMasters.Find(uid);
                usrm.Active = false;
                iwafl.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            try
            {

                SqlDataAdapter adp = new SqlDataAdapter("IWL2Verification", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@procDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                adp.SelectCommand.Parameters.Add("@ClrType", SqlDbType.NVarChar).Value = Session["CtsSessionType"].ToString();
                //--------------------Customer Selection---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);


                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<L2Helper>();
                L2Helper def;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    def = new L2Helper
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
                        //EntrySerialNo = ds.Tables[0].Rows[0].ItemArray[9].ToString(),
                        //EntryPayorBankRoutNo = ds.Tables[0].Rows[0].ItemArray[10].ToString(),
                        //EntrySAN = ds.Tables[0].Rows[0].ItemArray[11].ToString(),
                        //EntryTransCode = ds.Tables[0].Rows[0].ItemArray[12].ToString(),
                        RejectReason = ds.Tables[0].Rows[0].ItemArray[13].ToString(),//Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[13]),
                        RejectDescription = ds.Tables[0].Rows[0].ItemArray[14].ToString(),
                        //L1By = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[15]),
                        ReturnReasonDescription = ds.Tables[0].Rows[0].ItemArray[16].ToString(),

                        FrontGreyImagePath = ds.Tables[0].Rows[0].ItemArray[17].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[18].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                        CBSClientAccountDtls = ds.Tables[0].Rows[0].ItemArray[19].ToString(),
                        CBSJointHoldersName = ds.Tables[0].Rows[0].ItemArray[20].ToString(),
                        L1Status = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[21]),
                        PresentingBankRoutNo = ds.Tables[0].Rows[0].ItemArray[22].ToString(),
                        DocType = ds.Tables[0].Rows[0].ItemArray[23].ToString(),
                        XMLMICRRepairFlags = ds.Tables[0].Rows[0].ItemArray[24].ToString(),
                        BackTiffImagePath = ds.Tables[0].Rows[0].ItemArray[25].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                        OpsStatus = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[26].ToString()),
                        DbtAccountNoOld = ds.Tables[0].Rows[0].ItemArray[3].ToString(),
                        DateOld = ds.Tables[0].Rows[0].ItemArray[4].ToString(),


                        // L1Status = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[22]),
                    };
                    objectlst.Add(def);
                    //------------------------END------------------------//
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        def = new L2Helper
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
                            //EntrySerialNo = ds.Tables[0].Rows[index].ItemArray[9].ToString(),
                            //EntryPayorBankRoutNo = ds.Tables[0].Rows[index].ItemArray[10].ToString(),
                            //EntrySAN = ds.Tables[0].Rows[index].ItemArray[11].ToString(),
                            //EntryTransCode = ds.Tables[0].Rows[index].ItemArray[12].ToString(),
                            RejectReason = ds.Tables[0].Rows[index].ItemArray[13].ToString(),//Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[13]),
                            RejectDescription = ds.Tables[0].Rows[index].ItemArray[14].ToString(),
                            //L1By = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[15]),
                            ReturnReasonDescription = ds.Tables[0].Rows[index].ItemArray[16].ToString(),
                            FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[17].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[18].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                            CBSClientAccountDtls = ds.Tables[0].Rows[index].ItemArray[19].ToString(),
                            CBSJointHoldersName = ds.Tables[0].Rows[index].ItemArray[20].ToString(),
                            L1Status = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[21]),
                            PresentingBankRoutNo = ds.Tables[0].Rows[index].ItemArray[22].ToString(),
                            DocType = ds.Tables[0].Rows[index].ItemArray[23].ToString(),
                            XMLMICRRepairFlags = ds.Tables[0].Rows[index].ItemArray[24].ToString(),
                            BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[25].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                            OpsStatus = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[26].ToString()),
                            DbtAccountNoOld = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                            DateOld = ds.Tables[0].Rows[index].ItemArray[4].ToString(),
                        };
                        //ViewBag.cnt = true;
                        objectlst.Add(def);
                        count = count - 1;
                        index = index + 1;
                    }
                    var rtnlist = (from i in iwafl.ItemReturnReasons select i).ToList();// iwafl.ItemReturnReasons.Select(m).ToList();
                    ViewBag.rtnlist = rtnlist.Select(m => m.RETURN_REASON_CODE).ToList();
                    ViewBag.rtnlistDescrp = rtnlist.Select(m => m.DESCRIPTION).ToList();
                    ViewBag.cnt = true;
                    Session["glob"] = null;
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
        public ActionResult IWl2(List<string> lst, bool snd, string img, string btnClose = "default", List<Int64> idlst = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            if ((bool)Session["VF"] == false)
            {

                UserMaster usrm = iwafl.UserMasters.Find(uid);
                usrm.Active = false;
                iwafl.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

           
            int ttcnt = 0;
            try
            {
                if (lst != null)
                    ttcnt = lst.Count() / 21;
                int stu;
                string resncode = "0";
                string resncodeL2 = "0";
                string rejctdecrptn = null;
                Int64 id = 0;
                string finaldate = null;
                string decsn = null;
                string cbsclientdtls = "";
                string cbsJointDtls = "";
                if (btnClose == "Close" && lst != null)
                    ttcnt = ttcnt + 1;
                
                //if (ttcnt == 1)
                //{
                //    for (int i = 0; i < ttcnt; i++)
                //    {
                //        // jt = new IWAmountTmpProcess();

                //        if (lst[11] != null)
                //            if (lst[11].ToString() != "")
                //                resncode = lst[11].ToString();
                //            else
                //                resncode = "0";
                //        //-------------------For Modification-----------------16-03-2017---------------
                //        decsn = lst[3].ToString();//Dicision
                //        if (decsn == "A")
                //        {
                //            if (lst[1].ToString() != lst[15].ToString())
                //                decsn = "AC";
                //            else if (lst[2].ToString() != lst[16].ToString())
                //            {
                //                if (decsn == "AC")
                //                    decsn = "M";
                //                else
                //                    decsn = "D";
                //            }
                //        }
                //        //----------------------------------------------------
                //        if (lst[2] != null)
                //        {
                //            if (lst[2].ToString().Length != 10)
                //                finaldate = "20" + lst[2].ToString().Substring(4, 2) + "-" + lst[2].ToString().Substring(2, 2) + "-" + lst[2].ToString().Substring(0, 2);
                //            else
                //                finaldate = lst[2].ToString();
                //        }
                //        //----------------- Added On 19-05-2017------------------//
                //        if (lst[12] != null)//CBSClient
                //            cbsclientdtls = lst[12].ToString();
                //        else
                //            cbsclientdtls = "Not Found";
                //        if (lst[13] != null)//CBSJointDtls
                //            cbsJointDtls = lst[13].ToString();
                //        else
                //            cbsJointDtls = "Not Found";
                //        //--------------------------------------------------------//

                //        id = Convert.ToInt64(lst[0]);
                //        iwpro.UpdateIWL2Verification(id, uid, lst[1].ToString(), finaldate, Convert.ToInt16(lst[4]), resncode, null, null, cbsclientdtls, cbsJointDtls, Convert.ToDouble(lst[5]), Convert.ToDouble(Session["RVERFNHIGHAMT"]), decsn, Session["LoginID"].ToString(), Convert.ToInt16(lst[14]));
                //        //lst.RemoveRange(0, 14);
                //        for (int k = 0; k < idlst.Count; k++)
                //        {
                //            if (idlst[k] == id)
                //                idlst.RemoveAt(k);
                //        }
                //    }
                //}
                //else
                //{
                    for (int i = 0; i < ttcnt - 1; i++)
                    {
                        // jt = new IWAmountTmpProcess();
                        if (lst[11] != null)
                            if (lst[11].ToString() != "")
                                resncode = lst[11].ToString();
                            else
                                resncode = "0";
                        //-------------------For Modification-----------------16-03-2017---------------
                        decsn = lst[3].ToString();//Dicision
                        if (decsn == "A")
                        {
                            if (lst[1].ToString() != lst[15].ToString())
                                decsn = "AC";
                            else if (lst[2].ToString() != lst[16].ToString())
                            {
                                if (decsn == "AC")
                                    decsn = "M";
                                else
                                    decsn = "D";
                            }
                        }
                        //----------------------------------------------------
                        if (lst[2] != null)
                        {
                            if (lst[2].ToString().Length != 10)
                                finaldate = "20" + lst[2].ToString().Substring(4, 2) + "-" + lst[2].ToString().Substring(2, 2) + "-" + lst[2].ToString().Substring(0, 2);
                            else
                                finaldate = lst[2].ToString();
                        }
                        //----------------- Added On 19-05-2017------------------//
                        if (lst[12] != null)//CBSClient
                            cbsclientdtls = lst[12].ToString();
                        else
                            cbsclientdtls = "Not Found";
                        if (lst[13] != null)//CBSJointDtls
                            cbsJointDtls = lst[13].ToString();
                        else
                            cbsJointDtls = "Not Found";
                        //--------------------------------------------------------//

                        id = Convert.ToInt64(lst[0]);
                        iwpro.UpdateIWL2Verification(id, uid, lst[1].ToString(), finaldate, Convert.ToInt16(lst[4]), resncode, null, null, cbsclientdtls, cbsJointDtls, Convert.ToDouble(lst[5]), Convert.ToDouble(Session["RVERFNHIGHAMT"]), decsn, @Session["LoginID"].ToString(), Convert.ToInt16(lst[14]));

                        for (int k = 0; k < idlst.Count; k++)
                        {
                            if (idlst[k] == id)
                                idlst.RemoveAt(k);
                        }
                        lst.RemoveRange(0, 21);
                    }
               // }
                
                //---------------------------IF Close button called--------------------------------//
                if (btnClose == "Close")
                {
                    @Session["glob"] = true;
                    for (int p = 0; p < idlst.Count; p++)
                    {
                        iwpro.UnlockRecords(idlst[p], "L2", 0, null, null, 0);
                    }
                    return Json(false);
                }

                //------------------Select next Pending Record------------------
                //------------------------------Calling Proc for selecting data-----------------------

                // Procommand = new System.Data.Sql.("{ call SelectForDwld('"+ date.ToString("yyyy-MM-dd")+"')}",con);

                SqlDataAdapter adp = new SqlDataAdapter("IWL2Verification", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = (int)Session["uid"];
                adp.SelectCommand.Parameters.Add("@procDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                adp.SelectCommand.Parameters.Add("@ClrType", SqlDbType.NVarChar).Value = Session["CtsSessionType"].ToString();
                //--------------------Customer Selection---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);


                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<L2Helper>();
                L2Helper def;
                ArrayList ids = new ArrayList();
                bool checkid = false;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //--------------Checking Last record and list record are same------
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        if (Convert.ToInt64(lst[0]) == Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]))
                        {

                            if (lst[11] != null)
                                if (lst[11].ToString() != "")
                                    resncode = lst[11].ToString();
                                else
                                    resncode = "0";
                            //-------------------For Modification-----------------16-03-2017---------------
                            decsn = lst[3].ToString();//Dicision
                            if (decsn == "A")
                            {
                                if (lst[1].ToString() != lst[15].ToString())
                                    decsn = "AC";
                                else if (lst[2].ToString() != lst[16].ToString())
                                {
                                    if (decsn == "AC")
                                        decsn = "M";
                                    else
                                        decsn = "D";
                                }
                            }
                            //----------------------------------------------------

                            if (lst[2] != null)
                            {
                                if (lst[2].ToString().Length != 10)
                                    finaldate = "20" + lst[2].ToString().Substring(4, 2) + "-" + lst[2].ToString().Substring(2, 2) + "-" + lst[2].ToString().Substring(0, 2);
                                else
                                    finaldate = lst[2].ToString();
                            }
                            //----------------- Added On 19-05-2017------------------//
                            if (lst[12] != null)//CBSClient
                                cbsclientdtls = lst[12].ToString();
                            else
                                cbsclientdtls = "Not Found";
                            if (lst[13] != null)//CBSJointDtls
                                cbsJointDtls = lst[13].ToString();
                            else
                                cbsJointDtls = "Not Found";
                            //--------------------------------------------------------//

                            id = Convert.ToInt64(lst[0]);
                            iwpro.UpdateIWL2Verification(id, uid, lst[1].ToString(), finaldate, Convert.ToInt16(lst[4]), resncode, null, null, cbsclientdtls, cbsJointDtls, Convert.ToDouble(lst[5]), Convert.ToDouble(Session["RVERFNHIGHAMT"]), decsn, @Session["LoginID"].ToString(), Convert.ToInt16(lst[14]));
                            goto Notfnd;
                        }

                    }
                    //---------------Adding last record of list-----------------//
                    if (lst[10] != null)
                        if (lst[10].ToString() != "")
                            resncode = lst[10].ToString();
                        else
                            resncode = "0";
                    if (lst[11] != null)
                        if (lst[11].ToString() != "")
                            resncodeL2 = lst[11];
                        else
                            resncodeL2 = "0";

                    //----------------- Added On 19-05-2017------------------//
                    if (lst[12] != null)//CBSClient
                        cbsclientdtls = lst[12].ToString();
                    else
                        cbsclientdtls = "Not Found";
                    if (lst[13] != null)//CBSJointDtls
                        cbsJointDtls = lst[13].ToString();
                    else
                        cbsJointDtls = "Not Found";
                    //--------------------------------------------------------//

                    def = new L2Helper
                    {
                        ID = Convert.ToInt64(lst[0]),
                        FrontGreyImagePath = img,
                        DbtAccountNo = lst[1].ToString(),
                        Date = lst[2].ToString(),
                        L2Opsts = lst[3].ToString(),
                        OpsStatus = Convert.ToInt32(lst[4]),
                        //EntrySerialNo = lst[4].ToString(),
                        //EntryPayorBankRoutNo = lst[5].ToString(),
                        //EntrySAN = lst[6].ToString(),
                        //EntryTransCode = lst[7].ToString(),
                        XMLAmount = Convert.ToDecimal(lst[5]),
                        XMLSerialNo = lst[6].ToString(),
                        XMLPayorBankRoutNo = lst[7].ToString(),
                        XMLSAN = lst[8].ToString(),
                        XMLTransCode = lst[9].ToString(),
                        RejectReason = resncode,
                        L2Rejectreason = resncodeL2,
                        //ReturnReasonDescription=lst[0].ToString(),
                        CBSClientAccountDtls = cbsclientdtls,//lst[12].ToString(),
                        CBSJointHoldersName = cbsJointDtls,//lst[13].ToString(),
                        L1Status = Convert.ToInt32(lst[14]),
                        DbtAccountNoOld = lst[15].ToString(),
                        DateOld = lst[16].ToString(),

                        PresentingBankRoutNo = lst[17].ToString(),
                        DocType = lst[18].ToString(),
                        XMLMICRRepairFlags = lst[19].ToString(),

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
                            def = new L2Helper
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
                                //EntrySerialNo = ds.Tables[0].Rows[index].ItemArray[9].ToString(),
                                //EntryPayorBankRoutNo = ds.Tables[0].Rows[index].ItemArray[10].ToString(),
                                //EntrySAN = ds.Tables[0].Rows[index].ItemArray[11].ToString(),
                                //EntryTransCode = ds.Tables[0].Rows[index].ItemArray[12].ToString(),
                                RejectReason = ds.Tables[0].Rows[index].ItemArray[13].ToString(),// Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[13]),
                                RejectDescription = ds.Tables[0].Rows[index].ItemArray[14].ToString(),
                                //L1By = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[15]),
                                ReturnReasonDescription = ds.Tables[0].Rows[index].ItemArray[16].ToString(),
                                FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[17].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                                FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[18].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                                CBSClientAccountDtls = ds.Tables[0].Rows[index].ItemArray[19].ToString(),
                                CBSJointHoldersName = ds.Tables[0].Rows[index].ItemArray[20].ToString(),
                                L1Status = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[21]),
                                PresentingBankRoutNo = ds.Tables[0].Rows[index].ItemArray[22].ToString(),
                                DocType = ds.Tables[0].Rows[index].ItemArray[23].ToString(),
                                XMLMICRRepairFlags = ds.Tables[0].Rows[index].ItemArray[24].ToString(),
                                BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[25].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                                OpsStatus = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[26].ToString()),
                                DbtAccountNoOld = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                                DateOld = ds.Tables[0].Rows[index].ItemArray[4].ToString(),
                            };
                            //ids.Add(def.ID);
                            objectlst.Add(def);
                        }
                        checkid = false;

                        //ViewBag.cnt = true;

                        count = count - 1;
                        index = index + 1;
                    }

                    ViewBag.cnt = true;
                    return Json(objectlst);
                }
                else
                    ViewBag.cnt = false;
            Notfnd:
                ViewBag.cnt = false;
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

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "IWL2 HttpPost Index- " + innerExcp });
                // return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }

        }
        //-------------------------------GetCBSDtls------------------
        [HttpPost]
        public PartialViewResult GetCBSDtls(string ac = null, string strcbsdls = null, string strJoinHldrs = null)
        {
            cbstetails model = new cbstetails();
            try
            {
                if (ac != null && (strcbsdls == null || strcbsdls == ""))
                {

                    //var model 
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
                        //------------------------For CBS Bank-----------
                        iwpro.GetOnlyIWUpdateCBSAccInfo(ac, 1, ref strcbsdls, ref strJoinHldrs);
                        model.cbsdls = strcbsdls;
                        model.JoinHldrs = strJoinHldrs;
                        //--------------------------------

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
                    return PartialView("_L2GetCBSDtls", model);
                }
                else
                {
                    //cbstetails model = new cbstetails();
                    model.cbsdls = strcbsdls;
                    model.JoinHldrs = strJoinHldrs;
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
                    return PartialView("_L2GetCBSDtls", model);
                }
            }
            catch (Exception e)
            {

                ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                er.ErrorMessage = e.InnerException.Message;

                return PartialView("Error", er);
                // return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }

        }
        public PartialViewResult GetBankName(string bankcode = null)
        {
            var Banks = (from c in iwafl.Banks
                         where c.BankCode == bankcode
                         select new { c.BankName }).SingleOrDefault();
            if (Banks != null)
                ViewBag.BankName = Banks.BankName;
            else
                ViewBag.BankName = null;

            return PartialView("_Bankname");
        }
        public PartialViewResult GetClientDlts(string ac = null)
        {
            var customer = (from c in iwafl.CMS_CustomerMaster
                            where c.Customer_Code == ac
                            select new { c.Customer_Name }).SingleOrDefault();
            if (customer != null)
                ViewBag.customer = customer.Customer_Name;
            else
                ViewBag.customer = null;

            return PartialView("GetClientDlts");
        }
        public PartialViewResult RejectReason(int id = 0)
        {

            var rjrs = (from r in iwafl.ItemReturnReasons
                        select new RejectReason
                        {
                            Description = r.DESCRIPTION,
                            ReasonCodeS = r.RETURN_REASON_CODE
                        });
            return PartialView("_L2RejectReason", rjrs);
        }
        public PartialViewResult getIwlogs(int id)
        {
            var model = iwafl.IWActivityLogs.Where(l => l.IWMainTrID == id).OrderBy(l => l.Timestamp).ToList();
            return PartialView("_IWActivitylogs", model);
        }
    }
}
