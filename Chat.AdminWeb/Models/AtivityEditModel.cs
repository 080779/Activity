﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.AdminWeb.Models
{
    public class AtivityEditModel
    {
        public long activityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long StatusId { get; set; }
        public HttpPostedFileBase imgUrl { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ExamEndTime { get; set; }
        public DateTime RewardTime { get; set; }
        public long PaperId { get; set; }
        public string PrizeName { get; set; }
        public HttpPostedFileBase PrizeImgUrl { get; set; }
    }
}