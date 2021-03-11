using HomeEconomicSystem.PL.Extensions;
using HomeEconomicSystem.PL.ViewModel.Catalog;
using HomeEconomicSystem.Utils;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using TriggersPC = HomeEconomicSystem.PL.ViewModel.Catalog.CatalogTriggers;

namespace HomeEconomicSystem.PL.ViewModel.PageDisplay
{
    class CatalogPageDisplay : INotifyPropertyChanged, IPageDisplay
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private NotifyProperyChanged _notifyPropertyChanged;
        public IReadOnlyList<MenuItem> MenuItems { get; }

        public UserControl Content { get; }

        public bool HasItems => MenuItems is not null && MenuItems.Count > 0;

        CatalogStateMachine _stateMachine;

        private string _state;
        public string State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        public string Title => "קטלוג";

        private bool _showMessage;
        public bool ShowMessage { get => _showMessage; set => SetProperty(ref _showMessage, value); }


        private string _messageToShow;
        public string MessageToShow { get => _messageToShow; set => SetProperty(ref _messageToShow, value); }

        public CatalogPageDisplay()
        {
            _notifyPropertyChanged = new NotifyProperyChanged(this, (property) => OnPropertyChanged(property));
            CatalogVM catalogVM = new CatalogVM();
            Content = new View.CatalogView();
            Content.DataContext = catalogVM;
            _stateMachine = catalogVM.CatalogStateMachine;
            _stateMachine.OnTransitionCompleted(t => State = t.Destination.ToString());

            MenuItems = new List<MenuItem>
            {
                new MenuItem("קטלוג מוצרים", PackIconKind.ClipboardList, _stateMachine.CreateCommand(TriggersPC.ProductCatalogSelected)),
                new MenuItem("קטלוג קטגוריות", PackIconKind.Category, _stateMachine.CreateCommand(TriggersPC.CategoryCatalogSelected)),
            };
        }
        private void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
        {
            _notifyPropertyChanged.SetProperty(ref property, value, propertyName);
        }

        private void OnPropertyChanged(PropertyChangedEventArgs property)
        {
            PropertyChanged?.Invoke(this, property);
        }
    }
}
