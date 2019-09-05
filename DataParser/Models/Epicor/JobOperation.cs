namespace DataParser.Models
{
    internal class JobOperation
    {
        public string Company { get; set; }
        public string Plant { get; set; }
        public string JobNum { get; set; }
        public int AssemblySeq { get; set; }
        public int OprSeq { get; set; }
        public string OpCode { get; set; }
        public decimal ProdStandard { get; set; }
        public bool OpComplete { get; set; }
        public int QtyCompleted { get; set; }
        public string StdFormat { get; set; }
        public string LaborEntryMethod { get; set; }
        public string SchedRelation { get; set; }
        public bool AutoReceive { get; set; }
        public bool FinalOpr { get; set; }
    }
}