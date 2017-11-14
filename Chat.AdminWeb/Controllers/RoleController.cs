using Chat.DTO.DTO;
using Chat.IService.Interface;
using Chat.WebCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Chat.AdminWeb.Controllers
{
    public class RoleController : Controller
    {
        //public IAdminUserService adminUserService { get; set; }
        public IRoleService roleService { get; set; }
        public ActionResult List()
        {            
            RoleDTO[] roles= roleService.GetAll();
            return View(roles);
        }
        public ActionResult load(long adminUserId)
        {
            RoleDTO[] roles= roleService.GetByAdminUserId(adminUserId);
            return Json(new AjaxResult { Status="ok",Data=roles});
        }
    }
}