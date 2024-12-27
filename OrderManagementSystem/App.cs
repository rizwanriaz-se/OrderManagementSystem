using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraRichEdit.Commands.Internal;
using OrderManagementSystem.Cache;

//using OrderManagementSystem.Cache;
using OrderManagementSystem.UIComponents.Classes;
using OrderManagementSystem.UIComponents.Views;
//using OrderManagementSystem.UIComponents.Views;


//using OrderManagementSystem.Cache;
//using OrderManagementSystem.Classes;

namespace OrderManagementSystem
{
    public class App
    {
        //private readonly CacheManager m_objCacheManager;
        //private readonly Connection m_objConnection;
        //private readonly MessageProcessor m_objMessageProcessor;
        //private readonly ClientManager m_objClientManager;
        private Connection m_objConnection;
        //public bool Connected = false;

        // move above private variables to constructor
        public App()
        {
            //m_objCacheManager = CacheManager.Instance();
            //m_objMessageProcessor = new MessageProcessor();
            //m_objClientManager = ClientManager.Instance();

            //private Connection m_Connection = Connection.Instance();
            //m_objConnection = new Connection();

        }

        public void Init()
        {
            m_objConnection = new Connection();
            if (!ClientManager.Instance().Connected)
            {
                //Connected = true;
                m_objConnection.ConnectionInit();
            }

            //m_objCacheManager.Init();
            //m_objConnection.Init();
            //m_objMessageProcessor.Init();
            //m_objClientManager.Init();
        }


        //public void Run()
        //{
        //    //m_objConnection = new Connection();

        //    // if not connected then call below method
        //    //m_objConnection.ConnectionInit();


        //    //GUIHandler.Instance.Init(m_CacheManager, m_Connection, m_MessageProcessor);
        //    //GUIHandler.Instance.Init(m_objCacheManager, m_objConnection, new MessageProcessor(), m_objClientManager);
        //    //GUIHandler.Instance.Init();
        //    //guiHandler.Init(m_objCacheManager, m_objConnection, m_objMessageProcessor, m_objClientManager);
        //}

        //public void OnServerConnect()
        //{
        //    System.Windows.Application app = new System.Windows.Application();

        //    AuthWindow authWindow = new AuthWindow();
        //    app.MainWindow = authWindow;
        //    authWindow.Show();
        //    app.Run();
        //}

        // make a property for connected, and use it in 
    }
}
