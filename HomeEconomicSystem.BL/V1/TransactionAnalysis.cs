using HomeEconomicSystem.BE;
using HomeEconomicSystem.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace HomeEconomicSystem.BL.V1
{
    internal class TransactionAnalysis : ITransactionAnalysis
    {
        private IDb _db;
        private IQRCodeFetcher _qRCodeFetcher;
        private Timer _timer;

        public TransactionAnalysis()
        {
            var dalFactory = new DalFactory();
            _db = dalFactory.GetDb();
            _qRCodeFetcher = dalFactory.GetQRCodeFetcher();
        }

        public IEnumerable<Transaction> GetNewTransactions()
        {
            return _db.Transactions;
        }

        public void StartFetchRemotePurchasesForTransactions(int interval, Action transactionDone)
        {
            _timer = new Timer(interval * 1000);
            _timer.Elapsed += (sender, e) =>
            {
                transactionDone?.Invoke();
            };
            _timer.Start();
        }

        public void StopFetchRemotePurchasesForTransactions()
        {
            if (_timer != null) _timer.Stop();
        }

        public void UpdateTransaction(Transaction transaction)
        {
            var oldTransaction = _db.Transactions.Single(t => t.Id == transaction.Id);
            oldTransaction.DateTime = transaction.DateTime;
            oldTransaction.ProductTransactions = transaction.ProductTransactions;
            _db.SaveChanges();
        }
    }
}
