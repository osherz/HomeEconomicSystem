using HomeEconomicSystem.BE;
using LiveCharts;
using System;
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

        public string Title => BasicGraph.Title;
        public string Description => BasicGraph.Description;

        public TimeType TitleX => BasicGraph.AggregationTimeType;
        public AmountOrCost TitleY => BasicGraph.AmountOrCost; 
        public SeriesCollection SeriesCollection{ get; set; }
        public Func<double, string> YFormatter { get; set; }
        public string[] Labels { get; set; }
    }
}
