using MediatR;
using TracNghiem.Domain.Entities;
using TracNghiem.Domain.Repositories;
using TracNghiem.WebAPI.Application.Commands.Categories;
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
    public class AddQuestionCommandHandler : IRequestHandler<AddQuestionCommand, Response<ResponseDefault>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMediator _mediator;
        private readonly IIdentityServices _identityServices;
        private readonly IHttpContextAccessor _httpContext;
        public AddQuestionCommandHandler(
            IQuestionRepository questionRepository,
            IMediator mediator,
            IIdentityServices identityServices,
            IHttpContextAccessor httpContext)
        {
            _questionRepository = questionRepository;
            _mediator = mediator;
            _identityServices = identityServices;
            _httpContext = httpContext;
        }
        public async Task<Response<ResponseDefault>> Handle(AddQuestionCommand request, CancellationToken cancellationToken)
        {
            string userName = _httpContext.HttpContext.User.Identity.Name.ToString();
            Question question = new Question();

            question.Explaint = request.Question.Explaint;
            question.QuestionContent = request.Question.QuestionContent;
            question.LevelId = request.Question.LevelId;
            //question.RightCount = request.Question.RightCount; 
            question.TypeId = request.Question.TypeId;
            question.UserCreate = userName;

            _questionRepository.Add(question);
            var result = await _questionRepository.UnitOfWork.SaveAsync(cancellationToken);
            if (result == 0)
            {
                return new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.ExcuteDB,
                    Result = new ResponseDefault()
                    {
                        Data = "add question fail"
                    }
                };
            }
            int rightCount = 0;
            foreach (var answerDto in request.Answers)
            {
                Answer answer = new Answer();
                answer.AnswerContent = answerDto.AnswerContent;
                answer.RightAnswer = answerDto.RightAnswer;
                answer.QuestionId = question.Id;
                if (answerDto.RightAnswer)
                {
                    rightCount++;
                }
                _questionRepository.Add(answer);
            }
            question.RightCount = rightCount;
            if (question.TypeId == 1 && question.RightCount > 1)
            {
                return new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.InvalidData,
                    Result = new ResponseDefault()
                    {
                        Data = "Single question can't have greater than 1 right answer"
                    }
                };
            }
            _questionRepository.Edit(question);

            result = await _questionRepository.UnitOfWork.SaveAsync(cancellationToken);
            if (result == 0)
            {
                return new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.ExcuteDB,
                    Result = new ResponseDefault()
                    {
                        Data = "add answer fail"
                    }
                };
            }
            if (request.Question.Categories.Count != 0)
            {
                var addQuestionCategoryCommand = new AddQuestionCategoryCommand()
                {
                    QuestionId = question.Id,
                    Categories = request.Question.Categories
                };
                await _mediator.Send(addQuestionCategoryCommand, cancellationToken);
            }
            return new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = "Add question success"
                }
            };
        }
    }
}
