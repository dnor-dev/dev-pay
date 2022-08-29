using dev_pay.Models.Transaction.Transactions;

namespace dev_pay.Models.Transaction
{
    public class Transaction
    {
        public bool status { get; set; }
        public string? message { get; set; }
        public TransData? data { get; set; }
    }

    public class TransData : TransactionData
    {
        public int? fees_split { get; set; }
        public new TransAuthorization? authorization { get; set; }
        public object? plan { get; set; }
        public int? order_id { get; set; }
    }

    public class TransAuthorization : Authorization
    {
        public bool reusable { get; set; }
        public string? signature { get; set; }
        
    }
}
