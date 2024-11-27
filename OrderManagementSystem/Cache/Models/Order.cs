using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Cache.Models
{
    public class Order
    {
        public enum OrderStatus
        {
            Pending,
            Shipped,
            Delivered
        }

        public int? Id { get; set; }
        public User? User { get; set; }
        public DateTime? OrderDate { get; set; }
        public OrderStatus? Status { get; set; }
        public Dictionary<Product, int> Products { get; set; }
        public DateTime? ShippedDate { get; set; }
        public string? ShippingAddress { get; set; }
    }
}
