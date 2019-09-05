namespace DataParser.Models.Epicor
{
    internal class IssueMaterial
    {
        public string Company { get; set; }
        public string TranDate { get; set; }
        public decimal TranQty { get; set; }
        public string ToJobNum { get; set; }
        public int ToAssemblySeq { get; set; }
        public int ToJobSeq { get; set; }
    }
}