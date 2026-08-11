using Clinic.Application.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Clinic.Application.Services.Implementation
{
    public class OtpService(IMemoryCache cache) : IOtpService
    {
        public string GenrateOtp(string mobile)
        {
            var otp = new Random().Next(10000, 99999).ToString();
            otp = "88888";
            cache.Set(mobile , otp , TimeSpan.FromMinutes(2));
            return otp;
        }

        public bool CanRegenrateOtp(string mobile)
        {
            return !cache.TryGetValue(mobile, out _);
        }

        public bool ValidateOtp(string mobile, string otp)
        {
            return cache.TryGetValue(mobile , out string? savedOtp) && savedOtp == otp;
        }
    }
}
