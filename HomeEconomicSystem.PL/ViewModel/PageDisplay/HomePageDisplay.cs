using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HomeEconomicSystem.PL.ViewModel.PageDisplay
{
    class HomePageDisplay : IPageDisplay
    {
        public IReadOnlyList<MenuItem> MenuItems { get; }
        public bool HasItems => MenuItems is not null && MenuItems.Count > 0;
        public UserControl Content { get; }

        public HomePageDisplay()
        {
            Content = new View.HomeView();
            Content.DataContext = new HomeVM();
            MenuItems = new List<MenuItem>();
        }
    }
}
