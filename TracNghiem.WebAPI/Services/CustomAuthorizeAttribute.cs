using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Services
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public string Allows { get; set; }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            List<string> userRoles = context.HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x=>x.Value).ToList();
            //nếu không cần quyền
            if (string.IsNullOrEmpty(Allows))
                return;
            //token gửi lên không có role
            List<string> allowRoles = null;
            if (Allows != null)
            {
                allowRoles = this.Allows.Split(',').ToList();
            }
            if (userRoles == null || userRoles.Count == 0)
            {
                context.Result = new BadRequestObjectResult(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.Forbidden,
                    Result = new ResponseDefault()
                    {
                        Data = "Bạn không có quyền truy cập."
                    }
                });
                context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            }
            int countPass = allowRoles.Intersect(userRoles).Count();
            if(countPass == 0)
            {
                context.Result = new BadRequestObjectResult(new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.Forbidden,
                    Result = new ResponseDefault()
                    {
                        Data = "Bạn không có quyền truy cập."
                    }
                });
                context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            }
            return;
        }
    }
}
