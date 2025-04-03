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

namespace ikloud_Aflatoon.Controllers
{
    public class IWReportController : Controller
    {
        //
        // GET: /IWReport/
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
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", 
                    popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            //  ViewBag.listuser = null;
            ViewBag.listuser = new SelectList(af.UserMasters.Where(u => u.L2StopLimit != 0 || u.L3StopLimit != 0), "LoginID", "LoginID").ToList();
            return View();
        }

        public ActionResult IWActionReport(string procdate = null, string clrtypr = null, string reporttype = null, string vflevel = null, string UserName = null, string filedwnldtype = null)
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
            else
                clr = "11";
            try
            {

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
                else
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.SpUserWiseVerificationReport", thisConnection);//

                if (reporttype == "Productivity Report" || reporttype == "Verification Report")
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add("@ProcessDate", SqlDbType.Date).Value = Convert.ToDateTime(procdate);
                    sqlCommand.Parameters.Add("@ClearingType", SqlDbType.VarChar).Value = clr;
                    sqlCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = Convert.ToInt16(Session["CustomerID"].ToString());
                    sqlCommand.Parameters.Add("@VerificationLevel", SqlDbType.Int).Value = Bytevflevel;
                }
                else if (reporttype == "Audit Report")
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add("@ProcessDate", SqlDbType.Date).Value = Convert.ToDateTime(procdate);
                    sqlCommand.Parameters.Add("@ClearingType", SqlDbType.VarChar).Value = clr;
                    sqlCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = Convert.ToInt16(Session["CustomerID"].ToString());
                    sqlCommand.Parameters.Add("@VerificationLevel", SqlDbType.Int).Value = Bytevflevel;
                    sqlCommand.Parameters.Add("@UserID", SqlDbType.VarChar).Value = UserName;
                }
                else if (reporttype == "L1 Rejected Report" || reporttype == "L2 Rejected Report")
                {
                    string tempprocdate = procdate.Substring(6, 4) + "-" + procdate.Substring(0, 2) + "-" + procdate.Substring(3, 2);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add("@ProcessDate", SqlDbType.Date).Value = Convert.ToDateTime(tempprocdate);
                    sqlCommand.Parameters.Add("@ClearingType", SqlDbType.VarChar).Value = clr;
                    sqlCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = Convert.ToInt16(Session["CustomerID"].ToString());
                }
                else if (reporttype == "Final Status Report")
                {
                    string tempprocdate = procdate.Substring(6, 4) + "-" + procdate.Substring(0, 2) + "-" + procdate.Substring(3, 2);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add("@ProcessDate", SqlDbType.Date).Value = Convert.ToDateTime(tempprocdate);
                    sqlCommand.Parameters.Add("@ClearingType", SqlDbType.VarChar).Value = clr;
                    sqlCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = Convert.ToInt16(Session["CustomerID"].ToString());
                }
                else
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add("@ProcessDate", SqlDbType.Date).Value = Convert.ToDateTime(procdate);
                    sqlCommand.Parameters.Add("@ClearingType", SqlDbType.VarChar).Value = clr;
                    sqlCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = Convert.ToInt16(Session["CustomerID"].ToString());
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
                else
                    reportPath = Server.MapPath("~/Reports/Crystal/IWReturnDetails.rpt");

                //string filename = DateTime.Now.ToString("ddMMyyyy") + "IwrepLogs.txt";
                //string fileNameWithPath = @"c:\Logs\" + filename;
                //StreamWriter str1 = new StreamWriter(fileNameWithPath, true, System.Text.Encoding.Default);
                //str1.WriteLine("reportPath is " + reportPath);
                //str1.Close();


                rDocument.Load(reportPath);

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    dTable.Load(reader);
                    rDocument.SetDataSource(dTable);
                }

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
                    //---------------------
                    //Stream str = rDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    //str.Seek(0, SeekOrigin.Begin);
                    //return File(str, "application/pdf");//, "maintransaction.pdf");
                    //rDocument.Close();
                    //rDocument.Dispose();
                    //GC.Collect();
                    //-------------
               
                   
                }
                catch (Exception e)
                {
                    return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
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
        public ActionResult getUsers(string VFlevel = null)
        {

            var users = af.UserMasters.Where(a => a.L2StopLimit != 0 || a.L3StopLimit != 0).Select(a => "<option value='" + a.LoginID + "'>" + a.FirstName + "'</option>'");

            return Content(String.Join("", users));
        }

    }
}
