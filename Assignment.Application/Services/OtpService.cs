using Assignment.Application.Interfaces;
using Assignment.Application.Models.Dtos;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Security.Cryptography;

namespace Assignment.Application.Services
{
    public class OtpService : IOtpService
    {
        private readonly StackExchange.Redis.IDatabase _redis;
        private readonly IConfiguration _config;

        public OtpService(IConfiguration config, IConnectionMultiplexer redis)
        {
            _config = config;
            _redis = redis.GetDatabase();
        }
        public async Task<string> GenerateOtp(string recipient, int Length, OtpChannel channel)
        {
           
                using var rng = RandomNumberGenerator.Create();
                var bytes = new byte[Length];
                rng.GetBytes(bytes);
                var otp = string.Concat(bytes.Select(b => (b % 10).ToString())); // Digits only


                // string phoneNumber = "";
                string hashedOtp = BCrypt.Net.BCrypt.HashPassword(otp);
                string redisKey = $"{channel}:{recipient}";
                await _redis.StringSetAsync(redisKey, hashedOtp, TimeSpan.FromMinutes(2));

                return otp;
            
        }

        public async Task<bool> VerifyOtpAsync(string recipient, string otpCode, OtpChannel channel)
        {
            string redisKey = $"{channel}:{recipient}";
            var storedOtp = await _redis.StringGetAsync(redisKey);
         

            if (!storedOtp.IsNullOrEmpty && BCrypt.Net.BCrypt.Verify(otpCode, storedOtp))
            {
                // ✅ OTP is valid

                await _redis.KeyDeleteAsync(redisKey); // Invalidate OTP after use

                return true;
            }
            else
            {
                // ❌ OTP is invalid or expired

                return false;
            }
           

          
        }
    }
}
