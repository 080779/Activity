using Chat.DTO.DTO;
using Chat.IService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chat.AdminWeb.Controllers
{
    public class AdminUserController : Controller
    {
        public IAdminUserService adminUserService { get; set; }
        public ActionResult Add()
        {
            return View();
        }

        public ActionResult List()
        {
            AdminUserDTO[] dtos= adminUserService.GetAll();
            return View(dtos);
        }
    }
}