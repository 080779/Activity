using Chat.IService.Interface;
using Chat.WebCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chat.AdminWeb.App_Start
{
    public sealed class ActionLogFilter : ActionFilterAttribute
    {
        public IAdminLogService logService = DependencyResolver.Current.GetService<IAdminLogService>();
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext == null)
            {
                base.OnActionExecuted(filterContext);
            }

            object[] attrs = filterContext.ActionDescriptor.GetCustomAttributes(typeof(ActDescriptionAttribute), false);
            if (attrs.Length > 0)
            {                
                if(filterContext.HttpContext.Session["AdminUserId"] ==null)
                {
                    base.OnActionExecuted(filterContext);
                }
                long userId = Convert.ToInt64(filterContext.HttpContext.Session["AdminUserId"]);
                string ipAddress = MVCHelper.GetWebClientIp();
                string funDescribe = ((ActDescriptionAttribute)attrs[0]).ActDescription;
                logService.AddNew(userId, ipAddress,"访问执行了"+funDescribe);
            }
            base.OnActionExecuted(filterContext);
        }
               
    }
}