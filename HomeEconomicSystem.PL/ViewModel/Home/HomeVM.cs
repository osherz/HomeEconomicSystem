using HomeEconomicSystem.BE;
using HomeEconomicSystem.PL.Extensions;
using HomeEconomicSystem.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.ViewModel.Home
{
    public class HomeVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private NotifyProperyChanged _notifyPropertyChanged;

        private TransactionsGraph _mainGraph;
        public TransactionsGraph MainGraph
        {
            get { return _mainGraph; }
            set { SetProperty(ref _mainGraph , value); }
        }

        private CategoryGraph _graph1;
        public CategoryGraph Graph1
        {
            get { return _graph1; }
            set { SetProperty(ref _graph1 , value); }
        }

        private ProductGraph _graph2;
        public ProductGraph Graph2
        {
            get { return _graph2; }
            set { SetProperty(ref _graph2 , value); }
        }


        public HomeVM()
        {
            _notifyPropertyChanged = new NotifyProperyChanged(this, (property) => OnPropertyChanged(property));
            var dataManagement = new BL.BL().DataManagement;

            MainGraph = new TransactionsGraph()
            {
                AggregationTimeType = TimeType.Month,
                AmountOrCost = AmountOrCost.Cost,
                GraphType = GraphType.Linear,
                EndDate = DateTime.Now,
                StartDate = DateTime.Now.AddMonths(-2),
                Title = "הוצאות ב3 החודשים האחרונים"
            };

            Graph1 = new CategoryGraph()
            {
                AggregationTimeType = TimeType.Week,
                AmountOrCost = AmountOrCost.Cost,
                GraphType = GraphType.Pie,
                EndDate = DateTime.Now,
                StartDate = DateTime.Now.AddMonths(-1),
                Title = "קטגוריות לעלות בחודש האחרון",
                Categories = dataManagement.GetCategories().ToObservableCollection(),
            };

            Graph2 = new ProductGraph()
            {
                AggregationTimeType = TimeType.Month,
                AmountOrCost = AmountOrCost.Cost,
                GraphType = GraphType.Bar,
                EndDate = DateTime.Now,
                StartDate = DateTime.Now.AddMonths(-1),
                Title = "מוצרים לעלות בחודש האחרון",
                Products = dataManagement.GetProducts().ToObservableCollection(),
            };
        }

        private void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
        {
            _notifyPropertyChanged.SetProperty(ref property, value, propertyName);
        }

        private void OnPropertyChanged(PropertyChangedEventArgs property)
        {
            PropertyChanged?.Invoke(this, property);
        }
    }
}
