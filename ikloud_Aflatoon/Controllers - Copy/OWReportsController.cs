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

            //List<Student> GetStudentList()
            //{
            //    Student s1 = new Student("1000001", "Ashkan Power", DateTime.Parse("07/17/1989"));
            //    Student s2 = new Student("1000002", "Peter Griffin", DateTime.Parse("02/10/1960"));
            //    Student s3 = new Student("1000003", "Super Man", DateTime.Parse("01/01/1980"));
            //    Student s4 = new Student("1000004", "Jim Carry", DateTime.Parse("05/14/1985"));
            //    Student s5 = new Student("1000005", "Bill Carter", DateTime.Parse("08/25/1940"));

            //    return new List<Student>() { s1, s2, s3, s4, s5 };
            //}

            //ReportDocument rDocument = new ReportDocument();
            //string reportPath = Server.MapPath("~/Reports/Crystal/StudentsReport.rpt");
            //rDocument.Load(reportPath);
            //rDocument.SetDataSource(GetStudentList());

            //Stream str = rDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            //str.Seek(0, SeekOrigin.Begin);
            ////--------------------Disposing Objects
            //rDocument.Close();
            //rDocument.Clone();
            //rDocument.Dispose();
            //rDocument = null;
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            //return File(str, "application/pdf");//, "maintransaction.pdf");

            //var scanningType = 
            return View();
        }

        //public class Student
        //{
        //    public Student(string Code, string Name, DateTime BirthDate)
        //    {
        //        this.Code = Code;
        //        this.Name = Name;
        //        this.BirthDate = BirthDate;
        //    }

        //    public string Code { set; get; }
        //    public string Name { set; get; }
        //    public DateTime BirthDate { set; get; }
        //    public int Age
        //    {
        //        get
        //        {
        //            int now = DateTime.Now.Year;
        //            int birth = BirthDate.Year;

        //            return now - birth;
        //        }
        //    }
        //}

        public ActionResult OWActionReport(string fromdate = null, string todate = null, string clrtypr = null, string reporttype = null,string filedwnldtype = null, int domainid = 0, int scanningtype = 0)
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
                    || reporttype == "Outward DataEntry Action Report" || reporttype == "Outward Verification Action Report")
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add("@FromDate", SqlDbType.Date).Value = Convert.ToDateTime(tempFromDate);
                    sqlCommand.Parameters.Add("@ToDate ", SqlDbType.Date).Value = Convert.ToDateTime(tempToDate);
                    sqlCommand.Parameters.Add("@ClearingType", SqlDbType.VarChar).Value = clr;
                    sqlCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = Convert.ToInt16(Session["CustomerID"].ToString());
                    sqlCommand.Parameters.Add("@DomainID", SqlDbType.Int).Value = domainid;
                    sqlCommand.Parameters.Add("@ScanningType", SqlDbType.Int).Value = scanningtype;
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
                            FilePath1 = FilePath1.Replace("ikloudpro.kores.in", "10.168.201.16");
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
                   
                    rDocument.SetDataSource(dTable);
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
                rDocument.Close();
                rDocument.Clone();
                rDocument.Dispose();
                rDocument = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();

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
    }       
}
