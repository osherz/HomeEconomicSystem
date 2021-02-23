using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeEconomicSystem.PL.View.UserControls
{
    public class GraphData
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public SeriesCollection SeriesCollection{ get; set; }
        public Func<double, string> YFormatter { get; set; }
        public string[] Labels { get; set; }
    }
}
