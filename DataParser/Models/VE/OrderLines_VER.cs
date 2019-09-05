using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models
{
    internal class OrderLines_VER
    {
        public string SalesOrderNo { get; set; }
        public string ItemCode { get; set; }
        public string ItemType { get; set; }
        public string ItemCodeDesc { get; set; }
        public string UnitOfMeasure { get; set; }
        public string DropShip { get; set; }
        public string PromiseDate { get; set; }
        public decimal QuantityOrdered { get; set; }
        public decimal QuantityShipped { get; set; }
        public decimal QuantityBackordered { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitCost { get; set; }
        public string ExtensionAmt { get; set; }
        public string VendorNo { get; set; }
        public string PurchaseOrderNo { get; set; }
    }
}