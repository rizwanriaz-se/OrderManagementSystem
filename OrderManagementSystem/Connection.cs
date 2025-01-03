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

        public async void ConnectionInit()
        {
            GUIHandler.Instance.Init();
            GUIHandler.Instance.ClientManager.OnConnected += OnServerConnect;
            GUIHandler.Instance.ClientManager.OnReceive += OnReceive;
            GUIHandler.Instance.ClientManager.OnDisconnected += OnDisconnected;

            GUIHandler.Instance.ClientManager.ConnectToServer();
        }

        private void OnDisconnected()
        {
            GUIHandler.Instance.ClientManager.HandleDisconnection();
        }

        private void OnServerConnect()
        {
            Task.Run(() =>
            {
                GUIHandler.Instance.ClientManager.InitializeHeartbeat();
                GUIHandler.Instance.ClientManager.ListenAsync();
            });
        }

        private void OnReceive(string jsonMessage)
        {
            GUIHandler.Instance.ClientManager.ProcessResponse(jsonMessage);
        }
    }
}
