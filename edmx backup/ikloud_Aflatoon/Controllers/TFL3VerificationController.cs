using ikloud_Aflatoon.Infrastructure;
using ikloud_Aflatoon.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ikloud_Aflatoon.Controllers
{
    public class TFL3VerificationController : Controller
    {
        //
        // GET: /TFL3Verification/

        AflatoonEntities iwafl = new AflatoonEntities();
        IWProcDataContext iwpro = new IWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);

        public ActionResult Selection(int id = 0)
        {
            ViewBag.ClearingType = new SelectList(iwafl.ClearingType, "Code", "Name").ToList();
            @Session["glob"] = true;
            if (id == 1)
                ViewBag.notfound = "No Data Found!!..";

            return View();
        }
        [HttpPost]
        public ActionResult Selection(L2Helper lHelpr)
        {
            string vftype = Request.Form["rpttype"];
            string clrtype = Request.Form["ClearingType"];
            if (vftype == "Rejected Cheques")
                vftype = "R";
            else
                vftype = "Y";
            Session["vftype"] = vftype;
            Session["clrtype"] = clrtype;
            lHelpr.Clrtype = clrtype;
            lHelpr.Vftype = vftype;
            @Session["glob"] = true;

            return RedirectToAction("Index", new { clrtype = lHelpr.Clrtype, vftype = lHelpr.Vftype });
        }

        public ActionResult Index(string clrtype = null, string vftype = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            if ((bool)Session["RVF"] == false)
            {

                UserMaster usrm = iwafl.UserMasters.Find(uid);
                usrm.Active = false;
                iwafl.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            try
            {
                SqlDataAdapter adp = new SqlDataAdapter("TFL3Verification", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@procDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                adp.SelectCommand.Parameters.Add("@VFType", SqlDbType.NVarChar).Value = vftype;
                adp.SelectCommand.Parameters.Add("@ClrType", SqlDbType.NVarChar).Value = clrtype;
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
                        //ActualAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[2]),
                        DbtAccountNo = ds.Tables[0].Rows[0].ItemArray[2].ToString(),
                        Date = ds.Tables[0].Rows[0].ItemArray[3].ToString(),
                        XMLSerialNo = ds.Tables[0].Rows[0].ItemArray[4].ToString(),
                        XMLPayorBankRoutNo = ds.Tables[0].Rows[0].ItemArray[5].ToString(),
                        XMLSAN = ds.Tables[0].Rows[0].ItemArray[6].ToString(),
                        XMLTransCode = ds.Tables[0].Rows[0].ItemArray[7].ToString(),
                        //EntrySerialNo = ds.Tables[0].Rows[0].ItemArray[9].ToString(),
                        //EntryPayorBankRoutNo = ds.Tables[0].Rows[0].ItemArray[10].ToString(),
                        //EntrySAN = ds.Tables[0].Rows[0].ItemArray[11].ToString(),
                        //EntryTransCode = ds.Tables[0].Rows[0].ItemArray[12].ToString(),
                        RejectReason = ds.Tables[0].Rows[0].ItemArray[8].ToString(),// Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[13]),
                        // RejectDescription = ds.Tables[0].Rows[0].ItemArray[9].ToString(),
                        ReturnReasonDescription = ds.Tables[0].Rows[0].ItemArray[9].ToString(),
                        L2By = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[11]),
                        FrontGreyImagePath = ds.Tables[0].Rows[0].ItemArray[12].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[13].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                        CBSClientAccountDtls = ds.Tables[0].Rows[0].ItemArray[14].ToString(),
                        CBSJointHoldersName = ds.Tables[0].Rows[0].ItemArray[15].ToString(),
                        OpsStatus = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[16]),
                        DbtAccountNoOld = ds.Tables[0].Rows[0].ItemArray[2].ToString(),
                        DateOld = ds.Tables[0].Rows[0].ItemArray[3].ToString(),
                        L1Status = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[17]),
                        L2Status = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[18]),
                        PresentingBankRoutNo = ds.Tables[0].Rows[0].ItemArray[19].ToString(),
                        DocType = ds.Tables[0].Rows[0].ItemArray[20].ToString(),
                        XMLMICRRepairFlags = ds.Tables[0].Rows[0].ItemArray[21].ToString(),
                        BackTiffImagePath = ds.Tables[0].Rows[0].ItemArray[22].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                    };
                    //def.Vftype = vftype;
                    //def.Clrtype = clrtype;
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
                            //ActualAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[2]),
                            DbtAccountNo = ds.Tables[0].Rows[index].ItemArray[2].ToString(),
                            Date = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                            XMLSerialNo = ds.Tables[0].Rows[index].ItemArray[4].ToString(),
                            XMLPayorBankRoutNo = ds.Tables[0].Rows[index].ItemArray[5].ToString(),
                            XMLSAN = ds.Tables[0].Rows[index].ItemArray[6].ToString(),
                            XMLTransCode = ds.Tables[0].Rows[index].ItemArray[7].ToString(),
                            //EntrySerialNo = ds.Tables[0].Rows[0].ItemArray[9].ToString(),
                            //EntryPayorBankRoutNo = ds.Tables[0].Rows[0].ItemArray[10].ToString(),
                            //EntrySAN = ds.Tables[0].Rows[0].ItemArray[11].ToString(),
                            //EntryTransCode = ds.Tables[0].Rows[0].ItemArray[12].ToString(),
                            RejectReason = ds.Tables[0].Rows[index].ItemArray[8].ToString(),// Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[13]),
                            // RejectDescription = ds.Tables[0].Rows[0].ItemArray[9].ToString(),
                            ReturnReasonDescription = ds.Tables[0].Rows[index].ItemArray[9].ToString(),
                            L2By = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[11]),
                            FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[12].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[13].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                            CBSClientAccountDtls = ds.Tables[0].Rows[index].ItemArray[14].ToString(),
                            CBSJointHoldersName = ds.Tables[0].Rows[index].ItemArray[15].ToString(),
                            OpsStatus = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[16]),
                            DbtAccountNoOld = ds.Tables[0].Rows[index].ItemArray[2].ToString(),
                            DateOld = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                            L1Status = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[17]),
                            L2Status = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[18]),
                            PresentingBankRoutNo = ds.Tables[0].Rows[index].ItemArray[19].ToString(),
                            DocType = ds.Tables[0].Rows[index].ItemArray[20].ToString(),
                            XMLMICRRepairFlags = ds.Tables[0].Rows[index].ItemArray[21].ToString(),
                            BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[22].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                        };
                        //ViewBag.cnt = true;
                        //def.Vftype = vftype;
                        //def.Clrtype = clrtype;
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
                    return RedirectToAction("Selection", new { id = 1 });
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

        [HttpPost]
        public ActionResult TFl3(List<string> lst, bool snd, string img, string btnClose = "default", List<Int64> idlst = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            if ((bool)Session["RVF"] == false)
            {

                UserMaster usrm = iwafl.UserMasters.Find(uid);
                usrm.Active = false;
                iwafl.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            //IWAmountTmpProcess jt;
            // int uid = (int)Session["uid"];//That will be Session value.
            int ttcnt = 0;
            try
            {

                if (lst != null)
                    ttcnt = lst.Count() / 21;
                int stu;
                string resncode = "0";
                string resncodeL2 = "0";
                string rejctdecrptn = null;
                string decsn = null;
                string cbsclientdtls = "";
                string cbsJointDtls = "";
                // string IWDicision = Request.Form["IWDecision"].ToUpper();
                Int64 id = 0;
                string finaldate = null;
                //   resncode = '0';
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

                //        id = Convert.ToInt64(lst[0]);
                //        decsn = lst[3].ToString();//Dicision
                //        if (decsn == "A")
                //        {
                //            if (lst[1].ToString() != lst[14].ToString())
                //                decsn = "AC";
                //            if (lst[2].ToString() != lst[15].ToString())
                //            {
                //                if (decsn == "AC")
                //                    decsn = "M";
                //                else
                //                    decsn = "D";
                //            }

                //        }
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

                //        iwpro.UpdateIWL3Verification(id, uid, lst[1].ToString(), finaldate, Convert.ToInt16(lst[4]), resncode, null, null, cbsclientdtls, cbsJointDtls, Convert.ToDouble(lst[5]), Convert.ToDouble(Session["RVERFNHIGHAMT"]), decsn, @Session["LoginID"].ToString(), Convert.ToInt16(lst[16]), Convert.ToInt16(lst[17]));
                //        //lst.RemoveRange(0, 16);
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

                    id = Convert.ToInt64(lst[0]);
                    decsn = lst[3].ToString();//Dicision
                    if (decsn == "A")
                    {
                        if (lst[1].ToString() != lst[14].ToString())
                            decsn = "AC";
                        if (lst[2].ToString() != lst[15].ToString())
                        {
                            if (decsn == "AC")
                                decsn = "M";
                            else
                                decsn = "D";
                        }
                    }
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

                    iwpro.UpdateTFL3Verification(id, uid, lst[1].ToString(), finaldate, Convert.ToInt16(lst[4]), resncode, null, null, cbsclientdtls, cbsJointDtls, Convert.ToDouble(lst[5]), Convert.ToDouble(Session["RVERFNHIGHAMT"]), decsn, @Session["LoginID"].ToString(), Convert.ToInt16(lst[16]), Convert.ToInt16(lst[17]));
                    for (int k = 0; k < idlst.Count; k++)
                    {
                        if (idlst[k] == id)
                            idlst.RemoveAt(k);
                    }
                    lst.RemoveRange(0, 21);
                }
                //  }
                //---------------------------IF Close button called--------------------------------//
                if (btnClose == "Close")
                {
                    @Session["glob"] = true;
                    for (int p = 0; p < idlst.Count; p++)
                    {
                        iwpro.UnlockRecords(idlst[p], "L3");
                    }
                    return Json(false);
                }

                //------------------Select next Pending Record------------------


                SqlDataAdapter adp = new SqlDataAdapter("TFL3Verification", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@procDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                adp.SelectCommand.Parameters.Add("@VFType", SqlDbType.NVarChar).Value = Session["vftype"];
                adp.SelectCommand.Parameters.Add("@ClrType", SqlDbType.NVarChar).Value = Session["clrtype"];
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

                            id = Convert.ToInt64(lst[0]);
                            decsn = lst[3].ToString();//Dicision
                            if (decsn == "A")
                            {
                                if (lst[1].ToString() != lst[14].ToString())
                                    decsn = "AC";
                                else if (lst[2].ToString() != lst[15].ToString())
                                {
                                    if (decsn == "AC")
                                        decsn = "M";
                                    else
                                        decsn = "D";
                                }
                            }
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

                            iwpro.UpdateTFL3Verification(id, uid, lst[1].ToString(), finaldate, Convert.ToInt16(lst[4]), resncode, null, null, cbsclientdtls, cbsJointDtls, Convert.ToDouble(lst[5]), Convert.ToDouble(Session["RVERFNHIGHAMT"]), decsn, @Session["LoginID"].ToString(), Convert.ToInt16(lst[16]), Convert.ToInt16(lst[17]));
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
                            resncodeL2 = lst[11].ToString();
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

                        XMLAmount = Convert.ToDecimal(lst[5]),
                        XMLSerialNo = lst[6].ToString(),
                        XMLPayorBankRoutNo = lst[7].ToString(),
                        XMLSAN = lst[8].ToString(),
                        XMLTransCode = lst[9].ToString(),
                        RejectReason = resncode,
                        L2Rejectreason = resncodeL2,

                        CBSClientAccountDtls = cbsclientdtls,//lst[12].ToString(),
                        CBSJointHoldersName = cbsJointDtls, //lst[13].ToString(),
                        DbtAccountNoOld = lst[14].ToString(),
                        DateOld = lst[15].ToString(),
                        L1Status = Convert.ToInt32(lst[16]),
                        L2Status = Convert.ToInt32(lst[17]),
                        PresentingBankRoutNo = lst[18].ToString(),
                        DocType = lst[19].ToString(),
                        XMLMICRRepairFlags = lst[20].ToString(),



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
                                //ActualAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[2]),
                                DbtAccountNo = ds.Tables[0].Rows[index].ItemArray[2].ToString(),
                                Date = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                                XMLSerialNo = ds.Tables[0].Rows[index].ItemArray[4].ToString(),
                                XMLPayorBankRoutNo = ds.Tables[0].Rows[index].ItemArray[5].ToString(),
                                XMLSAN = ds.Tables[0].Rows[index].ItemArray[6].ToString(),
                                XMLTransCode = ds.Tables[0].Rows[index].ItemArray[7].ToString(),
                                //EntrySerialNo = ds.Tables[0].Rows[0].ItemArray[9].ToString(),
                                //EntryPayorBankRoutNo = ds.Tables[0].Rows[0].ItemArray[10].ToString(),
                                //EntrySAN = ds.Tables[0].Rows[0].ItemArray[11].ToString(),
                                //EntryTransCode = ds.Tables[0].Rows[0].ItemArray[12].ToString(),
                                RejectReason = ds.Tables[0].Rows[index].ItemArray[8].ToString(),// Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[13]),
                                // RejectDescription = ds.Tables[0].Rows[0].ItemArray[9].ToString(),
                                ReturnReasonDescription = ds.Tables[0].Rows[index].ItemArray[9].ToString(),
                                L2By = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[11]),
                                FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[12].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                                FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[13].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                                CBSClientAccountDtls = ds.Tables[0].Rows[index].ItemArray[14].ToString(),
                                CBSJointHoldersName = ds.Tables[0].Rows[index].ItemArray[15].ToString(),
                                OpsStatus = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[16]),
                                DbtAccountNoOld = ds.Tables[0].Rows[index].ItemArray[2].ToString(),
                                DateOld = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                                L1Status = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[17]),
                                L2Status = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[18]),
                                PresentingBankRoutNo = ds.Tables[0].Rows[index].ItemArray[19].ToString(),
                                DocType = ds.Tables[0].Rows[index].ItemArray[20].ToString(),
                                XMLMICRRepairFlags = ds.Tables[0].Rows[index].ItemArray[21].ToString(),
                                BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[22].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
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

                //ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                //er.ErrorMessage = e.InnerException.Message;
                //return View("Error", er);
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.InnerException.Message });
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
                        //------------For CBS bank--------------

                        iwpro.GetOnlyIWUpdateCBSAccInfo(ac, 1, ref strcbsdls, ref strJoinHldrs);
                        model.cbsdls = strcbsdls;
                        model.JoinHldrs = strJoinHldrs;
                        //---------------------------------
                    }
                    if (model != null && model.cbsdls != null && model.cbsdls != "")
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
                    return PartialView("_TFL3GetCBSDtls", model);
                }
                else
                {
                    // cbstetails model = new cbstetails();
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
                    return PartialView("_TFL3GetCBSDtls", model);
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
        public PartialViewResult RejectReason(int id = 0)
        {

            var rjrs = (from r in iwafl.ItemReturnReasons
                        select new RejectReason
                        {
                            Description = r.DESCRIPTION,
                            ReasonCodeS = r.RETURN_REASON_CODE
                        });
            return PartialView("_TFL3RejectReason", rjrs);
        }
        public PartialViewResult getIwlogs(int id)
        {
            var model = iwafl.IWActivityLogs.Where(l => l.IWMainTrID == id).OrderBy(l => l.Timestamp).ToList();
            return PartialView("_IWActivitylogsL2", model);
        }
        public JsonResult getL1logs(int id)
        {
            string decr = null;

            var IWact = iwafl.IWActivityLogs.Where(l => l.IWMainTrID == id && l.LogLevel == "TFL1 Verification"
                && (l.Activity.ToUpper() == "R") && (l.RejectDesc != "0" && l.RejectDesc != null)).OrderByDescending(l => l.RejectDesc).FirstOrDefault();
            if (IWact != null)
            {
                decr = IWact.RejectDesc;
                var rjrs = iwafl.ItemReturnReasons.Where(m => m.RETURN_REASON_CODE == decr).SingleOrDefault();
                if (rjrs != null)
                {
                    decr = rjrs.DESCRIPTION;
                }

                //decr = rjrs;
                //(from r in iwafl.ItemReturnReasons
                //        select new RejectReason
                //        {
                //            Description = r.DESCRIPTION,
                //            ReasonCodeS = r.RETURN_REASON_CODE
                //        });
            }
            else
                decr = "";
            var result = new { descval = decr };
            return Json(decr, JsonRequestBehavior.AllowGet);
        }

    }
}
