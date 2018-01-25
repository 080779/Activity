using Chat.DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.IService.Interface
{
    /// <summary>
    /// 后台用户管理接口
    /// </summary>
    public interface IAdminUserService : IServiceSupport
    {
        long AddAdminUser(string name, string mobile, bool gender, string email, string password);
        bool AddNew(int id, string name, string password, string roleName, string description, List<long> permissionIds);
        bool Update(long id, string roleName, List<long> permissionIds);
        //void UpdateAdminUser(AdminUserDTO user);
        void UpdateAdminUser(long id, String name, String email, long? cityId);
        //获取 cityId 这个城市下的管理员
        //AdminUserDTO[] GetAll(long? cityId);
        //获取所有管理员
        AdminUserDTO[] GetAll();
        //分页获取管理员
        AdminUserSearchResult GetPage(DateTime? startTime, DateTime? endTime, string keyWord, int currentIndex, int pageSize);
        //根据 id 获取 DTO
        AdminUserDTO GetById(long id);
        //根据手机号获取 DTO
        AdminUserDTO GetByPhoneNum(String phoneNum);
        //根据用户名获取DTO
        AdminUserDTO GetByName(String name);
        //检查用户名密码是否正确
        bool CheckLogin(String name, String password);
        //软删除
        bool MarkDeleted(long adminUserId);
        //判断 adminUserId 这个用户是否有 permissionName 这个权限项（举个例子）
        //HasPermission(3,"User.Add")
        bool HasPermission(long adminUserId, String permissionName);
        bool UpdatePassword(long id, string Password);
        void RecordLoginError(long id);//记录错误登录一次
        void ResetLoginError(long id);//重置登录错误信息
    }

    public class AdminUserSearchResult
    {
        public AdminUserDTO[] AdminUsers { get; set; }
        public long TotalCount { get; set; }
    }
}
