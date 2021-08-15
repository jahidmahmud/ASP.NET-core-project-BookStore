using JMBookStore.DataAccess.Data;
using JMBookStore.Models;
using JMBookStore.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JMBookStore.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext context;

        public ProductRepository(ApplicationDbContext _context) : base(_context)
        {
            context = _context;
        }

        public void Update(Product product)
        {
            var proObj = context.Product.FirstOrDefault(x => x.Id == product.Id);
            if (proObj != null)
            {
                if (product.ImageUrl != null)
                {
                    proObj.ImageUrl = product.ImageUrl;
                }
                proObj.ISBN = product.ISBN;
                proObj.Price = product.Price;
                proObj.Price50 = product.Price50;
                proObj.ListPrice = product.ListPrice;
                proObj.Price100 = product.Price100;
                proObj.Title = product.Title;
                proObj.Description = product.Description;
                proObj.CategoryId = product.CategoryId;
                proObj.Author = product.Author;
                proObj.CoverTypeId = product.CoverTypeId;
            }
        }
    }
}
