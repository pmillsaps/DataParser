using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models
{
    internal class Epicor_AROpenInvoiceLoad
    {
        public string Invoice20 { get; set; }
        public string CustID { get; set; }
        public string TermsCode { get; set; }
        public string InvoiceDate { get; set; }
        public decimal BaseBalance { get; set; }
        public string ReferenceID20 { get; set; }
        public bool CreditMemo { get; set; }
    }
}