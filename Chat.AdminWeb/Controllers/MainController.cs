using CaptchaGen;
using Chat.AdminWeb.App_Start;
using Chat.AdminWeb.Models;
using Chat.IService.Interface;
using Chat.Service;
using Chat.WebCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chat.AdminWeb.Controllers
{       
    public class MainController : Controller
    {
        public IAdminUserService adminService { get; set; }
        [Permission("check")]
        public ActionResult Index()
        {
            return View();
        }

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

        public ActionResult CreateVerify()
        {
            string verifyCode = CommonHelper.GetCaptcha(4);
            TempData["verifyCode"] = verifyCode;
            MemoryStream ms = ImageFactory.GenerateImage(verifyCode, 45, 70, 15, 2); 
            return File(ms, "image/jpeg");
        }
    }
}