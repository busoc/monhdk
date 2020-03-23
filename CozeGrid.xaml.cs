using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace monhdk4
{
    /// <summary>
    /// Interaction logic for CozeGrid.xaml
    /// </summary>
    public partial class CozeGrid : UserControl
    {

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            "Items",
            typeof(ObservableCollection<Coze>),
            typeof(CozeGrid),
            new PropertyMetadata()
        );

        public static readonly DependencyProperty ConvertProperty = DependencyProperty.Register(
            "Convert",
            typeof(IValueConverter),
            typeof(CozeGrid),
            new PropertyMetadata()
        );

        public ObservableCollection<Coze> Items
        {
            get => (ObservableCollection<Coze>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public IValueConverter Convert
        {
            get => (IValueConverter)GetValue(ConvertProperty);
            set => SetValue(ConvertProperty, value);
        }


        public CozeGrid()
        {
            InitializeComponent();
            root.DataContext = this;
        }
    }
}
