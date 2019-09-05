namespace DataParser.Models
{
    internal class JobCostJobMasterfile
    {
        public string JobNo { get; set; }
        public string JobDesc { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public string RevisedContractAmt { get; set; }
        public string UnbilledCost { get; set; }
        public string EstimatedStartDate { get; set; }
        public string ContractNo { get; set; }
        public string ContractDate { get; set; }
        public string JobType { get; set; }
        public string JobStatus { get; set; }
        public string LastCostTransDate { get; set; }
        public string LastBillDate { get; set; }
        public string LastPaymentDate { get; set; }
    }
}