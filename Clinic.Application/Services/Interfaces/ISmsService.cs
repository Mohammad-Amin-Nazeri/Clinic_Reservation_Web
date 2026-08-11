namespace Clinic.Application.Services.Interfaces
{
    public interface ISmsService
    {
        Task SendOtp(string mobile, string otp);
        Task SendSingleSms(string mobile , string text);
    }
}
