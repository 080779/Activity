using Chat.AdminWeb.App_Start;
using Chat.AdminWeb.Models;
using Chat.AdminWeb.Models.AdminManager;
using Chat.DTO.DTO;
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
        public ISettingService settingService { get; set; }
        public IRoleService roleService { get; set; }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AdminUserLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = MVCHelper.GetValidMsg(ModelState) });
            }

            //settingService.UpdateValue("前端奖品图片地址", "http://104.151.50.99:8225");

            if (adminService.CheckLogin(model.Name, model.Password))
            {
                Session["AdminUserId"] = adminService.GetByName(model.Name).Id;
                return Json(new AjaxResult { Status = "redirect",Data="/testpaper/list" });
            }
            else
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "用户名密码错误" });
            }
        }

        [Permission("manager")]
        public ActionResult VuCe(AdminUserRegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = MVCHelper.GetValidMsg(ModelState) });
            }
            long id= adminService.AddAdminUser(model.Name, "", true, "aaa@qq.com", model.Password);
            if(id<=0)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "手机号已经存在" });
            }
            else
            {
                long[] roleids = { 1 };
                roleService.AddRoleIds(id,roleids);
                return Json(new AjaxResult { Status = "success"});
            }
        }

        public ActionResult Logout()
        {
            Session["AdminUserId"] = null;
            return Redirect("/home/login");
        }

        public ActionResult LoadManager()
        {
            long? id = (long?)Session["AdminUserId"];
            if(id==null)
            {
                id = 0;
            }
            AdminUserDTO dto= adminService.GetById((long)id);
            return Json(new AjaxResult { Status="success",Data=dto.Name});
        }

        [Permission("manager")]
        public ActionResult SetPicAdminUrl()
        {
            return View();
        }
        [Permission("manager")]
        [HttpPost]
        public ActionResult SetPicAdminUrl(string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "地址不能为空" });
            }
            if(!settingService.UpdateValue("前端奖品图片地址", value))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "地址设置不成功" });
            }
            return Json(new AjaxResult { Status = "success"});
        }

        public ActionResult Add()
        {
            AddAdminUserViewModel model = new AddAdminUserViewModel();
            model.Citys = roleService.GetByDescription("在培训活动管理-报名管理-按市级表格导入时，只能导入所在市，且不能导出汇总表格");
            model.Hall = roleService.GetByName("厅级管理员");
            return View(model);
        }
    }
}