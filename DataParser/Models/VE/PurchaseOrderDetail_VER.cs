using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models
{
    internal class PurchaseOrderDetail_VER
    {
        public string PurchaseOrderNo { get; set; }
        public string ItemCode { get; set; }
        public string ItemCodeDesc { get; set; }
        public string RequiredDate { get; set; }
        public string UnitOfMeasure { get; set; }
        public string JobNo { get; set; }
        public string CustomerPONo { get; set; }
        public decimal QuantityOrdered { get; set; }
        public decimal QuantityBackordered { get; set; }
        public decimal QuantityInvoiced { get; set; }
        public decimal UnitCost { get; set; }
        public decimal ExtensionAmt { get; set; }
        public decimal QuantityReceived { get; set; }
    }
}