﻿using System;
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
using OrderManagementSystemServer.Repository;
using OrderManagementSystem.UIComponents.ViewModels;


namespace OrderManagementSystem.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for AddOrderView.xaml
    /// </summary>
    public partial class AddOrderView : ThemedWindow
    {
        public AddOrderView()
        {
            InitializeComponent();

            AddOrderViewModel addOrderViewModel = new AddOrderViewModel();
            addOrderViewModel.CloseWindow = this.Close;
            
            this.DataContext = addOrderViewModel;
            


        }
    }
}
