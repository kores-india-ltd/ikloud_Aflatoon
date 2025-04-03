using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ikloud_Aflatoon.Infrastructure;
using System.Collections;
using ikloud_Aflatoon.Models;
using System.Drawing;

namespace ikloud_Aflatoon.Controllers
{
    public class IWL2Controller : Controller
    {
        //
        // GET: /L2Verification/
        AflatoonEntities af = new AflatoonEntities();
        IWProcDataContext iwpro = new IWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
        string imgpth = System.Configuration.ConfigurationManager.AppSettings["snippath"].ToString();
        List<Int64> listId = new List<Int64>();

        public ActionResult IWDataEntry(int id = 0)
        {
            int custid = Convert.ToInt16(Session["CustomerID"]);
            var varMinAclen = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "ACFrom").SettingValue;
            var varMaxAclen = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "ACTo").SettingValue;
            var varMaxPayeelen = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "PayeeNameMaxLength").SettingValue;

            int intMinAclen = Convert.ToInt32(varMinAclen);
            int intMaxAclen = Convert.ToInt32(varMaxAclen);
            int intMaxPayeelen = Convert.ToInt32(varMaxPayeelen);

            ViewBag.MinAclen = intMinAclen;
            ViewBag.MaxAclen = intMaxAclen;
            ViewBag.MaxPayeelen = intMaxPayeelen;

            ViewBag.MinAclen = intMinAclen;
            ViewBag.MaxAclen = intMaxAclen;

            var isBranchWiseInwardVerification = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "IsBranchWiseInwardVerification")?.SettingValue;
            if (isBranchWiseInwardVerification == null || isBranchWiseInwardVerification == "")
            {
                ViewBag.IsBranchWiseInwardVerification = 0;
            }
            else
            {
                if (isBranchWiseInwardVerification == "1")
                {
                    ViewBag.IsBranchWiseInwardVerification = 1;
                }
                else
                {
                    ViewBag.IsBranchWiseInwardVerification = 0;
                }
            }


            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["VF"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            try
            {
                DataSet ds = new DataSet();
                if (id == 1)
                {
                    ViewBag.BackButton = "Y";
                    int uid = (int)Session["uid"];
                    SqlDataAdapter adp = new SqlDataAdapter("IWSelectDataEntryForBackRecord", con);
                    adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                    adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                    adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();
                                                                                                                                                                                 //--------------------Customer Selection---------------------
                    adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                    

                    adp.Fill(ds);
                }
                else
                {
                    ViewBag.BackButton = "N";
                    int uid = (int)Session["uid"];
                    SqlDataAdapter adp = new SqlDataAdapter("IWSelectDataEntry", con);
                    adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                    adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                    adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();
                                                                                                                                                                                 //--------------------Customer Selection---------------------
                    adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                    adp.SelectCommand.Parameters.Add("@IsBranchWiseInwardVerification", SqlDbType.NVarChar).Value = Convert.ToInt16(isBranchWiseInwardVerification);

                    adp.Fill(ds);
                }


                var objectlst = new List<IWL2Verification>();
                IWL2Verification def;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    def = new IWL2Verification
                    {
                        Id = Convert.ToInt64(ds.Tables[0].Rows[0]["ID"]),
                        FrontGreyImagePath = ds.Tables[0].Rows[0]["FrontGreyImagePath"].ToString().Replace("\\", "/"),
                        FrontTiffImagePath = ds.Tables[0].Rows[0]["FrontTiffImagePath"].ToString().Replace("\\", "/"),
                        BackTiffImagePath = ds.Tables[0].Rows[0]["BackTiffImagePath"].ToString().Replace("\\", "/"),
                        XMLPayeeName = ds.Tables[0].Rows[0]["XMLPayeeName"].ToString(),
                        EntryAccountNo = ds.Tables[0].Rows[0]["DbtAccNo"] == null || ds.Tables[0].Rows[0]["DbtAccNo"].ToString() == "" ? "" : ds.Tables[0].Rows[0]["DbtAccNo"].ToString(),
                        EntryChqDate = ds.Tables[0].Rows[0]["ChqDate"] == null || ds.Tables[0].Rows[0]["ChqDate"].ToString() == "" ? "" : ds.Tables[0].Rows[0]["ChqDate"].ToString(),
                    };
                    //objectlst.Add(def);
                    //------------------------END------------------------//

                    if (id == 1)
                    {
                        Session["deEntryAccountNo"] = ds.Tables[0].Rows[0]["DbtAccNo"] == null ? "" : ds.Tables[0].Rows[0]["DbtAccNo"].ToString();
                        Session["deEntryChqDate"] = ds.Tables[0].Rows[0]["ChqDate"] == null ? "" : ds.Tables[0].Rows[0]["ChqDate"].ToString().Substring(8, 2) + ds.Tables[0].Rows[0]["ChqDate"].ToString().Substring(5, 2) + ds.Tables[0].Rows[0]["ChqDate"].ToString().Substring(2, 2);
                        Session["deXMLPayeeName"] = ds.Tables[0].Rows[0]["EntryPayeeName"] == null ? "" : ds.Tables[0].Rows[0]["EntryPayeeName"].ToString();
                    }
                    else
                    {
                        Session["deEntryAccountNo"] = ds.Tables[0].Rows[0]["DbtAccNo"] == null || ds.Tables[0].Rows[0]["DbtAccNo"].ToString() == "" ? "" : ds.Tables[0].Rows[0]["DbtAccNo"].ToString();
                        Session["deEntryChqDate"] = ds.Tables[0].Rows[0]["ChqDate"] == null || ds.Tables[0].Rows[0]["ChqDate"].ToString() == "" ? "" : ds.Tables[0].Rows[0]["ChqDate"].ToString().Substring(8, 2) + ds.Tables[0].Rows[0]["ChqDate"].ToString().Substring(5, 2) + ds.Tables[0].Rows[0]["ChqDate"].ToString().Substring(2, 2);
                        Session["deXMLPayeeName"] = ds.Tables[0].Rows[0]["XMLPayeeName"] == null ? "" : ds.Tables[0].Rows[0]["XMLPayeeName"].ToString();
                    }

                    var rtnlist = (from i in af.ItemReturnReasons select i).ToList();// iwafl.ItemReturnReasons.Select(m).ToList();
                    ViewBag.rtnlist = rtnlist.Select(m => m.RETURN_REASON_CODE).ToList();
                    ViewBag.rtnlistDescrp = rtnlist.Select(m => m.DESCRIPTION).ToList();

                    ViewBag.cnt = true;
                    Session["glob"] = null;
                    return View(def);
                }
                else
                {
                    return RedirectToAction("IWIndex", "Home", new { id = 1 });
                }

            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }

        [HttpPost]
        public ActionResult IWDataEntry(IWL2Verification dataEntryView, string btnSubmit, string IWDecision, string IWRemark, string rejectreasondescrpsn)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }

            int uid = (int)Session["uid"];
            string dt = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
            int custId = Convert.ToInt16(Session["CustomerID"]);
            try
            {
                if (btnSubmit == "Save")
                {
                    string cdate = dataEntryView.EntryChqDate;
                    string cdatenew = "";
                    if(cdate == "" || cdate == null)
                    {
                        cdatenew = "";
                    }
                    else
                    {
                        cdatenew = "20" + cdate.Substring(4, 2) + "-" +
                        cdate.Substring(2, 2) + "-" +
                        cdate.Substring(0, 2);
                    }

                    string accountNo = "";
                    if(dataEntryView.EntryAccountNo == "" || dataEntryView.EntryAccountNo == null)
                    {
                        accountNo = "";
                    }
                    else
                    {
                        accountNo = dataEntryView.EntryAccountNo;
                    }
                    

                    iwpro.UpdateIWL2VerificationF8(dataEntryView.Id, uid, accountNo, cdatenew, dataEntryView.XMLPayeeName,
                        @Session["LoginID"].ToString(), dt, custId, IWDecision, IWRemark, rejectreasondescrpsn);

                    Session["CurrentDataEntryCount"] = Convert.ToInt16(Session["CurrentDataEntryCount"]) + 1;
                    return RedirectToAction("IWDataEntry", "IWL2", new { id = 0 });
                }

            //---------------------------IF Close button called--------------------------------//
            Close:
                if (btnSubmit == "Close")
                {
                    iwpro.UnlockRecords(dataEntryView.Id, "DE", uid, @Session["LoginID"].ToString(), dt, custId);
                    /// Int64 SlipRawaDataID = 0;
                    Session["glob"] = true;
                    return RedirectToAction("IWIndex", "Home", new { id = 2 });
                }

                //-------------Calling next Records---------------
                if (btnSubmit == "Back")
                {
                    return RedirectToAction("IWDataEntry", "IWL2", new { id = 1 });
                }

                return RedirectToAction("IWIndex", "Home", new { id = 1 });
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }

        public ActionResult ShowPreviousRecord()
        {
            try
            {

                return RedirectToAction("IWDataEntry", "IWDataEntry", new { id = 1 });
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
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

        public ActionResult getTiffImageNew(string httpwebimgpath = null)
        {
            try
            {

                int custid = Convert.ToInt16(Session["CustomerID"]);
                var destpath = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "PhysicalPath");

                //Owsr.L1VerificationName = l1result.LoginID;
                string destroot = destpath.SettingValue;

                logerror(httpwebimgpath, httpwebimgpath.ToString() + " - >");

                const char delimiter = '\\';
                string[] destrootarr = destroot.Split(delimiter);

                string foldrname = destrootarr[destrootarr.Length - 1];

                logerror(foldrname, foldrname.ToString() + " - > Folder Name");

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
                logerror(destroot, destroot.ToString() + "-> Root Path");
                actualpath = destroot + "\\" + actualpath;
                // System.IO.File.AppendAllText("C:\\temp\\log1.txt", "actualpathOWL1:" + actualpath);
                logerror(actualpath, actualpath.ToString());
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
                logerror(imageDataURL, imageDataURL.ToString());
                return PartialView("_getTiffImage");
            }
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "SMB L2 Get Tiff Image Index- " + innerExcp });
            }

        }

        public ActionResult getTiffImage(string httpwebimgpath = null)
        {
            try
            {

                int custid = Convert.ToInt16(Session["CustomerID"]);
                var destpath = af.ApplicationSettings.FirstOrDefault((p) => p.CustomerId == custid && p.SettingName == "DestinationDownloadFolder");

                //Owsr.L1VerificationName = l1result.LoginID;
                string destroot = destpath.SettingValue;

                logerror(httpwebimgpath, httpwebimgpath.ToString() + " - >");

                const char delimiter = '\\';
                string[] destrootarr = destroot.Split(delimiter);

                string foldrname = destrootarr[destrootarr.Length - 1];

                logerror(foldrname, foldrname.ToString() + " - > Folder Name");

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
                logerror(destroot, destroot.ToString() + "-> Root Path");
                actualpath = destroot + "\\" + actualpath;
                // System.IO.File.AppendAllText("C:\\temp\\log1.txt", "actualpathOWL1:" + actualpath);
                logerror(actualpath, actualpath.ToString());
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
                logerror(imageDataURL, imageDataURL.ToString());
                return PartialView("_getTiffImage");
            }
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "SMB L2 Get Tiff Image Index- " + innerExcp });
            }

        }

        public ActionResult ShowProductivity()
        {
            try
            {
                int uid = (int)Session["uid"];
                string sessionType = Convert.ToString(Session["CtsSessionType"]);
                string type = Convert.ToString(Session["ProType"]);
                string loginId = Session["LoginID"].ToString();

                SqlDataAdapter sqladlog = new SqlDataAdapter("GetProductivityLogs", con);
                sqladlog.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqladlog.SelectCommand.Parameters.AddWithValue("@uid", SqlDbType.NVarChar).Value = uid;
                sqladlog.SelectCommand.Parameters.AddWithValue("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                sqladlog.SelectCommand.Parameters.AddWithValue("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                sqladlog.SelectCommand.Parameters.AddWithValue("@Type", SqlDbType.NVarChar).Value = type;
                sqladlog.SelectCommand.Parameters.AddWithValue("@LoginId", SqlDbType.NVarChar).Value = loginId;
                DataSet dslog = new DataSet();
                sqladlog.Fill(dslog);

                int totalcount = 0;
                int index = 0;

                List<ProductivityLogs> ProdLogs = new List<ProductivityLogs>();
                if (dslog.Tables[0].Rows.Count > 0)
                {
                    ProdLogs = (from DataRow dr in dslog.Tables[0].Rows
                                select new ProductivityLogs
                                {
                                    LogLevel = dr["Loglevel"].ToString(),
                                    LoginId = dr["LoginId"].ToString(),
                                    ProcessingDate = Convert.ToDateTime(dr["Processingdate"]).ToString("dd-MM-yyyy"),
                                    //CustomerId = Convert.ToInt16(dr["CustomerId"]),
                                    Count = Convert.ToInt64(dr["Count"])

                                }).ToList();

                }
                return View("_ShowProductivity", ProdLogs);
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }

        public PartialViewResult RejectReason(int id = 0)
        {

            var rjrs = (from r in af.ItemReturnReasons
                        select new RejectReason
                        {
                            Description = r.DESCRIPTION,
                            ReasonCodeS = r.RETURN_REASON_CODE
                        });
            return PartialView("_L2RejectReason", rjrs);
        }
    }
}
