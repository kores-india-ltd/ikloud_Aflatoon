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
    public class holidayController : Controller
    {
        //
        // GET: /holiday/
        string cs = ConfigurationManager.ConnectionStrings["iKloudProConnectionString2"].ConnectionString;
        AflatoonEntities db = new AflatoonEntities();

        public ActionResult Index()
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

            var CustomerId = Convert.ToInt16(Session["CustomerID"]);

            List<HolidayMaster> HolidayMasterList = new List<HolidayMaster>();
            HolidayMaster HolidayMaster = null;

            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("Get_HolidayMaster", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Customerid", SqlDbType.Int).Value = CustomerId;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        HolidayMaster = new HolidayMaster();
                        HolidayMaster.ID = int.Parse(reader["ID"].ToString());
                        HolidayMaster.Customerid = int.Parse(reader["Customerid"].ToString());
                        HolidayMaster.Holiday_Date = Convert.ToDateTime(reader["Holiday_Date"]).ToString("yyyy-MM-dd");
                        HolidayMaster.Holiday_Descripton = reader["Holiday_Descripton"].ToString();
                        HolidayMaster.Created_by = int.Parse(reader["Created_by"].ToString());
                        HolidayMaster.Created_on = reader["Created_on"].ToString();
                        //by me 
                        HolidayMaster.Modified_by = int.Parse(reader["Modified_by"].ToString());
                        HolidayMaster.Modified_on = reader["Modified_on"].ToString();
                        HolidayMasterList.Add(HolidayMaster);




                    }

                }
            }
            return View(HolidayMasterList);
        }

        public ActionResult Edit(int ID = 0)
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

            var CustomerId = Convert.ToInt16(Session["CustomerID"]);
            HolidayMaster HolidayMaster = new HolidayMaster();

            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("Get_HolidayMasterById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Customerid", SqlDbType.Int).Value = CustomerId;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;


                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        HolidayMaster.ID = int.Parse(reader["ID"].ToString());
                        HolidayMaster.Customerid = int.Parse(reader["Customerid"].ToString());
                        HolidayMaster.Holiday_Date = Convert.ToDateTime(reader["Holiday_Date"]).ToString("yyyy-MM-dd");
                        HolidayMaster.Holiday_Descripton = reader["Holiday_Descripton"].ToString();
                        HolidayMaster.Created_by = int.Parse(reader["Created_by"].ToString());
                        HolidayMaster.Created_on = reader["Created_on"].ToString();
                        HolidayMaster.Modified_by = int.Parse(reader["Modified_by"].ToString());
                        HolidayMaster.Modified_on = reader["Modified_on"].ToString();

                        //HolidayMaster.ID = int.Parse(reader["ID"].ToString());
                        //HolidayMaster.Customerid = int.Parse(reader["Customerid"].ToString());
                        //HolidayMaster.Holiday_Date = Convert.ToDateTime(reader["Holiday_Date"]).ToString("yyyy-MM-dd");
                        //HolidayMaster.Holiday_Descripton = reader["Holiday_Descripton"].ToString();
                        //HolidayMaster.Created_by = int.Parse(reader["Created_by"].ToString());
                        //HolidayMaster.Created_on = reader["Created_on"].ToString();
                        //if (HolidayMaster.Modified_by != 0)
                        //{
                        //    HolidayMaster.Modified_by = int.Parse(reader["Modified_by"].ToString());
                        //}
                        //else
                        //{
                        //    HolidayMaster.Modified_by = 0;
                        //}
                        //HolidayMaster.Modified_on = reader["Modified_on"].ToString();
                    }
                }
            }
            ViewBag.ID = HolidayMaster.ID;
            ViewBag.Holiday_Date = HolidayMaster.Holiday_Date;
            ViewBag.Holiday_Descripton = HolidayMaster.Holiday_Descripton;
            return View();
        }

        [HttpPost]
        public ActionResult SaveEdit(int ID, string Holiday_Date, string Holiday_Descripton)
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

            var CustomerId = Convert.ToInt16(Session["CustomerID"]);
            var userid = Convert.ToInt16(Session["uid"]);
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("Update_HolidayMaster", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@userid", SqlDbType.VarChar).Value = userid;
                    cmd.Parameters.Add("@Customerid", SqlDbType.Int).Value = CustomerId;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                    cmd.Parameters.Add("@Holiday_Date", SqlDbType.VarChar).Value = Holiday_Date;
                    cmd.Parameters.Add("@Holiday_Descripton", SqlDbType.VarChar).Value = Holiday_Descripton;

                    con.Open();
                    cmd.ExecuteNonQuery();

                }
            }

            return null;
            //return View("Index");

        }

        [HttpPost]
        public ActionResult AddHoliday(string Holiday_Date, string Holiday_Descripton)
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

            var CustomerId = Convert.ToInt16(Session["CustomerID"]);
            var userid = Convert.ToInt16(Session["uid"]);
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("Add_HolidayMaster", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@userid", SqlDbType.VarChar).Value = userid;
                    cmd.Parameters.Add("@Customerid", SqlDbType.Int).Value = CustomerId;
                    cmd.Parameters.Add("@Holiday_Date", SqlDbType.VarChar).Value = Holiday_Date;
                    cmd.Parameters.Add("@Holiday_Descripton", SqlDbType.VarChar).Value = Holiday_Descripton;

                    con.Open();
                    cmd.ExecuteNonQuery();

                }
            }

            return null;
            //return View("Index");

        }


        public ActionResult DeleteHoliday(int ID = 0)
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

            var CustomerId = Convert.ToInt16(Session["CustomerID"]);
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("Delete_HolidayMaster", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Customerid", SqlDbType.Int).Value = CustomerId;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;


                    con.Open();
                    cmd.ExecuteNonQuery();

                }
            }

            return RedirectToAction("Index");
            //return View("Index");

        }


        //--------------------------------------------anandi holiday ------------------------------------



        public JsonResult ExistinHolidayMaster(DateTime procdate)
        {
            string flg = "";
            //IFormatProvider culture = new CultureInfo("en-US", true);

            //DateTime dateVal = DateTime.ParseExact(procdate, "yyyy-MM-dd", culture);
            string dateVal = procdate.ToString("yyyy-MM-dd");
            var CustomerId = Convert.ToInt16(Session["CustomerID"]);

            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("ExistinHolidayMaster", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Customerid", SqlDbType.Int).Value = CustomerId;
                    cmd.Parameters.Add("@ProcessingDate", SqlDbType.VarChar).Value = dateVal;

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        flg = reader["HolidayMasterExists"].ToString();
                    }
                }
            }

            return Json(flg, JsonRequestBehavior.AllowGet);

        }


        //---------------------------------------------anandi holiday-------------------------------------------
    }
}
