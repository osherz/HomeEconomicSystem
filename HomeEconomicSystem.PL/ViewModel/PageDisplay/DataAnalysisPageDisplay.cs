using HomeEconomicSystem.PL.Extensions;
using HomeEconomicSystem.PL.ViewModel.DataAnalysis;
using HomeEconomicSystem.Utils;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using TriggersDA = HomeEconomicSystem.PL.ViewModel.DataAnalysis.Triggers;

namespace HomeEconomicSystem.PL.ViewModel.PageDisplay
{
    class DataAnalysisPageDisplay : INotifyPropertyChanged, IPageDisplay
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private NotifyProperyChanged _notifyPropertyChanged;

        BaseStateMachine<DataAnalysis.States, TriggersDA> _stateMachine;
        public IReadOnlyList<MenuItem> MenuItems { get; }
        public bool HasItems => MenuItems is not null && MenuItems.Count > 0;
        public UserControl Content { get; }
        
        private string _state;
        public string State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }


        public DataAnalysisPageDisplay()
        {
            _notifyPropertyChanged = new NotifyProperyChanged(this, (property) => OnPropertyChanged(property));

            Content = new View.DataAnalysisView();
            var VM = new DataAnalysisVM();
            VM.InnerStateChanged += VM_InnerStateChanged;
            Content.DataContext = VM;
            _stateMachine = VM.StateMachine;
            _stateMachine.OnTransitionCompleted(e => VM_InnerStateChanged(this, e.Destination.ToString()));
            MenuItems = new List<MenuItem>
            {
                new MenuItem("שמורים", PackIconKind.StarCircle, _stateMachine.CreateCommand(TriggersDA.FavoriteSelected)),
                new MenuItem("AR", PackIconKind.ChartSankeyVariant, _stateMachine.CreateCommand(TriggersDA.AssociationRulesSelected)),
                new MenuItem("טיוטה", PackIconKind.File, _stateMachine.CreateCommand(TriggersDA.DraftSelected))
            };
        }

        private void VM_InnerStateChanged(object sender, string state)
        {
            State = state;
        }

        private void SetProperty<T>(ref T property, T value)
        {
            _notifyPropertyChanged.SetProperty(ref property, value);
        }

        private void OnPropertyChanged(PropertyChangedEventArgs property)
        {
            PropertyChanged?.Invoke(this, property);
        }
    }
}
