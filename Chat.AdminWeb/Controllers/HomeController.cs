using Chat.AdminWeb.Models;
using Chat.IService.Interface;
using Chat.WebCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chat.AdminWeb.Controllers
{
    public class HomeController : Controller
    {
        public IAdminUserService adminService { get; set; }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = MVCHelper.GetValidMsg(ModelState) });
            }
            if (model.VerifyCode != (string)TempData["verifyCode"])
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "验证码错误" });
            }
            if (adminService.CheckLogin(model.Name, model.Password))
            {
                Session["AdminUserId"] = adminService.GetByName(model.Name).Id;
                return Json(new AjaxResult { Status = "ok" });
            }
            else
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "用户名密码错误" });
            }
        }
    }
}