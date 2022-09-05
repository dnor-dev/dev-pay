using System.ComponentModel.DataAnnotations;

namespace dev_pay.Models
{
    public class SignupModel
    {
        [EmailAddress]
        public string? email { get; set; }

        [MaxLength(11), MinLength(11)]
        public string? phone { get; set; }

        public string? first_name { get; set; }

        public string? last_name { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? password { get; set; }
    }
}
