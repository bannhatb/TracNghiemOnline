using MediatR;
using Microsoft.AspNetCore.Http;
using TracNghiem.Domain.Entities;
using TracNghiem.Domain.Repositories;
using TracNghiem.WebAPI.Infrastructure.Response;
using TracNghiem.WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Commands.Categories
{
    public class AddQuestionCategoryCommandHandler : IRequestHandler<AddQuestionCategoryCommand, Response<ResponseDefault>>
    {
        private readonly IQuestionRepository _questionRepository;
        private IHttpContextAccessor _httpContext;
        public AddQuestionCategoryCommandHandler(
            IQuestionRepository questionRepository,
            IHttpContextAccessor httpContext
            )
        {
            _questionRepository = questionRepository;
            _httpContext = httpContext;
        }
        public async Task<Response<ResponseDefault>> Handle(AddQuestionCategoryCommand request, CancellationToken cancellationToken)
        {
            Question question = _questionRepository.Questions.FirstOrDefault(x => x.Id == request.QuestionId);
            if (question == null)
            {
                return new Response<ResponseDefault>()
                {
                    State = true,
                    Message = "question Not found",
                    Result =
                    {
                        Data = ErrorCode.NotFound
                    }
                };
            }
            List<QuestionCategory> cateOld = _questionRepository.QuestionCategories
                .Where(x=>x.QuestionId == question.Id).ToList();
            if (cateOld.Count != 0)
            {
                //get cate giu nguyen
                HashSet<QuestionCategory> cateSame = new HashSet<QuestionCategory>();
                foreach (int cateId in request.Categories)
                {
                    QuestionCategory same = cateOld.FirstOrDefault(x => x.CategoryId == cateId);
                    if(same!= null)
                    {
                        cateSame.Add(same);
                    }
                }
                //get cate question delete 
                List<QuestionCategory> cateDel = cateOld.Except(cateSame).ToList();
                foreach (QuestionCategory quesCateDel in cateDel)
                {
                    _questionRepository.Delete(quesCateDel);
                }
                HashSet<int> cateNew = request.Categories
                    .Except(cateSame.Select(x => x.CategoryId)
                    .ToHashSet()).ToHashSet();
                foreach (int cateIdNew in cateNew)
                {
                    _questionRepository.Add(new QuestionCategory() { CategoryId = cateIdNew, QuestionId = question.Id });
                }
            }
            else
            {
                foreach (int cateId in request.Categories)
                {
                    _questionRepository.Add(new QuestionCategory() { CategoryId = cateId, QuestionId = question.Id });
                }
            }
            int result = await _questionRepository.UnitOfWork.SaveAsync(cancellationToken);
            if(result > 0)
            {
                return new Response<ResponseDefault>()
                {
                    State = true,
                    Message = ErrorCode.Success,
                    Result = new ResponseDefault()
                    {
                        Data = result.ToString()
                    }
                };
            }
            return new Response<ResponseDefault>()
            {
                State = false,
                Message = ErrorCode.BadRequest
            };
        }
    }
}
