using System.Web;
using System.Web.Mvc;
using ikloud_Aflatoon.Controllers;

namespace ikloud_Aflatoon
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}