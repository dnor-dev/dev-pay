using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace dev_pay.Models
{
    public class Customer
    {
        public int id { get; set; }

        [Required(ErrorMessage = "First name is required" )]
        public string? first_name { get; set; }

        public string? last_name { get; set; }

        [EmailAddress]
        public string? email { get; set; }

        public string? phone { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? password { get; set; }

        public string? customer_code { get; set; }

        public Identifications[]? identifications { get; set; } 

        public Boolean? identified { get; set; }

        public int? integration { get; set; }
    }

    public class Identifications
    {
        [Key]
        public Guid Id { get; set; }
        public string? country { get; set; }

        public string? type { get; set; }

        public string? value { get; set; }
    }
}
