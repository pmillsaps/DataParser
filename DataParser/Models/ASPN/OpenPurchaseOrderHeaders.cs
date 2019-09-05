using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models.ASPN
{
    public class OpenPurchaseOrderHeaders
    {
        public string Statusflg { get; set; }
        public string Pono { get; set; }
        public string Orddate { get; set; }
        public string Fobkey { get; set; }
        public string Shipviakey { get; set; }
        public string InternalBuyer { get; set; }
        public string Vendkey { get; set; }
        public string FOBDescription { get; set; }
        public string ShipViaDescription { get; set; }
    }
}