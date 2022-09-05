namespace dev_pay.Models.VTU
{
    public class VerifyVTUResponse
    {
        public string code { get; }
        public string message { get; }
        public VData? data { get; }
    }

    public class VData
    {
        public string? customer_id { get; }
        public string? customer_name { get; }
        public string? customer_address { get; }
        public string? customer_arrears { get; }
        public string? decoder_status { get; }
        public string? decoder_due_date { get; }
        public string? decoder_balance { get; }
    }
}
