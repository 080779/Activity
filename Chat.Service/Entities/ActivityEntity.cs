using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Service.Entities
{
    /// <summary>
    /// 活动实体类
    /// </summary>
    public class ActivityEntity:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public virtual ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
        public DateTime StartTime { get; set; }
        public DateTime ExamEndTime { get; set; }
        public DateTime RewardTime { get; set; }
    }
}
