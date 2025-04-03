using System;
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
    public class sanmicrController : Controller
    {
        //
        // GET: /sanmicr/

        string cs = ConfigurationManager.ConnectionStrings["iKloudProConnectionString2"].ConnectionString;
        AflatoonEntities db = new AflatoonEntities();
        // private object sqlBulkCopy;

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

            List<SANMICRMaster> SANMICRMasterList = new List<SANMICRMaster>();
            SANMICRMaster SANMICRMaster = null;

            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("Get_SANMICRMaster", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;



                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SANMICRMaster = new SANMICRMaster();
                        SANMICRMaster.ID = int.Parse(reader["ID"].ToString());

                        SANMICRMaster.MICR_Code = reader["MICR_Code"].ToString();
                        SANMICRMaster.SAN = reader["SAN"].ToString();

                        SANMICRMasterList.Add(SANMICRMaster);




                    }

                }
            }

            //return View();
            return View(SANMICRMasterList);
        }

        [HttpPost]
        public ActionResult AddSANMICR(string MICR_Code, string SAN)
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


            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("Add_SANMICRMaster", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@MICR", SqlDbType.VarChar).Value = MICR_Code;
                    cmd.Parameters.Add("@SAN", SqlDbType.VarChar).Value = SAN;

                    con.Open();
                    cmd.ExecuteNonQuery();

                }
            }

            return null;
            //return View("Index");

        }


        public ActionResult Deletesanmicr(int ID = 0)
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

            //var CustomerId = Convert.ToInt16(Session["CustomerID"]);
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("Delete_Deletesanmicr", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@Customerid", SqlDbType.Int).Value = CustomerId;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;


                    con.Open();
                    cmd.ExecuteNonQuery();

                }
            }

            return RedirectToAction("Index");
            //return View("Index");

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

            // var CustomerId = Convert.ToInt16(Session["CustomerID"]);
            SANMICRMaster SANMICRMaster = new SANMICRMaster();

            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("Get_SANMICRMasterById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // cmd.Parameters.Add("@Customerid", SqlDbType.Int).Value = CustomerId;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;


                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        SANMICRMaster.ID = int.Parse(reader["ID"].ToString());
                        SANMICRMaster.MICR_Code = reader["MICR_Code"].ToString();
                        SANMICRMaster.SAN = reader["SAN"].ToString();



                    }
                }
            }
            ViewBag.ID = SANMICRMaster.ID;
            ViewBag.MICR_Code = SANMICRMaster.MICR_Code;
            ViewBag.SAN = SANMICRMaster.SAN;
            return View();
        }

        [HttpPost]
        public ActionResult Savesanmicr(int ID, string MICR_Code, string SAN)
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

            // var CustomerId = Convert.ToInt16(Session["CustomerID"]);
            //var userid = Convert.ToInt16(Session["uid"]);
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("Update_sanmicr", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                    cmd.Parameters.Add("@MICR_Code", SqlDbType.VarChar).Value = MICR_Code;
                    cmd.Parameters.Add("@SAN", SqlDbType.VarChar).Value = SAN;

                    con.Open();
                    cmd.ExecuteNonQuery();

                }
            }

            return null;
            //return View("Index");

        }

        //-----------------For CSV-------------------------

        [HttpPost]
        public ActionResult IndexCSV(HttpPostedFileBase postedFile)
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

            string filePath = string.Empty;
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(filePath);

                //Create a DataTable.
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Account Number", typeof(string)),
                                new DataColumn("MICR", typeof(string)),
                                new DataColumn("SAN",typeof(string)) });


                //Read the contents of CSV file.
                string csvData = System.IO.File.ReadAllText(filePath);
                //List<string> objList = csvData.Split('\n').ToList();
                List<string> objList = csvData.Trim().Replace("\r", string.Empty).Replace("\"", string.Empty).Split('\n').ToList();

                for (int k = 1; k < objList.Count(); k++)
                {
                    //string[] objListRow = objList[k].Replace("\r", String.Empty).Split("\t".ToCharArray());
                    string[] objListRow = objList[k].Split(",".ToCharArray());
                    //if (objListRow[1] < 9 )
                    if (objListRow[1] != "" && objListRow[1].Length == 9)
                    {
                        if (objListRow[2] != "" && objListRow[2].Length < 7)
                        {
                            if ((objListRow[2].Length < 6))
                                objListRow[2] = objListRow[2].PadLeft(6, '0');
                            dt.Rows.Add(objListRow.ToArray());
                        }
                    }

                }

                //Execute a loop over the rows.
                //foreach (string row in csvData.Split('\n'))
                //{
                //    if (!string.IsNullOrEmpty(row))
                //    {
                //        dt.Rows.Add();
                //        int i = 0;

                //        //Execute a loop over the columns.
                //        foreach (string cell in row.Split(','))
                //        {
                //            dt.Rows[dt.Rows.Count - 1][i] = cell;
                //            i++;
                //        }
                //    }
                //}

                string conString = ConfigurationManager.ConnectionStrings["iKloudProConnectionString2"].ConnectionString;
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //Set the database table name.
                        sqlBulkCopy.DestinationTableName = "dbo.SANMICRMaster";

                        //[OPTIONAL]: Map the DataTable columns with that of the database table
                        sqlBulkCopy.ColumnMappings.Add("MICR", "MICR");
                        sqlBulkCopy.ColumnMappings.Add("SAN", "SAN");

                        con.Open();
                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                    }
                }
            }

            return RedirectToAction("Index");

        }
















    }
}
