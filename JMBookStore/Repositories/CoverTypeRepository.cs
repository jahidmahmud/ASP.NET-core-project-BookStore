using JMBookStore.DataAccess.Data;
using JMBookStore.Models;
using JMBookStore.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JMBookStore.Repositories
{
    public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        private readonly ApplicationDbContext context;

        public CoverTypeRepository(ApplicationDbContext _context) : base(_context)
        {
            context = _context;
        }

        public void Update(CoverType cover)
        {
            var coverObj = context.CoverType.FirstOrDefault(x => x.Id == cover.Id);
            if (coverObj != null)
            {
                coverObj.Name = cover.Name;
            }
        }
    }
}
