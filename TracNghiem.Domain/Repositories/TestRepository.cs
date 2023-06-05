using TracNghiem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracNghiem.Domain.Repositories
{
    public class TestRepository : ITestRepository
    {
        private readonly TracnghiemContext _context;
        public TestRepository(TracnghiemContext context)
        {
            _context = context;
        }
        public IQueryable<Test> Tests => _context.Tests;
        public IQueryable<TestUser> TestUsers => _context.TestUsers;
        public IQueryable<TestQuestionResult> testQuestionResults => _context.TestQuestionResults;
        public IQueryable<TestQuestion> TestQuestions => _context.TestQuestions;

        public IUnitOfWork UnitOfWork => _context;

        //Test
        public void Add(Test test)
        {
            _context.Tests.Add(test);
        }

        public void Delete(Test test)
        {
            _context.Entry(test).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Edit(Test test)
        {
            _context.Entry(test).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        //TestQuestion
        public void Add(TestQuestion testQuestion)
        {
            _context.TestQuestions.Add(testQuestion);
        }

        public void Delete(TestQuestion testQuestion)
        {
            _context.Entry(testQuestion).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Edit(TestQuestion testQuestion)
        {
            _context.Entry(testQuestion).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        //TestQuestionResult
        public void Add(TestQuestionResult testQuestionResult)
        {
            _context.TestQuestionResults.Add(testQuestionResult);
        }

        public void Delete(TestQuestionResult testQuestionResult)
        {
            _context.Entry(testQuestionResult).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Edit(TestQuestionResult testQuestionResult)
        {
            _context.Entry(testQuestionResult).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
        
        //TestUSer
        public void Add(TestUser testUser)
        {
            _context.TestUsers.Add(testUser);
        }

        public void Delete(TestUser testUser)
        {
            _context.Entry(testUser).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Edit(TestUser testUser)
        {
            _context.Entry(testUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
    }
}
