using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Service.Entities
{
    public class EntryEntity : BaseEntity
    {
        public string Name { get; set; }
        public bool Gender { get; set; }
        public string Mobile { get; set; }
        public string Workplace { get; set; }//工作地
        public string WorkUnits { get; set; }//工作单位
        public string Duty { get; set; }//职务
        public virtual IdNameEntity Stays { get; set; }//住宿
        public long StayId { get; set; }
        public virtual IdNameEntity Pays { get; set; }//支付方式
        public long PayId { get; set; }
        public string InvoiceUp { get; set; }//发票抬头
        public string Ein { get; set; }//税号
        public string Address { get; set; }//地址
        public string Contact { get; set; }//联系方式
        public string OpenBank { get; set; } //开户行
        public string BankAccount { get; set; } //银行账号
    }
}
