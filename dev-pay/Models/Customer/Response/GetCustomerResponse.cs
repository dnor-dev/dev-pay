using dev_pay.Models.Customer;

namespace dev_pay.Models
{
    public class GetCustomerResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public CustomerData data { get; set; }
    }

    public class CustomerData
    {
        public int integration { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public object? dedicated_account { get; set; }
        public Photo metadata { get; set; }
        public bool identified { get; set; }
        public Identifications[]? identifications { get; set; }
        public string domain { get; set; }
        public string customer_code { get; set; }
        public int id { get; set; }
        public dynamic[] transactions { get; set; }
        public dynamic[] subscriptions { get; set; }
        public dynamic[] authorization { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
    }

    public class Photo
    {
        public Meta[] photos { get; set; }
    }

    public class Meta
    {
        public string type { get; set; }
        public string typeId { get; set; }
        public string typeName { get; set; }
        public string url { get; set; }
        public bool isPrimary { get; set; }
    }
}
