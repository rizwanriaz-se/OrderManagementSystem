using OrderManagementSystem.Cache;
using OrderManagementSystemServer.Repository;

namespace OrderManagementSystem.UIComponents.Classes
{
    public class GUIHandler
    {
        private static GUIHandler m_objInstance;

        private User m_objCurrentUser;

        public User CurrentUser
        {
            get { return m_objCurrentUser; }
            set { m_objCurrentUser = value; }
        }

        private GUIHandler() { }

        private CacheManager m_objCacheManager;
        private MessageProcessor m_objMessageProcessor;
        private ClientManager m_objClientManager;

        public CacheManager CacheManager => m_objCacheManager;
        public MessageProcessor MessageProcessor => m_objMessageProcessor;
        public ClientManager ClientManager => m_objClientManager;

        public static GUIHandler Instance
        {
            get
            {
                if (m_objInstance == null)
                    m_objInstance = new GUIHandler();
                return m_objInstance;
            }
        }

        public void Init()
        {
            m_objCacheManager = CacheManager.Instance;
            m_objMessageProcessor = new MessageProcessor();
            m_objClientManager = ClientManager.Instance;
        }
    }
}
