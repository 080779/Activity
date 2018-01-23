using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.AdminWeb.Models.Train
{
    public class EntryEditModel
    {
        public long Id { get; set; }
        public long TrainId { get; set; }
        public string Name { get; set; }
        public bool? Gender { get; set; }
        public string Mobile { get; set; }
        //public string Workplace { get; set; }//工作地（暂时不用，用idname表）
        /// <summary>
        /// 工作单位
        /// </summary>
        public string WorkUnits { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string Duty { get; set; }
        public long StayId { get; set; }//住宿
        public long PayId { get; set; }//支付方式
        /// <summary>
        /// 报名渠道
        /// </summary>
        //public long EntryChannelId { get; set; }
        public long CityId { get; set; }
        /// <summary>
        /// 发票抬头
        /// </summary>
        public string InvoiceUp { get; set; }
        /// <summary>
        /// 税号
        /// </summary>
        public string Ein { get; set; }
        public string Address { get; set; }//地址
        /// <summary>
        /// 联系方式
        /// </summary>
        public string Contact { get; set; }
        public string OpenBank { get; set; } //开户行
        public string BankAccount { get; set; } //银行账号
    }
}