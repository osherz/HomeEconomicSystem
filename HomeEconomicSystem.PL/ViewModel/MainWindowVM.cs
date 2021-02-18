using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.ViewModel
{
    public class MainWindowVM
    {
        public MainMenuVM MainMenuVM { get; private set; }
        private Dictionary<States, Action> _stateActionDict;
        private StateMachine _stateMachine;

        public MainWindowVM()
        {
            _stateActionDict = new Dictionary<States, Action>();
            _stateMachine = new StateMachine(_stateActionDict);

            MainMenuVM = new MainMenuVM(_stateMachine);
        }
    }
}
