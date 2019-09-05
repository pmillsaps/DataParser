namespace DataParser.Models
{
    internal class JobXRef
    {
        public JobXRef()
        {
            OriginalJob = "";
        }

        public string OriginalJob { get; set; }
        public string NewJob { get; set; }
        public string NewOrder { get; set; }
        public int NewServiceCall { get; set; }
        public string CallCode { get; set; }
        public string OpCode { get; set; }
        public string OpCode2 { get; set; }
        public string OldSalesOrder { get; set; }
        public string NewSalesOrder { get; set; }
        public string OldPO { get; set; }
        public string NewPO { get; set; }
    }
}