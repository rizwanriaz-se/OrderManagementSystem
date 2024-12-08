using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using OrderManagementSystem.Cache.Models;

namespace OrderManagementSystem.Utils
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine($"{value}---------------------");
            //if (value is string status)
            //{
            //    Console.WriteLine($"-------------------{status}-----------------");
            //    return status switch
            //    {
            //        "Pending" => Brushes.Yellow,
            //        "Shipped" => Brushes.Green,
            //        "Delivered" => Brushes.Red,
            //        _ => Brushes.Transparent
            //    };
            //}
            return Brushes.Red;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
