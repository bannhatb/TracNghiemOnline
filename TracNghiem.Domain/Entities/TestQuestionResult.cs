
namespace TracNghiem.Domain.Entities
{
    public class TestQuestionResult : Entity // to support Multichoice Question
    {
        public int TestQuestionId { get; set; }
        public int Choose { get; set; } // AnswerId
    }
}
