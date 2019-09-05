using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models.ASPN
{
    public class OpenSalesOrderHeader
    {
        public string OeHdr_SOStatus { get; set; }
        public string OeHdr_SOOrderNo { get; set; }
        public string OeHdr_SOCustPONo { get; set; }
        public string OeHdrReqDate { get; set; }
        public string OeHdrOrdDate { get; set; }
        public string OeHdrFOBKey { get; set; }
        public string OeHdrShipViaKey { get; set; }
        public string OeHdrTermsKey { get; set; }
        public string OeHdrPromiseDate { get; set; }
        public string OeHdrCustKey { get; set; }
        public string OeHdrShipToKey { get; set; }
        public string OeHdrShipToName { get; set; }
        public string OeHdrQuoteNumber { get; set; }
        public string FOBDescription { get; set; }
        public string ShipViaDescription { get; set; }
        public string TermsDescription { get; set; }
    }
}