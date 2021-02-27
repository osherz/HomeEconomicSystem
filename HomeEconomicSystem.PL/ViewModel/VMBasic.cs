using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HomeEconomicSystem.PL.ViewModel
{
    public abstract class VMBasic<TState, TTrigger> : NotifyPropertyChanged, IVM
    {
        public BaseStateMachine<TState, TTrigger> StateMachine { get; }

        private UserControl _content;
        public UserControl Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        private Dictionary<TState, IInnerVM<TState, TTrigger>> _innerVMs;
        private Dictionary<TState, Func<UserControl>> _innerViewsCreation;

        public event EventHandler<string> InnerStateChanged;

        public VMBasic()
        {
            _innerVMs = CreateInnerVM();
            _innerViewsCreation = CreateViewsCreation();


            var statesExitAction = new Dictionary<TState, Action>();
            var statesEntryAction = new Dictionary<TState, Action>();
            foreach (var vm in _innerVMs)
            {
                TState state = vm.Key;
                vm.Value.InnerStateChanged += (e, state) => OnStateChanged(state);
                statesEntryAction.Add(state, () =>
                {
                    var view = _innerViewsCreation[state]();
                    view.DataContext = vm.Value;
                    Content = view;
                });
                statesEntryAction = statesEntryAction.Concat(vm.Value.GetStatesEntryAction()).ToDictionary(item => item.Key, item => item.Value);
                statesExitAction = statesExitAction.Concat(vm.Value.GetStatesExitAction()).ToDictionary(item => item.Key, item => item.Value);
            }

            StateMachine = CreateStateMachine(statesEntryAction, statesExitAction);
            StateMachine.OnTransitionCompleted(e => OnStateChanged(e.Destination.ToString()));

            // Set StateMachine to vms
            foreach (var vm in _innerVMs)
            {
                vm.Value.SetStateMachine(StateMachine);
            }
        }

        protected virtual void OnStateChanged(string state)
        {
            InnerStateChanged?.Invoke(this, state);
        }

        /// <summary>
        /// Create state machine and assign to it the entry and exit actions.
        /// </summary>
        /// <param name="statesEntryAction"></param>
        /// <param name="statesExitAction"></param>
        /// <returns></returns>
        protected abstract BaseStateMachine<TState, TTrigger> CreateStateMachine(Dictionary<TState, Action> statesEntryAction, Dictionary<TState, Action> statesExitAction);

        /// <summary>
        /// Create dictionaty that decide which view will created in which state.
        /// The appropriate VM will assign to him using InnerVm dictionary.
        /// </summary>
        /// <returns></returns>
        protected abstract Dictionary<TState, Func<UserControl>> CreateViewsCreation();

        /// <summary>
        /// Create dictionaty that decide which vm belongs to which state.
        /// </summary>
        /// <returns></returns>
        protected abstract Dictionary<TState, IInnerVM<TState, TTrigger>> CreateInnerVM();

    }
}
