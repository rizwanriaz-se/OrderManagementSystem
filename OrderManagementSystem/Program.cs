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

            if (ClientManager.Instance.Connected)
            {
                Application mainApp = new Application();

                ResourceDictionary resourceDictionary = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/OrderManagementSystem.UIComponents;component/Styles/UIStyles.xaml")
                };
                mainApp.Resources.MergedDictionaries.Add(resourceDictionary);

                ClientManager.Instance.LoadData();

                AuthWindow authWindow = new AuthWindow();
                mainApp.MainWindow = authWindow;
                authWindow.Show();
                mainApp.Run();
            }
        }
    }
}
