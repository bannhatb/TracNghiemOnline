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

namespace TracNghiem.WebAPI.Application.Commands.Categories
{
    public class AddExamCategoryCommandHandler : IRequestHandler<AddExamCategoryCommand, Response<ResponseDefault>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IExamRepository _examRepository;
        private IHttpContextAccessor _httpContext;
        public AddExamCategoryCommandHandler(
            IQuestionRepository questionRepository,
            IExamRepository examRepository,
            IHttpContextAccessor httpContext
            )
        {
            _questionRepository = questionRepository;
            _examRepository = examRepository;
            _httpContext = httpContext;
        }
        public async Task<Response<ResponseDefault>> Handle(AddExamCategoryCommand request, CancellationToken cancellationToken)
        {
            Exam exam = _examRepository.Exams.FirstOrDefault(x => x.Id == request.ExamId);
            if (exam == null)
            {
                return new Response<ResponseDefault>()
                {
                    State = true,
                    Message = "exam Not found",
                    Result =
                    {
                        Data = ErrorCode.NotFound
                    }
                };
            }
            List<ExamCategory> cateOld = _examRepository.ExamCategories
                .Where(x => x.ExamId == exam.Id).ToList();
            if (cateOld.Count != 0)
            {
                //get cate giu nguyen
                HashSet<ExamCategory> cateSame = new HashSet<ExamCategory>();
                foreach (int cateId in request.Categories)
                {
                    ExamCategory same = cateOld.FirstOrDefault(x => x.CategoryId == cateId);
                    if (same != null)
                    {
                        cateSame.Add(same);
                    }
                }
                //get cate exam delete 
                List<ExamCategory> cateDel = cateOld.Except(cateSame).ToList();
                foreach (ExamCategory examDel in cateDel)
                {
                    _examRepository.Delete(examDel);
                }
                HashSet<int> cateNew = request.Categories
                    .Except(cateSame.Select(x => x.CategoryId)
                    .ToHashSet()).ToHashSet();
                foreach (int cateIdNew in cateNew)
                {
                    _examRepository.Add(new ExamCategory() { CategoryId = cateIdNew, ExamId = exam.Id });
                }
            }
            else
            {
                foreach (int cateId in request.Categories)
                {
                    _examRepository.Add(new ExamCategory() { CategoryId = cateId, ExamId = exam.Id });
                }
            }
            int result = await _questionRepository.UnitOfWork.SaveAsync(cancellationToken);
            if (result > 0)
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
