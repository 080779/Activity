﻿using Chat.DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.AdminWeb.Models.Home
{
    public class AdminUserViewModel
    {
        public List<AdminUserListDTO> AdminUsers { get; set; }
        public string Page { get; set; }
    }
}