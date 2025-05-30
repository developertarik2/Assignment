using Assignment.Application.Interfaces;
using Assignment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Assignment.Infrastructure
{
    public class FinDbContext(DbContextOptions<FinDbContext> options) : DbContext(options), IFinDbContext
    {
        public IDbConnection Connection => Database.GetDbConnection();
        public DbSet<User> Users => Set<User>();
        public DbSet<SmsLog> SmsLogs => Set<SmsLog>();
        public DbSet<EmailLog> EmailLogs => Set<EmailLog>();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
