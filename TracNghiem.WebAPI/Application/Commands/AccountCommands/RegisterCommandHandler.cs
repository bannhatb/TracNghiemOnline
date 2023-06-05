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
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Response<ResponseDefault>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentityServices _identityServices;
        public RegisterCommandHandler(IUserRepository userRepository,
            IIdentityServices identityServices)
        {
            _userRepository = userRepository;
            _identityServices = identityServices;
        }
        public async Task<Response<ResponseDefault>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            User isExist = _userRepository.Users.FirstOrDefault(x => x.UserName == request.UserName || x.Email == request.Email);

            if (isExist != null)
            {
                return new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.ExistUserOrEmail
                };
            }

            var user = new User()
            {
                UserName = request.UserName,
                Email = request.Email,
                Password = _identityServices.GetMD5(request.Password),
                Gender = request.Gender,
                ClassId = request.ClassId,
                IsBlock = false
            };

            _userRepository.Add(user);

            await _userRepository.UnitOfWork.SaveAsync(cancellationToken);

            var userRole = new UserRole()
            {
                UserId = user.Id,
                RoleId = (int)UserRoleEnum.Student
            };

            _userRepository.Add(userRole);

            var result = await _userRepository.UnitOfWork.SaveAsync(cancellationToken);

            if (result > 0)
            {
                return new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.Success,
                    Result = new ResponseDefault()
                    {
                        Data = user.Id.ToString()
                    }
                };
            }
            else
            {
                return new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.BadRequest
                };
            }
        }
    }
}
