using ikloud_Aflatoon.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ikloud_Aflatoon.Controllers
{
    //[OutputCache(Duration = 0)]
    public class OWSearchController : Controller
    {
        //
        // GET: /OWSearch/
        AflatoonEntities af = new AflatoonEntities();
        OWProcDataContext OWpro = new OWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);

        public ActionResult Query()
        {

            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["Query"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            return View();
        }

        [HttpPost]
        public ActionResult Query(List<string> extrafields = null, string P2f = null, string EOD = null) //IWSearch iwsrch
        {
            string strFromDate = null, strToDate = null, strCheqNo = null, strSortCode = null, strTransCode = null, strAccountno = null;
            decimal amount = 0;
            try
            {

                if (extrafields[0] != null && extrafields[0] != "")
                    strFromDate = extrafields[0].ToString();
                if (extrafields[1] != null && extrafields[1] != "")
                    strToDate = extrafields[1].ToString();
                if (extrafields[2] != null && extrafields[2] != "")
                    strCheqNo = extrafields[2].ToString();
                if (extrafields[3] != null && extrafields[3] != "")
                    amount = Convert.ToDecimal(extrafields[3].ToString());
                else
                    amount = 0;
                if (extrafields[4] != null && extrafields[4] != "")
                    strAccountno = extrafields[4].ToString();
                if (extrafields[5] != null && extrafields[5] != "")
                    strSortCode = extrafields[5].ToString();
                if (extrafields[6] != null && extrafields[6] != "")
                    strTransCode = extrafields[6].ToString();



                if (strFromDate != null)
                {
                    DateTime Totempdate = new DateTime();
                    string tempDateFrom = strFromDate.Substring(6, 4) + "-" + strFromDate.Substring(3, 2) + "-" + strFromDate.Substring(0, 2);
                    string tempDateTo = "";
                    DateTime tempdate = Convert.ToDateTime(tempDateFrom);
                    if (strToDate != null)
                    {
                        tempDateTo = strToDate.Substring(6, 4) + "-" + strToDate.Substring(3, 2) + "-" + strToDate.Substring(0, 2);
                        Totempdate = Convert.ToDateTime(tempDateTo);
                    }

                    int custid = Convert.ToInt16(Session["CustomerID"]);

                    //---------------
                    if (EOD == null || EOD == "")
                    {
                        var Owsr = (from r in af.MainTransaction
                                    where (r.ProcessingDate >= tempdate) && (r.ProcessingDate <= Totempdate || Totempdate == null) && (r.CustomerId == custid) && (r.ChequeNoFinal == strCheqNo || strCheqNo == null) &&
                                    (r.FinalAmount == amount || amount == 0) && (r.FinalAccountNo == strAccountno || strAccountno == null) && (r.SortCodeFinal == strSortCode || strSortCode == null)
                                    && (r.TransCodeFinal == strTransCode || strTransCode == null) && (P2f == "true" ? r.DocType == "C" : r.DocType == r.DocType)
                                    select new IWSearch
                                    {
                                        ID = r.Id,
                                        XMLSerialNo = r.ChequeNoFinal,
                                        XMLAmount = r.FinalAmount,
                                        XMLPayeeName = r.PayeeName,
                                        XMLPayorBankRoutNo = r.SortCodeFinal,
                                        XMLSAN = r.SANFinal,
                                        XMLTrns = r.TransCodeFinal,
                                        FrontGreyImagePath = r.FrontGreyImagePath.ToString(),
                                        chiStatus = r.CHIStatus.ToString(),
                                        P2F = r.DocType,
                                        chequedate = r.FinalDate,
                                        CreditAccountNo = r.CreditAccountNo,

                                    }).ToList();

                        return PartialView("_OwSearchCheques", Owsr);
                    }
                    else
                    {
                        var Owsr = (from r in OWpro.EOD_MainTransactions
                                    where (r.ProcessingDate >= tempdate) && (r.ProcessingDate <= Totempdate || Totempdate == null) && (r.CustomerId == custid) && (r.ChequeNoFinal == strCheqNo || strCheqNo == null) &&
                                    (r.FinalAmount == amount || amount == 0) && (r.FinalAccountNo == strAccountno || strAccountno == null) && (r.SortCodeFinal == strSortCode || strSortCode == null)
                                    && (r.TransCodeFinal == strTransCode || strTransCode == null) && (P2f == "true" ? r.DocType == "C" : r.DocType == r.DocType)
                                    select new IWSearch
                                    {
                                        ID = r.Id,
                                        XMLSerialNo = r.ChequeNoFinal,
                                        XMLAmount = r.FinalAmount,
                                        XMLPayeeName = r.PayeeName,
                                        XMLPayorBankRoutNo = r.SortCodeFinal,
                                        XMLSAN = r.SANFinal,
                                        XMLTrns = r.TransCodeFinal,
                                        FrontGreyImagePath = r.FrontGreyImagePath.ToString(),
                                        chiStatus = r.CHIStatus.ToString(),
                                        P2F = r.DocType,
                                        chequedate = r.FinalDate,
                                        CreditAccountNo = r.CreditAccountNo,

                                    }).ToList();

                        return PartialView("_OwSearchCheques", Owsr);
                    }

                }
                else
                {
                    return PartialView("_SearchCheques");
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
        public ActionResult CheqDetls(Int64 id, string EOD = null)
        {
            try
            {

                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter("OWGetCheque", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@id", SqlDbType.BigInt).Value = id;
                adp.SelectCommand.Parameters.Add("@EOD", SqlDbType.NVarChar).Value = EOD;

                adp.Fill(ds);
                IWSearch Owsr = null;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        Owsr = new IWSearch
                        {
                            ID = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[0]),
                            XMLSerialNo = ds.Tables[0].Rows[index].ItemArray[1].ToString(),
                            XMLAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[2].ToString()),
                            XMLPayeeName = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                            XMLPayorBankRoutNo = ds.Tables[0].Rows[index].ItemArray[4].ToString(),
                            XMLSAN = ds.Tables[0].Rows[index].ItemArray[5].ToString(),
                            XMLTrns = ds.Tables[0].Rows[index].ItemArray[6].ToString(),
                            DocType = ds.Tables[0].Rows[index].ItemArray[7].ToString(),
                            PresentmentDate = ds.Tables[0].Rows[index].ItemArray[8].ToString(),
                            BOFDRoutNo = ds.Tables[0].Rows[index].ItemArray[9].ToString(),
                            MICRRepairFlags = ds.Tables[0].Rows[index].ItemArray[10].ToString(),
                            FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[11].ToString(),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[12].ToString(),
                            BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[13].ToString(),
                            chiStatus = ds.Tables[0].Rows[index].ItemArray[14].ToString(),
                            L1VerificationName = ds.Tables[0].Rows[index].ItemArray[15].ToString(),
                            L2VerificationName = ds.Tables[0].Rows[index].ItemArray[16].ToString(),
                            L3VerificationName = ds.Tables[0].Rows[index].ItemArray[17].ToString(),
                            L1VerificationAction = ds.Tables[0].Rows[index].ItemArray[18].ToString(),
                            L2VerificationAction = ds.Tables[0].Rows[index].ItemArray[19].ToString(),
                            L3VerificationAction = ds.Tables[0].Rows[index].ItemArray[20].ToString(),
                            L1RejectReason = ds.Tables[0].Rows[index].ItemArray[21].ToString(),
                            L2RejectReason = ds.Tables[0].Rows[index].ItemArray[22].ToString(),
                            L3RejectReason = ds.Tables[0].Rows[index].ItemArray[23].ToString(),
                            chequedate = ds.Tables[0].Rows[index].ItemArray[24].ToString(),
                            CreditAccountNo = ds.Tables[0].Rows[index].ItemArray[25].ToString(),
                            CustomerID = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[26].ToString()),
                            ScanningID = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[27].ToString()),
                            BranchCode = ds.Tables[0].Rows[index].ItemArray[28].ToString(),
                            BatchNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[29].ToString()),
                            SlipNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[30].ToString())



                        };
                        count = count - 1;
                        index = index + 1;
                    }
                    if (Owsr.PresentmentDate != null && Owsr.PresentmentDate != "")
                        Owsr.PresentmentDate = Convert.ToDateTime(Owsr.PresentmentDate).Date.ToString("dd-MM-yyyy");
                    if (Owsr.chequedate != null && Owsr.chequedate != "")
                        Owsr.chequedate = Convert.ToDateTime(Owsr.chequedate).Date.ToString("dd-MM-yyyy");

                    //-----------------------L1action---------------------------
                    if (Owsr.L1VerificationAction != null && Owsr.L1VerificationAction != "")
                    {
                        if (Owsr.L1VerificationAction == "2")
                            Owsr.L1VerificationAction = "Y";
                        else if (Owsr.L1VerificationAction == "3")
                            Owsr.L1VerificationAction = "R";
                        else if (Owsr.L1VerificationAction == "0")
                            Owsr.L1VerificationAction = "N";

                    }
                    //-----------------------L2action---------------------------
                    if (Owsr.L2VerificationAction != null && Owsr.L2VerificationAction != "")
                    {
                        if (Owsr.L2VerificationAction == "2")
                            Owsr.L2VerificationAction = "Y";
                        else if (Owsr.L2VerificationAction == "3")
                            Owsr.L2VerificationAction = "R";
                        else if (Owsr.L2VerificationAction == "8")
                            Owsr.L2VerificationAction = "M";
                        else if (Owsr.L2VerificationAction == "0")
                            Owsr.L2VerificationAction = "N";

                    }
                    //-----------------------L3action---------------------------
                    if (Owsr.L3VerificationAction != null && Owsr.L3VerificationAction != "")
                    {
                        if (Owsr.L3VerificationAction == "2")
                            Owsr.L3VerificationAction = "Y";
                        else if (Owsr.L3VerificationAction == "3")
                            Owsr.L3VerificationAction = "R";
                        else if (Owsr.L3VerificationAction == "6")
                            Owsr.L3VerificationAction = "M";
                        else if (Owsr.L3VerificationAction == "0")
                            Owsr.L3VerificationAction = "N";

                    }
                    //-----------------------L1User Name---------------------------
                    if (Owsr.L1VerificationName != null && Owsr.L1VerificationName != "" && Owsr.L1VerificationName != "0")
                    {
                        int L1id = Convert.ToInt16(Owsr.L1VerificationName);
                        var l1result = af.UserMasters.Where(m => m.ID == L1id).FirstOrDefault();
                        Owsr.L1VerificationName = l1result.LoginID;
                    }
                    //-----------------------L2User Name---------------------------
                    if (Owsr.L2VerificationName != null && Owsr.L2VerificationName != "" && Owsr.L2VerificationName != "0")
                    {
                        int L2id = Convert.ToInt16(Owsr.L2VerificationName);
                        var l2result = af.UserMasters.Where(m => m.ID == L2id).FirstOrDefault();
                        Owsr.L2VerificationName = l2result.LoginID;
                    }
                    //-----------------------L3User Name---------------------------
                    if (Owsr.L3VerificationName != null && Owsr.L3VerificationName != "" && Owsr.L3VerificationName != "0")
                    {
                        int L3id = Convert.ToInt16(Owsr.L3VerificationName);
                        var l3result = af.UserMasters.Where(m => m.ID == L3id).FirstOrDefault();
                        Owsr.L3VerificationName = l3result.LoginID;
                    }
                    //-------------------------Reject reason L1----------------------//
                    if (Owsr.L1RejectReason != null && Owsr.L1RejectReason != "" && Owsr.L1RejectReason != "0")
                    {
                        string l1reason = Owsr.L1RejectReason.PadLeft(2, '0');

                        var L1Retrn = af.ItemReturnReasons.Where(m => m.RETURN_REASON_CODE == l1reason).FirstOrDefault();
                        Owsr.L1RejectReason = L1Retrn.DESCRIPTION;
                    }
                    //-------------------------Reject reason L2----------------------//
                    if (Owsr.L2RejectReason != null && Owsr.L2RejectReason != "" && Owsr.L2RejectReason != "0")
                    {
                        string l2reason = Owsr.L2RejectReason.PadLeft(2, '0');
                        var L2Retrn = af.ItemReturnReasons.Where(m => m.RETURN_REASON_CODE == l2reason).FirstOrDefault();
                        Owsr.L2RejectReason = L2Retrn.DESCRIPTION;
                    }
                    //-------------------------Reject reason L3----------------------//
                    if (Owsr.L3RejectReason != null && Owsr.L3RejectReason != "" && Owsr.L3RejectReason != "0")
                    {
                        string l3reason = Owsr.L3RejectReason.PadLeft(2, '0');

                        var L3Retrn = af.ItemReturnReasons.Where(m => m.RETURN_REASON_CODE == l3reason).FirstOrDefault();
                        Owsr.L3RejectReason = L3Retrn.DESCRIPTION;
                    }

                    return PartialView("_OwCheqDetails", Owsr);
                }
                else
                {
                    return PartialView(false);
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
        public PartialViewResult RejectReason()
        {
            // string[] code = { "13", "30", "31", "32", "34", "35", "66" };
            var rjrs = (from r in af.ItemReturnReasons
                        select new RejectReason
                        {
                            Description = r.DESCRIPTION,
                            ReasonCodeS = r.RETURN_REASON_CODE
                        });
            return PartialView("_OwRejectDetails", rjrs);
        }
        //-------------------------Added On 24/07/2017-----------------------For Slip Image-----------
        public JsonResult slipImage(DateTime processingdate, int CustomerID = 0, int scanningNodeID = 0, string BranchCode = null, int batchNo = 0, int SlipNo = 0)
        {
            var owL1 = (from m in af.MainTransaction
                        where m.ProcessingDate == processingdate &&
            m.CustomerId == CustomerID && m.ScanningNodeId == scanningNodeID && m.BranchCode == BranchCode && m.BatchNo == batchNo && m.SlipNo == SlipNo
            && m.InstrumentType == "S"

                        select new
                        {
                            m.FrontGreyImagePath,
                            m.BackGreyImagePath
                        }).SingleOrDefault();




            return Json(owL1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getTiffImage(string httpwebimgpath = null)
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
                System.IO.File.AppendAllText("C:\\temp\\log1.txt", "actualpathOWSearch:" + actualpath);


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
            }
            catch (Exception ex)
            {

                System.IO.File.AppendAllText("C:\\temp\\log1.txt", "gettiffiame error :" + ex.Message.ToString());
            }


            return PartialView("_getTiffImage");
            //return Json(imageDataURL, JsonRequestBehavior.AllowGet);

        }

    }

}
