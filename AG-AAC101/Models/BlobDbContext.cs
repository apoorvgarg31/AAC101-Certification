using AG_AAC101.Data.Commodity;
using Microsoft.EntityFrameworkCore;


namespace AG_AAC101
{

    public class BlobDbContext : DbContext
    {
        private DbContextOptions<BlobDbContext> options;

        public BlobDbContext()
        {
        }

        public BlobDbContext(DbContextOptions<BlobDbContext> options)
        {
            this.options = options;
        }

        public DbSet<Commodity> Commodities { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("server=localhost\\SQLEXPRESS;database=CommodityInfo;trusted_connection=true;Integrated Security=True;");
        }
    }
}
