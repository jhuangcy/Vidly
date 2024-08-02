using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Vidly2.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        // disable caching
        //[OutputCache(Duration = 0, VaryByParam = "*", NoStore = true)]

        // VaryByParam: list specific params, or * for all. this will cache for each param.
        [OutputCache(Duration = 50, Location = OutputCacheLocation.Server, VaryByParam = "*")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            //throw new Exception();
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}