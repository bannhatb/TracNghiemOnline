using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MediatR;
using Microsoft.AspNetCore.Http;
using TracNghiem.Domain.Entities;
using TracNghiem.Domain.Repositories;
using TracNghiem.WebAPI.Application.Commands.QuestionCommands;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Commands.ExamCommands
{
    public class GenQuestionCommandHandler : IRequestHandler<GenQuestionCommand, Response<ResponseDefault>>
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IExamRepository _examRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IMediator _mediator;
        public GenQuestionCommandHandler(IHttpContextAccessor httpContext, IExamRepository examRepository,
            IQuestionRepository questionRepository,
            IMediator mediator)
        {
            _httpContext = httpContext;
            _examRepository = examRepository;
            _questionRepository = questionRepository;
            _mediator = mediator;
        }
        public async Task<Response<ResponseDefault>> Handle(GenQuestionCommand request, CancellationToken cancellationToken)
        {
            string userName = _httpContext.HttpContext.User.Identity.Name.ToString();
            List<string> data = new List<string>();

            WordprocessingDocument wordDocument = WordprocessingDocument.Open(request.FileUp, false);
            foreach (Paragraph co in wordDocument.MainDocumentPart.Document.Body.Descendants<Paragraph>())
            {
                if (!string.IsNullOrWhiteSpace(co.InnerText))
                    data.Add(co.InnerText.Trim());
            }
            List<int> questionIds = new List<int>();
            int LastQuestionId = 0;
            for (int i = 0; i < data.Count; i++)
            {
                StringBuilder sb = new StringBuilder();

                if (47 < (int)data[i][0] && (int)data[i][0] < 58)
                {
                    //loại bỏ stt câu
                    StringBuilder numericalOrder = new StringBuilder();
                    int k = 0;
                    while (47 < (int)data[i][k] && (int)data[i][k] < 58)
                    {
                        numericalOrder.Append(data[i][k]);
                        k++;
                    }
                    string str = "";
                    if (data[i][k].ToString() == request.SplitNumberAndContent)
                    {
                        str = data[i].Substring(k + 1);
                    }
                    else
                    {
                        str = data[i].Substring(k + 1);
                    }

                    sb.Append(str);
                    if (i < data.Count - 1)
                    {
                        int j = i + 1;
                        while (!((64 < (int)data[j][0] && (int)data[j][0] < 91) && data[j][1].ToString() == "."))
                        {
                            sb.Append(data[j]);
                            if (j == data.Count - 1)
                            {
                                break;
                            }
                            j++;
                        }
                        i = j - 1;
                    }

                    //update info pre question
                    if (LastQuestionId != 0)
                    {
                        await _questionRepository.UnitOfWork.SaveAsync();
                        Question PreQuestion = _questionRepository.Questions.FirstOrDefault(x => x.Id == LastQuestionId);
                        List<Answer> answerPre = _questionRepository.Answers.Where(x => x.QuestionId == LastQuestionId).ToList();
                        int countRight = answerPre.Where(x => x.RightAnswer == true).Count();
                        if (countRight > 1)
                        {
                            PreQuestion.TypeId = 2;
                            PreQuestion.RightCount = countRight;
                        }
                        else
                        {
                            PreQuestion.TypeId = 1;
                            PreQuestion.RightCount = countRight;
                        }
                        _questionRepository.Edit(PreQuestion);
                    }
                    Question question = new Question()
                    {
                        QuestionContent = sb.ToString(),
                        RightCount = 0,
                        TypeId = 1
                    };
                    _questionRepository.Add(question);
                    int a = await _questionRepository.UnitOfWork.SaveAsync();
                    if (question.Id != 0)
                    {
                        LastQuestionId = question.Id;
                        questionIds.Add(question.Id);
                    }
                }
                else if (((64 < (int)data[i][0] && (int)data[i][0] < 91) && data[i][1].ToString() == "."))
                {
                    sb.Append(data[i]);
                    bool right = false;
                    if (i < data.Count - 1)
                    {
                        int j = i + 1;
                        while (!((64 < (int)data[j][0] && (int)data[j][0] < 91) && data[j][1].ToString() == ".") && !(47 < (int)data[j][0] && (int)data[j][0] < 58))
                        {
                            sb.Append(data[j]);
                            if (j == data.Count - 1)
                            {
                                break;
                            }
                            j++;
                        }
                        i = j - 1;
                    }

                    if (sb.ToString().Contains(request.rightMark))
                    {
                        right = true;
                        int index = sb.ToString().IndexOf(request.rightMark);
                        sb.Remove(index, 1);
                    }
                    Answer answer = new Answer()
                    {
                        AnswerContent = sb.ToString(),
                        QuestionId = LastQuestionId,
                        RightAnswer = right
                    };
                    _questionRepository.Add(answer);
                }
            }
            AddQuestionExamCommand command = new AddQuestionExamCommand()
            {
                ExamId = request.examId,
                QuestionIds = questionIds
            };
            var result = await _mediator.Send(command);
            if (result.State == false)
            {
                return new Response<ResponseDefault>()
                {
                    State = false,
                    Message = ErrorCode.BadRequest,
                    Result = new ResponseDefault()
                    {
                        Data = "loi kghi add question vao exam"
                    }
                };
            }
            Exam exam = _examRepository.Exams.FirstOrDefault(x => x.Id == request.examId);
            int questionCount = _questionRepository.QuestionExams.Where(x => x.ExamId == request.examId).Count();
            exam.QuestionCount = questionCount;
            _examRepository.Edit(exam);
            await _examRepository.UnitOfWork.SaveAsync();
            return new Response<ResponseDefault>
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault
                {
                    Data = data
                }
            };
        }
    }
}
