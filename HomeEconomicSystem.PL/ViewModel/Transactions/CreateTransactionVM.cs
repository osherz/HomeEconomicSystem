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

        private ObservableCollection<ProductTransaction> _productTransaction;
        public ObservableCollection<ProductTransaction> ProductTransaction
        {
            get { return _productTransaction; }
            set { SetProperty(ref _productTransaction, value); }
        }

        private Transaction _transaction;
        public Transaction Transaction { get => _transaction; set => SetProperty(ref _transaction, value); }

        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<Store> Stores { get; set; }

        private bool _openDialog;
        public bool OpenDialog
        {
            get { return _openDialog; }
            set { SetProperty(ref _openDialog, value); }
        }

        private ProductTransaction _selectedProductTransaction;

        public ProductTransaction SelectedProductTransaction
        {
            get { return _selectedProductTransaction; }
            set { SetProperty(ref _selectedProductTransaction , value); }
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
            var bl = new BL.BL();
            _transactionAnalysis = bl.TransactionAnalysis;
            _dataManagement = bl.DataManagement;
            Transaction = _dataManagement.GetTransactions()
                .First();
            ProductTransaction = Transaction.ProductTransactions.ToObservableCollection();
            Categories = _dataManagement.GetCategories().ToObservableCollection();
            Stores = _dataManagement.GetStores().ToObservableCollection();

            _stateMachine = new TransactionsStateMachine(
                new Dictionary<TransactionsState, Action>
                {
                    {TransactionsState.ChoosingCategory, () => { OpenDialog=true; CategoryChoosing=true; } },
                    {TransactionsState.ChoosingStore, () => { OpenDialog=true; StoreChoosing=true; } },
                    {TransactionsState.ChoosingProduct, () => { OpenDialog=true; ProductChoosing=true; } },
                    {TransactionsState.UpdatingTransaction, () =>  {_transactionAnalysis.UpdateTransaction(Transaction); _stateMachine.Fire(TransactionsTriggers.Finish); } }
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
            AddProductTransaction = new RelayCommand(obj => ProductTransaction.Add(new ProductTransaction()));
            Finish = _stateMachine.CreateCommand(TransactionsTriggers.Finish);
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
