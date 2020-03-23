using System;
using System.Windows;

namespace monhdk4
{
    /// <summary>
    /// Interaction logic for FilterWindow.xaml
    /// </summary>
    public partial class FilterWindow : Window
    {

        public static bool Open(Window owner, ref Filter obj)
        {
            Window win = new FilterWindow()
            {
                DataContext = obj,
                Owner = owner,
            };
            win.ShowDialog();
            return win.DialogResult == true ? true : false;
        }

        public FilterWindow()
        {
            InitializeComponent();
        }

        private void OnApply(object sender, RoutedEventArgs _)
        {
            DialogResult = true;
        }
    }
}
