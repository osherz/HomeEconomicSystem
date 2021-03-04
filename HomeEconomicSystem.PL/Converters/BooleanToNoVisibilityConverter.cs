using MaterialDesignThemes.Wpf.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HomeEconomicSystem.PL.Converters
{
    public class BooleanToNoVisibilityConverter : BooleanConverter<Visibility>
    {
        public BooleanToNoVisibilityConverter() : base(Visibility.Collapsed, Visibility.Visible)
        {

        }
    }
}
