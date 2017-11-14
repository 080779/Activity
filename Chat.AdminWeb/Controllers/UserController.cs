using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chat.AdminWeb.Controllers
{
    public class UserController : Controller
    {
        public ActionResult List()
        {
            return View();
        }
    }
}