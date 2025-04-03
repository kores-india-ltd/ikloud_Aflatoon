using ikloud_Aflatoon.Infrastructure;
using ikloud_Aflatoon.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using NLog;
using System.Web.Routing;
using NPOI.SS.Formula.Functions;
using System.Globalization;
using System.Net.Http;

namespace ikloud_Aflatoon.Controllers
{
    public class OWL2Controller : Controller
    {
        //
        // GET: /OWL2/
        private static Logger logger = LogManager.GetCurrentClassLogger();
        AflatoonEntities af = new AflatoonEntities();
        //OWProcDataContext OWpro = new OWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
        string sInputString = ""; string sResposne = ""; string sgetAccountDetailsDBS = "";

        string sCasaClientId = "";
        string sCasaCorellationId = "";
        string sCasaServiceURL = "";
        string sAccountNo = "";

        List<string> lAccNames = new List<string>();

        public ActionResult Index(int id = 0)
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
                string VFType = "";
                if (id == 1)
                    VFType = "RNormal";
                else if (id == 2)
                    VFType = "RHold";
                else if (id == 3)
                    VFType = "BNormal";
                else if (id == 4)
                    VFType = "BHold";

                Session["VFType"] = VFType;

                SqlDataAdapter adp = new SqlDataAdapter("OWSelectL2", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@VFtype", SqlDbType.NVarChar).Value = VFType;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();
                //-------------Added on 17-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                //-------------Added on 18-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);

                //-------------Added on 12-09-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = Session["CtsSessionType"];
                //--------------Added on 29-09-2018-------------------For Amount wise selction...--------------
                adp.SelectCommand.Parameters.Add("@L2Amountwise", SqlDbType.NVarChar).Value = Session["L2Amountwise"].ToString();
                adp.SelectCommand.Parameters.Add("@L2StartLimit", SqlDbType.Float).Value = Convert.ToDouble(Session["L2StartLimit"]);
                adp.SelectCommand.Parameters.Add("@L2StopLimit", SqlDbType.Float).Value = Convert.ToDouble(Session["L2StopLimit"]);

                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<L2verificationModel>();
                L2verificationModel def;

                if (ds.Tables[0].Rows.Count > 0)
                {

                    def = new L2verificationModel
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
                        L1UserId = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[25].ToString()),
                        L1RejectReason = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[26].ToString()),
                        PayeeName = ds.Tables[0].Rows[0].ItemArray[27].ToString(),
                        CBSAccountInformation = ds.Tables[0].Rows[0].ItemArray[28].ToString(),
                        CBSJointAccountInformation = ds.Tables[0].Rows[0].ItemArray[29].ToString(),
                        UserNarration = ds.Tables[0].Rows[0].ItemArray[30].ToString(),
                        SlipUserNarration = ds.Tables[0].Rows[0].ItemArray[30].ToString(),
                        RejectReasonDescription = ds.Tables[0].Rows[0].ItemArray[31].ToString(),
                        Modified1 = ds.Tables[0].Rows[0].ItemArray[32].ToString(),
                        callby = "Slip",
                    };
                    objectlst.Add(def);
                    //------------------------END------------------------//
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        def = new L2verificationModel
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
                            L1UserId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[25].ToString()),
                            L1RejectReason = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[26].ToString()),
                            PayeeName = ds.Tables[0].Rows[index].ItemArray[27].ToString(),
                            CBSAccountInformation = ds.Tables[0].Rows[index].ItemArray[28].ToString(),
                            CBSJointAccountInformation = ds.Tables[0].Rows[index].ItemArray[29].ToString(),
                            UserNarration = ds.Tables[0].Rows[index].ItemArray[30].ToString(),
                            SlipUserNarration = ds.Tables[0].Rows[index].ItemArray[30].ToString(),
                            RejectReasonDescription = ds.Tables[0].Rows[index].ItemArray[31].ToString(),
                            Modified1 = ds.Tables[0].Rows[index].ItemArray[32].ToString(),
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
                    //----------------Clearingtype-------28-03-2018---------------
                    ViewBag.ctsnocts = new SelectList(af.ClearingType, "Code", "Name").ToList();

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

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL2 HttpGet Index- " + innerExcp });
            }
        }

        //public ActionResult getTiffImage(string httpwebimgpath = null)
        //{
        //    try
        //    {
        //        string bankCode = Session["BankCode"].ToString();
        //        if (bankCode == "")
        //        {
        //            return RedirectToAction("getTiffImageNew", new RouteValueDictionary(new { Controller = "OWL2", Action = "getTiffImageNew", httpwebimgpath = httpwebimgpath }));
        //        }

        //        string someUrl = httpwebimgpath;
        //        var webClient = new WebClient();

        //        byte[] imageBytes = webClient.DownloadData(someUrl);

        //        Stream streamactual = new MemoryStream(imageBytes);
        //        System.Drawing.Bitmap bmp = new Bitmap(streamactual);
        //        System.IO.MemoryStream stream = new System.IO.MemoryStream();
        //        bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
        //        stream.Position = 0;

        //        byte[] data = new byte[stream.Length];
        //        int lngth = (int)stream.Length;
        //        stream.Read(data, 0, lngth);
        //        stream.Close();

        //        string imageBase64Data = Convert.ToBase64String(data);
        //        Array.Clear(imageBytes, 0, imageBytes.Length);


        //        string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
        //        ViewBag.ImageData = imageDataURL;
        //    }
        //    catch (Exception e)
        //    {

        //        string message = "";
        //        string innerExcp = "";
        //        if (e.Message != null)
        //            message = e.Message.ToString();
        //        if (e.InnerException != null)
        //            innerExcp = e.InnerException.Message;

        //        string err = "OWL2Chq getTiffImg|" + message + "INNEREXP| " + innerExcp;

        //        //logger.Log(LogLevel.Error, "OWL2Chq getTiffImg|" + message + "INNEREXP| " + innerExcp, "Login Index-Msg");
        //        //logger.Log(LogLevel.Error, e.InnerException.StackTrace, "Login Index-Exception");

        //        return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });

        //        // return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL2 getTiffImage - " + innerExcp });
        //    }

        //    return PartialView("_getTiffImage");
        //    //return Json(imageDataURL, JsonRequestBehavior.AllowGet);

        //}

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
                actualpath = actualpath.Replace("\\\\", "\\");
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

        public ActionResult getTiffImageNew(string httpwebimgpath = null)
        {

            int custid = Convert.ToInt16(Session["CustomerID"]);
            //var destpath = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "DestinationDownloadFolder");
            var destpath = af.CustomerMasters.FirstOrDefault(a => a.Id == custid);

            //Owsr.L1VerificationName = l1result.LoginID;
            string destroot = destpath.PhysicalPath;
            logerror(destroot.ToString(), destroot.ToString() + " - > Customer MAster");
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

            //actualpath = destroot + "\\" + actualpath;
            actualpath = destroot + actualpath;
            actualpath = actualpath.Replace("\\\\", "\\");
            logerror(actualpath.ToString(), actualpath.ToString() + " - > Complete Path");
            //string v = "D:/iKloudPro-OwImagesDhan/20210913/3/1/2_008/1_Front.tif";
            //System.Drawing.Bitmap bmp = new Bitmap(v);

            //System.Drawing.Bitmap bmp = new Bitmap(actualpath);
            //System.IO.MemoryStream stream = new System.IO.MemoryStream();
            //bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            //stream.Position = 0;

            //byte[] data = new byte[stream.Length];
            //int lngth = (int)stream.Length;
            //stream.Read(data, 0, lngth);
            //stream.Close();

            //string imageBase64Data = Convert.ToBase64String(data);
            //Array.Clear(data, 0, data.Length);


            //string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
            //ViewBag.ImageData = imageDataURL;

            //logerror(imageDataURL.ToString(), imageDataURL.ToString() + " - > View");

            return PartialView("_getTiffImage");
            //return Json(imageDataURL, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetCMSAccount(string ac = null)
        {
            bool flg = false;
            var rest = (from c in af.CMS_BranchAccountMappings
                        where c.AccountNo == ac
                        select c).ToList();

            if (rest.Count != 0)
                flg = true;
            else
                flg = false;

            return Json(flg, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult GetCBSDtls(string ac = null, string strcbsdls = null, string strJoinHldrs = null, string callby = null, string payeename = null)
        {
            cbstetails model = new cbstetails();
            //logerror(ac.ToString(), ac.ToString() + " - > Entry - AccNo");
            try
            {
                //------------ Added on 2021-07-24 ------ By Anketadit Jamuar ----------- Begin
                //logerror(ac.ToString(), ac.ToString() + " - > Try Loop");
                string WebApiPermission = Session["WebApiUrlAccDtlsPermissionOw"].ToString().ToUpper();
                //logerror(WebApiPermission, WebApiPermission + " - > Permission");
                if (WebApiPermission == "Y" && ac != null)
                {
                    //logerror(ac.ToString(), ac.ToString() + " - > API function calling");
                    // model = GetCBSDetailsWithAPI(ac);
                    //logerror(model.cbsdls, model.cbsdls + " - > Model Cbsdls");
                    return PartialView("_GetCBSDtls", model);
                }
                //------------ Added on 2021-07-24 ------ By Anketadit Jamuar ----------- End
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

                        //OWpro.OWGetCBSAccInfoWithOutUpdate(ac, ref strcbsdls, ref strJoinHldrs);
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
                //if (e.InnerException != null)
                //    innerExcp = e.InnerException.Message;
                //er.ErrorMessage = message;
                // return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL2 HttpPost Index- " + innerExcp });
                return PartialView("Error", "An error occurred while procesing your request. Service Unavailable  !!!...");
            }
        }
        public PartialViewResult RejectReason(int id = 0)
        {

            //var rjrs = (from r in af.ItemReturnReasons
            //            select new RejectReason
            //            {
            //                Description = r.DESCRIPTION,
            //                ReasonCodeS = r.RETURN_REASON_CODE
            //            });
            //return PartialView("_RejectReason", rjrs);

            //-------------- Added on 17-06-2021 ---- by Aniketadit ----- Begin

            SqlDataAdapter adp = new SqlDataAdapter("SP_OwItemReturnReasons", con);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            adp.Fill(ds);
            var objectlst = new List<RejectReason>();
            var objectr = new RejectReason();
            RejectReason rr;

            int count = ds.Tables[0].Rows.Count;
            int index = 0;
            while (count > 0)
            {
                rr = new RejectReason
                {
                    ReasonCodeS = Convert.ToString((ds.Tables[0].Rows[index].ItemArray[0])),
                    Description = Convert.ToString((ds.Tables[0].Rows[index].ItemArray[1])),
                };
                objectlst.Add(rr);
                //objectr.ReasonCodeS = Convert.ToString((ds.Tables[0].Rows[0].ItemArray[0]));
                //objectr.Description = Convert.ToString((ds.Tables[0].Rows[0].ItemArray[1]));
                count = count - 1;
                index = index + 1;
            }

            return PartialView("_RejectReason", objectlst);

            //-------------- Added on 17-06-2021 ---- by Aniketadit ----- End

        }
        public PartialViewResult GetBankName(string bankcode = null)
        {
            if (bankcode != null && bankcode != "")
            {
                string tempbankcode = bankcode.Substring(3, 3);
                int bnkCust = Convert.ToInt16(Session["CustomerID"]);
                //var Banks = (from c in af.BankBranches
                //             from ct in af.CustomerMasters
                //             where c.GridID == ct.GridId && ct.Id == bnkCust && c.Bank_BankCode.Substring(3, 3) == tempbankcode//c.Bank_BankCode == bankcode
                //             select new { c.BankName }).FirstOrDefault();

                //vikram
                SqlDataAdapter adp = new SqlDataAdapter("SP_GetBankName", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@bnkCust", SqlDbType.Int).Value = bnkCust;
                //-------------Added on 17-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@tempbankcode", SqlDbType.NVarChar).Value = tempbankcode;

                DataTable dt = new DataTable();
                adp.Fill(dt);
                //var Banks = dt.Rows[0].ToString().Trim();
                //vikram


                //if (Banks != null)
                if (dt.Rows.Count > 0)
                {
                    //ViewBag.BankName = Banks.BankName;
                    ViewBag.BankName = dt.Rows[0].ItemArray[0].ToString().Trim();
                }
                else
                    ViewBag.BankName = null;
            }
            else
                ViewBag.BankName = null;

            //if (bankcode != null && bankcode != "")
            //{
            //    string tempbankcode = bankcode.Substring(3, 3);
            //    var Banks = (from c in af.Banks
            //                 where c.BankCode.Substring(3, 3) == tempbankcode
            //                 select new { c.BankName }).SingleOrDefault();
            //    if (Banks != null)
            //        ViewBag.BankName = Banks.BankName;
            //    else
            //        ViewBag.BankName = null;
            //}
            //else
            //    ViewBag.BankName = null;


            return PartialView("_Bankname");
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
        public PartialViewResult getOWlogs(int id)
        {
            try
            {
                var model = af.ActivityLogs.Where(l => l.RawDataId == id).OrderBy(l => l.TimeStamp).ToList();
                return PartialView("_OWActivitylogs", model);
            }
            catch (Exception e)
            {

                ErrorDisplay er = new ErrorDisplay();
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;
                er.ErrorMessage = message;
                // return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL2 HttpPost Index- " + innerExcp });
                return PartialView("Error", er);
            }

        }
        //-------------------------Added On 24/07/2017-----------------------For Slip Image-----------
        [HttpPost]
        public JsonResult slipImage(Int64 SlipId = 0)
        {
            string frontslipiage = null;
            var owL2 = af.L2Verification.Where(m => m.Id == SlipId).Select(m => m.FrontGreyImagePath);

            //if (owL2 != null)
            //    frontslipiage = owL2;
            //else
            //    frontslipiage = "Image Not Found!!";
            //var result = new { acstatus, strCbsClientsDetls, strJoinHldrs };
            var frontimg = new { owL2 };

            // OWpro.GetSlipImageVF(SlipId, "L2", ref frontslipiage);
            // var username = af.UserMasters.Where(m => m.ID == 1).FirstOrDefault().FirstName;
            return Json(frontimg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OWL2Chq(int id = 0, int DomainId = 0, string branchCode = null)
        {
            cbstetails cbsdtls = new cbstetails();
            //vikram forr web API
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            //vikram forr web API

            //Get token no. for API
            string NewApiCall = null;
            var OwApi = af.CommonSettings.FirstOrDefault(a => a.AppName == "CTSCONFIG1" && a.SettingName == "OWVerAPI")?.SettingValue;
            if (OwApi != null && OwApi != "")
            {
                NewApiCall = OwApi.ToString().ToUpper();
            }
            else
                NewApiCall = "N";

            ViewBag.NewApiCall = NewApiCall;
            //logerror("Calling CreateToken method start : ", "");
            //========= 1 uncomment For DBS Open start ==========
            if (NewApiCall == "Y")  // uncomment when deployed on bank
                Session["sToken"] = CreateToken();  // uncomment when deployed on bank
            //========= 1 uncomment For DBS Open end ==========

            ////Get token no. for API
            //logerror("Calling CreateToken method end : ", "");
            //logerror("sToken session value : ", Session["sToken"].ToString());


            int custid = Convert.ToInt16(Session["CustomerID"].ToString().Trim());

            var varMinAclen = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "ACFrom").SettingValue;
            var varMaxAclen = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "ACTo").SettingValue;

            int intMinAclen = Convert.ToInt32(varMinAclen);
            int intMaxAclen = Convert.ToInt32(varMaxAclen);


            ViewBag.MinAclen = intMinAclen;
            ViewBag.MaxAclen = intMaxAclen;


            //NRE for DBS
            Session["NR"] = "";
            cbsdtls.NREFlag = "NR";

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
                //vikram getting post date and stale date
                SqlDataAdapter adp1 = new SqlDataAdapter("OWPostStaleDates", con);
                adp1.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp1.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();
                adp1.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);

                DataTable dt = new DataTable();
                adp1.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    Session["sPostdate"] = dt.Rows[0]["PostDate"].ToString().Trim();
                    Session["sStaledate"] = dt.Rows[0]["StaleDate"].ToString().Trim();
                }

                //logerror("Calling OWPostStaleDates SP end : ", "");

                string VFType = "";
                if (id == 1)
                    VFType = "RNormal";
                else if (id == 2)
                    VFType = "RHold";
                else if (id == 3)
                    VFType = "BNormal";
                else if (id == 4)
                    VFType = "BHold";
                else if (id == 5)
                    VFType = "CDK";
                else if (id == 99)
                    VFType = "RNormal";
                else if (id == 98)
                    VFType = "RNormalHV";

                Session["VFType"] = VFType;


                SqlDataAdapter adp = new SqlDataAdapter("OWSelectCHQL2", con);
                //SqlDataAdapter adp = new SqlDataAdapter("OWSMBL2Verification", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                //adp.SelectCommand.Parameters.Add("@VFtype", SqlDbType.NVarChar).Value = "RNormal";
                adp.SelectCommand.Parameters.Add("@VFtype", SqlDbType.NVarChar).Value = Session["VFType"].ToString();
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();
                //-------------Added on 17-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                //-------------Added on 18-05-2017---------------------------
                if (id == 5)
                {
                    adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = DomainId;
                    Session["DomainselectID"] = DomainId;
                    adp.SelectCommand.Parameters.Add("@BranchCode", SqlDbType.NVarChar).Value = branchCode;

                }
                else
                    adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);

                if (branchCode != null)
                    Session["BranchCode"] = branchCode;
                else
                    Session["BranchCode"] = "Dummy";
                //-------------Added on 12-09-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = Session["CtsSessionType"];



                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<L2verificationModel>();
                L2verificationModel def;
                //logerror("Count of table : ", ds.Tables[0].Rows.Count.ToString());
                if (ds.Tables[0].Rows.Count > 0)
                {

                    //ViewBag.vbFrontTiffImage = ds.Tables[0].Rows[0]["FrontTiffImagePath"].ToString();
                    //ViewBag.vbFrontGreyImage = ds.Tables[0].Rows[0]["FrontGreyImagePath"].ToString();
                    //ViewBag.vbBackTiffImage = ds.Tables[0].Rows[0]["BackTiffImagePath"].ToString();

                    def = new L2verificationModel
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
                        DomainId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[16].ToString()),
                        CustomerId = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[18].ToString()),
                        ScanningType = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[19].ToString()),
                        L1VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[20].ToString()),
                        L1UserId = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[21].ToString()),
                        L1RejectReason = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[22].ToString()),
                        PayeeName = ds.Tables[0].Rows[0].ItemArray[23].ToString(),
                        CBSAccountInformation = ds.Tables[0].Rows[0].ItemArray[24].ToString(),
                        CBSJointAccountInformation = ds.Tables[0].Rows[0].ItemArray[25].ToString(),
                        UserNarration = ds.Tables[0].Rows[0].ItemArray[26].ToString(),
                        SlipUserNarration = ds.Tables[0].Rows[0].ItemArray[26].ToString(),
                        RejectReasonDescription = ds.Tables[0].Rows[0].ItemArray[27].ToString(),
                        FinalAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[28]),
                        FinalDate = ds.Tables[0].Rows[0].ItemArray[29].ToString(),
                        ChequeNoFinal = ds.Tables[0].Rows[0].ItemArray[30].ToString(),
                        SortCodeFinal = ds.Tables[0].Rows[0].ItemArray[31].ToString(),
                        SANFinal = ds.Tables[0].Rows[0].ItemArray[32].ToString(),
                        TransCodeFinal = ds.Tables[0].Rows[0].ItemArray[33].ToString(),
                        DocType = ds.Tables[0].Rows[0].ItemArray[34].ToString(),
                        Modified1 = ds.Tables[0].Rows[0].ItemArray[35].ToString(),
                        FrontUVImagePath = ds.Tables[0].Rows[0]["FrontUVImage"].ToString(),
                        DraweeName = ds.Tables[0].Rows[0]["DraweeName"].ToString(),
                        NRESourceOfFundId = Convert.ToInt32(ds.Tables[0].Rows[0]["NRESourceOfFundId"]),
                        NROSourceOfFundId = Convert.ToInt32(ds.Tables[0].Rows[0]["NROSourceOfFundId"]),
                        callby = "Cheq",
                    };

                    //Vikram 
                    if (ds.Tables[0].Rows.Count > 0)
                        ViewBag.vbDomainId = ds.Tables[0].Rows[0]["BranchCode"].ToString().Trim();
                    else
                        ViewBag.vbDomainId = "";

                    objectlst.Add(def);
                    //------------------------END------------------------//

                    //logerror("Count of table end : ", ds.Tables[0].Rows.Count.ToString());
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    //count = count - 1;
                    while (count > 0)
                    {
                        def = new L2verificationModel
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
                            DomainId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[16].ToString()),
                            CustomerId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[18].ToString()),
                            ScanningType = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[19].ToString()),
                            L1VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[20].ToString()),
                            L1UserId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[21].ToString()),
                            L1RejectReason = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[22].ToString()),
                            PayeeName = ds.Tables[0].Rows[index].ItemArray[23].ToString(),
                            CBSAccountInformation = ds.Tables[0].Rows[index].ItemArray[24].ToString(),
                            CBSJointAccountInformation = ds.Tables[0].Rows[index].ItemArray[25].ToString(),
                            UserNarration = ds.Tables[0].Rows[index].ItemArray[26].ToString(),
                            SlipUserNarration = ds.Tables[0].Rows[index].ItemArray[26].ToString(),
                            RejectReasonDescription = ds.Tables[0].Rows[index].ItemArray[27].ToString(),
                            FinalAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[28]),
                            FinalDate = ds.Tables[0].Rows[index].ItemArray[29].ToString(),
                            ChequeNoFinal = ds.Tables[0].Rows[index].ItemArray[30].ToString(),
                            SortCodeFinal = ds.Tables[0].Rows[index].ItemArray[31].ToString(),
                            SANFinal = ds.Tables[0].Rows[index].ItemArray[32].ToString(),
                            TransCodeFinal = ds.Tables[0].Rows[index].ItemArray[33].ToString(),
                            DocType = ds.Tables[0].Rows[index].ItemArray[34].ToString(),
                            Modified1 = ds.Tables[0].Rows[index].ItemArray[35].ToString(),
                            FrontUVImagePath = ds.Tables[0].Rows[index]["FrontUVImage"].ToString(),
                            DraweeName = ds.Tables[0].Rows[index]["DraweeName"].ToString(),
                            NRESourceOfFundId = Convert.ToInt32(ds.Tables[0].Rows[index]["NRESourceOfFundId"]),
                            NROSourceOfFundId = Convert.ToInt32(ds.Tables[0].Rows[index]["NROSourceOfFundId"]),
                            callby = "Cheq",
                        };
                        //ViewBag.cnt = true;
                        objectlst.Add(def);

                        count = count - 1;
                        index = index + 1;
                    }
                    //logerror("Count of table adding end: ", ds.Tables[0].Rows.Count.ToString());
                    //For UV image above 200000
                    ViewBag.DefaultImage = "~/Icons/noimagefound.jpg";
                    ViewBag.FrontUV = ds.Tables[0].Rows[0]["FrontUVImage"].ToString().Trim();
                    ViewBag.FrontGrey = ds.Tables[0].Rows[0].ItemArray[7].ToString().Trim();

                    string sFinalAmount = ds.Tables[0].Rows[0]["FinalAmount"].ToString().Trim();

                    if (Convert.ToDecimal(sFinalAmount.Substring(0, sFinalAmount.Length - 3)) >= 200000)
                        ViewBag.DefaultImage = ViewBag.FrontUV;
                    else
                        ViewBag.DefaultImage = ViewBag.FrontGrey;
                    //For UV image above 200000

                    //var rtnlist = iwafl.ItemReturnReasons.Select(m => m.RETURN_REASON_CODE).ToList();
                    //ViewBag.rtnlist = rtnlist;

                    //-------------- Added on 09-07-2021 ---- by Aniketadit ----- Begin
                    //logerror("calling SP_OWItemReturnReasons : ", "");
                    SqlDataAdapter adpRej = new SqlDataAdapter("SP_OwItemReturnReasons", con);
                    adpRej.SelectCommand.CommandType = CommandType.StoredProcedure;

                    DataSet dsRej = new DataSet();
                    adpRej.Fill(dsRej);
                    var objectlstRej = new List<RejectReason>();
                    var objectr = new RejectReason();
                    RejectReason rr;

                    int countRej = dsRej.Tables[0].Rows.Count;
                    int indexRej = 0;
                    while (countRej > 0)
                    {
                        rr = new RejectReason
                        {
                            ReasonCodeS = Convert.ToString((dsRej.Tables[0].Rows[indexRej].ItemArray[0])),
                            Description = Convert.ToString((dsRej.Tables[0].Rows[indexRej].ItemArray[1])),
                        };
                        objectlstRej.Add(rr);
                        //objectr.ReasonCodeS = Convert.ToString((ds.Tables[0].Rows[0].ItemArray[0]));
                        //objectr.Description = Convert.ToString((ds.Tables[0].Rows[0].ItemArray[1]));
                        countRej = countRej - 1;
                        indexRej = indexRej + 1;
                    }

                    //return PartialView("_RejectReason", objectlst);

                    ViewBag.rtnlist = objectlstRej.Select(m => m.ReasonCodeS).ToList();
                    ViewBag.rtnlistDescrp = objectlstRej.Select(m => m.Description).ToList();
                    //logerror("calling SP_OWItemReturnReasons end : ", "");
                    //-------------- Added on 09-07-2021 ---- by Aniketadit ----- End

                    //var rtnlist = (from i in af.ItemReturnReasons select i).ToList();// iwafl.ItemReturnReasons.Select(m).ToList();
                    //ViewBag.rtnlist = rtnlist.Select(m => m.RETURN_REASON_CODE).ToList();
                    //ViewBag.rtnlistDescrp = rtnlist.Select(m => m.DESCRIPTION).ToList();
                    //-------------------------------For Narration Accounts---------------------
                    ViewBag.narration = (from n in af.NarrationAccount where n.status == 1 select n.AccountNumber).ToList();
                    //-------------------------------For SchemCode---------------------
                    ViewBag.SchemCode = (from n in af.NreNroAccountList where n.IsActive == true select n.SchemCode).ToList();
                    @Session["glob"] = null;
                    ViewBag.cnt = true;
                    //logerror("Fetching L2 Record method end : ", ""); 
                    //account NO. length
                    // Session["acto"] = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == Convert.ToInt16(Session["CustomerID"]) && p.SettingName == "ACFrom").SettingValue;
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

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL2 HttpGet Index- " + innerExcp });
            }
            finally
            {
                con.Close();
            }

        }

        [HttpPost]
        public ActionResult OWL2Chq(List<string> lst, bool snd, string img = null, string btnClose = "default", List<Int64> idlst = null, double ChequeAmountTotal = 0)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["VF"] == false)
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
                    ttcnt = lst.Count() / 45;
                //ttcnt = lst.Count() / 44;
                //ttcnt = lst.Count() / 43;

                int stu;
                int resncode = 0;
                string rejctdecrptn = null;
                string cbdclnts = "";
                string cbdJointdtls = "";
                byte rejct = 0;
                string modaction = "";
                string userNarration = "";
                string Clearingtype = "";
                bool mark2pf = false;
                bool ignoreIQA = false;
                string DocType = "B";
                string rejectreasondescrpsn = "";
                string instrumenttype = "";
                int ScanningType = 0;
                Int64 SlipID = 0;
                Int64 SlipRawaDataID = 0;
                string finalmodified = "";
                DataSet ds = new DataSet();
                ObjectClass os = new ObjectClass();
                L2verificationModel def;
                var objectlst = new List<L2verificationModel>();
                Int64 id = 0;
                if (btnClose == "Close" && lst != null)
                    ttcnt = ttcnt + 1;
                else if (btnClose == "Close" && lst == null)
                    goto Close;
                // string IWDicision = Request.Form["IWDecision"].ToUpper();
                //if (snd == false)
                instrumenttype = lst[5].ToString();
                //logerror("In Post method OWChqL2 start : ", "");
                //logerror("In Post method OWChqL2 ttcnt count - : ", ttcnt.ToString());
                //vikram
                //while (lst.Count > 0)
                //{
                string api_data = "";
                string isOpenedDateOld = "";

                if (lst[5].ToString() == "S")
                {
                    if (ttcnt == 1)
                    {
                        //for (int i = 0; i < ttcnt; i++)
                        //{
                        if (lst[12].ToString() == "A")
                        {
                            //objectlst = os.selectL2Cheques(con, uid, Session["LoginID"].ToString(), lst, Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), img, "Slip", false, Convert.ToInt16(Session["CustomerID"]), Convert.ToInt32(Session["DomainselectID"]), Session["SlipOnlyAccept"].ToString(), Convert.ToDouble(Session["SlipOnlyAcceptAmtThreshold"]), Session["VFType"].ToString(), Session["CtsSessionType"].ToString());

                        }
                        else if (lst[12].ToString() == "F")
                        {

                            id = Convert.ToInt64(lst[0]);

                            //OWpro.UpdateOWL2(id, Convert.ToInt64(lst[14]), uid, lst[5].ToString(), Convert.ToDouble(lst[2].ToString()), null, null, null, null, null, lst[1].ToString(),
                            //    lst[27].ToString(), Convert.ToInt16(lst[13]), null, "RF", @Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"),
                            //    Convert.ToInt16(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Convert.ToInt32(lst[9].ToString()), null, Convert.ToDouble(lst[2].ToString()), null, userNarration, null, null, Session["sNREFlag"].ToString().Trim(), Session["sacct_status"].ToString().Trim(), null, null, null, "",null,0,0);


                            //OWpro.UpdateOWL2(id, Convert.ToInt64(lst[14]), uid, lst[5].ToString(), Convert.ToDouble(lst[2].ToString()), null, null, null, null, null, lst[1].ToString(),
                            //        lst[27].ToString(), Convert.ToInt16(lst[13]), null, "RF", @Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"),
                            //        Convert.ToInt16(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Convert.ToInt32(lst[9].ToString()), null, Convert.ToDouble(lst[2].ToString()), null, userNarration, null, null, null,null, null, null, null, "");

                            //objectlst = os.selectL2Cheques(con, uid, Session["LoginID"].ToString(), lst, Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), img, "Slip", true, Convert.ToInt16(Session["CustomerID"]), Convert.ToInt32(Session["DomainselectID"]), Session["SlipOnlyAccept"].ToString(), Convert.ToDouble(Session["SlipOnlyAcceptAmtThreshold"]), Session["VFType"].ToString(), Session["CtsSessionType"].ToString());

                        }
                        else if (lst[12].ToString() == "R")
                        {
                            if (lst[15] != null)
                                rejct = Convert.ToByte(lst[15].ToString());
                            if (lst[32] != null)
                                userNarration = lst[32].ToString();
                            if (rejct == 88)
                            {
                                if (lst[33] != null)
                                    rejectreasondescrpsn = lst[33].ToString();
                                else
                                    rejectreasondescrpsn = "Other Reason";
                            }

                            //---------------Added On 21/06/2017------------------
                            if (lst[36] != null)
                                SlipID = Convert.ToInt64(lst[36]);
                            if (lst[37] != null)
                                SlipRawaDataID = Convert.ToInt64(lst[37]);
                            //---------------Added on 14/07/2017----------------
                            if (lst[38] != null)
                                ScanningType = Convert.ToInt16(lst[38]);

                            if (lst[39] != null)
                                finalmodified = lst[39].ToString();


                            //OWpro.UpdateVerificationSlipOnly(Convert.ToInt64(lst[14]), uid, Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt32(lst[3].ToString()), Convert.ToInt32(lst[6].ToString()), Convert.ToInt32(lst[9].ToString()), lst[7].ToString(),
                            //          Convert.ToDouble(lst[2].ToString()), lst[1].ToString(), lst[27].ToString(), lst[18].ToString(), lst[19].ToString(), "L2R", rejct, null, userNarration, rejectreasondescrpsn,
                            //          Convert.ToInt16(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Session["LoginID"].ToString(), SlipID, SlipRawaDataID, Session["SlipOnlyAccept"].ToString(), ScanningType, finalmodified, "");

                            //OWpro.UpdateVerificationSlipOnly(Convert.ToInt64(lst[14]), uid, Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt32(lst[3].ToString()), Convert.ToInt32(lst[6].ToString()), Convert.ToInt32(lst[9].ToString()), lst[7].ToString(),
                            //Convert.ToDouble(lst[2].ToString()), lst[1].ToString(), lst[27].ToString(), Session["sNREFlag"].ToString().Trim(), Session["sacct_status"].ToString().Trim(), "L2R", rejct, null, userNarration, rejectreasondescrpsn,
                            //Convert.ToInt16(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Session["LoginID"].ToString(), SlipID, SlipRawaDataID, Session["SlipOnlyAccept"].ToString(), ScanningType, finalmodified, "");



                            //-----------------------------------Commented On 17-01-2017--------------------------
                            //id = Convert.ToInt64(lst[0]);
                            //OWpro.UpdateOWL2(id, Convert.ToInt64(lst[14]), uid, lst[5].ToString(), Convert.ToDouble(lst[2].ToString()), null, null, null, null, null, lst[1].ToString(),
                            //    null, Convert.ToInt16(lst[13]), rejct, lst[12].ToString(), @Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"),
                            //    Convert.ToInt16(lst[17].ToString()), Convert.ToInt16(lst[16].ToString()), Convert.ToInt16(lst[9].ToString()), null, Convert.ToDouble(lst[2].ToString()), null);


                            ////------------Update Allcheques as rejected--------------
                            //SqlDataAdapter adp = new SqlDataAdapter("SelectOnlyIDForVF", con);
                            //adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                            //adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                            //adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                            //adp.SelectCommand.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = Convert.ToInt16(lst[3].ToString());
                            //adp.SelectCommand.Parameters.Add("@SlipNo", SqlDbType.NVarChar).Value = Convert.ToInt16(lst[6].ToString());
                            //adp.SelectCommand.Parameters.Add("@ScanningNodeId", SqlDbType.NVarChar).Value = Convert.ToInt16(lst[9].ToString());
                            //adp.SelectCommand.Parameters.Add("@BranchCode", SqlDbType.NVarChar).Value = lst[7].ToString();
                            //adp.SelectCommand.Parameters.Add("@modeule", SqlDbType.NVarChar).Value = "L2";
                            //ds = new DataSet();

                            //adp.Fill(ds);

                            //if (ds.Tables[0].Rows.Count > 0)
                            //{
                            //    for (int index = 0; index < ds.Tables[0].Rows.Count; index++)
                            //    {
                            //        OWpro.UpdateOWL2(Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]), Convert.ToInt64(lst[14]), uid, lst[5].ToString(), Convert.ToDouble(lst[2].ToString()), null, null, null, null, null, lst[1].ToString(),
                            //                                   null, Convert.ToInt16(lst[13]), rejct, lst[12].ToString(), @Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"),
                            //                                   Convert.ToInt16(lst[17].ToString()), Convert.ToInt16(lst[16].ToString()), Convert.ToInt16(lst[9].ToString()), null, Convert.ToDouble(lst[2].ToString()), null);
                            //    }


                            //}//-----------------------------------Commented On 17-01-2017-----------END---------------
                            //------------------------------
                            //objectlst = os.selectL2Cheques(con, uid, Session["LoginID"].ToString(), lst, Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), img, "Slip", true, Convert.ToInt16(Session["CustomerID"]), Convert.ToInt32(Session["DomainselectID"]), Session["SlipOnlyAccept"].ToString(), Convert.ToDouble(Session["SlipOnlyAcceptAmtThreshold"]), Session["VFType"].ToString(), Session["CtsSessionType"].ToString());
                        }
                        if (lst[12].ToString() == "H")
                        {

                            //---------------Added On 21/06/2017------------------
                            if (lst[36] != null)
                                SlipID = Convert.ToInt64(lst[36]);
                            if (lst[37] != null)
                                SlipRawaDataID = Convert.ToInt64(lst[37]);
                            //---------------Added on 14/07/2017----------------
                            if (lst[38] != null)
                                ScanningType = Convert.ToInt16(lst[38]);

                            if (lst[39] != null)
                                finalmodified = lst[39].ToString();

                            //OWpro.UpdateVerificationSlipOnly(Convert.ToInt64(lst[14]), uid, Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt32(lst[3].ToString()), Convert.ToInt32(lst[6].ToString()), Convert.ToInt32(lst[9].ToString()), lst[7].ToString(),
                            //           Convert.ToDouble(lst[2].ToString()), lst[1].ToString(), lst[27].ToString(), lst[18].ToString(), lst[19].ToString(), "L2H", rejct, null, userNarration, rejectreasondescrpsn,
                            //           Convert.ToInt16(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Session["LoginID"].ToString(), SlipID, SlipRawaDataID, Session["SlipOnlyAccept"].ToString(), ScanningType, finalmodified, "");

                            //OWpro.UpdateVerificationSlipOnly(Convert.ToInt64(lst[14]), uid, Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt32(lst[3].ToString()), Convert.ToInt32(lst[6].ToString()), Convert.ToInt32(lst[9].ToString()), lst[7].ToString(),
                            //Convert.ToDouble(lst[2].ToString()), lst[1].ToString(), lst[27].ToString(), Session["sNREFlag"].ToString().Trim(), Session["sacct_status"].ToString().Trim(), "L2H", rejct, null, userNarration, rejectreasondescrpsn,
                            //Convert.ToInt16(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Session["LoginID"].ToString(), SlipID, SlipRawaDataID, Session["SlipOnlyAccept"].ToString(), ScanningType, finalmodified, "");

                            //------------------------------
                            //objectlst = os.selectL2Cheques(con, uid, Session["LoginID"].ToString(), lst, Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), img, "Slip", true, Convert.ToInt16(Session["CustomerID"]), Convert.ToInt32(Session["DomainselectID"]), Session["SlipOnlyAccept"].ToString(), Convert.ToDouble(Session["SlipOnlyAcceptAmtThreshold"]), Session["VFType"].ToString(), Session["CtsSessionType"].ToString());

                        }

                    }
                }
                else if (lst[5].ToString() == "C")
                {
                    //logerror("In Post method OWChqL2 In C - : ", "");

                    string sNREFlag = "";
                    string sacct_status = "";

                    if (Session["sNREFlag"] != null)
                    {
                        sNREFlag = Session["sNREFlag"].ToString().Trim();
                    }
                    else
                    {
                        sNREFlag = "";
                    }

                    if (Session["sacct_status"] != null)
                    {
                        sacct_status = Session["sacct_status"].ToString().Trim();
                    }
                    else
                    {
                        sacct_status = "";
                    }

                    string finaldate = "";
                    if (ttcnt == 1)
                    {
                        //logerror("In Post method OWChqL2 In C In If - : ", "");
                        for (int i = 0; i < ttcnt; i++)
                        {
                            //logerror("In Post method OWChqL2 In C In If lst[0] - : ", lst[0]);
                            id = Convert.ToInt64(lst[0]);
                            //logerror("In Post method OWChqL2 In C In If id - : ", id.ToString());

                            //logerror("In Post method OWChqL2 In C In If lst[15] - : ", lst[15]);
                            if (lst[15] != null && lst[15].ToString() != "")
                                rejct = Convert.ToByte(lst[15].ToString());
                            //logerror("In Post method OWChqL2 In C In If rejct - : ", rejct.ToString());

                            //logerror("In Post method OWChqL2 In C In If lst[21] - : ", lst[21]);
                            if (lst[21] != null)
                            {
                                if (lst[21].ToString().Length != 10)
                                    finaldate = "20" + lst[21].ToString().Substring(4, 2) + "-" + lst[21].ToString().Substring(2, 2) + "-" + lst[21].ToString().Substring(0, 2);
                                else
                                    finaldate = lst[21].ToString();
                            }
                            //logerror("In Post method OWChqL2 In C In If finaldate - : ", finaldate);

                            //--------Modification Validation------------
                            //logerror("In Post method OWChqL2 In C In If lst[12] - : ", lst[12]);
                            //logerror("In Post method OWChqL2 In C In If lst[13] - : ", lst[13]);
                            //logerror("In Post method OWChqL2 In C In If lst[30] - : ", lst[30]);
                            //logerror("In Post method OWChqL2 In C In If lst[39] - : ", lst[39]);
                            //logerror("In Post method OWChqL2 In C In If lst[33] - : ", lst[33]);
                            if (lst[12].ToString() == "A")
                            {
                                if (lst[13].ToString() == "8")
                                    modaction = "A";
                                else
                                {
                                    if (Convert.ToBoolean(lst[30]) == true || Convert.ToInt64(lst[39].ToString().Trim()) > 0)
                                        modaction = "M";
                                    else
                                        modaction = "A";
                                }
                            }
                            else if (lst[12].ToString() == "R")
                            {
                                modaction = "R";
                                if (rejct == 88)
                                {
                                    if (lst[33] != null)
                                        rejectreasondescrpsn = lst[33].ToString();
                                    else
                                        rejectreasondescrpsn = "Other Reason";
                                }
                            }
                            //logerror("In Post method OWChqL2 In C In If modaction - : ", modaction);
                            //logerror("In Post method OWChqL2 In C In If rejectreasondescrpsn - : ", rejectreasondescrpsn);

                            //logerror("In Post method OWChqL2 In C In If lst[32] - : ", lst[32]);
                            if (lst[32] != null)
                                userNarration = lst[32].ToString();
                            //logerror("In Post method OWChqL2 In C In If userNarration - : ", userNarration);

                            //logerror("In Post method OWChqL2 In C In If lst[34] - : ", lst[34]);
                            if (lst[34] != null)
                                Clearingtype = lst[34].ToString();
                            //logerror("In Post method OWChqL2 In C In If Clearingtype - : ", Clearingtype);
                            //------------------marking P2F--------------------//
                            //logerror("In Post method OWChqL2 In C In If lst[35] - : ", lst[35]);
                            if (lst[35] != null)
                            {
                                mark2pf = Convert.ToBoolean(lst[35]);
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
                            //logerror("In Post method OWChqL2 In C In If mark2pf - : ", mark2pf.ToString());
                            //---------------Added On 21/06/2017------------------
                            //logerror("In Post method OWChqL2 In C In If lst[36] - : ", lst[36]);
                            if (lst[36] != null)
                                SlipID = Convert.ToInt64(lst[36]);
                            //logerror("In Post method OWChqL2 In C In If SlipID - : ", SlipID.ToString());
                            //logerror("In Post method OWChqL2 In C In If lst[37] - : ", lst[37]);
                            if (lst[37] != null)
                                SlipRawaDataID = Convert.ToInt64(lst[37]);
                            //logerror("In Post method OWChqL2 In C In If SlipRawaDataID - : ", SlipRawaDataID.ToString());
                            //---------------Added on 14/07/2017----------------
                            //logerror("In Post method OWChqL2 In C In If lst[38] - : ", lst[38]);
                            if (lst[38] != null)
                                ScanningType = Convert.ToInt16(lst[38]);
                            //logerror("In Post method OWChqL2 In C In If ScanningType - : ", ScanningType.ToString());
                            //logerror("In Post method OWChqL2 In C In If lst[39] - : ", lst[39]);
                            if (lst[39] != null)
                                finalmodified = lst[39].ToString();
                            //logerror("In Post method OWChqL2 In C In If finalmodified - : ", finalmodified.ToString());

                            //logerror("In Post method OWChqL2 In C In else Session sNREFlag - : ", Session["sNREFlag"].ToString().Trim());
                            //logerror("In Post method OWChqL2 In C In else Session sacct_status - : ", Session["sacct_status"].ToString().Trim());

                            //OWpro.UpdateOWL2(id, Convert.ToInt64(lst[14]), uid, lst[5].ToString(), Convert.ToDouble(lst[20].ToString()), finaldate, lst[22].ToString(), lst[23].ToString(), lst[24].ToString(), lst[25].ToString(), lst[1].ToString(),
                            //    lst[27].ToString(), Convert.ToInt16(lst[13]), rejct, modaction, @Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"),
                            //    Convert.ToInt16(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Convert.ToInt32(lst[9].ToString()), null, 0, null, userNarration,
                            //    rejectreasondescrpsn, Clearingtype, lst[18].ToString(), lst[19].ToString(), ignoreIQA, DocType, finalmodified, "");

                            //OWpro.UpdateOWL2(id, Convert.ToInt64(lst[14]), uid, lst[5].ToString(), Convert.ToDouble(lst[20].ToString()), finaldate, lst[22].ToString(), lst[23].ToString(), lst[24].ToString(), lst[25].ToString(), lst[1].ToString(),
                            //lst[27].ToString(), Convert.ToInt16(lst[13]), rejct, modaction, @Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"),
                            //Convert.ToInt16(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Convert.ToInt32(lst[9].ToString()), null, 0, null, userNarration,
                            //rejectreasondescrpsn, Clearingtype, Session["sNREFlag"].ToString().Trim(), Session["sacct_status"].ToString().Trim(), ignoreIQA, DocType, finalmodified, "", lst[40].ToString(), Convert.ToInt32(lst[41]), Convert.ToInt32(lst[42]));

                            api_data = lst[43] ?? "";

                            isOpenedDateOld = lst[44] ?? "";

                            SqlCommand cmd = new SqlCommand("UpdateOWL2", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID", id);
                            cmd.Parameters.AddWithValue("@RawDataId", Convert.ToInt64(lst[14]));
                            cmd.Parameters.AddWithValue("@Uid", uid);
                            cmd.Parameters.AddWithValue("@InstrumentType", lst[5].ToString());
                            cmd.Parameters.AddWithValue("@FinalAmount", Convert.ToDouble(lst[20].ToString()));
                            cmd.Parameters.AddWithValue("@FinalDate", finaldate);
                            cmd.Parameters.AddWithValue("@FinalChqNo", lst[22].ToString());
                            cmd.Parameters.AddWithValue("@FinalSortcode", lst[23].ToString());
                            cmd.Parameters.AddWithValue("@FinalSAN", lst[24].ToString());
                            cmd.Parameters.AddWithValue("@FinalTransCode", lst[25].ToString());
                            cmd.Parameters.AddWithValue("@CreditAccountNo", lst[1].ToString());
                            cmd.Parameters.AddWithValue("@PayeName", lst[27].ToString());
                            cmd.Parameters.AddWithValue("@status", Convert.ToInt16(lst[13]));
                            cmd.Parameters.AddWithValue("@RejectReason", rejct);
                            cmd.Parameters.AddWithValue("@ActionTaken", modaction);
                            cmd.Parameters.AddWithValue("@LName", @Session["LoginID"].ToString());
                            cmd.Parameters.AddWithValue("@ProcessingDate", Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@CustomerId", Convert.ToInt16(lst[17].ToString()));
                            cmd.Parameters.AddWithValue("@DomainId", Convert.ToInt32(lst[16].ToString()));
                            cmd.Parameters.AddWithValue("@ScanningNodeId", Convert.ToInt32(lst[9].ToString()));
                            cmd.Parameters.AddWithValue("@ChequeAmtotal", null);
                            cmd.Parameters.AddWithValue("@SlipAmount", 0);
                            cmd.Parameters.AddWithValue("@ChequeTotal", null);
                            cmd.Parameters.AddWithValue("@UserNarration", userNarration);
                            cmd.Parameters.AddWithValue("@RejectReasonDescription", rejectreasondescrpsn);
                            cmd.Parameters.AddWithValue("@CTSNONCTS", Clearingtype);
                            //cmd.Parameters.AddWithValue("@CBSAccountInformation", Session["sNREFlag"].ToString().Trim());
                            //cmd.Parameters.AddWithValue("@CBSJointAccountInformation", Session["sacct_status"].ToString().Trim());

                            cmd.Parameters.AddWithValue("@CBSAccountInformation", sNREFlag);
                            cmd.Parameters.AddWithValue("@CBSJointAccountInformation", sacct_status);
                            cmd.Parameters.AddWithValue("@IgnoreIQA", ignoreIQA);
                            cmd.Parameters.AddWithValue("@DocType", DocType);
                            cmd.Parameters.AddWithValue("@Modified", finalmodified);
                            cmd.Parameters.AddWithValue("@strHoldReason", "");
                            cmd.Parameters.AddWithValue("@DraweeName", lst[40].ToString());
                            cmd.Parameters.AddWithValue("@NRESourceOfFundId", Convert.ToInt32(lst[41]));
                            cmd.Parameters.AddWithValue("@NROSourceOfFundId", Convert.ToInt32(lst[42]));

                            //============= Added by Amol on 29/02/2024 for handling HighValue L3 cheques start ===========
                            cmd.Parameters.AddWithValue("@VFTYPE", Session["VFType"].ToString());
                            //============= Added by Amol on 29/02/2024 for handling HighValue L3 cheques end ===========

                            //============= Added by Amol on 01/03/2024 for handling API details start ===========
                            cmd.Parameters.AddWithValue("@API_Data", api_data);
                            //============= Added by Amol on 29/02/2024 for handling API details end ===========

                            //============= Added by Amol on 21/03/2024 for handling API details start ===========
                            cmd.Parameters.AddWithValue("@IsOpenedDateOld", isOpenedDateOld);
                            //============= Added by Amol on 21/03/2024 for handling API details end ===========

                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();

                            //for (int k = 0; k < idlst.Count; k++)
                            //{
                            //    if (idlst[k] == id)
                            //        idlst.RemoveAt(k);
                            //}
                            //logerror("In Post method OWChqL2 In C In If Update record end - : ", "Successful");
                            for (int k = 0; k < idlst.Count; k++)
                            {
                                if (idlst[k] == id)
                                    idlst.RemoveAt(k);
                            }
                            //lst.RemoveRange(0, 43);
                            //lst.RemoveRange(0, 44);
                            lst.RemoveRange(0, 45);
                        }
                        //if (ChequeAmountTotal != Convert.ToDouble(lst[26]))
                        //{
                        //    OWpro.UpdateChequeAmountTotalL1(Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt32(lst[3].ToString()), Convert.ToInt32(lst[6].ToString()), Convert.ToInt32(lst[9].ToString()), lst[7].ToString(), ChequeAmountTotal, "L2");
                        //}
                        if (btnClose == "Close")
                            goto Close;
                    }
                    else
                    {
                        //logerror("In Post method OWChqL2 In C In else - : ", "");

                        for (int i = 0; i < ttcnt - 1; i++)
                        {
                            //logerror("In Post method OWChqL2 In C In else lst[0] - : ", lst[0]);
                            id = Convert.ToInt64(lst[0]);
                            //logerror("In Post method OWChqL2 In C In else id - : ", id.ToString());

                            //logerror("In Post method OWChqL2 In C In else lst[15] - : ", lst[15]);
                            if (lst[15] != null && lst[15].ToString() != "")
                                rejct = Convert.ToByte(lst[15].ToString());
                            //logerror("In Post method OWChqL2 In C In else rejct - : ", rejct.ToString());

                            //logerror("In Post method OWChqL2 In C In else lst[21] - : ", lst[21]);
                            if (lst[21] != null)
                            {
                                if (lst[21].ToString().Length != 10)
                                    finaldate = "20" + lst[21].ToString().Substring(4, 2) + "-" + lst[21].ToString().Substring(2, 2) + "-" + lst[21].ToString().Substring(0, 2);
                                else
                                    finaldate = lst[21].ToString();
                            }
                            //logerror("In Post method OWChqL2 In C In else finaldate - : ", finaldate.ToString());
                            //--------Modification Validation------------
                            //logerror("In Post method OWChqL2 In C In else lst[13] - : ", lst[13]);
                            //logerror("In Post method OWChqL2 In C In else lst[30] - : ", lst[30]);
                            //logerror("In Post method OWChqL2 In C In else lst[12] - : ", lst[12]);
                            //logerror("In Post method OWChqL2 In C In else lst[33] - : ", lst[33]);
                            if (lst[12].ToString() == "A")
                            {
                                if (lst[13].ToString() == "8")
                                    modaction = "A";
                                else
                                {
                                    if (Convert.ToBoolean(lst[30]) == true || Convert.ToInt64(lst[39].ToString().Trim()) > 0)
                                        modaction = "M";
                                    else
                                        modaction = "A";
                                }
                            }
                            else if (lst[12].ToString() == "R")
                            {
                                modaction = "R";
                                if (rejct == 88)
                                {
                                    if (lst[33] != null)
                                        rejectreasondescrpsn = lst[33].ToString();
                                    else
                                        rejectreasondescrpsn = "Other Reason";
                                }
                            }
                            //logerror("In Post method OWChqL2 In C In else modaction - : ", modaction.ToString());
                            //logerror("In Post method OWChqL2 In C In else rejectreasondescrpsn - : ", rejectreasondescrpsn.ToString());

                            //logerror("In Post method OWChqL2 In C In else lst[32] - : ", lst[32]);
                            if (lst[32] != null)
                                userNarration = lst[32].ToString();
                            //logerror("In Post method OWChqL2 In C In else userNarration - : ", userNarration.ToString());

                            //logerror("In Post method OWChqL2 In C In else lst[34] - : ", lst[34]);
                            if (lst[34] != null)
                                Clearingtype = lst[34].ToString();
                            //logerror("In Post method OWChqL2 In C In else Clearingtype - : ", Clearingtype.ToString());

                            //-----------------Marking P2F----------------------//
                            //logerror("In Post method OWChqL2 In C In else lst[35] - : ", lst[35]);
                            if (lst[35] != null)
                            {
                                mark2pf = Convert.ToBoolean(lst[35]);
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
                            //logerror("In Post method OWChqL2 In C In else mark2pf - : ", mark2pf.ToString());
                            //---------------Added On 21/06/2017------------------
                            //logerror("In Post method OWChqL2 In C In else lst[36] - : ", lst[36]);
                            if (lst[36] != null)
                                SlipID = Convert.ToInt64(lst[36]);
                            //logerror("In Post method OWChqL2 In C In else SlipID - : ", SlipID.ToString());

                            //logerror("In Post method OWChqL2 In C In else lst[37] - : ", lst[37]);
                            if (lst[37] != null)
                                SlipRawaDataID = Convert.ToInt64(lst[37]);
                            //logerror("In Post method OWChqL2 In C In else SlipRawaDataID - : ", SlipRawaDataID.ToString());
                            //---------------Added on 14/07/2017----------------

                            //logerror("In Post method OWChqL2 In C In else lst[38] - : ", lst[38]);
                            if (lst[38] != null)
                                ScanningType = Convert.ToInt16(lst[38]);
                            //logerror("In Post method OWChqL2 In C In else ScanningType - : ", ScanningType.ToString());

                            //---------------Added on 14/07/2017----------------
                            //logerror("In Post method OWChqL2 In C In else lst[39] - : ", lst[0]);
                            if (lst[39] != null)
                                finalmodified = lst[39].ToString();
                            //logerror("In Post method OWChqL2 In C In else finalmodified - : ", finalmodified.ToString());

                            //logerror("In Post method OWChqL2 In C In else Session sNREFlag - : ", Session["sNREFlag"].ToString().Trim());
                            //logerror("In Post method OWChqL2 In C In else Session sacct_status - : ", Session["sacct_status"].ToString().Trim());
                            //OWpro.UpdateOWL2(id, Convert.ToInt64(lst[14]), uid, lst[5].ToString(), Convert.ToDouble(lst[20].ToString()), finaldate, lst[22].ToString(), lst[23].ToString(), lst[24].ToString(), lst[25].ToString(), lst[1].ToString(),
                            //    lst[27].ToString(), Convert.ToInt16(lst[13]), rejct, modaction, @Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"),
                            //    Convert.ToInt16(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Convert.ToInt32(lst[9].ToString()), null, 0, null, userNarration, rejectreasondescrpsn, Clearingtype, lst[18].ToString(), lst[19].ToString(), ignoreIQA, DocType, finalmodified, "");

                            //OWpro.UpdateOWL2(id, Convert.ToInt64(lst[14]), uid, lst[5].ToString(), Convert.ToDouble(lst[20].ToString()), finaldate, lst[22].ToString(), lst[23].ToString(), lst[24].ToString(), lst[25].ToString(), lst[1].ToString(),
                            //lst[27].ToString(), Convert.ToInt16(lst[13]), rejct, modaction, @Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"),
                            //Convert.ToInt16(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Convert.ToInt32(lst[9].ToString()), null, 0, null, userNarration, rejectreasondescrpsn, Clearingtype, Session["sNREFlag"].ToString().Trim(), Session["sacct_status"].ToString().Trim(), ignoreIQA, DocType, finalmodified, "", lst[40].ToString(), Convert.ToInt32(lst[41]), Convert.ToInt32(lst[42]));

                            api_data = lst[43] ?? "";

                            isOpenedDateOld = lst[44] ?? "";

                            SqlCommand cmd = new SqlCommand("UpdateOWL2", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID", id);
                            cmd.Parameters.AddWithValue("@RawDataId", Convert.ToInt64(lst[14]));
                            cmd.Parameters.AddWithValue("@Uid", uid);
                            cmd.Parameters.AddWithValue("@InstrumentType", lst[5].ToString());
                            cmd.Parameters.AddWithValue("@FinalAmount", Convert.ToDouble(lst[20].ToString()));
                            cmd.Parameters.AddWithValue("@FinalDate", finaldate);
                            cmd.Parameters.AddWithValue("@FinalChqNo", lst[22].ToString());
                            cmd.Parameters.AddWithValue("@FinalSortcode", lst[23].ToString());
                            cmd.Parameters.AddWithValue("@FinalSAN", lst[24].ToString());
                            cmd.Parameters.AddWithValue("@FinalTransCode", lst[25].ToString());
                            cmd.Parameters.AddWithValue("@CreditAccountNo", lst[1].ToString());
                            cmd.Parameters.AddWithValue("@PayeName", lst[27].ToString());
                            cmd.Parameters.AddWithValue("@status", Convert.ToInt16(lst[13]));
                            cmd.Parameters.AddWithValue("@RejectReason", rejct);
                            cmd.Parameters.AddWithValue("@ActionTaken", modaction);
                            cmd.Parameters.AddWithValue("@LName", @Session["LoginID"].ToString());
                            cmd.Parameters.AddWithValue("@ProcessingDate", Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@CustomerId", Convert.ToInt16(lst[17].ToString()));
                            cmd.Parameters.AddWithValue("@DomainId", Convert.ToInt32(lst[16].ToString()));
                            cmd.Parameters.AddWithValue("@ScanningNodeId", Convert.ToInt32(lst[9].ToString()));
                            cmd.Parameters.AddWithValue("@ChequeAmtotal", null);
                            cmd.Parameters.AddWithValue("@SlipAmount", 0);
                            cmd.Parameters.AddWithValue("@ChequeTotal", null);
                            cmd.Parameters.AddWithValue("@UserNarration", userNarration);
                            cmd.Parameters.AddWithValue("@RejectReasonDescription", rejectreasondescrpsn);
                            cmd.Parameters.AddWithValue("@CTSNONCTS", Clearingtype);
                            //cmd.Parameters.AddWithValue("@CBSAccountInformation", Session["sNREFlag"].ToString().Trim());
                            //cmd.Parameters.AddWithValue("@CBSJointAccountInformation", Session["sacct_status"].ToString().Trim());

                            cmd.Parameters.AddWithValue("@CBSAccountInformation", sNREFlag);
                            cmd.Parameters.AddWithValue("@CBSJointAccountInformation", sacct_status);
                            cmd.Parameters.AddWithValue("@IgnoreIQA", ignoreIQA);
                            cmd.Parameters.AddWithValue("@DocType", DocType);
                            cmd.Parameters.AddWithValue("@Modified", finalmodified);
                            cmd.Parameters.AddWithValue("@strHoldReason", "");
                            cmd.Parameters.AddWithValue("@DraweeName", lst[40].ToString());
                            cmd.Parameters.AddWithValue("@NRESourceOfFundId", Convert.ToInt32(lst[41]));
                            cmd.Parameters.AddWithValue("@NROSourceOfFundId", Convert.ToInt32(lst[42]));

                            //============= Added by Amol on 29/02/2024 for handling HighValue L3 cheques start ===========
                            cmd.Parameters.AddWithValue("@VFTYPE", Session["VFType"].ToString());
                            //============= Added by Amol on 29/02/2024 for handling HighValue L3 cheques end ===========

                            //============= Added by Amol on 01/03/2024 for handling API details start ===========
                            cmd.Parameters.AddWithValue("@API_Data", api_data);
                            //============= Added by Amol on 29/02/2024 for handling API details end ===========

                            //============= Added by Amol on 21/03/2024 for handling API details start ===========
                            cmd.Parameters.AddWithValue("@IsOpenedDateOld", isOpenedDateOld);
                            //============= Added by Amol on 21/03/2024 for handling API details end ===========

                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();

                            //OWpro.UpdateOWL2(id, Convert.ToInt64(lst[14]), uid, lst[5].ToString(), Convert.ToDouble(lst[20].ToString()), finaldate, lst[22].ToString(), lst[23].ToString(), lst[24].ToString(), lst[25].ToString(), lst[1].ToString(),
                            //    lst[27].ToString(), Convert.ToInt16("2"), rejct, modaction, @Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"),
                            //    Convert.ToInt16(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Convert.ToInt32(lst[9].ToString()), null, 0, null, userNarration, rejectreasondescrpsn, Clearingtype, lst[18].ToString(), lst[19].ToString(), ignoreIQA, DocType, finalmodified, "");
                            //logerror("In Post method OWChqL2 In C In else Update Record - : ", "Successful");
                            for (int k = 0; k < idlst.Count; k++)
                            {
                                if (idlst[k] == id)
                                    idlst.RemoveAt(k);
                            }
                            //lst.RemoveRange(0, 43);

                            //lst.RemoveRange(0, 44);
                            lst.RemoveRange(0, 45);

                            //if (ChequeAmountTotal != Convert.ToDouble(lst[26]))
                            //{
                            //    OWpro.UpdateChequeAmountTotalL1(Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt32(lst[3].ToString()), Convert.ToInt32(lst[6].ToString()), Convert.ToInt32(lst[9].ToString()), lst[7].ToString(), ChequeAmountTotal, "L2");
                            //}
                        }

                        if (btnClose == "Close")
                            goto Close;


                    }
                    //if (lst.Count > 0)
                    objectlst = os.selectL2ChequesOnly(con, uid, Session["LoginID"].ToString(), lst, Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), img, "Cheq", false, Convert.ToInt16(Session["CustomerID"]), Convert.ToInt32(Session["DomainselectID"]), null, 0, Session["VFType"].ToString(), Session["CtsSessionType"].ToString(), Session["VFType"].ToString());
                    //else
                    //objectlst = null;
                    //logerror("In Post method OWChqL2 In C In objectlst - : ", objectlst.Count.ToString());
                }
            //}

            //---------------------------IF Close button called--------------------------------//
            Close:
                if (btnClose == "Close")
                {
                    //logerror("In Post method OWChqL2 In Close - : ", " Close");
                    @Session["glob"] = true;
                    for (int p = 0; p < idlst.Count; p++)
                    {
                        //OWpro.OWUnlockRecords(idlst[p], "L2CDK");

                        SqlCommand com = new SqlCommand("OWUnlockRecords", con);
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@id", idlst[p]);

                        if (Session["VFType"].ToString() == "RNormalHV")
                        {
                            com.Parameters.AddWithValue("@module", "L3");
                        }
                        else
                        {
                            com.Parameters.AddWithValue("@module", "L2");
                        }
                        

                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();

                        //logerror("In Post method OWChqL2 In Close Release Record end - : ", " Close end");
                    }
                    //if (instrumenttype == "C")
                    //    OWpro.OWUnlockRecords(SlipID, "L2");

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

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL2 HttpPost Index- " + innerExcp });
            }
            finally
            {
                con.Close();
            }

        }
        public ActionResult getCDKlist(int? page, int id = 0)
        {
            //
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


                SqlDataAdapter adp = new SqlDataAdapter("GetListOfCDK", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();

                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);

                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);

                adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = Session["CtsSessionType"];

                adp.SelectCommand.Parameters.Add("@VFTYPE", SqlDbType.NVarChar).Value = "L2";


                DataSet ds = new DataSet();
                adp.Fill(ds);

                CdkList cdkList = new CdkList();

                int pageSize = 30;
                int pageIndex = 1;
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
                // IPagedList<ReleasePagelist> relhold = null;
                List<CdkList> objectlst = new List<CdkList>();

                IPagedList<CdkList> listrelease = null;

                int totalCount = 0;
                int cdkindex = 0;
                totalCount = ds.Tables[0].Rows.Count;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        objectlst.Add(
                            new CdkList
                            {
                                DomainName = ds.Tables[0].Rows[cdkindex].ItemArray[0].ToString(),
                                BranchCode = ds.Tables[0].Rows[cdkindex].ItemArray[1].ToString(),
                                BranchName = ds.Tables[0].Rows[cdkindex].ItemArray[2].ToString(),
                                CDKName = ds.Tables[0].Rows[cdkindex].ItemArray[3].ToString(),
                                Count = Convert.ToInt32(ds.Tables[0].Rows[cdkindex].ItemArray[5].ToString()),
                                CustomerID = Convert.ToInt16(Session["CustomerID"]),
                                DomainId = Convert.ToInt32(ds.Tables[0].Rows[cdkindex].ItemArray[4].ToString()),
                                indexid = id,
                            }
                            );
                        cdkindex = cdkindex + 1;
                    }
                    listrelease = objectlst.ToPagedList(pageIndex, pageSize);
                    //while (totalCount > 0)
                    //{
                    //    cdkList.DomainName = ds.Tables[0].Rows[cdkindex].ItemArray[0].ToString();
                    //    cdkList.BranchCode = ds.Tables[0].Rows[cdkindex].ItemArray[1].ToString();
                    //    cdkList.BranchName = ds.Tables[0].Rows[cdkindex].ItemArray[2].ToString();
                    //    cdkList.CDKName = ds.Tables[0].Rows[cdkindex].ItemArray[3].ToString();
                    //    cdkList.Count = Convert.ToInt32(ds.Tables[0].Rows[cdkindex].ItemArray[5].ToString());
                    //    cdkList.CustomerID = Convert.ToInt16(Session["CustomerID"]);
                    //    cdkList.DomainId = Convert.ToInt32(ds.Tables[0].Rows[cdkindex].ItemArray[4].ToString());

                    //    totalCount = totalCount - 1;
                    //    cdkindex = cdkindex + 1;
                    //}

                    return View(listrelease);
                }
                else
                {
                    return RedirectToAction("IWIndex", "Home", new { id = 1 });
                }

            }
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL2 getCDKList - " + innerExcp });
            }

        }

        //------------ Added on 2021-07-29 ------ By Anketadit Jamuar ----------- to get cbsdtls using .asmx service
        //public cbstetails GetCBSDetailsWithAPI(string ac = null)
        //{
        //    cbstetails cbsdtls = new cbstetails();
        //    string act = "0001234567890";

        //    List<string> sNames = new List<string>();
        //    List<string> sNamesList = new List<string>();
        //    List<string> sAccNo = new List<string>();
        //    List<string> sBranchCode = new List<string>();
        //    List<string> sBranchName = new List<string>();
        //    List<string> sAccType = new List<string>();
        //    List<string> sStaffSNo = new List<string>();
        //    List<string> sAccStatusCode = new List<string>();
        //    List<string> sStatusDesc = new List<string>();
        //    List<string> sDateAccOpen = new List<string>();
        //    List<string> sAccAging = new List<string>();
        //    List<string> sStatus = new List<string>();

        //    string name = null;
        //    string accNo = null;
        //    string branchCode = null;
        //    string branchName = null;
        //    string accType = null;
        //    string staffSNo = null;
        //    string accStatusCode = null;
        //    string statusDesc = null;
        //    string dateAccOpen = null;
        //    string accAging = null;
        //    string status = null;

        //    try
        //    {
        //        AccountDetailsService.ServiceSoapClient _obj = new AccountDetailsService.ServiceSoapClient();
        //        AccountDetailsService.CustomerName objPro = new AccountDetailsService.CustomerName();

        //        objPro = _obj.GetPayeeName(ac);

        //        if (objPro.status == "Success")
        //        {
        //            cbsdtls.status = objPro.status.ToUpper();

        //            name = objPro.cust_full_name;
        //            sNamesList.Add(name);
        //            char ch = '/';
        //            int countN = name.Count(f => (f == ch));
        //            if (countN != 0)
        //            {
        //                string[] nameList = name.Split('/');
        //                for (int i = 0; i <= countN; i++)
        //                {
        //                    sNames.Add(nameList[i]);
        //                }
        //                cbsdtls.PayeeName = sNames;
        //            }
        //            else
        //            {
        //                sNames.Add(name);
        //                cbsdtls.PayeeName = sNames;
        //            }

        //            cbsdtls.PayeeNameList = sNamesList;
        //            cbsdtls.AccountStatus = objPro.acc_status_code;
        //            cbsdtls.AccountStatusDescrp = objPro.status_desc;
        //            cbsdtls.AccountOwnership = objPro.brnname;

        //            //cbsdtls.status = objPro.status.ToUpper();
        //            //name = objPro.cust_full_name;
        //            //sNames.Add(name);
        //            //cbsdtls.PayeeName = sNames;
        //            ////cbsdtls.AccountStatus = objPro.status_desc;
        //            //cbsdtls.AccountStatus = objPro.acc_status_code;
        //            //cbsdtls.AccountStatusDescrp = objPro.status_desc;
        //            //cbsdtls.AccountOwnership = objPro.brnname;
        //        }
        //        else
        //        {
        //            cbsdtls.status = "FAILURE";

        //            ////Test 
        //            //cbsdtls.status = "SUCCESS";
        //            //name = "Aniket Adit Jamuar";
        //            //string name1 = "Mansi Singh";
        //            //sNames.Add(name);
        //            //sNames.Add(name1);
        //            //cbsdtls.PayeeName = sNames;

        //            //sNamesList.Add(name);
        //            //sNamesList.Add(name1);
        //            //cbsdtls.PayeeNameList = sNamesList;

        //        }
        //        return cbsdtls;
        //    }
        //    catch (Exception ex)
        //    {
        //        cbsdtls.status = "FAILURE";

        //        ////Test 
        //        //cbsdtls.status = "SUCCESS";
        //        //name = "Aniket Adit Jamuar";
        //        //string name1 = "Mansi Singh";
        //        //sNames.Add(name);
        //        //sNames.Add(name1);
        //        //cbsdtls.PayeeName = sNames;

        //        //sNamesList.Add(name);
        //        //sNamesList.Add(name1);
        //        //cbsdtls.PayeeNameList = sNamesList;

        //        return cbsdtls;
        //    }
        //}
        public string ClosedAccount()
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
            sInputString = sInputString + "\"Code\":\"406\",";
            sInputString = sInputString + "\"Desc\":\"Failure\"";
            sInputString = sInputString + "},";
            sInputString = sInputString + "\"Body\": {";
            sInputString = sInputString + "\"Description\":\"Account is closed\"";
            sInputString = sInputString + "}";
            sInputString = sInputString + "}";
            sInputString = sInputString + "}";

            sResposne = sInputString.Trim();

            return sResposne;

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
        public string getAccountDetailsSIB(string sServiceUrl = null, string sClientId = null, string sClientSecretKey = null, string sSenderCode = null, string sSenderName = null, string sAccountNo = null)
        {
            string sResposne = "";
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            try
            {
                string sInputString = "";

                sInputString = "{";
                sInputString += "\"Request\": {";
                sInputString += "               \"Header\": {";
                sInputString += "                            \"Timestamp\": \"" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "\",";
                sInputString += "                            \"ChannelDetails\": {";
                sInputString += "                                                  \"ChannelID\": \"CRM\",";
                sInputString += "                                                  \"ChannelType\": \"WEB\",";
                sInputString += "                                                  \"ChannelSubClass\": \"Retail\",";
                sInputString += "                                                  \"BranchCode\": \"\",";
                sInputString += "                                                  \"ChannelCusHdr\": {}";
                sInputString += "                                                 }, ";
                sInputString += "                            \"DeviceDetails\": {";
                sInputString += "                                                \"DeviceID\": \"Device1\",";
                sInputString += "                                                \"IMEINumber\": \"\",";
                sInputString += "                                                \"ClientIP\": \"\",";
                sInputString += "                                                \"OS\": \"\",";
                sInputString += "                                                \"BrowserType\": \"\",";
                sInputString += "                                                \"MobileNumber\": \"\",";
                sInputString += "                                                \"GeoLocation\": {";
                sInputString += "                                                                  \"Latitude\": \"13.072090\",";
                sInputString += "                                                                  \"Longitude\": \"80.201859\"";
                sInputString += "                                                                  }";
                sInputString += "                                                }";
                sInputString += "                              },";
                sInputString += "";
                sInputString += "              \"Body\": {";
                sInputString += "                        \"UUID\": \"K" + sAccountNo + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "\","; //"FEBA_1512106219450\",";
                                                                                                                                           //sInputString += "			             \"merchantCode\": \"DMmwxBIZxzgqZAQHtLjyQmWgRLlMKzOTuZb\",";
                                                                                                                                           //sInputString += "                        \"sender_code\": \"DMmwxBIZxzgqZAQHtLjyQmWgRLlMKzOTuZb\",";
                                                                                                                                           //sInputString += "                        \"sender_name\": \"KORES_CLG_CENTRALIZATION\",";

                sInputString += "                        \"sender_code\": \"" + sSenderCode + "\",";
                sInputString += "                        \"sender_name\": \"" + sSenderName + "\",";
                sInputString += "                        \"channel_id\": \"CRM\",";
                sInputString += "                        \"acct_num\": \"" + sAccountNo + "\"";
                sInputString += "                         }";
                sInputString += "            }";
                sInputString += "      }";


                var httpWebRequest = (HttpWebRequest)WebRequest.Create(sServiceUrl);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("SIB-Client-Secret", sClientSecretKey); //"Q5sG7qB4sP2kI3cB4bB2dC5bI3hW0gK5cB8iV3kV6gY5eU8oV7");
                httpWebRequest.Headers.Add("SIB-Client-Id", sClientId);//"97aafbe1-0a88-4c49-986b-0e21025b4983");
                httpWebRequest.Headers.Add("GlobalTranID", "KOR" + DateTime.Now.ToString("yyyyMMddHHmmssfff"));

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
                sResposne = "{\"Request\":{\"Error\":\"Runtime Error While Sending the Request\"," +
                                          "\"Description\":\"" + Ex.Message
                                     + "\"}" +
                            "}";
            }


            return sResposne;


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
                //logerror("In GetToken method inside the Count : ", "");
                //sEtoken = dt.Rows[0]["SettingValue"].ToString().Trim();
                sEtoken = Session["sToken"].ToString().Trim();
                //logerror("In GetToken method inside the Count sEtoken value : ", sEtoken);
            }
            else
            {
                //logerror("In GetToken method inside the else : ", "");
                //logerror("In GetToken method calling sendCMCPTokenRequest method start : ", "");
                sTokenResponse = sendCMCPTokenRequest(TokenServiceURL, TokenClientId, TokenSecreteKey);
                //logerror("In GetToken method calling sendCMCPTokenRequest method end : ", "");
                //logerror("In GetToken method calling sTokenResponse value : ", sTokenResponse);

                //logerror("In GetToken method calling getCMCPToken method start : ", "");
                sEtoken = getCMCPToken(sTokenResponse);
                //logerror("In GetToken method calling getCMCPToken method end : ", "");
                //logerror("In GetToken method calling getCMCPToken method end sEtoken value : ", sEtoken);

                //Save new Token
                if (con.State == ConnectionState.Closed)
                    con.Open();

                //logerror("In GetToken method calling UpdateToken SP start : ", "");
                SqlCommand cmd = new SqlCommand("UpdateToken", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@SettingValue", sEtoken);
                int iExec = cmd.ExecuteNonQuery();
                //logerror("In GetToken method calling UpdateToken SP method end : ", "");
            }

            return sEtoken;
        }

        public string CreateToken()
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

            //logerror("TokenClientId : ", TokenClientId);
            //logerror("TokenSecreteKey : ", TokenSecreteKey);
            //logerror("TokenServiceURL : ", TokenServiceURL);

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
                sEtoken = dt.Rows[0]["SettingValue"].ToString().Trim();
            }
            else
            {
                sTokenResponse = sendCMCPTokenRequest(TokenServiceURL, TokenClientId, TokenSecreteKey);
                //logerror("sTokenResponse : ", sTokenResponse);

                sEtoken = getCMCPToken(sTokenResponse);

                //Save new Token
                if (con.State == ConnectionState.Closed)
                    con.Open();

                SqlCommand cmd = new SqlCommand("UpdateToken", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@SettingValue", sEtoken);
                int iExec = cmd.ExecuteNonQuery();
            }


            //logerror("sEtoken : ", sEtoken);

            return sEtoken;



        }
        public ActionResult GetCBSDetailsWithAPI(string ac = null) //, List<string> lst=null
        {
            //logerror("Calling GetCBSDetailsWithAPI method start : ", "");
            cbstetails cbsdtls = new cbstetails();
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

                    if (NewApiCall == "Y")
                    {

                        string sBankNm = "DBS";
                        if (sBankNm == "DBS")
                        {
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
                            //logerror("Calling GetToken method start : ", "");
                            //=========== 2 uncomment when deployed on bank start ===============
                            //Get Token 
                            string sEtoken = GetToken();
                            //logerror("Calling GetToken method end : ", "");
                            //logerror("sEtoken value : ", sEtoken);
                            //=========== 2 uncomment when deployed on bank end ===============

                            ViewBag.vbNRE = "";
                            Session["sNR"] = "";
                            Session["sacct_status"] = "";
                            Session["sNREFlag"] = "";

                            ViewBag.Currency = "";
                            Session["SourceCustomerId"] = "";
                            Session["AccountCurrency"] = "";
                            Session["IsOpenedDateOld"] = "";
                            Session["productCode"] = "";
                            Session["productType"] = "";
                            Session["accountBalances"] = "0";

                            cbsdtls.Currency = "";
                            cbsdtls.ACBALAmount = "0";
                            cbsdtls.acOpenDate = "";

                            long openDate = 0;

                            if (ac.Trim() == "999999999999" || ac.Trim() == "9999999999999999")
                            {
                                cbsdtls.sCAPA = "Invalid Account";

                                cbsdtls.status = "SUCCESS";
                                cbsdtls.cbsdls = null;
                                cbsdtls.PayeeNameList = null;
                                cbsdtls.sInvalidAcFlag = "T";
                            }
                            else
                            {
                                //======= For Local Testing Open =================
                                //sgetAccountDetailsDBS = getAccountDetailsDBSResponse(CasaServiceURL, CasaClientId, CasaCorellationId, ac); //local testing

                                //sgetAccountDetailsDBS = getCustomerId(CasaServiceURL, CasaClientId, CasaCorellationId, ac);
                                //sgetAccountDetailsDBS = getAccountDetailsDBSInvalid(CasaServiceURL, CasaClientId, CasaCorellationId, ac);
                                //logerror("Calling getAccountDetailsDBSRequest method start : ", "");

                                //========= 3 uncomment For DBS Open start ==========
                                sgetAccountDetailsDBS = getAccountDetailsDBSRequest(CasaServiceURL, CasaClientId, CasaCorellationId, ac.ToUpper(), sEtoken); // dbs testing

                                //logerror("Calling getAccountDetailsDBSRequest method end : ", "");
                                //logerror("sgetAccountDetailsDBS : ", sgetAccountDetailsDBS);
                                var jObject = Newtonsoft.Json.Linq.JObject.Parse(sgetAccountDetailsDBS);

                                if (jObject["error"] != null)
                                {
                                    cbsdtls.sCAPA = "Invalid Account";
                                    cbsdtls.PayeeNameList = null;
                                    cbsdtls.status = "SUCCESS";
                                    cbsdtls.cbsdls = null;
                                    cbsdtls.sInvalidAcFlag = "T";
                                }
                                else
                                {
                                    if (jObject["accountClosedFlag"].ToString().Trim().ToUpper() != "Y")
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
                                            cbsdtls.Currency = jObject["accountCurrencyCode"].ToString().Trim();
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
                                                cbsdtls.acOpenDate = "Y";
                                            }
                                            else
                                            {
                                                //Console.WriteLine("The difference is not greater than six months.");
                                                Session["IsOpenedDateOld"] = "N";
                                                cbsdtls.acOpenDate = "N";
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
                                            cbsdtls.ACBALAmount = jObject["accountBalances"]["effectiveAvailableAmount"] == null ? "0" :
                                                                        jObject["accountBalances"]["effectiveAvailableAmount"].ToString().Trim();
                                        }
                                        else
                                        {
                                            Session["accountBalances"] = "0";
                                            cbsdtls.ACBALAmount = "0";
                                        }
                                        //=========== Amol changes on 27/02/2024 for capturing SourceCustomerId end ======
                                        if (jObject["accountStatus"] != null)
                                        {
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
                                                    cbsdtls.sInvalidAcFlag = "T";
                                                }
                                                else if (jObject["freezeStatusCode"].ToString().Trim() == "C")
                                                {
                                                    cbsdtls.sCAPA = "Account is Credit freeze";

                                                    //cbsdtls.PayeeNameList = null;
                                                    cbsdtls.sInvalidAcFlag = "T";
                                                }
                                                else if (jObject["freezeStatusCode"].ToString().Trim() == "D")
                                                {
                                                    cbsdtls.sCAPA = "Account is Debit freeze";

                                                    //cbsdtls.PayeeNameList = null;
                                                    cbsdtls.sInvalidAcFlag = "T";
                                                }

                                            }
                                            else if (jObject["accountStatus"].ToString().Trim() == "Dormant")
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
                                                }

                                                cbsdtls.sCAPA = "Account is dormant";
                                                cbsdtls.sInvalidAcFlag = "T";
                                            }
                                            else if (jObject["accountStatus"].ToString().Trim() == "Inactive")
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
                                                }

                                                cbsdtls.sCAPA = "Account is inactive";

                                                //cbsdtls.PayeeNameList = null;
                                                cbsdtls.sInvalidAcFlag = "T";
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
                                            cbsdtls.Currency = jObject["accountCurrencyCode"].ToString().Trim();
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
                                                cbsdtls.acOpenDate = "Y";
                                            }
                                            else
                                            {
                                                //Console.WriteLine("The difference is not greater than six months.");
                                                Session["IsOpenedDateOld"] = "N";
                                                cbsdtls.acOpenDate = "N";
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
                                            cbsdtls.ACBALAmount = jObject["accountBalances"]["effectiveAvailableAmount"] == null ? "0" :
                                                                        jObject["accountBalances"]["effectiveAvailableAmount"].ToString().Trim();
                                        }
                                        else
                                        {
                                            Session["accountBalances"] = "0";
                                            cbsdtls.ACBALAmount = "0";
                                        }
                                        //=========== Amol changes on 27/02/2024 for capturing SourceCustomerId end ======

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
                                        }

                                        cbsdtls.sCAPA = "Account is closed";
                                        //cbsdtls.PayeeNameList = null;
                                        cbsdtls.sInvalidAcFlag = "T";
                                    }
                                }

                            } // else of 999999999999 ac no.
                        }
                        else
                        {
                            string sgetAccountDetailsSIB = JointAccount();
                            // string sgetAccountDetailsSIB = ClosedAccount();

                            //string sgetAccountDetailsSIB = getAccountDetailsSIB(sServiceUrl, sClientId, sClientSecretKey, sSenderCode, sSenderName, ac);
                            var jObject = Newtonsoft.Json.Linq.JObject.Parse(sgetAccountDetailsSIB);

                            if (jObject["Response"] != null)
                            {

                                if (jObject["Response"]["Body"]["RelPartyRec"] != null)
                                {
                                    if (jObject["Response"]["Body"]["RelPartyRec"].Count() > 0)
                                    {
                                        lAccNames.Add(jObject["Response"]["Body"]["AcctName"].ToString().Trim());
                                        int iIndex = 0;
                                        while (iIndex < jObject["Response"]["Body"]["RelPartyRec"].Count())
                                        {
                                            if (jObject["Response"]["Body"]["AcctName"].ToString().Trim() != jObject["Response"]["Body"]["RelPartyRec"][iIndex]["CustId"]["PersonName"]["Name"].ToString().Trim())
                                                lAccNames.Add(jObject["Response"]["Body"]["RelPartyRec"][iIndex]["CustId"]["PersonName"]["Name"].ToString().Trim());

                                            iIndex++;
                                        }

                                        cbsdtls.PayeeNameList = lAccNames;
                                        cbsdtls.status = "SUCCESS";
                                        cbsdtls.cbsdls = null;
                                        //cbsdtls.AccountStatus = "6";


                                        //Data from WebAPI 

                                        cbsdtls.sSchmCode = jObject["Response"]["Body"]["AcctType"]["SchmCode"].ToString().Trim();

                                        cbsdtls.sFreezeStatusCode = jObject["Response"]["Body"]["FreezeStatusCode"].ToString().Trim();


                                        cbsdtls.sModeOfOper = jObject["Response"]["Body"]["ModeOfOper"].ToString().Trim();
                                        cbsdtls.sacct_status = jObject["Response"]["Body"]["acct_status"].ToString().Trim();
                                        //NRE - NOt found
                                        cbsdtls.sSchmType = jObject["Response"]["Body"]["AcctType"]["SchmType"].ToString().Trim();
                                        cbsdtls.sName = jObject["Response"]["Body"]["RelPartyRec"][0]["CustId"]["PersonName"]["Name"].ToString().Trim();
                                        cbsdtls.scustomerisMinor = jObject["Response"]["Body"]["customerisMinor"].ToString().Trim();

                                        if (jObject["Response"]["Body"]["Description"] != null)
                                        {
                                            cbsdtls.sClosedAccount = jObject["Response"]["Body"]["Description"].ToString().Trim();
                                            cbsdtls.sClosedAccount = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToUpper(cbsdtls.sClosedAccount.ToString().Trim());
                                        }
                                        cbsdtls.AccountStatus = cbsdtls.sacct_status;

                                        cbsdtls.sInvalid = jObject["Response"]["Status"]["Desc"].ToString().Trim();
                                        //Data from WebAPI 

                                        //For VAlidation
                                        cbsdtls.sInvalidAcFlag = "F";


                                        //if(Session["Accesslevel"].ToString().Trim()=="BRN")
                                        //{

                                        //}

                                        //Account Validation
                                        //int iAc1 = ac.IndexOf("256000000013", 1);
                                        //int iAc2 = ac.IndexOf("256000000026", 1);
                                        //int iAc3 = ac.IndexOf("256000000059", 1);

                                        // SIBFEE_COLL_SUSP
                                        int iAc4 = ac.IndexOf("256000000026", 1);
                                        if (iAc4 > 0)
                                            cbsdtls.sSIBFEE = "SIBFEE";
                                        else
                                            cbsdtls.sSIBFEE = "";

                                        //SIBCOLL CLEARING SUSPENSE ACCOUNT
                                        int iAc5 = ac.IndexOf("256000000059", 1);
                                        if (iAc5 > 0)
                                            cbsdtls.sSCSA = "SCSA";
                                        else
                                            cbsdtls.sSCSA = "";

                                        // NPS_COLLECTION_AC  
                                        int iAc6 = ac.IndexOf("253000000002", 1);
                                        if (iAc6 > 0)
                                            cbsdtls.sNCA = "NCA";
                                        else
                                            cbsdtls.sNCA = "";

                                        // NPSLITE_COLLECTION_AC  
                                        int iAc7 = ac.IndexOf("253000000003", 1);
                                        if (iAc7 > 0)
                                            cbsdtls.sNSCA = "NSCA";
                                        else
                                            cbsdtls.sNSCA = "";

                                        // TAX_DUTIES_PAYABLE_AC
                                        int iAc8 = ac.IndexOf("252000000040", 1);
                                        if (iAc8 > 0)
                                            cbsdtls.sTDPA = "TDPA";
                                        else
                                            cbsdtls.sTDPA = "";

                                        //FCRA Accounts
                                        int iAc9 = ac.IndexOf("05035", 1);
                                        if (iAc9 > 0)
                                            cbsdtls.sFCRA = "FCRA";
                                        else
                                            cbsdtls.sFCRA = "";

                                        int iAc10 = ac.IndexOf("05036", 1);
                                        if (iAc10 > 0)
                                            cbsdtls.sFCRA = "FCRA";
                                        else
                                            cbsdtls.sFCRA = "";

                                        int iAc11 = ac.IndexOf("05090", 1);
                                        if (iAc11 > 0)
                                            cbsdtls.sFCRA = "FCRA";
                                        else
                                            cbsdtls.sFCRA = "";

                                        int iAc12 = ac.IndexOf("07010", 1);
                                        if (iAc12 > 0)
                                            cbsdtls.sFCRA = "FCRA";
                                        else
                                            cbsdtls.sFCRA = "";

                                        int iAc13 = ac.IndexOf("07011", 1);
                                        if (iAc13 > 0)
                                            cbsdtls.sFCRA = "FCRA";
                                        else
                                            cbsdtls.sFCRA = "";


                                        // TATA AIG PREMIUM COLLECTION
                                        int iAc14 = ac.IndexOf("073000001582", 1);
                                        if (iAc14 > 0)
                                            cbsdtls.sTAPC = "TAPC";
                                        else
                                            cbsdtls.sTAPC = "";

                                        //CUSTOMS DUTY PAYMENT
                                        int iAc15 = ac.IndexOf("252000000040", 1);
                                        if (iAc15 > 0)
                                            cbsdtls.sCDP = "CDP";
                                        else
                                            cbsdtls.sCDP = "";

                                        // ICICI PREMIUM COLLECTION  
                                        int iAc16 = ac.IndexOf("073000001062", 1);
                                        if (iAc16 > 0)
                                            cbsdtls.sIPC = "IPC";
                                        else
                                            cbsdtls.sIPC = "";

                                        // RPMC
                                        int iAc17 = ac.IndexOf("073000000688", 1);
                                        if (iAc17 > 0)
                                            cbsdtls.sRPMC = "RPMC";
                                        else
                                            cbsdtls.sRPMC = "";

                                        // CM DISTRESS RELIEF FUND
                                        int iAc18 = ac.IndexOf("053000002584", 1);
                                        if (iAc18 > 0)
                                            cbsdtls.sCDRF = "CDRF";
                                        else
                                            cbsdtls.sCDRF = "";

                                        // CM DISTRESS RELIEF FUND-COVID19   
                                        int iAc19 = ac.IndexOf("053000003020", 1);
                                        if (iAc19 > 0)
                                            cbsdtls.sCDRFC = "CDRFC";
                                        else
                                            cbsdtls.sCDRFC = "";

                                        //   MANAPPURAM CHITS
                                        int iAc20 = ac.IndexOf("073000001896", 1);
                                        if (iAc20 > 0)
                                            cbsdtls.sMC = "MC";
                                        else
                                            cbsdtls.sMC = "";

                                        // PREPAID CARD
                                        int iAc21 = ac.IndexOf("259.239", 1);
                                        if (iAc21 > 0)
                                            cbsdtls.sPC = "PC";
                                        else
                                            cbsdtls.sPC = "";

                                        // KNRK REG FEE
                                        int iAc22 = ac.IndexOf("053000002844", 1);
                                        if (iAc22 > 0)
                                            cbsdtls.sKRF = "KRF";
                                        else
                                            cbsdtls.sKRF = "";

                                        // KNRK MONTH SUB 
                                        int iAc23 = ac.IndexOf("053000002845", 1);
                                        if (iAc23 > 0)
                                            cbsdtls.sKMS = "KMS";
                                        else
                                            cbsdtls.sKMS = "";

                                        // KNRK PENALTY
                                        int iAc24 = ac.IndexOf("053000002846", 1);
                                        if (iAc24 > 0)
                                            cbsdtls.sKP = "KP";
                                        else
                                            cbsdtls.sKP = "";

                                        // EXIDE LIFE PREMIUM COLLECTION
                                        int iAc25 = ac.IndexOf("073000001368", 1);
                                        if (iAc25 > 0)
                                            cbsdtls.sELPC = "ELPC";
                                        else
                                            cbsdtls.sELPC = "";

                                        // GURUVAYUR DEVASWOM DONATION
                                        int iAc26 = ac.IndexOf("053000012084", 1);
                                        if (iAc26 > 0)
                                            cbsdtls.sGDD = "GDD";
                                        else
                                            cbsdtls.sGDD = "";

                                        // NETC FASTAG
                                        int iAc27 = ac.IndexOf("252000000178", 1);
                                        if (iAc27 > 0)
                                            cbsdtls.sNF = "sNF";
                                        else
                                            cbsdtls.sNF = "";

                                        // OUSHADHI THRISSUR
                                        int iAc28 = ac.IndexOf("073000000213", 1);
                                        if (iAc28 > 0)
                                            cbsdtls.sOT = "OT";
                                        else
                                            cbsdtls.sOT = "";

                                        // OUSHADHI PATHANAPURAM
                                        int iAc29 = ac.IndexOf("073000000233", 1);
                                        if (iAc29 > 0)
                                            cbsdtls.sOP = "OP";
                                        else
                                            cbsdtls.sOP = "";

                                        // OUSHADHI PARIYARAM
                                        int iAc30 = ac.IndexOf("073000000181", 1);
                                        if (iAc30 > 0)
                                            cbsdtls.sOPR = "OPR";
                                        else
                                            cbsdtls.sOPR = "";


                                        //CLEARING ADJUSTMENT PAYABLE ACCOUNT
                                        int iAc31 = ac.IndexOf("250000000001", 1);
                                        if (iAc31 > 0)
                                            cbsdtls.sCAPA = "CAPA";
                                        else
                                            cbsdtls.sCAPA = "";

                                        //CLEARING ADJUSTMENT RECEIVABLE ACCOUNT
                                        int iAc32 = ac.IndexOf("250000000002", 1);
                                        if (iAc32 > 0)
                                            cbsdtls.sCARA = "CARA";
                                        else
                                            cbsdtls.sCARA = "";


                                        //OUTWARD CLG SUSP ACCOUNT
                                        int iAc33 = ac.IndexOf("256000000005", 1);
                                        if (iAc33 > 0)
                                            cbsdtls.sOCSA = "OCSA";
                                        else
                                            cbsdtls.sOCSA = "";


                                        //REALISATION SUSPENSE ACCOUNT
                                        int iAc34 = ac.IndexOf("256000000006", 1);
                                        if (iAc34 > 0)
                                            cbsdtls.sRSA = "RSA";
                                        else
                                            cbsdtls.sRSA = "";


                                        //O/W CLEARING RETURN SUSPENSE ACCOUNT
                                        int iAc35 = ac.IndexOf("256000000009", 1);
                                        if (iAc35 > 0)
                                            cbsdtls.sOCRSA = "OCRSA";
                                        else
                                            cbsdtls.sOCRSA = "";


                                        //RTGS-NEFT PARKING ACCOUNT
                                        int iAc36 = ac.IndexOf("256000000021", 1);
                                        if (iAc36 > 0)
                                            cbsdtls.sRNPA = "RNPA";
                                        else
                                            cbsdtls.sRNPA = "";

                                        //RTGS BANK INWARD STP PARKING ACCOUNT
                                        int iAc37 = ac.IndexOf("256000000022", 1);
                                        if (iAc37 > 0)
                                            cbsdtls.sRBISPA = "RBISPA";
                                        else
                                            cbsdtls.sRBISPA = "";



                                        //RO Sanction >10,00,00,000
                                        Double iAcctBalAmt = 0;
                                        if (jObject["Response"]["Body"]["AcctBalAmt"]["amountValue"].ToString().Trim() != "" || jObject["Response"]["Body"]["AcctBalAmt"]["amountValue"].ToString().Trim() != null)
                                            iAcctBalAmt = Convert.ToDouble(jObject["Response"]["Body"]["AcctBalAmt"]["amountValue"].ToString().Trim());


                                        //if (iAc4 > 0 || iAc5 > 0 || iAc6 > 0 || iAc7 > 0 || iAc8 > 0 || iAc9 > 0 || iAc10 > 0 || iAc11 > 0 || iAc12 > 0 || iAc13 > 0 || iAc14 > 0 || iAc15 > 0 || iAc16 > 0 || iAc17 > 0 || iAc18 > 0 || iAc19 > 0 || iAc20 > 0 || iAc21 > 0 || iAc22 > 0 || iAc23 > 0 || iAc24 > 0 || iAc25 > 0 || iAc26 > 0 || iAc27 > 0 || iAc28 > 0 || iAc29 > 0 || iAc30 > 0 || iAc31 > 0 || iAc32 > 0 || iAc33 > 0 || iAc34 > 0 || iAc35 > 0 || iAc36 > 0 || iAc37 > 0 || cbsdtls.sSchmCode == "CARUP" || cbsdtls.sSchmCode == "SBPIN" || cbsdtls.sSchmCode == "SBPIS" || cbsdtls.sSchmCode == "SBSOB" || iAcctBalAmt >= 100000000 || cbsdtls.scustomerisMinor.ToUpper() == "Y" || cbsdtls.sInvalid == "Failure" || (cbsdtls.sacct_status == "D" && cbsdtls.sFreezeStatusCode == "T") || (cbsdtls.sacct_status == "D" && cbsdtls.sFreezeStatusCode == "C") || (cbsdtls.sacct_status == "A" && cbsdtls.sFreezeStatusCode == "T") || (cbsdtls.sacct_status == "A" && cbsdtls.sFreezeStatusCode == "C") || cbsdtls.sClosedAccount == "ACCOUNT IS CLOSED")
                                        //    cbsdtls.sInvalidAcFlag = "T";

                                    }// if(count) 

                                }


                            }
                            //vikram

                            // Log Request

                            //string ServerPath = Server.MapPath("~/Logs/");
                            //if (System.IO.Directory.Exists(ServerPath) == false)
                            //{
                            //    System.IO.Directory.CreateDirectory(ServerPath);
                            //}
                            //string filename = DateTime.Now.ToString("ddMMyyyy") + "Logs.txt";
                            //string fileNameWithPath = ServerPath + filename;
                            //System.IO.StreamWriter str = new System.IO.StreamWriter(fileNameWithPath, true, System.Text.Encoding.Default);

                            //str.WriteLine(DateTime.Now.ToShortTimeString() + ": Request: " + sInputString);
                            //str.Close();

                        }
                    }
                }
                //logerror("Calling CBS details method end : ", "");
                return PartialView("_GetApiCbsDtls", cbsdtls);
            }
            catch (Exception e)
            {
                logerrorInCatch("Error : ", e.Message.ToString());
                //if (ex.Message.ToString().Trim().IndexOf("error", 1) > 0)
                //{
                //    cbsdtls.sCAPA = "Invalid Account";
                //    cbsdtls.PayeeNameList = null;
                //    cbsdtls.status = "SUCCESS";
                //    cbsdtls.cbsdls = null;
                //    cbsdtls.sInvalidAcFlag = "T";
                //}

                //else
                //    cbsdtls.status = "FAILURE";

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
                logerrorInCatch(e.Message.ToString(), message);
                //====================================================================================

                return PartialView("Error", er);
            }
        }

        private string getCustomerName(string sCMPCResponse)
        {
            string sResposne =
           "{" +
                 "\"data\": {" +
                        "\"cmcpId\": \"IN1B22X3223B4\"," +
                        "\"cin\": \"21798482\"," +
                        "\"cinSuffix\": \"00\"," +
                        "\"customerType\": 1," +
                        "\"profileInfo\": {" +
                                             "\"dbsIndustryCode\": {" +
                                                                    "\"codeValueId\": 583," +
                                                                    "\"codeValueCd\": \"99950\"," +
                                                                    "\"referenceCodeValueCd\": \"99950\"," +
                                                                    "\"codeValueDisplay\": \"PRIVATE INDIVIDUALS\"" +
                                                                "}," +
                                            "\"countryOfResidenceCode\": {" +
                                                                        "\"codeValueId\": 1959," +
                                                                        "\"codeValueCd\": \"IN\"," +
                                                                        "\"referenceCodeValueCd\": \"IN\"," +
                                                                        "\"codeValueDisplay\": \"INDIA\"" +
                                                                    "}," +
                                            "\"registeredName\": \"P XIXXX\"," +
                                            "\"nameLine1\": \"P XIXXX\"," +
                                            "\"nameLine2\": \"\"," +
                                            "\"customerSubTypeCode\": {" +
                                                                        "\"codeValueId\": 694," +
                                                                        "\"codeValueCd\": \"R\"," +
                                                                        "\"referenceCodeValueCd\": \"R\"," +
                                                                        "\"codeValueDisplay\": \"RETAIL\"" +
                                                                    "}," +
                                            "\"relationshipStartDate\": \"2017-11-11\"," +
                                            "\"nameInNativeLanguage\": \"\"," +
                                            "\"preferredLanguageCode\": {" +
                                                                            "\"codeValueId\": 12014," +
                                                                            "\"codeValueCd\": \"001\"," +
                                                                            "\"referenceCodeValueCd\": \"001\"," +
                                                                            "\"codeValueDisplay\": \"ENGLISH\" " +
                                                                        "}," +
                                            "\"remarks\": \"\"," +
                                            "\"taxDeductionAtSourceTableCode\": {" +
                                                                                "\"codeValueId\": 57956," +
                                                                                "\"codeValueCd\": \"TDS01\"," +
                                                                                "\"referenceCodeValueCd\": \"TDS01\"," +
                                                                                "\"codeValueDisplay\": \"TDS01\"" +
                                                                            "}," +
                                            "\"forexTierGroupCode\": {" +
                                                                            "\"codeValueId\": 57193," +
                                                                            "\"codeValueCd\": \"12\"," +
                                                                            "\"referenceCodeValueCd\": \"12\"," +
                                                                            "\"codeValueDisplay\": \"FX Group 12\"" +
                                                                        "}," +
                                            "\"taxStatus\": {" +
                                                                            "\"codeValueId\": 57971," +
                                                                            "\"codeValueCd\": \"800001\"," +
                                                                            "\"referenceCodeValueCd\": \"800001\"," +
                                                                            "\"codeValueDisplay\": \"Resident  Tax\"" +
                                                                        "}," +
                                            "\"constitutionCode\": {" +
                                                                        "\"codeValueId\": 1856," +
                                                                        "\"codeValueCd\": \"15\"," +
                                                                        "\"referenceCodeValueCd\": \"15\"," +
                                                                        "\"codeValueDisplay\": \"INDIVIDUAL\"" +
                                                                    "}," +
                                            "\"customerNetWorth\": \"74999999\"," +
                                            "\"profitCenterCode\": {" +
                                                                        "\"codeValueId\": 57240," +
                                                                        "\"codeValueCd\": \"06\"," +
                                                                        "\"referenceCodeValueCd\": \"06\"," +
                                                                        "\"codeValueDisplay\": \"Retail\"" +
                                                                    "}," +
                                            "\"previousName\": \"\"," +
                                            "\"primaryBranchCode\": {" +
                                                                        "\"codeValueId\": 58731," +
                                                                        "\"codeValueCd\": \"811\"," +
                                                                        "\"referenceCodeValueCd\": \"811\"," +
                                                                        "\"codeValueDisplay\": \"MUMBAI\"" +
                                                                    "}," +
                                            "\"alias\": \"\"," +
                                            "\"salutationCode\": {" +
                                                                    "\"codeValueId\": 16," +
                                                                    "\"codeValueCd\": \"MR\"," +
                                                                    "\"referenceCodeValueCd\": \"MR\"," +
                                                                    "\"codeValueDisplay\": \"MR\"" +
                                                                "}," +
                                            "\"sexCode\": {" +
                                                                "\"codeValueId\": 25," +
                                                                "\"codeValueCd\": \"M\"," +
                                                                "\"referenceCodeValueCd\": \"M\"," +
                                                                "\"codeValueDisplay\": \"MALE\"" +
                                                            "}," +
                                            "\"maritalStatusCode\": {" +
                                                                        "\"codeValueId\": 27," +
                                                                        "\"codeValueCd\": \"1\"," +
                                                                        "\"referenceCodeValueCd\": \"1\"," +
                                                                        "\"codeValueDisplay\": \"UNMARRIED\"" +
                                                                    "}," +
                                            "\"nativeLanguageCode\": {" +
                                                                        "\"codeValueId\": 13662," +
                                                                        "\"codeValueCd\": \"INFENG\"," +
                                                                        "\"referenceCodeValueCd\": \"INFENG\"," +
                                                                        "\"codeValueDisplay\": \"ENGLISH\"" +
                                                                    "}," +
                                            "\"countryOfBirthCode\": {" +
                                                                        "\"codeValueId\": 12535," +
                                                                        "\"codeValueCd\": \"99\"," +
                                                                        "\"referenceCodeValueCd\": \"99\"," +
                                                                        "\"codeValueDisplay\": \"NOT APPLICABLE\"" +
                                                                    "}," +
                                            "\"maidenName\": \"\"," +
                                            "\"dateOfBirth\": \"1994-06-18\"," +
                                            "\"motherMaidenName\": \"X XXXXXX\"," +
                                            "\"fatherName\": \"p Hari\"," +
                                            "\"spouseName\": \"\"," +
                                            "\"staffFlag\": false," +
                                            "\"staffId\": \"\"," +
                                            "\"industryType2Code\": {" +
                                                                        "\"codeValueId\": 56936," +
                                                                        "\"codeValueCd\": \"410\"," +
                                                                        "\"referenceCodeValueCd\": \"410\"," +
                                                                        "\"codeValueDisplay\": \"4.1  Individuals (including HUF)\"" +
                                                                    "}," +
                                            "\"preferredName\": \"P XIXXX\"," +
                                            "\"version\": 1," +
                                            "\"createdBy\": \"FIVUSR\"," +
                                            "\"createdByChannel\": \"DGB\"," +
                                            "\"createdTimeStamp\": \"2017-11-11T14:04:03+08:00\"," +
                                            "\"updatedBy\": \"FINACLECRM\"," +
                                            "\"updatedByChannel\": \"MIGRATION\"," +
                                            "\"updatedTimeStamp\": \"2020-10-14T23:12:14+08:00\"" +
                                      "}," +
                                "\"owners\": [" +
                                                "{" +
                                                    "\"id\": 828160948," +
                                                    "\"ownerCode\": {" +
                                                                        "\"codeValueId\": 58818," +
                                                                        "\"codeValueCd\": \"0101\"," +
                                                                        "\"referenceCodeValueCd\": \"0101\"," +
                                                                        "\"codeValueDisplay\": \"Mass Market - DB\"" +
                                                                    "}," +
                                                    "\"ownerOrder\": 1," +
                                                    "\"version\": 1," +
                                                    "\"createdBy\": \"FIVUSR\"," +
                                                    "\"createdByChannel\": \"MIGRATION\"," +
                                                    "\"createdByDepartment\": \"MIGRATION\"," +
                                                    "\"createdTimeStamp\": \"2017-11-11T14:04:03+08:00\"," +
                                                    "\"updatedBy\": \"FINACLECRM\"," +
                                                    "\"updatedByChannel\": \"MIGRATION\"," +
                                                    "\"updatedByDepartment\": \"MIGRATION\"," +
                                                    "\"updatedTimeStamp\": \"2020-10-14T23:12:14+08:00\"" +
                                                "}" +
                                              "]," +
                                "\"attributes\": [" +
                                    "{" +
                                        "\"id\": 8014375275," +
                                        "\"customerAttributeCode\": {" +
                                                                        "\"codeValueId\": 58865," +
                                                                        "\"codeValueCd\": \"SUSPENDEDSR1\"," +
                                                                        "\"referenceCodeValueCd\": \"SUSPENDEDSR1\"," +
                                                                        "\"codeValueDisplay\": \"CLOSED BY CUSTOMER\"," +
                                                                        "\"attributeValue\": \"2\"" +
                                                                    "}," +
                                        "\"statusSource\": \"CLOSED BY CUSTOMER\"," +
                                        "\"version\": 1," +
                                        "\"createdBy\": \"FIVUSR\"," +
                                        "\"createdByChannel\": \"DGB\"," +
                                        "\"createdTimeStamp\": \"2017-11-11T14:04:03+08:00\"," +
                                        "\"updatedBy\": \"FINACLECRM\"," +
                                        "\"updatedByChannel\": \"MIGRATION\"," +
                                        "\"updatedTimeStamp\": \"2020-10-14T23:12:14+08:00\"" +
                                    "}," +
                                    "{ " +
                                        "\"id\": 8010687031," +
                                        "\"customerAttributeCode\": {" +
                                            "\"codeValueId\": 2466," +
                                            "\"codeValueCd\": \"01\"," +
                                            "\"referenceCodeValueCd\": \"01\"," +
                                            "\"codeValueDisplay\": \"ACTIVE\"," +
                                            "\"attributeValue\": \"3\"" +
                                        "}," +
                                        "\"statusSource\": \"\"," +
                                        "\"version\": 1," +
                                        "\"createdBy\": \"FIVUSR\"," +
                                        "\"createdByChannel\": \"DGB\"," +
                                        "\"createdTimeStamp\": \"2017-11-11T14:04:03+08:00\"," +
                                        "\"updatedBy\": \"FINACLECRM\"," +
                                        "\"updatedByChannel\": \"MIGRATION\"," +
                                        "\"updatedTimeStamp\": \"2020-10-14T23:12:14+08:00\"" +
                                    "}" +
                                "]," +
                        "\"segments\": [" +
                                        "{" +
                                            "\"id\": 410451637303," +
                                            "\"segmentCode\": {" +
                                                "\"codeValueId\": 58163," +
                                                "\"codeValueCd\": \"IN000601\"," +
                                                "\"referenceCodeValueCd\": \"IN000601\"," +
                                                "\"codeValueDisplay\": \"Mass Market - DB\"" +
                                            "}," +
                                            "\"segmentTypeCode\": {" +
                                                "\"codeValueId\": 2530," +
                                                "\"codeValueCd\": \"01\"," +
                                                "\"referenceCodeValueCd\": \"01\"," +
                                                "\"codeValueDisplay\": \"SEGMENT LEVEL 1\"" +
                                            "}," +
                                            "\"version\": 1," +
                                            "\"createdBy\": \"FIVUSR\"," +
                                            "\"createdByChannel\": \"MIGRATION\"," +
                                            "\"createdTimeStamp\": \"2017-11-11T14:04:03+08:00\"," +
                                            "\"updatedBy\": \"FINACLECRM\"," +
                                            "\"updatedByChannel\": \"MIGRATION\"," +
                                            "\"updatedTimeStamp\": \"2020-10-14T23:12:14+08:00\"" +
                                        "}" +
                                    "]," +
                        "\"addresses\": [" +
                            "{             " +
                                "\"id\": 908700," +
                                "\"startDate\": \"2017-11-11\"," +
                                "\"endDate\": \"2099-12-31\"," +
                                "\"addressLabel\": \"\"," +
                                "\"addressTypeCode\": {" +
                                    "\"codeValueId\": 22768," +
                                    "\"codeValueCd\": \"Home\"," +
                                    "\"referenceCodeValueCd\": \"Home\"," +
                                    "\"codeValueDisplay\": \"Home\"" +
                                "}," +
                                "\"levelNumber\": \"\"," +
                                "\"unitNumber\": \"\"," +
                                "\"blockNumber\": \"\"," +
                                "\"streetName1\": \"6XXB\"," +
                                "\"streetName2\": \"SXXIXXXDX XAXXX\"," +
                                "\"streetName3\": \"PXXUXXXU .\"," +
                                "\"streetName4\": \"\"," +
                                "\"streetName5\": \"CHITTOOR\"," +
                                "\"streetName6\": \"\"," +
                                "\"postalCode\": \"517126\"," +
                                "\"cityCode\": {" +
                                    "\"codeValueId\": 51140," +
                                    "\"codeValueCd\": \".\"," +
                                    "\"referenceCodeValueCd\": \".\"," +
                                    "\"codeValueDisplay\": \".\"" +
                                "}," +
                                "\"stateCode\": {" +
                                    "\"codeValueId\": 57308," +
                                    "\"codeValueCd\": \"AP\"," +
                                    "\"referenceCodeValueCd\": \"AP\"," +
                                    "\"codeValueDisplay\": \"ANDHRAPRADESH\"" +
                                "}," +
                                "\"countryCode\": {" +
                                    "\"codeValueId\": 1959," +
                                    "\"codeValueCd\": \"IN\"," +
                                    "\"referenceCodeValueCd\": \"IN\"," +
                                    "\"codeValueDisplay\": \"INDIA\"" +
                                "}," +
                                "\"nativeStreetName1\": \"\"," +
                                "\"nativeStreetName2\": \"\"," +
                                "\"nativeStreetName3\": \"\"," +
                                "\"nativeStreetName4\": \"\"," +
                                "\"preferredFlag\": false," +
                                "\"holdMailFlag\": false," +
                                "\"holdMailReason\": \"\"," +
                                "\"formatAddress1\": \"6XXB\"," +
                                "\"formatAddress2\": \"SXXIXXXDX XAXXX\"," +
                                "\"formatAddress3\": \"PXXUXXXU .\"," +
                                "\"formatAddress4\": \"SINGAPORE 517126\"," +
                                "\"version\": 1," +
                                "\"createdBy\": \"MIGRATION\"," +
                                "\"createdByChannel\": \"MIGRATION\"," +
                                "\"createdByDepartment\": \"\"," +
                                "\"createdTimeStamp\": \"2022-02-22T00:00:00+08:00\"," +
                                "\"updatedBy\": \"MIGRATION\"," +
                                "\"updatedByChannel\": \"MIGRATION\"," +
                                "\"updatedByDepartment\": \"\"," +
                                "\"updatedTimeStamp\": \"2017-11-11T14:29:01+08:00\"" +
                            "}," +
                            "{" +
                                "\"id\": 991262," +
                                "\"startDate\": \"2017-11-11\"," +
                                "\"endDate\": \"2099-12-31\"," +
                                "\"addressLabel\": \"\"," +
                                "\"addressTypeCode\": {" +
                                    "\"codeValueId\": 11003," +
                                    "\"codeValueCd\": \"Mailing\"," +
                                    "\"referenceCodeValueCd\": \"Mailing\"," +
                                    "\"codeValueDisplay\": \"Mailing\"" +
                                "}," +
                                "\"levelNumber\": \"\"," +
                                "\"unitNumber\": \"\"," +
                                "\"blockNumber\": \"\"," +
                                "\"streetName1\": \"#XX, XXX XXXSX X1XXXUXXXGXXXR XXNXXX XXX XXXDX\"," +
                                 "\"streetName2\": \"MXXNXXXLXXXMXXXTXXXAXXX.\"," +
                                 "\"streetName3\": \".\"," +
                                 "\"streetName4\": \"\"," +
                                 "\"streetName5\": \"\"," +
                                 "\"streetName6\": \"\"," +
                                 "\"postalCode\": \"560037\"," +
                                 "\"cityCode\": {" +
                                     "\"codeValueId\": 55728," +
                                     "\"codeValueCd\": \"BANGL\"," +
                                     "\"referenceCodeValueCd\": \"BANGL\"," +
                                     "\"codeValueDisplay\": \"BANGALORE\"" +
                                "}," +
                                "\"stateCode\": {" +
                                    "\"codeValueId\": 57347," +
                                    "\"codeValueCd\": \"KA\"," +
                                    "\"referenceCodeValueCd\": \"KA\"," +
                                    "\"codeValueDisplay\": \"KARNATAKA\"" +
                                "}," +
                                "\"countryCode\": {" +
                                    "\"codeValueId\": 1959," +
                                    "\"codeValueCd\": \"IN\"," +
                                    "\"referenceCodeValueCd\": \"IN\"," +
                                    "\"codeValueDisplay\": \"INDIA\"" +
                                "}," +
                                "\"nativeStreetName1\": \"\"," +
                                "\"nativeStreetName2\": \"\"," +
                                "\"nativeStreetName3\": \"\"," +
                                "\"nativeStreetName4\": \"\"," +
                                "\"preferredFlag\": true," +
                                "\"holdMailFlag\": false," +
                                "\"holdMailReason\": \"\"," +
                                "\"formatAddress1\": \"#XX,XXX XXXSX X1XXXUXXXGXXXR XXNXXX XXX\"," +
                                 "\"formatAddress2\": \"MXXNXXXLXXXMXXXTXXXAXXX.\"," +
                                 "\"formatAddress3\": \".\"," +
                                 "\"formatAddress4\": \"SINGAPORE 560037\"," +
                                 "\"version\": 1," +
                                 "\"createdBy\": \"MIGRATION\"," +
                                 "\"createdByChannel\": \"MIGRATION\"," +
                                 "\"createdByDepartment\": \"\"," +
                                 "\"createdTimeStamp\": \"2022-02-22T00:00:00+08:00\"," +
                                 "\"updatedBy\": \"MIGRATION\"," +
                                 "\"updatedByChannel\": \"MIGRATION\"," +
                                 "\"updatedByDepartment\": \"\"," +
                                 "\"updatedTimeStamp\": \"2017-11-11T14:04:03+08:00\"" +
                            "}," +
                            "{" +
                                "\"id\": 5576186," +
                                "\"startDate\": \"2017-11-11\"," +
                                "\"endDate\": \"2099-12-31\"," +
                                "\"addressLabel\": \"\"," +
                                "\"addressTypeCode\": {" +
                                    "\"codeValueId\": 22771," +
                                    "\"codeValueCd\": \"Mailing3\"," +
                                    "\"referenceCodeValueCd\": \"Mailing3\"," +
                                    "\"codeValueDisplay\": \"Mailing 3\"" +
                                "}," +
                                "\"levelNumber\": \"\"," +
                                "\"unitNumber\": \"\"," +
                                "\"blockNumber\": \"\"," +
                                "\"streetName1\": \"6XXB\"," +
                                "\"streetName2\": \"SXXIXXXDX XAXXX\"," +
                                "\"streetName3\": \"PXXUXXXU .\"," +
                                "\"streetName4\": \"\"," +
                                "\"streetName5\": \"CHITTOOR\"," +
                                "\"streetName6\": \"\"," +
                                "\"postalCode\": \"517126\"," +
                                "\"cityCode\": {" +
                                    "\"codeValueId\": 51140," +
                                    "\"codeValueCd\": \".\"," +
                                    "\"referenceCodeValueCd\": \".\"," +
                                    "\"codeValueDisplay\": \".\"" +
                                "}," +
                                "\"stateCode\": {" +
                                    "\"codeValueId\": 57308," +
                                    "\"codeValueCd\": \"AP\"," +
                                    "\"referenceCodeValueCd\": \"AP\"," +
                                    "\"codeValueDisplay\": \"ANDHRAPRADESH\"" +
                                "}," +
                                "\"countryCode\": {" +
                                    "\"codeValueId\": 1959," +
                                    "\"codeValueCd\": \"IN\"," +
                                    "\"referenceCodeValueCd\": \"IN\"," +
                                    "\"codeValueDisplay\": \"INDIA\"" +
                                "}," +
                                "\"nativeStreetName1\": \"\"," +
                                "\"nativeStreetName2\": \"\"," +
                                "\"nativeStreetName3\": \"\"," +
                                "\"nativeStreetName4\": \"\"," +
                                "\"preferredFlag\": false," +
                                "\"holdMailFlag\": false," +
                                "\"holdMailReason\": \"\"," +
                                "\"formatAddress1\": \"6XXB\"," +
                                "\"formatAddress2\": \"SXXIXXXDX XAXXX\"," +
                                "\"formatAddress3\": \"PXXUXXXU .\"," +
                                "\"formatAddress4\": \"SINGAPORE 517126\"," +
                                "\"version\": 1," +
                                "\"createdBy\": \"MIGRATION\"," +
                                "\"createdByChannel\": \"MIGRATION\"," +
                                "\"createdByDepartment\": \"\"," +
                                "\"createdTimeStamp\": \"2022-02-22T00:00:00+08:00\"," +
                                "\"updatedBy\": \"MIGRATION\"," +
                                "\"updatedByChannel\": \"MIGRATION\"," +
                                "\"updatedByDepartment\": \"\"," +
                                "\"updatedTimeStamp\": \"2017-11-11T14:29:01+08:00\"" +
                            "}" +
                        "]," +
                        "\"emails\": [" +
                           " {  " +
                                "\"id\": 18378606," +
                                "\"emailAddress\": \"VIJXXXXX.X@XXXXX.XXX\"," +
                                "\"emailTypeCode\": {" +
                                    "\"codeValueId\": 23090," +
                                    "\"codeValueCd\": \"51\"," +
                                    "\"referenceCodeValueCd\": \"51\"," +
                                    "\"codeValueDisplay\": \"OFFICIAL\"" +
                                "}," +
                                "\"emailStatusCode\": {" +
                                    "\"codeValueId\": 902," +
                                    "\"codeValueCd\": \"V\"," +
                                    "\"referenceCodeValueCd\": \"V\"," +
                                    "\"codeValueDisplay\": \"VERIFIED\"" +
                                "}," +
                                "\"preferredFlag\": true," +
                                "\"version\": 1," +
                                "\"createdBy\": \"MIGRATION\"," +
                                "\"createdByChannel\": \"MIGRATION\"," +
                                "\"createdTimeStamp\": \"2022-02-22T00:00:00+08:00\"," +
                                "\"updatedBy\": \"MIGRATION\"," +
                                "\"updatedByChannel\": \"MIGRATION\"," +
                                "\"updatedTimeStamp\": \"2017-11-11T14:04:03+08:00\"" +
                            "}" +
                        "]," +
                        "\"contacts\": [" +
                            "{ " +
                                "\"id\": 784028927," +
                                "\"contactTypeCode\": {" +
                                    "\"codeValueId\": 899," +
                                    "\"codeValueCd\": \"4\"," +
                                    "\"referenceCodeValueCd\": \"4\"," +
                                    "\"codeValueDisplay\": \"HANDPHONE\"" +
                                "}," +
                                "\"contactSuffix\": \"00\"," +
                                "\"contactPersonName\": \"\"," +
                                "\"contactPhoneNumber\": \"919999999999\"," +
                                "\"workExtensionNumber\": \"\"," +
                                "\"localNumber\": \"9999999999\"," +
                                "\"areaNumber\": \"\"," +
                                "\"countryCode\": {" +
                                    "\"codeValueId\": 1539," +
                                    "\"codeValueCd\": \"91\"," +
                                    "\"referenceCodeValueCd\": \"91\"," +
                                    "\"codeValueDisplay\": \"INDIA\"" +
                                "}," +
                                "\"preferredFlag\": true," +
                                "\"version\": 1," +
                                "\"createdBy\": \"MIGRATION\"," +
                                "\"createdByChannel\": \"MIGRATION\"," +
                                "\"createdTimeStamp\": \"2022-02-22T00:00:00+08:00\"," +
                                "\"updatedBy\": \"MIGRATION\"," +
                                "\"updatedByChannel\": \"MIGRATION\"," +
                                "\"updatedTimeStamp\": \"2017-11-11T16:02:55+08:00\"" +
                            "}" +
                       " ]," +
                        "\"nationalities\": [" +
                           " { " +
                                "\"id\": 400585239695," +
                                "\"nationalityCode\": {" +
                                    "\"codeValueId\": 57046," +
                                    "\"codeValueCd\": \"IN\"," +
                                    "\"referenceCodeValueCd\": \"IN\"," +
                                    "\"codeValueDisplay\": \"INDIA\"" +
                                "}," +
                                "\"version\": 1," +
                                "\"createdBy\": \"MIGRATION\"," +
                                "\"createdByChannel\": \"MIGRATION\"," +
                                "\"createdTimeStamp\": \"2017-11-11T16:02:55+08:00\"," +
                                "\"updatedBy\": \"MIGRATION\"," +
                                "\"updatedByChannel\": \"MIGRATION\"," +
                                "\"updatedTimeStamp\": \"2017-11-11T16:02:55+08:00\"" +
                            "}" +
                       " ]," +
                        "\"educationDetails\": []," +
                        "\"employmentDetails\": [" +
                          "  {       " +
                                "\"id\": 9120689416," +
                                "\"employerName\": \"\"," +
                                "\"employerNameInAlternateLanguage\": \"\"," +
                                "\"addressLine1\": \"\"," +
                                "\"addressLine2\": \"\"," +
                                "\"postalCode\": \"\"," +
                                "\"phoneNumber\": \"\"," +
                                "\"phoneAreaNumber\": \"\"," +
                                "\"faxNumber\": \"\"," +
                                "\"incomeRangeFrom\": \"1\"," +
                                "\"incomeRangeTo\": \"99999\"," +
                                "\"employmentStatusCode\": {" +
                                    "\"codeValueId\": 887," +
                                    "\"codeValueCd\": \"08\"," +
                                    "\"referenceCodeValueCd\": \"08\"," +
                                    "\"codeValueDisplay\": \"NOT APPLICABLE\"" +
                                "}," +
                                "\"occupationalGroupCode\": {" +
                                    "\"codeValueId\": 56063," +
                                    "\"codeValueCd\": \"22\"," +
                                    "\"referenceCodeValueCd\": \"22\"," +
                                    "\"codeValueDisplay\": \"OTHERS - PROFESSIONAL\"" +
                                "}," +
                                "\"version\": 1," +
                                "\"createdBy\": \"FIVUSR\"," +
                                "\"createdByChannel\": \"DGB\"," +
                                "\"createdTimeStamp\": \"2017-11-11T14:04:03+08:00\"," +
                                "\"updatedBy\": \"FINACLECRM\"," +
                                "\"updatedByChannel\": \"MIGRATION\"," +
                                "\"updatedTimeStamp\": \"2017-11-11T16:02:55+08:00\"" +
                            "}" +
                        "]," +
                        "\"relationshipManagers\": [" +
                            "{" +
                                "\"id\": 13502155000," +
                                "\"ownerId\": 828160948," +
                                "\"officerIncharge\": \"DIGITALBANK\"," +
                                "\"order\": 1," +
                                "\"version\": 1," +
                                "\"createdBy\": \"FIVUSR\"," +
                                "\"createdByChannel\": \"MIGRATION\"," +
                                "\"createdTimeStamp\": \"2017-11-11T14:04:03+08:00\"," +
                                "\"updatedBy\": \"FINACLECRM\"," +
                                "\"updatedByChannel\": \"MIGRATION\"," +
                                "\"updatedTimeStamp\": \"2020-10-14T23:12:14+08:00\"" +
                            "}" +
                        "]" +
                    "}," +
                    "\"status\": {" +
                        "\"statusCode\": 0" +
                    "}," +
                    "\"traceInfo\": {" +
                        "\"spanId\": \"c10a48880717ebbc\"," +
                        "\"timestamp\": \"2022-07-28T14:58:30.69869+08:00\"" +
                    "}" +
            "}";


            return sResposne;
        }

        private string getCustomerId(string sServiceUrl, string sClientId, string sCoreRelationId, string sAccountNo)
        {
            string sResposne =
            "{ " +
                "\"sourceAccountNumber\": \"881028754855\"," +
                "\"ibanAccountNumber\": \"\"," +
                "\"sourceSystemId\": \"\"," +
                "\"sourceCustomerId\": \"22945768\"," +
                "\"productType\": \"SBA\"," +
                "\"productTypeDescription\": []," +
                "\"subProductType\": \"\"," +
                "\"productCode\": \"DBSBA\"," +
                "\"productCodeDescription\": [ { \"productCodeDescription\": \"DIGISAVINGS\"," +
                    "                              \"languageCode\": \"INFENG\"         } ]," +
                "\"productName\": \"DBSBA\"," +
                "\"nativeProductName\": \"DBSBA\"," +
                "\"accountCurrency\": \"INR\"," +
                "\"openedDate\": 1546041600000," +
                "\"modeOfOperation\": \"0003\"," +
                "\"serviceChargeExemption\": 0," +
                "\"staffIndicator\": false," +
                "\"officerId\": \"\"," +
                "\"officerUnit\": \"\"," +
                "\"firstExcessDate\": 0," +
                "\"lastUpdatedEvent\": \"\"," +
                "\"returnChequeDetailsInfo\": []," +
                "\"accountStatus\": \"Inactive\"," +
                "\"accountStatusCode\": \"I\"," +
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
                "\"accountName\": \"A XAXX GXXXSX\"," +
                "\"accountShortName\": \"A XAXX GXX\"," +
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
                "\"freezeCode\": \"T\"," +
                "\"freezeStatusCode\": \"T\"," +
                "\"freezeReasonCode\": \"0009\"," +
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
                        "				   \"effectiveAvailableAmount\": -374," +
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
                        "                            \"relatedPartyCustomerId\": \"22945768\"," +
                        "                            \"relatedPartySourceCustomerId\": \"\"," +
                        "                            \"relatedPartyType\": \"M\"," +
                        "                            \"relatedPartyTypeDescription\": \"Main Holder\"," +
                        "                            \"relatedPartyDeleteFlag\": \"N\"," +
                        "                            \"relatedPartyAddressType\": \"Mailing\"" +
                        "                            }," +
                        "                            { " +
                        "                            \"relatedPartyCode\": \"\"," +
                        "                            \"relatedPartyCodeDescription\": \"\"," +
                        "                            \"relatedPartyCustomerId\": \"22945769\"," +
                        "                            \"relatedPartySourceCustomerId\": \"\"," +
                        "                            \"relatedPartyType\": \"M\"," +
                        "                            \"relatedPartyTypeDescription\": \"2nd Holder\"," +
                        "                            \"relatedPartyDeleteFlag\": \"N\"," +
                        "                            \"relatedPartyAddressType\": \"Mailing\"" +
                        "                            }" +

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
        private void logerror(string errormsg, string errordesc)
        {
            var writeLog = ConfigurationManager.AppSettings["WriteLog"].ToString().ToUpper();

            if(writeLog == "Y")
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

        private string getAccountDetails(string sServiceUrl, string sClientId, string sCoreRelationId, string sAccountNo)
        {
            string sResposne = "";

            try
            {
                string sInputString = "";


                sInputString = " {";
                sInputString += "   \"sourceAccountNumber\": \"" + sAccountNo + "\"";
                sInputString += "}";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(sServiceUrl);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("x-Correlation-Id", sCoreRelationId);
                httpWebRequest.Headers.Add("x-sourcetimestamp", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                httpWebRequest.Headers.Add("x-sourceclientid", sClientId);

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
                sResposne = "{" +
                             "\"errorCode\":\"Runtime Error While Sending the Request\"," +
                             "\"errorDescription\":\"" + Ex.Message +
                            "}";
            }


            return sResposne;
        }

        private string getAccountDetailsDBSInvalid(string sServiceUrl, string sClientId, string sCoreRelationId, string sAccountNo)
        {
            string sResposne =
            "{ " +
                "\"error\":{ " +
                "\"timestamp\": \"1658131418469\"," +
                "\"status\": \"INTERNAL_SERVER_ERROR\"," +
                "\"errorType\": \"BE\"," +
                "\"errorCode\": \"MSTD-CD-5008\"," +
                "\"errorMessage\": \"Finacle Business Error\"," +
                "\"methodName\": \"\"," +
                "\"restError\":[ " +
                 "  {" +
                 "\"errorCode\": \"W025\"," +
                 "\"errorMessage\": \"Invalid Entity Details\"," +
                 "  }" +
                 "  ]" +
                 "  }," +
                 "\"timestamp\": \"1658131418469\"," +
                 "\"status\": \"INTERNAL_SERVER_ERROR\"," +
                 "\"errorType\": \"BE\"," +
                 "\"errorCode\": \"MSTD-CD-5008\"," +
                 "\"errorDescription\": \"Finacle Business Error\"," +
                 "\"methodName\": \"\"," +
                 "\"restError\": " +
                 "  [" +
                 "  {" +
                 "\"errorCode\": \"W025\"," +
                 "\"errorDescription\": \"Invalid Entity Details " +
                  "  }" +
                   "  ]" +
                    "  }";



            return sResposne;
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
                sResposne = "{" +
                             "\"errorCode\":\"Runtime Error While Sending the Request\"," +
                             "\"errorDescription\":\"" + Ex.Message +
                            "}";
            }


            return sResposne;
        }
        private string getAccountDetailsDBSResponse(string sServiceUrl, string sClientId, string sCoreRelationId, string sAccountNo)
        {
            string sResposne = "";

            if (sAccountNo == "222222222222")
            {
                sResposne =
            "{ " +
                "\"sourceAccountNumber\": \"881028754855\"," +
                "\"ibanAccountNumber\": \"\"," +
                "\"sourceSystemId\": \"\"," +
                "\"sourceCustomerId\": \"22945768\"," +
                "\"productType\": \"SBA\"," +
                "\"productTypeDescription\": []," +
                "\"subProductType\": \"\"," +
                "\"productCode\": \"DBSBA\"," +
                "\"productCodeDescription\": [ { \"productCodeDescription\": \"DIGISAVINGS\"," +
                    "                              \"languageCode\": \"INFENG\"         } ]," +
                //"\"productName\": \"NA\"," +
                "\"productName\": \"SARSA\"," +
                "\"nativeProductName\": \"DBSBA\"," +
                "\"accountCurrency\": \"INR\"," +
                "\"openedDate\": 1546041600000," +
                "\"modeOfOperation\": \"0003\"," +
                "\"serviceChargeExemption\": 0," +
                "\"staffIndicator\": false," +
                "\"officerId\": \"\"," +
                "\"officerUnit\": \"\"," +
                "\"firstExcessDate\": 0," +
                "\"lastUpdatedEvent\": \"\"," +
                "\"returnChequeDetailsInfo\": []," +
                "\"accountStatus\": \"A\"," +
                "\"accountStatusCode\": \"I\"," +
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
                "\"accountName\": \"A XAXX GXXXSX\"," +
                "\"accountShortName\": \"A XAXX GXX\"," +
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
                "\"freezeCode\": \"X\"," +
                "\"freezeStatusCode\": \"X\"," +
                "\"freezeReasonCode\": \"0009\"," +
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
                "\"accountClosedFlag\": \"A\"," +
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
                        "				   \"effectiveAvailableAmount\": -374," +
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
                        "                            \"relatedPartyCustomerId\": \"22945768\"," +
                        "                            \"relatedPartySourceCustomerId\": \"\"," +
                        "                            \"relatedPartyType\": \"M\"," +
                        "                            \"relatedPartyTypeDescription\": \"Main Holder\"," +
                        "                            \"relatedPartyDeleteFlag\": \"N\"," +
                        "                            \"relatedPartyAddressType\": \"Mailing\"" +
                        "                            }," +
                        "                            { " +
                        "                            \"relatedPartyCode\": \"\"," +
                        "                            \"relatedPartyCodeDescription\": \"\"," +
                        "                            \"relatedPartyCustomerId\": \"22945769\"," +
                        "                            \"relatedPartySourceCustomerId\": \"\"," +
                        "                            \"relatedPartyType\": \"M\"," +
                        "                            \"relatedPartyTypeDescription\": \"2nd Holder\"," +
                        "                            \"relatedPartyDeleteFlag\": \"N\"," +
                        "                            \"relatedPartyAddressType\": \"Mailing\"" +
                        "                            }" +
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
            }
            else if (sAccountNo == "333333333333")
            {
                sResposne =
            "{ " +
                "\"sourceAccountNumber\": \"881028754855\"," +
                "\"ibanAccountNumber\": \"\"," +
                "\"sourceSystemId\": \"\"," +
                "\"sourceCustomerId\": \"22945768\"," +
                "\"productType\": \"SBA\"," +
                "\"productTypeDescription\": []," +
                "\"subProductType\": \"\"," +
                "\"productCode\": \"DBSBA\"," +
                "\"productCodeDescription\": [ { \"productCodeDescription\": \"DIGISAVINGS\"," +
                    "                              \"languageCode\": \"INFENG\"         } ]," +
                //"\"productName\": \"NA\"," +
                "\"productName\": \"NRESA\"," +
                "\"nativeProductName\": \"DBSBA\"," +
                "\"accountCurrency\": \"INR\"," +
                "\"openedDate\": 1546041600000," +
                "\"modeOfOperation\": \"0003\"," +
                "\"serviceChargeExemption\": 0," +
                "\"staffIndicator\": false," +
                "\"officerId\": \"\"," +
                "\"officerUnit\": \"\"," +
                "\"firstExcessDate\": 0," +
                "\"lastUpdatedEvent\": \"\"," +
                "\"returnChequeDetailsInfo\": []," +
                "\"accountStatus\": \"A\"," +
                "\"accountStatusCode\": \"I\"," +
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
                "\"accountName\": \"A XAXX GXXXSX\"," +
                "\"accountShortName\": \"A XAXX GXX\"," +
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
                "\"freezeCode\": \"X\"," +
                "\"freezeStatusCode\": \"X\"," +
                "\"freezeReasonCode\": \"0009\"," +
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
                "\"accountClosedFlag\": \"A\"," +
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
                        "				   \"effectiveAvailableAmount\": -374," +
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
                        "                            \"relatedPartyCustomerId\": \"22945768\"," +
                        "                            \"relatedPartySourceCustomerId\": \"\"," +
                        "                            \"relatedPartyType\": \"M\"," +
                        "                            \"relatedPartyTypeDescription\": \"Main Holder\"," +
                        "                            \"relatedPartyDeleteFlag\": \"N\"," +
                        "                            \"relatedPartyAddressType\": \"Mailing\"" +
                        "                            }," +
                        "                            { " +
                        "                            \"relatedPartyCode\": \"\"," +
                        "                            \"relatedPartyCodeDescription\": \"\"," +
                        "                            \"relatedPartyCustomerId\": \"22945769\"," +
                        "                            \"relatedPartySourceCustomerId\": \"\"," +
                        "                            \"relatedPartyType\": \"M\"," +
                        "                            \"relatedPartyTypeDescription\": \"2nd Holder\"," +
                        "                            \"relatedPartyDeleteFlag\": \"N\"," +
                        "                            \"relatedPartyAddressType\": \"Mailing\"" +
                        "                            }" +
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
            }
            else if (sAccountNo == "444444444444")
            {
                sResposne =
            "{ " +
                "\"sourceAccountNumber\": \"881028754855\"," +
                "\"ibanAccountNumber\": \"\"," +
                "\"sourceSystemId\": \"\"," +
                "\"sourceCustomerId\": \"22945768\"," +
                "\"productType\": \"SBA\"," +
                "\"productTypeDescription\": []," +
                "\"subProductType\": \"\"," +
                "\"productCode\": \"DBSBA\"," +
                "\"productCodeDescription\": [ { \"productCodeDescription\": \"DIGISAVINGS\"," +
                    "                              \"languageCode\": \"INFENG\"         } ]," +
                //"\"productName\": \"NA\"," +
                "\"productName\": \"CAGOS\"," +
                "\"nativeProductName\": \"DBSBA\"," +
                "\"accountCurrency\": \"INR\"," +
                "\"openedDate\": 1546041600000," +
                "\"modeOfOperation\": \"0003\"," +
                "\"serviceChargeExemption\": 0," +
                "\"staffIndicator\": false," +
                "\"officerId\": \"\"," +
                "\"officerUnit\": \"\"," +
                "\"firstExcessDate\": 0," +
                "\"lastUpdatedEvent\": \"\"," +
                "\"returnChequeDetailsInfo\": []," +
                "\"accountStatus\": \"A\"," +
                "\"accountStatusCode\": \"I\"," +
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
                "\"accountName\": \"A XAXX GXXXSX\"," +
                "\"accountShortName\": \"A XAXX GXX\"," +
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
                "\"freezeCode\": \"X\"," +
                "\"freezeStatusCode\": \"X\"," +
                "\"freezeReasonCode\": \"0009\"," +
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
                "\"accountClosedFlag\": \"A\"," +
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
                        "				   \"effectiveAvailableAmount\": -374," +
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
                        "                            \"relatedPartyCustomerId\": \"22945768\"," +
                        "                            \"relatedPartySourceCustomerId\": \"\"," +
                        "                            \"relatedPartyType\": \"M\"," +
                        "                            \"relatedPartyTypeDescription\": \"Main Holder\"," +
                        "                            \"relatedPartyDeleteFlag\": \"N\"," +
                        "                            \"relatedPartyAddressType\": \"Mailing\"" +
                        "                            }," +
                        "                            { " +
                        "                            \"relatedPartyCode\": \"\"," +
                        "                            \"relatedPartyCodeDescription\": \"\"," +
                        "                            \"relatedPartyCustomerId\": \"22945769\"," +
                        "                            \"relatedPartySourceCustomerId\": \"\"," +
                        "                            \"relatedPartyType\": \"M\"," +
                        "                            \"relatedPartyTypeDescription\": \"2nd Holder\"," +
                        "                            \"relatedPartyDeleteFlag\": \"N\"," +
                        "                            \"relatedPartyAddressType\": \"Mailing\"" +
                        "                            }" +
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
            }
            //string sResposne =
            //"{ " +
            //    "\"sourceAccountNumber\": \"881028754855\"," +
            //    "\"ibanAccountNumber\": \"\"," +
            //    "\"sourceSystemId\": \"\"," +
            //    "\"sourceCustomerId\": \"22945768\"," +
            //    "\"productType\": \"SBA\"," +
            //    "\"productTypeDescription\": []," +
            //    "\"subProductType\": \"\"," +
            //    "\"productCode\": \"DBSBA\"," +
            //    "\"productCodeDescription\": [ { \"productCodeDescription\": \"DIGISAVINGS\"," +
            //        "                              \"languageCode\": \"INFENG\"         } ]," +
            //    //"\"productName\": \"NA\"," +
            //    "\"productName\": \"NRESA\"," +
            //    "\"nativeProductName\": \"DBSBA\"," +
            //    "\"accountCurrency\": \"INR\"," +
            //    "\"openedDate\": 1546041600000," +
            //    "\"modeOfOperation\": \"0003\"," +
            //    "\"serviceChargeExemption\": 0," +
            //    "\"staffIndicator\": false," +
            //    "\"officerId\": \"\"," +
            //    "\"officerUnit\": \"\"," +
            //    "\"firstExcessDate\": 0," +
            //    "\"lastUpdatedEvent\": \"\"," +
            //    "\"returnChequeDetailsInfo\": []," +
            //    "\"accountStatus\": \"A\"," +
            //    "\"accountStatusCode\": \"I\"," +
            //    "\"accountSignatoryType\": \"01\"," +
            //    "\"accountSignal\": \"\"," +
            //    "\"loanServicingIndicator\": false," +
            //    "\"accountFrozenIndicator\": true," +
            //    "\"noDebitIndicator\": false," +
            //    "\"debitReferralIndicator\": false," +
            //    "\"irregularSignalIndicator\": false," +
            //    "\"lineOfferedIndicator\": false," +
            //    "\"closureNoticeIndicator\": false," +
            //    "\"multipleAccountIndicator\": false," +
            //    "\"recallPassbookIndicator\": false," +
            //    "\"updateRequiredIndicator\": false," +
            //    "\"productIndicator\": \"\"," +
            //    "\"brandIndicator\": \"\"," +
            //    "\"ibanIndicator\": \"N\"," +
            //    "\"virtualAccountIndicator\": \"N\"," +
            //    "\"spclCustomerType\": \"\"," +
            //    "\"accountType\": \"\"," +
            //    "\"accountTypeDescription\": \"\"," +
            //    "\"currencyDecimal\": 2," +
            //    "\"generalLedgerSubHeadCode\": \"21201\"," +
            //    "\"accountCurrencyCode\": \"INR\"," +
            //    "\"odLimitType\": \"\"," +
            //    "\"odInterestAmount\": 0," +
            //    "\"accountName\": \"A XAXX GXXXSX\"," +
            //    "\"accountShortName\": \"A XAXX GXX\"," +
            //    "\"virtualAccountName\": \"\"," +
            //    "\"accountStatement\": { \"statementMode\": \"S\"," +
            //                              "  \"statementCalendar\": \"00\"," +
            //                              "  \"frequency\": \"M\"," +
            //                              "  \"frequencyStartDate\": 31," +
            //                              "  \"frequencyDay\": 0," +
            //                              "  \"frequencyWeekNumber\": 0," +
            //                              "  \"frequencyHolidayStatus\": \"N\"," +
            //                              "  \"nextPrintDate\": 1546214400000," +
            //                              "  \"despatchMode\": \"N\" " +
            //                              "  }" +
            //                              " ," +
            //    "\"balanceDebitCreditIndicator\": \"C\"," +
            //    "\"freezeCode\": \"X\"," +
            //    "\"freezeStatusCode\": \"X\"," +
            //    "\"freezeReasonCode\": \"0009\"," +
            //    "\"freezeReasonCode1\": \"\"," +
            //    "\"additionalFreezeReasonCodes\": []," +
            //    "\"additionalFreezeRemarks\": []," +
            //    "\"freezeReasonCodeDescriptionList\": []," +
            //    "\"freezeRemarks\": \"\"," +
            //    "\"freezeRemarks1\": \"\"," +
            //    "\"accountInterest\": { \"interestRate\": 3.25," +
            //        "                     \"interestCalInterest\": 0," +
            //        "                     \"interestFrequencyType\": \"M\"," +
            //        "                     \"interestFrequencyStartDate\": 31," +
            //        "                     \"interestFrequencyDay\": 0," +
            //        "                     \"interestFrequencyWeekNum\": 0," +
            //        "                     \"accountInterestFrequencyHolidayStatus\": \"P\"," +
            //        "                     \"interestRateCode\": \"DBSB1\"," +
            //        "                     \"netInterestRate\": 0," +
            //        "                     \"netInterestDebitCreditIndicator\": \"C\"," +
            //        "                     \"accruedInterestDebitCreditIndicator\": \"C\"," +
            //        "                     \"unpaidInterestFee\": 0," +
            //        "                     \"bookedamount\": 0," +
            //        "                     \"interestAmount\": 0," +
            //        "                     \"preferentialInterest\": 0" +
            //        "                   }" +
            //        "," +
            //    "\"taxCategory\": \"A\"," +
            //    "\"taxFloorLimit\": 0," +
            //    "\"taxFloorLimitCurrencyCode\": \"INR\"," +
            //    "\"withholdingTaxPercent\": 0," +
            //    "\"gstin\": \"\"," +
            //    "\"gstExemptionFlag\": \"\"," +
            //    "\"nickName\": \"\"," +
            //    "\"productShortName\": \"DBSBA\"," +
            //    "\"preferredLanguageProductShortName\": \"DBSBA\"," +
            //    "\"sourceMultiCurrencyAccountNumber\": \"\"," +
            //    "\"multiCurrencyAccountFlag\": false," +
            //    "\"branchCode\": \"811\"," +
            //    "\"branchCodeDescription\": \"\"," +
            //    "\"bankCode\": \"DBSIN\"," +
            //    //"\"accountClosedFlag\": \"N\"," +
            //    "\"accountClosedFlag\": \"A\"," +
            //    "\"accountClosedReasonCode\": \"\"," +
            //    "\"accountClosedRemarks\": \"\"," +
            //    "\"accountClosedDate\": 0," +
            //    "\"lastBalanceUpdateDateTime\": 0," +
            //    "\"earmarkUpdateDateTime\": 0," +
            //    "\"holdBalanceUpdateDateTime\": 0," +
            //    "\"sanctionLimitUpdateDateTime\": 0," +
            //    "\"staticDataUpdateDateTime\": 1656516121000," +
            //    "\"halfDayHoldBalanceExpiryDate\": 0," +
            //    "\"childAccounts\": []," +
            //    "\"accountBalances\": 	{" +
            //            "				   \"availableBalance\": -374," +
            //            "				   \"availableBalanceCurrencyCode\": \"INR\"," +
            //            "				   \"accountBalance\": 126," +
            //            "				   \"accountBalanceCurrencyCode\": \"INR\"," +
            //            "				   \"sanctionLimit\": 0," +
            //            "				   \"sanctionLimitCurrencyCode\": \"INR\"," +
            //            "				   \"ledgerBalance\": 126," +
            //            "				   \"ledgerBalanceCurrencyCode\": \"INR\"," +
            //            "				   \"halfDayHoldBalance\": 0," +
            //            "				   \"oneDayHoldBalance\": 0," +
            //            "				   \"twoDayHoldBalance\": 0," +
            //            "				   \"earmarkDebitAmount\": 0," +
            //            "				   \"earmarkCreditAmount\": 500," +
            //            "				   \"floatAmount\": 0," +
            //            "				   \"earmarkAmount\": 500," +
            //            "				   \"effectiveAvailableAmount\": -374," +
            //            "				   \"drawingPower\": 0," +
            //            "				   \"overDueLiableAmount\": 0," +
            //            "				   \"openingBalanceAmount\": 0," +
            //            "				   \"closingBalanceAmount\": 0," +
            //            "				   \"fundsClearingAmount\": 0," +
            //            "				   \"cumulativeCreditAmount\": 35170," +
            //            "				   \"cumulativeDebitAmount\": 35044," +
            //            "				   \"utilizedAmount\": 0," +
            //            "				   \"systemReservedAmount\": 0," +
            //            "				   \"overdueFutureAmount\": 0," +
            //            "				   \"utilizedFutureAmount\": 0," +
            //            "				   \"effectiveFutureAvailableAmount\": 0," +
            //            "				   \"availableAmountLineOfCredit\": 0," +
            //            "				   \"unclearDrawingAmount\": 0," +
            //            "				   \"ffdAvailableAmount\": 0," +
            //            "				   \"sweepsEffectiveAvailableAmount\": 0," +
            //            "				   \"hcAvailableAmount\": 0," +
            //            "				   \"futureAmount\": 0," +
            //            "				   \"futureCreditAmount\": 0," +
            //            "				   \"futureClearBalanceAmount\": 0," +
            //            "				   \"futureUnclearBalanceAmount\": 0," +
            //            "				   \"daccLimit\": 0," +
            //            "				   \"dafaLimit\": 0 " +
            //            "					}," +
            //            "\"relatedCustomerInfo\": [" +
            //            "                            { " +
            //            "                            \"relatedPartyCode\": \"\"," +
            //            "                            \"relatedPartyCodeDescription\": \"\"," +
            //            "                            \"relatedPartyCustomerId\": \"22945768\"," +
            //            "                            \"relatedPartySourceCustomerId\": \"\"," +
            //            "                            \"relatedPartyType\": \"M\"," +
            //            "                            \"relatedPartyTypeDescription\": \"Main Holder\"," +
            //            "                            \"relatedPartyDeleteFlag\": \"N\"," +
            //            "                            \"relatedPartyAddressType\": \"Mailing\"" +
            //            "                            }," +
            //            "                            { " +
            //            "                            \"relatedPartyCode\": \"\"," +
            //            "                            \"relatedPartyCodeDescription\": \"\"," +
            //            "                            \"relatedPartyCustomerId\": \"22945769\"," +
            //            "                            \"relatedPartySourceCustomerId\": \"\"," +
            //            "                            \"relatedPartyType\": \"M\"," +
            //            "                            \"relatedPartyTypeDescription\": \"2nd Holder\"," +
            //            "                            \"relatedPartyDeleteFlag\": \"N\"," +
            //            "                            \"relatedPartyAddressType\": \"Mailing\"" +
            //            "                            }" +
            //            "                      ]," +
            //            "\"promoCode\": []," +
            //            "\"mobileMoneyIdentifier\": \"9641689\"," +
            //            "\"mobileNumbers\": [ \"7358386665\" ]," +
            //            "\"reference1\": \"\"," +
            //            "\"reference2\": \"\"," +
            //            "\"ifscCode\": \"DBSS0IN0811\"," +
            //            "\"channelId\": \"SOI\"," +
            //            "\"accountStatusDate\": 1623456000000," +
            //            "\"expressAccountExpiryDate\": 0," +
            //            "\"schemeConversionDate\": 0," +
            //            "\"virtualAccountType\": \"\"," +
            //            "\"faxIndeminity\": \"\"," +
            //            "\"nomineeAvailableFlag\": \"N\"," +
            //            "\"nomineeGuardianInfo\": []" +
            //    "}   ";




            return sResposne;
        }

        void LogError()
        {
            //error log
            string sServerPath = Server.MapPath("~/Logs/");
            if (System.IO.Directory.Exists(sServerPath) == false)
                System.IO.Directory.CreateDirectory(sServerPath);

            string sfilename = DateTime.Now.ToString("ddMMyyyy") + "Logs.txt";
            string sfileNameWithPath = sServerPath + sfilename;
            System.IO.StreamWriter str1 = new System.IO.StreamWriter(sfileNameWithPath, true, System.Text.Encoding.Default);

            str1.WriteLine("\n");
            str1.WriteLine(DateTime.Now.ToShortTimeString() + ": Request: " + sInputString);
            str1.WriteLine("\n");

            str1.WriteLine(DateTime.Now.ToShortTimeString() + ": sCoreRelationId: " + sCasaCorellationId);
            str1.WriteLine("\n");
            str1.WriteLine(DateTime.Now.ToShortTimeString() + ": Timestamp: " + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            str1.WriteLine("\n");
            str1.WriteLine(DateTime.Now.ToShortTimeString() + ": sClientId: " + sCasaClientId);
            str1.WriteLine("\n");
            str1.WriteLine(DateTime.Now.ToShortTimeString() + ": sAccountNo: " + sAccountNo);
            str1.WriteLine("\n");
            str1.WriteLine(DateTime.Now.ToShortTimeString() + ": sServiceUrl: " + sCasaServiceURL);
            str1.WriteLine("\n");

            str1.WriteLine(DateTime.Now.ToShortTimeString() + ": Return API: " + sgetAccountDetailsDBS);
            str1.WriteLine("\n");
            str1.WriteLine("Response: " + sResposne);

            str1.Close();
        }

        public PartialViewResult GetSourceofFunds()
        {

            try
            {
                List<SelectListItem> items = new List<SelectListItem>();
                SqlDataAdapter adp = new SqlDataAdapter();
                logerror("Calling GetSourceOfFunds method start : ", "");
                if (Session["sNR"] == null)
                    Session["sNR"] = "";

                if (Session["sNR"].ToString().Trim() != "")
                {
                    if (Session["sNR"].ToString().Trim() == "NRE")
                    {
                        SqlDataAdapter adp1 = new SqlDataAdapter();
                        adp1 = new SqlDataAdapter("SP_NRESourceOfFund", con);
                        adp1.SelectCommand.CommandType = CommandType.StoredProcedure;
                        DataTable dt1 = new DataTable();
                        adp1.Fill(dt1);
                        ViewBag.vbSrcFndsNRE = dt1.DefaultView;
                    }
                    else if (Session["sNR"].ToString().Trim() == "NRO")
                    {
                        SqlDataAdapter adp2 = new SqlDataAdapter();
                        adp2 = new SqlDataAdapter("SP_NROSourceOfFund", con);
                        adp2.SelectCommand.CommandType = CommandType.StoredProcedure;
                        DataTable dt2 = new DataTable();
                        adp2.Fill(dt2);
                        ViewBag.vbSrcFndsNRO = dt2.DefaultView;
                    }

                }
                else
                    ViewBag.vbSrcFndsNRE = "";
                ViewBag.vbSrcFndsNRO = "";
                logerror("Calling GetSourceOfFunds method end : ", "");
                return PartialView("GetSourceofFunds");
            }
            catch (Exception e)
            {
                ErrorDisplay er = new ErrorDisplay();
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;
                er.ErrorMessage = message;

                return PartialView("Error", er);

            }
        }

        public JsonResult GetSourceofFundsFromNRE_NRO(Int64 ID = 0, string schemeType = null)
        {
            SqlDataAdapter SQLDA = new SqlDataAdapter("GetSourceofFundsFromNRE_NRO", con);
            SQLDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            SQLDA.SelectCommand.Parameters.AddWithValue("@ID", ID);
            SQLDA.SelectCommand.Parameters.AddWithValue("@schemeType", schemeType);

            DataSet ds = new DataSet();
            SQLDA.Fill(ds);
            string schemeName = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0].ItemArray[0] != null && ds.Tables[0].Rows[0].ItemArray[0].ToString() != "")
                    schemeName = ds.Tables[0].Rows[0].ItemArray[0].ToString();

            }
            return Json(schemeName, JsonRequestBehavior.AllowGet);
            // return PartialView("_IWActivitylogs", model);

        }

        public JsonResult GetNRE_NRO_Id_From_RawDataId(Int64 ID = 0, Int64 RawDataId = 0)
        {
            SqlDataAdapter SQLDA = new SqlDataAdapter("GetNRE_NRO_Id_From_RawDataId", con);
            SQLDA.SelectCommand.CommandType = CommandType.StoredProcedure;
            SQLDA.SelectCommand.Parameters.AddWithValue("@ID", ID);
            SQLDA.SelectCommand.Parameters.AddWithValue("@RawDataId", RawDataId);

            DataSet ds = new DataSet();
            SQLDA.Fill(ds);
            SourceOfFundData def = new SourceOfFundData();
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0].ItemArray[0] != null && ds.Tables[0].Rows[0].ItemArray[0].ToString() != "" &&
                    ds.Tables[0].Rows[0].ItemArray[1] != null && ds.Tables[0].Rows[0].ItemArray[1].ToString() != "")
                {
                    def.NRESourceOfFundId = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[0]);
                    def.NROSourceOfFundId = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[1]);
                }


            }
            return Json(def, JsonRequestBehavior.AllowGet);

        }

        public class SourceOfFundData
        {
            public int NRESourceOfFundId { get; set; }
            public int NROSourceOfFundId { get; set; }
        }

        //============ Amol changes for High Value cheque on 15/02/2024 start ======================

        public ActionResult OWL2Chq_HighValue(int id = 0, int DomainId = 0, string branchCode = null)
        {
            cbstetails cbsdtls = new cbstetails();
            //vikram forr web API
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            //vikram forr web API

            //Get token no. for API
            string NewApiCall = null;
            var OwApi = af.CommonSettings.FirstOrDefault(a => a.AppName == "CTSCONFIG1" && a.SettingName == "OWVerAPI")?.SettingValue;
            if (OwApi != null && OwApi != "")
            {
                NewApiCall = OwApi.ToString().ToUpper();
            }
            else
                NewApiCall = "N";

            ViewBag.NewApiCall = NewApiCall;
            //logerror("Calling CreateToken method start : ", "");
            //========= 1 uncomment For DBS Open start ==========
            if (NewApiCall == "Y")  // uncomment when deployed on bank
                Session["sToken"] = CreateToken();  // uncomment when deployed on bank
            //========= 1 uncomment For DBS Open end ==========

            ////Get token no. for API
            //logerror("Calling CreateToken method end : ", "");
            //logerror("sToken session value : ", Session["sToken"].ToString());


            int custid = Convert.ToInt16(Session["CustomerID"].ToString().Trim());

            var varMinAclen = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "ACFrom").SettingValue;
            var varMaxAclen = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "ACTo").SettingValue;

            int intMinAclen = Convert.ToInt32(varMinAclen);
            int intMaxAclen = Convert.ToInt32(varMaxAclen);


            ViewBag.MinAclen = intMinAclen;
            ViewBag.MaxAclen = intMaxAclen;


            //NRE for DBS
            Session["NR"] = "";
            cbsdtls.NREFlag = "NR";

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
                //vikram getting post date and stale date
                SqlDataAdapter adp1 = new SqlDataAdapter("OWPostStaleDates", con);
                adp1.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp1.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();
                adp1.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);

                DataTable dt = new DataTable();
                adp1.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    Session["sPostdate"] = dt.Rows[0]["PostDate"].ToString().Trim();
                    Session["sStaledate"] = dt.Rows[0]["StaleDate"].ToString().Trim();
                }

                //logerror("Calling OWPostStaleDates SP end : ", "");

                string VFType = "";
                if (id == 1)
                    VFType = "RNormal";
                else if (id == 2)
                    VFType = "RHold";
                else if (id == 3)
                    VFType = "BNormal";
                else if (id == 4)
                    VFType = "BHold";
                else if (id == 5)
                    VFType = "CDK";

                Session["VFType"] = VFType;


                SqlDataAdapter adp = new SqlDataAdapter("OWSelectCHQL2", con);
                //SqlDataAdapter adp = new SqlDataAdapter("OWSMBL2Verification", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@VFtype", SqlDbType.NVarChar).Value = "RNormalHV";
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();
                //-------------Added on 17-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                //-------------Added on 18-05-2017---------------------------
                if (id == 5)
                {
                    adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = DomainId;
                    Session["DomainselectID"] = DomainId;
                    adp.SelectCommand.Parameters.Add("@BranchCode", SqlDbType.NVarChar).Value = branchCode;

                }
                else
                    adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);

                if (branchCode != null)
                    Session["BranchCode"] = branchCode;
                else
                    Session["BranchCode"] = "Dummy";
                //-------------Added on 12-09-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = Session["CtsSessionType"];



                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<L2verificationModel>();
                L2verificationModel def;
                //logerror("Count of table : ", ds.Tables[0].Rows.Count.ToString());
                if (ds.Tables[0].Rows.Count > 0)
                {

                    //ViewBag.vbFrontTiffImage = ds.Tables[0].Rows[0]["FrontTiffImagePath"].ToString();
                    //ViewBag.vbFrontGreyImage = ds.Tables[0].Rows[0]["FrontGreyImagePath"].ToString();
                    //ViewBag.vbBackTiffImage = ds.Tables[0].Rows[0]["BackTiffImagePath"].ToString();

                    def = new L2verificationModel
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
                        DomainId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[16].ToString()),
                        CustomerId = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[18].ToString()),
                        ScanningType = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[19].ToString()),
                        L1VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[20].ToString()),
                        L1UserId = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[21].ToString()),
                        L1RejectReason = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[22].ToString()),
                        PayeeName = ds.Tables[0].Rows[0].ItemArray[23].ToString(),
                        CBSAccountInformation = ds.Tables[0].Rows[0].ItemArray[24].ToString(),
                        CBSJointAccountInformation = ds.Tables[0].Rows[0].ItemArray[25].ToString(),
                        UserNarration = ds.Tables[0].Rows[0].ItemArray[26].ToString(),
                        SlipUserNarration = ds.Tables[0].Rows[0].ItemArray[26].ToString(),
                        RejectReasonDescription = ds.Tables[0].Rows[0].ItemArray[27].ToString(),
                        FinalAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[28]),
                        FinalDate = ds.Tables[0].Rows[0].ItemArray[29].ToString(),
                        ChequeNoFinal = ds.Tables[0].Rows[0].ItemArray[30].ToString(),
                        SortCodeFinal = ds.Tables[0].Rows[0].ItemArray[31].ToString(),
                        SANFinal = ds.Tables[0].Rows[0].ItemArray[32].ToString(),
                        TransCodeFinal = ds.Tables[0].Rows[0].ItemArray[33].ToString(),
                        DocType = ds.Tables[0].Rows[0].ItemArray[34].ToString(),
                        Modified1 = ds.Tables[0].Rows[0].ItemArray[35].ToString(),
                        FrontUVImagePath = ds.Tables[0].Rows[0]["FrontUVImage"].ToString(),
                        DraweeName = ds.Tables[0].Rows[0]["DraweeName"].ToString(),
                        NRESourceOfFundId = Convert.ToInt32(ds.Tables[0].Rows[0]["NRESourceOfFundId"]),
                        NROSourceOfFundId = Convert.ToInt32(ds.Tables[0].Rows[0]["NROSourceOfFundId"]),
                        callby = "Cheq",
                    };

                    //Vikram 
                    if (ds.Tables[0].Rows.Count > 0)
                        ViewBag.vbDomainId = ds.Tables[0].Rows[0]["BranchCode"].ToString().Trim();
                    else
                        ViewBag.vbDomainId = "";

                    objectlst.Add(def);
                    //------------------------END------------------------//

                    //logerror("Count of table end : ", ds.Tables[0].Rows.Count.ToString());
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    //count = count - 1;
                    while (count > 0)
                    {
                        def = new L2verificationModel
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
                            DomainId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[16].ToString()),
                            CustomerId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[18].ToString()),
                            ScanningType = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[19].ToString()),
                            L1VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[20].ToString()),
                            L1UserId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[21].ToString()),
                            L1RejectReason = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[22].ToString()),
                            PayeeName = ds.Tables[0].Rows[index].ItemArray[23].ToString(),
                            CBSAccountInformation = ds.Tables[0].Rows[index].ItemArray[24].ToString(),
                            CBSJointAccountInformation = ds.Tables[0].Rows[index].ItemArray[25].ToString(),
                            UserNarration = ds.Tables[0].Rows[index].ItemArray[26].ToString(),
                            SlipUserNarration = ds.Tables[0].Rows[index].ItemArray[26].ToString(),
                            RejectReasonDescription = ds.Tables[0].Rows[index].ItemArray[27].ToString(),
                            FinalAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[28]),
                            FinalDate = ds.Tables[0].Rows[index].ItemArray[29].ToString(),
                            ChequeNoFinal = ds.Tables[0].Rows[index].ItemArray[30].ToString(),
                            SortCodeFinal = ds.Tables[0].Rows[index].ItemArray[31].ToString(),
                            SANFinal = ds.Tables[0].Rows[index].ItemArray[32].ToString(),
                            TransCodeFinal = ds.Tables[0].Rows[index].ItemArray[33].ToString(),
                            DocType = ds.Tables[0].Rows[index].ItemArray[34].ToString(),
                            Modified1 = ds.Tables[0].Rows[index].ItemArray[35].ToString(),
                            FrontUVImagePath = ds.Tables[0].Rows[index]["FrontUVImage"].ToString(),
                            DraweeName = ds.Tables[0].Rows[index]["DraweeName"].ToString(),
                            NRESourceOfFundId = Convert.ToInt32(ds.Tables[0].Rows[index]["NRESourceOfFundId"]),
                            NROSourceOfFundId = Convert.ToInt32(ds.Tables[0].Rows[index]["NROSourceOfFundId"]),
                            callby = "Cheq",
                        };
                        //ViewBag.cnt = true;
                        objectlst.Add(def);

                        count = count - 1;
                        index = index + 1;
                    }
                    //logerror("Count of table adding end: ", ds.Tables[0].Rows.Count.ToString());
                    //For UV image above 200000
                    ViewBag.DefaultImage = "~/Icons/noimagefound.jpg";
                    ViewBag.FrontUV = ds.Tables[0].Rows[0]["FrontUVImage"].ToString().Trim();
                    ViewBag.FrontGrey = ds.Tables[0].Rows[0].ItemArray[7].ToString().Trim();

                    string sFinalAmount = ds.Tables[0].Rows[0]["FinalAmount"].ToString().Trim();

                    if (Convert.ToDecimal(sFinalAmount.Substring(0, sFinalAmount.Length - 3)) >= 200000)
                        ViewBag.DefaultImage = ViewBag.FrontUV;
                    else
                        ViewBag.DefaultImage = ViewBag.FrontGrey;
                    //For UV image above 200000

                    //var rtnlist = iwafl.ItemReturnReasons.Select(m => m.RETURN_REASON_CODE).ToList();
                    //ViewBag.rtnlist = rtnlist;

                    //-------------- Added on 09-07-2021 ---- by Aniketadit ----- Begin
                    //logerror("calling SP_OWItemReturnReasons : ", "");
                    SqlDataAdapter adpRej = new SqlDataAdapter("SP_OwItemReturnReasons", con);
                    adpRej.SelectCommand.CommandType = CommandType.StoredProcedure;

                    DataSet dsRej = new DataSet();
                    adpRej.Fill(dsRej);
                    var objectlstRej = new List<RejectReason>();
                    var objectr = new RejectReason();
                    RejectReason rr;

                    int countRej = dsRej.Tables[0].Rows.Count;
                    int indexRej = 0;
                    while (countRej > 0)
                    {
                        rr = new RejectReason
                        {
                            ReasonCodeS = Convert.ToString((dsRej.Tables[0].Rows[indexRej].ItemArray[0])),
                            Description = Convert.ToString((dsRej.Tables[0].Rows[indexRej].ItemArray[1])),
                        };
                        objectlstRej.Add(rr);
                        //objectr.ReasonCodeS = Convert.ToString((ds.Tables[0].Rows[0].ItemArray[0]));
                        //objectr.Description = Convert.ToString((ds.Tables[0].Rows[0].ItemArray[1]));
                        countRej = countRej - 1;
                        indexRej = indexRej + 1;
                    }

                    //return PartialView("_RejectReason", objectlst);

                    ViewBag.rtnlist = objectlstRej.Select(m => m.ReasonCodeS).ToList();
                    ViewBag.rtnlistDescrp = objectlstRej.Select(m => m.Description).ToList();
                    //logerror("calling SP_OWItemReturnReasons end : ", "");
                    //-------------- Added on 09-07-2021 ---- by Aniketadit ----- End

                    //var rtnlist = (from i in af.ItemReturnReasons select i).ToList();// iwafl.ItemReturnReasons.Select(m).ToList();
                    //ViewBag.rtnlist = rtnlist.Select(m => m.RETURN_REASON_CODE).ToList();
                    //ViewBag.rtnlistDescrp = rtnlist.Select(m => m.DESCRIPTION).ToList();
                    //-------------------------------For Narration Accounts---------------------
                    ViewBag.narration = (from n in af.NarrationAccount where n.status == 1 select n.AccountNumber).ToList();
                    //-------------------------------For SchemCode---------------------
                    ViewBag.SchemCode = (from n in af.NreNroAccountList where n.IsActive == true select n.SchemCode).ToList();
                    @Session["glob"] = null;
                    ViewBag.cnt = true;
                    //logerror("Fetching L2 Record method end : ", ""); 
                    //account NO. length
                    // Session["acto"] = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == Convert.ToInt16(Session["CustomerID"]) && p.SettingName == "ACFrom").SettingValue;
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

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL2 HttpGet Index- " + innerExcp });
            }
            finally
            {
                con.Close();
            }

        }

        [HttpPost]
        public ActionResult OWL2ChqHV(List<string> lst, bool snd, string img = null, string btnClose = "default", List<Int64> idlst = null, double ChequeAmountTotal = 0)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["VF"] == false)
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
                    ttcnt = lst.Count() / 43;

                int stu;
                int resncode = 0;
                string rejctdecrptn = null;
                string cbdclnts = "";
                string cbdJointdtls = "";
                byte rejct = 0;
                string modaction = "";
                string userNarration = "";
                string Clearingtype = "";
                bool mark2pf = false;
                bool ignoreIQA = false;
                string DocType = "B";
                string rejectreasondescrpsn = "";
                string instrumenttype = "";
                int ScanningType = 0;
                Int64 SlipID = 0;
                Int64 SlipRawaDataID = 0;
                string finalmodified = "";
                DataSet ds = new DataSet();
                ObjectClass os = new ObjectClass();
                L2verificationModel def;
                var objectlst = new List<L2verificationModel>();
                Int64 id = 0;
                if (btnClose == "Close" && lst != null)
                    ttcnt = ttcnt + 1;
                else if (btnClose == "Close" && lst == null)
                    goto Close;
                // string IWDicision = Request.Form["IWDecision"].ToUpper();
                //if (snd == false)
                instrumenttype = lst[5].ToString();
                //logerror("In Post method OWChqL2 start : ", "");
                //logerror("In Post method OWChqL2 ttcnt count - : ", ttcnt.ToString());
                //vikram
                //while (lst.Count > 0)
                //{

                if (lst[5].ToString() == "S")
                {
                    if (ttcnt == 1)
                    {
                        //for (int i = 0; i < ttcnt; i++)
                        //{
                        if (lst[12].ToString() == "A")
                        {
                            //objectlst = os.selectL2Cheques(con, uid, Session["LoginID"].ToString(), lst, Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), img, "Slip", false, Convert.ToInt16(Session["CustomerID"]), Convert.ToInt32(Session["DomainselectID"]), Session["SlipOnlyAccept"].ToString(), Convert.ToDouble(Session["SlipOnlyAcceptAmtThreshold"]), Session["VFType"].ToString(), Session["CtsSessionType"].ToString());

                        }
                        else if (lst[12].ToString() == "F")
                        {

                            id = Convert.ToInt64(lst[0]);

                        }
                        else if (lst[12].ToString() == "R")
                        {
                            if (lst[15] != null)
                                rejct = Convert.ToByte(lst[15].ToString());
                            if (lst[32] != null)
                                userNarration = lst[32].ToString();
                            if (rejct == 88)
                            {
                                if (lst[33] != null)
                                    rejectreasondescrpsn = lst[33].ToString();
                                else
                                    rejectreasondescrpsn = "Other Reason";
                            }

                            //---------------Added On 21/06/2017------------------
                            if (lst[36] != null)
                                SlipID = Convert.ToInt64(lst[36]);
                            if (lst[37] != null)
                                SlipRawaDataID = Convert.ToInt64(lst[37]);
                            //---------------Added on 14/07/2017----------------
                            if (lst[38] != null)
                                ScanningType = Convert.ToInt16(lst[38]);

                            if (lst[39] != null)
                                finalmodified = lst[39].ToString();

                        }
                        if (lst[12].ToString() == "H")
                        {

                            //---------------Added On 21/06/2017------------------
                            if (lst[36] != null)
                                SlipID = Convert.ToInt64(lst[36]);
                            if (lst[37] != null)
                                SlipRawaDataID = Convert.ToInt64(lst[37]);
                            //---------------Added on 14/07/2017----------------
                            if (lst[38] != null)
                                ScanningType = Convert.ToInt16(lst[38]);

                            if (lst[39] != null)
                                finalmodified = lst[39].ToString();

                        }

                    }
                }
                else if (lst[5].ToString() == "C")
                {
                    //logerror("In Post method OWChqL2 In C - : ", "");
                    string sNREFlag = "";
                    string sacct_status = "";
                    if (Session["sNREFlag"] != null)
                    {
                        sNREFlag = Session["sNREFlag"].ToString().Trim();
                    }
                    else
                    {
                        sNREFlag = "";
                    }

                    if (Session["sacct_status"] != null)
                    {
                        sacct_status = Session["sacct_status"].ToString().Trim();
                    }
                    else
                    {
                        sacct_status = "";
                    }

                    string finaldate = "";
                    if (ttcnt == 1)
                    {
                        //logerror("In Post method OWChqL2 In C In If - : ", "");
                        for (int i = 0; i < ttcnt; i++)
                        {
                            //logerror("In Post method OWChqL2 In C In If lst[0] - : ", lst[0]);
                            id = Convert.ToInt64(lst[0]);
                            //logerror("In Post method OWChqL2 In C In If id - : ", id.ToString());

                            //logerror("In Post method OWChqL2 In C In If lst[15] - : ", lst[15]);
                            if (lst[15] != null && lst[15].ToString() != "")
                                rejct = Convert.ToByte(lst[15].ToString());
                            //logerror("In Post method OWChqL2 In C In If rejct - : ", rejct.ToString());

                            //logerror("In Post method OWChqL2 In C In If lst[21] - : ", lst[21]);
                            if (lst[21] != null)
                            {
                                if (lst[21].ToString().Length != 10)
                                    finaldate = "20" + lst[21].ToString().Substring(4, 2) + "-" + lst[21].ToString().Substring(2, 2) + "-" + lst[21].ToString().Substring(0, 2);
                                else
                                    finaldate = lst[21].ToString();
                            }
                            //logerror("In Post method OWChqL2 In C In If finaldate - : ", finaldate);

                            //--------Modification Validation------------
                            //logerror("In Post method OWChqL2 In C In If lst[12] - : ", lst[12]);
                            //logerror("In Post method OWChqL2 In C In If lst[13] - : ", lst[13]);
                            //logerror("In Post method OWChqL2 In C In If lst[30] - : ", lst[30]);
                            //logerror("In Post method OWChqL2 In C In If lst[39] - : ", lst[39]);
                            //logerror("In Post method OWChqL2 In C In If lst[33] - : ", lst[33]);
                            if (lst[12].ToString() == "A")
                            {
                                if (lst[13].ToString() == "8")
                                    modaction = "A";
                                else
                                {
                                    if (Convert.ToBoolean(lst[30]) == true || Convert.ToInt64(lst[39].ToString().Trim()) > 0)
                                        modaction = "M";
                                    else
                                        modaction = "A";
                                }
                            }
                            else if (lst[12].ToString() == "R")
                            {
                                modaction = "R";
                                if (rejct == 88)
                                {
                                    if (lst[33] != null)
                                        rejectreasondescrpsn = lst[33].ToString();
                                    else
                                        rejectreasondescrpsn = "Other Reason";
                                }
                            }
                            //logerror("In Post method OWChqL2 In C In If modaction - : ", modaction);
                            //logerror("In Post method OWChqL2 In C In If rejectreasondescrpsn - : ", rejectreasondescrpsn);

                            //logerror("In Post method OWChqL2 In C In If lst[32] - : ", lst[32]);
                            if (lst[32] != null)
                                userNarration = lst[32].ToString();
                            //logerror("In Post method OWChqL2 In C In If userNarration - : ", userNarration);

                            //logerror("In Post method OWChqL2 In C In If lst[34] - : ", lst[34]);
                            if (lst[34] != null)
                                Clearingtype = lst[34].ToString();
                            //logerror("In Post method OWChqL2 In C In If Clearingtype - : ", Clearingtype);
                            //------------------marking P2F--------------------//
                            //logerror("In Post method OWChqL2 In C In If lst[35] - : ", lst[35]);
                            if (lst[35] != null)
                            {
                                mark2pf = Convert.ToBoolean(lst[35]);
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
                            //logerror("In Post method OWChqL2 In C In If mark2pf - : ", mark2pf.ToString());
                            //---------------Added On 21/06/2017------------------
                            //logerror("In Post method OWChqL2 In C In If lst[36] - : ", lst[36]);
                            if (lst[36] != null)
                                SlipID = Convert.ToInt64(lst[36]);
                            //logerror("In Post method OWChqL2 In C In If SlipID - : ", SlipID.ToString());
                            //logerror("In Post method OWChqL2 In C In If lst[37] - : ", lst[37]);
                            if (lst[37] != null)
                                SlipRawaDataID = Convert.ToInt64(lst[37]);
                            //logerror("In Post method OWChqL2 In C In If SlipRawaDataID - : ", SlipRawaDataID.ToString());
                            //---------------Added on 14/07/2017----------------
                            //logerror("In Post method OWChqL2 In C In If lst[38] - : ", lst[38]);
                            if (lst[38] != null)
                                ScanningType = Convert.ToInt16(lst[38]);
                            //logerror("In Post method OWChqL2 In C In If ScanningType - : ", ScanningType.ToString());
                            //logerror("In Post method OWChqL2 In C In If lst[39] - : ", lst[39]);
                            if (lst[39] != null)
                                finalmodified = lst[39].ToString();
                            //logerror("In Post method OWChqL2 In C In If finalmodified - : ", finalmodified.ToString());

                            //logerror("In Post method OWChqL2 In C In else Session sNREFlag - : ", Session["sNREFlag"].ToString().Trim());
                            //logerror("In Post method OWChqL2 In C In else Session sacct_status - : ", Session["sacct_status"].ToString().Trim());

                            
                            SqlCommand cmd = new SqlCommand("UpdateOWL2", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID", id);
                            cmd.Parameters.AddWithValue("@RawDataId", Convert.ToInt64(lst[14]));
                            cmd.Parameters.AddWithValue("@Uid", uid);
                            cmd.Parameters.AddWithValue("@InstrumentType", lst[5].ToString());
                            cmd.Parameters.AddWithValue("@FinalAmount", Convert.ToDouble(lst[20].ToString()));
                            cmd.Parameters.AddWithValue("@FinalDate", finaldate);
                            cmd.Parameters.AddWithValue("@FinalChqNo", lst[22].ToString());
                            cmd.Parameters.AddWithValue("@FinalSortcode", lst[23].ToString());
                            cmd.Parameters.AddWithValue("@FinalSAN", lst[24].ToString());
                            cmd.Parameters.AddWithValue("@FinalTransCode", lst[25].ToString());
                            cmd.Parameters.AddWithValue("@CreditAccountNo", lst[1].ToString());
                            cmd.Parameters.AddWithValue("@PayeName", lst[27].ToString());
                            cmd.Parameters.AddWithValue("@status", Convert.ToInt16(lst[13]));
                            cmd.Parameters.AddWithValue("@RejectReason", rejct);
                            cmd.Parameters.AddWithValue("@ActionTaken", modaction);
                            cmd.Parameters.AddWithValue("@LName", @Session["LoginID"].ToString());
                            cmd.Parameters.AddWithValue("@ProcessingDate", Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@CustomerId", Convert.ToInt16(lst[17].ToString()));
                            cmd.Parameters.AddWithValue("@DomainId", Convert.ToInt32(lst[16].ToString()));
                            cmd.Parameters.AddWithValue("@ScanningNodeId", Convert.ToInt32(lst[9].ToString()));
                            cmd.Parameters.AddWithValue("@ChequeAmtotal", null);
                            cmd.Parameters.AddWithValue("@SlipAmount", 0);
                            cmd.Parameters.AddWithValue("@ChequeTotal", null);
                            cmd.Parameters.AddWithValue("@UserNarration", userNarration);
                            cmd.Parameters.AddWithValue("@RejectReasonDescription", rejectreasondescrpsn);
                            cmd.Parameters.AddWithValue("@CTSNONCTS", Clearingtype);
                            //cmd.Parameters.AddWithValue("@CBSAccountInformation", Session["sNREFlag"].ToString().Trim());
                            //cmd.Parameters.AddWithValue("@CBSJointAccountInformation", Session["sacct_status"].ToString().Trim());

                            cmd.Parameters.AddWithValue("@CBSAccountInformation", sNREFlag);
                            cmd.Parameters.AddWithValue("@CBSJointAccountInformation", sacct_status);
                            cmd.Parameters.AddWithValue("@IgnoreIQA", ignoreIQA);
                            cmd.Parameters.AddWithValue("@DocType", DocType);
                            cmd.Parameters.AddWithValue("@Modified", finalmodified);
                            cmd.Parameters.AddWithValue("@strHoldReason", "");
                            cmd.Parameters.AddWithValue("@DraweeName", lst[40].ToString());
                            cmd.Parameters.AddWithValue("@NRESourceOfFundId", Convert.ToInt32(lst[41]));
                            cmd.Parameters.AddWithValue("@NROSourceOfFundId", Convert.ToInt32(lst[42]));
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();

                            //logerror("In Post method OWChqL2 In C In If Update record end - : ", "Successful");
                            for (int k = 0; k < idlst.Count; k++)
                            {
                                if (idlst[k] == id)
                                    idlst.RemoveAt(k);
                            }
                            lst.RemoveRange(0, 43);
                        }
                        
                        if (btnClose == "Close")
                            goto Close;
                    }
                    else
                    {
                        //logerror("In Post method OWChqL2 In C In else - : ", "");

                        for (int i = 0; i < ttcnt - 1; i++)
                        {
                            //logerror("In Post method OWChqL2 In C In else lst[0] - : ", lst[0]);
                            id = Convert.ToInt64(lst[0]);
                            //logerror("In Post method OWChqL2 In C In else id - : ", id.ToString());

                            //logerror("In Post method OWChqL2 In C In else lst[15] - : ", lst[15]);
                            if (lst[15] != null && lst[15].ToString() != "")
                                rejct = Convert.ToByte(lst[15].ToString());
                            //logerror("In Post method OWChqL2 In C In else rejct - : ", rejct.ToString());

                            //logerror("In Post method OWChqL2 In C In else lst[21] - : ", lst[21]);
                            if (lst[21] != null)
                            {
                                if (lst[21].ToString().Length != 10)
                                    finaldate = "20" + lst[21].ToString().Substring(4, 2) + "-" + lst[21].ToString().Substring(2, 2) + "-" + lst[21].ToString().Substring(0, 2);
                                else
                                    finaldate = lst[21].ToString();
                            }
                            //logerror("In Post method OWChqL2 In C In else finaldate - : ", finaldate.ToString());
                            //--------Modification Validation------------
                            //logerror("In Post method OWChqL2 In C In else lst[13] - : ", lst[13]);
                            //logerror("In Post method OWChqL2 In C In else lst[30] - : ", lst[30]);
                            //logerror("In Post method OWChqL2 In C In else lst[12] - : ", lst[12]);
                            //logerror("In Post method OWChqL2 In C In else lst[33] - : ", lst[33]);
                            if (lst[12].ToString() == "A")
                            {
                                if (lst[13].ToString() == "8")
                                    modaction = "A";
                                else
                                {
                                    if (Convert.ToBoolean(lst[30]) == true || Convert.ToInt64(lst[39].ToString().Trim()) > 0)
                                        modaction = "M";
                                    else
                                        modaction = "A";
                                }
                            }
                            else if (lst[12].ToString() == "R")
                            {
                                modaction = "R";
                                if (rejct == 88)
                                {
                                    if (lst[33] != null)
                                        rejectreasondescrpsn = lst[33].ToString();
                                    else
                                        rejectreasondescrpsn = "Other Reason";
                                }
                            }
                            //logerror("In Post method OWChqL2 In C In else modaction - : ", modaction.ToString());
                            //logerror("In Post method OWChqL2 In C In else rejectreasondescrpsn - : ", rejectreasondescrpsn.ToString());

                            //logerror("In Post method OWChqL2 In C In else lst[32] - : ", lst[32]);
                            if (lst[32] != null)
                                userNarration = lst[32].ToString();
                            //logerror("In Post method OWChqL2 In C In else userNarration - : ", userNarration.ToString());

                            //logerror("In Post method OWChqL2 In C In else lst[34] - : ", lst[34]);
                            if (lst[34] != null)
                                Clearingtype = lst[34].ToString();
                            //logerror("In Post method OWChqL2 In C In else Clearingtype - : ", Clearingtype.ToString());

                            //-----------------Marking P2F----------------------//
                            //logerror("In Post method OWChqL2 In C In else lst[35] - : ", lst[35]);
                            if (lst[35] != null)
                            {
                                mark2pf = Convert.ToBoolean(lst[35]);
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
                            //logerror("In Post method OWChqL2 In C In else mark2pf - : ", mark2pf.ToString());
                            //---------------Added On 21/06/2017------------------
                            //logerror("In Post method OWChqL2 In C In else lst[36] - : ", lst[36]);
                            if (lst[36] != null)
                                SlipID = Convert.ToInt64(lst[36]);
                            //logerror("In Post method OWChqL2 In C In else SlipID - : ", SlipID.ToString());

                            //logerror("In Post method OWChqL2 In C In else lst[37] - : ", lst[37]);
                            if (lst[37] != null)
                                SlipRawaDataID = Convert.ToInt64(lst[37]);
                            //logerror("In Post method OWChqL2 In C In else SlipRawaDataID - : ", SlipRawaDataID.ToString());
                            //---------------Added on 14/07/2017----------------

                            //logerror("In Post method OWChqL2 In C In else lst[38] - : ", lst[38]);
                            if (lst[38] != null)
                                ScanningType = Convert.ToInt16(lst[38]);
                            //logerror("In Post method OWChqL2 In C In else ScanningType - : ", ScanningType.ToString());

                            //---------------Added on 14/07/2017----------------
                            //logerror("In Post method OWChqL2 In C In else lst[39] - : ", lst[0]);
                            if (lst[39] != null)
                                finalmodified = lst[39].ToString();
                            //logerror("In Post method OWChqL2 In C In else finalmodified - : ", finalmodified.ToString());

                            //logerror("In Post method OWChqL2 In C In else Session sNREFlag - : ", Session["sNREFlag"].ToString().Trim());
                            //logerror("In Post method OWChqL2 In C In else Session sacct_status - : ", Session["sacct_status"].ToString().Trim());
                            
                            SqlCommand cmd = new SqlCommand("UpdateOWL2", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID", id);
                            cmd.Parameters.AddWithValue("@RawDataId", Convert.ToInt64(lst[14]));
                            cmd.Parameters.AddWithValue("@Uid", uid);
                            cmd.Parameters.AddWithValue("@InstrumentType", lst[5].ToString());
                            cmd.Parameters.AddWithValue("@FinalAmount", Convert.ToDouble(lst[20].ToString()));
                            cmd.Parameters.AddWithValue("@FinalDate", finaldate);
                            cmd.Parameters.AddWithValue("@FinalChqNo", lst[22].ToString());
                            cmd.Parameters.AddWithValue("@FinalSortcode", lst[23].ToString());
                            cmd.Parameters.AddWithValue("@FinalSAN", lst[24].ToString());
                            cmd.Parameters.AddWithValue("@FinalTransCode", lst[25].ToString());
                            cmd.Parameters.AddWithValue("@CreditAccountNo", lst[1].ToString());
                            cmd.Parameters.AddWithValue("@PayeName", lst[27].ToString());
                            cmd.Parameters.AddWithValue("@status", Convert.ToInt16(lst[13]));
                            cmd.Parameters.AddWithValue("@RejectReason", rejct);
                            cmd.Parameters.AddWithValue("@ActionTaken", modaction);
                            cmd.Parameters.AddWithValue("@LName", @Session["LoginID"].ToString());
                            cmd.Parameters.AddWithValue("@ProcessingDate", Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@CustomerId", Convert.ToInt16(lst[17].ToString()));
                            cmd.Parameters.AddWithValue("@DomainId", Convert.ToInt32(lst[16].ToString()));
                            cmd.Parameters.AddWithValue("@ScanningNodeId", Convert.ToInt32(lst[9].ToString()));
                            cmd.Parameters.AddWithValue("@ChequeAmtotal", null);
                            cmd.Parameters.AddWithValue("@SlipAmount", 0);
                            cmd.Parameters.AddWithValue("@ChequeTotal", null);
                            cmd.Parameters.AddWithValue("@UserNarration", userNarration);
                            cmd.Parameters.AddWithValue("@RejectReasonDescription", rejectreasondescrpsn);
                            cmd.Parameters.AddWithValue("@CTSNONCTS", Clearingtype);
                            //cmd.Parameters.AddWithValue("@CBSAccountInformation", Session["sNREFlag"].ToString().Trim());
                            //cmd.Parameters.AddWithValue("@CBSJointAccountInformation", Session["sacct_status"].ToString().Trim());

                            cmd.Parameters.AddWithValue("@CBSAccountInformation", sNREFlag);
                            cmd.Parameters.AddWithValue("@CBSJointAccountInformation", sacct_status);
                            cmd.Parameters.AddWithValue("@IgnoreIQA", ignoreIQA);
                            cmd.Parameters.AddWithValue("@DocType", DocType);
                            cmd.Parameters.AddWithValue("@Modified", finalmodified);
                            cmd.Parameters.AddWithValue("@strHoldReason", "");
                            cmd.Parameters.AddWithValue("@DraweeName", lst[40].ToString());
                            cmd.Parameters.AddWithValue("@NRESourceOfFundId", Convert.ToInt32(lst[41]));
                            cmd.Parameters.AddWithValue("@NROSourceOfFundId", Convert.ToInt32(lst[42]));
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();

                            //logerror("In Post method OWChqL2 In C In else Update Record - : ", "Successful");
                            for (int k = 0; k < idlst.Count; k++)
                            {
                                if (idlst[k] == id)
                                    idlst.RemoveAt(k);
                            }
                            lst.RemoveRange(0, 43);

                        }

                        if (btnClose == "Close")
                            goto Close;


                    }
                    //if (lst.Count > 0)
                    objectlst = os.selectL2ChequesOnlyHV(con, uid, Session["LoginID"].ToString(), lst, Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), img, "Cheq", false, Convert.ToInt16(Session["CustomerID"]), Convert.ToInt32(Session["DomainselectID"]), null, 0, Session["VFType"].ToString(), Session["CtsSessionType"].ToString(), Session["VFType"].ToString());
                    //else
                    //objectlst = null;
                    //logerror("In Post method OWChqL2 In C In objectlst - : ", objectlst.Count.ToString());
                }
            //}

            //---------------------------IF Close button called--------------------------------//
            Close:
                if (btnClose == "Close")
                {
                    //logerror("In Post method OWChqL2 In Close - : ", " Close");
                    @Session["glob"] = true;
                    for (int p = 0; p < idlst.Count; p++)
                    {
                        //OWpro.OWUnlockRecords(idlst[p], "L2CDK");

                        SqlCommand com = new SqlCommand("OWUnlockRecords", con);
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@id", idlst[p]);
                        com.Parameters.AddWithValue("@module", "L2");

                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();

                        //logerror("In Post method OWChqL2 In Close Release Record end - : ", " Close end");
                    }
                    //if (instrumenttype == "C")
                    //    OWpro.OWUnlockRecords(SlipID, "L2");

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

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL2 HV HttpPost Index- " + innerExcp });
            }
            finally
            {
                con.Close();
            }

        }
        //============ Amol changes for High Value cheque on 15/02/2024 end ======================

        static int CalculateDifferenceInMonths(DateTimeOffset laterDate, DateTimeOffset earlierDate)
        {
            int monthsApart = 12 * (laterDate.Year - earlierDate.Year) + laterDate.Month - earlierDate.Month;

            if (laterDate.Day < earlierDate.Day)
            {
                monthsApart--;
            }

            return monthsApart;
        }

        //============ Amol changes for checking sortcode in BankBranches master on 01/06/2024 start =============

        [HttpPost]
        public ActionResult IsSortCodeExistInBankBranches(string sortcode = null)
        {
            bool OutputData = false;

            try
            {
                con.Open();
                SqlDataAdapter adp = new SqlDataAdapter("IsSortCodeExistInBankBranches", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@SortCode", SqlDbType.VarChar).Value = sortcode;

                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(ds.Tables[0].Rows[0]["SortCode"].ToString()) > 0)
                    {
                        OutputData = true;
                    }

                }

            }
            catch (Exception e)
            {
                logerrorInCatch(e.Message, e.Message.ToString() + " - > In Catch block message");
                logerrorInCatch(e.InnerException.ToString(), e.InnerException.ToString() + " - > In Catch block InnerException");
            }

            return Json(OutputData, JsonRequestBehavior.AllowGet);
        }
        //============ Amol changes for checking sortcode in BankBranches master on 01/06/2024 end =============

    }
}
