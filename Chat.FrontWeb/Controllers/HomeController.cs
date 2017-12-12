using Chat.DTO.DTO;
using Chat.FrontWeb.Models;
using Chat.FrontWeb.Models.user;
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
        public ISettingService settingService { get; set; }

        public ActionResult Index()
        {
            ActivityViewModel model = new ActivityViewModel();
            ActivityDTO activity= activityService.GetIsCurrent();
            if(activity==null)
            {
                model.Id = 0;
                model.Name = "当前暂无活动";
                model.Description = "当前暂无活动简介";
                model.StartTime = "****-**-**";
                model.ExamEndTime = "****-**-**";
                model.RewardTime = "****-**-**";
                model.AnswerCount = 0;
                model.StatusName = "无活动";
            }
            else
            {
                model.Id = activity.Id;
                model.Name = activity.Name;
                model.Description = activity.Description;
                model.StartTime = activity.StartTime.ToString("yyyy-MM-dd HH:mm");
                model.ExamEndTime = activity.ExamEndTime.ToString("yyyy-MM-dd HH:mm");
                model.RewardTime = activity.RewardTime.ToString("yyyy-MM-dd HH:mm");
                model.AnswerCount = activity.AnswerCount;
                model.StatusName = activity.StatusName;
            }            

            activityService.UpdateCount(model.Id, true, false, false, false, false);
            return View(model);
        }

        public ActionResult Judge(string typeName,string statusName,long id= 0)
        {
            if (id == 0)
            {
                return Json(new AjaxResult { Status = "nonExist", ErrorMsg = "当前暂无活动" });
            }
            if (statusName=="无活动")
            {
                return Json(new AjaxResult { Status = "nonExist", ErrorMsg = "当前暂无活动" });
            }
            if(statusName=="待开始")
            {
                if (typeName == "topic")
                {
                    return Json(new AjaxResult { Status = "redirect", Data = "/home/topic?id=" + id });
                }
                if (typeName == "answer")
                {
                    return Json(new AjaxResult { Status = "tip", ErrorMsg="活动未开始"});
                }
                if (typeName == "prize")
                {
                    return Json(new AjaxResult { Status = "tip", ErrorMsg = "活动未开始" });
                }
            }
            if (statusName == "答题进行中")
            {
                if (typeName == "topic")
                {
                    return Json(new AjaxResult { Status = "redirect", Data = "/home/topic?id=" + id });
                }
                if (typeName == "answer")
                {
                    return Json(new AjaxResult { Status = "redirect", Data = "/home/answer?id=" + id });
                }
                if (typeName == "prize")
                {
                    return Json(new AjaxResult { Status = "redirect", Data = "/home/prize?id=" + id });
                }
            }
            if (statusName == "待开奖")
            {
                if (typeName == "topic")
                {
                    return Json(new AjaxResult { Status = "redirect", Data = "/home/topic?id=" + id });
                }
                if (typeName == "answer")
                {
                    return Json(new AjaxResult { Status = "tip", ErrorMsg = "待开奖" });
                }
                if (typeName == "prize")
                {
                    return Json(new AjaxResult { Status = "redirect", Data = "/home/prize?id=" + id });
                }
            }
            if (statusName == "已结束")
            {
                if (typeName == "topic")
                {
                    return Json(new AjaxResult { Status = "redirect", Data = "/home/topic?id=" + id });
                }
                if (typeName == "answer")
                {
                    return Json(new AjaxResult { Status = "tip", ErrorMsg = "已结束" });
                }
                if (typeName == "prize")
                {
                    return Json(new AjaxResult { Status = "redirect", Data = "/home/prize?id=" + id });
                }
            }
            return Json(new AjaxResult { Status="success"});
        }

        public ActionResult Answer(long id=0)
        {
            AnswerViewModel model = new AnswerViewModel();
            ActivityDTO activity=new ActivityDTO();
            if (id==0)
            {
                return Redirect("/home/index");
            }            
            if (!activityService.ExistActivity(id))
            {
                return Redirect("/home/index");
            }
            else if (!activityService.IsCurrentAct(id))
            {
                return Redirect("/home/index");
            }
            else
            {
                activity = activityService.GetById(id);                
            }
            if(activity.StatusId==5 || activity.StatusId==7 || activity.StatusId==8)
            {
                return Redirect("/home/index");
            }
            model.ActivityName = activity.Name;
            model.Exercises = exeService.GetExercisesByPaperId(activity.PaperId);
            model.Id = activity.Id;
            model.ImgUrl = activity.ImgUrl;
            model.FirstUrl = settingService.GetValue("前端奖品图片地址");
            Session["IsFirst"] = true;
            return View(model);
        }

        public ActionResult Topic(long id=0,string topic="home")
        {
            TopicModel model = new TopicModel();
            ActivityDTO activity=new ActivityDTO();
            if (id == 0)
            {
                return Redirect("/home/index");
            }
            if (!activityService.ExistActivity(id))
            {
                return Redirect("/home/index");
            }
            else if (!activityService.IsCurrentAct(id))
            {
                return Redirect("/home/index");
            }
            else
            {
                activity = activityService.GetById(id);
            }
            model.ActivityName = activity.Name;
            var exetips = exeService.GetExercisesByPaperId(activity.PaperId);
            List<string> lists = new List<string>();
            foreach(var exetip in exetips)
            {
                lists.Add(exetip.Tip);
            }
            model.Topic = topic;
            model.ExesTip = lists;
            return View(model);
        }

        public ActionResult Prize(long id=0)
        {
            PrizeViewModel model = new PrizeViewModel();
            ActivityDTO activity=new ActivityDTO();
            if (id == 0)
            {
                return Redirect("/home/index");
            }
            if (!activityService.ExistActivity(id))
            {
                return Redirect("/home/index");
            }
            else if (!activityService.IsCurrentAct(id))
            {
                return Redirect("/home/index");
            }
            else
            {
                activity = activityService.GetById(id);
            }
            model.ActivityId = activity.Id;
            model.ActivityName = activity.Name;
            model.PrizeName = activity.PrizeName;
            model.PrizeImgUrl = activity.PrizeImgUrl;
            model.PrizeTime = activity.RewardTime.ToString("yyyy-MM-dd HH:mm");
            model.PrizeFirstUrl = settingService.GetValue("前端奖品图片地址");
            model.StatusName = activity.StatusName;
            //var users = userService.GetByActivityIdIsWon1(activity.Id);
            if(activity.RewardTime<=DateTime.Now)
            {
                UserSearchResult result = userService.GetByActivityIdIsWon(activity.Id, null, null);
                List<IsWonUser> winUsers = new List<IsWonUser>();
                foreach (var user in result.Users)
                {
                    IsWonUser winUser = new IsWonUser();
                    winUser.UserName = CommonHelper.FormatUserName(user.Name);
                    winUser.Mobile = CommonHelper.FormatMoblie(user.Mobile);
                    winUser.Gender = user.Gender ? "先生" : "女士";
                    winUsers.Add(winUser);
                }
                model.Users = winUsers;
                model.winCount = result.TotalCount;
            }
            else
            {
                model.winCount = 0;
            }
            //string mobile = (string)Session["Mobile"];
            //model.UserIsWon = userService.UserIsWonByMobile(mobile);
            return View(model);
        }

        public ActionResult PrizeUserSearch(string lastM,long id=0)
        {
            if(id==0)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "活动不存在" });
            }
            var act= activityService.GetById(id);
            if(act==null)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "活动不存在" });
            }
            if(act.StatusName!= "已结束")
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "活动尚未开奖" });
            }
            if (string.IsNullOrEmpty(lastM))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "手机号尾数不能为空" });
            }
            long lastm;
            bool b= long.TryParse(lastM, out lastm);
            if(!b)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "手机号码必须是数字" });
            }
            if(lastM.Length!=4)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "请输入四位手机尾数" });
            }
            var users= userService.SearchIsWon(id,lastM,0,10);
            if(users==null)
            {
                return Json(new AjaxResult { Status = "error",ErrorMsg="活动不存在" });
            }
            List<SearchUser> lists = new List<SearchUser>();
            foreach(var user in users.Users)
            {
                SearchUser item = new SearchUser();
                item.UserName = CommonHelper.FormatUserName(user.Name);
                item.Mobile = CommonHelper.FormatMoblie(user.Mobile);
                item.Gender = user.Gender ? "先生" : "女士";
                lists.Add(item);
            }
            return Json(new AjaxResult { Status="success",Data=lists});
        }

        public ActionResult Result(string asks,long id=0)
        {
            ActivityDTO activity = new ActivityDTO();
            if (id == 0)
            {
                return Redirect("/home/index");
            }
            if (!activityService.ExistActivity(id))
            {
                return Redirect("/home/index");
            }
            else if (!activityService.IsCurrentAct(id))
            {
                return Redirect("/home/index");
            }
            else
            {
                activity = activityService.GetById(id);
            }
            if (activity.StatusId==5 || activity.StatusId == 7 || activity.StatusId == 8)
            {
                return Redirect("/home/index");
            }
            if (Session["IsFirst"]==null)
            {
                return Redirect("/home/index");
            }
            if((bool)Session["IsFirst"])
            {
                if (string.IsNullOrEmpty(asks))
                {
                    return Content("请答完题再提交");
                }
                ResultModel model = new ResultModel();
                model.ActivityName = activity.Name;
                model.Id = activity.Id;
                model.PrizeTime = activity.RewardTime.ToString("yyyy-MM-dd HH:mm");
                long paperId = activity.PaperId;
                string[] strs = asks.Trim(',').Split(',');
                List<string> lists = new List<string>();
                int count = 0;
                foreach (string str in strs)
                {
                    string[] results = str.Split(':');
                    bool b = exeService.IsRightOrWrong(paperId, Convert.ToInt64(results[0]), Convert.ToInt64(results[1]));
                    if (b)
                    {
                        count++;
                    }
                    lists.Add(b ? "right" : "wrong");
                }
                model.IsAllRight = count == strs.Count();
                model.Result = lists;
                model.IsFirst = true;
                return View(model);
            }
            else
            {
                if (string.IsNullOrEmpty(asks))
                {
                    return Content("请答完题再提交");
                }
                ResultModel model = new ResultModel();
                model.ActivityName = activity.Name;
                model.Id = activity.Id;
                model.PrizeTime = activity.RewardTime.ToString("yyyy-MM-dd HH:mm");
                long paperId = activity.PaperId;
                string[] strs = asks.Trim(',').Split(',');
                List<string> lists = new List<string>();
                int count = 0;
                foreach (string str in strs)
                {
                    string[] results = str.Split(':');
                    bool b = exeService.IsRightOrWrong(paperId, Convert.ToInt64(results[0]), Convert.ToInt64(results[1]));
                    if (b)
                    {
                        count++;
                    }
                    lists.Add(b ? "right" : "wrong");
                }
                model.IsAllRight = count == strs.Count();
                model.Result = lists;
                model.IsFirst = false;
                return View(model);
            }
        }

        public ActionResult SaveUser(AddUser model)
        {
            Session["IsFirst"] = false;
            activityService.UpdateCount(model.Id, false, false, true, false, false);
            //if(!ModelState.IsValid)
            //{
            //    return Json(new AjaxResult { Status="error",ErrorMsg=MVCHelper.GetValidMsg(ModelState)});
            //}
            if (string.IsNullOrEmpty(model.Name))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "姓名不能为空" });
            }
            if (string.IsNullOrEmpty(model.Mobile))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "手机号不能为空" });
            }
            if (string.IsNullOrEmpty(model.Address))
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "地址不能为空" });
            }
            if (model.Name.Length<2 || model.Name.Length>20)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "姓名长度在2-20之间" });
            }            
            long m;
            if(!long.TryParse(model.Mobile,out m))
            {
                return Json(new AjaxResult { Status="error",ErrorMsg="手机号必须是数字"});
            }
            if (model.Mobile.Length != 11)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "请输入11位手机号" });
            }
            if (model.Address.Length < 2 || model.Address.Length > 300)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "地址长度在2-300之间" });
            }
            long userId= userService.AddNew(model.Name,model.Id, "", model.Mobile, model.Gender, model.Address);
            activityService.AddUserId(model.Id, userId);
            userService.RetSetWon(userId);
            userService.IsHavePrizeChance(userId);            
            if (userId==-1)
            {
                return Json(new AjaxResult { Status = "error",ErrorMsg="你已参加本次活动，完成答题，无法再次参与！" });
                //userService.UpdateUser(model.Mobile, model.Name, model.Gender, model.Address);                
            }
            if(userId>0)
            {                
                Session["Mobile"] = model.Mobile;
            }
            activityService.UpdateCount(model.Id, false, false, false, true, false);
            return Json(new AjaxResult { Status="success"});
        }
    }
}