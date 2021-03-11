using HomeEconomicSystem.PL.ViewModel.Home;
using HomeEconomicSystem.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HomeEconomicSystem.PL.ViewModel.PageDisplay
{
    class HomePageDisplay : IPageDisplay, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private NotifyProperyChanged _notifyPropertyChanged;

        public IReadOnlyList<MenuItem> MenuItems { get; }
        public bool HasItems => MenuItems is not null && MenuItems.Count > 0;
        public UserControl Content { get; }

        public string State => "NotImplementedException";

        public string Title => "דף הבית";

        private bool _showMessage;
        public bool ShowMessage { get => _showMessage; set => SetProperty(ref _showMessage, value); }


        private string _messageToShow;
        public string MessageToShow { get => _messageToShow; set => SetProperty(ref _messageToShow, value); }

        public HomePageDisplay()
        {
            _notifyPropertyChanged = new NotifyProperyChanged(this, (property) => OnPropertyChanged(property));

            Content = new View.HomeView();
            Content.DataContext = new HomeVM();
            MenuItems = new List<MenuItem>();
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
