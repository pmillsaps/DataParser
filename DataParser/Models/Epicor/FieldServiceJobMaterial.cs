namespace DataParser.Models
{
    internal class FieldServiceJobMaterial
    {
        public string Company { get; set; }
        public string Plant { get; set; }
        public int AssemblySeq { get; set; }
        public string JobNum { get; set; }
        public bool IssuedComplete { get; set; }
        public int MtlSeq { get; set; }
        public string PartNum { get; set; }
        public string Description { get; set; }
        public decimal QtyPer { get; set; }
        public decimal RequiredQty { get; set; }
        public decimal MaterialMtlCost { get; set; }
    }
}