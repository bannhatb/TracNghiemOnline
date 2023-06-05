using FluentValidation;
using TracNghiem.WebAPI.Application.Commands.AccountCommands;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Validations.AccountValidators
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.UserName).MinimumLength(3).WithMessage("UserName length must geater than 5 character")
                .WithErrorCode(ErrorCode.ShortLengthData);
            RuleFor(x => x.UserName).MaximumLength(100).WithMessage("UserName length must less than 100 character")
                .WithErrorCode(ErrorCode.OverLengthData);
            RuleFor(x => x.Password).MinimumLength(5).WithMessage("Password length must geater than 5 character")
                .WithErrorCode(ErrorCode.ShortLengthData);
            RuleFor(x => x.Password).MaximumLength(100).WithMessage("Password length must less than 100 character")
                .WithErrorCode(ErrorCode.OverLengthData);
            // verify password
            RuleFor(x => x.Password).Equal(y => y.VerifyPassword).WithMessage("Password doesn't match verify")
                .WithErrorCode(ErrorCode.InvalidVerifyPassword);
            // username format
            RuleFor(x => x.UserName).Matches(@"^[A-Za-z0-9]+$")
                .WithErrorCode(ErrorCode.InvalidFormat);
            // email format
            RuleFor(x => x.Email).Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage("please type a correct email address")
                .WithErrorCode(ErrorCode.InvalidFormat);
            RuleFor(x => x.ClassId).GreaterThanOrEqualTo(0).WithErrorCode(ErrorCode.InvalidData);
            RuleFor(x => x.ClassId).LessThanOrEqualTo(6).WithErrorCode(ErrorCode.InvalidData);
        }
    }
}
