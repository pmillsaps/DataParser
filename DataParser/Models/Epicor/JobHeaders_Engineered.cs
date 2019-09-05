namespace DataParser.Models
{
    internal class JobHeaders_Engineered
    {
        public string Company { get; set; }
        public string Plant { get; set; }
        public string JobNum { get; set; }
        public string PartNum { get; set; }
        public bool JobEngineered { get; set; }
        public bool JobReleased { get; set; }
        public bool JobFirm { get; set; }
    }
}