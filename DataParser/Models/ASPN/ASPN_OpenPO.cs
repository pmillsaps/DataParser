namespace DataParser.Models.ASPN
{
    internal class ASPN_OpenPO
    {
        public string PONum { get; set; }
        public string Stock_Reference { get; set; }
        public string Supplier { get; set; }
        public decimal Quantity_Required { get; set; }
        public decimal Quantity_Received { get; set; }
        public string Date_Created { get; set; }
        public string Reference { get; set; }
        public string Date_Required { get; set; }
        public string Order_Completed { get; set; }
        public string Invoice_Completed { get; set; }
        public string Ledger_Account { get; set; }
        public string Cost_Code { get; set; }
        public decimal Price { get; set; }
        public decimal Value { get; set; }
        public string Discount_Percentage { get; set; }
        public string Discount_Value { get; set; }
    }
}