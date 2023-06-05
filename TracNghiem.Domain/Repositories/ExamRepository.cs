using TracNghiem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracNghiem.Domain.Repositories
{
    public class ExamRepository : IExamRepository
    {
        private readonly TracnghiemContext _context;
        public ExamRepository(TracnghiemContext context)
        {
            _context = context;
        }
        public IQueryable<Exam> Exams => _context.Exams;
        public IQueryable<ExamCategory> ExamCategories => _context.ExamCategories;

        public IUnitOfWork UnitOfWork => _context;

        //Exam
        public void Add(Exam exam)
        {
            _context.Exams.Add(exam);
        }

        public void Delete(Exam exam)
        {
            _context.Entry(exam).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Edit(Exam exam)
        {
            _context.Entry(exam).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        //ExamCategory
        public void Add(ExamCategory examCategory)
        {
            _context.ExamCategories.Add(examCategory);
        }

        public void Delete(ExamCategory examCategory)
        {
            _context.Entry(examCategory).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Edit(ExamCategory examCategory)
        {
            _context.Entry(examCategory).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
    }
}
