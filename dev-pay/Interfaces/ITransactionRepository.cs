using dev_pay.Models.Transaction;

namespace dev_pay.Interfaces
{
    public interface ITransactionRepository
    {
        Task<TransactionDBModel> AddTransactionReference(TransactionDBModel reference);
        Task<TransactionDBModel> FindTransactionReference(string reference);
    }
}
