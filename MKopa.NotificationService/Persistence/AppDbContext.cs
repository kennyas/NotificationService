using Microsoft.EntityFrameworkCore;
using MKopaMessageBox.Domain.Entities;

namespace MKopaMessageBox.Persistence
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            modelBuilder.HasDefaultSchema("Primus_Plus");
        }

        public DbSet<Domain.Entities.MessageBox> MessagesBoxz { get; set; }
        public DbSet<AccessKey> AccessKeys { get; set; }
        public DbSet<AppActivityLog> AppActivityLogs { get; set; }
    }
}
