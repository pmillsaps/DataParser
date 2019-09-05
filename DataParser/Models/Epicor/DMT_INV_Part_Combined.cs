using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models.Epicor
{
    public class DMT_INV_Part_Combined
    {
        public string Company { get; set; }
        public string PartNum { get; set; }
        public string SearchWord { get; set; }
        public string PartDescription { get; set; }
        public string UOMClassID { get; set; }
        public string IUM { get; set; }
        public string PUM { get; set; }
        public string SalesUM { get; set; }
        public string ClassID { get; set; }
        public string TypeCode { get; set; }
        public string ProdCode { get; set; }
        public string CostMethod { get; set; }
        public double UnitPrice { get; set; }
        public string QtyBearing { get; set; }
        public string InActive { get; set; }
        public string TrackSerialNum { get; set; }
        public string SNBaseDataType { get; set; }
        public string TrackLots { get; set; }
        public string BuyToOrder { get; set; }
        public string DropShip { get; set; }
        public string NonStock { get; set; }
        public string PhantomBOM { get; set; }
        public string VendGrup_c { get; set; }
        public string PartsPDF_c { get; set; }
        public string LiftFamily_c { get; set; }
        public string c_MTO_c { get; set; }
        public string HoseBuild_c { get; set; }
        public string HoseEndOne_c { get; set; }
        public string HoseEndTwo_c { get; set; }
        public double HoseLength_c { get; set; }
        public double HoseOverallLen_c { get; set; }
        public string HoseRawMtl_c { get; set; }
        public string HoseWrap_c { get; set; }
        public string CommentText { get; set; }
        public string Type_c { get; set; }
        public string RefDesignator_c { get; set; }
        public string PartPlant__Plant { get; set; }
        public string PartPlant__GenerateSugg { get; set; }
        public string PartPlant__ProcessMRP { get; set; }
        public string PartPlant__QtyBearing { get; set; }
        public string PartPlant__PrimWhse { get; set; }
        public string PartPlant__VendorNumVendorID { get; set; }
        public string PartPlant__NonStock { get; set; }
        public string PartPlant__PhantomBOM { get; set; }
        public string PartPlant__SNBaseDataType { get; set; }
        public string PartWhse__WarehouseCode { get; set; }
        public string PartWhse__PrimBinNum { get; set; }
        public string PartWhse__ManualABC { get; set; }
        public string PartWhse__SystemABC { get; set; }
        public string PartRev__RevisionNum { get; set; }
        public string PartRev__RevShortDesc { get; set; }
        public string PartRev__RevDescription { get; set; }
        public string PartRev__Approved { get; set; }
        public string PartRev__ApprovedBy { get; set; }
        public string PartRev__ApprovedDate { get; set; }
        public string PartRev__EffectiveDate { get; set; }
        public string DeleteAfterThis { get; set; }
        public string VendorName { get; set; }
    }
}