using TracNghiem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TracNghiem.Domain.Repositories
{
    public class LevelRepository : ILevelRepository
    {
        private readonly TracnghiemContext _context;
        public LevelRepository(TracnghiemContext context)
        {
            _context = context;
        }
        public IQueryable<Level> Levels => _context.Levels;

        public IUnitOfWork UnitOfWork => _context;

        public void Add(Level level)
        {
            _context.Levels.Add(level);
        }

        public void Delete(Level level)
        {
            _context.Entry(level).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Edit(Level level)
        {
            _context.Entry(level).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public async Task<List<Level>> GetLevels()
        {
            var levels = await _context.Levels.Select(x => x).ToListAsync();
            return levels;
        }
    }
}
