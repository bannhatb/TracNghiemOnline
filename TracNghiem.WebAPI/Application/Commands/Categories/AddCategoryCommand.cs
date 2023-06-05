using MediatR;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Commands.Categories
{
    public class AddCategoryCommand : IRequest<Response<ResponseDefault>>
    {
        public string CategoryName { get; set; }
    }
}
