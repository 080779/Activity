using Chat.IService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.DTO.DTO;
using Chat.Service;
using Chat.Service.Entities;
using Chat.WebCommon;

namespace Chat.Service.Service
{
    public class AdminUserService : IAdminUserService
    {
        public long AddAdminUser(string name, string mobile, string password, string email)
        {
            AdminUserEntity user = new AdminUserEntity();
            user.Name = name;
            user.Mobile = mobile;
            user.PasswordHash = password;
            user.PasswordSalt = password;
            user.LoginErrorTimes = 0;
            //user.LastLoginErrorTime = DateTime.Now;
            user.Email = email;
            //user.CityId = cityId;
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                bool exists= cs.GetAll().Any(a => a.Mobile == mobile);
                if(exists)
                {
                    return -1;
                }
                else
                {
                    dbc.AdminUsers.Add(user);
                    dbc.SaveChanges();
                    return user.Id;
                }
            }
        }

        public bool CheckLogin(string name, string password)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                var user = cs.GetAll().SingleOrDefault(u => u.Name == name);
                if (user == null)
                {
                    return false;
                }
                string pwdHash = CommonHelper.GetMD5(user.PasswordSalt + password);
                return pwdHash == user.PasswordHash;
            }
        }

        public AdminUserDTO ToDTO(AdminUserEntity user)
        {
            AdminUserDTO dto = new AdminUserDTO();
            dto.CreateDateTime = user.CreateDateTime;
            dto.Email = user.Email;
            dto.Id = user.Id;
            dto.Name = user.Name;
            dto.Gender = user.Gender;
            dto.Mobile = user.Mobile;
            return dto;
        }

        public AdminUserDTO[] GetAll()
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                return cs.GetAll().ToList().Select(a => ToDTO(a)).ToArray();
            }
        }
               
        public AdminUserDTO GetById(long id)
        {
            throw new NotImplementedException();
        }
        
        public AdminUserDTO GetByName(string name)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                var user = cs.GetAll().Where(u => u.Name == name);
                int count = user.Count();
                if (count == 0)
                {
                    return null;
                }
                else if (count == 1)
                {
                    return ToDTO(user.Single());
                }
                else
                {
                    throw new ArgumentException("找到用户名为：" + name + "的多条数据");
                }
            }
        }

        public AdminUserDTO GetByPhoneNum(string phoneNum)
        {
            throw new NotImplementedException();
        }

        public bool HasPermission(long adminUserId, string permissionName)
        {
            throw new NotImplementedException();
        }

        public bool MarkDeleted(long adminUserId)
        {
            throw new NotImplementedException();
        }

        public void RecordLoginError(long id)
        {
            throw new NotImplementedException();
        }

        public void ResetLoginError(long id)
        {
            throw new NotImplementedException();
        }

        public void UpdateAdminUser(long id, string name, string email, long? cityId)
        {
            throw new NotImplementedException();
        }

        public bool UpdatePassword(long id, string Password)
        {
            throw new NotImplementedException();
        }
    }
}
