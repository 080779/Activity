using Chat.DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.FrontWeb.Models.train
{
    public class JoininViewModel
    {
        public long TrainId { get; set; }
        public IdNameDTO[] Cities { get; set; }
        public IdNameDTO[] Staies { get; set; }
        public IdNameDTO[] Paies { get; set; }
    }
}