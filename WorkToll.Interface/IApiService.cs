using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using WorkTool.Model;

namespace WorkTool.Interface
{
    public interface IApiService:IDependency
    {
        BaseDTO SetApiResult(BaseDTO result, DoResult Code, string message);

        T GetApiResult<T>(DoResult Code = DoResult.Success, string message = "") where T : BaseDTO, new();

        int GetUserID(ClaimsPrincipal User);

        T ValiateIsSelf<T>(ClaimsPrincipal User, int ownUserID) where T : BaseDTO, new();
    }
}
