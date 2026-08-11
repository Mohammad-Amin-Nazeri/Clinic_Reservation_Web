namespace Clinic.Application.Services.Interfaces
{
    public interface IOtpService
    {
        string GenrateOtp(string mobile);
        bool ValidateOtp(string mobile , string otp);
        bool CanRegenrateOtp(string mobile);
    }
}
