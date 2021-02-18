using HomeEconomicSystem.PL.Command;
using HomeEconomicSystem.PL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HomeEconomicSystem.PL.Extensions
{
    public static class StateMachineExtensions
    {
        public static ICommand CreateCommand<TState, TTrigger>(this Stateless.StateMachine<TState, TTrigger> stateMachine, TTrigger trigger)
        {
            return new RelayCommand
              (
                () => stateMachine.Fire(trigger),
                () => stateMachine.CanFire(trigger)
              );
        }
    }
}
