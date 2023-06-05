using MediatR;
using Microsoft.AspNetCore.Http;
using TracNghiem.Domain.Entities;
using TracNghiem.Domain.Enum;
using TracNghiem.Domain.Repositories;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Commands.TestCommands
{
    public class StartUserTestCommandHandler : IRequestHandler<StartUserTestCommand, Response<ResponseDefault>>
    {
        private readonly ITestRepository _testRepository;
        private readonly IExamRepository _examRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContext;
        public StartUserTestCommandHandler(
            ITestRepository testRepository,
            IMediator mediator,
            IExamRepository examRepository,
            IQuestionRepository questionRepository,
            IUserRepository userRepository,
            IHttpContextAccessor httpContext)
        {
            _testRepository = testRepository;
            _mediator = mediator;
            _examRepository = examRepository;
            _questionRepository = questionRepository;
            _userRepository = userRepository;
            _httpContext = httpContext;
        }
        public async Task<Response<ResponseDefault>> Handle(StartUserTestCommand request, CancellationToken cancellationToken)
        {
            int userId = Convert.ToInt32
                (_httpContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            TestUser testUser = _testRepository.TestUsers.FirstOrDefault(x => x.UserId == userId && x.TestId == request.TestId);
            if(testUser == null)
            {
                return new Response<ResponseDefault>
                {
                    State = false,
                    Message = ErrorCode.NotFound
                };
            }
            if(testUser.Status == (int)TestStatusEnum.Done)
            {
                return new Response<ResponseDefault>
                {
                    State = true,
                    Message = ErrorCode.TestUserDone,
                    Result = new ResponseDefault()
                    {
                        Data = "User đã hoàn thành bài thi"
                    }
                };
            }
            testUser.Status = (int)TestStatusEnum.Doing;
            _testRepository.Edit(testUser);
            int result = await _testRepository.UnitOfWork.SaveAsync(cancellationToken);
            if (result == 0)
            {
                return new Response<ResponseDefault>()
                {
                    State = true,
                    Message = "User dang lam test",
                    Result = new ResponseDefault()
                    {
                        Data = ErrorCode.UserDoingTest
                    }
                };
            }

            return new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = testUser.Id
                }
            };
        }
    }
}
