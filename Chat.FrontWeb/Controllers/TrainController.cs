using Chat.DTO.DTO;
using Chat.FrontWeb.Models.train;
using Chat.IService.Interface;
using Chat.WebCommon;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chat.FrontWeb.Controllers
{
    public class TrainController : Controller
    {
        public ITrainService trainService { get; set; }
        public IIdNameService idNameService { get; set; }
        public IEntryService entryService { get; set; }
        public ISettingService settingService { get; set; }

        public ActionResult List()
        {
            TrainListViewModel model = new TrainListViewModel();
            model.Trains= trainService.GetAll();
            model.Link = settingService.GetValue("前端奖品图片地址");
            return View(model);
        }
        public ActionResult Details(long id)
        {
            JoinInDetailsViewModel model = new JoinInDetailsViewModel();
            model.Train= trainService.GetById(id);
            model.Link = settingService.GetValue("前端奖品图片地址");
            trainService.SetVisitCount(id);//增加访问次数
            return View(model);
        }
        public ActionResult Joinin(long id)
        {
            JoininViewModel model = new JoininViewModel();
            model.TrainId = id;
            model.Cities = idNameService.GetAll("市级");
            model.Paies = idNameService.GetAll("支付方式");
            model.Staies = idNameService.GetAll("住宿要求");
            return View(model);
        }
        public ActionResult JoininInfo(long id,long trainId)
        {
            JoininInfoViewModel model = new JoininInfoViewModel();
            model.Entry = entryService.GetById(id);
            model.TrainId = trainId;
            return View(model);
        }
        [HttpPost]
        public ActionResult CheckJoinin(JoininModel model)
        {
            #region 数据验证
            if(model.TrainId<=0)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "未知培训" });
            }
            TrainDTO train = trainService.GetById(model.TrainId);
            if (train==null)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "未知培训" });
            }
            if(train.StatusName=="已结束")
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "活动已结束" });
            }
            if(train.UpToOne!=0)
            {
                if(train.EntryCount>=train.UpToOne)
                {
                    return Json(new AjaxResult { Status = "0", ErrorMsg = "报名人数已满" });
                }
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "姓名不能为空" });
            }
            if (model.Gender==0)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "请选择性别" });
            }
            if (string.IsNullOrEmpty(model.Mobile))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "手机号不能为空" });
            }
            
            long phoneNum;
            if(!long.TryParse(model.Mobile,out phoneNum))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "手机号必须是数字" });
            }
            if(model.Mobile.Length!=11)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "手机号必须是11位数字" });
            }
            if(entryService.IsJoinined(model.TrainId,model.Mobile))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "该手机号已报过名" });
            }
            if (string.IsNullOrEmpty(model.WorkUnits))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "工作单位不能为空" });
            }
            if (string.IsNullOrEmpty(model.Duty))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "职务不能为空" });
            }
            if (model.CityId == 0)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "请选择工作地" });
            }
            if (model.StayId == 0)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "请选择住宿" });
            }
            if (model.PayId == 0)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "请选择支付方式" });
            }
            if (string.IsNullOrEmpty(model.InvoiceUp))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "发票不能为空" });
            }
            if (string.IsNullOrEmpty(model.Ein))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "税号不能为空" });
            }
            if (string.IsNullOrEmpty(model.Address))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "详细地址不能为空" });
            }
            if (string.IsNullOrEmpty(model.Contact))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "联系方式不能为空" });
            }
            if (string.IsNullOrEmpty(model.OpenBank))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "开户行不能为空" });
            }
            if (string.IsNullOrEmpty(model.BankAccount))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "银行账号不能为空" });
            }
            #endregion

            #region 报名添加数据
            EntryDTO dto = new EntryDTO();
            dto.Address = model.Address;
            dto.BankAccount = model.BankAccount;
            dto.CityId = model.CityId;
            dto.Contact = model.Contact;
            dto.Duty = model.Duty;
            dto.Ein = model.Ein;
            dto.EntryChannelId = 37;//微信公众号id=37，后台录入id=38
            dto.Gender = model.Gender == 1;
            dto.InvoiceUp = model.InvoiceUp;
            dto.Mobile = model.Mobile;
            dto.Name = model.Name;
            dto.OpenBank = model.OpenBank;
            dto.PayId = model.PayId;
            dto.StayId = model.StayId;
            dto.TrainId = model.TrainId;
            dto.WorkUnits = model.WorkUnits;
            long id = entryService.Add(dto);
            if (id > 0)
            {
                return Json(new AjaxResult { Status = "1", Data = "/train/joinininfo?id=" + id + "&trainId=" + model.TrainId });
            }
            else
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "报名失败" });
            }
            #endregion            
        }
    }
}