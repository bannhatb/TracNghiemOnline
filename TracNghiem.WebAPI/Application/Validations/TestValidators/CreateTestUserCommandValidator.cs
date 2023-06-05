using FluentValidation;
using TracNghiem.WebAPI.Application.Commands.TestCommands;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Validations.TestValidators
{
    public class CreateTestUserCommandValidator : AbstractValidator<CreateTestUserCommand>
    {
        public CreateTestUserCommandValidator()
        {
            RuleFor(x => x.TestId).GreaterThanOrEqualTo(1)
                .WithMessage("Test id must large than 0").WithErrorCode(ErrorCode.InvalidData);
        }
    }
}
