using Chat.AdminWeb.App_Start;
using Chat.AdminWeb.Models.Train;
using Chat.DTO.DTO;
using Chat.IService.Interface;
using Chat.WebCommon;
using CodeCarvings.Piczard;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chat.AdminWeb.Controllers
{
    public class TrainController : Controller
    {
        public ITrainService trainService { get; set; }
        public IIdNameService idNameService { get; set; }
        public IEntryService entryService { get; set; }

        [Permission("train")]
        public ActionResult List(int pageIndex=1)
        {
            TrainSearchResult result= trainService.Search(null, null, null, null, (pageIndex-1)*20, 20);
            TrainListViewModel model = new TrainListViewModel();
            model.Trains = result.Trains;

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
            return View(model);
        }

        [Permission("train")]
        public ActionResult TrainSearch(long? statusId,DateTime? startTime,DateTime? endTime,string keyWord,int pageIndex=1)
        {
            TrainSearchResult result = trainService.Search(statusId, startTime, endTime, keyWord, (pageIndex - 1) * 20, 20);
            TrainListViewModel model = new TrainListViewModel();
            model.Trains = result.Trains;

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

        [Permission("train")]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Permission("train")]
        [ValidateInput(false)]
        public ActionResult Add(TrainAddModel model)
        {
            if(string.IsNullOrEmpty(model.Title))
            {
                return Json(new AjaxResult { Status="0",ErrorMsg="培训主题不能为空"});
            }
            if(string.IsNullOrEmpty(model.Img))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "培训头图不能为空" });
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
            string ext = "."+format.Split('/')[1];
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
            return Json(new AjaxResult { Status = "1",Data="/train/list" });
        }

        [Permission("train")]
        public ActionResult Edit(long id)
        {
            TrainDTO dto= trainService.GetById(id);
            return View(dto);
        }

        [HttpPost]
        [Permission("train")]
        [ValidateInput(false)]
        public ActionResult Edit(TrainEditModel model)
        {
            if(model.Id<=0)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "培训活动不存在" });
            }
            if (string.IsNullOrEmpty(model.Title))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "培训主题不能为空" });
            }
            if (string.IsNullOrEmpty(model.Img))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "培训头图不能为空" });
            }
            string ext = "";
            byte[] imgBytes=null;
            if (!model.Img.Contains("/upload/"))
            {
                string[] strs = model.Img.Split(',');
                string[] formats = strs[0].Replace(";base64", "").Split(':');
                string img = strs[1];
                string format = formats[1];
                string[] imgFormats = { "image/png", "image/jpg", "image/jpeg", "image/bmp", "IMAGE/PNG", "IMAGE/JPG", "IMAGE/JPEG", "IMAGE/BMP" };
                
                if (!imgFormats.Contains(format))
                {
                    return Json(new AjaxResult { Status = "0", ErrorMsg = "请选择正确的图片格式，支持png、jpg、jpeg、png格式" });
                }
                ext = "." + format.Split('/')[1];
                try
                {
                    imgBytes = Convert.FromBase64String(img);
                }
                catch (Exception ex)
                {
                    return Json(new AjaxResult { Status = "0", ErrorMsg = "图片解密错误" });
                }
            }           
            if (string.IsNullOrEmpty(model.Address))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "培训地点不能为空" });
            }
            if (model.StartTime == null)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "培训开始时间为空,或格式不正确" });
            }
            if (model.EndTime == null)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "培训截止时间为空,或格式不正确" });
            }
            if (model.EndTime < model.StartTime)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "培训截止时间必须大于开始时间" });
            }
            if (model.EntryFee < 0)
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
            if(model.Img.Contains("/upload/"))
            {
                if (!trainService.Update(model.Id,model.Title,model.Img, model.Address, model.StartTime, model.EndTime, model.EntryFee, model.UpToOne, model.Description))
                {
                    return Json(new AjaxResult { Status = "0", ErrorMsg = "培训编辑失败" });
                }
            }
            else if(imgBytes==null)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "图片解密错误" });
            }
            else
            {
                if (!trainService.Update(model.Id, model.Title, SaveImg(imgBytes, ext), model.Address, model.StartTime, model.EndTime, model.EntryFee, model.UpToOne, model.Description))
                {
                    return Json(new AjaxResult { Status = "0", ErrorMsg = "培训编辑失败" });
                }
            }            
            return Json(new AjaxResult { Status = "1", Data = "/train/list" });
        }

        [Permission("train")]
        public ActionResult Delete(long id)
        {
            if(id<=0)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg="参数错误" });
            }
            if(!trainService.Delete(id))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "删除失败" });
            }
            return Json(new AjaxResult { Status = "1", Data = "/train/list" });
        }

        [Permission("entry")]
        public ActionResult EntryList(long id=0,int pageIndex=1)
        {
            EntryListViewModel model = new EntryListViewModel();
            EntryGetPageDTO dto = new EntryGetPageDTO();
            dto.Id = id;
            EntrySearchResult result = entryService.GetPageByTrainId(dto);
            model.TrainId = id;
            model.Cities = idNameService.GetAll("市级");
            model.Entries = result.Entries;

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
            return View(model);
        }

        [HttpPost]
        [Permission("entry")]
        public ActionResult EntryList(EntryGetPageDTO dto)
        {
            EntryListViewModel model = new EntryListViewModel();
            dto.CurrentIndex = (dto.PageIndex - 1) * dto.PageSize;
            EntrySearchResult result = entryService.GetPageByTrainId(dto);
            model.TrainId = dto.Id;
            //model.Cities = idNameService.GetAll("市级");
            model.Entries = result.Entries;

            //分页
            Pagination pager = new Pagination();
            pager.PageIndex = dto.PageIndex;
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

        [Permission("entry")]
        public ActionResult EntryAdd(long id)
        {
            EntryAddViewModel model = new EntryAddViewModel();
            model.TrainId = id;
            model.Cities = idNameService.GetAll("市级");
            model.Pays = idNameService.GetAll("支付方式");
            return View(model);
        }

        [HttpPost]
        [Permission("entry")]
        public ActionResult EntryAdd(EntryAddModel model)
        {
            #region 数据验证
            if (model.TrainId <= 0)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "未知培训" });
            }
            TrainDTO train = trainService.GetById(model.TrainId);
            if (train == null)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "未知培训" });
            }
            if (train.StatusName == "已结束")
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "活动已结束" });
            }
            if (train.UpToOne != 0)
            {
                if (train.EntryCount >= train.UpToOne)
                {
                    return Json(new AjaxResult { Status = "0", ErrorMsg = "报名人数已满" });
                }
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "姓名不能为空" });
            }
            if (model.Gender==null)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "请选择性别" });
            }
            if (string.IsNullOrEmpty(model.Mobile))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "手机号不能为空" });
            }

            long phoneNum;
            if (!long.TryParse(model.Mobile, out phoneNum))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "手机号必须是数字" });
            }
            if (model.Mobile.Length != 11)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "手机号必须是11位数字" });
            }
            if (entryService.IsJoinined(model.TrainId, model.Mobile))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "该手机号已报过名" });
            }
            if (model.CityId == 0)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "请选择工作地" });
            }
            if (string.IsNullOrEmpty(model.WorkUnits))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "工作单位不能为空" });
            }
            if (string.IsNullOrEmpty(model.Duty))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "职务不能为空" });
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

            EntryDTO dto = new EntryDTO();
            dto.Address = model.Address;
            dto.BankAccount = model.BankAccount;
            dto.CityId = model.CityId;
            dto.Contact = model.Contact;
            dto.Duty = model.Duty;
            dto.Ein = model.Ein;
            dto.EntryChannelId = 38;
            dto.Gender = (bool)model.Gender;
            dto.InvoiceUp = model.InvoiceUp;
            dto.Mobile = model.Mobile;
            dto.Name = model.Name;
            dto.OpenBank = model.OpenBank;
            dto.PayId = model.PayId;
            dto.StayId = model.StayId;
            dto.TrainId = model.TrainId;
            dto.WorkUnits = model.WorkUnits;
            long id=entryService.Add(dto);
            if(id<=0)
            {
                return Json(new AjaxResult { Status = "0",ErrorMsg="新增报名用户失败" });
            }
            return Json(new AjaxResult { Status = "1", Data = "/train/entrylist?id=" + model.TrainId });
        }

        [Permission("entry")]
        public ActionResult EntryEdit(long id,long trainId)
        {
            EntryEditViewModel model = new EntryEditViewModel();
            model.Cities = idNameService.GetAll("市级");
            model.Pays = idNameService.GetAll("支付方式");
            model.Entry = entryService.GetByEntryId(id);
            model.TrainId = trainId;
            return View(model);
        }

        [HttpPost]
        [Permission("entry")]
        public ActionResult EntryEdit(EntryEditModel model)
        {
            #region 数据验证
            if(model.Id<=0)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "报名用户不存在" });
            }
            if (model.TrainId <= 0)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "未知培训" });
            }
            TrainDTO train = trainService.GetById(model.TrainId);
            if (train == null)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "未知培训" });
            }
            if (train.StatusName == "已结束")
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "活动已结束" });
            }
            if (train.UpToOne != 0)
            {
                if (train.EntryCount >= train.UpToOne)
                {
                    return Json(new AjaxResult { Status = "0", ErrorMsg = "报名人数已满" });
                }
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "姓名不能为空" });
            }
            if (model.Gender == null)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "请选择性别" });
            }
            if (string.IsNullOrEmpty(model.Mobile))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "手机号不能为空" });
            }

            long phoneNum;
            if (!long.TryParse(model.Mobile, out phoneNum))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "手机号必须是数字" });
            }
            if (model.Mobile.Length != 11)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "手机号必须是11位数字" });
            }
            //if (entryService.IsJoinined(model.TrainId, model.Mobile))
            //{
            //    return Json(new AjaxResult { Status = "0", ErrorMsg = "该手机号已报过名" });
            //}
            if (model.CityId == 0)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "请选择工作地" });
            }
            if (string.IsNullOrEmpty(model.WorkUnits))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "工作单位不能为空" });
            }
            if (string.IsNullOrEmpty(model.Duty))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "职务不能为空" });
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

            EntryDTO dto = new EntryDTO();
            dto.Id = model.Id;
            dto.Address = model.Address;
            dto.BankAccount = model.BankAccount;
            dto.CityId = model.CityId;
            dto.Contact = model.Contact;
            dto.Duty = model.Duty;
            dto.Ein = model.Ein;
            dto.Gender = (bool)model.Gender;
            dto.InvoiceUp = model.InvoiceUp;
            dto.Mobile = model.Mobile;
            dto.Name = model.Name;
            dto.OpenBank = model.OpenBank;
            dto.PayId = model.PayId;
            dto.StayId = model.StayId;
            dto.WorkUnits = model.WorkUnits;
            if(!entryService.Update(dto))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "编辑报名用户失败" });
            }
            return Json(new AjaxResult { Status = "1" ,Data="/train/entrylist?id="+model.TrainId });
        }

        [Permission("entry")]
        public ActionResult EntryDelete(long id=0,long trainId=0)
        {
            if (id <= 0)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "参数错误" });
            }
            if(trainId<=0)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "参数错误" });
            }
            if (!entryService.Delete(id))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "删除失败" });
            }
            return Json(new AjaxResult { Status = "1", Data = "/train/entrylist?id="+trainId });
        }

        [Permission("entry")]
        /// <summary>
        /// 报名导入
        /// </summary>
        /// <returns></returns>
        public ActionResult EntryImport(EntryImportModel model)
        {
            if(model.Id<=0)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "参数错误" });
            }
            if(model.CityId<=0)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "请选择导入市级" });
            }
            if(model.File==null || model.File.ContentLength<0)
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "请选择要上传的文件" });
            }
            string[] excelFormats = { ".xlsx", ".xls" };
            string md5 = CommonHelper.GetMD5(model.File.InputStream);
            string ext = Path.GetExtension(model.File.FileName);
            if(!excelFormats.Contains(ext))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg = "请上传excel文件" });
            }
            string path = "/upload/" + DateTime.Now.ToString("yyyy/MM/dd") + "/" + md5 + ext;
            string fullPath = HttpContext.Server.MapPath("~" + path);
            new FileInfo(fullPath).Directory.Create();
            model.File.SaveAs(fullPath);

            DataTable dt = ExcelHelper.GetDataTable(fullPath);
            if(!entryService.EntryImport(model.Id, model.CityId, 38, dt))
            {
                return Json(new AjaxResult { Status = "0", ErrorMsg="报名导入失败" });
            }
            return Json(new AjaxResult { Status = "1",Data="/train/entrylist?id="+model.Id });
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

        [Permission("entry")]
        public ActionResult ExportExcel(long id)
        {
            IWorkbook wb = new XSSFWorkbook();

            ICellStyle style1 = wb.CreateCellStyle();//样式
            IFont font1 = wb.CreateFont();//字体
            font1.FontName = "楷体";
            font1.Boldweight = (short)FontBoldWeight.Normal;//字体加粗样式
            style1.SetFont(font1);//样式里的字体设置具体的字体样式
            style1.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;//文字水平对齐方式
            style1.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;//文字垂直对齐方式
            //设置边框
            style1.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            style1.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            style1.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            style1.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

            IdNameDTO[] cities = idNameService.GetAll("市级");
            foreach (var city in cities)
            {
                ISheet sheet = wb.CreateSheet(city.Name);
                //int[] columnWidth = { 10, 10, 20, 10 };
                //for (int i = 0; i < columnWidth.Length; i++)
                //{
                //    //设置列宽度，256*字符数，因为单位是1/256个字符
                //    sheet.SetColumnWidth(i, 256 * columnWidth[i]);
                //}
                int rowCount = 1;
                var entries=entryService.GetByTrainIdCityId(id, city.Id);

                string[] columnName = { "编号", "姓名", "性别", "工作单位", "职务", "手机号", "住宿要求", "支付方式", "发票抬头", "税号", "地址", "联系方式", "开户行", "银行账号" };
                IRow row;
                ICell cell;
                row = sheet.CreateRow(0);
                for (int j = 0; j < columnName.Length; j++)
                {
                    cell = row.CreateCell(j);//创建第j列
                    cell.CellStyle = style1;
                    ExcelHelper.SetCellValue(cell, columnName[j]);
                }
                if (entries != null)
                {
                    foreach(var entry in entries)
                    {
                        row = sheet.CreateRow(rowCount);
                        cell = row.CreateCell(0);
                        cell.CellStyle = style1;
                        ExcelHelper.SetCellValue(cell, entry.Id);
                        sheet.AutoSizeColumn(0);
                        cell = row.CreateCell(1);
                        cell.CellStyle = style1;
                        ExcelHelper.SetCellValue(cell, entry.Name);
                        sheet.AutoSizeColumn(1);
                        cell = row.CreateCell(2);
                        cell.CellStyle = style1;
                        ExcelHelper.SetCellValue(cell, entry.Gender);
                        sheet.AutoSizeColumn(2);
                        cell = row.CreateCell(3);
                        cell.CellStyle = style1;
                        ExcelHelper.SetCellValue(cell, entry.WorkUnits);
                        sheet.AutoSizeColumn(3);
                        cell = row.CreateCell(4);
                        cell.CellStyle = style1;
                        ExcelHelper.SetCellValue(cell, entry.Duty);
                        sheet.AutoSizeColumn(4);
                        cell = row.CreateCell(5);
                        cell.CellStyle = style1;
                        ExcelHelper.SetCellValue(cell, entry.Mobile);
                        sheet.AutoSizeColumn(5);
                        cell = row.CreateCell(6);
                        cell.CellStyle = style1;
                        ExcelHelper.SetCellValue(cell, entry.StayName);
                        sheet.AutoSizeColumn(6);
                        cell = row.CreateCell(7);
                        cell.CellStyle = style1;
                        ExcelHelper.SetCellValue(cell, entry.PayName);
                        sheet.AutoSizeColumn(7);
                        cell = row.CreateCell(8);
                        cell.CellStyle = style1;
                        ExcelHelper.SetCellValue(cell, entry.InvoiceUp);
                        sheet.AutoSizeColumn(8);
                        cell = row.CreateCell(9);
                        cell.CellStyle = style1;
                        ExcelHelper.SetCellValue(cell, entry.Ein);
                        sheet.AutoSizeColumn(9);
                        cell = row.CreateCell(10);
                        cell.CellStyle = style1;
                        ExcelHelper.SetCellValue(cell, entry.Address);
                        sheet.AutoSizeColumn(10);
                        cell = row.CreateCell(11);
                        cell.CellStyle = style1;
                        ExcelHelper.SetCellValue(cell, entry.Contact);
                        sheet.AutoSizeColumn(11);
                        cell = row.CreateCell(12);
                        cell.CellStyle = style1;
                        ExcelHelper.SetCellValue(cell, entry.OpenBank);
                        sheet.AutoSizeColumn(12);
                        cell = row.CreateCell(13);
                        cell.CellStyle = style1;
                        ExcelHelper.SetCellValue(cell, entry.BankAccount);
                        sheet.AutoSizeColumn(13);
                    }
                }
            }

            var ms = new NpoiMemoryStream();
            ms.AllowClose = false;
            wb.Write(ms);
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            ms.AllowClose = true;
            return File(ms, "application/vnd.ms-excel", "报名用户汇总.xls");
        }

        [Permission("entry")]
        public ActionResult SearchEntry()
        {
            return Json(new AjaxResult { Status = "0", ErrorMsg = "培训主题不能为空" });
        }
    }
}