using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using ikloud_Aflatoon.Models;
using System.Net;
using CrystalDecisions.Web.HtmlReportRender;
//using Microsoft.Office.Interop.Excel;

namespace ikloud_Aflatoon.Controllers
{
    public class OWReportsController : Controller
    {
        AflatoonEntities af = new AflatoonEntities();
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
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            int custid = Convert.ToInt16(Session["CustomerID"].ToString());

            var vardom = (from a in af.DomainMaster
                          from ud in af.DomainUserMapMasters
                          where a.Id == ud.DomainId && a.CustomerId == custid &&
                          ud.UserId == uid
                          select new
                          {
                              Id = a.Id,
                              Name = a.Name
                          }
                             ).ToList();

            ViewBag.gridDomains = new SelectList(vardom, "Id", "Name");
            
            //----------------Clearingtype-------28-03-2018---------------
            ViewBag.clrtype = new SelectList(af.ClearingType, "Code", "Name").ToList();

            ViewBag.BankCode = Session["BankCode"].ToString();

            ViewBag.Accesslevel = Session["Accesslevel"].ToString();

            ViewBag.ProcessingDate = Convert.ToDateTime(Session["processdate"].ToString()).ToString("dd/MM/yyyy");

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

       

        public ActionResult OWActionReport(string fromdate = null, string todate = null, string clrtypr = null, string reporttype = null,string filedwnldtype = null, int domainid = 0, int scanningtype = 0,string expiryTime=null,string branchCode=null)
        {
           // logerror("in OWActionReport==>", "fromdate==>" + fromdate);
           // logerror("in OWActionReport==>", "todate==>" + todate);
          //  logerror("in OWActionReport==>", "clrtypr==>" + clrtypr);
          //  logerror("in OWActionReport==>", "reporttype==>" + reporttype);
          //  logerror("in OWActionReport==>", "filedwnldtype==>" + filedwnldtype);
          //  logerror("in OWActionReport==>", "domainid==>" + domainid.ToString());
          //  logerror("in OWActionReport==>", "scanningtype==>" + scanningtype.ToString());
          //  logerror("in OWActionReport==>", "expiryTime==>" + expiryTime.ToString());
          //  logerror("in OWActionReport==>", "branchCode==>" + branchCode.ToString());


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

            ReportDocument rDocument = new ReportDocument();

            try
            {
                if ((bool)Session["Report"] == false)
                {
                    UserMaster usrm = af.UserMasters.Find(uid);
                    usrm.Active = false;
                    af.SaveChanges();
                    return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
                }

                string tempFromDate = fromdate.Substring(6, 4) + "-" + fromdate.Substring(3, 2) + "-" + fromdate.Substring(0, 2);
                string tempToDate = todate.Substring(6, 4) + "-" + todate.Substring(3, 2) + "-" + todate.Substring(0, 2);

                string clr = "";

                clr = clrtypr;

               // logerror("in OWActionReport==>", "LN130");
              //  logerror("in OWActionReport==> reporttype==>", reporttype);

                SqlConnectionStringBuilder SConn = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
                string thisConnectionString = SConn.ConnectionString;
                SqlConnection thisConnection = new SqlConnection(thisConnectionString);
                thisConnection.Open();
                SqlCommand sqlCommand = null;
                string reportPath = "";
                DataTable dTable = new DataTable("DataTable");

                if (reporttype == "Batch Wise Summary Report")                                 
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_BatchWiseSummaryReport", thisConnection);
                else if (reporttype == "P2F PullOut Report" || reporttype == "Bank wise P2F Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_P2FPulloutReport", thisConnection);
                else if (reporttype == "Bundled File Summary Report")                                               
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_BundledFileSummaryReport", thisConnection);
                else if (reporttype == "Day's Summary Report")                                      
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_DaysSummaryReport", thisConnection);
                else if (reporttype == "Discrepant Report" || reporttype == "Reject Memo")                              
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_DiscrepantReport", thisConnection);
                else if (reporttype == "Duplicate Cheques Report")                                                          
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_DuplicateChequesReport", thisConnection);
                else if (reporttype == "Extension Report")                                          
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_ExtensionReport", thisConnection);
                else if (reporttype == "Item Wise Presentation Details")                                        
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_PresentationDetails", thisConnection);
                else if (reporttype == "OW Productivity Report")                                            
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_ProductivityReport", thisConnection);  
                else if (reporttype == "Return PullOut Report")                
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_ReturnPullOutReport", thisConnection);
                else if (reporttype == "Return Memo Report With Image" || reporttype == "Return Memo Report Without Image" || reporttype == "Return Memo Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_ReturnMemo", thisConnection);
                else if (reporttype == "Verification / CHI Reject Report")                                                  
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.VerificationCHIRejectReport", thisConnection);
                else if (reporttype == "Presentment Details BranchWise Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_PresentationDetailsWithBranchWise", thisConnection);
                else if (reporttype == "Presentment BranchWise Summary Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_PresentationDetailsWithBranchWiseSummaryReport", thisConnection);
                else if (reporttype == "Settlement Details BranchWise Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_SettlementDetailsWithBranchWise", thisConnection);
                else if (reporttype == "Settlement BranchWise Summary Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_SettlementDetailsWithBranchWiseSummaryReport", thisConnection);
                else if (reporttype == "P2F Details BranchWise Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_P2FDetailsWithBranchWise", thisConnection);
                else if (reporttype == "P2F BranchWise Summary Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_P2FDetailsWithBranchWiseSummaryReport", thisConnection);
                else if (reporttype == "Return Memo With BranchName Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_IWReturnMemoWithBranchNameReport", thisConnection);
                else if (reporttype == "Return Memo With Image Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_IWReturnMemoWithImageReport", thisConnection);
                else if (reporttype == "Return Details Report With BranchWise")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.IWReturnDetailsReport_BranchWise_ForSIB", thisConnection);
                else if (reporttype == "Return Details Report With BranchWise Summary")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.IWReturnDetailsSummaryReport_BranchWise_ForSIB", thisConnection);
                //Added on 2021-09-04 by Aniketadit
                else if (reporttype == "Login Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.LoginReport", thisConnection);
                else if (reporttype == "User Management Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.UserActivity", thisConnection);
                else if (reporttype == "Role Management Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RoleReport", thisConnection);
                else if (reporttype == "PPS Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_PPS_Report", thisConnection);
                else if (reporttype == "DBS And LVB Settlement BranchWise Summary")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_SettlementDetailsWithBranchWiseSummaryConsolidatedReport", thisConnection);
                else if (reporttype == "DBS And LVB Presentment Gridwise Summary")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_PresentmentDetailsWithGridWiseSummaryConsolidatedReport", thisConnection);
                else if (reporttype == "DBS And LVB Uploaded Gridwise Summary")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_UploadedDetailsWithGridWiseSummaryConsolidatedReport", thisConnection);
                else if (reporttype == "NRE Presentment Details BranchWise Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_PresentationDetailsWithBranchWiseNRE", thisConnection);
                else if (reporttype == "Outward Lodgment Reverse Feed Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_SettlementDetailsWithBranchWise", thisConnection);
                else if (reporttype == "Outward Return Reverse Feed Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.IWReturnDetailsReport_BranchWise_ForSIB", thisConnection);

                //========= new report added on 23_01_2023 by amol =============
                else if (reporttype == "Consolidated Outward Return Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_ConsolidatedOutwardReturnReport", thisConnection);

                //========= new report added on 28_02_2023 by amol =============
                else if (reporttype == "Outward DataEntry Action Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_OutwardDataEntryActionReport", thisConnection);

                //========= new report added on 28_02_2023 by amol =============
                else if (reporttype == "Outward Verification Action Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_OutwardVerificationActionReport", thisConnection);

                //========= new report added on 14_02_2024 by amol =============
                else if (reporttype == "Outward MICR Repair Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_MICRRepairReport", thisConnection);

                //========= new report added on 19_02_2024 by Amol =============
                else if (reporttype == "SMB Images Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_OWSMBImagesReport", thisConnection);

                //========= new report added on 27_02_2024 by Amol =============
                else if (reporttype == "Source Of Fund Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_SourceOfFundReport", thisConnection);

                //new 
                else if(reporttype == "Six Month Old Account")
                    sqlCommand=new System.Data.SqlClient.SqlCommand("dbo.RPT_SixMonthOldAccount", thisConnection);

                else if (reporttype== "Outward Lodgment Failure Report")
                    sqlCommand=new System.Data.SqlClient.SqlCommand("dbo.RPT_OutwardlodgmentFailureReport", thisConnection);

                else if (reporttype == "Outward Return Lodgment Failure Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_OutwardlodgmentFailureReport", thisConnection);

                else if(reporttype== "All Transactions Lien Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_OWAllTransactionsReportWithLienApi", thisConnection);
                else if(reporttype== "Cheque Return Charges Report") //08-01-24
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.OW_ChequeReturnChargesReport", thisConnection);
                else if (reporttype== "Outward Return Transaction Report")
                    sqlCommand= new System.Data.SqlClient.SqlCommand("dbo.OW_OutwardReturnTransactionReport", thisConnection);
                else if(reporttype== "INR 1 cr and above Cheque Returns Details")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.OW_INR1crandaboveChequeReturnsDetails", thisConnection);








                if (reporttype == "Item Wise Presentation Details" || reporttype == "Batch Wise Summary Report" || reporttype == "Presentment Details BranchWise Report"
                    || reporttype == "Return PullOut Report" || reporttype == "Verification / CHI Reject Report" || reporttype == "P2F PullOut Report"
                    || reporttype == "Return Memo Report" || reporttype == "Presentment BranchWise Summary Report"
                    || reporttype == "Settlement Details BranchWise Report" || reporttype == "Settlement BranchWise Summary Report"
                    || reporttype == "Return Memo With BranchName Report" || reporttype == "Return Memo With Image Report" || reporttype == "Return Details Report With BranchWise"
                    || reporttype == "Return Details Report With BranchWise Summary"
                    || reporttype == "P2F Details BranchWise Report" || reporttype == "P2F BranchWise Summary Report" || reporttype == "PPS_Report_ForDBS"
                    || reporttype == "DBS & LVB Settlement BranchWise Summary" || reporttype == "DBS And LVB Presentment Gridwise Summary"
                    || reporttype == "DBS And LVB Uploaded Gridwise Summary" || reporttype == "NRE Presentment Details BranchWise Report"
                    || reporttype == "Outward Lodgment Reverse Feed Report" || reporttype == "Outward Return Reverse Feed Report"
                    || reporttype == "Outward DataEntry Action Report" || reporttype == "Outward Verification Action Report"
                    || reporttype == "Outward MICR Repair Report" || reporttype == "Source Of Fund Report")
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add("@FromDate", SqlDbType.Date).Value = Convert.ToDateTime(tempFromDate);
                    sqlCommand.Parameters.Add("@ToDate ", SqlDbType.Date).Value = Convert.ToDateTime(tempToDate);
                    sqlCommand.Parameters.Add("@ClearingType", SqlDbType.VarChar).Value = clr;
                    sqlCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = Convert.ToInt16(Session["CustomerID"].ToString());
                    sqlCommand.Parameters.Add("@DomainID", SqlDbType.Int).Value = domainid;
                    sqlCommand.Parameters.Add("@ScanningType", SqlDbType.Int).Value = scanningtype;

                    //new filter added
                    sqlCommand.Parameters.Add("@ExpiryTime", SqlDbType.VarChar).Value = expiryTime;
                    sqlCommand.Parameters.Add("@BranchCode", SqlDbType.VarChar).Value = branchCode;

                   // logerror("in OWActionReport if block (reporttype)==>", "LN261");

                }
                else if (reporttype == "Login Report" || reporttype == "User Management Report" || reporttype == "Role Management Report")
                {
                    string fromDate = tempFromDate + " 00:00:00.000";
                    string toDate = tempToDate + " 23:59:59.999";
                    DateTime newToDate = Convert.ToDateTime(tempToDate).AddDays(1);

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add("@FromDate", SqlDbType.Date).Value = Convert.ToDateTime(tempFromDate);
                    sqlCommand.Parameters.Add("@ToDate ", SqlDbType.Date).Value = newToDate;
                }
                else if (reporttype == "Consolidated Outward Return Report")
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add("@FromDate", SqlDbType.Date).Value = Convert.ToDateTime(tempFromDate);
                    sqlCommand.Parameters.Add("@ToDate ", SqlDbType.Date).Value = Convert.ToDateTime(tempToDate);
                    sqlCommand.Parameters.Add("@ClearingType", SqlDbType.VarChar).Value = clr;
                }
                else
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add("@FromDate", SqlDbType.Date).Value = Convert.ToDateTime(tempFromDate);
                    sqlCommand.Parameters.Add("@ToDate ", SqlDbType.Date).Value = Convert.ToDateTime(tempToDate);
                    sqlCommand.Parameters.Add("@ClearingType", SqlDbType.VarChar).Value = clr;
                    sqlCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = Convert.ToInt16(Session["CustomerID"].ToString());
                    sqlCommand.Parameters.Add("@DomainId", SqlDbType.Int).Value = domainid;

                    //new filter added 
                    sqlCommand.Parameters.Add("@ExpiryTime", SqlDbType.VarChar).Value = expiryTime;
                    sqlCommand.Parameters.Add("@BranchCode", SqlDbType.VarChar).Value = branchCode;
                }
                

                if  (reporttype == "Batch Wise Summary Report")                                 
                    reportPath = Server.MapPath("~/Reports/Crystal/BatchWiseSummaryReport.rpt");
                else if (reporttype == "Bank wise P2F Report")                                      
                    reportPath = Server.MapPath("~/Reports/Crystal/BankWiseP2FReport.rpt");
                else if (reporttype == "Bundled File Summary Report")                       
                    reportPath = Server.MapPath("~/Reports/Crystal/BundledFileSummaryReport.rpt");
                else if (reporttype == "Day's Summary Report")                                      
                    reportPath = Server.MapPath("~/Reports/Crystal/DaysSummaryReport.rpt");
                else if (reporttype == "Discrepant Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/Discrepant_Report.rpt");
                else if (reporttype == "Duplicate Cheques Report")                                          
                    reportPath = Server.MapPath("~/Reports/Crystal/DuplicateChequesReport.rpt");
                else if (reporttype == "Extension Report")                                          
                    reportPath = Server.MapPath("~/Reports/Crystal/ExtensionReport.rpt");
                else if (reporttype == "Item Wise Presentation Details")                            
                    reportPath = Server.MapPath("~/Reports/Crystal/PresentationDetails.rpt");
                else if (reporttype == "OW Productivity Report")                                                    
                    reportPath = Server.MapPath("~/Reports/Crystal/ProductivityReport.rpt"); 
                else if (reporttype == "P2F PullOut Report")                                           
                    reportPath = Server.MapPath("~/Reports/Crystal/P2FPulloutReport.rpt");
                else if (reporttype == "Return PullOut Report")             
                    reportPath = Server.MapPath("~/Reports/Crystal/OWReturnPulloutNew.rpt");
                else if (reporttype == "Return Memo Report With Image"|| reporttype == "Return Memo Report")             
                    reportPath = Server.MapPath("~/Reports/Crystal/ReturnMemo.rpt");
                else if (reporttype == "Verification / CHI Reject Report")              
                    reportPath = Server.MapPath("~/Reports/Crystal/VerificationCHIRejectReport.rpt");
                else if (reporttype == "Presentment Details BranchWise Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/PresentationDetailsWithBranchWise.rpt");
                else if (reporttype == "Presentment BranchWise Summary Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/PresentationDetailsWithBranchWiseSummary.rpt");
                else if (reporttype == "Settlement Details BranchWise Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/SettlementDetailsWithBranchWise.rpt");
                else if (reporttype == "Settlement BranchWise Summary Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/SettlementDetailsWithBranchWiseSummary.rpt");
                else if (reporttype == "P2F Details BranchWise Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/P2FDetailsWithBranchWise.rpt");
                else if (reporttype == "P2F BranchWise Summary Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/P2FDetailsWithBranchWiseSummary.rpt");
                else if (reporttype == "Return Memo With BranchName Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_ReturnMemoWithBranchNameReport.rpt");
                else if (reporttype == "Return Memo With Image Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_ReturnMemoWithImageReport.rpt");
                else if (reporttype == "Return Details Report With BranchWise")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_ReturnDetailsWithBranchWiseReport.rpt");
                else if (reporttype == "Return Details Report With BranchWise Summary")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_ReturnDetailsWithSummaryReport.rpt");
                else if (reporttype == "Login Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/LoginReport.rpt");
                else if (reporttype == "User Management Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/UserManagementReport.rpt");
                else if (reporttype == "Role Management Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RoleManagementReport.rpt");
                else if (reporttype == "PPS Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/PPSReport.rpt");
                else if (reporttype == "DBS And LVB Settlement BranchWise Summary")
                    reportPath = Server.MapPath("~/Reports/Crystal/SettlementDetailsWithBranchWiseSummaryConsolidated.rpt");
                else if (reporttype == "DBS And LVB Presentment Gridwise Summary")
                    reportPath = Server.MapPath("~/Reports/Crystal/PresentmentDetailsWithGridWiseSummaryConsolidated.rpt");
                else if (reporttype == "DBS And LVB Uploaded Gridwise Summary")
                    reportPath = Server.MapPath("~/Reports/Crystal/UploadedDetailsWithGridWiseSummaryConsolidated.rpt");
                else if (reporttype == "NRE Presentment Details BranchWise Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/PresentationDetailsWithBranchWiseNRE.rpt");
                else if (reporttype == "Outward Lodgment Reverse Feed Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/OwLodgmentReverseFeed.rpt");
                else if (reporttype == "Outward Return Reverse Feed Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/OwReturnReverseFeed.rpt");

                //========= new report added on 23_01_2023 by amol =============
                else if (reporttype == "Consolidated Outward Return Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_ConsolidatedOutwardReturnReport.rpt");

                //========= new report added on 28_02_2023 by amol =============
                else if (reporttype == "Outward DataEntry Action Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_OutwardDataEntryActionReport.rpt");
                //========= new report added on 28_02_2023 by amol =============
                else if (reporttype == "Outward Verification Action Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_OutwardVerificationActionReport.rpt");

                //========= new report added on 14_02_2024 by amol =============
                else if (reporttype == "Outward MICR Repair Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_OutwardMICRRepairReport.rpt");

                //========= new report added on 19_02_2024 by Amol =============
                else if (reporttype == "SMB Images Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_OWSMBImagesReport.rpt");

                //========= new report added on 27_02_2024 by Amol =============
                else if (reporttype == "Source Of Fund Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_SourceOfFundReport.rpt");
                else if(reporttype== "Six Month Old Account")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_SixMonthOldAccount.rpt");
                else if(reporttype== "Outward Lodgment Failure Report")
                    reportPath= Server.MapPath("~/Reports/Crystal/RPT_OutwardLodgmentFailure.rpt");
                else if(reporttype== "Outward Return Lodgment Failure Report")
                    reportPath= Server.MapPath("~/Reports/Crystal/RPT_OutwardReturnLodgmentFailure.rpt");
                else if(reporttype== "All Transactions Lien Report")
                    reportPath= Server.MapPath("~/Reports/Crystal/RPT_AllTransactionsLienReport.rpt");
                //08-1-24
                else if(reporttype == "Cheque Return Charges Report")
                    reportPath= Server.MapPath("~/Reports/Crystal/RPT_OWChequeReturnChargesReport.rpt");
                else if(reporttype== "Outward Return Transaction Report")
                    reportPath= Server.MapPath("~/Reports/Crystal/RPT_OutwardReturnTransactionReport.rpt");
                else if (reporttype== "INR 1 cr and above Cheque Returns Details")
                    reportPath= Server.MapPath("~/Reports/Crystal/RPT_INR1crandaboveChequeReturnsDetails.rpt");

                // logerror("in OWActionReport LN388 reportPath==>", reportPath);

                rDocument.Load(reportPath);

               // logerror("in OWActionReport LN392 reportPath==>", "After rDocument.load");
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
                            FilePath1 = FilePath1.Replace("ikloudpro.kores.in", "10.168.201.16");
                            string localPath = new Uri(FilePath1).LocalPath;
                            try
                            {
                                //------------------FrontGreyImagePath convert into byte--------
                               // var tst = "https://picsum.photos/200/300";
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
                    if (reporttype == "Return Memo Report With Image")              
                    {

                        for (int i = 0; i < dTable.Rows.Count; i++)
                        {
                            DataRow dr = dTable.Rows[i];
                            string FilePath = (string)dr["FrontTiffImagePath"];
                            string localPath = new Uri(FilePath).LocalPath;
                            try
                            {
                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(FilePath);
                                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                Stream receiveStream = response.GetResponseStream();

                                byte[] imgbyte = ReadFully(receiveStream);

                                dr["Img"] = imgbyte;
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                        }

                    }

                    if (reporttype == "SMB Images Report")
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
                                //===== FrontGrey Image ====================
                                //byte[] imgbyte1 = GetImageFromPhysicalPath(localPath1);
                                //logerror("Image byte1 value :- ", imgbyte1.ToString());

                                HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(FilePath1);
                                HttpWebResponse response1 = (HttpWebResponse)request1.GetResponse();
                                Stream receiveStream1 = response1.GetResponseStream();
                                byte[] imgbyte1 = ReadFully(receiveStream1);
                                dr["FGrayImg"] = imgbyte1;

                                //========== BackTiff Image ===================
                                HttpWebRequest request3 = (HttpWebRequest)WebRequest.Create(FilePath3);
                                HttpWebResponse response3 = (HttpWebResponse)request3.GetResponse();
                                Stream receiveStream3 = response3.GetResponseStream();
                                byte[] imgbyte3 = ReadFully(receiveStream3);
                                dr["BTiffImg"] = imgbyte3;

                            }
                            catch (Exception ex)
                            {
                                //throw ex;
                                //continue;
                            }
                        }
                    }
                   // logerror("in OWActionReport LN495", "");
                    rDocument.SetDataSource(dTable);
                  //  logerror("in OWActionReport LN497", "after  rDocument.SetDataSource");
                }

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                ///----------------PDF----------------------

                if (filedwnldtype == "EXCEL")
                {
                    //-------------------------EXCEl------------------------
                    Stream str = rDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.ExcelWorkbook);
                    str.Seek(0, SeekOrigin.Begin);

                    //if (reporttype == "Consolidated Outward Return Report")
                    //{
                        //Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();

                        //using (var excelApp = new Microsoft.Office.Interop.Excel.Application())
                        //{
                        //    var workbook = excelApp.Workbooks.Open(str);
                        //    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;
                        //    worksheet.Name = "Consolidated Outward Return Report";

                        //    // save the workbook back to the stream
                        //    workbook.Save();
                        //    workbook.Close();
                        //}
                    //}


                    //--------------------Disposing Objects
                    rDocument.Close();
                    rDocument.Clone();
                    rDocument.Dispose();
                    rDocument = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    return File(str, "application/excel", reporttype + tempFromDate + tempToDate + ".xls");
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
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;


                rDocument.Close();
                rDocument.Clone();
                rDocument.Dispose();
                rDocument = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                logerror("in OWActionReport catch===>message==>"+message,"innerexp==>"+innerExcp);
                return RedirectToAction("ReportError", "Error", new { msg = e.Message.ToString(), popmsg = "OW Report" });
            }
        }

        public ActionResult GetPdf(string filename)
        {
            return File(filename, "application/pdf");
        }

        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        [NonAction]
        public ActionResult Error(string msg = null)
        {

            string ServerPath = "";
            string filename = "";
            string fileNameWithPath = "";
            ServerPath = Server.MapPath("~/Logs/");
            if (System.IO.Directory.Exists(ServerPath) == false)
            {
                System.IO.Directory.CreateDirectory(ServerPath);
            }
            filename = "reports_logs.txt";
            fileNameWithPath = ServerPath + filename;
            StreamWriter str = new StreamWriter(fileNameWithPath, true, System.Text.Encoding.Default);
            //  str.WriteLine("hello");
            str.WriteLine(DateTime.Now.ToShortTimeString() + ": Exception: " + msg);
            str.Close();

            return View();


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
        //swapnali
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

    }       
}
