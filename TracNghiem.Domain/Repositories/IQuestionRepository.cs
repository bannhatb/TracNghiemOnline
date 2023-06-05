using TracNghiem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TracNghiem.Domain.Repositories;

namespace TracNghiem.Domain.Repositories
{
    public interface IQuestionRepository : IRepository<Question>
    {
        IQueryable<Question> Questions { get; }
        void Add(Question question);
        void Edit(Question question);
        void Delete(Question question);

        IQueryable<QuestionExam> QuestionExams { get; }
        void Add(QuestionExam questionExam);
        void Edit(QuestionExam questionExam);
        void Delete(QuestionExam questionExam);

        IQueryable<QuestionCategory> QuestionCategories { get; }
        void Add(QuestionCategory questionCategory);
        void Edit(QuestionCategory questionCategory);
        void Delete(QuestionCategory questionCategory);

        IQueryable<Answer> Answers { get; }
        void Add(Answer answer);
        void Edit(Answer answer);
        void Delete(Answer answer);

        List<int> GetListIdQuestionByCategoryId(int categoryId, int questionCount, int levelId);
    }
}
