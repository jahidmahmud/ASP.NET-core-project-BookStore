using JMBookStore.DataAccess.Data;
using JMBookStore.Models;
using JMBookStore.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JMBookStore.Repositories
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext context;

        public ApplicationUserRepository(ApplicationDbContext _context) : base(_context)
        {
            context = _context;
        }
    }
}
