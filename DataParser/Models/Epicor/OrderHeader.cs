namespace DataParser.Models
{
    internal class OrderHeader
    {
        public string Company { get; set; }
        public int OrderNum { get; set; }
        public string PONum { get; set; }
        public string RequestDate { get; set; }
        public string OrderDate { get; set; }
        public string FOB { get; set; }
        public string ShipViaCode { get; set; }
        public string TermsCode { get; set; }
        public string NeedByDate { get; set; }
        public string CustomerCustID { get; set; }
        public string UD_LiftNumber_c { get; set; }
        public string UD_SerialNo_c { get; set; }
        public string SubmittedBy_c { get; set; }
        public string StockOrder_c { get; set; }
        public string UpfitterJobNum_c { get; set; }
        public string EndCustomerName_c { get; set; }
        public string LinkedQuote_c { get; set; }
        public string BodySN_c { get; set; }
        public string TruckVIN_c { get; set; }
        public string QuotedHours_c { get; set; }
        public string LiftModel_c { get; set; }
        public string TruckMake_c { get; set; }
        public string TruckModel_c { get; set; }
        public string TruckYear_c { get; set; }
        public string BodyMake_c { get; set; }
        public string BodyModel_c { get; set; }
        public string OTSName { get; set; }
        public string OTSAddress1 { get; set; }
        public string OTSAddress2 { get; set; }
        public string OTSAddress3 { get; set; }
        public string OTSCity { get; set; }
        public string OTSState { get; set; }
        public string OTSZip { get; set; }
        public bool UseOTS { get; set; }
        public string OTSCountryNum { get; set; }
        public string OrderComment { get; set; }
    }
}