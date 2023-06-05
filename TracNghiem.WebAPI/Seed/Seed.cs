using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;
using TracNghiem.Domain;
using TracNghiem.Domain.Entities;
using TracNghiem.WebAPI.Services;

namespace DatingApp.API.Data.Seed
{
    public static class Seed
    {

        public static void SeedUsers(TracnghiemContext _dataContext)
        {

            if (_dataContext.Users.Any()) return;

            var usersText = System.IO.File.ReadAllText("/Seed/users.json");

            var users = JsonSerializer.Deserialize<List<User>>(usersText);
            // var users = JsonConvert.DeserializeObject<List<User>>(usersText);

            // if(users == null) return;


            foreach (var user in users)
            {
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                md5.ComputeHash(Encoding.ASCII.GetBytes("123456"));
                byte[] result = md5.Hash;
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                {
                    str.Append(result[i].ToString("x2"));
                }
                user.Password = str.ToString();
                _dataContext.Users.Add(user);
            }
            _dataContext.SaveChanges();
        }
    }
}