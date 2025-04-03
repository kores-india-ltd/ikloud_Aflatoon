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

namespace ikloud_Aflatoon.Controllers
{
    //[OutputCache(Duration = 0)]
    public class OWReportsController : Controller
    {
        //
        // GET: /OWReports/
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

            var vardom = (from a in af.DomainMasters
                          from ud in af.DomainUserMapMasters
                          where a.Id == ud.DomainId && a.CustomerId == custid &&
                          ud.UserId == uid
                          select new
                          {
                              Id = a.Id,
                              Name = a.Name
                          }
                             ).ToList();
            //  var resultdom= from a i

            ViewBag.gridDomains = new SelectList(vardom, "Id", "Name");

            return View();
        }
        //public ActionResult OWActionReport(string procdate = null, string clrtypr = null, string reporttype = null, string vflevel = null, string filedwnldtype = null, int domainid = 0)
        public ActionResult OWActionReport(string fromdate = null, string todate = null, string clrtypr = null, string reporttype = null, string vflevel = null, string filedwnldtype = null, int domainid = 0)
        {


            string tmpdata = "fromdate=" + fromdate + " todate=" + todate + " clrtypr=" +
                clrtypr + " reporttype=" + reporttype + " vflevel=" + vflevel + " filedwnldtype=" + filedwnldtype + " domainid=" + domainid;

            //Error("entered OWActionReport with parameters " + tmpdata);

            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            //Error("statement1");
            
            
            int uid = (int)Session["uid"];
            try
            {
                //Error("statement2");

                if ((bool)Session["Report"] == false)
                {
                    UserMaster usrm = af.UserMasters.Find(uid);//02/02/2017
                    usrm.Active = false;
                    af.SaveChanges();
                    return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
                }
                //----------------------Added On 09/05/2017---------------
                //int intdomainid = Convert.ToInt16(domainid);
                //string tempDate = procdate.Substring(6, 4) + "-" + procdate.Substring(3, 2) + "-" + procdate.Substring(0, 2);

               

                string tempFromDate = fromdate.Substring(6, 4) + "-" + fromdate.Substring(3, 2) + "-" + fromdate.Substring(0, 2);
                string tempToDate = todate.Substring(6, 4) + "-" + todate.Substring(3, 2) + "-" + todate.Substring(0, 2);
                byte Bytevflevel = 0;

                //Error("tempFromDate " + tempFromDate + " ,tempToDate " + tempToDate);

                string clr = "";
                if (vflevel == "L1 Verification")
                    Bytevflevel = 1;
                else if (vflevel == "L2 Verification")
                    Bytevflevel = 2;
                else if (vflevel == "L3 Verification")
                    Bytevflevel = 3;
                if (clrtypr == "CTS")
                    clr = "01";
                else
                    clr = "11";

                //Error("opening  connection");

                //System.IO.File.WriteAllText(@"D:\Webroot\iKloudPro\Logs\WriteLines.txt", "line 1");

                SqlConnectionStringBuilder SConn = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
                string thisConnectionString = SConn.ConnectionString;
                SqlConnection thisConnection = new SqlConnection(thisConnectionString);
                thisConnection.Open();
                SqlCommand sqlCommand = null;

                //Error("opened  connection and constring was " + thisConnectionString);

                //System.IO.File.WriteAllText(@"D:\Webroot\iKloudPro\Logs\WriteLines.txt", "line 2");

                string reportPath = "";
                DataTable dTable = new DataTable("DataTable");
                if (reporttype == "Verification Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.SpLVerificationReport", thisConnection);
                else if (reporttype == "P2FDetails Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.P2FDetailsReport", thisConnection);
                else if (reporttype == "Return PullOut Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.ReturnPullOutReport", thisConnection);
                else if (reporttype == "P2F PullOut Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.P2FPulloutReport", thisConnection);
                else if (reporttype == "Verification / CHI Reject Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.VerificationCHIRejectReport", thisConnection);
                else if (reporttype == "Bank wise P2F Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.P2FPulloutReport", thisConnection);
                else if (reporttype == "Batch Wise Summary Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.BatchWiseSummaryReport", thisConnection);
                else if (reporttype == "Day's Summary Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.DaysSummaryReport", thisConnection);
                else if (reporttype == "MICR Repair Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.MICRRepairReport", thisConnection);
                else if (reporttype == "L2 Modification Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.L2ModificationReport", thisConnection);
                else if (reporttype == "L3 Modification Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.L3ModificationReport", thisConnection);
                else if (reporttype == "Extension Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.ExtensionReport", thisConnection);
                else if (reporttype == "OW Productivity Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.ProductivityReport", thisConnection);
                else if (reporttype == "P2F Bankwise Summary")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.P2FBankwiseSummary", thisConnection);
                else if (reporttype == "IQA Failure Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.IQAFailureReport", thisConnection);
                else if (reporttype == "Unbundled Cheques Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.UnbundledChequesReport", thisConnection);
                else if (reporttype == "Duplicate Cheques Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.DuplicateChequesReport", thisConnection);
                else if (reporttype == "Return Memo Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.ReturnMemo", thisConnection);
                else if (reporttype == "Discrepant Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.DiscrepantReport", thisConnection);
                else if (reporttype == "Item Wise Presentation Details")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.PresentationDetails", thisConnection);
                else if (reporttype == "Audit Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.[sp_auditlogs]", thisConnection);
                else if (reporttype == "Bundled File Summary Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_BundledFileSummaryReport", thisConnection);
                else if (reporttype == "Return Report Unmatch")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_ReturnReport_Unmatch", thisConnection);
                else if (reporttype == "Return Report Unmatch Memo")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_ReturnReport_Unmatch", thisConnection);
                //Audit Report

                //Error("sqlCommand was " + sqlCommand);


                if (reporttype == "Verification Report")
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    //sqlCommand.Parameters.Add("@ProcessDate", SqlDbType.Date).Value = Convert.ToDateTime(tempDate);
                    sqlCommand.Parameters.Add("@FromDate", SqlDbType.Date).Value = Convert.ToDateTime(tempFromDate);
                    sqlCommand.Parameters.Add("@ToDate ", SqlDbType.Date).Value = Convert.ToDateTime(tempToDate);
                    sqlCommand.Parameters.Add("@ClearingType", SqlDbType.VarChar).Value = clr;
                    sqlCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = Convert.ToInt16(Session["CustomerID"].ToString());
                    sqlCommand.Parameters.Add("@VerificationLevel", SqlDbType.Int).Value = Bytevflevel;
                    sqlCommand.Parameters.Add("@DomainId", SqlDbType.Int).Value = domainid;
                }
                else
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    //sqlCommand.Parameters.Add("@ProcessDate", SqlDbType.Date).Value = Convert.ToDateTime(procdate);
                    // DateTime date = DateTime.ParseExact(procdate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    //string reformatted = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    //sqlCommand.Parameters.Add("@ProcessDate", SqlDbType.Date).Value = Convert.ToDateTime(tempDate); //Format(Convert.ToDateTime(procdate);
                    sqlCommand.Parameters.Add("@FromDate", SqlDbType.Date).Value = Convert.ToDateTime(tempFromDate);
                    sqlCommand.Parameters.Add("@ToDate ", SqlDbType.Date).Value = Convert.ToDateTime(tempToDate);
                    sqlCommand.Parameters.Add("@ClearingType", SqlDbType.VarChar).Value = clr;
                    sqlCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = Convert.ToInt16(Session["CustomerID"].ToString());
                    sqlCommand.Parameters.Add("@DomainId", SqlDbType.Int).Value = domainid;
                }

                //Error("added parameters ");

                //System.IO.File.WriteAllText(@"D:\Webroot\iKloudPro\Logs\WriteLines.txt", "line 3");

                ReportDocument rDocument = new ReportDocument();

                //System.IO.File.WriteAllText(@"D:\Webroot\iKloudPro\Logs\WriteLines.txt", "line 4");

                if (reporttype == "Verification Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/IwLxVerification.rpt");
                else if (reporttype == "P2FDetails Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/IWP2FDetails.rpt");
                else if (reporttype == "Return PullOut Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/OWReturnPullout.rpt");
                else if (reporttype == "P2F PullOut Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/P2FPulloutReport.rpt");
                else if (reporttype == "Verification / CHI Reject Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/VerificationCHIRejectReport.rpt");
                else if (reporttype == "Bank wise P2F Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/BankWiseP2FReport.rpt");
                else if (reporttype == "Batch Wise Summary Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/BatchWiseSummaryReport.rpt");
                else if (reporttype == "Day's Summary Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/DaysSummaryReport.rpt");
                else if (reporttype == "MICR Repair Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/MICRRepairReport.rpt");
                else if (reporttype == "L2 Modification Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/L2ModificationReport.rpt");
                else if (reporttype == "L3 Modification Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/L3ModificationReport.rpt");
                else if (reporttype == "Extension Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/ExtensionReport.rpt");
                else if (reporttype == "OW Productivity Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/ProductivityReport.rpt");
                else if (reporttype == "P2F Bankwise Summary")
                    reportPath = Server.MapPath("~/Reports/Crystal/P2FBankwiseSummary.rpt");
                else if (reporttype == "IQA Failure Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/IQAFailureReport.rpt");
                else if (reporttype == "Unbundled Cheques Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/UnbundledChequesReport.rpt");
                else if (reporttype == "Duplicate Cheques Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/DuplicateChequesReport.rpt");
                else if (reporttype == "Return Memo Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/ReturnMemo.rpt");
                else if (reporttype == "Discrepant Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/Discrepant_Report.rpt");
                else if (reporttype == "Item Wise Presentation Details")
                    reportPath = Server.MapPath("~/Reports/Crystal/PresentationDetails.rpt");
                else if (reporttype == "Audit Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/AuditReport.rpt");
                else if (reporttype == "Bundled File Summary Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/BundledFileSummaryReport.rpt");
                else if (reporttype == "Return Report Unmatch")
                    reportPath = Server.MapPath("~/Reports/Crystal/ReturnReportUnmatch.rpt");
                else if (reporttype == "Return Report Unmatch Memo")
                    reportPath = Server.MapPath("~/Reports/Crystal/ReturnReportUnmatchMemo.rpt");

                //Error("report path was " + reportPath);

                //System.IO.File.WriteAllText(@"D:\Webroot\iKloudPro\Logs\WriteLines.txt", "line 5");
                rDocument.Load(reportPath);

                //System.IO.File.WriteAllText(@"D:\Webroot\iKloudPro\Logs\WriteLines.txt", "line 6");

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    dTable.Load(reader);
                    rDocument.SetDataSource(dTable);
                }

                //System.IO.File.WriteAllText(@"D:\Webroot\iKloudPro\Logs\WriteLines.txt", "line 7");

                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                ///----------------PDF----------------------

                if (filedwnldtype == "EXCEL")
                {
                    //-------------------------EXCEl------------------------
                    //Stream str = rDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                    Stream str = rDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.ExcelWorkbook);
                    str.Seek(0, SeekOrigin.Begin);
                    return File(str, "application/excel", reporttype + ".xls");
                }
                else if (filedwnldtype == "CSV")
                {
                    //--------CSV-----------------
                    Stream str = rDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.CharacterSeparatedValues);
                    str.Seek(0, SeekOrigin.Begin);
                    return File(str, "application/csv", reporttype + ".csv");
                }
                else
                {
                    //Error("generating report in PDF ");

                    //System.IO.File.WriteAllText(@"D:\Webroot\iKloudPro\Logs\WriteLines.txt", "line 8");

                    Stream str = rDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    str.Seek(0, SeekOrigin.Begin);

                    //System.IO.File.WriteAllText(@"D:\Webroot\iKloudPro\Logs\WriteLines.txt", "line 9");

                    //Error("returning from PDF ");

                    return File(str, "application/pdf");//, "maintransaction.pdf");                }
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

            // return File();
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
