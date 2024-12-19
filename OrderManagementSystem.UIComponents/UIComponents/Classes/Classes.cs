using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Threading.Tasks;

namespace OrderManagementSystem.UIComponents.UIComponents.Classes
{
    public class Classes
    {
        //[Serializable]
        public class Request
        {
            public Enums.MessageType MessageType { get; set; }
            public Enums.MessageAction MessageAction { get; set; }
            public object Data { get; set; }
        }

        public class Response
        {

            public Enums.MessageType MessageType { get; set; }
            public Enums.MessageAction MessageAction { get; set; }
            public object Data { get; set; }
        }

    }
}
