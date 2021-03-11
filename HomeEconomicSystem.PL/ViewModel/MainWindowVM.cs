using HomeEconomicSystem.BE;
using HomeEconomicSystem.PL.Command;
using HomeEconomicSystem.PL.Extensions;
using HomeEconomicSystem.PL.Model;
using HomeEconomicSystem.PL.ViewModel.PageDisplay;
using HomeEconomicSystem.Utils;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

            var homePageDisplay = new HomePageDisplay();
            var dataAnalysisDisplay = new DataAnalysisPageDisplay();
            var productCatalogDisplay = new CatalogPageDisplay();
            var transactionHistoryDisplay = new TransactionHistoryPageDisplay(_stateMachine);
            var transactionCreationDisplay = new TransactionCreationPageDisplay();

            _stateActionDict = new Dictionary<States, Action>
            {
                {States.Home, ()=>PageDisplay=new HomePageDisplay() },
                {States.DataAnalysis, ()=>PageDisplay=new DataAnalysisPageDisplay() },
                {States.ProductCatalog, ()=>PageDisplay=new CatalogPageDisplay() },
                {States.TransactionHistory, ()=>PageDisplay=new TransactionHistoryPageDisplay(_stateMachine) },
                {States.TransactionCreation,
                    ()=> {
                        transactionCreationDisplay.GenerateTransaction();
                        PageDisplay=transactionCreationDisplay;
                    }
                },
                {
                    States.CreatingList, () =>
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "Pdf File |*.pdf";
                        saveFileDialog.FileName = "Shop List";
                        if (saveFileDialog.ShowDialog() == true)
                        {
                            new AssociationsModel().CreateShopingListRecommendation(saveFileDialog.FileName);
                        }
                    }
                }
            };
            _stateMachine = new StateMachine(_stateActionDict);

            MainMenuVM = new MainMenuVM(_stateMachine);
            InitToolBarItems();

            _stateMachine.Fire(Triggers.HomeSelected);
        }

        private void InitToolBarItems()
        {
            ToolBarItems = new List<MenuItem>
            {
                new MenuItem("הצע לי רשימת קניות", PackIconKind.ListStatus, _stateMachine.CreateCommand(Triggers.CreatePurchaseList)),
                new MenuItem("צור עסקה חדשה", PackIconKind.TagPlus, _stateMachine.CreateCommand(Triggers.CreateTransaction)),
            };

        }

        private void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
        {
            _notifyPropertyChanged.SetProperty(ref property, value, propertyName);
        }

        private void OnPropertyChanged(PropertyChangedEventArgs property)
        {
            PropertyChanged?.Invoke(this, property);
        }

        public void CloseMessage()
        {
            PageDisplay.ShowMessage = false;
            PageDisplay.MessageToShow = "";
        }

    }
}
