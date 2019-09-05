using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser.Models.ASPN
{
    public class ItemLocationQOH
    {
        public string ItemNumber { get; set; }
        public string ItemDescription { get; set; }
        public string Location { get; set; }
        public string ItemClass { get; set; }
        public string ItemClassDescription { get; set; }
        public string InvControlAcct { get; set; }
        public string StdCstVarianceAcct { get; set; }
        public string StdcstvarMatOHacct { get; set; }
        public string AccruedReceiptAcct { get; set; }
        public string AccruedStdCstVarAcct { get; set; }
        public string Statuscode { get; set; }
        public string StatusDesc { get; set; }
        public string Effectivedate { get; set; }
        public string Stdcost { get; set; }
        public string Stdcostdate { get; set; }
        public double Avgcost { get; set; }
        public string Lstcost { get; set; }
        public string Distcost { get; set; }
        public string Baseprice { get; set; }
        public string Basepricedate { get; set; }
        public string QuantityOnHand { get; set; }
        public string QtyOnOrder { get; set; }
        public string QtyCommitSales { get; set; }
        public string QtyCommitProd { get; set; }
        public string CommitToShip { get; set; }
        public string ReorderPointQty { get; set; }
        public string MaxJobQuantity { get; set; }
        public string DfltBinno { get; set; }
        public string Revacct { get; set; }
        public string Cogsacct { get; set; }
        public string COGSAdjAcct { get; set; }
        public string Minstockqty { get; set; }
        public string Maxstockqty { get; set; }
        public string Reorderqty { get; set; }
        public string MaximumOrderQty { get; set; }
        public string SafetyStockLevel { get; set; }
        public string Primevend { get; set; }
        public string WIPClass { get; set; }
        public string WIPClassDescription { get; set; }
        public string WIPAccount_WCFixed { get; set; }
        public string WIPAccount_WCVar { get; set; }
        public string WIPAccount_Labor { get; set; }
        public string WIPAccount_LaborOH { get; set; }
        public string WIPAccount_Material { get; set; }
        public string WIPAccount_DP { get; set; }
        public string WIPAccount_SC { get; set; }
        public string Account_RejExpense { get; set; }
        public string Account_ScrapExpense { get; set; }
        public string Account_CancelledJobExpense { get; set; }
        public string Account_StdCostVarSC { get; set; }
        public string Account_StdCostVarDP { get; set; }
        public string Account_NegQtyAdjJobClose { get; set; }
    }
}