using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ikloud_Aflatoon.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace ikloud_Aflatoon.Controllers
{
    public class CMSDataEntryController : Controller
    {
        //
        // GET: /CMSDataEntry/
        AflatoonEntities af = new AflatoonEntities();
        OWProcDataContext OWpro = new OWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
        [Authorize]
        public ActionResult UploadDataEntry()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadDataEntry(int id = 0)
        {
            return View();
        }

        public ActionResult AdditionalDataEntry(int id = 0)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            DateTime processdate = Convert.ToDateTime(Session["processdate"].ToString());
            var allclient = (from a in af.CMSAdditionalTransactions
                             where a.ProcessingDate == processdate && a.ClientCode != "" &&
                             a.InstrumentType == "S"
                             group a by new { a.ClientCode } into g
                             select g.Key.ClientCode
                               ).ToList();
            if (allclient.Count() != 0 || allclient != null)
            {
                ViewBag.ClientCode = allclient;
            }
            //if (id==2)
            //{
            //    ViewBag.msg = "No Data was Found!!";
            //    return PartialView("AdditionalDataEntry");
            //}


            return View();
        }

        [HttpPost]
        public ActionResult AdditionalDataEntry(string ClientCode = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            try
            {

                SqlDataAdapter adp = new SqlDataAdapter("SelectAdditionalSlip", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Session["processdate"].ToString();
                adp.SelectCommand.Parameters.Add("@ClientCode", SqlDbType.NVarChar).Value = ClientCode;

                DataSet ds = new DataSet();
                adp.Fill(ds);
                // var objectlst = new List<L1Verification>();
                CMSCustAdditionalInput cmsprodMstr = new CMSCustAdditionalInput();

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
                    cmsprodMstr.Legend = "Slip Additional Information";
                    cmsprodMstr.SlipChequeCount = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[26].ToString());
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
                    return PartialView("ClientSlipAdditional", cmsprodMstr);
                }
                else
                {
                    //return PartialView("ClientSlipAdditional", cmsprodMstr);
                    return Json(false);
                }
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
        [HttpPost]
        public ActionResult UpdateSlipAdditional(string cmsAddInput = null, List<string> extrafields = null)
        {
            DateTime prodEffectiveDate = new DateTime();
            CMSCustAdditionalInput cmsslipOrCheq = new CMSCustAdditionalInput();
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];//That will be Session value.
            string hirarchy = null, division = null, CustDrawerCode = null, SubcustomerCode = null, ProdLCCPickup = null, subcustomerEntryLevel = null,
                Additional1ID = null, Additional2ID = null, Additional3ID = null, Additional4ID = null,
                Additional5ID = null, Additional1 = null, Additional2 = null, Additional3 = null, Additional4 = null, Additional5 = null, SlipRefNo = null;
            try
            {
                JObject Obj = JObject.Parse(cmsAddInput);
                if (extrafields != null)
                {
                    if (extrafields[0] != null)
                        hirarchy = extrafields[0].ToString();
                    if (extrafields[1] != null)
                        division = extrafields[1].ToString();
                    if (extrafields[2] != null)
                        CustDrawerCode = extrafields[2].ToString();
                    if (extrafields[3] != null)
                        SubcustomerCode = extrafields[3].ToString();
                    if (extrafields[4] != null)
                        ProdLCCPickup = extrafields[4].ToString();
                    if (extrafields[5] != null)
                        Additional1ID = extrafields[5].ToString();
                    if (extrafields[6] != null)
                        Additional2ID = extrafields[6].ToString();
                    if (extrafields[7] != null)
                        Additional3ID = extrafields[7].ToString();
                    if (extrafields[8] != null)
                        Additional4ID = extrafields[8].ToString();
                    if (extrafields[9] != null)
                        Additional5ID = extrafields[9].ToString();
                    if (extrafields[10] != null)
                        Additional1 = extrafields[10].ToString();
                    if (extrafields[11] != null)
                        Additional2 = extrafields[11].ToString();
                    if (extrafields[12] != null)
                        Additional3 = extrafields[12].ToString();
                    if (extrafields[13] != null)
                        Additional4 = extrafields[13].ToString();
                    if (extrafields[14] != null)
                        Additional5 = extrafields[14].ToString();
                    if (extrafields[15] != null)
                        SlipRefNo = extrafields[15].ToString();
                    if (extrafields[16] != null)
                        prodEffectiveDate = Convert.ToDateTime(extrafields[16].ToString());
                }


                if (Obj["InstrumentType"].ToString() == "S")
                    OWpro.UpdateCMSSlipAdditional(Convert.ToInt64(Obj["ID"].ToString()), uid, Convert.ToInt16(Obj["Status"].ToString()), Session["processdate"].ToString(), Obj["BranchCode"].ToString(),
                         Convert.ToInt16(Obj["BatchNo"].ToString()), Convert.ToInt16(Obj["ScanningNodeId"].ToString()), Convert.ToInt16(Obj["SlipNo"].ToString()), Obj["ProdCode"].ToString(),
                         Convert.ToString(Obj["ProdEffectiveDate"].ToString()), Obj["InstrumentDtlsRequired"].ToString(), division,
                         hirarchy, CustDrawerCode, SubcustomerCode, "S",
                          ProdLCCPickup, SlipRefNo, "5", Convert.ToInt16(Additional1ID),
                        Additional1, Convert.ToInt16(Additional2ID), Additional2, Convert.ToInt16(Additional3ID),
                       Additional3, Convert.ToInt16(Additional4ID), Additional4, Convert.ToInt16(Additional5ID),
                        Additional5,null);

                else if (Obj["InstrumentType"].ToString() == "C")
                    OWpro.UpdateCMSSlipAdditional(Convert.ToInt64(Obj["ID"].ToString()), uid, Convert.ToInt16(Obj["Status"].ToString()), Session["processdate"].ToString(), Obj["BranchCode"].ToString(),
                            Convert.ToInt16(Obj["BatchNo"].ToString()), Convert.ToInt16(Obj["ScanningNodeId"].ToString()), Convert.ToInt16(Obj["SlipNo"].ToString()), Obj["ProdCode"].ToString(),
                            Convert.ToString(Obj["ProdEffectiveDate"].ToString()), Obj["InstrumentDtlsRequired"].ToString(), division,
                            hirarchy, CustDrawerCode, SubcustomerCode, "C",
                             ProdLCCPickup, SlipRefNo, "5", Convert.ToInt16(Additional1ID),
                           Additional1, Convert.ToInt16(Additional2ID), Additional2, Convert.ToInt16(Additional3ID),
                          Additional3, Convert.ToInt16(Additional4ID), Additional4, Convert.ToInt16(Additional5ID),
                           Additional5,null);




                if (Obj["InstrumentDtlsRequired"].ToString() == "Y")
                {
                    //---------Call all cheques belonging to this slip--------------

                    SqlDataAdapter Chequeadp = new SqlDataAdapter("OWSelectCMSCheques", con);
                    Chequeadp.SelectCommand.CommandType = CommandType.StoredProcedure;
                    Chequeadp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                    Chequeadp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Session["processdate"].ToString();
                    Chequeadp.SelectCommand.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = Convert.ToInt16(Obj["BatchNo"].ToString());
                    Chequeadp.SelectCommand.Parameters.Add("@SlipNo", SqlDbType.NVarChar).Value = Convert.ToInt16(Obj["SlipNo"].ToString());
                    Chequeadp.SelectCommand.Parameters.Add("@ScanningNodeId", SqlDbType.NVarChar).Value = Convert.ToInt16(Obj["ScanningNodeId"].ToString());
                    Chequeadp.SelectCommand.Parameters.Add("@BranchCode", SqlDbType.NVarChar).Value = Obj["BranchCode"].ToString();

                    DataSet Chequeds = new DataSet();
                    Chequeadp.Fill(Chequeds);
                    // var objectlst = new List<L1Verification>();
                    CMSCustAdditionalInput cmsprodMstr = new CMSCustAdditionalInput();

                    if (Chequeds.Tables[0].Rows.Count > 0)
                    {
                        cmsprodMstr.ID = Convert.ToInt64(Chequeds.Tables[0].Rows[0].ItemArray[0]);
                        cmsprodMstr.Status = Convert.ToInt16(Chequeds.Tables[0].Rows[0].ItemArray[1]);
                        cmsprodMstr.BranchCode = Chequeds.Tables[0].Rows[0].ItemArray[2].ToString();
                        cmsprodMstr.BatchNo = Convert.ToInt16(Chequeds.Tables[0].Rows[0].ItemArray[3].ToString());
                        cmsprodMstr.ScanningNodeId = Convert.ToInt16(Chequeds.Tables[0].Rows[0].ItemArray[4].ToString());
                        cmsprodMstr.SlipNo = Convert.ToInt16(Chequeds.Tables[0].Rows[0].ItemArray[5].ToString());
                        cmsprodMstr.FrontGreyImagePath = Chequeds.Tables[0].Rows[0].ItemArray[7].ToString();
                        cmsprodMstr.BackGreyImagePath = Chequeds.Tables[0].Rows[0].ItemArray[8].ToString();
                        cmsprodMstr.InstrumentType = Chequeds.Tables[0].Rows[0].ItemArray[9].ToString();
                        cmsprodMstr.ClientCode = Chequeds.Tables[0].Rows[0].ItemArray[10].ToString();
                        cmsprodMstr.Legend = "Cheque Additional Information";
                        cmsprodMstr.ProdCode = Obj["ProdCode"].ToString();
                        cmsprodMstr.ProdEffectiveDate = prodEffectiveDate;
                        cmsprodMstr.InstrumentDtlsRequired = Obj["InstrumentDtlsRequired"].ToString();

                        cmsprodMstr.SlipChequeCount = Convert.ToInt32(Chequeds.Tables[0].Rows[0].ItemArray[11].ToString());


                        //------------------------------Check with CMS_CustomerProductAdditionaInfo table----------------

                        var mode = (from m in af.CMS_CustomerProductAdditionaInfo

                                    where m.cust_code == cmsprodMstr.ClientCode && m.prod_code == cmsprodMstr.ProdCode && m.customer_production_deposit_inst_level == "I"
                                    && m.customer_prod_eff_date == prodEffectiveDate
                                    orderby m.customer_additional_info_dtl_sl
                                    select new { m.customer_legend, m.CUSPAI_DATA_SIZE, m.customer_additional_Mandatore, m.ID, m.customer_addition_datatype, m.customer_master_serial }).ToList();

                        if (mode.Count() != 0)
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
                            return PartialView("ClientSlipAdditional", cmsprodMstr);
                        }
                        else
                        {
                            OWpro.UpdateCMSSlipAdditional(Convert.ToInt64(Obj["ID"].ToString()), uid, Convert.ToInt16(Obj["Status"].ToString()), Session["processdate"].ToString(), Obj["BranchCode"].ToString(),
                                              Convert.ToInt16(Obj["BatchNo"].ToString()), Convert.ToInt16(Obj["ScanningNodeId"].ToString()), Convert.ToInt16(Obj["SlipNo"].ToString()), Obj["ProdCode"].ToString(),
                                              Convert.ToString(Obj["ProdEffectiveDate"].ToString()), Obj["InstrumentDtlsRequired"].ToString(), division,
                                              hirarchy, CustDrawerCode, SubcustomerCode, "SLP",
                                               ProdLCCPickup, SlipRefNo, "5", Convert.ToInt16(Additional1ID),
                                             Additional1, Convert.ToInt16(Additional2ID), Additional2, Convert.ToInt16(Additional3ID),
                                            Additional3, Convert.ToInt16(Additional4ID), Additional4, Convert.ToInt16(Additional5ID),
                                             Additional5,null);

                            return Json(false);
                        }

                        //return PartialView("ClientSlipAdditional", cmsprodMstr);
                    }
                    else
                    {
                        //-----------update open slip------------------
                        OWpro.UpdateCMSSlipAdditional(Convert.ToInt64(Obj["ID"].ToString()), uid, Convert.ToInt16(Obj["Status"].ToString()), Session["processdate"].ToString(), Obj["BranchCode"].ToString(),
                                               Convert.ToInt16(Obj["BatchNo"].ToString()), Convert.ToInt16(Obj["ScanningNodeId"].ToString()), Convert.ToInt16(Obj["SlipNo"].ToString()), Obj["ProdCode"].ToString(),
                                               Convert.ToString(Obj["ProdEffectiveDate"].ToString()), Obj["InstrumentDtlsRequired"].ToString(), division,
                                               hirarchy, CustDrawerCode, SubcustomerCode, "SLP",
                                                ProdLCCPickup, SlipRefNo, "5", Convert.ToInt16(Additional1ID),
                                              Additional1, Convert.ToInt16(Additional2ID), Additional2, Convert.ToInt16(Additional3ID),
                                             Additional3, Convert.ToInt16(Additional4ID), Additional4, Convert.ToInt16(Additional5ID),
                                              Additional5,null);

                        return Json(false);
                    }
                }
                else
                    return Json(false);


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

        public ActionResult ValidateMaster(string instrumentType = null, string Fieldname = null, string FieldValue = null, string additionalval = null, string CustCode = null, string BranchCode = null)
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
                Chequeadp.SelectCommand.Parameters.Add("@BranchCode", SqlDbType.NVarChar).Value = BranchCode;

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

        //-------------------------Added On 24/07/2017-----------------------For Slip Image-----------
        public JsonResult slipImage(Int64 SlipId = 0)
        {
            var owL1 = af.L1Verification.Where(m => m.Id == SlipId).FirstOrDefault().FrontGreyImagePath;


            return Json(owL1, JsonRequestBehavior.AllowGet);
        }
    }
}
