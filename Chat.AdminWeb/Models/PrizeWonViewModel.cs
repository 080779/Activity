using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.AdminWeb.Models
{
    public class PrizeWonViewModel
    {
        public long[] IsWonIds { get; set; }
        public long ActivityId { get; set; }
    }
}