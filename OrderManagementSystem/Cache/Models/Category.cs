﻿using DevExpress.XtraExport.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrderManagementSystem.Cache.Models
{
    
    public class Category
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public byte[]? Picture { get; set; }
        //public List<Product> Products { get; set; }
    }
}
