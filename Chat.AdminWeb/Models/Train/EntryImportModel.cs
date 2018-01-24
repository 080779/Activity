using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.AdminWeb.Models.Train
{
    public class EntryImportModel
    {
        public HttpPostedFileBase File { get; set; }
        public long Id { get; set; } = 0;
        public long CityId { get; set; } = 0;
    }
}