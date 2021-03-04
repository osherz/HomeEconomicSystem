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
    /// Interaction logic for EditAbleTextBox.xaml
    /// </summary>
    public partial class EditAbleTextBox : UserControl
    {


        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(EditAbleTextBox), new PropertyMetadata(""));



        public bool EditMode
        {
            get { return (bool)GetValue(EditModeProperty); }
            set { SetValue(EditModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditModeProperty =
            DependencyProperty.Register("EditMode", typeof(bool), typeof(EditAbleTextBox), new PropertyMetadata(false));


        public EditAbleTextBox()
        {
            InitializeComponent();
        }
    }
}
