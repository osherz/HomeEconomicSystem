using Stateless;
using System;
using System.Collections.Generic;

namespace HomeEconomicSystem.PL.ViewModel.DataAnalysis
{
    internal class AssociationRulesVM : IInnerVM<States, Triggers>
    {
        public event EventHandler<string> InnerStateChanged;

        public IReadOnlyDictionary<States, Action> GetStatesEntryAction()
        {
            return new Dictionary<States, Action>();
        }

        public IReadOnlyDictionary<States, Action> GetStatesExitAction()
        {
            return new Dictionary<States, Action>();
        }

        public void SetStateMachine(StateMachine<States, Triggers> stateMachine)
        {
            
        }
    }
}