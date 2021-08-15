using JMBookStore.DataAccess.Data;
using JMBookStore.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JMBookStore.Repositories
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext context;

        public UnitOfWork(ApplicationDbContext _context)
        {
            context = _context;
            Category = new CategoryRepository(context);
            SP_Call = new SP_Call(context);
            Cover = new CoverTypeRepository(context);
            Company = new CompanyRepository(context);
            Product = new ProductRepository(context);
            ApplicationUser = new ApplicationUserRepository(context);

        }
        public ICategoryRepository Category { get; private set; }
        public ICoverTypeRepository Cover { get; private set; }
        public IProductRepository Product { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public ISP_Call SP_Call { get; private set; }

        public void Dispose()
        {
            context.Dispose();
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
