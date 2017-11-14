using Chat.AdminWeb.App_Start;
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
    public class TestPaperController : Controller
    {
        public ITestPaperService testPaperService { get; set; }

        [Permission("list")]
        public ActionResult List()
        {
            TestPaperDTO[] dtos = testPaperService.GetAll();
            return View(dtos);
        }
        [Permission("manager")]
        public ActionResult Add(long testPaperId)
        {
            TestPaperDTO dto= testPaperService.GetById(testPaperId);
            return View(dto);
        }
        [Permission("manager")]
        public ActionResult AddPaper()
        {
            return View();
        }
        [Permission("manager")]
        [HttpPost]
        public ActionResult AddPaper(string testTitle)
        {
            if(string.IsNullOrEmpty(testTitle))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg="标题不能为空" });
            }
            long id = testPaperService.AddNew(testTitle,0);
            return Json(new AjaxResult { Status="success",Data=id});
        }
        [Permission("manager")]
        public ActionResult Edit()
        {
            return View();
        }
    }
}