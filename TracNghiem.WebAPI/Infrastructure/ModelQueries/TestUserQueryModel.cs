using TracNghiem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Infrastructure.ModelQueries
{
    public class TestUserQueryModel
    {
        public int TestId { get; set; }
        public string Title { get; set; }
        public string CreateBy { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public int Time { get; set; }
        public int QuestionCount { get; set; }
        public List<TestQuestionQueryModel> questions { get; set; }
    }
    public class TestUserStatusQueryModel
    {
        public int TestId { get; set; }
        public string StudentName { get; set; }
        public string Title { get; set; }
        public string CreateBy { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public int Time { get; set; }
        public int TimeRemain { get; set; }
        public int Status { get; set; }
        public int QuestionCount { get; set; }
    }
    public class TestQuestionQueryModel // get questionInfo in a specify test
    {
        public int Id { get; set; }
        public int NumericalOrder { get; set; }
        public int QuestionId { get; set; }
        public string QuestionContent { get; set; }
        public string Explaint { get; set; }
        public int TypeId { get; set; }
        public int RightCount { get; set; }
        public List<TestQuestionResult> UserChoose { get; set; }
        public List<AnswerQueryModel> ListAnswers { get; set; }
        public List<CategoryQueryModel> ListCategories { get; set; }
    }
}
