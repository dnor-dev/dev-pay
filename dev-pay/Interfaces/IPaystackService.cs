using dev_pay.Models;
using dev_pay.Models.Customer;
using dev_pay.Models.Customer.Requests;
using dev_pay.Models.Transaction;
using dev_pay.Models.Transaction.Transactions;
using dev_pay.Models.VTU;

namespace dev_pay.Interfaces
{
    public interface IPaystackService
    {
        Task<Customer> Create(SignupModel customer);
        Task<Response> ValidateCustomer(Customer user, ValidateModel model);
        Task<GetCustomerResponse> GetCustomerFullDetails(string? email);
        Task<GetCustomerResponse> UpdatePhone(string? email, UpdatePhone? model);
        Task<InitiateTransactionResponse> InitiateTransaction(InitiateTransactionModel model);
        Task<AirtimeResponseModel> AirtimeTransaction(string reference, AirtimeRequestModel model);
        Task<CableTVResponse> CableTvTransaction(string reference, CableTVModel model);
        Task<PayElectricityResponse> ElectricityTransaction(string reference, PayElectricityModel model);
        Task<Transactions> GetAllTransactions();
        Task<Transaction> GetSingleTransaction(int? id);
        Task<VerifyTransactionModel> VerifyCustomerTransaction(string reference);
    }
}
