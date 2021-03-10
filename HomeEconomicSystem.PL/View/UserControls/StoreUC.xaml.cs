using HomeEconomicSystem.BE;
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
    /// Interaction logic for ProductUC.xaml
    /// </summary>
    public partial class StoreUC : UserControl
    {


        public Store Store
        {
            get { return (Store)GetValue(StoreProperty); }
            set { SetValue(StoreProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Store.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StoreProperty =
            DependencyProperty.Register("Store", typeof(Store), typeof(StoreUC), new PropertyMetadata(null));



        public bool EditMode
        {
            get { return (bool)GetValue(EditModeProperty); }
            set { SetValue(EditModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditModeProperty =
            DependencyProperty.Register("EditMode", typeof(bool), typeof(StoreUC), new PropertyMetadata(false));


        public StoreUC()
        {
            InitializeComponent();
        }
    }
}
