using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HomeEconomicSystem.PL.ViewModel
{
    /// <summary>
    /// Represent view-model for menu item.
    /// </summary>
    public class MenuItem : ViewModelBase
    {
        public UserControl Content { get; private set; }
        public string Text { get; private set; }
        public PackIconKind IconKind { get; private set; }

        public MenuItem(string text, PackIconKind iconKind, UserControl content)
        {
            Content = content;
            Text = text;
            IconKind = iconKind;
        }
    }
}
