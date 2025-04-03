using CrystalDecisions.CrystalReports.Engine;
using ikloud_Aflatoon.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ikloud_Aflatoon.Controllers
{
    public class IWReportDBSController : Controller
    {
        AflatoonEntities af = new AflatoonEntities();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //
        // GET: /IWReportDBS/

        public ActionResult Report()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            if ((bool)Session["Report"] == false)
            {
                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new
                {
                    msg = "Session Expired",
                    popmsg = "Malicious activity has been detected, your id has been disabled!!",
                    id = 1
                });
            }
            //  ViewBag.listuser = null;
            ViewBag.listuser = new SelectList(af.UserMasters.Where(u => u.L2StopLimit != 0 || u.L3StopLimit != 0), "LoginID", "LoginID").ToList();
            ViewBag.ProcessingDate = Convert.ToDateTime(Session["processdate"].ToString()).ToString("dd/MM/yyyy");

            ViewBag.BankCode = Session["BankCode"].ToString();

            ViewBag.Accesslevel = Session["Accesslevel"].ToString();
            ViewBag.clrtype = new SelectList(af.ClearingType, "Code", "Name").ToList();

            var ExpTimeList = GetExpiryTime();
            var selectList = ExpTimeList.Select((x, index) => new
            {
                x.Id,
                DisplayText = $"{x.decription}"
                //x.ExpiryTime,
                //DisplayText = $"Cycle {index + 1} [{x.StartTime} - {x.EndTime} -> Expiry {DateTime.Parse(x.ExpiryTime).ToString("h:mm tt")}]"
            }).ToList();

            // Create a SelectListItem list from the expiry times
            var expiryTimeSelectList = selectList.Select(x => new SelectListItem
            {
                Value = Convert.ToString(x.Id),
                Text = x.DisplayText
            }).ToList();

            ViewBag.SessionExpiryTime = expiryTimeSelectList;
            Session["SessionExpiryTime_dropDown"] = expiryTimeSelectList;






            return View();
        }

        public ActionResult IWActionReport(string fromdate = null, string todate = null, string clrtypr = null, string reporttype = null, string vflevel = null, string UserName = null, string filedwnldtype = null, string expiryTime = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            if ((bool)Session["Report"] == false)
            {
                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            byte Bytevflevel = 0;
            string clr = "";
            if (vflevel == "L1 Verification")
                Bytevflevel = 1;
            else if (vflevel == "L2 Verification")
                Bytevflevel = 2;
            else if (vflevel == "L3 Verification")
                Bytevflevel = 3;
            else if (vflevel == "PayeeName Verification")
                Bytevflevel = 4;


            if (clrtypr == "CTS")
                clr = "01";
            else if (clrtypr == "SPECIAL")
                clr = "99";
            else
                clr = "11";

            clr = clrtypr;
            try
            {
                //string tempProcDate = procdate.Substring(6, 4) + "-" + procdate.Substring(3, 2) + "-" + procdate.Substring(0, 2);
                string tempFromDate = fromdate.Substring(6, 4) + "-" + fromdate.Substring(3, 2) + "-" + fromdate.Substring(0, 2);
                string tempToDate = todate.Substring(6, 4) + "-" + todate.Substring(3, 2) + "-" + todate.Substring(0, 2);

                SqlConnectionStringBuilder SConn = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
                string thisConnectionString = SConn.ConnectionString;
                SqlConnection thisConnection = new SqlConnection(thisConnectionString);
                thisConnection.Open();
                SqlCommand sqlCommand;
                string reportPath = "";
                DataTable dTable = new DataTable("DataTable");

                if (reporttype == "Productivity Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.SpLVerificationProductivityReport", thisConnection);
                else if (reporttype == "Verification Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.SpLVerificationReport", thisConnection);
                else if (reporttype == "P2FDetails Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.P2FDetailsReport", thisConnection);
                else if (reporttype == "Return Details Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.IWReturnDetailsReport", thisConnection);
                else if (reporttype == "L1 Rejected Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.IWL1RejectedReport", thisConnection);
                else if (reporttype == "L2 Rejected Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.IWL2RejectedReport", thisConnection);
                else if (reporttype == "Final Status Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.IWFinalStatusReport", thisConnection);
                else if (reporttype == "SMB Images Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_IWSMBImagesReport", thisConnection);
                else if (reporttype == "Return Memo With BranchName Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_IWReturnMemoWithBranchNameReport", thisConnection);
                else if (reporttype == "Return Memo With Image Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_IWReturnMemoWithImageReport", thisConnection);
                else if (reporttype == "Inward Data Entry Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.InwardDataEntryReport", thisConnection);
                else if (reporttype == "Inward BankWise Received Cheques Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_Inward_BankWise_ReceivedCheques", thisConnection);
                else if (reporttype == "Inward BranchWise Summary Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_Inward_BranchWise_Summary", thisConnection);
                else if (reporttype == "DBS AND LVB Inward Summary Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_Inward_BranchWise_Summary_Consolidated", thisConnection);
                else if (reporttype == "P2FDetails BranchWise Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.P2FDetailsReportBranchWise_ForDBS", thisConnection);
                else if (reporttype == "Suspense Queue Transaction Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_SuspenseQueueTransaction", thisConnection);
                else if (reporttype == "SMB Images Up To 10k Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_IWSMBImagesBelow10kReport", thisConnection);
                else if (reporttype== "All Transactions MIS Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_IWAllTransactionsReport", thisConnection);
                else if (reporttype== "Above 50L Cheque Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_IWAbove50LReport", thisConnection);
                else if(reporttype== "Above 1CR Return Details Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.IW_Above1cr_ReturnDetailsReport", thisConnection);
                else if (reporttype== "Cheque Return Charges Failure Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.IW_ChequeReturnChargesFailureReport", thisConnection);
                else if(reporttype== "Suspense Queue Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.IW_SuspenseQueueCMReport", thisConnection);
                else if(reporttype== "Inward Productivity Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.IW_InwardProductivityReport", thisConnection);
                else if(reporttype== "ItemWise Presentation Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.IW_ItemWisePresentationReport", thisConnection);
                else if(reporttype== "Inward Cheque Returns charges Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.IW_ChequeReturnschargesReport", thisConnection);
                else if(reporttype== "Inward Returns Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.IW_ReturnsReport", thisConnection);
                else if(reporttype== "ICR/OCR vs NPCI Values Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.IW_ICR_OCRvsNPCIValues", thisConnection);
                else if(reporttype== "L1 Verification Action Report")
                    sqlCommand= new System.Data.SqlClient.SqlCommand("dbo.IW_L1VerificationActionReport", thisConnection);
                else if(reporttype == "L2 Verification Action Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.IW_L2VerificationActionReport", thisConnection);
                else if(reporttype=="L3 Verification Action Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.IW_L3VerificationActionReport", thisConnection);
                else if(reporttype== "Manual Exceptions Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.IW_ManualExceptionsReport", thisConnection);
                else if (reporttype == "Drawer A/C New Flag Status")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.IW_DrawerACNewFlagStatusReport", thisConnection);
                else if (reporttype == "Benificiary A/C New Flag Status")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.IW_BenificiaryACNewFlagStatusReport", thisConnection);
                else if (reporttype == "Cheque Status Modified under Query Module")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.IW_ChequeStatusModifiedunderQueryModule", thisConnection);
                else if (reporttype == "Modified Field Report")//pending
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.IW_ModifiedFieldReport", thisConnection);
                else if (reporttype == "Bulk Return Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.IW_BulkReturnReport", thisConnection);
                else if (reporttype == "Deemed Approved Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.IW_DeemedApprovedReport", thisConnection);
                else if (reporttype == "L0 Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.IW_L0ReportReport", thisConnection);


                else
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.SpUserWiseVerificationReport", thisConnection);//

                if (reporttype == "Productivity Report" || reporttype == "Verification Report")
                {
                    //sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    //sqlCommand.Parameters.Add("@ProcessDate", SqlDbType.Date).Value = Convert.ToDateTime(tempProcDate);
                    //sqlCommand.Parameters.Add("@ClearingType", SqlDbType.VarChar).Value = clr;
                    //sqlCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = Convert.ToInt16(Session["CustomerID"].ToString());
                    //sqlCommand.Parameters.Add("@VerificationLevel", SqlDbType.Int).Value = Bytevflevel;
                }
                else if (reporttype == "Audit Report")
                {
                    //sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    //sqlCommand.Parameters.Add("@ProcessDate", SqlDbType.Date).Value = Convert.ToDateTime(tempProcDate);
                    //sqlCommand.Parameters.Add("@ClearingType", SqlDbType.VarChar).Value = clr;
                    //sqlCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = Convert.ToInt16(Session["CustomerID"].ToString());
                    //sqlCommand.Parameters.Add("@VerificationLevel", SqlDbType.Int).Value = Bytevflevel;
                    //sqlCommand.Parameters.Add("@UserID", SqlDbType.VarChar).Value = UserName;
                }
                else if (reporttype == "L1 Rejected Report" || reporttype == "L2 Rejected Report")
                {
                    //string tempprocdate = procdate.Substring(6, 4) + "-" + procdate.Substring(0, 2) + "-" + procdate.Substring(3, 2);
                    //sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    //sqlCommand.Parameters.Add("@ProcessDate", SqlDbType.Date).Value = Convert.ToDateTime(tempprocdate);
                    //sqlCommand.Parameters.Add("@ClearingType", SqlDbType.VarChar).Value = clr;
                    //sqlCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = Convert.ToInt16(Session["CustomerID"].ToString());
                }
                else if (reporttype == "Final Status Report")
                {
                    //string tempprocdate = procdate.Substring(6, 4) + "-" + procdate.Substring(0, 2) + "-" + procdate.Substring(3, 2);
                    //sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    //sqlCommand.Parameters.Add("@ProcessDate", SqlDbType.Date).Value = Convert.ToDateTime(tempprocdate);
                    //sqlCommand.Parameters.Add("@ClearingType", SqlDbType.VarChar).Value = clr;
                    //sqlCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = Convert.ToInt16(Session["CustomerID"].ToString());
                }
                else
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    //sqlCommand.Parameters.Add("@ProcessDate", SqlDbType.Date).Value = Convert.ToDateTime(tempProcDate);
                    sqlCommand.Parameters.Add("@FromDate", SqlDbType.Date).Value = Convert.ToDateTime(tempFromDate);
                    sqlCommand.Parameters.Add("@ToDate ", SqlDbType.Date).Value = Convert.ToDateTime(tempToDate);
                    sqlCommand.Parameters.Add("@ClearingType", SqlDbType.VarChar).Value = clr;
                    sqlCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = Convert.ToInt16(Session["CustomerID"].ToString());

                    //expiry time added
                    sqlCommand.Parameters.Add("@ExpiryTime", SqlDbType.VarChar).Value = expiryTime;
                }

                ReportDocument rDocument = new ReportDocument();

                if (reporttype == "Productivity Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/IwLxProductivity.rpt");
                else if (reporttype == "Verification Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/IwLxVerification.rpt");
                else if (reporttype == "P2FDetails Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/IWP2FDetails.rpt");
                else if (reporttype == "Audit Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/IwVFUserwiseReport.rpt");
                else if (reporttype == "L1 Rejected Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/IWL1RejectedReport.rpt");
                else if (reporttype == "L2 Rejected Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/IWL2RejectedReport.rpt");
                else if (reporttype == "Final Status Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/IWFinalStatusReport.rpt");
                else if (reporttype == "SMB Images Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_IWSMBImagesReport.rpt");
                else if (reporttype == "Return Memo With BranchName Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_ReturnMemoWithBranchNameReport.rpt");
                else if (reporttype == "Return Memo With Image Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_ReturnMemoWithImageReport.rpt");
                else if (reporttype == "Inward Data Entry Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_InwardDataEntryReport.rpt");
                else if (reporttype == "Inward BankWise Received Cheques Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_InwardBankWiseReceivedChequesReport.rpt");
                else if (reporttype == "Inward BranchWise Summary Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_InwardBranchWiseSummaryReport.rpt");
                else if (reporttype == "DBS AND LVB Inward Summary Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_InwardBranchWiseConsolidatedSummaryReport.rpt");
                else if (reporttype == "P2FDetails BranchWise Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_InwardP2FDetailsBranchWise.rpt");
                else if (reporttype == "Suspense Queue Transaction Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_SuspenseQueueTransaction.rpt");
                else if (reporttype == "SMB Images Up To 10k Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_IWSMBImagesBelow10kReport.rpt");
                else if (reporttype == "All Transactions MIS Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_IWAllTransactionsMIS.rpt");
                else if (reporttype == "Above 50L Cheque Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_IWAbove50LReport.rpt");
                else if (reporttype == "Above 1CR Return Details Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_IWAbove1CR_ReturnDetailsReport.rpt");
                else if (reporttype == "Cheque Return Charges Failure Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_IWChequeReturnChargesFailureReport.rpt");
                else if (reporttype == "Suspense Queue Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_IW_SuspenseQueueCMReport.rpt");
                else if (reporttype == "Inward Productivity Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_InwardProductivityReport.rpt");
                else if (reporttype == "Inward Cheque Returns charges Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_InwardChequeReturnschargesReport.rpt");
                else if (reporttype == "ItemWise Presentation Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_IW_ItemWisePresentationReport.rpt");
                else if(reporttype == "Inward Returns Report")
                    reportPath= Server.MapPath("~/Reports/Crystal/RPT_IW_ReturnsReport.rpt");
                else if (reporttype == "ICR/OCR vs NPCI Values Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_ICR_OCRvsNPCIValuesReport.rpt");
                else if (reporttype == "L1 Verification Action Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_IWL1VerificationActionReport.rpt");
                else if (reporttype == "L2 Verification Action Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_IWL2VerificationActionReport.rpt");
                else if (reporttype == "L3 Verification Action Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_IWL3VerificationActionReport.rpt");
                else if (reporttype == "Manual Exceptions Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_IWManualExceptionsReport.rpt");
                else if (reporttype == "Drawer A/C New Flag Status")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_IWDrawerACNewFlagStatus.rpt");
                else if (reporttype == "Benificiary A/C New Flag Status")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_IWBenificiaryACNewFlagStatus.rpt");
                else if (reporttype == "Cheque Status Modified under Query Module")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_IWChequeStatusModifiedunderQueryModule.rpt");
                else if (reporttype == "Bulk Return Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_IWBulkReturnReport.rpt");
                else if (reporttype == "Deemed Approved Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_IWDeemedApprovedReport.rpt");
                else if (reporttype == "L0 Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_IWL0Report.rpt");
                else if (reporttype == "Modified Field Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_IWModifiedFieldReport.rpt");

                else
                    reportPath = Server.MapPath("~/Reports/Crystal/IWReturnDetails.rpt");

                //string filename = DateTime.Now.ToString("ddMMyyyy") + "IwrepLogs.txt";
                //string fileNameWithPath = @"c:\Logs\" + filename;
                //StreamWriter str1 = new StreamWriter(fileNameWithPath, true, System.Text.Encoding.Default);
                //str1.WriteLine("reportPath is " + reportPath);
                //str1.Close();


                rDocument.Load(reportPath);

                using (SqlDataAdapter da = new SqlDataAdapter(sqlCommand))
                {

                    da.Fill(dTable);
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                    if (reporttype == "Return Memo With Image Report")
                    {
                        for (int i = 0; i < dTable.Rows.Count; i++)
                        {
                            DataRow dr = dTable.Rows[i];
                            string FilePath1 = (string)dr["FrontGreyImagePath"];
                            string localPath = new Uri(FilePath1).LocalPath;
                            try
                            {
                                //------------------FrontGreyImagePath convert into byte--------
                                HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(FilePath1);
                                HttpWebResponse response1 = (HttpWebResponse)request1.GetResponse();
                                Stream receiveStream1 = response1.GetResponseStream();
                                byte[] imgbyte1 = ReadFully(receiveStream1);
                                dr["FGrayImg"] = imgbyte1;
                            }
                            catch (Exception ex)
                            {
                                //throw ex;
                                //continue;
                            }
                        }
                    }

                    if (reporttype == "SMB Images Report" || reporttype == "SMB Images Up To 10k Report")
                    {
                        //logerror("reporttype value :- ", reporttype.ToString());
                        //logerror("row count value :- ", dTable.Rows.Count.ToString());
                        for (int i = 0; i < dTable.Rows.Count; i++)
                        {
                            DataRow dr = dTable.Rows[i];
                            string FilePath1 = (string)dr["FrontGreyImagePath"];
                            string FilePath2 = (string)dr["FrontTiffImagePath"];
                            string FilePath3 = (string)dr["BackTiffImagePath"];

                            string localPath1 = new Uri(FilePath1).LocalPath;
                            string localPath2 = new Uri(FilePath2).LocalPath;
                            string localPath3 = new Uri(FilePath3).LocalPath;
                            //logerror("localPath1 value :- ", localPath1.ToString());
                            //logerror("localPath2 value :- ", localPath2.ToString());
                            //logerror("localPath3 value :- ", localPath3.ToString());
                            try
                            {
                                //------------------FrontGreyImagePath convert into byte--------
                                var tstimg = "https://picsum.photos/536/354";
                                HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(FilePath1);
                                HttpWebResponse response1 = (HttpWebResponse)request1.GetResponse();
                                Stream receiveStream1 = response1.GetResponseStream();
                                byte[] imgbyte1 = ReadFully(receiveStream1);

                                //  byte[] imgbyte1 = GetImageFromPhysicalPath(localPath1);
                                //logerror("Image byte1 value :- ", imgbyte1.ToString());
                                dr["FGrayImg"] = imgbyte1;

                                //------------------FrontTiffImagePath convert into byte--------
                                HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(FilePath2);
                                HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();
                                Stream receiveStream2 = response2.GetResponseStream();
                                byte[] imgbyte2 = ReadFully(receiveStream2);
                               // byte[] imgbyte2 = GetImageFromPhysicalPath(localPath2);
                               // logerror("Image byte2 value :- ", imgbyte2.ToString());
                                dr["FTiffImg"] = imgbyte2;

                                //------------------BackTiffImagePath convert into byte--------
                                HttpWebRequest request3 = (HttpWebRequest)WebRequest.Create(FilePath3);
                                HttpWebResponse response3 = (HttpWebResponse)request3.GetResponse();
                                Stream receiveStream3 = response3.GetResponseStream();
                                byte[] imgbyte3 = ReadFully(receiveStream3);
                               // byte[] imgbyte3 = GetImageFromPhysicalPath(localPath3);
                               // logerror("Image byte3 value :- ", imgbyte3.ToString());
                                dr["BTiffImg"] = imgbyte3;
                            }
                            catch (Exception ex)
                            {
                                //throw ex;
                                //continue;
                            }
                        }
                    }

                    rDocument.SetDataSource(dTable);
                }

                //using (SqlDataReader reader = sqlCommand.ExecuteReader())
                //{
                //    dTable.Load(reader);
                //    rDocument.SetDataSource(dTable);
                //}

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                try
                {
                    ///----------------PDF----------------------

                    if (filedwnldtype == "EXCEL")
                    {
                        //-------------------------EXCEl------------------------
                        //Stream str = rDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                        Stream str = rDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.ExcelWorkbook);
                        str.Seek(0, SeekOrigin.Begin);
                        //--------------------Disposing Objects
                        rDocument.Close();
                        rDocument.Clone();
                        rDocument.Dispose();
                        rDocument = null;
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        return File(str, "application/excel", reporttype + ".xls");
                    }
                    else if (filedwnldtype == "CSV")
                    {
                        //--------CSV-----------------
                        Stream str = rDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.CharacterSeparatedValues);
                        str.Seek(0, SeekOrigin.Begin);
                        //--------------------Disposing Objects
                        rDocument.Close();
                        rDocument.Clone();
                        rDocument.Dispose();
                        rDocument = null;
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        return File(str, "application/csv", reporttype + ".csv");
                    }
                    else
                    {
                        Stream str = rDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                        str.Seek(0, SeekOrigin.Begin);
                        //--------------------Disposing Objects
                        rDocument.Close();
                        rDocument.Clone();
                        rDocument.Dispose();
                        rDocument = null;
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        return File(str, "application/pdf");//, "maintransaction.pdf");                }
                    }
                    
                }
                catch (Exception e)
                {
                    return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
                }


            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }

        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        [HttpPost]
        public ActionResult getUsers(string VFlevel = null)
        {

            var users = af.UserMasters.Where(a => a.L2StopLimit != 0 || a.L3StopLimit != 0).Select(a => "<option value='" + a.LoginID + "'>" + a.FirstName + "'</option>'");

            return Content(String.Join("", users));
        }

        public byte[] GetImageFromPhysicalPath(string httpwebimgpath = null)
        {
            int custid = Convert.ToInt16(Session["CustomerID"]);
            //var destpath = af.CustomerMasters.FirstOrDefault((p) => p.Id == custid);
            var settingName = "PhysicalPath";
            var destpath = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == settingName);
            //logerror("httpwebimgpath Name :- ", httpwebimgpath.ToString());
            string destroot = destpath.SettingValue;
            //logerror("destroot Name :- ", destroot.ToString());

            const char delimiter = '\\';
            string[] destrootarr = destroot.Split(delimiter);

            string foldrname = destrootarr[destrootarr.Length - 1];
            //logerror("Folder Name :- ", foldrname.ToString());

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
            actualpath = actualpath.Replace("\\\\", "\\");
            // System.IO.File.AppendAllText("C:\\temp\\log1.txt", "actualpathOWL1:" + actualpath);
            //logerror(actualpath, actualpath.ToString());

            //System.Drawing.Bitmap bmp = new Bitmap(actualpath);
            //System.IO.MemoryStream stream = new System.IO.MemoryStream();
            //bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            //stream.Position = 0;

            //byte[] data = new byte[stream.Length];
            //int lngth = (int)stream.Length;
            //stream.Read(data, 0, lngth);
            //stream.Close();

            //Array.Clear(data, 0, data.Length);

            byte[] data = System.IO.File.ReadAllBytes(actualpath);
            return data;
            //return PartialView("_getTiffImage");
        }


        //swapnali
        public class SessionExpiryTime
        {
            public int Id { get; set; }
            //public string StartTime { get; set; }
            //public string EndTime { get; set; }
            //public string ExpiryTime { get; set; }

            public string decription { get; set; }
        }
        //swapnali
        public class branches
        {
            public string branchName { get; set; }
            public string branchCode { get; set; }
        }
        //swapnali
        public JsonResult getbranchname(int Did)
        {

            List<branches> list = new List<branches>();
            try
            {
                SqlConnectionStringBuilder SConn = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
                string thisConnectionString = SConn.ConnectionString;
                SqlConnection con = new SqlConnection(thisConnectionString);
                con.Open();
                using (SqlCommand cmd = new SqlCommand("getBranches", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@DomainID", SqlDbType.Int).Value = Did;

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            branches lstObj = new branches
                            {
                                branchCode = rdr["BranchCode"].ToString(),
                                branchName = rdr["BranchName"].ToString()


                            };
                            list.Add(lstObj);
                        }
                    }
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
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }






        public List<SessionExpiryTime> GetExpiryTime()
        {
            List<SessionExpiryTime> list = new List<SessionExpiryTime>();


            try
            {
                SqlConnectionStringBuilder SConn = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
                string thisConnectionString = SConn.ConnectionString;
                SqlConnection con = new SqlConnection(thisConnectionString);
                con.Open();
                using (SqlCommand cmd = new SqlCommand("GetSessionExpiryTime", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            SessionExpiryTime lstObj = new SessionExpiryTime
                            {
                                Id = Convert.ToInt32(rdr["Id"]),
                                //StartTime = rdr["StartTime"].ToString(),
                                //EndTime = rdr["EndTime"].ToString(),
                                //ExpiryTime = rdr["ExpiryTime"].ToString(),
                                decription = rdr["description"].ToString()
                            };
                            list.Add(lstObj);

                        }
                    }
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

                logerror("In owreport GetExpiryTime Catch==>" + message, "InnerExp===>" + innerExcp);
            }

            return list;
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
