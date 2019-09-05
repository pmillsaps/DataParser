namespace DataParser.Models.Epicor
{
    internal class CostAdjustment
    {
        public string Company { get; set; }
        public string PartNum { get; set; }
        public string ReasonCode { get; set; }
        public decimal AvgMtlUnitCost { get; set; }
        public decimal LastMtlUnitCost { get; set; }
        public decimal StdMtlUnitCost { get; set; }
        public string Plant { get; set; }
        public string TransDate { get; set; }
    }
}