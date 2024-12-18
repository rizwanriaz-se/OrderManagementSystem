using OrderManagementSystem.Cache;
//using OrderManagementSystem.Cache.Models;
using OrderManagementSystem.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem
{
    public class GUIHandler
    {
        //private CacheManager 

        private static GUIHandler m_Instance; 


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

        private CacheManager m_CacheManager;
        private Connection m_Connection;
        private MessageProcessor m_MessageProcessor;
        private ClientManager m_ClientManager;

        public CacheManager CacheManager
        {
            get { return m_CacheManager; }
            set { m_CacheManager = value; }
        }
        
        public MessageProcessor MessageProcessor
        {
            get { return m_MessageProcessor; }
            set { m_MessageProcessor = value; }
        }

        public ClientManager ClientManager
        {
            get { return m_ClientManager; }
            set { m_ClientManager = value; }
        }

        public Connection Connection
        {
            get { return m_Connection; }
            set { m_Connection = value; }
        }

        public static GUIHandler GetInstance()
        {

            if (m_Instance == null)
               m_Instance = new GUIHandler();
                    
            return m_Instance;
        }

        public void Init(CacheManager cacheManager, Connection connection, MessageProcessor messageProcessor)
        {
            m_CacheManager = cacheManager;
            m_Connection = connection;
            m_MessageProcessor = messageProcessor;
            m_Connection.ConnectionInit();
        }
    }
}
