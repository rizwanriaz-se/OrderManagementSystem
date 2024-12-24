//using OrderManagementSystem.Cache;
//using OrderManagementSystem.UIComponents.Views;
//using OrderManagementSystem.App;
//using OrderManagementSystem.UI
//using OrderManagementSystem.UIComponents.UIComponents.Views;

namespace OrderManagementSystem.UIComponents.Classes
{
    public class Connection
    {
        private static Connection m_Instance;
        //private Application m_Application;
        private Connection() { }
        public static Connection Instance()
        {
            if (m_Instance == null) m_Instance = new Connection();
            return m_Instance;
        }

        public async void ConnectionInit()
        {
            // this needs to be removed as results in calling instance multiple times
            //GUIHandler.Instance.CacheManager = CacheManager.Instance;
            //GUIHandler.Instance.ClientManager = ClientManager.Instance();
            //GUIHandler.Instance.ClientManager.onConnected += OnServerConnect;

            //App app = new App();
            //app.OnServerConnect();
            ClientManager.Instance().onConnected += OnServerConnect;


            if (ClientManager.Instance().Connected)
            {
                //GUIHandler.Init
                await ClientManager.Instance().ConnectToServer();
            }
        }

        private void OnServerConnect()
        {
            //System.Windows.Application app = new System.Windows.Application();

            //AuthWindow authWindow = new AuthWindow();
            //app.MainWindow = authWindow;
            //authWindow.Show();
            //app.Run();
        }
    }
}
