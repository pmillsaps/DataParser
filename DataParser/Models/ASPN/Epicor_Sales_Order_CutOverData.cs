namespace DataParser.Models.ASPN
{
    internal class Epicor_Sales_Order_CutOverData
    {
        public string Month { get; set; }
        public string JobNo { get; set; }
        public string CustPONo { get; set; }
        public string CustomerID { get; set; }
        public string Customer { get; set; }
        public string OrderDate { get; set; }
        public string RequestDate { get; set; }
        public string NeedByDate { get; set; }
        public string Shipping { get; set; }
        public string Model { get; set; }
        public string LiftSerialNo { get; set; }
        public string VehicleType { get; set; }
        public string ChassisNo { get; set; }
        public string Supply { get; set; }
        public string SalesLineValue { get; set; }
        public string WorkOrderNo { get; set; }
        public string StockIssued { get; set; }
        public string Installation { get; set; }
        public string Lift { get; set; }
        public string Vehicle { get; set; }
        public decimal WIP { get; set; }
        public string LiftRecd { get; set; }
        public string LiftWIP { get; set; }
        public string ChassisRecd { get; set; }
        public string ChassisWIP { get; set; }
        public int NewServiceCall { get; set; }
        public string NewJob { get; set; }

        public string OpCode { get; set; }
        public string OpCode2 { get; set; }
    }
}