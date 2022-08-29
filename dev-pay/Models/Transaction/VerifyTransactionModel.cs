using dev_pay.Models.Transaction.Transactions;

namespace dev_pay.Models.Transaction
{
    public class VerifyTransactionModel
    {
        public bool status { get; set; }
        public string? message { get; set; }
        public VerifyTransactionData? data { get; set; }
    }

    public class VerifyTransactionData : TransData
    {
        public new VerifyCustomer? customer { get; set; }
        public object? pos_transaction_data { get; set; }
        public object? source { get; set; }
        public object? fees_breakdown { get; set; }
        public string? transaction_date { get; set; }
        public object? plan_object { get; set; }
        public object? subaccount { get; set; }
        
    }

    public class VerifyCustomer : ShortCustomerModel
    {
        public string? international_format_phone { get; set; }
    }
}
