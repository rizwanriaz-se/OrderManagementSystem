using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagementSystem.Cache;
using OrderManagementSystem.UIComponents.Classes;


//using OrderManagementSystem.Cache;
//using OrderManagementSystem.Classes;

namespace OrderManagementSystem.Client.Application
{
    public class App
    {
        private readonly CacheManager m_objCacheManager;
        private readonly Connection m_objConnection;
        private readonly MessageProcessor m_objMessageProcessor;
        private readonly ClientManager m_objClientManager;

        // move above private variables to constructor
        public App()
        {
            m_objCacheManager = CacheManager.Instance();
            m_objConnection = Connection.Instance();
            m_objMessageProcessor = new MessageProcessor();
            m_objClientManager = ClientManager.Instance();
            //private Connection m_Connection = Connection.Instance();

        }

        public void Run()
        {
            //GUIHandler.Instance.Init(m_CacheManager, m_Connection, m_MessageProcessor);
            GUIHandler guiHandler = GUIHandler.Instance;
            guiHandler.Init(m_objCacheManager, m_objConnection, m_objMessageProcessor, m_objClientManager);
            //m_Connection.ConnectionInit();
        }
    }
}
