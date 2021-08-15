using JMBookStore.DataAccess.Data;
using JMBookStore.Models;
using JMBookStore.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JMBookStore.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext context;

        public CategoryRepository(ApplicationDbContext _context) : base(_context)
        {
            context = _context;
        }

        public void Update(Category category)
        {
            var catObj = context.Categories.FirstOrDefault(x => x.Id == category.Id);
            if (catObj != null)
            {
                catObj.Name = category.Name;
            }
        }
    }
}
