using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Chat.WebCommon
{
    public static class MVCHelper
    {
        /// <summary>
        /// mvc中model属性验证
        /// </summary>
        public static string GetValidMsg(ModelStateDictionary modelSatae)
        {
            StringBuilder builer = new StringBuilder();
            foreach (var propName in modelSatae.Keys)
            {
                if (modelSatae[propName].Errors.Count <= 0)
                {
                    continue;
                }
                builer.Append("数据验证失败：");
                foreach (ModelError modelError in modelSatae[propName].Errors)
                {
                    builer.Append(modelError.ErrorMessage+"......");
                }
            }
            return builer.ToString();
        }

        #region 获取客户端IP地址  

        public static string GetWebClientIp()
        {
            string userIP = "未获取用户IP";
            try
            {
                if (System.Web.HttpContext.Current == null || System.Web.HttpContext.Current.Request == null || System.Web.HttpContext.Current.Request.ServerVariables == null)
                {
                    return "";
                }
                string CustomerIP = "";
                //CDN加速后取到的IP simone 090805
                CustomerIP = System.Web.HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
                if (!string.IsNullOrEmpty(CustomerIP))
                {
                    return CustomerIP;
                }
                CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!String.IsNullOrEmpty(CustomerIP))
                {
                    return CustomerIP;
                }
                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (CustomerIP == null)
                    {
                        CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    }
                }
                else
                {
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                if (string.Compare(CustomerIP, "unknown", true) == 0 || String.IsNullOrEmpty(CustomerIP))
                {
                    return System.Web.HttpContext.Current.Request.UserHostAddress;
                }
                return CustomerIP;
            }
            catch { }
            return userIP;
        }

        #endregion

    }
}
