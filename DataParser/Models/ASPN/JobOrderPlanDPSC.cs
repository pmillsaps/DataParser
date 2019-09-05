using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models.ASPN
{
    public class JobOrderPlanDPSC
    {
        public string JobNumber { get; set; }
        public int OperationSeq { get; set; }
        public string LineType { get; set; }
        public string DPSCItemNumber { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public double BOMQuantity { get; set; }
        public string UOM { get; set; }
        public string FlatUnit { get; set; }
        public double LotSize { get; set; }
        public double Yield { get; set; }
        public double QtyRequired { get; set; }
        public double QtyReceived { get; set; }
        public double NaturalUnitCost { get; set; }
        public string Backflush { get; set; }
        public bool IgnoreOnJobCls { get; set; }
        public string SupplyType { get; set; }
        public string SupplyNo { get; set; }
        public int SeqOrderBy { get; set; }
        public double HomeUnitCost { get; set; }
        public string VendorID { get; set; }
        public double QtyPendingPost { get; set; }
        public int UniqueID { get; set; }
        public string AuditUser { get; set; }
        public DateTime AuditDate { get; set; }
        public DateTime AuditTime { get; set; }
        public string CreatedFromPO { get; set; }
        public string CreatedFromVoucher { get; set; }
        public string Location { get; set; }
        public string BinNumber { get; set; }
        public string PhantomItemNumber { get; set; }
        public int PhantomUniqueID { get; set; }
        public string StdTextID { get; set; }
        public string DPSCText { get; set; }
        public string StandardText { get; set; }
    }
}