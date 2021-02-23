using HomeEconomicSystem.BE;
using HomeEconomicSystem.BL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HomeEconomicSystem.PL.Converters
{
    public class BasicGraphToGraphDataFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IGraphManagement graphManagement = new BL.BL().GraphManagement;
            CategoryGraph graph = null;
            if ((graph = value as CategoryGraph) is not null)
            {
                graphManagement.AnalyzeGraph(graph);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
