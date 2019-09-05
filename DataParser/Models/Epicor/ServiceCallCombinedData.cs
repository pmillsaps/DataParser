using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models
{
    internal class ServiceCallCombinedData
    {
        public string Company { get; set; }
        public string CallCode { get; set; }
        public int CallNum { get; set; }
        public string CustNumCustID { get; set; }
        public string EntryDate { get; set; }
        public string RequestDate { get; set; }
        public bool OpenCall { get; set; }
        public string CallPriority { get; set; }
        public string CallComment { get; set; }
        public int FSCallDt_CallLine { get; set; }
        public string FSCallDt_PartNum { get; set; }
        public string FSCallDt_LineDesc { get; set; }
        public string FSCallDt_Plant { get; set; }
        public string FSCallDt_ProbReasonDesc { get; set; }
        public string FSCallDt_PartDescription { get; set; }
        public string FSCallDt_IssueTopicID1 { get; set; }
        public bool FSCallDt_CreateJob { get; set; }
    }
}