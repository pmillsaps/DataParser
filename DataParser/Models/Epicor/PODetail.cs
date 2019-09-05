namespace DataParser.Models
{
    public class PODetail
    {
        public string Company { get; set; }
        public decimal DocUnitCost { get; set; }
        public int PONum { get; set; }
        public int POLine { get; set; }
        public string PartNum { get; set; }
        public decimal CalcOurQty { get; set; }
        public decimal CalcVendQty { get; set; }
        public string CalcTranType { get; set; }
        public string LineDesc { get; set; }
        public string CalcDueDate { get; set; }
        public bool OverridePriceList { get; set; }
        //public string CalcJobNum { get; set; }

        //public int CalcAssemlySeq { get; set; }

        //public int CalcJobSeq { get; set; }
        //public string CalcJobSeqType { get; set; }
    }
}