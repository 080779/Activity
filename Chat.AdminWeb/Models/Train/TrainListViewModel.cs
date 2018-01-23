using Chat.DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.AdminWeb.Models.Train
{
    public class TrainListViewModel
    {
        public TrainDTO[] Trains { get; set; }
        public string Page { get; set; }
    }
}