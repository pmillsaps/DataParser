namespace DataParser.Models.Epicor
{
    internal class Part
    {
        public string Company { get; set; }
        public string PartNum { get; set; }
        public string PartDescription { get; set; }
        public string IUM { get; set; }
        public string PUM { get; set; }
        public string ClassID { get; set; }
        public string ProdCode { get; set; }
        public string TypeCode { get; set; }
        public string CostMethod { get; set; }
        public bool BuyToOrder { get; set; }
        public string UOMClassID { get; set; }
        public bool PhantomBOM { get; set; }
        public bool NonStock { get; set; }
        public string Type_c { get; set; }
        public decimal UnitPrice { get; set; }
        public bool TrackSerialNum { get; set; }
        public string SNFormat { get; set; }
        public string SNBaseDataType { get; set; }
    }
}