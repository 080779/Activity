using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.AdminWeb.App_Start
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ActDescriptionAttribute:Attribute
    {
        public string ActDescription { get; set; }
        public ActDescriptionAttribute(string actDescription)
        {
            this.ActDescription = actDescription;
        }
    }
}