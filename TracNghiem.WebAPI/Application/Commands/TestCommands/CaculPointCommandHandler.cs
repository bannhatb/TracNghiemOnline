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
    public class CaculPointCommandHandler : IRequestHandler<CaculPointCommand, Response<ResponseDefault>>
    {
        private readonly ITestRepository _testRepository;
        private readonly IExamRepository _examRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContext;
        public CaculPointCommandHandler(
            ITestRepository testRepository,
            IMediator mediator,
            IExamRepository examRepository,
            IQuestionRepository questionRepository,
            IHttpContextAccessor httpContext)
        {
            _testRepository = testRepository;
            _mediator = mediator;
            _examRepository = examRepository;
            _questionRepository = questionRepository;
            _httpContext = httpContext;
        }
        public async Task<Response<ResponseDefault>> Handle(CaculPointCommand request, CancellationToken cancellationToken)
        {
            int userId = Convert.ToInt32(_httpContext.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            TestUser testUser = _testRepository.TestUsers.FirstOrDefault(x => x.TestId == request.TestId && x.UserId == userId);
            if (testUser == null)
            {
                return new Response<ResponseDefault>
                {
                    State = false,
                    Message = ErrorCode.NotFound
                };
            }
            double point = 0;
            //get list Testquestion
            List<TestQuestion> testQuestions = _testRepository.TestQuestions.Where(x => x.TestId == request.TestId && x.UserId == userId).ToList();
            Test test = _testRepository.Tests.FirstOrDefault(x => x.Id == request.TestId);
            double pointPerQuestion = 10.0/test.QuestionCount;
            Question question = null;
            //save rightAnswer
            Answer rightAnswer = null;
            List<int> listAnswers = null;
            //for user choose
            TestQuestionResult userChoose = null;
            List<int> listChooses = null;
            foreach(TestQuestion tquestion in testQuestions)
            {
                question = _questionRepository.Questions.FirstOrDefault(x => x.Id == tquestion.QuestionId);
                if(question!= null)
                {
                    if(question.TypeId == 1)
                    {
                        //get rightAnswer
                        rightAnswer = _questionRepository.Answers.FirstOrDefault(x => x.QuestionId == question.Id && x.RightAnswer == true);
                        userChoose = _testRepository.testQuestionResults.FirstOrDefault(x => x.TestQuestionId == tquestion.Id);
                        if(rightAnswer != null && userChoose != null)
                        {
                            if (userChoose.Choose == rightAnswer.Id)
                                point += pointPerQuestion;
                        }
                    }
                    else // multy choice question
                    {
                        listAnswers = _questionRepository.Answers
                            .Where(x => x.QuestionId == question.Id && x.RightAnswer == true).Select(x => x.Id).ToList();
                        listChooses = _testRepository.testQuestionResults
                            .Where(x => x.TestQuestionId == tquestion.Id).Select(x => x.Choose).ToList();
                        if(listAnswers != null && listChooses != null)
                        {
                            point += listChooses.Intersect(listAnswers)
                                .Count() * (pointPerQuestion / listAnswers.Count());
                        }
                    }
                }
            }
            point = Math.Round(point, 2);
            if(testUser.Point == point)
            {
                return new Response<ResponseDefault>()
                {
                    State = false,
                    Message = "same point after cacul",
                    Result = new ResponseDefault()
                    {
                        Data = ErrorCode.SameData
                    }
                };
            }
            testUser.Status = (int)TestStatusEnum.Done;
            testUser.Point = point;
            _testRepository.Edit(testUser);
            int result = await _testRepository.UnitOfWork.SaveAsync(cancellationToken);
            if (result == 0)
            {
                return new Response<ResponseDefault>()
                {
                    State = true,
                    Message = "nothing change",
                    Result = new ResponseDefault()
                    {
                        Data = ErrorCode.NothingChange
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
