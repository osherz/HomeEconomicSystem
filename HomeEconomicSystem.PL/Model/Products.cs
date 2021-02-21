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
            ProductsList = new ObservableCollection<Product>();
        }

        void Filter(string name = "", params Category[] categories)
        {
            ProductsList.Clear();
            _dataMenegement.GetProducts(name, categories).ForEach(p => ProductsList.Add(p));
        }
    }
}
