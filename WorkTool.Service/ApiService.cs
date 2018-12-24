using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using WorkTool.Interface;
using WorkTool.Model;
using System.Linq;

namespace WorkTool.Service
{
    public class ApiService: IApiService
    {
        /// <summary>
        /// 设置输出参数
        /// </summary>
        /// <param name="result"></param>
        /// <param name="Code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public BaseDTO SetApiResult(BaseDTO result, DoResult Code, string message)
        {
            if (result == null)
            {
                return new BaseDTO()
                {
                    Code = Code,
                    Msg = message
                };
            }
            else
            {
                result.Code = Code;
                result.Msg = message;
                return result;
            }
        }


        /// <summary>
        /// 取得默认的Api返回值 
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public T GetApiResult<T>(DoResult Code = DoResult.Success, string message = "") where T : BaseDTO, new()
        {
            var result = new T()
            {
                Code = Code,
                Msg = message
            };
            return result;
        }

        public int GetUserID(ClaimsPrincipal User)
        {
            var UserID = Convert.ToInt32(User.Claims.First(x => x.Type == "sub").Value);
            return UserID;
        }

        public T ValiateIsSelf<T>(ClaimsPrincipal User,int ownUserID) where T : BaseDTO, new()
        { 
            var result = this.GetApiResult<T>();
            var UserID =this.GetUserID(User);
            if (UserID != ownUserID)
            {
                this.SetApiResult(result, DoResult.Failed, "只能操作自己建立的资源！");
            }

            return result;
        }
    }
}
