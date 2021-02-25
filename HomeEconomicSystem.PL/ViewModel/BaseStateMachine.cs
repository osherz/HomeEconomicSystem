using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.ViewModel
{
    public abstract class BaseStateMachine<TState, TTrigger> : Stateless.StateMachine<TState, TTrigger>
    {
        IReadOnlyDictionary<TState, Action> _stateEntryActionDict;
        IReadOnlyDictionary<TState, Action> _stateExitActionDict;

        protected Action GetStateEntryAction(TState state)
        {
            if (_stateEntryActionDict.ContainsKey(state))
            {
                return _stateEntryActionDict[state];
            }
            return () => { };
        }

        protected Action GetStateExitAction(TState state)
        {
            if (_stateExitActionDict!= null && _stateExitActionDict.ContainsKey(state))
            {
                return _stateExitActionDict[state];
            }
            return () => { };
        }

        /// <summary>
        /// Basic configuration, including OnEntry and OnExit.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public StateConfiguration BasicConfigure(TState state)
        {
            var conf = Configure(state)
                .OnEntry(GetStateEntryAction(state))
                .OnExit(GetStateExitAction(state));
            return conf;
        }

        public StateConfiguration BasicConfigure(TState state, TState subState)
        {
            var conf = Configure(state)
                .SubstateOf(subState)
                .OnEntry(GetStateEntryAction(state))
                .OnExit(GetStateExitAction(state));
            return conf;
        }

        public BaseStateMachine(TState initialState, IReadOnlyDictionary<TState, Action> stateEntryActionDict, IReadOnlyDictionary<TState, Action> stateExitActionDict = null) : base(initialState)
        {
            _stateEntryActionDict = stateEntryActionDict;
            _stateExitActionDict = stateExitActionDict;
        }
    }
}
