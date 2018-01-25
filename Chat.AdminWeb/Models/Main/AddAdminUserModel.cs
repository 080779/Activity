using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.AdminWeb.Models.Main
{
    public class AddAdminUserModel
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public long[] PermissionIds { get; set; }
    }
}