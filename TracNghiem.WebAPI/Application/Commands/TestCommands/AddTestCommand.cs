using MediatR;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Commands.TestCommands
{
    public class AddTestCommand : IRequest<Response<ResponseDefault>>
    {
        public int ExamId { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public int QuestionCount { get; set; }
        public int Time { get; set; }
        public bool HideAnswer { get; set; }
        public bool ShuffleQuestion { get; set; }
        public string Password { get; set; }
    }
}
