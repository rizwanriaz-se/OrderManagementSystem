using System;
using System.Collections.Generic;
using System.Linq;
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
using OrderManagementSystem.UIComponents.ViewModels;

namespace OrderManagementSystem.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for DisplayOrdersView.xaml
    /// </summary>
    public partial class DisplayOrdersView : UserControl
    {
        public DisplayOrdersView()
        {
            InitializeComponent();

            // Set the data context of the view to the view model
            DataContext = new OrderViewModel();
        }
    }
}
