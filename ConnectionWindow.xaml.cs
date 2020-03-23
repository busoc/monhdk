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
    /// Interaction logic for ConnectionWindow.xaml
    /// </summary>
    public partial class ConnectionWindow : Window
    {
        public static Group Open(Window owner)
        {
            var grp = Group.Default();
            var win = new ConnectionWindow()
            {
                DataContext = grp,
                Owner = owner,
            };
            win.ShowDialog();
            if (win.DialogResult == true)
            {
                return grp;
            }
            return null;
        }

        public ConnectionWindow()
        {
            InitializeComponent();

            name.Focus();
            name.SelectAll();
        }

        public void Enter(object sender, RoutedEventArgs _)
        {
            if (sender is TextBox box)
            {
                box.SelectAll();
            }
        }

        public void Accept(object sender, RoutedEventArgs _)
        {
            Group grp = (Group)DataContext;
            if (grp.Name.Trim().Length > 0)
            {
                try
                {
                    Group.Save(grp);
                }
                catch(Exception e)
                {
                    MessageBox.Show(this, e.Message, "Fail to save setting", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            try
            {
                grp.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message, "Fail to connect", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DialogResult = true;
        }
    }
}
