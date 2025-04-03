using ikloud_Aflatoon.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ikloud_Aflatoon.Models;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using NLog;

namespace ikloud_Aflatoon.Controllers
{
    public class OWL1Controller : Controller
    {
        //
        // GET: /OWL1/
        private static Logger logger = LogManager.GetCurrentClassLogger();
        AflatoonEntities af = new AflatoonEntities();
        OWProcDataContext OWpro = new OWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
        public ActionResult Index()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            if ((bool)Session["QC"] == false)
            {

                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            try
            {

                SqlDataAdapter adp = new SqlDataAdapter("OWSelectL1", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();
                //-------------Added on 17-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                //-------------Added on 18-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);
                //-------------Added on 12-09-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = Session["CtsSessionType"];


                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<L1verificationModel>();
                L1verificationModel def;

                if (ds.Tables[0].Rows.Count > 0)
                {

                    def = new L1verificationModel
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
                        callby = "Slip",
                        //CBSAccountInformation = ds.Tables[0].Rows[0].ItemArray[16].ToString(),
                        //CBSJointAccountInformation = ds.Tables[0].Rows[0].ItemArray[17].ToString(),
                    };
                    objectlst.Add(def);
                    //------------------------END------------------------//
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        def = new L1verificationModel
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
                            callby = "Slip",
                            //CBSAccountInformation = ds.Tables[0].Rows[index].ItemArray[15].ToString(),
                            //CBSJointAccountInformation = ds.Tables[0].Rows[index].ItemArray[16].ToString(),
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

                    Session["glob"] = null;
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

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL1 HttpGet Index- " + innerExcp });
            }
        }

        public ActionResult getTiffImage(string httpwebimgpath = null)
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
            System.IO.File.AppendAllText("C:\\temp\\log1.txt", "actualpathOWL1:" + actualpath);

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

           // imageDataURL = "";

            return PartialView("_getTiffImage");
            //return Json(imageDataURL, JsonRequestBehavior.AllowGet);

        }

        

        public PartialViewResult GetClientDlts(string ac = null, string branchcode = null)
        {
            try
            {
                var customer = (from c in af.CMS_CustomerMaster
                                from p in af.CMS_CustomerProductMaster
                                //bf
                                where c.Customer_Code == ac && p.customer_product_code == "LCC" && c.Customer_Code == p.customer_code &&
                                p.customer_prod_effective_date != null
                                select new { c.Customer_Name, p.customer_prod_effective_date, p.customer_prod_subcustomer_mand }).SingleOrDefault();

                if (customer != null)
                {
                    //---------------Location serching-----
                    var branchlocation = (from s in af.CMS_CustomerProductLocationMaster
                                          from o in af.CMS_OurBranchMaster
                                          where s.Customer_Code == ac && o.OUR_BRANCH_CODE == Convert.ToInt32(branchcode) && s.Location_Code == o.OUR_BRANCH_LOCATION_CODE &&
                                          s.Product_Code == "LCC" //&& s.Effective_Date == customer.customer_prod_effective_date
                                          select s.Customer_Code
                                     ).ToList();

                    if (branchlocation.Count() != 0)
                    {
                        var suspanded = (from s in af.CMS_ProductSuspensionMaster
                                         orderby s.SUSREVPX_EFF_DATE descending
                                         where s.SUSREVPX_CUST_CODE == ac && s.SUSREVPX_PROD_CODE == "LCC"
                                         select new { s.SUSREVPX_STAT_FLAG }
                                     ).Take(1).FirstOrDefault();

                        if (suspanded == null)
                        {
                            if (customer.customer_prod_subcustomer_mand == "Y")
                            {
                                ViewBag.subcustomercode = "true";
                            }
                            else
                            {
                                ViewBag.customer = customer.Customer_Name;
                            }

                        }
                        else
                        {
                            if (suspanded.SUSREVPX_STAT_FLAG == "D")
                                ViewBag.customer = "Customer are Suspended";
                            else
                                ViewBag.customer = customer.Customer_Name;
                        }

                    }
                    else
                        ViewBag.customer = "location";
                    //---------------Suspendead-----

                }
                else
                    ViewBag.customer = "Not Found";

                return PartialView("GetClientDlts");

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

                // return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL1 HttpGet Index- " + innerExcp });
                return PartialView("Error", er);
            }

        }
        public PartialViewResult GetBankName(string bankcode = null)
        {
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
                // model.Account = ac;
                model.callby = callby;
                model.payeenameselected = payeename;
                //model.CheckAc = ac;
                return PartialView("_GetCBSDtls", model);
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

                // return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL1 HttpGet Index- " + innerExcp });
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
            return PartialView("_RejectReason", rjrs);

        }
        //-------------------------Added On 24/07/2017-----------------------For Slip Image-----------
        public JsonResult slipImage(Int64 SlipId = 0)
        {
            var owL1 = af.L1Verification.Where(m => m.Id == SlipId).FirstOrDefault().FrontGreyImagePath;


            return Json(owL1, JsonRequestBehavior.AllowGet);
        }
        //-------------------------Added On 05/08/2017-----------------------For Slip Image-----------
        public PartialViewResult subClientCode(string subac = null, string ac = null)
        {
            try
            {
                var customer = (from c in af.CMS_SubCustomerMaster

                                where c.SUBCUST_SUB_CUST_CODE == subac && c.SUBCUST_CUST_CODE == ac
                                select new { c.SUBCUST_SUB_CUST_NAME }).SingleOrDefault();
                if (customer != null)
                    ViewBag.customer = customer.SUBCUST_SUB_CUST_NAME;
                else
                    ViewBag.customer = "Sub Client Not Found";

                return PartialView("GetClientDlts");
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

                // return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWL1 HttpGet Index- " + innerExcp });
                return PartialView("Error", er);
            }
        }

        public ActionResult OWChqL1(string ChqType = null)
        {
            Session["ChqType"] = ChqType;

            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            if ((bool)Session["QC"] == false)
            {

                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            try
            {

                SqlDataAdapter adp = new SqlDataAdapter("OWSelectCHQL1", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();
                //-------------Added on 17-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                //-------------Added on 18-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);
                //-------------Added on 12-09-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = Session["CtsSessionType"];
                //-------------Added on 20-05-2019-----------------------------
                adp.SelectCommand.Parameters.Add("@ChqType", SqlDbType.NVarChar).Value = ChqType;


                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<L1verificationModel>();
                L1verificationModel def;
                decimal branchAmt = 0;
                byte AiFinalResult = 0;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (!DBNull.Value.Equals(ds.Tables[0].Rows[0].ItemArray[31]))
                        branchAmt = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[31].ToString());

                    if (!DBNull.Value.Equals(ds.Tables[0].Rows[0].ItemArray[29]))
                        AiFinalResult = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[29].ToString());

                    def = new L1verificationModel
                    {

                        Id = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]),
                        BatchNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[1]),
                        BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[2]),
                        InstrumentType = ds.Tables[0].Rows[0].ItemArray[3].ToString(),
                        ClearingType = ds.Tables[0].Rows[0].ItemArray[4].ToString(),
                        Status = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[5]),
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[6].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]).Replace((string)Session["SrcWebIP1"], (string)Session["DestWepIP1"]).Replace((string)Session["SrcWebName1"], (string)Session["DestWebName1"]),
                        FrontGreyImagePath = ds.Tables[0].Rows[0].ItemArray[7].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]).Replace((string)Session["SrcWebIP1"], (string)Session["DestWepIP1"]).Replace((string)Session["SrcWebName1"], (string)Session["DestWebName1"]),
                        BackTiffImagePath = ds.Tables[0].Rows[0].ItemArray[8].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]).Replace((string)Session["SrcWebIP1"], (string)Session["DestWepIP1"]).Replace((string)Session["SrcWebName1"], (string)Session["DestWebName1"]),
                        CreditAccountNo = ds.Tables[0].Rows[0].ItemArray[11].ToString(),
                        BranchCode = ds.Tables[0].Rows[0].ItemArray[12].ToString(),
                        ProcessingDate = Convert.ToDateTime(ds.Tables[0].Rows[0].ItemArray[13].ToString()),
                        ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[14].ToString()),
                        RawDataId = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[15].ToString()),
                        DomainId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[16].ToString()),
                        CustomerId = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[17].ToString()),
                        ScanningType = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[18].ToString()),
                        FinalAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[19].ToString()),
                        FinalDate = ds.Tables[0].Rows[0].ItemArray[20].ToString(),
                        ChequeNoFinal = ds.Tables[0].Rows[0].ItemArray[21].ToString(),
                        SortCodeFinal = ds.Tables[0].Rows[0].ItemArray[22].ToString(),
                        SANFinal = ds.Tables[0].Rows[0].ItemArray[23].ToString(),
                        TransCodeFinal = ds.Tables[0].Rows[0].ItemArray[24].ToString(),
                        //RejectedField = ds.Tables[0].Rows[0].ItemArray[28].ToString(),
                        //AiFinalResult = AiFinalResult,// Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[29].ToString()),
                        //BranchAccount = ds.Tables[0].Rows[0].ItemArray[30].ToString(),
                        //BranchAmount = branchAmt,//Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[31].ToString()),


                        callby = "Chq",
                    };
                    ////------------------------Added on 03-05-2019--------------------------------
                    //if (ChqType == "CHQATVF")
                    //{
                    //    def.ATVAccountPass = ds.Tables[0].Rows[0].ItemArray[28].ToString().Trim();
                    //    def.ATVAmountPass = ds.Tables[0].Rows[0].ItemArray[29].ToString().Trim();
                    //    def.ATVDatePass = ds.Tables[0].Rows[0].ItemArray[30].ToString().Trim();
                    //    def.ATVMICRPasss = ds.Tables[0].Rows[0].ItemArray[31].ToString().Trim();
                    //}
                    //------------------------Added on 03-05-2019--------------------------END--
                    objectlst.Add(def);
                    //------------------------------------END---------------------------------------//
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        branchAmt = 0;
                        AiFinalResult = 0;
                        if (!DBNull.Value.Equals(ds.Tables[0].Rows[index].ItemArray[31]))
                            branchAmt = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[31].ToString());

                        if (!DBNull.Value.Equals(ds.Tables[0].Rows[index].ItemArray[29]))
                            AiFinalResult = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[29].ToString());

                        def = new L1verificationModel
                        {
                            Id = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),
                            BatchNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[1]),
                            BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[2]),
                            InstrumentType = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                            ClearingType = ds.Tables[0].Rows[index].ItemArray[4].ToString(),
                            Status = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[5]),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[6].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]).Replace((string)Session["SrcWebIP1"], (string)Session["DestWepIP1"]).Replace((string)Session["SrcWebName1"], (string)Session["DestWebName1"]),
                            FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[7].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]).Replace((string)Session["SrcWebIP1"], (string)Session["DestWepIP1"]).Replace((string)Session["SrcWebName1"], (string)Session["DestWebName1"]),
                            BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[8].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]).Replace((string)Session["SrcWebIP1"], (string)Session["DestWepIP1"]).Replace((string)Session["SrcWebName1"], (string)Session["DestWebName1"]),
                            CreditAccountNo = ds.Tables[0].Rows[index].ItemArray[11].ToString(),
                            BranchCode = ds.Tables[0].Rows[index].ItemArray[12].ToString(),
                            ProcessingDate = Convert.ToDateTime(ds.Tables[0].Rows[index].ItemArray[13].ToString()),
                            ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[14].ToString()),
                            RawDataId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[15].ToString()),
                            DomainId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[16].ToString()),
                            CustomerId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[17].ToString()),
                            ScanningType = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[18].ToString()),
                            FinalAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[19].ToString()),
                            FinalDate = ds.Tables[0].Rows[index].ItemArray[20].ToString(),
                            ChequeNoFinal = ds.Tables[0].Rows[index].ItemArray[21].ToString(),
                            SortCodeFinal = ds.Tables[0].Rows[index].ItemArray[22].ToString(),
                            SANFinal = ds.Tables[0].Rows[index].ItemArray[23].ToString(),
                            TransCodeFinal = ds.Tables[0].Rows[index].ItemArray[24].ToString(),
                            //RejectedField = ds.Tables[0].Rows[index].ItemArray[28].ToString(),
                            //AiFinalResult = AiFinalResult,// Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[29].ToString()),
                            //BranchAccount = ds.Tables[0].Rows[index].ItemArray[30].ToString(),
                            //BranchAmount = branchAmt,// Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[31].ToString()),
                            callby = "Chq",
                            //CBSAccountInformation = ds.Tables[0].Rows[index].ItemArray[15].ToString(),
                            //CBSJointAccountInformation = ds.Tables[0].Rows[index].ItemArray[16].ToString(),
                        };
                        //------------------------Added on 03-05-2019--------------------------------
                        //if (ChqType == "CHQATVF")
                        //{
                        //    def.ATVAccountPass = ds.Tables[0].Rows[index].ItemArray[28].ToString();
                        //    def.ATVAmountPass = ds.Tables[0].Rows[index].ItemArray[29].ToString();
                        //    def.ATVDatePass = ds.Tables[0].Rows[index].ItemArray[30].ToString();
                        //    def.ATVMICRPasss = ds.Tables[0].Rows[index].ItemArray[31].ToString();
                        //}
                        //------------------------Added on 03-05-2019--------------------------END--
                        objectlst.Add(def);
                        count = count - 1;
                        index = index + 1;
                    }

                    var rtnlist = (from i in af.ItemReturnReasons select i).ToList();// iwafl.ItemReturnReasons.Select(m).ToList();
                    ViewBag.rtnlist = rtnlist.Select(m => m.RETURN_REASON_CODE).ToList();
                    ViewBag.rtnlistDescrp = rtnlist.Select(m => m.DESCRIPTION).ToList();
                    //-------------------------------For Narration Accounts---------------------
                    ViewBag.narration = (from n in af.NarrationAccount where n.status == 1 select n.AccountNumber).ToList();
                    //-------------------------------For SchemCode---------------------
                    ViewBag.SchemCode = (from n in af.NreNroAccountList where n.IsActive == true select n.SchemCode).ToList();

                    Session["glob"] = null;
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

                logger.Log(LogLevel.Error, "OWChqL1 HttpGet|" + message + "INNEREXP| " + innerExcp, "Login Index-Msg");
                //logger.Log(LogLevel.Error, e.InnerException.StackTrace, "Login Index-Exception");
                return RedirectToAction("Error", "Error", new { msg = "Error", popmsg = "Error" });

            }
        }
    }
}
