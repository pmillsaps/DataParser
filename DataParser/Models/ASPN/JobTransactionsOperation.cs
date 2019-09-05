using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models.ASPN
{
    public class JobTransactionsOperation
    {
        public string JobNumber { get; set; }
        public int OperationSeq { get; set; }
        public string SeqDtlType { get; set; }
        public string WCorResID { get; set; }
        public DateTime TransxDate { get; set; }
        public DateTime TransxTime { get; set; }
        public string EmployeeID { get; set; }
        public string TransxType { get; set; }
        public string TransxDesc { get; set; }
        public string HoldReason { get; set; }
        public double QtyComplete { get; set; }
        public double QtyRejects { get; set; }
        public double WorkTimeHrs { get; set; }
        public double ActiveRate { get; set; }
        public string CostCenter { get; set; }
        public double OHRate { get; set; }
        public string LabOHType { get; set; }
        public bool SeqComplete { get; set; }
        public DateTime TransxEndDate { get; set; }
        public DateTime TransxEndTime { get; set; }
        public string ShopFloorTransx { get; set; }
        public double WCFixedRate { get; set; }
        public double WCVarRate { get; set; }
        public int UniqTrkSqID { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime CreateTime { get; set; }
        public double BreakTime { get; set; }
        public string Reference { get; set; }
        public string TimeCode { get; set; }
        public string Shift { get; set; }
        public bool Billable { get; set; }
        public string OTCalculation { get; set; }
        public string ApprovedStatus { get; set; }
        public string ApprovedBy { get; set; }
        public string AdjReasonCode { get; set; }
        public bool JobFullClosed { get; set; }
        public string JobItemNumber { get; set; }
        public string Department { get; set; }
        public string Status { get; set; }
        public string EmpJobClass { get; set; }
        public string EmpJobDesc { get; set; }
        public bool ReworkTxn { get; set; }
        public bool ReworkTxnExpensed { get; set; }
        public string WorkCenter { get; set; }
        public string FullCloseApplyDate { get; set; }
    }
}