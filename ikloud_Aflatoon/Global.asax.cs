using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace ikloud_Aflatoon
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            //-----------Added on ---22-03-2017----
            MvcHandler.DisableMvcResponseHeader = true;//------------Preventing response headers


            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalFilters.Filters.Add(new HandleErrorAttribute());

        }
        //-----------Added on ---22-03-2017----
        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    Exception exception = Server.GetLastError();
        //    Server.ClearError();
        //    Response.Clear();
        //    Response.Flush();

        //  //  Response.RedirectToRoute("Error");
        //    Response.RedirectToRoute("Default");
        //}
        //-----------Added on ---22-03-2017----
        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            if (app != null && app.Context != null)
            {
                HttpContext.Current.Response.Headers.Remove("Server");
                // HttpContext.Current.Response.Headers.Remove("X-Frame-Options");
                //  Response.AddHeader("X-Frame-Options","");
                HttpContext.Current.Response.Headers.Set("Server", "My http server");
                HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
                HttpContext.Current.Response.Headers.Remove("X-AspNetMvc-Version");
            }
        }

        protected void Application_BeginRequest()
        {
            //Response.AddHeader("X-Frame-Options", "DENY");
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            //Response.Cache.SetNoStore();
        }
        //protected void Application_EndRequest()
        //{
        //    HttpContext.Current.Response.Headers.Remove("Server");
        //    //if (Context.Response.StatusCode == 404)
        //    //{
        //    //    if ((!Request.RawUrl.Contains("style")) && (!Request.RawUrl.Contains("images")))
        //    //    {
        //    //        Response.Clear();
        //    //        if (Response.StatusCode == 404)
        //    //        {
        //    //            Response.Redirect("/Error/NotFound");
        //    //        }
        //    //    }
        //    //}
        //}

        protected void Application_Error()
        {
            Exception exception = Server.GetLastError();
            Response.Clear();
            HttpException httpException = exception as HttpException;

            if (httpException != null)
            {
                string action;

                switch (httpException.GetHttpCode())
                {
                    case 404:
                        action = "NotFound";
                        break;
                    case 500:
                        action = "InternalServerError";
                        break;
                    default:
                        action = "General";
                        break;
                }

                // Clear the error on server
                Server.ClearError();

                // Redirect to the appropriate error page
                Response.Redirect($"~/Error/{action}");
            }
            else
            {
                // If the exception is not an HttpException, show the general error page
                Server.ClearError();
                Response.Redirect("~/Error/General");
            }
        }

    }
}