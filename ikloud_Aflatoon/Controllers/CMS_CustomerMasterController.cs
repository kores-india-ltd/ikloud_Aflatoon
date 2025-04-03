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
    public class CMS_CustomerMasterController : Controller
    {
        //
        // GET: /CMS_CustomerMaster/
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
                CMS_CustomerMasterRepository CustRepo = new CMS_CustomerMasterRepository();
                ModelState.Clear();

                if(id == 1)
                {
                    ViewBag.Message = "CMS_Customer Data added successfully";
                }

                if (id == 2)
                {
                    ViewBag.Message = "CMS_Customer Data updated successfully";
                }

                if (id == 3)
                {
                    ViewBag.Message = "CMS_Customer Data deleted successfully";
                }

                return View(CustRepo.GetAllCustomerMasterData());
            }
            catch(Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "CMS_Customer Index- " + innerExcp });
            }
            
        }

        // GET: Customer
        public ActionResult AddCustomer()
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
        public ActionResult AddCustomer(CMS_CustomerMaster_DBS_View obj)
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
                    CMS_CustomerMasterRepository CustRepo = new CMS_CustomerMasterRepository();
                    CustRepo.AddCustomerMasterData(obj, loginId);
                    
                    //return View("Index");
                    return RedirectToAction("Index", "CMS_CustomerMaster", new { id = 1 });
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

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "CMS_Customer Add Post - " + innerExcp });
            }
        }

        public ActionResult EditCMS_CustomerData(Int64 id)
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
                CMS_CustomerMasterRepository CustRepo = new CMS_CustomerMasterRepository();
                return View(CustRepo.GetAllCustomerMasterData().Find(cust => cust.ID == id));
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "CMS_Customer Edit get - " + innerExcp });
            }

        }

        [HttpPost]
        public ActionResult EditCMS_CustomerData(Int64 id, CMS_CustomerMaster_DBS_View obj)
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
                CMS_CustomerMasterRepository CustRepo = new CMS_CustomerMasterRepository();
                CustRepo.UpdateCustomerMasterData(obj, loginId);
                
                
                //return RedirectToAction("Index");
                return RedirectToAction("Index", "CMS_CustomerMaster", new { id = 2 });

            }
            catch(Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "CMS_Customer Edit Post - " + innerExcp });
            }
        }

        // GET: Employee/DeleteEmp/5    
        public ActionResult DeleteCMS_CustomerData(Int64 Id)
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
                CMS_CustomerMasterRepository CustRepo = new CMS_CustomerMasterRepository();
                CustRepo.DeleteCustomerMasterData(Id);
                
                //return RedirectToAction("Index");
                return RedirectToAction("Index", "CMS_CustomerMaster", new { id = 3 });

            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "CMS_Customer Delete Post - " + innerExcp });
            }
        }


        //======= methods =======================================
        
        public ActionResult CheckForDuplicateRecord(string customer_code = null, string customer_name = null)
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
                SqlCommand com = new SqlCommand("CMS_Check_ForCustomerDuplicateRecord", con);

                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@customer_code", customer_code);
                com.Parameters.AddWithValue("@customer_name", customer_name);
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
            catch(Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "CMS_Customer Duplicate - " + innerExcp });
            }
        }

        public ActionResult CheckForDuplicateRecordUpdate(string customer_code = null, string customer_name = null, Int64 ID = 0)
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
                SqlCommand com = new SqlCommand("CMS_Check_ForCustomerDuplicateRecordUpdate", con);

                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@customer_code", customer_code);
                com.Parameters.AddWithValue("@customer_name", customer_name);
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

                return RedirectToAction("Error", "Error", new { msg = message, popmsg = "CMS_Customer Duplicate - " + innerExcp });
            }
        }

    }
}
