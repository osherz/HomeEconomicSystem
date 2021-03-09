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
    public class CategoriesModel
    {
        IDataManagement _dataMenegement;
        public ObservableCollection<Category> CategoriesList { get; private set; }
        public CategoriesModel()
        {
            _dataMenegement = new BL.BL().DataManagement;
            CategoriesList = new ObservableCollection<Category>();
            Filter();
        }

        public void Filter(string name = "")
        {
            CategoriesList.Clear();
            _dataMenegement.GetCategories(name).ForEach(c => CategoriesList.Add(c));
        }

        public void Update(Category category)
        {
            _dataMenegement.EditCategory(category);
        }

        public void AddCategory(Category category)
        {
            _dataMenegement.AddCategory(category);
        }

    }
}
