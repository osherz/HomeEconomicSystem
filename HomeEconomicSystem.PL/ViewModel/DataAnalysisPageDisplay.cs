using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HomeEconomicSystem.PL.ViewModel
{
    class DataAnalysisPageDisplay : IPageDisplay
    {
        public IReadOnlyList<MenuItem> MenuItems { get; }

        public UserControl Content { get; }

        public DataAnalysisPageDisplay()
        {
            Content = new View.DataAnalysisView();
            MenuItems = new List<MenuItem>();
        }
    }
}
