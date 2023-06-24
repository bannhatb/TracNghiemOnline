using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TracNghiem.Domain.Entities;
using TracNghiem.WebAPI.Infrastructure.ModelQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracNghiem.WebAPI.Infrastructure.Queries
{
    public class AppQueries : IAppQueries
    {
        public IConfiguration Configuration { get; }
        public string _connectionString { get; }
        public AppQueries(string connectionString)
        {
            _connectionString = connectionString;
        }
        #region Category
        public async Task<List<CategoryQueryModel>> GetAllCategory()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" select * from Categories ");
                var result = await connection.QueryAsync<CategoryQueryModel>(sb.ToString());
                return result.ToList();
            }
        }
        #endregion

        #region Question
        public async Task<QuestionDetailQueryModel> GetQuestionDetailById(int questionId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" SELECT q.Id, q.QuestionContent, q.Explaint, q.RightCount, q.TypeId, q.LevelId   
FROM Questions as q where q.id = @questionId ");
                var result = await connection
                    .QueryFirstAsync<QuestionDetailQueryModel>(sb.ToString(), new { questionId = questionId });
                return result;
            }
        }

        public async Task<List<AnswerQueryModel>> GetAnswerQuestion(int questionId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" SELECT a.Id, a.AnswerContent, a.QuestionId, a.RightAnswer 
FROM Answers as a WHERE a.QuestionId = @questionId ");
                var result = await connection
                    .QueryAsync<AnswerQueryModel>(sb.ToString(), new { questionId = questionId });
                return result.ToList();
            }
        }

        public async Task<List<CategoryQueryModel>> GetCategoryQuestion(int questionId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" SELECT c.Id, C.CategoryName 
FROM Categories as c INNER JOIN QuestionCategories as qc on c.Id = qc.CategoryId 
INNER JOIN Questions as q on qc.QuestionId = q.Id where q.Id = @questionId ");
                var result = await connection
                    .QueryAsync<CategoryQueryModel>(sb.ToString(), new { questionId = questionId });
                return result.ToList();
            }
        }

        public async Task<List<QuestionQueryModel>> GetListQuestionInExam(int examId, UrlQuery urlQuery)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" SELECT q.Id, q.QuestionContent, q.Explaint, q.TypeId, q.RightCount, q.LevelId  
FROM Questions as q INNER JOIN QuestionExams as qe on q.Id = qe.QuestionId 
INNER JOIN Exams as e on qe.ExamId = e.Id 
where e.Id = @examId ");
                if (urlQuery.Keyword != null)
                {
                    sb.Append("AND ( q.QuestionContent COLLATE Latin1_General_CI_AI like  N'%'+@keyWord+'%' )");
                }
                sb.Append(@"  
ORDER BY q.Id ASC 
OFFSET @pageSize*(@page-1) ROWS 
FETCH NEXT @pageSize ROWS ONLY 
");
                var result = await connection
                    .QueryAsync<QuestionQueryModel>(sb.ToString(), new
                    {
                        examId = examId,
                        page = urlQuery.Page,
                        pageSize = urlQuery.PageSize,
                        keyWord = urlQuery.Keyword
                    });
                return result.ToList();
            }
        }
        #endregion

        #region Test
        public async Task<List<TestQuestionQueryModel>> GetQuestionTest(int userId, int testId, UrlQuery urlQuery)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" SELECT tq.Id, tq.NumericalOrder, q.Id as QuestionId, 
q.QuestionContent, q.Explaint, q.TypeId, q.RightCount 
from [dbo].[TestQuestions] as tq INNer join [dbo].[Questions] as q on tq.QuestionId = q.Id 
WHERE tq.UserId = @userId and tq.TestId = @testId ");
                sb.Append(@"  
ORDER BY tq.NumericalOrder ASC 
OFFSET @pageSize*(@page-1) ROWS 
FETCH NEXT @pageSize ROWS ONLY 
");
                var result = await connection
                    .QueryAsync<TestQuestionQueryModel>(sb.ToString(), new
                    {
                        userId = userId,
                        testId = testId,
                        page = urlQuery.Page,
                        pageSize = urlQuery.PageSize
                    });
                return result.ToList();
            }
        }


        public async Task<TestUserQueryModel> GetTestInfo(int testId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" SELECT t.Id as TestId, e.Title, t.CreateBy, t.StartAt, t.EndAt, t.Time, t.QuestionCount 
FROM [dbo].[Tests] as t INNER JOIN Exams as e on t.ExamId = e.Id 
WHERE t.Id = @testId ");
                var result = await connection
                    .QueryFirstAsync<TestUserQueryModel>(sb.ToString(), new
                    {
                        testId = testId
                    });
                return result;
            }
        }

        public async Task<List<TestQuestionResult>> GetUserAnswer(int testQuestionId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" SELECT tqr.Id, tqr.TestQuestionId, tqr.Choose 
FROM [dbo].[TestQuestionResults] as tqr join [dbo].[TestQuestions] as tq on tqr.TestQuestionId = tq.Id 
WHERE tqr.TestQuestionId = @testQuestionId ");
                var result = await connection
                    .QueryAsync<TestQuestionResult>(sb.ToString(), new
                    {
                        testQuestionId = testQuestionId
                    });
                return result.ToList();
            }
        }
        public async Task<TestResultQueryModel> GetTestUserInfo(int testId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" SELECT t.Id as TestId, t.CreateBy, t.StartAt, t.EndAt, t.Time, t.QuestionCount 
FROM [dbo].[Tests] as t 
WHERE t.Id = @testId ");
                var result = await connection
                    .QueryFirstAsync<TestResultQueryModel>(sb.ToString(), new
                    {
                        testId = testId
                    });
                return result;
            }
        }

        public async Task<List<UserResultQueryModel>> GetListTestResult(int testId, int classId, UrlQuery urlQuery)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" select u.Id as UserId , u.UserName, tu.Id as TestUserId, tu.Point, c.Id as ClassId, c.Name as ClassName 
From TestUsers as tu join Users as u on tu.UserId = u.Id  join Classes as c on u.ClassId = c.Id 
where tu.TestId = @testId  ");
                if (classId != -1)
                {
                    sb.Append(@" AND u.ClassId = @classId ");
                }
                if (!string.IsNullOrWhiteSpace(urlQuery.Keyword))
                {
                    sb.Append(@" AND u.UserName COLLATE Latin1_General_CI_AI LIKE N'%'+@Keyword+'%' ");
                }
                sb.Append(@"  
ORDER BY u.Id ASC 
OFFSET @pageSize*(@page-1) ROWS 
FETCH NEXT @pageSize ROWS ONLY 
");
                var result = await connection
                    .QueryAsync<UserResultQueryModel>(sb.ToString(), new
                    {
                        testId = testId,
                        classId = classId,
                        Keyword = urlQuery.Keyword,
                        page = urlQuery.Page,
                        pageSize = urlQuery.PageSize

                    });
                return result.ToList();
            }
        }
        #endregion

        #region Exam 
        public async Task<ExamDetailQueryModel> GetExamDetailById(int examId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" SELECT e.Id, e.Title, e.Time, e.CreateBy, e.QuestionCount,  e.IsPublic
FROM Exams as e where e.id = @examId ");
                var result = await connection
                    .QueryFirstAsync<ExamDetailQueryModel>(sb.ToString(), new { examid = examId });
                return result;
            }
        }

        public async Task<List<CategoryQueryModel>> GetCategoryExam(int examId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" SELECT c.Id, C.CategoryName 
FROM Categories as c INNER JOIN ExamCategories as ec on c.Id = ec.CategoryId 
INNER JOIN Exams as e on ec.ExamId = e.Id where e.Id = @examId ");
                var result = await connection
                    .QueryAsync<CategoryQueryModel>(sb.ToString(), new { examid = examId });
                return result.ToList();
            }
        }

        public async Task<LevelQueryModel> GetNameLevelQuestion(int questionId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" SELECT l.LevelName 
FROM Levels as l INNER JOIN Questions as q on q.LevelId = l.Id   
where q.Id = @questionId ");
                var result = await connection
                    .QueryFirstAsync<LevelQueryModel>(sb.ToString(), new
                    {
                        questionid = questionId
                    });
                return result;
            }
        }

        public async Task<List<ExamQueryModel>> GetListExam(string username, UrlQuery urlQuery)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" SELECT e.Id, e.Title, e.Time, e.CreateBy, e.QuestionCount, e.IsPublic
FROM Exams as e where e.CreateBy = @username ");
                if (!string.IsNullOrWhiteSpace(urlQuery.Keyword))
                {
                    sb.Append(@" AND e.Title COLLATE Latin1_General_CI_AI LIKE N'%'+@Keyword+'%' ");
                }
                sb.Append(@"  
ORDER BY e.Id ASC 
OFFSET @pageSize*(@page-1) ROWS 
FETCH NEXT @pageSize ROWS ONLY 
");

                var result = await connection
                    .QueryAsync<ExamQueryModel>(sb.ToString(), new
                    {
                        username = username,
                        Keyword = urlQuery.Keyword,
                        page = urlQuery.Page,
                        pageSize = urlQuery.PageSize
                    });
                return result.ToList();
            }
        }

        public async Task<List<TestUserQueryModel>> GetListTestInfo(UrlQuery urlQuery)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" SELECT t.Id as TestId, e.Title, t.CreateBy, t.StartAt, t.EndAt, t.Time, t.QuestionCount 
FROM [dbo].[Tests] as t INNER JOIN Exams as e on t.ExamId = e.Id 
 ");
                if (urlQuery.Keyword != null)
                {
                    sb.Append("WHERE e.Title COLLATE Latin1_General_CI_AI like  N'%'+@keyWord+'%' ");
                }
                sb.Append(@"  
ORDER BY t.Id ASC 
OFFSET @pageSize*(@page-1) ROWS 
FETCH NEXT @pageSize ROWS ONLY 
");
                var result = await connection
                    .QueryAsync<TestUserQueryModel>(sb.ToString(), new
                    {
                        page = urlQuery.Page,
                        pageSize = urlQuery.PageSize,
                        keyWord = urlQuery.Keyword
                    });
                return result.ToList();
            }
        }

        public async Task<int> CountGetListTestInfo(UrlQuery urlQuery)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" SELECT Count(t.Id) 
FROM [dbo].[Tests] as t INNER JOIN Exams as e on t.ExamId = e.Id 
 ");
                if (urlQuery.Keyword != null)
                {
                    sb.Append("WHERE e.Title COLLATE Latin1_General_CI_AI like N'%'+@keyWord+'%' ");
                }
                var result = await connection
                        .ExecuteScalarAsync<int>(sb.ToString(), new
                        {
                            keyWord = urlQuery.Keyword
                        });
                return result;
            }
        }

        public async Task<TestUserStatusQueryModel> GetUserTestStatus(int testId, int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" 
SELECT t.Id as TestId, u.UserName as StudentName, e.Title, t.CreateBy, t.StartAt, t.EndAt, 
t.Time, tu.TimeRemain, tu.Status, t.QuestionCount 
FROM [dbo].[Tests] as t INNER JOIN Exams as e on t.ExamId = e.Id INNER JOIN [dbo].[TestUsers] as tu on 
t.Id = tu.TestId inner join Users as u on tu.UserId = u.Id 
WHERE t.Id = @testId AND tu.UserId = @userId 
");
                var result = await connection
                        .QueryFirstAsync<TestUserStatusQueryModel>(sb.ToString(), new
                        {
                            testId,
                            userId
                        });
                return result;
            }
        }

        public async Task<TestQuestionQueryModel> GetOneQuestionTest(int userId, int testId, int page)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" SELECT tq.Id, tq.NumericalOrder, q.Id as QuestionId, 
q.QuestionContent, q.Explaint, q.TypeId, q.RightCount 
from [dbo].[TestQuestions] as tq INNer join [dbo].[Questions] as q on tq.QuestionId = q.Id 
WHERE tq.UserId = @userId and tq.TestId = @testId ");
                sb.Append(@"  
ORDER BY tq.NumericalOrder ASC 
OFFSET 1*(@page-1) ROWS 
FETCH NEXT 1 ROWS ONLY 
");
                var result = await connection
                    .QueryFirstAsync<TestQuestionQueryModel>(sb.ToString(), new
                    {
                        userId = userId,
                        testId = testId,
                        page = page,
                    });
                return result;
            }
        }

        public async Task<UserResultQueryModel> GetUserTestResut(int testId, int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" select u.Id as UserId , u.UserName, tu.Id as TestUserId, tu.Point, c.Id as ClassId, c.Name as ClassName 
From TestUsers as tu join Users as u on tu.UserId = u.Id  join Classes as c on u.ClassId = c.Id 
where tu.TestId = @testId AND tu.UserId = @userId ");
                var result = await connection
                    .QueryFirstAsync<UserResultQueryModel>(sb.ToString(), new
                    {
                        testId = testId,
                        userId = userId

                    });
                return result;
            }
        }

        public async Task<List<ExamQueryModel>> GetListExamByCategoryId(int cateId, UrlQuery urlQuery)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" SELECT e.Id, e.Title, e.Time, e.CreateBy, e.QuestionCount, e.IsPublic 
FROM Exams as e INNER JOIN ExamCategories as ec on e.Id = ec.ExamId 
INNER JOIN Categories as c on ec.CategoryId = c.Id 
WHERE c.Id = @cateId ");
                if (!string.IsNullOrWhiteSpace(urlQuery.Keyword))
                {
                    sb.Append(@" AND e.Title COLLATE Latin1_General_CI_AI LIKE N'%'+@Keyword+'%' ");
                }
                sb.Append(@"  
ORDER BY e.Id ASC 
OFFSET @pageSize*(@page-1) ROWS 
FETCH NEXT @pageSize ROWS ONLY 
");

                var result = await connection
                    .QueryAsync<ExamQueryModel>(sb.ToString(), new
                    {
                        cateId = cateId,
                        Keyword = urlQuery.Keyword,
                        page = urlQuery.Page,
                        pageSize = urlQuery.PageSize
                    });
                return result.ToList();
            }
        }


        #endregion
        #region Users
        public async Task<List<UserQueryModel>> GetAllUser(UrlQuery urlQuery, List<int> classIds)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" select u.Id as UserId , u.UserName, u.Email, u.Avatar, u.DateOfBirth, u.Gender, 
u.FirstName, u.LastName, u.IsBlock, 
c.Id as ClassId, c.Name as ClassName 
FROM Users as u 
inner join Classes as c on u.ClassId = c.Id WHERE 1=1 
 ");
                if (classIds.Count > 0 && classIds.Any(x => x > 0))
                {
                    sb.Append("AND ( ");
                    foreach (int classId in classIds)
                    {
                        if (classId > 0)
                        {
                            sb.Append($" ( u.ClassId = {classId}  ) OR ");
                        }
                    }
                    sb.Append(" 1!=1 ) ");
                }



                if (urlQuery.Keyword != null)
                {
                    sb.Append("AND ( u.UserName COLLATE Latin1_General_CI_AI like  N'%'+@keyWord+'%' )");
                }
                sb.Append(@"  
ORDER BY u.Id ASC 
OFFSET @pageSize*(@page-1) ROWS 
FETCH NEXT @pageSize ROWS ONLY 
");
                var result = await connection
                    .QueryAsync<UserQueryModel>(sb.ToString(), new
                    {
                        page = urlQuery.Page,
                        pageSize = urlQuery.PageSize,
                        keyWord = urlQuery.Keyword
                    });
                return result.ToList();
            }
        }

        public async Task<List<UserRoleQueryModel>> GetRoleUser(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" select ur.RoleId 
From Users as u inner join UserRoles as ur on ur.UserId = u.Id where u.Id = @userId ");
                var result = await connection
                    .QueryAsync<UserRoleQueryModel>(sb.ToString(), new
                    {
                        userId = userId
                    });
                return result.ToList();
            }
        }

        public async Task<List<ExamQueryModel>> GetAllExam(UrlQuery urlQuery)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" SELECT e.Id, e.Title, e.Time, e.CreateBy, e.QuestionCount, e.IsPublic 
FROM Exams as e  ");
                if (urlQuery.Keyword != null)
                {
                    sb.Append("WHERE e.Title COLLATE Latin1_General_CI_AI like  N'%'+@keyWord+'%' ");
                }
                sb.Append(@"  
ORDER BY e.Id ASC 
OFFSET @pageSize*(@page-1) ROWS 
FETCH NEXT @pageSize ROWS ONLY 
");
                var result = await connection
                    .QueryAsync<ExamQueryModel>(sb.ToString(), new
                    {
                        page = urlQuery.Page,
                        pageSize = urlQuery.PageSize,
                        keyWord = urlQuery.Keyword
                    });
                return result.ToList();
            }
        }

        public async Task<int> CountExam(UrlQuery urlQuery)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" SELECT Count(e.Id) 
FROM Exams as e 
 ");
                if (urlQuery.Keyword != null)
                {
                    sb.Append("WHERE e.Title COLLATE Latin1_General_CI_AI like N'%'+@keyWord+'%' ");
                }
                var result = await connection
                        .ExecuteScalarAsync<int>(sb.ToString(), new
                        {
                            keyWord = urlQuery.Keyword
                        });
                return result;
            }
        }

        public async Task<List<TestUserQueryModel>> GetListTestForStudent(DateTime dateTime, UrlQuery urlQuery)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" SELECT t.Id as TestId, e.Title, t.CreateBy, t.StartAt, t.EndAt, t.Time, t.QuestionCount 
FROM [dbo].[Tests] as t INNER JOIN Exams as e on t.ExamId = e.Id 
WHERE t.StartAt < @dateTime AND @dateTime < t.EndAt 
 ");
                if (urlQuery.Keyword != null)
                {
                    sb.Append("AND e.Title COLLATE Latin1_General_CI_AI like  N'%'+@keyWord+'%' ");
                }
                sb.Append(@"  
ORDER BY t.Id ASC 
OFFSET @pageSize*(@page-1) ROWS 
FETCH NEXT @pageSize ROWS ONLY 
");
                var result = await connection
                    .QueryAsync<TestUserQueryModel>(sb.ToString(), new
                    {
                        page = urlQuery.Page,
                        pageSize = urlQuery.PageSize,
                        keyWord = urlQuery.Keyword,
                        dateTime = dateTime
                    });
                return result.ToList();
            }
        }

        public async Task<int> CountListTestStudent(DateTime dateTime, UrlQuery urlQuery)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" SELECT count(t.Id) 
FROM [dbo].[Tests] as t  inner join Exams as e on t.ExamId = e.Id 
WHERE t.StartAt < @dateTime AND @dateTime < t.EndAt 
 ");
                if (urlQuery.Keyword != null)
                {
                    sb.Append("AND e.Title COLLATE Latin1_General_CI_AI like  N'%'+@keyWord+'%' ");
                }
                var result = await connection
                    .ExecuteScalarAsync<int>(sb.ToString(), new
                    {
                        keyWord = urlQuery.Keyword,
                        dateTime = dateTime
                    });
                return result;
            }
        }

        public async Task<int> CountGetllUser(UrlQuery urlQuery, List<int> classIds)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" select count(u.Id) as total 
FROM Users as u 
inner join Classes as c on u.ClassId = c.Id WHERE 1=1 
 ");
                if (classIds.Count > 0 && classIds.Any(x => x > 0))
                {
                    sb.Append("AND ( ");
                    foreach (int classId in classIds)
                    {
                        if (classId > 0)
                        {
                            sb.Append($" ( u.ClassId = {classId}  ) OR ");
                        }
                    }
                    sb.Append(" 1!=1 ) ");
                }

                if (urlQuery.Keyword != null)
                {
                    sb.Append("AND ( u.UserName COLLATE Latin1_General_CI_AI like  N'%'+@keyWord+'%' )");
                }
                var result = await connection
                    .ExecuteScalarAsync<int>(sb.ToString(), new
                    {
                        keyWord = urlQuery.Keyword
                    });
                return result;
            }
        }

        public async Task<UserDetailQueryModel> GetUserDetail(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" select u.Id as UserId , u.UserName, u.Email, u.Avatar, u.DateOfBirth, u.Gender, 
u.FirstName, u.LastName, u.IsBlock, 
c.Id as ClassId, c.Name as ClassName 
FROM Users as u 
inner join Classes as c on u.ClassId = c.Id WHERE u.Id = @userId 
 ");
                var result = await connection
                    .QueryFirstAsync<UserDetailQueryModel>(sb.ToString(), new
                    {
                        userId = userId
                    });
                return result;
            }
        }

        public async Task<List<ListTestCreate>> GetListTestCreateByUser(string userName)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" Select t.ExamId, t.Id as TestId, e.Title, t.StartAt, t.EndAt, t.Time, t.QuestionCount, t.Password, i.CountDo 
FROM Tests as t join Exams as e on t.ExamId = e.Id JOIN  
(Select tu.TestId as TestId, count(tu.TestId) as CountDo 
FROM Tests as t join TestUsers as tu on t.Id = tu.TestId 
GROUP by tu.TestId) as i on i.TestId = t.Id 
WHERE t.CreateBy =  @userName 
 ");
                var result = await connection
                    .QueryAsync<ListTestCreate>(sb.ToString(), new
                    {
                        userName = userName
                    });
                return result.ToList();
            }
        }

        public async Task<List<ListTestUserDid>> GetListTestDidByUser(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" select t.ExamId, t.Id as TestId, e.Title, t.CreateBy, t.StartAt, t.EndAt, tu.Point, t.Time, t.QuestionCount, t.Password 
FROM TestUsers as tu join Tests as t on tu.TestId = t.Id join Exams as e on t.ExamId = e.Id 
WHERE tu.UserId = @userId 
 ");
                var result = await connection
                    .QueryAsync<ListTestUserDid>(sb.ToString(), new
                    {
                        userId = userId
                    });
                return result.ToList();
            }
        }

        public async Task<int> CountUserTestResult(int testId, int classId, UrlQuery urlQuery)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" select count(u.Id) as UserCount 
From TestUsers as tu join Users as u on tu.UserId = u.Id  join Classes as c on u.ClassId = c.Id 
where tu.TestId = @testId  ");
                if (classId != -1)
                {
                    sb.Append(@" AND u.ClassId = @classId ");
                }
                if (!string.IsNullOrWhiteSpace(urlQuery.Keyword))
                {
                    sb.Append(@" AND u.UserName COLLATE Latin1_General_CI_AI LIKE N'%'+@Keyword+'%' ");
                }
                var result = await connection
                    .ExecuteScalarAsync<int>(sb.ToString(), new
                    {
                        testId = testId,
                        classId = classId,
                        Keyword = urlQuery.Keyword

                    });
                return result;
            }
        }

        public async Task<int> CountGetListQuestionInExam(int examId, UrlQuery urlQuery)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" SELECT count(q.Id) as total 
FROM Questions as q INNER JOIN QuestionExams as qe on q.Id = qe.QuestionId 
INNER JOIN Exams as e on qe.ExamId = e.Id 
where e.Id = @examId ");
                if (urlQuery.Keyword != null)
                {
                    sb.Append("AND ( q.QuestionContent COLLATE Latin1_General_CI_AI like  N'%'+@keyWord+'%' )");
                }
                var result = await connection
                    .ExecuteScalarAsync<int>(sb.ToString(), new
                    {
                        examId = examId,
                        keyWord = urlQuery.Keyword
                    });
                return result;
            }
        }

        public async Task<int> countListExamUser(string username, UrlQuery urlQuery)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" SELECT count(e.Id) as total 
FROM Exams as e where e.CreateBy = @username ");
                if (!string.IsNullOrWhiteSpace(urlQuery.Keyword))
                {
                    sb.Append(@" AND e.Title COLLATE Latin1_General_CI_AI LIKE N'%'+@Keyword+'%' ");
                }
                var result = await connection
                    .ExecuteScalarAsync<int>(sb.ToString(), new
                    {
                        username = username,
                        Keyword = urlQuery.Keyword
                    });
                return result;
            }
        }

        public async Task<int> CountGetOneQuestionTest(int userId, int testId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" SELECT count(tq.Id) as total 
from [dbo].[TestQuestions] as tq INNer join [dbo].[Questions] as q on tq.QuestionId = q.Id 
WHERE tq.UserId = @userId and tq.TestId = @testId ");
                var result = await connection
                    .ExecuteScalarAsync<int>(sb.ToString(), new
                    {
                        userId = userId,
                        testId = testId
                    });
                return result;
            }
        }
        public async Task<List<ListTestCreate>> GetListTestOfExam(int examId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" Select t.ExamId, t.Id as TestId, e.Title, t.StartAt, t.EndAt, t.Time, t.QuestionCount, t.Password, i.CountDo 
FROM Tests as t join Exams as e on t.ExamId = e.Id JOIN  
(Select tu.TestId as TestId, count(tu.TestId) as CountDo 
FROM Tests as t join TestUsers as tu on t.Id = tu.TestId 
GROUP by tu.TestId) as i on i.TestId = t.Id 
WHERE t.ExamId =  @examId 
 ");
                var result = await connection
                    .QueryAsync<ListTestCreate>(sb.ToString(), new
                    {
                        examId = examId
                    });
                return result.ToList();
            }
        }
        public async Task<List<User>> GetUserClass(int classId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" Select * 
FROM Users as u JOIN Classes as c on u.ClassId = c.Id
WHERE u.ClassId = @classId
 ");
                var result = await connection
                    .QueryAsync<User>(sb.ToString(), new
                    {
                        classId = classId
                    });
                return result.ToList();
            }
        }

        #endregion
    }
}
