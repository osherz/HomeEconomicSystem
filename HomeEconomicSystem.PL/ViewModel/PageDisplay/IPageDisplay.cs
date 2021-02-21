using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HomeEconomicSystem.PL.ViewModel.PageDisplay
{
    public interface IPageDisplay
    {
        bool HasItems { get; }
        IReadOnlyList<MenuItem> MenuItems { get; }
        UserControl Content { get; }
    }
}
