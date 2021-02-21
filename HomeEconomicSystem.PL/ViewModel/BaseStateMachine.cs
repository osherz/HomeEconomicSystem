using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.ViewModel
{
    public abstract class BaseStateMachine<TState, TTrigger> : Stateless.StateMachine<TState, TTrigger>
    {
        protected abstract IReadOnlyDictionary<States, Action> StateActionDict {get;}

        protected Action GetStateAction(States state)
        {
            if(StateActionDict.ContainsKey(state))
            {
                return StateActionDict[state];
            }
            return () => { };
        }

        public BaseStateMachine(TState initialState) : base(initialState)
        {

        }
    }
}
