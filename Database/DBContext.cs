using Microsoft.EntityFrameworkCore;

namespace Aes.Database
{
    public class DBContext : DbContext
    {
        public static string ConnectionString = "Data Source=databse.db";

        public DbSet<Table.Transaction.Category.Model> TransactionCategory { get; set; }
        public DbSet<Table.Transaction.Recurring.Model> TransactionRecurring { get; set; }
        public DbSet<Table.Transaction.Transaction.Model> TransactionTransaction { get; set; }
        public DBContext() {
            Database.EnsureCreated();
        }
        public DBContext(DbContextOptions<DBContext> options)
            : base(options) {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(ConnectionString);
            }
        }
    }
}
