﻿//using DevExpress.XtraExport.Implementation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace OrderManagementSystem.Repositories
{

    [Serializable]
    public class Product : INotifyPropertyChanged
    {
        private int m_nId;
        private string m_stName;
        private string m_stDescription;
        private Category m_Category;
        private byte[] m_Picture;
        private decimal m_UnitPrice;
        private int m_nUnitsInStock;


        public int Id
        {
            get { return m_nId; }
            set
            {
                m_nId = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        public string Name
        {
            get { return m_stName; }
            set
            {
                m_stName = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Description
        {
            get { return m_stDescription; }
            set
            {
                m_stDescription = value;
                OnPropertyChanged(nameof(Description));
            }
        }
        public Category Category
        {
            get { return m_Category; }
            set
            {
                m_Category = value;
                OnPropertyChanged(nameof(Category));
            }
        }
        public byte[] Picture
        {
            get { return m_Picture; }
            set
            {
                m_Picture = value;
                OnPropertyChanged(nameof(Picture));
            }
        }
        public decimal UnitPrice
        {
            get { return m_UnitPrice; }
            set
            {
                m_UnitPrice = value;
                OnPropertyChanged(nameof(UnitPrice));
            }
        }
        public int UnitsInStock
        {
            get { return m_nUnitsInStock; }
            set
            {
                m_nUnitsInStock = value;
                OnPropertyChanged(nameof(UnitsInStock));
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is not Product other) return false;

            return Id == other.Id && Name == other.Name && Description == other.Description && Category == other.Category && UnitsInStock == other.UnitsInStock && UnitPrice == other.UnitPrice;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
