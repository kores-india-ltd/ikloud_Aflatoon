using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ikloud_Aflatoon.Infrastructure;
using System.Collections;
using ikloud_Aflatoon.Models;
using System.Drawing;

namespace ikloud_Aflatoon.Controllers
{
    public class IWDataEntryController : Controller
    {
        //
        // GET: /IWDataEntry/
        AflatoonEntities af = new AflatoonEntities();
        IWProcDataContext iwpro = new IWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
        string imgpth = System.Configuration.ConfigurationManager.AppSettings["snippath"].ToString();
        List<Int64> listId = new List<Int64>();

        //public IWDataEntryController()
        //{
        //    imgpth = imgpth.Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]);
        //}
        [HttpPost]
        public ActionResult Index(int id = 0)
        {
            string slcttype = Request.Form["selctDE"];
            if (slcttype == "DebtAcct")
            {
                return RedirectToAction("DebtAc");
            }
            else if (slcttype == "Amount")
            {
                return RedirectToAction("IWAmount");
            }
            else if (slcttype == "Date")
            {
                return RedirectToAction("IWDate");
            }
            else if (slcttype == "Payee")
            {
                return RedirectToAction("IWPayee");
            }
            else if (slcttype == "IWMICR")
            {
                return RedirectToAction("IWMICR");
            }
            return View();
        }
        public ActionResult DeSelection(int id = 0)
        {
            if (id == 1)
                ViewBag.msg = "No data found!!";
            return View();
        }

        public ActionResult DebtAc()
        {

            //------------------------------Calling Proc for selecting data-----------------------
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
            // Procommand = new System.Data.Sql.("{ call SelectForDwld('"+ date.ToString("yyyy-MM-dd")+"')}",con);
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
                SqlDataAdapter adp = new SqlDataAdapter("IWSelectDebtAccount", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();
                //--------------------Customer Selection---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);

                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<IWDebtAccountTmpProcess>();
                IWDebtAccountTmpProcess def;
                imgpth = imgpth.Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]);

                string[] ddt = new string[0];
                String tempstr = Session["SnipDate"].ToString();// "04.06.2016";//String.Concat(imgpth + Session["SnipDate"].ToString());
                string imgefileID = "";
                string[] imgefilearray;
                string localimagepath = "";
                ddt = tempstr.Split('/');
                tempstr = ddt[0] + '.' + ddt[1] + '.' + ddt[2];
                // imgpth = imgpth + tempstr + "//DbtAccNo-"; chnange on 07-06-2017
                //imgpth = imgpth + tempstr + "//Snip//" + tempstr + "//DbtAccNo-";


                if (ds.Tables[0].Rows.Count > 0)
                {
                    //=
                    localimagepath = imgpth;
                    imgefileID = ds.Tables[0].Rows[0].ItemArray[1].ToString().Substring(ds.Tables[0].Rows[0].ItemArray[1].ToString().IndexOf(tempstr));
                    imgefilearray = imgefileID.Split('\\');
                    imgefileID = imgefilearray[1];

                    localimagepath = localimagepath + tempstr + "//" + imgefileID + "//Snip//DbtAccNo-";

                    def = new IWDebtAccountTmpProcess
                    {
                        ID = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]),
                        FrontGreyImagePath = localimagepath + Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]) + ".jpg",//ds.Tables[0].Rows[0].ItemArray[1].ToString(),
                        OCRDebtAccountNo = ds.Tables[0].Rows[0].ItemArray[2].ToString(),
                        CBSDebtAccountNo = ds.Tables[0].Rows[0].ItemArray[3].ToString(),
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[1].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                    };
                    objectlst.Add(def);
                    //------------------------END------------------------//
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        localimagepath = imgpth;

                        imgefileID = ds.Tables[0].Rows[index].ItemArray[1].ToString().Substring(ds.Tables[0].Rows[index].ItemArray[1].ToString().IndexOf(tempstr));
                        imgefilearray = imgefileID.Split('\\');
                        imgefileID = imgefilearray[1];

                        localimagepath = localimagepath + tempstr + "//" + imgefileID + "//Snip//DbtAccNo-";

                        def = new IWDebtAccountTmpProcess
                        {
                            ID = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),
                            FrontGreyImagePath = localimagepath + Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]) + ".jpg",//ds.Tables[0].Rows[index].ItemArray[1].ToString(),
                            OCRDebtAccountNo = ds.Tables[0].Rows[index].ItemArray[2].ToString(),
                            CBSDebtAccountNo = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[1].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                        };
                        //ViewBag.cnt = true;
                        objectlst.Add(def);
                        count = count - 1;
                        index = index + 1;
                    }

                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                    return View("DebtAcNo", objectlst);
                }
                else
                    // return RedirectToAction("DeSelection", new { id = 1 });
                    return RedirectToAction("IWIndex", "Home", new { id = 1 });
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

        [HttpPost]
        public ActionResult DebtAc(List<string> lst, bool snd, string img, string fulimg = null, string btnClose = "default", List<Int64> idlst = null)
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
            string imgefileID = "";
            string[] imgefilearray;
            string localimagepath = "";

            int uid = (int)Session["uid"];//That will be Session value.
            int ttcnt = 0;
            Int64 id = 0;
            try
            {
                if (lst != null)
                    ttcnt = lst.Count() / 5;

                int stu = 0;

                if (btnClose == "Close" && lst != null)
                    ttcnt = ttcnt + 1;

                if (ttcnt == 1)
                {
                    for (int i = 0; i < ttcnt; i++)
                    {
                        if (Convert.ToBoolean(lst[2]) == true)
                            stu = 1;
                        else
                            stu = 0;
                        id = Convert.ToInt64(lst[0]);
                        iwpro.UpdateIWDebtAccountTmpProcess(id, uid, lst[1].ToString(), stu, @Session["LoginID"].ToString());

                        for (int k = 0; k < idlst.Count; k++)
                        {
                            if (idlst[k] == id)
                                idlst.RemoveAt(k);
                        }
                    }
                }
                else
                {
                    id = 0;
                    for (int i = 0; i < ttcnt - 1; i++)
                    {
                        if (Convert.ToBoolean(lst[2]) == true)
                            stu = 1;
                        else
                            stu = 0;
                        id = Convert.ToInt64(lst[0]);
                        iwpro.UpdateIWDebtAccountTmpProcess(id, uid, lst[1].ToString(), stu, @Session["LoginID"].ToString());

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
                        lst.RemoveRange(0, 5);
                    }
                }

               

                //---------------------------IF Close button called--------------------------------//
                if (btnClose == "Close")
                {
                    @Session["glob"] = true;
                    for (int p = 0; p < idlst.Count; p++)
                    {
                        iwpro.UnlockRecords(idlst[p], "Dbt", 0, null, null, 0);
                    }
                    return Json(false);
                }
                //------------------Select next Pending Record------------------
                //------------------------------Calling Proc for selecting data-----------------------
                // SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
                // Procommand = new System.Data.Sql.("{ call SelectForDwld('"+ date.ToString("yyyy-MM-dd")+"')}",con);

                SqlDataAdapter adp = new SqlDataAdapter("IWSelectDebtAccount", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                //--------------------Customer Selection---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);


                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<IWDebtAccountTmpProcess>();
                IWDebtAccountTmpProcess def;
                ArrayList ids = new ArrayList();
                bool checkid = false;
                imgpth = imgpth.Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //--------------Checking Last record and list record are same------
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        if (Convert.ToInt64(lst[0]) == Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]))
                        {
                            if (Convert.ToBoolean(lst[2]) == true)
                                stu = 1;
                            else
                                stu = 0;
                            id = Convert.ToInt64(lst[0]);
                            iwpro.UpdateIWDebtAccountTmpProcess(id, uid, lst[1].ToString(), stu, @Session["LoginID"].ToString());
                            goto Notfnd;
                        }

                    }
                    //---------------Adding last record of list-----------------//
                    def = new IWDebtAccountTmpProcess
                    {
                        ID = Convert.ToInt64(lst[0]),
                        FrontGreyImagePath = img,
                        EntryDebtAccountNo = lst[1].ToString(),
                        stts = Convert.ToBoolean(lst[2]),
                        OCRDebtAccountNo = lst[3].ToString(),
                        CBSDebtAccountNo = lst[4].ToString(),
                        FrontTiffImagePath = fulimg,
                    };
                    objectlst.Add(def);
                    ids.Add(def.ID);

                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    string[] ddt = new string[0];
                    String tempstr = Session["SnipDate"].ToString();// "04.06.2016";//String.Concat(imgpth + Session["SnipDate"].ToString());
                    ddt = tempstr.Split('/');
                    tempstr = ddt[0] + '.' + ddt[1] + '.' + ddt[2];
                   // imgpth = imgpth + tempstr + "//Snip//" + tempstr + "//DbtAccNo-";
                  
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
                            localimagepath = imgpth;
                            imgefileID = ds.Tables[0].Rows[index].ItemArray[1].ToString().Substring(ds.Tables[0].Rows[index].ItemArray[1].ToString().IndexOf(tempstr));
                            imgefilearray = imgefileID.Split('\\');
                            imgefileID = imgefilearray[1];

                            localimagepath = localimagepath + tempstr + "//" + imgefileID + "//Snip//DbtAccNo-";

                            def = new IWDebtAccountTmpProcess
                            {
                                ID = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),

                                FrontGreyImagePath = localimagepath + Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]) + ".jpg",//ds.Tables[0].Rows[index].ItemArray[1].ToString(),
                                OCRDebtAccountNo = ds.Tables[0].Rows[index].ItemArray[2].ToString(),
                                CBSDebtAccountNo = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                                FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[1].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                            };
                            ids.Add(def.ID);
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

                //ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                //er.ErrorMessage = e.InnerException.Message;
                //return View("Error", er);
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }
        //------------------------------IWAmount DE Entry----------------
        public ActionResult IWAmount()
        {

            //------------------------------Calling Proc for selecting data-----------------------
            //  SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
            // Procommand = new System.Data.Sql.("{ call SelectForDwld('"+ date.ToString("yyyy-MM-dd")+"')}",con);
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

                SqlDataAdapter adp = new SqlDataAdapter("IWSelectAmount", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                //--------------------Customer Selection---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);


                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<IWAmountTmpProcess>();
                IWAmountTmpProcess def;
                string[] ddt = new string[0];
                string imgefileID = "";
                string[] imgefilearray;
                string localimagepath = "";

                String tempstr = Session["SnipDate"].ToString();// "04.06.2016";//String.Concat(imgpth + Session["SnipDate"].ToString());
                ddt = tempstr.Split('/');
                tempstr = ddt[0] + '.' + ddt[1] + '.' + ddt[2];
                imgpth = imgpth.Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]);
                //imgpth = imgpth + tempstr + "//Snip//" + tempstr + "//Amt-";
                //tempstr = tempstr.Substring(1, (tempstr.Length - 2));
                //ds.Tables[0].Rows[0].ItemArray[1].ToString().Substring(ds.Tables[0].Rows[0].ItemArray[1].ToString().IndexOf("iKloudIwImages\\"),ds.Tables[0].Rows[0].ItemArray[1].ToString().IndexOf("iKloudIwImages\\")) 
                if (ds.Tables[0].Rows.Count > 0)
                {
                    localimagepath = imgpth;
                    imgefileID = ds.Tables[0].Rows[0].ItemArray[1].ToString().Substring(ds.Tables[0].Rows[0].ItemArray[1].ToString().IndexOf(tempstr));
                    imgefilearray = imgefileID.Split('\\');
                    imgefileID = imgefilearray[1];

                    localimagepath = localimagepath + tempstr + "//" + imgefileID + "//Snip//Amt-";

                    def = new IWAmountTmpProcess
                    {
                        ID = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]),

                        FrontGreyImagePath = localimagepath + Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]) + ".jpg",//ds.Tables[0].Rows[0].ItemArray[1].ToString(),
                        Amount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[2]),
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[1].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                        // EntryAmount = if(ds.Tables[0].Rows[0].ItemArray[3])!=null)
                    };
                    objectlst.Add(def);

                    //------------------------END------------------------//

                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        localimagepath = imgpth;
                        imgefileID = ds.Tables[0].Rows[index].ItemArray[1].ToString().Substring(ds.Tables[0].Rows[index].ItemArray[1].ToString().IndexOf(tempstr));
                        imgefilearray = imgefileID.Split('\\');
                        imgefileID = imgefilearray[1];

                        localimagepath = localimagepath + tempstr + "//" + imgefileID + "//Snip//Amt-";

                        def = new IWAmountTmpProcess
                        {
                            ID = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),

                            FrontGreyImagePath = localimagepath + Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]) + ".jpg",//ds.Tables[0].Rows[index].ItemArray[1].ToString(),
                            Amount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[2]),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[1].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                            //  EntryAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[3])
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

                //ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                //er.ErrorMessage = e.InnerException.Message;
                //return View("Error", er);
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }
        [HttpPost]
        public ActionResult IWAmount(List<string> lst, bool snd, string img, string fulimg = null, string btnClose = "default", List<Int64> idlst = null)
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
            
            try
            {
                if (lst != null)
                    ttcnt = lst.Count() / 4;

                int stts = 0;

                if (btnClose == "Close" && lst != null)
                    ttcnt = ttcnt + 1;

                if (ttcnt == 1)
                {
                    for (int i = 0; i < ttcnt; i++)
                    {

                        if (Convert.ToBoolean(lst[2]) == true)
                            stts = 1;
                        else
                            stts = 0;
                        Int64 id = Convert.ToInt64(lst[0]);
                        iwpro.UpdateIWAmountTmpProcess(id, uid, Convert.ToDouble(lst[1]), stts, @Session["LoginID"].ToString());

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

                        if (Convert.ToBoolean(lst[2]) == true)
                            stts = 1;
                        else
                            stts = 0;
                        Int64 id = Convert.ToInt64(lst[0]);
                        iwpro.UpdateIWAmountTmpProcess(id, uid, Convert.ToDouble(lst[1]), stts, @Session["LoginID"].ToString());

                        for (int k = 0; k < idlst.Count; k++)
                        {
                            if (idlst[k] == id)
                                idlst.RemoveAt(k);

                        }
                        lst.RemoveRange(0, 4);

                    }
                }

                //---------------------------IF Close button called--------------------------------//
                if (btnClose == "Close")
                {
                    @Session["glob"] = true;
                    for (int p = 0; p < idlst.Count; p++)
                    {
                        iwpro.UnlockRecords(idlst[p], "IWAmt", 0, null, null, 0);
                    }
                    return Json(false);
                }
                //------------------Select next Pending Record------------------
                //------------------------------Calling Proc for selecting data-----------------------

                // Procommand = new System.Data.Sql.("{ call SelectForDwld('"+ date.ToString("yyyy-MM-dd")+"')}",con);

                SqlDataAdapter adp = new SqlDataAdapter("IWSelectAmount", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                //--------------------Customer Selection---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);


                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<IWAmountTmpProcess>();
                IWAmountTmpProcess def;
                ArrayList ids = new ArrayList();
                bool checkid = false;
                string imgefileID = "";
                string[] imgefilearray;
                string localimagepath = "";

                imgpth = imgpth.Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]);
               
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //--------------Checking Last record and list record are same------
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        if (Convert.ToInt64(lst[0]) == Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]))
                        {
                            if (Convert.ToBoolean(lst[2]) == true)
                                stts = 1;
                            else
                                stts = 0;
                            Int64 id = Convert.ToInt64(lst[0]);
                            iwpro.UpdateIWAmountTmpProcess(id, uid, Convert.ToDouble(lst[1]), stts, @Session["LoginID"].ToString());
                            goto Notfnd;
                        }

                    }
                    //---------------Adding last record of list-----------------//
                    def = new IWAmountTmpProcess
                    {
                        ID = Convert.ToInt64(lst[0]),
                        FrontGreyImagePath = img,
                        sttsamt = Convert.ToBoolean(lst[2]),
                        EntryAmount = Convert.ToDecimal(lst[1]),
                        Amount = Convert.ToDecimal(lst[3]),
                        FrontTiffImagePath = fulimg,
                    };
                    objectlst.Add(def);
                    ids.Add(def.ID);

                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;

                    string[] ddt = new string[0];
                    String tempstr = Session["SnipDate"].ToString();// "04.06.2016";//String.Concat(imgpth + Session["SnipDate"].ToString());
                    ddt = tempstr.Split('/');
                    tempstr = ddt[0] + '.' + ddt[1] + '.' + ddt[2];
                   // imgpth = imgpth + tempstr + "//Snip//" + tempstr + "//Amt-";

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

                            localimagepath = imgpth;
                            imgefileID = ds.Tables[0].Rows[index].ItemArray[1].ToString().Substring(ds.Tables[0].Rows[index].ItemArray[1].ToString().IndexOf(tempstr));
                            imgefilearray = imgefileID.Split('\\');
                            imgefileID = imgefilearray[1];

                            localimagepath = localimagepath + tempstr + "//" + imgefileID + "//Snip//Amt-";


                            def = new IWAmountTmpProcess
                            {
                                ID = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),

                                FrontGreyImagePath = localimagepath + Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]) + ".jpg",//ds.Tables[0].Rows[index].ItemArray[1].ToString(),
                                Amount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[2]),
                                FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[1].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                                // EntryAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[3])
                            };
                            ids.Add(def.ID);
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

                //ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                //er.ErrorMessage = e.InnerException.Message;
                //return View("Error", er);
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }
        //------------------------------For Date---------------------------
        public ActionResult IWDate()
        {

            //------------------------------Calling Proc for selecting data-----------------------
            //  SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
            // Procommand = new System.Data.Sql.("{ call SelectForDwld('"+ date.ToString("yyyy-MM-dd")+"')}",con);
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

                SqlDataAdapter adp = new SqlDataAdapter("IWSelectDate", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                //--------------------Customer Selection---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);


                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<IWDateTmpProcess>();
                IWDateTmpProcess def;

                string[] ddt = new string[0];
                String tempstr = Session["SnipDate"].ToString();// "04.06.2016";//String.Concat(imgpth + Session["SnipDate"].ToString());
                string imgefileID = "";
                string[] imgefilearray;
                string localimagepath = "";
                ddt = tempstr.Split('/');
                tempstr = ddt[0] + '.' + ddt[1] + '.' + ddt[2];
                imgpth = imgpth.Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]);
               // imgpth = imgpth + tempstr + "//Snip//" + tempstr + "//Date-";

                if (ds.Tables[0].Rows.Count > 0)
                {
                    localimagepath = imgpth;
                    imgefileID = ds.Tables[0].Rows[0].ItemArray[1].ToString().Substring(ds.Tables[0].Rows[0].ItemArray[1].ToString().IndexOf(tempstr));
                    imgefilearray = imgefileID.Split('\\');
                    imgefileID = imgefilearray[1];

                    localimagepath = localimagepath + tempstr + "//" + imgefileID + "//Snip//Date-";

                    def = new IWDateTmpProcess
                    {
                        ID = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]),

                        FrontGreyImagePath = localimagepath + Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]) + ".jpg",
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[1].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                    };
                    objectlst.Add(def);
                    //------------------------END------------------------//
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        localimagepath = imgpth;
                        imgefileID = ds.Tables[0].Rows[index].ItemArray[1].ToString().Substring(ds.Tables[0].Rows[index].ItemArray[1].ToString().IndexOf(tempstr));
                        imgefilearray = imgefileID.Split('\\');
                        imgefileID = imgefilearray[1];

                        localimagepath = localimagepath + tempstr + "//" + imgefileID + "//Snip//Date-";

                        def = new IWDateTmpProcess
                        {
                            ID = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),

                            FrontGreyImagePath = localimagepath + Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]) + ".jpg",
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[1].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
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

                //ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                //er.ErrorMessage = e.InnerException.Message;
                //return View("Error", er);
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }


        [HttpPost]
        public ActionResult IWDate(List<string> lst, bool snd, string img, string fulimg = null, string btnClose = "default", List<Int64> idlst = null)
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
            int ttcnt = 0;
            int stts = 0;
            Int64 id = 0;
            try
            {
                if (lst != null)
                    ttcnt = lst.Count() / 3;

                if (btnClose == "Close" && lst != null)
                    ttcnt = ttcnt + 1;

                if (ttcnt == 1)
                {
                    for (int i = 0; i < ttcnt; i++)
                    {
                        // jt = new IWAmountTmpProcess();
                        if (Convert.ToBoolean(lst[2]) == true)
                            stts = 1;
                        else
                            stts = 0;

                        id = Convert.ToInt64(lst[0]);

                        iwpro.UpdateIWDateTmpProcess(id, uid, lst[1].ToString(), stts, @Session["LoginID"].ToString());
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
                        if (Convert.ToBoolean(lst[2]) == true)
                            stts = 1;
                        else
                            stts = 0;

                        id = Convert.ToInt64(lst[0]);
                        iwpro.UpdateIWDateTmpProcess(id, uid, lst[1].ToString(), stts, @Session["LoginID"].ToString());
                        for (int k = 0; k < idlst.Count; k++)
                        {
                            if (idlst[k] == id)
                                idlst.RemoveAt(k);

                        }
                        lst.RemoveRange(0, 3);
                    }
                }

                //---------------------------IF Close button called--------------------------------//
                if (btnClose == "Close")
                {
                    @Session["glob"] = true;
                    for (int p = 0; p < idlst.Count; p++)
                    {
                        iwpro.UnlockRecords(idlst[p], "Iwdate", 0, null, null, 0);
                    }
                    return Json(false);
                }
                // ViewBag.Secondcall = true;
                //------------------Select next Pending Record------------------
                //------------------------------Calling Proc for selecting data-----------------------

                // Procommand = new System.Data.Sql.("{ call SelectForDwld('"+ date.ToString("yyyy-MM-dd")+"')}",con);

                SqlDataAdapter adp = new SqlDataAdapter("IWSelectDate", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                //--------------------Customer Selection---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);


                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<IWDateTmpProcess>();
                IWDateTmpProcess def;
                ArrayList ids = new ArrayList();
                bool checkid = false;
                imgpth = imgpth.Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]);

                string imgefileID = "";
                string[] imgefilearray;
                string localimagepath = "";

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //--------------Checking Last record and list record are same------
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        if (Convert.ToInt64(lst[0]) == Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]))
                        {

                            if (Convert.ToBoolean(lst[2]) == true)
                                stts = 1;
                            else
                                stts = 0;

                            id = Convert.ToInt64(lst[0]);
                            iwpro.UpdateIWDateTmpProcess(id, uid, lst[1].ToString(), stts, @Session["LoginID"].ToString());
                            goto Notfnd;
                        }

                    }
                    //---------------Adding last record of list-----------------//
                    def = new IWDateTmpProcess
                    {
                        ID = Convert.ToInt64(lst[0]),
                        FrontGreyImagePath = img,
                        EntryDate = lst[1].ToString(),
                        sttsdtqc = Convert.ToBoolean(lst[2]),
                        FrontTiffImagePath = fulimg,

                    };
                    objectlst.Add(def);
                    ids.Add(def.ID);

                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;

                    string[] ddt = new string[0];
                    String tempstr = Session["SnipDate"].ToString();// "04.06.2016";//String.Concat(imgpth + Session["SnipDate"].ToString());
                    ddt = tempstr.Split('/');
                    tempstr = ddt[0] + '.' + ddt[1] + '.' + ddt[2];
                   // imgpth = imgpth + tempstr + "//Snip//" + tempstr + "//Date-";

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
                            localimagepath = imgpth;
                            imgefileID = ds.Tables[0].Rows[index].ItemArray[1].ToString().Substring(ds.Tables[0].Rows[index].ItemArray[1].ToString().IndexOf(tempstr));
                            imgefilearray = imgefileID.Split('\\');
                            imgefileID = imgefilearray[1];

                            localimagepath = localimagepath + tempstr + "//" + imgefileID + "//Snip//Date-";

                            def = new IWDateTmpProcess
                            {
                                ID = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),

                                FrontGreyImagePath = localimagepath + Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]) + ".jpg",
                                FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[1].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                            };
                            ids.Add(def.ID);
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

                //ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                //er.ErrorMessage = e.InnerException.Message;
                //return View("Error", er);
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }

        public ActionResult IWDateQC()
        {

            //------------------------------Calling Proc for selecting data-----------------------
            //  SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
            // Procommand = new System.Data.Sql.("{ call SelectForDwld('"+ date.ToString("yyyy-MM-dd")+"')}",con);
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["QC"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            try
            {

                //IWAmountTmpProcess jt;
                int uid = (int)Session["uid"];
                SqlDataAdapter adp = new SqlDataAdapter("IWSelectDateQC", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                //--------------------Customer Selection---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);


                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<IWDateTmpProcess>();
                IWDateTmpProcess def;

                string[] ddt = new string[0];
                String tempstr = Session["SnipDate"].ToString();// "04.06.2016";//String.Concat(imgpth + Session["SnipDate"].ToString());
                string imgefileID = "";
                string[] imgefilearray;
                string localimagepath = "";

                ddt = tempstr.Split('/');
                tempstr = ddt[0] + '.' + ddt[1] + '.' + ddt[2];
                imgpth = imgpth.Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]);
               // imgpth = imgpth + tempstr + "//Snip//" + tempstr + "//Date-";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    localimagepath = imgpth;
                    imgefileID = ds.Tables[0].Rows[0].ItemArray[3].ToString().Substring(ds.Tables[0].Rows[0].ItemArray[3].ToString().IndexOf(tempstr));
                    imgefilearray = imgefileID.Split('\\');
                    imgefileID = imgefilearray[1];

                    localimagepath = localimagepath + tempstr + "//" + imgefileID + "//Snip//Date-";

                    def = new IWDateTmpProcess
                    {
                        ID = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]),
                        FrontGreyImagePath = localimagepath + Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]) + ".jpg",
                        ICRDate = ds.Tables[0].Rows[0].ItemArray[1].ToString(),
                        EntryDate = ds.Tables[0].Rows[0].ItemArray[2].ToString(),
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[3].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                    };
                    objectlst.Add(def);
                    //------------------------END------------------------//
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        localimagepath = imgpth;
                        imgefileID = ds.Tables[0].Rows[index].ItemArray[3].ToString().Substring(ds.Tables[0].Rows[index].ItemArray[3].ToString().IndexOf(tempstr));
                        imgefilearray = imgefileID.Split('\\');
                        imgefileID = imgefilearray[1];

                        localimagepath = localimagepath + tempstr + "//" + imgefileID + "//Snip//Date-";

                        def = new IWDateTmpProcess
                        {
                            ID = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),
                            FrontGreyImagePath = localimagepath + Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]) + ".jpg",
                            ICRDate = ds.Tables[0].Rows[index].ItemArray[1].ToString(),
                            EntryDate = ds.Tables[0].Rows[index].ItemArray[2].ToString(),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[3].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
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

                //ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                //er.ErrorMessage = e.InnerException.Message;
                //return View("Error", er);
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }

        [HttpPost]
        public ActionResult IWDateQC(List<string> lst, bool snd, string img, string fulimg = null, string btnClose = "default", List<Int64> idlst = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["QC"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            //IWAmountTmpProcess jt;
            int uid = (int)Session["uid"];//That will be Session value.
            int ttcnt = 0;
            try
            {
                if (lst != null)
                    ttcnt = lst.Count() / 5;
                int stu;

                if (btnClose == "Close" && lst != null)
                    ttcnt = ttcnt + 1;

                if (ttcnt == 1)
                {
                    for (int i = 0; i < ttcnt; i++)
                    {
                        // jt = new IWAmountTmpProcess();
                        if (Convert.ToBoolean(lst[2]) == true)
                            stu = 1;
                        else
                            stu = 0;
                        Int64 id = Convert.ToInt64(lst[0]);
                        iwpro.UpdateIWDateQCTmpProcess(id, uid, lst[1].ToString(), stu, @Session["LoginID"].ToString());
                        // lst.RemoveRange(0, 5);
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
                    }
                }
                else
                {
                    for (int i = 0; i < ttcnt - 1; i++)
                    {
                        // jt = new IWAmountTmpProcess();
                        if (Convert.ToBoolean(lst[2]) == true)
                            stu = 1;
                        else
                            stu = 0;
                        Int64 id = Convert.ToInt64(lst[0]);
                        iwpro.UpdateIWDateQCTmpProcess(id, uid, lst[1].ToString(), stu, @Session["LoginID"].ToString());
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
                        lst.RemoveRange(0, 5);

                    }
                }

                //---------------------------IF Close button called--------------------------------//
                if (btnClose == "Close")
                {
                    @Session["glob"] = true;
                    for (int p = 0; p < idlst.Count; p++)
                    {
                        iwpro.UnlockRecords(idlst[p], "IWDtQC", 0, null, null, 0);
                    }
                    return Json(false);
                }
                //------------------Select next Pending Record------------------
                //------------------------------Calling Proc for selecting data-----------------------

                // Procommand = new System.Data.Sql.("{ call SelectForDwld('"+ date.ToString("yyyy-MM-dd")+"')}",con);

                SqlDataAdapter adp = new SqlDataAdapter("IWSelectDateQC", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                //--------------------Customer Selection---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);


                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<IWDateTmpProcess>();
                IWDateTmpProcess def;
                ArrayList ids = new ArrayList();
                bool checkid = false;
                imgpth = imgpth.Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]);
                string imgefileID = "";
                string[] imgefilearray;
                string localimagepath = "";

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //--------------Checking Last record and list record are same------
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        if (Convert.ToInt64(lst[0]) == Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]))
                        {
                            if (Convert.ToBoolean(lst[2]) == true)
                                stu = 1;
                            else
                                stu = 0;
                            Int64 id = Convert.ToInt64(lst[0]);

                            iwpro.UpdateIWDateQCTmpProcess(id, uid, lst[1].ToString(), stu, @Session["LoginID"].ToString());
                            goto Notfnd;
                        }

                    }
                    //---------------Adding last record of list-----------------//
                    def = new IWDateTmpProcess
                    {
                        ID = Convert.ToInt64(lst[0]),
                        FrontGreyImagePath = img,
                        QCDate = lst[1].ToString(),
                        sttsdtqc = Convert.ToBoolean(lst[2]),
                        ICRDate = lst[3].ToString(),
                        EntryDate = lst[4].ToString(),
                        FrontTiffImagePath = fulimg,
                    };
                    objectlst.Add(def);
                    ids.Add(def.ID);

                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;

                    string[] ddt = new string[0];
                    String tempstr = Session["SnipDate"].ToString();// "04.06.2016";//String.Concat(imgpth + Session["SnipDate"].ToString());
                    ddt = tempstr.Split('/');
                    tempstr = ddt[0] + '.' + ddt[1] + '.' + ddt[2];
                   // imgpth = imgpth + tempstr + "//Snip//" + tempstr + "//Date-";
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
                            localimagepath = imgpth;
                            imgefileID = ds.Tables[0].Rows[index].ItemArray[3].ToString().Substring(ds.Tables[0].Rows[index].ItemArray[3].ToString().IndexOf(tempstr));
                            imgefilearray = imgefileID.Split('\\');
                            imgefileID = imgefilearray[1];

                            localimagepath = localimagepath + tempstr + "//" + imgefileID + "//Snip//Date-";

                            def = new IWDateTmpProcess
                            {
                                ID = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),

                                FrontGreyImagePath = localimagepath + Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]) + ".jpg",
                                ICRDate = ds.Tables[0].Rows[index].ItemArray[1].ToString(),
                                EntryDate = ds.Tables[0].Rows[index].ItemArray[2].ToString(),
                                FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[3].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                            };
                            ids.Add(def.ID);
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

                //ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                //er.ErrorMessage = e.InnerException.Message;
                //return View("Error", er);
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }
        //------------------------------- IWMICR-------------------------------//
        public ActionResult IWMICR()
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

                SqlDataAdapter adp = new SqlDataAdapter("IWSelectMICR", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();
                //--------------------Customer Selection---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);


                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<IWMICRTmpProcess>();
                IWMICRTmpProcess def;

                //string[] ddt = new string[0];
                //String tempstr = Session["SnipDate"].ToString();// "04.06.2016";//String.Concat(imgpth + Session["SnipDate"].ToString());
                //ddt = tempstr.Split('/');
                //tempstr = ddt[0] + '.' + ddt[1] + '.' + ddt[2];
                //tempstr = ddt[2] + '-' + ddt[1] + '-' + ddt[0];

                //imgpth = imgpth.Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]);
               // imgpth = imgpth + tempstr + "//Snip//" + tempstr + "//Code-";
                //string imgefileID = "";
                //string[] imgefilearray;
                //string localimagepath = "";

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //localimagepath = imgpth;
                    //imgefileID = ds.Tables[0].Rows[0].ItemArray[13].ToString().Substring(ds.Tables[0].Rows[0].ItemArray[13].ToString().IndexOf(tempstr));
                    //imgefilearray = imgefileID.Split('\\');
                    //imgefileID = imgefilearray[1];

                    //localimagepath = localimagepath + tempstr + "//" + imgefileID + "//Snip//Code-";

                    def = new IWMICRTmpProcess//
                    {
                        ID = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]),
                        XMLSerialNo = ds.Tables[0].Rows[0].ItemArray[1].ToString(),
                        SerialNoOCR1 = ds.Tables[0].Rows[0].ItemArray[2].ToString(),
                        SerialNoOCR2 = ds.Tables[0].Rows[0].ItemArray[3].ToString(),
                        XMLPayorBankRoutNo = ds.Tables[0].Rows[0].ItemArray[4].ToString(),
                        PayorBankRoutNoOCR1 = ds.Tables[0].Rows[0].ItemArray[5].ToString(),
                        PayorBankRoutNoOCR2 = ds.Tables[0].Rows[0].ItemArray[6].ToString(),
                        XMLSAN = ds.Tables[0].Rows[0].ItemArray[7].ToString(),
                        SANOCR1 = ds.Tables[0].Rows[0].ItemArray[8].ToString(),
                        SANOCR2 = ds.Tables[0].Rows[0].ItemArray[9].ToString(),
                        XMLTransCode = ds.Tables[0].Rows[0].ItemArray[10].ToString(),
                        TransCodeOCR1 = ds.Tables[0].Rows[0].ItemArray[11].ToString(),
                        TransCodeOCR2 = ds.Tables[0].Rows[0].ItemArray[12].ToString(),
                        //FrontGreyImagePath = localimagepath + Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]) + ".jpg",
                        MICRRepairFlags = ds.Tables[0].Rows[0].ItemArray[14].ToString(),
                        //FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[13].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                        FrontGreyImagePath = ds.Tables[0].Rows[0].ItemArray[13].ToString().Replace("\\", "/"),
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[15].ToString().Replace("\\", "/"),
                        BackTiffImagePath = ds.Tables[0].Rows[0].ItemArray[16].ToString().Replace("\\", "/"),
                        
                    };
                    objectlst.Add(def);
                    //------------------------END------------------------------//
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        //localimagepath = imgpth;
                        //imgefileID = ds.Tables[0].Rows[index].ItemArray[13].ToString().Substring(ds.Tables[0].Rows[index].ItemArray[13].ToString().IndexOf(tempstr));
                        //imgefilearray = imgefileID.Split('\\');
                        //imgefileID = imgefilearray[1];

                        //localimagepath = localimagepath + tempstr + "//" + imgefileID + "//Snip//Code-";

                        def = new IWMICRTmpProcess
                        {
                            ID = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),
                            XMLSerialNo = ds.Tables[0].Rows[index].ItemArray[1].ToString(),
                            SerialNoOCR1 = ds.Tables[0].Rows[index].ItemArray[2].ToString(),
                            SerialNoOCR2 = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                            XMLPayorBankRoutNo = ds.Tables[0].Rows[index].ItemArray[4].ToString(),
                            PayorBankRoutNoOCR1 = ds.Tables[0].Rows[index].ItemArray[5].ToString(),
                            PayorBankRoutNoOCR2 = ds.Tables[0].Rows[index].ItemArray[6].ToString(),
                            XMLSAN = ds.Tables[0].Rows[index].ItemArray[7].ToString(),
                            SANOCR1 = ds.Tables[0].Rows[index].ItemArray[8].ToString(),
                            SANOCR2 = ds.Tables[0].Rows[index].ItemArray[9].ToString(),
                            XMLTransCode = ds.Tables[0].Rows[index].ItemArray[10].ToString(),
                            TransCodeOCR1 = ds.Tables[0].Rows[index].ItemArray[11].ToString(),
                            TransCodeOCR2 = ds.Tables[0].Rows[index].ItemArray[12].ToString(),
                            //FrontGreyImagePath = localimagepath + Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]) + ".jpg",
                            MICRRepairFlags = ds.Tables[0].Rows[index].ItemArray[14].ToString(),
                            //FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[13].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                            FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[13].ToString().Replace("\\", "/"),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[15].ToString().Replace("\\", "/"),
                            BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[16].ToString().Replace("\\", "/"),
                        };
                        //ViewBag.cnt = true;
                        objectlst.Add(def);
                        count = count - 1;
                        index = index + 1;
                    }
                    //Session["Pending_Count_MICR"] = ds.Tables[0].Rows[0].ItemArray[17].ToString();
                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                    return View(objectlst);
                }
                else
                    return RedirectToAction("IWIndex", "Home", new { id = 1 });//return RedirectToAction("DeSelection", new { id = 1 });
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
        [HttpPost]
        public ActionResult IWMICR(List<string> lst = null, bool snd = false, string img = null, string fulimg = null, string btnClose = "default", List<Int64> idlst = null)
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
            //int uid = (int)Session["uid"];//That will be Session value.
            int uid = (int)Session["uid"];
            string dt = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
            int custId = Convert.ToInt16(Session["CustomerID"]);
            int ttcnt = 0;

            try
            {
                if (lst != null)
                    ttcnt = lst.Count() / 19;
                int stu;

                if (btnClose == "Close" && lst != null)
                    ttcnt = ttcnt + 1;

                if (ttcnt == 1)
                {
                    for (int i = 0; i < ttcnt; i++)
                    {
                        // jt = new IWAmountTmpProcess();
                        if (Convert.ToBoolean(lst[5]) == true)
                            stu = 1;
                        else
                            stu = 0;
                        Int64 id = Convert.ToInt64(lst[0]);
                        iwpro.UpdateIWMICRTmpProcess(id, uid, lst[1].ToString(), lst[2].ToString(), lst[3].ToString(), lst[4].ToString(), stu, @Session["LoginID"].ToString());
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
                        if (Convert.ToBoolean(lst[5]) == true)
                            stu = 1;
                        else
                            stu = 0;
                        Int64 id = Convert.ToInt64(lst[0]);
                        iwpro.UpdateIWMICRTmpProcess(id, uid, lst[1].ToString(), lst[2].ToString(), lst[3].ToString(), lst[4].ToString(), stu, @Session["LoginID"].ToString());

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
                        lst.RemoveRange(0, 19);
                    }
                }

                //---------------------------IF Close button called--------------------------------//
                if (btnClose == "Close")
                {
                    @Session["glob"] = true;
                    for (int p = 0; p < idlst.Count; p++)
                    {
                        iwpro.UnlockRecords(idlst[p], "IwMicr", uid, @Session["LoginID"].ToString(), dt, custId);
                        //iwpro.UnlockRecords(dataEntryView.Id, "DE", uid, @Session["LoginID"].ToString(), dt, custId);
                        //iwpro.UnlockRecords(idlst[p], "IwMicr", 0, null, null, 0);
                    }
                    return Json(false);
                }
                //------------------Select next Pending Record------------------
                //------------------------------Calling Proc for selecting data-----------------------

                // Procommand = new System.Data.Sql.("{ call SelectForDwld('"+ date.ToString("yyyy-MM-dd")+"')}",con);

                SqlDataAdapter adp = new SqlDataAdapter("IWSelectMICR", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                //--------------------Customer Selection---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);


                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<IWMICRTmpProcess>();
                IWMICRTmpProcess def;
                ArrayList ids = new ArrayList();
                bool checkid = false;
                //imgpth = imgpth.Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //--------------Checking Last record and list record are same------
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        if (Convert.ToInt64(lst[0]) == Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]))
                        {
                            if (Convert.ToBoolean(lst[5]) == true)
                                stu = 1;
                            else
                                stu = 0;
                            Int64 id = Convert.ToInt64(lst[0]);
                            iwpro.UpdateIWMICRTmpProcess(id, uid, lst[1].ToString(), lst[2].ToString(), lst[3].ToString(), lst[4].ToString(), stu, @Session["LoginID"].ToString());
                            goto Notfnd;
                        }

                    }
                    //---------------Adding last record of list-----------------//
                    def = new IWMICRTmpProcess
                    {
                        ID = Convert.ToInt64(lst[0]),
                        FrontGreyImagePath = img,
                        sttsdtqc = Convert.ToBoolean(lst[5]),
                        EntrySerialNo = lst[1].ToString(),
                        EntryPayorBankRoutNo = lst[2].ToString(),
                        EntrySAN = lst[3].ToString(),
                        EntryTransCode = lst[4].ToString(),
                        XMLSerialNo = lst[6].ToString(),
                        SerialNoOCR1 = lst[7].ToString(),
                        SerialNoOCR2 = lst[8].ToString(),
                        XMLPayorBankRoutNo = lst[9].ToString(),
                        PayorBankRoutNoOCR1 = lst[10].ToString(),
                        PayorBankRoutNoOCR2 = lst[11].ToString(),
                        XMLSAN = lst[12].ToString(),
                        SANOCR1 = lst[13].ToString(),
                        SANOCR2 = lst[14].ToString(),
                        XMLTransCode = lst[15].ToString(),
                        TransCodeOCR1 = lst[16].ToString(),
                        TransCodeOCR2 = lst[17].ToString(),
                        MICRRepairFlags = lst[18].ToString(),
                        FrontTiffImagePath = fulimg,
                        //-------------------------------//
                    };
                    objectlst.Add(def);
                    ids.Add(def.ID);

                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;

                    //string[] ddt = new string[0];
                    //String tempstr = Session["SnipDate"].ToString();// "04.06.2016";//String.Concat(imgpth + Session["SnipDate"].ToString());
                    //ddt = tempstr.Split('/');
                    ////tempstr = ddt[0] + '.' + ddt[1] + '.' + ddt[2];
                    //tempstr = ddt[2] + '-' + ddt[1] + '-' + ddt[0];
                    //// imgpth = imgpth + tempstr + "//Snip//" + tempstr + "//Code-";
                    //string imgefileID = "";
                    //string[] imgefilearray;
                    //string localimagepath = "";

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
                            //localimagepath = imgpth;
                            //imgefileID = ds.Tables[0].Rows[index].ItemArray[13].ToString().Substring(ds.Tables[0].Rows[index].ItemArray[13].ToString().IndexOf(tempstr));
                            //imgefilearray = imgefileID.Split('\\');
                            //imgefileID = imgefilearray[1];

                            //localimagepath = localimagepath + tempstr + "//" + imgefileID + "//Snip//Code-";

                            def = new IWMICRTmpProcess
                            {
                                ID = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),
                                XMLSerialNo = ds.Tables[0].Rows[index].ItemArray[1].ToString(),
                                SerialNoOCR1 = ds.Tables[0].Rows[index].ItemArray[2].ToString(),
                                SerialNoOCR2 = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                                XMLPayorBankRoutNo = ds.Tables[0].Rows[index].ItemArray[4].ToString(),
                                PayorBankRoutNoOCR1 = ds.Tables[0].Rows[index].ItemArray[5].ToString(),
                                PayorBankRoutNoOCR2 = ds.Tables[0].Rows[index].ItemArray[6].ToString(),
                                XMLSAN = ds.Tables[0].Rows[index].ItemArray[7].ToString(),
                                SANOCR1 = ds.Tables[0].Rows[index].ItemArray[8].ToString(),
                                SANOCR2 = ds.Tables[0].Rows[index].ItemArray[9].ToString(),
                                XMLTransCode = ds.Tables[0].Rows[index].ItemArray[10].ToString(),
                                TransCodeOCR1 = ds.Tables[0].Rows[index].ItemArray[11].ToString(),
                                TransCodeOCR2 = ds.Tables[0].Rows[index].ItemArray[12].ToString(),
                                //FrontGreyImagePath = localimagepath + Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]) + ".jpg",
                                MICRRepairFlags = ds.Tables[0].Rows[index].ItemArray[14].ToString(),
                                //FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[13].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                                FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[13].ToString().Replace("\\", "/"),
                                FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[15].ToString().Replace("\\", "/"),
                                BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[16].ToString().Replace("\\", "/"),
                            };
                            ids.Add(def.ID);
                            objectlst.Add(def);
                        }
                        checkid = false;
                        count = count - 1;
                        index = index + 1;
                    }
                    //Session["Pending_Count_MICR"] = ds.Tables[0].Rows[0].ItemArray[17].ToString();
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

                //ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                //er.ErrorMessage = e.InnerException.Message;
                //return View("Error", er);
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }
        //---------------------------------Payee Name Entry--------------------------//
        public ActionResult IWPayee()
        {

            //------------------------------Calling Proc for selecting data-----------------------
            //  SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
            // Procommand = new System.Data.Sql.("{ call SelectForDwld('"+ date.ToString("yyyy-MM-dd")+"')}",con);
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["QC"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            //IWAmountTmpProcess jt;
            try
            {
                int uid = (int)Session["uid"];
                SqlDataAdapter adp = new SqlDataAdapter("IWSelectPayee", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                //--------------------Customer Selection---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);


                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<IWPayeeTmpProcess>();
                IWPayeeTmpProcess def;

                if (ds.Tables[0].Rows.Count > 0)
                {

                    def = new IWPayeeTmpProcess
                    {
                        ID = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]),

                        FrontGreyImagePath = ds.Tables[0].Rows[0].ItemArray[1].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                        XMLPayee = ds.Tables[0].Rows[0].ItemArray[2].ToString(),
                        XMLAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[3]),
                        OpsStatus = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[4])

                    };
                    objectlst.Add(def);
                    //------------------------END------------------------//
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        def = new IWPayeeTmpProcess
                        {
                            ID = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),

                            FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[1].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                            XMLPayee = ds.Tables[0].Rows[index].ItemArray[2].ToString(),
                            XMLAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[3]),
                            OpsStatus = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[4])
                        };

                        objectlst.Add(def);
                        count = count - 1;
                        index = index + 1;
                    }

                    ViewBag.cnt = true;
                    @Session["glob"] = null;
                    return View(objectlst);
                }
                else
                    return RedirectToAction("IWIndex", "Home", new { id = 1 });
            }
            catch (Exception e)
            {

                //ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                //er.ErrorMessage = e.InnerException.Message;
                //return View("Error", er);
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
            // return RedirectToAction("DeSelection", new { id = 1 });
        }
        [HttpPost]
        public ActionResult IWPayee(List<string> lst, bool snd, string img, string btnClose = "default", List<Int64> idlst = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["QC"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            //IWAmountTmpProcess jt;
            int uid = (int)Session["uid"];//That will be Session value.
            int ttcnt = 0;
            try
            {
                if (lst != null)
                    ttcnt = lst.Count() / 6;
                //int stts = 0;
                string rtncode = "0";

                if (btnClose == "Close" && lst != null)
                    ttcnt = ttcnt + 1;

                if (ttcnt == 1)
                {
                    for (int i = 0; i < ttcnt; i++)
                    {
                        if (lst[4] != null)
                            if (lst[4].ToString() != "")
                                rtncode = lst[4].ToString();
                            else
                                rtncode = "0";

                        Int64 id = Convert.ToInt64(lst[0]);
                        iwpro.UpdateIWPayeeTmpProcess(id, uid, lst[1].ToString().ToUpper(), Convert.ToInt16(lst[5]), rtncode, @Session["LoginID"].ToString());
                        //lst.RemoveRange(0, 6);
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
                    }
                }
                else
                {
                    for (int i = 0; i < ttcnt - 1; i++)
                    {
                        if (lst[4] != null)
                            if (lst[4].ToString() != "")
                                rtncode = lst[4].ToString();
                            else
                                rtncode = "0";

                        Int64 id = Convert.ToInt64(lst[0]);

                        iwpro.UpdateIWPayeeTmpProcess(id, uid, lst[1].ToString().ToUpper(), Convert.ToInt16(lst[5]), rtncode, @Session["LoginID"].ToString());

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
                        lst.RemoveRange(0, 6);
                    }
                }

                //---------------------------IF Close button called--------------------------------//
                if (btnClose == "Close")
                {
                    @Session["glob"] = true;
                    for (int p = 0; p < idlst.Count; p++)
                    {
                        iwpro.UnlockRecords(idlst[p], "IWPaye", 0, null, null, 0);
                    }
                    return Json(false);
                }
                // ViewBag.Secondcall = true;
                //------------------Select next Pending Record------------------
                //------------------------------Calling Proc for selecting data-----------------------

                // Procommand = new System.Data.Sql.("{ call SelectForDwld('"+ date.ToString("yyyy-MM-dd")+"')}",con);

                SqlDataAdapter adp = new SqlDataAdapter("IWSelectPayee", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                //--------------------Customer Selection---------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);


                DataSet ds = new DataSet();
                adp.Fill(ds);
                var objectlst = new List<IWPayeeTmpProcess>();
                IWPayeeTmpProcess def;
                ArrayList ids = new ArrayList();
                bool checkid = false;
                //  Int64 tempId = 0;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //--------------Checking Last record and list record are same------
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        if (Convert.ToInt64(lst[0]) == Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]))
                        {
                            if (lst[4] != null)
                                if (lst[4].ToString() != "")
                                    rtncode = lst[4].ToString();
                                else
                                    rtncode = "0";
                            Int64 id = Convert.ToInt64(lst[0]);
                            iwpro.UpdateIWPayeeTmpProcess(id, uid, lst[1].ToString().ToUpper(), Convert.ToInt16(lst[5]), rtncode, @Session["LoginID"].ToString());
                            goto Notfnd;
                        }

                    }
                    //---------------Adding last record of list-----------------//

                    if (lst[4] != null)
                        if (lst[4].ToString() != "")
                            rtncode = lst[4].ToString();
                        else
                            rtncode = "0";

                    def = new IWPayeeTmpProcess
                    {
                        ID = Convert.ToInt64(lst[0]),
                        FrontGreyImagePath = img,
                        EntryPayee = lst[1].ToString(),
                        XMLPayee = lst[2].ToString(),
                        XMLAmount = Convert.ToDecimal(lst[3]),
                        RejectReason = rtncode,
                        OpsStatus = Convert.ToInt16(lst[5]),
                    };
                    objectlst.Add(def);
                    ids.Add(def.ID);

                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;

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
                            def = new IWPayeeTmpProcess
                            {
                                ID = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),

                                FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[1].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                                XMLPayee = ds.Tables[0].Rows[index].ItemArray[2].ToString(),
                                XMLAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[3]),
                                OpsStatus = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[4]),
                            };
                            ids.Add(def.ID);
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

                //ErrorDisplay er = new ErrorDisplay();
                //ViewBag.Error = e.InnerException;
                //er.ErrorMessage = e.InnerException.Message;
                //return View("Error", er);
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }


        //--------------------------------------Common For Image Display-----------------------
        public ActionResult showImage(string imgurl, string imgtype = "", string iwidth = "", string iheight = "")
        {
            if (iwidth == "")
                iwidth = "620px";
            if (iheight == "")
                iheight = "310px";

            ViewBag.imgurl = imgurl;
            ViewBag.imgtype = imgtype;
            ViewBag.iwidth = iwidth;
            ViewBag.iheight = iheight;

            return PartialView();

        }
        public PartialViewResult RejectReason()
        {
            string[] code = { "13", "30", "31", "32", "34", "35", "66" };
            var rjrs = (from r in af.ItemReturnReasons
                        where code.Contains(r.RETURN_REASON_CODE)
                        select new RejectReason
                        {
                            Description = r.DESCRIPTION,
                            ReasonCodeS = r.RETURN_REASON_CODE
                        });
            return PartialView("_RejectDetails", rjrs);
        }
        public JsonResult ValidTrans(string transcode)
        {
            var trnscd = (from t in af.TransCodes
                          where t.TrCode == transcode
                          select t).ToList();
            if (trnscd.Count != 0)
                return Json(true, JsonRequestBehavior.AllowGet);
            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }

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

                    adp.Fill(ds);
                }

                
                var objectlst = new List<IWDataEntryView>();
                IWDataEntryView def;
                //imgpth = imgpth.Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]);

                //string[] ddt = new string[0];
                //String tempstr = Session["SnipDate"].ToString();// "04.06.2016";//String.Concat(imgpth + Session["SnipDate"].ToString());
                //string imgefileID = "";
                //string[] imgefilearray;
                //string localimagepath = "";
                //ddt = tempstr.Split('/');
                //tempstr = ddt[0] + '.' + ddt[1] + '.' + ddt[2];
                // imgpth = imgpth + tempstr + "//DbtAccNo-"; chnange on 07-06-2017
                //imgpth = imgpth + tempstr + "//Snip//" + tempstr + "//DbtAccNo-";

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //localimagepath = imgpth;
                    //imgefileID = ds.Tables[0].Rows[0].ItemArray[1].ToString().Substring(ds.Tables[0].Rows[0].ItemArray[1].ToString().IndexOf(tempstr));
                    //imgefilearray = imgefileID.Split('\\');
                    //imgefileID = imgefilearray[1];

                    //localimagepath = localimagepath + tempstr + "//" + imgefileID + "//Snip//DbtAccNo-";

                    def = new IWDataEntryView
                    {
                        Id = Convert.ToInt64(ds.Tables[0].Rows[0]["ID"]),
                        //FrontGreyImagePath = localimagepath + Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]) + ".jpg",//ds.Tables[0].Rows[0].ItemArray[1].ToString(),
                        //FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[1].ToString().Replace((string)Session["SrcWebIP"], (string)Session["DestWepIP"]).Replace((string)Session["SrcWebName"], (string)Session["DestWebName"]),
                        FrontGreyImagePath = ds.Tables[0].Rows[0]["FrontGreyImagePath"].ToString().Replace("\\", "/"),
                        FrontTiffImagePath = ds.Tables[0].Rows[0]["FrontTiffImagePath"].ToString().Replace("\\", "/"),
                        BackTiffImagePath = ds.Tables[0].Rows[0]["BackTiffImagePath"].ToString().Replace("\\", "/"),
                        XMLPayeeName = ds.Tables[0].Rows[0]["XMLPayeeName"].ToString(),
                    };
                    //objectlst.Add(def);
                    //------------------------END------------------------//

                    if(id == 1)
                    {
                        Session["deEntryAccountNo"] = ds.Tables[0].Rows[0]["DbtAccNo"] == null ? "" : ds.Tables[0].Rows[0]["DbtAccNo"].ToString();
                        Session["deEntryChqDate"] = ds.Tables[0].Rows[0]["ChqDate"] == null ? "" : ds.Tables[0].Rows[0]["ChqDate"].ToString().Substring(8,2) + ds.Tables[0].Rows[0]["ChqDate"].ToString().Substring(5, 2) + ds.Tables[0].Rows[0]["ChqDate"].ToString().Substring(2, 2);
                        Session["deXMLPayeeName"] = ds.Tables[0].Rows[0]["EntryPayeeName"] == null ? "" : ds.Tables[0].Rows[0]["EntryPayeeName"].ToString();
                    }
                    else
                    {
                        Session["deEntryAccountNo"] = "";
                        Session["deEntryChqDate"] = "";
                        Session["deXMLPayeeName"] = ds.Tables[0].Rows[0]["XMLPayeeName"] == null ? "" : ds.Tables[0].Rows[0]["XMLPayeeName"].ToString();
                    }
                    

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
        public ActionResult IWDataEntry(IWDataEntryView dataEntryView, string btnSubmit)
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
                    string cdatenew = "20" + cdate.Substring(4, 2) + "-" +
                        cdate.Substring(2, 2) + "-" +
                        cdate.Substring(0, 2);

                    iwpro.UpdateIWDataEntry(dataEntryView.Id, uid, dataEntryView.EntryAccountNo, cdatenew, dataEntryView.XMLPayeeName,
                        @Session["LoginID"].ToString(),dt,custId);

                    Session["CurrentDataEntryCount"] = Convert.ToInt16(Session["CurrentDataEntryCount"]) + 1;
                    return RedirectToAction("IWDataEntry", "IWDataEntry", new { id = 0 });
                }

            //---------------------------IF Close button called--------------------------------//
                Close:
                if (btnSubmit == "Close")
                {
                    iwpro.UnlockRecords(dataEntryView.Id, "DE",uid, @Session["LoginID"].ToString(),dt, custId);
                    /// Int64 SlipRawaDataID = 0;
                    Session["glob"] = true;
                    return RedirectToAction("IWIndex", "Home", new { id = 2 });
                }

                //-------------Calling next Records---------------
                if(btnSubmit == "Back")
                {
                    return RedirectToAction("IWDataEntry", "IWDataEntry", new { id = 1 });
                }

                return RedirectToAction("IWIndex", "Home", new { id = 1 });
            }
            catch(Exception e)
            {
                return RedirectToAction("Error", "Error", new { msg = e.Message.ToString(), popmsg = e.StackTrace.ToString() });
            }
        }

        [HttpPost]
        public ActionResult IWDataEntry111(IWDataEntryView dataEntryView, string btnSubmit)
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
                    string cdatenew = "20" + cdate.Substring(4, 2) + "-" +
                        cdate.Substring(2, 2) + "-" +
                        cdate.Substring(0, 2);
                    

                    iwpro.UpdateIWDataEntry(dataEntryView.Id, uid, dataEntryView.EntryAccountNo, cdatenew, dataEntryView.XMLPayeeName,
                        @Session["LoginID"].ToString(), dt, custId);

                    if (Session["IdLists"] != null)
                    {
                        listId = (List<Int64>)Session["IdLists"];
                    }

                    listId.Add(dataEntryView.Id);
                    Session["IdLists"] = listId;
                    Session["CurrentDataEntryCount"] = Convert.ToInt16(Session["CurrentDataEntryCount"]) + 1;
                    return RedirectToAction("IWDataEntry", "IWDataEntry");
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
            catch(Exception e)
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

        public class ShowCountAll
        {
            public Int32 Total_Count { get; set; }
            public Int32 MICR_Pending_Count { get; set; }
            public Int32 ICR_Pending_Count { get; set; }
        }

        public JsonResult ShowPendingCount()
        {
            try
            {
                ShowCountAll cnt = new ShowCountAll();
                int uid = (int)Session["uid"];
                string sessionType = Convert.ToString(Session["CtsSessionType"]);
                string type = Convert.ToString(Session["ProType"]);
                string loginId = Session["LoginID"].ToString();

                SqlDataAdapter sqladlog = new SqlDataAdapter("ShowPendingRecordsForMICR", con);
                sqladlog.SelectCommand.CommandType = CommandType.StoredProcedure;
                //sqladlog.SelectCommand.Parameters.AddWithValue("@uid", SqlDbType.NVarChar).Value = uid;
                sqladlog.SelectCommand.Parameters.AddWithValue("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");
                sqladlog.SelectCommand.Parameters.AddWithValue("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                //sqladlog.SelectCommand.Parameters.AddWithValue("@Type", SqlDbType.NVarChar).Value = type;
                //sqladlog.SelectCommand.Parameters.AddWithValue("@LoginId", SqlDbType.NVarChar).Value = loginId;
                DataSet dslog = new DataSet();
                sqladlog.Fill(dslog);

                int totalcount = 0;
                int index = 0;

                if (dslog.Tables[0].Rows.Count > 0)
                {
                    //totalcount = Convert.ToInt32(dslog.Tables[0].Rows[0].ItemArray[0]);
                    cnt.MICR_Pending_Count = Convert.ToInt32(dslog.Tables[0].Rows[0].ItemArray[0]);
                    cnt.Total_Count = Convert.ToInt32(dslog.Tables[0].Rows[0].ItemArray[1]);
                    cnt.ICR_Pending_Count = Convert.ToInt32(dslog.Tables[0].Rows[0].ItemArray[2]);
                }
                //return Json(totalcount.ToString(), JsonRequestBehavior.AllowGet);
                return Json(cnt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
