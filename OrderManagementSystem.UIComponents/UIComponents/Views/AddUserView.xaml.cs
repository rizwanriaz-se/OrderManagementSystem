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
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using OrderManagementSystem.UIComponents.UIComponents.ViewModels;
//using OrderManagementSystem.ViewModels;


namespace OrderManagementSystem.UIComponents.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for AddUserView.xaml
    /// </summary>
    public partial class AddUserView : ThemedWindow
    {
        public AddUserView()
        {
            InitializeComponent();

            UserViewModel userViewModel = new UserViewModel();
            userViewModel.CloseWindow = this.Close;
            this.DataContext = userViewModel;
        }
    }
}
