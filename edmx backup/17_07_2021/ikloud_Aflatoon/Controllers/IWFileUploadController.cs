using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ikloud_Aflatoon.Controllers
{
    public class IWFileUploadController : Controller
    {
        //
        // GET: /IWFileUpload/
        AflatoonEntities af = new AflatoonEntities();
        IWProcDataContext iwpro = new IWProcDataContext();
        public ActionResult Index()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            //----------------------------------Checking user rights--------------------//

            if ((bool)Session["fildwnd"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            return View();
        }
        public ActionResult IWReturnUpload()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["fildwnd"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            return View("IWReturnUpload");
        }

        public FileStreamResult IWCreateFile(string id = null)
        {


            byte[] buff = null;
            FileStream fs;
            BinaryReader br;
            MemoryStream stream = new MemoryStream();
            string file = "";
            if (System.IO.File.Exists(Server.MapPath("~/FileUploads/" + id)) == true)
            {
                fs = new FileStream(Server.MapPath("~/FileUploads/" + id),
                                              FileMode.Open,
                                              FileAccess.Read);
                br = new BinaryReader(fs);
                long numBytes = new FileInfo(Server.MapPath("~/FileUploads/" + id)).Length;
                buff = br.ReadBytes((int)numBytes);
                //var string_with_your_data = "Maraj";
                //var byteArray = Encoding.ASCII.GetBytes(Server.MapPath("~/FileDownloads/04252013_Finacle.txt"));
                stream = new MemoryStream(buff);

                //string[] FileName = id.Split('_');
                //file = FileName[0] + ".txt";
            }
            return File(stream, "text/plain", id);

        }

        [HttpPost]
        public ActionResult IWReturnUpload(HttpPostedFileBase file)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["fildwnd"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            // int dmnid = (int)Session["domainid"];
            DateTime date = Convert.ToDateTime(Session["processdate"]).Date;
            int Uid = (int)Session["uid"];
            // List<int> procid = new List<int>();
            //var procid1 = (from p in db.ProcessMaster where p.ProcessDate == date && p.Domain.ID == dmnid select p);
            //procid = db.ProcessMaster.Where(p => p.Domain.ID == dmnid && p.ProcessDate == date).Select(p1 => p1.ID).ToList();

            int totrecs = 0;
            int successrecs = 0;
            int failercs = 0;

            //Download dw = new Download();
            bool errFlg = false;

            try
            {
                if (ModelState.IsValid)
                {
                    if (file == null)
                    {
                        ModelState.AddModelError("File", "Please Upload Your file");
                    }
                    else if (file.ContentLength > 0)
                    {
                        int MaxContentLength = 1024 * 1024 * 3; //3 MB
                        string[] AllowedFileExtensions = new string[] { ".dat", ".txt" };
                        //string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".pdf", ".txt" };

                        if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                        {
                            ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
                        }

                        else if (file.ContentLength > MaxContentLength)
                        {
                            ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + MaxContentLength + " MB");
                        }
                        else
                        {
                            //TO:DO
                            var fileName = Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath("~/FileUploads"), fileName);
                            file.SaveAs(path);

                            using (var textFile = System.IO.File.OpenText(path))
                            {

                                string line = null;
                                string sChkSrNo, sPresenBank, sChkDate, sSeqNo, sReturnReason, sCycleNo, sPresenBankRoutno, ItemSeqNo, sDraweeBank,sTransCode, sUDK;
                                double sAmount;
                                Int64 FileID=0;
                                int FileSeqno=0;
                                //   var custID = af.Domains.Find(dmnid).Customer_ID.ToString();

                                //var iwFormatName = (from f in af.CommonSettings
                                //                    where f.AppName == "IWFileFormat" && f.SettingName == custID
                                //                    select f.SettingValue).FirstOrDefault().ToString();
                                string custID = Session["CustomerID"].ToString();
                                int intcustID = Convert.ToInt16(custID);
                                bool GlobalErrFlg;


                                var iwFormatName = (from f in af.ApplicationSettings
                                                    where f.SettingName == "CBSExtractFileFormat" && f.CustomerId == intcustID
                                                    select f.SettingValue).FirstOrDefault().ToString();

                                string dt = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                                IWMainTransaction rcdid = new IWMainTransaction();
                                while ((line = textFile.ReadLine()) != null)
                                {
                                    totrecs += 1;
                                    switch (iwFormatName)
                                    {
                                        case "AxisBank":
                                            try
                                            {
                                                sPresenBank = line.Substring(0, 9).Trim();
                                                sChkSrNo = line.Substring(9, 6).Trim();
                                                sChkDate = line.Substring(19, 4).Trim() + "-" + line.Substring(17, 2).Trim() + "-" + line.Substring(15, 2).Trim();
                                                sPresenBankRoutno = line.Substring(23, 9).Trim();
                                                sCycleNo = line.Substring(32, 2);
                                                sSeqNo = line.Substring(34, 44).Trim();
                                                //FileID = Convert.ToInt64(line.Substring(34, 11).Trim());
                                                //FileSeqno = Convert.ToInt32(line.Substring(45, 3).Trim());
                                                ItemSeqNo = line.Substring(34, 14).Trim();

                                                sReturnReason = line.Substring(96, 2);

                                                if (sReturnReason == "36" || sReturnReason == "40")
                                                    sReturnReason = "34";
                                                else if (sReturnReason == "38")
                                                    sReturnReason = "37";
                                                else if (sReturnReason.ToUpper() == "ZY")
                                                    sReturnReason = "84";

                                                if (af.ItemReturnReasons.Where(r => r.CBS_REASON_CODE == line.Substring(96, 2)).FirstOrDefault() != null)
                                                    sReturnReason = af.ItemReturnReasons.Where(r => r.CBS_REASON_CODE == line.Substring(96, 2)).FirstOrDefault().RETURN_REASON_CODE;
                                                else
                                                {

                                                    StreamWriter str = new StreamWriter(path.Replace(fileName.Substring(fileName.LastIndexOf('.')), ".err"), true, System.Text.Encoding.Default);
                                                    str.WriteLine("Return Reason Code " + line.Substring(96, 2) + " Not Available in Mapper At Line - " + line);
                                                    str.Close();
                                                    errFlg = true;
                                                }

                                                int? updateFlg = 0;
                                                iwpro.IWSearchRnt(FileID, FileSeqno, sReturnReason, sChkDate, sPresenBankRoutno, sCycleNo, ItemSeqNo, sChkSrNo, 0, ref updateFlg);
                                                if (updateFlg == 1)
                                                    successrecs += 1;
                                                else
                                                {

                                                    StreamWriter str = new StreamWriter(path.Replace(fileName.Substring(fileName.LastIndexOf('.')), ".err"), true, System.Text.Encoding.Default);
                                                    str.WriteLine("Could Not Found Item For Line - " + line);
                                                    str.Close();
                                                    errFlg = true;
                                                }
                                            }
                                            catch (Exception e)
                                            {

                                                StreamWriter str = new StreamWriter(path.Replace(fileName.Substring(fileName.LastIndexOf('.')), ".err"), true, System.Text.Encoding.Default);
                                                str.WriteLine(e.Message + " On Line -" + line);
                                                str.Close();
                                                errFlg = true;
                                            }
                                            if (errFlg)
                                            {
                                                failercs += 1;
                                                errFlg = false;
                                                GlobalErrFlg = true;
                                            }
                                            break;
                                            ///-----------------------------------
                                        case "KANARADCC":
                                            try
                                            {
                                                sPresenBank = line.Substring(0, 9).Trim();
                                                sDraweeBank = line.Substring(9, 9).Trim();
                                                sChkDate = line.Substring(22, 4).Trim() + "-" + line.Substring(20, 2).Trim() + "-" + line.Substring(18, 2).Trim();
                                                sAmount = Convert.ToDouble(line.Substring(26, 13))/100;
                                                sChkSrNo = line.Substring(39, 6).Trim();
                                                sSeqNo = line.Substring(45, 10).Trim();
                                                sTransCode = line.Substring(55, 2).Trim();
                                                sUDK = line.Substring(57, 36).Trim();

                                                sReturnReason = line.Substring(93, 2);

                                                if (sReturnReason == "36" || sReturnReason == "40")
                                                    sReturnReason = "34";
                                                else if (sReturnReason == "38")
                                                    sReturnReason = "37";
                                                else if (sReturnReason.ToUpper() == "ZY")
                                                    sReturnReason = "84";

                                                if (af.ItemReturnReasons.Where(r => r.CBS_REASON_CODE == line.Substring(93, 2)).FirstOrDefault() != null)
                                                    sReturnReason = af.ItemReturnReasons.Where(r => r.CBS_REASON_CODE == line.Substring(93, 2)).FirstOrDefault().RETURN_REASON_CODE;
                                                else
                                                {

                                                    StreamWriter str = new StreamWriter(path.Replace(fileName.Substring(fileName.LastIndexOf('.')), ".err"), true, System.Text.Encoding.Default);
                                                    str.WriteLine("Return Reason Code " + line.Substring(93, 2) + " Not Available in Mapper At Line - " + line);
                                                    str.Close();
                                                    errFlg = true;
                                                }

                                                int? updateFlg = 0;
                                                iwpro.IWSearchRnt(FileID, FileSeqno, sReturnReason, sChkDate, sPresenBank, sTransCode, sSeqNo, sChkSrNo, sAmount, ref updateFlg);
                                                if (updateFlg == 1)
                                                    successrecs += 1;
                                                else
                                                {

                                                    StreamWriter str = new StreamWriter(path.Replace(fileName.Substring(fileName.LastIndexOf('.')), ".err"), true, System.Text.Encoding.Default);
                                                    str.WriteLine("Could Not Found Item For Line - " + line);
                                                    str.Close();
                                                    errFlg = true;
                                                }
                                            }
                                            catch (Exception e)
                                            {

                                                StreamWriter str = new StreamWriter(path.Replace(fileName.Substring(fileName.LastIndexOf('.')), ".err"), true, System.Text.Encoding.Default);
                                                str.WriteLine(e.Message + " On Line -" + line);
                                                str.Close();
                                                errFlg = true;
                                            }
                                            if (errFlg)
                                            {
                                                failercs += 1;
                                                errFlg = false;
                                                GlobalErrFlg = true;
                                            }
                                            break;
                                        /////-----------------------------------
                                        case "SIB":
                                            try
                                            {
                                                sPresenBankRoutno = line.Substring(46, 9).Trim();
                                                sChkSrNo = line.Substring(55, 6).Trim();
                                                sAmount = Convert.ToDouble(line.Substring(21, 13)) / 100;
                                                sTransCode = line.Substring(61, 2).Trim();
                                                ItemSeqNo = line.Substring(63, 14).Trim();

                                                sReturnReason = line.Substring(42, 2);

                                                if (sReturnReason == "36" || sReturnReason == "40")
                                                    sReturnReason = "34";
                                                else if (sReturnReason == "38")
                                                    sReturnReason = "37";
                                                else if (sReturnReason.ToUpper() == "ZY")
                                                    sReturnReason = "84";

                                                if (af.ItemReturnReasons.Where(r => r.CBS_REASON_CODE == line.Substring(42, 2)).FirstOrDefault() != null)
                                                    sReturnReason = af.ItemReturnReasons.Where(r => r.CBS_REASON_CODE == line.Substring(42, 2)).FirstOrDefault().RETURN_REASON_CODE;
                                                else
                                                {
                                                    StreamWriter str = new StreamWriter(path.Replace(fileName.Substring(fileName.LastIndexOf('.')), ".err"), true, System.Text.Encoding.Default);
                                                    str.WriteLine("Return Reason Code " + line.Substring(42, 2) + " Not Available in Mapper At Line - " + line);
                                                    str.Close();
                                                    errFlg = true;
                                                }

                                                int? updateFlg = 0;
                                                iwpro.IWSearchRnt(FileID, FileSeqno, sReturnReason, dt, sPresenBankRoutno, sTransCode, ItemSeqNo, sChkSrNo, sAmount, ref updateFlg);
                                                if (updateFlg == 1)
                                                    successrecs += 1;
                                                else
                                                {

                                                    StreamWriter str = new StreamWriter(path.Replace(fileName.Substring(fileName.LastIndexOf('.')), ".err"), true, System.Text.Encoding.Default);
                                                    str.WriteLine("Could Not Found Item For Line - " + line);
                                                    str.Close();
                                                    errFlg = true;
                                                }
                                            }
                                            catch(Exception e)
                                            {

                                            }
                                            if (errFlg)
                                            {
                                                failercs += 1;
                                                errFlg = false;
                                                GlobalErrFlg = true;
                                            }
                                            break;

                                        default:
                                            break;
                                    }
                                }
                                iwpro.Dispose();
                            }

                            ModelState.Clear();
                            if (errFlg)
                            {
                                ViewBag.Message = "File uploaded successfully With Errors. Please download the error file from the link";
                                ViewBag.Counts = "Total Records = " + totrecs + " | Uploaded Successfully = " + successrecs + " | Failed To Upload = " + failercs;
                                ViewBag.dwnFname = fileName.Replace(fileName.Substring(fileName.LastIndexOf('.')), ".err");

                            }
                            else
                                ViewBag.Message = "File uploaded successfully With No Error";
                        }
                    }
                }

                return View("IWReturnUpload");
            }
            catch (Exception e)
            {

                //ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                //er.ErrorMessage = e.InnerException.Message;
                //return View("Error", er);
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }
    }
}
