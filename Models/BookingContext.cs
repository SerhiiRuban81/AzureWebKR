using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AzureWebKR.Models
{
    public class BookingContext : DbContext
    {
        public DbSet<Appartment> Appartments { get; set; }

        public BookingContext(DbContextOptions<BookingContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
