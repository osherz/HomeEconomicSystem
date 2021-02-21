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
    public class Categories
    {
        IDataManagement _dataMenegement;
        public ObservableCollection<Category> CategoriesList { get; private set; }
        public Categories()
        {
            _dataMenegement = new BL.BL().DataManagement;
            CategoriesList = _dataMenegement.GetCategories().ToObservableCollection();
        }

        void Filter(string name = "")
        {
            CategoriesList = _dataMenegement.GetCategories(name).ToObservableCollection();
        }
    }
}
