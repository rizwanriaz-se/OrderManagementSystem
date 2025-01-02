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
        private Connection m_objConnection;
     
        public void Init()
        {
            m_objConnection = new Connection();
            //if (!ClientManager.Instance().Connected)
            //{
            //    m_objConnection.ConnectionInit();
            //}
            m_objConnection.ConnectionInit();
        }
    }
}
