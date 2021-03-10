using HomeEconomicSystem.BE;
using HomeEconomicSystem.BL;
using HomeEconomicSystem.PL.Extensions;
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
using System.Windows.Input;

namespace HomeEconomicSystem.PL.ViewModel.Catalog
{
    public class CatalogVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private NotifyProperyChanged _notifyPropertyChanged;
        private ProductsModel _productsModel;
        private CategoriesModel _categoriesModel;

        public CatalogStateMachine CatalogStateMachine { get; }

        ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products { get => _products; set => SetProperty(ref _products, value); }

        ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories { get => _categories; set => SetProperty(ref _categories, value); }

        ObservableCollection<Category> _allCategories;
        public ObservableCollection<Category> AllCategories { get => _allCategories; set => SetProperty(ref _allCategories, value); }


        private Category _category;
        public Category SelectedCategory
        {
            get { return _category; }
            set { SetProperty(ref _category, value); }
        }

        private Product _product;
        public Product SelectedProduct
        {
            get { return _product; }
            set { SetProperty(ref _product, value); }
        }

        private bool _creatingNew;
        public bool CreatingNew
        {
            get => _creatingNew;
            set => SetProperty(ref _creatingNew, value);
        }

        private bool _editMode;

        public bool EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }


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

        public ICommand EditCategory { get; }
        public ICommand EditProduct { get; }
        public ICommand Finish { get; }
        public ICommand Cancel { get; }
        public ICommand CreateNew { get; }

        public CatalogVM()
        {
            _notifyPropertyChanged = new NotifyProperyChanged(this, (property) => OnPropertyChanged(property));
            ProductCatalogVM productVM = new ProductCatalogVM();
            _productsModel = new ProductsModel();
            _categoriesModel = new CategoriesModel();
            Dictionary<CatalogStates, Action> statesEntryAction = GetStatesEntryAction();
            Dictionary<CatalogStates, Action> statesExitAction = GetStatesExitAction();

            CatalogStateMachine = new CatalogStateMachine(statesEntryAction, statesExitAction);
            EditCategory = CatalogStateMachine.CreateCommand(
                CatalogTriggers.Edit,
                obj =>
                {
                    SelectedProduct = null;
                    SelectedCategory = new Category();
                    (obj as Category).Copy(SelectedCategory);
                });
            EditProduct = CatalogStateMachine.CreateCommand(
                CatalogTriggers.Edit,
                obj =>
                {
                    SelectedCategory = null;
                    SelectedProduct = new Product();
                    (obj as Product).Copy(SelectedProduct);
                });
            Finish = CatalogStateMachine.CreateCommand(CatalogTriggers.Finish);
            Cancel = CatalogStateMachine.CreateCommand(CatalogTriggers.Cancel);
            CreateNew = CatalogStateMachine.CreateCommand(CatalogTriggers.Create);

            Products = _productsModel.ProductsList;
            Categories = _categoriesModel.CategoriesList;
            LoadData();
            ShowCategories = true;
            ShowProducts = false;

            CatalogStateMachine.Fire(CatalogTriggers.ProductCatalogSelected);
        }

        private void LoadData()
        {
            AllCategories = new CategoriesModel().CategoriesList.ToObservableCollection();
        }

        private Dictionary<CatalogStates, Action> GetStatesEntryAction()
        {
            return new Dictionary<CatalogStates, Action>{
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
                },
                {
                    CatalogStates.ProductCreating , () =>
                    {
                        SelectedProduct = new Product();
                        CreatingNew = true;
                        EditMode = true;
                    }
                },
                {
                    CatalogStates.CategoryCreating , () =>
                    {
                        SelectedCategory = new Category();
                        CreatingNew = true;
                        EditMode = true;
                    }
                },
                { CatalogStates.EditingCategory, () => EditMode = true },
                { CatalogStates.EditingProduct, () => EditMode = true },
                {
                    CatalogStates.SavingCategory, () =>
                    {
                        if(CreatingNew)
                        {
                            _categoriesModel.AddCategory(SelectedCategory);
                            CreatingNew = false;
                        }
                        else
                        {
                            _categoriesModel.Update(SelectedCategory);
                            CreatingNew = false;
                        }
                        SelectedCategory = null;
                        CatalogStateMachine.Fire(CatalogTriggers.Finish);
                        LoadData();
                        CatalogStateMachine.Fire(CatalogTriggers.Search);
                    }
                },
                {
                    CatalogStates.SavingProduct, () =>
                    {
                        if(CreatingNew)
                        {
                            _productsModel.AddProduct(SelectedProduct);
                        }
                        else
                        {
                            _productsModel.Update(SelectedProduct);
                        }
                        SelectedProduct = null;
                        CatalogStateMachine.Fire(CatalogTriggers.Finish);
                        LoadData();
                        CatalogStateMachine.Fire(CatalogTriggers.Search);
                    }
                }
            };
        }

        private Dictionary<CatalogStates, Action> GetStatesExitAction()
        {
            return new Dictionary<CatalogStates, Action>{
                { CatalogStates.EditingCategory, () => EditMode = false },
                { CatalogStates.EditingProduct, () => EditMode = false },
                { CatalogStates.ProductCreating, () => EditMode = false },
                { CatalogStates.CategoryCreating, () => EditMode = false },
            };
        }

        private void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
        {
            _notifyPropertyChanged.SetProperty(ref property, value, propertyName);
        }

        private void OnPropertyChanged(PropertyChangedEventArgs property)
        {
            PropertyChanged?.Invoke(this, property);
            if (property.PropertyName == nameof(SearchText))
            {
                CatalogStateMachine.Fire(CatalogTriggers.Search);
            }
        }
    }
}
