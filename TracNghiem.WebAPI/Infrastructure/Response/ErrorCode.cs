using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Infrastructure.Response
{
    public class ErrorCode
    {
        public const string Success = "00000000";
        public const string BadRequest = "00000001";
        public const string ValidateError = "00000010";
        public const string NotFound = "00000011";
        public const string NotEmpty = "00000100";
        public const string ShortLengthData = "00000101";
        public const string OverLengthData = "00000111";
        public const string InvalidFormat = "00001000";
        public const string InvalidVerifyPassword = "00001001";
        public const string InvalidCurrentPassword = "00001011";
        public const string LockoutUser = "00001111";
        public const string ExistUserOrEmail = "00010000";
        public const string ExistStore = "00010001";
        public const string ExcuteDB = "00010011";
        public const string Forbidden = "00010111";
        public const string NullList = "00011111";
        public const string InvalidData = "00100000";
        public const string AnswerLess = "00100001";
        public const string MissingRightAnswer = "00100011";
        public const string FalseTypeQuestion = "00100111";
        public const string QuestionNotExist = "00101111";
        public const string ExamNotExist = "00111111";
        public const string GreaterThanZero = "01000000";
        public const string DataExist = "00101111";
        public const string SameData = "00111111";
        public const string NothingChange = "01000000";
        public const string TestUserDone = "01000001";
        public const string UserDoingTest = "01000010";
        public const string NullFileUpload = "01000011";
        public const string InvalidQuestionCount = "01000100";
        public const string BlockAdmin = "01000101";
        public const string WrongPasss = "01000111";
        public const string BlockUser = "01001000";
        public const string NotMapQuestionCount = "01001001";
    }
}
