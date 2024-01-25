using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Merve.Models
{
    public class Order
    {
        public string fullName { get; set; }
        public string Date { get; set; }
        public string Adress { get; set; }
        public string paymentType { get; set; }
        public string amount { get; set; }

        public string summary { get; set; }
    }
}
