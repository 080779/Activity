using Chat.IService.Interface;
using Chat.Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            IAdminUserService adminService = new AdminUserService();
            ICityService cityService = new CityService();
            //long i = 1;
            //long id= adminService.AddAdminUser("aaa", "18318318383", "123456", "edfe@qq.com", i);
            long id = cityService.AddNew("南宁");
            Console.WriteLine(id);
            Console.ReadKey();
        }
    }
}
