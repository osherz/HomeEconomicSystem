using HomeEconomicSystem.BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.BL
{
    public interface ITransactionAnalysis
    {
        /// <summary>
        /// Return all new transactions that the user still not approve.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Transaction> GetNewTransactions();

        /// <summary>
        /// When active, start fetching purchases from remote drive in another threads,
        /// When there is a new transaction call transactionDone callback.
        /// </summary>
        /// <param name="interval">Frequence of sample in seconds.</param>
        /// <param name="transactionDone">Call when a new transaction found</param>
        void StartFetchRemotePurchasesForTransactions(int interval, Action transactionDone);

        /// <summary>
        /// Stop StartFetchRemotePurchasesForTransactions.
        /// </summary>
        void StopFetchRemotePurchasesForTransactions();
    }
}
