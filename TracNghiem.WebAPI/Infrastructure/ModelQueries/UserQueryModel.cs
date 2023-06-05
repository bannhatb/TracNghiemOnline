using TracNghiem.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Infrastructure.ModelQueries
{
    public class UserQueryModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int? ClassId { get; set; }
        public string ClassName { get; set; }
        public string Avatar { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsBlock { get; set; }
        public List<UserRoleQueryModel> ListRoles { get; set; }
    }
    public class UserRoleQueryModel
    {
        public int RoleId { get; set; }
        public string RoleName => ((UserRoleEnum)RoleId).ToString();
    }

    public class UserDetailQueryModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int? ClassId { get; set; }
        public string ClassName { get; set; }
        public string Avatar { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsBlock { get; set; }
        public List<UserRoleQueryModel> ListRoles { get; set; }
        public List<ExamQueryModel> ListExams { get; set; }
        public List<ListTestCreate> ListTestCreate { get; set; }
        public List<ListTestUserDid> ListTestDid { get; set; }
    }
    public class ListTestUserDid
    {
        public int ExamId { get; set; }
        public int TestId { get; set; }
        public string Title { get; set; }
        public string CreateBy { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public double Point { get; set; }
        public int Time { get; set; }
        public int QuestionCount { get; set; }
        public string Password { get; set; }
    }
    public class ListTestCreate
    {
        public int ExamId { get; set; }
        public int TestId { get; set; }
        public string Title { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public int Time { get; set; }
        public int QuestionCount { get; set; }
        public string Password { get; set; }
        public int CountDo { get; set; }
    }
}
