using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models
{
    internal class APOpenInvoices_VER
    {
        public string VendorNo { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceDueDate { get; set; }
        public string InvoiceDiscountDate { get; set; }
        public string InvoiceHistoryHeaderSeqNo { get; set; }
        public string TermsCode { get; set; }
        public string HoldPayment { get; set; }
        public string Comment { get; set; }
        public string JobNo { get; set; }
        public string Form1099 { get; set; }
        public string Box1099 { get; set; }
        public string SeparateCheck { get; set; }
        public string Balance { get; set; }
    }
}