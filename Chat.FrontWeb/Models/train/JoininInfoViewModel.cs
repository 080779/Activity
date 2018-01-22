using Chat.DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.FrontWeb.Models.train
{
    public class JoininInfoViewModel
    {
        public EntryListDTO Entry { get; set; }
        public long TrainId { get; set; }
    }
}