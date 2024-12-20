using OrderManagementSystem.Cache;
using OrderManagementSystem.UIComponents.Views;

//using OrderManagementSystem.UI
//using OrderManagementSystem.UIComponents.UIComponents.Views;

namespace OrderManagementSystem.UIComponents.Classes
{
    public class Connection
    {
        private static Connection m_Instance;

        private Connection() { }
        public static Connection Instance()
        {
            if (m_Instance == null) m_Instance = new Connection();
            return m_Instance;
        }

        public async void ConnectionInit()
        {
            GUIHandler.GetInstance().CacheManager = CacheManager.Instance();
            GUIHandler.GetInstance().ClientManager = ClientManager.Instance();
            GUIHandler.GetInstance().ClientManager.onConnected += OnServerConnect;

            if (!GUIHandler.GetInstance().ClientManager.Connected)
            {
                //GUIHandler.Init
                await GUIHandler.GetInstance().ClientManager.ConnectToServer();
            }
        }

        private void OnServerConnect()
        {
            System.Windows.Application app = new System.Windows.Application();

            AuthWindow authWindow = new AuthWindow();
            app.MainWindow = authWindow;
            authWindow.Show();
            app.Run();
        }
    }
}
