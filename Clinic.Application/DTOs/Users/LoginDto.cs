using System.ComponentModel.DataAnnotations;

namespace Clinic.Application.DTOs.Users
{
    public class LoginDto
    {
        [MinLength(11 , ErrorMessage = "نمی توان کمتر از 11 کاراکتر باشد.")]
        [MaxLength(11 ,ErrorMessage = "نمی توان بیشتر از 11 کاراکتر باشد.")]
        public string Mobile { get; set; }

        [MinLength(5, ErrorMessage = "نمی توان کمتر از 5 کاراکتر باشد.")]
        [MaxLength(5, ErrorMessage = "نمی توان بیشتر از 5 کاراکتر باشد.")]
        public string CaptchaCode { get; set; }
    }
}
