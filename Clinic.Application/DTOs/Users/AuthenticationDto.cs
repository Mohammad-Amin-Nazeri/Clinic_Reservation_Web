using System.ComponentModel.DataAnnotations;

namespace Clinic.Application.DTOs.Users
{
    public class AuthenticationDto
    {
        public string Mobile { get; set; }

        [MaxLength(1)]
        [MinLength(1)]
        public string num1 { get; set; }

        [MaxLength(1)]
        [MinLength(1)]
        public string num2 { get; set; }

        [MaxLength(1)]
        [MinLength(1)]
        public string num3 { get; set; }

        [MaxLength(1)]
        [MinLength(1)]
        public string num4 { get; set; }

        [MaxLength(1)]
        [MinLength(1)]
        public string num5 { get; set; }
    }
}
