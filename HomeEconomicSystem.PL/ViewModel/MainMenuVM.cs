using HomeEconomicSystem.PL.Extensions;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.ViewModel
{
    public class MainMenuVM
    {
        public IReadOnlyList<MenuItem> MenuItems { get; private set; }

        public MainMenuVM(StateMachine stateMachine)
        {
            MenuItems = new List<MenuItem>
            {
                new MenuItem("בית", PackIconKind.Home, stateMachine.CreateCommand(Triggers.HomeSelected)),
                new MenuItem("ניתוח נתונים", PackIconKind.BarChart,stateMachine.CreateCommand(Triggers.DataAnalysisSelected)),
                new MenuItem("קטלוג מוצרים", PackIconKind.Newspaper, stateMachine.CreateCommand(Triggers.ProductCatalogSelected)),
                new MenuItem("היסטוריית קניות", PackIconKind.History,stateMachine.CreateCommand(Triggers.TransactionHistorySelected)),
            };
        }

        
    }
}
