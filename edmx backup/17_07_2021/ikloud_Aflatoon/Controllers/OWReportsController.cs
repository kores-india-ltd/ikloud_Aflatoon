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

            //var scanningType = (from a)
            return View();
        }

        public ActionResult OWActionReport(string fromdate = null, string todate = null, string clrtypr = null, string reporttype = null,string filedwnldtype = null, int domainid = 0)
        {

            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];

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
                else if (reporttype == "Return Memo Report With Image" || reporttype == "Return Memo Report Without Image")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_ReturnMemo", thisConnection);
                else if (reporttype == "CHI Reject Report")                                                  
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.VerificationCHIRejectReport", thisConnection);

                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.Add("@FromDate", SqlDbType.Date).Value = Convert.ToDateTime(tempFromDate);
                sqlCommand.Parameters.Add("@ToDate ", SqlDbType.Date).Value = Convert.ToDateTime(tempToDate);
                sqlCommand.Parameters.Add("@ClearingType", SqlDbType.VarChar).Value = clr;
                sqlCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = Convert.ToInt16(Session["CustomerID"].ToString());
                sqlCommand.Parameters.Add("@DomainId", SqlDbType.Int).Value = domainid;

                if  (reporttype == "Batch Wise Summary Report")                                 
                    reportPath = Server.MapPath("~/Reports/Crystal/BatchWiseSummaryReport.rpt");
                else if (reporttype == "Bank wise P2F Report")                                      
                    reportPath = Server.MapPath("~/Reports/Crystal/BankWiseP2FReport.rpt");
                else if (reporttype == "Bundled File Summary Report")                       
                    reportPath = Server.MapPath("~/Reports/Crystal/BundledFileSummaryReport.rpt");
                else if (reporttype == "Day's Summary Report")                                      
                    reportPath = Server.MapPath("~/Reports/Crystal/DaysSummaryReport.rpt");
                else if (reporttype == "Discrepant Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/DiscrepantReport.rpt");
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
                    reportPath = Server.MapPath("~/Reports/Crystal/OWReturnPullout.rpt");
                else if (reporttype == "Return Memo Report With Image")             
                    reportPath = Server.MapPath("~/Reports/Crystal/ReturnMemo.rpt");
                else if (reporttype == "CHI Reject Report")              
                    reportPath = Server.MapPath("~/Reports/Crystal/CHIRejectReport.rpt");

                rDocument.Load(reportPath);

                using (SqlDataAdapter da = new SqlDataAdapter(sqlCommand))
                {

                    da.Fill(dTable);
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
