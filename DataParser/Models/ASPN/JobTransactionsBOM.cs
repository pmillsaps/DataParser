using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models.ASPN
{
    public class JobTransactionsBOM
    {
        public string JobNumber { get; set; }
        public string TransxType { get; set; }
        public string TransxDescription { get; set; }
        public int OperationSeq { get; set; }
        public string ItemNumber { get; set; }
        public string Location { get; set; }
        public string BinNumber { get; set; }
        public string SerialLotNo { get; set; }
        public double StockingQuantity { get; set; }
        public string StockingUOM { get; set; }
        public double UnitCost { get; set; }
        public double QtyRemain { get; set; }
        public string DocumentNo { get; set; }
        public string PurchaseDate { get; set; }
        public string PONumber { get; set; }
        public string VendorID { get; set; }
        public string EmployeeNo { get; set; }
        public DateTime AppliedDate { get; set; }
        public DateTime AppliedTime { get; set; }
        public int EventID { get; set; }
        public int EventSeq { get; set; }
        public string LayerType { get; set; }
        public bool JobFullClosed { get; set; }
        public int AllocUniqueID { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime CreateTime { get; set; }
        public string DocReference { get; set; }
        public string ShiftCode { get; set; }
        public string FullCloseApplyDate { get; set; }
        public string TransxUOM { get; set; }
        public double TransxQty { get; set; }
        public double TransxQtyConvMultiplier { get; set; }
        public int CAParentEventID { get; set; }
        public double JobUnitCost { get; set; }
    }
}