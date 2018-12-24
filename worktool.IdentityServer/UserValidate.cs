using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkTool.Interface;
using WorkTool.Model;

namespace WorkTool.IdentityServer
{
    public class UserValidate : IResourceOwnerPasswordValidator
    {
        private IUserService service;
        public UserValidate(IUserService _service)
        {
            this.service = _service;
        }
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var User = service.ValiDateUser(context.UserName, context.Password);
            if (User != null)
            {
                context.Result = new GrantValidationResult(
                subject: context.UserName,
                authenticationMethod: OidcConstants.AuthenticationMethods.Password,
                claims: GetUserClaims(User));
            }
            else
            {
                //验证失败
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "用户名或者密码错误");

            }

            return Task.CompletedTask;
        }

        //可以根据需要设置相应的Claim
        private Claim[] GetUserClaims(User user)
        {
            return new Claim[]
            {
            new Claim("UserId", user.ID.ToString()),
            new Claim(JwtClaimTypes.Name,user.USERNAME),
            //new Claim(JwtClaimTypes.GivenName, "jaycewu"),
            //new Claim(JwtClaimTypes.FamilyName, "yyy"),
            //new Claim(JwtClaimTypes.Email, "977865769@qq.com"),
            //new Claim(JwtClaimTypes.Role,"admin")
            };
        }
    }
}
