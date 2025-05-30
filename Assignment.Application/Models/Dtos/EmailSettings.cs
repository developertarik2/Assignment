using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Application.Models.Dtos
{
    public class EmailSettings
    {
        public string? SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string? SmtpUsername { get; set; }
        public string? SmtpPassword { get; set; }
        public string? SenderEmail { get; set; }
        public string? DeliveryMethod { get; set; }
        public string? PickupDirectoryLocation { get; set; }
        public bool? EnableSsl { get; set; }       
    }
}
