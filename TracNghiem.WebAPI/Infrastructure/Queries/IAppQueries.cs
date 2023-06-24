using TracNghiem.Domain.Entities;
using TracNghiem.WebAPI.Infrastructure.ModelQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Infrastructure.Queries
{
    public interface IAppQueries
    {
        #region category
        Task<List<CategoryQueryModel>> GetAllCategory();
        #endregion
        #region Question
        Task<QuestionDetailQueryModel> GetQuestionDetailById(int questionId);
        Task<List<AnswerQueryModel>> GetAnswerQuestion(int questionId);
        Task<List<CategoryQueryModel>> GetCategoryQuestion(int questionId);
        Task<List<QuestionQueryModel>> GetListQuestionInExam(int examId, UrlQuery urlQuery);
        Task<int> CountGetListQuestionInExam(int examId, UrlQuery urlQuery);
        #endregion
        #region Test
        Task<List<TestUserQueryModel>> GetListTestInfo(UrlQuery urlQuery);
        Task<List<TestUserQueryModel>> GetListTestForStudent(DateTime dateTime, UrlQuery urlQuery);
        Task<int> CountListTestStudent(DateTime dateTime, UrlQuery urlQuery);
        Task<int> CountGetListTestInfo(UrlQuery urlQuery);
        Task<List<TestQuestionQueryModel>> GetQuestionTest(int userId, int testId, UrlQuery urlQuery);
        Task<TestQuestionQueryModel> GetOneQuestionTest(int userId, int testId, int page);
        Task<int> CountGetOneQuestionTest(int userId, int testId);
        Task<List<TestQuestionResult>> GetUserAnswer(int testQuestionId);
        Task<TestUserQueryModel> GetTestInfo(int testId);
        Task<TestUserStatusQueryModel> GetUserTestStatus(int testId, int userId);
        Task<TestResultQueryModel> GetTestUserInfo(int testId);
        Task<List<UserResultQueryModel>> GetListTestResult(int testId, int classId, UrlQuery urlQuery);
        Task<int> CountUserTestResult(int testId, int classId, UrlQuery urlQuery);
        Task<UserResultQueryModel> GetUserTestResut(int testId, int userId);
        #endregion
        #region Exam 
        Task<List<CategoryQueryModel>> GetCategoryExam(int examId);
        Task<ExamDetailQueryModel> GetExamDetailById(int examId);
        Task<List<ExamQueryModel>> GetListExam(string username, UrlQuery urlQuery);
        Task<int> countListExamUser(string username, UrlQuery urlQuery);
        Task<List<ExamQueryModel>> GetAllExam(UrlQuery urlQuery);
        Task<int> CountExam(UrlQuery urlQuery);

        Task<LevelQueryModel> GetNameLevelQuestion(int questionId);
        Task<List<ExamQueryModel>> GetListExamByCategoryId(int cateId, UrlQuery urlQuery);
        #endregion
        #region user
        Task<List<UserQueryModel>> GetAllUser(UrlQuery urlQuery, List<int> classIds);
        Task<int> CountGetllUser(UrlQuery urlQuery, List<int> classIds);
        Task<List<UserRoleQueryModel>> GetRoleUser(int userId);
        Task<UserDetailQueryModel> GetUserDetail(int userId);
        Task<List<ListTestCreate>> GetListTestCreateByUser(string userName);
        Task<List<ListTestUserDid>> GetListTestDidByUser(int userId);
        Task<List<ListTestCreate>> GetListTestOfExam(int examId);
        Task<List<User>> GetUserClass(int classId);
        #endregion
    }
}
