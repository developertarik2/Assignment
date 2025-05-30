using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Application.Models.Dtos
{
    public class SmsSettings
    {
        public string? SingleSmsUrl { get; set; }
        public string? SingleSmsSid { get; set; }
        public string? SingleSmsCsmsId { get; set; }
        public string? SingleSmsApiToken { get; set; }
    }

    public enum OtpChannel
    {
        Sms,
        Email
    }
}
