using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Infrastructure.ModelQueries
{
    public class TestResultQueryModel // get list user test result
    {
        public int TestId { get; set; }
        public string CreateBy { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public int Time { get; set; }
        public int QuestionCount { get; set; }
        public List<UserResultQueryModel> ListResult { get; set; }
    }
    public class UserResultQueryModel // just get brief result of user (listed)
    {
        public int UserId { get; set; }
        public string UserName { set; get; }
        public int TestUserId { get; set; }
        public double Point { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
    }
}
