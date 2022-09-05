namespace dev_pay.Models.Customer
{
    public class CustomerCreateResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public Customer data { get; set; }
    }
}

