using Chat.AdminWeb.Models.Train;
using Chat.DTO.DTO;
using Chat.IService.Interface;
using Chat.WebCommon;
using CodeCarvings.Piczard;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
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
        public IIdNameService idNameService { get; set; }
        public IEntryService entryService { get; set; }

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

        public ActionResult EntryList(long id)
        {
            EntryListViewModel model = new EntryListViewModel();
            model.TrainId = id;
            model.Cities = idNameService.GetAll("市级");
            model.Entries = entryService.GetByTrainId(id);
            return View(model);
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult EntryAdd(long id)
        {
            EntryAddViewModel model = new EntryAddViewModel();
            model.TrainId = id;
            model.Cities = idNameService.GetAll("市级");
            model.Pays = idNameService.GetAll("支付方式");
            return View(model);
        }

        [HttpPost]
        public ActionResult EntryAdd(EntryAddModel model)
        {
            EntryDTO dto = new EntryDTO();
            dto.Address = model.Address;
            dto.BankAccount = model.BankAccount;
            dto.CityId = model.CityId;
            dto.Contact = model.Contact;
            dto.Duty = model.Duty;
            dto.Ein = model.Ein;
            dto.EntryChannelId = 38;
            dto.Gender = model.Gender==1;
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
            return Json(new AjaxResult { Status = "1"});
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

        public ActionResult SearchEntry()
        {
            return Json(new AjaxResult { Status = "0", ErrorMsg = "培训主题不能为空" });
        }
    }
}