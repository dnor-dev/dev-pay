namespace dev_pay.Models.Customer.Requests
{
    public class ChangePasswordModel
    {
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
