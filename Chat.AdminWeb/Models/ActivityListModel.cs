using Chat.DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.AdminWeb.Models
{
    public class ActivityListModel
    {
        public ActivityDTO[] Activities { get; set; }
        public string Page { get; set; }
    }
}