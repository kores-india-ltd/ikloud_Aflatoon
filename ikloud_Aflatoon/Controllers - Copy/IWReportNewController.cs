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
    public class IWReportNewController : Controller
    {
        //
        // GET: /IWReport/SMB Images Report
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
            
            string clr = "";
            
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

                if (reporttype == "SMB Images Report")
                {
                    sqlCommand = new System.Data.SqlClient.SqlCommand("dbo.RPT_IWSMBImagesReport", thisConnection);

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add("@ProcessDate", SqlDbType.Date).Value = Convert.ToDateTime(procdate);
                    sqlCommand.Parameters.Add("@ClearingType", SqlDbType.VarChar).Value = clr;
                    sqlCommand.Parameters.Add("@CustomerId", SqlDbType.Int).Value = Convert.ToInt16(Session["CustomerID"].ToString());

                    reportPath = Server.MapPath("~/Reports/Crystal/RPT_IWSMBImagesReport.rpt");

                    ReportDocument rDocument = new ReportDocument();
                    rDocument.Load(reportPath);

                    using (SqlDataAdapter da = new SqlDataAdapter(sqlCommand))
                    {
                        da.Fill(dTable);
                        if (reporttype == "SMB Images Report")
                        {
                            for (int i = 0; i < dTable.Rows.Count; i++)
                            {
                                DataRow dr = dTable.Rows[i];
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

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    Stream str = rDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    str.Seek(0, SeekOrigin.Begin);
                    //--------------------Disposing Objects
                    rDocument.Close();
                    rDocument.Clone();
                    rDocument.Dispose();
                    rDocument = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    return File(str, "application/pdf");//, "maintransaction.pdf");    
                }
                return RedirectToAction("IWIndex", "Home");
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
    }
}
