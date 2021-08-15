using JMBookStore.DataAccess.Data;
using JMBookStore.Models;
using JMBookStore.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JMBookStore.Repositories
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext context;

        public CompanyRepository(ApplicationDbContext _context) : base(_context)
        {
            context = _context;
        }

        public void Update(Company company)
        {
            context.Update(company);
        }
    }
}
