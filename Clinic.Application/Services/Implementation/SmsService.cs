using Clinic.Application.Services.Interfaces;
using IPE.SmsIrClient;
using IPE.SmsIrClient.Models.Requests;

namespace Clinic.Application.Services.Implementation
{
    public class SmsService : ISmsService
    {
        /// <summary>
        /// SMS panel API Key
        /// </summary>
        private readonly SmsIr _smsIr = new("cgPeotyBFeXmcdb18CvbAxXfsUN3FgvuZdskrx6kHl19KUwx");
        public async Task SendOtp(string mobile, string otp)
        { 
            await _smsIr.VerifySendAsync(mobile, 718643, [new VerifySendParameter("Code" , otp)]);
        }

        #region Send Single SMS
        public async Task SendSingleSms(string mobile, string text)
        {
            await _smsIr.BulkSendAsync(30000123456, text, [mobile]);
        }
        #endregion
    }
}
