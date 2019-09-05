using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models
{
    internal class V_VE_Customer
    {
        public string Company { get; set; }
        public string CustID { get; set; }
        public string CustNum { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string TermsCode { get; set; }
        public string ShipViaCode { get; set; }
    }
}