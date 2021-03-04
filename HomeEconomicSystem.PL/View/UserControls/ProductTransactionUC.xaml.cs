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
    /// Interaction logic for ProductTransactionUC.xaml
    /// </summary>
    public partial class ProductTransactionUC : UserControl
    {


        public bool EditMode
        {
            get { return (bool)GetValue(EditModeProperty); }
            set { SetValue(EditModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditModeProperty =
            DependencyProperty.Register("EditMode", typeof(bool), typeof(ProductTransactionUC), new PropertyMetadata(false));



        public ProductTransactionUC()
        {
            InitializeComponent();

            DataContext = new ProductTransaction
            {
                Product = new Product
                {
                    BarCode = "1111",
                    Name = "קטשופ",
                    Description = "קטשופ",
                    Category = new Category
                    {
                        Name = "אוכל",
                    }
                },

                UnitPrice = 20.1f,
                Amount = 3,
                Store = new Store
                {
                    Name = "asas"
                },
            };
        }
    }
}
