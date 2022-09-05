namespace dev_pay.Models.VTU
{
    public class CableTVResponse
    {
        public string code { get; }
        public string message { get; }
        public CData? data { get; }
    }

    public class CData
    {
        public string? cable_tv { get; }
        public string? subscription_plan { get; }
        public string? smartcard_number { get; }
        public string? phone { get; }
        public string? amount { get; }
        public string? amount_charged { get; }
        public string? service_fee { get; }
        public string? order_id { get; }
    }
}
