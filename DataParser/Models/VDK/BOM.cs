namespace DataParser.Models.VDK
{
    internal class BOM
    {
        public string Company { get; set; }
        public string PartNum { get; set; }
        public string RevisionNum { get; set; }
        public string MtlSeq { get; set; }
        public string NAV_Type { get; set; }
        public string MtlPartNum { get; set; }
        public string NAV_Description { get; set; }
        public string QtyPer { get; set; }
        public string RelatedOperation { get; set; }
        public string Plant { get; set; }
        public string ECOGroupID { get; set; }
        public string Source { get; set; }
    }
}