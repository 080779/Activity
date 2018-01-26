using Chat.AdminWeb.App_Start;
using Chat.AdminWeb.Models.AdminManager;
using Chat.AdminWeb.Models.Home;
using Chat.AdminWeb.Models.Main;
using Chat.DTO.DTO;
using Chat.IService.Interface;
using Chat.WebCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Chat.AdminWeb.Controllers
{
    public class MainController : Controller
    {
        public IAdminUserService adminService { get; set; }
        public IRoleService roleService { get; set; }
        public IPermissionService perService { get; set; }
        public IIdNameService idNameservice { get; set; }

        [Permission("adminUser")]
        [ActDescription("后台用户管理列表")]
        public ActionResult List()
        {
            AdminUserSearchResult result = adminService.GetPage(null, null, null, 0, 20);
            AdminUserViewModel model = new AdminUserViewModel();
            string[] roleCities = new[] { "南宁市", "柳州市", "桂林市", "梧州市", "北海市", "防城港市", "钦州市", "玉林市", "贵港市", "百色市", "河池市", "贺州市", "来宾市", "崇左市", "厅机关处室、直属单位" };

            List<AdminUserListDTO> AdminUsers = new List<AdminUserListDTO>();
            foreach (var list in result.AdminUsers)
            {
                AdminUserListDTO dto = new AdminUserListDTO();
                dto.CreateDateTime = list.CreateDateTime;
                dto.Email = list.Email;
                dto.Gender = list.Gender;
                dto.Id = list.Id;
                dto.LastLoginErrorDateTime = list.LastLoginErrorDateTime;
                dto.Mobile = list.Mobile;
                dto.Name = list.Name;
                if (roleCities.Contains(list.Roles.First().Name.Split('-')[0]))
                {
                    dto.RoleName = "市级管理员";
                }
                else
                {
                    dto.RoleName = list.Roles.First().Name.Split('-')[0];
                }
                if (adminService.GetById(list.LoginErrorTimes) == null)
                {
                    dto.Creator = "amdin";
                }
                else
                {
                    dto.Creator = adminService.GetById(list.LoginErrorTimes).Name;
                }
                AdminUsers.Add(dto);
            }
            model.AdminUsers = AdminUsers;

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
        [Permission("adminUser")]
        public ActionResult list(DateTime? startTime, DateTime? endTime, string keyWord, int pageIndex)
        {
            AdminUserSearchResult result = adminService.GetPage(startTime, endTime, keyWord, (pageIndex - 1) * 20, 20);
            AdminUserViewModel model = new AdminUserViewModel();
            string[] roleCities = new[] { "南宁市", "柳州市", "桂林市", "梧州市", "北海市", "防城港市", "钦州市", "玉林市", "贵港市", "百色市", "河池市", "贺州市", "来宾市", "崇左市", "厅机关处室、直属单位" };

            List<AdminUserListDTO> AdminUsers = new List<AdminUserListDTO>();
            foreach (var list in result.AdminUsers)
            {
                AdminUserListDTO dto = new AdminUserListDTO();
                dto.CreateDateTime = list.CreateDateTime;
                dto.Email = list.Email;
                dto.Gender = list.Gender;
                dto.Id = list.Id;
                dto.LastLoginErrorDateTime = list.LastLoginErrorDateTime;
                dto.Mobile = list.Mobile;
                dto.Name = list.Name;
                if (roleCities.Contains(list.Roles.First().Name.Split('-')[0]))
                {
                    dto.RoleName = "市级管理员";
                }
                else
                {
                    dto.RoleName = list.Roles.First().Name.Split('-')[0];
                }
                if (adminService.GetById(list.LoginErrorTimes) == null)
                {
                    dto.Creator = "admin";
                }
                else
                {
                    dto.Creator = adminService.GetById(list.LoginErrorTimes).Name;
                }
                AdminUsers.Add(dto);
            }
            model.AdminUsers = AdminUsers;

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

        [Permission("adminUser")]
        public ActionResult Add()
        {
            AddAdminUserViewModel model = new AddAdminUserViewModel();
            model.Citys = idNameservice.GetAll("市级");
            model.Hall = roleService.GetByName("厅级管理员-");
            model.ActivityId = perService.GetByName("activity").Id;
            model.AdminUserId = perService.GetByName("adminUser").Id;
            model.EntryId = perService.GetByName("entry").Id;
            model.ListId = perService.GetByName("list").Id;
            model.LogId = perService.GetByName("log").Id;
            model.PaperId = perService.GetByName("paper").Id;
            model.UserId = perService.GetByName("user").Id;
            model.TrainId = perService.GetByName("train").Id;

            return View(model);
        }

        [Permission("adminUser")]
        [HttpPost]
        [ActDescription("创建新管理员")]
        public ActionResult Add(AddAdminUserModel model)
        {
            if(string.IsNullOrEmpty(model.Name))
            {
                return Json(new AjaxResult { Status = "0" ,ErrorMsg="管理员用户名不能为空"});
            }
            if(string.IsNullOrEmpty(model.Password))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "管理员密码不能为空" });
            }
            if(model.Password.Length<6)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "管理员密码比须大于6个字符" });
            }
            if(string.IsNullOrEmpty(model.RoleName))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "请选择所属市级" });
            }
            List<long> lists = new List<long>();
            for(int i=0;i< model.PermissionIds.Length;i++)
            {
                if(model.PermissionIds[i]==0)
                {
                    continue;
                }
                lists.Add(model.PermissionIds[i]);
            }
            int id = Convert.ToInt32(Session["AdminUserId"]);
            string description = roleService.GetByName(model.RoleName).Description;
            bool b=adminService.AddNew(id, model.Name, model.Password, model.RoleName,description, lists);
            if(!b)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "新增管理员用户失败" });
            }
            return Json(new AjaxResult { Status = "1" });
        }

        [Permission("adminUser")]
        public ActionResult Edit(long id)
        {
            EditAdminUserViewModel model = new EditAdminUserViewModel();
            model.Id = id;
            model.Citys = idNameservice.GetAll("市级");
            model.Hall = roleService.GetByName("厅级管理员-");
            model.ActivityId = perService.GetByName("activity").Id;
            model.AdminUserId = perService.GetByName("adminUser").Id;
            model.EntryId = perService.GetByName("entry").Id;
            model.ListId = perService.GetByName("list").Id;
            model.LogId = perService.GetByName("log").Id;
            model.PaperId = perService.GetByName("paper").Id;
            model.UserId = perService.GetByName("user").Id;
            model.TrainId = perService.GetByName("train").Id;
            RoleDTO dto= roleService.GetByAdminUserId(id).First();
            model.RoleName = dto.Name.Split('-')[0]+"-";
            List<long> lists = new List<long>();
            foreach(var per in perService.GetByRoleId(dto.Id))
            {
                lists.Add(per.Id);
            }
            model.PermissionIds = lists;
            return View(model);
        }
        
        [Permission("adminUser")]
        [HttpPost]
        [ActDescription("编辑管理员")]
        public ActionResult Edit(EditAdminUserModel model)
        {
            if(model.Id<=0)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "参数错误" });
            }
            if (string.IsNullOrEmpty(model.RoleName))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "请选择所属市级" });
            }
            List<long> lists = new List<long>();
            for (int i = 0; i < model.PermissionIds.Length; i++)
            {
                if (model.PermissionIds[i] == 0)
                {
                    continue;
                }
                lists.Add(model.PermissionIds[i]);
            }
            string description = roleService.GetByName(model.RoleName).Description;
            bool b = adminService.Update(model.Id, model.RoleName, description, lists);
            if (!b)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "编辑管理员用户失败" });
            }
            return Json(new AjaxResult { Status = "1" });
        }

        [Permission("adminUser")]
        [HttpPost]
        [ActDescription("删除管理员")]
        public ActionResult Delete(long id)
        {
            if(id<=0)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "参数错误" });
            }
            if(!adminService.MarkDeleted(id))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "后台管理员用户删除失败" });
            }
            return Json(new AjaxResult { Status ="1"});
        }
    }
}