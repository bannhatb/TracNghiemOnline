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
    public class CheckPassCommandHandler : IRequestHandler<CheckPassTestCommand, Response<ResponseDefault>>
    {
        private readonly ITestRepository _testRepository;
        private readonly IExamRepository _examRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        public CheckPassCommandHandler(
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
        public async Task<Response<ResponseDefault>> Handle(CheckPassTestCommand request, CancellationToken cancellationToken)
        {
            Test test = _testRepository.Tests.FirstOrDefault(x => x.Id == request.TestId);
            if (test == null)
            {
                return new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.NotFound,
                    Result = new ResponseDefault()
                    {
                        Data = "test khong ton tai"
                    }
                };
            }
            if(test.Password != "" && test.Password != null)
            {
                if(test.Password != request.Password)
                {
                    return new Response<ResponseDefault>
                    {
                        State = true,
                        Message = ErrorCode.WrongPasss,
                        Result = new ResponseDefault()
                        {
                            Data = "Sai pass"
                        }
                    };
                }
            }
            return new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = "same"
                }
            };
        }
    }
}
