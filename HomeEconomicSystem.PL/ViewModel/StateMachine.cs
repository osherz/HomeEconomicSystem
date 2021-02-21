using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HomeEconomicSystem.PL.ViewModel
{

    public class StateMachine: BaseStateMachine<States, Triggers>
    {
        //TODO: to check if need to unable the option to permit to themself.
        public StateMachine(IReadOnlyDictionary<States, Action> stateActionDict) : base (States.Start, stateActionDict)
        {
            ConfigureStart();
            ConfigureHome();
            ConfigureDataAnalysis();
            ConfigureProductCatalog();
            ConfigureTransactionHistory();

            OnTransitioned
          (
            (t) =>
            {
                CommandManager.InvalidateRequerySuggested();
            }
        );

            //used to debug commands and UI components
            OnTransitioned
              (
                (t) => Debug.WriteLine
                  (
                    "State Machine transitioned from {0} -> {1} [{2}]",
                    t.Source, t.Destination, t.Trigger
                  )
              );
        }

        private void ConfigureStart()
        {
            Configure(States.Start)
                .Permit(Triggers.HomeSelected, States.Home)
                .Permit(Triggers.DataAnalysisSelected, States.DataAnalysis)
                .Permit(Triggers.ProductCatalogSelected, States.ProductCatalog)
                .Permit(Triggers.TransactionHistorySelected, States.TransactionHistory);
        }

        private void ConfigureHome()
        {
            Configure(States.Home)
                .SubstateOf(States.Start)
                .OnEntry(GetStateAction(States.Home));
        }

        private void ConfigureDataAnalysis()
        {
            Configure(States.DataAnalysis)
                .SubstateOf(States.Start)
                .OnEntry(GetStateAction(States.DataAnalysis));

        }

        private void ConfigureProductCatalog()
        {
            Configure(States.ProductCatalog)
                            .SubstateOf(States.Start)
                            .OnEntry(GetStateAction(States.ProductCatalog));
        }

        private void ConfigureTransactionHistory()
        {
            Configure(States.TransactionHistory)
                            .SubstateOf(States.Start)
                            .OnEntry(GetStateAction(States.TransactionHistory));
        }

    }

}
