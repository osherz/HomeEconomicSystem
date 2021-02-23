using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HomeEconomicSystem.PL.View.UserControls
{
    /// <summary>
    /// Interaction logic for DateRangeUC.xaml
    /// </summary>
    public partial class DateRangeUC : UserControl
    {
        public DatePicker StartDatePicker => _startDatePicker;

        public DateTime? StartDate
        {
            get { return (DateTime?)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartDateProperty =
            DependencyProperty.Register("StartDate", typeof(DateTime?), typeof(DateRangeUC), new PropertyMetadata(DateTime.Now, SelectedDateChanged));


        public DateTime? EndDate
        {
            get { return (DateTime?)GetValue(EndDateProperty); }
            set { SetValue(EndDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EndDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndDateProperty =
            DependencyProperty.Register("EndDate", typeof(DateTime?), typeof(DateRangeUC), new PropertyMetadata(DateTime.Now, SelectedDateChanged));

        public bool IsNotValidRange
        {
            get { return (bool)GetValue(IsNotValidRangeProperty); }
            private set { SetValue(IsNotValidRangeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsValidRange.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsNotValidRangeProperty =
            DependencyProperty.Register("IsNotValidRange", typeof(bool), typeof(DateRangeUC), new PropertyMetadata(true));



        public DateRangeUC()
        {
            InitializeComponent();
            IsNotValidRange = !IsValidRange();
        }

        private static void SelectedDateChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DateRangeUC dateRangeUC = ((DateRangeUC)sender);
            dateRangeUC.IsNotValidRange = !dateRangeUC.IsValidRange();
        }

        public bool IsValidRange()
        {
            return EndDate.Value.Date >= StartDate.Value.Date;
        }
    }
}
