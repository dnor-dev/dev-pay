using dev_pay.DB;
using dev_pay.Interfaces;
using dev_pay.Models.Transaction;
using Microsoft.EntityFrameworkCore;

namespace dev_pay.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly CustomerContext CustomerDB;

        public TransactionRepository(CustomerContext transactionDB)
        {
            CustomerDB = transactionDB;
        }

        public async Task<TransactionDBModel> AddTransactionReference(TransactionDBModel reference)
        {
            await CustomerDB.Transactions.AddAsync(reference);
             await CustomerDB.SaveChangesAsync();
            return reference;
            throw new NotImplementedException();
        }

        public async Task<TransactionDBModel> FindTransactionReference(string reference)
        {
            var transaction = await CustomerDB.Transactions.Where((x) => x.Reference == reference).FirstOrDefaultAsync();
            if (transaction is null)
            {
                return null;
            }

            return transaction;
        }
    }
}
