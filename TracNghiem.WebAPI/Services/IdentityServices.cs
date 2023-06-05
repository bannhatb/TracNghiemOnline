using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using TracNghiem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Services
{
    public class IdentityServices : IIdentityServices
    {
        private readonly Audience _audience;
        public IdentityServices(
            IOptions<Audience> options)
        {
            _audience = options.Value ?? throw new ArgumentException(nameof(options.Value));
        }
        public string GenerateToken(int userId, string username, List<string> roles, int expires)
        {
            var now = DateTime.UtcNow;
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, JsonConvert.SerializeObject(roles),
                JsonClaimValueTypes.JsonArray)
            };

            var signningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_audience.Secret));

            var jwt = new JwtSecurityToken(
                issuer: _audience.Issuer,
                audience: _audience.Name,
                claims: claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromSeconds(expires)),
                signingCredentials: new SigningCredentials(signningKey,
                SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public string GetMD5(string text)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                md5.ComputeHash(Encoding.ASCII.GetBytes(text));
                byte[] result = md5.Hash;
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                {
                    str.Append(result[i].ToString("x2"));
                }
                return str.ToString();
            }
        }

        public string GetRandomLetters(int count)
        {
            string chars = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&";
            StringBuilder output = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < count; i++)
            {
                output.Append(random.Next(0, chars.Length - 1));
            }
            return output.ToString();
        }

        public bool VerifyMD5Hash(string inputHash, string hashVerify)
        {
            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(inputHash, hashVerify))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
