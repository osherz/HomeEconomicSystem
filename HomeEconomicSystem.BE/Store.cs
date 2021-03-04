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
    public class Store : IName, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private NotifyProperyChanged _notifyPropertyChanged;

        private int _id;
        public int Id { get => _id; set => SetProperty(ref _id, value); }

        private string _name;
        [MaxLength(100)]
        public string Name { get => _name; set => SetProperty(ref _name, value); }

        private string _address;
        public string Address { get => _address; set => SetProperty(ref _address, value); }

        private ICollection<ProductTransaction> _productTransaction;
        public virtual ICollection<ProductTransaction> ProductTransaction { get => _productTransaction; set => SetProperty(ref _productTransaction, value); }

        public Store()
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
