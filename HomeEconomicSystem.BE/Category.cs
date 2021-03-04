using HomeEconomicSystem.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HomeEconomicSystem.BE
{
    public class Category : IName, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private NotifyProperyChanged _notifyPropertyChanged;

        private int _id;
        public int Id { get => _id; set=>SetProperty(ref _id, value); }

        private string _name;
        public string Name { get => _name; set=>SetProperty(ref _name, value); }

        private string _imageFileName;
        public string ImageFileName { get => _imageFileName; set => SetProperty(ref _imageFileName, value); }

        private ICollection<Product> _products;
        public virtual ICollection<Product> Products { get => _products; set => SetProperty(ref _products, value); }

        public Category()
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