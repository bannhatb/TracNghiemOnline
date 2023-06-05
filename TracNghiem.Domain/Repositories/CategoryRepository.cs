using TracNghiem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracNghiem.Domain.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly TracnghiemContext _context;
        public CategoryRepository(TracnghiemContext context)
        {
            _context = context;
        }
        public IQueryable<Category> Categories => _context.Categories;

        public IUnitOfWork UnitOfWork => _context;

        public void Add(Category category)
        {
            _context.Categories.Add(category);
        }

        public void Delete(Category category)
        {
            _context.Entry(category).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Edit(Category category)
        {
            _context.Entry(category).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
    }
}
