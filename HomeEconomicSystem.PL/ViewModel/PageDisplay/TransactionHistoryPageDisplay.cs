using HomeEconomicSystem.PL.Extensions;
using HomeEconomicSystem.PL.ViewModel.TransactionHistory;
using HomeEconomicSystem.Utils;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HomeEconomicSystem.PL.ViewModel.PageDisplay
{
    class TransactionHistoryPageDisplay : INotifyPropertyChanged, IPageDisplay
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private NotifyProperyChanged _notifyPropertyChanged;

        public bool HasItems => MenuItems is not null && MenuItems.Count > 0;
        public IReadOnlyList<MenuItem> MenuItems { get; }

        public UserControl Content { get; }

        public string State => "NotImplementedException";

        public string Title => "היסטוריית עסקאות";


        private bool _showMessage;
        public bool ShowMessage { get => _showMessage; set => SetProperty(ref _showMessage, value); }


        private string _messageToShow;
        public string MessageToShow { get => _messageToShow; set => SetProperty(ref _messageToShow, value); }

        public TransactionHistoryPageDisplay(StateMachine stateMachine)
        {
            _notifyPropertyChanged = new NotifyProperyChanged(this, (property) => OnPropertyChanged(property));

            Content = new View.TransactionHistoryView();
            Content.DataContext = new TransactionHistoryVM();
            MenuItems = new List<MenuItem>();
        }

        private void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
        {
            _notifyPropertyChanged.SetProperty(ref property, value, propertyName);
        }

        private void OnPropertyChanged(PropertyChangedEventArgs property)
        {
            PropertyChanged?.Invoke(this, property);
        }
    }
}
