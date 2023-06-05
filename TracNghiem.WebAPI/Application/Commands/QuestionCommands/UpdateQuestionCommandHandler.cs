using MediatR;
using Microsoft.EntityFrameworkCore;
using TracNghiem.Domain.Entities;
using TracNghiem.Domain.Repositories;
using TracNghiem.WebAPI.Application.Commands.Categories;
using TracNghiem.WebAPI.Infrastructure.Exceptions;
using TracNghiem.WebAPI.Infrastructure.ModelDto;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Commands.QuestionCommands
{
    public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, Response<ResponseDefault>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMediator _mediator;
        public UpdateQuestionCommandHandler(
            IQuestionRepository questionRepository,
            IMediator mediator)
        {
            _questionRepository = questionRepository;
            _mediator = mediator;
        }
        public async Task<Response<ResponseDefault>> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await _questionRepository.Questions.FirstOrDefaultAsync(q => q.Id == request.Question.QuestionId);
            if (question is null)
            {
                throw new DomainException("Question is not exist!", new DomainException(ErrorCode.QuestionNotExist));
            }
            question.Explaint = request.Question.Explaint;
            question.QuestionContent = request.Question.QuestionContent;
            question.LevelId = request.Question.LevelId;
            //question.RightCount = request.Question.RightCount;
            question.TypeId = request.Question.TypeId;
            _questionRepository.Edit(question);

            List<Answer> listSame = null;
            //danh sach answer do nguoi dung nhap vao
            // danh sach answer giu nguyen
            foreach (var ansSame in request.Answers)
            {
                if (ansSame.AnswerId == 0) continue;
                Answer temp = await _questionRepository.Answers.FirstOrDefaultAsync(x => x.Id == ansSame.AnswerId, cancellationToken);
                if (temp == null)
                {
                    return new Response<ResponseDefault>()
                    {
                        State = false,
                        Message = ErrorCode.NotFound,
                        Result = new ResponseDefault()
                        {
                            Data = "not found answer"
                        }
                    };
                }
                temp.AnswerContent = ansSame.AnswerContent;
                listSame.Add(temp);
                _questionRepository.Edit(temp);
            }

            var listAnswerDatabase = _questionRepository.Answers.Where(x => x.QuestionId == question.Id).ToList();
            //
            if (listAnswerDatabase.Count > 0)
            {
                var deleteAnswes = listAnswerDatabase.Except(listSame).ToList();
                if (deleteAnswes.Count > 0)
                {
                    foreach (var idDelete in deleteAnswes)
                    {
                        _questionRepository.Delete(idDelete);
                    }
                }
            }


            //var newAnswer = listAnswerRequest.Except(listAnswerDatabase).ToList();
            //Them moi answer

            foreach (var answerDto in request.Answers)
            {
                if (answerDto.AnswerId != 0) continue;
                Answer ans = new Answer();
                ans.AnswerContent = answerDto.AnswerContent;
                ans.RightAnswer = answerDto.RightAnswer;
                ans.QuestionId = question.Id;
                _questionRepository.Add(ans);
            }


            //if (listAnswerRequest!= null && listAnswerRequest.Count > 0) //xoa 1 so cau tra loi
            //{
            //    //xoa answer khong co trong danh sach submit
            //    var deleteItems = await _questionRepository.Answers
            //        .Where(x => x.QuestionId == request.Question.QuestionId && !listAnswerRequest.Contains(x.Id))
            //        .ToListAsync(cancellationToken);
            //    foreach(var item in deleteItems)
            //    {
            //        _questionRepository.Delete(item);
            //    }

            //    //cap nhat cac item con lại
            //    foreach(var id in listAnswerRequest)
            //    {
            //        var answerUpdate = await request.Answers.FirstOrDefault(x => x.AnswerId == id.TryFormat(int))
            //    }
            //}

            ////new answer list
            //List<AnswerDto> newAnswers = request.Answers.Where(x => x.AnswerId == 0).ToList();

            //foreach (var answerDto in newAnswers)
            //{
            //    Answer newAnswer = new Answer();
            //    newAnswer.AnswerContent = answerDto.AnswerContent;
            //    newAnswer.RightAnswer = answerDto.RightAnswer;
            //    newAnswer.QuestionId = question.Id;
            //    _questionRepository.Add(newAnswer);

            //}

            var result = await _questionRepository.UnitOfWork.SaveAsync(cancellationToken);

            if (result == 0)
            {
                throw new DomainException("Edit Question Failure!", new DomainException(ErrorCode.ExcuteDB));
            }

            var addQuestionCategoryCommand = new AddQuestionCategoryCommand()
            {
                QuestionId = question.Id,
                Categories = request.Question.Categories
            };
            await _mediator.Send(addQuestionCategoryCommand, cancellationToken);

            return new Response<ResponseDefault>()
            {
                State = true,
                Message = ErrorCode.Success,
                Result = new ResponseDefault()
                {
                    Data = "Update question success"
                }
            };

        }
    }
}
