using DevExpress.Office.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OrderManagementSystem
{
    public class MessageProcessor
    {
        //private ClientManager m_ClientManager;

        public MessageProcessor() { }
        //public MessageProcessor(ClientManager clientManager)
        //{
        //    m_ClientManager = clientManager;
        //}

        public void SendMessage(Enums.MessageType messageType, Enums.MessageAction messageAction, object message=null)
        {
            Classes.Request request = new Classes.Request
            {
                MessageAction = messageAction,
                MessageType = messageType,
                Data = message
            };
            try
            {
                 GUIHandler.GetInstance().ClientManager.SendMessage(request);
            }
            catch (Exception ex) {
                Debug.WriteLine("Error in Client MessageProcessor SendMessage", ex.Message);
            }
        }

        public void ReceiveMessage(Enums.MessageType messageType, Enums.MessageAction messageAction, object message = null)
        {
            Classes.Request request = new Classes.Request
            {
                MessageAction = messageAction,
                MessageType = messageType,
                Data = message
            };
            try
            {
                GUIHandler.GetInstance().ClientManager.SendMessage(request);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Client MessageProcessor SendMessage", ex.Message);
            }
        }
    }
}


