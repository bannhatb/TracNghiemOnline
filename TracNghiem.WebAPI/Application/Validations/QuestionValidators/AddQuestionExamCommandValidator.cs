using FluentValidation;
using TracNghiem.WebAPI.Application.Commands.QuestionCommands;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Validations.QuestionValidators
{
    public class AddQuestionExamCommandValidator : AbstractValidator<AddQuestionExamCommand>
    {
        public AddQuestionExamCommandValidator()
        {
            RuleFor(x => x.QuestionIds).NotEmpty()
                .WithErrorCode(ErrorCode.NotEmpty)
                .WithMessage("QuestionId is empty !!!");
            RuleFor(x => x.ExamId).NotEmpty()
                .WithErrorCode(ErrorCode.NotEmpty)
                .WithMessage("ExamId is empty !!!");
        }
    }
}
