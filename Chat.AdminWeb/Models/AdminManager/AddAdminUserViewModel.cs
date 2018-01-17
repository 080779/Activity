using Chat.DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.AdminWeb.Models.AdminManager
{
    public class AddAdminUserViewModel
    {
        public RoleDTO Hall { get; set; }
        public RoleDTO[] Citys { get; set; }
    }
}