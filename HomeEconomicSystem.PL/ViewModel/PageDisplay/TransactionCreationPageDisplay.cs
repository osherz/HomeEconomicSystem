using HomeEconomicSystem.BE;
using HomeEconomicSystem.PL.View;
using HomeEconomicSystem.PL.ViewModel.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HomeEconomicSystem.PL.ViewModel.PageDisplay
{
    public class TransactionCreationPageDisplay : IPageDisplay
    {
        private CreateTransactionVM _vm;
        public string State => "NotImplementedException";

        public bool HasItems => false;

        public IReadOnlyList<MenuItem> MenuItems => null;

        public UserControl Content { get; }

        public TransactionCreationPageDisplay()
        {
            Content = new CreateTransactionView();
            _vm = new CreateTransactionVM();
            Content.DataContext = _vm;
        }

        public void SetTransaction(Transaction transaction)
        {
            _vm.SetTransaction(transaction);
        }

        public void GenerateTransaction()
        {
            _vm.GenerateNewTransaction();
        }
    }
}
