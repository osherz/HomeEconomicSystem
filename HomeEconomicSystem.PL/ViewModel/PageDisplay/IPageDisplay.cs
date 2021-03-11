using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HomeEconomicSystem.PL.ViewModel.PageDisplay
{
    public interface IPageDisplay : INotifyPropertyChanged
    {
        bool ShowMessage { get; set; }
        string MessageToShow { get; set; }
        string Title { get; }
        string State { get; }
        bool HasItems { get; }
        IReadOnlyList<MenuItem> MenuItems { get; }
        UserControl Content { get; }
    }
}
