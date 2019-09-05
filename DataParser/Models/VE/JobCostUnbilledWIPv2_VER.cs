namespace DataParser.Models
{
    internal class JobCostUnbilledWIPv2_VER
    {
        public string JobNo { get; set; }
        public decimal TransactionAmt { get; set; }
        public string CostCode { get; set; }
        public string CostType { get; set; }
        public string TransactionDate { get; set; }
        public string SourceCode { get; set; }
        public string SeqNo { get; set; }
        public string SourceTranType { get; set; }
        public string SourceTranNo { get; set; }
        public string SourceTranDate { get; set; }
        public string SourceReference { get; set; }
        public string SourceCommentText { get; set; }
        public decimal TransactionUnits { get; set; }
        public decimal UnitCost { get; set; }
        public string TransactionType { get; set; }
    }
}