using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models.ASPN
{
    public class JobOrderPlanBOM
    {
        public string JobNumber { get; set; }
        public int OperationSeq { get; set; }
        public string LineType { get; set; }
        public string ComponentItemNumber { get; set; }
        public string DrawingVersion { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string Location { get; set; }
        public string BinNumber { get; set; }
        public string BOMQty { get; set; }
        public string StockingUOM { get; set; }
        public string FlatUnit { get; set; }
        public string Lotsize { get; set; }
        public string Yield { get; set; }
        public decimal QtyRequired { get; set; }
        public decimal QtyIssued { get; set; }
        public decimal UnitCost { get; set; }
        public string CommitToProdDone { get; set; }
        public string Backflush { get; set; }
        public string BOMReference { get; set; }
        public string PhantomItemNumber { get; set; }
        public string PhantomUniqueID { get; set; }
        public string SupplyType { get; set; }
        public string SupplyNo { get; set; }
        public string SeqOrderBy { get; set; }
        public string Status { get; set; }
        public string QtyPendingPost { get; set; }
        public string ClosedStatus { get; set; }
        public string SupplierID { get; set; }
        public string ComponentVersion { get; set; }
        public string OverheadUnitCost { get; set; }
        public string QtyMoved { get; set; }
        public string SubstituteCode { get; set; }
        public string SubstituteLinkUniqueID { get; set; }
        public string UniqueID { get; set; }
        public string AuditUser { get; set; }
        public string AuditDate { get; set; }
        public string AuditTime { get; set; }
        public string CreatedFromPO { get; set; }
        public string CreatedFromVoucher { get; set; }
        public string DisplayOnShipBOM { get; set; }
        public string DfltShipGroupNo { get; set; }
        public string JobTxtStdTextID { get; set; }
        public string BOMText { get; set; }
        public string StandardText { get; set; }
    }
}
