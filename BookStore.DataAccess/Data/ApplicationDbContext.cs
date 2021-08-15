using JMBookStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JMBookStore.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Category> Categories { set; get; }
        public DbSet<CoverType> CoverType { set; get; }
        public DbSet<Product> Product { set; get; }
        public DbSet<ApplicationUser> ApplicationUser { set; get; }
        public DbSet<Company> Company { set; get; }
    }
}
