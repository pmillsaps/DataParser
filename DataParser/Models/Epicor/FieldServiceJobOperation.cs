using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models
{
    internal class FieldServiceJobOperation
    {
        public string Company { get; set; }
        public string Plant { get; set; }
        public string JobNum { get; set; }
        public string OpCode { get; set; }
        public int OprSeq { get; set; }
    }
}