using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace monhdk4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Filter filter;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new Group();

            filter = new Filter();
        }

        private void OnShowFilter(object sender, RoutedEventArgs _)
        {
            CollectionViewSource src = (CollectionViewSource)Resources["entries"];

            bool ok = FilterWindow.Open(this, ref filter);
            src.View.Filter = null;
            if (ok)
            {
                src.View.Filter = filter.Keep;
            }
        }

        private void OnShowStats(object sender, RoutedEventArgs _)
        {
            StatsWindow.Open(this, (Group)DataContext);
        }

        private void OnLoad(object sender, RoutedEventArgs _)
        {
            Connect();
        }

        private void OnExit(object sender, RoutedEventArgs e)
        {
            Disconnect();
            Application.Current.Shutdown();
        }

        private void OnConnect(object sender, RoutedEventArgs _)
        {
            Connect();
        }

        private void OnDisconnect(object sender, RoutedEventArgs _)
        {
            Disconnect();
        }

        private void Disconnect()
        {
            ((Group)DataContext)?.Stop();
        }

        private void Connect()
        {
            
            Group grp = ConnectionWindow.Open(this);
            if (grp == null)
            {
                return;
            }
            DataContext = grp;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftAlt || e.Key == Key.RightAlt)
            {
                if (winMenu.IsVisible)
                {
                    winMenu.Visibility = Visibility.Collapsed;
                }
                else
                {
                    winMenu.Visibility = Visibility.Visible;
                }
            }
        }
    }

    public class BoolToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b && b)
            {
                return Visibility.Visible;
            }
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
