using TracNghiem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracNghiem.Domain.Repositories
{
    public interface ITestRepository : IRepository<Test>
    {
        IQueryable<Test> Tests { get; }
        void Add(Test test);
        void Edit(Test test);
        void Delete(Test test);

        IQueryable<TestUser> TestUsers { get; }
        void Add(TestUser testUser);
        void Edit(TestUser testUser);
        void Delete(TestUser testUser);

        IQueryable<TestQuestionResult> testQuestionResults { get; }
        void Add(TestQuestionResult testQuestionResult);
        void Edit(TestQuestionResult testQuestionResult);
        void Delete(TestQuestionResult testQuestionResult);

        IQueryable<TestQuestion> TestQuestions { get; }
        void Add(TestQuestion testQuestion);
        void Edit(TestQuestion testQuestion);
        void Delete(TestQuestion testQuestion);
    }
}
