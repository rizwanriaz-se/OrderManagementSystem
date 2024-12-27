using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.UIComponents.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
//using OrderManagementSystem.Clien
using System.Text;
using System.Threading.Tasks;
//using OrderManagementSystem.Client.Application;
//using OrderManagementSystem.Client.Application;


namespace OrderManagementSystem
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            App app = new App();
            //app.Run(); // rename it to Init()
            app.Init();

            // TO-DO: if client manager is connected, then show the auth window
           if (ClientManager.Instance().Connected)
            {
                System.Windows.Application mainApp = new System.Windows.Application();

                AuthWindow authWindow = new AuthWindow();
                mainApp.MainWindow = authWindow;
                authWindow.Show();
                mainApp.Run();
            }

        }
    }
}
