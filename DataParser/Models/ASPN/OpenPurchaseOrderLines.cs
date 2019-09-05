using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models.ASPN
{
    public class OpenPurchaseOrderLines
    {
        public string Amt { get; set; }
        public string Pono { get; set; }
        public string RowNum { get; set; }
        public string ReqPrcRequisitType { get; set; }
        public string ReqPrcItemNo { get; set; }
        public string ReqPrcUOMPurchase { get; set; }
        public string ReqPrcJobNo { get; set; }
        public string Qtyremn { get; set; }
        public string Desc { get; set; }
        public string Reqdate { get; set; }
        public string cLineTypeDesc { get; set; }
    }
}