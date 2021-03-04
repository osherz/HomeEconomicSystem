using HomeEconomicSystem.PL.Extensions;
using HomeEconomicSystem.PL.ViewModel.PageDisplay;
using HomeEconomicSystem.Utils;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace HomeEconomicSystem.PL.ViewModel
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private NotifyProperyChanged _notifyPropertyChanged;

        private Dictionary<States, Action> _stateActionDict;
        private StateMachine _stateMachine;

        public MainMenuVM MainMenuVM { get; private set; }
        public IReadOnlyList<MenuItem> ToolBarItems { get; private set; }

        private IPageDisplay _pageDisplay;
        public IPageDisplay PageDisplay
        {
            get => _pageDisplay;
            set => SetProperty(ref _pageDisplay, value);
        }

        public MainWindowVM()
        {
            _notifyPropertyChanged = new NotifyProperyChanged(this, (property) => OnPropertyChanged(property));

            IPageDisplay homePageDisplay = new HomePageDisplay();
            IPageDisplay dataAnalysisDisplay = new DataAnalysisPageDisplay();
            IPageDisplay productCatalogDisplay = new ProductCatalogPageDisplay();
            IPageDisplay transactionHistoryDisplay = new TransactionHistoryPageDisplay(_stateMachine);

            _stateActionDict = new Dictionary<States, Action>
            {
                {States.Home, ()=>PageDisplay=homePageDisplay },
                {States.DataAnalysis, ()=>PageDisplay=dataAnalysisDisplay },
                {States.ProductCatalog, ()=>PageDisplay=productCatalogDisplay },
                {States.TransactionHistory, ()=>PageDisplay=transactionHistoryDisplay }
            };
            _stateMachine = new StateMachine(_stateActionDict);

            MainMenuVM = new MainMenuVM(_stateMachine);
            InitToolBarItems();
        }

        private void InitToolBarItems()
        {
            ToolBarItems = new List<MenuItem>
            {
                new MenuItem("צור רשימת קניות", PackIconKind.ListStatus, _stateMachine.CreateCommand(Triggers.HomeSelected)),
                new MenuItem("", PackIconKind.CashCheck, _stateMachine.CreateCommand(Triggers.HomeSelected)),
            };

        }

        private void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName="")
        {
            _notifyPropertyChanged.SetProperty(ref property, value, propertyName);
        }

        private void OnPropertyChanged(PropertyChangedEventArgs property)
        {
            PropertyChanged?.Invoke(this, property);
        }
    }
}
