using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HomeEconomicSystem.PL.ViewModel
{
    /// <summary>
    /// Store data for page displaying.
    /// </summary>
    public class PageDisplay : IPageDisplay
    {
        public IReadOnlyList<MenuItem> MenuItems { get; }

        public UserControl Content {get;}

        public PageDisplay(UserControl content, IReadOnlyList<MenuItem> menuItems)
        {
            Content = content;
            MenuItems = menuItems;
        }
    }
}
