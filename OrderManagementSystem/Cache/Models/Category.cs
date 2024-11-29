using DevExpress.XtraExport.Implementation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrderManagementSystem.Cache.Models
{
    
    public class Category : INotifyPropertyChanged
    {
        private int? m_nId;
        private string? m_stName;
        private string? m_stDescription;
        private byte[]? m_Picture;


        public int? Id
        {
            get { return m_nId; }
            set
            {
                m_nId = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Id)));
            }
        }
        public string? Name {
            get { return m_stName; }
            set
            {
                m_stName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }
        public string? Description {
            get { return m_stDescription; }
            set
            {
                m_stDescription = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
            }
        }
        public byte[]? Picture {
            get { return m_Picture; }
            set
            {
                m_Picture = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Picture)));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
