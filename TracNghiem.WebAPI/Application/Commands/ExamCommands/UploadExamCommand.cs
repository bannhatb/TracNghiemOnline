using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using TracNghiem.WebAPI.Infrastructure.Response;

namespace TracNghiem.WebAPI.Application.Commands.ExamCommands
{
    public class UploadExam : IRequest<Response<ResponseDefault>>
    {
        public int Id { get; set; }
        public List<int> N1 { get; set; }
        public List<IFormFile> Att { get; set;}
    }
}