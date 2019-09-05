namespace DataParser.Models
{
    internal class JobMtlAdjustment
    {
        public string Company { get; set; }
        public string TranType { get; set; }
        public string TranDate { get; set; }
        public int TranQty { get; set; }
        public decimal MtlUnitCost { get; set; }
        public decimal LbrUnitCost { get; set; }
        public decimal SubUnitCost { get; set; }
        public decimal BurUnitCost { get; set; }
        public decimal MtlBurUnitCost { get; set; }
        public string JobNum { get; set; }
        public int AssemblySeq { get; set; }
        public int JobSeq { get; set; }
        public string Plant { get; set; }
    }
}