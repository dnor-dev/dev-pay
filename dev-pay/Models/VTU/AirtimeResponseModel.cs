namespace dev_pay.Models.VTU
{
    public class AirtimeResponseModel
    {
        public string? code { get; set; }
        public string? message { get; set; }
        public Data? data { get; set; }
    }

    public class Data
    {
        public string? network { get; set; }
        public string? phone { get; set; }
        public string? amount { get; set; }
        public string? order_id { get; set; }
    }
}
