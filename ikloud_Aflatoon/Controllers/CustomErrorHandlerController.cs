using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ikloud_Aflatoon.Controllers
{
    public class CustomErrorHandlerController : HandleErrorAttribute
    {
        //
        // GET: /CustomErrorHandler/

        public override void OnException(ExceptionContext filterContext)
        {
            Exception e = filterContext.Exception;
            filterContext.ExceptionHandled = true;
            var result = new ViewResult();
            {
              //  ViewName = "Error";
            }; ;
            result.ViewBag.Error = "Error Occur While Processing Your Request Please Check After Some Time";
            filterContext.Result = result;

            //public ActionResult Index()
            //{
            //    return View();
            //}

        }
    }
}
