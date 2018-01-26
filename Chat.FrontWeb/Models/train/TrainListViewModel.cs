using Chat.DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.FrontWeb.Models.train
{
    public class TrainListViewModel
    {
        public TrainDTO[] Trains { get; set; }
        public string Link { get; set; }
    }
}