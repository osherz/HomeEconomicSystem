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
    public class ProductsModel
    {
        IDataManagement _dataMenegement;
        public ObservableCollection<Product> ProductsList { get; private set; }
        public ProductsModel()
        {
            _dataMenegement = new BL.BL().DataManagement;
            ProductsList = new ObservableCollection<Product>();
            Filter();
        }

        void Filter(string name = "")
        {
            ProductsList.Clear();
            _dataMenegement.GetProducts(name).ForEach(c => ProductsList.Add(c));
        }
    }
}