using HomeEconomicSystem.Utils;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace HomeEconomicSystem.BE
{
    public class ProductTransaction: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private NotifyProperyChanged _notifyPropertyChanged;

        private int _id;
        public int Id { get => _id; set => SetProperty(ref _id, value); }

        private float _unitPrice;
        public float UnitPrice { get=> _unitPrice; set => SetProperty(ref _unitPrice, value); }

        private float _amount;
        /// <summary>
        ///Amount of products.
        /// </summary>
        public float Amount { get => _amount; set => SetProperty(ref _amount, value); }

        private QRData _qRData;
        [Required]
        public virtual QRData QRData { get => _qRData; set => SetProperty(ref _qRData, value); }

        private Product _product;
        public virtual Product Product { get=> _product; set=> SetProperty(ref _product, value); }

        private Transaction _transaction;
        public virtual Transaction Transaction { get => _transaction; set => SetProperty(ref _transaction, value); }

        private Store _store;
        public virtual Store Store { get => _store; set => SetProperty(ref _store, value); }

        public ProductTransaction()
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