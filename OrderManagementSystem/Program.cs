using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.UIComponents.Views;
using System;
using System.Windows;


namespace OrderManagementSystem
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            App app = new App();
            app.Init();

            // TO-DO: if client manager is connected, then show the auth window
            if (ClientManager.Instance().Connected)
            {
                Application mainApp = new Application();

                // Load the ResourceDictionary from UIStyles.xaml
                var resourceDictionary = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/OrderManagementSystem.UIComponents;component/Styles/UIStyles.xaml")
                };
                mainApp.Resources.MergedDictionaries.Add(resourceDictionary);

                AuthWindow authWindow = new AuthWindow();
                mainApp.MainWindow = authWindow;
                authWindow.Show();
                mainApp.Run();
            }
        }
    }
}
