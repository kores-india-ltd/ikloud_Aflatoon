using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace ikloud_Aflatoon.Controllers
{
    public class IwVerSelectionController : Controller
    {
        //
        // GET: /IwVerSelection/
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AflatoonConnectionString"].ConnectionString);
        public ActionResult Index(string Flg="",int id=0)
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("Error", "Error", new { msg = "Session Expired" });
            }


            Session["FlagForVerification"] = Flg;
            ViewBag.SessionExpiryTime = Session["SessionExpiryTime_dropDown"];
            var amtrange = GetAmountRange();

            var amtSelection = amtrange.Select(x => new SelectListItem
            {
                Value = x.FromAmount.ToString()+"-"+x.ToAmount.ToString(),
                Text = x.Description.ToString(),
            }).ToList();

            ViewBag.AmtSelection = amtSelection;


            if (id == 1)
                ViewBag.msg = "No Record was found!!..";




            return View();
        }


        [HttpPost]
        public ActionResult SendData() 
        {
            var ControllerUrl = "";
            try
            {
                if (Request.Form["AmtSelection"] != null)
                {
                    Session["iwAmtSelection"] = Request.Form["AmtSelection"].ToString();
                }
                else
                {
                    Session["iwAmtSelection"] = "0";
                }

                if(Request.Form["SessionExpiryTime"] != null)
                {
                    Session["iwSessionExpiryTime"] = Request.Form["SessionExpiryTime"].ToString();
                }
                else
                {
                    Session["iwSessionExpiryTime"] = "00:00";
                }

                if(Request.Form["queue"] != null)
                {
                    Session["iwqueue"] = Request.Form["queue"].ToString();
                }
                else
                {
                    Session["iwqueue"] = "Normal";
                }

                if(Request.Form["flghdId"] != null)
                {
                    Session["iwFlag"] = Request.Form["flghdId"].ToString();
                }

                var urlflg = Session["iwFlag"].ToString();
               
                if (urlflg == "L1") { ControllerUrl = "IWL1"; }else if (urlflg == "L2") { ControllerUrl = "IWL2_New"; } else { ControllerUrl = "IWL3"; }

            }
            catch (Exception e)
            {

                string message = "";
                string innerExcp = "";
                string trace = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                {
                    innerExcp = e.InnerException.Message;
                    trace = e.InnerException.StackTrace;
                }
                logerror("In SendData Catch==>>" + message, "InnerExp===>>" + innerExcp);
            }
          

            return RedirectToAction("Index", ControllerUrl);

        
        }











        public List<GetAmount> GetAmountRange()
        {
            List<GetAmount> list = new List<GetAmount>();

            try
            {
                using (SqlCommand cmd = new SqlCommand("GetAmountRange", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            GetAmount lstObj = new GetAmount
                            {
                                Id = Convert.ToInt32(rdr["Id"]),
                                Description = rdr["Description"].ToString(),
                                FromAmount = rdr["FromAmount"].ToString(),
                                ToAmount = rdr["ToAmount"].ToString()
                            };
                            list.Add(lstObj);
                        }
                    }
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

                // logerror("In GetExpiryTime Catch==>" + message, "InnerExp===>" + innerExcp);
            }

            return list;
        }

        public class GetAmount
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public string FromAmount { get; set; }
            public string ToAmount { get; set; }
        }



        private void logerror(string errormsg, string errordesc)
        {
            //ErrorDisplay er = new ErrorDisplay();
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


    }

   

}
