using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;

namespace OrderManagementSystem.Cache.Models
{
    [Serializable]
    public class User
    {
        [XmlElement]
        public int Id { get; set; }
        [XmlElement]
        public string Name { get; set; }
        [XmlElement]
        public string Email { get; set; }
        [XmlElement]
        public string Phone { get; set; }
        [XmlElement]
        public string Password { get; set; }
        [XmlElement]
        public bool IsAdmin { get; set; }
    }
}
