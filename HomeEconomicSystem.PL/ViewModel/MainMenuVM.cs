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
        public ObservableCollection<MenuItem> MenuItems { get; private set; }

        public MainMenuVM()
        {
            MenuItems = new ObservableCollection<MenuItem>
            {
                new MenuItem("בית", PackIconKind.Home, new System.Windows.Controls.UserControl()),
                new MenuItem("ניתוח נתונים", PackIconKind.BarChart, new System.Windows.Controls.UserControl()),
                new MenuItem("קטלוג מוצרים", PackIconKind.Newspaper, new System.Windows.Controls.UserControl()),
                new MenuItem("היסטוריית קניות", PackIconKind.History, new System.Windows.Controls.UserControl()),
            };
        }
    }
}
