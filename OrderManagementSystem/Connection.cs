//using OrderManagementSystem.Cache;
//using OrderManagementSystem.UIComponents.Views;
//using OrderManagementSystem.App;
//using OrderManagementSystem.UI
//using OrderManagementSystem.UIComponents.UIComponents.Views;

using System;
using System.Threading.Tasks;

namespace OrderManagementSystem.UIComponents.Classes
{
    public class Connection
    {
        //private static Connection m_Instance;
        ////private Application m_Application;
        //private Connection() { }
        //public static Connection Instance()
        //{
        //    if (m_Instance == null) m_Instance = new Connection();
        //    return m_Instance;
        //}

        public async void ConnectionInit()
        {
            // this needs to be removed as results in calling instance multiple times
            //GUIHandler.Instance.CacheManager = CacheManager.Instance;
            //GUIHandler.Instance.ClientManager = ClientManager.Instance();
            //GUIHandler.Instance.ClientManager.onConnected += OnServerConnect;
            //GUIHandler.Instance.ClientManager.onConnected += ;
            
            //GUIHandler.Instance.ClientManager.onDisconnected += OnDisconnected;


            //App app = new App();
            //app.OnServerConnect();
            //ClientManager.Instance().onConnected += OnServerConnect;

            GUIHandler.Instance.Init();
            GUIHandler.Instance.ClientManager.onConnected += OnServerConnect;
            GUIHandler.Instance.ClientManager.onReceive += OnReceive;
            GUIHandler.Instance.ClientManager.onDisconnected += OnDisconnected;

            GUIHandler.Instance.ClientManager.ConnectToServer();

            //if (!ClientManager.Instance().Connected)
            //{
            //    //GUIHandler.Init
            //    await ClientManager.Instance().ConnectToServer();
            //}
        }

        private void OnDisconnected()
        {
            GUIHandler.Instance.ClientManager.HandleServerDisconnected();
        }

        private void OnServerConnect()
        {
            // Task.Run (() => ListenAsync());
            Task.Run(() =>
            {
                 GUIHandler.Instance.ClientManager.InitializeHeartbeat();
                 GUIHandler.Instance.ClientManager.ListenAsync();
            });
            //System.Windows.Application app = new System.Windows.Application();

            //AuthWindow authWindow = new AuthWindow();
            //app.MainWindow = authWindow;
            //authWindow.Show();
            //app.Run();
        }

        private void OnReceive(string jsonMessage)
        {
            GUIHandler.Instance.ClientManager.ProcessResponse(jsonMessage);
            //GUIHandler.Instance.MessageProcessor.ProcessCategoryMessage(response);
        }
    }
}
