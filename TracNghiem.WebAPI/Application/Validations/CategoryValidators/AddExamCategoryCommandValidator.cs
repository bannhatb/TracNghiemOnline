using FluentValidation;
using TracNghiem.WebAPI.Application.Commands.Categories;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Validations.CategoryValidators
{
    public class AddExamCategoryCommandValidator : AbstractValidator<AddExamCategoryCommand>
    {
        public AddExamCategoryCommandValidator()
        {
            RuleFor(x => x.Categories.Count).GreaterThan(0).WithErrorCode(ErrorCode.NullList).WithMessage("Categories is null");
            RuleFor(x => x.ExamId).GreaterThan(0).WithErrorCode(ErrorCode.InvalidData).WithMessage("Question Id is invalid");
            RuleForEach(x => x.Categories).GreaterThanOrEqualTo(0)
                .WithErrorCode(ErrorCode.InvalidData).WithMessage("Category Id is negative");
        }
    }
}
