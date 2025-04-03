using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ikloud_Aflatoon.Models;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using ikloud_Aflatoon.Infrastructure;

namespace ikloud_Aflatoon.Controllers
{
    //[OutputCache(Duration = 0)]
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        private AflatoonEntities af = new AflatoonEntities();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);

        
        public ActionResult IWIndex(int id = 0)
        {

            //int num2 = 0;

            //int num1 = 10 / num2;

            bool flg = true;
            if (TempData["flg"] != null)
            {
                flg = Convert.ToBoolean(TempData["flg"]);
                TempData.Remove("flg");
            }

            if (flg == false)
                ViewBag.userlogin = false;

            else
                ViewBag.userlogin = true;


            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }
            if (id == 1)
                ViewBag.msg = "No Records found for verification.!!..";

            if (id == 2)
                ViewBag.msg = "Data Entry screen Closed!!..";

            if (id == 3)
                ViewBag.msg = "Data Entry screen Closed!!..";
            if (id == 4)
                ViewBag.msg = "No Records found for verification.!!..";
            if (id == 5)
                ViewBag.msg = "No Record was found!!..";

            //================ New Changes on 09_02_2022_by_Amol====================
            //var IsBRF_MessageDisplay = "N";
            //ViewBag.IsBRF_MessageDisplay = IsBRF_MessageDisplay;
            var IsBRF_MessageDisplay = af.CommonSettings.FirstOrDefault((p) => p.AppName == "CTSCONFIG" && p.SettingName == "IsBRF_MessageDisplay")?.SettingValue;
            if (IsBRF_MessageDisplay == null || IsBRF_MessageDisplay != "")
            {
                ViewBag.IsBRF_MessageDisplay = "N";
            }
            else
            {
                
                if (IsBRF_MessageDisplay.ToUpper() == "Y")
                {
                    ViewBag.IsBRF_MessageDisplay = "Y";

                    int cid = Convert.ToInt32(Session["CustomerID"]);
                    string p1 = Convert.ToString(Session["processdate"]);
                    string pDate = Convert.ToDateTime(Session["processdate"].ToString()).ToString("yyyy-MM-dd");

                    SqlDataAdapter adp = new SqlDataAdapter("BrfAlert", con);
                    adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                    adp.SelectCommand.Parameters.Add("@pDate", SqlDbType.VarChar).Value = pDate;
                    adp.SelectCommand.Parameters.Add("@cid", SqlDbType.Int).Value = cid;

                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    var objectlst = new List<BrfAlert>();
                    BrfAlert def;

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        def = new BrfAlert
                        {
                            DateTimeUpload = Convert.ToString(ds.Tables[0].Rows[0]["DownloadedDateTime"])
                        };

                        string BrfDatetime = Convert.ToString(def.DateTimeUpload);
                        ViewBag.Initial = "Y";
                        ViewBag.ValMsg = BrfDatetime;
                    }
                    else
                    {
                        ViewBag.Initial = "N";
                        ViewBag.ValMsg = "N";
                    }
                }
                else
                {
                    ViewBag.IsBRF_MessageDisplay = "N";
                }
            }


            //==================END ================================================

            @Session["glob"] = true;
            //   id = 0;

            return View();
        }

        public class BrfAlert
        {
            public string DateTimeUpload { get; set; }
        }

        public void setSession()
        {
            int uid = Convert.ToInt16(Session["uid"]);
            var usmtr = af.UserMasters.Where(u => u.ID == uid).SingleOrDefault();
            if (usmtr != null)
            {
                //usmtr.sessionid = Session.SessionID;
                // usmtr.LastLogin = Convert.ToDateTime(DateTime.Now.ToString(format));
                //usmtr.loginFlg = 0;
                usmtr.sessionid = Session.SessionID;
                usmtr.loginFlg = 1;
                usmtr.InvalidPasswordAttempts = 0;
                af.Entry(usmtr).State = EntityState.Modified;
                af.SaveChanges();
                Session["afterlogin"] = true;
            }
        }

        public bool ValidateSession()
        {

            if (Session["uid"] == null)
                return false;

            int uid = (int)Session["uid"];
            var model = af.UserMasters.Find(uid);
            if (model.sessionid != Session.SessionID)
            {

                return false;
            }
            else
                return true;

        }

        //public ActionResult SQSelection()
        //{
        //    return View();
        //}
    }
}
