using MediatR;
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
    public class ChooseAnswerCommandHandler : IRequestHandler<ChooseAnswerCommand, Response<ResponseDefault>>
    {
        private readonly ITestRepository _testRepository;
        private readonly IExamRepository _examRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        public ChooseAnswerCommandHandler(
            ITestRepository testRepository,
            IMediator mediator,
            IExamRepository examRepository,
            IQuestionRepository questionRepository,
            IUserRepository userRepository)
        {
            _testRepository = testRepository;
            _mediator = mediator;
            _examRepository = examRepository;
            _questionRepository = questionRepository;
            _userRepository = userRepository;
        }
        public async Task<Response<ResponseDefault>> Handle(ChooseAnswerCommand request, CancellationToken cancellationToken)
        {
            
            int questionId = _testRepository.TestQuestions.
                FirstOrDefault(x => x.Id == request.TestQuestionId).QuestionId;
            int typeQuestion = _questionRepository.Questions.FirstOrDefault(x => x.Id == questionId).TypeId;
            if(typeQuestion == 1) // single
            {
                TestQuestionResult exist = _testRepository.testQuestionResults
                    .FirstOrDefault(x => x.TestQuestionId == request.TestQuestionId);
                if(exist!= null)
                {
                    if(exist.Choose != request.AnswerIds[0])
                    {
                        exist.Choose = request.AnswerIds[0];
                        _testRepository.Edit(exist);
                    }
                }
                else
                {
                    exist = new TestQuestionResult()
                    {
                        Choose = request.AnswerIds[0],
                        TestQuestionId = request.TestQuestionId
                    };
                    _testRepository.Add(exist);
                }
            }
            else
            {
                List<int> existIds = _testRepository.testQuestionResults
                .Where(x => x.TestQuestionId == request.TestQuestionId).Select(x => x.Choose).ToList();
                //
                if (existIds != null)
                {
                    List<int> same = existIds.Intersect(request.AnswerIds).ToList();

                    List<int> del = existIds.Except(same).ToList();
                    TestQuestionResult answerCu = null;
                    foreach (int d in del)
                    {
                        answerCu = _testRepository.testQuestionResults.FirstOrDefault(x => x.Choose == d);
                        _testRepository.Delete(answerCu);
                    }
                    HashSet<int> answerNews = request.AnswerIds
                        .Except(same).ToHashSet();
                    foreach (int anNew in answerNews)
                    {
                        _testRepository.Add(new TestQuestionResult() { Choose = anNew, TestQuestionId = request.TestQuestionId });
                    }
                }
                else
                {
                    foreach (int anNew in request.AnswerIds)
                    {
                        _testRepository.Add(new TestQuestionResult()
                        { Choose = anNew, TestQuestionId = request.TestQuestionId });
                    }
                }
            }
            
            int result = await _testRepository.UnitOfWork.SaveAsync(cancellationToken);
            if (result == 0)
            {
                return new Response<ResponseDefault>()
                {
                    State = true,
                    Message = "Nothing change",
                    Result = new ResponseDefault()
                    {
                        Data = ErrorCode.NothingChange
                    }
                };
            }
            return new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success
            };
        }
    }
}
