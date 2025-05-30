using Assignment.Application.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Application.Interfaces
{
    public interface IOtpService
    {
        Task<string> GenerateOtp(string recipient, int Length, OtpChannel channel);
        Task<bool> VerifyOtpAsync(string recipient, string otpCode, OtpChannel channel);
    }
}
