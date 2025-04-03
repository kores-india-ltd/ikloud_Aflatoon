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
using NLog;

namespace ikloud_Aflatoon.Controllers
{
    public class IWL3Controller : Controller
    {
        //
        // GET: /IWL3/

        AflatoonEntities iwafl = new AflatoonEntities();
        IWProcDataContext iwpro = new IWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ActionResult Selection(int id = 0)
        {
            ViewBag.ClearingType = new SelectList(iwafl.ClearingTypes, "Code", "Name").ToList();
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

        public ActionResult Index(string vftype = null, string STP = null)
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
                Session["vftype"] = vftype;
                Session["STP"] = STP;

                SqlDataAdapter adp = new SqlDataAdapter("IWL3Verification", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@procDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                adp.SelectCommand.Parameters.Add("@VFType", SqlDbType.NVarChar).Value = Session["vftype"];
                adp.SelectCommand.Parameters.Add("@ClrType", SqlDbType.NVarChar).Value = Session["CtsSessionType"];
                //--------------------Customer Selection---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                adp.SelectCommand.Parameters.Add("@STPNSTP", SqlDbType.NVarChar).Value = Session["STP"];


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
                        RejectReason = ds.Tables[0].Rows[0].ItemArray[13].ToString(),// Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[13]),
                        RejectDescription = ds.Tables[0].Rows[0].ItemArray[14].ToString(),
                        L2By = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[16]),
                        ReturnReasonDescription = ds.Tables[0].Rows[0].ItemArray[17].ToString(),

                        FrontGreyImagePath = ds.Tables[0].Rows[0].ItemArray[18].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[19].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                        CBSClientAccountDtls = ds.Tables[0].Rows[0].ItemArray[20].ToString(),
                        CBSJointHoldersName = ds.Tables[0].Rows[0].ItemArray[21].ToString(),
                        OpsStatus = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[22]),
                        DbtAccountNoOld = ds.Tables[0].Rows[0].ItemArray[3].ToString(),
                        DateOld = ds.Tables[0].Rows[0].ItemArray[4].ToString(),
                        L1Status = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[23]),
                        L2Status = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[24]),
                        PresentingBankRoutNo = ds.Tables[0].Rows[0].ItemArray[25].ToString(),
                        DocType = ds.Tables[0].Rows[0].ItemArray[26].ToString(),
                        XMLMICRRepairFlags = ds.Tables[0].Rows[0].ItemArray[27].ToString(),
                        BackTiffImagePath = ds.Tables[0].Rows[0].ItemArray[28].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                        Modified2 = ds.Tables[0].Rows[0].ItemArray[29].ToString(),
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
                            L2By = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[16]),
                            ReturnReasonDescription = ds.Tables[0].Rows[index].ItemArray[17].ToString(),
                            FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[18].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[19].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                            CBSClientAccountDtls = ds.Tables[0].Rows[index].ItemArray[20].ToString(),
                            CBSJointHoldersName = ds.Tables[0].Rows[index].ItemArray[21].ToString(),
                            OpsStatus = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[22]),
                            DbtAccountNoOld = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                            DateOld = ds.Tables[0].Rows[index].ItemArray[4].ToString(),
                            L1Status = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[23]),
                            L2Status = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[24]),
                            PresentingBankRoutNo = ds.Tables[0].Rows[index].ItemArray[25].ToString(),
                            DocType = ds.Tables[0].Rows[index].ItemArray[26].ToString(),
                            XMLMICRRepairFlags = ds.Tables[0].Rows[index].ItemArray[27].ToString(),
                            BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[28].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                            Modified2 = ds.Tables[0].Rows[index].ItemArray[29].ToString(),
                        };
                        //ViewBag.cnt = true;
                        //def.Vftype = vftype;
                        //def.Clrtype = clrtype;
                        objectlst.Add(def);
                        count = count - 1;
                        index = index + 1;
                    }

                    //-------------Added on 17-09-2020------By Abid----------
                    string[] code = { "05", "10", "11", "12", "13", "14", "15", "16", "17", "30", "31", "32", "33","34","35","36","37","38","39","40","50","52","53","54"
                                    ,"55","56","57","58","59","60","61","62","63","64","65","66","67","68","71","75","76","85","86","87","88","105","106","107","108"};
                    //var rtnlist = (from r in iwafl.ItemReturnReasons
                    //               where code.Contains(r.RETURN_REASON_CODE)
                    //               select new RejectReason
                    //               {
                    //                   Description = r.DESCRIPTION,
                    //                   ReasonCodeS = r.RETURN_REASON_CODE
                    //               });
                    //-------------------------END---------------------------

                    // var rtnlist = (from i in iwafl.ItemReturnReasons select i).ToList();// iwafl.ItemReturnReasons.Select(m).ToList();

                    SqlDataAdapter adp1 = new SqlDataAdapter("IW_GetRejectReason", con);
                    adp1.SelectCommand.CommandType = CommandType.StoredProcedure;

                    DataSet ds1 = new DataSet();
                    adp1.Fill(ds1);

                    var rtnlist = new List<RejectReason>();
                    RejectReason def1;

                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                        {
                            def1 = new RejectReason
                            {
                                ReasonCodeS = ds1.Tables[0].Rows[i].ItemArray[0].ToString(),
                                Description = ds1.Tables[0].Rows[i].ItemArray[1].ToString()
                            };
                            rtnlist.Add(def1);
                        }
                    }

                    ViewBag.rtnlist = rtnlist.Select(m => m.ReasonCodeS).ToList();
                    ViewBag.rtnlistDescrp = rtnlist.Select(m => m.Description).ToList();

                    //========= Amol changes for Hold on 26/08/2023 start =================
                    SqlDataAdapter adp2 = new SqlDataAdapter("Get_HoldReleaseLocationMasterList", con);
                    adp2.SelectCommand.CommandType = CommandType.StoredProcedure;

                    DataSet ds2 = new DataSet();
                    adp2.Fill(ds2);
                    var objectlst2 = new List<Hold_Release_LocationMaster_View>();
                    Hold_Release_LocationMaster_View def2;

                    if (ds2.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                        {
                            def2 = new Hold_Release_LocationMaster_View
                            {
                                Id = Convert.ToInt64(ds2.Tables[0].Rows[i].ItemArray[0]),
                                LocationName = ds2.Tables[0].Rows[i].ItemArray[1].ToString(),

                            };
                            objectlst2.Add(def2);
                        }

                    }

                    ViewBag.LocationMaster = new SelectList(objectlst2.AsEnumerable(), "Id", "LocationName");
                    //========= Amol changes for Hold on 26/08/2023 end =================

                    ViewBag.cnt = true;
                    Session["glob"] = null;
                    return View(objectlst);
                }
                else
                    return RedirectToAction("IWIndex", "Home", new { id = 1 });
                //return RedirectToAction("Selection", new { id = 1 });
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
        public ActionResult IWl3(List<string> lst, bool snd, string img, string btnClose = "default", List<Int64> idlst = null)
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
            //logerror(lst[4], lst[4].ToString() + " - > OpsStatus");
            //logerror(lst[5], lst[5].ToString() + " - > XMLAmount");
            //logerror(lst[3], lst[3].ToString() + " - > Decision");
            //logerror(Session["RVERFNHIGHAMT"].ToString(), Session["RVERFNHIGHAMT"].ToString() + " - > AmountLimit");
            int ttcnt = 0;
            try
            {
                //logerror(lst.Count().ToString(), lst.Count().ToString() + " - > In lst count");
                if (lst != null)
                    ttcnt = lst.Count() / 30;
                //ttcnt = lst.Count() / 28;
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
                string positiveDate = "";
                string positivepayee = "";
                double positiveAmount = 0;
                string positiveSrcOfOrigin = "";
                string positiveReceivedDate = "";
                double accountBalance = 0;

                //======== Amol changes for Hold on 26/08/2023 start ==================
                string holdreason = "";
                int holdLocationId = 0;
                //======== Amol changes for Hold on 26/08/2023 end ==================

                if (btnClose == "Close" && lst != null)
                    ttcnt = ttcnt + 1;
                //logerror(ttcnt.ToString(), ttcnt.ToString() + " - > In ttcnt count");

                for (int i = 0; i < ttcnt - 1; i++)
                {
                    //logerror(i.ToString(), i.ToString() + " - > In ttcnt count for loop index i");
                    // jt = new IWAmountTmpProcess();
                    //logerror(lst[11].ToString(), lst[11].ToString() + " - > In ttcnt count for loop lst[11]");

                    if (lst[11] != null)
                        if (lst[11].ToString() != "")
                            resncode = lst[11].ToString();
                        else
                            resncode = "0";
                    //logerror(resncode.ToString(), resncode.ToString() + " - > In ttcnt count for loop resncode");
                    id = Convert.ToInt64(lst[0]);
                    //logerror(lst[3].ToString(), lst[3].ToString() + " - > In ttcnt count for loop lst[3]");
                    decsn = lst[3].ToString();//Dicision
                    //logerror(decsn.ToString(), decsn.ToString() + " - > In ttcnt count for loop decsn");
                    if (decsn == "A")
                    {
                        //logerror(lst[1].ToString(), lst[1].ToString() + " - > In ttcnt count for loop lst[1]");
                        //logerror(lst[14].ToString(), lst[14].ToString() + " - > In ttcnt count for loop lst[14]");
                        //logerror(lst[2].ToString(), lst[2].ToString() + " - > In ttcnt count for loop lst[2]");
                        //logerror(lst[15].ToString(), lst[15].ToString() + " - > In ttcnt count for loop lst[15]");
                        if (lst[1].ToString() != lst[14].ToString())
                            decsn = "AC";
                        if (lst[2].ToString() != lst[15].ToString())
                        {
                            if (decsn == "AC")
                                decsn = "M";
                            else
                                decsn = "D";
                        }
                        //logerror(decsn.ToString(), decsn.ToString() + " - > In ttcnt count for loop in If A decsn");
                    }
                    //======== Amol changes for Hold on 26/08/2023 start ==================
                    else if (decsn == "H" || decsn == "h")
                    {
                        if (lst[29] != null)
                        {
                            if (lst[29].ToString() != "")
                            {
                                holdreason = lst[29].ToString();
                            }
                            else
                            {
                                holdreason = "";
                            }
                        }
                        else
                        {
                            holdreason = "";
                        }

                        if (lst[28] != null)
                        {
                            if (lst[28].ToString() != "")
                            {
                                holdLocationId = Convert.ToInt16(lst[28].ToString());
                            }
                            else
                            {
                                holdLocationId = 0;
                            }
                        }
                        else
                        {
                            holdLocationId = 0;
                        }

                    }
                    //======== Amol changes for Hold on 26/08/2023 end ==================
                    if (lst[2] != null)
                    {
                        if (lst[2].ToString().Length != 10)
                            finaldate = "20" + lst[2].ToString().Substring(4, 2) + "-" + lst[2].ToString().Substring(2, 2) + "-" + lst[2].ToString().Substring(0, 2);
                        else
                            finaldate = lst[2].ToString();
                        //logerror(finaldate.ToString(), finaldate.ToString() + " - > In ttcnt count for loop in finaldate");
                    }
                    //----------------- Added On 19-05-2017------------------//
                    //logerror(lst[12].ToString(), lst[12].ToString() + " - > In ttcnt count for loop lst[12]");
                    if (lst[12] != null)//CBSClient
                        cbsclientdtls = lst[12].ToString();
                    else
                        cbsclientdtls = "Not Found";
                    //logerror(cbsclientdtls.ToString(), cbsclientdtls.ToString() + " - > In ttcnt count for loop cbsclientdtls");
                    //logerror(lst[13].ToString(), lst[13].ToString() + " - > In ttcnt count for loop lst[13]");
                    if (lst[13] != null)//CBSJointDtls
                        cbsJointDtls = lst[13].ToString();
                    else
                        cbsJointDtls = "Not Found";
                    //logerror(cbsJointDtls.ToString(), cbsJointDtls.ToString() + " - > In ttcnt count for loop cbsJointDtls");
                    //--------------------------------------------------------//
                    //--------------------------------------------------------//Positive pay Data-------
                    //logerror(lst[23], lst[23].ToString() + " - > Positive Amount lst[23]");
                    //logerror(lst[24], lst[24].ToString() + " - > Positive Date lst[24]");
                    //logerror(lst[25], lst[25].ToString() + " - > Positive Payee lst[25]");
                    //logerror(lst[26], lst[26].ToString() + " - > Positive Src lst[26]");
                    //logerror(lst[27], lst[27].ToString() + " - > Positive Received Date lst[27]");

                    if (lst[23] != null)
                    {
                        if(lst[23] != "")
                        {
                            if (lst[23].ToString().Length == 0)
                            {
                                positiveAmount = 0;
                            }
                            else
                            {
                                positiveAmount = Convert.ToDouble(lst[23].ToString());
                            }
                        }
                        else
                        {
                            positiveAmount = 0;
                        }
                        
                    }
                    else
                    {
                        positiveAmount = 0;
                    }
                    //logerror(positiveAmount.ToString(), positiveAmount.ToString() + " - > Positive Amount positiveAmount");

                    if (lst[24] != null)
                    {
                        if(lst[24] != "")
                        {
                            if (lst[24].ToString().Length == 0)
                            {
                                positiveDate = "";
                            }
                            else
                            {
                                positiveDate = lst[24].ToString();
                            }
                        }
                        else
                        {
                            positiveDate = "";
                        }
                        
                    }
                    else
                    {
                        positiveDate = "";
                    }
                    //logerror(positiveDate.ToString(), positiveDate.ToString() + " - > Positive Amount positiveDate");

                    if (lst[25] != null)
                    {
                        if(lst[25] != "")
                        {
                            if (lst[25].ToString().Length == 0)
                            {
                                positivepayee = "";
                            }
                            else
                            {
                                positivepayee = lst[25].ToString();
                            }
                        }
                        else
                        {
                            positivepayee = "";
                        }
                        
                    }
                    else
                    {
                        positivepayee = "";
                    }
                    //logerror(positivepayee.ToString(), positivepayee.ToString() + " - > Positive Amount positivepayee");

                    if (lst[26] != null)
                    {
                        if(lst[26] != "")
                        {
                            if (lst[26].ToString().Length == 0)
                            {
                                positiveSrcOfOrigin = "";
                            }
                            else
                            {
                                positiveSrcOfOrigin = lst[26].ToString();
                            }
                        }
                        else
                        {
                            positiveSrcOfOrigin = "";
                        }
                        
                    }
                    else
                    {
                        positiveSrcOfOrigin = "";
                    }
                    //logerror(positiveSrcOfOrigin.ToString(), positiveSrcOfOrigin.ToString() + " - > Positive Amount positiveSrcOfOrigin");

                    if (lst[27] != null)
                    {
                        if(lst[27] != "")
                        {
                            if (lst[27].ToString().Length == 0)
                            {
                                positiveReceivedDate = "";
                            }
                            else
                            {
                                positiveReceivedDate = lst[27].ToString();
                            }
                        }
                        else
                        {
                            positiveReceivedDate = "";
                        }
                        
                    }
                    else
                    {
                        positiveReceivedDate = "";
                    }
                    //logerror(positiveReceivedDate.ToString(), positiveReceivedDate.ToString() + " - > Positive Amount positiveReceivedDate");



                    //logerror(resncode, resncode.ToString() + " - > Reason Code");

                    //logerror(decsn, decsn.ToString() + " - > Decision After");
                    //logerror(lst[23], lst[23].ToString() + " - > Positive Amount");
                    //logerror(lst[24], lst[24].ToString() + " - > Positive Date");
                    //logerror(lst[25], lst[25].ToString() + " - > Positive Payee");
                    //logerror(lst[26], lst[26].ToString() + " - > Positive Src");
                    //logerror(lst[27], lst[27].ToString() + " - > Positive Received Date");
                    //logerror(lst[16], lst[16].ToString() + " - > L1Status");
                    //logerror(lst[17], lst[17].ToString() + " - > L2Status");
                    //logerror(positiveAmount.ToString(), positiveAmount.ToString() + " - > Positive Amount");
                    //logerror(positiveDate, positiveDate.ToString() + " - > Positive Date");
                    //logerror(positivepayee, positivepayee.ToString() + " - > Positive Payee");
                    //logerror(positiveSrcOfOrigin, positiveSrcOfOrigin.ToString() + " - > Positive Src");
                    //logerror(positiveReceivedDate, positiveReceivedDate.ToString() + " - > Positive Received Date");

                    Double num = 0;
                    bool isDouble = false;

                    if (lst[12] != null)
                    {
                        //string newStr = lst[12].Substring(lst[12].LastIndexOf("-") + 1);
                        //string result = lst[12].Split('|').Last();
                        //========== amol changes on 05/11/2022 for report code start ======================
                        string result = lst[12].Split('|').ElementAt(13);
                        //========== amol changes on 05/11/2022 for report code start ======================
                        isDouble = Double.TryParse(result, out num);

                        if (isDouble)
                        {
                            accountBalance = Convert.ToDouble(result);
                        }

                    }
                    else
                    {
                        accountBalance = 0;
                    }

                    //iwpro.UpdateIWL3Verification(id, uid, lst[1].ToString(), finaldate, Convert.ToInt32(lst[4]), resncode, null, null, 
                    //    cbsclientdtls, cbsJointDtls, Convert.ToDouble(lst[5]), Convert.ToDouble(Session["RVERFNHIGHAMT"]), decsn, 
                    //    @Session["LoginID"].ToString(), Convert.ToInt16(lst[16]), Convert.ToInt16(lst[17]), 
                    //    Convert.ToInt16(Session["CustomerID"]), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), 
                    //    lst[21].ToString(), positivepayee, positiveAmount, positiveDate, positiveSrcOfOrigin, positiveReceivedDate, 
                    //    accountBalance);

                    //============== Amol changes for hold on 26/08/2023 start =============================


                    SqlCommand cmd = new SqlCommand("UpdateIWL3Verification", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Uid", uid);
                    cmd.Parameters.AddWithValue("@DbtAccno", lst[1].ToString());
                    cmd.Parameters.AddWithValue("@Date", finaldate);
                    cmd.Parameters.AddWithValue("@OpsStatus", Convert.ToInt32(lst[4]));
                    cmd.Parameters.AddWithValue("@Reasoncode", resncode);
                    cmd.Parameters.AddWithValue("@ReasonDescrp", null);
                    cmd.Parameters.AddWithValue("@ReturnReasonDescription", null);
                    cmd.Parameters.AddWithValue("@cbsdtls", cbsclientdtls);
                    cmd.Parameters.AddWithValue("@jointdtls", cbsJointDtls);
                    cmd.Parameters.AddWithValue("@XmlAmount", Convert.ToDouble(lst[5]));
                    cmd.Parameters.AddWithValue("@AmountLimit", Convert.ToDouble(Session["RVERFNHIGHAMT"]));
                    cmd.Parameters.AddWithValue("@Decision", decsn);
                    cmd.Parameters.AddWithValue("@LName", @Session["LoginID"].ToString());
                    cmd.Parameters.AddWithValue("@L1Status", Convert.ToInt16(lst[16]));
                    cmd.Parameters.AddWithValue("@L2Status", Convert.ToInt16(lst[17]));
                    cmd.Parameters.AddWithValue("@CustomerId", Convert.ToInt16(Session["CustomerID"]));
                    cmd.Parameters.AddWithValue("@Processingdate", Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Modified", lst[21].ToString());
                    cmd.Parameters.AddWithValue("@PositivePayee", positivepayee);
                    cmd.Parameters.AddWithValue("@PositiveAmount", positiveAmount);
                    cmd.Parameters.AddWithValue("@PositiveDate", positiveDate);
                    cmd.Parameters.AddWithValue("@PositiveSourceOfOrigin", positiveSrcOfOrigin);
                    cmd.Parameters.AddWithValue("@PositiveReceivedDate", positiveReceivedDate);
                    cmd.Parameters.AddWithValue("@AccountBalance", accountBalance);
                    cmd.Parameters.AddWithValue("@HoldLocationId", holdLocationId);
                    cmd.Parameters.AddWithValue("@HoldReason", holdreason);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    //============== Amol changes for hold on 26/08/2023 end =============================

                    for (int k = 0; k < idlst.Count; k++)
                    {
                        if (idlst[k] == id)
                            idlst.RemoveAt(k);
                    }
                    //lst.RemoveRange(0, 28);
                    lst.RemoveRange(0, 30);
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
                //logerror("select next pending record", "select next pending record" + " - > select next pending record");

                SqlDataAdapter adp = new SqlDataAdapter("IWL3Verification", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@procDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                adp.SelectCommand.Parameters.Add("@VFType", SqlDbType.NVarChar).Value = Session["vftype"];
                adp.SelectCommand.Parameters.Add("@ClrType", SqlDbType.NVarChar).Value = Session["CtsSessionType"];
                //--------------------Customer Selection---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                adp.SelectCommand.Parameters.Add("@STPNSTP", SqlDbType.NVarChar).Value = Session["STP"];



                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<L2Helper>();
                L2Helper def;
                ArrayList ids = new ArrayList();
                bool checkid = false;

                //logerror(ds.Tables[0].Rows.Count.ToString(), ds.Tables[0].Rows.Count.ToString() + " - > ds.Tables[0].Rows.Count");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //--------------Checking Last record and list record are same------
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        if (Convert.ToInt64(lst[0]) == Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]))
                        {
                            //logerror("Last and List record are same", "Last and List record are same" + " - > Last and List record are same");
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
                            //======== Amol changes for Hold on 26/08/2023 start ==================
                            else if (decsn == "H" || decsn == "h")
                            {
                                if (lst[29] != null)
                                {
                                    if (lst[29].ToString() != "")
                                    {
                                        holdreason = lst[29].ToString();
                                    }
                                    else
                                    {
                                        holdreason = "";
                                    }
                                }
                                else
                                {
                                    holdreason = "";
                                }

                                if (lst[28] != null)
                                {
                                    if (lst[28].ToString() != "")
                                    {
                                        holdLocationId = Convert.ToInt16(lst[28].ToString());
                                    }
                                    else
                                    {
                                        holdLocationId = 0;
                                    }
                                }
                                else
                                {
                                    holdLocationId = 0;
                                }
                            }
                            //======== Amol changes for Hold on 26/08/2023 end ==================
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


                            //--------------------------------------------------------//Positive pay Data-------
                            //if (lst[23] != null)
                            //    positiveAmount = Convert.ToDouble(lst[23].ToString());
                            //if (lst[24] != null)
                            //    positiveDate = lst[24].ToString();
                            //if (lst[25] != null)
                            //    positivepayee = lst[25].ToString();
                            //if (lst[26] != null)
                            //    positiveSrcOfOrigin = lst[26].ToString();
                            //if (lst[27] != null)
                            //    positiveReceivedDate = lst[27].ToString();

                            if (lst[23] != null)
                            {
                                if(lst[23] != "")
                                {
                                    if (lst[23].ToString().Length == 0)
                                    {
                                        positiveAmount = 0;
                                    }
                                    else
                                    {
                                        positiveAmount = Convert.ToDouble(lst[23].ToString());
                                    }
                                }
                                else
                                {
                                    positiveAmount = 0;
                                }
                                
                            }
                            else
                            {
                                positiveAmount = 0;
                            }
                            //logerror(positiveAmount.ToString(), positiveAmount.ToString() + " - > Positive Amount positiveAmount");

                            if (lst[24] != null)
                            {
                                if(lst[24] != "")
                                {
                                    if (lst[24].ToString().Length == 0)
                                    {
                                        positiveDate = "";
                                    }
                                    else
                                    {
                                        positiveDate = lst[24].ToString();
                                    }
                                }
                                else
                                {
                                    positiveDate = "";
                                }
                                
                            }
                            else
                            {
                                positiveDate = "";
                            }
                            //logerror(positiveDate.ToString(), positiveDate.ToString() + " - > Positive Amount positiveDate");

                            if (lst[25] != null)
                            {
                                if(lst[25] != "")
                                {
                                    if (lst[25].ToString().Length == 0)
                                    {
                                        positivepayee = "";
                                    }
                                    else
                                    {
                                        positivepayee = lst[25].ToString();
                                    }
                                }
                                else
                                {
                                    positivepayee = "";
                                }
                                
                            }
                            else
                            {
                                positivepayee = "";
                            }
                            //logerror(positivepayee.ToString(), positivepayee.ToString() + " - > Positive Amount positivepayee");

                            if (lst[26] != null)
                            {
                                if(lst[26] != "")
                                {
                                    if (lst[26].ToString().Length == 0)
                                    {
                                        positiveSrcOfOrigin = "";
                                    }
                                    else
                                    {
                                        positiveSrcOfOrigin = lst[26].ToString();
                                    }
                                }
                                else
                                {
                                    positiveSrcOfOrigin = "";
                                }
                                
                            }
                            else
                            {
                                positiveSrcOfOrigin = "";
                            }
                            //logerror(positiveSrcOfOrigin.ToString(), positiveSrcOfOrigin.ToString() + " - > Positive Amount positiveSrcOfOrigin");

                            if (lst[27] != null)
                            {
                                if(lst[27] != "")
                                {
                                    if (lst[27].ToString().Length == 0)
                                    {
                                        positiveReceivedDate = "";
                                    }
                                    else
                                    {
                                        positiveReceivedDate = lst[27].ToString();
                                    }
                                }
                                else
                                {
                                    positiveReceivedDate = "";
                                }
                                
                            }
                            else
                            {
                                positiveReceivedDate = "";
                            }
                            //logerror(positiveReceivedDate.ToString(), positiveReceivedDate.ToString() + " - > Positive Amount positiveReceivedDate");

                            Double num = 0;
                            bool isDouble = false;

                            if (lst[12] != null)
                            {
                                //string newStr = lst[12].Substring(lst[12].LastIndexOf("-") + 1);
                                //string result = lst[12].Split('|').Last();
                                //========== amol changes on 05/11/2022 for report code start ======================
                                string result = lst[12].Split('|').ElementAt(13);
                                //========== amol changes on 05/11/2022 for report code start ======================
                                isDouble = Double.TryParse(result, out num);

                                if (isDouble)
                                {
                                    accountBalance = Convert.ToDouble(result);
                                }

                            }
                            else
                            {
                                accountBalance = 0;
                            }

                            //iwpro.UpdateIWL3Verification(id, uid, lst[1].ToString(), finaldate, Convert.ToInt32(lst[4]), resncode, null, null, 
                            //    cbsclientdtls, cbsJointDtls, Convert.ToDouble(lst[5]), Convert.ToDouble(Session["RVERFNHIGHAMT"]), decsn, 
                            //    @Session["LoginID"].ToString(), Convert.ToInt16(lst[16]), Convert.ToInt16(lst[17]), 
                            //    Convert.ToInt16(Session["CustomerID"]), 
                            //    Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), lst[21].ToString(), positivepayee, 
                            //    positiveAmount, positiveDate, positiveSrcOfOrigin, positiveReceivedDate, accountBalance);

                            //============== Amol changes for hold on 26/08/2023 start =============================


                            SqlCommand cmd = new SqlCommand("UpdateIWL3Verification", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID", id);
                            cmd.Parameters.AddWithValue("@Uid", uid);
                            cmd.Parameters.AddWithValue("@DbtAccno", lst[1].ToString());
                            cmd.Parameters.AddWithValue("@Date", finaldate);
                            cmd.Parameters.AddWithValue("@OpsStatus", Convert.ToInt32(lst[4]));
                            cmd.Parameters.AddWithValue("@Reasoncode", resncode);
                            cmd.Parameters.AddWithValue("@ReasonDescrp", null);
                            cmd.Parameters.AddWithValue("@ReturnReasonDescription", null);
                            cmd.Parameters.AddWithValue("@cbsdtls", cbsclientdtls);
                            cmd.Parameters.AddWithValue("@jointdtls", cbsJointDtls);
                            cmd.Parameters.AddWithValue("@XmlAmount", Convert.ToDouble(lst[5]));
                            cmd.Parameters.AddWithValue("@AmountLimit", Convert.ToDouble(Session["RVERFNHIGHAMT"]));
                            cmd.Parameters.AddWithValue("@Decision", decsn);
                            cmd.Parameters.AddWithValue("@LName", @Session["LoginID"].ToString());
                            cmd.Parameters.AddWithValue("@L1Status", Convert.ToInt16(lst[16]));
                            cmd.Parameters.AddWithValue("@L2Status", Convert.ToInt16(lst[17]));
                            cmd.Parameters.AddWithValue("@CustomerId", Convert.ToInt16(Session["CustomerID"]));
                            cmd.Parameters.AddWithValue("@Processingdate", Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@Modified", lst[21].ToString());
                            cmd.Parameters.AddWithValue("@PositivePayee", positivepayee);
                            cmd.Parameters.AddWithValue("@PositiveAmount", positiveAmount);
                            cmd.Parameters.AddWithValue("@PositiveDate", positiveDate);
                            cmd.Parameters.AddWithValue("@PositiveSourceOfOrigin", positiveSrcOfOrigin);
                            cmd.Parameters.AddWithValue("@PositiveReceivedDate", positiveReceivedDate);
                            cmd.Parameters.AddWithValue("@AccountBalance", accountBalance);
                            cmd.Parameters.AddWithValue("@HoldLocationId", holdLocationId);
                            cmd.Parameters.AddWithValue("@HoldReason", holdreason);

                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();

                            //============== Amol changes for hold on 26/08/2023 end =============================

                            //lst.RemoveRange(0, 28);
                            lst.RemoveRange(0, 30);
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
                    //--------------------------------------------------------//Positive pay Data-------
                    //if (lst[23] != null)
                    //    positiveAmount = Convert.ToDouble(lst[23].ToString());
                    //if (lst[24] != null)
                    //    positiveDate = lst[24].ToString();
                    //if (lst[25] != null)
                    //    positivepayee = lst[25].ToString();
                    //if (lst[26] != null)
                    //    positiveSrcOfOrigin = lst[26].ToString();
                    //if (lst[27] != null)
                    //    positiveReceivedDate = lst[27].ToString();

                    //=========== Amol changes for Hold on 26/08/2023 start ==============
                    if (lst[29] != null)
                    {
                        if (lst[29].ToString() != "")
                        {
                            holdreason = lst[29].ToString();
                        }
                        else
                        {
                            holdreason = "";
                        }
                    }
                    else
                    {
                        holdreason = "";
                    }

                    if (lst[28] != null)
                    {
                        if (lst[28].ToString() != "")
                        {
                            holdLocationId = Convert.ToInt16(lst[28].ToString());
                        }
                        else
                        {
                            holdLocationId = 0;
                        }
                    }
                    else
                    {
                        holdLocationId = 0;
                    }
                    //=========== Amol changes for Hold on 26/08/2023 end ==============

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
                        Modified3 = lst[21].ToString(),
                        Modified2 = lst[22].ToString(),
                        //pAmt = positiveAmount,
                        //pDate = positiveDate,
                        //pPayee = positivepayee,
                        //pSrcOfOrigin = positiveSrcOfOrigin,
                        //pReceivedDate = positiveReceivedDate,

                        //-------------------------------//------------------//
                        //=========== Amol changes for Hold on 26/08/2023 start ==============
                        HoldLocationId = holdLocationId,
                        HoldReason = holdreason,
                        //=========== Amol changes for Hold on 26/08/2023 end ==============
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
                                RejectReason = ds.Tables[0].Rows[index].ItemArray[13].ToString(), //Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[13]),
                                RejectDescription = ds.Tables[0].Rows[index].ItemArray[14].ToString(),
                                L2By = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[16]),
                                ReturnReasonDescription = ds.Tables[0].Rows[index].ItemArray[17].ToString(),
                                FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[18].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                                FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[19].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                                CBSClientAccountDtls = ds.Tables[0].Rows[index].ItemArray[20].ToString(),
                                CBSJointHoldersName = ds.Tables[0].Rows[index].ItemArray[21].ToString(),
                                OpsStatus = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[22]),
                                DbtAccountNoOld = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                                DateOld = ds.Tables[0].Rows[index].ItemArray[4].ToString(),
                                L1Status = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[23]),
                                L2Status = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[24]),
                                PresentingBankRoutNo = ds.Tables[0].Rows[index].ItemArray[25].ToString(),
                                DocType = ds.Tables[0].Rows[index].ItemArray[26].ToString(),
                                XMLMICRRepairFlags = ds.Tables[0].Rows[index].ItemArray[27].ToString(),
                                BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[28].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                                Modified2 = ds.Tables[0].Rows[index].ItemArray[29].ToString(),
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
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
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
                        //======= Adding try catch block for handling finacle down error view on 01/03/2023 by amol ==========
                        try
                        {
                            //------------For CBS bank--------------

                            iwpro.GetOnlyIWUpdateCBSAccInfo(ac, 1, ref strcbsdls, ref strJoinHldrs);
                            model.cbsdls = strcbsdls;
                            model.JoinHldrs = strJoinHldrs;
                            //---------------------------------
                        }
                        catch (Exception ex)
                        {
                            model.CustomError = "Finacle down. Service unavailable!";
                            return PartialView("_L3GetCBSDtls", model);
                        }
                        
                    }
                    if (model != null && model.cbsdls != null && model.cbsdls != "")
                    {
                        if (model.cbsdls.Split('|').ElementAt(1) == "S")
                        {
                            if (model.cbsdls.Split('|').ElementAt(5).Trim() != "")
                            {
                                string MOP = iwafl.MOPCodeMasters.Find(model.cbsdls.Split('|').ElementAt(5)).Description;
                                model.MOP = MOP != null ? MOP : "";

                                //-------------------------Added by Abid on 19-10-2020----------------for checking AI MOP Active------
                                SqlDataAdapter adpAI = new SqlDataAdapter("Getvalidatewith_AIMop", con);
                                adpAI.SelectCommand.CommandType = CommandType.StoredProcedure;
                                adpAI.SelectCommand.Parameters.Add("@MOP", SqlDbType.NVarChar).Value = model.cbsdls.Split('|').ElementAt(5).Trim();

                                DataSet dsAI = new DataSet();
                                adpAI.Fill(dsAI);

                                if (dsAI.Tables[0].Rows.Count > 0)
                                    model.AI_MOP = "True";
                                else
                                    model.AI_MOP = "False";

                                //-------------------------END------------------------------------------------------------------------
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
                            adp.SelectCommand.Parameters.Add("@Varificationlvl", SqlDbType.NVarChar).Value = "L3";
                            adp.SelectCommand.Parameters.Add("@OutInward", SqlDbType.NVarChar).Value = "IW";
                            //  adp.SelectCommand.Parameters.Add("@InstrumentType", SqlDbType.NVarChar).Value = instrntType;
                            adp.SelectCommand.Parameters.Add("@ReportCode", SqlDbType.NVarChar).Value = model.cbsdls.Split('|').ElementAt(14).ToString();
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
                    }
                    else
                    {
                        cbstetails Tempcbdtls = new cbstetails();
                        Tempcbdtls.cbsdls = "|F|Account does not exist";
                        model = Tempcbdtls;
                    }

                    return PartialView("_L3GetCBSDtls", model);
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

                            //-------------------------Added by Abid on 19-10-2020----------------for checking AI MOP Active------
                            SqlDataAdapter adpAI = new SqlDataAdapter("Getvalidatewith_AIMop", con);
                            adpAI.SelectCommand.CommandType = CommandType.StoredProcedure;
                            adpAI.SelectCommand.Parameters.Add("@MOP", SqlDbType.NVarChar).Value = model.cbsdls.Split('|').ElementAt(5).Trim();

                            DataSet dsAI = new DataSet();
                            adpAI.Fill(dsAI);

                            if (dsAI.Tables[0].Rows.Count > 0)
                                model.AI_MOP = "True";
                            else
                                model.AI_MOP = "False";

                            //-------------------------END------------------------------------------------------------------------
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
                        adp.SelectCommand.Parameters.Add("@Varificationlvl", SqlDbType.NVarChar).Value = "L3";
                        adp.SelectCommand.Parameters.Add("@OutInward", SqlDbType.NVarChar).Value = "IW";
                        //  adp.SelectCommand.Parameters.Add("@InstrumentType", SqlDbType.NVarChar).Value = instrntType;
                        adp.SelectCommand.Parameters.Add("@ReportCode", SqlDbType.NVarChar).Value = model.cbsdls.Split('|').ElementAt(14).ToString();
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

                    return PartialView("_L3GetCBSDtls", model);
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

                                //-------------------------Added by Abid on 19-10-2020----------------for checking AI MOP Active------
                                SqlDataAdapter adpAI = new SqlDataAdapter("Getvalidatewith_AIMop", con);
                                adpAI.SelectCommand.CommandType = CommandType.StoredProcedure;
                                adpAI.SelectCommand.Parameters.Add("@MOP", SqlDbType.NVarChar).Value = model.cbsdls.Split('|').ElementAt(6).Trim();

                                DataSet dsAI = new DataSet();
                                adpAI.Fill(dsAI);

                                if (dsAI.Tables[0].Rows.Count > 0)
                                    model.AI_MOP = "True";
                                else
                                    model.AI_MOP = "False";

                                //-------------------------END------------------------------------------------------------------------

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
                        //---------For CBS Bank----------------

                        iwpro.OWGetCBSAccInfoWithOutUpdate_New(ac, ref strcbsdls, ref strJoinHldrs);
                        model.cbsdls = strcbsdls;
                        model.JoinHldrs = strJoinHldrs;
                        //---------------------------------
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

                                //-------------------------Added by Abid on 19-10-2020----------------for checking AI MOP Active------
                                SqlDataAdapter adpAI = new SqlDataAdapter("Getvalidatewith_AIMop", con);
                                adpAI.SelectCommand.CommandType = CommandType.StoredProcedure;
                                adpAI.SelectCommand.Parameters.Add("@MOP", SqlDbType.NVarChar).Value = model.cbsdls.Split('|').ElementAt(6).Trim();

                                DataSet dsAI = new DataSet();
                                adpAI.Fill(dsAI);

                                if (dsAI.Tables[0].Rows.Count > 0)
                                    model.AI_MOP = "True";
                                else
                                    model.AI_MOP = "False";

                                //-------------------------END------------------------------------------------------------------------
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
                    adp.SelectCommand.Parameters.Add("@Varificationlvl", SqlDbType.NVarChar).Value = "L3";
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
                //return PartialView("Error", "Error");
            }
        }
        //---------------------------PositivePay Data------------------------//
        [HttpPost]
        public ActionResult GetPositiveData(string AccountNo = null, string chequeNo = null, Int64 ID = 0, string FromSrc = null, string SAN = null)
        {
            //-------------------------------Added BY Abid on 07-11-2020------------------
            string OutputData = null;
            string date = null;
            string amount = null;
            string payee = null;
            string Finaldata = null;
            string src = null;
            string receivedDate = null;
            //logerror(AccountNo, AccountNo.ToString() + " - > AccountNo");
            //logerror(chequeNo, chequeNo.ToString() + " - > chequeNo");
            //logerror(ID.ToString(), ID.ToString() + " - > ID");

            try
            {
                //logerror("In Try", "In Try" + " - > In Try block");
                //iwpro.GetPositivePayInfo(AccountNo, chequeNo, ref OutputData);
                con.Open();
                SqlCommand cmd = new SqlCommand("GetPositivePayInfo", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@AccNo", AccountNo);
                cmd.Parameters.AddWithValue("@ChqNo", chequeNo);
                cmd.Parameters.AddWithValue("@SAN", SAN);
                cmd.Parameters.Add("@AccDet", SqlDbType.VarChar, 1000);
                cmd.Parameters["@AccDet"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                OutputData = (string)cmd.Parameters["@AccDet"].Value;
                con.Close();
                //logerror(OutputData, OutputData.ToString() + " - > OutputData");


                if (OutputData != null && OutputData != "")
                {
                    //logerror("In OutputData", "In OutputData" + " - > In OutputData IF");
                    if (OutputData.Split('|').ElementAt(0) != "F")
                    {
                        date = OutputData.Split('|').ElementAt(0).ToString();
                        amount = OutputData.Split('|').ElementAt(1).ToString();
                        payee = OutputData.Split('|').ElementAt(2).ToString();
                        date = Convert.ToDateTime(date).ToString("yyyy-MM-dd");

                        //logerror(amount, amount.ToString() + " - > amount");
                        //logerror(payee, payee.ToString() + " - > payee");
                        //logerror(date, date.ToString() + " - > date");

                        src = OutputData.Split('|').ElementAt(3).ToString();
                        receivedDate = OutputData.Split('|').ElementAt(4).ToString();
                        receivedDate = Convert.ToDateTime(receivedDate).ToString("yyyy-MM-dd");

                        date = date.Split('-').ElementAt(2) + date.Split('-').ElementAt(1) + date.Split('-').ElementAt(0).Substring(2, 2);
                        receivedDate = receivedDate.Split('-').ElementAt(2) + receivedDate.Split('-').ElementAt(1) + receivedDate.Split('-').ElementAt(0).Substring(2, 2);

                        Finaldata = OutputData.Split('|').ElementAt(1).ToString() + "|" + date + "|" + payee + "|" + src + "|" + receivedDate;
                        //logerror("In IF", "In IF" + " - > OutputData IF");
                    }
                    else
                    {
                        //logerror("In Else", "In Else" + " - > OutputData IF F");
                        Finaldata = OutputData.ToString();
                    }
                        
                }

                //logerror(ID.ToString(), ID.ToString() + " - > ID");
                //logerror(amount, amount.ToString() + " - > amount");
                //logerror(payee, payee.ToString() + " - > payee");
                //logerror(date, date.ToString() + " - > date");
                //logerror(src, src.ToString() + " - > src");
                //logerror(receivedDate, receivedDate.ToString() + " - > receivedDate");
                //logerror(Finaldata, Finaldata.ToString() + " - > Finaldata");

                con.Open();
                SqlCommand cmd1 = new SqlCommand("InsertPositivePayInfo_L3", con);
                cmd1.CommandType = CommandType.StoredProcedure;

                cmd1.Parameters.AddWithValue("@Main_Tr_ID", ID);
                cmd1.Parameters.AddWithValue("@PositiveAmount", amount == null ? "" : amount);
                cmd1.Parameters.AddWithValue("@PositiveDate", date == null ? "" : date);
                cmd1.Parameters.AddWithValue("@PositivePayeeName", payee == null ? "" : payee);
                cmd1.Parameters.AddWithValue("@PositiveSource", src == null ? "" : src);
                cmd1.Parameters.AddWithValue("@PositiveReceivedDate", receivedDate == null ? "" : receivedDate);
                cmd1.Parameters.AddWithValue("@PositiveFinalData", Finaldata == null ? "" : Finaldata);
                cmd1.Parameters.AddWithValue("@FromSrc", FromSrc == null ? "" : FromSrc);
                cmd1.ExecuteNonQuery();
                con.Close();

                //ViewBag.PositiveAmount = amount == null ? "" : amount;
                //ViewBag.PositiveDate = date == null ? "" : date;
                //ViewBag.PositivePayeeName = payee == null ? "" : payee;
                //ViewBag.PositiveSource = src == null ? "" : src;
                //ViewBag.PositiveReceivedDate = receivedDate == null ? "" : receivedDate;
                //ViewBag.PositiveFinalData = Finaldata == null ? "" : Finaldata;
            }
            catch (Exception e)
            {
                logerror(e.Message, e.Message.ToString() + " - > In Catch block message");
                logerror(e.InnerException.ToString(), e.InnerException.ToString() + " - > In Catch block InnerException");
            }

            

            return Json(Finaldata, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult RejectReason(int id = 0)
        {

            //-------------Added on 17-09-2020------By Abid----------
            string[] code = { "05", "10", "11", "12", "13", "14", "15", "16", "17", "30", "31", "32", "33","34","35","36","37","38","39","40","50","52","53","54"
                                    ,"55","56","57","58","59","60","61","62","63","64","65","66","67","68","71","75","76","85","86","87","88"};
            //-------------------------END---------------------------

            //var rjrs = (from r in iwafl.ItemReturnReasons
            //            where code.Contains(r.RETURN_REASON_CODE)
            //            select new RejectReason
            //            {
            //                Description = r.DESCRIPTION,
            //                ReasonCodeS = r.RETURN_REASON_CODE
            //            });
            SqlDataAdapter adp = new SqlDataAdapter("IW_GetRejectReason", con);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            adp.Fill(ds);

            var objectlst = new List<RejectReason>();
            RejectReason def;

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    def = new RejectReason
                    {
                        ReasonCodeS = ds.Tables[0].Rows[i].ItemArray[0].ToString(),
                        Description = ds.Tables[0].Rows[i].ItemArray[1].ToString()
                    };
                    objectlst.Add(def);
                }
            }

            return PartialView("_L3RejectReason", objectlst);
        }
        public PartialViewResult getIwlogs(Int64 id = 0)
        {
            SqlDataAdapter sqladlog = new SqlDataAdapter("GetKoresAndAILogs", con);
            sqladlog.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqladlog.SelectCommand.Parameters.AddWithValue("@ID", id);
            sqladlog.SelectCommand.Parameters.AddWithValue("@PROCESSINGDATE", Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"));
            DataSet dslog = new DataSet();
            sqladlog.Fill(dslog);

            int totalcount = 0;
            int index = 0;
            List<IWActivityLog> IWactlog = new List<IWActivityLog>();
            if (dslog.Tables[0].Rows.Count > 0)
            {

                IWactlog = (from DataRow dr in dslog.Tables[0].Rows
                            select new IWActivityLog
                            {
                                LogLevel = dr["LogLevel"].ToString(),
                                Activity = dr["Activity"].ToString(),
                                LoginID = dr["LoginID"].ToString(),
                                RejectDesc = dr["RejectDesc"].ToString(),
                                Comments = dr["Comments"].ToString(),
                                Timestamp = Convert.ToDateTime(dr["Timestamp"]),
                                AIMICRDecision = dr["MICR_AIDecision"].ToString(),
                                AIAccountDecision = dr["AccountNo_AIDecision"].ToString(),
                                AIFigureAmtDecision = dr["AmountFigure_AIDecision"].ToString(),
                                AIWordAmtDecision = dr["AmountWord_AIDecision"].ToString(),
                                AIDateDecision = dr["Date_AIDecision"].ToString(),
                                AISignautre = dr["Signature_AIDecision"].ToString(),
                                MICR_AIRejectReason = dr["MICR_AIRejectReason"].ToString(),
                                AccountNo_AIRejectReason = dr["AccountNo_AIRejectReason"].ToString(),
                                AmountFigure_AIRejectReason = dr["AmountFigure_AIRejectReason"].ToString(),
                                AmountWord_AIRejectReason = dr["AmountWord_AIRejectReason"].ToString(),
                                Date_AIRejectReason = dr["Date_AIRejectReason"].ToString(),
                                Signature_AIRejectReason = dr["Signature_AIRejectReason"].ToString(),
                                MICR_AIConfidence = dr["MICR_AIConfidence"].ToString(),
                                AccountNo_AIConfidence = dr["AccountNo_AIConfidence"].ToString(),
                                AmountFigure_AIConfidence = dr["AmountFigure_AIConfidence"].ToString(),
                                AmountWord_AIConfidence = dr["AmountWord_AIConfidence"].ToString(),
                                Date_AIConfidence = dr["Date_AIConfidence"].ToString(),
                                Signature_AIConfidence = dr["Signature_AIConfidence"].ToString(),
                                MICR_AIValue = dr["MICR_AIValue"].ToString(),
                                AccountNo_AIValue = dr["AccountNo_AIValue"].ToString(),
                                AmountFigure_AIValue = dr["AmountFigure_AIValue"].ToString(),
                                AmountWord_AIValue = dr["AmountWord_AIValue"].ToString(),
                                Date_AIValue = dr["Date_AIValue"].ToString(),
                                Signature_AIDecision = dr["Signature_AIDecision"].ToString(),

                            }).ToList();

            }

            // var model = iwafl.IWActivityLogs.Where(l => l.IWMainTrID == id).OrderBy(l => l.Timestamp).ToList();
            return PartialView("_IWActivitylogsL2", IWactlog);
        }
        public JsonResult getL1logs(int id)
        {
            string decr = null;

            var IWact = iwafl.IWActivityLogs.Where(l => l.IWMainTrID == id && l.LogLevel == "L1 VERIFICATION"
                && (l.Activity.ToUpper() == "R" || l.Activity == "Payee Name") && (l.RejectDesc != "0" && l.RejectDesc != null)).OrderByDescending(l => l.RejectDesc).FirstOrDefault();
            if (IWact != null)
            {
                decr = IWact.RejectDesc;
                var rjrs = iwafl.ItemReturnReasons.Where(m => m.RETURN_REASON_CODE == decr).SingleOrDefault();
                if (rjrs != null)
                {
                    decr = rjrs.DESCRIPTION;
                }

            }
            else
                decr = "";
            var result = new { descval = decr };
            return Json(decr, JsonRequestBehavior.AllowGet);
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
                    AIRejectDesp = Convert.ToString(ds.Tables[0].Rows[0].ItemArray[0]);//-MICR Reject
                if (ds.Tables[0].Rows[0].ItemArray[1] != null && ds.Tables[0].Rows[0].ItemArray[1] != "")
                    AIRejectDesp = AIRejectDesp + "- " + Convert.ToString(ds.Tables[0].Rows[0].ItemArray[1]);// - Account Reject
                if (ds.Tables[0].Rows[0].ItemArray[2] != null && ds.Tables[0].Rows[0].ItemArray[2] != "")
                    AIRejectDesp = AIRejectDesp + "- " + Convert.ToString(ds.Tables[0].Rows[0].ItemArray[2]);//-- Amount Figure Reject
                if (ds.Tables[0].Rows[0].ItemArray[3] != null && ds.Tables[0].Rows[0].ItemArray[3] != "")
                    AIRejectDesp = AIRejectDesp + "- " + Convert.ToString(ds.Tables[0].Rows[0].ItemArray[3]);//--Amount Word Reject
                if (ds.Tables[0].Rows[0].ItemArray[4] != null && ds.Tables[0].Rows[0].ItemArray[4] != "")
                    AIRejectDesp = AIRejectDesp + "- " + Convert.ToString(ds.Tables[0].Rows[0].ItemArray[4]);//--Date Reject
                if (ds.Tables[0].Rows[0].ItemArray[5] != null && ds.Tables[0].Rows[0].ItemArray[5] != "")
                    AIRejectDesp = AIRejectDesp + "- " + Convert.ToString(ds.Tables[0].Rows[0].ItemArray[5]);//--Signature Reject

            }
            return Json(AIRejectDesp, JsonRequestBehavior.AllowGet);
            // return Json();
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
    }
}
