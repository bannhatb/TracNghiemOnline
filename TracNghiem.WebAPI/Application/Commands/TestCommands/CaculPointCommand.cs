using MediatR;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Commands.TestCommands
{
    public class CaculPointCommand : IRequest<Response<ResponseDefault>>
    {
        public int TestId { get; set; }
    }
}
