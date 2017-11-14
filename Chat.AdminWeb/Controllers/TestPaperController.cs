using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chat.AdminWeb.Controllers
{
    public class TestPaperController : Controller
    {
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }
    }
}