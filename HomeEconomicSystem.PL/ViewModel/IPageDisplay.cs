using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HomeEconomicSystem.PL.ViewModel
{
    public interface IPageDisplay
    {
        IReadOnlyList<MenuItem> MenuItems { get; }
        UserControl Content { get; }
    }
}
