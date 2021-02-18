﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace HomeEconomicSystem.PL.ViewModel
{
    public class MainWindowVM : NotifyPropertyChanged
    {
        private Dictionary<States, Action> _stateActionDict;
        private StateMachine _stateMachine;

        public MainMenuVM MainMenuVM { get; private set; }

        private IPageDisplay _pageDisplay;
        public IPageDisplay PageDisplay
        {
            get => _pageDisplay;
            set => SetProperty(ref _pageDisplay, value);
        }

        public MainWindowVM()
        {
            IPageDisplay homePageDisplay = new HomePageDisplay();
            IPageDisplay dataAnalysisDisplay = new DataAnalysisPageDisplay(_stateMachine);
            IPageDisplay productCatalogDisplay = new ProductCatalogPageDisplay();
            IPageDisplay transactionHistoryDisplay = new TransactionHistoryPageDisplay();

            _stateActionDict = new Dictionary<States, Action>
            {
                {States.Home, ()=>PageDisplay=homePageDisplay },
                {States.DataAnalysis, ()=>PageDisplay=dataAnalysisDisplay },
                {States.ProductCatalog, ()=>PageDisplay=productCatalogDisplay },
                {States.TransactionHistory, ()=>PageDisplay=transactionHistoryDisplay }
            };
            _stateMachine = new StateMachine(_stateActionDict);

            MainMenuVM = new MainMenuVM(_stateMachine);
        }
    }
}
