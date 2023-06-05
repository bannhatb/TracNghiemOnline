using TracNghiem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracNghiem.Domain.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TracnghiemContext _context;
        public UserRepository(TracnghiemContext context)
        {
            _context = context;
        }

        public IQueryable<UserRole> UserRoles => _context.UserRoles;
        public IQueryable<User> Users => _context.Users;

        public IUnitOfWork UnitOfWork => _context;

        public IQueryable<Class> Classes => _context.Classes;

        //User
        public void Add(User user)
        {
            _context.Users.Add(user);
        }

        public void Delete(User user)
        {
            _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Edit(User user)
        {
            _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        //UserRole
        public void Add(UserRole userRole)
        {
            _context.UserRoles.Add(userRole);
        }

        public void Delete(UserRole userRole)
        {
            _context.Entry(userRole).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Edit(UserRole userRole)
        {
            _context.Entry(userRole).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void Add(Class cl)
        {
            _context.Classes.Add(cl);
        }

        public void Edit(Class cl)
        {
            _context.Entry(cl).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void Delete(Class cl)
        {
            _context.Entry(cl).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }
    }
}
