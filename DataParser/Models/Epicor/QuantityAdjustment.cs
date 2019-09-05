namespace DataParser.Models
{
    public class QuantityAdjustment
    {
        public string Company { get; set; }
        public string PartNum { get; set; }
        public string WareHseCode { get; set; }
        public string BinNum { get; set; }
        public decimal AdjustQuantity { get; set; }
        public string ReasonCode { get; set; }
        public string Reference { get; set; }
        public string Plant { get; set; }
        public string TransDate { get; set; }
    }
}