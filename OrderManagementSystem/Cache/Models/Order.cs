using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Cache.Models
{
    public class Order
    {

        public Order()
        {

        }
        public int? Id { get; set; }
        public User? User { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? Status { get; set; }
        public Dictionary<Product, int> Products { get; set; }
        public DateTime? ShippedDate { get; set; }
        public string? ShippingAddress { get; set; }
    }
}
