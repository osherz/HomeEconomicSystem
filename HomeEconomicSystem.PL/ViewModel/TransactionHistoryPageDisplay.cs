using HomeEconomicSystem.PL.Extensions;
using MaterialDesignThemes.Wpf;
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
        public bool HasItems => MenuItems is not null && MenuItems.Count > 0;
        public IReadOnlyList<MenuItem> MenuItems { get; }

        public UserControl Content { get; }

        public TransactionHistoryPageDisplay(StateMachine stateMachine)
        {
            Content = new View.TransactionHistoryView();
            MenuItems = new List<MenuItem>();
        }
    }
}
