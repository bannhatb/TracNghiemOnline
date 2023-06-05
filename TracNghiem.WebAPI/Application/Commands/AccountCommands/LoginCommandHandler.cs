using MediatR;
using TracNghiem.Domain.Entities;
using TracNghiem.Domain.Enum;
using TracNghiem.Domain.Repositories;
using TracNghiem.WebAPI.Infrastructure.Response;
using TracNghiem.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Commands.AccountCommands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Response<ResponseToken>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentityServices _identityServices;
        public LoginCommandHandler(IUserRepository userRepository,
            IIdentityServices identityServices)
        {
            _userRepository = userRepository;
            _identityServices = identityServices;
        }
        public async Task<Response<ResponseToken>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            User user = _userRepository.Users.FirstOrDefault(x => x.UserName == request.UserName || x.Email == request.UserName);

            if (user == null)
            {
                return new Response<ResponseToken>()
                {
                    State = false,
                    Message = ErrorCode.NotFound
                };
            }
            if(user.IsBlock == true)
            {
                return new Response<ResponseToken>()
                {
                    State = false,
                    Message = ErrorCode.BlockUser
                };
            }
            if (_identityServices.VerifyMD5Hash(user.Password, _identityServices.GetMD5(request.Password)))
            {
                int timeOut = 60 * 60 *24;

                List<int> roleIds = _userRepository.UserRoles
                    .Where(x => x.UserId == user.Id).Select(x => x.RoleId).ToList();
                string token = _identityServices.GenerateToken(
                    user.Id,user.UserName,
                    roleIds.Select(x=>((UserRoleEnum)x).ToString()).ToList(),
                    timeOut);

                return new Response<ResponseToken>()
                {
                    State = true,
                    Message = ErrorCode.Success,
                    Result = new ResponseToken()
                    {
                        Token = token,
                    }
                };
            }
            else
            {
                return new Response<ResponseToken>()
                {
                    State = false,
                    Message = ErrorCode.BadRequest,
                };
            }
        }
    }
}
