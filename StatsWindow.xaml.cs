using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace monhdk4
{
    /// <summary>
    /// Interaction logic for StatsWindow.xaml
    /// </summary>
    public partial class StatsWindow : Window
    {
        public static void Open(Window owner, Group grp)
        {
            Window win = new StatsWindow()
            {
                Owner = owner,
                DataContext = grp,
            };
            win.ShowDialog();
        }

        public StatsWindow()
        {
            InitializeComponent();
        }
    }
}
