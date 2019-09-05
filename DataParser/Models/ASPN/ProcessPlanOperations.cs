using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models.ASPN
{
    public class ProcessPlanOperations
    {
        public string PlanItemNumber { get; set; }
        public string PlanVersion { get; set; }
        public string PlanIsRework { get; set; }
        public int OpSequence { get; set; }
        public string SeqDetailType { get; set; }
        public string WCLabActivty { get; set; }
        public string WCorResDescription { get; set; }
        public double SetupTimeInHrs { get; set; }
        public double RunTime { get; set; }
        public int DelayFromOpSeq { get; set; }
        public string RunTimeCode { get; set; }
        public string ScheduledFlag { get; set; }
        public string GrpSchedFlag { get; set; }
        public double Quantity { get; set; }
        public string ContiguousSeq { get; set; }
        public string BackflushCost { get; set; }
        public int LastBOMLineNo { get; set; }
        public string CrossFlag { get; set; }
        public double CrossTime { get; set; }
        public string TimeFlag { get; set; }
        public string AuditUser { get; set; }
        public DateTime AuditDate { get; set; }
        public DateTime AuditTime { get; set; }
        public string DelayModifier { get; set; }
        public int DelayToOpSeq { get; set; }
        public double RateSetupFixed { get; set; }
        public double RateRuntimeFixed { get; set; }
        public double RateSetupVar { get; set; }
        public double RateRuntimeVar { get; set; }
        public string RateMethod { get; set; }
        public double RateSetup { get; set; }
        public double RateRuntime { get; set; }
        public double RateLabOvHd { get; set; }
        public string RateLabOvHdMethod { get; set; }
        public string SetupGroup { get; set; }
        public double CleanupTimeinHours { get; set; }
        public int ResLvlSetup { get; set; }
        public int ResLvlRuntime { get; set; }
        public int ResLvlCleanup { get; set; }
        public double SCTimeInHrs { get; set; }
        public string EngChangeAct { get; set; }
        public string ECN { get; set; }
        public string StdTextID { get; set; }
        public string SequenceText { get; set; }
        public string StandardText { get; set; }
    }
}