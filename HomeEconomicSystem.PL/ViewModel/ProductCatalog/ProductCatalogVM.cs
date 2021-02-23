using HomeEconomicSystem.PL.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HomeEconomicSystem.PL.ViewModel.ProductCatalog
{
    class ProductCatalogVM : NotifyPropertyChanged
    {
        public ProductCatalogStateMachine ProductCatalogStateMachine { get; private set; }

        private UserControl _content;
        public UserControl Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public ProductCatalogVM()
        {
            var statesEntryAction = new Dictionary<States, Action>{
                { States.ProductCatalog, ()=> Content = new ProductCatalogView() },
                { States.CategoryCatalog, () => Content = new CategoryCatalogView() }
            };

            ProductCatalogStateMachine = new ProductCatalogStateMachine(statesEntryAction);
        }


    }
}
