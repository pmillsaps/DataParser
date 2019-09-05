using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models.ASPN
{
    public class ProcessPlanHeader
    {
        public string PlanItemNumber { get; set; }
        public string ItemDescription { get; set; }
        public string PlanVersion { get; set; }
        public string PlanIsRework { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public string EffectiveDate { get; set; }
        public string Drawing { get; set; }
        public string DrawingRev { get; set; }
        public string DrawingEffectiveDate { get; set; }
        public string FileNameOne { get; set; }
        public bool DrawFile1IsALink { get; set; }
        public string ClassCode { get; set; }
        public double MakeQuantity { get; set; }
        public double LotSize { get; set; }
        public double Yield { get; set; }
        public string ManufactUOM { get; set; }
        public string CreateUser { get; set; }
        public string CreateDate { get; set; }
        public string CreateTime { get; set; }
        public string AuditUser { get; set; }
        public string AuditDate { get; set; }
        public string AuditTime { get; set; }
        public string StdTextID { get; set; }
        public string PPHeaderText { get; set; }
        public string StandardText { get; set; }
    }
}