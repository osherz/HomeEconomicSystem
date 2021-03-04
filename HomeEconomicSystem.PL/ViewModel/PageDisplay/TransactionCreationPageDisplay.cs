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
        public string State => "NotImplementedException";

        public bool HasItems => false;

        public IReadOnlyList<MenuItem> MenuItems => null;

        public UserControl Content { get; }

        public TransactionCreationPageDisplay()
        {
            Content = new CreateTransactionView();
            Content.DataContext = new CreateTransactionVM();
        }
    }
}
