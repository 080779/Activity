using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DTO.DTO
{
    public class ActivityDTO:BaseDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ExamEndTime { get; set; }
        public DateTime RewardTime { get; set; }
    }
}
