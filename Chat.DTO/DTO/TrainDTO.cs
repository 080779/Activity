using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DTO.DTO
{
    public class TrainDTO:BaseDTO
    {
        /// <summary>
        /// 培训标题
        /// </summary>
        public string Title { get; set; }//培训标题
        /// <summary>
        /// 培训头图
        /// </summary>
        public string Img { get; set; }//培训头图
        /// <summary>
        /// 培训地址
        /// </summary>
        public string Address { get; set; }//培训地址
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string StatusName { get; set; }
        public long VisitCount { get; set; }//访问量
        /// <summary>
        /// 报名费用
        /// </summary>
        public decimal EntryFee { get; set; }//报名费用
        /// <summary>
        /// 最多可报名
        /// </summary>
        public long UpToOne { get; set; }//最多可报名
        /// <summary>
        /// 已报名人数
        /// </summary>
        public long EntryCount { get; set; }
        /// <summary>
        /// 培训详情
        /// </summary>
        public string Description { get; set; }//培训详情
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsDisplayed { get; set; }
    }
}
