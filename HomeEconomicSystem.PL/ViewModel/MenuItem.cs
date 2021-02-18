using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace HomeEconomicSystem.PL.ViewModel
{
    /// <summary>
    /// Represent view-model for menu item.
    /// </summary>
    public class MenuItem : ViewModelBase
    {
        public string Text { get; private set; }
        public PackIconKind IconKind { get; private set; }

        public ICommand Command { get; private set; }

        public MenuItem(string text, PackIconKind iconKind, ICommand command)
        {
            Command = command;
            Text = text;
            IconKind = iconKind;
        }
    }
}
