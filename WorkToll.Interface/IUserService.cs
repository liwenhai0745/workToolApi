using System;
using System.Collections.Generic;
using System.Text;
using WorkTool.Model;

namespace WorkTool.Interface
{
     public interface IUserService:IDBService<User>,IDependency
    {
        User ValiDateUser(string UName, string PassWord);
    }
}
