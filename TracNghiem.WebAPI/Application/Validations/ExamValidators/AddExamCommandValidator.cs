using FluentValidation;
using TracNghiem.WebAPI.Application.Commands.ExamCommands;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Validations.ExamValidators
{
    public class AddExamCommandValidator : AbstractValidator<AddExamCommand>
    {
        public AddExamCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithErrorCode(ErrorCode.NotEmpty)
                .WithMessage("Exam Title is empty !!!");
            RuleFor(x => x.Title)
                .MaximumLength(255)
                .WithErrorCode(ErrorCode.OverLengthData)
                .WithMessage("Exam Title is over length !!!");
        }
    }
}
