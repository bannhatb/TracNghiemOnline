using MediatR;
using Microsoft.EntityFrameworkCore;
using TracNghiem.Domain.Entities;
using TracNghiem.Domain.Repositories;
using TracNghiem.WebAPI.Infrastructure.Exceptions;
using TracNghiem.WebAPI.Infrastructure.Response;
using TracNghiem.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Commands.QuestionCommands
{
    public class AddQuestionExamCommandHandler : IRequestHandler<AddQuestionExamCommand, Response<ResponseDefault>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMediator _mediator;
        private readonly IExamRepository _examRepository;
        private readonly IIdentityServices _identityServices;
        public AddQuestionExamCommandHandler(
            IQuestionRepository questionRepository,
            IMediator mediator,
            IExamRepository examRepository,
            IIdentityServices identityServices)
        {
            _questionRepository = questionRepository;
            _mediator = mediator;
            _examRepository = examRepository;
            _identityServices = identityServices;
        }
        public async Task<Response<ResponseDefault>> Handle(AddQuestionExamCommand request, CancellationToken cancellationToken)
        {
            
            var examId = request.ExamId;
            var exam = await _examRepository.Exams.FirstOrDefaultAsync(x => x.Id == examId, cancellationToken);
            if (exam is null)
            {
                throw new DomainException("Exam is not exist!", new DomainException(ErrorCode.NotFound));
            }
            var qInExam = _questionRepository.QuestionExams.Where(x => x.ExamId == examId).Select(x => x.QuestionId).ToList();
            foreach (var qs in request.QuestionIds)
            {
                //int q = q.QuestionId;
                if (qInExam.Contains(qs))
                {
                    continue;
                }

                var question = await _questionRepository.Questions
                    .FirstOrDefaultAsync(q => q.Id == qs, cancellationToken);

                if (question is null)
                {
                    throw new DomainException("question can't be added to exam!", new DomainException(ErrorCode.QuestionNotExist));
                }

                var questionExam = new QuestionExam()
                {
                    ExamId = examId,
                    QuestionId = qs
                };
                _questionRepository.Add(questionExam);

            }
            var result = await _examRepository.UnitOfWork.SaveAsync(cancellationToken);
            if (result == 0)
            {
                return new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.ExcuteDB,
                    Result = new ResponseDefault()
                    {
                        Data = "Add question exam fail"
                    }
                };
            }

            return new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = "Add question exam success"
                }
            };
        }
    }
    
}
