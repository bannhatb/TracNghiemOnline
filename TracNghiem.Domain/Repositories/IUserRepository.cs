using TracNghiem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracNghiem.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        IQueryable<User> Users { get; }
        void Add(User user);
        void Edit(User user);
        void Delete(User user);

        IQueryable<UserRole> UserRoles { get; }
        void Add(UserRole userRole);
        void Edit(UserRole userRole);
        void Delete(UserRole userRole);
        IQueryable<Class> Classes { get; }
        void Add(Class cl);
        void Edit(Class cl);
        void Delete(Class cl);
    }
}
