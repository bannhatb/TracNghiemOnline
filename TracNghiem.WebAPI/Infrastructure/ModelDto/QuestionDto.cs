using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Infrastructure.ModelDto
{
    public class QuestionDto
    {
        public int QuestionId { get; set; }
        public string QuestionContent { get; set; }
        public string Explaint { get; set; }
        public int TypeId { get; set; }
        public int LevelId { get; set; }
        //public int RightCount { get; set; }
        public HashSet<int> Categories { get; set; }
    }
    public class AnswerDto
    {
        public int AnswerId { get; set; }
        public string AnswerContent { get; set; }
        public int QuestionId { get; set; }
        public bool RightAnswer { get; set; }
    }
}
