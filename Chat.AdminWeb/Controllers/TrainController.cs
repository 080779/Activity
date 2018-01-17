using Chat.AdminWeb.Models.Train;
using Chat.DTO.DTO;
using Chat.IService.Interface;
using Chat.WebCommon;
using CodeCarvings.Piczard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chat.AdminWeb.Controllers
{
    public class TrainController : Controller
    {
        public ITrainService trainService { get; set; }
        public ActionResult List()
        {
            TrainDTO[] dtos = trainService.GetAll();
            //TrainListViewModel model = new TrainListViewModel();
            return View(dtos);
        }

        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(TrainAddModel model)
        {
            if(string.IsNullOrEmpty(model.Title))
            {
                return Json(new AjaxResult { Status="0",ErrorMsg="培训主题不能为空"});
            }
            if(string.IsNullOrEmpty(model.Img))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "培训主题不能为空" });
            }
            string[] strs = model.Img.Split(',');
            string[] formats = strs[0].Replace(";base64", "").Split(':');
            string img = strs[1];
            string format = formats[1];
            string[] imgFormats = { "image/png", "image/jpg", "image/jpeg", "image/bmp", "IMAGE/PNG", "IMAGE/JPG", "IMAGE/JPEG", "IMAGE/BMP" };
            byte[] imgBytes;
            if (!imgFormats.Contains(format))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "请选择正确的图片格式，支持png、jpg、jpeg、png格式" });
            }
            string ext = format.Split('/')[1];
            try
            {
                imgBytes = Convert.FromBase64String(img);
            }
            catch(Exception ex)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "图片解密错误" });
            }
            if (string.IsNullOrEmpty(model.Address))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "培训地点不能为空" });
            }
            if (model.StartTime==null)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "培训开始时间为空,或格式不正确" });
            }
            if (model.EndTime == null)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "培训截止时间为空,或格式不正确" });
            }
            if(model.EndTime<model.StartTime)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "培训截止时间必须大于开始时间" });
            }
            if (model.EntryFee<0)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "培训报名费用不能小于零" });
            }
            if (model.UpToOne < 0)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "培训最多可报名不能小于零" });
            }
            if (string.IsNullOrEmpty(model.Description))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "培训详情不能为空" });
            }
            if(trainService.AddNew(model.Title, SaveImg(imgBytes, ext),model.Address, model.StartTime, model.EndTime, model.EntryFee, model.UpToOne, model.Description)<0)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "培训添加失败" });
            }
            return Json(new AjaxResult { Status = "1" });
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult EntryAdd()
        {
            return View();
        }

        public ActionResult EntryEdit()
        {
            return View();
        }

        /// <summary>
        /// 报名导入
        /// </summary>
        /// <returns></returns>
        public ActionResult EntryImport()
        {
            return View();
        }

        private string SaveImg(byte[] imgBytes,string ext)
        {
            string md5 = CommonHelper.GetMD5(imgBytes);
            string path = "/upload/" + DateTime.Now.ToString("yyyy/MM/dd") + "/" + md5 + ext;
            //string thumbPath = "/upload/" + DateTime.Now.ToString("yyyy/MM/dd") + "/" + md5 + "_thumb" + ext;
            string fullPath = HttpContext.Server.MapPath("" + path);
            //string thumbFullPath = HttpContext.Server.MapPath("~" + thumbPath);
            new FileInfo(fullPath).Directory.Create();
            //file.SaveAs(fullPath);
            //缩略图
            //file.InputStream.Position = 0;
            //ImageProcessingJob jobThumb = new ImageProcessingJob();
            //jobThumb.Filters.Add(new FixedResizeConstraint(200, 200));//缩略图尺寸 200*200
            //jobThumb.SaveProcessedImageToFileSystem(file.InputStream, thumbFullPath);
            //水印
            //file.InputStream.Position = 0;
            //ImageWatermark imgWatermark = new ImageWatermark(HttpContext.Server.MapPath("~/images/fb.png"));
            //imgWatermark.ContentAlignment = System.Drawing.ContentAlignment.BottomRight;//水印位置
            //imgWatermark.Alpha = 50;//透明度，需要水印图片是背景透明的 png 图片
            ImageProcessingJob jobNormal = new ImageProcessingJob();
            //jobNormal.Filters.Add(imgWatermark);//添加水印
            jobNormal.Filters.Add(new FixedResizeConstraint(600, 600));//限制图片的大小，避免生成
            //jobNormal.SaveProcessedImageToFileSystem(file.InputStream, fullPath);
            jobNormal.SaveProcessedImageToFileSystem(imgBytes, fullPath);
            return path;
        }
    }
}