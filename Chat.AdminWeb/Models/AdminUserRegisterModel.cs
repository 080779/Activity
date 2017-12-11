using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Chat.AdminWeb.Models
{
    public class AdminUserRegisterModel
    {
        [Required(ErrorMessage ="用户名不能为空")]
        [StringLength(20, MinimumLength = 2,ErrorMessage ="用户名在2-20字之间")]
        public string Name { get; set; }
        [Required(ErrorMessage = "手机号不能为空")]
        [StringLength(11,MinimumLength =11,ErrorMessage ="请输入11位手机号")]
        public string Mobile { get; set; }
        [Required(ErrorMessage ="密码不能为空")]
        [StringLength(120, MinimumLength = 6,ErrorMessage ="密码必须大于6位")]
        public string Password { get; set; }
    }
}