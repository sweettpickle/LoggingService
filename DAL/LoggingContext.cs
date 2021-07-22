using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoggingService.DAL
{
    public class LoggingContext : DbContext
    {
        public DbSet<Logging> Logging { get; set; }
        public DbSet<LoggingLevel> LoggingLevel { get; set; }
        public DbSet<ActionSource> ActionSource { get; set; }

        public LoggingContext(DbContextOptions options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=logging;Username=postgres;Password=root");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Logging>();

            modelBuilder.Entity<LoggingLevel>().Property(c => c.Name).UseCollation("SQL_Latin1_General_CP1_CS_AS");

            modelBuilder.Entity<ActionSource>().Property(c => c.Name).UseCollation("SQL_Latin1_General_CP1_CS_AS");
        }
    }
}
