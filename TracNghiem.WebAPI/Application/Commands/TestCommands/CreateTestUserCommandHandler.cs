using MediatR;
using Microsoft.AspNetCore.Http;
using TracNghiem.Domain.Entities;
using TracNghiem.Domain.Enum;
using TracNghiem.Domain.Repositories;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Commands.TestCommands
{
    public class CreateTestUserCommandHandler : IRequestHandler<CreateTestUserCommand, Response<ResponseDefault>>
    {
        private readonly ITestRepository _testRepository;
        private readonly IExamRepository _examRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContext;
        public CreateTestUserCommandHandler(
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
        public async Task<Response<ResponseDefault>> Handle(CreateTestUserCommand request, CancellationToken cancellationToken)
        {
            string userName = _httpContext.HttpContext.User.Identity.Name.ToString();
            User user = _userRepository.Users.FirstOrDefault(x => x.UserName == userName);
            Test test = _testRepository.Tests.FirstOrDefault(x => x.Id == request.TestId);
            if(test == null)
            {
                return new Response<ResponseDefault>()
                {
                    State = false,
                    Message = "Test khong ton tai",
                    Result = new ResponseDefault()
                    {
                        Data = ErrorCode.NotFound
                    }
                };
            }
            TestUser checkExist = _testRepository.TestUsers.FirstOrDefault(x => x.TestId == request.TestId && x.UserId == user.Id);
            if(checkExist != null)
            {
                return new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.DataExist,
                    Result = new ResponseDefault()
                    {
                        Data = checkExist.Id
                    }
                };
            }
            // get all question of exam 
            //check suffle and get number question satisfy questioncount 
            //create Test question 
            List<int> questionExamIds = _questionRepository.QuestionExams
                .Where(x => x.ExamId == test.ExamId).Select(x => x.QuestionId).ToList();
            TestUser testUser = new TestUser()
            {
                UserId = user.Id,
                TestId = request.TestId,
                Point = 0,
                Status = (int)TestStatusEnum.Created,
                TimeRemain = test.Time
            };
            _testRepository.Add(testUser);
            List<TestQuestion> questionTests = new List<TestQuestion>();
            if (!test.SuffleQuestion)
            {
                for(int i=0; i< test.QuestionCount; i++)
                {
                    TestQuestion tquestion = new TestQuestion() { 
                        NumericalOrder = i+1,
                        TestId = test.Id,
                        QuestionId = questionExamIds[i],
                        UserId = user.Id
                    };
                    _testRepository.Add(tquestion);
                }
            }
            else
            {
                List<int> questionSuffles = questionExamIds
                    .OrderBy(arg => Guid.NewGuid()).Take(test.QuestionCount).ToList();
                for (int i = 0; i < questionSuffles.Count; i++)
                {
                    TestQuestion tquestion = new TestQuestion()
                    {
                        NumericalOrder = i + 1,
                        TestId = test.Id,
                        QuestionId = questionSuffles[i],
                        UserId = user.Id
                    };
                    _testRepository.Add(tquestion);
                }
            }
            int result = await _testRepository.UnitOfWork.SaveAsync(cancellationToken);
            if(result == 0)
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
                    Data = testUser.Id
                }
            };
        }
    }
}
