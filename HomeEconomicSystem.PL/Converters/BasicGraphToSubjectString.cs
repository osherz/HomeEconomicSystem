using HomeEconomicSystem.BE;
using HomeEconomicSystem.PL.ViewModel.DataAnalysis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HomeEconomicSystem.PL.Converters
{
    public class BasicGraphToSubjectString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is CategoryGraph) return "קטגוריות";
            if (value is ProductGraph) return "מוצרים";
            if (value is StoreGraph) return "חנויות";
            if (value is TransactionsGraph) return "עסקאות";

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
