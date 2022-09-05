namespace dev_pay.Models.VTU
{
    public class PayElectricityResponse
    {
        public string code { get; }
        public string message { get; }
        public EData? data { get; }
    }

    public class EData
    {
        public string? electricity { get; }
        public string? meter_number { get; }
        public string? token { get; }
        public string? units { get; }
        public string? phone { get; }
        public string? amount { get; }
        public string? amount_charged { get; }
        public string? order_id { get; }
    }
}
