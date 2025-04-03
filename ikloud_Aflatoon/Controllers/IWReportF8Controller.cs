using CrystalDecisions.CrystalReports.Engine;
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
    public class IWReportF8Controller : Controller
    {
        //
        // GET: /IWReportF8/

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

            int custid = Convert.ToInt16(Session["CustomerID"].ToString());

            //var vardom = (from a in af.DomainMaster
            //              from ud in af.DomainUserMapMasters
            //              where a.Id == ud.DomainId && a.CustomerId == custid &&
            //              ud.UserId == uid
            //              select new
            //              {
            //                  Id = a.Id,
            //                  Name = a.Name
            //              }
            //                 ).ToList();

            var vardom = (
                          from d in af.DomainMaster
                          from du in af.DomainUserMapMasters
                          from b in af.BranchMaster
                          where b.OwDomainId == d.Id && d.Id == du.DomainId && d.CustomerId == custid && du.UserId == uid
                          select new
                          {
                              //Id = (b.MICRCode).Substring(6,3),
                              Id = b.MICRCode,
                              Name = b.BranchName + " (" + b.BranchCode + ") (" + b.MICRCode + ")"
                          }).ToList();

            ViewBag.gridDomains = new SelectList(vardom, "Id", "Name");

            return View();
        }

        public ActionResult IWActionReport(string procdate = null, string clrtypr = null, string reporttype = null, string vflevel = null, string UserName = null, string filedwnldtype = null, int domainid = 0)
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
                string tempProcDate = procdate.Substring(6, 4) + "-" + procdate.Substring(3, 2) + "-" + procdate.Substring(0, 2);

                SqlConnectionStringBuilder SConn = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
                string thisConnectionString = SConn.ConnectionString;
                SqlConnection thisConnection = new SqlConnection(thisConnectionString);
                thisConnection.Open();
                SqlCommand sqlCommand;
                string reportPath = "";
                DataTable dTable = new DataTable("DataTable");

                if (reporttype == "Return Details Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.IWReturnDetailsReportF8", thisConnection);
                else if (reporttype == "SMB Images Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_IWSMBImagesReportF8", thisConnection);
                else if (reporttype == "Inward BankWise Received Cheques Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_Inward_BankWise_ReceivedChequesF8", thisConnection);
                else if (reporttype == "Inward BranchWise Received Cheques Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_Inward_BranchWise_ReceivedChequesF8", thisConnection);
                else if (reporttype == "Inward BranchWise Summary Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_Inward_BranchWise_SummaryF8", thisConnection);
                else if (reporttype == "Inward Outward Clearing Settlement Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_Inward_Outward_Clearing_Settlement_Report", thisConnection);
                else if (reporttype == "Inward Outward Return Settlement Report")
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_Inward_Outward_Return_Settlement_Report", thisConnection);
                else
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.SpUserWiseVerificationReport", thisConnection);//

                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.Add("@ProcessDate", SqlDbType.Date).Value = Convert.ToDateTime(tempProcDate);
                sqlCommand.Parameters.Add("@ClearingType", SqlDbType.VarChar).Value = clr;
                sqlCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = Convert.ToInt16(Session["CustomerID"].ToString());
                sqlCommand.Parameters.Add("@DomainID", SqlDbType.Int).Value = domainid;

                ReportDocument rDocument = new ReportDocument();

                if (reporttype == "SMB Images Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_IWSMBImagesReport.rpt");
                else if (reporttype == "Inward BankWise Received Cheques Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_InwardBankWiseReceivedChequesReport.rpt");
                else if (reporttype == "Inward BranchWise Received Cheques Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_InwardBranchWiseReceivedChequesReport.rpt");
                else if (reporttype == "Inward BranchWise Summary Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_InwardBranchWiseSummaryReport.rpt");
                else if (reporttype == "Inward Outward Clearing Settlement Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_Inward_Outward_Clearing_Settlement_Report.rpt");
                else if (reporttype == "Inward Outward Return Settlement Report")
                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_Inward_Outward_Return_Settlement_Report.rpt");
                else
                    reportPath = Server.MapPath("~/Reports/Crystal/IWReturnDetailsF8.rpt");

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

                    if (reporttype == "SMB Images Report")
                    {
                        for (int i = 0; i < dTable.Rows.Count; i++)
                        {
                            DataRow dr = dTable.Rows[i];
                            string FilePath1 = (string)dr["FrontGreyImagePath"];
                            string FilePath2 = (string)dr["FrontTiffImagePath"];
                            string FilePath3 = (string)dr["BackTiffImagePath"];
                            FilePath1 = FilePath1.Replace("ikloudpro.kores.in", "10.168.201.16");
                            FilePath2 = FilePath2.Replace("ikloudpro.kores.in", "10.168.201.16");
                            FilePath3 = FilePath3.Replace("ikloudpro.kores.in", "10.168.201.16");
                            string localPath = new Uri(FilePath1).LocalPath;
                            try
                            {
                                //------------------FrontGreyImagePath convert into byte--------
                                HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(FilePath1);
                                HttpWebResponse response1 = (HttpWebResponse)request1.GetResponse();
                                Stream receiveStream1 = response1.GetResponseStream();
                                byte[] imgbyte1 = ReadFully(receiveStream1);
                                dr["FGrayImg"] = imgbyte1;
                                //------------------FrontTiffImagePath convert into byte--------
                                HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(FilePath2);
                                HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();
                                Stream receiveStream2 = response2.GetResponseStream();
                                byte[] imgbyte2 = ReadFully(receiveStream2);
                                dr["FTiffImg"] = imgbyte2;
                                //------------------BackTiffImagePath convert into byte--------
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

    }
}
