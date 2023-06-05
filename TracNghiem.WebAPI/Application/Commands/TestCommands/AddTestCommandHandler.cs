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

namespace TracNghiem.WebAPI.Application.Commands.TestCommands
{
    public class AddTestCommandHandler : IRequestHandler<AddTestCommand, Response<ResponseDefault>>
    {
        private readonly ITestRepository _testRepository;
        private readonly IExamRepository _examRepository;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContext;
        public AddTestCommandHandler(
            ITestRepository testRepository,
            IMediator mediator,
            IExamRepository examRepository,
            IHttpContextAccessor httpContext)
        {
            _testRepository = testRepository;
            _mediator = mediator;
            _examRepository = examRepository;
            _httpContext = httpContext;
        }
        public async Task<Response<ResponseDefault>> Handle(AddTestCommand request, CancellationToken cancellationToken)
        {
            string userName = _httpContext.HttpContext.User.Identity.Name.ToString();
            Exam checkSoCau = _examRepository.Exams.FirstOrDefault(x => x.Id == request.ExamId);
            if (checkSoCau == null)
            {
                return new Response<ResponseDefault>()
                {
                    State = false,
                    Message = "Exam khong ton tai",
                    Result = new ResponseDefault()
                    {
                        Data = ErrorCode.NotFound
                    }
                };
            }
            if (checkSoCau.QuestionCount < request.QuestionCount)
            {
                return new Response<ResponseDefault>()
                {
                    State = true,
                    Message = "So cau hoi vuot qua so cau hoi cua exam",
                    Result = new ResponseDefault()
                    {
                        Data = ErrorCode.InvalidQuestionCount
                    }
                };
            }
            Test test = new Test();
            if (request.QuestionCount == 0)
            {
                test.QuestionCount = checkSoCau.QuestionCount;
            }
            else
            {
                test.QuestionCount = request.QuestionCount;
            }

            test.CreateBy = userName;
            test.HideAnswer = request.HideAnswer;
            test.StartAt = request.StartAt;
            test.EndAt = request.EndAt;
            test.ExamId = request.ExamId;
            test.SuffleQuestion = request.ShuffleQuestion;
            test.Password = request.Password;
            test.Time = request.Time;
            _testRepository.Add(test);
            int result = await _testRepository.UnitOfWork.SaveAsync(cancellationToken);
            if (result == 0)
            {
                return new Response<ResponseDefault>()
                {
                    State = false,
                    Message = "Db excute error",
                    Result = new ResponseDefault()
                    {
                        Data = ErrorCode.ExcuteDB
                    }
                };
            }

            return new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = test.Id
                }
            };
        }
    }
}
