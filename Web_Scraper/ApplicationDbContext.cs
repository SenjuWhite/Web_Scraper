using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Web_Scraper.Models;

namespace Web_Scraper
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
             
    }
        public DbSet<Phone> PhoneInfo { get; set; }
        public DbSet<Store> StoreInfo { get; set; }
    }
}

