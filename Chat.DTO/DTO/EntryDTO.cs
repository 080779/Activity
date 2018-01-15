using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DTO.DTO
{
    public class EntryDTO:BaseDTO
    {
        public string Name { get; set; }
        public bool Gender { get; set; }
        public string Mobile { get; set; }
        public string Workplace { get; set; }//工作地（暂时不用，用idname表）
        public string WorkUnits { get; set; }//工作单位
        public string Duty { get; set; }//职务
        public long StayId { get; set; }//住宿id
        public long PayId { get; set; }//支付方式id
        public long CityId { get; set; }
        public string InvoiceUp { get; set; }//发票抬头
        public string Ein { get; set; }//税号
        public string Address { get; set; }//地址
        public string Contact { get; set; }//联系方式
        public string OpenBank { get; set; } //开户行
        public string BankAccount { get; set; } //银行账号
    }
}
