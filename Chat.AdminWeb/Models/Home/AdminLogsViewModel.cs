using Chat.DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.AdminWeb.Models.Home
{
    public class AdminLogsViewModel
    {
        public AdminLogDTO[] Logs { get; set; }
        public string Page { get; set; }
    }
}