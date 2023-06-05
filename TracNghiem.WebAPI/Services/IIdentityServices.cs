using TracNghiem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Services
{
    public interface IIdentityServices
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        /// <param name="expires"></param>
        /// <returns></returns>
        string GenerateToken(int userId, string username, List<string> roles, int expires);
        string GetMD5(string text);
        bool VerifyMD5Hash(string inputHash, string hashVerify);
    }
}
