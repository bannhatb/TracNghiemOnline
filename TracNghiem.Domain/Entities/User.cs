
namespace TracNghiem.Domain.Entities
{
    public class User : Entity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int? ClassId { get; set; }
        public string Avatar { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsBlock { get; set; }
    }
}
