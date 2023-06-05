
namespace TracNghiem.Domain.Entities
{
    public class Answer : Entity
    {
        public string AnswerContent { get; set; }
        public int QuestionId { get; set; }
        public bool RightAnswer { get; set; }
    }
}
