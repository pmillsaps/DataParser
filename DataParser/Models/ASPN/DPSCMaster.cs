using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models.ASPN
{
    public class DPSCMaster
    {
        public string ImDscDpScItem { get; set; }
        public string ImDscDpScType { get; set; }
        public string ImDscDescription { get; set; }
        public string ImDscPrimeVendor { get; set; }
        public string ImDscPlannerID { get; set; }
        public string ImDscQtyMask { get; set; }
        public string ImDscCostMask { get; set; }
        public string ImDscStatus { get; set; }
        public string ImDscLeadTimCriteria { get; set; }
        public double ImDscLeadDaysBuffer { get; set; }
        public string ImDscPONotes { get; set; }
    }
}