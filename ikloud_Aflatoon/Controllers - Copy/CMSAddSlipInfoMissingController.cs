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
    public class CMSAddSlipInfoMissingController : Controller
    {
        //
        // GET: /CMSClientCodeMissing/
        AflatoonEntities af = new AflatoonEntities();
        OWProcDataContext OWpro = new OWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
        CMSReferral CMSObj = new CMSReferral();
        // Boolean bSubClnt;


        public ActionResult ClientCodeSelection(int id = 0, Int64 LockID = 0, string LockModule = null, string InstrumentType = null)
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

            if (LockModule == "AT")
            {
                OWpro.CMSUnlockRecords(LockID, LockModule);
            }

            if (id == 1)
                ViewBag.msg = "No Data Found!!..";

            Session["instType"] = InstrumentType;

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

            if (btn == "Close")
            {
                return RedirectToAction("IWIndex", "Home");
            }


            return RedirectToAction("AdditionalDataEntry", new { ProcType = Request.Form["ClientCode"].ToString() });


        }


        public ActionResult AdditionalDataEntry(string ProcType)
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

                SqlDataAdapter adp = new SqlDataAdapter("CMSAddSlipInfoMissing", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                adp.SelectCommand.Parameters.Add("@ProcType", SqlDbType.NVarChar).Value = ProcType;
                adp.SelectCommand.Parameters.Add("@InstrumentType", SqlDbType.NVarChar).Value = Session["instType"].ToString();

                DataSet ds = new DataSet();
                adp.Fill(ds);
                // var objectlst = new List<L1Verification>();
                CMSCustAdditionalInput cmsprodMstr = new CMSCustAdditionalInput();
                if (ds.Tables.Count > 0)
                {

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //cmsprodMstr = new CMSCustAdditionalInput
                        //{
                        cmsprodMstr.ProdEffectiveDate = Convert.ToDateTime(ds.Tables[0].Rows[0].ItemArray[0]);
                        cmsprodMstr.ProdDivisionRequired = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                        cmsprodMstr.ProdDivisionMandate = ds.Tables[0].Rows[0].ItemArray[2].ToString();
                        cmsprodMstr.ProdHierRequired = ds.Tables[0].Rows[0].ItemArray[3].ToString();
                        cmsprodMstr.ProdHierMandate = ds.Tables[0].Rows[0].ItemArray[4].ToString();
                        cmsprodMstr.InstrumentDtlsRequired = ds.Tables[0].Rows[0].ItemArray[5].ToString();
                        cmsprodMstr.InstrumentDtlsMandate = ds.Tables[0].Rows[0].ItemArray[6].ToString();
                        cmsprodMstr.ProdDrawerRequired = ds.Tables[0].Rows[0].ItemArray[7].ToString();
                        cmsprodMstr.ProdDrawerMandate = ds.Tables[0].Rows[0].ItemArray[8].ToString();
                        cmsprodMstr.ProdAdditionalinfoRequired = ds.Tables[0].Rows[0].ItemArray[9].ToString();
                        cmsprodMstr.ProdAdditionalinfoMandate = ds.Tables[0].Rows[0].ItemArray[10].ToString();
                        cmsprodMstr.ProdSubcustomerRequired = ds.Tables[0].Rows[0].ItemArray[11].ToString();
                        cmsprodMstr.ProdSubcustomerMandate = ds.Tables[0].Rows[0].ItemArray[12].ToString();
                        cmsprodMstr.ProdSubCustomerEntrylevel = ds.Tables[0].Rows[0].ItemArray[13].ToString();
                        cmsprodMstr.ProdlccPickupRequired = ds.Tables[0].Rows[0].ItemArray[14].ToString();
                        cmsprodMstr.ClientCode = ds.Tables[0].Rows[0].ItemArray[15].ToString();
                        cmsprodMstr.FrontGreyImagePath = ds.Tables[0].Rows[0].ItemArray[16].ToString();
                        cmsprodMstr.BranchCode = ds.Tables[0].Rows[0].ItemArray[17].ToString();
                        cmsprodMstr.BatchNo = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[18].ToString());
                        cmsprodMstr.ScanningNodeId = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[19].ToString());
                        cmsprodMstr.SlipNo = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[20].ToString());
                        cmsprodMstr.ProdCode = ds.Tables[0].Rows[0].ItemArray[21].ToString();
                        cmsprodMstr.ID = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[22]);
                        cmsprodMstr.Status = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[23]);
                        cmsprodMstr.InstrumentType = ds.Tables[0].Rows[0].ItemArray[24].ToString();
                        cmsprodMstr.BackGreyImagePath = ds.Tables[0].Rows[0].ItemArray[25].ToString();
                        cmsprodMstr.ClientName = ds.Tables[0].Rows[0].ItemArray[26].ToString();
                        cmsprodMstr.CustomerId = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[27]);
                        cmsprodMstr.DivisionCode = ds.Tables[0].Rows[0].ItemArray[28].ToString();
                        cmsprodMstr.HiratchyCode = ds.Tables[0].Rows[0].ItemArray[29].ToString();
                        cmsprodMstr.CustDrawerCode = ds.Tables[0].Rows[0].ItemArray[30].ToString();
                        cmsprodMstr.SubcustomerCode = ds.Tables[0].Rows[0].ItemArray[31].ToString();
                        cmsprodMstr.ProdLCCPickup = ds.Tables[0].Rows[0].ItemArray[32].ToString();
                        cmsprodMstr.SlipRefNo = ds.Tables[0].Rows[0].ItemArray[33].ToString();
                        cmsprodMstr.Legend = "Slip Additional Information";
                        // };

                        var mode = (from m in af.CMS_CustomerProductAdditionaInfo

                                    where m.cust_code == cmsprodMstr.ClientCode && m.prod_code == cmsprodMstr.ProdCode && m.customer_production_deposit_inst_level == "D"
                                   && m.customer_prod_eff_date == cmsprodMstr.ProdEffectiveDate
                                    orderby m.customer_additional_info_dtl_sl
                                    select new { m.customer_legend, m.CUSPAI_DATA_SIZE, m.customer_additional_Mandatore, m.ID, m.customer_addition_datatype, m.customer_master_serial }).ToList();

                        if (mode.Count() != 0 || mode != null)
                        {
                            for (int i = 0; i < mode.Count; i++)
                            {
                                if (i == 0)
                                {
                                    cmsprodMstr.Additional1Legend = mode[i].customer_legend;
                                    cmsprodMstr.Additional1mandate = mode[i].customer_additional_Mandatore;
                                    cmsprodMstr.Additional1ID = mode[i].ID;
                                    cmsprodMstr.Additional1_CUSPAI_DATA_SIZE = mode[i].CUSPAI_DATA_SIZE;
                                    cmsprodMstr.Additional1_addition_datatype = mode[i].customer_addition_datatype;
                                    cmsprodMstr.Additional1_master_serial = mode[i].customer_master_serial;

                                }
                                else if (i == 1)
                                {
                                    cmsprodMstr.Additional2Legend = mode[i].customer_legend;
                                    cmsprodMstr.Additional2mandate = mode[i].customer_additional_Mandatore;
                                    cmsprodMstr.Additional2ID = mode[i].ID;
                                    cmsprodMstr.Additional2_CUSPAI_DATA_SIZE = mode[i].CUSPAI_DATA_SIZE;
                                    cmsprodMstr.Additional2_addition_datatype = mode[i].customer_addition_datatype;
                                    cmsprodMstr.Additional2_master_serial = mode[i].customer_master_serial;
                                }
                                else if (i == 2)
                                {
                                    cmsprodMstr.Additional3Legend = mode[i].customer_legend;
                                    cmsprodMstr.Additional3mandate = mode[i].customer_additional_Mandatore;
                                    cmsprodMstr.Additional3ID = mode[i].ID;
                                    cmsprodMstr.Additional3_CUSPAI_DATA_SIZE = mode[i].CUSPAI_DATA_SIZE;
                                    cmsprodMstr.Additional3_addition_datatype = mode[i].customer_addition_datatype;
                                    cmsprodMstr.Additional3_master_serial = mode[i].customer_master_serial;
                                }
                                else if (i == 3)
                                {
                                    cmsprodMstr.Additional4Legend = mode[i].customer_legend;
                                    cmsprodMstr.Additional4mandate = mode[i].customer_additional_Mandatore;
                                    cmsprodMstr.Additional4ID = mode[i].ID;
                                    cmsprodMstr.Additional4_CUSPAI_DATA_SIZE = mode[i].CUSPAI_DATA_SIZE;
                                    cmsprodMstr.Additional4_addition_datatype = mode[i].customer_addition_datatype;
                                    cmsprodMstr.Additional4_master_serial = mode[i].customer_master_serial;
                                }
                                else if (i == 4)
                                {
                                    cmsprodMstr.Additional5Legend = mode[i].customer_legend;
                                    cmsprodMstr.Additional5mandate = mode[i].customer_additional_Mandatore;
                                    cmsprodMstr.Additional5ID = mode[i].ID;
                                    cmsprodMstr.Additional5_CUSPAI_DATA_SIZE = mode[i].CUSPAI_DATA_SIZE;
                                    cmsprodMstr.Additional5_addition_datatype = mode[i].customer_addition_datatype;
                                    cmsprodMstr.Additional5_master_serial = mode[i].customer_master_serial;
                                }
                            }
                        }

                        ViewBag.cnt = true;
                        @Session["glob"] = null;
                        return View(cmsprodMstr);
                    }
                    else
                    {
                        return RedirectToAction("ClientCodeSelection", "CMSAddSlipInfoMissing", new { id = 1 });// return RedirectToAction("DeSelection", new { id = 1 });
                    }
                }
                else
                {
                    return RedirectToAction("ClientCodeSelection", "CMSAddSlipInfoMissing", new { id = 1 });// return RedirectToAction("DeSelection", new { id = 1 });
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

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "CMSClientCodeMissing HttpGet ClientCode- " + innerExcp });
            }
            //return View();
        }


        [HttpPost]
        public ActionResult AdditionalDataEntry(CMSCustAdditionalInput CMSObj, string btn)
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


            try
            {


                if (btn == "Accept")
                {



                    OWpro.CMSAddSlipInfoMissingUpdate(CMSObj.ID, uid, 4, "A", CMSObj.DivisionCode, CMSObj.HiratchyCode, CMSObj.CustDrawerCode, CMSObj.SubcustomerCode,CMSObj.ProdLCCPickup, CMSObj.SlipRefNo,
                        Convert.ToInt16(CMSObj.Additional1ID), CMSObj.Additional1, Convert.ToInt16(CMSObj.Additional2ID), CMSObj.Additional2, Convert.ToInt16(CMSObj.Additional3ID), CMSObj.Additional3, Convert.ToInt16(CMSObj.Additional4ID), CMSObj.Additional4, Convert.ToInt16(CMSObj.Additional5ID), CMSObj.Additional5);

                }

                else if (btn == "Hold")
                {
                    OWpro.CMSAddSlipInfoMissingUpdate(CMSObj.ID, uid, 5, "H", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);

                    //  OWpro.CMSClientCodeUpdate(CMSObj.ID, CMSObj.RawDataID, uid, payeename, 7, null, "H", @Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"),
                    //CMSObj.CustomerId, CMSObj.DomainId, CMSObj.ScanningNodeId, ClntCode, null, null);
                }

                return RedirectToAction("AdditionalDataEntry", new { ProcType = Request.Form["ProcType"].ToString() });
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

        public ActionResult ShowCMSChq(int CustomerId = 0, int branchcode = 0, int ScanningNodeId = 0, int SlipNo = 0, int BatchNo = 0, string BtnCicked = null)
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
                    adp = new SqlDataAdapter("select top 1 FrontGreyImagePath,BackGreyImagePath from L1Verification with (nolock) where ProcessingDate='" + Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd") + "' and CustomerId=" + CustomerId + " and branchcode=" + branchcode + " and ScanningNodeId=" + ScanningNodeId + "  and SlipNo=" + SlipNo + " and BatchNo=" + BatchNo + " and InstrumentType='C'", con);
                }
                else if (BtnCicked == "btnSupDoc")
                {
                    adp = new SqlDataAdapter("select top 1 WebDocumentPath from SupportingDocRawData with (nolock) where ProcessingDate='" + Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd") + "' and CustomerId=" + CustomerId + " and branchcode=" + branchcode + " and ScanningNodeId=" + ScanningNodeId + "  and SlipNo=" + SlipNo + " and BatchNo=" + BatchNo + "", con);
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


        public ActionResult ValidateMaster(string instrumentType = null, string Fieldname = null, string FieldValue = null, string additionalval = null, string CustCode = null)
        {
            //if (Fieldname=="HiratchyCode")
            //{
            //[CMS_SPValidateMasters]
            try
            {
                SqlDataAdapter Chequeadp = new SqlDataAdapter("CMS_SPValidateMasters", con);
                Chequeadp.SelectCommand.CommandType = CommandType.StoredProcedure;
                Chequeadp.SelectCommand.Parameters.Add("@Tablename", SqlDbType.NVarChar).Value = Fieldname;
                Chequeadp.SelectCommand.Parameters.Add("@Fieldvalue", SqlDbType.NVarChar).Value = FieldValue;
                Chequeadp.SelectCommand.Parameters.Add("@Additionalinfo", SqlDbType.NVarChar).Value = additionalval;
                Chequeadp.SelectCommand.Parameters.Add("@CustomerCode", SqlDbType.NVarChar).Value = CustCode;

                SqlDataReader sqdreader;
                if (con.State == ConnectionState.Broken || con.State == ConnectionState.Closed)
                    con.Open();


                Chequeadp.SelectCommand.Connection = con;

                sqdreader = Chequeadp.SelectCommand.ExecuteReader();
                if (sqdreader.Read())
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                //}

                return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                //Server.MapPath(strMappath);
                // ErrorDisplay er = new ErrorDisplay();
                //er.ErrorMessage = e.Message.ToString();
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
                //return View("Error", er);
            }
        }

    }
}
