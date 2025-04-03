using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ikloud_Aflatoon.Infrastructure;

namespace ikloud_Aflatoon.Controllers
{
    public class ChiRejectHandlerController : Controller
    {
        //
        // GET: /ChiRejectHandler/
        AflatoonEntities af = new AflatoonEntities();
        //OWProcDataContext OWpro = new OWProcDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);

        public ActionResult Index()
        {//Convert.ToInt16(Session["CustomerID"]);

            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }

            if ((bool)Session["chirjct"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            if (Session["domainid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }

            int custid = Convert.ToInt16(Session["CustomerID"]);

            ViewBag.Customer = new SelectList(af.CustomerMasters.Where(m => m.Id == custid), "Id", "Name").ToList();
            @Session["glob"] = true;
            return View();
        }

        public ActionResult ChiReject(int customerid = 0)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            int uid = (int)Session["uid"];
            if ((bool)Session["chirjct"] == false)
            {
                //int uid = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            
            int domainId = Convert.ToInt32(Session["DomainselectID"].ToString());
            //  int customerID = 0;
            //if (Request.Form["Customer"] != "" && Request.Form["Customer"] != null)
            //    customerid = Convert.ToInt16(Request.Form["Customer"]);
            try
            {
                DataSet ds = new DataSet();
                SqlDataAdapter adp = new SqlDataAdapter("SelectChiReject", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@Sessionuid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");//sDate;//Session["processdate"].ToString();
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = Convert.ToInt16(Session["CustomerID"]);
                adp.SelectCommand.Parameters.Add("@DomainID", SqlDbType.NVarChar).Value = domainId;
                adp.Fill(ds);
                ChiReject Owsr = null;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        Owsr = new ChiReject
                        {
                            Id = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),
                            ScanningNodeId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[1].ToString()),
                            FinalAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[2].ToString()),
                            FinalDate = Convert.ToDateTime(ds.Tables[0].Rows[index].ItemArray[3].ToString()),
                            ChequeNoFinal = ds.Tables[0].Rows[index].ItemArray[4].ToString(),
                            SortCodeFinal = ds.Tables[0].Rows[index].ItemArray[5].ToString(),
                            SANFinal = ds.Tables[0].Rows[index].ItemArray[6].ToString(),
                            TransCodeFinal = ds.Tables[0].Rows[index].ItemArray[7].ToString(),
                            CHIStatus = ds.Tables[0].Rows[index].ItemArray[8].ToString(),
                            CHIRejectReason = ds.Tables[0].Rows[index].ItemArray[9].ToString() == null ? 0 : Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[9].ToString()),
                            DocType = ds.Tables[0].Rows[index].ItemArray[10].ToString(),
                            IgnoreIQA = Convert.ToBoolean(ds.Tables[0].Rows[index].ItemArray[11].ToString()),
                            FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[12].ToString(),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[13].ToString(),
                            BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[14].ToString(),
                            Customer_ID = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[15].ToString()),
                            DomainID = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[16].ToString()),
                            RawDataID = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[17].ToString()),


                        };
                        count = count - 1;
                        index = index + 1;
                    }

                    Owsr.Customerid = customerid;
                    if (Owsr.CHIRejectReason != 0)
                    {
                        Owsr.ChiRejectdescription = af.CHIRejectReasons.Where(m => m.CHIRejectReasonID == Owsr.CHIRejectReason).Select(m => m.REJECTREASONDESCRIPTION).SingleOrDefault();
                    }
                    @Session["glob"] = null;
                    //return View("_ChiRejectHandler", Owsr);
                    return View("ChiRejectNew", Owsr);
                }
                else
                {
                    return RedirectToAction("IWIndex", "Home", new { id = 5 });
                    //return Json(false, JsonRequestBehavior.AllowGet);

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
        [HttpPost]
        public ActionResult ChiReject(List<string> allfieldsval = null, bool IQAIgnore = false, string btnvalue = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }

            // string ignoreIQA = Request.Form["ignriqa"];
            int uid = (int)Session["uid"];//That will be Session value.

            if ((bool)Session["chirjct"] == false)
            {
                //int uid = (int)Session["uid"];
                UserMaster usrm = af.UserMasters.Find(uid);
                usrm.Active = false;
                af.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            try
            {
                if (btnvalue == "Closed")
                {
                    //OWpro.UpdateCHIReject(Convert.ToInt64(allfieldsval[0]), 0, uid, null, null,
                    //null, null, 0,
                    // "C", Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), 0,
                    // 0, 0, 0, false);

                    SqlCommand cmd = new SqlCommand("UpdateCHIReject", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", Convert.ToInt64(allfieldsval[0]));
                    cmd.Parameters.AddWithValue("@RawDataId", 0);
                    cmd.Parameters.AddWithValue("@Uid", uid);
                    cmd.Parameters.AddWithValue("@ChequeNoEntry", null);
                    cmd.Parameters.AddWithValue("@SortCodeEntry", null);
                    cmd.Parameters.AddWithValue("@SANEntry", null);
                    cmd.Parameters.AddWithValue("@TransCodeEntry", null);
                    cmd.Parameters.AddWithValue("@status", 0);
                    cmd.Parameters.AddWithValue("@ActionTaken", "C");
                    cmd.Parameters.AddWithValue("@LName", Session["LoginID"].ToString());
                    cmd.Parameters.AddWithValue("@ProcessingDate", Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@CustomerId", 0);
                    cmd.Parameters.AddWithValue("@DomainId", 0);
                    cmd.Parameters.AddWithValue("@ScanningNodeId", 0);
                    cmd.Parameters.AddWithValue("@SettingValue", 0);
                    cmd.Parameters.AddWithValue("@IgnoreIQA", false);


                    con.Open();

                    cmd.ExecuteNonQuery();
                    con.Close();

                    return Json(false, JsonRequestBehavior.AllowGet);
                }

                string Action = allfieldsval[11].ToString().ToUpper();


                //OWpro.UpdateCHIReject(Convert.ToInt64(allfieldsval[6]), Convert.ToInt64(allfieldsval[9]), uid, allfieldsval[1].ToString(), allfieldsval[2].ToString(),
                //    allfieldsval[3].ToString(), allfieldsval[4].ToString(), 0,
                //    Action, Session["LoginID"].ToString(), Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"), Convert.ToInt16(allfieldsval[10]),
                //    Convert.ToInt16(allfieldsval[7]), Convert.ToInt16(allfieldsval[8]), 0, IQAIgnore);

                SqlCommand cmd1 = new SqlCommand("UpdateCHIReject", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@ID", Convert.ToInt64(allfieldsval[6]));
                cmd1.Parameters.AddWithValue("@RawDataId", Convert.ToInt64(allfieldsval[9]));
                cmd1.Parameters.AddWithValue("@Uid", uid);
                cmd1.Parameters.AddWithValue("@ChequeNoEntry", allfieldsval[1].ToString());
                cmd1.Parameters.AddWithValue("@SortCodeEntry", allfieldsval[2].ToString());
                cmd1.Parameters.AddWithValue("@SANEntry", allfieldsval[3].ToString());
                cmd1.Parameters.AddWithValue("@TransCodeEntry", allfieldsval[4].ToString());
                cmd1.Parameters.AddWithValue("@status", 0);
                cmd1.Parameters.AddWithValue("@ActionTaken", Action);
                cmd1.Parameters.AddWithValue("@LName", Session["LoginID"].ToString());
                cmd1.Parameters.AddWithValue("@ProcessingDate", Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd"));
                cmd1.Parameters.AddWithValue("@CustomerId", Convert.ToInt16(allfieldsval[10]));
                cmd1.Parameters.AddWithValue("@DomainId", Convert.ToInt16(allfieldsval[7]));
                cmd1.Parameters.AddWithValue("@ScanningNodeId", Convert.ToInt16(allfieldsval[8]));
                cmd1.Parameters.AddWithValue("@SettingValue", 0);
                cmd1.Parameters.AddWithValue("@IgnoreIQA", IQAIgnore);


                con.Open();

                cmd1.ExecuteNonQuery();
                con.Close();


                //  return RedirectToAction("", new { customerid=});

                return Json(true, JsonRequestBehavior.AllowGet);
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

    }
}
