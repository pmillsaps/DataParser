using AutoMapper;
using CsvHelper;
using DataParser.Models;
using DataParser.Models.ASPN;
using DataParser.Models.Epicor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DataParser
{
    public class ASPN
    {
        private static IMapper mapper { get; set; }
        private static string ErrorList { get; set; }
        private static string MissingParts { get; set; }
        private static string MissingCustomers { get; set; }
        private static string MissingSuppliers { get; set; }
        private static string MissingAdjustments { get; set; }
        private static List<string> UOMList { get; set; }
        private static List<UOMConvert> ASPN_UOMConvert { get; set; }

        private static List<DMT_BillOfMaterial> dmt_BOM { get; set; }
        private static List<DMT_BillOfOperations> dmt_BOO { get; set; }

        private static List<string> IgnoredPO = new List<string>();

        //private static List<JobOperation> stdoperations = new List<JobOperation>();
        private static List<DMT_INV_Part_Combined> currentParts { get; set; }

        private static List<SupplierList> currentVendors { get; set; }
        private static List<DMT_CUS_Customers> currentCustomers { get; set; }

        //private static List<DefaultWhse_Bins> whse { get; set; }
        //private static List<JobCostUnbilledWIPv2_VER> wipJobs { get; set; }
        //private static List<JobCostUnbilledWIPv2_VER> cleanWipJobs { get; set; }
        //private static List<JobXRef> JobXRefList { get; set; }
        //private static List<JobCostJobMasterfile> SageJobHeaders { get; set; }
        private static List<OrderHeader> OrderHeaders { get; set; }

        private static List<OrderDetail> OrderDetails { get; set; }
        private static List<OrderRelease> OrderReleases { get; set; }
        private static List<JobHeader> JobHeaders { get; set; }
        private static List<JobOperation> JobOperations { get; set; }
        private static List<JobMaterial> JobMaterials { get; set; }
        private static List<JobProd> JobProds { get; set; }
        private static List<JobMtlAdjustment> JobMtlAdjustments { get; set; }
        private static List<POHeader> POHeaders { get; set; }
        private static List<PODetail> PODetails { get; set; }
        public static List<PoRelease> POReleases { get; set; }
        private static List<POHeader> POHeaderApprovals { get; set; }
        private static List<Epicor_APOpenInvoiceLoad> APOpenInvoiceLoads { get; set; }
        private static List<Epicor_AROpenInvoiceLoad> AROpenInvoiceLoads { get; set; }
        private static List<JobOrderPlanHeader> currentJobs { get; set; }
        private static List<JobOrderPlanOperations> currentJobOperations { get; set; }
        private static List<JobOrderPlanBOM> currentJobMaterials { get; set; }
        private static List<JobOrderPlanDPSC> currentJobSubCon { get; set; }
        private static List<JobTransactionsBOM> currentJobTranBOM { get; set; }
        private static List<JobTransactionsOperation> currentJobTranOper { get; set; }
        private static List<ProcessPlanOperations> ASPN_ProcessPlanOperations { get; set; }
        private static List<ProcessPlanHeader> ASPN_ProcessPlanHeader { get; set; }
        private static List<ProcessPlanBOM> ASPN_ProcessPlanBOM { get; set; }
        private static List<ItemMaster> ASPN_ItemMaster { get; set; }
        private static List<ItemLocationQOH> ASPN_ItemLocationQOH { get; set; }
        private static List<FinalProcessPlans> ASPN_FinalProcessPlans { get; set; }
        private static List<DMT_INV_Part_Combined_Clean> DMT_PartCombined { get; set; }

        //private static List<Part> EpicorParts { get; set; }
        private static List<CostAdjustment> EpicorCostAdjustments { get; set; }

        private static List<QuantityAdjustment> EpicorQuantityAdjustments { get; set; }
        private static List<IssueMaterial> EpicorIssueMaterials { get; set; }
        public static object EpicorIssueMater { get; private set; }

        private const string SubFolder = @"Output\";
        private const string CompanyID = "ASPN";

        public static void Process()
        {
            if (!Directory.Exists(SubFolder))
            {
                Directory.CreateDirectory(SubFolder);
            }

            //IgnoredPO = new List<string>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ASPN_MappingProfile>();
            });
            mapper = config.CreateMapper();

            DMT_PartCombined = new List<DMT_INV_Part_Combined_Clean>();
            ErrorList = "";
            MissingParts = "";
            ASPN_UOMConvert = BuildASPN_UOMConvert();
            currentParts = BuildCurrentParts();
            currentCustomers = BuildCurrentCustomers();
            currentJobs = BuildCurrentJobs();
            currentJobOperations = BuildCurrentJobOperations();
            currentJobMaterials = BuildCurrentJobMaterial();
            currentJobSubCon = BuildCurrentJobSubCon();
            currentJobTranBOM = BuildCurrentJobTranBOM();
            currentJobTranOper = BuildCurrentJobTranOper();
            ASPN_ProcessPlanHeader = BuildASPN_ProcessPlanHeader();
            ASPN_ProcessPlanBOM = BuildASPN_ProcessPlanBOM();
            ASPN_ProcessPlanOperations = BuildASPN_ProcessPlanOperations();
            ASPN_ItemMaster = BuildASPN_ItemMaster();
            ASPN_ItemLocationQOH = BuildASPN_ItemLocationQOH();
            ASPN_FinalProcessPlans = BuildASPN_FinalProcessPlans();

            BuildBoo_BOM();

            //BuildIgnoredPO();

            //currentVendors = BuildCurrentVendors();

            //EpicorStandardOperations = BuildEpicorStandardOperations();
            //BuildStandardOperations();
            //CreatePurchaseOrders();
            //serviceJobs = BuildServiceJobList();

            //Create_AR_OpenInvoiceLoad();
            CreateInstallJobs();

            //CreateServiceJobs();
            //Create_AP_OpenInvoiceLoad();

            //whse = BuildDefaultBins();

            //wipJobs = BuildWipList();
            //WriteWIPData("WipJobs.csv", wipJobs);
            //cleanWipJobs = CleanWipList(wipJobs);
            //WriteWIPData("WipJobs_Net.csv", cleanWipJobs);
            //JobXRefList = BuildJobTranslationList();
            //WriteJobTranslationList("JobXRefList.csv", JobXRefList);
            //BuildSageJobHeaders();

            //CreateinventoryAdjustments();
            //CreateJobAdjustments();

            //CreatePartsOrders();
            //CreateInstallJobs();
            //CreateOtherJobs();

            #region Write Data Files

            WriteDMTBOM("DMT-ENG-BillOfMaterials.csv", dmt_BOM);
            WriteDMTBOO("DMT-ENG-BillOfOperations.csv", dmt_BOO);
            WriteDMTInvPartCombined("DMT-INV-PartCombined-ASPN.csv", DMT_PartCombined);

            WriteOrderHeaders("GL05-OrderHeaders.csv", OrderHeaders);
            WriteOrderDetails("GL06-OrderDetails.csv", OrderDetails);
            WriteOrderReleases("GL07-OrderReleases.csv", OrderReleases);

            WriteJobHeaders("GL10-JobHeaders.csv", JobHeaders);
            WriteJobOperations("GL11-JobOperations.csv", JobOperations);
            WriteJobMaterials("GL12-JobMaterials.csv", JobMaterials);
            WriteJobProds("GL13-JobProd.csv", JobProds);
            //WriteIssueMaterials("GL14-IssueMaterial.csv", EpicorIssueMaterials);
            //WriteJobMtlAdjustment("GL49-JobMtlAdjustments.csv", JobMtlAdjustments);

            //WritePOHeaders("GL20-POHeaders.csv", POHeaders);
            //WritePODetails("GL21-PODetails.csv", PODetails);
            //WritePOReleases("GL22-POReleases.csv", POReleases);
            //WritePOHeaderApprovals("GL23-POHeaderApprovals.csv", POHeaderApprovals);

            //WriteParts("GL00-Parts.csv", EpicorParts);
            //WriteCostAdjustments("GL01-CostAdjustments.csv", EpicorCostAdjustments);
            //WriteQtyAdjustmentss("GL02-QuantityAdjustments.csv", EpicorQuantityAdjustments);
            //WritePOHeaders("GL20-POHeaders.csv", POHeaders);

            //WriteAPInvoiceLoad("GLXX-OpenAPInvoiceLoad.csv", APOpenInvoiceLoads);
            //WriteARInvoiceLoad("GLXX-OpenARInvoiceLoad.csv", AROpenInvoiceLoads);

            //CreateBatchFile();
            if (ErrorList.Length > 5) File.WriteAllText(@"Output\ErrorList.txt", ErrorList);
            if (MissingParts.Length > 5) File.WriteAllText(@"Output\MissingParts.txt", MissingParts);
            if (!String.IsNullOrEmpty(MissingCustomers) && MissingCustomers.Length > 5) File.WriteAllText(@"Output\MissingCustomers.txt", MissingCustomers);
            if (!String.IsNullOrEmpty(MissingSuppliers) && MissingSuppliers.Length > 5) File.WriteAllText(@"Output\MissingSuppliers.txt", MissingSuppliers);
            if (!String.IsNullOrEmpty(MissingAdjustments) && MissingAdjustments.Length > 5) File.WriteAllText(@"Output\MissingAdjustments.txt", MissingAdjustments);

            #endregion Write Data Files

            // Open Output Folder
            //Process.Start(cmd, arg);
            openInExplorer(Path.Combine(Environment.CurrentDirectory, "Output"));
        }

        private static void BuildBoo_BOM()
        {
            Console.WriteLine(Extensions.GetCurrentMethod());
            dmt_BOM = new List<DMT_BillOfMaterial>();
            dmt_BOO = new List<DMT_BillOfOperations>();

            var headers = ASPN_ProcessPlanHeader.Where(x => !x.PlanItemNumber.StartsWith("UB") && !x.PlanItemNumber.StartsWith("W") && !x.PlanItemNumber.StartsWith("S") && !x.ItemDescription.ToUpper().Contains("REBUILD"));
            // Check if we have a PN for the item
            // Will need to filter UB list later
            foreach (var item in headers)
            {
                var final = ASPN_FinalProcessPlans.Any(x => x.PartNum == item.PlanItemNumber && x.Revision == item.PlanVersion);

                if (!(item.PlanItemNumber.StartsWith("A") || item.PlanItemNumber.StartsWith("B")) || final)
                {
                    //if (String.IsNullOrEmpty(item.PlanVersion)) item.PlanVersion = "NEW";

                    if (!currentParts.Any(x => x.PartNum == item.PlanItemNumber))
                    {
                        if (ASPN_ItemMaster.Any(x => x.Itemkey == item.PlanItemNumber))
                        {
                            var aspn_part = ASPN_ItemMaster.First(x => x.Itemkey == item.PlanItemNumber);
                            if (String.IsNullOrEmpty(aspn_part.TransUOM)) aspn_part.TransUOM = "EA";

                            var uom = ASPN_UOMConvert.First(x => x.Aspen_UOM == aspn_part.TransUOM);
                            var part = new DMT_INV_Part_Combined_Clean
                            {
                                Company = CompanyID,
                                PartNum = aspn_part.Itemkey,
                                PartDescription = aspn_part.Desc1 + aspn_part.Desc2,
                                IUM = uom.Epicor_UOM,
                                PUM = uom.Epicor_UOM,
                                SalesUM = uom.Epicor_UOM,
                                UOMClassID = uom.Epicor_UOMClass,
                                InActive = "False",
                                ClassID = "TBD",
                                ProdCode = "PAR",
                                CostMethod = "A",
                                BuyToOrder = "False",
                                DropShip = "False",
                                NonStock = "False",
                                PartPlant__NonStock = "False",
                                QtyBearing = "True",
                                SearchWord = aspn_part.Desc1.Substring(0, 8),
                                TypeCode = "M",
                                TrackSerialNum = "False",
                                TrackLots = "False",
                                PartPlant__Plant = "MfgSys",
                                PartPlant__PhantomBOM = "False",
                                PartPlant__PrimWhse = "Duluth",
                                PartPlant__VendorNumVendorID = aspn_part.Manufacturer,
                                PartWhse__WarehouseCode = "Duluth",
                                PartWhse__PrimBinNum = "NoBin",
                                PartRev__RevisionNum = item.PlanVersion.NonBlankValueOf(),
                                PartRev__RevShortDesc = item.PlanVersion.NonBlankValueOf(),
                                PartRev__RevDescription = item.PlanVersion.NonBlankValueOf(),
                                PartRev__Approved = "True",
                                PartRev__ApprovedBy = "manager",
                                PartRev__ApprovedDate = "01/01/2019",
                                PartRev__EffectiveDate = "01/01/2000",
                                //CommentText = aspn_part.Cost_Dec,
                            };

                            if (final) part.ProdCode = "INS";

                            if (ASPN_ItemLocationQOH.Any(x => x.ItemNumber == part.PartNum))
                            {
                                var locqoh = ASPN_ItemLocationQOH.First(x => x.ItemNumber == part.PartNum);
                                part.PartWhse__PrimBinNum = locqoh.Location;
                                part.UnitPrice = locqoh.Avgcost;
                            }
                            if (!DMT_PartCombined.Any(x => x.PartNum == part.PartNum && x.PartRev__RevisionNum == part.PartRev__RevisionNum)) DMT_PartCombined.Add(part);
                        }
                        else
                        {
                            MissingParts += String.Format("Process Plan Header - Part:{0} | {1} | {2} does not exist in Epicor and does not exist in the NDustrious item master", item.PlanItemNumber, item.PlanVersion, item.ItemDescription);
                            MissingParts += Environment.NewLine;
                        }
                    }
                    else
                    {
                        if ((!currentParts.Any(x => x.PartNum == item.PlanItemNumber && x.PartRev__RevisionNum == item.PlanVersion)) && (!currentParts.Any(x => x.PartNum == item.PlanItemNumber && x.PartRev__RevisionNum == "NEW")))
                        {
                            try
                            {
                                var prt1 = currentParts.First(x => x.PartNum == item.PlanItemNumber);
                                item.PlanVersion = prt1.PartRev__RevisionNum;
                            }
                            catch (Exception)
                            {
                                // Apparently there were multiple revisions available.
                                MissingParts += String.Format("Process Plan Header-Revision:{0} | {1} | {2} multiple rev in Epicor - cannot choose default", item.PlanItemNumber, item.PlanVersion, item.ItemDescription);
                                MissingParts += Environment.NewLine;
                            }
                            if ((!currentParts.Any(x => x.PartNum == item.PlanItemNumber && x.PartRev__RevisionNum == item.PlanVersion)) && (!currentParts.Any(x => x.PartNum == item.PlanItemNumber && x.PartRev__RevisionNum == "NEW")))
                            {
                                MissingParts += String.Format("Process Plan Header-Revision:{0} | {1} | {2} does not exist in Epicor", item.PlanItemNumber, item.PlanVersion, item.ItemDescription);
                                MissingParts += Environment.NewLine;
                            }
                        }
                    }

                    var planops = ASPN_ProcessPlanOperations.Where(x => x.PlanItemNumber == item.PlanItemNumber && x.PlanVersion == item.PlanVersion && x.SeqDetailType != "W");
                    foreach (var op in planops)
                    {
                        var OP = new DMT_BillOfOperations
                        {
                            Company = CompanyID,
                            PartNum = op.PlanItemNumber,
                            ECOGroupID = "PEM",
                            QtyPer = 1,
                            RevisionNum = op.PlanVersion.NonBlankValueOf(),
                            StdFormat = "HP",
                            LaborEntryMethod = "T",
                            SchedRelation = "FS",
                            OpCode = op.WCLabActivty,
                            OprSeq = ConvertOprSeq(op.OpSequence),
                            ProdStandard = op.RunTime,
                            EstProdHours = op.RunTime,
                            EstSetHours = op.SetupTimeInHrs,
                            ECOOpDtl__ResourceGrpID = op.WCLabActivty,
                        };

                        //if (String.IsNullOrEmpty(OP.RevisionNum)) OP.RevisionNum = "NEW";

                        dmt_BOO.Add(OP);
                    }

                    // placed filter to make sure I do not add a BOM for the root part
                    var planBOM = ASPN_ProcessPlanBOM.Where(x => x.PlanItemNumber == item.PlanItemNumber && x.ComponentItemNumber != item.PlanItemNumber);
                    foreach (var mtl in planBOM)
                    {
                        if (!currentParts.Any(x => x.PartNum == mtl.ComponentItemNumber))
                        {
                            if (ASPN_ItemMaster.Any(x => x.Itemkey == mtl.ComponentItemNumber))
                            {
                                var aspn_part = ASPN_ItemMaster.First(x => x.Itemkey == mtl.ComponentItemNumber);
                                var part = new DMT_INV_Part_Combined_Clean
                                {
                                    Company = CompanyID,
                                    PartNum = aspn_part.Itemkey,
                                    PartDescription = aspn_part.Desc1 + aspn_part.Desc2,
                                    IUM = aspn_part.StockUOM,
                                    PUM = aspn_part.StockUOM,
                                    SalesUM = aspn_part.StockUOM,
                                    UOMClassID = "Count",
                                    InActive = "False",
                                    ClassID = "TBD",
                                    ProdCode = "PAR",
                                    CostMethod = "A",
                                    BuyToOrder = "False",
                                    DropShip = "False",
                                    NonStock = "False",
                                    PartPlant__NonStock = "False",
                                    QtyBearing = "True",
                                    SearchWord = aspn_part.Desc1.Substring(0, 8),
                                    TypeCode = "M",
                                    TrackSerialNum = "False",
                                    TrackLots = "False",
                                    PartPlant__Plant = "MfgSys",
                                    PartPlant__PhantomBOM = "False",
                                    PartPlant__PrimWhse = "Duluth",
                                    PartPlant__VendorNumVendorID = aspn_part.Manufacturer,
                                    PartWhse__WarehouseCode = "Duluth",
                                    PartWhse__PrimBinNum = "NoBin",
                                    PartRev__RevisionNum = item.PlanVersion.NonBlankValueOf(),
                                    PartRev__RevShortDesc = item.PlanVersion.NonBlankValueOf(),
                                    PartRev__RevDescription = item.PlanVersion.NonBlankValueOf(),
                                    PartRev__Approved = "True",
                                    PartRev__ApprovedBy = "manager",
                                    PartRev__ApprovedDate = "01/01/2019",
                                    PartRev__EffectiveDate = "01/01/2000",
                                    //CommentText = aspn_part.Cost_Dec,
                                };

                                if (ASPN_ItemLocationQOH.Any(x => x.ItemNumber == part.PartNum))
                                {
                                    var locqoh = ASPN_ItemLocationQOH.First(x => x.ItemNumber == part.PartNum);
                                    part.PartWhse__PrimBinNum = locqoh.Location;
                                    part.UnitPrice = locqoh.Avgcost;
                                }

                                if (!DMT_PartCombined.Any(x => x.PartNum == part.PartNum && x.PartRev__RevisionNum == part.PartRev__RevisionNum)) DMT_PartCombined.Add(part);
                            }
                            else
                            {
                                if (mtl.ComponentItemNumber.StartsWith("A") || mtl.ComponentItemNumber.StartsWith("B") || mtl.ComponentItemNumber.StartsWith("C") || mtl.ComponentItemNumber.StartsWith("D"))
                                {
                                    var part = new DMT_INV_Part_Combined_Clean
                                    {
                                        Company = CompanyID,
                                        PartNum = mtl.ComponentItemNumber,
                                        PartDescription = mtl.Description1,
                                        IUM = "EA",
                                        PUM = "EA",
                                        SalesUM = "EA",
                                        UOMClassID = "Count",
                                        InActive = "False",
                                        ClassID = "TBD",
                                        ProdCode = "PAR",
                                        CostMethod = "A",
                                        BuyToOrder = "False",
                                        DropShip = "False",
                                        NonStock = "False",
                                        PartPlant__NonStock = "False",
                                        QtyBearing = "False",
                                        SearchWord = mtl.Description1.Substring(0, 8),
                                        TypeCode = "M",
                                        TrackLots = "False",
                                        TrackSerialNum = "False",
                                        PartPlant__Plant = "MfgSys",
                                        PartPlant__PhantomBOM = "False",
                                        PartPlant__PrimWhse = "Duluth",
                                        //PartPlant__VendorNumVendorID = aspn_part.Manufacturer,
                                        PartWhse__WarehouseCode = "Duluth",
                                        PartWhse__PrimBinNum = "NoBin",
                                        //PartRev__RevisionNum = item.PlanVersion.NonBlankValueOf(),
                                        //PartRev__RevShortDesc = item.PlanVersion.NonBlankValueOf(),
                                        //PartRev__RevDescription = item.PlanVersion.NonBlankValueOf(),
                                        //PartRev__Approved = "True",
                                        //PartRev__ApprovedBy = "manager",
                                        //PartRev__ApprovedDate = "01/01/2019",
                                        //PartRev__EffectiveDate = "01/01/2000",
                                        //CommentText = aspn_part.Cost_Dec,
                                    };

                                    if (ASPN_ItemLocationQOH.Any(x => x.ItemNumber == part.PartNum))
                                    {
                                        var locqoh = ASPN_ItemLocationQOH.First(x => x.ItemNumber == part.PartNum);
                                        part.PartWhse__PrimBinNum = locqoh.Location;
                                        part.UnitPrice = locqoh.Avgcost;
                                    }

                                    if (!DMT_PartCombined.Any(x => x.PartNum == part.PartNum && x.PartRev__RevisionNum == part.PartRev__RevisionNum)) DMT_PartCombined.Add(part);
                                }
                                else
                                {
                                    MissingParts += String.Format("Process Plan Item:{0} | {1} | {2} | {3} does not exist in Epicor or in the NDustrious Item Master", mtl.PlanItemNumber, mtl.PlanVersion, mtl.ComponentItemNumber, mtl.Description1);
                                    MissingParts += Environment.NewLine;
                                }
                            }
                        }
                        else
                        {
                            var MTL = new DMT_BillOfMaterial
                            {
                                Company = CompanyID,
                                PartNum = mtl.PlanItemNumber,
                                RevisionNum = mtl.PlanVersion.NonBlankValueOf(),
                                ECOGroupID = "PEM",
                                MtlPartNum = mtl.ComponentItemNumber,
                                QtyPer = mtl.QtyPer,
                                RelatedOperation = ConvertOprSeq(mtl.OpSequence),
                                FixedQty = mtl.FlatUnit == "F",
                                UOMCode = currentParts.First(x => x.PartNum == mtl.ComponentItemNumber).IUM,
                                MtlSeq = 0,
                            };

                            //if (String.IsNullOrEmpty(MTL.RevisionNum)) MTL.RevisionNum = "NEW";

                            dmt_BOM.Add(MTL);
                        }
                    }
                }
            }

            dmt_BOM = ResequenceBOM(dmt_BOM);
        }

        private static List<DMT_BillOfMaterial> ResequenceBOM(List<DMT_BillOfMaterial> cleanBOM)
        {
            Console.WriteLine("ReSequencing BOM's...");
            var newList = cleanBOM.OrderBy(x => x.PartNum).ThenBy(x => x.RevisionNum).ThenBy(x => x.RelatedOperation).ThenBy(x => x.MtlPartNum).ToList();
            int sequence = 10;
            string holdPart = "";
            foreach (var item in newList)
            {
                string testPart = item.PartNum + "|" + item.RevisionNum;
                if (testPart != holdPart)
                {
                    sequence = 10;
                    holdPart = testPart;
                    Console.WriteLine("ReSequencing {0} - {1}", item.PartNum, item.RevisionNum);
                }

                item.MtlSeq = sequence;
                sequence += 10;

                if (cleanBOM.Any(x => x.PartNum == item.MtlPartNum))
                {
                    item.PullAsAsm = true;
                    item.ViewAsAsm = true;
                    item.PlanAsAsm = false;
                }
            }

            return newList;
        }

        private static void openInExplorer(string path)
        {
            string cmd = "explorer.exe";
            string arg = path;
            System.Diagnostics.Process.Start(cmd, arg);
        }

        private static List<JobOrderPlanHeader> BuildCurrentJobs()
        {
            Console.WriteLine(Extensions.GetCurrentMethod());
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", CompanyID, "JobOrderPlanHeader.txt");
            filePath = FixASPNFileHeader(filePath);

            StreamReader textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            //csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            csv.Configuration.Quote = '\'';
            var list = csv.GetRecords<JobOrderPlanHeader>().ToList();
            foreach (var item in list.Where(x => x.ItemNumber.StartsWith(".")))
            {
                item.ItemNumber = item.ItemNumber.Substring(1);
            }

            //.OrderBy(x => x.CustID).ToList();
            csv.Dispose();
            textreader.Close();
            return list;
        }

        private static List<JobOrderPlanOperations> BuildCurrentJobOperations()
        {
            Console.WriteLine(Extensions.GetCurrentMethod());
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", CompanyID, "JobOrderPlanOperations.txt");
            filePath = FixASPNFileHeader(filePath);

            StreamReader textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            //csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            csv.Configuration.Quote = '\'';
            var list = csv.GetRecords<JobOrderPlanOperations>().ToList();
            //.OrderBy(x => x.CustID).ToList();
            csv.Dispose();
            textreader.Close();
            return list;
        }

        private static List<JobOrderPlanBOM> BuildCurrentJobMaterial()
        {
            Console.WriteLine(Extensions.GetCurrentMethod());
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", CompanyID, "JobOrderPlanBOM.txt");
            filePath = FixASPNFileHeader(filePath);

            StreamReader textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            //csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            csv.Configuration.Quote = '\'';
            csv.Configuration.MissingFieldFound = null;
            var list = csv.GetRecords<JobOrderPlanBOM>().ToList();
            //.OrderBy(x => x.CustID).ToList();
            csv.Dispose();
            textreader.Close();
            return list;
        }

        private static List<JobOrderPlanDPSC> BuildCurrentJobSubCon()
        {
            Console.WriteLine(Extensions.GetCurrentMethod());
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", CompanyID, "JobOrderPlanDPSC.txt");
            filePath = FixASPNFileHeader(filePath);

            StreamReader textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            //csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            csv.Configuration.Quote = '\'';
            csv.Configuration.MissingFieldFound = null;
            var list = csv.GetRecords<JobOrderPlanDPSC>().ToList();
            //.OrderBy(x => x.CustID).ToList();
            csv.Dispose();
            textreader.Close();
            return list;
        }

        private static List<JobTransactionsBOM> BuildCurrentJobTranBOM()
        {
            Console.WriteLine(Extensions.GetCurrentMethod());
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", CompanyID, "JobTransactionsBOM.txt");
            filePath = FixASPNFileHeader(filePath);

            StreamReader textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            //csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            csv.Configuration.Quote = '\'';
            csv.Configuration.MissingFieldFound = null;
            var list = csv.GetRecords<JobTransactionsBOM>().ToList();
            //.OrderBy(x => x.CustID).ToList();
            csv.Dispose();
            textreader.Close();
            return list;
        }

        private static List<JobTransactionsOperation> BuildCurrentJobTranOper()
        {
            Console.WriteLine(Extensions.GetCurrentMethod());
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", CompanyID, "JobTransactionsOperation.txt");
            filePath = FixASPNFileHeader(filePath);

            StreamReader textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            //csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            csv.Configuration.Quote = '\'';
            csv.Configuration.MissingFieldFound = null;
            var list = csv.GetRecords<JobTransactionsOperation>().ToList();
            //.OrderBy(x => x.CustID).ToList();
            csv.Dispose();
            textreader.Close();
            return list;
        }

        private static List<ProcessPlanOperations> BuildASPN_ProcessPlanOperations()
        {
            Console.WriteLine(Extensions.GetCurrentMethod());
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", CompanyID, "ProcessPlanOperations.txt");
            filePath = FixASPNFileHeader(filePath);

            StreamReader textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            //csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            csv.Configuration.Quote = '\'';
            csv.Configuration.MissingFieldFound = null;
            var list = csv.GetRecords<ProcessPlanOperations>().ToList();
            foreach (var item in list.Where(item => item.PlanItemNumber.StartsWith(".")).Select(item => item))
            {
                item.PlanItemNumber = item.PlanItemNumber.Substring(1);
            }

            csv.Dispose();
            textreader.Close();
            return list;
        }

        private static List<ProcessPlanHeader> BuildASPN_ProcessPlanHeader()
        {
            Console.WriteLine(Extensions.GetCurrentMethod());
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", CompanyID, "ProcessPlanHeader.txt");
            filePath = FixASPNFileHeader(filePath);

            StreamReader textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            //csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            csv.Configuration.Quote = '\'';
            csv.Configuration.MissingFieldFound = null;
            var list = csv.GetRecords<ProcessPlanHeader>().ToList();
            foreach (var item in list.Where(item => item.PlanItemNumber.StartsWith(".")).Select(item => item))
            {
                item.PlanItemNumber = item.PlanItemNumber.Substring(1);
            }

            csv.Dispose();
            textreader.Close();
            return list;
        }

        private static List<ProcessPlanBOM> BuildASPN_ProcessPlanBOM()
        {
            Console.WriteLine(Extensions.GetCurrentMethod());
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", CompanyID, "ProcessPlanBOM.txt");
            filePath = FixASPNFileHeader(filePath);

            StreamReader textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            //csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            csv.Configuration.Quote = '\'';
            csv.Configuration.MissingFieldFound = null;
            var list = csv.GetRecords<ProcessPlanBOM>().ToList();
            foreach (var item in list)
            {
                if (item.PlanItemNumber.StartsWith(".")) item.PlanItemNumber = item.PlanItemNumber.Substring(1);
                if (item.ComponentItemNumber.StartsWith(".")) item.ComponentItemNumber = item.ComponentItemNumber.Substring(1);
            }

            csv.Dispose();
            textreader.Close();
            return list;
        }

        private static List<ItemMaster> BuildASPN_ItemMaster()
        {
            Console.WriteLine(Extensions.GetCurrentMethod());
            UOMList = new List<string>();
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", CompanyID, "ItemMaster.txt");
            filePath = FixASPNFileHeader(filePath);

            StreamReader textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            //csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            csv.Configuration.Quote = '\'';
            //csv.Configuration.MissingFieldFound = null;
            var list = csv.GetRecords<ItemMaster>().OrderBy(x => x.Itemkey).ToList();
            foreach (var item in list)
            {
                if (item.Itemkey.StartsWith("."))
                {
                    item.Itemkey = item.Itemkey.Substring(1);
                }

                if (!UOMList.Contains(item.TransUOM))
                {
                    UOMList.Add(item.TransUOM);
                }
            }

            csv.Dispose();
            textreader.Close();
            return list;
        }

        private static List<UOMConvert> BuildASPN_UOMConvert()
        {
            Console.WriteLine(Extensions.GetCurrentMethod());
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", CompanyID, "UOMConvert.csv");
            //filePath = FixASPNFileHeader(filePath);

            StreamReader textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            csv.Configuration.BadDataFound = null;
            var list = csv.GetRecords<UOMConvert>().ToList();

            csv.Dispose();
            textreader.Close();
            return list;
        }

        private static List<ItemLocationQOH> BuildASPN_ItemLocationQOH()
        {
            Console.WriteLine(Extensions.GetCurrentMethod());
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", CompanyID, "ItemLocationQOH.txt");
            filePath = FixASPNFileHeader(filePath);

            StreamReader textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            //csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            csv.Configuration.Quote = '\'';
            //csv.Configuration.MissingFieldFound = null;
            var list = csv.GetRecords<ItemLocationQOH>().OrderBy(x => x.ItemNumber).ToList();
            foreach (var item in list.Where(item => item.ItemNumber.StartsWith(".")).Select(item => item))
            {
                item.ItemNumber = item.ItemNumber.Substring(1);
            }

            csv.Dispose();
            textreader.Close();
            return list;
        }

        private static List<FinalProcessPlans> BuildASPN_FinalProcessPlans()
        {
            Console.WriteLine(Extensions.GetCurrentMethod());
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", CompanyID, "FinalProcessPlans.txt");
            //filePath = FixASPNFileHeader(filePath);

            StreamReader textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            //csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            //csv.Configuration.Quote = '\'';
            //csv.Configuration.MissingFieldFound = null;
            var list = csv.GetRecords<FinalProcessPlans>().OrderBy(x => x.PartNum).ToList();

            csv.Dispose();
            textreader.Close();
            return list;
        }

        private static string FixASPNFileHeader(string filePath)
        {
            String newFilePath = filePath + ".Fixed.csv";
            StringBuilder newString = new StringBuilder();
            StreamReader sr = new StreamReader(filePath);
            StreamWriter sw = new StreamWriter(newFilePath);

            var line = sr.ReadLine();
            line = line.Replace("'", "").Replace("$", "").Replace("?", "").Replace(".", "").Replace(" ", "_").Replace("#", "_");
            var headercnt = Extensions.CountStringOccurrences(line, ",");

            while (line != null)
            {
                line = line.Replace("\"", "in");

                sw.WriteLine(line);
                line = sr.ReadLine();
                if (!String.IsNullOrEmpty(line))
                {
                    var cnt = Extensions.CountStringOccurrences(line, ",");
                    int counter = 1;
                    while (cnt < headercnt)
                    {
                        //if (counter > 2) Console.WriteLine(cnt);
                        //Console.WriteLine(cnt);
                        var holdLine = line;
                        line = sr.ReadLine();
                        line = holdLine + line;
                        cnt = Extensions.CountStringOccurrences(line, ",");
                        //if (cnt < headercnt) Console.WriteLine(cnt);
                        counter++;
                    }
                }
            }

            sr.Close();
            sw.Close();

            return newFilePath;
        }

        private static string FixEpicorFileHeader(string filePath)
        {
            String newFilePath = filePath + ".Fixed.csv";
            StringBuilder newString = new StringBuilder();
            StreamReader sr = new StreamReader(filePath);
            StreamWriter sw = new StreamWriter(newFilePath);

            var line = sr.ReadLine();
            line = line.Replace("'", "").Replace("$", "").Replace("?", "").Replace(".", "")
                .Replace(" ", "").Replace("#", "__").Replace("0", "");

            while (line != null)
            {
                //line = line.Replace("\"", "in");
                sw.WriteLine(line);
                line = sr.ReadLine();
            }

            sr.Close();
            sw.Close();

            return newFilePath;
        }

        private static List<DMT_INV_Part_Combined> BuildCurrentParts()
        {
            Console.WriteLine(Extensions.GetCurrentMethod());
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", CompanyID, "DMT-INV-Part_Combined.csv");
            filePath = FixEpicorFileHeader(filePath);
            StreamReader textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            //csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            var list = csv.GetRecords<DMT_INV_Part_Combined>().OrderBy(x => x.PartNum).ToList();
            csv.Dispose();
            textreader.Close();
            return list;
        }

        private static void BuildIgnoredPO()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", CompanyID, "IgnoredPO.txt");
            IgnoredPO = File.ReadAllLines(filePath).ToList();
        }

        private static void CreatePurchaseOrders()
        {
            string missingSuppliers2 = "";
            //string missingSuppliers2 = "Unique Supplier Missing List:" + Environment.NewLine;

            POHeaders = new List<POHeader>();
            PODetails = new List<PODetail>();
            POReleases = new List<PoRelease>();
            POHeaderApprovals = new List<POHeader>();
            var SqlPOCostAdjustment = String.Empty;

            var poheads = BuildPurchaseOrderHeaders();
            //var polines = BuildPurchaseOrderDetails();
            //var poheaders = JobXRefList.Where(x => !String.IsNullOrEmpty(x.OldPO));
            int polinenum = 1;

            foreach (var item in poheads.Where(x => x.Quantity_Required - x.Quantity_Received > 0))
            {
                var poNumber = int.Parse(item.PONum.Replace("ON ", ""));

                var pohead = POHeaders.FirstOrDefault(x => x.PONum == poNumber);

                if (pohead == null) // Only add the PO Header if it does not yet exist
                {
                    polinenum = 1;  // Reset line number for a new PO
                    //var head = poheads.First(x => x.PurchaseOrderNo == item.OldPO);
                    var vendor = currentVendors.FirstOrDefault(x => x.OldVendorID_c == item.Supplier);
                    if (vendor == null)
                    {
                        vendor = currentVendors.FirstOrDefault(x => x.OperaCode == item.Supplier);
                    }

                    string vendorid = "Missing";
                    if (vendor == null)
                    {
                        MissingSuppliers += String.Format("Old PO:{0} has Vendor: {1} which does not exist in Epicor",
                                   item.PONum, item.Supplier);
                        MissingSuppliers += Environment.NewLine;

                        if (!missingSuppliers2.Contains(item.Supplier + Environment.NewLine)) missingSuppliers2 += item.Supplier + Environment.NewLine;
                    }
                    else
                        vendorid = vendor.VendorID;
                    var defaultFOB = String.Empty;
                    //if (vendor == null) defaultFOB = "Bethlehem"; else defaultFOB = vendor.;

                    pohead = new POHeader
                    {
                        Company = CompanyID,
                        BuyerID = "01",
                        Approve = false,
                        PONum = int.Parse(item.PONum.Replace("ON ", "")),
                        FOB = defaultFOB,
                        OrderDate = item.Date_Created,
                        VendorVendorID = vendorid,
                        ShipViaCode = "BEST"
                    };
                    POHeaders.Add(pohead);

                    var poheadapp = new POHeader
                    {
                        Company = CompanyID,
                        BuyerID = "01",
                        Approve = true,
                        PONum = int.Parse(item.PONum.Replace("ON ", "")),
                        FOB = defaultFOB,
                        OrderDate = item.Date_Created,
                        VendorVendorID = vendorid,
                        ShipViaCode = "BEST"
                    };
                    POHeaderApprovals.Add(poheadapp);
                }

                #region PO Lines

                if (String.IsNullOrEmpty(item.Stock_Reference))
                {
                    item.Stock_Reference = item.Reference;
                }
                var part = currentParts.FirstOrDefault(x => x.PartNum == item.Stock_Reference);

                var poline = new PODetail
                {
                    Company = pohead.Company,
                    PONum = pohead.PONum,
                    POLine = polinenum,
                    PartNum = item.Stock_Reference,
                    LineDesc = item.Reference,
                    CalcDueDate = item.Date_Required,
                    CalcOurQty = item.Quantity_Required - item.Quantity_Received,
                    CalcVendQty = item.Quantity_Required - item.Quantity_Received,
                    DocUnitCost = item.Price,
                    OverridePriceList = true,
                    CalcTranType = "PUR-UKN"
                };
                if (String.IsNullOrEmpty(poline.PartNum)) poline.PartNum = "MISC PART";
                if (String.IsNullOrEmpty(poline.LineDesc)) poline.LineDesc = "MISC PART";

                if (part != null)
                {
                    poline.LineDesc = part.PartDescription; poline.CalcTranType = "PUR-STK";
                    if (poline.CalcOurQty % 1 != 0)
                    {
                        if (part.IUM == "EA" && poline.CalcOurQty < 1)
                            poline.CalcOurQty = 1;
                        else
                            poline.CalcOurQty = Math.Round(poline.CalcOurQty + .5M);

                        poline.CalcVendQty = poline.CalcOurQty;
                    }
                }
                var porel = new PoRelease
                {
                    Company = poline.Company,
                    PONum = poline.PONum,
                    POLine = poline.POLine,
                    PoRelNum = 1,
                    DueDate = poline.CalcDueDate,
                    RelQty = poline.CalcOurQty,
                    TranType = poline.CalcTranType,
                };

                poline.CalcOurQty = Math.Abs(poline.CalcOurQty);
                poline.CalcVendQty = Math.Abs(poline.CalcVendQty);

                if (poline.CalcOurQty == 0) poline.CalcOurQty = 1;
                if (poline.CalcVendQty == 0) poline.CalcVendQty = 1;

                PODetails.Add(poline);
                var adjcost = poline.DocUnitCost;
                if (pohead.VendorVendorID == "TMC")
                {
                    adjcost = adjcost * 1.5M;
                }
                if (pohead.VendorVendorID == "VDK")
                {
                    adjcost = adjcost * 1.15M;
                }
                SqlPOCostAdjustment += String.Format("Update ERP.PODetail Set UnitCost = {0}, DocUnitCost = {4} Where Company = '{3}' AND PONum = {1} AND PartNum = '{2}' AND OpenLine = 1",
                    poline.DocUnitCost, poline.PONum, poline.PartNum, CompanyID, adjcost);
                SqlPOCostAdjustment += Environment.NewLine;

                POReleases.Add(porel);
                polinenum += 1;

                #endregion PO Lines
            }
            if (missingSuppliers2.Length > 3)
            {
                MissingSuppliers += "Missing Suppliers Unique:" + Environment.NewLine;
                MissingSuppliers += Environment.NewLine + missingSuppliers2;
            }
            File.WriteAllText(@"Output\SqlPOCostAdjustment.sql", SqlPOCostAdjustment);
        }

        private static IEnumerable<ASPN_OpenPO> BuildPurchaseOrderHeaders()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, @"Data\ASPN", @"Open PO's.csv");
            StreamReader textreader = new StreamReader(filePath);
            CsvReader csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            //csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            var list = csv.GetRecords<ASPN_OpenPO>().OrderBy(x => x.PONum).ToList();
            csv.Dispose();
            textreader.Close();
            return list;
        }

        private static List<DMT_CUS_Customers> BuildCurrentCustomers()
        {
            Console.WriteLine(Extensions.GetCurrentMethod());
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", CompanyID, "DMT-CUS-Customers.csv");
            StreamReader textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            //csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            var list = csv.GetRecords<DMT_CUS_Customers>().OrderBy(x => x.CustID).ToList();
            csv.Dispose();
            textreader.Close();
            return list;
        }

        private static int ConvertOprSeq(int oprSeq)
        {
            int newSeq = oprSeq;

            if (oprSeq > 10000)
            {
                var tempremainder = oprSeq % 1000;
                var tempNum = oprSeq - tempremainder;
                var tempNum2 = tempNum / 10;
                newSeq = tempNum2 + tempremainder;
            }

            return newSeq;
        }

        private static void CreateInstallJobs()
        {
            Console.WriteLine(Extensions.GetCurrentMethod());
            String missingCustomers2 = "";

            OrderHeaders = new List<OrderHeader>();
            OrderDetails = new List<OrderDetail>();
            OrderReleases = new List<OrderRelease>();
            JobHeaders = new List<JobHeader>();
            JobOperations = new List<JobOperation>();
            JobMaterials = new List<JobMaterial>();
            JobProds = new List<JobProd>();
            //EpicorParts = new List<Part>();
            EpicorCostAdjustments = new List<CostAdjustment>();
            EpicorQuantityAdjustments = new List<QuantityAdjustment>();
            EpicorIssueMaterials = new List<IssueMaterial>();

            var itemcount = 1;
            int currentJobNo = 500;
            string CustID = "";

            foreach (var item in currentJobs)
            {
                int orderNum = 0;
                Console.WriteLine(itemcount++);

                if (item.JobNumber.StartsWith("S/N"))
                {
                    var custinfo = currentCustomers.FirstOrDefault(x => x.CustID.Trim().ToUpper() == item.CustomerID.Trim().ToUpper());
                    if (custinfo == null)
                    {
                        custinfo = currentCustomers.FirstOrDefault(x => x.OldCustID_c.Trim() == item.CustomerID.Trim().Replace("CUST", ""));
                    }
                    if (custinfo == null)
                    {
                        custinfo = currentCustomers.FirstOrDefault(x => x.CustID == item.CustomerID.Replace("CUST", ""));
                    }

                    if (custinfo == null)
                    {
                        MissingCustomers += String.Format("Customer {0} for Job {1} was not found in Epicor", item.CustomerID, item.JobNumber);
                        MissingCustomers += Environment.NewLine;

                        if (!missingCustomers2.Contains(item.CustomerID + Environment.NewLine)) missingCustomers2 += item.CustomerID + Environment.NewLine;

                        custinfo = new DMT_CUS_Customers
                        {
                        };

                        custinfo = currentCustomers.FirstOrDefault(x => x.CustID == "AAA");
                    }

                    if (item.JobNumber == "TBA")
                    {
                        item.JobNumber = currentJobNo.ToString();
                        currentJobNo++;
                    }

                    if (item.JobNumber.StartsWith("S/N"))
                    {
                        try
                        {
                            orderNum = int.Parse(item.JobNumber.Substring(3));
                        }
                        catch (Exception)
                        {
                            orderNum = currentJobNo;
                            currentJobNo++;
                        }
                    }
                    var header = new OrderHeader
                    {
                        Company = CompanyID,
                        OrderNum = orderNum,
                        CustomerCustID = custinfo.CustID,
                        PONum = item.CustomerPO,
                        RequestDate = item.NextDueDate,
                        FOB = custinfo.DefaultFOB,
                        UseOTS = true,
                        OTSName = custinfo.Name,
                        OTSAddress1 = custinfo.Address1,
                        OTSAddress2 = custinfo.Address2,
                        OTSAddress3 = custinfo.Address3,
                        OTSCity = custinfo.City,
                        OTSState = custinfo.State,
                        OTSZip = custinfo.Zip,
                        OrderDate = item.CreateDate,
                        ShipViaCode = custinfo.ShipViaCode,
                        NeedByDate = item.NextDueDate,
                        TermsCode = custinfo.TermsCode,
                        //LiftModel_c = item.Model.Trim().Substring(0, item.Model.Trim().Length > 20 ? 20 : item.Model.Trim().Length),
                        //UD_SerialNo_c = item.LiftSerialNo.Trim(),
                        //TruckMake_c = item.VehicleType.Trim(),
                        //TruckVIN_c = item.ChassisNo.Trim(),
                        //UD_LiftNumber_c = item.Lift.Trim(),
                        //OrderComment = item.WorkOrderNo.Trim(),
                    };

                    CustID = header.CustomerCustID;

                    if (string.IsNullOrEmpty(header.OrderDate)) header.OrderDate = "12-01-2018";
                    if (string.IsNullOrEmpty(header.RequestDate)) header.RequestDate = header.OrderDate;
                    if (string.IsNullOrEmpty(header.NeedByDate)) header.NeedByDate = header.OrderDate;
                    if (string.IsNullOrEmpty(header.PONum)) header.PONum = header.OrderNum.ToString();
                    while (header.PONum == "" || OrderHeaders.FirstOrDefault(x => x.PONum == header.PONum && header.OrderNum != x.OrderNum) != null)
                    {
                        header.PONum = header.OrderNum + "A";
                    }

                    if (string.IsNullOrEmpty(header.NeedByDate)) header.NeedByDate = "01-01-2019";
                    if (string.IsNullOrEmpty(header.RequestDate)) header.RequestDate = "01-01-2019";
                    OrderHeaders.Add(header);
                    int lineno = 1;

                    var detail = new OrderDetail
                    {
                        Company = header.Company,
                        OrderNum = header.OrderNum,
                        OrderLine = lineno,
                        PartNum = item.ItemNumber.Trim(),
                        IUM = "EA",
                        SalesUM = "EA",
                        LineDesc = item.ItemNumber,
                        //DocunitPrice = decimal.Parse(item.),
                        OrderQty = 1,
                        NeedByDate = header.NeedByDate,
                    };
                    lineno++;

                    var release = new OrderRelease
                    {
                        Company = detail.Company,
                        OrderNum = detail.OrderNum,
                        OrderLine = detail.OrderLine,
                        Linetype = "",
                        OrderRelNum = 1,
                        OurReqQty = detail.OrderQty,
                        Make = true,
                    };
                    OrderDetails.Add(detail);
                    OrderReleases.Add(release);

                    // Section of Order Creation related to Jobs
                }
                var jobhead = new JobHeader
                {
                    Company = CompanyID,
                    JobEngineered = true,
                    JobFirm = true,
                    JobReleased = true,
                    JobNum = item.JobNumber,
                    PartNum = item.ItemNumber,
                    Plant = "MfgSys",

                    PartDescription = item.ItemNumber,
                    ProdQty = 0,
                    ReqDueDate = item.NextDueDate,
                    StartDate = item.SchedStartDate,
                    SyncReqBy = true
                };

                if (item.JobNumber.StartsWith("S/N"))
                {
                    jobhead.CustID = CustID;
                }

                if (String.IsNullOrEmpty(jobhead.ReqDueDate)) jobhead.ReqDueDate = "01/01/2019";
                JobHeaders.Add(jobhead);

                // BuildJobOperations
                var ops = currentJobOperations.Where(x => x.JobNumber == item.JobNumber && x.SeqDetType == "L").ToList();
                foreach (var op in ops)
                {
                    var jobOp = new JobOperation
                    {
                        Company = CompanyID,
                        Plant = "MfgSys",
                        JobNum = op.JobNumber,
                        AssemblySeq = 0,
                        LaborEntryMethod = "T",
                        OpCode = op.WCorResID,
                        OprSeq = op.OperationSeq,
                        SchedRelation = "FS",
                        StdFormat = "HP",
                        AutoReceive = false,
                        FinalOpr = false,
                        OpComplete = false,
                        QtyCompleted = 0,
                        ProdStandard = op.EstRTInHrs,
                    };

                    jobOp.OprSeq = ConvertOprSeq(jobOp.OprSeq);

                    JobOperations.Add(jobOp);
                }

                var materials = currentJobMaterials.Where(x => x.JobNumber == item.JobNumber && x.LineType != "TL").OrderBy(x => x.JobNumber).ThenBy(x => x.OperationSeq).ToList();
                int mtlseq = 10;

                if (item.JobNumber.StartsWith("S/N"))
                {
                    var relopr = ConvertOprSeq(currentJobOperations.Where(x => x.JobNumber == item.JobNumber && x.SeqDetType == "L").OrderBy(x => x.OperationSeq).First().OperationSeq);

                    // TODO: get the total cost for the adjustment from the other files
                    var camtl = new JobMaterial
                    {
                        Company = jobhead.Company,
                        JobNum = jobhead.JobNum,
                        PartNum = "CostAdjustment",
                        Description = "CostAdjustment",
                        MtlSeq = 5,
                        QtyPer = 1,
                        AssemblySeq = 0,
                        FixedQty = true,
                        RelatedOperation = relopr,
                        IssuedComplete = false,
                        BuyIt = false,
                        UnitCost = 0,
                    };
                    JobMaterials.Add(camtl);
                    AddToMaterialIssues(camtl);
                }
                foreach (var mtl in materials)
                {
                    decimal quantity = 0m;
                    try
                    {
                        quantity = Convert.ToDecimal(mtl.BOMQty);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine(mtl.BOMQty);
                    }
                    if (mtl.ComponentItemNumber.StartsWith(".")) mtl.ComponentItemNumber = mtl.ComponentItemNumber.Substring(1);
                    var camtl = new JobMaterial
                    {
                        Company = jobhead.Company,
                        JobNum = jobhead.JobNum,
                        PartNum = mtl.ComponentItemNumber,
                        Description = mtl.Description1,
                        MtlSeq = mtlseq,
                        QtyPer = quantity,
                        AssemblySeq = 0,
                        FixedQty = mtl.FlatUnit == "F",
                        RelatedOperation = ConvertOprSeq(mtl.OperationSeq),
                        //RequiredQty = 1,
                        IssuedComplete = (mtl.QtyIssued >= mtl.QtyRequired),
                        BuyIt = false,
                        UnitCost = mtl.UnitCost,
                    };

                    //if (mtl.QtyIssued >= mtl.QtyRequired)
                    //{
                    //    camtl.IssuedComplete = true;
                    //}

                    JobMaterials.Add(camtl);

                    mtlseq += 10;
                }
                //int mtlseq = 10;
                //if (item.WIP > 0)
                //{
                //    var camtl = new JobMaterial
                //    {
                //        Company = jobhead.Company,
                //        JobNum = jobhead.JobNum,
                //        //Plant = jobhead.Plant,
                //        IssuedComplete = true,
                //        PartNum = item.JobNumber,
                //        Description = item.JobNumber,
                //        //MaterialMtlCost = 0,
                //        MtlSeq = 5,
                //        QtyPer = 1,
                //        AssemblySeq = 0,
                //        FixedQty = true,
                //        RelatedOperation = 10
                //        //RequiredQty = 1
                //    };

                //    JobMaterials.Add(camtl);

                //}

                if (item.JobNumber.StartsWith("S/N"))
                {
                    var prod = new JobProd
                    {
                        Company = jobhead.Company,
                        JobNum = jobhead.JobNum,
                        Plant = jobhead.Plant,
                        ProdQty = 1,
                        PartNum = jobhead.PartNum,
                        MakeToType = "Order",
                        OrderNum = orderNum,
                        OrderLine = 1,
                        OrderRelNum = 1
                    };
                    JobProds.Add(prod);
                }

                // TODO: Add Part
                //var part = new Part
                //{
                //    Company = CompanyID,
                //    PartNum = item.JobNumber,
                //    PartDescription = item.JobNumber,
                //    ClassID = "87",
                //    CostMethod = "A",
                //    IUM = "EA",
                //    PUM = "EA",
                //    TypeCode = "P",
                //    ProdCode = "INS",
                //    UnitPrice = item.WIP,
                //    UOMClassID = "COUNT",
                //    NonStock = true,
                //    TrackSerialNum = false,
                //    BuyToOrder = false,
                //    PhantomBOM = false,
                //    SNBaseDataType = "",
                //    SNFormat = "",
                //    Type_c = "",
                //};
                //EpicorParts.Add(part);
                //currentParts.Add(mapper.Map<Part, PartMaster>(part));

                // TODO: Add Cost Adjustment
                var cost = new CostAdjustment
                {
                    Company = CompanyID,
                    PartNum = item.JobNumber,
                    Plant = "MfgSys",
                    ReasonCode = "34",
                    TransDate = "2019/02/28",
                    //StdMtlUnitCost = item.WIP,
                    //AvgMtlUnitCost = item.WIP,
                    //LastMtlUnitCost = item.WIP,
                };
                if (cost.StdMtlUnitCost != 0 || cost.LastMtlUnitCost != 0 || cost.AvgMtlUnitCost != 0)
                {
                    EpicorCostAdjustments.Add(cost);
                }

                // TODO: Add Quantity Adjustment
                var adjust = new QuantityAdjustment
                {
                    Company = CompanyID,
                    PartNum = item.JobNumber,
                    AdjustQuantity = 1,
                    ReasonCode = "10",
                    Plant = "MfgSys",
                    TransDate = "2019/02/28",
                    Reference = "Load WIP Cost from Opera",
                    WareHseCode = "MAIN",
                    BinNum = "YARD",
                };

                EpicorQuantityAdjustments.Add(adjust);

                //if (item.LiftRecd.ToUpper() != "YES" && !String.IsNullOrEmpty(item.Lift))
                //{
                //    // Lift not here or lift here and not invoiced
                //    // Create line on Sales Order - generic
                //    var detail2 = new OrderDetail
                //    {
                //        Company = header.Company,
                //        OrderNum = header.OrderNum,
                //        OrderLine = lineno,
                //        PartNum = item.Lift,
                //        IUM = "EA",
                //        SalesUM = "EA",
                //        LineDesc = item.Lift,
                //        DocunitPrice = 0,
                //        OrderQty = 1,
                //        NeedByDate = header.NeedByDate,
                //    };
                //    lineno++;

                //    var release2 = new OrderRelease
                //    {
                //        Company = detail2.Company,
                //        OrderNum = detail2.OrderNum,
                //        OrderLine = detail2.OrderLine,
                //        Linetype = "",
                //        OrderRelNum = 1,
                //        OurReqQty = detail2.OrderQty,
                //        Make = false,
                //        BuyToOrder = true,
                //    };
                //    OrderDetails.Add(detail2);
                //    OrderReleases.Add(release2);
                //}

                //if (item.LiftRecd.ToUpper() == "YES" && item.LiftWIP.ToUpper() != "YES" && !String.IsNullOrEmpty(item.LiftSerialNo))
                //{
                //    // Lift here and not invoiced
                //    // Create line on Sales Order -sn part
                //    var detail2 = new OrderDetail
                //    {
                //        Company = header.Company,
                //        OrderNum = header.OrderNum,
                //        OrderLine = lineno,
                //        PartNum = item.LiftSerialNo,
                //        IUM = "EA",
                //        SalesUM = "EA",
                //        LineDesc = item.Model,
                //        DocunitPrice = 0,
                //        OrderQty = 1,
                //        NeedByDate = header.NeedByDate,
                //    };

                //    var release2 = new OrderRelease
                //    {
                //        Company = detail2.Company,
                //        OrderNum = detail2.OrderNum,
                //        OrderLine = detail2.OrderLine,
                //        Linetype = "",
                //        OrderRelNum = 1,
                //        OurReqQty = detail2.OrderQty,
                //        Make = false,
                //    };
                //    OrderDetails.Add(detail2);
                //    OrderReleases.Add(release2);

                //    //  Create Part - sn part
                //    var part2 = new Part
                //    {
                //        Company = CompanyID,
                //        PartNum = item.LiftSerialNo,
                //        PartDescription = item.Model,
                //        ClassID = "15",
                //        CostMethod = "A",
                //        IUM = "EA",
                //        PUM = "EA",
                //        TypeCode = "P",
                //        ProdCode = "15",
                //        UnitPrice = 0,
                //        UOMClassID = "COUNT",
                //        NonStock = true,
                //        BuyToOrder = false,
                //        PhantomBOM = false,
                //        SNBaseDataType = "",
                //        SNFormat = "",
                //        TrackSerialNum = false,
                //        Type_c = "",
                //    };
                //    EpicorParts.Add(part2);
                //    currentParts.Add(mapper.Map<Part, PartMaster>(part2));

                //    // create inventory Adjustment for the part - sn part
                //    var adjust2 = new QuantityAdjustment
                //    {
                //        Company = CompanyID,
                //        PartNum = item.LiftSerialNo,
                //        AdjustQuantity = 1,
                //        ReasonCode = "10",
                //        Plant = "MfgSys",
                //        TransDate = "2019/02/28",
                //        Reference = "Load SN Part that has not yet been invoiced",
                //        WareHseCode = "MAIN",
                //        BinNum = "YARD",
                //    };

                //    EpicorQuantityAdjustments.Add(adjust2);
                //}

                //if (item.ChassisRecd.ToUpper() != "YES" && !String.IsNullOrEmpty(item.Vehicle))
                //{
                //    // Lift not here or lift here and not invoiced
                //    // Create line on Sales Order - generic
                //    var detail2 = new OrderDetail
                //    {
                //        Company = header.Company,
                //        OrderNum = header.OrderNum,
                //        OrderLine = lineno,
                //        PartNum = item.Vehicle,
                //        IUM = "EA",
                //        SalesUM = "EA",
                //        LineDesc = item.VehicleType,
                //        DocunitPrice = 0,
                //        OrderQty = 1,
                //        NeedByDate = header.NeedByDate,
                //    };
                //    lineno++;

                //    var release2 = new OrderRelease
                //    {
                //        Company = detail2.Company,
                //        OrderNum = detail2.OrderNum,
                //        OrderLine = detail2.OrderLine,
                //        Linetype = "",
                //        OrderRelNum = 1,
                //        OurReqQty = detail2.OrderQty,
                //        BuyToOrder = true,
                //        Make = false,
                //    };
                //    OrderDetails.Add(detail2);
                //    OrderReleases.Add(release2);
                //}

                //if (item.ChassisRecd.ToUpper() == "YES" && item.ChassisWIP.ToUpper() != "YES" && !String.IsNullOrEmpty(item.ChassisNo))
                //{
                //    // Lift here and not invoiced
                //    // Create line on Sales Order -sn part
                //    var detail2 = new OrderDetail
                //    {
                //        Company = header.Company,
                //        OrderNum = header.OrderNum,
                //        OrderLine = lineno,
                //        PartNum = item.ChassisNo,
                //        IUM = "EA",
                //        SalesUM = "EA",
                //        LineDesc = item.VehicleType,
                //        DocunitPrice = 0,
                //        OrderQty = 1,
                //        NeedByDate = header.NeedByDate,
                //    };

                //    var release2 = new OrderRelease
                //    {
                //        Company = detail2.Company,
                //        OrderNum = detail2.OrderNum,
                //        OrderLine = detail2.OrderLine,
                //        Linetype = "",
                //        OrderRelNum = 1,
                //        OurReqQty = detail2.OrderQty,
                //        BuyToOrder = false,
                //        Make = false,
                //    };
                //    OrderDetails.Add(detail2);
                //    OrderReleases.Add(release2);

                //    //  Create Part - sn part
                //    var part2 = new Part
                //    {
                //        Company = CompanyID,
                //        PartNum = item.ChassisNo,
                //        PartDescription = item.VehicleType,
                //        ClassID = "86",
                //        ProdCode = "86",
                //        CostMethod = "A",
                //        IUM = "EA",
                //        PUM = "EA",
                //        TypeCode = "P",
                //        UnitPrice = 0,
                //        UOMClassID = "COUNT",
                //        NonStock = true,
                //        BuyToOrder = false,
                //        PhantomBOM = false,
                //        SNBaseDataType = "",
                //        SNFormat = "",
                //        TrackSerialNum = false,
                //        Type_c = "",
                //    };
                //    EpicorParts.Add(part2);
                //    currentParts.Add(mapper.Map<Part, PartMaster>(part2));

                //    // create inventory Adjustment for the part - sn part
                //    var adjust2 = new QuantityAdjustment
                //    {
                //        Company = CompanyID,
                //        PartNum = item.Part,
                //        AdjustQuantity = 1,
                //        ReasonCode = "10",
                //        Plant = "MfgSys",
                //        TransDate = "2019/02/28",
                //        Reference = "Load Chassis SN Part that has not yet been invoiced",
                //        WareHseCode = "MAIN",
                //        BinNum = "YARD",
                //    };

                //    EpicorQuantityAdjustments.Add(adjust2);
                //}
            }
            if (missingCustomers2.Length > 3)
            {
                MissingCustomers += Environment.NewLine + "Unique Missing Customers" + Environment.NewLine;
                MissingCustomers += missingCustomers2;
            }
        }

        private static void AddToMaterialIssues(JobMaterial camtl)
        {
            var mi = new IssueMaterial
            {
                Company = CompanyID,
                ToJobNum = camtl.JobNum,
                ToAssemblySeq = camtl.AssemblySeq,
                ToJobSeq = camtl.MtlSeq,
                TranQty = camtl.QtyPer,
                TranDate = "2019/09/30",
            };

            EpicorIssueMaterials.Add(mi);
        }

        //private static void BuildSageJobHeaders()
        //{
        //    var filePath = Path.Combine(Environment.CurrentDirectory, "Data", "99-JobCostJobMasterfile");
        //    StreamReader textreader = new StreamReader(filePath);
        //    var csv = new CsvReader(textreader);
        //    // Turn off.
        //    csv.Configuration.IgnoreBlankLines = true;
        //    csv.Configuration.HasHeaderRecord = true;
        //    csv.Configuration.Delimiter = ";";
        //    csv.Configuration.BadDataFound = null;
        //    var list = csv.GetRecords<JobCostJobMasterfile>().OrderBy(x => x.JobNo).ToList();
        //    csv.Dispose();
        //    textreader.Close();
        //    SageJobHeaders = list;
        //}

        private static void Create_AP_OpenInvoiceLoad()
        {
            APOpenInvoiceLoads = new List<Epicor_APOpenInvoiceLoad>();
            ErrorList += "***** Errors During Create_AP_OpenInvoiceLoad *****" + Environment.NewLine;
            //List<APOpenInvoices_VER> list = new List<APOpenInvoices_VER>();
            // Read Data File
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", CompanyID, "Open AP transactions.csv");
            StreamReader textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            //csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            var list = csv.GetRecords<APOpenInvoices_VER>().ToList();
            csv.Dispose();
            textreader.Close();
            APOpenInvoiceLoads = mapper.Map<List<APOpenInvoices_VER>, List<Epicor_APOpenInvoiceLoad>>(list);
            foreach (var item in AROpenInvoiceLoads)
            {
                var customer = currentCustomers.FirstOrDefault(x => x.CustID == item.CustID);
                if (customer == null)
                {
                    MissingCustomers += String.Format("Open AP Invoice Load:{0} has Vendor: {1} which does not exist in Epicor",
                               item.Invoice20, item.CustID);
                    MissingCustomers += Environment.NewLine;
                }

                if (item.BaseBalance < 0)
                {
                    item.BaseBalance = Math.Abs(item.BaseBalance);
                    item.CreditMemo = true;
                }
            }
        }

        private static void Create_AR_OpenInvoiceLoad()
        {
            String missingCustomers2 = "Unique Missing Custoemrs:" + Environment.NewLine;
            AROpenInvoiceLoads = new List<Epicor_AROpenInvoiceLoad>();
            // Read Data File
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", CompanyID, "Open AR transactions.csv");
            var textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            //csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            var arlist = csv.GetRecords<OpenARtransactions>().ToList();
            csv.Dispose();
            textreader.Close();
            AROpenInvoiceLoads = mapper.Map<List<OpenARtransactions>, List<Epicor_AROpenInvoiceLoad>>(arlist);
            foreach (var item in AROpenInvoiceLoads)
            {
                var custinfo = currentCustomers.FirstOrDefault(x => x.OldCustID_c == item.CustID.Trim());
                if (custinfo == null)
                {
                    MissingCustomers += String.Format("Customer {0} for Invoice {1} was not found in Epicor", item.CustID, item.Invoice20, item.ReferenceID20);
                    MissingCustomers += Environment.NewLine;

                    if (!missingCustomers2.Contains(item.CustID + Environment.NewLine)) missingCustomers2 += item.CustID + Environment.NewLine;
                }
                else
                {
                    item.CustID = custinfo.CustID;
                }

                if (item.BaseBalance < 0)
                {
                    item.BaseBalance = Math.Abs(item.BaseBalance);
                    item.CreditMemo = true;
                }
            }

            MissingCustomers += Environment.NewLine + missingCustomers2;
        }

        //private static void CreateinventoryAdjustments()
        //{
        //    ErrorList += "***** Errors During CreateinventoryAdjustments *****" + Environment.NewLine;
        //    List<OpenInventoryQtyAdjustmentsWhse_VER> list = new List<OpenInventoryQtyAdjustmentsWhse_VER>();
        //    // Read Data File
        //    var filePath = Path.Combine(Environment.CurrentDirectory, "Data", "99-OpenInventoryQtyAdjustmentsWhse_VER");
        //    StreamReader textreader = new StreamReader(filePath);
        //    var csv = new CsvReader(textreader);
        //    // Turn off.
        //    csv.Configuration.IgnoreBlankLines = true;
        //    csv.Configuration.HasHeaderRecord = true;
        //    csv.Configuration.Delimiter = ";";
        //    csv.Configuration.BadDataFound = null;

        //    while (csv.Read())
        //    {
        //        var record = csv.GetRecord<OpenInventoryQtyAdjustmentsWhse_VER>();
        //        if (record.WarehouseCode != "114")
        //        {
        //            decimal newqty = 0;
        //            bool good = decimal.TryParse(record.QuantityOnHand, out newqty);

        //            if (!good) Console.WriteLine("");
        //            list.Add(record);
        //        }
        //    }

        //    //list = csv.GetRecords<OpenInventoryQtyAdjustmentsWhse_VER>()
        //    //    .OrderBy(x => x.ItemCode)
        //    //    .Where(x => x.WarehouseCode != "114")
        //    //    .ToList();
        //    //csv.Dispose();
        //    //textreader.Close();

        //    // Transform Data
        //    List<QuantityAdjustment> adjustments = mapper.Map<List<OpenInventoryQtyAdjustmentsWhse_VER>, List<QuantityAdjustment>>(list);
        //    foreach (var item in adjustments)
        //    {
        //        var part = currentParts.FirstOrDefault(x => x.PartNum == item.PartNum);
        //        if (part == null)
        //        {
        //            // part is not in Epicor
        //            MissingParts += String.Format("Item {0} from Sage with balance {1} is not currently in Epicor", item.PartNum, item.AdjustQuantity);
        //            MissingParts += Environment.NewLine;
        //        }

        //        if (item.AdjustQuantity != Math.Abs(item.AdjustQuantity)) item.AdjustQuantity = Math.Abs(item.AdjustQuantity);

        //        if (String.IsNullOrEmpty(item.BinNum))
        //        {
        //            if (part != null)
        //            {
        //                if (part.PrimWhse == item.WareHseCode)
        //                {
        //                    if (!String.IsNullOrEmpty(part.PrimBin))
        //                    {
        //                        item.BinNum = part.PrimBin;
        //                    }
        //                    else
        //                    {
        //                        //ErrorList += String.Format("Item {0} from Sage shows Qty: {3} in Whse: {1} | Bin: {2} and there is no default Bin", item.PartNum, item.WareHseCode, item.BinNum, item.AdjustQuantity);
        //                        //ErrorList += Environment.NewLine;
        //                        item.BinNum = "NO BIN";
        //                    }
        //                }
        //                else
        //                {
        //                    ErrorList += String.Format("Item {0} from Sage shows Qty: {3} in Whse: {1} | Bin: {2}", item.PartNum, item.WareHseCode, item.BinNum, item.AdjustQuantity);
        //                    ErrorList += Environment.NewLine;
        //                    item.BinNum = "NO BIN";
        //                }
        //            }
        //            else
        //            {
        //                //ErrorList += String.Format("Item {0} from Sage shows Qty: {3} in Whse: {1} | Bin: {2}", item.PartNum, item.WareHseCode, item.BinNum, item.AdjustQuantity);
        //                //ErrorList += Environment.NewLine;
        //                item.BinNum = "NO BIN";
        //            }
        //        }
        //    }

        //    // Write Data File
        //    StreamWriter textwriter = new StreamWriter("GL02-QuantityAdjustments.csv");
        //    var csvout = new CsvWriter(textwriter);
        //    csvout.WriteHeader<QuantityAdjustment>();
        //    csvout.Flush();
        //    csvout.NextRecord();
        //    csvout.WriteRecords<QuantityAdjustment>(adjustments);
        //    csvout.Dispose();
        //    textwriter.Close();
        //}

        #region Write Data Files

        private static void WriteARInvoiceLoad(string fileName, List<Epicor_AROpenInvoiceLoad> data)
        {
            StreamWriter textwriter = new StreamWriter(SubFolder + fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<Epicor_AROpenInvoiceLoad>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<Epicor_AROpenInvoiceLoad>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WriteAPInvoiceLoad(string fileName, List<Epicor_APOpenInvoiceLoad> data)
        {
            StreamWriter textwriter = new StreamWriter(SubFolder + fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<Epicor_APOpenInvoiceLoad>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<Epicor_APOpenInvoiceLoad>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WritePOHeaders(string fileName, List<POHeader> data)
        {
            StreamWriter textwriter = new StreamWriter(SubFolder + fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<POHeader>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<POHeader>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WritePODetails(string fileName, List<PODetail> data)
        {
            StreamWriter textwriter = new StreamWriter(SubFolder + fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<PODetail>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<PODetail>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WritePOReleases(string fileName, List<PoRelease> data)
        {
            StreamWriter textwriter = new StreamWriter(SubFolder + fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<PoRelease>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<PoRelease>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WritePOHeaderApprovals(string fileName, List<POHeader> data)
        {
            StreamWriter textwriter = new StreamWriter(SubFolder + fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<POHeader>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<POHeader>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WriteOrderHeaders(string fileName, List<OrderHeader> data)
        {
            StreamWriter textwriter = new StreamWriter(SubFolder + fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<OrderHeader>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<OrderHeader>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WriteOrderDetails(string fileName, List<OrderDetail> data)
        {
            StreamWriter textwriter = new StreamWriter(SubFolder + fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<OrderDetail>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<OrderDetail>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WriteOrderReleases(string fileName, List<OrderRelease> data)
        {
            StreamWriter textwriter = new StreamWriter(SubFolder + fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<OrderRelease>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<OrderRelease>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WriteJobHeaders(string fileName, List<JobHeader> data)
        {
            StreamWriter textwriter = new StreamWriter(SubFolder + fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<JobHeader>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<JobHeader>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WriteJobOperations(string fileName, List<JobOperation> data)
        {
            StreamWriter textwriter = new StreamWriter(SubFolder + fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<JobOperation>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<JobOperation>(data.OrderBy(x => x.OprSeq));
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WriteJobMaterials(string fileName, List<JobMaterial> data)
        {
            StreamWriter textwriter = new StreamWriter(SubFolder + fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<JobMaterial>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<JobMaterial>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WriteJobProds(string fileName, List<JobProd> data)
        {
            StreamWriter textwriter = new StreamWriter(SubFolder + fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<JobProd>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<JobProd>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WriteJobMtlAdjustment(string fileName, List<JobMtlAdjustment> data)
        {
            StreamWriter textwriter = new StreamWriter(SubFolder + fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<JobMtlAdjustment>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<JobMtlAdjustment>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WriteServiceCalls(string fileName, List<ServiceCallCombinedData> data)
        {
            StreamWriter textwriter = new StreamWriter(SubFolder + fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<ServiceCallCombinedData>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<ServiceCallCombinedData>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WriteJobHeadersEngineered(string fileName, List<JobHeaders_Engineered> data)
        {
            StreamWriter textwriter = new StreamWriter(SubFolder + fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<JobHeaders_Engineered>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<JobHeaders_Engineered>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WriteFSJobOperations(string fileName, List<FieldServiceJobOperation> data)
        {
            StreamWriter textwriter = new StreamWriter(SubFolder + fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<FieldServiceJobOperation>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<FieldServiceJobOperation>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WriteFSJobMaterials(string fileName, List<FieldServiceJobMaterial> data)
        {
            StreamWriter textwriter = new StreamWriter(SubFolder + fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<FieldServiceJobMaterial>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<FieldServiceJobMaterial>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WriteWIPData(string fileName, List<JobCostUnbilledWIPv2_VER> data)
        {
            var dataType = data.GetType().Name;
            StreamWriter textwriter = new StreamWriter(SubFolder + fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<JobCostUnbilledWIPv2_VER>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<JobCostUnbilledWIPv2_VER>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WriteJobTranslationList(string fileName, List<JobXRef> data)
        {
            StreamWriter textwriter = new StreamWriter(SubFolder + fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<JobXRef>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<JobXRef>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WriteQtyAdjustmentss(string fileName, List<QuantityAdjustment> data)
        {
            StreamWriter textwriter = new StreamWriter(SubFolder + fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<QuantityAdjustment>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<QuantityAdjustment>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WriteCostAdjustments(string fileName, List<CostAdjustment> data)
        {
            StreamWriter textwriter = new StreamWriter(SubFolder + fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<CostAdjustment>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<CostAdjustment>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WriteIssueMaterials(string fileName, List<IssueMaterial> data)
        {
            StreamWriter textwriter = new StreamWriter(SubFolder + fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<IssueMaterial>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<IssueMaterial>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WriteParts(string fileName, List<Part> data)
        {
            StreamWriter textwriter = new StreamWriter(SubFolder + fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<Part>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<Part>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WriteDMTBOM(string fileName, List<DMT_BillOfMaterial> data)
        {
            StreamWriter textwriter = new StreamWriter(SubFolder + fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<DMT_BillOfMaterial>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<DMT_BillOfMaterial>(data);
            csvout.Dispose();
            textwriter.Close();
            FixHeaderForDataFile(fileName);
        }

        private static void WriteDMTBOO(string fileName, List<DMT_BillOfOperations> data)
        {
            StreamWriter textwriter = new StreamWriter(SubFolder + fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<DMT_BillOfOperations>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<DMT_BillOfOperations>(data);
            csvout.Dispose();
            textwriter.Close();
            FixHeaderForDataFile(fileName);
        }

        private static void WriteDMTInvPartCombined(string fileName, List<DMT_INV_Part_Combined_Clean> data)
        {
            //List<DMT_INV_Part_Combined_Clean> output = new List<DMT_INV_Part_Combined_Clean>();
            //output = mapper.Map<List<DMT_INV_Part_Combined>, List<DMT_INV_Part_Combined_Clean>>(data);
            StreamWriter textwriter = new StreamWriter(SubFolder + fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<DMT_INV_Part_Combined_Clean>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<DMT_INV_Part_Combined_Clean>(data);
            csvout.Dispose();
            textwriter.Close();

            FixHeaderForDataFile(fileName);
        }

        #endregion Write Data Files

        private static void FixHeaderForDataFile(string fileName)
        {
            //StreamWriter streamWriter = new StreamWriter(fileName);
            var li = File.ReadAllLines(SubFolder + fileName);
            if (li.Count() <= 1)
                File.Delete(SubFolder + fileName);
            else
            {
                li[0] = li[0].Replace("__", "#");
                File.WriteAllLines(SubFolder + fileName, li);
            }
        }
    }
}