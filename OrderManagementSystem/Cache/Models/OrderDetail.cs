﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace OrderManagementSystem.Cache.Models
{
    [Serializable]
    public class OrderDetail : INotifyPropertyChanged
    {
        //private Order m_order;

        //public Order Order
        //{
        //    get { return m_order; }
        //    set
        //    {
        //        m_order = value;
        //        OnPropertyChanged(nameof(Order));
        //    }
        //}

        private Product m_Product;

        [XmlElement]
        public Product Product
        {
            get { return m_Product; }
            set
            {
                m_Product = value;
                OnPropertyChanged(nameof(Product));
            }
        }

        [XmlElement]
        private int m_Quantity;

        public int Quantity
        {
            get
            {
                return m_Quantity;
            }
            set
            {
                m_Quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
