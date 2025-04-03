using ikloud_Aflatoon.Infrastructure;
using ikloud_Aflatoon.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ikloud_Aflatoon.Controllers
{
    public class IWFileDownloadController : Controller
    {
        //
        // GET: /IWFileDownload/
        UserAflatoonDbContext af = new UserAflatoonDbContext();
        AflatoonEntities cl = new AflatoonEntities();
        public ActionResult InwardFileDownload()
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

            ViewBag.ClearingType = new SelectList(cl.ClearingType, "Code", "Name").ToList();
            Session["glob"] = null;
            return View();
        }
        [HttpPost]
        public ActionResult IWDownload(Download dw)
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

            //Download dw = new Download();
            string ServerPath = "";
            string fileNameWithPath = "";
            string webPath = "";
            string filename = "";
            int Ccnt;
            int domainid;
            int Uid;
            DateTime date;
            try
            {
                //domainid = (int)Session["domainid"];
                Uid = (int)Session["uid"];
                date = Convert.ToDateTime(Session["processdate"]).Date;
            }
            catch (Exception)
            {
                ErrorDisplay er = new ErrorDisplay();
                er.ErrorMessage = "Session Expired!!";
                return View("Error", er);
            }
            try
            {
                string BtnView = Request.Form.Get("btnView");
                string optn = Request.Form.Get("FileType");
                string Sesntype = Request.Form.Get("ClearingType");
                //string fname = Request.Form.Get("Fname");
                string custID = Session["CustomerID"].ToString();
                int intcustID = Convert.ToInt16(custID);
                //string CustomerIDTemp = Session["CustomerIDTemp"].ToString();



                var iwFormatName = (from f in cl.ApplicationSettings
                                    where f.SettingName == "CBSExtractFileFormat" && f.CustomerId == intcustID
                                    select f.SettingValue).FirstOrDefault().ToString();

                var Customers = cl.CustomerMasters.Find(intcustID);

                if (BtnView == "View")
                {

                    if (iwFormatName == "AxisBank")
                    {
                        dw = getCounts(optn, Sesntype, date, intcustID);
                        dw.status = 0;
                    }

                }
                else if (BtnView == "Download")
                {
                    dw = getCounts(optn, Sesntype, date, intcustID);
                    if (optn == "Disbusrsement DD")
                    {
                        IWProcDataContext iwpro = new IWProcDataContext();
                        // SPClassesDataContext iwpro = new SPClassesDataContext();
                        var Axisquery = (from c in cl.IWFinalMainTransactions
                                         where c.ProcessingDate == date && c.MiscStatus == 1 && c.BatchNo == 4
                                            && c.ClearingType == Sesntype && c.CustomerId == intcustID
                                         select new
                                         {
                                             FileID = c.File_ID,
                                             c.FileSeqNo,
                                             c.ID,
                                             c.XMLSerialNo,
                                             c.XMLPayorBankRoutNo,
                                             c.XMLSAN,
                                             c.XMLTransCode,
                                             c.XMLAmount,
                                             c.ChqDate,
                                             c.ItemSeqNo,
                                             c.PresentingBankRoutNo,
                                             c.EntryPayeeName,
                                             c.MiscStatus,
                                             c.PresentmentDate,
                                             c.DbtAccNo,
                                             c.ReturnCode,
                                             c.CycleNo,
                                             c.ProcessingDate
                                         }).ToList();

                        Ccnt = Axisquery.Count();
                        if (Axisquery != null && Ccnt != 0)
                        {

                            ServerPath = Server.MapPath("~/FileDownloads/");
                            if (System.IO.Directory.Exists(ServerPath) == false)
                            {
                                System.IO.Directory.CreateDirectory(ServerPath);
                            }

                            filename = DateTime.Now.ToString("MMddyyyy") + DateTime.Now.ToString("hh:mm:ss").Replace(":", "") + "" + optn.Replace(" ", "") + ".txt";
                            fileNameWithPath = ServerPath + filename;
                            dw.filename = filename;
                            StreamWriter str = new StreamWriter(fileNameWithPath, false, System.Text.Encoding.Default);
                            foreach (var item in Axisquery)
                            {
                                string particulars = "".PadRight(30, ' ');
                                //string acc = item.CrdAccNo.PadLeft(15, '0');
                                string chqno = item.XMLSerialNo.PadLeft(6, '0');
                                string SortCode = item.XMLPayorBankRoutNo.PadLeft(9, '0');

                                string chqAccNo = (item.XMLSAN == null ? "0000000000" : item.XMLSAN.PadLeft(10, '0'));                   //.PadLeft(6, '0');
                                string TrnCode = item.XMLTransCode.PadLeft(2, '0');
                                string amt = (item.XMLAmount * 100).ToString("f0").PadLeft(13, '0'); //item.Amount.ToString().PadLeft(13, ' ');
                                string presentybnkcode = item.PresentingBankRoutNo.PadLeft(9, '0');
                                //string chqDate = item.ChqDate.PadLeft(8, '0');
                                string clrDate = item.ProcessingDate.ToString("ddMMyyyy");
                                string itemseqno = item.ID.ToString().PadLeft(10, '0');
                                string accno = item.DbtAccNo.ToString().PadRight(16, ' ');
                                if (item.EntryPayeeName != null)
                                    particulars = (item.EntryPayeeName.Trim().Length > 30 ? item.EntryPayeeName.Substring(1, 30) : (item.EntryPayeeName != null ? item.EntryPayeeName.PadRight(30, ' ') : " ".PadRight(30, ' ')));
                                // string instrimgtid = (clrDate + presentybnkcode + item.CycleNo.PadLeft(2, '0') + item.ItemSeqNo).PadRight(80, ' ');
                                //string instrimgtid = (clrDate + presentybnkcode + item.CycleNo.PadLeft(2, '0') + item.FileID.ToString().PadLeft(11, '0') + item.FileSeqNo.ToString().PadLeft(3, '0') + ' '.ToString().PadRight(66, ' '));// item.ItemSeqNo).PadRight(80, ' ');
                                string instrimgtid = (clrDate + presentybnkcode + item.CycleNo.PadLeft(2, '0') + item.FileID.ToString().PadLeft(11, '0') + item.FileSeqNo.ToString().PadLeft(3, '0')).PadRight(80, ' ');
                                string flg = "Y";
                                string rtnflg = "Y";
                                if (item.ReturnCode != "0" && item.ReturnCode != null)
                                {
                                    rtnflg = "I" + cl.ItemReturnReasons.Find(item.ReturnCode).CBS_REASON_CODE;

                                }
                                else
                                {
                                    rtnflg = "N  ";
                                }
                                string pad = "        ";

                                if (presentybnkcode.Substring(0, 3) != Customers.PresentingBankRouteNo.Substring(0, 3))
                                {
                                    presentybnkcode = Customers.PresentingBankRouteNo.Substring(0, 3) + presentybnkcode.Substring(3, 3) + "001";
                                }

                                str.WriteLine(presentybnkcode + SortCode + clrDate + amt + chqno + itemseqno + TrnCode + chqAccNo + accno);

                                iwpro.UpdateDwnloadFile("IW", "CBS", item.ID);

                            }
                            dw.status = 1;
                            str.Close();
                            dw.filename = filename;
                            dw.dwn = true;
                            //Thread.Sleep(200);
                            //ModelState.AddModelError("", "Sucessfully Downloaded");
                            //if (filename != "")
                            //{
                            //    //FileDwnHistory objfile = new FileDwnHistory();
                            //    //objfile.DownloadDate = DateTime.Now;
                            //    //objfile.ProcessDate = date;
                            //    //objfile.ProcessID = processid;
                            //    //objfile.FileName = filename;
                            //    //objfile.FileType = optn;
                            //    //objfile.DownloadedBy = Uid;
                            //    //dbIW.FileDwnHistory.Add(objfile);
                            //    //dbIW.SaveChanges();
                            //}





                        }
                        iwpro.Dispose();
                    }
                    //----------------------------------next Files----------------
                    else if (optn == "CMS")
                    {
                        IWProcDataContext iwpro = new IWProcDataContext();
                        var Axisquery = (from c in cl.IWFinalMainTransactions
                                         where c.ProcessingDate == date
                                           && c.MiscStatus == 1 && c.BatchNo == 3
                                           && c.ClearingType == Sesntype && c.CustomerId == intcustID
                                         select new
                                         {
                                             c.ID,
                                             FileID = c.File_ID,
                                             c.FileSeqNo,
                                             c.XMLSerialNo,
                                             c.XMLPayorBankRoutNo,
                                             c.XMLSAN,
                                             c.XMLTransCode,
                                             c.XMLAmount,
                                             c.ChqDate,
                                             c.ItemSeqNo,
                                             c.PresentingBankRoutNo,
                                             c.EntryPayeeName,
                                             c.MiscStatus,
                                             c.PresentmentDate,
                                             c.DbtAccNo,
                                             c.ReturnCode,
                                             c.CycleNo,
                                             c.ProcessingDate
                                         }).ToList();

                        Ccnt = Axisquery.Count();
                        if (Axisquery != null && Ccnt != 0)
                        {

                            ServerPath = Server.MapPath("~/FileDownloads/"); ;

                            if (System.IO.Directory.Exists(ServerPath) == false)
                            {
                                System.IO.Directory.CreateDirectory(ServerPath);
                            }

                            //string fileNameWithPath = Server.MapPath("~/Download/" + DateTime.Now.ToString("MMddyyyy") + DateTime.Now.ToString("hh:mm:ss").Replace(":", "") + "_" + optn + ".txt");
                            //string fileNameWithPath = ServerPath + "\\" + DateTime.Now.ToString("MMddyyyy") + DateTime.Now.ToString("hh:mm:ss").Replace(":", "") + "_" + optn + ".txt";
                            filename = DateTime.Now.ToString("MMddyyyy") + DateTime.Now.ToString("hh:mm:ss").Replace(":", "") + "" + optn.Replace(" ", "") + ".txt";
                            fileNameWithPath = ServerPath + filename;
                            dw.filename = filename;
                            StreamWriter str = new StreamWriter(fileNameWithPath, false, System.Text.Encoding.Default);
                            foreach (var item in Axisquery)
                            {
                                string particulars = "".PadRight(30, ' ');
                                //string acc = item.CrdAccNo.PadLeft(15, '0');
                                string chqno = item.XMLSerialNo.PadLeft(6, '0');
                                string SortCode = item.XMLPayorBankRoutNo.PadLeft(9, '0');

                                string chqAccNo = (item.XMLSAN == null ? "0000000000" : item.XMLSAN.PadLeft(10, '0'));                   //.PadLeft(6, '0');
                                string TrnCode = item.XMLTransCode.PadLeft(2, '0');
                                string amt = (item.XMLAmount * 100).ToString("f0").PadLeft(13, '0'); //item.Amount.ToString().PadLeft(13, ' ');
                                string presentybnkcode = item.PresentingBankRoutNo.PadLeft(9, '0');
                                //string chqDate = item.ChqDate.PadLeft(8, '0');
                                string clrDate = item.ProcessingDate.ToString("ddMMyyyy");
                                string itemseqno = item.ID.ToString().PadLeft(10, '0');
                                string accno = item.DbtAccNo.ToString().PadRight(16, ' ');
                                if (item.EntryPayeeName != null)
                                    particulars = (item.EntryPayeeName.Trim().Length > 30 ? item.EntryPayeeName.Substring(1, 30) : (item.EntryPayeeName != null ? item.EntryPayeeName.PadRight(30, ' ') : " ".PadRight(30, ' ')));
                                //string instrimgtid = (clrDate + presentybnkcode + item.CycleNo.PadLeft(2, '0') + item.ItemSeqNo).PadRight(80, ' ');
                                //By Prasad on 05-03-14
                                //string instrimgtid = (clrDate + presentybnkcode + item.CycleNo.PadLeft(2, '0') + item.FileID.ToString().PadLeft(11, '0') + item.FileSeqNo.ToString().PadLeft(3, '0') + ' '.ToString().PadRight(66, ' '));// item.ItemSeqNo).PadRight(80, ' ');
                                string instrimgtid = (clrDate + presentybnkcode + item.CycleNo.PadLeft(2, '0') + item.FileID.ToString().PadLeft(11, '0') + item.FileSeqNo.ToString().PadLeft(3, '0')).PadRight(80, ' ');
                                string flg = "Y";
                                string rtnflg = "Y";
                                if (item.ReturnCode != "0" && item.ReturnCode != null)
                                {
                                    rtnflg = "I" + cl.ItemReturnReasons.Find(item.ReturnCode).CBS_REASON_CODE;

                                }
                                else
                                {
                                    rtnflg = "N  ";
                                }
                                string pad = "        ";

                                if (presentybnkcode.Substring(0, 3) != Customers.PresentingBankRouteNo.Substring(0, 3))
                                {
                                    presentybnkcode = Customers.PresentingBankRouteNo.Substring(0, 3) + presentybnkcode.Substring(3, 3) + "001";
                                }

                                str.WriteLine(presentybnkcode + SortCode + clrDate + amt + chqno + itemseqno + TrnCode + chqAccNo + accno + particulars + instrimgtid + rtnflg + pad + flg);

                                iwpro.UpdateDwnloadFile("IW", "CBS", item.ID);

                            }
                            dw.status = 1;
                            str.Close();
                            dw.filename = filename;
                            dw.dwn = true;
                            //Thread.Sleep(200);
                            //ModelState.AddModelError("", "Sucessfully Downloaded");
                            //if (filename != "")
                            //{
                            //    FileDwnHistory objfile = new FileDwnHistory();
                            //    objfile.DownloadDate = DateTime.Now;
                            //    objfile.ProcessDate = date;
                            //    objfile.ProcessID = processid;
                            //    objfile.FileName = filename;
                            //    objfile.FileType = optn;
                            //    objfile.DownloadedBy = Uid;
                            //    dbIW.FileDwnHistory.Add(objfile);
                            //    dbIW.SaveChanges();
                            //}





                        }
                        iwpro.Dispose();
                    }
                    //--------------------------SMB---------08-01-2014-----------------//for smb bank ---------804--------
                    else if (optn == "SMB-804")
                    {
                        IWProcDataContext spdb = new IWProcDataContext();
                        var Axisquery = (from c in cl.IWFinalMainTransactions
                                         where c.ProcessingDate == date
                                           && c.MiscStatus == 1 && c.BatchNo == 5
                                           && c.ClearingType == Sesntype && c.CustomerId == intcustID
                                           && c.XMLPayorBankRoutNo.Substring(3, 3) == "804"

                                         select new
                                         {
                                             c.ID,
                                             FileID = c.File_ID,
                                             c.FileSeqNo,
                                             c.XMLSerialNo,
                                             c.XMLPayorBankRoutNo,
                                             c.XMLSAN,
                                             c.XMLTransCode,
                                             c.XMLAmount,
                                             c.ChqDate,
                                             c.ItemSeqNo,
                                             c.PresentingBankRoutNo,
                                             c.EntryPayeeName,
                                             c.MiscStatus,
                                             c.PresentmentDate,
                                             c.DbtAccNo,
                                             c.ReturnCode,
                                             c.CycleNo,
                                             c.ProcessingDate
                                         }).ToList();

                        Ccnt = Axisquery.Count();
                        if (Axisquery != null && Ccnt != 0)
                        {
                            ServerPath = Server.MapPath("~/FileDownloads/"); ;
                            if (System.IO.Directory.Exists(ServerPath) == false)
                            {
                                System.IO.Directory.CreateDirectory(ServerPath);
                            }

                            //string fileNameWithPath = Server.MapPath("~/Download/" + DateTime.Now.ToString("MMddyyyy") + DateTime.Now.ToString("hh:mm:ss").Replace(":", "") + "_" + optn + ".txt");
                            //string fileNameWithPath = ServerPath + "\\" + DateTime.Now.ToString("MMddyyyy") + DateTime.Now.ToString("hh:mm:ss").Replace(":", "") + "_" + optn + ".txt";
                            filename = DateTime.Now.ToString("MMddyyyy") + DateTime.Now.ToString("hh:mm:ss").Replace(":", "") + "" + optn.Replace(" ", "") + ".txt";
                            fileNameWithPath = ServerPath + filename;
                            dw.filename = filename;
                            StreamWriter str = new StreamWriter(fileNameWithPath, false, System.Text.Encoding.Default);
                            //----------------------------------- Putting Header---------------------
                            str.WriteLine(" ANAND CO OP MARCHANTILE BANK - MUMBAI" + "                  " + date.ToString("dd/MM/yyyy"));
                            double totalAmt = 0;
                            Int32 totalrecrd = 0;
                            foreach (var item in Axisquery)
                            {
                                totalrecrd = totalrecrd + 1;
                                totalAmt = totalAmt + Convert.ToDouble(item.XMLAmount);
                                string particulars = "".PadRight(30, ' ');
                                //string acc = item.CrdAccNo.PadLeft(15, '0');
                                string chqno = item.XMLSerialNo.PadLeft(6, '0');
                                string SortCode = item.XMLPayorBankRoutNo.PadLeft(9, '0');

                                string chqAccNo = (item.XMLSAN == null ? "0000000000" : item.XMLSAN.PadLeft(10, '0'));                   //.PadLeft(6, '0');
                                string TrnCode = item.XMLTransCode.PadLeft(2, '0');
                                string amt = (item.XMLAmount * 100).ToString("f0").PadLeft(13, '0'); //item.Amount.ToString().PadLeft(13, ' ');
                                string presentybnkcode = item.PresentingBankRoutNo.PadLeft(9, '0');
                                //string chqDate = item.ChqDate.PadLeft(8, '0');
                                string clrDate = item.ProcessingDate.ToString("ddMMyyyy");
                                string itemseqno = item.ID.ToString().PadLeft(10, '0');
                                string accno = item.DbtAccNo.ToString().PadRight(16, ' ');
                                if (item.EntryPayeeName != null)
                                    particulars = (item.EntryPayeeName.Trim().Length > 50 ? item.EntryPayeeName.Substring(1, 50) : (item.EntryPayeeName != null ? item.EntryPayeeName.PadRight(50, ' ') : " ".PadRight(50, ' ')));

                                string instrimgtid = (clrDate + presentybnkcode + item.CycleNo.PadLeft(2, '0') + item.FileID.ToString().PadLeft(11, '0') + item.FileSeqNo.ToString().PadLeft(3, '0'));
                                string flg = "Y";
                                string rtnflg = "Y";
                                if (item.ReturnCode != "0" && item.ReturnCode != null)
                                {
                                    rtnflg = "I" + cl.ItemReturnReasons.Find(item.ReturnCode).CBS_REASON_CODE;

                                }
                                else
                                {
                                    rtnflg = "N  ";
                                }
                                string pad = "        ";

                                if (presentybnkcode.Substring(0, 3) != Customers.PresentingBankRouteNo.Substring(0, 3))
                                {
                                    presentybnkcode = Customers.PresentingBankRouteNo.Substring(0, 3) + presentybnkcode.Substring(3, 3) + "001";
                                }

                                str.WriteLine(presentybnkcode + SortCode + clrDate + amt + chqno + itemseqno + TrnCode + chqAccNo + particulars + clrDate + instrimgtid + "F" + instrimgtid + "R" + instrimgtid + "G" + instrimgtid.PadRight(80, ' '));

                                spdb.UpdateDwnloadFile("IW", "CBS", item.ID);

                            }
                            //----------------------------------- Putting Header---------------------
                            str.WriteLine("TOTAL NO OF ITEMS:     " + totalrecrd + "                " + String.Format(new CultureInfo("en-IN"), "{0:C}", totalAmt).Substring(1));
                            str.Close();
                            dw.status = 1;
                            dw.filename = filename;
                            dw.dwn = true;
                            //Thread.Sleep(200);
                            //ModelState.AddModelError("", "Sucessfully Downloaded");
                            //if (filename != "")
                            //{
                            //    FileDwnHistory objfile = new FileDwnHistory();
                            //    objfile.DownloadDate = DateTime.Now;
                            //    objfile.ProcessDate = date;
                            //    objfile.ProcessID = processid;
                            //    objfile.FileName = filename;
                            //    objfile.FileType = optn;
                            //    objfile.DownloadedBy = Uid;
                            //    dbIW.FileDwnHistory.Add(objfile);
                            //    dbIW.SaveChanges();
                            //}
                        }
                        spdb.Dispose();
                    }


                }
                return View("_IWDownload", dw);
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
        //--------------------Gets All types of file count---------------

        public Download getCounts(string fileType, string Sesntype, DateTime date, int intcustID)
        {
            Download dw = new Download();
            if (fileType == "Disbusrsement DD")
            {
                //------------------total record-------------------------Batch No 4--------
                var totalrecord = (from m in cl.IWFinalMainTransactions
                                   where m.ProcessingDate == date && m.BatchNo == 4 &&
                                   m.ClearingType == Sesntype && m.CustomerId == intcustID
                                   select m.ID);

                if (totalrecord != null)
                    dw.TotalRecord = totalrecord.Count();

                //-------------------------------------------pending For VF-----------------we have to replace m.EntryAmount with L2Status---
                var totalvfpen = (from m in cl.IWFinalMainTransactions
                                  where m.ProcessingDate == date && m.BatchNo == 4 &&
                                  m.ClearingType == Sesntype && m.MiscStatus == 0 && m.CustomerId == intcustID
                                  select m.ID);

                if (totalvfpen != null)
                    dw.PendingVF = totalvfpen.Count();
                //-------------------------------------------Redy To download ----------we have to replace m.EntryAmount with L2Status---
                var ReadyToDwnld = (from m in cl.IWFinalMainTransactions
                                    where m.ProcessingDate == date && m.BatchNo == 4 &&
                                    m.ClearingType == Sesntype && m.MiscStatus == 1 && m.CustomerId == intcustID
                                    select m.ID);

                if (ReadyToDwnld != null)
                    dw.readyTodwnld = ReadyToDwnld.Count();

                //-------------------------------------------Already downloaded ----------we have to replace m.EntryAmount with L2Status---
                var AlreadyDwnld = (from m in cl.IWFinalMainTransactions
                                    where m.ProcessingDate == date && m.BatchNo == 4 &&
                                    m.ClearingType == Sesntype && m.MiscStatus == 2 && m.CustomerId == intcustID
                                    select m.ID);

                if (AlreadyDwnld != null)
                    dw.AlreadyDwnld = AlreadyDwnld.Count();

            }
            else if (fileType == "SMB-804")//----------------- For SMB BnakCode=804---------------//
            {
                var totalrecord = (from c in cl.IWFinalMainTransactions
                                   where c.ProcessingDate == date && c.BatchNo == 5 && c.ClearingType == Sesntype && c.CustomerId == intcustID &&
                                   c.XMLPayorBankRoutNo.Substring(3, 3) == "804"
                                   select c.ID);

                if (totalrecord != null)
                    dw.TotalRecord = totalrecord.Count();
                //-------------------totalvfpen Pending-------
                dw.PendingVF = 0;

                var ReadyToDwnld = (
                                   from c in cl.IWFinalMainTransactions
                                   where c.ProcessingDate == date && c.BatchNo == 5
                                     && c.ClearingType == Sesntype && c.MiscStatus == 1 && c.CustomerId == intcustID && c.XMLPayorBankRoutNo.Substring(3, 3) == "804"
                                   select c.ID);


                if (ReadyToDwnld != null)
                    dw.readyTodwnld = ReadyToDwnld.Count();


                var AlreadyDwnld = (from c in cl.IWFinalMainTransactions
                                    where c.ProcessingDate == date && c.BatchNo == 5 &&
                                   c.ClearingType == Sesntype && c.MiscStatus == 2 && c.CustomerId == intcustID
                                   && c.XMLPayorBankRoutNo.Substring(3, 3) == "804"
                                    select c.ID);

                if (AlreadyDwnld != null)
                    dw.AlreadyDwnld = AlreadyDwnld.Count();

            }
            else if (fileType == "CMS")
            {

                var totalrecord = (from c in cl.IWFinalMainTransactions
                                   where c.ProcessingDate == date && c.BatchNo == 3 && c.ClearingType == Sesntype && c.CustomerId == intcustID
                                   select c.ID);

                if (totalrecord != null)
                    dw.TotalRecord = totalrecord.Count();
                //-------------------totalvfpen Pending-------
                dw.PendingVF = 0;

                var ReadyToDwnld = (
                                    from c in cl.IWFinalMainTransactions
                                    where c.ProcessingDate == date && c.BatchNo == 3
                                      && c.ClearingType == Sesntype && c.MiscStatus == 1 && c.CustomerId == intcustID
                                    select c.ID);


                if (ReadyToDwnld != null)
                    dw.readyTodwnld = ReadyToDwnld.Count();




                var AlreadyDwnld = (from c in cl.IWFinalMainTransactions
                                    where c.ProcessingDate == date && c.BatchNo == 3 &&
                                   c.ClearingType == Sesntype && c.MiscStatus == 2 && c.CustomerId == intcustID
                                    select c.ID);

                if (AlreadyDwnld != null)
                    dw.AlreadyDwnld = AlreadyDwnld.Count();
            }

            return dw;
        }
        public FileStreamResult IWCreateFile(string id = null)
        {

            byte[] buff = null;
            FileStream fs;
            BinaryReader br;
            MemoryStream stream = new MemoryStream();
            string file = "";
            if (System.IO.File.Exists(Server.MapPath("~/FileDownloads/" + id)) == true)
            {
                fs = new FileStream(Server.MapPath("~/FileDownloads/" + id),
                                              FileMode.Open,
                                              FileAccess.Read);
                br = new BinaryReader(fs);
                long numBytes = new FileInfo(Server.MapPath("~/FileDownloads/" + id)).Length;
                buff = br.ReadBytes((int)numBytes);
                //var string_with_your_data = "Maraj";
                //var byteArray = Encoding.ASCII.GetBytes(Server.MapPath("~/FileDownloads/04252013_Finacle.txt"));
                stream = new MemoryStream(buff);

                //string[] FileName = id.Split('_');
                //file = FileName[0] + ".txt";
            }
            return File(stream, "text/plain", id);

        }

    }
}
