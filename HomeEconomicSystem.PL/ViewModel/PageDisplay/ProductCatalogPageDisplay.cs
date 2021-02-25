using HomeEconomicSystem.PL.Extensions;
using HomeEconomicSystem.PL.ViewModel.ProductCatalog;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using TriggersPC = HomeEconomicSystem.PL.ViewModel.ProductCatalog.Triggers;

namespace HomeEconomicSystem.PL.ViewModel.PageDisplay
{
    class ProductCatalogPageDisplay : NotifyPropertyChanged, IPageDisplay
    {
       
        public IReadOnlyList<MenuItem> MenuItems { get; }

        public UserControl Content { get; }

        public bool HasItems => MenuItems is not null && MenuItems.Count > 0;

        ProductCatalogStateMachine _stateMachine;

        private string _state;
        public string State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        
        public ProductCatalogPageDisplay()
        {
            ProductCatalogVM productCatalogVM = new ProductCatalogVM();
            Content = new View.ProductCatalogView();
            Content.DataContext = productCatalogVM;
            _stateMachine = productCatalogVM.ProductCatalogStateMachine;
            _stateMachine.OnTransitionCompleted(t => State = t.Destination.ToString());

            MenuItems = new List<MenuItem>
            {
                new MenuItem("קטלוג מוצרים", PackIconKind.ClipboardList, _stateMachine.CreateCommand(TriggersPC.ProductCatalogSelected)),
                new MenuItem("קטלוג קטגוריות", PackIconKind.Category, _stateMachine.CreateCommand(TriggersPC.CategoryCatalogSelected)),
            };
        }
    }
}
