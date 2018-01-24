using Chat.IService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.DTO.DTO;
using Chat.Service.Entities;
using System.Data.Entity;

namespace Chat.Service.Service
{
    public class AdminLogService : IAdminLogService
    {
        public long AddNew(long adminUserId,string ipAddress, string message)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                AdminLogEntity adminLog = new AdminLogEntity();
                adminLog.AdminUserId = adminUserId;
                adminLog.IpAddress = ipAddress;
                adminLog.Message = message;
                dbc.AdminLogs.Add(adminLog);
                dbc.SaveChanges();
                return adminLog.Id;
            }
        }
        
        public AdminLogSearchResult GetPage(DateTime? startTime, DateTime? endTime, string keyWord, int currentIndex,int pageSize)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<AdminLogEntity> cs = new CommonService<AdminLogEntity>(dbc);
                AdminLogSearchResult result = new AdminLogSearchResult();
                var logs=cs.GetAll();
                if(startTime!=null)
                {
                    logs = logs.Where(l => l.CreateDateTime > startTime);
                }
                if(endTime!=null)
                {
                    logs = logs.Where(l => l.CreateDateTime < endTime);
                }
                if(!string.IsNullOrEmpty(keyWord))
                {
                    logs = logs.Where(l => l.Message.Contains(keyWord));
                }
                result.TotalCount = logs.LongCount();
                result.AdminLogs = logs.Include(l=>l.AdminUser).OrderByDescending(l => l.CreateDateTime).Skip(currentIndex).Take(pageSize).ToList().
                    Select(l => new AdminLogDTO { AdminUserId = l.AdminUserId, AdminUserName = l.AdminUser.Name, AdminUserPhoneNum = l.AdminUser.Mobile, CreateDateTime = l.CreateDateTime, Id = l.Id, IpAddress = l.IpAddress, Message = l.Message }).ToArray();
                return result;
            }
        }      
    }
}
