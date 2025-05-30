using Assignment.Application.Interfaces;
using Assignment.Application.Models.Dtos;
using Assignment.Domain.Entities;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using RestSharp;

namespace Assignment.Application.Services
{
    public class BroadcastHandler : IBroadcastHandler
    {
        private readonly EmailSettings _emailSettings;
        private readonly SmsSettings _smsSettings;

        private readonly IFinDbContext _context;
        public BroadcastHandler(
            IOptions<EmailSettings> emailSettings,
            IOptions<SmsSettings> smsSettings,
            IFinDbContext context)
        {
            _emailSettings = emailSettings.Value;
            _smsSettings = smsSettings.Value;
            _context = context;
        }

        public Task<bool> SaveEmailLog(EmailLog model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveSmsLog(SmsLog model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SendEmail(string to, string subject, string body, string? memberName = null, int? UserId = null)
        {

            try
            {
                using var smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
                {
                    Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword),
                    EnableSsl = true,
                };
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SenderEmail!),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(to);
                await smtpClient.SendMailAsync(mailMessage);

                EmailLog email = new EmailLog()
                {
                    EmailId = to,
                    Message = body,
                    EmailDate = DateTime.UtcNow,
                    MemberName = memberName ?? "",
                    UserId = UserId,
                    Status = "Ok"
                };
                await SaveEmailLog(email);


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SendMessage(string mobile,  string template, int? UserId = null)
        {
            try
            {
                var client = new RestClient(_smsSettings.SingleSmsUrl!);
                var request = new RestRequest
                {
                    Timeout = TimeSpan.FromMilliseconds(30000),  // Set timeout in milliseconds (e.g., 10 seconds)
                    Method = Method.Post
                };

                request.AddParameter("api_token", _smsSettings.SingleSmsApiToken);
                request.AddParameter("sid", _smsSettings.SingleSmsSid);
                request.AddParameter("msisdn", mobile);
                request.AddParameter("sms", template);
                request.AddParameter("csms_id", _smsSettings.SingleSmsCsmsId);
                RestResponse response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    SmsLog sms = new SmsLog()
                    {
                        PhoneNo = mobile,
                        Message = template,
                        SmsDate = DateTime.UtcNow,
                        // MemberName = memberName != null ? memberName : "",
                        UserId = UserId,
                        Status = response.IsSuccessful ? "Ok" : "No"
                    };
                    await SaveSmsLog(sms);
                }


                return response.IsSuccessful;
            }
            catch (Exception ex) { return false; }
        }
    }
}
