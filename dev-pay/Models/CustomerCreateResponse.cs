namespace dev_pay.Models
{
    public class CustomerCreateResponse
    {
        public Boolean status { get; set; }
        public string message { get; set; }
        public Customer data { get; set; }
    }
}
