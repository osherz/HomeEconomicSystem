using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.ViewModel
{
    public interface IInnerVM<TParentStates, TParentTriggers>: IVM
    {
        void SetStateMachine(Stateless.StateMachine<TParentStates, TParentTriggers> stateMachine);

        /// <summary>
        /// Dictinary that decide which action to active when the state-machine enter to a state.
        /// </summary>
        /// <returns></returns>
        IReadOnlyDictionary<TParentStates, Action> GetStatesEntryAction();

        /// <summary>
        /// Dictinary that decide which action to active when the state-machine exit from a state.
        /// </summary>
        /// <returns></returns>
        IReadOnlyDictionary<TParentStates, Action> GetStatesExitAction();
    }
}
