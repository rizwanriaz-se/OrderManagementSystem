using OrderManagementSystem.Cache;
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

        private GUIHandler()
        {
            CacheManager = new CacheManager();
        }
        private CacheManager m_CacheManager;

        public CacheManager CacheManager
        {
            get { return m_CacheManager; }
            private set { m_CacheManager = value; }
        }
        


        //private static readonly object _lock = new object();

        public static GUIHandler GetInstance()
        {

            if (m_Instance == null)
               m_Instance = new GUIHandler();
                    
            return m_Instance;

        }

        public void Init(CacheManager cacheManager)
        {
            m_CacheManager = cacheManager;
            
        }


       
    }
}
