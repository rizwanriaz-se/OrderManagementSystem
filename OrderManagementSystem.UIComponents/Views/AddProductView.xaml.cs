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
using OrderManagementSystem.UIComponents.ViewModels;


namespace OrderManagementSystem.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for AddProductView.xaml
    /// </summary>
    public partial class AddProductView : ThemedWindow
    {
        public AddProductView()
        {
            InitializeComponent();
            ProductViewModel productViewModel = new ProductViewModel();
            productViewModel.CloseWindow = this.Close;
            this.DataContext = productViewModel;
        }
    }
}
