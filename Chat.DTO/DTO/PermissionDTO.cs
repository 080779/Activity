﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DTO.DTO
{
    public class PermissionDTO:BaseDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int LevelList { get; set; }
    }
}
