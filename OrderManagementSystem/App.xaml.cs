using DevExpress.Xpf.Core;
using OrderManagementSystem.Cache;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace OrderManagementSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {


        static App()
        {
            CompatibilitySettings.UseLightweightThemes = true;

        }
    }
}
