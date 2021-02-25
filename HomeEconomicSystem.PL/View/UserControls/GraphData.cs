using HomeEconomicSystem.BE;
using HomeEconomicSystem.PL.ViewModel.DataAnalysis;
using LiveCharts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HomeEconomicSystem.PL.View.UserControls
{
    public class GraphData : DependencyObject
    {


        public BasicGraph BasicGraph
        {
            get { return (BasicGraph)GetValue(BasicGraphProperty); }
            set { SetValue(BasicGraphProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BasicGraph.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BasicGraphProperty =
            DependencyProperty.Register("BasicGraph", typeof(BasicGraph), typeof(GraphData), new PropertyMetadata(null));

        public int Id => BasicGraph.Id;
        public string Title => BasicGraph.Title;
        public string Description => BasicGraph.Description;
        public bool IsTable => BasicGraph.GraphType == GraphType.Table;
        public bool IsLine => BasicGraph.GraphType == GraphType.Linear;
        public bool IsColumn => BasicGraph.GraphType == GraphType.Bar;
        public bool IsPie => BasicGraph.GraphType == GraphType.Pie;

        public TimeType TitleX => BasicGraph.AggregationTimeType;
        public AmountOrCost TitleY => BasicGraph.AmountOrCost;
        public SeriesCollection LineSeriesCollection { get; set; }
        public SeriesCollection ColumnSeriesCollection { get; set; }

        private SeriesCollection _pieSeriesCollection;
        public SeriesCollection PieSeriesCollection
        {
            get => _pieSeriesCollection;
            set
            {
                _pieSeriesCollection = value;
                PieDropDownViewModel = new PieDropDownViewModel(PieSeriesCollection, TitleX.ToString(), TitleY.ToString());
            }
        }
        public PieDropDownViewModel PieDropDownViewModel { get; private set; }
        public Func<double, string> YFormatter { get; set; }
        public string[] Labels { get; set; }

        /// <summary>
        /// The IEnumerable should be of {X="?", Y="?"} 
        /// </summary>
        public IEnumerable<KeyValuePair<string, IEnumerable>> ItemsDataForTable  { get;set;}

    }
}
