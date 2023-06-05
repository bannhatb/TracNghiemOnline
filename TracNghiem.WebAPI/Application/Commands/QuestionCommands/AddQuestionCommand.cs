using MediatR;
using TracNghiem.WebAPI.Infrastructure.ModelDto;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Commands.QuestionCommands
{
    public class AddQuestionCommand : IRequest<Response<ResponseDefault>>
    {
        public QuestionDto Question { get; set; }
        public List<AnswerDto> Answers { get; set; } 
    }
}
