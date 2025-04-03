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
using Microsoft.SqlServer.Server;
using NPOI.Util;
using System.Net;
using System.IO;
using System.Net.Http;
using NLog;
using System.Drawing;
using Newtonsoft.Json.Linq;
using System.Web.Helpers;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;


namespace ikloud_Aflatoon.Controllers
{
    public class IWL1Controller : Controller
    {
        //
        // GET: /IWL1/
        AflatoonEntities iwafl = new AflatoonEntities();
        IWProcDataContext iwpro = new IWProcDataContext();
        AflatoonEntities af = new AflatoonEntities();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);

        string sInputString = ""; string sResposne = ""; string sgetAccountDetailsDBS = "";

        string sCasaClientId = "";
        string sCasaCorellationId = "";
        string sCasaServiceURL = "";
        string sAccountNo = "";

        List<string> lAccNames = new List<string>();

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


            string flag = null;
            string queue = null;
            string AmtValue = null;
            string ExpiryTime = null;

            AmtValue = Session["iwAmtSelection"].ToString();
            ExpiryTime = Session["iwSessionExpiryTime"].ToString();
            queue = Session["iwqueue"].ToString();
            flag = Session["iwFlag"].ToString();





            decimal fromamt = 0;
            decimal toamt = 0;
            var splitamt = AmtValue.Split('-');
            var splitedFromamt = splitamt[0];
            var splitedToamt=splitamt[1];

            if (splitedToamt == "") { toamt = 0; }
            if(splitedFromamt == "") { fromamt = 0; }

            if(splitedFromamt!="")
                fromamt =  Convert.ToDecimal(splitedFromamt);
            if(splitedToamt!="")
                toamt =  Convert.ToDecimal(splitedToamt);

            Session["fromamt"]= fromamt;
            Session["toamt"]= toamt;
           
           
               
           





            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            //if ((bool)Session["QC"] == false)
            //{

            //    UserMaster usrm = iwafl.UserMasters.Find(uid);
            //    usrm.Active = false;
            //    iwafl.SaveChanges();
            //    return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            //}
            try
            {


                //sod
                SqlDataAdapter adp111 = new SqlDataAdapter("GetIwSod_DBS", con);
                adp111.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp111.SelectCommand.Parameters.Add("@CustomerId", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                adp111.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"); ;
                DataSet ds111 = new DataSet();
                adp111.Fill(ds111);
                if (ds111.Tables[0].Rows.Count > 0)
                {
                    Session["PostDate"] = ds111.Tables[0].Rows[0].ItemArray[1];
                    Session["StaleDate"] = ds111.Tables[0].Rows[0].ItemArray[0];
                }

                //sod end







                SqlDataAdapter adp = new SqlDataAdapter("IWL1Verification", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@procDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                adp.SelectCommand.Parameters.Add("@ClrType", SqlDbType.NVarChar).Value = Session["CtsSessionType"].ToString();// "01";//clrtype;
                //--------------------Customer Selection---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);

                adp.SelectCommand.Parameters.Add("@FromAmount", SqlDbType.Decimal).Value = fromamt;
                adp.SelectCommand.Parameters.Add("@ToAmount", SqlDbType.Decimal).Value = toamt;
                adp.SelectCommand.Parameters.Add("@ItemExpiryTime", SqlDbType.VarChar).Value = ExpiryTime;
                adp.SelectCommand.Parameters.Add("@Queue", SqlDbType.VarChar).Value = queue;



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
                        ActualAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[2]),//0,
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

                        // FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[14].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[14].ToString(),
                        CBSClientAccountDtls = ds.Tables[0].Rows[0].ItemArray[15].ToString(),
                        CBSJointHoldersName = ds.Tables[0].Rows[0].ItemArray[16].ToString(),
                        //BackTiffImagePath = ds.Tables[0].Rows[0].ItemArray[17].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),

                        BackTiffImagePath = ds.Tables[0].Rows[0].ItemArray[17].ToString(),
                        XMLPayeeName = ds.Tables[0].Rows[0].ItemArray[18].ToString(),
                        StrModified = ds.Tables[0].Rows[0].ItemArray[19].ToString(),
                        // AiPayeeName= ds.Tables[0].Rows[0].ItemArray[19].ToString(),
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
                            ActualAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[2]),//0,
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
                            // FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[14].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),

                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[14].ToString(),
                            CBSClientAccountDtls = ds.Tables[0].Rows[index].ItemArray[15].ToString(),
                            CBSJointHoldersName = ds.Tables[0].Rows[index].ItemArray[16].ToString(),
                            // BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[17].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                            BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[17].ToString(),
                            XMLPayeeName = ds.Tables[0].Rows[index].ItemArray[18].ToString(),
                            StrModified = ds.Tables[0].Rows[index].ItemArray[19].ToString(),
                            //AiPayeeName = ds.Tables[0].Rows[index].ItemArray[19].ToString(),
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

                    ExtensionRejectReason();

                    Session["glob"] = null;
                    ViewBag.cnt = true;
                    return View(objectlst);
                }
                else
                    return RedirectToAction("Index", "IwVerSelection", new { Flg = "L1", id=1 });
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                string trace = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                {
                    innerExcp = e.InnerException.Message;
                    trace = e.InnerException.StackTrace;
                }
                logerror("In IWL1 Index GET Catch==>>" + message, "InnerExp===>>" + innerExcp);
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
                    ttcnt = lst.Count() / 22;

                int stu=0;
                string resncode = "0";
                string rejctdecrptn = "";

                string exresncode = "0";
                string exrejctdecrptn = "";

                string cbdclnts = "";
                string cbdJointdtls = "";
                string PayeeName = "";
                string FinalModified = "";

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
                    //if (Convert.ToBoolean(lst[8]) == true)
                    //    stu = 1;
                    //else
                    //{
                    //    stu = 0;
                    //}

                    if (lst[8] != null)
                    {
                        if (lst[8].ToString() == "A")
                        {
                            stu = 1;
                        }
                        else if(lst[8].ToString() == "R")
                        {
                            stu = 2;
                        }
                        else if( lst[8].ToString() == "E")
                        {
                            stu = 3;
                        }
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

                    if (lst[17] != null)
                        if (lst[17].ToString() != "")
                            PayeeName = lst[17].ToString();

                    if (lst[18]!=null)
                        if (lst[18].ToString()!="")
                            rejctdecrptn=lst[18].ToString();


                    if (lst[19] != null)
                        if (lst[19].ToString() != "")
                            exresncode = lst[19].ToString();

                    if (lst[20] != null)
                        if (lst[20].ToString() != "")
                            exrejctdecrptn = lst[20].ToString();

                    if(lst[21] != null)
                        if (lst[21].ToString()!="")
                            FinalModified = lst[21].ToString();

                    Int64 id = Convert.ToInt64(lst[0]);

                   // iwpro.UpdateIWL1Verification(id, uid, lst[4].ToString(), lst[5].ToString(), lst[6].ToString(), lst[7].ToString(), lst[1].ToString(), Convert.ToDouble(lst[2]), lst[3].ToString(), stu, resncode, rejctdecrptn, cbdclnts, cbdJointdtls, @Session["LoginID"].ToString());

                    UpdateIWL1Verification(id, uid, lst[4].ToString(), lst[5].ToString(), lst[6].ToString(), lst[7].ToString(), lst[1].ToString(), Convert.ToDouble(lst[2]), lst[3].ToString(), stu, resncode, rejctdecrptn, cbdclnts, cbdJointdtls, @Session["LoginID"].ToString(), PayeeName,exresncode,exrejctdecrptn, FinalModified);


                    for (int k = 0; k < idlst.Count; k++)
                    {
                        if (idlst[k] == id)
                            idlst.RemoveAt(k);
                    }
                    lst.RemoveRange(0, 22);
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
                adp.SelectCommand.Parameters.Add("@ClrType", SqlDbType.NVarChar).Value = Session["CtsSessionType"].ToString();// "01";//clrtype;
                //--------------------Customer Selection---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);

                adp.SelectCommand.Parameters.Add("@FromAmount", SqlDbType.Float).Value = Session["fromamt"];
                adp.SelectCommand.Parameters.Add("@ToAmount", SqlDbType.Float).Value = Session["toamt"];
                adp.SelectCommand.Parameters.Add("@ItemExpiryTime", SqlDbType.VarChar).Value = Session["iwSessionExpiryTime"].ToString();
                adp.SelectCommand.Parameters.Add("@Queue", SqlDbType.VarChar).Value = Session["iwqueue"].ToString();





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
                            //if (Convert.ToBoolean(lst[8]) == true)
                            //    stu = 1;
                            //else
                            //    stu = 0;


                            if (lst[8] != null)
                            {
                                if (lst[8].ToString() == "A")
                                {
                                    stu = 1;
                                }
                                else if (lst[8].ToString() == "R")
                                {
                                    stu = 2;
                                }
                                else if (lst[8].ToString() == "E")
                                {
                                    stu = 3;
                                }
                            }


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

                            if (lst[17] != null)
                                if (lst[17].ToString() != "")
                                    PayeeName = lst[17].ToString();

                            if (lst[18] != null)
                                if (lst[18].ToString() != "")
                                    rejctdecrptn = lst[18].ToString();

                            if (lst[19] != null)
                                if (lst[19].ToString() != "")
                                    exresncode = lst[19].ToString();

                            if (lst[20] != null)
                                if (lst[20].ToString() != "")
                                    exrejctdecrptn = lst[20].ToString();

                            if (lst[21] != null)
                                if (lst[21].ToString()!="")
                                    FinalModified = lst[21].ToString();

                            Int64 id = Convert.ToInt64(lst[0]);


                            //iwpro.UpdateIWL1Verification(id, uid, lst[4].ToString(), lst[5].ToString(), lst[6].ToString(), lst[7].ToString(), lst[1].ToString(), Convert.ToDouble(lst[2]), lst[3].ToString(), stu, resncode, rejctdecrptn, cbdclnts, cbdJointdtls, @Session["LoginID"].ToString());

                            UpdateIWL1Verification(id, uid, lst[4].ToString(), lst[5].ToString(), lst[6].ToString(), lst[7].ToString(), lst[1].ToString(), Convert.ToDouble(lst[2]), lst[3].ToString(), stu, resncode, rejctdecrptn, cbdclnts, cbdJointdtls, @Session["LoginID"].ToString(), PayeeName,exresncode,exrejctdecrptn, FinalModified);


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

                    if (lst[17] != null)
                        if (lst[17].ToString() != "")
                            PayeeName = lst[17].ToString();

                    if (lst[18] != null)
                        if (lst[18].ToString() != "")
                            rejctdecrptn = lst[18].ToString();

                    if (lst[19] != null)
                        if (lst[19].ToString() != "")
                            exresncode = lst[19].ToString();

                    if (lst[20] != null)
                        if (lst[20].ToString() != "")
                            exrejctdecrptn = lst[20].ToString();

                    if(lst[21] != null)
                        if (lst[21].ToString()!="")
                            FinalModified=lst[21].ToString();

                    def = new IWTempL1VerificationModel
                    {
                        ID = Convert.ToInt64(lst[0]),
                        FrontGreyImagePath = img,
                        DbtAccountNo = lst[1].ToString(),
                        ActualAmount = Convert.ToDecimal(lst[2]),
                        Date = lst[3].ToString(),
                        sttsdtqc = lst[8].ToString(),//Convert.ToBoolean(lst[8]),
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
                        XMLPayeeName=PayeeName,
                        RejectDescription=rejctdecrptn,
                        ExRejectReason=exresncode,
                        ExRejectDescription=exrejctdecrptn,
                        StrModified = FinalModified,


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
                                ActualAmount= Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[2]),// 0,
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
                                // FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[14].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                                FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[14].ToString(),

                                CBSClientAccountDtls = ds.Tables[0].Rows[index].ItemArray[15].ToString(),
                                CBSJointHoldersName = ds.Tables[0].Rows[index].ItemArray[16].ToString(),
                                //BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[17].ToString().Replace("tif", "jpg").Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                                BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[17].ToString(),
                                XMLPayeeName = ds.Tables[0].Rows[index].ItemArray[18].ToString(),
                                StrModified = ds.Tables[0].Rows[index].ItemArray[19].ToString(),
                                // AiPayeeName = ds.Tables[0].Rows[index].ItemArray[19].ToString(),
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

                string message = "";
                string innerExcp = "";
                string trace = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                {
                    innerExcp = e.InnerException.Message;
                    trace = e.InnerException.StackTrace;
                }
                logerror("In IWL1 Index Post Catch==>>" + message, "InnerExp===>>" + innerExcp);
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
        //[HttpPost]
        //public PartialViewResult GetCBSDtls(string ac = null, string strcbsdls = null, string strJoinHldrs = null)
        //{
        //    cbstetails model = new cbstetails();
        //    try
        //    {

        //        if (ac != null)
        //        {
        //            if (Session["GetAccountDetails "].ToString().ToUpper() == "L")
        //            {
        //                //var model
        //                if (ac.Length == 6)
        //                {
        //                    //model = (from c in iwafl.ACDetails
        //                    //         where c.Cbsdtls.Contains(ac)
        //                    //         select new cbstetails
        //                    //         {
        //                    //             cbsdls = c.Cbsdtls,
        //                    //             JoinHldrs = c.JoinHldrs,
        //                    //             Account = c.Ac
        //                    //         }
        //                    //).SingleOrDefault();

        //                    try
        //                    {
        //                        var model1 = (from c in iwafl.ACDetails
        //                                      where c.Cbsdtls.Contains(ac)
        //                                      select c
        //                                      ).ToList();

        //                        if (model1.Count > 1)                                
        //                            model = null;

        //                        else
        //                        {
        //                            model = (from c in iwafl.ACDetails
        //                                     where c.Cbsdtls.Contains(ac)
        //                                     select new cbstetails
        //                                     {
        //                                         cbsdls = c.Cbsdtls,
        //                                         JoinHldrs = c.JoinHldrs,
        //                                         Account = c.Ac
        //                                     }
        //                                   ).FirstOrDefault();
        //                        }

        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        model = null;

        //                    }





        //                }
        //                else
        //                {
        //                    //model = (from c in iwafl.ACDetails
        //                    //         where c.Ac == ac
        //                    //         select new cbstetails
        //                    //         {
        //                    //             cbsdls = c.Cbsdtls,
        //                    //             JoinHldrs = c.JoinHldrs,
        //                    //         }
        //                    //).SingleOrDefault();

        //                    try
        //                    {
        //                        model = (from c in iwafl.ACDetails
        //                                 where c.Ac == ac
        //                                 select new cbstetails
        //                                 {
        //                                     cbsdls = c.Cbsdtls,
        //                                     JoinHldrs = c.JoinHldrs,
        //                                 }
        //                                                 ).First();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        model = null;

        //                    }


        //                }

        //            }
        //            else if (Session["GetAccountDetails "].ToString().ToUpper() == "C")
        //            {
        //                //---------For CBS Bank----------------

        //                iwpro.GetOnlyIWUpdateCBSAccInfo(ac, 1, ref strcbsdls, ref strJoinHldrs);
        //                model.cbsdls = strcbsdls;
        //                model.JoinHldrs = strJoinHldrs;
        //            }

        //            if (model != null && model.cbsdls != null)
        //            {
        //                if (model.cbsdls.Split('|').ElementAt(1) == "S")
        //                {
        //                    if (model.cbsdls.Split('|').ElementAt(5).Trim() != "")
        //                    {
        //                        string MOP = iwafl.MOPCodeMasters.Find(model.cbsdls.Split('|').ElementAt(5)).Description;
        //                        model.MOP = MOP != null ? MOP : "";
        //                    }
        //                    else
        //                    {
        //                        model.MOP = "";
        //                    }
        //                    if (model.cbsdls.Split('|').ElementAt(6).Trim() != "")
        //                    {
        //                        string AccountStatus = iwafl.AccStatusCodeMasters.Find(model.cbsdls.Split('|').ElementAt(6)).Description;
        //                        model.AccountStatus = AccountStatus != null ? AccountStatus : "";
        //                    }
        //                    else
        //                    {
        //                        model.AccountStatus = "";
        //                    }

        //                    if (model.cbsdls.Split('|').ElementAt(12).Trim() != "")
        //                    {
        //                        string AccountOwnership = iwafl.AccOwnershipCodeMasters.Find(model.cbsdls.Split('|').ElementAt(12).ToString()).Description;
        //                        model.AccountOwnership = AccountOwnership != null ? AccountOwnership : "";
        //                    }
        //                    else
        //                    {
        //                        model.AccountOwnership = "";
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                cbstetails Tempcbdtls = new cbstetails();
        //                Tempcbdtls.cbsdls = "|F|Account does not exist";
        //                model = Tempcbdtls;
        //            }
        //            return PartialView("_GetCBSDtls", model);
        //        }
        //        else
        //        {
        //            // cbstetails model = new cbstetails();
        //            model.cbsdls = strcbsdls;
        //            model.JoinHldrs = strJoinHldrs;
        //            if (model.cbsdls != null)
        //            {
        //                if (model.cbsdls.Split('|').ElementAt(1) == "S")
        //                {
        //                    if (model.cbsdls.Split('|').ElementAt(5).Trim() != "")
        //                    {
        //                        string MOP = iwafl.MOPCodeMasters.Find(model.cbsdls.Split('|').ElementAt(5)).Description;
        //                        model.MOP = MOP != null ? MOP : "";
        //                    }
        //                    else
        //                    {
        //                        model.MOP = "";
        //                    }
        //                    if (model.cbsdls.Split('|').ElementAt(6).Trim() != "")
        //                    {
        //                        string AccountStatus = iwafl.AccStatusCodeMasters.Find(model.cbsdls.Split('|').ElementAt(6)).Description;
        //                        model.AccountStatus = AccountStatus != null ? AccountStatus : "";
        //                    }
        //                    else
        //                    {
        //                        model.AccountStatus = "";
        //                    }

        //                    if (model.cbsdls.Split('|').ElementAt(12).Trim() != "")
        //                    {
        //                        string AccountOwnership = iwafl.AccOwnershipCodeMasters.Find(model.cbsdls.Split('|').ElementAt(12).ToString()).Description;
        //                        model.AccountOwnership = AccountOwnership != null ? AccountOwnership : "";
        //                    }
        //                    else
        //                    {
        //                        model.AccountOwnership = "";
        //                    }
        //                }
        //                else
        //                {
        //                    cbstetails Tempcbdtls = new cbstetails();
        //                    Tempcbdtls.cbsdls = "|F|Account does not exist";
        //                    model = Tempcbdtls;
        //                }
        //            }
        //            //else
        //            //{
        //            //    cbstetails cbdtls = new cbstetails();
        //            //    cbdtls.cbsdls = "|F|Account does not exist";
        //            //    model = cbdtls;
        //            //}
        //            return PartialView("_GetCBSDtls", model);
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //        ErrorDisplay er = new ErrorDisplay();
        //        string ServerPath = "";
        //        string filename = "";
        //        string fileNameWithPath = "";
        //        //FormsAuthentication.SignOut();
        //        Session.Abandon();

        //        //ViewBag.Error = e.InnerException;
        //        er.ErrorMessage = e.InnerException.Message;
        //        //-------------------------------- ServerPath = Server.MapPath("~/Logs/");----
        //        if (System.IO.Directory.Exists(ServerPath) == false)
        //        {
        //            System.IO.Directory.CreateDirectory(ServerPath);
        //        }
        //        filename = DateTime.Now.ToString("ddMMyyyy") + "Logs.txt";
        //        fileNameWithPath = ServerPath + filename;
        //        System.IO.StreamWriter str = new System.IO.StreamWriter(fileNameWithPath, true, System.Text.Encoding.Default);
        //        //  str.WriteLine("hello");
        //        str.WriteLine(DateTime.Now.ToShortTimeString() + ": Exception: " + e.Message);
        //        str.WriteLine(DateTime.Now.ToShortTimeString() + ": StackTrace: " + er.ErrorMessage);
        //        str.Close();
        //        return PartialView("Error", er);
        //        // return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
        //        //return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
        //    }
        //}



        public PartialViewResult ExtensionRejectReason(int id = 0)
        {
            var rjrs = new List<RejectReason>();

            
                con.Open();
                using (SqlCommand cmd = new SqlCommand("GetExtensionRejectReasons", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var rejectReason = new RejectReason
                            {
                                Description = reader["DESCRIPTION"].ToString(),
                                ReasonCodeS = reader["RETURN_REASON_CODE"].ToString()
                            };
                            rjrs.Add(rejectReason);
                        }
                    }
                }
            ViewBag.rtnlistex = rjrs.Select(m => m.ReasonCodeS).ToList();
            ViewBag.rtnlistDescrpex = rjrs.Select(m => m.Description).ToList();

            return PartialView("_ExRejectReason", rjrs);
        }




        [HttpPost]
        public PartialViewResult GetCBSDtls1(string ac = null, string strcbsdls = null, string strJoinHldrs = null)
        {
            cbstetails model = new cbstetails();
            try
            {
                ac = "032110000000104";
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
                                var MOP = iwafl.MOPCodeMasters.Find(model.cbsdls.Split('|').ElementAt(5));
                                model.MOP = MOP != null ? MOP.Description : "";
                            }
                            else
                            {
                                model.MOP = "";
                            }
                            if (model.cbsdls.Split('|').ElementAt(6).Trim() != "")
                            {
                                var AccountStatus = iwafl.AccStatusCodeMasters.Find(model.cbsdls.Split('|').ElementAt(6));
                                model.AccountStatus = AccountStatus != null ? AccountStatus.Description : "";
                            }
                            else
                            {
                                model.AccountStatus = "";
                            }

                            model.AccountOwnership = "";
                            //if (model.cbsdls.Split('|').ElementAt(12).Trim() != "")
                            //{
                            //    var AccountOwnership = iwafl.AccOwnershipCodeMasters.Find(model.cbsdls.Split('|').ElementAt(12).ToString());
                            //    model.AccountOwnership = AccountOwnership != null ? AccountOwnership.Description : "";
                            //}
                            //else
                            //{
                            //    model.AccountOwnership = "";
                            //}
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

                            model.AccountStatus = "";
                            //if (model.cbsdls.Split('|').ElementAt(6).Trim() != "")
                            //{
                            //    string AccountStatus = iwafl.AccStatusCodeMasters.Find(model.cbsdls.Split('|').ElementAt(6)).Description;
                            //    model.AccountStatus = AccountStatus != null ? AccountStatus : "";
                            //}
                            //else
                            //{
                            //    model.AccountStatus = "";
                            //}

                            model.AccountOwnership = "";
                            //if (model.cbsdls.Split('|').ElementAt(12).Trim() != "")
                            //{
                            //    string AccountOwnership = iwafl.AccOwnershipCodeMasters.Find(model.cbsdls.Split('|').ElementAt(12).ToString()).Description;
                            //    model.AccountOwnership = AccountOwnership != null ? AccountOwnership : "";
                            //}
                            //else
                            //{
                            //    model.AccountOwnership = "";
                            //}
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


        //cbs details api call start
        //old name==GetCBSDetailsWithAPI
        [HttpPost]
        public ActionResult GetCBSDtls(string ac = null,Int64 id=0) //, List<string> lst=null
        {
            //logerror("Calling GetCBSDetailsWithAPI method start : ", "");
            cbstetails cbsdtls = new cbstetails();
            CPPS_NewAccFlag ob=new CPPS_NewAccFlag();
            //string act = "0001234567890";
            try
            {
                //vikram
                cbsdtls.sInvalidAcFlag = "F";

                if (ac != null && ac != "")
                {
                    string NewApiCall = null;
                    var OwApi = af.CommonSettings.FirstOrDefault(a => a.AppName == "CTSCONFIG1" && a.SettingName == "OWVerAPI")?.SettingValue;
                   
                    if (OwApi != null && OwApi != "")
                    {
                        NewApiCall = OwApi.ToString().ToUpper();
                    }
                    else
                        NewApiCall = "N";


                    ViewBag.NewApiCallIWL1 = NewApiCall;

                    if (NewApiCall == "Y")
                    {

                        string sBankNm = "DBS";
                        
                            cbsdtls.NREFlag = null;

                            //CASA variables
                            var CasaClientId = ConfigurationManager.AppSettings["CasaClientId"].ToString();
                            var CasaCorellationId = ConfigurationManager.AppSettings["CasaCorellationId"].ToString();
                            var CasaServiceURL = ConfigurationManager.AppSettings["CasaServiceURL"].ToString();

                            sCasaClientId = CasaClientId;
                            sCasaCorellationId = CasaCorellationId;
                            sCasaServiceURL = CasaServiceURL;
                            sAccountNo = ac;

                            //Token variables
                            var TokenClientId = ConfigurationManager.AppSettings["TokenClientId"].ToString();
                            var TokenSecreteKey = ConfigurationManager.AppSettings["TokenSecreteKey"].ToString();
                            var TokenServiceURL = ConfigurationManager.AppSettings["TokenServiceURL"].ToString();

                            //CMPC variables
                            var CMCPCountry = ConfigurationManager.AppSettings["CMCPCountry"].ToString();
                            var CMCPReqUID = ConfigurationManager.AppSettings["CMCPReqUID"].ToString();
                            var CMCPReqClientId = ConfigurationManager.AppSettings["CMCPReqClientId"].ToString();
                            var CMCPServiceURL = ConfigurationManager.AppSettings["CMCPServiceURL"].ToString();


                            cbsdtls.sacct_status = "";
                            cbsdtls.sFreezeStatusCode = "";
                            cbsdtls.NREFlag = "";

                            cbsdtls.sCAPA = "";
                           // logerror("Calling GetToken method start : ", "");
                            //=========== 2 uncomment when deployed on bank start ===============
                            //Get Token 
                            string sEtoken = GetToken();
                           // logerror("Calling GetToken method end : ", "");
                          // logerror("sEtoken value : ", sEtoken);
                            //=========== 2 uncomment when deployed on bank end ===============

                            
                           
                            long openDate = 0;

                            if (ac.Trim() == "999999999999" || ac.Trim() == "9999999999999999")
                            {
                                cbsdtls.sCAPA = "Invalid Account";

                                cbsdtls.status = "SUCCESS";
                                cbsdtls.cbsdls = null;
                                cbsdtls.PayeeNameList = null;
                                cbsdtls.sInvalidAcFlag = "T";
                                cbsdtls.Invalidtbl = "E";
                            }
                            else
                            {
                                //======= For Local Testing Open =================
                                //sgetAccountDetailsDBS = getAccountDetailsDBSResponse(CasaServiceURL, CasaClientId, CasaCorellationId, ac); //local testing

                                //sgetAccountDetailsDBS = getCustomerId(CasaServiceURL, CasaClientId, CasaCorellationId, ac);
                                //sgetAccountDetailsDBS = getAccountDetailsDBSInvalid(CasaServiceURL, CasaClientId, CasaCorellationId, ac);
                               // logerror("Calling getAccountDetailsDBSRequest method start : ", "");

                                //========= 3 uncomment For DBS Open start ==========
                                sgetAccountDetailsDBS = getAccountDetailsDBSRequest(CasaServiceURL, CasaClientId, CasaCorellationId, ac.ToUpper(), sEtoken); // dbs testing
                                 var newResponse = sgetAccountDetailsDBS.Replace(", Please", " Please");
                               // logerror("Calling getAccountDetailsDBSRequest method end : ", "");
                               // logerror("sgetAccountDetailsDBS : ", sgetAccountDetailsDBS);
                              //  logerror("sgetAccountDetailsDBS ==> newResponse==> : ", newResponse);
                                var jObject = Newtonsoft.Json.Linq.JObject.Parse(newResponse);

                                // logerror("sgetAccountDetailsDBS ==> jObject==> : ", jObject.ToString());
                                // logerror("sgetAccountDetailsDBS : ", "LN1308");

                                Session["JobjectForSignature"] = jObject;
                               // logerror("sgetAccountDetailsDBS : ", "LN1311");
                                cbsdtls.Invalidtbl = "";
                            if (jObject["error"] != null)
                            {


                                cbsdtls.sCAPA = "Invalid Account";
                                cbsdtls.PayeeNameList = null;
                                cbsdtls.status = "SUCCESS";
                                cbsdtls.cbsdls = null;
                                cbsdtls.sInvalidAcFlag = "T";
                                cbsdtls.Invalidtbl = "E";
                            }
                            else
                            {
                                if (jObject["accountClosedFlag"].ToString().Trim().ToUpper() != "Y")
                                {

                                    if (jObject["accountStatus"] != null)
                                    {
                                        logerror("in cbs details LN=1325 accountStatus==>", jObject["accountStatus"].ToString());
                                        if (jObject["accountStatus"].ToString().Trim() == "Active")
                                        {
                                            if (jObject["accountName"] != null)
                                            {
                                                //lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                //if (lAccNames.Count > 0)
                                                //    cbsdtls.PayeeNameList = lAccNames;

                                                if (((Newtonsoft.Json.Linq.JContainer)jObject["relatedCustomerInfo"]).Count > 0)
                                                {
                                                    int iIndex = 0;

                                                    while (iIndex < jObject["relatedCustomerInfo"].Count())
                                                    {
                                                        //Call for account holders

                                                        //====== 4 uncomment when deployed on bank start ===================
                                                        string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, sEtoken, jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                        //string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, "", jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                        //====== 4 uncomment when deployed on bank end ===================
                                                        logerror("In GetCBSDetailsWithAPI Ac", "Active and sCMPCResponse - " + sCMPCResponse);
                                                        //Get account holders
                                                        string sCustomerName = GetCMCPCustomerName(sCMPCResponse);
                                                        //string sCustomerName = getCustomerName(sCMPCResponse);
                                                        logerror("In GetCBSDetailsWithAPI Ac", "Active and GetCMCPCustomerName - " + sCustomerName);
                                                        //var jObject1 = Newtonsoft.Json.Linq.JObject.Parse(sCustomerName);
                                                        //string sname = jObject1["data"]["profileInfo"]["registeredName"].ToString().Trim();

                                                        if (sCustomerName != "")
                                                        {
                                                            lAccNames.Add(sCustomerName);
                                                        }

                                                        logerror("Joint Account Name : ", sCustomerName.ToString());
                                                        iIndex++;
                                                    }

                                                    if (lAccNames.Count > 0)
                                                        cbsdtls.PayeeNameList = lAccNames;
                                                }
                                                else
                                                {
                                                    lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                    if (lAccNames.Count > 0)
                                                        cbsdtls.PayeeNameList = lAccNames;
                                                }

                                                if (lAccNames.Count == 0)
                                                {
                                                    lAccNames.Add(jObject["accountName"].ToString().Trim());

                                                    if (lAccNames.Count > 0)
                                                        cbsdtls.PayeeNameList = lAccNames;
                                                }

                                                cbsdtls.status = "SUCCESS";
                                                cbsdtls.cbsdls = null;
                                                cbsdtls.sacct_status = jObject["accountStatus"].ToString().Trim();
                                                cbsdtls.sFreezeStatusCode = jObject["freezeStatusCode"].ToString().Trim();
                                                cbsdtls.NREFlag = jObject["productCode"] == null ? "" : jObject["productCode"].ToString().Trim();
                                                //cbsdtls.NREFlag = jObject["productName"].ToString().Trim();

                                                cbsdtls.sCAPA = jObject["productName"].ToString().Trim();

                                                Session["sacct_status"] = jObject["accountStatus"].ToString().Trim();
                                                Session["sNREFlag"] = jObject["productName"].ToString().Trim();

                                                if (jObject["productCode"].ToString().Trim() == "NRESA" || jObject["productCode"].ToString().Trim() == "NROSA"
                                                        || jObject["productCode"].ToString().Trim() == "NRESP" || jObject["productCode"].ToString().Trim() == "NROSP"
                                                        || jObject["productCode"].ToString().Trim() == "NRETR" || jObject["productCode"].ToString().Trim() == "NROTR"
                                                        || jObject["productCode"].ToString().Trim() == "NRET1" || jObject["productCode"].ToString().Trim() == "NROT1"
                                                        || jObject["productCode"].ToString().Trim() == "NRET3" || jObject["productCode"].ToString().Trim() == "NEPIS"
                                                        || jObject["productCode"].ToString().Trim() == "NOPIS" || jObject["productCode"].ToString().Trim() == "NREWL"
                                                        || jObject["productCode"].ToString().Trim() == "NROWL" || jObject["productCode"].ToString().Trim() == "NRSAV"
                                                        || jObject["productCode"].ToString().Trim() == "SFNRE" || jObject["productCode"].ToString().Trim() == "SFNRO"
                                                        || jObject["productCode"].ToString().Trim() == "NROT3")
                                                {
                                                    cbsdtls.sInvalidAcFlag = "N";
                                                    //ViewBag.vbNRE = "NRE Account";
                                                    ViewBag.vbNRE = "N";

                                                    if (jObject["productCode"].ToString().Trim() == "NRESA" || jObject["productCode"].ToString().Trim() == "NRESP"
                                                            || jObject["productCode"].ToString().Trim() == "NRETR" || jObject["productCode"].ToString().Trim() == "NRET1"
                                                            || jObject["productCode"].ToString().Trim() == "NRET3" || jObject["productCode"].ToString().Trim() == "NEPIS"
                                                            || jObject["productCode"].ToString().Trim() == "NREWL" || jObject["productCode"].ToString().Trim() == "NRSAV"
                                                            || jObject["productCode"].ToString().Trim() == "SFNRE")
                                                        Session["sNR"] = "NRE";
                                                    else if (jObject["productCode"].ToString().Trim() == "NROSA" || jObject["productCode"].ToString().Trim() == "NROSP"
                                                            || jObject["productCode"].ToString().Trim() == "NROTR" || jObject["productCode"].ToString().Trim() == "NROT1"
                                                            || jObject["productCode"].ToString().Trim() == "NOPIS" || jObject["productCode"].ToString().Trim() == "NROWL"
                                                            || jObject["productCode"].ToString().Trim() == "SFNRO" || jObject["productCode"].ToString().Trim() == "NROT3")
                                                        Session["sNR"] = "NRO";

                                                }
                                                else
                                                {
                                                    Session["sNR"] = "";
                                                }


                                                //opendate
                                                if (jObject["openedDate"] != null)
                                                {
                                                    openDate = Convert.ToInt64(jObject["openedDate"].ToString().Trim());
                                                }
                                                else
                                                {
                                                    openDate = 0;
                                                }

                                                if (openDate != 0)
                                                {
                                                    DateTimeOffset dateTimeFromUnix = DateTimeOffset.FromUnixTimeMilliseconds(openDate);
                                                    DateTimeOffset currentDateTime = DateTimeOffset.Now;
                                                    int differenceInMonths = CalculateDifferenceInMonths(currentDateTime, dateTimeFromUnix);

                                                    if (differenceInMonths > 6)
                                                    {
                                                        //Console.WriteLine("The difference is greater than six months.");
                                                        Session["IsOpenedDateOld"] = "Y";
                                                        cbsdtls.acOpenDate = "Y";
                                                    }
                                                    else
                                                    {
                                                        //Console.WriteLine("The difference is not greater than six months.");
                                                        Session["IsOpenedDateOld"] = "N";
                                                        cbsdtls.acOpenDate = "N";
                                                    }
                                                }



                                                //mop 
                                                if (jObject["modeOfOperation"] != null)
                                                {
                                                    cbsdtls.MOP = jObject["modeOfOperation"].ToString().Trim();
                                                }
                                                if (jObject["staffIndicator"] != null)
                                                {
                                                    cbsdtls.StaffAcc = (bool)jObject["staffIndicator"] ? "Y" : "N";
                                                }
                                                if (jObject["accountStatus"] != null)
                                                {
                                                    cbsdtls.sacct_status = jObject["accountStatus"].ToString();
                                                }
                                                if (jObject["freezeStatusCode"] != null)
                                                {
                                                    cbsdtls.sFreezeStatusCode = jObject["freezeStatusCode"].ToString();
                                                }
                                                if (jObject["productCode"] != null)
                                                {
                                                    cbsdtls.productCode = jObject["productCode"].ToString().Trim();
                                                }
                                                if (jObject["productType"] != null)
                                                {
                                                    cbsdtls.productType = jObject["productType"].ToString().Trim();
                                                }
                                                if (jObject["accountCurrencyCode"] != null)
                                                {
                                                    cbsdtls.accountCurrencyCode = jObject["accountCurrencyCode"].ToString().Trim();
                                                }

                                                if (jObject["accountBalances"] != null)
                                                {
                                                    Session["accountBalances"] = jObject["accountBalances"]["effectiveAvailableAmount"] == null ? "0" :
                                                                                jObject["accountBalances"]["effectiveAvailableAmount"].ToString().Trim();
                                                    cbsdtls.ACBALAmount = jObject["accountBalances"]["effectiveAvailableAmount"] == null ? "0" :
                                                                                jObject["accountBalances"]["effectiveAvailableAmount"].ToString().Trim();
                                                }
                                                else
                                                {
                                                    Session["accountBalances"] = "0";
                                                    cbsdtls.ACBALAmount = "0";
                                                }


                                                if (jObject["sourceCustomerId"] != null)
                                                {
                                                    cbsdtls.SourceCustomerId = jObject["sourceCustomerId"].ToString().Trim();
                                                }

                                                if (jObject["branchCode"] != null)
                                                {
                                                    cbsdtls.SolId = jObject["branchCode"].ToString().Trim();
                                                }

                                                if(jObject["productType"] != null)
                                                {
                                                    cbsdtls.OffAcc = jObject["productType"].ToString() == "OAB" ? "Y" : "N";
                                                }


                                                //string to store api data
                                                //                      sourceCustomerId|accountCurrencyCode|openedDate|productCode|productType|accountBalances|modeOfOperation|accountStatus|staffIndicator|freezeStatusCode|openDate|SOLID|offAcc
                                                cbsdtls.ApiDataString = cbsdtls.SourceCustomerId + "|" + cbsdtls.accountCurrencyCode + "|" + cbsdtls.acOpenDate + "|" + cbsdtls.productCode + "|" + cbsdtls.productType + "|" + cbsdtls.ACBALAmount + "|" + cbsdtls.MOP + "|" + cbsdtls.sacct_status + "|" + cbsdtls.StaffAcc + "|" + cbsdtls.sFreezeStatusCode + "|" + openDate + "|" + cbsdtls.SolId + "|" + cbsdtls.OffAcc;
                                                logerror("in cbsdetails==> value for ApiDataString==> " + cbsdtls.ApiDataString.ToString(), "===LN1509");

                                            }
                                            else
                                            {
                                                cbsdtls.sCAPA = "Invalid Account";
                                                cbsdtls.PayeeNameList = null;
                                                cbsdtls.status = "SUCCESS";
                                                cbsdtls.cbsdls = null;
                                                cbsdtls.sInvalidAcFlag = "T";
                                            }

                                            if (jObject["freezeStatusCode"].ToString().Trim() == "T")
                                            {
                                                cbsdtls.sCAPA = "Account is Total freeze";

                                                //cbsdtls.PayeeNameList = null;
                                               // cbsdtls.sInvalidAcFlag = "T";
                                                cbsdtls.sInvalidAcFlag = "F"; //allowing total freze 21/12/24
                                            }
                                            else if (jObject["freezeStatusCode"].ToString().Trim() == "C")
                                            {
                                                cbsdtls.sCAPA = "Account is Credit freeze";

                                                //cbsdtls.PayeeNameList = null;
                                                cbsdtls.sInvalidAcFlag = "F";   //allowing credit freze 12/12/24
                                            }
                                            else if (jObject["freezeStatusCode"].ToString().Trim() == "D")
                                            {
                                                cbsdtls.sCAPA = "Account is Debit freeze";

                                                //cbsdtls.PayeeNameList = null;
                                                cbsdtls.sInvalidAcFlag = "F";  ////allowing Debit freze 21/12/24
                                            }

                                        }
                                        else if (jObject["accountStatus"].ToString().Trim() == "Dormant") //for dormant acc
                                        {
                                            if (jObject["accountName"] != null)
                                            {
                                                //lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                //if (lAccNames.Count > 0)
                                                //    cbsdtls.PayeeNameList = lAccNames;

                                                if (((Newtonsoft.Json.Linq.JContainer)jObject["relatedCustomerInfo"]).Count > 0)
                                                {
                                                    int iIndex = 0;

                                                    while (iIndex < jObject["relatedCustomerInfo"].Count())
                                                    {
                                                        //Call for account holders

                                                        //====== 4 uncomment when deployed on bank start ===================
                                                        string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, sEtoken, jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                        //string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, "", jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                        //====== 4 uncomment when deployed on bank end ===================
                                                        logerror("In GetCBSDetailsWithAPI Ac", "Active and sCMPCResponse - " + sCMPCResponse);
                                                        //Get account holders
                                                        string sCustomerName = GetCMCPCustomerName(sCMPCResponse);
                                                        //string sCustomerName = getCustomerName(sCMPCResponse);
                                                        logerror("In GetCBSDetailsWithAPI Ac", "Active and GetCMCPCustomerName - " + sCustomerName);
                                                        //var jObject1 = Newtonsoft.Json.Linq.JObject.Parse(sCustomerName);
                                                        //string sname = jObject1["data"]["profileInfo"]["registeredName"].ToString().Trim();

                                                        if (sCustomerName != "")
                                                        {
                                                            lAccNames.Add(sCustomerName);
                                                        }

                                                        logerror("Joint Account Name : ", sCustomerName.ToString());
                                                        iIndex++;
                                                    }

                                                    if (lAccNames.Count > 0)
                                                        cbsdtls.PayeeNameList = lAccNames;
                                                }
                                                else
                                                {
                                                    lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                    if (lAccNames.Count > 0)
                                                        cbsdtls.PayeeNameList = lAccNames;
                                                }

                                                if (lAccNames.Count == 0)
                                                {
                                                    lAccNames.Add(jObject["accountName"].ToString().Trim());

                                                    if (lAccNames.Count > 0)
                                                        cbsdtls.PayeeNameList = lAccNames;
                                                }

                                                cbsdtls.status = "SUCCESS";
                                                cbsdtls.cbsdls = null;
                                                cbsdtls.sacct_status = jObject["accountStatus"].ToString().Trim();
                                                cbsdtls.sFreezeStatusCode = jObject["freezeStatusCode"].ToString().Trim();
                                                //cbsdtls.NREFlag = jObject["productName"].ToString().Trim();
                                                cbsdtls.NREFlag = jObject["productCode"] == null ? "" : jObject["productCode"].ToString().Trim();

                                                cbsdtls.sCAPA = jObject["productName"].ToString().Trim();

                                                Session["sacct_status"] = jObject["accountStatus"].ToString().Trim();
                                                Session["sNREFlag"] = jObject["productName"].ToString().Trim();

                                                if (jObject["productCode"].ToString().Trim() == "NRESA" || jObject["productCode"].ToString().Trim() == "NROSA"
                                                        || jObject["productCode"].ToString().Trim() == "NRESP" || jObject["productCode"].ToString().Trim() == "NROSP"
                                                        || jObject["productCode"].ToString().Trim() == "NRETR" || jObject["productCode"].ToString().Trim() == "NROTR"
                                                        || jObject["productCode"].ToString().Trim() == "NRET1" || jObject["productCode"].ToString().Trim() == "NROT1"
                                                        || jObject["productCode"].ToString().Trim() == "NRET3" || jObject["productCode"].ToString().Trim() == "NEPIS"
                                                        || jObject["productCode"].ToString().Trim() == "NOPIS" || jObject["productCode"].ToString().Trim() == "NREWL"
                                                        || jObject["productCode"].ToString().Trim() == "NROWL" || jObject["productCode"].ToString().Trim() == "NRSAV"
                                                        || jObject["productCode"].ToString().Trim() == "SFNRE" || jObject["productCode"].ToString().Trim() == "SFNRO"
                                                        || jObject["productCode"].ToString().Trim() == "NROT3")
                                                {
                                                    cbsdtls.sInvalidAcFlag = "N";
                                                    //ViewBag.vbNRE = "NRE Account";
                                                    ViewBag.vbNRE = "N";

                                                    if (jObject["productCode"].ToString().Trim() == "NRESA" || jObject["productCode"].ToString().Trim() == "NRESP"
                                                            || jObject["productCode"].ToString().Trim() == "NRETR" || jObject["productCode"].ToString().Trim() == "NRET1"
                                                            || jObject["productCode"].ToString().Trim() == "NRET3" || jObject["productCode"].ToString().Trim() == "NEPIS"
                                                            || jObject["productCode"].ToString().Trim() == "NREWL" || jObject["productCode"].ToString().Trim() == "NRSAV"
                                                            || jObject["productCode"].ToString().Trim() == "SFNRE")
                                                        Session["sNR"] = "NRE";
                                                    else if (jObject["productCode"].ToString().Trim() == "NROSA" || jObject["productCode"].ToString().Trim() == "NROSP"
                                                            || jObject["productCode"].ToString().Trim() == "NROTR" || jObject["productCode"].ToString().Trim() == "NROT1"
                                                            || jObject["productCode"].ToString().Trim() == "NOPIS" || jObject["productCode"].ToString().Trim() == "NROWL"
                                                            || jObject["productCode"].ToString().Trim() == "SFNRO" || jObject["productCode"].ToString().Trim() == "NROT3")
                                                        Session["sNR"] = "NRO";

                                                }
                                                else
                                                {
                                                    Session["sNR"] = "";
                                                }


                                                //opendate
                                                if (jObject["openedDate"] != null)
                                                {
                                                    openDate = Convert.ToInt64(jObject["openedDate"].ToString().Trim());
                                                }
                                                else
                                                {
                                                    openDate = 0;
                                                }

                                                if (openDate != 0)
                                                {
                                                    DateTimeOffset dateTimeFromUnix = DateTimeOffset.FromUnixTimeMilliseconds(openDate);
                                                    DateTimeOffset currentDateTime = DateTimeOffset.Now;
                                                    int differenceInMonths = CalculateDifferenceInMonths(currentDateTime, dateTimeFromUnix);

                                                    if (differenceInMonths > 6)
                                                    {
                                                        //Console.WriteLine("The difference is greater than six months.");
                                                        Session["IsOpenedDateOld"] = "Y";
                                                        cbsdtls.acOpenDate = "Y";
                                                    }
                                                    else
                                                    {
                                                        //Console.WriteLine("The difference is not greater than six months.");
                                                        Session["IsOpenedDateOld"] = "N";
                                                        cbsdtls.acOpenDate = "N";
                                                    }
                                                }



                                                //mop 
                                                if (jObject["modeOfOperation"] != null)
                                                {
                                                    cbsdtls.MOP = jObject["modeOfOperation"].ToString().Trim();
                                                }
                                                if (jObject["staffIndicator"] != null)
                                                {
                                                    cbsdtls.StaffAcc = (bool)jObject["staffIndicator"] ? "Y" : "N";
                                                }
                                                if (jObject["accountStatus"] != null)
                                                {
                                                    cbsdtls.sacct_status = jObject["accountStatus"].ToString();
                                                }
                                                if (jObject["freezeStatusCode"] != null)
                                                {
                                                    cbsdtls.sFreezeStatusCode = jObject["freezeStatusCode"].ToString();
                                                }
                                                if (jObject["productCode"] != null)
                                                {
                                                    cbsdtls.productCode = jObject["productCode"].ToString().Trim();
                                                }
                                                if (jObject["productType"] != null)
                                                {
                                                    cbsdtls.productType = jObject["productType"].ToString().Trim();
                                                }
                                                if (jObject["accountCurrencyCode"] != null)
                                                {
                                                    cbsdtls.accountCurrencyCode = jObject["accountCurrencyCode"].ToString().Trim();
                                                }

                                                if (jObject["accountBalances"] != null)
                                                {
                                                    Session["accountBalances"] = jObject["accountBalances"]["effectiveAvailableAmount"] == null ? "0" :
                                                                                jObject["accountBalances"]["effectiveAvailableAmount"].ToString().Trim();
                                                    cbsdtls.ACBALAmount = jObject["accountBalances"]["effectiveAvailableAmount"] == null ? "0" :
                                                                                jObject["accountBalances"]["effectiveAvailableAmount"].ToString().Trim();
                                                }
                                                else
                                                {
                                                    Session["accountBalances"] = "0";
                                                    cbsdtls.ACBALAmount = "0";
                                                }


                                                if (jObject["sourceCustomerId"] != null)
                                                {
                                                    cbsdtls.SourceCustomerId = jObject["sourceCustomerId"].ToString().Trim();
                                                }

                                                if (jObject["branchCode"] != null)
                                                {
                                                    cbsdtls.SolId = jObject["branchCode"].ToString().Trim();
                                                }

                                                if (jObject["productType"] != null)
                                                {
                                                    cbsdtls.OffAcc = jObject["productType"].ToString() == "OAB" ? "Y" : "N";
                                                }


                                                //string to store api data
                                                //                      sourceCustomerId|accountCurrencyCode|openedDate|productCode|productType|accountBalances|modeOfOperation|accountStatus|staffIndicator|freezeStatusCode|openDate|SOLID|offAcc
                                                cbsdtls.ApiDataString = cbsdtls.SourceCustomerId + "|" + cbsdtls.accountCurrencyCode + "|" + cbsdtls.acOpenDate + "|" + cbsdtls.productCode + "|" + cbsdtls.productType + "|" + cbsdtls.ACBALAmount + "|" + cbsdtls.MOP + "|" + cbsdtls.sacct_status + "|" + cbsdtls.StaffAcc + "|" + cbsdtls.sFreezeStatusCode + "|" + openDate + "|" + cbsdtls.SolId + "|" + cbsdtls.OffAcc;
                                                logerror("in cbsdetails==> value for ApiDataString==> " + cbsdtls.ApiDataString.ToString(), "===LN1509");

                                            }

                                            cbsdtls.sCAPA = "Account is dormant";
                                            cbsdtls.sInvalidAcFlag = "T";
                                        }
                                        else if (jObject["accountStatus"].ToString().Trim() == "Inactive") //for Inactive account
                                        {
                                            if (jObject["accountName"] != null)
                                            {
                                                //lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                //if (lAccNames.Count > 0)
                                                //    cbsdtls.PayeeNameList = lAccNames;

                                                if (((Newtonsoft.Json.Linq.JContainer)jObject["relatedCustomerInfo"]).Count > 0)
                                                {
                                                    int iIndex = 0;

                                                    while (iIndex < jObject["relatedCustomerInfo"].Count())
                                                    {
                                                        //Call for account holders

                                                        //====== 4 uncomment when deployed on bank start ===================
                                                        string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, sEtoken, jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                        //string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, "", jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                        //====== 4 uncomment when deployed on bank end ===================
                                                        logerror("In GetCBSDetailsWithAPI Ac", "Active and sCMPCResponse - " + sCMPCResponse);
                                                        //Get account holders
                                                        string sCustomerName = GetCMCPCustomerName(sCMPCResponse);
                                                        //string sCustomerName = getCustomerName(sCMPCResponse);
                                                        logerror("In GetCBSDetailsWithAPI Ac", "Active and GetCMCPCustomerName - " + sCustomerName);
                                                        //var jObject1 = Newtonsoft.Json.Linq.JObject.Parse(sCustomerName);
                                                        //string sname = jObject1["data"]["profileInfo"]["registeredName"].ToString().Trim();

                                                        if (sCustomerName != "")
                                                        {
                                                            lAccNames.Add(sCustomerName);
                                                        }

                                                        logerror("Joint Account Name : ", sCustomerName.ToString());
                                                        iIndex++;
                                                    }

                                                    if (lAccNames.Count > 0)
                                                        cbsdtls.PayeeNameList = lAccNames;
                                                }
                                                else
                                                {
                                                    lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                    if (lAccNames.Count > 0)
                                                        cbsdtls.PayeeNameList = lAccNames;
                                                }

                                                if (lAccNames.Count == 0)
                                                {
                                                    lAccNames.Add(jObject["accountName"].ToString().Trim());

                                                    if (lAccNames.Count > 0)
                                                        cbsdtls.PayeeNameList = lAccNames;
                                                }

                                                cbsdtls.status = "SUCCESS";
                                                cbsdtls.cbsdls = null;
                                                cbsdtls.sacct_status = jObject["accountStatus"].ToString().Trim();
                                                cbsdtls.sFreezeStatusCode = jObject["freezeStatusCode"].ToString().Trim();
                                                //cbsdtls.NREFlag = jObject["productName"].ToString().Trim();
                                                cbsdtls.NREFlag = jObject["productCode"] == null ? "" : jObject["productCode"].ToString().Trim();

                                                cbsdtls.sCAPA = jObject["productName"].ToString().Trim();

                                                Session["sacct_status"] = jObject["accountStatus"].ToString().Trim();
                                                Session["sNREFlag"] = jObject["productName"].ToString().Trim();


                                                //opendate
                                                if (jObject["openedDate"] != null)
                                                {
                                                    openDate = Convert.ToInt64(jObject["openedDate"].ToString().Trim());
                                                }
                                                else
                                                {
                                                    openDate = 0;
                                                }

                                                if (openDate != 0)
                                                {
                                                    DateTimeOffset dateTimeFromUnix = DateTimeOffset.FromUnixTimeMilliseconds(openDate);
                                                    DateTimeOffset currentDateTime = DateTimeOffset.Now;
                                                    int differenceInMonths = CalculateDifferenceInMonths(currentDateTime, dateTimeFromUnix);

                                                    if (differenceInMonths > 6)
                                                    {
                                                        //Console.WriteLine("The difference is greater than six months.");
                                                        Session["IsOpenedDateOld"] = "Y";
                                                        cbsdtls.acOpenDate = "Y";
                                                    }
                                                    else
                                                    {
                                                        //Console.WriteLine("The difference is not greater than six months.");
                                                        Session["IsOpenedDateOld"] = "N";
                                                        cbsdtls.acOpenDate = "N";
                                                    }
                                                }



                                                //mop 
                                                if (jObject["modeOfOperation"] != null)
                                                {
                                                    cbsdtls.MOP = jObject["modeOfOperation"].ToString().Trim();
                                                }
                                                if (jObject["staffIndicator"] != null)
                                                {
                                                    cbsdtls.StaffAcc = (bool)jObject["staffIndicator"] ? "Y" : "N";
                                                }
                                                if (jObject["accountStatus"] != null)
                                                {
                                                    cbsdtls.sacct_status = jObject["accountStatus"].ToString();
                                                }
                                                if (jObject["freezeStatusCode"] != null)
                                                {
                                                    cbsdtls.sFreezeStatusCode = jObject["freezeStatusCode"].ToString();
                                                }
                                                if (jObject["productCode"] != null)
                                                {
                                                    cbsdtls.productCode = jObject["productCode"].ToString().Trim();
                                                }
                                                if (jObject["productType"] != null)
                                                {
                                                    cbsdtls.productType = jObject["productType"].ToString().Trim();
                                                }
                                                if (jObject["accountCurrencyCode"] != null)
                                                {
                                                    cbsdtls.accountCurrencyCode = jObject["accountCurrencyCode"].ToString().Trim();
                                                }

                                                if (jObject["accountBalances"] != null)
                                                {
                                                    Session["accountBalances"] = jObject["accountBalances"]["effectiveAvailableAmount"] == null ? "0" :
                                                                                jObject["accountBalances"]["effectiveAvailableAmount"].ToString().Trim();
                                                    cbsdtls.ACBALAmount = jObject["accountBalances"]["effectiveAvailableAmount"] == null ? "0" :
                                                                                jObject["accountBalances"]["effectiveAvailableAmount"].ToString().Trim();
                                                }
                                                else
                                                {
                                                    Session["accountBalances"] = "0";
                                                    cbsdtls.ACBALAmount = "0";
                                                }


                                                if (jObject["sourceCustomerId"] != null)
                                                {
                                                    cbsdtls.SourceCustomerId = jObject["sourceCustomerId"].ToString().Trim();
                                                }

                                                if (jObject["branchCode"] != null)
                                                {
                                                    cbsdtls.SolId = jObject["branchCode"].ToString().Trim();
                                                }

                                                if (jObject["productType"] != null)
                                                {
                                                    cbsdtls.OffAcc = jObject["productType"].ToString() == "OAB" ? "Y" : "N";
                                                }


                                                //string to store api data
                                                //                      sourceCustomerId|accountCurrencyCode|openedDate|productCode|productType|accountBalances|modeOfOperation|accountStatus|staffIndicator|freezeStatusCode|openDate|SOLID|offAcc
                                                cbsdtls.ApiDataString = cbsdtls.SourceCustomerId + "|" + cbsdtls.accountCurrencyCode + "|" + cbsdtls.acOpenDate + "|" + cbsdtls.productCode + "|" + cbsdtls.productType + "|" + cbsdtls.ACBALAmount + "|" + cbsdtls.MOP + "|" + cbsdtls.sacct_status + "|" + cbsdtls.StaffAcc + "|" + cbsdtls.sFreezeStatusCode + "|" + openDate + "|" + cbsdtls.SolId + "|" + cbsdtls.OffAcc;
                                                logerror("in cbsdetails==> value for ApiDataString==> " + cbsdtls.ApiDataString.ToString(), "===LN1509");


                                            }



                                            cbsdtls.sCAPA = "Account is inactive";

                                            //cbsdtls.PayeeNameList = null;
                                            cbsdtls.sInvalidAcFlag = "F";  //allowing inactive acc in l1 21-12-24
                                        }
                                    }
                                    else
                                    {
                                        cbsdtls.sCAPA = "Invalid Account";
                                        cbsdtls.PayeeNameList = null;
                                        cbsdtls.status = "SUCCESS";
                                        cbsdtls.cbsdls = null;
                                        cbsdtls.sInvalidAcFlag = "T";
                                    }
                                }
                                else
                                {
                                    //for close account

                                    if (jObject["accountName"] != null)
                                    {
                                        //lAccNames.Add(jObject["accountName"].ToString().Trim());
                                        //if (lAccNames.Count > 0)
                                        //    cbsdtls.PayeeNameList = lAccNames;

                                        if (((Newtonsoft.Json.Linq.JContainer)jObject["relatedCustomerInfo"]).Count > 0)
                                        {
                                            int iIndex = 0;

                                            while (iIndex < jObject["relatedCustomerInfo"].Count())
                                            {
                                                //Call for account holders

                                                //====== 4 uncomment when deployed on bank start ===================
                                                string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, sEtoken, jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                //string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, "", jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                //====== 4 uncomment when deployed on bank end ===================
                                                logerror("In GetCBSDetailsWithAPI Ac", "Active and sCMPCResponse - " + sCMPCResponse);
                                                //Get account holders
                                                string sCustomerName = GetCMCPCustomerName(sCMPCResponse);
                                                //string sCustomerName = getCustomerName(sCMPCResponse);
                                                logerror("In GetCBSDetailsWithAPI Ac", "Active and GetCMCPCustomerName - " + sCustomerName);
                                                //var jObject1 = Newtonsoft.Json.Linq.JObject.Parse(sCustomerName);
                                                //string sname = jObject1["data"]["profileInfo"]["registeredName"].ToString().Trim();

                                                if (sCustomerName != "")
                                                {
                                                    lAccNames.Add(sCustomerName);
                                                }

                                                logerror("Joint Account Name : ", sCustomerName.ToString());
                                                iIndex++;
                                            }

                                            if (lAccNames.Count > 0)
                                                cbsdtls.PayeeNameList = lAccNames;
                                        }
                                        else
                                        {
                                            lAccNames.Add(jObject["accountName"].ToString().Trim());
                                            if (lAccNames.Count > 0)
                                                cbsdtls.PayeeNameList = lAccNames;
                                        }

                                        if (lAccNames.Count == 0)
                                        {
                                            lAccNames.Add(jObject["accountName"].ToString().Trim());

                                            if (lAccNames.Count > 0)
                                                cbsdtls.PayeeNameList = lAccNames;
                                        }

                                        cbsdtls.status = "SUCCESS";
                                        cbsdtls.cbsdls = null;
                                        cbsdtls.sacct_status = jObject["accountStatus"].ToString().Trim();
                                        cbsdtls.sFreezeStatusCode = jObject["freezeStatusCode"].ToString().Trim();
                                        //cbsdtls.NREFlag = jObject["productName"].ToString().Trim();
                                        cbsdtls.NREFlag = jObject["productCode"] == null ? "" : jObject["productCode"].ToString().Trim();

                                        cbsdtls.sCAPA = jObject["productName"].ToString().Trim();

                                        Session["sacct_status"] = jObject["accountStatus"].ToString().Trim();
                                        Session["sNREFlag"] = jObject["productName"].ToString().Trim();


                                        //opendate
                                        if (jObject["openedDate"] != null)
                                        {
                                            openDate = Convert.ToInt64(jObject["openedDate"].ToString().Trim());
                                        }
                                        else
                                        {
                                            openDate = 0;
                                        }

                                        if (openDate != 0)
                                        {
                                            DateTimeOffset dateTimeFromUnix = DateTimeOffset.FromUnixTimeMilliseconds(openDate);
                                            DateTimeOffset currentDateTime = DateTimeOffset.Now;
                                            int differenceInMonths = CalculateDifferenceInMonths(currentDateTime, dateTimeFromUnix);

                                            if (differenceInMonths > 6)
                                            {
                                                //Console.WriteLine("The difference is greater than six months.");
                                                Session["IsOpenedDateOld"] = "Y";
                                                cbsdtls.acOpenDate = "Y";
                                            }
                                            else
                                            {
                                                //Console.WriteLine("The difference is not greater than six months.");
                                                Session["IsOpenedDateOld"] = "N";
                                                cbsdtls.acOpenDate = "N";
                                            }
                                        }



                                        //mop 
                                        if (jObject["modeOfOperation"] != null)
                                        {
                                            cbsdtls.MOP = jObject["modeOfOperation"].ToString().Trim();
                                        }
                                        if (jObject["staffIndicator"] != null)
                                        {
                                            cbsdtls.StaffAcc = (bool)jObject["staffIndicator"] ? "Y" : "N";
                                        }
                                        if (jObject["accountStatus"] != null)
                                        {
                                            cbsdtls.sacct_status = jObject["accountStatus"].ToString();
                                        }
                                        if (jObject["freezeStatusCode"] != null)
                                        {
                                            cbsdtls.sFreezeStatusCode = jObject["freezeStatusCode"].ToString();
                                        }
                                        if (jObject["productCode"] != null)
                                        {
                                            cbsdtls.productCode = jObject["productCode"].ToString().Trim();
                                        }
                                        if (jObject["productType"] != null)
                                        {
                                            cbsdtls.productType = jObject["productType"].ToString().Trim();
                                        }
                                        if (jObject["accountCurrencyCode"] != null)
                                        {
                                            cbsdtls.accountCurrencyCode = jObject["accountCurrencyCode"].ToString().Trim();
                                        }

                                        if (jObject["accountBalances"] != null)
                                        {
                                            Session["accountBalances"] = jObject["accountBalances"]["effectiveAvailableAmount"] == null ? "0" :
                                                                        jObject["accountBalances"]["effectiveAvailableAmount"].ToString().Trim();
                                            cbsdtls.ACBALAmount = jObject["accountBalances"]["effectiveAvailableAmount"] == null ? "0" :
                                                                        jObject["accountBalances"]["effectiveAvailableAmount"].ToString().Trim();
                                        }
                                        else
                                        {
                                            Session["accountBalances"] = "0";
                                            cbsdtls.ACBALAmount = "0";
                                        }


                                        if (jObject["sourceCustomerId"] != null)
                                        {
                                            cbsdtls.SourceCustomerId = jObject["sourceCustomerId"].ToString().Trim();
                                        }

                                        if (jObject["branchCode"] != null)
                                        {
                                            cbsdtls.SolId = jObject["branchCode"].ToString().Trim();
                                        }

                                        if (jObject["productType"] != null)
                                        {
                                            cbsdtls.OffAcc = jObject["productType"].ToString() == "OAB" ? "Y" : "N";
                                        }


                                        //string to store api data
                                        //                      sourceCustomerId|accountCurrencyCode|openedDate|productCode|productType|accountBalances|modeOfOperation|accountStatus|staffIndicator|freezeStatusCode|openDate|SOLID|offAcc
                                        cbsdtls.ApiDataString = cbsdtls.SourceCustomerId + "|" + cbsdtls.accountCurrencyCode + "|" + cbsdtls.acOpenDate + "|" + cbsdtls.productCode + "|" + cbsdtls.productType + "|" + cbsdtls.ACBALAmount + "|" + cbsdtls.MOP + "|" + cbsdtls.sacct_status + "|" + cbsdtls.StaffAcc + "|" + cbsdtls.sFreezeStatusCode + "|" + openDate + "|" + cbsdtls.SolId + "|" + cbsdtls.OffAcc;
                                        logerror("in cbsdetails==> value for ApiDataString==> " + cbsdtls.ApiDataString.ToString(), "===LN1509");





                                    }

                                    cbsdtls.sCAPA = "Account is closed";
                                    //cbsdtls.PayeeNameList = null;
                                    cbsdtls.sInvalidAcFlag = "T";
                                }
                            }

                            } // else of 999999999999 ac no.
                        
                       
                    }
                    else// for testing purpose block
                    {
                        ViewBag.Currency = "";
                        ViewBag.sCAPA = "";
                        ViewBag.vbNRE = "";
                        Session["sNR"] = "";
                        Session["SourceCustomerId"] = "";
                        Session["AccountCurrency"] = "";
                        Session["IsOpenedDateOld"] = "";
                        Session["productCode"] = "";
                        Session["productType"] = "";
                        Session["accountBalances"] = "0";

                        long openDate = 0;

                        //sgetAccountDetailsDBS = testCMCP_Response();
                        sgetAccountDetailsDBS = getAccountDetailsDBSResponseTest();
                        var newResponse = sgetAccountDetailsDBS.Replace(", Please", " Please");
                        var jObject = Newtonsoft.Json.Linq.JObject.Parse(newResponse);
                        cbsdtls.Invalidtbl = "";
                        if (jObject["error"] != null)
                        {
                            if (jObject["errorDescription"] != null)
                            {
                                ViewBag.vberror = jObject["errorDescription"].ToString();
                                ViewBag.vbstatus = "SUCCESS";
                                ViewBag.vbcbsdls = null;
                                ViewBag.block = 1;


                                if (jObject["error"]["restError"][0] != null)
                                {

                                    cbsdtls.sCAPA = jObject["error"]["restError"][0]["errorMessage"].ToString().Trim();
                                    cbsdtls.PayeeNameList = null;
                                    cbsdtls.status = "SUCCESS";
                                    cbsdtls.cbsdls = null;
                                    cbsdtls.sInvalidAcFlag = "T";
                                    cbsdtls.Invalidtbl = "E";
                                }
                                else
                                {
                                    cbsdtls.sCAPA = "Invalid Account";
                                    cbsdtls.PayeeNameList = null;
                                    cbsdtls.status = "SUCCESS";
                                    cbsdtls.cbsdls = null;
                                    cbsdtls.sInvalidAcFlag = "T";
                                    cbsdtls.Invalidtbl = "E";
                                }



                            }
                            else
                            {
                                ViewBag.vberror = "Invalid Account";
                                ViewBag.vbstatus = "SUCCESS";
                                ViewBag.vbcbsdls = null;
                                ViewBag.block = 1;
                            }

                        }
                        else
                        {
                            if (jObject["accountClosedFlag"].ToString().Trim().ToUpper() != "Y")
                            {
                                logerror("In GetJointAcNms method sourceCustomerId - ", jObject["sourceCustomerId"].ToString().Trim());
                                logerror("In GetJointAcNms method accountCurrency - ", jObject["accountCurrency"].ToString().Trim());
                                //=========== Amol changes on 27/02/2024 for capturing SourceCustomerId start ======
                                if (jObject["sourceCustomerId"] != null)
                                {
                                    Session["SourceCustomerId"] = jObject["sourceCustomerId"].ToString().Trim();
                                    //cbsdtls.SourceCustomerId = jObject["sourceCustomerId"].ToString().Trim();
                                }
                                else
                                {
                                    Session["SourceCustomerId"] = "";
                                }

                                if (jObject["accountCurrencyCode"] != null)
                                {
                                    Session["AccountCurrency"] = jObject["accountCurrencyCode"].ToString().Trim();
                                    ViewBag.Currency = jObject["accountCurrencyCode"].ToString().Trim();
                                    //cbsdtls.Currency = jObject["accountCurrency"].ToString().Trim();
                                }
                                else
                                {
                                    Session["AccountCurrency"] = "";
                                    ViewBag.Currency = "";
                                }

                                if (jObject["openedDate"] != null)
                                {
                                    openDate = Convert.ToInt64(jObject["openedDate"].ToString().Trim());

                                    //var dateTime = DateTimeOffset.FromUnixTimeSeconds(1550962800);
                                }
                                else
                                {
                                    openDate = 0;
                                }

                                if (openDate != 0)
                                {
                                    // DateTimeOffset from Unix timestamp
                                    DateTimeOffset dateTimeFromUnix = DateTimeOffset.FromUnixTimeMilliseconds(openDate);
                                    // Current DateTimeOffset
                                    DateTimeOffset currentDateTime = DateTimeOffset.Now;
                                    // Calculate the difference
                                    //TimeSpan difference = currentDateTime - dateTimeFromUnix;

                                    int differenceInMonths = CalculateDifferenceInMonths(currentDateTime, dateTimeFromUnix);

                                    // Check if the difference is greater than 6 months
                                    if (differenceInMonths > 6)
                                    {
                                        //Console.WriteLine("The difference is greater than six months.");
                                        Session["IsOpenedDateOld"] = "Y";
                                    }
                                    else
                                    {
                                        //Console.WriteLine("The difference is not greater than six months.");
                                        Session["IsOpenedDateOld"] = "N";
                                    }
                                }
                                else
                                {
                                    Session["IsOpenedDateOld"] = "";
                                }

                                if (jObject["productCode"] != null)
                                {
                                    Session["productCode"] = jObject["productCode"].ToString().Trim();
                                }
                                else
                                {
                                    Session["productCode"] = "";
                                }

                                if (jObject["productType"] != null)
                                {
                                    Session["productType"] = jObject["productType"].ToString().Trim();
                                }
                                else
                                {
                                    Session["productType"] = "";
                                }

                                if (jObject["accountBalances"] != null)
                                {
                                    Session["accountBalances"] = jObject["accountBalances"]["effectiveAvailableAmount"] == null ? "0" :
                                                                jObject["accountBalances"]["effectiveAvailableAmount"].ToString().Trim();
                                }
                                else
                                {
                                    Session["accountBalances"] = "0";
                                }

                                logerror("In GetJointAcNms method sourceCustomerId session - ", Session["SourceCustomerId"].ToString().Trim());
                                logerror("In GetJointAcNms method accountCurrency session - ", Session["AccountCurrency"].ToString().Trim());
                                //=========== Amol changes on 27/02/2024 for capturing SourceCustomerId end ======

                                if (jObject["accountStatus"] != null)
                                {
                                    if (jObject["accountStatus"].ToString().Trim() == "Active")
                                    {
                                        if (jObject["accountName"] != null)
                                        {
                                            if (((Newtonsoft.Json.Linq.JContainer)jObject["relatedCustomerInfo"]).Count > 0)
                                            {
                                                logerror("In GetJointAcNms method relatedCustomerInfo count - ", ((Newtonsoft.Json.Linq.JContainer)jObject["relatedCustomerInfo"]).Count.ToString());
                                                int iIndex = 0;

                                                while (iIndex < jObject["relatedCustomerInfo"].Count())
                                                {
                                                    //Call for account holders
                                                    logerror("In GetJointAcNms method related party customerId - ", jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                    //====== 4 uncomment when deployed on bank start ===================
                                                    //string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, sEtoken, jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                    //string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, "", jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                    //string sCMPCResponse = testCMCP_Response();
                                                    //====== 4 uncomment when deployed on bank end ===================
                                                    //logerror("In Join Ac", "Active and sCMPCResponse - " + sCMPCResponse);
                                                    //Get account holders
                                                    string sCustomerName = GetCMCPCustomerNameTest();
                                                    //string sCustomerName = getCustomerName(sCMPCResponse);
                                                    logerror("In Join Ac", "Active and GetCMCPCustomerName - " + sCustomerName);
                                                    //var jObject1 = Newtonsoft.Json.Linq.JObject.Parse(sCustomerName);
                                                    //string sname = jObject1["data"]["profileInfo"]["registeredName"].ToString().Trim();

                                                    if (sCustomerName != "")
                                                    {
                                                        lAccNames.Add(sCustomerName);
                                                        lAccNames.Add("test");
                                                    }


                                                    logerror("Joint Account Name : ", sCustomerName.ToString());
                                                    iIndex++;
                                                }

                                                ViewBag.vbstatus = "SUCCESS";
                                                ViewBag.vbcbsdls = null;
                                                cbsdtls.status = "SUCCESS";

                                                if (lAccNames.Count > 0)
                                                    ViewBag.vbAcctName = lAccNames;

                                                if (lAccNames.Count > 0)
                                                    cbsdtls.PayeeNameList = lAccNames;
                                            }
                                            else
                                            {
                                                lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                ViewBag.vbstatus = "SUCCESS";
                                                ViewBag.vbcbsdls = null;

                                                if (lAccNames.Count > 0)
                                                    ViewBag.vbAcctName = lAccNames;

                                                if (lAccNames.Count > 0)
                                                    cbsdtls.PayeeNameList = lAccNames;
                                            }

                                            if (lAccNames.Count == 0)
                                            {
                                                lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                ViewBag.vbstatus = "SUCCESS";
                                                ViewBag.vbcbsdls = null;

                                                if (lAccNames.Count > 0)
                                                    ViewBag.vbAcctName = lAccNames;

                                                if (lAccNames.Count > 0)
                                                    cbsdtls.PayeeNameList = lAccNames;
                                            }


                                            cbsdtls.status = "SUCCESS";
                                            cbsdtls.cbsdls = null;
                                            cbsdtls.sacct_status = jObject["accountStatus"].ToString().Trim();
                                            cbsdtls.sFreezeStatusCode = jObject["freezeStatusCode"].ToString().Trim();
                                            cbsdtls.NREFlag = jObject["productCode"] == null ? "" : jObject["productCode"].ToString().Trim();
                                            //cbsdtls.NREFlag = jObject["productName"].ToString().Trim();

                                            cbsdtls.sCAPA = jObject["productName"].ToString().Trim();

                                            //======================================================================= comment by amol for CMCP account name on 07/06/2023 start ==================
                                            //lAccNames.Add(jObject["accountName"].ToString().Trim());
                                            //ViewBag.vbstatus = "SUCCESS";
                                            //ViewBag.vbcbsdls = null;

                                            //if (lAccNames.Count > 0)
                                            //    ViewBag.vbAcctName = lAccNames;
                                            //======================================================================= comment by amol for CMCP account name on 07/06/2023 end ==================

                                            if (jObject["productCode"].ToString().Trim() == "NRESA" || jObject["productCode"].ToString().Trim() == "NROSA"
                                                            || jObject["productCode"].ToString().Trim() == "NRESP" || jObject["productCode"].ToString().Trim() == "NROSP"
                                                            || jObject["productCode"].ToString().Trim() == "NRETR" || jObject["productCode"].ToString().Trim() == "NROTR"
                                                            || jObject["productCode"].ToString().Trim() == "NRET1" || jObject["productCode"].ToString().Trim() == "NROT1"
                                                            || jObject["productCode"].ToString().Trim() == "NRET3" || jObject["productCode"].ToString().Trim() == "NEPIS"
                                                            || jObject["productCode"].ToString().Trim() == "NOPIS" || jObject["productCode"].ToString().Trim() == "NREWL"
                                                            || jObject["productCode"].ToString().Trim() == "NROWL" || jObject["productCode"].ToString().Trim() == "NRSAV"
                                                            || jObject["productCode"].ToString().Trim() == "SFNRE" || jObject["productCode"].ToString().Trim() == "SFNRO"
                                                            || jObject["productCode"].ToString().Trim() == "NROT3")
                                            {
                                                ViewBag.sCAPA = "NRE Account";
                                                ViewBag.vbNRE = "NRE Account";

                                                if (jObject["productCode"].ToString().Trim() == "NRESA" || jObject["productCode"].ToString().Trim() == "NRESP"
                                                    || jObject["productCode"].ToString().Trim() == "NRETR" || jObject["productCode"].ToString().Trim() == "NRET1"
                                                    || jObject["productCode"].ToString().Trim() == "NRET3" || jObject["productCode"].ToString().Trim() == "NEPIS"
                                                    || jObject["productCode"].ToString().Trim() == "NREWL" || jObject["productCode"].ToString().Trim() == "NRSAV"
                                                    || jObject["productCode"].ToString().Trim() == "SFNRE")
                                                    Session["sNR"] = "NRE";
                                                else if (jObject["productCode"].ToString().Trim() == "NROSA" || jObject["productCode"].ToString().Trim() == "NROSP"
                                                    || jObject["productCode"].ToString().Trim() == "NROTR" || jObject["productCode"].ToString().Trim() == "NROT1"
                                                    || jObject["productCode"].ToString().Trim() == "NOPIS" || jObject["productCode"].ToString().Trim() == "NROWL"
                                                    || jObject["productCode"].ToString().Trim() == "SFNRO" || jObject["productCode"].ToString().Trim() == "NROT3")
                                                    Session["sNR"] = "NRO";

                                            }


                                            //mop
                                            if (jObject["modeOfOperation"] != null)
                                            {
                                                cbsdtls.MOP = jObject["modeOfOperation"].ToString().Trim();
                                            }
                                            if (jObject["staffIndicator"] != null)
                                            {
                                                cbsdtls.StaffAcc = (bool)jObject["staffIndicator"] ? "Y" : "N";
                                            }
                                            if (jObject["accountStatus"] != null)
                                            {
                                                cbsdtls.sacct_status = jObject["accountStatus"].ToString();
                                            }
                                            if (jObject["freezeStatusCode"] != null)
                                            {
                                                cbsdtls.sFreezeStatusCode = jObject["freezeStatusCode"].ToString();
                                            }

                                            if (jObject["accountCurrencyCode"] != null)
                                            {
                                                cbsdtls.accountCurrencyCode = jObject["accountCurrencyCode"].ToString().Trim();
                                            }
                                            cbsdtls.ApiDataString = cbsdtls.SourceCustomerId + "|" + cbsdtls.accountCurrencyCode + "|" + "Y" + "|" + cbsdtls.productCode + "|" + cbsdtls.productType + "|" + cbsdtls.ACBALAmount + "|" + cbsdtls.MOP + "|" + cbsdtls.sacct_status + "|" + cbsdtls.StaffAcc + "|" + cbsdtls.sFreezeStatusCode;
                                        }
                                        else
                                        {
                                            ViewBag.vberror = "Invalid Account";
                                            ViewBag.vbstatus = "SUCCESS";
                                            ViewBag.vbcbsdls = null;
                                            ViewBag.block = 1;

                                            cbsdtls.sCAPA = "Invalid Account";
                                            cbsdtls.PayeeNameList = null;
                                            cbsdtls.status = "SUCCESS";
                                            cbsdtls.cbsdls = null;
                                            cbsdtls.sInvalidAcFlag = "T";
                                        }

                                        if (jObject["freezeStatusCode"].ToString().Trim() == "T")
                                        {
                                            ViewBag.sCAPA = "Total freeze";
                                            ViewBag.vberror = "Account is Total freeze";
                                            ViewBag.block = 1;


                                            cbsdtls.sCAPA = "Account is Total freeze";
                                            cbsdtls.sInvalidAcFlag = "T";

                                        }
                                        else if (jObject["freezeStatusCode"].ToString().Trim() == "C")
                                        {
                                            ViewBag.sCAPA = "Credit freeze";
                                            ViewBag.vberror = "Account is Credit freeze";
                                            ViewBag.block = 1;

                                            cbsdtls.sCAPA = "Account is Credit freeze";

                                            //cbsdtls.PayeeNameList = null;
                                            cbsdtls.sInvalidAcFlag = "T";
                                        }
                                        else if (jObject["freezeStatusCode"].ToString().Trim() == "D")
                                        {
                                            ViewBag.sCAPA = "Debit freeze";
                                            ViewBag.vberror = "Account is Debit freeze";
                                            ViewBag.block = 1;

                                            cbsdtls.sCAPA = "Account is Debit freeze";

                                            //cbsdtls.PayeeNameList = null;
                                            cbsdtls.sInvalidAcFlag = "T";
                                        }

                                    }
                                    else if (jObject["accountStatus"].ToString().Trim() == "Dormant")
                                    {
                                        if (jObject["accountName"] != null)
                                        {
                                            if (((Newtonsoft.Json.Linq.JContainer)jObject["relatedCustomerInfo"]).Count > 0)
                                            {
                                                int iIndex = 0;

                                                while (iIndex < jObject["relatedCustomerInfo"].Count())
                                                {
                                                    //Call for account holders

                                                    //====== 5 uncomment when deployed on bank start ===================
                                                    //string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, sEtoken, jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                    //string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, "", jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                    //====== 5 uncomment when deployed on bank end ===================

                                                    //Get account holders
                                                    string sCustomerName = GetCMCPCustomerNameTest();
                                                    //string sCustomerName = getCustomerName(sCMPCResponse);

                                                    //var jObject1 = Newtonsoft.Json.Linq.JObject.Parse(sCustomerName);
                                                    //string sname = jObject1["data"]["profileInfo"]["registeredName"].ToString().Trim();

                                                    if (sCustomerName != "")
                                                    {
                                                        lAccNames.Add(sCustomerName);
                                                        lAccNames.Add("test");
                                                    }


                                                    logerror("Joint Account Name : ", sCustomerName.ToString());
                                                    iIndex++;
                                                }

                                                ViewBag.vbstatus = "SUCCESS";
                                                ViewBag.vbcbsdls = null;

                                                if (lAccNames.Count > 0)
                                                    ViewBag.vbAcctName = lAccNames;

                                                if (lAccNames.Count > 0)
                                                    cbsdtls.PayeeNameList = lAccNames;
                                            }
                                            else
                                            {
                                                lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                ViewBag.vbstatus = "SUCCESS";
                                                ViewBag.vbcbsdls = null;

                                                if (lAccNames.Count > 0)
                                                    ViewBag.vbAcctName = lAccNames;

                                                if (lAccNames.Count > 0)
                                                    cbsdtls.PayeeNameList = lAccNames;
                                            }

                                            if (lAccNames.Count == 0)
                                            {
                                                lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                ViewBag.vbstatus = "SUCCESS";
                                                ViewBag.vbcbsdls = null;

                                                if (lAccNames.Count > 0)
                                                    ViewBag.vbAcctName = lAccNames;

                                                if (lAccNames.Count > 0)
                                                    cbsdtls.PayeeNameList = lAccNames;
                                            }

                                            //======================================================================= comment by amol for CMCP account name on 07/06/2023 start ==================
                                            //lAccNames.Add(jObject["accountName"].ToString().Trim());
                                            //ViewBag.vbstatus = "SUCCESS";
                                            //ViewBag.vbcbsdls = null;

                                            //if (lAccNames.Count > 0)
                                            //    ViewBag.vbAcctName = lAccNames;
                                            //======================================================================= comment by amol for CMCP account name on 07/06/2023 end ==================

                                            cbsdtls.status = "SUCCESS";
                                            cbsdtls.cbsdls = null;
                                            cbsdtls.sacct_status = jObject["accountStatus"].ToString().Trim();
                                            cbsdtls.sFreezeStatusCode = jObject["freezeStatusCode"].ToString().Trim();
                                            //cbsdtls.NREFlag = jObject["productName"].ToString().Trim();
                                            cbsdtls.NREFlag = jObject["productCode"] == null ? "" : jObject["productCode"].ToString().Trim();

                                            cbsdtls.sCAPA = jObject["productName"].ToString().Trim();

                                        }

                                        ViewBag.vberror = "Dormant Account";
                                        ViewBag.vbstatus = "SUCCESS";
                                        ViewBag.vbcbsdls = null;
                                        ViewBag.block = 1;

                                        cbsdtls.sCAPA = "Account is dormant";
                                        cbsdtls.sInvalidAcFlag = "T";

                                    }
                                    else if (jObject["accountStatus"].ToString().Trim() == "Inactive")
                                    {
                                        if (jObject["accountName"] != null)
                                        {
                                            if (((Newtonsoft.Json.Linq.JContainer)jObject["relatedCustomerInfo"]).Count > 0)
                                            {
                                                int iIndex = 0;

                                                while (iIndex < jObject["relatedCustomerInfo"].Count())
                                                {
                                                    //Call for account holders

                                                    //====== 6 uncomment when deployed on bank start ===================
                                                    //string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, sEtoken, jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                    //string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, "", jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                                    //====== 6 uncomment when deployed on bank end ===================

                                                    //Get account holders
                                                    string sCustomerName = GetCMCPCustomerNameTest();
                                                    //string sCustomerName = getCustomerName(sCMPCResponse);

                                                    //var jObject1 = Newtonsoft.Json.Linq.JObject.Parse(sCustomerName);
                                                    //string sname = jObject1["data"]["profileInfo"]["registeredName"].ToString().Trim();

                                                    if (sCustomerName != "")
                                                    {
                                                        lAccNames.Add(sCustomerName);
                                                        lAccNames.Add("test");
                                                    }


                                                    logerror("Joint Account Name : ", sCustomerName.ToString());
                                                    iIndex++;
                                                }

                                                ViewBag.vbstatus = "SUCCESS";
                                                ViewBag.vbcbsdls = null;

                                                if (lAccNames.Count > 0)
                                                    ViewBag.vbAcctName = lAccNames;

                                                if (lAccNames.Count > 0)
                                                    cbsdtls.PayeeNameList = lAccNames;
                                            }
                                            else
                                            {
                                                lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                ViewBag.vbstatus = "SUCCESS";
                                                ViewBag.vbcbsdls = null;

                                                if (lAccNames.Count > 0)
                                                    ViewBag.vbAcctName = lAccNames;

                                                if (lAccNames.Count > 0)
                                                    cbsdtls.PayeeNameList = lAccNames;
                                            }

                                            if (lAccNames.Count == 0)
                                            {
                                                lAccNames.Add(jObject["accountName"].ToString().Trim());
                                                ViewBag.vbstatus = "SUCCESS";
                                                ViewBag.vbcbsdls = null;

                                                if (lAccNames.Count > 0)
                                                    ViewBag.vbAcctName = lAccNames;

                                                if (lAccNames.Count > 0)
                                                    cbsdtls.PayeeNameList = lAccNames;
                                            }

                                            //======================================================================= comment by amol for CMCP account name on 07/06/2023 start ==================
                                            //lAccNames.Add(jObject["accountName"].ToString().Trim());
                                            //ViewBag.vbstatus = "SUCCESS";
                                            //ViewBag.vbcbsdls = null;

                                            //if (lAccNames.Count > 0)
                                            //    ViewBag.vbAcctName = lAccNames;
                                            //======================================================================= comment by amol for CMCP account name on 07/06/2023 end ==================
                                            cbsdtls.status = "SUCCESS";
                                            cbsdtls.cbsdls = null;
                                            cbsdtls.sacct_status = jObject["accountStatus"].ToString().Trim();
                                            cbsdtls.sFreezeStatusCode = jObject["freezeStatusCode"].ToString().Trim();
                                            //cbsdtls.NREFlag = jObject["productName"].ToString().Trim();
                                            cbsdtls.NREFlag = jObject["productCode"] == null ? "" : jObject["productCode"].ToString().Trim();

                                            cbsdtls.sCAPA = jObject["productName"].ToString().Trim();

                                        }

                                        ViewBag.sCAPA = "Account is inactive";
                                        ViewBag.vberror = "Account is inactive";
                                        ViewBag.vbstatus = "SUCCESS";
                                        ViewBag.vbcbsdls = null;
                                        ViewBag.block = 1;

                                        cbsdtls.sCAPA = "Account is inactive";

                                        //cbsdtls.PayeeNameList = null;
                                        cbsdtls.sInvalidAcFlag = "T";

                                    }


                                }
                                else
                                {
                                    ViewBag.vberror = "Invalid Account";
                                    ViewBag.vbstatus = "SUCCESS";
                                    ViewBag.vbcbsdls = null;
                                    ViewBag.block = 1;

                                    cbsdtls.sCAPA = "Invalid Account";
                                    cbsdtls.PayeeNameList = null;
                                    cbsdtls.status = "SUCCESS";
                                    cbsdtls.cbsdls = null;
                                    cbsdtls.sInvalidAcFlag = "T";

                                }
                            }
                            else
                            {
                                //=========== Amol changes on 27/02/2024 for capturing SourceCustomerId start ======
                                if (jObject["sourceCustomerId"] != null)
                                {
                                    Session["SourceCustomerId"] = jObject["sourceCustomerId"].ToString().Trim();
                                }
                                else
                                {
                                    Session["SourceCustomerId"] = "";
                                }

                                if (jObject["accountCurrencyCode"] != null)
                                {
                                    Session["AccountCurrency"] = jObject["accountCurrencyCode"].ToString().Trim();
                                    ViewBag.Currency = jObject["accountCurrencyCode"].ToString().Trim();
                                }
                                else
                                {
                                    Session["AccountCurrency"] = "";
                                    ViewBag.Currency = "";
                                }

                                if (jObject["openedDate"] != null)
                                {
                                    openDate = Convert.ToInt64(jObject["openedDate"].ToString().Trim());

                                    //var dateTime = DateTimeOffset.FromUnixTimeSeconds(1550962800);
                                }
                                else
                                {
                                    openDate = 0;
                                }

                                if (openDate != 0)
                                {
                                    // DateTimeOffset from Unix timestamp
                                    DateTimeOffset dateTimeFromUnix = DateTimeOffset.FromUnixTimeMilliseconds(openDate);
                                    // Current DateTimeOffset
                                    DateTimeOffset currentDateTime = DateTimeOffset.Now;
                                    // Calculate the difference
                                    //TimeSpan difference = currentDateTime - dateTimeFromUnix;

                                    int differenceInMonths = CalculateDifferenceInMonths(currentDateTime, dateTimeFromUnix);

                                    // Check if the difference is greater than 6 months
                                    if (differenceInMonths > 6)
                                    {
                                        //Console.WriteLine("The difference is greater than six months.");
                                        Session["IsOpenedDateOld"] = "Y";
                                    }
                                    else
                                    {
                                        //Console.WriteLine("The difference is not greater than six months.");
                                        Session["IsOpenedDateOld"] = "N";
                                    }
                                }
                                else
                                {
                                    Session["IsOpenedDateOld"] = "";
                                }

                                if (jObject["productCode"] != null)
                                {
                                    Session["productCode"] = jObject["productCode"].ToString().Trim();
                                }
                                else
                                {
                                    Session["productCode"] = "";
                                }

                                if (jObject["productType"] != null)
                                {
                                    Session["productType"] = jObject["productType"].ToString().Trim();
                                }
                                else
                                {
                                    Session["productType"] = "";
                                }

                                if (jObject["accountBalances"] != null)
                                {
                                    Session["accountBalances"] = jObject["accountBalances"]["effectiveAvailableAmount"] == null ? "0" :
                                                                jObject["accountBalances"]["effectiveAvailableAmount"].ToString().Trim();
                                }
                                else
                                {
                                    Session["accountBalances"] = "0";
                                }
                                //=========== Amol changes on 27/02/2024 for capturing SourceCustomerId end ======

                                if (jObject["accountName"] != null)
                                {
                                    if (((Newtonsoft.Json.Linq.JContainer)jObject["relatedCustomerInfo"]).Count > 0)
                                    {
                                        int iIndex = 0;

                                        while (iIndex < jObject["relatedCustomerInfo"].Count())
                                        {
                                            //Call for account holders
                                            //====== 7 uncomment when deployed on bank start ===================
                                            //string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, sEtoken, jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                            //string sCMPCResponse = sendCMPCPRequest(CMCPServiceURL, CMCPCountry, CMCPReqUID, CMCPReqClientId, "", jObject["relatedCustomerInfo"][iIndex]["relatedPartyCustomerId"].ToString().Trim());
                                            //====== 7 uncomment when deployed on bank start ===================
                                            //Get account holders
                                            string sCustomerName = GetCMCPCustomerNameTest();
                                            //string sCustomerName = getCustomerName(sCMPCResponse);

                                            //var jObject1 = Newtonsoft.Json.Linq.JObject.Parse(sCustomerName);
                                            //string sname = jObject1["data"]["profileInfo"]["registeredName"].ToString().Trim();

                                            if (sCustomerName != "")
                                            {
                                                lAccNames.Add(sCustomerName);
                                                lAccNames.Add("test");
                                            }

                                            //lAccNames.Add(jObject["accountName"].ToString().Trim());
                                            //logerror("Joint Account Name : ", sCustomerName.ToString());
                                            iIndex++;
                                        }

                                        ViewBag.vbstatus = "SUCCESS";
                                        ViewBag.vbcbsdls = null;

                                        if (lAccNames.Count > 0)
                                            ViewBag.vbAcctName = lAccNames;

                                        if (lAccNames.Count > 0)
                                            cbsdtls.PayeeNameList = lAccNames;
                                    }
                                    else
                                    {
                                        lAccNames.Add(jObject["accountName"].ToString().Trim());
                                        ViewBag.vbstatus = "SUCCESS";
                                        ViewBag.vbcbsdls = null;

                                        if (lAccNames.Count > 0)
                                            ViewBag.vbAcctName = lAccNames;

                                        lAccNames.Add(jObject["accountName"].ToString().Trim());
                                        if (lAccNames.Count > 0)
                                            cbsdtls.PayeeNameList = lAccNames;
                                    }

                                    if (lAccNames.Count == 0)
                                    {
                                        lAccNames.Add(jObject["accountName"].ToString().Trim());
                                        ViewBag.vbstatus = "SUCCESS";
                                        ViewBag.vbcbsdls = null;

                                        if (lAccNames.Count > 0)
                                            ViewBag.vbAcctName = lAccNames;
                                    }

                                    if (lAccNames.Count == 0)
                                    {
                                        lAccNames.Add(jObject["accountName"].ToString().Trim());

                                        if (lAccNames.Count > 0)
                                            cbsdtls.PayeeNameList = lAccNames;
                                    }

                                    cbsdtls.status = "SUCCESS";
                                    cbsdtls.cbsdls = null;
                                    cbsdtls.sacct_status = jObject["accountStatus"].ToString().Trim();
                                    cbsdtls.sFreezeStatusCode = jObject["freezeStatusCode"].ToString().Trim();
                                    //cbsdtls.NREFlag = jObject["productName"].ToString().Trim();
                                    cbsdtls.NREFlag = jObject["productCode"] == null ? "" : jObject["productCode"].ToString().Trim();

                                    cbsdtls.sCAPA = jObject["productName"].ToString().Trim();


                                    //======================================================================= comment by amol for CMCP account name on 07/06/2023 start ==================
                                    //lAccNames.Add(jObject["accountName"].ToString().Trim());
                                    //ViewBag.vbstatus = "SUCCESS";
                                    //ViewBag.vbcbsdls = null;

                                    //if (lAccNames.Count > 0)
                                    //    ViewBag.vbAcctName = lAccNames;
                                    //======================================================================= comment by amol for CMCP account name on 07/06/2023 start ==================
                                }

                                ViewBag.sCAPA = "Account is closed";
                                ViewBag.vberror = "Account is closed";
                                ViewBag.vbstatus = "SUCCESS";
                                ViewBag.vbcbsdls = null;
                                ViewBag.block = 1;

                            }
                        }

                    }



                }
                //logerror("Calling CBS details method end : ", "");
                ob = GetCPPSFlagAndPayeeNewAccFlag(id);

                cbsdtls.P2F = ob.P2F;
                cbsdtls.PayeeNewAccFlag = ob.PayeeNewAccFlag;
                cbsdtls.cpps = ob.CPPS_Flag;

                cbsdtls.BOFDRoutNo = GetBOFDRoutNo(id);
                 return PartialView("_GetApiCbsDtls", cbsdtls);
               //return PartialView("_AccountNames");
            }
            catch (Exception e)
            {
                logerrorInCatch("Error : ", e.Message.ToString());
               

                ErrorDisplay er = new ErrorDisplay();
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;
                er.ErrorMessage = message;
                //==================================================================================
                //error log
                logerrorInCatch("in GetCBSDtls catch==>"+e.Message.ToString(), "innerExp==>"+innerExcp);
                //====================================================================================

                return PartialView("Error", er);
            }
        }

        public string GetToken()
        {

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
               | SecurityProtocolType.Tls11
               | SecurityProtocolType.Tls12
               | SecurityProtocolType.Ssl3;

            //logic for fetching joint holders names from CMCP API

            //Token variables
            var TokenClientId = ConfigurationManager.AppSettings["TokenClientId"].ToString();
            var TokenSecreteKey = ConfigurationManager.AppSettings["TokenSecreteKey"].ToString();
            var TokenServiceURL = ConfigurationManager.AppSettings["TokenServiceURL"].ToString();

            //checking for date and 8 hours to get new token
            SqlDataAdapter adp = new SqlDataAdapter("GetToken", con);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataTable dt = new DataTable();
            adp.Fill(dt);

            //Call for token
            string sTokenResponse = "", sEtoken = "";

            //Get token
            if (dt.Rows.Count > 0)
            {
               // logerror("In GetToken method inside the Count : ", "");
                sEtoken = dt.Rows[0]["SettingValue"].ToString().Trim();
               // sEtoken = Session["sToken"].ToString().Trim();
               // logerror("In GetToken method inside the Count sEtoken value : ", sEtoken);
            }
            else
            {
               // logerror("In GetToken method inside the else : ", "");
               // logerror("In GetToken method calling sendCMCPTokenRequest method start : ", "");
                sTokenResponse = sendCMCPTokenRequest(TokenServiceURL, TokenClientId, TokenSecreteKey);
              //  logerror("In GetToken method calling sendCMCPTokenRequest method end : ", "");
              //  logerror("In GetToken method calling sTokenResponse value : ", sTokenResponse);

               // logerror("In GetToken method calling getCMCPToken method start : ", "");
                sEtoken = getCMCPToken(sTokenResponse);
               // logerror("In GetToken method calling getCMCPToken method end : ", "");
               // logerror("In GetToken method calling getCMCPToken method end sEtoken value : ", sEtoken);

                //Save new Token
                if (con.State == ConnectionState.Closed)
                    con.Open();

                logerror("In GetToken method calling UpdateToken SP start : ", "");
                SqlCommand cmd = new SqlCommand("UpdateToken", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@SettingValue", sEtoken);
                int iExec = cmd.ExecuteNonQuery();
                logerror("In GetToken method calling UpdateToken SP method end : ", "");
            }

            return sEtoken;
        }
        private string getAccountDetailsDBSRequest(string sServiceUrl, string sClientId, string sCoreRelationId, string sAccountNo, string sEToken)
        {
            sResposne = "";

            try
            {

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;

                sInputString = "";

                sInputString = " {";
                sInputString += "   \"sourceAccountNumber\": \"" + sAccountNo + "\"";
                sInputString += "}";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(sServiceUrl);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("x-Correlation-Id", sCoreRelationId);
                httpWebRequest.Headers.Add("x-sourcetimestamp", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                httpWebRequest.Headers.Add("x-sourceclientid", sClientId);
                httpWebRequest.Headers.Add("Authorization", "Bearer " + sEToken);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(sInputString);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    sResposne = result;
                }


            }
            catch (Exception Ex)
            {
                var exceptionMessage = "";
                if (Ex.Message == "The remote server returned an error: (422) Unprocessable Entity.")
                {
                    exceptionMessage = "Invalid Account";
                }
                else
                {
                    exceptionMessage = Ex.Message;
                }

                sResposne = "{" +
                             "\"error\":\"Runtime Error While Sending the Request\"," +
                             "\"errorDescription\":\"" + exceptionMessage +
                            "\"}";

                string message = "";
                string innerExcp = "";
                string trace = "";
                if (Ex.Message != null)
                    message = Ex.Message.ToString();
                if (Ex.InnerException != null)
                {
                    innerExcp = Ex.InnerException.Message;
                    trace = Ex.InnerException.StackTrace;
                }

                logerror("In getAccountDetailsDBSRequest catch===>>", message.ToString()+"InnerExp==>"+innerExcp);
            }


            return sResposne;
        }
        static int CalculateDifferenceInMonths(DateTimeOffset laterDate, DateTimeOffset earlierDate)
        {
            int monthsApart = 12 * (laterDate.Year - earlierDate.Year) + laterDate.Month - earlierDate.Month;

            if (laterDate.Day < earlierDate.Day)
            {
                monthsApart--;
            }

            return monthsApart;
        }
        string sendCMPCPRequest(string CMCPServiceURL, string CMCPCountry, string CMCPReqUID, string CMCPReqClientId, string sEtoken, string sAccountNo)
        {
            string sResponse = "";
            try
            {
                string sInputString = "";

                //sInputString = " {";
                //sInputString += "   \"cmcpId\": \"" + sAccountNo + "\"";
                //sInputString += "}";

                sInputString = " {";
                sInputString += "   \"cin\": \"" + sAccountNo + "\",";
                sInputString += "   \"cinSuffix\": \"00\"";
                sInputString += "}";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(CMCPServiceURL);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("X-DBS-Country", CMCPCountry);
                httpWebRequest.Headers.Add("X-DBS-ReqUID", CMCPReqUID);
                httpWebRequest.Headers.Add("X-DBS-ReqClientId", CMCPReqClientId);
                httpWebRequest.Headers.Add("Authorization", "Bearer " + sEtoken);

                //
                //txtRequest.Text = sInputString; ;
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(sInputString);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    sResponse = result;
                }
            }
            catch (Exception Ex)
            {
                sResponse = "{" +
                            "\"errorCode\":\"Runtime Error While Sending the Request\"," +
                            "\"errorDescription\":\"" + Ex.Message +
                           "}";
            }

            return sResponse;
        }
        string GetCMCPCustomerName(string sResposne)
        {
            string sCustomerName = "";
            try
            {
                
                var jObject = Newtonsoft.Json.Linq.JObject.Parse(sResposne);
                if (jObject["data"]["profileInfo"]["registeredName"] != null)
                {
                    sCustomerName = jObject["data"]["profileInfo"]["registeredName"].ToString().Trim();
                }
                else
                {
                    //WriteState code to log error
                    logerror(jObject["errorDescription"].ToString(), "");
                }
                return sCustomerName;
            }
            catch (Exception Ex)
            {
                logerrorInCatch("Exception in GetCMCPCustomerName ", Ex.Message.ToString());
                return sCustomerName;
            }
        }

        public string JointAccount()
        {
            string sResposne = "";

            string sInputString = "";

            sInputString = sInputString + "{";
            sInputString = sInputString + "\"Response\": {";
            sInputString = sInputString + "\"Header\": {";
            sInputString = sInputString + "\"Timestamp\":\"20220412153826344\",";
            sInputString = sInputString + "\"APIName\":\"casa-validator-api\",";
            sInputString = sInputString + "\"APIVersion\":\"1.1.0\",";
            sInputString = sInputString + "\"Interface\":\"CASA_Validator\"";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"Status\": {";
            sInputString = sInputString + "\"Code\":\"200\",";
            sInputString = sInputString + "\"Desc\":\"Success\"";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"Body\": {";
            sInputString = sInputString + "\"Description\":\"account is closed\",";
            sInputString = sInputString + "\"UUID\":\"kores20220412153225\",";
            sInputString = sInputString + "\"solID\":\"0012\",";
            sInputString = sInputString + "\"AcctId\":\"0012053000004770\",";
            sInputString = sInputString + "\"AcctType\": {";
            sInputString = sInputString + "\"SchmCode\":\"SBGEN\",";
            sInputString = sInputString + "\"SchmType\":\"SBA\"";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"AcctCurr\":\"INR\",";
            sInputString = sInputString + "\"AcctOpnDt\":\"16-02-1987\",";
            sInputString = sInputString + "\"ModeOfOper\":\"003\",";
            sInputString = sInputString + "\"BankAcctStatusCode\":\"\",";
            sInputString = sInputString + "\"CustId\": {";
            sInputString = sInputString + "\"CustId\":\"A00823501\",";
            sInputString = sInputString + "\"PersonName\": {";
            sInputString = sInputString + "\"LastName\":\"\",";
            sInputString = sInputString + "\"FirstName\":\"\",";
            sInputString = sInputString + "\"MiddleName\":\"\",";
            sInputString = sInputString + "\"Name\":\"PAUL P K\",";
            sInputString = sInputString + "\"TitlePrefix\":\"MR\"";
            sInputString = sInputString + "}";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"AcctName\":\"PAUL P K\",";
            sInputString = sInputString + "\"AcctShortName\":\"PAUL\",";
            sInputString = sInputString + "\"AcctStmtMode\":\"B\",";
            sInputString = sInputString + "\"AcctStmtFreq\": {";
            sInputString = sInputString + "\"Cal\":\"\",";
            sInputString = sInputString + "\"Type\":\"M\",";
            sInputString = sInputString + "\"StartDt\":\"1\",";
            sInputString = sInputString + "\"WeekDay\":\"0\",";
            sInputString = sInputString + "\"WeekNum\":\" \",";
            sInputString = sInputString + "\"HolStat\":\"N\"";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"AcctStmtNxtPrintDt\":\"01-06-2015\",";
            sInputString = sInputString + "\"DespatchMode\":\"E\",";
            sInputString = sInputString + "\"AcctBalCrDrInd\":\"C\",";
            sInputString = sInputString + "\"AcctBalAmt\": {";
            sInputString = sInputString + "\"amountValue\":\"36131.96\",";
            sInputString = sInputString + "\"currencyCode\":\"INR\"";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"FreezeStatusCode\":\"X\",";
            sInputString = sInputString + "\"FreezeReasonCode\":\"\",";
            sInputString = sInputString + "\"AccrIntDrCrInd\":\"C\",";
            sInputString = sInputString + "\"AccrIntRate\": {";
            sInputString = sInputString + "\"value\":\"2.35\"";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"IntCalcFreq\": {";
            sInputString = sInputString + "\"Cal\":\"\",";
            sInputString = sInputString + "\"Type\":\" \",";
            sInputString = sInputString + "\"StartDt\":\"0\",";
            sInputString = sInputString + "\"WeekDay\":\"0\",";
            sInputString = sInputString + "\"WeekNum\":\" \",";
            sInputString = sInputString + "\"HolStat\":\" \"";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"IntRateCode\":\"SBGEN\",";
            sInputString = sInputString + "\"NetIntDrCrInd\":\"C\",";
            sInputString = sInputString + "\"NetIntRate\": {";
            sInputString = sInputString + "\"value\":\"2.35\"";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"RelPartyRec\": [";
            sInputString = sInputString + "{";
            sInputString = sInputString + "\"RelPartyType\":\"M\",";
            sInputString = sInputString + "\"RelPartyTypeDesc\":\"MAIN HOLDER OF ACCOUNT\",";
            sInputString = sInputString + "\"RelPartyCode\":\"\",";
            sInputString = sInputString + "\"RelPartyCodeDesc\":\"\",";
            sInputString = sInputString + "\"CustId\": {";
            sInputString = sInputString + "\"CustId\":\"A00823501\",";
            sInputString = sInputString + "\"PersonName\": {";
            sInputString = sInputString + "\"LastName\":\"\",";
            sInputString = sInputString + "\"FirstName\":\"\",";
            sInputString = sInputString + "\"MiddleName\":\"\",";
            sInputString = sInputString + "\"Name\":\"PAUL P K\",";
            sInputString = sInputString + "\"TitlePrefix\":\"MR\"";
            sInputString = sInputString + "}";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"RelPartyContactInfo\": {";
            sInputString = sInputString + "\"PhoneNum\": {";
            sInputString = sInputString + "\"TelephoneNum\":\"+912702387\",";
            sInputString = sInputString + "\"FaxNum\":\"\",";
            sInputString = sInputString + "\"TelexNum\":\"\"";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"EmailAddr\":\"\",";
            sInputString = sInputString + "\"PostAddr\": {";
            sInputString = sInputString + "\"Addr1\":\"PEREPPADAN HOUSE\",";
            sInputString = sInputString + "\"Addr2\":\"SANTHIPURAM,MELOOR PO\",";
            sInputString = sInputString + "\"Addr3\":\"\",";
            sInputString = sInputString + "\"City\":\"KL972\",";
            sInputString = sInputString + "\"StateProv\":\"KL\",";
            sInputString = sInputString + "\"PostalCode\":\"680311\",";
            sInputString = sInputString + "\"Country\":\"IN\",";
            sInputString = sInputString + "\"AddrType\":\"\"";
            sInputString = sInputString + "}";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"RecDelFlg\":\"N\"";
            sInputString = sInputString + "},";
            sInputString = sInputString + "{";
            sInputString = sInputString + "\"RelPartyType\":\"J\",";
            sInputString = sInputString + "\"RelPartyTypeDesc\":\"JOINT HOLDER OF ACCOUNT\",";
            sInputString = sInputString + "\"RelPartyCode\":\"\",";
            sInputString = sInputString + "\"RelPartyCodeDesc\":\"WIFE\",";
            sInputString = sInputString + "\"CustId\": {";
            sInputString = sInputString + "\"CustId\":\"A03194062\",";
            sInputString = sInputString + "\"PersonName\": {";
            sInputString = sInputString + "\"LastName\":\"\",";
            sInputString = sInputString + "\"FirstName\":\"\",";
            sInputString = sInputString + "\"MiddleName\":\"\",";
            sInputString = sInputString + "\"Name\":\"TREESA PAUL\",";
            sInputString = sInputString + "\"TitlePrefix\":\"MRS\"";
            sInputString = sInputString + "}";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"RelPartyContactInfo\": {";
            sInputString = sInputString + "\"PhoneNum\": {";
            sInputString = sInputString + "\"TelephoneNum\":\"2739575\",";
            sInputString = sInputString + "\"FaxNum\":\"\",";
            sInputString = sInputString + "\"TelexNum\":\"\"";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"EmailAddr\":\"\",";
            sInputString = sInputString + "\"PostAddr\": {";
            sInputString = sInputString + "\"Addr1\":\"W/O P K PAUL,PEREPADAN HOUSE\",";
            sInputString = sInputString + "\"Addr2\":\"SANTHIPURAM,MELOOR PO\",";
            sInputString = sInputString + "\"Addr3\":\"\",";
            sInputString = sInputString + "\"City\":\"KL972\",";
            sInputString = sInputString + "\"StateProv\":\"KL\",";
            sInputString = sInputString + "\"PostalCode\":\"680311\",";
            sInputString = sInputString + "\"Country\":\"IN\",";
            sInputString = sInputString + "\"AddrType\":\"\"";
            sInputString = sInputString + "}";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"RecDelFlg\":\"N\"";
            sInputString = sInputString + "}";
            sInputString = sInputString + "],";
            sInputString = sInputString + "\"acct_status\":\"A\",";
            sInputString = sInputString + "\"customer_band\":\"\",";
            sInputString = sInputString + "\"customerisMinor\":\"N\",";
            sInputString = sInputString + "\"customerisNRE\":\"N\"";
            sInputString = sInputString + "}";
            sInputString = sInputString + "}";
            sInputString = sInputString + "}";

            sResposne = sInputString.Trim();

            return sResposne;

        }

        private string sendCMCPTokenRequest(string TokenServiceURL, string TokenClientId, string TokenSecreteKey)
        {
            string sResposne = "";
            try
            {
                string sBase64String = TokenClientId + ":" + TokenSecreteKey;
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(sBase64String);
                sBase64String = System.Convert.ToBase64String(plainTextBytes);
                var stringContent = new StringContent(string.Empty);

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(TokenServiceURL);
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", "basic " + sBase64String);
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(stringContent);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    sResposne = result;
                }
            }
            catch (Exception Ex)
            {
                sResposne = "{" +
                            "\"errorCode\":\"Runtime Error While Sending the Request\"," +
                            "\"errorDescription\":\"" + Ex.Message +
                           "}";
            }
            return sResposne;
        }
        string getCMCPToken(string sResposne)
        {
            string sEtoken = "";

            var jObject = Newtonsoft.Json.Linq.JObject.Parse(sResposne);
            if (jObject["access_token"] != null)
                sEtoken = jObject["access_token"].ToString();
            else
            {
                //WriteState code to log error

                //jObject["errorDescription"].ToString()
            }

            return sEtoken;
        }

        //test function
        public string testCMCP_Response()
        {
            var data = "";
            return data = "{\"data\":{\"cmcpId\":\"IN1I2200Z52FL\",\"cin\":\"23997348\",\"cinSuffix\":\"00\",\"customerType\":1,\"profileInfo\":{\"dbsIndustryCode\":{\"codeValueId\":583,\"codeValueCd\":\"99950\",\"referenceCodeValueCd\":\"99950\",\"codeValueDisplay\":\"PRIVATEINDIVIDUALS\"},\"countryOfResidenceCode\":{\"codeValueId\":1959,\"codeValueCd\":\"IN\",\"referenceCodeValueCd\":\"IN\",\"codeValueDisplay\":\"INDIA\"},\"registeredName\":\"5MvnqWFkVzt9bnJjndaGIndNkzombY0RyqBlI4csfPsnmlWzE9H9c7MEPpPLWm97ENV4vLjcWE0SMAZSUgOfqBfsG0moX0BMrIX\",\"nameLine1\":\"j873YaKrblaMd1Pjgx51UyTs1TUlEdvCfuXidNhdUT3yUOxgRg3EOMK3FxaXiN4CsCqe7ztcrotZjp7v\",\"nameLine2\":\"8YdAHBeNIK5VIJGk0j5ke5A9h2N7TGVaiubsL9CDXeUvCJB8b5cMCxGaYqYZLra4V8y3z88XzpoDYcFh\",\"customerSubTypeCode\":{\"codeValueId\":694,\"codeValueCd\":\"R\",\"referenceCodeValueCd\":\"R\",\"codeValueDisplay\":\"RETAIL\"},\"relationshipStartDate\":\"2021-03-20\",\"nameInNativeLanguage\":\"P5ljQEZ7U9KQYHkkzWZCXZ3jc2TKldNyjGD5VJg5fT25IyPH236u22Su41VR8utPgoMrfQXanJxzUXPB\",\"preferredLanguageCode\":{\"codeValueId\":12014,\"codeValueCd\":\"001\",\"referenceCodeValueCd\":\"001\",\"codeValueDisplay\":\"ENGLISH\"},\"remarks\":\"\",\"taxDeductionAtSourceTableCode\":{\"codeValueId\":57956,\"codeValueCd\":\"TDS01\",\"referenceCodeValueCd\":\"TDS01\",\"codeValueDisplay\":\"TDS01\"},\"sectorCode\":{\"codeValueId\":57264,\"codeValueCd\":\"31005\",\"referenceCodeValueCd\":\"31005\",\"codeValueDisplay\":\"BANKING/FINANCE\"},\"subSectorCode\":{\"codeValueId\":57698,\"codeValueCd\":\"30310\",\"referenceCodeValueCd\":\"30310\",\"codeValueDisplay\":\"BANKS-REPRESENTATIVEOFFICES\"},\"forexTierGroupCode\":{\"codeValueId\":57191,\"codeValueCd\":\"10\",\"referenceCodeValueCd\":\"10\",\"codeValueDisplay\":\"FXGroup10\"},\"regionCode\":{\"codeValueId\":57245,\"codeValueCd\":\"02\",\"referenceCodeValueCd\":\"02\",\"codeValueDisplay\":\"West\"},\"taxStatus\":{\"codeValueId\":57971,\"codeValueCd\":\"800001\",\"referenceCodeValueCd\":\"800001\",\"codeValueDisplay\":\"ResidentTax\"},\"constitutionCode\":{\"codeValueId\":1856,\"codeValueCd\":\"15\",\"referenceCodeValueCd\":\"15\",\"codeValueDisplay\":\"INDIVIDUAL\"},\"customerNetWorth\":\"4999999\",\"profitCenterCode\":{\"codeValueId\":57240,\"codeValueCd\":\"06\",\"referenceCodeValueCd\":\"06\",\"codeValueDisplay\":\"Retail\"},\"previousName\":\"TYiCedQ9mhPPVL9euoniv0F3SDg5Ae1kdDAGZUgahODYgK2fyOpcSarD1cBQmDhbaM1nKzELAU86znYkbrHVJKL0FRB8fD6DzMOFz2N4Q0OL1VTUb0hLPPfieNIQYeOo39iMRdLDG9x49EhJsrH3PbgYMdipSgOi\",\"primaryBranchCode\":{\"codeValueId\":58744,\"codeValueCd\":\"854\",\"referenceCodeValueCd\":\"854\",\"codeValueDisplay\":\"VIKHROLI\"},\"alias\":\"IVMZ5NbKfJlipTnpCBfVqW1Ms132PUOmqqAWBYhhDTMOTiBKsOFRPdX2Pf0f\",\"salutationCode\":{\"codeValueId\":16,\"codeValueCd\":\"MR\",\"referenceCodeValueCd\":\"MR\",\"codeValueDisplay\":\"MR\"},\"sexCode\":{\"codeValueId\":25,\"codeValueCd\":\"M\",\"referenceCodeValueCd\":\"M\",\"codeValueDisplay\":\"MALE\"},\"maritalStatusCode\":{\"codeValueId\":28,\"codeValueCd\":\"2\",\"referenceCodeValueCd\":\"2\",\"codeValueDisplay\":\"MARRIED\"},\"nativeLanguageCode\":{\"codeValueId\":13662,\"codeValueCd\":\"INFENG\",\"referenceCodeValueCd\":\"INFENG\",\"codeValueDisplay\":\"ENGLISH\"},\"countryOfBirthCode\":{\"codeValueId\":1959,\"codeValueCd\":\"IN\",\"referenceCodeValueCd\":\"IN\",\"codeValueDisplay\":\"INDIA\"},\"maidenName\":\"xIFPJmGkrc2q3gbaRjSo94EBUYx8duqkSekcuMPQ3e9ojuhMCJ0f3W9CUxXK3Au8zk6BZeUS3QfCFMnqkCk5IdvXT8Vjr5phesN\",\"dateOfBirth\":\"1980-11-15\",\"motherMaidenName\":\"Vhc2GyrQqbb5mlhQLRGB2i2987EPsdNqBvj6C5UsfGfEhW81bYWQmSe1r4U4l3W48f64q5uUabLIcDPc5LZ3EPMu6eL1KfIv9r3\",\"fatherName\":\"oLai5QCdDDK1m7tYqs05T3GXp6j09Q6N9c8TSmcHjJZTdfHKMxo4am822mWIHn7zPHI1gVv3NRNEJ2c7lVX1NWBTRExiVaLTBKe\",\"spouseName\":\"PAOFN4MCilVWqsXvbeWmpVh3Jf3VK5GMxe6mmKtDhsm1MH8TWOgAgp8RRujlSfPhmj8J8ujCZbkBY6vQ0MO1N9gA8MHgOqi5buC\",\"staffFlag\":true,\"staffId\":\"ViniaHeng\",\"industryType2Code\":{\"codeValueId\":56936,\"codeValueCd\":\"410\",\"referenceCodeValueCd\":\"410\",\"codeValueDisplay\":\"4.1Individuals(includingHUF)\"},\"preferredName\":\"zOA05eVKpFvRNDVadOLz5rAhya1folMFFYuDbC8abf3hheNVFW5JzMEuVsuRAanT6jerDxORB7hSCTBkQ0e3UgE7zOeTN96XJYo\",\"version\":1,\"createdBy\":\"FIVUSR\",\"createdByChannel\":\"MIGRATION\",\"createdTimeStamp\":\"2021-03-20T13:11:58+08:00\",\"updatedBy\":\"SEJALAGRAWAL\",\"updatedByChannel\":\"MIGRATION\",\"updatedTimeStamp\":\"2022-02-10T19:56:58+08:00\"},\"owners\":[{\"id\":831384,\"ownerCode\":{\"codeValueId\":1835,\"codeValueCd\":\"0001\",\"referenceCodeValueCd\":\"0001\",\"codeValueDisplay\":\"MassMarket\"},\"ownerOrder\":1,\"customerOwnerTPCIndicator\":\"04\",\"version\":1,\"createdBy\":\"FIVUSR\",\"createdByChannel\":\"MIGRATION\",\"createdByDepartment\":\"MIGRATION\",\"createdTimeStamp\":\"2021-03-20T13:11:58+08:00\",\"updatedBy\":\"SEJALAGRAWAL\",\"updatedByChannel\":\"MIGRATION\",\"updatedByDepartment\":\"MIGRATION\",\"updatedTimeStamp\":\"2022-02-10T19:56:58+08:00\"}],\"attributes\":[{\"id\":6072502,\"customerAttributeCode\":{\"codeValueId\":2466,\"codeValueCd\":\"01\",\"referenceCodeValueCd\":\"01\",\"codeValueDisplay\":\"ACTIVE\",\"attributeValue\":\"3\"},\"statusSource\":\"bmxhUy3nBg86VLGkd46XuprF4JAWgX0jVyPbqTs4lTE9y8kzYJAZyUSDVb5x9XoT5k9Gq8NlNJJs4DXYMOpFs60ZIxg3Z9nHUUbVg6p6j0cYyYINojkK7H9LCX1L8pdntdECAEfhGd9PPFPSOQxuDHRB7n3hIYtehhDCS4VdyeZfiDjXRyXS1S3RXUIftNBbchNmU3WiCy6hAdM05XCEyJpL7nrBerIJ9mQ0VxjiHgcOvsJAbN63PO4atNg3kyi310b79yfF0tIIl2RY8ZOCVmqOsI2O8yFrkNNmMTyHf8O3\",\"version\":1,\"createdBy\":\"FIVUSR\",\"createdByChannel\":\"MIGRATION\",\"createdTimeStamp\":\"2021-03-20T13:11:58+08:00\",\"updatedBy\":\"SEJALAGRAWAL\",\"updatedByChannel\":\"MIGRATION\",\"updatedTimeStamp\":\"2022-02-10T19:56:58+08:00\"}],\"segments\":[{\"id\":2130415,\"segmentCode\":{\"codeValueId\":57990,\"codeValueCd\":\"IN000015\",\"referenceCodeValueCd\":\"IN000015\",\"codeValueDisplay\":\"MASSMKTMUMBAI\"},\"segmentTypeCode\":{\"codeValueId\":2530,\"codeValueCd\":\"01\",\"referenceCodeValueCd\":\"01\",\"codeValueDisplay\":\"SEGMENTLEVEL1\"},\"segmentSubTypeCode\":{\"codeValueId\":2533,\"codeValueCd\":\"1\",\"referenceCodeValueCd\":\"0001\",\"codeValueDisplay\":\"PRIMARY\"},\"tpcIndicator\":\"04\",\"version\":1,\"createdBy\":\"FIVUSR\",\"createdByChannel\":\"MIGRATION\",\"createdTimeStamp\":\"2021-03-20T13:11:58+08:00\",\"updatedBy\":\"SEJALAGRAWAL\",\"updatedByChannel\":\"MIGRATION\",\"updatedTimeStamp\":\"2022-02-10T19:56:58+08:00\"}],\"addresses\":[{\"id\":1097352,\"startDate\":\"2021-03-20\",\"endDate\":\"2099-12-31\",\"addressLabel\":\"ZByfmmxZxKbov7HAvJdaDUxFlSq16JHVZzddscvnFXRgq6CrXV2TifE7J5ozTDpDOSNvvXR5sULFx1Jx3daRLs2YtXdoEKrndPa\",\"addressTypeCode\":{\"codeValueId\":22771,\"codeValueCd\":\"Mailing3\",\"referenceCodeValueCd\":\"Mailing3\",\"codeValueDisplay\":\"Mailing3\"},\"levelNumber\":\"AVH\",\"unitNumber\":\"nfgGZvE\",\"blockNumber\":\"SpbMK\",\"streetName1\":\"F4O0OjnHVJsKcpg1iKZISeRRzMdvHraY6XrJ3vKbNm2Ho3oQ0g1qVTJFkXpzCdlWveh9KtCtyU6FzXayW5xz9WC7AOOEbeYbktD\",\"streetName2\":\"ujIIeZFbf2p2m7ILcBrWgMiUnl83m1nU2E5uJEtU5bBJ0PkyHjBpVFcfjULHA5ipkK1i6cLd3Z8ZIQbaGND79nNAk8BpL6NYcW5\",\"streetName3\":\"EURxm8XAUtAxrzgzpamPHqqxuOFZnfR7kyYbJSvim8Yy8BqAVCagDGYSSRJ5GqcRVjIS2HsnGvkihSQaBLKmxZepaByZpj9Asa0\",\"streetName4\":\"YMTWJHNuQdSJmPrdxPJ6EQAV7bMoaLXvWgJ9Jq5gAU2Q7DDqDvL66dC5hryYloYfPopQ9MKCai3K48pGI6ZnWsxGa8o4847BSqI\",\"streetName5\":\"kRQbqfgW7p7H9SNBbodeSFob3hbPEg8RuIDFoSeLxs1WoYH5dAqqNqMae1LBGTr5AaF87z57GdWLfb7MOOzGNPqDafWGLTkfxfy\",\"streetName6\":\"VKtZoAQhUpOXgpG8a755efmiisZa8oVLhhylRIYp5SOGiat0t6SpEMn4M3GElczlbK6x3xSeI6vdPvrpEL5XTnR3C6J1I00hneX\",\"postalCode\":\"547333\",\"cityCode\":{\"codeValueId\":51305,\"codeValueCd\":\"30805\",\"referenceCodeValueCd\":\"30805\",\"codeValueDisplay\":\"KALYAN\"},\"stateCode\":{\"codeValueId\":57353,\"codeValueCd\":\"MH\",\"referenceCodeValueCd\":\"MH\",\"codeValueDisplay\":\"MAHARASHTRA\"},\"countryCode\":{\"codeValueId\":1959,\"codeValueCd\":\"IN\",\"referenceCodeValueCd\":\"IN\",\"codeValueDisplay\":\"INDIA\"},\"nativeStreetName1\":\"lX64uqsWYkaApbTDqyZPDsKFWBjxOnNjorc7gy4PsQLvPuMvYh6CZk2Ra7pxg0UHdIqsxJOSakxHSEKrOes44syCFs11KVhQBAR\",\"nativeStreetName2\":\"TgARkCahh1mg2ultSQrrS2mlnYDBU6Ndv7E42ECyrdWJLGbuSz3vMn8FnueLTRVzYugRTgsOnb9eUsZJ2GBYQcU30JM8JAEdmQJ\",\"nativeStreetName3\":\"2oqOLOeZpoMzXllzfLN0XryEecPtF3mSzDb9kN9eE5shj7XAoCcZKZPKt0IWd0fF6Se8BCaP5oUqBAZUBn5nxixbB64XBgIpEye\",\"nativeStreetName4\":\"EQA8d49m2gQq5YcH7s7TI8FlJ4q6IEudjILXHC65MIMFcYNTdkKblbUn8yBuCrFUyUuYNjcplMfEVdr4WpCcV5QYvgl987ML1An\",\"preferredFlag\":false,\"holdMailFlag\":false,\"holdMailReason\":\"klvGY0NGpZjxHCrtD9HZGUAmKnQzcGWJiD69Tj9XzpaC6O0vYfqj52UgzcHTNE5pxO2lkHmklYKiRToX0thTkbSS890KpO48mXZ\",\"formatAddress1\":\"BLKSpbMKF4O0OjnHVJsKcpg1iKZISeRRzMdvHr\",\"formatAddress2\":\"ujIIeZFbf2p2m7ILcBrWgMiUnl83m1nU2E5uJEtU5bBJ0PkyHjBpVFcfjULHA5ipkK1i6cLd3Z8ZIQbaGND79nNAk8BpL6NYcW5\",\"formatAddress3\":\"EURxm8XAUtAxrzgzpamPHqqxuOFZnfR7kyYbJSvim8Yy8BqAVCagDGYSSRJ5GqcRVjIS2HsnGvkihSQaBLKmxZepaByZpj9Asa0\",\"formatAddress4\":\"#AVH-nfgGZvESINGAPORE547333\",\"version\":1,\"createdBy\":\"MIGRATION\",\"createdByChannel\":\"MIGRATION\",\"createdByDepartment\":\"\",\"createdTimeStamp\":\"2022-09-17T00:00:00+08:00\",\"updatedBy\":\"MIGRATION\",\"updatedByChannel\":\"MIGRATION\",\"updatedByDepartment\":\"\",\"updatedTimeStamp\":\"2021-03-20T13:11:58+08:00\"},{\"id\":1097353,\"startDate\":\"2021-03-20\",\"endDate\":\"2099-12-31\",\"addressLabel\":\"E153hYLHclTIoYmFgjNeQopHZMBNJgPgxlLynlgr2UuWeOOvC7ivR3HpVymbdAX4nCcKddzlCmYLWpeXZe3HC7MArXIUQKqGZus\",\"addressTypeCode\":{\"codeValueId\":11003,\"codeValueCd\":\"Mailing\",\"referenceCodeValueCd\":\"Mailing\",\"codeValueDisplay\":\"Mailing\"},\"levelNumber\":\"WbV\",\"unitNumber\":\"6uWHOMg\",\"blockNumber\":\"XeO7u\",\"streetName1\":\"KVRhalVhSI00kNx1WylD6NQKKgFYyQCX3hGWoEdr59lm2XGbQJrIELPl74E7prYeIGONP2rcqti5iFrXvIDK7Pu5DxFYQ8Ll1nv\",\"streetName2\":\"BU2IRRjtneN8zUZuyR4mVmvqokCOsC2HNEtNm5NJqUQ90B0FT02bqPrXEMM0ZrVUvD5eZmDnccVofGcYnQDzC0yoBqrRkcvn3mr\",\"streetName3\":\"SSz2LTKpRdLrF5TnKDqk9SFE0biXqN8B4WIdZjLtEuGs49ye17E9IxQ4Zbuz1ZqtyxvX34mkQZyRD1TPmfkQ84LrdGobfoWcVhz\",\"streetName5\":\"ls7MJyXH8dMefc8rhyaR80erKa3YeDFkZXti43dEdXLSJKsMLy5ulM5o60PvizY51U57fXTTlyS4TXY0M9WgsQIzb7HkP5z895l\",\"streetName6\":\"FRlX3HRBHJCHpZPYl7hnmvt3rbNl6Vipe58GqGdDU2hJtr63tTRQv386gHgH78NP8IiSYeSbeNN9sUSy8sadeLEYPHMkpNq72MW\",\"postalCode\":\"902276\",\"cityCode\":{\"codeValueId\":51305,\"codeValueCd\":\"30805\",\"referenceCodeValueCd\":\"30805\",\"codeValueDisplay\":\"KALYAN\"},\"stateCode\":{\"codeValueId\":57353,\"codeValueCd\":\"MH\",\"referenceCodeValueCd\":\"MH\",\"codeValueDisplay\":\"MAHARASHTRA\"},\"countryCode\":{\"codeValueId\":1959,\"codeValueCd\":\"IN\",\"referenceCodeValueCd\":\"IN\",\"codeValueDisplay\":\"INDIA\"},\"nativeStreetName1\":\"I5aL7aCmrjcU1Jn9UM1gFnvYnHBnQOW160aGC4NNO5G11hRpvaU4vxLAYkETEaSQqq2ozttc8GG71vP6fBydoK2A1vyelZ58jrB\",\"nativeStreetName2\":\"dGACNZPlJzFpurukqb0mA4Jr7694Bg6yHl812JuvI2V0OMgz0QLLLD13hpajtIltRsl5ZjMYomm5Zh7ic9TUysfFOl9rsFTVnZo\",\"nativeStreetName3\":\"oiFXxdqVZW8WqX0dEnrtF9FHFnLyH4y6Pg1xU68t4pc0O8HCscd8oJIB8HdGMazN481KII1NNbtMZA3LpMX7E6tVu3DKlCWcEzm\",\"nativeStreetName4\":\"RuBCREzmbjqk0PLViUe1Nu4FdxXf6QKty1XBHzxvJk7i1oC3yBeZO0NxtGxYzdoCrWUKlF2hRujFmZVLaQLzcHx9Z0LdQrC1XLY\",\"preferredFlag\":true,\"holdMailFlag\":false,\"holdMailReason\":\"VhOYu8Mj6bmIhEzvPqKXRu8ntxoI7Wf8EF2iz810dpxR0qN6hN1F59ql9rOCdpjZ6l9yPu1IqQLq5smXUXVlgd50qUL1RArMTOu\",\"formatAddress1\":\"BLKXeO7uKVRhalVhSI00kNx1WylD6NQKKgFYyQ\",\"formatAddress2\":\"BU2IRRjtneN8zUZuyR4mVmvqokCOsC2HNEtNm5NJqUQ90B0FT02bqPrXEMM0ZrVUvD5eZmDnccVofGcYnQDzC0yoBqrRkcvn3mr\",\"formatAddress3\":\"SSz2LTKpRdLrF5TnKDqk9SFE0biXqN8B4WIdZjLtEuGs49ye17E9IxQ4Zbuz1ZqtyxvX34mkQZyRD1TPmfkQ84LrdGobfoWcVhz\",\"formatAddress4\":\"#WbV-6uWHOMgSINGAPORE902276\",\"version\":1,\"createdBy\":\"MIGRATION\",\"createdByChannel\":\"MIGRATION\",\"createdByDepartment\":\"\",\"createdTimeStamp\":\"2022-09-17T00:00:00+08:00\",\"updatedBy\":\"MIGRATION\",\"updatedByChannel\":\"MIGRATION\",\"updatedByDepartment\":\"\",\"updatedTimeStamp\":\"2021-03-30T15:05:39+08:00\"},{\"id\":1097355,\"startDate\":\"2021-03-20\",\"endDate\":\"2099-12-31\",\"addressLabel\":\"Ak5jiECxf10UVAk6CL6aI7LhJjB0dZuRyZeolmpSoxuFAiQjEUobVMb2PxGbeycltO4cneGfKSkgnb8PTkrssSi7GS3dHfLlNF4\",\"addressTypeCode\":{\"codeValueId\":22768,\"codeValueCd\":\"Home\",\"referenceCodeValueCd\":\"Home\",\"codeValueDisplay\":\"Home\"},\"levelNumber\":\"PQg\",\"unitNumber\":\"IWZq3I0\",\"blockNumber\":\"T9xyI\",\"streetName1\":\"1jRYzB6e9n3RSgTC7IyyvmqMYD6ICxFDQ1laMM5BBT1VlK1M7C8uY3hbK7bu84ElgXJzWk673BzDnH9njhfLZvGFROygj0Iq0G6\",\"streetName2\":\"Fba0XQXB9UuOpWIsraBlJCWT9KM1yLCKMgKelrmKPlGVuepE79JV6g5vTQfuig5QjEgYeLthmR2gIcqe25r03DlsWzhcQc6xD3V\",\"streetName3\":\"YiQqp6ton2f0BamZgZ9toJcLNmxqLnEURPIiHNvnX8aN8rUxcnK1vFMrlzj1vQakegvaopqIOfrM6thtP2XShtfNKSEOxzgtkmk\",\"streetName4\":\"uAov9Rd5hUY2W47A88VmmDD2psEjsv0hUJsgBJjpPKUk8pqXZDD6iLjtnjXQmksOOr9mrxBK5TyOkjhlamo315yCspcJsRJ9cx2\",\"streetName5\":\"iX9NUr4fDtvA0difyLgkrGRcBKuIMNe6PPKHBYeKlFEAJ60XEXWCvPstg5Kc7zFAq3eghdKWqVA1TROKaikziAZC03GRlI75Jkt\",\"streetName6\":\"VQsEpT12Yk8xy43KeHouol5qyVY0mX0Ft9UWEl5mQtYf9PsK4U7eqFTvHus0Dnq64uQFrHyos4d8jhnh0RqFvfyUPuNDJavFP1R\",\"postalCode\":\"142353\",\"cityCode\":{\"codeValueId\":51305,\"codeValueCd\":\"30805\",\"referenceCodeValueCd\":\"30805\",\"codeValueDisplay\":\"KALYAN\"},\"stateCode\":{\"codeValueId\":57353,\"codeValueCd\":\"MH\",\"referenceCodeValueCd\":\"MH\",\"codeValueDisplay\":\"MAHARASHTRA\"},\"countryCode\":{\"codeValueId\":1959,\"codeValueCd\":\"IN\",\"referenceCodeValueCd\":\"IN\",\"codeValueDisplay\":\"INDIA\"},\"nativeStreetName1\":\"X9mIGi0n9YYRbRHmBb1AF5hTAhE2sTQg1RRVKW4NUH7Sz9Ihr0GNttABcOolC4IseMJ2qrMsFxibW68bJMdTD61DyOHDhKMOYtf\",\"nativeStreetName2\":\"A0Up4AGRu5P6g5zG90N4M11LqAqiuWgnDzBOraolrrEs3URGp9AqptWJClxUUl1LgUEiNRqk7C0PqPclzk6RTUSsCWYiI3zubAu\",\"nativeStreetName3\":\"mZQQt8jqnltTQGqKmn62PTpidxdREr4lE2tz4rGFolldFNLE2HPh4LihXZzpW4Ept7jqME9DL3eQgPQ3uYu055ikmrQRMuNvCiy\",\"nativeStreetName4\":\"kW36MpncWgPg1OjPYAEeZnOa3yksFDrgQT1CaivF0OYfcWg59qpMRjfQM8vfrWBZycmkRNrYNGosNe8e1ZY3MCT3CrBDGm01GCc\",\"preferredFlag\":false,\"holdMailFlag\":false,\"holdMailReason\":\"Ae10ZQsrnVIfqXxUfFWaUmLvMXBlBMMJldpJGWrcPOyEUFCJ4zArbi3jgHaq4bPSPhFn1m05tpBZm9tzd5RB6Las0XqD2CMP2KX\",\"formatAddress1\":\"BLKT9xyI1jRYzB6e9n3RSgTC7IyyvmqMYD6ICx\",\"formatAddress2\":\"Fba0XQXB9UuOpWIsraBlJCWT9KM1yLCKMgKelrmKPlGVuepE79JV6g5vTQfuig5QjEgYeLthmR2gIcqe25r03DlsWzhcQc6xD3V\",\"formatAddress3\":\"YiQqp6ton2f0BamZgZ9toJcLNmxqLnEURPIiHNvnX8aN8rUxcnK1vFMrlzj1vQakegvaopqIOfrM6thtP2XShtfNKSEOxzgtkmk\",\"formatAddress4\":\"#PQg-IWZq3I0SINGAPORE142353\",\"version\":1,\"createdBy\":\"MIGRATION\",\"createdByChannel\":\"MIGRATION\",\"createdByDepartment\":\"\",\"createdTimeStamp\":\"2022-09-17T00:00:00+08:00\",\"updatedBy\":\"MIGRATION\",\"updatedByChannel\":\"MIGRATION\",\"updatedByDepartment\":\"\",\"updatedTimeStamp\":\"2021-03-20T13:11:58+08:00\"}],\"emails\":[{\"id\":1951478,\"emailAddress\":\"IN1I2200Z52FL@dbstest.com\",\"emailTypeCode\":{\"codeValueId\":23090,\"codeValueCd\":\"51\",\"referenceCodeValueCd\":\"51\",\"codeValueDisplay\":\"OFFICIAL\"},\"emailStatusCode\":{\"codeValueId\":902,\"codeValueCd\":\"V\",\"referenceCodeValueCd\":\"V\",\"codeValueDisplay\":\"VERIFIED\"},\"preferredFlag\":true,\"version\":1,\"createdBy\":\"MIGRATION\",\"createdByChannel\":\"MIGRATION\",\"createdTimeStamp\":\"2022-09-17T00:00:00+08:00\",\"updatedBy\":\"MIGRATION\",\"updatedByChannel\":\"MIGRATION\",\"updatedTimeStamp\":\"2021-03-30T15:05:40+08:00\"}],\"contacts\":[{\"id\":130347,\"contactTypeCode\":{\"codeValueId\":899,\"codeValueCd\":\"4\",\"referenceCodeValueCd\":\"4\",\"codeValueDisplay\":\"HANDPHONE\"},\"contactSuffix\":\"00\",\"contactPersonName\":\"mGsp6eu6bTayXRuQ4eF9IVpqrTl75fK5Rdb2SfZzHKZVJhVIm6BTXIavNNH5u0RfmYOePzl5mLyGCyzA6clfjI9UzCnIPcBSyCkVZPhZbGVHQVlRzjv3eykIg9ejfUAFIQGMlxZjJTt6FAUMOIT2yy6yK6jOLxZKNVNhIuYIn2u5hk3Eqjchn7DvWClXdKRN0oC7qcQH\",\"contactPhoneNumber\":\"378936910\",\"localNumber\":\"280718784\",\"countryCode\":{\"codeValueId\":1539,\"codeValueCd\":\"91\",\"referenceCodeValueCd\":\"91\",\"codeValueDisplay\":\"INDIA\"},\"preferredFlag\":true,\"version\":1,\"createdBy\":\"MIGRATION\",\"createdByChannel\":\"MIGRATION\",\"createdTimeStamp\":\"2022-09-17T00:00:00+08:00\",\"updatedBy\":\"MIGRATION\",\"updatedByChannel\":\"MIGRATION\",\"updatedTimeStamp\":\"2021-03-30T15:05:40+08:00\"}],\"nationalities\":[{\"id\":934479,\"nationalityCode\":{\"codeValueId\":57046,\"codeValueCd\":\"IN\",\"referenceCodeValueCd\":\"IN\",\"codeValueDisplay\":\"INDIA\"},\"version\":1,\"createdBy\":\"MIGRATION\",\"createdByChannel\":\"MIGRATION\",\"createdTimeStamp\":\"2021-03-30T15:05:40+08:00\",\"updatedBy\":\"MIGRATION\",\"updatedByChannel\":\"MIGRATION\",\"updatedTimeStamp\":\"2021-03-30T15:05:40+08:00\"}],\"educationDetails\":[],\"employmentDetails\":[{\"id\":3402614,\"employerName\":\"LY1vfUFhAubjUzVJyfb5ISnRdzTN3ZoG6x0TqUsgyiNEHt7hAXFn0skdGYinAe7ZEEat7rJOjdOKxWhT0FLVrED0JvPf2hbpbRKyBMi0Ir22ieenyfpeUKsvL3fq1i8Fz5lLhXQUggHmoAK4b01A5CAVrbxooboii\",\"employerNameInAlternateLanguage\":\"ecY2J3Oyg5cAQhNKqppyjAjWmIS3tZr0cZrDnXC8thAGnT0z1SUJHk5Blytz2CnidJeW2m9krgJupML3JNC2mXdmY2hj5pSXK2m1lQkLYWJJ49bDY2Em7PKsKGaGVtI2gC9YeyPPSY7gFNgIDxbhEd5JALSvSaNzqzDdI02HIgeiOnh51hgGdDaagPV5XRQf1mg6QljPIvvHansjz2U4THzvo06KZAZmbx4UkH0OSr7jE81Z5ytRQfeYSeXjR7V\",\"addressLine1\":\"AMKQgMD7Me3sRzO9UWqFsL7fU6gaJKIxKC1LGIKB2mK31tXGRzFHBVtExfmsbQtCfvAx2IYjTeSLtoTU9BCjk2pCKCVSJleEsJZCVYeRCjaG8pu8caWAmpuqNUEeK4hYz05e9CnpIKcDyOJfAYfMStWq2X9mvWmU3d4gSa8pn7ytkVTxe3N9Hu2kR5umXpfjdgAttgOHxQ6dN9RmdLv3uzg3V5qnOfVnXZh8E9728JnuAkhpTb1D12uRUcyoff2\",\"addressLine2\":\"zTWIjB7ETHYM8GiFYE20J382gNmTe8gMHoj0PihO3UMZvBCu0KAjdaDKvcLUZyFJaSoqGpl8KsfmG0N2hmOJoloZp39nIHNPFPvqKxXUOblCYsWriYHoiKGrgjnNU8GQ5SHjMFV4lkOdnAAYSCJnbmv4U6b5EeCSFa2bD2BSp20Jh3oP8mqBOO0yDR2vjcQqBxvT1d0brSCfTvvoVk16FEearxZzLtjM6N3JEriR0jq1d0TVFdjdUko7yXgYYQG\",\"postalCode\":\"GBygB43055\",\"cityCode\":{\"codeValueId\":51140,\"codeValueCd\":\".\",\"referenceCodeValueCd\":\".\",\"codeValueDisplay\":\".\"},\"stateCode\":{\"codeValueId\":57304,\"codeValueCd\":\".\",\"referenceCodeValueCd\":\".\",\"codeValueDisplay\":\".\"},\"phoneNumber\":\"lc5L4ccRuqpLdZypU3Me\",\"phoneAreaNumber\":\"G9ABYCAtIjzWNCLnoI28\",\"faxNumber\":\"VlfZhGeCfhzqob27NLemyOgAaPial3\",\"incomeRangeFrom\":\"1500000\",\"incomeRangeTo\":\"2999999\",\"employmentStatusCode\":{\"codeValueId\":881,\"codeValueCd\":\"02\",\"referenceCodeValueCd\":\"02\",\"codeValueDisplay\":\"PERMANENTEMPLOYED-FULLTIME\"},\"sourceOfIncomeCode\":{\"codeValueId\":23107,\"codeValueCd\":\"003\",\"referenceCodeValueCd\":\"003\",\"codeValueDisplay\":\"Inheritance\"},\"occupationalGroupCode\":{\"codeValueId\":14495,\"codeValueCd\":\"13\",\"referenceCodeValueCd\":\"13\",\"codeValueDisplay\":\"SERVICE–PRIVATESECTOR\"},\"version\":1,\"createdBy\":\"FIVUSR\",\"createdByChannel\":\"MIGRATION\",\"createdTimeStamp\":\"2021-03-20T13:11:58+08:00\",\"updatedBy\":\"SEJALAGRAWAL\",\"updatedByChannel\":\"MIGRATION\",\"updatedTimeStamp\":\"2021-03-30T15:05:40+08:00\"}],\"relationshipManagers\":[{\"id\":1463044,\"ownerId\":831384,\"officerIncharge\":\"Bj2uzCjkIRf7QI8F0P3\",\"order\":1,\"version\":1,\"createdBy\":\"FIVUSR\",\"createdByChannel\":\"MIGRATION\",\"createdTimeStamp\":\"2021-03-20T13:11:58+08:00\",\"updatedBy\":\"SEJALAGRAWAL\",\"updatedByChannel\":\"MIGRATION\",\"updatedTimeStamp\":\"2022-02-10T19:56:58+08:00\"}]},\"status\":{\"statusCode\":0},\"traceInfo\":{\"traceId\":\"83bb2940-1f37-11eb-adc1-0242ac120009\",\"spanId\":\"d69a1b3d67d757df\",\"timestamp\":\"2024-03-15T15:26:59.906126+08:00\"}}";
        }

        private string getAccountDetailsDBSResponseTest()
        {
            string sResposne = "";

            sResposne =
            "{ " +
                "\"sourceAccountNumber\": \"857010068475\"," +
                "\"ibanAccountNumber\": \"\"," +
                "\"sourceSystemId\": \"\"," +
                "\"sourceCustomerId\": \"24298597\"," +
                "\"productType\": \"SBA\"," +
                "\"productTypeDescription\": []," +
                "\"subProductType\": \"\"," +
                "\"productCode\": \"NROT3\"," +
                "\"productCodeDescription\": [ { \"productCodeDescription\": \"DIGISAVINGS\"," +
                    "                              \"languageCode\": \"INFENG\"         } ]," +
                //"\"productName\": \"NA\"," +
                //"\"productName\": \"ODACC\"," +
                "\"productName\": \"TREPB\"," +
                "\"nativeProductName\": \"DBSBA\"," +
                "\"accountCurrency\": \"INR\"," +
                "\"openedDate\": 1656374400000," +
                "\"modeOfOperation\": \"0003\"," +
                "\"serviceChargeExemption\": 0," +
                "\"staffIndicator\": false," +
                "\"officerId\": \"\"," +
                "\"officerUnit\": \"\"," +
                "\"firstExcessDate\": 0," +
                "\"lastUpdatedEvent\": \"\"," +
                "\"returnChequeDetailsInfo\": []," +
                "\"accountStatus\": \"Active\"," +
                "\"accountStatusCode\": \"A\"," +
                "\"accountSignatoryType\": \"01\"," +
                "\"accountSignal\": \"\"," +
                "\"loanServicingIndicator\": false," +
                "\"accountFrozenIndicator\": true," +
                "\"noDebitIndicator\": false," +
                "\"debitReferralIndicator\": false," +
                "\"irregularSignalIndicator\": false," +
                "\"lineOfferedIndicator\": false," +
                "\"closureNoticeIndicator\": false," +
                "\"multipleAccountIndicator\": false," +
                "\"recallPassbookIndicator\": false," +
                "\"updateRequiredIndicator\": false," +
                "\"productIndicator\": \"\"," +
                "\"brandIndicator\": \"\"," +
                "\"ibanIndicator\": \"N\"," +
                "\"virtualAccountIndicator\": \"N\"," +
                "\"spclCustomerType\": \"\"," +
                "\"accountType\": \"\"," +
                "\"accountTypeDescription\": \"\"," +
                "\"currencyDecimal\": 2," +
                "\"generalLedgerSubHeadCode\": \"21201\"," +
                "\"accountCurrencyCode\": \"INR\"," +
                "\"odLimitType\": \"\"," +
                "\"odInterestAmount\": 0," +
                "\"accountName\": \"XXX XXXXX XXXXXXX XXXXXXX\"," +
                "\"accountShortName\": \"XXX XXXXX\"," +
                "\"virtualAccountName\": \"\"," +
                "\"accountStatement\": { \"statementMode\": \"S\"," +
                                          "  \"statementCalendar\": \"00\"," +
                                          "  \"frequency\": \"M\"," +
                                          "  \"frequencyStartDate\": 31," +
                                          "  \"frequencyDay\": 0," +
                                          "  \"frequencyWeekNumber\": 0," +
                                          "  \"frequencyHolidayStatus\": \"N\"," +
                                          "  \"nextPrintDate\": 1546214400000," +
                                          "  \"despatchMode\": \"N\" " +
                                          "  }" +
                                          " ," +
                "\"balanceDebitCreditIndicator\": \"C\"," +
                "\"freezeCode\": \"\"," +
                "\"freezeStatusCode\": \"\"," +
                "\"freezeReasonCode\": \"\"," +
                "\"freezeReasonCode1\": \"\"," +
                "\"additionalFreezeReasonCodes\": []," +
                "\"additionalFreezeRemarks\": []," +
                "\"freezeReasonCodeDescriptionList\": []," +
                "\"freezeRemarks\": \"\"," +
                "\"freezeRemarks1\": \"\"," +
                "\"accountInterest\": { \"interestRate\": 3.25," +
                    "                     \"interestCalInterest\": 0," +
                    "                     \"interestFrequencyType\": \"M\"," +
                    "                     \"interestFrequencyStartDate\": 31," +
                    "                     \"interestFrequencyDay\": 0," +
                    "                     \"interestFrequencyWeekNum\": 0," +
                    "                     \"accountInterestFrequencyHolidayStatus\": \"P\"," +
                    "                     \"interestRateCode\": \"DBSB1\"," +
                    "                     \"netInterestRate\": 0," +
                    "                     \"netInterestDebitCreditIndicator\": \"C\"," +
                    "                     \"accruedInterestDebitCreditIndicator\": \"C\"," +
                    "                     \"unpaidInterestFee\": 0," +
                    "                     \"bookedamount\": 0," +
                    "                     \"interestAmount\": 0," +
                    "                     \"preferentialInterest\": 0" +
                    "                   }" +
                    "," +
                "\"taxCategory\": \"A\"," +
                "\"taxFloorLimit\": 0," +
                "\"taxFloorLimitCurrencyCode\": \"INR\"," +
                "\"withholdingTaxPercent\": 0," +
                "\"gstin\": \"\"," +
                "\"gstExemptionFlag\": \"\"," +
                "\"nickName\": \"\"," +
                "\"productShortName\": \"DBSBA\"," +
                "\"preferredLanguageProductShortName\": \"DBSBA\"," +
                "\"sourceMultiCurrencyAccountNumber\": \"\"," +
                "\"multiCurrencyAccountFlag\": false," +
                "\"branchCode\": \"811\"," +
                "\"branchCodeDescription\": \"\"," +
                "\"bankCode\": \"DBSIN\"," +
                //"\"accountClosedFlag\": \"N\"," +
                "\"accountClosedFlag\": \"N\"," +
                "\"accountClosedReasonCode\": \"\"," +
                "\"accountClosedRemarks\": \"\"," +
                "\"accountClosedDate\": 0," +
                "\"lastBalanceUpdateDateTime\": 0," +
                "\"earmarkUpdateDateTime\": 0," +
                "\"holdBalanceUpdateDateTime\": 0," +
                "\"sanctionLimitUpdateDateTime\": 0," +
                "\"staticDataUpdateDateTime\": 1656516121000," +
                "\"halfDayHoldBalanceExpiryDate\": 0," +
                "\"childAccounts\": []," +
                "\"accountBalances\": 	{" +
                        "				   \"availableBalance\": -374," +
                        "				   \"availableBalanceCurrencyCode\": \"INR\"," +
                        "				   \"accountBalance\": 126," +
                        "				   \"accountBalanceCurrencyCode\": \"INR\"," +
                        "				   \"sanctionLimit\": 0," +
                        "				   \"sanctionLimitCurrencyCode\": \"INR\"," +
                        "				   \"ledgerBalance\": 126," +
                        "				   \"ledgerBalanceCurrencyCode\": \"INR\"," +
                        "				   \"halfDayHoldBalance\": 0," +
                        "				   \"oneDayHoldBalance\": 0," +
                        "				   \"twoDayHoldBalance\": 0," +
                        "				   \"earmarkDebitAmount\": 0," +
                        "				   \"earmarkCreditAmount\": 500," +
                        "				   \"floatAmount\": 0," +
                        "				   \"earmarkAmount\": 500," +
                        "				   \"effectiveAvailableAmount\": 0," +
                        "				   \"drawingPower\": 0," +
                        "				   \"overDueLiableAmount\": 0," +
                        "				   \"openingBalanceAmount\": 0," +
                        "				   \"closingBalanceAmount\": 0," +
                        "				   \"fundsClearingAmount\": 0," +
                        "				   \"cumulativeCreditAmount\": 35170," +
                        "				   \"cumulativeDebitAmount\": 35044," +
                        "				   \"utilizedAmount\": 0," +
                        "				   \"systemReservedAmount\": 0," +
                        "				   \"overdueFutureAmount\": 0," +
                        "				   \"utilizedFutureAmount\": 0," +
                        "				   \"effectiveFutureAvailableAmount\": 0," +
                        "				   \"availableAmountLineOfCredit\": 0," +
                        "				   \"unclearDrawingAmount\": 0," +
                        "				   \"ffdAvailableAmount\": 0," +
                        "				   \"sweepsEffectiveAvailableAmount\": 0," +
                        "				   \"hcAvailableAmount\": 0," +
                        "				   \"futureAmount\": 0," +
                        "				   \"futureCreditAmount\": 0," +
                        "				   \"futureClearBalanceAmount\": 0," +
                        "				   \"futureUnclearBalanceAmount\": 0," +
                        "				   \"daccLimit\": 0," +
                        "				   \"dafaLimit\": 0 " +
                        "					}," +
                        "\"relatedCustomerInfo\": [" +
                        "                            { " +
                        "                            \"relatedPartyCode\": \"\"," +
                        "                            \"relatedPartyCodeDescription\": \"\"," +
                        "                            \"relatedPartyCustomerId\": \"24298597\"," +
                        "                            \"relatedPartySourceCustomerId\": \"\"," +
                        "                            \"relatedPartyType\": \"M\"," +
                        "                            \"relatedPartyTypeDescription\": \"Main Holder\"," +
                        "                            \"relatedPartyDeleteFlag\": \"N\"," +
                        "                            \"relatedPartyAddressType\": \"Mailing\"" +
                        "                            }" +
                        //"                            { " +
                        //"                            \"relatedPartyCode\": \"\"," +
                        //"                            \"relatedPartyCodeDescription\": \"\"," +
                        //"                            \"relatedPartyCustomerId\": \"22945769\"," +
                        //"                            \"relatedPartySourceCustomerId\": \"\"," +
                        //"                            \"relatedPartyType\": \"M\"," +
                        //"                            \"relatedPartyTypeDescription\": \"2nd Holder\"," +
                        //"                            \"relatedPartyDeleteFlag\": \"N\"," +
                        //"                            \"relatedPartyAddressType\": \"Mailing\"" +
                        //"                            }" +
                        "                      ]," +
                        "\"promoCode\": []," +
                        "\"mobileMoneyIdentifier\": \"9641689\"," +
                        "\"mobileNumbers\": [ \"7358386665\" ]," +
                        "\"reference1\": \"\"," +
                        "\"reference2\": \"\"," +
                        "\"ifscCode\": \"DBSS0IN0811\"," +
                        "\"channelId\": \"SOI\"," +
                        "\"accountStatusDate\": 1623456000000," +
                        "\"expressAccountExpiryDate\": 0," +
                        "\"schemeConversionDate\": 0," +
                        "\"virtualAccountType\": \"\"," +
                        "\"faxIndeminity\": \"\"," +
                        "\"nomineeAvailableFlag\": \"N\"," +
                        "\"nomineeGuardianInfo\": []" +
                "}   ";


            string jsonResponse = @"
                                    {
                                      ""error"": {
                                        ""timestamp"": 1733730385113,
                                        ""status"": ""INTERNAL_SERVER_ERROR"",
                                        ""errorType"": ""BE"",
                                        ""errorCode"": ""MSTD-CD-5008"",
                                        ""errorMessage"": ""Finacle Business Error"",
                                        ""methodName"": """",
                                        ""restError"": [
                                          {
                                            ""errorCode"": ""W025"",
                                            ""errorMessage"": ""Invalid Account Details ""
                                          }
                                        ]
                                      },
                                      ""timestamp"": 1733730385113,
                                      ""status"": ""INTERNAL_SERVER_ERROR"",
                                      ""errorType"": ""BE"",
                                      ""errorCode"": ""MSTD-CD-5008"",
                                      ""errorDescription"": ""Finacle Business Error"",
                                      ""methodName"": """",
                                      ""restError"": [
                                        {
                                          ""errorCode"": ""W025"",
                                          ""errorDescription"": ""Invalid Account Details ""
                                        }
                                      ]
                                    }";


            //return jsonResponse;
           return sResposne;
        }

        string GetCMCPCustomerNameTest()
        {
            string tst = "\"{\\r\\n    \\\"data\\\": {\\r\\n        \\\"cmcpId\\\": \\\"IN1I2200Z52FL\\\",\\r\\n        \\\"cin\\\": \\\"23997348\\\",\\r\\n        \\\"cinSuffix\\\": \\\"00\\\",\\r\\n        \\\"customerType\\\": 1,\\r\\n        \\\"profileInfo\\\": {\\r\\n            \\\"dbsIndustryCode\\\": {\\r\\n                \\\"codeValueId\\\": 583,\\r\\n                \\\"codeValueCd\\\": \\\"99950\\\",\\r\\n                \\\"referenceCodeValueCd\\\": \\\"99950\\\",\\r\\n                \\\"codeValueDisplay\\\": \\\"PRIVATE INDIVIDUALS\\\"\\r\\n            },\\r\\n            \\\"countryOfResidenceCode\\\": {\\r\\n                \\\"codeValueId\\\": 1959,\\r\\n                \\\"codeValueCd\\\": \\\"IN\\\",\\r\\n                \\\"referenceCodeValueCd\\\": \\\"IN\\\",\\r\\n                \\\"codeValueDisplay\\\": \\\"INDIA\\\"\\r\\n            },\\r\\n            \\\"registeredName\\\": \\\"5MvnqWFkVzt9bnJjndaGIndNkzombY0RyqBlI4csfPsnmlWzE9H9c7MEPpPLWm97ENV4vLjcWE0SMAZSUgOfqBfsG0moX0BMrIX\\\",\\r\\n            \\\"nameLine1\\\": \\\"j873YaKrblaMd1Pjgx51UyTs1TUlEdvCfuXidNhdUT3yUOxgRg3EOMK3FxaXiN4CsCqe7ztcrotZjp7v\\\",\\r\\n            \\\"nameLine2\\\": \\\"8YdAHBeNIK5VIJGk0j5ke5A9h2N7TGVaiubsL9CDXeUvCJB8b5cMCxGaYqYZLra4V8y3z88XzpoDYcFh\\\",\\r\\n            \\\"customerSubTypeCode\\\": {\\r\\n                \\\"codeValueId\\\": 694,\\r\\n                \\\"codeValueCd\\\": \\\"R\\\",\\r\\n                \\\"referenceCodeValueCd\\\": \\\"R\\\",\\r\\n                \\\"codeValueDisplay\\\": \\\"RETAIL\\\"\\r\\n            },\\r\\n            \\\"relationshipStartDate\\\": \\\"2021-03-20\\\",\\r\\n            \\\"nameInNativeLanguage\\\": \\\"P5ljQEZ7U9KQYHkkzWZCXZ3jc2TKldNyjGD5VJg5fT25IyPH236u22Su41VR8utPgoMrfQXanJxzUXPB\\\",\\r\\n            \\\"preferredLanguageCode\\\": {\\r\\n                \\\"codeValueId\\\": 12014,\\r\\n                \\\"codeValueCd\\\": \\\"001\\\",\\r\\n                \\\"referenceCodeValueCd\\\": \\\"001\\\",\\r\\n                \\\"codeValueDisplay\\\": \\\"ENGLISH\\\"\\r\\n            },\\r\\n            \\\"remarks\\\": \\\"\\\",\\r\\n            \\\"taxDeductionAtSourceTableCode\\\": {\\r\\n                \\\"codeValueId\\\": 57956,\\r\\n                \\\"codeValueCd\\\": \\\"TDS01\\\",\\r\\n                \\\"referenceCodeValueCd\\\": \\\"TDS01\\\",\\r\\n                \\\"codeValueDisplay\\\": \\\"TDS01\\\"\\r\\n            },\\r\\n            \\\"sectorCode\\\": {\\r\\n                \\\"codeValueId\\\": 57264,\\r\\n                \\\"codeValueCd\\\": \\\"31005\\\",\\r\\n                \\\"referenceCodeValueCd\\\": \\\"31005\\\",\\r\\n                \\\"codeValueDisplay\\\": \\\"BANKING/FINANCE\\\"\\r\\n            },\\r\\n            \\\"subSectorCode\\\": {\\r\\n                \\\"codeValueId\\\": 57698,\\r\\n                \\\"codeValueCd\\\": \\\"30310\\\",\\r\\n                \\\"referenceCodeValueCd\\\": \\\"30310\\\",\\r\\n                \\\"codeValueDisplay\\\": \\\"BANKS-REPRESENTATIVEOFFICES\\\"\\r\\n            },\\r\\n            \\\"forexTierGroupCode\\\": {\\r\\n                \\\"codeValueId\\\": 57191,\\r\\n                \\\"codeValueCd\\\": \\\"10\\\",\\r\\n                \\\"referenceCodeValueCd\\\": \\\"10\\\",\\r\\n                \\\"codeValueDisplay\\\": \\\"FX Group 10\\\"\\r\\n            },\\r\\n            \\\"regionCode\\\": {\\r\\n                \\\"codeValueId\\\": 57245,\\r\\n                \\\"codeValueCd\\\": \\\"02\\\",\\r\\n                \\\"referenceCodeValueCd\\\": \\\"02\\\",\\r\\n                \\\"codeValueDisplay\\\": \\\"West\\\"\\r\\n            },\\r\\n            \\\"taxStatus\\\": {\\r\\n                \\\"codeValueId\\\": 57971,\\r\\n                \\\"codeValueCd\\\": \\\"800001\\\",\\r\\n                \\\"referenceCodeValueCd\\\": \\\"800001\\\",\\r\\n                \\\"codeValueDisplay\\\": \\\"Resident  Tax\\\"\\r\\n            },\\r\\n            \\\"constitutionCode\\\": {\\r\\n                \\\"codeValueId\\\": 1856,\\r\\n                \\\"codeValueCd\\\": \\\"15\\\",\\r\\n                \\\"referenceCodeValueCd\\\": \\\"15\\\",\\r\\n                \\\"codeValueDisplay\\\": \\\"INDIVIDUAL\\\"\\r\\n            },\\r\\n            \\\"customerNetWorth\\\": \\\"4999999\\\",\\r\\n            \\\"profitCenterCode\\\": {\\r\\n                \\\"codeValueId\\\": 57240,\\r\\n                \\\"codeValueCd\\\": \\\"06\\\",\\r\\n                \\\"referenceCodeValueCd\\\": \\\"06\\\",\\r\\n                \\\"codeValueDisplay\\\": \\\"Retail\\\"\\r\\n            },\\r\\n            \\\"previousName\\\": \\\"TYiCedQ9mhPPVL9euoniv0F3SDg5Ae1kdDAGZUgahODYgK2fyOpcSarD1cBQmDhbaM1nKzELAU86znYkbrHVJKL0FRB8fD6DzMOFz2N4Q0OL1VTUb0hLPPfieNIQYeOo39iMRdLDG9x49EhJsrH3PbgYMdipSgOi\\\",\\r\\n            \\\"primaryBranchCode\\\": {\\r\\n                \\\"codeValueId\\\": 58744,\\r\\n                \\\"codeValueCd\\\": \\\"854\\\",\\r\\n                \\\"referenceCodeValueCd\\\": \\\"854\\\",\\r\\n                \\\"codeValueDisplay\\\": \\\"VIKHROLI\\\"\\r\\n            },\\r\\n            \\\"alias\\\": \\\"IVMZ5NbKfJlipTnpCBfVqW1Ms132PUOmqqAWBYhhDTMOTiBKsOFRPdX2Pf0f\\\",\\r\\n            \\\"salutationCode\\\": {\\r\\n                \\\"codeValueId\\\": 16,\\r\\n                \\\"codeValueCd\\\": \\\"MR\\\",\\r\\n                \\\"referenceCodeValueCd\\\": \\\"MR\\\",\\r\\n                \\\"codeValueDisplay\\\": \\\"MR\\\"\\r\\n            },\\r\\n            \\\"sexCode\\\": {\\r\\n                \\\"codeValueId\\\": 25,\\r\\n                \\\"codeValueCd\\\": \\\"M\\\",\\r\\n                \\\"referenceCodeValueCd\\\": \\\"M\\\",\\r\\n                \\\"codeValueDisplay\\\": \\\"MALE\\\"\\r\\n            },\\r\\n            \\\"maritalStatusCode\\\": {\\r\\n                \\\"codeValueId\\\": 28,\\r\\n                \\\"codeValueCd\\\": \\\"2\\\",\\r\\n                \\\"referenceCodeValueCd\\\": \\\"2\\\",\\r\\n                \\\"codeValueDisplay\\\": \\\"MARRIED\\\"\\r\\n            },\\r\\n            \\\"nativeLanguageCode\\\": {\\r\\n                \\\"codeValueId\\\": 13662,\\r\\n                \\\"codeValueCd\\\": \\\"INFENG\\\",\\r\\n                \\\"referenceCodeValueCd\\\": \\\"INFENG\\\",\\r\\n                \\\"codeValueDisplay\\\": \\\"ENGLISH\\\"\\r\\n            },\\r\\n            \\\"countryOfBirthCode\\\": {\\r\\n                \\\"codeValueId\\\": 1959,\\r\\n                \\\"codeValueCd\\\": \\\"IN\\\",\\r\\n                \\\"referenceCodeValueCd\\\": \\\"IN\\\",\\r\\n                \\\"codeValueDisplay\\\": \\\"INDIA\\\"\\r\\n            },\\r\\n            \\\"maidenName\\\": \\\"xIFPJmGkrc2q3gbaRjSo94EBUYx8duqkSekcuMPQ3e9ojuhMCJ0f3W9CUxXK3Au8zk6BZeUS3QfCFMnqkCk5IdvXT8Vjr5phesN\\\",\\r\\n            \\\"dateOfBirth\\\": \\\"1980-11-15\\\",\\r\\n            \\\"motherMaidenName\\\": \\\"Vhc2GyrQqbb5mlhQLRGB2i2987EPsdNqBvj6C5UsfGfEhW81bYWQmSe1r4U4l3W48f64q5uUabLIcDPc5LZ3EPMu6eL1KfIv9r3\\\",\\r\\n            \\\"fatherName\\\": \\\"oLai5QCdDDK1m7tYqs05T3GXp6j09Q6N9c8TSmcHjJZTdfHKMxo4am822mWIHn7zPHI1gVv3NRNEJ2c7lVX1NWBTRExiVaLTBKe\\\",\\r\\n            \\\"spouseName\\\": \\\"PAOFN4MCilVWqsXvbeWmpVh3Jf3VK5GMxe6mmKtDhsm1MH8TWOgAgp8RRujlSfPhmj8J8ujCZbkBY6vQ0MO1N9gA8MHgOqi5buC\\\",\\r\\n            \\\"staffFlag\\\": true,\\r\\n            \\\"staffId\\\": \\\"ViniaHeng\\\",\\r\\n            \\\"industryType2Code\\\": {\\r\\n                \\\"codeValueId\\\": 56936,\\r\\n                \\\"codeValueCd\\\": \\\"410\\\",\\r\\n                \\\"referenceCodeValueCd\\\": \\\"410\\\",\\r\\n                \\\"codeValueDisplay\\\": \\\"4.1  Individuals (including HUF)\\\"\\r\\n            },\\r\\n            \\\"preferredName\\\": \\\"zOA05eVKpFvRNDVadOLz5rAhya1folMFFYuDbC8abf3hheNVFW5JzMEuVsuRAanT6jerDxORB7hSCTBkQ0e3UgE7zOeTN96XJYo\\\",\\r\\n            \\\"version\\\": 1,\\r\\n            \\\"createdBy\\\": \\\"FIVUSR\\\",\\r\\n            \\\"createdByChannel\\\": \\\"MIGRATION\\\",\\r\\n            \\\"createdTimeStamp\\\": \\\"2021-03-20T13:11:58+08:00\\\",\\r\\n            \\\"updatedBy\\\": \\\"SEJALAGRAWAL\\\",\\r\\n            \\\"updatedByChannel\\\": \\\"MIGRATION\\\",\\r\\n            \\\"updatedTimeStamp\\\": \\\"2022-02-10T19:56:58+08:00\\\"\\r\\n        },\\r\\n        \\\"owners\\\": [\\r\\n            {\\r\\n                \\\"id\\\": 831384,\\r\\n                \\\"ownerCode\\\": {\\r\\n                    \\\"codeValueId\\\": 1835,\\r\\n                    \\\"codeValueCd\\\": \\\"0001\\\",\\r\\n                    \\\"referenceCodeValueCd\\\": \\\"0001\\\",\\r\\n                    \\\"codeValueDisplay\\\": \\\"Mass Market\\\"\\r\\n                },\\r\\n                \\\"ownerOrder\\\": 1,\\r\\n                \\\"customerOwnerTPCIndicator\\\": \\\"04\\\",\\r\\n                \\\"version\\\": 1,\\r\\n                \\\"createdBy\\\": \\\"FIVUSR\\\",\\r\\n                \\\"createdByChannel\\\": \\\"MIGRATION\\\",\\r\\n                \\\"createdByDepartment\\\": \\\"MIGRATION\\\",\\r\\n                \\\"createdTimeStamp\\\": \\\"2021-03-20T13:11:58+08:00\\\",\\r\\n                \\\"updatedBy\\\": \\\"SEJALAGRAWAL\\\",\\r\\n                \\\"updatedByChannel\\\": \\\"MIGRATION\\\",\\r\\n                \\\"updatedByDepartment\\\": \\\"MIGRATION\\\",\\r\\n                \\\"updatedTimeStamp\\\": \\\"2022-02-10T19:56:58+08:00\\\"\\r\\n            }\\r\\n        ],\\r\\n        \\\"attributes\\\": [\\r\\n            {\\r\\n                \\\"id\\\": 6072502,\\r\\n                \\\"customerAttributeCode\\\": {\\r\\n                    \\\"codeValueId\\\": 2466,\\r\\n                    \\\"codeValueCd\\\": \\\"01\\\",\\r\\n                    \\\"referenceCodeValueCd\\\": \\\"01\\\",\\r\\n                    \\\"codeValueDisplay\\\": \\\"ACTIVE\\\",\\r\\n                    \\\"attributeValue\\\": \\\"3\\\"\\r\\n                },\\r\\n                \\\"statusSource\\\": \\\"bmxhUy3nBg86VLGkd46XuprF4JAWgX0jVyPbqTs4lTE9y8kzYJAZyUSDVb5x9XoT5k9Gq8NlNJJs4DXYMOpFs60ZIxg3Z9nHUUbVg6p6j0cYyYINojkK7H9LCX1L8pdntdECAEfhGd9PPFPSOQxuDHRB7n3hIYtehhDCS4VdyeZfiDjXRyXS1S3RXUIftNBbchNmU3WiCy6hAdM05XCEyJpL7nrBerIJ9mQ0VxjiHgcOvsJAbN63PO4atNg3kyi310b79yfF0tIIl2RY8ZOCVmqOsI2O8yFrkNNmMTyHf8O3\\\",\\r\\n                \\\"version\\\": 1,\\r\\n                \\\"createdBy\\\": \\\"FIVUSR\\\",\\r\\n                \\\"createdByChannel\\\": \\\"MIGRATION\\\",\\r\\n                \\\"createdTimeStamp\\\": \\\"2021-03-20T13:11:58+08:00\\\",\\r\\n                \\\"updatedBy\\\": \\\"SEJALAGRAWAL\\\",\\r\\n                \\\"updatedByChannel\\\": \\\"MIGRATION\\\",\\r\\n                \\\"updatedTimeStamp\\\": \\\"2022-02-10T19:56:58+08:00\\\"\\r\\n            }\\r\\n        ],\\r\\n        \\\"segments\\\": [\\r\\n            {\\r\\n                \\\"id\\\": 2130415,\\r\\n                \\\"segmentCode\\\": {\\r\\n                    \\\"codeValueId\\\": 57990,\\r\\n                    \\\"codeValueCd\\\": \\\"IN000015\\\",\\r\\n                    \\\"referenceCodeValueCd\\\": \\\"IN000015\\\",\\r\\n                    \\\"codeValueDisplay\\\": \\\"MASS MKT MUMBAI\\\"\\r\\n                },\\r\\n                \\\"segmentTypeCode\\\": {\\r\\n                    \\\"codeValueId\\\": 2530,\\r\\n                    \\\"codeValueCd\\\": \\\"01\\\",\\r\\n                    \\\"referenceCodeValueCd\\\": \\\"01\\\",\\r\\n                    \\\"codeValueDisplay\\\": \\\"SEGMENT LEVEL 1\\\"\\r\\n                },\\r\\n                \\\"segmentSubTypeCode\\\": {\\r\\n                    \\\"codeValueId\\\": 2533,\\r\\n                    \\\"codeValueCd\\\": \\\"1\\\",\\r\\n                    \\\"referenceCodeValueCd\\\": \\\"0001\\\",\\r\\n                    \\\"codeValueDisplay\\\": \\\"PRIMARY\\\"\\r\\n                },\\r\\n                \\\"tpcIndicator\\\": \\\"04\\\",\\r\\n                \\\"version\\\": 1,\\r\\n                \\\"createdBy\\\": \\\"FIVUSR\\\",\\r\\n                \\\"createdByChannel\\\": \\\"MIGRATION\\\",\\r\\n                \\\"createdTimeStamp\\\": \\\"2021-03-20T13:11:58+08:00\\\",\\r\\n                \\\"updatedBy\\\": \\\"SEJALAGRAWAL\\\",\\r\\n                \\\"updatedByChannel\\\": \\\"MIGRATION\\\",\\r\\n                \\\"updatedTimeStamp\\\": \\\"2022-02-10T19:56:58+08:00\\\"\\r\\n            }\\r\\n        ],\\r\\n        \\\"addresses\\\": [\\r\\n            {\\r\\n                \\\"id\\\": 1097353,\\r\\n                \\\"startDate\\\": \\\"2021-03-20\\\",\\r\\n                \\\"endDate\\\": \\\"2099-12-31\\\",\\r\\n                \\\"addressLabel\\\": \\\"E153hYLHclTIoYmFgjNeQopHZMBNJgPgxlLynlgr2UuWeOOvC7ivR3HpVymbdAX4nCcKddzlCmYLWpeXZe3HC7MArXIUQKqGZus\\\",\\r\\n                \\\"addressTypeCode\\\": {\\r\\n                    \\\"codeValueId\\\": 11003,\\r\\n                    \\\"codeValueCd\\\": \\\"Mailing\\\",\\r\\n                    \\\"referenceCodeValueCd\\\": \\\"Mailing\\\",\\r\\n                    \\\"codeValueDisplay\\\": \\\"Mailing\\\"\\r\\n                },\\r\\n                \\\"levelNumber\\\": \\\"WbV\\\",\\r\\n                \\\"unitNumber\\\": \\\"6uWHOMg\\\",\\r\\n                \\\"blockNumber\\\": \\\"XeO7u\\\",\\r\\n                \\\"streetName1\\\": \\\"KVRhalVhSI00kNx1WylD6NQKKgFYyQCX3hGWoEdr59lm2XGbQJrIELPl74E7prYeIGONP2rcqti5iFrXvIDK7Pu5DxFYQ8Ll1nv\\\",\\r\\n                \\\"streetName2\\\": \\\"BU2IRRjtneN8zUZuyR4mVmvqokCOsC2HNEtNm5NJqUQ90B0FT02bqPrXEMM0ZrVUvD5eZmDnccVofGcYnQDzC0yoBqrRkcvn3mr\\\",\\r\\n                \\\"streetName3\\\": \\\"SSz2LTKpRdLrF5TnKDqk9SFE0biXqN8B4WIdZjLtEuGs49ye17E9IxQ4Zbuz1ZqtyxvX34mkQZyRD1TPmfkQ84LrdGobfoWcVhz\\\",\\r\\n                \\\"streetName5\\\": \\\"ls7MJyXH8dMefc8rhyaR80erKa3YeDFkZXti43dEdXLSJKsMLy5ulM5o60PvizY51U57fXTTlyS4TXY0M9WgsQIzb7HkP5z895l\\\",\\r\\n                \\\"streetName6\\\": \\\"FRlX3HRBHJCHpZPYl7hnmvt3rbNl6Vipe58GqGdDU2hJtr63tTRQv386gHgH78NP8IiSYeSbeNN9sUSy8sadeLEYPHMkpNq72MW\\\",\\r\\n                \\\"postalCode\\\": \\\"902276\\\",\\r\\n                \\\"cityCode\\\": {\\r\\n                    \\\"codeValueId\\\": 51305,\\r\\n                    \\\"codeValueCd\\\": \\\"30805\\\",\\r\\n                    \\\"referenceCodeValueCd\\\": \\\"30805\\\",\\r\\n                    \\\"codeValueDisplay\\\": \\\"KALYAN\\\"\\r\\n                },\\r\\n                \\\"stateCode\\\": {\\r\\n                    \\\"codeValueId\\\": 57353,\\r\\n                    \\\"codeValueCd\\\": \\\"MH\\\",\\r\\n                    \\\"referenceCodeValueCd\\\": \\\"MH\\\",\\r\\n                    \\\"codeValueDisplay\\\": \\\"MAHARASHTRA\\\"\\r\\n                },\\r\\n                \\\"countryCode\\\": {\\r\\n                    \\\"codeValueId\\\": 1959,\\r\\n                    \\\"codeValueCd\\\": \\\"IN\\\",\\r\\n                    \\\"referenceCodeValueCd\\\": \\\"IN\\\",\\r\\n                    \\\"codeValueDisplay\\\": \\\"INDIA\\\"\\r\\n                },\\r\\n                \\\"nativeStreetName1\\\": \\\"I5aL7aCmrjcU1Jn9UM1gFnvYnHBnQOW160aGC4NNO5G11hRpvaU4vxLAYkETEaSQqq2ozttc8GG71vP6fBydoK2A1vyelZ58jrB\\\",\\r\\n                \\\"nativeStreetName2\\\": \\\"dGACNZPlJzFpurukqb0mA4Jr7694Bg6yHl812JuvI2V0OMgz0QLLLD13hpajtIltRsl5ZjMYomm5Zh7ic9TUysfFOl9rsFTVnZo\\\",\\r\\n                \\\"nativeStreetName3\\\": \\\"oiFXxdqVZW8WqX0dEnrtF9FHFnLyH4y6Pg1xU68t4pc0O8HCscd8oJIB8HdGMazN481KII1NNbtMZA3LpMX7E6tVu3DKlCWcEzm\\\",\\r\\n                \\\"nativeStreetName4\\\": \\\"RuBCREzmbjqk0PLViUe1Nu4FdxXf6QKty1XBHzxvJk7i1oC3yBeZO0NxtGxYzdoCrWUKlF2hRujFmZVLaQLzcHx9Z0LdQrC1XLY\\\",\\r\\n                \\\"preferredFlag\\\": true,\\r\\n                \\\"holdMailFlag\\\": false,\\r\\n                \\\"holdMailReason\\\": \\\"VhOYu8Mj6bmIhEzvPqKXRu8ntxoI7Wf8EF2iz810dpxR0qN6hN1F59ql9rOCdpjZ6l9yPu1IqQLq5smXUXVlgd50qUL1RArMTOu\\\",\\r\\n                \\\"formatAddress1\\\": \\\"BLK XeO7u KVRhalVhSI00kNx1WylD6NQKKgFYyQ\\\",\\r\\n                \\\"formatAddress2\\\": \\\"BU2IRRjtneN8zUZuyR4mVmvqokCOsC2HNEtNm5NJqUQ90B0FT02bqPrXEMM0ZrVUvD5eZmDnccVofGcYnQDzC0yoBqrRkcvn3mr\\\",\\r\\n                \\\"formatAddress3\\\": \\\"SSz2LTKpRdLrF5TnKDqk9SFE0biXqN8B4WIdZjLtEuGs49ye17E9IxQ4Zbuz1ZqtyxvX34mkQZyRD1TPmfkQ84LrdGobfoWcVhz\\\",\\r\\n                \\\"formatAddress4\\\": \\\"#WbV-6uWHOMg SINGAPORE 902276\\\",\\r\\n                \\\"version\\\": 1,\\r\\n                \\\"createdBy\\\": \\\"MIGRATION\\\",\\r\\n                \\\"createdByChannel\\\": \\\"MIGRATION\\\",\\r\\n                \\\"createdByDepartment\\\": \\\"\\\",\\r\\n                \\\"createdTimeStamp\\\": \\\"2022-09-17T00:00:00+08:00\\\",\\r\\n                \\\"updatedBy\\\": \\\"MIGRATION\\\",\\r\\n                \\\"updatedByChannel\\\": \\\"MIGRATION\\\",\\r\\n                \\\"updatedByDepartment\\\": \\\"\\\",\\r\\n                \\\"updatedTimeStamp\\\": \\\"2021-03-30T15:05:39+08:00\\\"\\r\\n            },\\r\\n            {\\r\\n                \\\"id\\\": 1097355,\\r\\n                \\\"startDate\\\": \\\"2021-03-20\\\",\\r\\n                \\\"endDate\\\": \\\"2099-12-31\\\",\\r\\n                \\\"addressLabel\\\": \\\"Ak5jiECxf10UVAk6CL6aI7LhJjB0dZuRyZeolmpSoxuFAiQjEUobVMb2PxGbeycltO4cneGfKSkgnb8PTkrssSi7GS3dHfLlNF4\\\",\\r\\n                \\\"addressTypeCode\\\": {\\r\\n                    \\\"codeValueId\\\": 22768,\\r\\n                    \\\"codeValueCd\\\": \\\"Home\\\",\\r\\n                    \\\"referenceCodeValueCd\\\": \\\"Home\\\",\\r\\n                    \\\"codeValueDisplay\\\": \\\"Home\\\"\\r\\n                },\\r\\n                \\\"levelNumber\\\": \\\"PQg\\\",\\r\\n                \\\"unitNumber\\\": \\\"IWZq3I0\\\",\\r\\n                \\\"blockNumber\\\": \\\"T9xyI\\\",\\r\\n                \\\"streetName1\\\": \\\"1jRYzB6e9n3RSgTC7IyyvmqMYD6ICxFDQ1laMM5BBT1VlK1M7C8uY3hbK7bu84ElgXJzWk673BzDnH9njhfLZvGFROygj0Iq0G6\\\",\\r\\n                \\\"streetName2\\\": \\\"Fba0XQXB9UuOpWIsraBlJCWT9KM1yLCKMgKelrmKPlGVuepE79JV6g5vTQfuig5QjEgYeLthmR2gIcqe25r03DlsWzhcQc6xD3V\\\",\\r\\n                \\\"streetName3\\\": \\\"YiQqp6ton2f0BamZgZ9toJcLNmxqLnEURPIiHNvnX8aN8rUxcnK1vFMrlzj1vQakegvaopqIOfrM6thtP2XShtfNKSEOxzgtkmk\\\",\\r\\n                \\\"streetName4\\\": \\\"uAov9Rd5hUY2W47A88VmmDD2psEjsv0hUJsgBJjpPKUk8pqXZDD6iLjtnjXQmksOOr9mrxBK5TyOkjhlamo315yCspcJsRJ9cx2\\\",\\r\\n                \\\"streetName5\\\": \\\"iX9NUr4fDtvA0difyLgkrGRcBKuIMNe6PPKHBYeKlFEAJ60XEXWCvPstg5Kc7zFAq3eghdKWqVA1TROKaikziAZC03GRlI75Jkt\\\",\\r\\n                \\\"streetName6\\\": \\\"VQsEpT12Yk8xy43KeHouol5qyVY0mX0Ft9UWEl5mQtYf9PsK4U7eqFTvHus0Dnq64uQFrHyos4d8jhnh0RqFvfyUPuNDJavFP1R\\\",\\r\\n                \\\"postalCode\\\": \\\"142353\\\",\\r\\n                \\\"cityCode\\\": {\\r\\n                    \\\"codeValueId\\\": 51305,\\r\\n                    \\\"codeValueCd\\\": \\\"30805\\\",\\r\\n                    \\\"referenceCodeValueCd\\\": \\\"30805\\\",\\r\\n                    \\\"codeValueDisplay\\\": \\\"KALYAN\\\"\\r\\n                },\\r\\n                \\\"stateCode\\\": {\\r\\n                    \\\"codeValueId\\\": 57353,\\r\\n                    \\\"codeValueCd\\\": \\\"MH\\\",\\r\\n                    \\\"referenceCodeValueCd\\\": \\\"MH\\\",\\r\\n                    \\\"codeValueDisplay\\\": \\\"MAHARASHTRA\\\"\\r\\n                },\\r\\n                \\\"countryCode\\\": {\\r\\n                    \\\"codeValueId\\\": 1959,\\r\\n                    \\\"codeValueCd\\\": \\\"IN\\\",\\r\\n                    \\\"referenceCodeValueCd\\\": \\\"IN\\\",\\r\\n                    \\\"codeValueDisplay\\\": \\\"INDIA\\\"\\r\\n                },\\r\\n                \\\"nativeStreetName1\\\": \\\"X9mIGi0n9YYRbRHmBb1AF5hTAhE2sTQg1RRVKW4NUH7Sz9Ihr0GNttABcOolC4IseMJ2qrMsFxibW68bJMdTD61DyOHDhKMOYtf\\\",\\r\\n                \\\"nativeStreetName2\\\": \\\"A0Up4AGRu5P6g5zG90N4M11LqAqiuWgnDzBOraolrrEs3URGp9AqptWJClxUUl1LgUEiNRqk7C0PqPclzk6RTUSsCWYiI3zubAu\\\",\\r\\n                \\\"nativeStreetName3\\\": \\\"mZQQt8jqnltTQGqKmn62PTpidxdREr4lE2tz4rGFolldFNLE2HPh4LihXZzpW4Ept7jqME9DL3eQgPQ3uYu055ikmrQRMuNvCiy\\\",\\r\\n                \\\"nativeStreetName4\\\": \\\"kW36MpncWgPg1OjPYAEeZnOa3yksFDrgQT1CaivF0OYfcWg59qpMRjfQM8vfrWBZycmkRNrYNGosNe8e1ZY3MCT3CrBDGm01GCc\\\",\\r\\n                \\\"preferredFlag\\\": false,\\r\\n                \\\"holdMailFlag\\\": false,\\r\\n                \\\"holdMailReason\\\": \\\"Ae10ZQsrnVIfqXxUfFWaUmLvMXBlBMMJldpJGWrcPOyEUFCJ4zArbi3jgHaq4bPSPhFn1m05tpBZm9tzd5RB6Las0XqD2CMP2KX\\\",\\r\\n                \\\"formatAddress1\\\": \\\"BLK T9xyI 1jRYzB6e9n3RSgTC7IyyvmqMYD6ICx\\\",\\r\\n                \\\"formatAddress2\\\": \\\"Fba0XQXB9UuOpWIsraBlJCWT9KM1yLCKMgKelrmKPlGVuepE79JV6g5vTQfuig5QjEgYeLthmR2gIcqe25r03DlsWzhcQc6xD3V\\\",\\r\\n                \\\"formatAddress3\\\": \\\"YiQqp6ton2f0BamZgZ9toJcLNmxqLnEURPIiHNvnX8aN8rUxcnK1vFMrlzj1vQakegvaopqIOfrM6thtP2XShtfNKSEOxzgtkmk\\\",\\r\\n                \\\"formatAddress4\\\": \\\"#PQg-IWZq3I0 SINGAPORE 142353\\\",\\r\\n                \\\"version\\\": 1,\\r\\n                \\\"createdBy\\\": \\\"MIGRATION\\\",\\r\\n                \\\"createdByChannel\\\": \\\"MIGRATION\\\",\\r\\n                \\\"createdByDepartment\\\": \\\"\\\",\\r\\n                \\\"createdTimeStamp\\\": \\\"2022-09-17T00:00:00+08:00\\\",\\r\\n                \\\"updatedBy\\\": \\\"MIGRATION\\\",\\r\\n                \\\"updatedByChannel\\\": \\\"MIGRATION\\\",\\r\\n                \\\"updatedByDepartment\\\": \\\"\\\",\\r\\n                \\\"updatedTimeStamp\\\": \\\"2021-03-20T13:11:58+08:00\\\"\\r\\n            },\\r\\n            {\\r\\n                \\\"id\\\": 1097352,\\r\\n                \\\"startDate\\\": \\\"2021-03-20\\\",\\r\\n                \\\"endDate\\\": \\\"2099-12-31\\\",\\r\\n                \\\"addressLabel\\\": \\\"ZByfmmxZxKbov7HAvJdaDUxFlSq16JHVZzddscvnFXRgq6CrXV2TifE7J5ozTDpDOSNvvXR5sULFx1Jx3daRLs2YtXdoEKrndPa\\\",\\r\\n                \\\"addressTypeCode\\\": {\\r\\n                    \\\"codeValueId\\\": 22771,\\r\\n                    \\\"codeValueCd\\\": \\\"Mailing3\\\",\\r\\n                    \\\"referenceCodeValueCd\\\": \\\"Mailing3\\\",\\r\\n                    \\\"codeValueDisplay\\\": \\\"Mailing 3\\\"\\r\\n                },\\r\\n                \\\"levelNumber\\\": \\\"AVH\\\",\\r\\n                \\\"unitNumber\\\": \\\"nfgGZvE\\\",\\r\\n                \\\"blockNumber\\\": \\\"SpbMK\\\",\\r\\n                \\\"streetName1\\\": \\\"F4O0OjnHVJsKcpg1iKZISeRRzMdvHraY6XrJ3vKbNm2Ho3oQ0g1qVTJFkXpzCdlWveh9KtCtyU6FzXayW5xz9WC7AOOEbeYbktD\\\",\\r\\n                \\\"streetName2\\\": \\\"ujIIeZFbf2p2m7ILcBrWgMiUnl83m1nU2E5uJEtU5bBJ0PkyHjBpVFcfjULHA5ipkK1i6cLd3Z8ZIQbaGND79nNAk8BpL6NYcW5\\\",\\r\\n                \\\"streetName3\\\": \\\"EURxm8XAUtAxrzgzpamPHqqxuOFZnfR7kyYbJSvim8Yy8BqAVCagDGYSSRJ5GqcRVjIS2HsnGvkihSQaBLKmxZepaByZpj9Asa0\\\",\\r\\n                \\\"streetName4\\\": \\\"YMTWJHNuQdSJmPrdxPJ6EQAV7bMoaLXvWgJ9Jq5gAU2Q7DDqDvL66dC5hryYloYfPopQ9MKCai3K48pGI6ZnWsxGa8o4847BSqI\\\",\\r\\n                \\\"streetName5\\\": \\\"kRQbqfgW7p7H9SNBbodeSFob3hbPEg8RuIDFoSeLxs1WoYH5dAqqNqMae1LBGTr5AaF87z57GdWLfb7MOOzGNPqDafWGLTkfxfy\\\",\\r\\n                \\\"streetName6\\\": \\\"VKtZoAQhUpOXgpG8a755efmiisZa8oVLhhylRIYp5SOGiat0t6SpEMn4M3GElczlbK6x3xSeI6vdPvrpEL5XTnR3C6J1I00hneX\\\",\\r\\n                \\\"postalCode\\\": \\\"547333\\\",\\r\\n                \\\"cityCode\\\": {\\r\\n                    \\\"codeValueId\\\": 51305,\\r\\n                    \\\"codeValueCd\\\": \\\"30805\\\",\\r\\n                    \\\"referenceCodeValueCd\\\": \\\"30805\\\",\\r\\n                    \\\"codeValueDisplay\\\": \\\"KALYAN\\\"\\r\\n                },\\r\\n                \\\"stateCode\\\": {\\r\\n                    \\\"codeValueId\\\": 57353,\\r\\n                    \\\"codeValueCd\\\": \\\"MH\\\",\\r\\n                    \\\"referenceCodeValueCd\\\": \\\"MH\\\",\\r\\n                    \\\"codeValueDisplay\\\": \\\"MAHARASHTRA\\\"\\r\\n                },\\r\\n                \\\"countryCode\\\": {\\r\\n                    \\\"codeValueId\\\": 1959,\\r\\n                    \\\"codeValueCd\\\": \\\"IN\\\",\\r\\n                    \\\"referenceCodeValueCd\\\": \\\"IN\\\",\\r\\n                    \\\"codeValueDisplay\\\": \\\"INDIA\\\"\\r\\n                },\\r\\n                \\\"nativeStreetName1\\\": \\\"lX64uqsWYkaApbTDqyZPDsKFWBjxOnNjorc7gy4PsQLvPuMvYh6CZk2Ra7pxg0UHdIqsxJOSakxHSEKrOes44syCFs11KVhQBAR\\\",\\r\\n                \\\"nativeStreetName2\\\": \\\"TgARkCahh1mg2ultSQrrS2mlnYDBU6Ndv7E42ECyrdWJLGbuSz3vMn8FnueLTRVzYugRTgsOnb9eUsZJ2GBYQcU30JM8JAEdmQJ\\\",\\r\\n                \\\"nativeStreetName3\\\": \\\"2oqOLOeZpoMzXllzfLN0XryEecPtF3mSzDb9kN9eE5shj7XAoCcZKZPKt0IWd0fF6Se8BCaP5oUqBAZUBn5nxixbB64XBgIpEye\\\",\\r\\n                \\\"nativeStreetName4\\\": \\\"EQA8d49m2gQq5YcH7s7TI8FlJ4q6IEudjILXHC65MIMFcYNTdkKblbUn8yBuCrFUyUuYNjcplMfEVdr4WpCcV5QYvgl987ML1An\\\",\\r\\n                \\\"preferredFlag\\\": false,\\r\\n                \\\"holdMailFlag\\\": false,\\r\\n                \\\"holdMailReason\\\": \\\"klvGY0NGpZjxHCrtD9HZGUAmKnQzcGWJiD69Tj9XzpaC6O0vYfqj52UgzcHTNE5pxO2lkHmklYKiRToX0thTkbSS890KpO48mXZ\\\",\\r\\n                \\\"formatAddress1\\\": \\\"BLK SpbMK F4O0OjnHVJsKcpg1iKZISeRRzMdvHr\\\",\\r\\n                \\\"formatAddress2\\\": \\\"ujIIeZFbf2p2m7ILcBrWgMiUnl83m1nU2E5uJEtU5bBJ0PkyHjBpVFcfjULHA5ipkK1i6cLd3Z8ZIQbaGND79nNAk8BpL6NYcW5\\\",\\r\\n                \\\"formatAddress3\\\": \\\"EURxm8XAUtAxrzgzpamPHqqxuOFZnfR7kyYbJSvim8Yy8BqAVCagDGYSSRJ5GqcRVjIS2HsnGvkihSQaBLKmxZepaByZpj9Asa0\\\",\\r\\n                \\\"formatAddress4\\\": \\\"#AVH-nfgGZvE SINGAPORE 547333\\\",\\r\\n                \\\"version\\\": 1,\\r\\n                \\\"createdBy\\\": \\\"MIGRATION\\\",\\r\\n                \\\"createdByChannel\\\": \\\"MIGRATION\\\",\\r\\n                \\\"createdByDepartment\\\": \\\"\\\",\\r\\n                \\\"createdTimeStamp\\\": \\\"2022-09-17T00:00:00+08:00\\\",\\r\\n                \\\"updatedBy\\\": \\\"MIGRATION\\\",\\r\\n                \\\"updatedByChannel\\\": \\\"MIGRATION\\\",\\r\\n                \\\"updatedByDepartment\\\": \\\"\\\",\\r\\n                \\\"updatedTimeStamp\\\": \\\"2021-03-20T13:11:58+08:00\\\"\\r\\n            }\\r\\n        ],\\r\\n        \\\"emails\\\": [\\r\\n            {\\r\\n                \\\"id\\\": 1951478,\\r\\n                \\\"emailAddress\\\": \\\"IN1I2200Z52FL@dbstest.com\\\",\\r\\n                \\\"emailTypeCode\\\": {\\r\\n                    \\\"codeValueId\\\": 23090,\\r\\n                    \\\"codeValueCd\\\": \\\"51\\\",\\r\\n                    \\\"referenceCodeValueCd\\\": \\\"51\\\",\\r\\n                    \\\"codeValueDisplay\\\": \\\"OFFICIAL\\\"\\r\\n                },\\r\\n                \\\"emailStatusCode\\\": {\\r\\n                    \\\"codeValueId\\\": 902,\\r\\n                    \\\"codeValueCd\\\": \\\"V\\\",\\r\\n                    \\\"referenceCodeValueCd\\\": \\\"V\\\",\\r\\n                    \\\"codeValueDisplay\\\": \\\"VERIFIED\\\"\\r\\n                },\\r\\n                \\\"preferredFlag\\\": true,\\r\\n                \\\"version\\\": 1,\\r\\n                \\\"createdBy\\\": \\\"MIGRATION\\\",\\r\\n                \\\"createdByChannel\\\": \\\"MIGRATION\\\",\\r\\n                \\\"createdTimeStamp\\\": \\\"2022-09-17T00:00:00+08:00\\\",\\r\\n                \\\"updatedBy\\\": \\\"MIGRATION\\\",\\r\\n                \\\"updatedByChannel\\\": \\\"MIGRATION\\\",\\r\\n                \\\"updatedTimeStamp\\\": \\\"2021-03-30T15:05:40+08:00\\\"\\r\\n            }\\r\\n        ],\\r\\n        \\\"contacts\\\": [\\r\\n            {\\r\\n                \\\"id\\\": 130347,\\r\\n                \\\"contactTypeCode\\\": {\\r\\n                    \\\"codeValueId\\\": 899,\\r\\n                    \\\"codeValueCd\\\": \\\"4\\\",\\r\\n                    \\\"referenceCodeValueCd\\\": \\\"4\\\",\\r\\n                    \\\"codeValueDisplay\\\": \\\"HANDPHONE\\\"\\r\\n                },\\r\\n                \\\"contactSuffix\\\": \\\"00\\\",\\r\\n                \\\"contactPersonName\\\": \\\"mGsp6eu6bTayXRuQ4eF9IVpqrTl75fK5Rdb2SfZzHKZVJhVIm6BTXIavNNH5u0RfmYOePzl5mLyGCyzA6clfjI9UzCnIPcBSyCkVZPhZbGVHQVlRzjv3eykIg9ejfUAFIQGMlxZjJTt6FAUMOIT2yy6yK6jOLxZKNVNhIuYIn2u5hk3Eqjchn7DvWClXdKRN0oC7qcQH\\\",\\r\\n                \\\"contactPhoneNumber\\\": \\\"378936910\\\",\\r\\n                \\\"localNumber\\\": \\\"280718784\\\",\\r\\n                \\\"countryCode\\\": {\\r\\n                    \\\"codeValueId\\\": 1539,\\r\\n                    \\\"codeValueCd\\\": \\\"91\\\",\\r\\n                    \\\"referenceCodeValueCd\\\": \\\"91\\\",\\r\\n                    \\\"codeValueDisplay\\\": \\\"INDIA\\\"\\r\\n                },\\r\\n                \\\"preferredFlag\\\": true,\\r\\n                \\\"version\\\": 1,\\r\\n                \\\"createdBy\\\": \\\"MIGRATION\\\",\\r\\n                \\\"createdByChannel\\\": \\\"MIGRATION\\\",\\r\\n                \\\"createdTimeStamp\\\": \\\"2022-09-17T00:00:00+08:00\\\",\\r\\n                \\\"updatedBy\\\": \\\"MIGRATION\\\",\\r\\n                \\\"updatedByChannel\\\": \\\"MIGRATION\\\",\\r\\n                \\\"updatedTimeStamp\\\": \\\"2021-03-30T15:05:40+08:00\\\"\\r\\n            }\\r\\n        ],\\r\\n        \\\"nationalities\\\": [\\r\\n            {\\r\\n                \\\"id\\\": 934479,\\r\\n                \\\"nationalityCode\\\": {\\r\\n                    \\\"codeValueId\\\": 57046,\\r\\n                    \\\"codeValueCd\\\": \\\"IN\\\",\\r\\n                    \\\"referenceCodeValueCd\\\": \\\"IN\\\",\\r\\n                    \\\"codeValueDisplay\\\": \\\"INDIA\\\"\\r\\n                },\\r\\n                \\\"version\\\": 1,\\r\\n                \\\"createdBy\\\": \\\"MIGRATION\\\",\\r\\n                \\\"createdByChannel\\\": \\\"MIGRATION\\\",\\r\\n                \\\"createdTimeStamp\\\": \\\"2021-03-30T15:05:40+08:00\\\",\\r\\n                \\\"updatedBy\\\": \\\"MIGRATION\\\",\\r\\n                \\\"updatedByChannel\\\": \\\"MIGRATION\\\",\\r\\n                \\\"updatedTimeStamp\\\": \\\"2021-03-30T15:05:40+08:00\\\"\\r\\n            }\\r\\n        ],\\r\\n        \\\"educationDetails\\\": [],\\r\\n        \\\"employmentDetails\\\": [\\r\\n            {\\r\\n                \\\"id\\\": 3402614,\\r\\n                \\\"employerName\\\": \\\"LY1vfUFhAubjUzVJyfb5ISnRdzTN3ZoG6x0TqUsgyiNEHt7hAXFn0skdGYinAe7ZEEat7rJOjdOKxWhT0FLVrED0JvPf2hbpbRKyBMi0Ir22ieenyfpeUKsvL3fq1i8Fz5lLhXQUggHmoAK4b01A5CAVrbxooboii\\\",\\r\\n                \\\"employerNameInAlternateLanguage\\\": \\\"ecY2J3Oyg5cAQhNKqppyjAjWmIS3tZr0cZrDnXC8thAGnT0z1SUJHk5Blytz2CnidJeW2m9krgJupML3JNC2mXdmY2hj5pSXK2m1lQkLYWJJ49bDY2Em7PKsKGaGVtI2gC9YeyPPSY7gFNgIDxbhEd5JALSvSaNzqzDdI02HIgeiOnh51hgGdDaagPV5XRQf1mg6QljPIvvHansjz2U4THzvo06KZAZmbx4UkH0OSr7jE81Z5ytRQfeYSeXjR7V\\\",\\r\\n                \\\"addressLine1\\\": \\\"AMKQgMD7Me3sRzO9UWqFsL7fU6gaJKIxKC1LGIKB2mK31tXGRzFHBVtExfmsbQtCfvAx2IYjTeSLtoTU9BCjk2pCKCVSJleEsJZCVYeRCjaG8pu8caWAmpuqNUEeK4hYz05e9CnpIKcDyOJfAYfMStWq2X9mvWmU3d4gSa8pn7ytkVTxe3N9Hu2kR5umXpfjdgAttgOHxQ6dN9RmdLv3uzg3V5qnOfVnXZh8E9728JnuAkhpTb1D12uRUcyoff2\\\",\\r\\n                \\\"addressLine2\\\": \\\"zTWIjB7ETHYM8GiFYE20J382gNmTe8gMHoj0PihO3UMZvBCu0KAjdaDKvcLUZyFJaSoqGpl8KsfmG0N2hmOJoloZp39nIHNPFPvqKxXUOblCYsWriYHoiKGrgjnNU8GQ5SHjMFV4lkOdnAAYSCJnbmv4U6b5EeCSFa2bD2BSp20Jh3oP8mqBOO0yDR2vjcQqBxvT1d0brSCfTvvoVk16FEearxZzLtjM6N3JEriR0jq1d0TVFdjdUko7yXgYYQG\\\",\\r\\n                \\\"postalCode\\\": \\\"GBygB43055\\\",\\r\\n                \\\"cityCode\\\": {\\r\\n                    \\\"codeValueId\\\": 51140,\\r\\n                    \\\"codeValueCd\\\": \\\".\\\",\\r\\n                    \\\"referenceCodeValueCd\\\": \\\".\\\",\\r\\n                    \\\"codeValueDisplay\\\": \\\".\\\"\\r\\n                },\\r\\n                \\\"stateCode\\\": {\\r\\n                    \\\"codeValueId\\\": 57304,\\r\\n                    \\\"codeValueCd\\\": \\\".\\\",\\r\\n                    \\\"referenceCodeValueCd\\\": \\\".\\\",\\r\\n                    \\\"codeValueDisplay\\\": \\\".\\\"\\r\\n                },\\r\\n                \\\"phoneNumber\\\": \\\"lc5L4ccRuqpLdZypU3Me\\\",\\r\\n                \\\"phoneAreaNumber\\\": \\\"G9ABYCAtIjzWNCLnoI28\\\",\\r\\n                \\\"faxNumber\\\": \\\"VlfZhGeCfhzqob27NLemyOgAaPial3\\\",\\r\\n                \\\"incomeRangeFrom\\\": \\\"1500000\\\",\\r\\n                \\\"incomeRangeTo\\\": \\\"2999999\\\",\\r\\n                \\\"employmentStatusCode\\\": {\\r\\n                    \\\"codeValueId\\\": 881,\\r\\n                    \\\"codeValueCd\\\": \\\"02\\\",\\r\\n                    \\\"referenceCodeValueCd\\\": \\\"02\\\",\\r\\n                    \\\"codeValueDisplay\\\": \\\"PERMANENT EMPLOYED - FULL TIME\\\"\\r\\n                },\\r\\n                \\\"sourceOfIncomeCode\\\": {\\r\\n                    \\\"codeValueId\\\": 23107,\\r\\n                    \\\"codeValueCd\\\": \\\"003\\\",\\r\\n                    \\\"referenceCodeValueCd\\\": \\\"003\\\",\\r\\n                    \\\"codeValueDisplay\\\": \\\"Inheritance\\\"\\r\\n                },\\r\\n                \\\"occupationalGroupCode\\\": {\\r\\n                    \\\"codeValueId\\\": 14495,\\r\\n                    \\\"codeValueCd\\\": \\\"13\\\",\\r\\n                    \\\"referenceCodeValueCd\\\": \\\"13\\\",\\r\\n                    \\\"codeValueDisplay\\\": \\\"SERVICE – PRIVATE SECTOR\\\"\\r\\n                },\\r\\n                \\\"version\\\": 1,\\r\\n                \\\"createdBy\\\": \\\"FIVUSR\\\",\\r\\n                \\\"createdByChannel\\\": \\\"MIGRATION\\\",\\r\\n                \\\"createdTimeStamp\\\": \\\"2021-03-20T13:11:58+08:00\\\",\\r\\n                \\\"updatedBy\\\": \\\"SEJALAGRAWAL\\\",\\r\\n                \\\"updatedByChannel\\\": \\\"MIGRATION\\\",\\r\\n                \\\"updatedTimeStamp\\\": \\\"2021-03-30T15:05:40+08:00\\\"\\r\\n            }\\r\\n        ],\\r\\n        \\\"relationshipManagers\\\": [\\r\\n            {\\r\\n                \\\"id\\\": 1463044,\\r\\n                \\\"ownerId\\\": 831384,\\r\\n                \\\"officerIncharge\\\": \\\"Bj2uzCjkIRf7QI8F0P3\\\",\\r\\n                \\\"order\\\": 1,\\r\\n                \\\"version\\\": 1,\\r\\n                \\\"createdBy\\\": \\\"FIVUSR\\\",\\r\\n                \\\"createdByChannel\\\": \\\"MIGRATION\\\",\\r\\n                \\\"createdTimeStamp\\\": \\\"2021-03-20T13:11:58+08:00\\\",\\r\\n                \\\"updatedBy\\\": \\\"SEJALAGRAWAL\\\",\\r\\n                \\\"updatedByChannel\\\": \\\"MIGRATION\\\",\\r\\n                \\\"updatedTimeStamp\\\": \\\"2022-02-10T19:56:58+08:00\\\"\\r\\n            }\\r\\n        ]\\r\\n    },\\r\\n    \\\"status\\\": {\\r\\n        \\\"statusCode\\\": 0\\r\\n    },\\r\\n    \\\"traceInfo\\\": {\\r\\n        \\\"traceId\\\": \\\"83bb2940-1f37-11eb-adc1-0242ac120009\\\",\\r\\n        \\\"spanId\\\": \\\"d6109be91404d6ea\\\",\\r\\n        \\\"timestamp\\\": \\\"2024-12-07T15:42:30.515062+08:00\\\"\\r\\n    }\\r\\n}\"";

            //string sCustomerName = "89SdOhEnyHC5xJjOiHua4ZpiC02WBGKJFG0OyzBJfZsUUlKtev3XZMyjS4SCmjMoRPDMsjRnVs0M9dET4gtyiobeTK45TyKyrPsSAQamW5OXsdCnJd93ypKLJRZDbKknhEB0vKEQHOfYB463sMfdLmhMyIuOvVL";
            string sCustomerName = "i6dzbff29ch5uDkqCO7JxxLVin6jW1tk1qBePUbVK35n2yiiI6If4Op7dxJpjAblBvMpARqqGB6fObMBRnFdDP16h60VVSZ9xWK";

            try
            {

                //var jObject = Newtonsoft.Json.Linq.JObject.Parse(sResposne);
                //if (jObject["data"]["profileInfo"]["registeredName"] != null)
                //{
                //    sCustomerName = jObject["data"]["profileInfo"]["registeredName"].ToString().Trim();
                //}
                //else
                //{
                //    //WriteState code to log error
                //    logerror(jObject["errorDescription"].ToString(), "");
                //}
                return sCustomerName;
            }
            catch (Exception Ex)
            {
                logerrorInCatch("Exception in GetCMCPCustomerName ", Ex.Message.ToString());
                return sCustomerName;
            }

        }

        //test function


        //cbs details api call end




        //signature api call 11/11/24 start==================================
        public JsonResult GetSignatureData(string ac=null,string documentType=null)
        {


           


            logerror("in GetSignatureData==>", "");
            logerror("in GetSignatureData==>", "ac==>"+ac+"documenttype==>"+documentType);
            //CASA variables
            var CasaClientId = ConfigurationManager.AppSettings["CasaClientId"].ToString();
            var CasaCorellationId = ConfigurationManager.AppSettings["CasaCorellationId"].ToString();
            var CasaServiceURL = ConfigurationManager.AppSettings["CasaServiceURL"].ToString();


            //Token variables
            var TokenClientId = ConfigurationManager.AppSettings["TokenClientId"].ToString();
            var TokenSecreteKey = ConfigurationManager.AppSettings["TokenSecreteKey"].ToString();
            var TokenServiceURL = ConfigurationManager.AppSettings["TokenServiceURL"].ToString();


            //signature url variable
            var SignatureDocument_list= ConfigurationManager.AppSettings["SignatureDocument_list"].ToString();
            var Signaturedocument_enquiry_blob= ConfigurationManager.AppSettings["Signaturedocument_enquiry_blob"].ToString();
            var specialInstructions= ConfigurationManager.AppSettings["SpecialInstructions"].ToString();
            var specialRequirements = ConfigurationManager.AppSettings["SpecialRequirements"].ToString();


            //CMPC variables
            var CMCPCountry = ConfigurationManager.AppSettings["CMCPCountry"].ToString();
            var CMCPReqUID = ConfigurationManager.AppSettings["CMCPReqUID"].ToString();
            var CMCPReqClientId = ConfigurationManager.AppSettings["CMCPReqClientId"].ToString();
            var CMCPServiceURL = ConfigurationManager.AppSettings["CMCPServiceURL"].ToString();



            //document type
            // var SignatureDocumentType = ConfigurationManager.AppSettings["SignatureDocumentType"].ToString();



            var ProductType = "";
            var codeValueCd = "";
            var spReq = "";
            var spInst = "";

            List<Int64> idlst= new List<Int64>();
            List<string> signatureResponses = new List<string>();

            try
            {
               // string sEtoken = "";
                string sEtoken = GetToken();
               // logerror("in GetSignatureData==>", "token==>"+sEtoken);
                //call casa api to get product code
               // var casaResponce= getAccountDetailsDBSRequest(CasaServiceURL, CasaClientId, CasaCorellationId, ac.ToUpper(), sEtoken);
                var casaResponce = sgetAccountDetailsDBS;

               // var casatest = "{\"sourceAccountNumber\":\"8811010101056687\",\"ibanAccountNumber\":\"\",\"sourceSystemId\":\"\",\"sourceCustomerId\":\"2I06JMT\",\"productType\":\"SBA\",\"productTypeDescription\":[],\"subProductType\":\"\",\"productCode\":\"TREPB\",\"productCodeDescription\":[{\"productCodeDescription\":\"SAVINGS TREASURES\",\"descriptionCategory\":\"default\",\"languageCode\":\"INFENG\"}],\"productName\":\"TREPB\",\"nativeProductName\":\"TREPB\",\"accountCurrency\":\"INR\",\"openedDate\":1548979200000,\"modeOfOperation\":\"0003\",\"serviceChargeExemption\":0,\"staffIndicator\":false,\"officerId\":\"\",\"officerUnit\":\"\",\"firstExcessDate\":0,\"lastUpdatedEvent\":\"\",\"disbursementDate\":0,\"maturityDate\":0,\"returnChequeDetailsInfo\":[],\"accountStatus\":\"Active\",\"accountStatusCode\":\"A\",\"accountSignatoryType\":\"01\",\"accountSignal\":\"\",\"loanServicingIndicator\":false,\"accountFrozenIndicator\":false,\"noDebitIndicator\":false,\"debitReferralIndicator\":false,\"irregularSignalIndicator\":false,\"lineOfferedIndicator\":false,\"closureNoticeIndicator\":false,\"multipleAccountIndicator\":false,\"recallPassbookIndicator\":false,\"updateRequiredIndicator\":false,\"productIndicator\":\"\",\"brandIndicator\":\"\",\"ibanIndicator\":\"N\",\"virtualAccountIndicator\":\"N\",\"spclCustomerType\":\"\",\"accountType\":\"\",\"accountTypeDescription\":\"\",\"currencyDecimal\":2,\"generalLedgerSubHeadCode\":\"21201\",\"accountCurrencyCode\":\"INR\",\"odLimitType\":\"\",\"limitId\":\"\",\"limitEffectiveDate\":0,\"odInterestAmount\":0,\"accountName\":\"INDER\",\"accountShortName\":\"INDER\",\"virtualAccountName\":\"\",\"accountStatement\":{\"statementMode\":\"B\",\"statementCalendar\":\"00\",\"frequency\":\"M\",\"frequencyStartDate\":31,\"frequencyDay\":0,\"frequencyWeekNumber\":0,\"frequencyHolidayStatus\":\"N\",\"nextPrintDate\":4102358400000,\"despatchMode\":\"N\"},\"balanceDebitCreditIndicator\":\"C\",\"freezeCode\":\"\",\"freezeStatusCode\":\"\",\"freezeReasonCode\":\"\",\"freezeReasonCode1\":\"\",\"additionalFreezeReasonCodes\":[],\"additionalFreezeRemarks\":[],\"freezeReasonCodeDescriptionList\":[],\"freezeRemarks\":\"\",\"freezeRemarks1\":\"\",\"accountInterest\":{\"interestRate\":2,\"penalInterestRate\":0,\"accruedInterestAmount\":23554.122118,\"penalInterestAmount\":0,\"uncollectedInterestAmount\":0,\"interestCalInterest\":0,\"interestFrequencyType\":\"M\",\"interestFrequencyStartDate\":31,\"interestFrequencyDay\":0,\"interestFrequencyWeekNum\":0,\"accountInterestFrequencyHolidayStatus\":\"P\",\"interestRateCode\":\"SBGEN\",\"netInterestRate\":2,\"netInterestDebitCreditIndicator\":\"C\",\"accruedInterestDebitCreditIndicator\":\"C\",\"interestBookednotApplied\":345.12},\"taxCategory\":\"A\",\"taxFloorLimit\":0,\"taxFloorLimitCurrencyCode\":\"INR\",\"withholdingTaxPercent\":0,\"gstin\":\"\",\"gstExemptionFlag\":\"N\",\"nickName\":\"\",\"productShortName\":\"TREPB\",\"preferredLanguageProductShortName\":\"TREPB\",\"sourceMultiCurrencyAccountNumber\":\"\",\"multiCurrencyAccountFlag\":false,\"branchCode\":\"811\",\"branchCodeDescription\":\"UPDATE SOL MUMBAI\",\"bankCode\":\"DBSIN\",\"accountClosedFlag\":\"N\",\"accountClosedReasonCode\":\"\",\"accountClosedRemarks\":\"\",\"accountClosedDate\":0,\"lastBalanceUpdateDateTime\":0,\"earmarkUpdateDateTime\":0,\"holdBalanceUpdateDateTime\":0,\"sanctionLimitUpdateDateTime\":0,\"staticDataUpdateDateTime\":1727628541000,\"halfDayHoldBalanceExpiryDate\":0,\"childAccounts\":[],\"accountBalances\":{\"availableBalance\":699684.63,\"availableBalanceCurrencyCode\":\"INR\",\"accountBalance\":699684.63,\"accountBalanceCurrencyCode\":\"INR\",\"sanctionLimit\":0,\"sanctionLimitCurrencyCode\":\"INR\",\"ledgerBalance\":700409.63,\"ledgerBalanceCurrencyCode\":\"INR\",\"halfDayHoldBalance\":0,\"oneDayHoldBalance\":0,\"twoDayHoldBalance\":0,\"earmarkDebitAmount\":0,\"earmarkCreditAmount\":0,\"floatAmount\":0,\"earmarkAmount\":0,\"effectiveAvailableAmount\":699684.63,\"drawingPower\":0,\"overDueLiableAmount\":0,\"openingBalanceAmount\":700409.63,\"closingBalanceAmount\":700409.63,\"fundsClearingAmount\":0,\"cumulativeCreditAmount\":871591.5,\"cumulativeDebitAmount\":171181.87,\"utilizedAmount\":0,\"systemReservedAmount\":0,\"overdueFutureAmount\":0,\"utilizedFutureAmount\":0,\"effectiveFutureAvailableAmount\":699684.63,\"availableAmountLineOfCredit\":0,\"unclearDrawingAmount\":0,\"ffdAvailableAmount\":0,\"sweepsEffectiveAvailableAmount\":699684.63,\"hcAvailableAmount\":0,\"futureAmount\":725,\"futureCreditAmount\":725,\"futureClearBalanceAmount\":725,\"futureUnclearBalanceAmount\":0,\"daccLimit\":0,\"dafaLimit\":0,\"principalBalance\":700409.63,\"amountPaidToday\":0,\"totalOutstandingAmount\":700409.63},\"relatedCustomerInfo\":[{\"relatedPartyCode\":\"\",\"relatedPartyCodeDescription\":\"\",\"relatedPartyCustomerId\":\"2I06JMT\",\"relatedPartySourceCustomerId\":\"\",\"relatedPartyType\":\"M\",\"relatedPartyTypeDescription\":\"Main Holder\",\"relatedPartyDeleteFlag\":\"N\",\"relatedPartyAddressType\":\"Mailing\"}],\"promoCode\":[],\"mobileMoneyIdentifier\":\"\",\"mobileNumbers\":[],\"reference1\":\"\",\"reference2\":\"\",\"ifscCode\":\"DBSS0IN0811\",\"channelId\":\"\",\"accountStatusDate\":1675641600000,\"expressAccountExpiryDate\":0,\"schemeConversionDate\":0,\"virtualAccountType\":\"\",\"faxIndeminity\":\"\",\"autoSaverFlag\":\"\",\"sweepAllowFlag\":\"N\",\"pastDueFlag\":\"\",\"totalDaysPastOverDue\":\"0\",\"sanctionLimitExpiryDate\":0,\"collateralId\":\"\",\"advancePaidAmount\":0,\"nomineeAvailableFlag\":\"N\",\"prioritySectorDetails\":{\"prioritySectorCategory\":\"\",\"prioritySectorSubCategory\":\"\",\"natureOfActivity\":\"\",\"mandateReferenceNumber\":\"\",\"weakerSectionType\":\"\"},\"nomineeGuardianInfo\":[]}";
               // var jObject = Newtonsoft.Json.Linq.JObject.Parse(casatest);

              // var jObject = Newtonsoft.Json.Linq.JObject.Parse(casaResponce);
               var jObject = Session["JobjectForSignature"] as Newtonsoft.Json.Linq.JObject;

               // logerror("in GetSignatureData==>", "jObject==>" + jObject.ToString());

                if (jObject["error"] != null)
                {
                    ProductType = "";
                }
                else
                {
                    if(jObject["accountClosedFlag"].ToString().Trim().ToUpper() != "Y" && (jObject["productType"].ToString()!="" || jObject["productType"].ToString() != null))
                    {
                        ProductType = jObject["productType"].ToString();

                        logerror("in GetSignatureData==>", "productType==>" + ProductType.ToString());
                    }

                    codeValueCd = ProductType;
                }
            
                //call signature list api and get id 
                idlst = FetchthevalueIDfromsignaturedocumentlist(SignatureDocument_list, sEtoken,ac, documentType, codeValueCd, CMCPCountry, CMCPReqUID,CMCPReqClientId);

                logerror("in GetSignatureData==>", "idlst==>" + idlst[0].ToString());

                //call signature blob api and get signature 
                foreach (var id in idlst) 
                {
                    var str = SignatureBlob(id, Signaturedocument_enquiry_blob, sEtoken, CMCPCountry, CMCPReqUID,CMCPReqClientId);
                    signatureResponses.Add(str);
                }

                logerror("in GetSignatureData==>", "signatureResponses==>" + signatureResponses[0].ToString());


                //call special instruction api 
                spReq = SpecialRequirement(specialRequirements, ac, codeValueCd, sEtoken, CMCPCountry, CMCPReqUID,CMCPReqClientId);

                logerror("in GetSignatureData==>", "SpecialRequirement==>" + spReq.ToString());


                //call spec instruction api
                spInst = SpecialInstruction(specialInstructions, ac, codeValueCd, sEtoken, CMCPCountry, CMCPReqUID,CMCPReqClientId);

                logerror("in GetSignatureData==>", "SpecialInstruction==>" + spInst.ToString());



            }
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                string trace = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                {
                    innerExcp = e.InnerException.Message;
                    trace = e.InnerException.StackTrace;
                }
                logerror("In GetSignatureData Catch==>>" + message, "InnerExp===>>" + innerExcp);
            }



        
           
            return Json(new
            {
                Signatures = signatureResponses,
                SpecialRequirement = spReq,
                SpecialInstruction = spInst
            }, JsonRequestBehavior.AllowGet);


           // return Json(signatureResponses, JsonRequestBehavior.AllowGet);
        }

        //signature api document list
        public List<Int64> FetchthevalueIDfromsignaturedocumentlist(string signatureDocumentListUrl,string token,string ac,string documentType,string codeValueCdstr,string CMCPCountry, string CMCPReqUID,string CMCPReqClientId)
        {
            Int64 output = 0;
            var responce = "";
            var ids = new List<Int64>();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;

                //
                string dt = DateTime.Now.ToString("yyyy-MM-dd");
                


               




                var inputstr = "";

                inputstr = "{";
                inputstr += "   \"query\": {";
                inputstr += "       \"cmcpId\": null,";
                inputstr += "       \"cinDetails\": null,";
                inputstr += "       \"accountNumber\": \"" + ac + "\",";
                inputstr += "       \"productCode\": {";
                inputstr += "           \"codeValueId\": null,";
                inputstr += "           \"codeValueCd\": \""+ codeValueCdstr + "\",";
                inputstr += "           \"referenceCodeValueCd\": null,";
                inputstr += "           \"codeValueDisplay\": null,";
                inputstr += "           \"attributeValue\": null";
                inputstr += "       },";
                inputstr += "       \"sigCmcpId\": null,";
                inputstr += "       \"sigCinDetails\": null,";
                inputstr += "       \"type\": \""+ documentType+"\",";
                inputstr += "       \"startDate\": null,";
                inputstr += "       \"endDate\": \""+dt+"\",";
                inputstr += "       \"excludeErrorSignatures\": true";
                inputstr += "   },";
                inputstr += "   \"page\": {";
                inputstr += "       \"pageSize\": 1000,";
                inputstr += "       \"pageNumber\": 1";
                inputstr += "   }";
                inputstr += "}";


                var httpWebRequest = (HttpWebRequest)WebRequest.Create(signatureDocumentListUrl);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("X-DBS-Country", CMCPCountry);
                httpWebRequest.Headers.Add("X-DBS-ReqUID", CMCPReqUID);
                httpWebRequest.Headers.Add("X-DBS-ReqClientId", CMCPReqClientId);
                httpWebRequest.Headers.Add("Authorization", "Bearer " + token);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(inputstr);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    responce = result;
                }

                var jObject = Newtonsoft.Json.Linq.JObject.Parse(responce);

                if(jObject != null)
                {
                    // Validate statusCode and statusDescription
                    int statusCode = (int)jObject["status"]["statusCode"];
                    string statusDescription = (string)jObject["status"]["statusDescription"];

                    if (statusCode == 0 && statusDescription.Contains("Success"))
                    {
                        foreach (var item in jObject["data"])
                        {
                            ids.Add((Int64)item["id"]);
                        }
                    }
                    else
                    {
                        ids.Add(0);
                    }

                }



            }
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                string trace = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                {
                    innerExcp = e.InnerException.Message;
                    trace = e.InnerException.StackTrace;
                }
                logerror("In FetchthevalueIDfromsignaturedocumentlist Catch==>>" + message, "InnerExp===>>" + innerExcp);
            }

            return ids;
        }


        //signature blob api 
        public string SignatureBlob(Int64 compositeId,string url,string token,string CMCPCountry, string CMCPReqUID,string CMCPReqClientId)
        {
            string responce = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;

                string nwurl = $"{url}?compositeId={compositeId}";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(nwurl);
                request.Method = "GET";

                request.Headers.Add("X-DBS-Country", CMCPCountry);
                request.Headers.Add("X-DBS-ReqUID", CMCPReqUID);
                request.Headers.Add("X-DBS-ReqClientId", CMCPReqClientId);
                request.Headers.Add("Authorization", "Bearer " + token);


                //var httpResponse = (HttpWebResponse)request.GetResponse();
                //if (httpResponse.StatusCode == HttpStatusCode.OK)
                //{
                //    using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream()))
                //    {
                //        string responseBody = reader.ReadToEnd();
                //        responce = responseBody;
                //    }
                //}

                if (compositeId != 0)
                {
                    var httpResponse = (HttpWebResponse)request.GetResponse();
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream responseStream = httpResponse.GetResponseStream())
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            responseStream.CopyTo(memoryStream);
                            byte[] imageData = memoryStream.ToArray();

                            // Convert the byte array to a Base64 string
                            responce = Convert.ToBase64String(imageData);
                        }
                    }
                }
                else
                {
                    responce = NosignaturefoundBase64();
                }

               




            }
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                string trace = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                {
                    innerExcp = e.InnerException.Message;
                    trace = e.InnerException.StackTrace;
                }
                logerror("In SignatureBlob Catch==>>" + message, "InnerExp===>>" + innerExcp);
            }

           
            return  responce;
        }


        //special Requirement api
        public string SpecialRequirement(string url,string ac,string codeValueCd,string token,string CMCPCountry, string CMCPReqUID,string CMCPReqClientId)
        {
            var responce = "";

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;


                var InputString = "";

                InputString = " {";
                InputString += "   \"accountNumber\": \"" + ac + "\",";
                InputString += "   \"productCode\": {";
                InputString += "     \"codeValueId\": null,";
                InputString += "      \"codeValueCd\": \"" + codeValueCd + "\",";
                InputString += "     \"referenceCodeValueCd\": \"null\",";
                InputString += "     \"codeValueDisplay\": null,";
                InputString += "     \"codeType\": {";
                InputString += "       \"codeTypeId\": null,";
                InputString += "       \"codeTypeCd\": \"PRODUCT_CODE\",";
                InputString += "       \"codeTypeDisplayValue\": \"PRODUCT CODE\",";
                InputString += "       \"codeTypeDescription\": null,";
                InputString += "       \"systemControlledIndicator\": 0,";
                InputString += "       \"status\": 1,";
                InputString += "       \"version\": 1,";
                InputString += "       \"createdBy\": \"SYS\",";
                InputString += "       \"updatedBy\": \"SYS\"";
                InputString += "     },";
                InputString += "     \"codeValueDescription\": null,";
                InputString += "     \"systemControlledIndicator\": 0,";
                InputString += "     \"status\": 1,";
                InputString += "     \"version\": 1,";
                InputString += "     \"createdBy\": \"SYS\",";
                InputString += "     \"updatedBy\": \"SYS\"";
                InputString += "   }";
                InputString += "}";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("X-DBS-Country", CMCPCountry);
                httpWebRequest.Headers.Add("X-DBS-ReqUID", CMCPReqUID);
                httpWebRequest.Headers.Add("X-DBS-ReqClientId", CMCPReqClientId);
                httpWebRequest.Headers.Add("Authorization", "Bearer " + token);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(InputString);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    responce = result;
                }
                var jObject = Newtonsoft.Json.Linq.JObject.Parse(responce);


                if(jObject != null)
                {
                    int statusCode = (int)jObject["status"]["statusCode"];
                    string statusDescription = (string)jObject["status"]["statusDescription"];

                    if (statusCode == 0 && statusDescription.Contains("Success"))
                    {

                        foreach (var item in jObject["data"])
                        {
                            responce = item["requirementText"].ToString();
                        }

                       
                    }
                    else
                    {
                        responce = "No data found by search criteria";
                    }
                }


            }
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                string trace = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                {
                    innerExcp = e.InnerException.Message;
                    trace = e.InnerException.StackTrace;
                }
                logerror("In SpecialRequirement Catch==>>" + message, "InnerExp===>>" + innerExcp);
            }

            return responce;

        }


        //special instruction api 
        public string SpecialInstruction(string url, string ac, string codeValueCd, string token, string CMCPCountry, string CMCPReqUID,string CMCPReqClientId)
        {
            var responce = "";


            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                var InputString = "";

                InputString = " {";
                InputString += "   \"accountNumber\": \"" + ac + "\",";
                InputString += "   \"productCode\": {";
                InputString += "     \"codeValueId\": null,";
                InputString += "      \"codeValueCd\": \"" + codeValueCd + "\",";
                InputString += "     \"referenceCodeValueCd\": \"null\",";
                InputString += "     \"codeValueDisplay\": null,";
                InputString += "     \"codeType\": {";
                InputString += "       \"codeTypeId\": null,";
                InputString += "       \"codeTypeCd\": \"PRODUCT_CODE\",";
                InputString += "       \"codeTypeDisplayValue\": \"PRODUCT CODE\",";
                InputString += "       \"codeTypeDescription\": null,";
                InputString += "       \"systemControlledIndicator\": 0,";
                InputString += "       \"status\": 1,";
                InputString += "       \"version\": 1,";
                InputString += "       \"createdBy\": \"SYS\",";
                InputString += "       \"updatedBy\": \"SYS\"";
                InputString += "     },";
                InputString += "     \"codeValueDescription\": null,";
                InputString += "     \"systemControlledIndicator\": 0,";
                InputString += "     \"status\": 1,";
                InputString += "     \"version\": 1,";
                InputString += "     \"createdBy\": \"SYS\",";
                InputString += "     \"updatedBy\": \"SYS\"";
                InputString += "   }";
                InputString += "}";


                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("X-DBS-Country", CMCPCountry);
                httpWebRequest.Headers.Add("X-DBS-ReqUID", CMCPReqUID);
                httpWebRequest.Headers.Add("X-DBS-ReqClientId", CMCPReqClientId);
                httpWebRequest.Headers.Add("Authorization", "Bearer " + token);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(InputString);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    responce = result;
                }
                var jObject = Newtonsoft.Json.Linq.JObject.Parse(responce);

                if (jObject != null)
                {
                    int statusCode = (int)jObject["status"]["statusCode"];
                    string statusDescription = (string)jObject["status"]["statusDescription"];

                    if (statusCode == 0 && statusDescription.Contains("Success"))
                    {
                        foreach(var item in jObject["data"])
                        {
                            responce = item["instructionText"].ToString();
                        }
                       
                    }
                    else
                    {
                        responce = "No data found by search criteria";
                    }
                }





            }
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                string trace = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                {
                    innerExcp = e.InnerException.Message;
                    trace = e.InnerException.StackTrace;
                }
                logerror("In SpecialInstruction Catch==>>" + message, "InnerExp===>>" + innerExcp);
            }

            return responce;

        }

        //signature api call 11/11/24 End==================================


        public JsonResult testsign()
        {
            string url = "https://picsum.photos/seed/picsum/200/300";
            string responce = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            var httpResponse = (HttpWebResponse)request.GetResponse();
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                using (Stream responseStream = httpResponse.GetResponseStream())
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    responseStream.CopyTo(memoryStream);
                    byte[] imageData = memoryStream.ToArray();

                    // Convert the byte array to a Base64 string
                    responce = Convert.ToBase64String(imageData);
                }
            }
            List<string> signatureResponses = new List<string>();
            signatureResponses.Add(responce);
            return Json(new
            {
                Signatures = signatureResponses,
                SpecialRequirement = "spReq",
                SpecialInstruction = "spInst",
            }, JsonRequestBehavior.AllowGet);
        }


        //get GetCPPSFlagAndPayeeNewAccFlag
        public CPPS_NewAccFlag GetCPPSFlagAndPayeeNewAccFlag(Int64 id = 0)
        {
            CPPS_NewAccFlag ob=new CPPS_NewAccFlag();

            try
            {
                    string date= Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");

                    using (SqlCommand cmd = new SqlCommand("IWGetMICRRepairStausandCPPSFlag", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Adding parameters
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@procDate", date);
                        cmd.Parameters.AddWithValue("@Module", "L1");

                        con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string payeeNewAccFlag = reader["PayeeNewAccFlag"].ToString();
                                string p2f = reader["P2F"].ToString();
                                string cppsFlag = reader["CPPS_FLAG"].ToString();

                                ob.P2F = p2f;
                                ob.CPPS_Flag = cppsFlag;
                                ob.PayeeNewAccFlag= payeeNewAccFlag;

                                
                            }
                            
                        }
                    }
                
            }
            
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                string trace = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                {
                    innerExcp = e.InnerException.Message;
                    trace = e.InnerException.StackTrace;
                }
                logerror("In GetCPPSFlagAndPayeeNewAccFlag Catch==>> " + message, "InnerExp===>>" + innerExcp);
            }

            return ob;



        }


        public ActionResult getTiffImage_New2(string httpwebimgpath = null)
        {

            try
            {
                int custid = Convert.ToInt16(Session["CustomerID"]);
                var destpath = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "DestinationDownloadFolder");

                //Owsr.L1VerificationName = l1result.LoginID;
                string destroot = destpath.SettingValue;

                const char delimiter = '\\';
                string[] destrootarr = destroot.Split(delimiter);

                string foldrname = destrootarr[destrootarr.Length - 1];

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

                actualpath = destroot + "\\" + actualpath;
               // System.IO.File.AppendAllText("C:\\temp\\log1.txt", "actualpathOWL1:" + actualpath);

                actualpath = actualpath.Replace("\\\\", "\\");
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
            }
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;
                logerror("In OWL1 getTiffImage_New2 Catch==>" + message, "Inner Exp==>" + innerExcp);
               // logger.Log(LogLevel.Error, "OWL1 getTiffImage_New2|" + message + "INNEREXP| " + innerExcp, "Login Index-Msg");
                //logger.Log(LogLevel.Error, e.InnerException.StackTrace, "Login Index-Exception");

                return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });
            }



            return PartialView("_getTiffImage");
            //return Json(imageDataURL, JsonRequestBehavior.AllowGet);

        }

        public ActionResult getTiffImage(string httpwebimgpath = null)
        {

            try
            {
                logerror("In OWL1 getTiffImage", "LN1997");
                string someUrl = httpwebimgpath;
                var webClient = new WebClient();
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                logerror("In OWL1 getTiffImage", "LN2002");

                byte[] imageBytes = webClient.DownloadData(someUrl);
                logerror("In OWL1 getTiffImage", "LN2005");
                Stream streamactual = new MemoryStream(imageBytes);
                System.Drawing.Bitmap bmp = new Bitmap(streamactual);
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                stream.Position = 0;
                logerror("In OWL1 getTiffImage", "LN2011");

                byte[] data = new byte[stream.Length];
                int lngth = (int)stream.Length;
                stream.Read(data, 0, lngth);
                stream.Close();

                string imageBase64Data = Convert.ToBase64String(data);
                Array.Clear(imageBytes, 0, imageBytes.Length);
                logerror("In OWL1 getTiffImage", "LN2020");

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
                logerror("In OWL1 getTiffImage Catch==>" + message, "Inner Exp==>" + innerExcp);
               // logger.Log(LogLevel.Error, "OWL1 getTiffImg|" + message + "INNEREXP| " + innerExcp, "Login Index-Msg");
                //logger.Log(LogLevel.Error, e.InnerException.StackTrace, "Login Index-Exception");

                return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });
            }



            return PartialView("_getTiffImage");
            //return Json(imageDataURL, JsonRequestBehavior.AllowGet);

        }



        public void UpdateIWL1Verification(
       long id,
       int uid,
       string entrySerialNo,
       string entryPayorRoutNo,
       string entrySAN,
       string transCode,
       string dbtAccno,
       double amount,
       string date,
       int status,
       string reasonCode,
       string reasonDescrp,
       string cbsdtls,
       string jointdtls,
       string lName,
       string payeeName,
       string exreasonCode,
       string exreasonDescrp,
       string FinalModified)
        {

            using (SqlCommand cmd = new SqlCommand("UpdateIWL1Verification", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Uid", uid);
                cmd.Parameters.AddWithValue("@EntrySerialNo", entrySerialNo);
                cmd.Parameters.AddWithValue("@EntrypayorRoutNo", entryPayorRoutNo);
                cmd.Parameters.AddWithValue("@EntrySAN", entrySAN);
                cmd.Parameters.AddWithValue("@TransCode", transCode);
                cmd.Parameters.AddWithValue("@DbtAccno", dbtAccno);
                cmd.Parameters.AddWithValue("@Amount", amount);
                cmd.Parameters.AddWithValue("@Date", date);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@Reasoncode", reasonCode);
                cmd.Parameters.AddWithValue("@ReasonDescrp", reasonDescrp);
                cmd.Parameters.AddWithValue("@cbsdtls", cbsdtls);
                cmd.Parameters.AddWithValue("@jointdtls", jointdtls);
                cmd.Parameters.AddWithValue("@LName", lName);
                cmd.Parameters.AddWithValue("@PayeeName", payeeName);

                cmd.Parameters.AddWithValue("@ExReasoncode", exreasonCode);
                cmd.Parameters.AddWithValue("@ExReasonDescrp", exreasonDescrp);

                cmd.Parameters.AddWithValue("@FinalModified", FinalModified);
                 

                try
                {

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Data updated successfully.");
                }
                catch (Exception e)
                {

                    string message = "";
                    string innerExcp = "";
                    if (e.Message != null)
                        message = e.Message.ToString();
                    if (e.InnerException != null)
                        innerExcp = e.InnerException.Message;
                    logerror("In UpdateIWL1Verification Catch ===>>" + message, "InnerExp==>" + innerExcp);

                }
            }

        }








        private void logerror(string errormsg, string errordesc)
        {
            //ErrorDisplay er = new ErrorDisplay();
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

        private void logerrorInCatch(string errormsg, string errordesc)
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


        public string NosignaturefoundBase64()
        {
            return "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";
        }


        //get BOFD BOFDRoutNo
        public string GetBOFDRoutNo(Int64 id=0)
        {
            string BOFDRoutNo = "";

            try
            {
                string date = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");

                using (SqlCommand cmd = new SqlCommand("IWGetBOFDRoutNo", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Adding parameters
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@procDate", date);
                    cmd.Parameters.AddWithValue("@Module", "L1");

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            BOFDRoutNo= reader["BOFDRoutNo"].ToString();

                        }

                    }
                }
            }
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                string trace = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                {
                    innerExcp = e.InnerException.Message;
                    trace = e.InnerException.StackTrace;
                }
                logerror("In IWL1GetBOFDRoutNo Catch==>> " + message, "InnerExp===>>" + innerExcp);
            }

            return BOFDRoutNo;


        }


    }
}
