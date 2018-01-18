using Chat.DTO.DTO;
using Chat.IService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chat.FrontWeb.Controllers
{
    public class TrainController : Controller
    {
        public ITrainService trainService { get; set; }

        public ActionResult List()
        {
            TrainDTO[] dtos= trainService.GetAll();
            return View(dtos);
        }
        public ActionResult Details(long id)
        {
            TrainDTO dto = trainService.GetById(id);
            return View(dto);
        }
    }
}