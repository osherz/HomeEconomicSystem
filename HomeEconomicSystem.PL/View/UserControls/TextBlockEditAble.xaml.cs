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
    /// Interaction logic for EditAbleTextBox.xaml
    /// </summary>
    public partial class TextBlockEditAble : UserControl
    {


        public IEnumerable<IName> Options
        {
            get { return (IEnumerable<IName>)GetValue(OptionsProperty); }
            set { SetValue(OptionsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Options.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OptionsProperty =
            DependencyProperty.Register("Options", typeof(IEnumerable<IName>), typeof(TextBlockEditAble), new PropertyMetadata(null, OnOptionsChanged));

        public bool ShowComboBox
        {
            get { return (bool)GetValue(ShowComboBoxProperty); }
            private set { SetValue(ShowComboBoxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowComboBox.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowComboBoxProperty =
            DependencyProperty.Register("ShowComboBox", typeof(bool), typeof(TextBlockEditAble), new PropertyMetadata(false));


        public IName SelectedItem
        {
            get { return (IName)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(IName), typeof(TextBlockEditAble), new PropertyMetadata(null, OnSelectedItemChanged));


        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextBlockEditAble), new PropertyMetadata(""));



        public bool EditMode
        {
            get { return (bool)GetValue(EditModeProperty); }
            set { SetValue(EditModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditModeProperty =
            DependencyProperty.Register("EditMode", typeof(bool), typeof(TextBlockEditAble), new PropertyMetadata(false));


        public TextBlockEditAble()
        {
            InitializeComponent();
        }


        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is not null)
            {
                (d as TextBlockEditAble).Text = (e.NewValue as IName).Name;
            }
        }

        private static void OnOptionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as TextBlockEditAble).ShowComboBox = e.NewValue is not null;
        }

    }
}
