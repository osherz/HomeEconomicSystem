using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HomeEconomicSystem.PL.ViewModel
{
    class HomePageDisplay : IPageDisplay
    {
        public IReadOnlyList<MenuItem> MenuItems { get; }

        public UserControl Content { get; }

        public HomePageDisplay()
        {
            Content = new View.HomeView();
            MenuItems = new List<MenuItem>();
        }
    }
}
