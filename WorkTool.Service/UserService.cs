using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using WorkTool.Interface;
using WorkTool.Model;
using System.Linq;


namespace WorkTool.Service
{
    public class UserService:DBService<User>, IUserService
    {
        public UserService(IDbConnection _dbConnection):base(_dbConnection)
        {
        }

        /// <summary>
        /// 验证用户信息
        /// </summary>
        /// <param name="UName"></param>
        /// <param name="PassWord"></param>
        /// <returns></returns>
        public User ValiDateUser(string UName, string PassWord)
        {
            var User = GetAll().ToList().Find(t=>t.USERNAME==UName&&t.PASSWORD==PassWord);
            return User;
            
        }
    }
}
