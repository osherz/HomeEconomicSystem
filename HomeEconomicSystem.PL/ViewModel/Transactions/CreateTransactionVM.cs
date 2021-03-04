using HomeEconomicSystem.BE;
using HomeEconomicSystem.PL.Extensions;
using HomeEconomicSystem.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.ViewModel.Transactions
{
    public class CreateTransactionVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private NotifyProperyChanged _notifyPropertyChanged;

        private ObservableCollection<ProductTransaction> _productTransaction;
        public ObservableCollection<ProductTransaction> ProductTransaction
        {
            get { return _productTransaction; }
            set { SetProperty(ref _productTransaction, value); }
        }

        public CreateTransactionVM()
        {
            _notifyPropertyChanged = new NotifyProperyChanged(this, (property) => OnPropertyChanged(property));
            ProductTransaction = new BL.BL().DataManagement.GetTransactions()
                .First()
                .ProductTransactions.ToObservableCollection();
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
