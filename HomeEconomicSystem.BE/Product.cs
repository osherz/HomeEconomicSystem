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


    public class Product : IName, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private NotifyProperyChanged _notifyPropertyChanged;

        private int _id;
        public int Id { get => _id; set => SetProperty(ref _id, value); }

        private string _barCode;
        [MinLength(13), MaxLength(15)]
        public string BarCode { get => _barCode; set => SetProperty(ref _barCode, value); }


        private string _name;
        public string Name { get => _name; set => SetProperty(ref _name, value); }

        private string _description;
        public string Description { get => _description; set => SetProperty(ref _description, value); }


        private string _imageFileName;
        public string ImageFileName { get => _imageFileName; set => SetProperty(ref _imageFileName, value); }

        private Category _category;
        public virtual Category Category { get => _category; set => SetProperty(ref _category, value); }

        private ICollection<ProductTransaction> _productTransactions;
        public virtual ICollection<ProductTransaction> ProductTransactions { get=> _productTransactions; set => SetProperty(ref _productTransactions, value); }

        public Product()
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
