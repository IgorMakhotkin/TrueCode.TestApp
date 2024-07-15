using Microsoft.EntityFrameworkCore;
using Npgsql;
using UrlLoader.Database.Configurations;
using UrlLoader.Database.Model;

namespace UrlLoader.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }
        public DbSet<FileEntity> Files{ get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new FileEntityConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        public void MigrateDatabase()
        {
            if (!Database.CanConnect())
            {
                throw new NpgsqlException("Can't connect to database");
            }

            try
            {
                Database.Migrate();
            }
            catch (Exception innerException)
            {
                throw new NpgsqlException("Migration failed", innerException);
            }
        }
    }
}
