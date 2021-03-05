using HomeEconomicSystem.BE;
using HomeEconomicSystem.PL.ViewModel.Transactions;
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

namespace HomeEconomicSystem.PL.View
{
    /// <summary>
    /// Interaction logic for CreateTransactionView.xaml
    /// </summary>
    public partial class CreateTransactionView : UserControl
    {
        public CreateTransactionView()
        {
            InitializeComponent();
        }

        private void ProductsTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(e.NewValue is Product product)
            {
                (DataContext as CreateTransactionVM).SelectedProductTransaction.Product = product;
            }
        }
    }
}
