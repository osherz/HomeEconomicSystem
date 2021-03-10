using HomeEconomicSystem.BE;
using HomeEconomicSystem.BL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.Model
{
    public class TransactionsModel
    {
        IDataManagement _dataManagement;

        public ObservableCollection<Transaction> Transactions { get; private set; }

        public TransactionsModel()
        {
            _dataManagement = new BL.BL().DataManagement;
            Transactions = new ObservableCollection<Transaction>();
            Filter();
        }

        public void Filter(DateTime? startDate = null, DateTime? endDate = null)
        {
            Transactions.Clear();
            _dataManagement.GetTransactions(startDate, endDate).ToList().ForEach(t => Transactions.Add(t));
        }

        public void Add(Transaction transaction)
        {
            _dataManagement.AddTransaction(transaction);
        }

        public void Update(Transaction transaction)
        {
            _dataManagement.UpdateTransaction(transaction);
        }

    }
}
