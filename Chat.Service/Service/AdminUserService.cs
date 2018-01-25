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
using System.Data.Entity;

namespace Chat.Service.Service
{
    public class AdminUserService : IAdminUserService
    {
        public long AddAdminUser(string name, string mobile, bool gender, string email, string password)
        {
            AdminUserEntity user = new AdminUserEntity();
            user.Name = name;
            user.Mobile = mobile;
            user.Gender = gender;
            string salt = CommonHelper.GetCaptcha(5);
            user.PasswordSalt = salt;
            user.PasswordHash = CommonHelper.GetMD5(salt + password);
            user.LoginErrorTimes = 0;
            user.Email = email;
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                bool exists = cs.GetAll().Any(a => a.Mobile == mobile);
                if (exists)
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

        public bool AddNew(int id,string name,string password,string roleName,string description,List<long> permissionIds)
        {
            AdminUserEntity user = new AdminUserEntity();
            user.Name = name;
            user.Mobile = "";
            user.Gender = true;
            string salt = CommonHelper.GetCaptcha(5);
            user.PasswordSalt = salt;
            user.PasswordHash = CommonHelper.GetMD5(salt + password);
            user.LoginErrorTimes = id;
            user.Email = "";
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                CommonService<PermissionEntity> pcs = new CommonService<PermissionEntity>(dbc);
                bool exists = cs.GetAll().Any(a => a.Name == name);
                if (exists)
                {
                    return false;
                }
                else
                {
                    dbc.AdminUsers.Add(user);
                    dbc.SaveChanges();
                    RoleEntity role = new RoleEntity();
                    role.Description = description;
                    role.Name = roleName + user.Id;
                    dbc.Roles.Add(role);
                    role.AdminUsers.Add(user);
                    dbc.SaveChanges();
                    var pms = pcs.GetAll().Where(p => permissionIds.Contains(p.Id)).ToArray();
                    foreach (var pm in pms)
                    {
                        role.Permissions.Add(pm);
                    }
                    dbc.SaveChanges();
                }
                return true;
            }
        }

        public bool Update(long id,string roleName, List<long> permissionIds)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                CommonService<PermissionEntity> pcs = new CommonService<PermissionEntity>(dbc);
                var user = cs.GetAll().SingleOrDefault(a=>a.Id==id);
                if (user==null)
                {
                    return false;
                }
                else
                {
                    var role = user.Roles.Where(r => r.IsDeleted == false).First();
                    role.Name = roleName + user.Id;
                    role.AdminUsers.Add(user);
                    role.Permissions.Clear();
                    var pms = pcs.GetAll().Where(p => permissionIds.Contains(p.Id)).ToArray();
                    foreach (var pm in pms)
                    {
                        role.Permissions.Add(pm);
                    }
                    dbc.SaveChanges();
                }
                return true;
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

        public AdminUserSearchResult GetPage(DateTime? startTime, DateTime? endTime, string keyWord, int currentIndex, int pageSize)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                AdminUserSearchResult result = new AdminUserSearchResult();
                var adminUsers = cs.GetAll();
                if (startTime != null)
                {
                    adminUsers = adminUsers.Where(a => a.CreateDateTime > startTime);
                }
                if (endTime != null)
                {
                    adminUsers = adminUsers.Where(a => a.CreateDateTime < endTime);
                }
                if (!string.IsNullOrEmpty(keyWord))
                {
                    adminUsers = adminUsers.Where(a => a.Name.Contains(keyWord));
                }
                result.TotalCount = adminUsers.LongCount();
                result.AdminUsers = adminUsers.Include(a => a.Roles).OrderByDescending(a => a.CreateDateTime).Skip(currentIndex).Take(pageSize).ToList().
                    Select(a => new AdminUserDTO { CreateDateTime=a.CreateDateTime,Email=a.Email,Gender=a.Gender,Id=a.Id,LastLoginErrorDateTime=a.LastLoginErrorTime,LoginErrorTimes=a.LoginErrorTimes,Mobile=a.Mobile,Name=a.Name,Roles=a.Roles.Where(r=>r.IsDeleted==false).ToList().Select(r=>ToRoleDTO(r)).ToArray()}).ToArray();
                return result;
            }
        }

        public AdminUserDTO GetById(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                var user = cs.GetAll().SingleOrDefault(a => a.Id == id);
                if(user==null)
                {
                    return null;
                }
                return ToDTO(user);
            }
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
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                var user = cs.GetAll().Include(u => u.Roles).AsNoTracking().SingleOrDefault(u => u.Id == adminUserId);
                if (user == null)
                {
                    return false;
                }
                return user.Roles.SelectMany(r => r.Permissions).Any(p => p.Name == permissionName);
            }
        }

        public bool MarkDeleted(long adminUserId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                var user = cs.GetAll().SingleOrDefault(a => a.Id == adminUserId);
                if(user==null)
                {
                    return false;
                }
                user.IsDeleted = true;
                dbc.SaveChanges();
                return true;
            }
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

        public RoleDTO ToRoleDTO(RoleEntity entity)
        {
            RoleDTO dto = new RoleDTO();
            dto.CreateDateTime = entity.CreateDateTime;
            dto.Description = entity.Description;
            dto.Id = entity.Id;
            dto.Name = entity.Name;
            return dto;
        }
    }
}
