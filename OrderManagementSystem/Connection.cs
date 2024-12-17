using OrderManagementSystem.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem
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
            //GUIHandler.GetInstance().ClientManager.Receive += ReceiveData;

            if (!GUIHandler.GetInstance().ClientManager.Connected)
            {
               await GUIHandler.GetInstance().ClientManager.ConnectToServer();
                //GUIHandler.GetInstance().ClientManager.Login("admin", "admin");
            }
        }

        private void OnServerConnect()
        {
            //ClientManager.Instance().InitializeHeartbeat();
        }

        private void ReceiveData()
        {
            //GUIHandler.GetInstance().CacheManager.LoadData(ClientManager.Instance().Client);
        }

    }
}
