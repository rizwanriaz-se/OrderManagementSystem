using DevExpress.Xpf.Core;
using OrderManagementSystem.Cache;
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

namespace OrderManagementSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ThemedWindow
    {
        private CacheManager m_CacheManager = new CacheManager();

        public MainWindow()
        {
            InitializeComponent();
            GUIHandler.GetInstance().Init(m_CacheManager);

        }

        private void ThemedWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GUIHandler.GetInstance().CacheManager.SaveData();
        }
    }
}
