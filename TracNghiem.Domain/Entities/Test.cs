
namespace TracNghiem.Domain.Entities
{
    public class Test : Entity
    {
        public int ExamId { get; set; }
        public string CreateBy { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public int QuestionCount { get; set; }
        public int Time { get; set; }
        public bool HideAnswer { get; set; }
        public bool SuffleQuestion { get; set; }
        public string Password { get; set; }
    }
}
