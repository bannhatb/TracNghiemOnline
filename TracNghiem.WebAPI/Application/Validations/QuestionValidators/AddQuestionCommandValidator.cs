using FluentValidation;
using TracNghiem.WebAPI.Application.Commands.QuestionCommands;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Validations.QuestionValidators
{
    public class AddQuestionCommandValidator : AbstractValidator<AddQuestionCommand>
    {
        public AddQuestionCommandValidator()
        {
            #region question
            RuleFor(x => x.Question.QuestionContent).NotEmpty()
                .WithErrorCode(ErrorCode.NotEmpty)
                .WithMessage("QuestionContent is empty !!!");
            RuleFor(x => x.Question.QuestionContent).MaximumLength(255)
                .WithErrorCode(ErrorCode.OverLengthData)
                .WithMessage("QuestionContent is OverLength !!!");
            RuleFor(x => x.Question.Explaint).MaximumLength(255)
                .WithErrorCode(ErrorCode.OverLengthData)
                .WithMessage("Explaint is over length !!!");
            RuleFor(x => x.Question).Must(x=>x.TypeId >=0 && x.TypeId <3)
                .WithErrorCode(ErrorCode.FalseTypeQuestion)
                .WithMessage("Question QuestionTypeId is invalid !!!");
            #endregion

            RuleFor(x => x.Answers)
                .Must(x => x.Count > 1)
                .WithErrorCode(ErrorCode.AnswerLess)
                .WithMessage("Quantity of answers must be more than 1 !!!");

            RuleFor(x => x.Answers).Must(x => x.FindAll(a => a.RightAnswer == true).Count > 0)
                .WithErrorCode(ErrorCode.MissingRightAnswer)
                .WithMessage("Right answer is missing !!!");
        }
    }
}
