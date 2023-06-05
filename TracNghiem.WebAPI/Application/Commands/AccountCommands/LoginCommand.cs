using MediatR;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Commands.AccountCommands
{
    public class LoginCommand : IRequest<Response<ResponseToken>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
