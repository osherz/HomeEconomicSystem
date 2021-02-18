using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HomeEconomicSystem.PL.ViewModel
{
    class TransactionHistoryPageDisplay : IPageDisplay
    {
        public IReadOnlyList<MenuItem> MenuItems { get; }

        public UserControl Content { get; }

        public TransactionHistoryPageDisplay()
        {
            Content = new View.TransactionHistoryView();
            MenuItems = new List<MenuItem>();
        }
    }
}
