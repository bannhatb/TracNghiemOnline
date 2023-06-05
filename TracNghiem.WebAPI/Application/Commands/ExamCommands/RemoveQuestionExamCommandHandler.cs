using MediatR;
using Microsoft.AspNetCore.Http;
using TracNghiem.Domain.Entities;
using TracNghiem.Domain.Repositories;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Commands.ExamCommands
{
    public class RemoveQuestionExamCommandHandler : IRequestHandler<RemoveQuestionExamCommand, Response<ResponseDefault>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IExamRepository _examRepository; 
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContext;
        public RemoveQuestionExamCommandHandler(
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
        public async Task<Response<ResponseDefault>> Handle(RemoveQuestionExamCommand request, CancellationToken cancellationToken)
        {
            QuestionExam qe = _questionRepository.QuestionExams.FirstOrDefault(x => x.ExamId == request.ExamId && x.QuestionId == request.QuestionId);
            if(qe == null)
            {
                return new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound
                };
            }
            _questionRepository.Delete(qe);
            int result = await _questionRepository.UnitOfWork.SaveAsync();
            if (result == 0)
            {
                return new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.ExcuteDB,
                    Result = new ResponseDefault()
                    {
                        Data = "Excute Db error"
                    }
                };
            }
            return new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = request.ExamId
                }
            };
        }
    }
}
