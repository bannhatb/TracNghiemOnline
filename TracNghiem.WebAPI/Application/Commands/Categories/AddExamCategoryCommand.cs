using MediatR;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Commands.Categories
{
    public class AddExamCategoryCommand : IRequest<Response<ResponseDefault>>
    {
        public int ExamId { get; set; }
        public HashSet<int> Categories { get; set; }
    }
}
