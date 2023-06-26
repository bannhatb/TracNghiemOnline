using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TracNghiem.Domain.Repositories;
using TracNghiem.WebAPI.Infrastructure.Exceptions;
using TracNghiem.WebAPI.Infrastructure.Response;

namespace TracNghiem.WebAPI.Application.Commands.AccountCommands
{
    public class UpdateProfileUserCommandHandler : IRequestHandler<UpdateProfileUserCommand, Response<ResponseDefault>>
    {
        private readonly IUserRepository _userRepository;
        public UpdateProfileUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Response<ResponseDefault>> Handle(UpdateProfileUserCommand request, CancellationToken cancellationToken)
        {
            // string userName = _httpContext.HttpContext.User.Identity.Name.ToString();

            var user = await _userRepository.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
            if (user is null)
            {
                throw new DomainException("User is not exist!", new DomainException(ErrorCode.ExamNotExist));
            }
            user.UserName = request.UserName;
            user.Email = request.Email;
            user.LastName = request.LastName;
            user.FirstName = request.FirstName;
            user.DateOfBirth = request.DateOfBirth;
            user.Gender = request.Gender;


            _userRepository.Edit(user);

            var result = await _userRepository.UnitOfWork.SaveAsync(cancellationToken);
            if (result == 0)
            {
                return new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.ExcuteDB,
                    Result = new ResponseDefault()
                    {
                        Data = "Update user fail"
                    }
                };
            }

            return new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = "Update user success"
                }
            };
        }
    }
}