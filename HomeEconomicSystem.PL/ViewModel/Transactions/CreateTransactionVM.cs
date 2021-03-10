using HomeEconomicSystem.BE;
using HomeEconomicSystem.BL;
using HomeEconomicSystem.PL.Command;
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
using System.Windows.Input;

namespace HomeEconomicSystem.PL.ViewModel.Transactions
{
    public class CreateTransactionVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private NotifyProperyChanged _notifyPropertyChanged;
        private ITransactionAnalysis _transactionAnalysis;
        private IDataManagement _dataManagement;
        TransactionsStateMachine _stateMachine;

        // TODO: Add option to add new store

        private ObservableCollection<ProductTransaction> _productTransaction;
        public ObservableCollection<ProductTransaction> ProductTransactions
        {
            get { return _productTransaction; }
            set { SetProperty(ref _productTransaction, value); }
        }

        private bool _chooseFromExistence;
        public bool ChooseFromExistence
        {
            get { return _chooseFromExistence; }
            set { SetProperty(ref _chooseFromExistence, value); }
        }

        private bool _createNew;

        public bool CreateNew
        {
            get { return _createNew; }
            set { SetProperty(ref _createNew, value); }
        }


        private Store _newStore;
        public Store NewStore
        {
            get { return _newStore; }
            set { SetProperty(ref _newStore, value); }
        }

        private Product _newProduct;
        public Product NewProduct
        {
            get { return _newProduct; }
            set { SetProperty(ref _newProduct, value); }
        }

        private Category _newCategory;
        public Category NewCategory
        {
            get { return _newCategory; }
            set { SetProperty(ref _newCategory, value); }
        }

        private Transaction _transaction;
        public Transaction Transaction { get => _transaction; private set => SetProperty(ref _transaction, value); }

        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories { get => _categories; set => SetProperty(ref _categories, value); }

        private ObservableCollection<Store> _stores;
        public ObservableCollection<Store> Stores { get => _stores; set => SetProperty(ref _stores, value); }

        private bool _openDialog;
        public bool OpenDialog
        {
            get { return _openDialog; }
            set
            {
                SetProperty(ref _openDialog, value);
                if (OpenDialog) ChooseFromExistence = true;
            }
        }

        private ProductTransaction _selectedProductTransaction;

        public ProductTransaction SelectedProductTransaction
        {
            get { return _selectedProductTransaction; }
            set { SetProperty(ref _selectedProductTransaction, value); }
        }


        private bool _storeChoosing;
        public bool StoreChoosing
        {
            get { return _storeChoosing; }
            set { SetProperty(ref _storeChoosing, value); }
        }

        private bool _categoryChoosing;
        public bool CategoryChoosing
        {
            get { return _categoryChoosing; }
            set { SetProperty(ref _categoryChoosing, value); }
        }

        private bool _productChoosing;
        public bool ProductChoosing
        {
            get { return _productChoosing; }
            set { SetProperty(ref _productChoosing, value); }
        }

        public ICommand ChangeCategory { get; set; }
        public ICommand ChangeStore { get; set; }
        public ICommand ChangeProduct { get; set; }
        public ICommand AddProductTransaction { get; set; }
        public ICommand Finish { get; set; }

        public CreateTransactionVM()
        {
            _notifyPropertyChanged = new NotifyProperyChanged(this, (property) => OnPropertyChanged(property));
            NewCategory = new Category();
            NewStore = new Store();
            NewProduct = new Product();

            var bl = new BL.BL();
            _transactionAnalysis = bl.TransactionAnalysis;
            _dataManagement = bl.DataManagement;
            GenerateNewTransaction();
            LoadAllData();

            _stateMachine = new TransactionsStateMachine(
                new Dictionary<TransactionsState, Action>
                {
                    {TransactionsState.ChoosingCategory, () => { OpenDialog=true; CategoryChoosing=true; } },
                    {TransactionsState.ChoosingStore, () => { OpenDialog=true; StoreChoosing=true; } },
                    {TransactionsState.ChoosingProduct, () => { OpenDialog=true; ProductChoosing=true;  } },
                    {TransactionsState.ChoosedProduct, () =>
                    {
                        if(!ChooseFromExistence)
                        {
                            SelectedProductTransaction.Product = NewProduct;
                            NewProduct = new Product();
                        }
                    } },
                    {TransactionsState.ChoosedCategory, () =>
                    {
                        if(!ChooseFromExistence)
                        {
                            SelectedProductTransaction.Product.Category = NewCategory;
                            NewCategory = new Category();
                        }
                    } },
                    {TransactionsState.ChoosedStore, () =>
                    {
                        if(!ChooseFromExistence)
                        {
                            SelectedProductTransaction.Store = NewStore;
                            NewStore= new Store();
                        }
                    }},
                    {TransactionsState.UpdatingTransaction, UpdateOrCreateTransaction }
                },
                new Dictionary<TransactionsState, Action>
                {
                    {TransactionsState.ChoosingCategory, () => { OpenDialog=false;CategoryChoosing=false; } },
                    {TransactionsState.ChoosingStore, () => { OpenDialog=false; StoreChoosing=false; } },
                    {TransactionsState.ChoosingProduct, () => { OpenDialog=false; ProductChoosing=false; } }
                }
            );

            ChangeCategory = _stateMachine.CreateCommand(TransactionsTriggers.ChangeCategory, ChangeSelectedProductTransaction);
            ChangeStore = _stateMachine.CreateCommand(TransactionsTriggers.ChangeStore, ChangeSelectedProductTransaction);
            ChangeProduct = _stateMachine.CreateCommand(TransactionsTriggers.ChangeProduct, ChangeSelectedProductTransaction);
            AddProductTransaction = new RelayCommand(obj =>
            {
                var productTransaction = new ProductTransaction
                {
                    Amount = 1,
                    UnitPrice = 1
                };
                if (ProductTransactions.Count > 0)
                {
                    productTransaction.Store = ProductTransactions.Last().Store;
                }
                ProductTransactions.Add(productTransaction);
            });
            Finish = _stateMachine.CreateCommand(TransactionsTriggers.Finish);
        }

        private void LoadAllData()
        {
            ProductTransactions = Transaction.ProductTransactions.ToObservableCollection();
            Categories = _dataManagement.GetCategories().ToObservableCollection();
            Stores = _dataManagement.GetStores().ToObservableCollection();
        }

        public void GenerateNewTransaction()
        {
            ProductTransactions = new ObservableCollection<ProductTransaction>();
            Transaction = new Transaction
            {
                Id = 0,
                ProductTransactions = ProductTransactions,
                DateTime = DateTime.Now,
            };
        }

        public void SetTransaction(Transaction transaction)
        {
            Transaction = transaction;
        }

        private void UpdateOrCreateTransaction()
        {
            // Updating transaction
            Transaction.ProductTransactions = ProductTransactions;
            if (Transaction.Id > 0)
            {
                _transactionAnalysis.UpdateTransaction(Transaction);
            }
            else // Adding new transaction
            {
                _transactionAnalysis.AddTransaction(Transaction);
            }
            LoadAllData();
            GenerateNewTransaction();
            _stateMachine.Fire(TransactionsTriggers.Finish);

        }

        private void ChangeSelectedProductTransaction(object obj)
        {
            SelectedProductTransaction = obj as ProductTransaction;
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
