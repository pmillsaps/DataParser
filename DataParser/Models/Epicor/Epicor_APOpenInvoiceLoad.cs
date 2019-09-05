using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models
{
    internal class Epicor_APOpenInvoiceLoad
    {
        public string Invoice20 { get; set; }
        public string SupplierID { get; set; }
        public string Terms { get; set; }
        public string InvoiceDate { get; set; }
        public decimal Balance { get; set; }
        public string ReferenceID20 { get; set; }
        public bool DebitMemo { get; set; }
    }
}