using MediatR;
using Microsoft.AspNetCore.Http;
using TracNghiem.Domain.Entities;
using TracNghiem.Domain.Repositories;
using TracNghiem.WebAPI.Application.Commands.Categories;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TracNghiem.WebAPI.Application.Commands.QuestionCommands;

namespace TracNghiem.WebAPI.Application.Commands.ExamCommands
{
    public class AddExamCommandHandler : IRequestHandler<AddExamCommand, Response<ResponseDefault>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IExamRepository _examRepository;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContext;
        public AddExamCommandHandler(
            IQuestionRepository questionRepository,
            IMediator mediator,
            IExamRepository examRepository,
            IHttpContextAccessor httpContext)
        {
            _questionRepository = questionRepository;
            _mediator = mediator;
            _examRepository = examRepository;
            _httpContext = httpContext;
        }
        public async Task<Response<ResponseDefault>> Handle(AddExamCommand request, CancellationToken cancellationToken)
        {
            string userName = _httpContext.HttpContext.User.Identity.Name.ToString();
            Exam exam = new Exam();
            exam.IsPublic = request.IsPublic;
            exam.QuestionCount = request.QuestionCount;
            exam.Title = request.Title;
            exam.Time = request.Time;
            exam.CreateBy = userName;

            _examRepository.Add(exam);

            var result = await _examRepository.UnitOfWork.SaveAsync(cancellationToken);
            if (result == 0)
            {
                return new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.ExcuteDB,
                    Result = new ResponseDefault()
                    {
                        Data = "add exam fail"
                    }
                };
            }

            // Thêm đề thi, danh mục đề thi
            var addExamCategoryCommand = new AddExamCategoryCommand()
            {
                ExamId = exam.Id,
                Categories = request.Categories
            };
            await _mediator.Send(addExamCategoryCommand, cancellationToken);

            // Thêm question vào exam

            List<int> totalIdQuestion = new List<int>();
            List<int> listQuestionLevel1 = new List<int>(),
                        listQuestionLevel2 = new List<int>(),
                        listQuestionLevel3 = new List<int>(),
                        listQuestionLevel4 = new List<int>(),
                        listQuestionLevel5 = new List<int>();

            if (request.RandomQuestion == true)
            {
                foreach (int cate in request.Categories)
                {
                    totalIdQuestion.AddRange(_questionRepository.GetListIdQuestionByCateId(cate, request.QuestionCount));
                }
                totalIdQuestion.Distinct().OrderBy(x => Guid.NewGuid()).Take(request.QuestionCount).ToList();
            }
            else
            {
                //Kiem tra tong so cau hoi
                int totalQuestion = request.QuestionCountLevel1 + request.QuestionCountLevel2 + request.QuestionCountLevel3 + request.QuestionCountLevel4 + request.QuestionCountLevel5;
                if (totalQuestion != request.QuestionCount)
                {
                    return new Response<ResponseDefault>()
                    {
                        State = false,
                        Message = ErrorCode.NotMapQuestionCount,
                        Result = new ResponseDefault()
                        {
                            Data = "total question not equal"
                        }
                    };
                }

                // lay question id theo tung loai cate
                foreach (int cate in request.Categories)
                {
                    listQuestionLevel1.AddRange(_questionRepository.GetListIdQuestionByCateIdAndLevelId(cate, request.QuestionCountLevel1, 1));
                    listQuestionLevel2.AddRange(_questionRepository.GetListIdQuestionByCateIdAndLevelId(cate, request.QuestionCountLevel2, 2));
                    listQuestionLevel3.AddRange(_questionRepository.GetListIdQuestionByCateIdAndLevelId(cate, request.QuestionCountLevel3, 3));
                    listQuestionLevel4.AddRange(_questionRepository.GetListIdQuestionByCateIdAndLevelId(cate, request.QuestionCountLevel4, 4));
                    listQuestionLevel5.AddRange(_questionRepository.GetListIdQuestionByCateIdAndLevelId(cate, request.QuestionCountLevel5, 5));
                }

                // 1 question có nhiều loại nên phải xét trường hợp trùng. sau đó lấy lại random số câu hỏi trong danh sách đã lọc
                listQuestionLevel1.Distinct().OrderBy(x => Guid.NewGuid()).Take(request.QuestionCountLevel1).ToList();
                listQuestionLevel2.Distinct().OrderBy(x => Guid.NewGuid()).Take(request.QuestionCountLevel2).ToList();
                listQuestionLevel3.Distinct().OrderBy(x => Guid.NewGuid()).Take(request.QuestionCountLevel3).ToList();
                listQuestionLevel4.Distinct().OrderBy(x => Guid.NewGuid()).Take(request.QuestionCountLevel4).ToList();
                listQuestionLevel5.Distinct().OrderBy(x => Guid.NewGuid()).Take(request.QuestionCountLevel5).ToList();

                //mỗi question có 1 level nên không cần xét trùng
                totalIdQuestion.AddRange(listQuestionLevel1);
                totalIdQuestion.AddRange(listQuestionLevel2);
                totalIdQuestion.AddRange(listQuestionLevel3);
                totalIdQuestion.AddRange(listQuestionLevel4);
                totalIdQuestion.AddRange(listQuestionLevel5);
            }

            totalIdQuestion = totalIdQuestion.OrderBy(x => x).ToList();

            //them question vao exam
            var addQuestionExamCommand = new AddQuestionExamCommand()
            {
                ExamId = exam.Id,
                QuestionIds = totalIdQuestion
            };
            await _mediator.Send(addQuestionExamCommand, cancellationToken);


            return new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = "Add exam success"
                }
            };
        }
    }
}
