using Assignment.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Application.Interfaces
{
    public interface IFinDbContext
    {
        DbSet<User> Users { get; }
        DbSet<EmailLog> EmailLogs { get; }
        DbSet<SmsLog> SmsLogs { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
