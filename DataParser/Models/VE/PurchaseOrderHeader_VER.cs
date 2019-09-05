namespace DataParser.Models
{
    public class PurchaseOrderHeader_VER
    {
        public string PurchaseOrderNo { get; set; }
        public string PurchaseOrderDate { get; set; }
        public string RequiredExpireDate { get; set; }
        public string VendorNo { get; set; }
        public string TermsCode { get; set; }
        public string ShipVia { get; set; }
        public string ShipToName { get; set; }
        public string ShipToAddress1 { get; set; }
        public string ShipToAddress2 { get; set; }
        public string ShipToAddress3 { get; set; }
        public string ShipToCity { get; set; }
        public string ShipToState { get; set; }
        public string ShipToZipCode { get; set; }
        public string ShipToCountryCode { get; set; }
        public string OrderStatus { get; set; }
    }
}