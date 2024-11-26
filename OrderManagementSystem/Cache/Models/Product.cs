using DevExpress.XtraExport.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrderManagementSystem.Cache.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public byte[] Picture { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        //public Order Order { get; set; }
    }
}
