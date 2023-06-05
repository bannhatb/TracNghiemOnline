

namespace TracNghiem.Domain.Entities
{
    public class TestQuestion : Entity
    {
        public int QuestionId { get; set; }
        public int UserId { get; set; } // who is doing
        public int TestId { get; set; }
        public int NumericalOrder { get; set; }
    }
}
