using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasySettings.Demo.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            return Content("It didn't blow up\nMiniProfiler: " + Current<MySettings>.Settings.MiniProfiler + "\n" + Current<MySettings>.Settings.MyTestEnum + "\n\nVisit /settings.axd for settings page", "text");
        }

    }
}
