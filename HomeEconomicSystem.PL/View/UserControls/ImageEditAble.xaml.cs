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
    /// Interaction logic for ImageEditAble.xaml
    /// </summary>
    public partial class ImageEditAble : UserControl
    {


        public bool EditMode
        {
            get { return (bool)GetValue(EditModeProperty); }
            set { SetValue(EditModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditModeProperty =
            DependencyProperty.Register("EditMode", typeof(bool), typeof(ImageEditAble), new PropertyMetadata(false));



        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(ImageEditAble), new PropertyMetadata(null));



        public ImageEditAble()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (EditMode)
            {
                var dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
                if (dialog.ShowDialog() == true)
                {
                    Source = new BitmapImage(new Uri(dialog.FileName));
                }
            }
        }
    }
}
