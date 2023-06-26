using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using TracNghiem.WebAPI.Infrastructure.Response;

namespace TracNghiem.WebAPI.Application.Commands.AccountCommands
{
    public class UpdateProfileUserCommand : IRequest<Response<ResponseDefault>>
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}