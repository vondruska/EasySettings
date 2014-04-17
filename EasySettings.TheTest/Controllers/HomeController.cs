using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasySettings.TheTest.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            return Content("It didn't blow up\n" + Current<MySettings>.Settings.MiniProfiler + "\n" + Current<MySettings>.Settings.MyTestEnum);
        }

    }
}
