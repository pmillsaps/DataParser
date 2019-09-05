using AutoMapper;
using CsvHelper;
using DataParser.Models;
using DataParser.Models.Epicor;
using DataParser.Models.VUK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataParser
{
    public class VUK
    {
        private static IMapper mapper { get; set; }
        private static string ErrorList { get; set; }
        private static string MissingParts { get; set; }
        private static string MissingCustomers { get; set; }
        private static string MissingSuppliers { get; set; }
        private static string MissingAdjustments { get; set; }

        private static List<string> IgnoredPO = new List<string>();

        private static List<JobOperation> stdoperations = new List<JobOperation>();
        private static List<PartMaster> currentParts { get; set; }
        private static List<SupplierList> currentVendors { get; set; }
        private static List<DMT05CustomerVUKMasterLoad> currentCustomers { get; set; }
        private static List<DefaultWhse_Bins> whse { get; set; }
        private static List<JobCostUnbilledWIPv2_VER> wipJobs { get; set; }
        private static List<JobCostUnbilledWIPv2_VER> cleanWipJobs { get; set; }
        private static List<JobXRef> JobXRefList { get; set; }
        private static List<JobCostJobMasterfile> SageJobHeaders { get; set; }
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
        private static List<Epicor_Sales_Order_CutOverData> currentJobs { get; set; }

        private static List<Epicor_Sales_Order_CutOverData> serviceJobs;

        private static List<Part> EpicorParts { get; set; }
        private static List<CostAdjustment> EpicorCostAdjustments { get; set; }
        private static List<QuantityAdjustment> EpicorQuantityAdjustments { get; set; }
        private static List<StandardOperations> EpicorStandardOperations { get; set; }
        private static List<IssueMaterial> EpicorIssueMaterials { get; set; }
        public static object EpicorIssueMater { get; private set; }

        private const string SubFolder = @"Output\";
        private const string CompanyID = "VUK";

        public static void Process()
        {
            if (!Directory.Exists(SubFolder))
            {
                Directory.CreateDirectory(SubFolder);
            }

            //IgnoredPO = new List<string>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            mapper = config.CreateMapper();

            ErrorList = "";
            MissingParts = "";
            BuildIgnoredPO();

            currentParts = BuildCurrentParts();
            currentVendors = BuildCurrentVendors();
            currentCustomers = BuildCurrentCustomers();
            currentJobs = BuildCurrentJobs();
            serviceJobs = BuildServiceJobList();
            EpicorStandardOperations = BuildEpicorStandardOperations();
            BuildStandardOperations();

            //Create_AR_OpenInvoiceLoad();
            //CreateInstallJobs();
            CreatePurchaseOrders();
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

            //WriteOrderHeaders("GL05-OrderHeaders.csv", OrderHeaders);
            //WriteOrderDetails("GL06-OrderDetails.csv", OrderDetails);
            //WriteOrderReleases("GL07-OrderReleases.csv", OrderReleases);

            //WriteJobHeaders("GL10-JobHeaders.csv", JobHeaders);
            //WriteJobOperations("GL11-JobOperations.csv", JobOperations);
            //WriteJobMaterials("GL12-JobMaterials.csv", JobMaterials);
            //WriteJobProds("GL13-JobProd.csv", JobProds);
            //WriteIssueMaterials("GL14-IssueMaterial.csv", EpicorIssueMaterials);
            //WriteJobMtlAdjustment("GL49-JobMtlAdjustments.csv", JobMtlAdjustments);

            WritePOHeaders("GL20-POHeaders.csv", POHeaders);
            WritePODetails("GL21-PODetails.csv", PODetails);
            WritePOReleases("GL22-POReleases.csv", POReleases);
            WritePOHeaderApprovals("GL23-POHeaderApprovals.csv", POHeaderApprovals);

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

        private static void openInExplorer(string path)
        {
            string cmd = "explorer.exe";
            string arg = path;
            System.Diagnostics.Process.Start(cmd, arg);
        }

        private static void CreateServiceJobs()
        {
            List<ServiceCallCombinedData> servicecalls = new List<ServiceCallCombinedData>();
            List<FieldServiceJobMaterial> jobmaterials = new List<FieldServiceJobMaterial>();
            List<FieldServiceJobOperation> joboperations = new List<FieldServiceJobOperation>();
            List<JobHeaders_Engineered> jobheaders = new List<JobHeaders_Engineered>();

            ErrorList += "****** Errors During Create Service Job ******";
            ErrorList += Environment.NewLine;
            foreach (var item in serviceJobs)
            {
                var custinfo = currentCustomers.FirstOrDefault(x => x.OldCustId_c.Trim().ToUpper() == item.CustomerID.Trim().ToUpper());
                if (custinfo == null)
                {
                    MissingCustomers += String.Format("Customer {0} for Service Job {1} was not found in Epicor", item.CustomerID, item.JobNo);
                    MissingCustomers += Environment.NewLine;
                }

                #region service call

                //if (string.IsNullOrEmpty(item.OriginalJob)) item.OriginalJob = "BlankOriginalJob";

                var line = new ServiceCallCombinedData
                {
                    Company = CompanyID,
                    CallCode = "FS",
                    CallNum = item.NewServiceCall,
                    CustNumCustID = custinfo.CustID,
                    EntryDate = item.OrderDate,
                    RequestDate = item.NeedByDate,
                    OpenCall = true,
                    CallPriority = "Normal",
                    CallComment = item.JobNo,
                    FSCallDt_CallLine = 1,
                    FSCallDt_PartNum = item.Installation,
                    FSCallDt_LineDesc = item.JobNo,
                    FSCallDt_Plant = "MfgSys",
                    FSCallDt_PartDescription = item.JobNo,
                    FSCallDt_IssueTopicID1 = "Repair",
                    FSCallDt_ProbReasonDesc = item.JobNo,
                    FSCallDt_CreateJob = true
                };

                if (String.IsNullOrEmpty(line.RequestDate)) line.RequestDate = line.EntryDate;
                if (String.IsNullOrEmpty(line.FSCallDt_PartNum)) line.FSCallDt_PartNum = item.JobNo;

                servicecalls.Add(line);

                #endregion service call

                var jobhead = new JobHeaders_Engineered
                {
                    Company = line.Company,
                    JobEngineered = true,
                    JobFirm = true,
                    JobReleased = true,
                    JobNum = item.NewJob,
                    PartNum = line.FSCallDt_PartNum,
                    Plant = line.FSCallDt_Plant,
                };
                jobheaders.Add(jobhead);

                var op = new FieldServiceJobOperation
                {
                    Company = line.Company,
                    JobNum = jobhead.JobNum,
                    Plant = jobhead.Plant,
                    OprSeq = 10,
                    OpCode = item.OpCode,
                };
                joboperations.Add(op);
                if (!String.IsNullOrEmpty(item.OpCode2))
                {
                    op.OprSeq = 20;
                    op.OpCode = item.OpCode2;
                    joboperations.Add(op);
                }

                //int mtlseq = 10;

                //var camtl = new FieldServiceJobMaterial
                //{
                //    Company = jobhead.Company,
                //    JobNum = jobhead.JobNum,
                //    Plant = jobhead.Plant,
                //    AssemblySeq = 0,
                //    IssuedComplete = true,
                //    PartNum = "CostAdjustment",
                //    Description = "CostAdjustment",
                //    MaterialMtlCost = 0,
                //    MtlSeq = 5,
                //    QtyPer = 1,
                //    RequiredQty = 1,
                //};

                //jobmaterials.Add(camtl);

                if (item.WIP > 0)
                {
                    var camtl = new FieldServiceJobMaterial
                    {
                        Company = jobhead.Company,
                        JobNum = jobhead.JobNum,
                        //Plant = jobhead.Plant,
                        IssuedComplete = true,
                        PartNum = item.JobNo,
                        Description = item.JobNo,
                        //MaterialMtlCost = 0,
                        MtlSeq = 5,
                        QtyPer = 1,
                        AssemblySeq = 0,
                        //FixedQty = true,
                        //RelatedOperation = 10
                        //RequiredQty = 1
                    };

                    jobmaterials.Add(camtl);

                    var mi = new IssueMaterial
                    {
                        Company = CompanyID,
                        ToJobNum = camtl.JobNum,
                        ToAssemblySeq = camtl.AssemblySeq,
                        ToJobSeq = camtl.MtlSeq,
                        TranQty = camtl.QtyPer,
                        TranDate = "2019/02/28",
                    };

                    EpicorIssueMaterials.Add(mi);
                }

                // TODO: Add Part
                var part = new Part
                {
                    Company = CompanyID,
                    PartNum = item.JobNo,
                    PartDescription = item.JobNo,
                    ClassID = "87",
                    CostMethod = "A",
                    IUM = "EA",
                    PUM = "EA",
                    TypeCode = "P",
                    ProdCode = "INS",
                    UnitPrice = item.WIP,
                    UOMClassID = "COUNT",
                    NonStock = true,
                };
                EpicorParts.Add(part);
                currentParts.Add(mapper.Map<Part, PartMaster>(part));

                // TODO: Add Cost Adjustment
                var cost = new CostAdjustment
                {
                    Company = CompanyID,
                    PartNum = item.JobNo,
                    Plant = "MfgSys",
                    ReasonCode = "34",
                    TransDate = "2019/02/28",
                    StdMtlUnitCost = item.WIP,
                    AvgMtlUnitCost = item.WIP,
                    LastMtlUnitCost = item.WIP,
                };
                if (cost.StdMtlUnitCost != 0 || cost.LastMtlUnitCost != 0 || cost.AvgMtlUnitCost != 0)
                {
                    EpicorCostAdjustments.Add(cost);
                }

                // TODO: Add Quantity Adjustment
                var adjust = new QuantityAdjustment
                {
                    Company = CompanyID,
                    PartNum = item.JobNo,
                    AdjustQuantity = 1,
                    ReasonCode = "10",
                    Plant = "MfgSys",
                    TransDate = "2019/02/28",
                    Reference = "Load WIP Cost from Opera",
                    WareHseCode = "MAIN",
                    BinNum = "YARD",
                };

                EpicorQuantityAdjustments.Add(adjust);

                // No material lists for the VUK Service Jobs
                //    foreach (var mtlitem in cleanWipJobs.Where(x => x.JobNo == item.OriginalJob && x.CostType == "M" && x.SourceReference != ""))
                //    {
                //        var prt = currentParts.FirstOrDefault(x => x.PartNum == mtlitem.SourceReference);
                //        if (prt == null && !mtlitem.SourceReference.StartsWith("/"))
                //        {
                //            MissingParts += String.Format("Old Job:{0} has Part {1} which does not exist in Epicor",
                //                item.OriginalJob, mtlitem.SourceReference);
                //            MissingParts += Environment.NewLine;
                //        }
                //        var mtl = new FieldServiceJobMaterial
                //        {
                //            Company = jobhead.Company,
                //            JobNum = jobhead.JobNum,
                //            Plant = jobhead.Plant,
                //            IssuedComplete = true,
                //        };
                //        mtl.PartNum = mtlitem.SourceReference;
                //        if (prt != null) mtl.Description = prt.PartDescription;
                //        else mtl.Description = mtlitem.SourceReference;
                //        mtl.QtyPer = mtlitem.TransactionUnits;
                //        mtl.RequiredQty = mtlitem.TransactionUnits;
                //        mtl.MtlSeq = mtlseq;
                //        mtlseq += 10;

                //        if (prt == null | (prt != null && prt.IUM == "EA"))
                //        {
                //            mtl.QtyPer = Math.Abs(mtl.QtyPer);
                //            mtl.RequiredQty = Math.Abs(mtl.RequiredQty);
                //        }
                //        mtl.QtyPer = Math.Abs(mtl.QtyPer);
                //        mtl.RequiredQty = Math.Abs(mtl.RequiredQty);

                //        if (mtl.QtyPer == 0) mtl.QtyPer = 1;
                //        if (mtl.RequiredQty == 0) mtl.RequiredQty = 1;

                //        jobmaterials.Add(mtl);
                //    }
            }

            WriteServiceCalls("GL40-Service_Call_Center_Combined.csv", servicecalls);
            WriteJobHeadersEngineered("GL46-FSJobHeadersEngineered.csv", jobheaders);
            WriteFSJobOperations("GL41-FSJobOperations.csv", joboperations);
            WriteFSJobMaterials("GL42-FSJobMaterials.csv", jobmaterials);
        }

        private static List<StandardOperations> BuildEpicorStandardOperations()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", "VUK", "EpicorStdOperations.csv");
            StreamReader textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            //csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            var list = csv.GetRecords<StandardOperations>().ToList();
            //.OrderBy(x => x.CustID).ToList();
            csv.Dispose();
            textreader.Close();
            return list;
        }

        private static List<Epicor_Sales_Order_CutOverData> BuildCurrentJobs()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", "VUK", "Epicor_Sales_Order_CutOverData.csv");
            StreamReader textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            //csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            var list = csv.GetRecords<Epicor_Sales_Order_CutOverData>().ToList();
            //.OrderBy(x => x.CustID).ToList();
            csv.Dispose();
            textreader.Close();
            return list;
        }

        private static List<Epicor_Sales_Order_CutOverData> BuildServiceJobList()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", "VUK", "Epicor_Sales_Order_CutOverData.csv");
            StreamReader textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            //csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            var list = csv.GetRecords<Epicor_Sales_Order_CutOverData>().Where(x => x.Month == "Service").ToList();
            //.OrderBy(x => x.CustID).ToList();
            csv.Dispose();
            textreader.Close();
            return list;
        }

        private static List<PartMaster> BuildCurrentParts()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", "VUK", "DMT-93-Parts.csv");
            StreamReader textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            //csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            var list = csv.GetRecords<PartMaster>().OrderBy(x => x.PartNum).ToList();
            csv.Dispose();
            textreader.Close();
            return list;
        }

        private static void BuildIgnoredPO()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", "VUK", "IgnoredPO.txt");
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
                        Company = "VUK",
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
                        Company = "VUK",
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

                //if (!String.IsNullOrEmpty(polinedata.JobNo))
                //{
                //    var job = JobXRefList.FirstOrDefault(x => x.OriginalJob == polinedata.JobNo);

                //    if (job != null)
                //    {
                //        var jobline = JobMaterials.FirstOrDefault(x => x.JobNum == job.NewJob && x.PartNum == poline.PartNum);

                //        poline.CalcTranType = "PUR-MTL";
                //        poline.CalcAssemlySeq = 0;
                //        poline.CalcJobNum = job.NewJob;
                //        poline.CalcJobSeqType = "M";

                //        porel.JobNum = job.NewJob;
                //        porel.AssemblySeq = 0;
                //        porel.JobSeqType = "M";
                //        if (jobline != null)
                //        {
                //            porel.JobSeq = jobline.MtlSeq;
                //            poline.CalcJobSeq = jobline.MtlSeq;
                //        }
                //        else
                //        {
                //            porel.JobSeq = 5;
                //            poline.CalcJobSeq = 5;
                //        }
                //    }
                //}

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

        private static IEnumerable<VUK_OpenPO> BuildPurchaseOrderHeaders()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, @"Data\VUK", @"Open PO's.csv");
            StreamReader textreader = new StreamReader(filePath);
            CsvReader csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            //csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            var list = csv.GetRecords<VUK_OpenPO>().OrderBy(x => x.PONum).ToList();
            csv.Dispose();
            textreader.Close();
            return list;
        }

        private static List<DMT05CustomerVUKMasterLoad> BuildCurrentCustomers()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", "VUK", "DMT-05-Customer_VUK_Master_Load.csv");
            StreamReader textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            //csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            var list = csv.GetRecords<DMT05CustomerVUKMasterLoad>().OrderBy(x => x.CustID).ToList();
            csv.Dispose();
            textreader.Close();
            return list;
        }

        private static List<SupplierList> BuildCurrentVendors()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, @"Data\VUK", "DMT-75-Supplier_Combined.csv");
            StreamReader textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            //csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            var list = csv.GetRecords<SupplierList>().OrderBy(x => x.VendorID).ToList();
            csv.Dispose();
            textreader.Close();
            return list;
        }

        private static void BuildStandardOperations()
        {
            for (int i = 1; i <= 13; i++)
            {
                var op = new JobOperation
                {
                    OprSeq = i * 10,
                    AssemblySeq = 0,
                    Company = CompanyID,
                    OpComplete = false,
                    QtyCompleted = 0
                };
                switch (i)
                {
                    case 1:
                        op.OpCode = "EngDwg";
                        op.StdFormat = "HR";
                        op.LaborEntryMethod = "Q";
                        op.ProdStandard = 1;
                        op.SchedRelation = "SS";
                        break;

                    case 2:
                        op.OpCode = "LiftRec";
                        op.StdFormat = "HR";
                        op.ProdStandard = 1;
                        op.LaborEntryMethod = "Q";
                        op.SchedRelation = "SS";
                        break;

                    case 3:
                        op.OpCode = "BodyRec";
                        op.StdFormat = "HR";
                        op.ProdStandard = 1;
                        op.LaborEntryMethod = "Q";
                        op.SchedRelation = "SS";
                        break;

                    case 4:
                        op.OpCode = "ChassisR";
                        op.StdFormat = "HR";
                        op.ProdStandard = 1;
                        op.LaborEntryMethod = "Q";
                        op.SchedRelation = "SS";
                        break;

                    case 5:
                        op.OpCode = "WhsePull";
                        op.StdFormat = "HP";
                        op.ProdStandard = 1;
                        op.LaborEntryMethod = "T";
                        op.SchedRelation = "FS";
                        break;

                    case 6:
                        op.OpCode = "ChassisP";
                        op.StdFormat = "HP";
                        op.ProdStandard = 0;
                        op.LaborEntryMethod = "T";
                        op.SchedRelation = "FS";
                        break;

                    case 7:
                        op.OpCode = "ChasStag";
                        op.StdFormat = "HP";
                        op.LaborEntryMethod = "T";
                        op.ProdStandard = 0;
                        op.SchedRelation = "FS";
                        break;

                    case 8:
                        op.OpCode = "BodyAcc";
                        op.StdFormat = "HP";
                        op.LaborEntryMethod = "T";
                        op.ProdStandard = 0;
                        op.SchedRelation = "FS";
                        break;

                    case 9:
                        op.OpCode = "Electric";
                        op.StdFormat = "HP";
                        op.LaborEntryMethod = "T";
                        op.ProdStandard = 0;
                        op.SchedRelation = "FS";
                        break;

                    case 10:
                        op.OpCode = "Hyd";
                        op.StdFormat = "HP";
                        op.LaborEntryMethod = "T";
                        op.ProdStandard = 0;
                        op.SchedRelation = "FS";
                        break;

                    case 11:
                        op.OpCode = "LiftPrep";
                        op.StdFormat = "HP";
                        op.LaborEntryMethod = "T";
                        op.ProdStandard = 0;
                        op.SchedRelation = "FS";
                        break;

                    case 12:
                        op.OpCode = "Paint";
                        op.StdFormat = "HP";
                        op.LaborEntryMethod = "T";
                        op.ProdStandard = 0;
                        op.SchedRelation = "FS";
                        break;

                    case 13:
                        op.OpCode = "QA";
                        op.StdFormat = "HP";
                        op.LaborEntryMethod = "T";
                        op.ProdStandard = 0;
                        op.SchedRelation = "FS";
                        op.AutoReceive = true;
                        op.FinalOpr = true;
                        break;
                }

                stdoperations.Add(op);
            }

            var op1 = new JobOperation
            {
                OprSeq = 15,
                AssemblySeq = 0,
                Company = CompanyID,
                OpComplete = false,
                QtyCompleted = 0,
                OpCode = "IVAApp",
                StdFormat = "HR",
                LaborEntryMethod = "Q",
                ProdStandard = 1,
                SchedRelation = "SS",
            };
            stdoperations.Add(op1);

            var op2 = new JobOperation
            {
                OprSeq = 95,
                AssemblySeq = 0,
                Company = CompanyID,
                OpComplete = false,
                QtyCompleted = 0,
                OpCode = "IVA",
                StdFormat = "HP",
                LaborEntryMethod = "T",
                ProdStandard = 5,
                SchedRelation = "FS",
            };
            stdoperations.Add(op2);
        }

        private static void CreateInstallJobs()
        {
            String missingCustomers2 = "";

            OrderHeaders = new List<OrderHeader>();
            OrderDetails = new List<OrderDetail>();
            OrderReleases = new List<OrderRelease>();
            JobHeaders = new List<JobHeader>();
            JobOperations = new List<JobOperation>();
            JobMaterials = new List<JobMaterial>();
            JobProds = new List<JobProd>();
            EpicorParts = new List<Part>();
            EpicorCostAdjustments = new List<CostAdjustment>();
            EpicorQuantityAdjustments = new List<QuantityAdjustment>();
            EpicorIssueMaterials = new List<IssueMaterial>();

            var itemcount = 1;
            int currentJobNo = 500;
            foreach (var item in currentJobs)
            {
                Console.WriteLine(itemcount++);

                //if (jobinfo == null)
                //{
                //    ErrorList += String.Format("Job Header not found for Job: {0}", item.OriginalJob);
                //    ErrorList += Environment.NewLine;
                //}
                var custinfo = currentCustomers.FirstOrDefault(x => x.OldCustId_c.Trim().ToUpper() == item.CustomerID.Trim().ToUpper());
                if (custinfo == null)
                {
                    custinfo = currentCustomers.FirstOrDefault(x => x.OldCustId_c.Trim() == item.CustomerID.Trim().Replace("CUST", ""));
                }
                if (custinfo == null)
                {
                    custinfo = currentCustomers.FirstOrDefault(x => x.CustID.Trim() == item.CustomerID.Trim().Replace("CUST", ""));
                }

                if (custinfo == null)
                {
                    MissingCustomers += String.Format("Customer {0} for Job {1} was not found in Epicor", item.CustomerID, item.JobNo);
                    MissingCustomers += Environment.NewLine;

                    if (!missingCustomers2.Contains(item.CustomerID + Environment.NewLine)) missingCustomers2 += item.CustomerID + Environment.NewLine;

                    custinfo = new DMT05CustomerVUKMasterLoad
                    {
                    };

                    custinfo = currentCustomers.FirstOrDefault(x => x.CustID == "C10093");
                }

                if (item.JobNo == "TBA")
                {
                    item.JobNo = currentJobNo.ToString();
                    currentJobNo++;
                }

                var header = new OrderHeader
                {
                    Company = "VUK",
                    OrderNum = int.Parse(item.JobNo),
                    CustomerCustID = custinfo.CustID,
                    PONum = item.CustPONo,
                    RequestDate = item.RequestDate,
                    FOB = custinfo.DefaultFOB,
                    UseOTS = true,
                    OTSName = custinfo.Name,
                    OTSAddress1 = custinfo.Address1,
                    OTSAddress2 = custinfo.Address2,
                    OTSAddress3 = custinfo.Address3,
                    OTSCity = custinfo.City,
                    OTSState = custinfo.State,
                    OTSZip = custinfo.Zip,
                    OrderDate = item.OrderDate,
                    ShipViaCode = custinfo.ShipViaCode,
                    NeedByDate = item.NeedByDate,
                    TermsCode = custinfo.TermsCode,
                    LiftModel_c = item.Model.Trim().Substring(0, item.Model.Trim().Length > 20 ? 20 : item.Model.Trim().Length),
                    UD_SerialNo_c = item.LiftSerialNo.Trim(),
                    TruckMake_c = item.VehicleType.Trim(),
                    TruckVIN_c = item.ChassisNo.Trim(),
                    UD_LiftNumber_c = item.Lift.Trim(),
                    OrderComment = item.WorkOrderNo.Trim(),
                };

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
                    PartNum = item.Installation.Trim(),
                    IUM = "EA",
                    SalesUM = "EA",
                    LineDesc = item.Installation,
                    DocunitPrice = decimal.Parse(item.SalesLineValue),
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

                var jobhead = new JobHeader
                {
                    Company = header.Company,
                    JobEngineered = true,
                    JobFirm = true,
                    JobReleased = true,
                    JobNum = item.JobNo,
                    PartNum = detail.PartNum,
                    Plant = "MfgSys",
                    CustID = header.CustomerCustID,
                    PartDescription = detail.LineDesc,
                    ProdQty = 0,
                    ReqDueDate = header.NeedByDate,
                    StartDate = header.NeedByDate,
                    SyncReqBy = true
                };

                if (String.IsNullOrEmpty(jobhead.ReqDueDate)) jobhead.ReqDueDate = "01/01/2019";
                JobHeaders.Add(jobhead);
                var stdop = EpicorStandardOperations.FirstOrDefault(x => x.InstallationCode == item.Installation);

                foreach (var op in stdoperations)
                {
                    var op1 = new JobOperation
                    {
                        Company = op.Company,
                        AssemblySeq = op.AssemblySeq,
                        OpCode = op.OpCode,
                        OpComplete = op.OpComplete,
                        OprSeq = op.OprSeq,
                        ProdStandard = op.ProdStandard,
                        QtyCompleted = 0,
                        StdFormat = op.StdFormat,
                        LaborEntryMethod = op.LaborEntryMethod,
                        JobNum = jobhead.JobNum,
                        SchedRelation = op.SchedRelation,
                        Plant = "MfgSys",
                    };

                    if (stdop != null)
                    {
                        switch (op1.OpCode)
                        {
                            case "EngDwg":
                                op1.ProdStandard = stdop.EngDwg;
                                break;

                            case "LiftRec":
                                op1.ProdStandard = stdop.LiftRec;
                                break;

                            case "BodyRec":
                                op1.ProdStandard = stdop.BodyRec;
                                break;

                            case "ChassisR":
                                op1.ProdStandard = stdop.ChassisR;
                                break;

                            case "WhsePull":
                                op1.ProdStandard = stdop.WhsePull;
                                break;

                            case "ChassisP":
                                op1.ProdStandard = stdop.ChassisP;
                                break;

                            case "ChasStag":
                                op1.ProdStandard = stdop.ChasStag;
                                break;

                            case "BodyAcc":
                                op1.ProdStandard = stdop.BodyAcc;
                                break;

                            case "Electric":
                                op1.ProdStandard = stdop.Electric;
                                break;

                            case "Hyd":
                                op1.ProdStandard = stdop.Hyd;
                                break;

                            case "LiftPrep":
                                op1.ProdStandard = stdop.LiftPrep;
                                break;

                            case "Paint":
                                op1.ProdStandard = stdop.Paint;
                                break;

                            case "QA":
                                op1.ProdStandard = stdop.QA;
                                break;

                            case "IVAApp":
                                op1.ProdStandard = stdop.IVAApp;
                                break;

                            case "IVA":
                                op1.ProdStandard = stdop.IVA;
                                break;

                            default:
                                break;
                        }
                    }
                    JobOperations.Add(op1);
                }

                //int mtlseq = 10;
                if (item.WIP > 0)
                {
                    var camtl = new JobMaterial
                    {
                        Company = jobhead.Company,
                        JobNum = jobhead.JobNum,
                        //Plant = jobhead.Plant,
                        IssuedComplete = true,
                        PartNum = item.JobNo,
                        Description = item.JobNo,
                        //MaterialMtlCost = 0,
                        MtlSeq = 5,
                        QtyPer = 1,
                        AssemblySeq = 0,
                        FixedQty = true,
                        RelatedOperation = 10
                        //RequiredQty = 1
                    };

                    JobMaterials.Add(camtl);

                    var mi = new IssueMaterial
                    {
                        Company = CompanyID,
                        ToJobNum = camtl.JobNum,
                        ToAssemblySeq = camtl.AssemblySeq,
                        ToJobSeq = camtl.MtlSeq,
                        TranQty = camtl.QtyPer,
                        TranDate = "2019/02/28",
                    };

                    EpicorIssueMaterials.Add(mi);
                }

                var prod = new JobProd
                {
                    Company = jobhead.Company,
                    JobNum = jobhead.JobNum,
                    Plant = jobhead.Plant,
                    ProdQty = 1,
                    PartNum = jobhead.PartNum,
                    MakeToType = "Order",
                    OrderNum = release.OrderNum,
                    OrderLine = release.OrderLine,
                    OrderRelNum = release.OrderRelNum
                };

                JobProds.Add(prod);

                // TODO: Add Part
                var part = new Part
                {
                    Company = CompanyID,
                    PartNum = item.JobNo,
                    PartDescription = item.JobNo,
                    ClassID = "87",
                    CostMethod = "A",
                    IUM = "EA",
                    PUM = "EA",
                    TypeCode = "P",
                    ProdCode = "INS",
                    UnitPrice = item.WIP,
                    UOMClassID = "COUNT",
                    NonStock = true,
                    TrackSerialNum = false,
                    BuyToOrder = false,
                    PhantomBOM = false,
                    SNBaseDataType = "",
                    SNFormat = "",
                    Type_c = "",
                };
                EpicorParts.Add(part);
                currentParts.Add(mapper.Map<Part, PartMaster>(part));

                // TODO: Add Cost Adjustment
                var cost = new CostAdjustment
                {
                    Company = CompanyID,
                    PartNum = item.JobNo,
                    Plant = "MfgSys",
                    ReasonCode = "34",
                    TransDate = "2019/02/28",
                    StdMtlUnitCost = item.WIP,
                    AvgMtlUnitCost = item.WIP,
                    LastMtlUnitCost = item.WIP,
                };
                if (cost.StdMtlUnitCost != 0 || cost.LastMtlUnitCost != 0 || cost.AvgMtlUnitCost != 0)
                {
                    EpicorCostAdjustments.Add(cost);
                }

                // TODO: Add Quantity Adjustment
                var adjust = new QuantityAdjustment
                {
                    Company = CompanyID,
                    PartNum = item.JobNo,
                    AdjustQuantity = 1,
                    ReasonCode = "10",
                    Plant = "MfgSys",
                    TransDate = "2019/02/28",
                    Reference = "Load WIP Cost from Opera",
                    WareHseCode = "MAIN",
                    BinNum = "YARD",
                };

                EpicorQuantityAdjustments.Add(adjust);

                if (item.LiftRecd.ToUpper() != "YES" && !String.IsNullOrEmpty(item.Lift))
                {
                    // Lift not here or lift here and not invoiced
                    // Create line on Sales Order - generic
                    var detail2 = new OrderDetail
                    {
                        Company = header.Company,
                        OrderNum = header.OrderNum,
                        OrderLine = lineno,
                        PartNum = item.Lift,
                        IUM = "EA",
                        SalesUM = "EA",
                        LineDesc = item.Lift,
                        DocunitPrice = 0,
                        OrderQty = 1,
                        NeedByDate = header.NeedByDate,
                    };
                    lineno++;

                    var release2 = new OrderRelease
                    {
                        Company = detail2.Company,
                        OrderNum = detail2.OrderNum,
                        OrderLine = detail2.OrderLine,
                        Linetype = "",
                        OrderRelNum = 1,
                        OurReqQty = detail2.OrderQty,
                        Make = false,
                        BuyToOrder = true,
                    };
                    OrderDetails.Add(detail2);
                    OrderReleases.Add(release2);
                }

                if (item.LiftRecd.ToUpper() == "YES" && item.LiftWIP.ToUpper() != "YES" && !String.IsNullOrEmpty(item.LiftSerialNo))
                {
                    // Lift here and not invoiced
                    // Create line on Sales Order -sn part
                    var detail2 = new OrderDetail
                    {
                        Company = header.Company,
                        OrderNum = header.OrderNum,
                        OrderLine = lineno,
                        PartNum = item.LiftSerialNo,
                        IUM = "EA",
                        SalesUM = "EA",
                        LineDesc = item.Model,
                        DocunitPrice = 0,
                        OrderQty = 1,
                        NeedByDate = header.NeedByDate,
                    };

                    var release2 = new OrderRelease
                    {
                        Company = detail2.Company,
                        OrderNum = detail2.OrderNum,
                        OrderLine = detail2.OrderLine,
                        Linetype = "",
                        OrderRelNum = 1,
                        OurReqQty = detail2.OrderQty,
                        Make = false,
                    };
                    OrderDetails.Add(detail2);
                    OrderReleases.Add(release2);

                    //  Create Part - sn part
                    var part2 = new Part
                    {
                        Company = CompanyID,
                        PartNum = item.LiftSerialNo,
                        PartDescription = item.Model,
                        ClassID = "15",
                        CostMethod = "A",
                        IUM = "EA",
                        PUM = "EA",
                        TypeCode = "P",
                        ProdCode = "15",
                        UnitPrice = 0,
                        UOMClassID = "COUNT",
                        NonStock = true,
                        BuyToOrder = false,
                        PhantomBOM = false,
                        SNBaseDataType = "",
                        SNFormat = "",
                        TrackSerialNum = false,
                        Type_c = "",
                    };
                    EpicorParts.Add(part2);
                    currentParts.Add(mapper.Map<Part, PartMaster>(part2));

                    // create inventory Adjustment for the part - sn part
                    var adjust2 = new QuantityAdjustment
                    {
                        Company = CompanyID,
                        PartNum = item.LiftSerialNo,
                        AdjustQuantity = 1,
                        ReasonCode = "10",
                        Plant = "MfgSys",
                        TransDate = "2019/02/28",
                        Reference = "Load SN Part that has not yet been invoiced",
                        WareHseCode = "MAIN",
                        BinNum = "YARD",
                    };

                    EpicorQuantityAdjustments.Add(adjust2);
                }

                if (item.ChassisRecd.ToUpper() != "YES" && !String.IsNullOrEmpty(item.Vehicle))
                {
                    // Lift not here or lift here and not invoiced
                    // Create line on Sales Order - generic
                    var detail2 = new OrderDetail
                    {
                        Company = header.Company,
                        OrderNum = header.OrderNum,
                        OrderLine = lineno,
                        PartNum = item.Vehicle,
                        IUM = "EA",
                        SalesUM = "EA",
                        LineDesc = item.VehicleType,
                        DocunitPrice = 0,
                        OrderQty = 1,
                        NeedByDate = header.NeedByDate,
                    };
                    lineno++;

                    var release2 = new OrderRelease
                    {
                        Company = detail2.Company,
                        OrderNum = detail2.OrderNum,
                        OrderLine = detail2.OrderLine,
                        Linetype = "",
                        OrderRelNum = 1,
                        OurReqQty = detail2.OrderQty,
                        BuyToOrder = true,
                        Make = false,
                    };
                    OrderDetails.Add(detail2);
                    OrderReleases.Add(release2);
                }

                if (item.ChassisRecd.ToUpper() == "YES" && item.ChassisWIP.ToUpper() != "YES" && !String.IsNullOrEmpty(item.ChassisNo))
                {
                    // Lift here and not invoiced
                    // Create line on Sales Order -sn part
                    var detail2 = new OrderDetail
                    {
                        Company = header.Company,
                        OrderNum = header.OrderNum,
                        OrderLine = lineno,
                        PartNum = item.ChassisNo,
                        IUM = "EA",
                        SalesUM = "EA",
                        LineDesc = item.VehicleType,
                        DocunitPrice = 0,
                        OrderQty = 1,
                        NeedByDate = header.NeedByDate,
                    };

                    var release2 = new OrderRelease
                    {
                        Company = detail2.Company,
                        OrderNum = detail2.OrderNum,
                        OrderLine = detail2.OrderLine,
                        Linetype = "",
                        OrderRelNum = 1,
                        OurReqQty = detail2.OrderQty,
                        BuyToOrder = false,
                        Make = false,
                    };
                    OrderDetails.Add(detail2);
                    OrderReleases.Add(release2);

                    //  Create Part - sn part
                    var part2 = new Part
                    {
                        Company = CompanyID,
                        PartNum = item.ChassisNo,
                        PartDescription = item.VehicleType,
                        ClassID = "86",
                        ProdCode = "86",
                        CostMethod = "A",
                        IUM = "EA",
                        PUM = "EA",
                        TypeCode = "P",
                        UnitPrice = 0,
                        UOMClassID = "COUNT",
                        NonStock = true,
                        BuyToOrder = false,
                        PhantomBOM = false,
                        SNBaseDataType = "",
                        SNFormat = "",
                        TrackSerialNum = false,
                        Type_c = "",
                    };
                    EpicorParts.Add(part2);
                    currentParts.Add(mapper.Map<Part, PartMaster>(part2));

                    // create inventory Adjustment for the part - sn part
                    var adjust2 = new QuantityAdjustment
                    {
                        Company = CompanyID,
                        PartNum = item.ChassisNo,
                        AdjustQuantity = 1,
                        ReasonCode = "10",
                        Plant = "MfgSys",
                        TransDate = "2019/02/28",
                        Reference = "Load Chassis SN Part that has not yet been invoiced",
                        WareHseCode = "MAIN",
                        BinNum = "YARD",
                    };

                    EpicorQuantityAdjustments.Add(adjust2);
                }
            }
            if (missingCustomers2.Length > 3)
            {
                MissingCustomers += Environment.NewLine + "Unique Missing Customers" + Environment.NewLine;
                MissingCustomers += missingCustomers2;
            }
        }

        private static void BuildSageJobHeaders()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", "99-JobCostJobMasterfile");
            StreamReader textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            var list = csv.GetRecords<JobCostJobMasterfile>().OrderBy(x => x.JobNo).ToList();
            csv.Dispose();
            textreader.Close();
            SageJobHeaders = list;
        }

        private static void Create_AP_OpenInvoiceLoad()
        {
            APOpenInvoiceLoads = new List<Epicor_APOpenInvoiceLoad>();
            ErrorList += "***** Errors During Create_AP_OpenInvoiceLoad *****" + Environment.NewLine;
            //List<APOpenInvoices_VER> list = new List<APOpenInvoices_VER>();
            // Read Data File
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", "VUK", "Open AP transactions.csv");
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
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", "VUK", "Open AR transactions.csv");
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
                var custinfo = currentCustomers.FirstOrDefault(x => x.OldCustId_c == item.CustID.Trim());
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

        #endregion Write Data Files
    }
}