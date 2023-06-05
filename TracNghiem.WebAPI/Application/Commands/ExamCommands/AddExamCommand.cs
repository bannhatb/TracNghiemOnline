using MediatR;
using TracNghiem.WebAPI.Infrastructure.ModelDto;
using TracNghiem.WebAPI.Infrastructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Application.Commands.ExamCommands
{
    public class AddExamCommand : IRequest<Response<ResponseDefault>>
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public int Time { get; set; } = 0;
        public bool IsPublic { get; set; }
        public bool RandomQuestion { get; set; }
        public int QuestionCount { get; set; } = 0;
        public int QuestionCountLevel1 { get; set; } = 0;
        public int QuestionCountLevel2 { get; set; } = 0;
        public int QuestionCountLevel3 { get; set; } = 0;
        public int QuestionCountLevel4 { get; set; } = 0;
        public int QuestionCountLevel5 { get; set; } = 0;
        public HashSet<int> Categories { get; set; }
    }
}
