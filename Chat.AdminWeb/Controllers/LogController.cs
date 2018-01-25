using Chat.AdminWeb.App_Start;
using Chat.AdminWeb.Models.Home;
using Chat.IService.Interface;
using Chat.WebCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chat.AdminWeb.Controllers
{
    public class LogController : Controller
    {
        public IAdminLogService logService { get; set; }

        [Permission("log")]
        public ActionResult Logs()
        {
            AdminLogSearchResult result = logService.GetPage(null, null, null, 0, 20);
            AdminLogsViewModel model = new AdminLogsViewModel();
            model.Logs = result.AdminLogs;
            //分页
            Pagination pager = new Pagination();
            pager.PageIndex = 1;
            pager.PageSize = 20;
            pager.TotalCount = result.TotalCount;

            if (result.TotalCount <= 20)
            {
                model.Page = "";
            }
            else
            {
                model.Page = pager.GetPagerHtml();
            }
            return View(model);
        }

        [HttpPost]
        [Permission("log")]
        public ActionResult Logs(DateTime? startTime, DateTime? endTime, string keyWord, int pageIndex)
        {
            AdminLogSearchResult result = logService.GetPage(startTime, endTime, keyWord, (pageIndex - 1) * 20, 20);
            AdminLogsViewModel model = new AdminLogsViewModel();
            model.Logs = result.AdminLogs;
            //分页
            Pagination pager = new Pagination();
            pager.PageIndex = pageIndex;
            pager.PageSize = 20;
            pager.TotalCount = result.TotalCount;

            if (result.TotalCount <= 20)
            {
                model.Page = "";
            }
            else
            {
                model.Page = pager.GetPagerHtml();
            }
            return Json(new AjaxResult { Status = "1", Data = model });
        }
    }
}