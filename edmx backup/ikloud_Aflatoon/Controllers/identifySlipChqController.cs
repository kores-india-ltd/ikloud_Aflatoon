using ikloud_Aflatoon;
using ikloud_Aflatoon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ikloudprowebapp.Controllers
{
    public class identifySlipChqController : Controller
    {
        //
        // GET: /identifySlipChq/
        private AflatoonEntities db = new AflatoonEntities();

        public ActionResult Index()
        {
            //Session["processdate"] = DateTime.ParseExact("2017-02-24", "yyyy-MM-dd", null);
            //Session["CustomerID"] = 6;
            //Session["UserID"] = 1;

            return View();
        }


        [HttpPost]
        [ActionName("Index")]
        public ActionResult DisplayDoc(string instrument, string  command)
        {

            //required processdate and userid from application

            DateTime procdate = Convert.ToDateTime(Session["processdate"].ToString());
            string strUsrid = Convert.ToString(Session["UserID"]);
            
            BatchMaster btch = new BatchMaster();

            int intUserId = 0;
            string doctype = "";

            int domainid = 0;
            int scannodeid = 0;
            string brnchno = "";
            int btchId =0;
            int btchno =0;
            string intstrtype = "";
            string currntinstrtype = "";
            int slipNo=0;

            if (command == "Close")
            {
                return RedirectToAction("Index");
            }
            else if (command == "Save")
            {
                if (!ModelState.IsValid)
                    return View();

                long lngCartureRawId = 0;
                lngCartureRawId = Convert.ToInt64(Request["captureRawId"]);

                domainid = Convert.ToInt32(Request["domainid"]);
                scannodeid = Convert.ToInt32(Request["scannodeid"]);
                brnchno = Request["brnchno"];
                btchId = Convert.ToInt32(Request["btchid"]);
                btchno = Convert.ToInt32(Request["btchno"]);
                intstrtype = Request["doctype"];
                currntinstrtype = Request["InstrumentType"];
                slipNo = Convert.ToInt32(Request["slipno"]);

                if (currntinstrtype != intstrtype)//user has updated the instrument type
                {
                    //var slipRcrds = (from dta in db.CaptureRawDatas
                    //           where dta.ProcessingDate == procdate &&
                    //               dta.DomainId == domainid &&
                    //               dta.ScanningNodeId == scannodeid &&
                    //               dta.BranchCode == brnchno &&
                    //               dta.BatchNo == btchno &&
                    //               dta.SlipNo == slipNo
                    //           orderby dta.Id ascending
                    //           select new instrumentView()
                    //           {
                                   
                    //           }).FirstOrDefault();


                    if (intstrtype == "S")
                    {
                        var prevSlipno=db.CaptureRawData.Where(m=>(m.ProcessingDate==procdate) && 
                                                                   (m.DomainId==domainid) && 
                                                                   (m.ScanningNodeId==scannodeid) && 
                                                                   (m.BranchCode==brnchno) && 
                                                                   (m.BatchNo==btchno) && 
                                                                   (m.SlipNo<slipNo)).Max(m=>m.SlipNo).Value;

                        if ((prevSlipno == null) ||(slipNo==prevSlipno))
                            return RedirectToAction("Index");

                        var updatedta = db.CaptureRawData.Where(m => (m.ProcessingDate == procdate) &&
                                                                   (m.DomainId == domainid) &&
                                                                   (m.ScanningNodeId == scannodeid) &&
                                                                   (m.BranchCode == brnchno) &&
                                                                   (m.BatchNo == btchno) &&
                                                                   (m.SlipNo == slipNo));
                        foreach (var item in updatedta)
                        {
                            item.SlipNo = prevSlipno;
                            if (item.InstrumentType == "S")
                            { 
                                item.InstrumentType = "C";
                                item.DocStatus = "2";
                            }
                        }

                        db.SaveChanges();

                    }
                    else if (intstrtype == "C")
                    {
                        //get current slipno
                        //get current capturerawdata id
                        //get max slip no+1
                        //get all cheques with current slip no
                        //mark instrument type as 'S' for current capturerawdata id
                        //update all slipnumbers with new slipno

                        var newSlipno = db.CaptureRawData.Where(m => (m.ProcessingDate == procdate) &&
                                                                   (m.DomainId == domainid) &&
                                                                   (m.ScanningNodeId == scannodeid) &&
                                                                   (m.BranchCode == brnchno) &&
                                                                   (m.BatchNo == btchno)).Max(m => m.SlipNo).Value;
                        
                        int intNewSlipNo = Convert.ToInt32(newSlipno)+1;
                        if ((intNewSlipNo == null) || (slipNo == intNewSlipNo))
                            return RedirectToAction("Index");

                        var updatedta = db.CaptureRawData.Where(m => (m.ProcessingDate == procdate) &&
                                                                  (m.DomainId == domainid) &&
                                                                  (m.ScanningNodeId == scannodeid) &&
                                                                  (m.BranchCode == brnchno) &&
                                                                  (m.BatchNo == btchno) &&
                                                                  (m.SlipNo == slipNo) && 
                                                                  (m.Id>=lngCartureRawId) && 
                                                                  (m.InstrumentType=="C"));
                        foreach (var item in updatedta)
                        {
                            item.SlipNo = intNewSlipNo;
                            if (item.InstrumentType == "C")
                            {
                                item.InstrumentType = "S";
                                item.DocStatus = "2";
                            }
                        }

                        db.SaveChanges();



                    }
                }
                else
                {
                    //update capturedata
                    CaptureRawData crd = new CaptureRawData();
                    crd = db.CaptureRawData.Find(lngCartureRawId);
                    crd.DocStatus = "1";
                    db.SaveChanges();
                }

            }
            else if (command == null)
            {
                
                //load batch for first time 
                Session["UserID"] = 1;
                strUsrid = Convert.ToString(Session["UserID"]);

                string statusLockBtch = "",statusRegularBtch = "";

                if (instrument == "0")//load slip
                {
                    doctype = "S";
                    statusLockBtch = "1";
                    statusRegularBtch = "0";
                }
                else//load cheque
                {
                    doctype = "C";
                    statusLockBtch = "3";
                    statusRegularBtch = "2";
                }

                byte sts = Convert.ToByte(statusLockBtch);

                //first check for user having locked batch
                //if there load it
                btch = db.BatchMaster.Where(m => (m.ProcessDate == procdate) &&
                    (m.Status == sts) &&
                    (m.LockUserId == strUsrid)).FirstOrDefault();

                if (btch != null)
                {
                }
                else if (btch == null)
                {
                    sts = Convert.ToByte(statusRegularBtch);
                    //else get the first row i batchmaster with status is 0
                    btch = db.BatchMaster.Where(m => (m.ProcessDate == procdate) &&
                    (m.Status ==sts) &&
                    (m.LockUserId == null)).FirstOrDefault();

                    if (btch == null)
                        return RedirectToAction("Index");
                }
                
                

                //get btch data

                ViewData["domainid"] = btch.DomainId;
                ViewData["scannodeid"] = btch.ScanningNodeId;
                ViewData["brnchno"] = btch.BranchCode;
                ViewData["btchid"] = btch.Id;
                ViewData["btchno"] = btch.BatchNo;
                ViewData["doctype"] = doctype;

                domainid = btch.DomainId;
                scannodeid = btch.ScanningNodeId;
                brnchno = btch.BranchCode;
                btchId = btch.Id;
                btchno = btch.BatchNo;
                intstrtype = doctype;

                byte sts1 = Convert.ToByte(statusLockBtch);
                btch.Status = sts1;//locked the batch
                btch.LockUserId = Convert.ToString(Session["UserID"]);
                db.SaveChanges();

            }


            var result1 =(instrumentView) null;

            if (intstrtype == "S")
            {
                result1 = (from dta in db.CaptureRawData
                           where dta.ProcessingDate == procdate &&
                               dta.DomainId == domainid &&
                               dta.ScanningNodeId == scannodeid &&
                               dta.BranchCode == brnchno &&
                               dta.BatchNo == btchno &&
                               dta.InstrumentType == intstrtype &&
                               dta.DocStatus == null
                           orderby dta.Id ascending
                           select new instrumentView()
                           {
                               domainId = dta.DomainId,
                               scanningNodeId = dta.ScanningNodeId,
                               branchNo = dta.BranchCode,
                               batchId = btchId,
                               batchNo = dta.BatchNo,
                               captureRawId = dta.Id,
                               FrontGreyImage = dta.FrontGreyImage,
                               FrontTiffImage = dta.FrontTiffImage,
                               BackGreyImage=dta.BackGreyImage,
                               BackTiffImage = dta.BackTiffImage,
                               InstrumentType = dta.InstrumentType,
                               batchSeqNo = dta.BatchSeqNo,
                               slipSeqNo = dta.SlipNo
                           }).FirstOrDefault();
            }
            else
            {
                result1 = (from dta in db.CaptureRawData
                           where dta.ProcessingDate == procdate &&
                               dta.DomainId == domainid &&
                               dta.ScanningNodeId == scannodeid &&
                               dta.BranchCode == brnchno &&
                               dta.BatchNo == btchno &&
                               dta.InstrumentType == intstrtype &&
                               dta.DocStatus == null && 
                               dta.MICRRepairRequired==1
                           orderby dta.Id ascending
                           select new instrumentView()
                           {
                               domainId = dta.DomainId,
                               scanningNodeId = dta.ScanningNodeId,
                               branchNo = dta.BranchCode,
                               batchId = btchId,
                               batchNo = dta.BatchNo,
                               captureRawId = dta.Id,
                               FrontGreyImage = dta.FrontGreyImage,
                               FrontTiffImage = dta.FrontTiffImage,
                               BackGreyImage = dta.BackGreyImage,
                               BackTiffImage = dta.BackTiffImage,
                               InstrumentType = dta.InstrumentType,
                               batchSeqNo = dta.BatchSeqNo,
                               slipSeqNo = dta.SlipNo
                           }).FirstOrDefault();
            }


            if (result1 != null)
            {
                ViewData["domainid"] = result1.domainId;
                ViewData["scannodeid"] = result1.scanningNodeId;
                ViewData["brnchno"] = result1.branchNo;
                ViewData["btchid"] = result1.batchId;
                ViewData["btchno"] = result1.batchNo;
                ViewData["doctype"] = intstrtype;
                ViewData["captureRawId"] = result1.captureRawId;
                ViewData["slipno"] = result1.slipSeqNo;
            }
            else
            {
                btch = db.BatchMaster.Find(btchId);

                if(intstrtype=="S")
                    btch.Status = 2;
                else
                    btch.Status = 4;

                btch.LockUserId = null;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            //// if slip order is Slip and Cheque
            //// {
            //// 1.
            //// slip to chq conversion logic
            //// if instrument type was S and it is updated to C
            //// mark the current record instrument type as 'C'

            //// save the slipno
            //// get all records after current record till next slip is found
            //// update current record and all records found to slipno saved
            //// else{
            //// 2.
            //// cheque to slip conversion logic
            //// if instrument type was C and it is updated to S
            //// get the max slipno in the batch
            //// save the slipno
            //// get all records after this cheque till next slip is found
            //// update current record and all records found to slipno saved
            //// }

            // get the slipno previous to this cheque record
            

            


            return View("DisplayDoc",result1);


        }


        private void convertdocument(string doctype,long lngCaptureRawid,int documentorder)
        {


            if (documentorder == 1)//cheque>slip
            {
                int currSlipno=0, NextSlipNo=0;

                //// if slip order is Cheque and Slip
                //// {
                //// 1.
                //// slip to chq conversion logic
                //// if instrument type was S and it is updated to C
                //// get the slipno next to the current record
                //// save the slipno
                //// get all records after current record till next slip is found
                //// update current record and all records found to slipno saved
                //// else{
                //// 2.
                //// cheque to slip conversion logic
                //// if instrument type was C and it is updated to S
                //// get the max slipno in the batch
                //// save the slipno
                //// get all records previous to current record till previous slip is found
                //// update current record and all records found to slipno saved
                //// }

                //mark slip as cheque
                //if (doctype == "S")
                //{
                //    //get next slipno
                //    var slipno = db.CaptureRawDatas.Where(m => (m.Id > lngCaptureRawid) &&
                //        (m.InstrumentType == "S")).Select(m => m.SlipNo).FirstOrDefault();

                //    if (slipno == null)
                //        return;

                //    NextSlipNo = Convert.ToInt32(slipno);


                //    var updtrecords=db.CaptureRawDatas.Where(m=>m.)



                //    db.SaveChanges();

                //}





            }

        }
    }
}
