using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models
{
    internal class OrderDetail
    {
        public string Company { get; set; }
        public int OrderNum { get; set; }
        public int OrderLine { get; set; }
        public string PartNum { get; set; }
        public string LineDesc { get; set; }
        public decimal OrderQty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DocunitPrice { get; set; }
        public string NeedByDate { get; set; }
        public string IUM { get; set; }
        public string SalesUM { get; set; }
    }
}