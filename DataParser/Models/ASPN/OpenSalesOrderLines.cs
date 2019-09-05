using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models.ASPN
{
    public class OpenSalesOrderLines
    {
        public string OeLinOrderNo { get; set; }
        public string OeLinLineItemType { get; set; }
        public string OeLinRowNum { get; set; }
        public string OeLinItemNumber { get; set; }
        public string OeLinDescription { get; set; }
        public string IOSSellQtyRemain { get; set; }
        public string IOSSellUnitPrice { get; set; }
        public string TotalRemainingNetWDiscWTax { get; set; }
    }
}