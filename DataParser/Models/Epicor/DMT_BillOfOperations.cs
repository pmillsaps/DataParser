using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models.Epicor
{
    public class DMT_BillOfOperations
    {
        public string Company { get; set; }
        public string ECOGroupID { get; set; }
        public string PartNum { get; set; }
        public string RevisionNum { get; set; }
        public int OprSeq { get; set; }
        public string OpCode { get; set; }
        public decimal QtyPer { get; set; }
        public string ECOOpDtl__ResourceGrpID { get; set; }
        public double EstSetHours { get; set; }
        public double EstProdHours { get; set; }
        public double ProdStandard { get; set; }
        public string StdFormat { get; set; }
        public string StdBasis { get; set; }
        public string SchedRelation { get; set; }
        public string LaborEntryMethod { get; set; }
        public string CommentText { get; set; }
        public string SubContract { get; set; }
        public string SubPartNum { get; set; }
        public string VendorNumVendorID { get; set; }
        public string EstUnitCost { get; set; }
        public string DaysOut { get; set; }
    }
}