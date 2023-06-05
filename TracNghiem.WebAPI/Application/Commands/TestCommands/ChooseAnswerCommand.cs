using MediatR;
using TracNghiem.WebAPI.Infrastructure.ModelDto;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Commands.TestCommands
{
    public class ChooseAnswerCommand : IRequest<Response<ResponseDefault>>
    {
        public int TestQuestionId { get; set; }
        public List<int> AnswerIds { get; set; }
    }
}
