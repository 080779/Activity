﻿using Chat.DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.AdminWeb.Models.Train
{
    public class EntryAddViewModel
    {
        public long TrainId { get; set; }
        public IdNameDTO[] Cities { get; set; }
        public IdNameDTO[] Pays { get; set; }
    }
}