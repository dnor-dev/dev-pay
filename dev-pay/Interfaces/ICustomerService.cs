using dev_pay.Models;
using dev_pay.Models.Customer;
using dev_pay.Models.Customer.Requests;
using dev_pay.Models.Transaction;
using dev_pay.Models.Transaction.Transactions;

namespace dev_pay.Interfaces
{
    public interface ICustomerService
    {
        Task<Customer> Create(SignupModel customer);
        Task<Response> ValidateCustomer(Customer user, ValidateModel model);
        Task<GetCustomerResponse> GetCustomerFullDetails(string? email);
        Task<GetCustomerResponse> UpdatePhone(string? email, UpdatePhone? model);
        Task<InitiateTransactionResponse> InitiateTransaction(InitiateTransactionModel model);
        Task<Transactions> GetAllTransactions();
        Task<Transaction> GetSingleTransaction(int? id);
        Task<VerifyTransactionModel> VerifyCustomerTransaction(string reference);
    }
}
