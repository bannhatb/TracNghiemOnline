
namespace TracNghiem.Domain.Entities
{
    public class Exam : Entity
    {
        public string Title { get; set; }
        public int Time { get; set; }
        public string CreateBy { get; set; }
        public int QuestionCount { get; set; }
        public bool IsPublic { get; set; }
    }
}
