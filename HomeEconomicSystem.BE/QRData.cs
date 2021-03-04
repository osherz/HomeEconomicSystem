using HomeEconomicSystem.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.BE
{
    public class QRData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private NotifyProperyChanged _notifyPropertyChanged;

        private int _id;
        public int Id { get => _id; set => SetProperty(ref _id, value); }

        private string _barCode;
        public string BarCode { get => _barCode; set => SetProperty(ref _barCode, value); }

        private string _name;
        public string Name { get => _name; set => SetProperty(ref _name, value); }

        private float _amount;
        public float Amount { get => _amount; set => SetProperty(ref _amount, value); }

        private float _unitPrice;
        public float UnitPrice { get => _unitPrice; set => SetProperty(ref _unitPrice, value); }

        private string _storeName;
        public string StoreName{ get => _storeName; set => SetProperty(ref _storeName, value); }

        private DateTime _scanDate;
        public DateTime ScanDate { get => _scanDate; set => SetProperty(ref _scanDate, value); }

        private ProductTransaction _productTransaction;
        public virtual ProductTransaction ProductTransaction { get => _productTransaction; set => SetProperty(ref _productTransaction, value); }

        public QRData()
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
