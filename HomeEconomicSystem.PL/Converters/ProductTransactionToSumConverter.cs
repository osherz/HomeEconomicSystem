using HomeEconomicSystem.BE;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HomeEconomicSystem.PL.Converters
{
    public class ProductTransactionToSumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ProductTransaction)
            {
                ProductTransaction productTransaction = value as ProductTransaction;
                return Math.Round(productTransaction.Amount * productTransaction.UnitPrice, 2).ToString("C");
            }
            else throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
