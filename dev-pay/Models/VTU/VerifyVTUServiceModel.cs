namespace dev_pay.Models.VTU
{
    public class VerifyVTUServiceModel
    {
        public string customer_id { get; set; }
        public string service_id { get; set; }
        public string? variation_id { get; set; }
    }
}
