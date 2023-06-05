using FluentValidation;
using TracNghiem.WebAPI.Application.Commands.AdminCommands;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Validations.AdminValidators
{
    public class BlockUserCommandValidator : AbstractValidator<BlockUserCommand>
    {
        public BlockUserCommandValidator()
        {
            RuleFor(x => x.userId).GreaterThan(0)
                .WithErrorCode(ErrorCode.InvalidData);
        }
    }
}
