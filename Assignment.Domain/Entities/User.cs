using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Domain.Entities
{
    public class User:BaseEntity
    {
        public string? CustomerName {  get; set; }
        public string? ICNumber {  get; set; }
        public string? Email {  get; set; }
        public string? Phone {  get; set; }
        public bool? EmailVerified { get; set; }
        public bool? PhoneVerified { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public int LoginFailedAttemptCount { get; set; }
        public DateTime? LastLoginFailedAttemptDate { get; set; }
        public DateTime? LastLoginDate { get; set; }

        public bool IsActive {  get; set; }
        
    }
}
