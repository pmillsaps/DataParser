namespace DataParser.Models
{
    internal class POHeader
    {
        public string Company { get; set; }
        public int PONum { get; set; }
        public string OrderDate { get; set; }
        public string FOB { get; set; }
        public string ShipViaCode { get; set; }
        public string BuyerID { get; set; }
        public bool Approve { get; set; }
        public string VendorVendorID { get; set; }
    }
}