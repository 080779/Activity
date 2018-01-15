using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Service.Entities
{
    public class TrainEntity:BaseEntity
    {
        public string Title { get; set; }//培训标题
        public string Img { get; set; }//培训头图
        public string Address { get; set; }//培训地址
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public virtual IdNameEntity Status { get; set; }
        public long StatusId { get; set; }
        public long VisitCount { get; set; } = 0;//访问量
        public decimal EntryFee { get; set; }//报名费用
        public long UpToOne { get; set; }//最多可报名
        public string Description { get; set; }//培训详情
        public virtual ICollection<EntryEntity> Entries { get; set; } = new List<EntryEntity>();
    }
}
