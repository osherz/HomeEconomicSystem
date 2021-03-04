using HomeEconomicSystem.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HomeEconomicSystem.PL.ViewModel
{
    public abstract class VMBasic<TState, TTrigger> : INotifyPropertyChanged, IVM
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private NotifyProperyChanged _notifyPropertyChanged;
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
            _notifyPropertyChanged = new NotifyProperyChanged(this, (property) => OnPropertyChanged(property));
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
                statesEntryAction = UnionToDictionary(statesEntryAction, vm.Value.GetStatesEntryAction());
                statesExitAction = UnionToDictionary(statesExitAction, vm.Value.GetStatesExitAction());
            }

            StateMachine = CreateStateMachine(statesEntryAction, statesExitAction);
            StateMachine.OnTransitionCompleted(e => OnStateChanged(e.Destination.ToString()));

            // Set StateMachine to vms
            foreach (var vm in _innerVMs)
            {
                vm.Value.SetStateMachine(StateMachine);
            }
        }

        private Dictionary<TState, Action> UnionToDictionary(params IReadOnlyDictionary<TState, Action>[] dicts)
        {
            return dicts
                .SelectMany(item=>item)
                .GroupBy(item => item.Key, item => item.Value)
                .ToDictionary(
                    group => group.Key, 
                    group => new Action(() => group
                                                .ToList()
                                                .ForEach(f => f())));
        }

        protected virtual void OnStateChanged(string state)
        {
            InnerStateChanged?.Invoke(this, state);
        }

        protected void SetProperty<T>(ref T property, T value)
        {
            _notifyPropertyChanged.SetProperty(ref property, value);
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs property)
        {
            PropertyChanged?.Invoke(this, property);
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
