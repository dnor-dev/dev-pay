using System.ComponentModel.DataAnnotations;

namespace dev_pay.Models
{
    public class PaystackModel
    {
        [EmailAddress]
        public string? email { get; set; }

        [MaxLength(11), MinLength(11)]
        public string? phone { get; set; }

        public string? first_name { get; set; }

        public string? last_name { get; set; }
    }
}
