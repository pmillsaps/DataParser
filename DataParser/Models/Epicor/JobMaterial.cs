namespace DataParser.Models
{
    internal class JobMaterial
    {
        public string Company { get; set; }
        public string JobNum { get; set; }
        public int AssemblySeq { get; set; }
        public int MtlSeq { get; set; }
        public string PartNum { get; set; }
        public decimal QtyPer { get; set; }
        public string Description { get; set; }
        public int RelatedOperation { get; set; }
        public bool FixedQty { get; set; }
        public bool IssuedComplete { get; set; }
        public bool BuyIt { get; set; }
        public decimal UnitCost { get; set; }
    }
}