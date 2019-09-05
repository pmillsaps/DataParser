namespace DataParser.Models.Epicor
{
    internal class PartRevision
    {
        public string Company { get; set; }
        public string Plant { get; set; }
        public string PartNum { get; set; }
        public string RevisionNum { get; set; }
        public string RevShortDesc { get; set; }
        public string RevDescription { get; set; }
        public bool Approved { get; set; }
        public string ApprovedDate { get; set; }
        public string ApprovedBy { get; set; }
        public string EffectiveDate { get; set; }
        public string AltMethod { get; set; }
        public string AltMethodDesc { get; set; }
    }
}