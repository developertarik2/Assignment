using Assignment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Application.Interfaces
{
    public interface IBroadcastHandler
    {
        Task<bool> SendEmail(string to, string subject, string body, string? memberName = null, int? UserId = null);
        Task<bool> SendMessage(string mobile,  string template, int? UserId = null);

        Task<bool> SaveSmsLog(SmsLog model);
        Task<bool> SaveEmailLog(EmailLog model);
    }
}
