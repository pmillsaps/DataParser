using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models
{
    internal class JobHeader
    {
        public string Company { get; set; }
        public string Plant { get; set; }
        public string JobNum { get; set; }
        public string PartNum { get; set; }
        public string PartDescription { get; set; }
        public string ReqDueDate { get; set; }
        public int ProdQty { get; set; }
        public string StartDate { get; set; }
        public bool JobFirm { get; set; }
        public bool JobEngineered { get; set; }
        public bool JobReleased { get; set; }
        public string CustID { get; set; }
        public bool SyncReqBy { get; set; }
    }
}