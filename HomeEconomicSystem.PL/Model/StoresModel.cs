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
    public class StoresModel
    {
        IDataManagement _dataMenegement;
        public ObservableCollection<Store> StoresList { get; private set; }
        public StoresModel()
        {
            _dataMenegement = new BL.BL().DataManagement;
            StoresList = new ObservableCollection<Store>();
            Filter();
        }

        void Filter(string name = "")
        {
            StoresList.Clear();
            _dataMenegement.GetStores(name).ForEach(c => StoresList.Add(c));
        }
    }
}