using HomeEconomicSystem.BE;
using HomeEconomicSystem.BL;
using HomeEconomicSystem.PL.Model;
using HomeEconomicSystem.PL.View;
using HomeEconomicSystem.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HomeEconomicSystem.PL.ViewModel.Catalog
{
    public class CatalogVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private NotifyProperyChanged _notifyPropertyChanged;
        private ProductsModel _productsModel;
        private CategoriesModel _categoriesModel;

        public CatalogStateMachine CatalogStateMachine { get;}

        ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products { get => _products; set => SetProperty(ref _products, value); }

        ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories { get => _categories; set => SetProperty(ref _categories, value); }

        private bool _showProducts;
        public bool ShowProducts
        {
            get { return _showProducts; }
            set { SetProperty(ref _showProducts, value); }
        }

        private bool _showCategories;

        public bool ShowCategories
        {
            get { return _showCategories; }
            set { SetProperty(ref _showCategories, value); }
        }


        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set => SetProperty(ref _searchText, value);
        }



        public CatalogVM()
        {
            _notifyPropertyChanged = new NotifyProperyChanged(this, (property) => OnPropertyChanged(property));
            ProductCatalogVM productVM = new ProductCatalogVM();
            _productsModel = new ProductsModel();
            _categoriesModel = new CategoriesModel();

            var statesEntryAction = new Dictionary<CatalogStates, Action>{
                {
                    CatalogStates.ProductCatalog, ()=>
                    {
                        ShowProducts = true;
                        ShowCategories = false;
                        Products = _productsModel.ProductsList;
                    }
                },
                {
                    CatalogStates.CategoryCatalog, () =>
                    {
                        ShowCategories = true;
                        ShowProducts = false;
                        Categories = _categoriesModel.CategoriesList;
                    }
                },
                {
                    CatalogStates.SearchingCategory, () =>
                    {
                        _categoriesModel.Filter(SearchText);
                        CatalogStateMachine.Fire(CatalogTriggers.SearchSucceeded);
                    }
                },
                {
                    CatalogStates.SearchingProduct, () =>
                    {
                        _productsModel.Filter(SearchText);
                        CatalogStateMachine.Fire(CatalogTriggers.SearchSucceeded);
                    }
                }
            };
            //TODO: Concat to productView state machine dictionary.

            CatalogStateMachine = new CatalogStateMachine(statesEntryAction);

            Products = _productsModel.ProductsList;
            Categories = _categoriesModel.CategoriesList;
            ShowCategories = true;
            ShowProducts = false;
        }

        private void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
        {
            _notifyPropertyChanged.SetProperty(ref property, value, propertyName);
        }

        private void OnPropertyChanged(PropertyChangedEventArgs property)
        {
            PropertyChanged?.Invoke(this, property);
            if(property.PropertyName == nameof(SearchText))
            {
                CatalogStateMachine.Fire(CatalogTriggers.Search);
            }
        }
    }
}
