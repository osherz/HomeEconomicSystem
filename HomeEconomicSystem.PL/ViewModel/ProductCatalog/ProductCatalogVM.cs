﻿using HomeEconomicSystem.PL.View;
using HomeEconomicSystem.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HomeEconomicSystem.PL.ViewModel.ProductCatalog
{
    public class ProductCatalogVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private NotifyProperyChanged _notifyPropertyChanged;

        public ProductCatalogStateMachine ProductCatalogStateMachine { get;}

        private UserControl _content;

        public UserControl Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        ProductView productView { get; set; }
        CategoryView CategoryView { get; set; }

        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set => SetProperty(ref _searchText, value);
        }



        public ProductCatalogVM()
        {
            _notifyPropertyChanged = new NotifyProperyChanged(this, (property) => OnPropertyChanged(property));
            ProductVM productVM = new ProductVM();

            var statesEntryAction = new Dictionary<States, Action>{
                {
                    States.ProductCatalog, ()=>
                    {
                        if(productView is null)
                        {
                            productView = new ProductView();
                            productView.DataContext = productVM;
                        }
                        Content = productView;
                    }
                },
                {States.CategoryCatalog, () => Content = new CategoryView() }
            };
            //TODO: Concat to productView state machine dictionary.

            ProductCatalogStateMachine = new ProductCatalogStateMachine(statesEntryAction);
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
