using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Papeleria2.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ContentResult Loggeado()
        {
            return Content("Sólo los usuarios loggeados pueden acceder");
        }

        public ContentResult Anonimo()
        {
            return Content("Sólo los usuarios anónimos pueden acceder");
        }
    }
}