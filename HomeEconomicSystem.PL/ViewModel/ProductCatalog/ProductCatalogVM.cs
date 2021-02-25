using HomeEconomicSystem.PL.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HomeEconomicSystem.PL.ViewModel.ProductCatalog
{
    public class ProductCatalogVM : NotifyPropertyChanged
    {
        public ProductCatalogStateMachine productCatalogStateMachine { get;}

        private UserControl _content;
        public UserControl Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        ProductView productView { get; set; }
        CategoryView CategoryView { get; set; }

        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; }
        }



        public ProductCatalogVM()
        {
            ProductVM productVM = new ProductVM();

            var statesEntryAction = new Dictionary<States, Action>{
                {
                    States.ProductCatalog, ()=>
                    {
                        if(productView is null)
                        {
                            productView = new ProductView();
                            productView.DataContext = productVM;
                        }
                        Content = productView;
                    }
                },
                {States.CategoryCatalog, () => Content = new CategoryView() }
            };
            //TODO: Concat to productView state machine dictionary.

            productCatalogStateMachine = new ProductCatalogStateMachine(statesEntryAction);
        }


    }
}
