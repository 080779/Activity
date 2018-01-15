using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chat.AdminWeb.Controllers
{
    public class TrainController : Controller
    {
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }
        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult EntryAdd()
        {
            return View();
        }

        public ActionResult EntryEdit()
        {
            return View();
        }

        /// <summary>
        /// 报名导入
        /// </summary>
        /// <returns></returns>
        public ActionResult EntryImport()
        {
            return View();
        }
    }
}