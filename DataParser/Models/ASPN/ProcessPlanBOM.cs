using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models.ASPN
{
    public class ProcessPlanBOM
    {
        public string PlanItemNumber { get; set; }
        public string PlanVersion { get; set; }
        public string PlanIsRework { get; set; }
        public int OpSequence { get; set; }
        public int LineSequence { get; set; }
        public string LineType { get; set; }
        public string ComponentItemNumber { get; set; }
        public string InMastMakeBuy { get; set; }
        public string DrawingRevision { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public double Lot { get; set; }
        public double QtyPer { get; set; }
        public double UnitCost { get; set; }
        public string UOM { get; set; }
        public string Location { get; set; }
        public string BinNo { get; set; }
        public string BackFlush { get; set; }
        public string ECAction { get; set; }
        public string ECNumber { get; set; }
        public double Yield { get; set; }
        public string DisplayOnShippingBOM { get; set; }
        public string DfltShipGroupNo { get; set; }
        public string Formula { get; set; }
        public string FlatUnit { get; set; }
        public string BOMRefID { get; set; }
        public int UniqueID { get; set; }
        public string AuditUser { get; set; }
        public DateTime AuditDate { get; set; }
        public DateTime AuditTime { get; set; }
        public string PpBomSupplierID { get; set; }
        public string PpBomCompoVersion { get; set; }
        public string PpBomStdTextID { get; set; }
        public string BOMText { get; set; }
        public string StandardText { get; set; }
    }
}