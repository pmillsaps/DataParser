namespace DataParser.Models
{
    internal class OrderRelease
    {
        public string Company { get; set; }
        public int OrderNum { get; set; }
        public int OrderLine { get; set; }
        public int OrderRelNum { get; set; }
        public string Linetype { get; set; }
        public decimal OurReqQty { get; set; }
        public bool BuyToOrder { get; set; }
        public bool DropShip { get; set; }
        public bool Make { get; set; }
    }
}