using HomeEconomicSystem.BE;
using HomeEconomicSystem.PL.View;
using HomeEconomicSystem.PL.ViewModel.Transactions;
using HomeEconomicSystem.Utils;
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
    public class TransactionCreationPageDisplay : IPageDisplay, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private NotifyProperyChanged _notifyPropertyChanged;

        private CreateTransactionVM _vm;
        public string State => "NotImplementedException";

        public bool HasItems => false;

        public IReadOnlyList<MenuItem> MenuItems => null;

        public UserControl Content { get; }

        public string Title => "יצירת עסקה חדשה";

        private bool _showMessage;
        public bool ShowMessage { get => _showMessage; set => SetProperty(ref _showMessage, value); }


        private string _messageToShow;
        public string MessageToShow { get => _messageToShow; set => SetProperty(ref _messageToShow, value); }

        public TransactionCreationPageDisplay()
        {
            _notifyPropertyChanged = new NotifyProperyChanged(this, (property) => OnPropertyChanged(property));

            Content = new CreateTransactionView();
            _vm = new CreateTransactionVM(this);
            Content.DataContext = _vm;
        }

        public void SetTransaction(Transaction transaction)
        {
            _vm.SetTransaction(transaction);
        }

        public void GenerateTransaction()
        {
            _vm.GenerateNewTransaction();
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
