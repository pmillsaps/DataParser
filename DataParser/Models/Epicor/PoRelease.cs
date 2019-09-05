namespace DataParser.Models
{
    public class PoRelease
    {
        public string Company { get; set; }
        public int PONum { get; set; }
        public int POLine { get; set; }
        public int PoRelNum { get; set; }
        public decimal RelQty { get; set; }
        public string DueDate { get; set; }
        public string TranType { get; set; }
        public string JobNum { get; set; }

        public int AssemblySeq { get; set; }
        public int JobSeq { get; set; }
        public string JobSeqType { get; set; }


    }
}