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
        private CacheManager m_CacheManager = CacheManager.Instance();
        private Connection m_Connection = Connection.Instance();
        private MessageProcessor m_MessageProcessor = new MessageProcessor();

        public App()
        {
            //GUIHandler.GetInstance().Init(m_CacheManager, m_MessageProcessor);
        }

        public void Run()
        {
            GUIHandler.GetInstance().Init(m_CacheManager, m_Connection, m_MessageProcessor);

            //m_Connection.ConnectionInit();
        }
    }
}
