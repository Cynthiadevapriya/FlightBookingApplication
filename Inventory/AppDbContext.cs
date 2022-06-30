using Inventory.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        public DbSet<Airline> Airline { get; set; }
        public DbSet<FlightSchedule> FlightSchedule { get; set; }
        public DbSet<Coupon> Discount { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                        .AddJsonFile("appsettings.json").Build();
            var connectionString = configuration.GetConnectionString("MyConn");
            optionsBuilder.UseSqlServer(connectionString);

        }
        
    }
}
