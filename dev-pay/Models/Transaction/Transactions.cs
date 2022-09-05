namespace dev_pay.Models.Transaction.Transactions
{
    public class Transactions
    {
        public bool status { get; set; }
        public string? message { get; set; }
        public TransactionData[]? data { get; set; }
        public Meta meta { get; set; }
    }

    public class TransactionData
    {
        public int? id { get; set; }
        public string? domain { get; set; }
        public string? status { get; set; }
        public string? reference { get; set; }
        public int amount { get; set; }
        public string? message { get; set; }
        public string? gateway_response { get; set; }
        public string? paid_at { get; set; }
        public string? created_at { get; set; }
        public string? channel { get; set; }
        public string? currency { get; set; }
        public string? ip_address { get; set; }
        public dynamic? timeline { get; set; }
        public MetaData? metadata { get; set; }
        public Authorization? authorization { get; set; }
        public ShortCustomerModel? customer { get; set; }
        public string? requested_amount { get; set; }
    }

    public class MetaData
    {
        public CustomFields[]? custom_fields { get; set; }
    }

    public class CustomFields
    {
        public string? display_name { get; set; }
        public string? variable_name { get; set; }
        public string? value { get; set; }
        public Log? log { get; set; }
        public int? fees { get; set; }
    }

    public class Log
    {
        public int? start_time { get; set; }
        public int? time_spent { get; set; }
        public int? attempts { get; set; }
        public int? errors { get; set; }
        public bool success { get; set; }
        public bool mobile { get; set; }
        public object[]? input { get; set; }
        public History[]? history { get; set; }
    }

    public class History
    {
        public string? type { get; set; } 
        public string? message { get; set; }
        public int? time { get; set; }
    }

    public class Authorization
    {
        public string? authorization_code { get; set; }
        public string? bin { get; set; }
        public string? last4 { get; set; }
        public string? exp_month { get; set; }
        public string? exp_year { get; set; }
        public string? card_type { get; set; }
        public string? bank { get; set; }
        public string? country_code { get; set; }
        public string? brand { get; set; }
        public string? account_name { get; set; }
    }

    public class ShortCustomerModel
    {
        public int? id { get; set; }
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string? customer_code { get; set; }
        public object? metadata { get; set; }
        public string? risk_action { get; set; }
    }

    public class Meta
    {
        public int? total { get; set; }
        public int? skipped { get; set; }
        public int? perPage { get; set; }
        public int? page { get; set; }
        public int? pageCount { get; set; }
    }
}
