using MediatR;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Commands.ExamCommands
{
    public class RemoveQuestionExamCommand : IRequest<Response<ResponseDefault>>
    {
        public int ExamId { get; set; }
        public int QuestionId { get; set; }
    }
}
