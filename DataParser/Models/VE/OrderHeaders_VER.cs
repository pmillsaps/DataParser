using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models
{
    internal class OrderHeaders_VER
    {
        public string SalesOrderNo { get; set; }
        public string OrderDate { get; set; }
        public string OrderType { get; set; }
        public string ShipExpireDate { get; set; }
        public string CustomerNo { get; set; }
        public string ShipToCode { get; set; }
        public string ShipToName { get; set; }
        public string ShipToAddress1 { get; set; }
        public string ShipToAddress2 { get; set; }
        public string ShipToAddress3 { get; set; }
        public string ShipToCity { get; set; }
        public string ShipToState { get; set; }
        public string ShipToZipCode { get; set; }
        public string ShipToCountryCode { get; set; }
        public string ShipVia { get; set; }
        public string CustomerPONo { get; set; }
        public string FOB { get; set; }
    }
}