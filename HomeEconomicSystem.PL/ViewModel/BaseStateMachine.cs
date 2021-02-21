using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.ViewModel
{
    public abstract class BaseStateMachine<TState, TTrigger> : Stateless.StateMachine<TState, TTrigger>
    {
        IReadOnlyDictionary<TState, Action> _stateActionDict;

        protected Action GetStateAction(TState state)
        {
            if(_stateActionDict.ContainsKey(state))
            {
                return _stateActionDict[state];
            }
            return () => { };
        }

        public BaseStateMachine(TState initialState, IReadOnlyDictionary<TState, Action> stateActionDict) : base(initialState)
        {
            _stateActionDict = stateActionDict;
        }
    }
}
