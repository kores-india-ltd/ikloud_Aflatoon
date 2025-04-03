using ikloud_Aflatoon.Infrastructure;
using ikloud_Aflatoon.Models;
using System;
using System.Collections;
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
    public class OWDataEntryController : Controller
    {
        //
        // GET: /OWDataEntry/

        AflatoonEntities af = new AflatoonEntities();
        OWProcDataContext OWpro = new OWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
        string imgpth = System.Configuration.ConfigurationManager.AppSettings["OWsnippath"].ToString();

        public ActionResult SlipAmount()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["DE"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            int uid = (int)Session["uid"];
            try
            {

                SqlDataAdapter adp = new SqlDataAdapter("OWSelectSLPAmount", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();
                //-------------Added on 17-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                //-------------Added on 18-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);

                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<SlipAmountCapture>();
                SlipAmountCapture def;
                string[] ddt = new string[0];
                String tempstr = Session["SnipDate"].ToString();// "04.06.2016";//String.Concat(imgpth + Session["SnipDate"].ToString());
                ddt = tempstr.Split('/');
                tempstr = ddt[0] + '.' + ddt[1] + '.' + ddt[2];


                if (ds.Tables[0].Rows.Count > 0)
                {

                    def = new SlipAmountCapture
                    {
                        Id = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]),

                        FrontGrayImagePath = imgpth + "/" + tempstr + "/Amt-" + Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]) + ".jpg",//ds.Tables[0].Rows[0].ItemArray[1].ToString(),
                        SlipAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[2]),
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[1].ToString(),
                        Status = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[3]),
                        RawDataId = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[4]),
                        CustomerId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[5]),
                        DomainId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[6]),
                        ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[7]),
                    };
                    objectlst.Add(def);
                    //------------------------END------------------------//
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        def = new SlipAmountCapture
                        {
                            Id = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),

                            FrontGrayImagePath = imgpth + "/" + tempstr + "/Amt-" + Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]) + ".jpg",//ds.Tables[0].Rows[index].ItemArray[1].ToString(),
                            SlipAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[2]),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[1].ToString(),
                            Status = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[3]),
                            RawDataId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[4]),
                            CustomerId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[5]),
                            DomainId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[6]),
                            ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[7]),
                        };
                        //ViewBag.cnt = true;
                        objectlst.Add(def);
                        count = count - 1;
                        index = index + 1;
                    }

                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                    return View(objectlst);
                }
                else
                    return RedirectToAction("IWIndex", "Home", new { id = 1 });// return RedirectToAction("DeSelection", new { id = 1 });
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWDataEntry HttpGet SlipAmt- " + innerExcp });
            }
            //return View();
        }
        [HttpPost]
        public ActionResult SlipAmount(List<string> lst, bool snd, string img, string fulimg = null, string btnClose = "default", List<Int64> idlst = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["DE"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            try
            {


                int ttcnt = 0;
                int uid = (int)Session["uid"];//That will be Session value.
                if (lst != null)
                    ttcnt = lst.Count() / 8;

                // string stts = "";

                if (btnClose == "Close" && lst != null)
                    ttcnt = ttcnt + 1;

                if (ttcnt == 1)
                {
                    for (int i = 0; i < ttcnt; i++)
                    {

                        //if (Convert.ToBoolean(lst[2]) == true)

                        // stts = "A";
                        //else
                        //stts = "RJ";
                        Int64 id = Convert.ToInt64(lst[0]);
                        OWpro.UpdateOWSLPAmount(id, Convert.ToInt64(lst[4]), uid, Convert.ToDouble(lst[1]), Convert.ToInt16(lst[3]), lst[2].ToString(), Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt32(lst[5]), Convert.ToInt32(lst[6]), Convert.ToInt32(lst[7]));

                        for (int k = 0; k < idlst.Count; k++)
                        {
                            if (idlst[k] == id)
                                idlst.RemoveAt(k);

                        }
                    }
                }
                else
                {
                    for (int i = 0; i < ttcnt - 1; i++)
                    {

                        // stts = "A";
                        //else
                        //stts = "RJ";
                        Int64 id = Convert.ToInt64(lst[0]);
                        OWpro.UpdateOWSLPAmount(id, Convert.ToInt64(lst[4]), uid, Convert.ToDouble(lst[1]), Convert.ToInt16(lst[3]), lst[2].ToString(), Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt32(lst[5]), Convert.ToInt32(lst[6]), Convert.ToInt32(lst[7]));


                        for (int k = 0; k < idlst.Count; k++)
                        {
                            if (idlst[k] == id)
                                idlst.RemoveAt(k);

                        }
                        lst.RemoveRange(0, 8);

                    }
                }

                //---------------------------IF Close button called--------------------------------//
                if (btnClose == "Close")
                {
                    @Session["glob"] = true;
                    for (int p = 0; p < idlst.Count; p++)
                    {
                        OWpro.OWUnlockRecords(idlst[p], "SLPAmt");
                    }
                    return Json(false);
                }
                //------------------Select next Pending Record------------------
                //------------------------------Calling Proc for selecting data-----------------------

                // Procommand = new System.Data.Sql.("{ call SelectForDwld('"+ date.ToString("yyyy-MM-dd")+"')}",con);

                SqlDataAdapter adp = new SqlDataAdapter("OWSelectSLPAmount", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();
              //----------------Added on 17-05-2017---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                //-------------Added on 18-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);


                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<SlipAmountCapture>();
                SlipAmountCapture def;
                ArrayList ids = new ArrayList();
                bool checkid = false;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //--------------Checking Last record and list record are same------
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        if (Convert.ToInt64(lst[0]) == Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]))
                        {
                            // stts = "A";
                            //else
                            // stts = "RJ";
                            Int64 id = Convert.ToInt64(lst[0]);
                            OWpro.UpdateOWSLPAmount(id, Convert.ToInt64(lst[4]), uid, Convert.ToDouble(lst[1]), Convert.ToInt16(lst[3]), lst[2].ToString(), Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt32(lst[5]), Convert.ToInt32(lst[6]), Convert.ToInt32(lst[7]));

                            goto Notfnd;
                        }

                    }
                    //---------------Adding last record of list-----------------//
                    def = new SlipAmountCapture
                    {
                        Id = Convert.ToInt64(lst[0]),
                        FrontGrayImagePath = img,
                        Status = Convert.ToByte(lst[3]),
                        SlipAmount = Convert.ToDecimal(lst[1]),
                        FrontTiffImagePath = fulimg,
                        Action = lst[2].ToString(),
                        RawDataId = Convert.ToInt64(lst[4]),
                        CustomerId = Convert.ToInt32(lst[5]),
                        DomainId = Convert.ToInt32(lst[6]),
                        ScanningNodeId = Convert.ToInt32(lst[7]),
                    };
                    objectlst.Add(def);
                    ids.Add(def.Id);

                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;

                    string[] ddt = new string[0];
                    String tempstr = Session["SnipDate"].ToString();// "04.06.2016";//String.Concat(imgpth + Session["SnipDate"].ToString());
                    ddt = tempstr.Split('/');
                    tempstr = ddt[0] + '.' + ddt[1] + '.' + ddt[2];

                    while (count > 0)
                    {
                        for (int i = 0; i < ids.Count; i++)
                        {
                            // tempId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]);
                            if (Convert.ToInt64(ids[i]) == Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]))
                            {
                                checkid = true;
                                break;
                            }
                        }
                        if (checkid == false)
                        {


                            def = new SlipAmountCapture
                            {
                                Id = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),

                                FrontGrayImagePath = imgpth + "/" + tempstr + "/Amt-" + Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]) + ".jpg",//ds.Tables[0].Rows[index].ItemArray[1].ToString(),
                                SlipAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[2]),
                                FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[1].ToString(),
                                Status = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[3]),
                                RawDataId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[4]),
                                CustomerId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[5]),
                                DomainId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[6]),
                                ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[7]),
                            };

                            ids.Add(def.Id);
                            objectlst.Add(def);
                        }
                        checkid = false;
                        count = count - 1;
                        index = index + 1;
                    }
                    ViewBag.cnt = true;
                    return Json(objectlst);
                }
                else
                {
                    ViewBag.cnt = false;
                    goto Notfnd;
                }
            Notfnd:
                ViewBag.cnt = false;
                @Session["glob"] = true;
                return Json(false);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWDataEntry HttpPost SlipAmt- " + innerExcp });
            }
        }
        public ActionResult SlpAccountCapture()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["DE"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            try
            {


                int uid = (int)Session["uid"];

                SqlDataAdapter adp = new SqlDataAdapter("OWSelectSLPAccount", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();//Session["processdate"].ToString();
                //----------------Added on 17-05-2017---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                //-------------Added on 18-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);


                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<customAccount>();
                customAccount def;
                string[] ddt = new string[0];
                String tempstr = Session["SnipDate"].ToString();// "04.06.2016";//String.Concat(imgpth + Session["SnipDate"].ToString());
                ddt = tempstr.Split('/');
                //tempstr = ddt[0] + '.' + ddt[1] + '.' + ddt[2];
                tempstr = ddt[2] + ddt[1] + ddt[0];


                if (ds.Tables[0].Rows.Count > 0)
                {

                    def = new customAccount
                    {
                        Id = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]),
                        //-----snippet path--web config-----procdate--------------------CustomerID-------------------------NodeID--------------------------------BatchNo------------------------------------------BranchCode----------
                        FrontGreyImagePath = imgpth + "/" + tempstr + "/" + tempstr + "_" + ds.Tables[0].Rows[0].ItemArray[7] + "_" + ds.Tables[0].Rows[0].ItemArray[9] + "_" + ds.Tables[0].Rows[0].ItemArray[12] + "_" + ds.Tables[0].Rows[0].ItemArray[11] + "_" + ds.Tables[0].Rows[0].ItemArray[13] + "_SlipAcc.jpg",//ds.Tables[0].Rows[0].ItemArray[1].ToString(),
                        AccountNo1 = ds.Tables[0].Rows[0].ItemArray[2].ToString(),
                        AccountNo2 = ds.Tables[0].Rows[0].ItemArray[3].ToString(),
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[1].ToString(),
                        Status = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[5]),
                        RawDataId = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[6]),
                        CustomerId = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[7]),
                        DomainId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[8]),
                        ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[9]),
                        SlipAccountNoSettings = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[10]),
                    };
                    objectlst.Add(def);
                    //------------------------END------------------------//
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        def = new customAccount
                        {
                            Id = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[0]),

                            //-----snippet path--web config-----procdate--------------------CustomerID-------------------------NodeID--------------------------------BatchNo------------------------------------------BranchCode----------
                            FrontGreyImagePath = imgpth + "/" + tempstr + "/" + tempstr + "_" + ds.Tables[0].Rows[index].ItemArray[7] + "_" + ds.Tables[0].Rows[index].ItemArray[9] + "_" + ds.Tables[0].Rows[index].ItemArray[12] + "_" + ds.Tables[0].Rows[index].ItemArray[11] + "_" + ds.Tables[0].Rows[index].ItemArray[13] + "_SlipAcc.jpg",//ds.Tables[0].Rows[0].ItemArray[1].ToString(),

                            AccountNo1 = ds.Tables[0].Rows[index].ItemArray[2].ToString(),
                            AccountNo2 = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[1].ToString(),
                            Status = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[5]),
                            RawDataId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[6]),
                            CustomerId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[7]),
                            DomainId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[8]),
                            ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[9]),
                            SlipAccountNoSettings = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[10]),
                        };
                        //ViewBag.cnt = true;
                        objectlst.Add(def);
                        count = count - 1;
                        index = index + 1;
                    }

                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                    return View(objectlst);
                }
                else
                    return RedirectToAction("IWIndex", "Home", new { id = 1 });// return RedirectToAction("DeSelection", new { id = 1 });
                //return View();
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWDataEntry HttpGet SlipAc- " + innerExcp });
            }
        }
        [HttpPost]
        public ActionResult SlpAccountCapture(List<string> lst, bool snd, string img, string fulimg = null, string btnClose = "default", List<Int64> idlst = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["DE"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            int ttcnt = 0;
            int uid = (int)Session["uid"];//That will be Session value.
            string finalAccount = null;
            string fianlCBSAcDtls = "";
            string cbsdtails = "", jointdetails = "";
            string creditcardno = "";

            try
            {

                if (lst != null)
                    ttcnt = lst.Count() / 12;

                // string stts = "";

                if (btnClose == "Close" && lst != null)
                    ttcnt = ttcnt + 1;

                if (ttcnt == 1)
                {
                    for (int i = 0; i < ttcnt; i++)
                    {

                        //if (Convert.ToBoolean(lst[2]) == true)

                        // stts = "A";
                        //else
                        //stts = "RJ";
                        if (Convert.ToByte(lst[9]) == 2 && Convert.ToByte(lst[4]) == 1)
                        {
                            if (lst[1].ToString() == lst[2].ToString())
                            {
                                finalAccount = lst[1].ToString();
                                if (lst[10] != null)
                                    fianlCBSAcDtls = lst[10].ToString();

                                if (lst[1].ToString().Length == 16)
                                {
                                    if (Session["CreditCardValidationReq"].ToString() == "1")
                                    {
                                        if (fianlCBSAcDtls.Split('|').ElementAt(1) == "S")
                                            creditcardno = Session["CreditCardValidAcNo"].ToString();
                                        else
                                            creditcardno = Session["CreditCardInValidAcNo"].ToString();

                                    }
                                }

                            }
                            else
                            {
                                finalAccount = null;
                                fianlCBSAcDtls = null;
                                creditcardno = null;
                            }

                        }
                        else if (Convert.ToByte(lst[9]) == 1 && Convert.ToByte(lst[4]) == 0)
                        {
                            finalAccount = lst[1].ToString();
                            if (lst[10] != null)
                                fianlCBSAcDtls = lst[10].ToString();

                            if (lst[1].ToString().Length == 16)
                            {
                                if (Session["CreditCardValidationReq"].ToString() == "1")
                                {
                                    if (fianlCBSAcDtls.Split('|').ElementAt(1) == "S")
                                        creditcardno = Session["CreditCardValidAcNo"].ToString();
                                    else
                                        creditcardno = Session["CreditCardInValidAcNo"].ToString();
                                }
                            }
                        }
                        else
                        {
                            finalAccount = null;
                            fianlCBSAcDtls = null;
                            creditcardno = null;

                        }
                        if (lst[10] != null)
                            cbsdtails = lst[10].ToString();
                        else
                            cbsdtails = null;
                        if (lst[11] != null)
                            jointdetails = lst[11].ToString();
                        else
                            jointdetails = null;


                        Int64 id = Convert.ToInt64(lst[0]);
                        OWpro.UpdateOWSlipAccount(id, Convert.ToInt64(lst[5]), uid, lst[1].ToString(), finalAccount, Convert.ToInt16(lst[4]), lst[3].ToString(), Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt32(lst[6]), Convert.ToInt32(lst[7]), Convert.ToInt32(lst[8]), Convert.ToInt32(lst[9]), cbsdtails, jointdetails, fianlCBSAcDtls, creditcardno);

                        for (int k = 0; k < idlst.Count; k++)
                        {
                            if (idlst[k] == id)
                                idlst.RemoveAt(k);

                        }
                    }
                }
                else
                {
                    for (int i = 0; i < ttcnt - 1; i++)
                    {

                        // stts = "A";
                        //else
                        //stts = "RJ";
                        if (Convert.ToByte(lst[9]) == 2 && Convert.ToByte(lst[4]) == 1)
                        {
                            if (lst[1].ToString() == lst[2].ToString())
                            {
                                finalAccount = lst[1].ToString();
                                if (lst[10] != null)
                                    fianlCBSAcDtls = lst[10].ToString();

                                if (lst[1].ToString().Length == 16)
                                {
                                    if (Session["CreditCardValidationReq"].ToString() == "1")
                                    {
                                        if (fianlCBSAcDtls.Split('|').ElementAt(1) == "S")
                                            creditcardno = Session["CreditCardValidAcNo"].ToString();
                                        else
                                            creditcardno = Session["CreditCardInValidAcNo"].ToString();
                                    }
                                }
                            }
                            else
                            {

                                finalAccount = null;
                                fianlCBSAcDtls = null;
                                creditcardno = null;
                            }

                        }
                        else if (Convert.ToByte(lst[9]) == 1 && Convert.ToByte(lst[4]) == 0)
                        {
                            finalAccount = lst[1].ToString();
                            if (lst[10] != null)
                                fianlCBSAcDtls = lst[10].ToString();

                            if (lst[1].ToString().Length == 16)
                            {
                                if (Session["CreditCardValidationReq"].ToString() == "1")
                                {
                                    if (fianlCBSAcDtls.Split('|').ElementAt(1) == "S")
                                        creditcardno = Session["CreditCardValidAcNo"].ToString();
                                    else
                                        creditcardno = Session["CreditCardInValidAcNo"].ToString();
                                }
                            }
                        }
                        else
                        {
                            finalAccount = null;
                            fianlCBSAcDtls = null;
                            creditcardno = null;
                        }
                        if (lst[10] != null)
                            cbsdtails = lst[10].ToString();
                        else
                            cbsdtails = null;
                        if (lst[11] != null)
                            jointdetails = lst[11].ToString();
                        else
                            jointdetails = null;

                        Int64 id = Convert.ToInt64(lst[0]);
                        OWpro.UpdateOWSlipAccount(id, Convert.ToInt64(lst[5]), uid, lst[1].ToString(), finalAccount, Convert.ToInt16(lst[4]), lst[3].ToString(), Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt32(lst[6]), Convert.ToInt32(lst[7]), Convert.ToInt32(lst[8]), Convert.ToInt32(lst[9]), cbsdtails, jointdetails, fianlCBSAcDtls, creditcardno);


                        for (int k = 0; k < idlst.Count; k++)
                        {
                            if (idlst[k] == id)
                                idlst.RemoveAt(k);

                        }
                        lst.RemoveRange(0, 12);

                    }
                }

                //---------------------------IF Close button called--------------------------------//
                if (btnClose == "Close")
                {
                    @Session["glob"] = true;
                    for (int p = 0; p < idlst.Count; p++)
                    {
                        OWpro.OWUnlockRecords(idlst[p], "SlpAcct");
                    }
                    return Json(false);
                }
                //------------------Select next Pending Record------------------
                //------------------------------Calling Proc for selecting data-----------------------

                // Procommand = new System.Data.Sql.("{ call SelectForDwld('"+ date.ToString("yyyy-MM-dd")+"')}",con);

                SqlDataAdapter adp = new SqlDataAdapter("OWSelectSLPAccount", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();

                //----------------Added on 17-05-2017---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                //-------------Added on 18-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);


                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<customAccount>();
                customAccount def;
                ArrayList ids = new ArrayList();
                bool checkid = false;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //--------------Checking Last record and list record are same------
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        if (Convert.ToInt64(lst[0]) == Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]))
                        {
                            // stts = "A";
                            //else
                            // stts = "RJ";
                            if (Convert.ToByte(lst[9]) == 2 && Convert.ToByte(lst[4]) == 1)
                            {
                                if (lst[1].ToString() == lst[2].ToString())
                                {
                                    finalAccount = lst[1].ToString();
                                    if (lst[10] != null)
                                        fianlCBSAcDtls = lst[10].ToString();

                                    if (lst[1].ToString().Length == 16)
                                    {
                                        if (Session["CreditCardValidationReq"].ToString() == "1")
                                        {
                                            if (fianlCBSAcDtls.Split('|').ElementAt(1) == "S")
                                                creditcardno = Session["CreditCardValidAcNo"].ToString();
                                            else
                                                creditcardno = Session["CreditCardInValidAcNo"].ToString();

                                        }
                                    }

                                }
                                else
                                {
                                    finalAccount = null;
                                    fianlCBSAcDtls = null;
                                    creditcardno = null;
                                }

                            }
                            else if (Convert.ToByte(lst[9]) == 1 && Convert.ToByte(lst[4]) == 0)
                            {
                                finalAccount = lst[1].ToString();
                                if (lst[10] != null)
                                    fianlCBSAcDtls = lst[10].ToString();

                                if (lst[1].ToString().Length == 16)
                                {
                                    if (Session["CreditCardValidationReq"].ToString() == "1")
                                    {
                                        if (fianlCBSAcDtls.Split('|').ElementAt(1) == "S")
                                            creditcardno = Session["CreditCardValidAcNo"].ToString();
                                        else
                                            creditcardno = Session["CreditCardInValidAcNo"].ToString();

                                    }
                                }

                            }
                            else
                            {
                                finalAccount = null;
                                fianlCBSAcDtls = null;
                                creditcardno = null;
                            }
                            if (lst[10] != null)
                                cbsdtails = lst[10].ToString();
                            else
                                cbsdtails = null;

                            if (lst[11] != null)
                                jointdetails = lst[11].ToString();
                            else
                                jointdetails = null;

                            Int64 id = Convert.ToInt64(lst[0]);
                            OWpro.UpdateOWSlipAccount(id, Convert.ToInt64(lst[5]), uid, lst[1].ToString(), finalAccount, Convert.ToInt16(lst[4]), lst[3].ToString(), Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt32(lst[6]), Convert.ToInt32(lst[7]), Convert.ToInt32(lst[8]), Convert.ToInt32(lst[9]), cbsdtails, jointdetails, fianlCBSAcDtls, creditcardno);

                            goto Notfnd;
                        }

                    }
                    //---------------Adding last record of list-----------------//

                    //if (lst[10] != null)
                    //    cbsdtails = lst[10].ToString();
                    //else
                    //    cbsdtails = null;

                    //if (lst[11] != null)
                    //    jointdetails = lst[11].ToString();

                    def = new customAccount
                    {
                        Id = Convert.ToInt64(lst[0]),
                        FrontGreyImagePath = img,
                        Status = Convert.ToByte(lst[4]),
                        AccountNo2 = lst[1].ToString(),
                        AccountNo1 = lst[2].ToString(),
                        FrontTiffImagePath = fulimg,
                        Action = lst[3].ToString(),
                        RawDataId = Convert.ToInt64(lst[5]),
                        CustomerId = Convert.ToInt16(lst[6]),
                        DomainId = Convert.ToInt32(lst[7]),
                        ScanningNodeId = Convert.ToInt32(lst[8]),
                        SlipAccountNoSettings = Convert.ToByte(lst[9]),
                        CbsClinDtls = lst[10].ToString(),
                        CbsjointHlds = lst[11].ToString(),
                    };
                    objectlst.Add(def);
                    ids.Add(def.Id);

                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;

                    string[] ddt = new string[0];
                    String tempstr = Session["SnipDate"].ToString();// "04.06.2016";//String.Concat(imgpth + Session["SnipDate"].ToString());
                    ddt = tempstr.Split('/');
                    // tempstr = ddt[0] + '.' + ddt[1] + '.' + ddt[2];
                    tempstr = ddt[2] + ddt[1] + ddt[0];

                    while (count > 0)
                    {
                        for (int i = 0; i < ids.Count; i++)
                        {
                            // tempId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]);
                            if (Convert.ToInt64(ids[i]) == Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]))
                            {
                                checkid = true;
                                break;
                            }
                        }
                        if (checkid == false)
                        {


                            def = new customAccount
                            {
                                Id = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),

                                //-----snippet path--web config-----procdate--------------------CustomerID-------------------------NodeID--------------------------------BatchNo------------------------------------------BranchCode----------
                                FrontGreyImagePath = imgpth + "/" + tempstr + "/" + tempstr + "_" + ds.Tables[0].Rows[index].ItemArray[7] + "_" + ds.Tables[0].Rows[index].ItemArray[9] + "_" + ds.Tables[0].Rows[index].ItemArray[12] + "_" + ds.Tables[0].Rows[index].ItemArray[11] + "_" + ds.Tables[0].Rows[index].ItemArray[13] + "_SlipAcc.jpg",//ds.Tables[0].Rows[0].ItemArray[1].ToString(),

                                AccountNo1 = ds.Tables[0].Rows[index].ItemArray[2].ToString(),
                                AccountNo2 = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                                FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[1].ToString(),
                                Status = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[5]),
                                RawDataId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[6]),
                                CustomerId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[7]),
                                DomainId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[8]),
                                ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[9]),
                                SlipAccountNoSettings = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[10]),
                            };
                            ids.Add(def.Id);
                            objectlst.Add(def);
                        }
                        checkid = false;
                        count = count - 1;
                        index = index + 1;
                    }
                    ViewBag.cnt = true;
                    return Json(objectlst);
                }
                else
                {
                    ViewBag.cnt = false;
                    goto Notfnd;
                }
            Notfnd:
                ViewBag.cnt = false;
                @Session["glob"] = true;
                return Json(false);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWDataEntry HttpPost SlipAc- " + innerExcp });
            }

        }

        public ActionResult ChqAmount()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["DE"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            int uid = (int)Session["uid"];
            try
            {

                SqlDataAdapter adp = new SqlDataAdapter("OWSelectChequeAmount", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();//Session["processdate"].ToString();

                //----------------Added on 17-05-2017---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                //-------------Added on 18-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);


                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<ChequeAmountCapture>();
                ChequeAmountCapture def;
                string[] ddt = new string[0];
                String tempstr = Session["SnipDate"].ToString();// "04.06.2016";//String.Concat(imgpth + Session["SnipDate"].ToString());
                ddt = tempstr.Split('/');
                //tempstr = ddt[0] + '.' + ddt[1] + '.' + ddt[2];
                tempstr = ddt[2] + ddt[1] + ddt[0];


                if (ds.Tables[0].Rows.Count > 0)
                {

                    def = new ChequeAmountCapture
                    {
                        Id = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]),
                        //-----snippet path--web config-----procdate--------------------CustomerID-------------------------NodeID--------------------------------BatchNo------------------------------------------BranchCode----------
                        FrontGreyImagePath = imgpth + "/" + tempstr + "/" + tempstr + "_" + ds.Tables[0].Rows[0].ItemArray[7] + "_" + ds.Tables[0].Rows[0].ItemArray[9] + "_" + ds.Tables[0].Rows[0].ItemArray[12] + "_" + ds.Tables[0].Rows[0].ItemArray[11] + "_" + ds.Tables[0].Rows[0].ItemArray[13] + "_Amt.jpg",//ds.Tables[0].Rows[0].ItemArray[1].ToString(),
                        Amount1 = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[2]),
                        Amount2 = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[3]),
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[1].ToString(),
                        Status = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[5]),
                        RawDataId = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[6]),
                        CustomerId = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[7]),
                        DomainId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[8]),
                        ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[9]),
                        ChequeAmountSettings = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[10]),
                        //BranchCode = ds.Tables[0].Rows[0].ItemArray[11].ToString(),
                        //BatchNo = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[12]),
                        //BatchSeqNo = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[13]),
                    };
                    objectlst.Add(def);
                    //------------------------END------------------------//
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        def = new ChequeAmountCapture
                        {
                            Id = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),

                            //-----snippet path--web config-----procdate--------------------CustomerID-------------------------NodeID--------------------------------BatchNo------------------------------------------BranchCode----------
                            FrontGreyImagePath = imgpth + "/" + tempstr + "/" + tempstr + "_" + ds.Tables[0].Rows[index].ItemArray[7] + "_" + ds.Tables[0].Rows[index].ItemArray[9] + "_" + ds.Tables[0].Rows[index].ItemArray[12] + "_" + ds.Tables[0].Rows[index].ItemArray[11] + "_" + ds.Tables[0].Rows[index].ItemArray[13] + "_Amt.jpg",
                            Amount1 = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[2]),
                            Amount2 = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[3]),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[1].ToString(),
                            Status = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[5]),
                            RawDataId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[6]),
                            CustomerId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[7]),
                            DomainId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[8]),
                            ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[9]),
                            ChequeAmountSettings = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[10]),
                            //BranchCode = ds.Tables[0].Rows[index].ItemArray[11].ToString(),
                            //BatchNo = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[12]),
                            //BatchSeqNo = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[13]),
                        };
                        //ViewBag.cnt = true;
                        objectlst.Add(def);
                        count = count - 1;
                        index = index + 1;
                    }

                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                    return View(objectlst);
                }
                else
                    return RedirectToAction("IWIndex", "Home", new { id = 1 });// return RedirectToAction("DeSelection", new { id = 1 });
                //return View();
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWDataEntry HttpGet ChqAmt- " + innerExcp });
            }
        }
        [HttpPost]
        public ActionResult ChqAmount(List<string> lst, bool snd, string img, string fulimg = null, string btnClose = "default", List<Int64> idlst = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["DE"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            int ttcnt = 0;
            int uid = (int)Session["uid"];//That will be Session value.
            double finalAmount = 0;

            try
            {

                if (lst != null)
                    ttcnt = lst.Count() / 10;

                // string stts = "";

                if (btnClose == "Close" && lst != null)
                    ttcnt = ttcnt + 1;

                if (ttcnt == 1)
                {
                    for (int i = 0; i < ttcnt; i++)
                    {

                        //if (Convert.ToBoolean(lst[2]) == true)

                        // stts = "A";
                        //else
                        //stts = "RJ";
                        if ((Convert.ToByte(lst[9]) == 1 && Convert.ToByte(lst[4]) == 1) || (Convert.ToByte(lst[9]) == 2 && Convert.ToByte(lst[4]) == 1))
                        {
                            if (Convert.ToDouble(lst[1]) == Convert.ToDouble(lst[2]))
                                finalAmount = Convert.ToDouble(lst[1]);
                            else
                                finalAmount = 0;

                        }
                        else if ((Convert.ToByte(lst[9]) == 3 && Convert.ToByte(lst[4]) == 0) || (Convert.ToByte(lst[9]) == 4 && Convert.ToByte(lst[4]) == 0))
                            finalAmount = Convert.ToDouble(lst[1]);
                        else
                            finalAmount = 0;

                        Int64 id = Convert.ToInt64(lst[0]);
                        OWpro.UpdateOWChequeAmount(id, Convert.ToInt64(lst[5]), uid, Convert.ToDouble(lst[1]), finalAmount, Convert.ToInt16(lst[4]), lst[3].ToString(), Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt16(lst[6]), Convert.ToInt32(lst[7]), Convert.ToInt32(lst[8]), Convert.ToInt32(lst[9]));

                        for (int k = 0; k < idlst.Count; k++)
                        {
                            if (idlst[k] == id)
                                idlst.RemoveAt(k);

                        }
                    }
                }
                else
                {
                    for (int i = 0; i < ttcnt - 1; i++)
                    {

                        // stts = "A";
                        //else
                        //stts = "RJ";
                        if ((Convert.ToByte(lst[9]) == 1 && Convert.ToByte(lst[4]) == 1) || (Convert.ToByte(lst[9]) == 2 && Convert.ToByte(lst[4]) == 1))
                        {
                            if (Convert.ToDouble(lst[1]) == Convert.ToDouble(lst[2]))
                                finalAmount = Convert.ToDouble(lst[1]);
                            else
                                finalAmount = 0;
                        }
                        else if ((Convert.ToByte(lst[9]) == 3 && Convert.ToByte(lst[4]) == 0) || (Convert.ToByte(lst[9]) == 4 && Convert.ToByte(lst[4]) == 0))
                            finalAmount = Convert.ToDouble(lst[1]);

                        else
                            finalAmount = 0;

                        Int64 id = Convert.ToInt64(lst[0]);
                        OWpro.UpdateOWChequeAmount(id, Convert.ToInt64(lst[5]), uid, Convert.ToDouble(lst[1]), finalAmount, Convert.ToInt16(lst[4]), lst[3].ToString(), Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt16(lst[6]), Convert.ToInt32(lst[7]), Convert.ToInt32(lst[8]), Convert.ToInt32(lst[9]));


                        for (int k = 0; k < idlst.Count; k++)
                        {
                            if (idlst[k] == id)
                                idlst.RemoveAt(k);

                        }
                        lst.RemoveRange(0, 10);

                    }
                }

                //---------------------------IF Close button called--------------------------------//
                if (btnClose == "Close")
                {
                    @Session["glob"] = true;
                    for (int p = 0; p < idlst.Count; p++)
                    {
                        OWpro.OWUnlockRecords(idlst[p], "ChqAmt");
                    }
                    return Json(false);
                }
                //------------------Select next Pending Record------------------
                //------------------------------Calling Proc for selecting data-----------------------

                // Procommand = new System.Data.Sql.("{ call SelectForDwld('"+ date.ToString("yyyy-MM-dd")+"')}",con);

                SqlDataAdapter adp = new SqlDataAdapter("OWSelectChequeAmount", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();
                //----------------Added on 17-05-2017---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                //-------------Added on 18-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);


                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<ChequeAmountCapture>();
                ChequeAmountCapture def;
                ArrayList ids = new ArrayList();
                bool checkid = false;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //--------------Checking Last record and list record are same------
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        if (Convert.ToInt64(lst[0]) == Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]))
                        {
                            // stts = "A";
                            //else
                            // stts = "RJ";
                            if ((Convert.ToByte(lst[9]) == 1 && Convert.ToByte(lst[4]) == 1) || (Convert.ToByte(lst[9]) == 2 && Convert.ToByte(lst[4]) == 1))
                            {
                                if (Convert.ToDouble(lst[1]) == Convert.ToDouble(lst[2]))
                                    finalAmount = Convert.ToDouble(lst[1]);
                                else
                                    finalAmount = 0;

                            }
                            else if ((Convert.ToByte(lst[9]) == 3 && Convert.ToByte(lst[4]) == 0) || (Convert.ToByte(lst[9]) == 4 && Convert.ToByte(lst[4]) == 0))
                                finalAmount = Convert.ToDouble(lst[1]);

                            else
                                finalAmount = 0;

                            Int64 id = Convert.ToInt64(lst[0]);
                            OWpro.UpdateOWChequeAmount(id, Convert.ToInt64(lst[5]), uid, Convert.ToDouble(lst[1]), finalAmount, Convert.ToInt16(lst[4]), lst[3].ToString(), Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt16(lst[6]), Convert.ToInt32(lst[7]), Convert.ToInt32(lst[8]), Convert.ToInt32(lst[9]));

                            goto Notfnd;
                        }

                    }
                    //---------------Adding last record of list-----------------//
                    def = new ChequeAmountCapture
                    {
                        Id = Convert.ToInt64(lst[0]),
                        FrontGreyImagePath = img,
                        Status = Convert.ToByte(lst[4]),
                        Amount2 = Convert.ToDecimal(lst[1]),
                        Amount1 = Convert.ToDecimal(lst[2]),
                        FrontTiffImagePath = fulimg,
                        Action = lst[3].ToString(),
                        RawDataId = Convert.ToInt64(lst[5]),
                        CustomerId = Convert.ToInt16(lst[6]),
                        DomainId = Convert.ToInt32(lst[7]),
                        ScanningNodeId = Convert.ToInt32(lst[8]),
                        ChequeAmountSettings = Convert.ToByte(lst[9]),
                        //BranchCode = lst[10].ToString(),
                        //BatchNo = Convert.ToInt16(lst[11].ToString()),
                        //BatchSeqNo = Convert.ToInt16(lst[12].ToString()),
                    };
                    objectlst.Add(def);
                    ids.Add(def.Id);

                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;

                    string[] ddt = new string[0];
                    String tempstr = Session["SnipDate"].ToString();// "04.06.2016";//String.Concat(imgpth + Session["SnipDate"].ToString());
                    ddt = tempstr.Split('/');
                    // tempstr = ddt[0] + '.' + ddt[1] + '.' + ddt[2];
                    tempstr = ddt[2] + ddt[1] + ddt[0];

                    while (count > 0)
                    {
                        for (int i = 0; i < ids.Count; i++)
                        {
                            // tempId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]);
                            if (Convert.ToInt64(ids[i]) == Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]))
                            {
                                checkid = true;
                                break;
                            }
                        }
                        if (checkid == false)
                        {


                            def = new ChequeAmountCapture
                            {
                                Id = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),

                                //-----snippet path--web config-----procdate--------------------CustomerID-------------------------NodeID--------------------------------BatchNo------------------------------------------BranchCode----------
                                FrontGreyImagePath = imgpth + "/" + tempstr + "/" + tempstr + "_" + ds.Tables[0].Rows[index].ItemArray[7] + "_" + ds.Tables[0].Rows[index].ItemArray[9] + "_" + ds.Tables[0].Rows[index].ItemArray[12] + "_" + ds.Tables[0].Rows[index].ItemArray[11] + "_" + ds.Tables[0].Rows[index].ItemArray[13] + "_Amt.jpg",
                                Amount1 = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[2]),
                                Amount2 = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[3]),
                                FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[1].ToString(),
                                Status = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[5]),
                                RawDataId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[6]),
                                CustomerId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[7]),
                                DomainId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[8]),
                                ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[9]),
                                ChequeAmountSettings = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[10]),
                                BranchCode = ds.Tables[0].Rows[index].ItemArray[11].ToString(),
                                BatchNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[12]),
                                BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[13]),
                            };
                            ids.Add(def.Id);
                            objectlst.Add(def);
                        }
                        checkid = false;
                        count = count - 1;
                        index = index + 1;
                    }
                    ViewBag.cnt = true;
                    return Json(objectlst);
                }
                else
                {
                    ViewBag.cnt = false;
                    goto Notfnd;
                }
            Notfnd:
                ViewBag.cnt = false;
                @Session["glob"] = true;
                return Json(false);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWDataEntry HttpPost ChqAmt- " + innerExcp });
            }
        }

        public ActionResult ChqDate()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["DE"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            int uid = (int)Session["uid"];
            try
            {

                SqlDataAdapter adp = new SqlDataAdapter("OWSelectChequeDate", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();//Session["processdate"].ToString();

                //----------------Added on 17-05-2017---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                //-------------Added on 18-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);


                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<ChequeDateCapture>();
                ChequeDateCapture def;
                string[] ddt = new string[0];
                String tempstr = Session["SnipDate"].ToString();// "04.06.2016";//String.Concat(imgpth + Session["SnipDate"].ToString());
                ddt = tempstr.Split('/');
                // tempstr = ddt[0] + '.' + ddt[1] + '.' + ddt[2];
                tempstr = ddt[2] + ddt[1] + ddt[0];


                if (ds.Tables[0].Rows.Count > 0)
                {

                    def = new ChequeDateCapture
                    {
                        Id = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]),

                        //-----snippet path--web config-----procdate--------------------CustomerID-------------------------NodeID--------------------------------BatchNo------------------------------------------BranchCode----------
                        FrontGreyImagePath = imgpth + "/" + tempstr + "/" + tempstr + "_" + ds.Tables[0].Rows[0].ItemArray[7] + "_" + ds.Tables[0].Rows[0].ItemArray[9] + "_" + ds.Tables[0].Rows[0].ItemArray[12] + "_" + ds.Tables[0].Rows[0].ItemArray[11] + "_" + ds.Tables[0].Rows[0].ItemArray[13] + "_Date.jpg",//ds.Tables[0].Rows[0].ItemArray[1].ToString(),
                        Date1 = ds.Tables[0].Rows[0].ItemArray[2].ToString(),
                        Date2 = ds.Tables[0].Rows[0].ItemArray[3].ToString(),
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[1].ToString(),
                        Status = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[5]),
                        RawDataId = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[6]),
                        CustomerId = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[7]),
                        DomainId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[8]),
                        ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[9]),
                        ChequeDateSettings = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[10]),
                    };
                    objectlst.Add(def);
                    //------------------------END------------------------//
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        def = new ChequeDateCapture
                        {
                            Id = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),
                            //-----snippet path--web config-----procdate--------------------CustomerID-------------------------NodeID--------------------------------BatchNo------------------------------------------BranchCode----------
                            FrontGreyImagePath = imgpth + "/" + tempstr + "/" + tempstr + "_" + ds.Tables[0].Rows[index].ItemArray[7] + "_" + ds.Tables[0].Rows[index].ItemArray[9] + "_" + ds.Tables[0].Rows[index].ItemArray[12] + "_" + ds.Tables[0].Rows[index].ItemArray[11] + "_" + ds.Tables[0].Rows[index].ItemArray[13] + "_Date.jpg",//ds.Tables[0].Rows[0].ItemArray[1].ToString(),
                            Date1 = ds.Tables[0].Rows[index].ItemArray[2].ToString(),
                            Date2 = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[1].ToString(),
                            Status = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[5]),
                            RawDataId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[6]),
                            CustomerId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[7]),
                            DomainId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[8]),
                            ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[9]),
                            ChequeDateSettings = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[10]),
                        };
                        //ViewBag.cnt = true;
                        objectlst.Add(def);
                        count = count - 1;
                        index = index + 1;
                    }

                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                    return View(objectlst);
                }
                else
                    return RedirectToAction("IWIndex", "Home", new { id = 1 });// return RedirectToAction("DeSelection", new { id = 1 });
                //return View();
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWDataEntry HttpGet ChqDate- " + innerExcp });
            }
        }

        [HttpPost]
        public ActionResult ChqDate(List<string> lst, bool snd, string img, string fulimg = null, string btnClose = "default", List<Int64> idlst = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["DE"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            int ttcnt = 0;
            int uid = (int)Session["uid"];//That will be Session value.
            string finalDate = "";
            try
            {

                if (lst != null)
                    ttcnt = lst.Count() / 10;

                // string stts = "";

                if (btnClose == "Close" && lst != null)
                    ttcnt = ttcnt + 1;

                if (ttcnt == 1)
                {
                    for (int i = 0; i < ttcnt; i++)
                    {

                        //if (Convert.ToBoolean(lst[2]) == true)120916

                        // stts = "A";
                        //else
                        //stts = "RJ";
                        if ((Convert.ToByte(lst[9]) == 1 && Convert.ToByte(lst[4]) == 1) || (Convert.ToByte(lst[9]) == 2 && Convert.ToByte(lst[4]) == 1))
                        {
                            string tempdate = "20" + lst[1].ToString().Substring(4, 2) + "-" + lst[1].ToString().Substring(2, 2) + "-" + lst[1].ToString().Substring(0, 2);
                            if (tempdate == lst[2].ToString())
                                finalDate = lst[1].ToString();
                            else
                                finalDate = "";

                        }
                        else if ((Convert.ToByte(lst[9]) == 3 && Convert.ToByte(lst[4]) == 0) || (Convert.ToByte(lst[9]) == 4 && Convert.ToByte(lst[4]) == 0))
                            finalDate = lst[1].ToString();
                        else
                            finalDate = "";

                        Int64 id = Convert.ToInt64(lst[0]);
                        OWpro.UpdateOWChequeDate(id, Convert.ToInt64(lst[5]), uid, lst[1].ToString(), finalDate, Convert.ToInt16(lst[4]), lst[3].ToString(), Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt16(lst[6]), Convert.ToInt32(lst[7]), Convert.ToInt32(lst[8]), Convert.ToInt32(lst[9]));

                        for (int k = 0; k < idlst.Count; k++)
                        {
                            if (idlst[k] == id)
                                idlst.RemoveAt(k);

                        }
                    }
                }
                else
                {
                    for (int i = 0; i < ttcnt - 1; i++)
                    {

                        // stts = "A";
                        //else
                        //stts = "RJ";
                        if ((Convert.ToByte(lst[9]) == 1 && Convert.ToByte(lst[4]) == 1) || (Convert.ToByte(lst[9]) == 2 && Convert.ToByte(lst[4]) == 1))
                        {
                            string tempdate = "20" + lst[1].ToString().Substring(4, 2) + "-" + lst[1].ToString().Substring(2, 2) + "-" + lst[1].ToString().Substring(0, 2);
                            if (tempdate == lst[2].ToString())
                                finalDate = lst[1].ToString();
                            else
                                finalDate = "";
                        }
                        else if ((Convert.ToByte(lst[9]) == 3 && Convert.ToByte(lst[4]) == 0) || (Convert.ToByte(lst[9]) == 4 && Convert.ToByte(lst[4]) == 0))
                            finalDate = lst[1].ToString();

                        else
                            finalDate = "";

                        Int64 id = Convert.ToInt64(lst[0]);
                        OWpro.UpdateOWChequeDate(id, Convert.ToInt64(lst[5]), uid, lst[1].ToString(), finalDate, Convert.ToInt16(lst[4]), lst[3].ToString(), Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt16(lst[6]), Convert.ToInt32(lst[7]), Convert.ToInt32(lst[8]), Convert.ToInt32(lst[9]));


                        for (int k = 0; k < idlst.Count; k++)
                        {
                            if (idlst[k] == id)
                                idlst.RemoveAt(k);

                        }
                        lst.RemoveRange(0, 10);

                    }
                }

                //---------------------------IF Close button called--------------------------------//
                if (btnClose == "Close")
                {
                    @Session["glob"] = true;
                    for (int p = 0; p < idlst.Count; p++)
                    {
                        OWpro.OWUnlockRecords(idlst[p], "ChqDate");
                    }
                    return Json(false);
                }
                //------------------Select next Pending Record------------------
                //------------------------------Calling Proc for selecting data-----------------------

                // Procommand = new System.Data.Sql.("{ call SelectForDwld('"+ date.ToString("yyyy-MM-dd")+"')}",con);

                SqlDataAdapter adp = new SqlDataAdapter("OWSelectChequeDate", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();

                //----------------Added on 17-05-2017---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                //-------------Added on 18-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);


                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<ChequeDateCapture>();
                ChequeDateCapture def;
                ArrayList ids = new ArrayList();
                bool checkid = false;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //--------------Checking Last record and list record are same------
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        if (Convert.ToInt64(lst[0]) == Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]))
                        {
                            // stts = "A";
                            //else
                            // stts = "RJ";
                            if ((Convert.ToByte(lst[9]) == 1 && Convert.ToByte(lst[4]) == 1) || (Convert.ToByte(lst[9]) == 2 && Convert.ToByte(lst[4]) == 1))
                            {
                                string tempdate = "20" + lst[1].ToString().Substring(4, 2) + "-" + lst[1].ToString().Substring(2, 2) + "-" + lst[1].ToString().Substring(0, 2);
                                if (tempdate == lst[2].ToString())
                                    finalDate = lst[1].ToString();
                                else
                                    finalDate = "";

                            }
                            else if ((Convert.ToByte(lst[9]) == 3 && Convert.ToByte(lst[4]) == 0) || (Convert.ToByte(lst[9]) == 4 && Convert.ToByte(lst[4]) == 0))
                                finalDate = lst[1].ToString();

                            else
                                finalDate = "";

                            Int64 id = Convert.ToInt64(lst[0]);
                            OWpro.UpdateOWChequeDate(id, Convert.ToInt64(lst[5]), uid, lst[1].ToString(), finalDate, Convert.ToInt16(lst[4]), lst[3].ToString(), Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt16(lst[6]), Convert.ToInt32(lst[7]), Convert.ToInt32(lst[8]), Convert.ToInt32(lst[9]));

                            goto Notfnd;
                        }

                    }
                    //---------------Adding last record of list-----------------//
                    def = new ChequeDateCapture
                    {
                        Id = Convert.ToInt64(lst[0]),
                        FrontGreyImagePath = img,
                        Status = Convert.ToByte(lst[4]),
                        Date1 = lst[2].ToString(),
                        Date2 = lst[1].ToString(),
                        FrontTiffImagePath = fulimg,
                        Action = lst[3].ToString(),
                        RawDataId = Convert.ToInt64(lst[5]),
                        CustomerId = Convert.ToInt16(lst[6]),
                        DomainId = Convert.ToInt32(lst[7]),
                        ScanningNodeId = Convert.ToInt32(lst[8]),
                        ChequeDateSettings = Convert.ToByte(lst[9]),
                    };
                    objectlst.Add(def);
                    ids.Add(def.Id);

                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;

                    string[] ddt = new string[0];
                    String tempstr = Session["SnipDate"].ToString();// "04.06.2016";//String.Concat(imgpth + Session["SnipDate"].ToString());
                    ddt = tempstr.Split('/');
                    // tempstr = ddt[0] + '.' + ddt[1] + '.' + ddt[2];
                    tempstr = ddt[2] + ddt[1] + ddt[0];

                    while (count > 0)
                    {
                        for (int i = 0; i < ids.Count; i++)
                        {
                            // tempId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]);
                            if (Convert.ToInt64(ids[i]) == Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]))
                            {
                                checkid = true;
                                break;
                            }
                        }
                        if (checkid == false)
                        {


                            def = new ChequeDateCapture
                            {
                                Id = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[0]),

                                //-----snippet path--web config-----procdate--------------------CustomerID-------------------------NodeID--------------------------------BatchNo------------------------------------------BranchCode----------
                                FrontGreyImagePath = imgpth + "/" + tempstr + "/" + tempstr + "_" + ds.Tables[0].Rows[index].ItemArray[7] + "_" + ds.Tables[0].Rows[index].ItemArray[9] + "_" + ds.Tables[0].Rows[index].ItemArray[12] + "_" + ds.Tables[0].Rows[index].ItemArray[11] + "_" + ds.Tables[0].Rows[index].ItemArray[13] + "_Date.jpg",//ds.Tables[0].Rows[0].ItemArray[1].ToString(),
                                Date1 = ds.Tables[0].Rows[index].ItemArray[2].ToString(),
                                Date2 = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                                FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[1].ToString(),
                                Status = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[5]),
                                RawDataId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[6]),
                                CustomerId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[7]),
                                DomainId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[8]),
                                ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[9]),
                                ChequeDateSettings = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[10]),
                            };
                            ids.Add(def.Id);
                            objectlst.Add(def);
                        }
                        checkid = false;
                        count = count - 1;
                        index = index + 1;
                    }
                    ViewBag.cnt = true;
                    return Json(objectlst);
                }
                else
                {
                    ViewBag.cnt = false;
                    goto Notfnd;
                }
            Notfnd:
                ViewBag.cnt = false;
                @Session["glob"] = true;
                return Json(false);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWDataEntry HttPost ChqDate- " + innerExcp });
            }
        }
        //------------------------------- IWMICR-------------------------------//
        public ActionResult OWMICR()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["DE"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            int uid = (int)Session["uid"];

            try
            {

                SqlDataAdapter adp = new SqlDataAdapter("OWSelectChequeMICR", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();

                //----------------Added on 17-05-2017---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                //-------------Added on 18-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);


                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<ChequeMICRCapture>();
                ChequeMICRCapture def;

                string[] ddt = new string[0];
                String tempstr = Session["SnipDate"].ToString();// "04.06.2016";//String.Concat(imgpth + Session["SnipDate"].ToString());
                ddt = tempstr.Split('/');
                //tempstr = ddt[0] + '.' + ddt[1] + '.' + ddt[2];
                tempstr = ddt[2] + ddt[1] + ddt[0];

                if (ds.Tables[0].Rows.Count > 0)
                {
                    def = new ChequeMICRCapture//
                    {
                        Id = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]),
                        //-----snippet path--web config-----procdate--------------------------------------CustomerID-------------------------NodeID--------------------------------BatchNo------------------------------------------BranchCode----------
                        FrontGreyImagePath = imgpth + "/" + tempstr + "/" + tempstr + "_" + ds.Tables[0].Rows[0].ItemArray[17] + "_" + ds.Tables[0].Rows[0].ItemArray[19] + "_" + ds.Tables[0].Rows[0].ItemArray[21] + "_" + ds.Tables[0].Rows[0].ItemArray[20] + "_" + ds.Tables[0].Rows[0].ItemArray[22] + "_MICR.jpg",//ds.Tables[0].Rows[0].ItemArray[1].ToString(),
                        //  FrontGreyImagePath = ds.Tables[0].Rows[0].ItemArray[1].ToString(),
                        ChequeNoMICR = ds.Tables[0].Rows[0].ItemArray[2].ToString(),
                        SortCodeMICR = ds.Tables[0].Rows[0].ItemArray[3].ToString(),
                        SANMICR = ds.Tables[0].Rows[0].ItemArray[4].ToString(),
                        TransCodeMICR = ds.Tables[0].Rows[0].ItemArray[5].ToString(),
                        ChequeNoNI = ds.Tables[0].Rows[0].ItemArray[6].ToString(),
                        SortCodeNI = ds.Tables[0].Rows[0].ItemArray[7].ToString(),
                        SANNI = ds.Tables[0].Rows[0].ItemArray[8].ToString(),
                        TransCodeNI = ds.Tables[0].Rows[0].ItemArray[9].ToString(),
                        ChequeNoPara = ds.Tables[0].Rows[0].ItemArray[10].ToString(),
                        SortCodePara = ds.Tables[0].Rows[0].ItemArray[11].ToString(),
                        SANPara = ds.Tables[0].Rows[0].ItemArray[12].ToString(),
                        TransCodePara = ds.Tables[0].Rows[0].ItemArray[13].ToString(),
                        //FrontGreyImagePath = imgpth + "/" + tempstr + "/Code-" + Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[0]) + ".jpg",
                        MICRRepairStatus = ds.Tables[0].Rows[0].ItemArray[15].ToString(),
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[1].ToString(),
                        RawDataId = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[16].ToString()),
                        CustomerId = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[17].ToString()),
                        DomainId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[18].ToString()),
                        ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[19].ToString()),
                        //BranchCode = ds.Tables[0].Rows[0].ItemArray[20].ToString(),
                        //BatchNo = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[21]),
                        //BatchSeqNo = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[22]),
                    };
                    objectlst.Add(def);
                    //------------------------END------------------------------//
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        def = new ChequeMICRCapture
                        {

                            Id = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),
                            //-----snippet path--web config-----procdate--------------------------------------CustomerID-------------------------NodeID--------------------------------BatchNo------------------------------------------BranchCode----------
                            FrontGreyImagePath = imgpth + "/" + tempstr + "/" + tempstr + "_" + ds.Tables[0].Rows[index].ItemArray[17] + "_" + ds.Tables[0].Rows[index].ItemArray[19] + "_" + ds.Tables[0].Rows[index].ItemArray[21] + "_" + ds.Tables[0].Rows[index].ItemArray[20] + "_" + ds.Tables[0].Rows[index].ItemArray[22] + "_MICR.jpg",//ds.Tables[0].Rows[0].ItemArray[1].ToString(),
                            ChequeNoMICR = ds.Tables[0].Rows[index].ItemArray[2].ToString(),
                            SortCodeMICR = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                            SANMICR = ds.Tables[0].Rows[index].ItemArray[4].ToString(),
                            TransCodeMICR = ds.Tables[0].Rows[index].ItemArray[5].ToString(),
                            ChequeNoNI = ds.Tables[0].Rows[index].ItemArray[6].ToString(),
                            SortCodeNI = ds.Tables[0].Rows[index].ItemArray[7].ToString(),
                            SANNI = ds.Tables[0].Rows[index].ItemArray[8].ToString(),
                            TransCodeNI = ds.Tables[0].Rows[index].ItemArray[9].ToString(),
                            ChequeNoPara = ds.Tables[0].Rows[index].ItemArray[10].ToString(),
                            SortCodePara = ds.Tables[0].Rows[index].ItemArray[11].ToString(),
                            SANPara = ds.Tables[0].Rows[index].ItemArray[12].ToString(),
                            TransCodePara = ds.Tables[0].Rows[index].ItemArray[13].ToString(),
                            //FrontGreyImagePath = imgpth + "/" + tempstr + "/Code-" + Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[0]) + ".jpg",
                            MICRRepairStatus = ds.Tables[0].Rows[index].ItemArray[14].ToString(),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[1].ToString(),
                            RawDataId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[16].ToString()),
                            CustomerId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[17].ToString()),
                            DomainId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[18].ToString()),
                            ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[19].ToString()),
                        };
                        //ViewBag.cnt = true;
                        objectlst.Add(def);
                        count = count - 1;
                        index = index + 1;
                    }

                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                    return View(objectlst);
                }
                else
                    return RedirectToAction("IWIndex", "Home", new { id = 1 });//return RedirectToAction("DeSelection", new { id = 1 });
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWDataEntry HttpGet MICR- " + innerExcp });
            }
        }
        [HttpPost]
        public ActionResult OWMICR(List<string> lst = null, bool snd = false, string img = null, string fulimg = null, string btnClose = "default", List<Int64> idlst = null)
        {

            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["DE"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            //IWAmountTmpProcess jt;
            int uid = (int)Session["uid"];//That will be Session value.
            try
            {
                int ttcnt = 0;
                if (lst != null)
                    ttcnt = lst.Count() / 23;
                int stu;

                if (btnClose == "Close" && lst != null)
                    ttcnt = ttcnt + 1;

                if (ttcnt == 1)
                {
                    for (int i = 0; i < ttcnt; i++)
                    {
                        // jt = new IWAmountTmpProcess();
                        //if (Convert.ToBoolean(lst[5]) == true)
                        //    stu = 1;
                        //else
                        //    stu = 0;
                        Int64 id = Convert.ToInt64(lst[0]);
                        OWpro.UpdateOWChequeMICR(id, Convert.ToInt64(lst[19]), uid, lst[1].ToString(), lst[2].ToString(), lst[3].ToString(), lst[4].ToString(), Convert.ToInt16(lst[5]),
                            "A", @Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt16(lst[20]),
                            Convert.ToInt32(lst[21]), Convert.ToInt32(lst[22]), 0);
                        // lst.RemoveRange(0, 19);
                        for (int k = 0; k < idlst.Count; k++)
                        {
                            if (idlst[k] == id)
                                idlst.RemoveAt(k);
                        }

                    }
                }
                else
                {
                    for (int i = 0; i < ttcnt - 1; i++)
                    {
                        // jt = new IWAmountTmpProcess();
                        //if (Convert.ToBoolean(lst[5]) == true)
                        //    stu = 1;
                        //else
                        //    stu = 0;
                        Int64 id = Convert.ToInt64(lst[0]);
                        OWpro.UpdateOWChequeMICR(id, Convert.ToInt64(lst[19]), uid, lst[1].ToString(), lst[2].ToString(), lst[3].ToString(), lst[4].ToString(), Convert.ToInt16(lst[5]),
                           "A", @Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt16(lst[20]),
                           Convert.ToInt32(lst[21]), Convert.ToInt32(lst[22]), 0);

                        for (int k = 0; k < idlst.Count; k++)
                        {
                            if (idlst[k] == id)
                                idlst.RemoveAt(k);

                            //if (k < idlst.Count)
                            //{
                            //    if (idlst[k] == id)
                            //        idlst.RemoveAt(k);
                            //}
                        }
                        lst.RemoveRange(0, 23);
                    }
                }

                //---------------------------IF Close button called--------------------------------//
                if (btnClose == "Close")
                {
                    @Session["glob"] = true;
                    for (int p = 0; p < idlst.Count; p++)
                    {
                        OWpro.OWUnlockRecords(idlst[p], "OWMICR");
                    }
                    return Json(false);
                }
                //------------------Select next Pending Record------------------
                //------------------------------Calling Proc for selecting data-----------------------

                // Procommand = new System.Data.Sql.("{ call SelectForDwld('"+ date.ToString("yyyy-MM-dd")+"')}",con);

                SqlDataAdapter adp = new SqlDataAdapter("OWSelectChequeMICR", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();
                //----------------Added on 17-05-2017---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                //-------------Added on 18-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);


                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<ChequeMICRCapture>();
                ChequeMICRCapture def;
                ArrayList ids = new ArrayList();
                bool checkid = false;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //--------------Checking Last record and list record are same------
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        if (Convert.ToInt64(lst[0]) == Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]))
                        {
                            //if (Convert.ToBoolean(lst[5]) == true)
                            //    stu = 1;
                            //else
                            //    stu = 0;
                            Int64 id = Convert.ToInt64(lst[0]);
                            OWpro.UpdateOWChequeMICR(id, Convert.ToInt64(lst[19]), uid, lst[1].ToString(), lst[2].ToString(), lst[3].ToString(), lst[4].ToString(), Convert.ToInt16(lst[5]),
                            "A", @Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt16(lst[20]),
                            Convert.ToInt32(lst[21]), Convert.ToInt32(lst[22]), 0);
                            goto Notfnd;
                        }

                    }
                    //---------------Adding last record of list-----------------//
                    def = new ChequeMICRCapture
                    {
                        Id = Convert.ToInt64(lst[0]),
                        FrontGreyImagePath = img,
                        Status = Convert.ToByte(lst[5]),
                        ChequeNoFinal = lst[1].ToString(),
                        SortCodeFinal = lst[2].ToString(),
                        SANFinal = lst[3].ToString(),
                        TransCodeFinal = lst[4].ToString(),
                        ChequeNoMICR = lst[6].ToString(),
                        SortCodeMICR = lst[7].ToString(),
                        SANMICR = lst[8].ToString(),
                        TransCodeMICR = lst[9].ToString(),
                        ChequeNoNI = lst[10].ToString(),
                        SortCodeNI = lst[11].ToString(),
                        SANNI = lst[12].ToString(),
                        TransCodeNI = lst[13].ToString(),
                        ChequeNoPara = lst[14].ToString(),
                        SortCodePara = lst[15].ToString(),
                        SANPara = lst[16].ToString(),
                        TransCodePara = lst[17].ToString(),
                        MICRRepairStatus = lst[18].ToString(),
                        RawDataId = Convert.ToInt64(lst[19]),
                        CustomerId = Convert.ToInt16(lst[20]),
                        DomainId = Convert.ToInt32(lst[21]),
                        ScanningNodeId = Convert.ToInt32(lst[22]),
                        FrontTiffImagePath = fulimg,
                        //-------------------------------//
                    };
                    objectlst.Add(def);
                    ids.Add(def.Id);

                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;

                    string[] ddt = new string[0];
                    String tempstr = Session["SnipDate"].ToString();// "04.06.2016";//String.Concat(imgpth + Session["SnipDate"].ToString());
                    ddt = tempstr.Split('/');
                    tempstr = ddt[0] + '.' + ddt[1] + '.' + ddt[2];

                    while (count > 0)
                    {
                        for (int i = 0; i < ids.Count; i++)
                        {
                            // tempId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]);
                            if (Convert.ToInt64(ids[i]) == Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]))
                            {
                                checkid = true;
                                break;
                            }
                        }
                        if (checkid == false)
                        {
                            def = new ChequeMICRCapture
                            {
                                Id = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),
                                //-----snippet path--web config-----procdate--------------------------------------CustomerID-------------------------NodeID--------------------------------BatchNo------------------------------------------BranchCode----------
                                FrontGreyImagePath = imgpth + "/" + tempstr + "/" + tempstr + "_" + ds.Tables[0].Rows[index].ItemArray[17] + "_" + ds.Tables[0].Rows[index].ItemArray[19] + "_" + ds.Tables[0].Rows[index].ItemArray[21] + "_" + ds.Tables[0].Rows[index].ItemArray[20] + "_" + ds.Tables[0].Rows[index].ItemArray[22] + "_MICR.jpg",//ds.Tables[0].Rows[0].ItemArray[1].ToString(),
                                ChequeNoMICR = ds.Tables[0].Rows[index].ItemArray[2].ToString(),
                                SortCodeMICR = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                                SANMICR = ds.Tables[0].Rows[index].ItemArray[4].ToString(),
                                TransCodeMICR = ds.Tables[0].Rows[index].ItemArray[5].ToString(),
                                ChequeNoNI = ds.Tables[0].Rows[index].ItemArray[6].ToString(),
                                SortCodeNI = ds.Tables[0].Rows[index].ItemArray[7].ToString(),
                                SANNI = ds.Tables[0].Rows[index].ItemArray[8].ToString(),
                                TransCodeNI = ds.Tables[0].Rows[index].ItemArray[9].ToString(),
                                ChequeNoPara = ds.Tables[0].Rows[index].ItemArray[10].ToString(),
                                SortCodePara = ds.Tables[0].Rows[index].ItemArray[11].ToString(),
                                SANPara = ds.Tables[0].Rows[index].ItemArray[12].ToString(),
                                TransCodePara = ds.Tables[0].Rows[index].ItemArray[13].ToString(),
                                //FrontGreyImagePath = imgpth + "/" + tempstr + "/Code-" + Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[0]) + ".jpg",
                                MICRRepairStatus = ds.Tables[0].Rows[index].ItemArray[14].ToString(),
                                FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[1].ToString(),
                                RawDataId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[16].ToString()),
                                CustomerId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[17].ToString()),
                                DomainId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[18].ToString()),
                                ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[19].ToString()),
                            };
                            ids.Add(def.Id);
                            objectlst.Add(def);
                        }
                        checkid = false;
                        count = count - 1;
                        index = index + 1;
                    }
                    ViewBag.cnt = true;
                    return Json(objectlst);
                }
                else
                {
                    ViewBag.cnt = false;
                    goto Notfnd;
                }
            Notfnd:
                ViewBag.cnt = false;
                @Session["glob"] = true;
                return Json(false);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWDataEntry HttpPost MICR- " + innerExcp });
            }
        }
        [HttpPost]
        public JsonResult GetCBSDtls(string ac = null)
        {
            cbstetails model = new cbstetails();
            //bool result = false;
            string strCbsClientsDetls = null;
            string strJoinHldrs = null;
            bool acstatus = false;
            try
            {

                if (ac != null)
                {
                    //--------------------------------------For Local DB--------------------
                    if (Session["GetAccountDetails "].ToString().ToUpper() == "L")
                    {
                        model = (from c in af.ACDetails
                                 where c.Ac == ac
                                 select new cbstetails
                                 {
                                     cbsdls = c.Cbsdtls,
                                     JoinHldrs = c.JoinHldrs
                                 }
                                ).SingleOrDefault();
                    }
                    else if (Session["GetAccountDetails "].ToString().ToUpper() == "C")
                    {
                        //---------For CBS Bank----------------
                        OWpro.OWGetCBSAccInfoWithOutUpdate(ac, ref strCbsClientsDetls, ref strJoinHldrs);
                        model.cbsdls = strCbsClientsDetls;
                        model.JoinHldrs = strJoinHldrs;
                        //---------------------------------
                    }

                    if (model != null && model.cbsdls != null)
                    {
                        if (model.cbsdls.Split('|').ElementAt(1) == "S")
                        {
                            acstatus = true;
                            if (ac.Length == 16)
                            {
                                if (Session["CreditCardValidationReq"].ToString() == "1")
                                {
                                    if (model.cbsdls.Length < 5)
                                    {
                                        strCbsClientsDetls = "|S|CREDIT CARD-BRANCH COLLECTION A/C|CRC1| || |0|N|N|N|OAB|O";
                                        strJoinHldrs = "|S|CREDIT CARD-BRANCH COLLECTION A/C|CRC1|VALID CARD || |0|N|N|N|OAB|O";
                                    }
                                    else
                                    {
                                        strCbsClientsDetls = model.cbsdls;
                                        strJoinHldrs = model.JoinHldrs;
                                    }
                                }
                            }
                            else
                            {
                                strCbsClientsDetls = model.cbsdls;
                                strJoinHldrs = model.JoinHldrs;
                            }

                            //if (model.cbsdls.Split('|').ElementAt(5).Trim() != "")
                            //{
                            //    string MOP = af.MOPCodeMasters.Find(model.cbsdls.Split('|').ElementAt(5)).Description;
                            //    model.MOP = MOP != null ? MOP : "";
                            //}
                            //else
                            //{
                            //    model.MOP = "";
                            //}
                            //if (model.cbsdls.Split('|').ElementAt(6).Trim() != "")
                            //{
                            //    string AccountStatus = af.AccStatusCodeMasters.Find(model.cbsdls.Split('|').ElementAt(6)).Description;
                            //    model.AccountStatus = AccountStatus != null ? AccountStatus : "";
                            //}
                            //else
                            //{
                            //    model.AccountStatus = "";
                            //}

                            //if (model.cbsdls.Split('|').ElementAt(12).Trim() != "")
                            //{
                            //    string AccountOwnership = af.AccOwnershipCodeMasters.Find(model.cbsdls.Split('|').ElementAt(12).ToString()).Description;
                            //    model.AccountOwnership = AccountOwnership != null ? AccountOwnership : "";
                            //}
                            //else
                            //{
                            //    model.AccountOwnership = "";
                            //}

                            //List<string> ar = new List<string>();
                            //ar.Add(model.cbsdls.Split('|').ElementAt(2).ToString());

                            //for (int i = 3; i < model.JoinHldrs.Split('|').Count() - 1; i++)
                            //{

                            //    ar.Add(model.JoinHldrs.Split('|').ElementAt(i).ToString());
                            //}
                            // model.PayeeName = ar;
                        }
                        else if (model.cbsdls.Split('|').ElementAt(1) == "F")
                        {
                            if (ac.Length == 16)
                            {
                                if (Session["CreditCardValidationReq"].ToString() == "1")
                                {
                                    acstatus = true;
                                    //strCbsClientsDetls = "|F|";//COLLECTION POOL A/C (FINNONE)|LIAB| || |99999999999999.99|N|N|N|OAB|O
                                    //strJoinHldrs = "|F|";
                                    if (ac != "9999999999999999")
                                    {

                                        if (model.cbsdls.Length < 5)
                                        {
                                            strCbsClientsDetls = "|S|CREDIT CARD-BRANCH COLLECTION A/C|CRC1| || |0|N|N|N|OAB|O";
                                            strJoinHldrs = "|F|CREDIT CARD-BRANCH COLLECTION A/C|CRC1|INVALID CARD || |0|N|N|N|OAB|O";
                                        }
                                        else
                                        {
                                            strCbsClientsDetls = model.cbsdls;
                                            strJoinHldrs = model.JoinHldrs;
                                        }
                                    }
                                    else
                                    {
                                        strCbsClientsDetls = "|F|Account does not exist";
                                        strJoinHldrs = "|F|";
                                    }

                                }

                            }
                            else
                            {
                                acstatus = false;
                                strCbsClientsDetls = "|F|Account does not exist";
                                strJoinHldrs = "|F|";
                            }

                        }

                    }
                    else
                    {
                        acstatus = false;
                        strCbsClientsDetls = "|F|Account does not exist";
                        //strCbsClientsDetls = "234234";
                        //strJoinHldrs = "324324";
                    }

                }
                var result = new { acstatus, strCbsClientsDetls, strJoinHldrs };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                string ServerPath = "";
                string filename = "";
                string fileNameWithPath = "";

                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                ErrorDisplay err = new ErrorDisplay();

                ServerPath = Server.MapPath("~/Logs/");
                if (System.IO.Directory.Exists(ServerPath) == false)
                {
                    System.IO.Directory.CreateDirectory(ServerPath);
                }
                filename = DateTime.Now.ToString("ddMMyyyy") + "Logs.txt";
                fileNameWithPath = ServerPath + filename;
                StreamWriter str = new StreamWriter(fileNameWithPath, true, System.Text.Encoding.Default);
                //  str.WriteLine("hello");
                str.WriteLine(DateTime.Now.ToShortTimeString() + ": Exception: " + message);
                str.WriteLine(DateTime.Now.ToShortTimeString() + ": StackTrace: " + innerExcp);
                str.Close();
                //return RedirectToAction("Error", "Error", new { msg = message, popmsg = "OWDataEntry HttpGet ChqDate- " + innerExcp });
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
