namespace DataParser.Models.Epicor
{
    internal class DMT_BillOfMaterial
    {
        public string Company { get; set; }
        public string ECOGroupID { get; set; }
        public string PartNum { get; set; }
        public string RevisionNum { get; set; }
        public int MtlSeq { get; set; }
        public string MtlPartNum { get; set; }
        public double QtyPer { get; set; }
        public bool FixedQty { get; set; }
        public int RelatedOperation { get; set; }
        public bool PullAsAsm { get; set; }
        public bool ViewAsAsm { get; set; }
        public bool PlanAsAsm { get; set; }
        public string UOMCode { get; set; }
    }
}