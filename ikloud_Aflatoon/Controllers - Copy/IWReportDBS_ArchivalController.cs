using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ikloud_Aflatoon.Controllers
{
    public class IWReportDBS_ArchivalController : Controller
    {
        AflatoonEntities af = new AflatoonEntities();

        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["IkloudPro_ImageArchival"].ConnectionString);

        //
        // GET: /IWReportDBS_Archival/

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

            return View();
        }

        public ActionResult IWActionReport(string fromdate = null, string todate = null, string clrtypr = null, string reporttype = null, string vflevel = null, string UserName = null, string filedwnldtype = null, string dbyear = null)
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
                //string tempProcDate = procdate.Substring(6, 4) + "-" + procdate.Substring(3, 2) + "-" + procdate.Substring(0, 2);
                string tempFromDate = fromdate.Substring(6, 4) + "-" + fromdate.Substring(3, 2) + "-" + fromdate.Substring(0, 2);
                string tempToDate = todate.Substring(6, 4) + "-" + todate.Substring(3, 2) + "-" + todate.Substring(0, 2);

                //SqlConnectionStringBuilder SConn = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
                //string thisConnectionString = SConn.ConnectionString;
                string thisConnectionString = GetConnectionString(dbyear);
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

                    //if (reporttype == "Return Memo With Image Report")
                    //{
                    //    for (int i = 0; i < dTable.Rows.Count; i++)
                    //    {
                    //        DataRow dr = dTable.Rows[i];
                    //        string FilePath1 = (string)dr["FrontGreyImagePath"];
                    //        string localPath = new Uri(FilePath1).LocalPath;
                    //        try
                    //        {
                    //            //------------------FrontGreyImagePath convert into byte--------
                    //            HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(FilePath1);
                    //            HttpWebResponse response1 = (HttpWebResponse)request1.GetResponse();
                    //            Stream receiveStream1 = response1.GetResponseStream();
                    //            byte[] imgbyte1 = ReadFully(receiveStream1);
                    //            dr["FGrayImg"] = imgbyte1;
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            //throw ex;
                    //            //continue;
                    //        }
                    //    }
                    //}

                    if (reporttype == "SMB Images Report")
                    {
                        for (int i = 0; i < dTable.Rows.Count; i++)
                        {
                            DataRow dr = dTable.Rows[i];
                            int status = 0;
                            status = Convert.ToInt32(dTable.Rows[i]["Status"] == DBNull.Value ? 0 : dTable.Rows[i]["Status"]);

                            if (status == 2 || status == 4 || status == 12 || status == 14 || status == 22 || status == 24)
                            {
                                string connectionString = GetConnectionString(dbyear);
                                using (SqlConnection con = new SqlConnection(connectionString))
                                {
                                    DataSet ds111 = new DataSet();
                                    SqlDataAdapter adp111 = new SqlDataAdapter("IWGetChequeImageEncrypted", con);
                                    adp111.SelectCommand.CommandType = CommandType.StoredProcedure;
                                    adp111.SelectCommand.Parameters.Add("@id", SqlDbType.BigInt).Value = Convert.ToInt32(dTable.Rows[i]["ID"].ToString());
                                    adp111.Fill(ds111);

                                    if (ds111.Tables[0].Rows.Count > 0)
                                    {

                                        byte[] imageBytesFrontGray = (byte[])(ds111.Tables[0].Rows[0]["FrontGray"]);
                                        //string imageDataURLFrontGray = CreateImageDataURL(imageBytesFrontGray);

                                        //------------------FrontTiffImagePath convert into byte--------
                                        byte[] imageBytesFrontTiff = (byte[])(ds111.Tables[0].Rows[0]["FrontTiff"]);
                                        //string imageDataURLFrontTiff = CreateImageDataURL(imageBytesFrontTiff);
                                        Byte[] jpegBytes;

                                        using (MemoryStream inStream = new MemoryStream(imageBytesFrontTiff))
                                        using (MemoryStream outStream = new MemoryStream())
                                        {
                                            System.Drawing.Bitmap.FromStream(inStream).Save(outStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                            jpegBytes = outStream.ToArray();
                                        }

                                        //------------------BackTiffImagePath convert into byte--------
                                        byte[] imageBytesBackTiff = (byte[])(ds111.Tables[0].Rows[0]["BackTiff"]);
                                        //string imageDataURLBackTiff = CreateImageDataURL(imageBytesBackTiff);
                                        Byte[] jpegBytes3;

                                        using (MemoryStream inStream3 = new MemoryStream(imageBytesBackTiff))
                                        using (MemoryStream outStream3 = new MemoryStream())
                                        {
                                            System.Drawing.Bitmap.FromStream(inStream3).Save(outStream3, System.Drawing.Imaging.ImageFormat.Jpeg);
                                            jpegBytes3 = outStream3.ToArray();
                                        }

                                        //frontGreyImagePath = imageDataURLFrontGray;
                                        //frontTiffImagePath = imageDataURLFrontTiff;
                                        //backTiffImagePath = imageDataURLBackTiff;

                                        dr["FGrayImg"] = imageBytesFrontGray;
                                        dr["FTiffImg"] = jpegBytes;
                                        dr["BTiffImg"] = jpegBytes3;
                                    }
                                }
                                    

                            }
                            else
                            {
                                string FilePath1 = (string)dr["FrontGreyImagePath"];
                                string FilePath2 = (string)dr["FrontTiffImagePath"];
                                string FilePath3 = (string)dr["BackTiffImagePath"];
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
                                    Byte[] jpegBytes;

                                    using (MemoryStream inStream = new MemoryStream(imgbyte2))
                                    using (MemoryStream outStream = new MemoryStream())
                                    {
                                        System.Drawing.Bitmap.FromStream(inStream).Save(outStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                        jpegBytes = outStream.ToArray();
                                    }
                                    dr["FTiffImg"] = jpegBytes;

                                    //------------------BackTiffImagePath convert into byte--------
                                    HttpWebRequest request3 = (HttpWebRequest)WebRequest.Create(FilePath3);
                                    HttpWebResponse response3 = (HttpWebResponse)request3.GetResponse();
                                    Stream receiveStream3 = response3.GetResponseStream();
                                    byte[] imgbyte3 = ReadFully(receiveStream3);
                                    Byte[] jpegBytes3;

                                    using (MemoryStream inStream3 = new MemoryStream(imgbyte3))
                                    using (MemoryStream outStream3 = new MemoryStream())
                                    {
                                        System.Drawing.Bitmap.FromStream(inStream3).Save(outStream3, System.Drawing.Imaging.ImageFormat.Jpeg);
                                        jpegBytes3 = outStream3.ToArray();
                                    }
                                    dr["BTiffImg"] = jpegBytes3;
                                }
                                catch (Exception ex)
                                {
                                    //throw ex;
                                    //continue;
                                }
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

        public string CreateImageDataURL(byte[] imageBytes)
        {
            //byte[] imageBytes = Encoding.ASCII.GetBytes(ds111.Tables[0].Rows[index]["FrontGray"].ToString());
            //byte[] imageBytes = (byte[])(dataString);
            //string imageBase64Data = Convert.ToBase64String(imageBytes);
            //Array.Clear(imageBytes, 0, imageBytes.Length);

            Stream streamactual = new MemoryStream(imageBytes);
            System.Drawing.Bitmap bmp = new Bitmap(streamactual);
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            stream.Position = 0;

            byte[] data = new byte[stream.Length];
            int lngth = (int)stream.Length;
            stream.Read(data, 0, lngth);
            stream.Close();

            string imageBase64Data = Convert.ToBase64String(data);
            Array.Clear(imageBytes, 0, imageBytes.Length);

            string imageDataURL = string.Format("data:image/jpeg;base64,{0}", imageBase64Data);
            //ViewBag.ImageDataFrontGray = imageDataURL;

            return imageDataURL;
        }

        public string GetConnectionString(string dbyear = null)
        {
            string dataSource = "", intialCatalog = "", userId = "", password = "", connectionString = "";

            dataSource = System.Configuration.ConfigurationManager.AppSettings["DataSource"].ToString();
            intialCatalog = System.Configuration.ConfigurationManager.AppSettings["InitialCatalog"].ToString();
            userId = System.Configuration.ConfigurationManager.AppSettings["UserID"].ToString();
            password = System.Configuration.ConfigurationManager.AppSettings["Password"].ToString();

            connectionString = "Data Source=" + dataSource + ";Initial Catalog=" + intialCatalog + "_" + dbyear + ";User ID=" + userId + ";Password=" + password;

            return connectionString;
        }
    }
}
