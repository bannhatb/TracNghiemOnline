using TracNghiem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TracNghiem.Domain.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly TracnghiemContext _context;
        public QuestionRepository(TracnghiemContext context)
        {
            _context = context;
        }
        public IQueryable<Question> Questions => _context.Questions;
        public IQueryable<QuestionCategory> QuestionCategories => _context.QuestionCategories;
        public IQueryable<QuestionExam> QuestionExams => _context.QuestionExams;
        public IQueryable<Answer> Answers => _context.Answers;

        public IUnitOfWork UnitOfWork => _context;

        //Question
        public void Add(Question question)
        {
            _context.Questions.Add(question);
        }

        public void Delete(Question question)
        {
            _context.Entry(question).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Edit(Question question)
        {
            _context.Entry(question).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        //QuestionExam
        public void Add(QuestionExam questionExam)
        {
            _context.QuestionExams.Add(questionExam);
        }

        public void Delete(QuestionExam questionExam)
        {
            _context.Entry(questionExam).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Edit(QuestionExam questionExam)
        {
            _context.Entry(questionExam).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        //QuestionCategory
        public void Add(QuestionCategory questionCategory)
        {
            _context.QuestionCategories.Add(questionCategory);
        }

        public void Delete(QuestionCategory questionCategory)
        {
            _context.Entry(questionCategory).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Edit(QuestionCategory questionCategory)
        {
            _context.Entry(questionCategory).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        //Answer
        public void Add(Answer answer)
        {
            _context.Answers.Add(answer);
        }

        public void Delete(Answer answer)
        {
            _context.Entry(answer).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Edit(Answer answer)
        {
            _context.Entry(answer).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }


        /// <summary>
        /// get IdQuestion by cateId and levelId
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="questionCount"></param>
        /// <param name="levelId"></param>
        /// <returns></returns>
        public List<int> GetListIdQuestionByCateIdAndLevelId(int categoryId, int questionCount, int levelId)
        {

            List<int> listQuestionId = _context.Categories
                                    .Where(x => x.Id == categoryId)
                                    .Join(_context.QuestionCategories,
                                    category => category.Id,
                                    questionCategory => questionCategory.CategoryId,
                                    (category, questionCategory) => new { category, questionCategory })
                                    .Join(_context.Questions,
                                    categoryQuestionCategory => categoryQuestionCategory.questionCategory.QuestionId,
                                    question => question.Id,
                                    (categoryQuestionCategory, question) => new { categoryQuestionCategory, question })
                                    .Join(_context.Levels,
                                    questionCategoryQuestionCategory => questionCategoryQuestionCategory.question.LevelId,
                                    level => level.Id,
                                    (questionCategoryQuestionCategory, level) => new { questionCategoryQuestionCategory, level })
                                    .Where(x => x.questionCategoryQuestionCategory.question.LevelId == levelId)
                                    .Select(x => x.questionCategoryQuestionCategory.question.Id)
                                    .OrderBy(x => Guid.NewGuid()).Take(questionCount)
                                    .ToList();

            return listQuestionId;
        }

        public List<int> GetListIdQuestionByCateId(int categoryId, int questionCount)
        {

            List<int> listQuestionId = _context.Categories
                                    .Where(x => x.Id == categoryId)
                                    .Join(_context.QuestionCategories,
                                    category => category.Id,
                                    questionCategory => questionCategory.CategoryId,
                                    (category, questionCategory) => new { category, questionCategory })
                                    .Join(_context.Questions,
                                    categoryQuestionCategory => categoryQuestionCategory.questionCategory.QuestionId,
                                    question => question.Id,
                                    (categoryQuestionCategory, question) => new { categoryQuestionCategory, question })
                                    .Select(x => x.question.Id)
                                    .OrderBy(x => Guid.NewGuid()).Take(questionCount)
                                    .ToList();

            return listQuestionId;
        }

        public List<Question> GetListQuestionOfUser(string userName)
        {
            return _context.Questions.Where(x => x.UserCreate == userName).Select(x => x).ToList();
        }
    }
}
