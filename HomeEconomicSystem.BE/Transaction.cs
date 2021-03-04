using HomeEconomicSystem.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HomeEconomicSystem.BE
{
    public class Transaction : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private NotifyProperyChanged _notifyPropertyChanged;

        private int _id;
        public int Id { get => _id; set => SetProperty(ref _id, value); }

        private DateTime _dateTime;
        public DateTime DateTime{ get => _dateTime; set => SetProperty(ref _dateTime, value); }

        private ICollection<ProductTransaction> _productTransactions;
        public virtual ICollection<ProductTransaction> ProductTransactions { get=> _productTransactions; set => SetProperty(ref _productTransactions, value); }

        public Transaction()
        {
            _notifyPropertyChanged = new NotifyProperyChanged(this, (property) => OnPropertyChanged(property));
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