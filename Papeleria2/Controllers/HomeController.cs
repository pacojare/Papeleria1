using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Papeleria2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["itemTotal"] == null)
                Session["itemTotal"] = 0;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}