using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models.ASPN
{
    public class JobOrderPlanOperations
    {
        public string JobNumber { get; set; }
        public int OperationSeq { get; set; }
        public string SeqStatus { get; set; }
        public string SeqStatusDesc { get; set; }
        public string SeqDetType { get; set; }
        public string WCorResID { get; set; }
        public string WCorResDesc { get; set; }
        public decimal EstRTInHrs { get; set; }
        public string EstSUInHrs { get; set; }
        public string RuntimeTime { get; set; }
        public string RuntimeUOM { get; set; }
        public string Schedule { get; set; }
        public string Contiguous { get; set; }
        public string Backflush { get; set; }
        public string GrpSchedID { get; set; }
        public string CrossFlag { get; set; }
        public string CrossTime { get; set; }
        public string TimeFlag { get; set; }
        public string SchedStrtDt { get; set; }
        public string SchStartTime { get; set; }
        public string SchedEndDt { get; set; }
        public string SchEndTime { get; set; }
        public string ActStartDt { get; set; }
        public string ActEndDt { get; set; }
        public string ActSetInHrs { get; set; }
        public string ActRTInHrs { get; set; }
        public string EstSetup { get; set; }
        public string ActSetup { get; set; }
        public string EstRun { get; set; }
        public string ActRun { get; set; }
        public string EstOverHead { get; set; }
        public string ActOverHead { get; set; }
        public string QtyComplete { get; set; }
        public string RejectQty { get; set; }
        public string HoldReasonID { get; set; }
        public string OnHoldAtDate { get; set; }
        public string OnHoldAtTime { get; set; }
        public string RemainInMin { get; set; }
        public string UsrRemnInMin { get; set; }
        public string WorkCenterID { get; set; }
        public string TrkSeqUniqueID { get; set; }
        public string TrkSeqAuditUser { get; set; }
        public string TrkSeqAuditDate { get; set; }
        public string TrkSeqAuditTime { get; set; }
        public string TrkSeqDelayModifier { get; set; }
        public string TrkSeqAnchor { get; set; }
        public string TrkSeqActStartTime { get; set; }
        public string TrkSeqActEndTime { get; set; }
        public string JobTxtStdTextID { get; set; }
        public string SequenceText { get; set; }
        public string StandardText { get; set; }
    }
}
