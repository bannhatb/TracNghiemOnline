using MediatR;
using TracNghiem.WebAPI.Infrastructure.ModelDto;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Commands.ExamCommands
{
    public class UpdateExamCommand : IRequest<Response<ResponseDefault>>
    {
        public ExamDto Exam { get; set; }
    }
}
