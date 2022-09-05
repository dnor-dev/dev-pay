using dev_pay.Models.VTU;

namespace dev_pay.Models.Transaction
{
    public class InitiateTransactionModel
    {
        public string? email { get; set; }
        public int amount { get; set; }
        public string? callback_url { get; set; }
        public Meta? metadata { get; set; }
    }

    public class Meta
    {
        public string cancel_action { get; set; }
    }
}
