﻿using Chat.DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.AdminWeb.Models.AdminManager
{
    public class AddAdminUserViewModel
    {
        public RoleDTO Hall { get; set; }
        public IdNameDTO[] Citys { get; set; }
        public long ListId { get; set; }
        public long PaperId { get; set; }
        public long ActivityId { get; set; }
        public long TrainId { get; set; }
        public long EntryId { get; set; }
        public long UserId { get; set; }
        public long LogId { get; set; }
        public long AdminUserId { get; set; }
    }
}