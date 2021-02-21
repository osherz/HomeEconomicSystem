using HomeEconomicSystem.BE;
using HomeEconomicSystem.BL;
using HomeEconomicSystem.PL.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.Model
{
    public class Products
    {
        IDataManagement _dataMenegement ;

        public ObservableCollection<Product> ProductsList { get; private set; }
        public Products()
        {
            _dataMenegement = new BL.BL().DataManagement;
            ProductsList = _dataMenegement.GetProducts().ToObservableCollection();
        }

        void Filter(string name = "", params Category[] categories)
        {
            ProductsList = _dataMenegement.GetProducts(name, categories).ToObservableCollection();
        }
    }
}
