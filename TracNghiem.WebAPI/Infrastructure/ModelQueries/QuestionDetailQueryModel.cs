using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Infrastructure.ModelQueries
{
    public class QuestionDetailQueryModel // for get one question  using quesionID
    {
        public int Id { get; set; }
        public string QuestionContent { get; set; }
        public string Explaint { get; set; }
        public int TypeId { get; set; }
        public int RightCount { get; set; }
        public int? LevelId { get; set; }

        public LevelQueryModel Level { get; set; }
        public List<AnswerQueryModel> ListAnswers { get; set; }
        //public List<CategoryQueryModel> ListCategories { get; set; } 
    }
    public class LevelQueryModel
    {
        public string LevelName { get; set; }
    }
    public class AnswerQueryModel
    {
        public int Id { get; set; }
        public string AnswerContent { get; set; }
        public int QuestionId { get; set; }
        public bool RightAnswer { get; set; }
    }
    public class CategoryQueryModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
    }

}
