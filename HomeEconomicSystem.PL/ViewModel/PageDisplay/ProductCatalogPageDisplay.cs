using HomeEconomicSystem.PL.Extensions;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HomeEconomicSystem.PL.ViewModel.PageDisplay
{
    class ProductCatalogPageDisplay : IPageDisplay
    {
        

        public IReadOnlyList<MenuItem> MenuItems { get; }

        public UserControl Content { get; }

        public bool HasItems => MenuItems is not null && MenuItems.Count > 0;

        public ProductCatalogPageDisplay(StateMachine stateMachine)
        {
            Content = new View.ProductCatalogView();
            Content.DataContext = new ProductCatalogVM();
            MenuItems = new List<MenuItem>
            {
                new MenuItem("קטלוג מוצרים", PackIconKind.ClipboardList, stateMachine.CreateCommand(Triggers.ProductCatalogSelected)),
                new MenuItem("קטלוג קטגוריות", PackIconKind.Category, stateMachine.CreateCommand(Triggers.ProductCatalogSelected)),
            };
        }
    }
}
