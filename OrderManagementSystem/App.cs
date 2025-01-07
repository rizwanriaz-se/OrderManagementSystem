using OrderManagementSystem.UIComponents.Classes;

namespace OrderManagementSystem
{
    public class App
    {
        private Connection m_objConnection;
     
        public void Init()
        {
            m_objConnection = new Connection();
            m_objConnection.ConnectionInit();
        }
    }
}
