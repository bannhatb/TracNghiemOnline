using FluentValidation;
using TracNghiem.WebAPI.Application.Commands.TestCommands;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Validations.TestValidators
{
    public class ChooseAnswserCommandValidator : AbstractValidator<ChooseAnswerCommand>
    { 
        public ChooseAnswserCommandValidator()
        {
            RuleFor(x => x.TestQuestionId).GreaterThan(0).WithErrorCode(ErrorCode.InvalidData);
        }
    }
}
