using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TracNghiem.Domain.Entities;
using TracNghiem.Domain.Repositories;
using TracNghiem.WebAPI.Application.Commands.Categories;
using TracNghiem.WebAPI.Infrastructure.Exceptions;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Commands.ExamCommands
{
    public class UpdateExamCommandHandler : IRequestHandler<UpdateExamCommand, Response<ResponseDefault>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IExamRepository _examRepository;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContext;
        public UpdateExamCommandHandler(
            IQuestionRepository questionRepository,
            IMediator mediator,
            IExamRepository examRepository,
            IHttpContextAccessor httpContext)
        {
            _questionRepository = questionRepository;
            _mediator = mediator;
            _examRepository = examRepository;
            _httpContext = httpContext;
        }

        public async Task<Response<ResponseDefault>> Handle(UpdateExamCommand request, CancellationToken cancellationToken)
        {
            string userName = _httpContext.HttpContext.User.Identity.Name.ToString();

            var exam = await _examRepository.Exams.FirstOrDefaultAsync(x => x.CreateBy == userName && x.Id == request.Exam.Id, cancellationToken);
            if (exam is null)
            {
                throw new DomainException("Exam is not exist!", new DomainException(ErrorCode.ExamNotExist));
            }
            exam.IsPublic = request.Exam.IsPublic;
            exam.QuestionCount = request.Exam.QuestionCount;
            exam.Title = request.Exam.Title;
            exam.Time = request.Exam.Time;

            _examRepository.Edit(exam);

            var result = await _examRepository.UnitOfWork.SaveAsync(cancellationToken);
            if (result == 0)
            {
                return new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.ExcuteDB,
                    Result = new ResponseDefault()
                    {
                        Data = "Update exam fail"
                    }
                };
            }

            //update category exam
            var addExamCategoryCommand = new AddExamCategoryCommand()
            {
                ExamId = exam.Id,
                Categories = request.Exam.Categories
            };
            await _mediator.Send(addExamCategoryCommand, cancellationToken);

            return new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = "Update exam success"
                }
            };
        }
    }
}
