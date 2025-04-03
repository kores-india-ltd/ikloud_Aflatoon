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
    public class CMS_LocationMasterController : Controller
    {
        //
        // GET: /CMS_LocationMaster/
        AflatoonEntities db = new AflatoonEntities();
        string cs = ConfigurationManager.ConnectionStrings["iKloudProConnectionString2"].ConnectionString;
        private SqlConnection con;
        //To Handle connection related activities    
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["iKloudProConnectionString2"].ToString();
            con = new SqlConnection(constr);

        }

        public ActionResult Index(int id = 0)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            
            if ((bool)Session["master"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            try
            {
                CMS_LocationMasterRepository LocRepo = new CMS_LocationMasterRepository();
                ModelState.Clear();

                if (id == 1)
                {
                    ViewBag.Message = "CMS_Location Data added successfully";
                }
                if (id == 2)
                {
                    ViewBag.Message = "CMS_Location Data updated successfully";
                }
                if (id == 3)
                {
                    ViewBag.Message = "CMS_Location Data deleted successfully";
                }

                return View(LocRepo.GetAllLocationMasterData());
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "CMS_Location Index- " + innerExcp });
            }

        }

        public ActionResult AddLocation()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            
            if ((bool)Session["master"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            return View();
        }

        [HttpPost]
        public ActionResult AddLocation(CMS_LocationMaster_DBS_View obj)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            
            if ((bool)Session["master"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            try
            {
                Int64 loginId = Convert.ToInt64(Session["uid"]);
                if (ModelState.IsValid)
                {
                    CMS_LocationMasterRepository LocRepo = new CMS_LocationMasterRepository();
                    LocRepo.AddLocationMasterData(obj, loginId);

                    //return View("Index");
                    return RedirectToAction("Index", "CMS_LocationMaster", new { id = 1 });
                }
                return View();
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "CMS_Location Add Post - " + innerExcp });
            }
        }

        public ActionResult EditCMS_LocationData(Int64 id)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            
            if ((bool)Session["master"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            try
            {
                CMS_LocationMasterRepository LocRepo = new CMS_LocationMasterRepository();
                return View(LocRepo.GetAllLocationMasterData().Find(cust => cust.ID == id));
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "CMS_Location Edit get - " + innerExcp });
            }

        }

        [HttpPost]
        public ActionResult EditCMS_LocationData(Int64 id, CMS_LocationMaster_DBS_View obj)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            
            if ((bool)Session["master"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            try
            {
                Int64 loginId = Convert.ToInt64(Session["uid"]);
                CMS_LocationMasterRepository LocRepo = new CMS_LocationMasterRepository();
                LocRepo.UpdateLocationMasterData(obj, loginId);


                //return RedirectToAction("Index");
                return RedirectToAction("Index", "CMS_LocationMaster", new { id = 2 });

            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "CMS_Location Edit Post - " + innerExcp });
            }
        }

        public ActionResult DeleteCMS_LocationData(Int64 Id)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            
            if ((bool)Session["master"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            try
            {
                CMS_LocationMasterRepository LocRepo = new CMS_LocationMasterRepository();
                LocRepo.DeleteLocationMasterData(Id);

                //return RedirectToAction("Index");
                return RedirectToAction("Index", "CMS_LocationMaster", new { id = 3 });

            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "CMS_Location Delete Post - " + innerExcp });
            }
        }

        //======= methods =======================================

        public ActionResult CheckForDuplicateRecord(string location_code = null, string location_name = null)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            
            if ((bool)Session["master"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            try
            {
                connection();
                SqlCommand com = new SqlCommand("CMS_Check_ForLocationDuplicateRecord", con);

                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@location_code", location_code);
                com.Parameters.AddWithValue("@location_name", location_name);
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();

                con.Open();
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    return Json("true", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("false", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "CMS_Location Duplicate - " + innerExcp });
            }
        }

        public ActionResult CheckForDuplicateRecordUpdate(string location_code = null, string location_name = null, Int64 ID = 0)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            
            if ((bool)Session["master"] == false)
            {
                int uid = (int)Session["uid"];
                UserMaster usrm = db.UserMasters.Find(uid);
                usrm.Active = false;
                db.SaveChanges();
                return RedirectToAction("Error", "Error", new { msg = "Session Expired", popmsg = "Malicious activity has been detected, your id has been disabled!!", id = 1 });
            }

            try
            {
                connection();
                SqlCommand com = new SqlCommand("CMS_Check_ForLocationDuplicateRecordUpdate", con);

                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@location_code", location_code);
                com.Parameters.AddWithValue("@location_name", location_name);
                com.Parameters.AddWithValue("@ID", ID);
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();

                con.Open();
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    return Json("true", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("false", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "CMS_Location Duplicate - " + innerExcp });
            }
        }

    }
}
