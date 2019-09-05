using System;

namespace DataParser.Models.ASPN
{
    public class JobOrderPlanHeader
    {
        public string JobNumber { get; set; }
        public string Priority { get; set; }
        public string ItemNumber { get; set; }
        public string PlanVersion { get; set; }
        public string PlanIsRework { get; set; }
        public string FinGoodLocn { get; set; }
        public string JobStatusID { get; set; }
        public string JobStatusDesc { get; set; }
        public string SchedStatusID { get; set; }
        public string SchedStatusDesc { get; set; }
        public string JobTypeID { get; set; }
        public string JobTypeDesc { get; set; }
        public string JobFreeForm { get; set; }
        public string SalesOrderNo { get; set; }
        public string SysDocID { get; set; }
        public string SysLinSeq { get; set; }
        public string CustomerID { get; set; }
        public string CustomerPO { get; set; }
        public string FirmLinkToSO { get; set; }
        public string OrderQty { get; set; }
        public string MfgUOM { get; set; }
        public string ConvFactor { get; set; }
        public string Yield { get; set; }
        public string ProdnQty { get; set; }
        public string ActMaterial { get; set; }
        public string ActWCenter { get; set; }
        public string ActLabor { get; set; }
        public string ActLaborOH { get; set; }
        public string ActSubContract { get; set; }
        public string ActDirPurchase { get; set; }
        public string EstMaterial { get; set; }
        public string EstWCenter { get; set; }
        public string EstLabor { get; set; }
        public string EstLaborOH { get; set; }
        public string EstSubContract { get; set; }
        public string EstDirPurchase { get; set; }
        public DateTime DateReleased { get; set; }
        public string NextDueDate { get; set; }
        public string SchedStartDate { get; set; }
        public string SchedEndDate { get; set; }
        public string ActStartDate { get; set; }
        public string ActEndDate { get; set; }
        public string PendingCloseQty { get; set; }
        public string PendingRejectQty { get; set; }
        public string QuantityClosed { get; set; }
        public string QuantityRejected { get; set; }
        public string LastAppldDate { get; set; }
        public string LastCloseDate { get; set; }
        public string OrderSpecPlan { get; set; }
        public string SpecPlanVersn { get; set; }
        public string AnchorSched { get; set; }
        public string ApplyDate { get; set; }
        public string CreatedUser { get; set; }
        public string CreateDate { get; set; }
        public string CreateTime { get; set; }
        public string AuditUser { get; set; }
        public string AuditDate { get; set; }
        public string AuditTime { get; set; }
        public string ProcessPlanID { get; set; }
        public string OnHoldReason { get; set; }
        public string ProcessPlanType { get; set; }
        public string DrawingNumber { get; set; }
        public string DrawingRevision { get; set; }
        public string JobTxtStdTextID { get; set; }
        public string JobText { get; set; }
        public string StandardText { get; set; }
    }
}