using TracNghiem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracNghiem.Domain.Repositories
{
    public interface ILevelRepository : IRepository<Level>
    {
        IQueryable<Level> Levels { get; }
        void Add(Level level);
        void Edit(Level level);
        void Delete(Level level);
        Task<List<Level>> GetLevels();
    }
}
