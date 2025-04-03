using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ikloud_Aflatoon.Models;

namespace ikloud_Aflatoon.Controllers
{
    public class CMSClientCodeMissingController : Controller
    {
        //
        // GET: /CMSClientCodeMissing/
        AflatoonEntities af = new AflatoonEntities();
        OWProcDataContext OWpro = new OWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
        CMSReferral CMSObj = new CMSReferral();
       // Boolean bSubClnt;

        public ActionResult ClientCodeSelection(int id = 0,Int64 LockID=0,string LockModule=null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["CCPH"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            if (LockModule=="L1")
            {
                OWpro.CMSUnlockRecords(LockID, LockModule);
                
            }

            if (id == 1)
                ViewBag.msg = "No Data Found!!..";
            return View();

        }

        [HttpPost]
        public ActionResult ClientCodeSelection(string btn)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["CCPH"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            // string ProcType=Request.Form["ClientCode"].ToString();
            //if (Request.Form["ClientCode"].ToString()=="Normal")
            //{
            //    @Session["ProcType"] = "Normal";

            //}
            //else
            //{
            //    @Session["ProcType"] = "Hold";
            //}

            //@Session["gStatus"] = Request.Form["ClientCode"].ToString(); 


            if (btn == "Close")
            {
                return RedirectToAction("IWIndex", "Home");
            }


            return RedirectToAction("ClientCode", new { ProcType = Request.Form["ClientCode"].ToString() });


        }


        public ActionResult ClientCode(string ProcType)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["CCPH"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            int uid = (int)Session["uid"];
            ViewBag.ProcType = ProcType;
            try
            {

                SqlDataAdapter adp = new SqlDataAdapter("CMSSelectClntCode", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();
                //-------------Added on 17-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                //-------------Added on 18-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);
                adp.SelectCommand.Parameters.Add("@ProcType", SqlDbType.NVarChar).Value = ProcType; //Session["ProcType"].ToString();

                DataSet ds = new DataSet();
                adp.Fill(ds);


                if (ds.Tables.Count > 0)
                {

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        CMSObj.ID = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]);
                        CMSObj.CustomerId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[1]);
                        CMSObj.DomainId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[2]);
                        CMSObj.ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[3]);
                        CMSObj.Status = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[4]);
                        CMSObj.PayeeName = ds.Tables[0].Rows[0].ItemArray[5].ToString();
                        CMSObj.FrontGrayImagePath = ds.Tables[0].Rows[0].ItemArray[6].ToString();
                        CMSObj.BackGreyImagePath = ds.Tables[0].Rows[0].ItemArray[7].ToString();
                        CMSObj.SlipNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[8]);
                        CMSObj.RawDataID = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[9]);
                        CMSObj.BatchNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[10]);
                        CMSObj.BranchCode = ds.Tables[0].Rows[0].ItemArray[11].ToString();


                        ViewBag.cnt = true;
                        @Session["glob"] = null;
                        return View(CMSObj);
                    }
                    else
                        return RedirectToAction("ClientCodeSelection", "CMSClientCodeMissing", new { id = 1 });// return RedirectToAction("DeSelection", new { id = 1 });
                }
                else
                    return RedirectToAction("ClientCodeSelection", "CMSClientCodeMissing", new { id = 1 });// return RedirectToAction("DeSelection", new { id = 1 });
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "CMSClientCodeMissing HttpGet ClientCode- " + innerExcp });
            }
            //return View();
        }


        [HttpPost]
        public ActionResult ClientCode(CMSReferral CMSObj, string btn)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["CCPH"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            //IWAmountTmpProcess jt;
            int uid = (int)Session["uid"];//That will be Session value.
            string payeename, ClntCode;
            try
            {
                payeename = Request.Form["txtpayee"].ToString();
                ClntCode = Request.Form["txtClntCode"].ToString();
            }
            catch
            {
                payeename = null;
                ClntCode = null;
            }
            try
            {


                if (btn == "Accept")
                {



                    OWpro.CMSClientCodeUpdate(CMSObj.ID, CMSObj.RawDataID, uid, payeename, 5, null, "A", @Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"),
                        CMSObj.CustomerId, CMSObj.DomainId, CMSObj.ScanningNodeId, ClntCode, null, null);
                }
                else if (btn == "Reject")
                {

                    // payeename = Request.Form["txtpayee"].ToString();

                    OWpro.CMSClientCodeUpdate(CMSObj.ID, CMSObj.RawDataID, uid, payeename, 6, 88, "R", @Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"),
                  CMSObj.CustomerId, CMSObj.DomainId, CMSObj.ScanningNodeId, ClntCode, null, Request.Form["RejectDesc"].ToString());
                }
                else if (btn == "Hold")
                {
                    // payeename = Request.Form["txtpayee"].ToString();

                    OWpro.CMSClientCodeUpdate(CMSObj.ID, CMSObj.RawDataID, uid, payeename, 7, null, "H", @Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"),
                  CMSObj.CustomerId, CMSObj.DomainId, CMSObj.ScanningNodeId, ClntCode, null, null);
                }
                else if (btn == "Get Client Name")
                {
                    string ac = Request.Form["ClientCode"].ToString();
                    string Brcode = Request.Form["BranchCode"].ToString();
                    string subreq;
                    try
                    { subreq = Request.Form["SubClntReq"].ToString(); }
                    catch
                    { subreq = ""; }

                    
                        ////var customer = (from c in af.CMS_CustomerMaster
                        ////                where c.Customer_Code == ac
                        ////                select new { c.Customer_Name, c.Customer_s_Subcustomer_Required }).SingleOrDefault();
                        ////if (customer != null)
                        ////{
                        ////    ViewBag.customer = customer.Customer_Name;
                        ////ViewBag.ClntCode = ac;
                        ////ViewBag.Subcustomer_Required = customer.Customer_s_Subcustomer_Required;

                           
                               

                        ////}
                        ////else
                        ////    ViewBag.customer = null;
                        ////    ViewBag.MsgType = "ClientCode";

                        var customer = (from c in af.CMS_CustomerMaster
                                        from p in af.CMS_CustomerProductMaster
                                        orderby p.customer_prod_effective_date descending
                                        where c.Customer_Code == ac && p.customer_product_code == "LCC" && c.Customer_Code == p.customer_code &&
                                        p.customer_prod_effective_date != null
                                        select new { c.Customer_Name, p.customer_prod_effective_date, p.customer_prod_subcustomer_mand, c.Customer_s_Subcustomer_Required }).SingleOrDefault();

                        if (customer != null)
                        {

                            //---------------Location serching-----
                            var branchlocation = (from s in af.CMS_CustomerProductLocationMaster
                                                  from o in af.CMS_OurBranchMaster
                                                  where s.Customer_Code == ac && o.OUR_BRANCH_CODE ==Convert.ToInt32(Brcode) && s.Location_Code == o.OUR_BRANCH_LOCATION_CODE &&
                                                  s.Product_Code == "LCC" //&& s.Effective_Date == customer.customer_prod_effective_date
                                                  select s.Customer_Code
                                             ).ToList();

                            if (branchlocation.Count() != 0)
                            {
                                var suspanded = (from s in af.CMS_ProductSuspensionMaster//.CMS_ProductSuspensionMaster
                                                 orderby s.SUSREVPX_EFF_DATE descending
                                                 where s.SUSREVPX_CUST_CODE == ac && s.SUSREVPX_PROD_CODE == "LCC"
                                                 select new { s.SUSREVPX_STAT_FLAG }
                                             ).Take(1).FirstOrDefault();

                                if (suspanded == null)
                                {
                                    
                                    if (subreq == "Y")
                                    {
                                    
                                        string SubClntCode = Request.Form["SubClntCode"].ToString();
                                        var Subcustomer = (from c in af.CMS_SubCustomerMaster
                                                        where c.SUBCUST_SUB_CUST_CODE == SubClntCode
                                                        select new { c.SUBCUST_SUB_CUST_NAME }).SingleOrDefault();
                                        if (Subcustomer != null)
                                        {
                                            ViewBag.customer = Subcustomer.SUBCUST_SUB_CUST_NAME;
                                            ViewBag.ClntCode = ac;
                                            ViewBag.Sub_customer = SubClntCode;

                                        }
                                        else
                                            ViewBag.customer = null;
                                            ViewBag.MsgType = "SubClntCode";
                                    }
                                    else
                                    {

                                        ViewBag.customer = customer.Customer_Name;
                                        ViewBag.ClntCode = ac;
                                        ViewBag.Subcustomer_Required = customer.Customer_s_Subcustomer_Required;
                                    }

                                }
                                else
                                {
                                    if (suspanded.SUSREVPX_STAT_FLAG == "D")
                                        ViewBag.customer = "Customer is Suspended";
                                    else
                                    {
                                        ViewBag.customer = customer.Customer_Name;
                                        ViewBag.ClntCode = ac;
                                        ViewBag.Subcustomer_Required = customer.Customer_s_Subcustomer_Required;
                                    }
                                }

                            }
                            else
                            {
                                ViewBag.customer = null;
                                ViewBag.MsgType = "location";

                            }
                               
                            //---------------Suspendead----------------------------------



                        }
                        else
                        {
                            ViewBag.customer = null;
                            ViewBag.MsgType = "ClientCode";
                        }
                            



                   



                    return PartialView("GetClientDlts", CMSObj);
                }
                return RedirectToAction("ClientCode", new { ProcType = Request.Form["ProcType"].ToString() });
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "CMSClientCodeMissing HttpPost ClientCode- " + innerExcp });
            }



        }

        public ActionResult ShowCMSChq(int CustomerId = 0, int DomainId = 0, int ScanningNodeId = 0, int SlipNo = 0, int BatchNo = 0, string BtnCicked = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }





            try
            {
                SqlDataAdapter adp = new SqlDataAdapter();
                if (BtnCicked == "btnShowChq")
                {
                    adp = new SqlDataAdapter("select top 1 FrontGreyImagePath,BackGreyImagePath from L1Verification with (nolock) where ProcessingDate='" + Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd") + "' and CustomerId=" + CustomerId + " and DomainId=" + DomainId + " and ScanningNodeId=" + ScanningNodeId + "  and SlipNo=" + SlipNo + " and BatchNo=" + BatchNo + " and InstrumentType='C'", con);
                }
                else if (BtnCicked == "btnSupDoc")
                {
                    adp = new SqlDataAdapter("select top 1 WebDocumentPath from SupportingDocRawData with (nolock) where ProcessingDate='" + Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd") + "' and CustomerId=" + CustomerId + " and DomainId=" + DomainId + " and ScanningNodeId=" + ScanningNodeId + "  and SlipNo=" + SlipNo + " and BatchNo=" + BatchNo + "", con);
                }

                adp.SelectCommand.CommandType = CommandType.Text;

                DataSet ds = new DataSet();
                adp.Fill(ds);


                if (ds.Tables.Count > 0)
                {

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //CMSObj.ID = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]);
                        //CMSObj.CustomerId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[1]);
                        //CMSObj.DomainId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[2]);
                        //CMSObj.ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[3]);
                        //CMSObj.Status = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[4]);
                        //CMSObj.PayeeName = ds.Tables[0].Rows[0].ItemArray[5].ToString();

                        if (BtnCicked == "btnShowChq")
                        {
                            CMSObj.FrontGrayImagePath = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                            CMSObj.BackGreyImagePath = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                        }
                        else if (BtnCicked == "btnSupDoc")
                        {
                            CMSObj.FrontGrayImagePath = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                            CMSObj.BackGreyImagePath = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                        }



                        //CMSObj.SlipNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[7]);
                        //CMSObj.RawDataID = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[8]);



                        return Json(CMSObj);
                    }
                    else
                        return Json(true);
                }
                else
                    return Json(true);
            }
            catch (Exception e)
            {
                return Json(true);
            }


        }

        public PartialViewResult GetClientDlts(string ac = null)
        {
            var customer = (from c in af.CMS_CustomerMaster
                            where c.Customer_Code == ac
                            select new { c.Customer_Name, c.Customer_s_Subcustomer_Required }).SingleOrDefault();
            if (customer != null)
            {
                ViewBag.customer = customer.Customer_Name;
                ViewBag.Sub_customer = customer.Customer_s_Subcustomer_Required;
            }
            else
                ViewBag.customer = null;


            return PartialView("GetClientDlts");
        }

    }
}
