using OrderManagementSystem.Cache;
using OrderManagementSystemServer.Repository;

//using OrderManagementSystem.Repositories;






//using OrderManagementSystem.Cache.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.UIComponents.Classes
{
    public class GUIHandler
    {
        //private CacheManager 

        private static GUIHandler m_objInstance;


        private User m_CurrentUser;

        public User CurrentUser
        {
            get { return m_CurrentUser; }
            set { m_CurrentUser = value; }
        }

        private GUIHandler()
        {
            //CacheManager = new CacheManager();
        }

        private CacheManager m_objCacheManager;
        private Connection m_objConnection;
        private MessageProcessor m_objMessageProcessor;
        private ClientManager m_objClientManager;
        //private ClientManager m_ClientManager;

        //public CacheManager CacheManager
        //{
        //    get { return m_CacheManager; }
        //    set { m_CacheManager = value; }
        //}

        //public MessageProcessor MessageProcessor
        //{
        //    get { return m_MessageProcessor; }
        //    set { m_MessageProcessor = value; }
        //}

        //public ClientManager ClientManager
        //{
        //    get { return m_ClientManager; }
        //    set { m_ClientManager = value; }
        //}

        //public Connection Connection
        //{
        //    get { return m_Connection; }
        //    set { m_Connection = value; }
        //}
        public CacheManager CacheManager => m_objCacheManager;
        public Connection Connection => m_objConnection;
        public MessageProcessor MessageProcessor => m_objMessageProcessor;

        public ClientManager ClientManager => m_objClientManager;
        //change it to property
        //public static GUIHandler GetInstance()
        //{

        //    if (m_Instance == null)
        //        m_Instance = new GUIHandler();

        //    return m_Instance;
        //}

        public static GUIHandler Instance
        {
            get
            {
                if (m_objInstance == null)
                    m_objInstance = new GUIHandler();
                return m_objInstance;
            }
        }


        public void Init(CacheManager cacheManager, Connection connection, MessageProcessor messageProcessor, ClientManager clientManager)
        {
            m_objCacheManager = cacheManager;
            m_objConnection = connection;
            m_objMessageProcessor = messageProcessor;
            m_objClientManager = clientManager;

            m_objConnection.ConnectionInit();
        }
    }
}
