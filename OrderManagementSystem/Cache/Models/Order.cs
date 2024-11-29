using DevExpress.XtraRichEdit.Model.History;
using OrderManagementSystem.UIComponents.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Cache.Models
{
    public class Order : INotifyPropertyChanged
    {
        public enum OrderStatus
        {
            Pending,
            Shipped,
            Delivered
        }

        private int? m_nId;
        private User? m_User;
        private DateTime? m_OrderDate;
        private OrderStatus? m_enStatus;
        private Dictionary<Product, int> m_Products;
        private DateTime? m_ShippedDate;
        private string? m_stShippingAddress;
        

        public int? Id
        {
            get { return m_nId; }
            set
            {
                m_nId = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        public User? User
        {
            get { return m_User;  }
            set
            {
                m_User = value;
                OnPropertyChanged(nameof(User));
            }
        }
        public DateTime? OrderDate
        {
            get { return m_OrderDate; }
            set
            {
                m_OrderDate = value;
                OnPropertyChanged(nameof(OrderDate));
            }
        }
        public OrderStatus? Status
        {
            get { return m_enStatus; }
            set
            {
                m_enStatus = value;
                OnPropertyChanged(nameof(Status));
            }
        }
        public Dictionary<Product, int> Products {
            get { return m_Products; }
            set
            {
                m_Products = value;
                OnPropertyChanged(nameof(Products));
                OnPropertyChanged(nameof(ProductRows)); // Notify that ProductRows has changed
            }
        }
        public DateTime? ShippedDate {
            get { return m_ShippedDate; }
            set
            {
                m_ShippedDate = value;
                OnPropertyChanged(nameof(ShippedDate));
            }
        }
        public string? ShippingAddress {
            get { return m_stShippingAddress; }
            set
            {
                m_stShippingAddress = value;
                OnPropertyChanged(nameof(ShippingAddress));
            }
        }

        // New property to expose Products as a collection
        public ObservableCollection<ProductRow> ProductRows
        {
            get
            {
                return new ObservableCollection<ProductRow>(
                    m_Products?.Select(p => new ProductRow
                    {
                        SelectedProduct = p.Key,
                        Quantity = p.Value
                    }) ?? Enumerable.Empty<ProductRow>()
                );
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
