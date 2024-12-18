using DevExpress.Office.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem
{
    public class Enums
    {
        public enum MessageType
        {
            Category,
            Order,
            Product,
            User,
        }
        public enum MessageAction
        {
            Add,
            Update,
            Delete,
            Load,
            
        }


    }
}
