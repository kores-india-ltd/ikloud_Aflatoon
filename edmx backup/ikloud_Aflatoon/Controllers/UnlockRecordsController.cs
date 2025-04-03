using ikloud_Aflatoon.Infrastructure;
using ikloud_Aflatoon.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ikloud_Aflatoon.Controllers
{
    public class UnlockRecordsController : Controller
    {
        //
        // GET: /UnlockRecords/
        AflatoonEntities af = new AflatoonEntities();
        UserAflatoonDbContext adc = new UserAflatoonDbContext();
        OWProcDataContext OWpro = new OWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
        public ActionResult Index()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["Settg"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }



            Session["glob"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult unlockData(string modename = null, string fieldname = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if ((bool)Session["Settg"] == false)
            {
                int uid1 = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid1);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }
            int uid = (int)Session["uid"];
            string unlockfield = "";
            Int64 rawdataid = 0;
            Int64 id = 0;
            unlockRecords unlk = new unlockRecords();
            try
            {


                if (modename == "DataEntry")
                    unlockfield = fieldname;
                else
                    unlockfield = modename;
                ViewBag.unlockfield = unlockfield;

                SqlDataAdapter adp = new SqlDataAdapter("UnlockRecordsData", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@ModuleName", SqlDbType.NVarChar).Value = unlockfield;
                adp.SelectCommand.Parameters.Add("@ClearingType", SqlDbType.NVarChar).Value = Session["ProType"].ToString();
                adp.SelectCommand.Parameters.Add("@processingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();

                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(Session["DomainselectID"]);

                DataSet ds = new DataSet();
                adp.Fill(ds);
                DataTable temptable = new DataTable();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    temptable = ds.Tables[0];
                    // var result = ds.Tables[0].Select().ToList();
                    if (unlockfield != "Slip Amount" && unlockfield != "CHI Reject Handler" && unlockfield != "Cheque MICR")
                    {
                        if (unlockfield == "Cheque Amount")
                        {
                            for (int i = 0; i < temptable.Rows.Count; i++)
                            {
                                rawdataid = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[1]);
                                id = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[0]);
                                var result = af.ChequeAmountCapture.Where(d => d.RawDataId == rawdataid).SingleOrDefault();
                                if (result != null)
                                {
                                    OWpro.UpdateSingleLock(unlockfield, id, rawdataid, Session["LoginID"].ToString(), Session["ProType"].ToString());
                                    //ds.Tables[0].Rows.RemoveAt(i);
                                }

                            }
                        }
                        else if (unlockfield == "Cheque Date")
                        {
                            for (int i = 0; i < temptable.Rows.Count; i++)
                            {
                                rawdataid = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[1]);
                                id = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[0]);
                                var result = af.ChequeDateCaptures.Where(d => d.RawDataId == rawdataid).SingleOrDefault();
                                if (result != null)
                                {
                                    OWpro.UpdateSingleLock(unlockfield, id, rawdataid, Session["LoginID"].ToString(), Session["ProType"].ToString());
                                    //ds.Tables[0].Rows.RemoveAt(i);
                                }

                            }

                        }
                        else if (unlockfield == "Slip Account NO")
                        {
                            for (int i = 0; i < temptable.Rows.Count; i++)
                            {
                                rawdataid = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[1]);
                                id = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[0]);
                                var result = af.SlipAccountNoCapture.Where(d => d.RawDataId == rawdataid).SingleOrDefault();
                                if (result != null)
                                {
                                    OWpro.UpdateSingleLock(unlockfield, id, rawdataid, Session["LoginID"].ToString(), Session["ProType"].ToString());
                                    //ds.Tables[0].Rows.RemoveAt(i);
                                }

                            }

                        }
                        else if (unlockfield == "L1 Verification")
                        {
                            for (int i = 0; i < temptable.Rows.Count; i++)
                            {
                                rawdataid = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[1]);
                                id = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[0]);
                                var result = af.L1Verification.Where(d => d.RawDataId == rawdataid).SingleOrDefault();
                                if (result != null)
                                {
                                    OWpro.UpdateSingleLock(unlockfield, id, rawdataid, Session["LoginID"].ToString(), Session["ProType"].ToString());
                                    //ds.Tables[0].Rows.RemoveAt(i);
                                }

                            }

                        }
                        else if (unlockfield == "L2 Verification")
                        {
                            for (int i = 0; i < temptable.Rows.Count; i++)
                            {
                                rawdataid = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[1]);
                                id = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[0]);
                                var result = af.L2Verification.Where(d => d.RawDataId == rawdataid).SingleOrDefault();
                                if (result != null)
                                {
                                    OWpro.UpdateSingleLock(unlockfield, id, rawdataid, Session["LoginID"].ToString(), Session["ProType"].ToString());
                                    //ds.Tables[0].Rows.RemoveAt(i);
                                }

                            }

                        }
                        else if (unlockfield == "L3 Verification")
                        {
                            for (int i = 0; i < temptable.Rows.Count; i++)
                            {
                                rawdataid = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[1]);
                                id = Convert.ToInt64(ds.Tables[0].Rows[i].ItemArray[0]);
                                var result = af.L3Verification.Where(d => d.RawDataId == rawdataid).SingleOrDefault();
                                if (result != null)
                                {
                                    OWpro.UpdateSingleLock(unlockfield, id, rawdataid, Session["LoginID"].ToString(), Session["ProType"].ToString());
                                    //ds.Tables[0].Rows.RemoveAt(i);
                                }

                            }

                        }

                    }
                    return PartialView("_ShowData", ds);
                }

                return View("_ShowData", ds);
            }
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "unlockData HttpPost- " + innerExcp });
            }
        }
        public ActionResult Username(int id = 0)
        {
            ViewBag.user = adc.UserMasters.Where(m => m.ID == id).Select(m => m.FirstName).FirstOrDefault();
            return View("_Username");
        }

        [HttpPost]
        public ActionResult clearLocks(string fieldname = null)
        {
            try
            {
                OWpro.Clearlocks(0, fieldname, Session["ProType"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"),
                Convert.ToInt16(Session["CustomerID"]), Convert.ToInt32(Session["DomainselectID"]), Session["LoginID"].ToString());

            }
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "clearLocks HttpPost- " + innerExcp });
            }

            return Json(true, JsonRequestBehavior.AllowGet);

        }

    }
}
