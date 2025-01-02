using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using OrderManagementSystem.UIComponents.ViewModels;
//using OrderManagementSystem.UIComponents.UViewModels;


namespace OrderManagementSystem.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for AddCategoryView.xaml
    /// </summary>
    public partial class AddCategoryView : ThemedWindow
    {
        public AddCategoryView()
        {
            InitializeComponent();

            CategoryViewModel categoryViewModel = new CategoryViewModel();
            categoryViewModel.CloseWindow = this.Close;
            this.DataContext = categoryViewModel;

        }

    }
}
