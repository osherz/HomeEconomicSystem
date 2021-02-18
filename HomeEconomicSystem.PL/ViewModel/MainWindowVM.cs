using System;
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
            UserControl userControl = new UserControl();
            userControl.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));

            IPageDisplay homePageDisplay = new PageDisplay(userControl, null);

            _stateActionDict = new Dictionary<States, Action>
            {
                {States.Home, ()=>PageDisplay=homePageDisplay },
                {States.DataAnalysis, ()=>PageDisplay=homePageDisplay },
                {States.ProductCatalog, ()=>PageDisplay=homePageDisplay },
                {States.TransactionHistory, ()=>PageDisplay=homePageDisplay }
            };
            _stateMachine = new StateMachine(_stateActionDict);

            MainMenuVM = new MainMenuVM(_stateMachine);
        }
    }
}
