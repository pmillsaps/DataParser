using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using System.Threading.Tasks;

namespace DataParser.Models
{
    internal class AROpenInvoices_VER
    {
        public string CustomerNo { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceType { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceDueDate { get; set; }
        public string InvoiceDiscountDate { get; set; }
        public string TermsCode { get; set; }
        public string TaxSchedule { get; set; }
        public string SalespersonNo { get; set; }
        public string Comment { get; set; }
        public string CreditMemoInvoiceReference { get; set; }
        public string JobNo { get; set; }
        public string CustomerPONo { get; set; }
        public string PostingReference { get; set; }
        public string Balance { get; set; }
    }
}