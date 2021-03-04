using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HomeEconomicSystem.PL.ViewModel
{

    public class StateMachine : BaseStateMachine<States, Triggers>
    {
        //TODO: to check if need to unable the option to permit to themself.
        public StateMachine(IReadOnlyDictionary<States, Action> stateActionDict) : base(States.Start, stateActionDict)
        {
            ConfigureStart();
            ConfigureHome();
            ConfigureDataAnalysis();
            ConfigureProductCatalog();
            ConfigureTransactionHistory();
            ConfigureTransactionCreation();

            OnTransitioned((t) => CommandManager.InvalidateRequerySuggested());

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
            BasicConfigure(States.Start)
                .Permit(Triggers.HomeSelected, States.Home)
                .Permit(Triggers.DataAnalysisSelected, States.DataAnalysis)
                .Permit(Triggers.ProductCatalogSelected, States.ProductCatalog)
                .Permit(Triggers.CreateTransaction, States.TransactionCreation)
                .Permit(Triggers.TransactionHistorySelected, States.TransactionHistory);
        }

        private void ConfigureHome()
        {
            BasicConfigure(States.Home,States.Start);
        }

        private void ConfigureDataAnalysis()
        {
            BasicConfigure(States.DataAnalysis,States.Start);

        }

        private void ConfigureProductCatalog()
        {
            BasicConfigure(States.ProductCatalog,States.Start);
        }

        private void ConfigureTransactionHistory()
        {
            BasicConfigure(States.TransactionHistory,States.Start);
        }

        private void ConfigureTransactionCreation()
        {
            BasicConfigure(States.TransactionCreation, States.Start);
        }

    }

}
