

namespace TracNghiem.Domain.Entities
{
    public class Question : Entity
    {
        public string QuestionContent { get; set; }
        public string Explaint { get; set; }
        public int TypeId { get; set; }
        public int RightCount { get; set; }
        public int? LevelId { get; set; }

    }
}
