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
using OrderManagementSystem.Cache;
using OrderManagementSystem.UIComponents.Classes;

//using OrderManagementSystem.Classes;
using OrderManagementSystem.UIComponents.ViewModels;


namespace OrderManagementSystem.UIComponents.Views
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : ThemedWindow
    {
        public Action CloseWindow { get; set; }
     
        public AuthWindow()
        {
            InitializeComponent();
            //GUIHandler.Instance.Init(m_CacheManager, m_Connection, m_MessageProcessor);
            MessageProcessor.SendMessage(Enums.MessageType.User, Enums.MessageAction.Load, null);
            MessageProcessor.SendMessage(Enums.MessageType.Category, Enums.MessageAction.Load, null);
            MessageProcessor.SendMessage(Enums.MessageType.Product, Enums.MessageAction.Load, null);
            MessageProcessor.SendMessage(Enums.MessageType.Order, Enums.MessageAction.Load, null);


            // Set the DataContext of the window to the ViewModel
            AuthViewModel authViewModel = new AuthViewModel();
            authViewModel.CloseWindow = this.Close;
            this.DataContext = authViewModel;
        }

        
    }
}
