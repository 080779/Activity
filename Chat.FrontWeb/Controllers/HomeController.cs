using Chat.DTO.DTO;
using Chat.FrontWeb.Models;
using Chat.IService.Interface;
using Chat.WebCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chat.FrontWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActivityService activityService { get; set; }
        public IExercisesService exeService { get; set; }
        public IUserService userService { get; set; }

        public ActionResult Index()
        {
            ActivityViewModel model = new ActivityViewModel();
            model.Activity= activityService.GetByStatus("答题进行中");
            return View(model);
        }

        public ActionResult Answer()
        {
            AnswerViewModel model = new AnswerViewModel();
            ActivityDTO activity = activityService.GetByStatus("答题进行中");
            model.ActivityName = activity.Name;
            model.Exercises= exeService.GetExercisesByPaperId(activity.PaperId);
            return View(model);
        }

        public ActionResult Topic()
        {
            TopicModel model = new TopicModel();
            ActivityDTO activity = activityService.GetByStatus("答题进行中");
            model.ActivityName = activity.Name;
            var exetips = exeService.GetExercisesByPaperId(activity.PaperId);
            List<string> lists = new List<string>();
            foreach(var exetip in exetips)
            {
                lists.Add(exetip.Tip);
            }
            model.ExesTip = lists;
            return View();
        }

        public ActionResult Prize()
        {
            PrizeViewModel model = new PrizeViewModel();
            ActivityDTO activity= activityService.GetByStatus("答题进行中");
            model.ActivityName = activity.Name;
            model.PrizeName = activity.PrizeName;
            model.PrizeImgUrl = activity.PrizeImgUrl;
            var users = userService.GetByActivityIdIsWon(activity.Id);
            List<IsWonUser> winUsers = new List<IsWonUser>();            
            foreach (var user in users)
            {
                IsWonUser winUser = new IsWonUser();
                winUser.UserName = user.Name;
                winUser.Mobile = CommonHelper.FormatMoblie(user.Mobile);
                winUsers.Add(winUser);
            }
            model.Users = winUsers;
            model.winCount = winUsers.Count();
            return View(model);
        }

        public ActionResult Result()
        {
            return View();
        }
    }
}