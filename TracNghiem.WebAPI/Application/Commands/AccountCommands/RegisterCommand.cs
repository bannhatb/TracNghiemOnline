using MediatR;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Commands.AccountCommands
{
    public class RegisterCommand : IRequest<Response<ResponseDefault>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VerifyPassword { get; set; }
        public string Email { get; set; }
        public int ClassId { get; set; }
        public bool Gender { get; set; }
        //public DateTime DateOfBirth { get; set; }
    }
}
