using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models
{
    internal class JobProd
    {
        public string Company { get; set; }
        public string Plant { get; set; }
        public int ProdQty { get; set; }
        public string PartNum { get; set; }
        public string JobNum { get; set; }
        public int OrderNum { get; set; }
        public int OrderLine { get; set; }
        public int OrderRelNum { get; set; }
        public string MakeToType { get; set; }
    }
}