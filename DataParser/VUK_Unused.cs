using AutoMapper;
using CsvHelper;
using DataParser.Models;
using DataParser.Models.VUK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataParser
{
    internal class VUK_Unused
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

        private const string SubFolder = @"Output\";
        private const string CompanyID = "VUK";

        private static void CreateJobAdjustments()
        {
            var adjustments = new List<JobMtlAdjustment>();
            decimal adjustmenttotals = 0.00M;
            decimal negadjustments = 0.00M;
            string negadjustmenterrors = "";

            foreach (var item in JobXRefList.Where(x => !String.IsNullOrEmpty(x.NewJob)))
            {
                var adjust = new JobMtlAdjustment
                {
                    JobNum = item.NewJob,
                    AssemblySeq = 0,
                    Company = CompanyID,
                    JobSeq = 5,
                    Plant = "MfgSys",
                    TranDate = "11/30/2018",
                    TranQty = 1,
                    TranType = "ADJ-MTL"
                };
                adjust.MtlUnitCost = cleanWipJobs.Where(x => x.JobNo == item.OriginalJob && x.CostType == "M").Sum(x => x.TransactionAmt);
                adjust.LbrUnitCost = cleanWipJobs.Where(x => x.JobNo == item.OriginalJob && x.CostType == "L").Sum(x => x.TransactionAmt);
                adjust.SubUnitCost = cleanWipJobs
                    .Where(x => x.JobNo == item.OriginalJob && !(x.CostType == "M" || x.CostType == "L"))
                    .Sum(x => x.TransactionAmt);

                adjustmenttotals += adjust.MtlUnitCost + adjust.LbrUnitCost + adjust.SubUnitCost;
                if (adjust.MtlUnitCost < 0)
                {
                    negadjustmenterrors += String.Format("Sage Job {0} ({2}) has a MtlUnitCost adjustment of {1}", item.OriginalJob, adjust.MtlUnitCost, item.NewJob);
                    negadjustmenterrors += Environment.NewLine;
                    if (adjust.MtlUnitCost < 0 && adjust.SubUnitCost >= (adjust.MtlUnitCost * -1))
                    {
                        adjust.SubUnitCost += adjust.MtlUnitCost;
                        negadjustmenterrors += String.Format("Absorbed the negative into the SubUnitCost for {0}", adjust.SubUnitCost);
                        adjust.MtlUnitCost = 0;
                        negadjustmenterrors += Environment.NewLine;
                    }
                    if (adjust.MtlUnitCost < 0 && adjust.LbrUnitCost >= (adjust.MtlUnitCost * -1))
                    {
                        adjust.LbrUnitCost += adjust.MtlUnitCost;
                        negadjustmenterrors += String.Format("Absorbed the negative into the LbrUnitCost for {0}", adjust.LbrUnitCost);
                        adjust.MtlUnitCost = 0;
                        negadjustmenterrors += Environment.NewLine;
                    }

                    negadjustments += adjust.MtlUnitCost;
                    adjust.MtlUnitCost = 0;
                }
                if (adjust.LbrUnitCost < 0)
                {
                    negadjustmenterrors += String.Format("Sage Job {0} ({2}) has a LbrUnitCost adjustment of {1}", item.OriginalJob, adjust.LbrUnitCost, item.NewJob);
                    negadjustmenterrors += Environment.NewLine;
                    if (adjust.LbrUnitCost < 0 && adjust.MtlUnitCost >= (adjust.LbrUnitCost * -1))
                    {
                        adjust.MtlUnitCost += adjust.LbrUnitCost;
                        negadjustmenterrors += String.Format("Absorbed the negative into the SubUnitCost for {0}", adjust.MtlUnitCost);
                        adjust.MtlUnitCost = 0;
                        negadjustmenterrors += Environment.NewLine;
                    }
                    if (adjust.LbrUnitCost < 0 && adjust.SubUnitCost >= (adjust.LbrUnitCost * -1))
                    {
                        adjust.SubUnitCost += adjust.LbrUnitCost;
                        negadjustmenterrors += String.Format("Absorbed the negative into the SubUnitCost for {0}", adjust.SubUnitCost);
                        adjust.LbrUnitCost = 0;
                        negadjustmenterrors += Environment.NewLine;
                    }
                    negadjustments += adjust.LbrUnitCost;
                    adjust.LbrUnitCost = 0;
                }
                if (adjust.SubUnitCost < 0)
                {
                    negadjustmenterrors += String.Format("Sage Job {0} ({2}) has a SubUnitCost adjustment of {1}", item.OriginalJob, adjust.SubUnitCost, item.NewJob);
                    negadjustmenterrors += Environment.NewLine;
                    if (adjust.SubUnitCost < 0 && adjust.MtlUnitCost >= (adjust.SubUnitCost * -1))
                    {
                        adjust.MtlUnitCost += adjust.SubUnitCost;
                        negadjustmenterrors += String.Format("Absorbed the negative into the MtlUnitCost for {0}", adjust.MtlUnitCost);
                        adjust.SubUnitCost = 0;
                        negadjustmenterrors += Environment.NewLine;
                    }
                    if (adjust.SubUnitCost < 0 && adjust.LbrUnitCost >= (adjust.SubUnitCost * -1))
                    {
                        adjust.LbrUnitCost += adjust.SubUnitCost;
                        negadjustmenterrors += String.Format("Absorbed the negative into the MtlUnitCost for {0}", adjust.LbrUnitCost);
                        adjust.SubUnitCost = 0;
                        negadjustmenterrors += Environment.NewLine;
                    }

                    negadjustments += adjust.SubUnitCost;
                    adjust.SubUnitCost = 0;
                }

                adjustments.Add(adjust);
            }
            negadjustmenterrors += String.Format("Total Adjustments = {0} (including negatives)", adjustmenttotals);
            negadjustmenterrors += Environment.NewLine;
            negadjustmenterrors += String.Format("Total Negative Adjustments = {0}", negadjustments);
            negadjustmenterrors += Environment.NewLine;
            negadjustmenterrors += String.Format("Total Adjustments = {0} (Not Including Negatives)", adjustmenttotals - negadjustments);
            negadjustmenterrors += Environment.NewLine;
            File.WriteAllText("NegativeJobAdjustments.txt", negadjustmenterrors);
            JobMtlAdjustments = adjustments;
        }

        private static List<JobCostUnbilledWIPv2_VER> BuildWipList()
        {
            var list = new List<JobCostUnbilledWIPv2_VER>();
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", "99-JobCostUnbilledWIPv2_VER");
            StreamReader textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            csv.Configuration.Delimiter = ";";
            //csv.Configuration.BadDataFound = null;
            while (csv.Read())
            {
                var record = csv.GetRecord<JobCostUnbilledWIPv2_VER>();
                if (record.JobNo == "F901168" && record.CostType == "M")
                {
                    Console.WriteLine("");
                }
                list.Add(record);
            }
            //var list = csv.GetRecords<JobCostUnbilledWIPv2_VER>().OrderBy(x => x.JobNo).ThenBy(x => x.SourceReference).ToList();
            csv.Dispose();
            textreader.Close();
            return list;
        }

        private static List<JobCostUnbilledWIPv2_VER> CleanWipList(List<JobCostUnbilledWIPv2_VER> wipJobs)
        {
            var tempList = new List<JobCostUnbilledWIPv2_VER>();
            foreach (var item in wipJobs)
            {
                if (item.JobNo == "F901168" && item.CostType == "M")
                {
                    Console.WriteLine("");
                }
                var tempItem = tempList.FirstOrDefault(x => x.JobNo == item.JobNo && x.SourceReference == item.SourceReference && x.CostType == item.CostType);
                if (tempItem == null)
                {
                    // Not in the list yet
                    tempList.Add(item);
                }
                else
                {
                    tempItem.TransactionUnits += item.TransactionUnits;
                    tempItem.TransactionAmt += item.TransactionAmt;
                }
            }
            return tempList;
        }

        private static List<DefaultWhse_Bins> BuildDefaultBins()
        {
            var bins = new List<DefaultWhse_Bins>();
            bins.Add(new DefaultWhse_Bins { Warehouse = "003", Bin = "NO BIN" });
            bins.Add(new DefaultWhse_Bins { Warehouse = "004", Bin = "NO BIN" });
            bins.Add(new DefaultWhse_Bins { Warehouse = "005", Bin = "NO BIN" });
            bins.Add(new DefaultWhse_Bins { Warehouse = "007", Bin = "NO BIN" });
            bins.Add(new DefaultWhse_Bins { Warehouse = "111", Bin = "NO BIN" });
            bins.Add(new DefaultWhse_Bins { Warehouse = "113", Bin = "NO BIN" });
            bins.Add(new DefaultWhse_Bins { Warehouse = "117", Bin = "NO BIN" });
            bins.Add(new DefaultWhse_Bins { Warehouse = "119", Bin = "NO BIN" });
            bins.Add(new DefaultWhse_Bins { Warehouse = "300", Bin = "NO BIN" });
            bins.Add(new DefaultWhse_Bins { Warehouse = "330", Bin = "NO BIN" });
            bins.Add(new DefaultWhse_Bins { Warehouse = "500", Bin = "NO BIN" });
            bins.Add(new DefaultWhse_Bins { Warehouse = "WAR", Bin = "NO BIN" });
            return bins;
        }

        private static List<JobXRef> BuildJobTranslationList()
        {
            int CallNum = 1;
            var jobXRef = new List<JobXRef>();
            foreach (var item in cleanWipJobs.OrderBy(x => x.JobNo))
            {
                if (jobXRef.FirstOrDefault(x => x.OriginalJob == item.JobNo) == null)
                {
                    jobXRef.Add(new JobXRef { OriginalJob = item.JobNo });
                }
            }
            foreach (var item in jobXRef.OrderBy(x => x.OriginalJob))
            {
                //if (item.OriginalJob.EndsWith("000"))
                //{
                //    item.NewJob = item.OriginalJob.Substring(0, item.OriginalJob.Length - 3);
                //    item.NewOrder = item.NewJob;
                //}
                var startDigit = item.OriginalJob.Substring(0, 1);
                switch (startDigit)
                {
                    case "0":
                        item.NewJob = item.OriginalJob.Substring(0, item.OriginalJob.Length - 3);
                        item.NewOrder = item.NewJob;
                        break;

                    case "1":
                        item.NewJob = item.OriginalJob.Substring(0, item.OriginalJob.Length - 3);
                        item.NewOrder = item.NewJob;
                        break;

                    case "2":
                        item.NewJob = item.OriginalJob.Substring(0, item.OriginalJob.Length - 3);
                        item.NewOrder = item.NewJob;
                        break;

                    case "3":
                        item.NewJob = item.OriginalJob.Substring(0, item.OriginalJob.Length - 3);
                        item.NewOrder = item.NewJob;
                        break;

                    case "4":
                        item.NewJob = item.OriginalJob.Substring(0, item.OriginalJob.Length - 3);
                        item.NewOrder = item.NewJob;
                        break;

                    case "5":
                        item.NewJob = item.OriginalJob.Substring(0, item.OriginalJob.Length - 3);
                        item.NewOrder = item.NewJob;
                        break;

                    case "6":
                        item.NewJob = item.OriginalJob.Substring(0, item.OriginalJob.Length - 3);
                        item.NewOrder = item.NewJob;
                        break;

                    case "7":
                        item.NewJob = item.OriginalJob.Substring(0, item.OriginalJob.Length - 3);
                        item.NewOrder = item.NewJob;
                        break;

                    case "8":
                        item.NewJob = item.OriginalJob.Substring(0, item.OriginalJob.Length - 3);
                        item.NewOrder = item.NewJob;
                        break;

                    case "9":
                        item.NewJob = item.OriginalJob.Substring(0, item.OriginalJob.Length - 3);
                        item.NewOrder = item.NewJob;
                        break;

                    case "C":
                        item.NewServiceCall = CallNum;
                        item.NewJob = String.Format("S{0:000000}0001", CallNum);
                        item.CallCode = "IH";
                        item.OpCode = "IHSvcOH";
                        item.OpCode2 = "";
                        CallNum++;
                        break;

                    case "F":
                        item.NewServiceCall = CallNum;
                        item.NewJob = String.Format("S{0:000000}0001", CallNum);
                        item.CallCode = "IH";
                        item.OpCode = "IHSvc";
                        item.OpCode2 = "Service";
                        CallNum++;
                        break;

                    case "G":
                        item.NewJob = item.OriginalJob;
                        item.OpCode = "Graphics";
                        break;

                    case "J":
                        item.NewServiceCall = CallNum;
                        item.NewJob = String.Format("S{0:000000}0001", CallNum);
                        CallNum++;
                        break;

                    case "M":
                        item.NewJob = item.OriginalJob;
                        item.OpCode = "PartInst";
                        break;

                    case "P":
                        item.NewJob = item.OriginalJob;
                        item.OpCode = "Fab";
                        break;

                    case "S":
                        item.NewServiceCall = CallNum;
                        item.NewJob = String.Format("S{0:000000}0001", CallNum);
                        item.CallCode = "INTWARR";
                        item.OpCode = "IHSvc";
                        item.OpCode2 = "Service";
                        CallNum++;
                        break;

                    case "T":
                        item.NewJob = item.OriginalJob;
                        item.OpCode = "Fab";
                        break;

                    case "U":
                        item.NewJob = item.OriginalJob;
                        item.OpCode = "Fab";
                        break;

                    case "V":
                        item.NewJob = item.OriginalJob;
                        item.OpCode = "Fab";
                        break;

                    case "W":
                        item.NewServiceCall = CallNum;
                        item.NewJob = String.Format("S{0:000000}0001", CallNum);
                        item.CallCode = "INTWARR";
                        item.OpCode = "IHSvc";
                        item.OpCode2 = "Service";
                        CallNum++;
                        break;

                    default:
                        item.NewJob = "Unhandled Case";
                        break;
                }
            }

            // Add OrderHeader crossreferences
            var heads = BuildOrderHeaders();
            int replaceso = 1000;
            foreach (var head in heads)
            {
                JobXRef xref = new JobXRef
                {
                    OldSalesOrder = head.SalesOrderNo,
                    NewSalesOrder = head.SalesOrderNo.Replace("18-", "").Replace("-", ""),
                };

                if (xref.NewSalesOrder.StartsWith("J")) { xref.NewSalesOrder = replaceso.ToString(); replaceso++; }
                if (xref.NewSalesOrder.StartsWith("P")) { xref.NewSalesOrder = replaceso.ToString(); replaceso++; }

                jobXRef.Add(xref);
            }

            //POHeader xRefs
            //var poheads = BuildPurchaseOrderHeaders();
            //foreach (var head in poheads)
            //{
            //    JobXRef xref = new JobXRef
            //    {
            //        OldPO = head.PurchaseOrderNo,
            //        NewPO = head.PurchaseOrderNo.Replace("18-", "").Replace("RA3", "").Replace("RAC4-87", "487").Replace("RGAC", "")
            //            .Replace("RGAT", "").Replace("RGH", "").Replace("RM00", "").Replace("RMA", "").Replace("RGA", ""),
            //    };

            //    jobXRef.Add(xref);
            //}

            return jobXRef;
        }

        private static IEnumerable<OrderHeaders_VER> BuildOrderHeaders()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", "05-OrderHeaders_VER");
            StreamReader textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            var list = csv.GetRecords<OrderHeaders_VER>().OrderBy(x => x.SalesOrderNo).ToList();
            csv.Dispose();
            textreader.Close();
            return list;
        }

        private static void CreatePartsOrders()
        {
            var oh = BuildOrderHeaders();
            var od = BuildOrderDetails();
            OrderHeaders = new List<OrderHeader>();
            OrderDetails = new List<OrderDetail>();
            OrderReleases = new List<OrderRelease>();

            foreach (var item in JobXRefList.Where(x => !String.IsNullOrEmpty(x.NewSalesOrder)))
            {
                var header_ver = oh.FirstOrDefault(x => x.SalesOrderNo == item.OldSalesOrder);

                var header = new OrderHeader
                {
                    Company = CompanyID,
                    OrderNum = int.Parse(item.NewSalesOrder),
                    CustomerCustID = header_ver.CustomerNo,
                    PONum = header_ver.CustomerPONo,
                    RequestDate = header_ver.ShipExpireDate,
                    FOB = "Bethlehem",
                    UseOTS = true,
                    OTSName = header_ver.ShipToName,
                    OTSAddress1 = header_ver.ShipToAddress1,
                    OTSAddress2 = header_ver.ShipToAddress2,
                    OTSAddress3 = header_ver.ShipToAddress3,
                    OTSCity = header_ver.ShipToCity,
                    OTSState = header_ver.ShipToState,
                    OTSZip = header_ver.ShipToZipCode,
                    OrderDate = header_ver.OrderDate,
                    ShipViaCode = "BEST",
                    NeedByDate = header_ver.ShipExpireDate,
                    TermsCode = "6",
                };
                while (header.PONum == "" || OrderHeaders.FirstOrDefault(x => x.PONum == header.PONum && header.OrderNum != x.OrderNum) != null)
                {
                    header.PONum = header.PONum + "A";
                }

                if (currentCustomers.FirstOrDefault(x => x.CustID == header.CustomerCustID) == null)
                {
                    MissingCustomers += String.Format("Customer {0} is missing from Sage parts order {1}", header.CustomerCustID, item.OldSalesOrder);
                    MissingCustomers += Environment.NewLine;
                }

                if (string.IsNullOrEmpty(header.RequestDate)) header.RequestDate = header.OrderDate;
                if (string.IsNullOrEmpty(header.NeedByDate)) header.NeedByDate = header.OrderDate;
                if (string.IsNullOrEmpty(header.PONum)) header.PONum = header.OrderNum.ToString();
                OrderHeaders.Add(header);
                int lineno = 1;

                foreach (var det in od.Where(x => x.SalesOrderNo == item.OldSalesOrder))
                {
                    var part = currentParts.FirstOrDefault(x => x.PartNum == det.ItemCode);
                    if (part == null)
                    {
                        // part is not in Epicor
                        MissingParts += String.Format("Item {0} from Sage Order {1} is not currently in Epicor", det.ItemCode, det.SalesOrderNo);
                        MissingParts += Environment.NewLine;
                    }

                    var detail = new OrderDetail
                    {
                        Company = header.Company,
                        OrderNum = header.OrderNum,
                        OrderLine = lineno,
                        PartNum = det.ItemCode,
                        IUM = det.UnitOfMeasure,
                        LineDesc = det.ItemCodeDesc,
                        DocunitPrice = det.UnitPrice,
                        OrderQty = det.QuantityOrdered - det.QuantityShipped,
                        NeedByDate = header.NeedByDate,
                    };
                    lineno++;
                    if (detail.IUM == "EACH") detail.IUM = "EA";
                    if (detail.IUM == "GAL") detail.IUM = "GA";
                    if (detail.IUM == "PAIR") detail.IUM = "EA";
                    if (detail.IUM == "") detail.IUM = "EA";
                    if (detail.IUM == "SET") detail.IUM = "EA";
                    if (detail.IUM == "EACH") detail.IUM = "EA";
                    detail.SalesUM = detail.IUM;

                    var release = new OrderRelease
                    {
                        Company = detail.Company,
                        OrderNum = detail.OrderNum,
                        OrderLine = detail.OrderLine,
                        Linetype = "",
                        OrderRelNum = 1,
                        OurReqQty = detail.OrderQty,
                        Make = false,
                    };
                    OrderDetails.Add(detail);
                    OrderReleases.Add(release);
                }
            }
        }

        private static IEnumerable<OrderLines_VER> BuildOrderDetails()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", "06-OrderLines_VER");
            StreamReader textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            var list = csv.GetRecords<OrderLines_VER>().OrderBy(x => x.SalesOrderNo).ToList();
            csv.Dispose();
            textreader.Close();
            return list;
        }

        private static IEnumerable<PurchaseOrderDetail_VER> BuildPurchaseOrderDetails()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "Data", "99-PurchaseOrderDetail_VER");
            StreamReader textreader = new StreamReader(filePath);
            var csv = new CsvReader(textreader);
            // Turn off.
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.HasHeaderRecord = true;
            csv.Configuration.Delimiter = ";";
            csv.Configuration.BadDataFound = null;
            var list = csv.GetRecords<PurchaseOrderDetail_VER>().OrderBy(x => x.PurchaseOrderNo).ToList();
            var removeList = new List<PurchaseOrderDetail_VER>();
            foreach (var item in list)
            {
                if (item.QuantityOrdered - item.QuantityReceived <= 0) removeList.Add(item);
                if (item.QuantityOrdered - item.QuantityInvoiced <= 0) removeList.Add(item);
                if (item.QuantityOrdered == item.QuantityInvoiced) removeList.Add(item);
                if (item.QuantityOrdered == item.QuantityReceived) removeList.Add(item);
            }
            foreach (var item in removeList)
            {
                list.Remove(item);
            }

            csv.Dispose();
            textreader.Close();
            return list;
        }

        private static void CreateOtherJobs()
        {
            const int MaxLength = 50;

            foreach (var item in JobXRefList.Where(x => x.OriginalJob.StartsWith("G") || x.OriginalJob.StartsWith("M") || x.OriginalJob.StartsWith("P")
                    || x.OriginalJob.StartsWith("T") || x.OriginalJob.StartsWith("V")).OrderBy(x => x.OriginalJob))
            {
                var jobinfo = SageJobHeaders.FirstOrDefault(x => x.JobNo == item.OriginalJob);
                if (jobinfo == null)
                {
                    ErrorList += String.Format("Job Header not found for Job: {0}", item.OriginalJob);
                    ErrorList += Environment.NewLine;
                }
                var custinfo = currentCustomers.FirstOrDefault(x => x.CustID == jobinfo.CustomerNo);
                if (custinfo == null)
                {
                    MissingCustomers += String.Format("Customer {0} for Job {1} was not found in Epicor", jobinfo.CustomerNo, item.OriginalJob);
                    MissingCustomers += Environment.NewLine;

                    custinfo = new DMT05CustomerVUKMasterLoad
                    {
                    };
                }

                var partnum = jobinfo.JobDesc;
                if (partnum.Length > MaxLength) partnum = partnum.Substring(0, MaxLength);
                var jobhead = new JobHeader
                {
                    Company = CompanyID,
                    JobEngineered = true,
                    JobFirm = true,
                    JobReleased = true,
                    JobNum = item.NewJob,
                    PartNum = partnum,
                    Plant = "MfgSys",
                    CustID = jobinfo.CustomerNo,
                    PartDescription = partnum,
                    ProdQty = 1,
                    ReqDueDate = jobinfo.ContractDate,
                    StartDate = jobinfo.ContractDate,
                    SyncReqBy = false,
                };

                if (String.IsNullOrEmpty(jobhead.ReqDueDate)) jobhead.ReqDueDate = "01/01/2019";
                JobHeaders.Add(jobhead);

                var op1 = new JobOperation
                {
                    Company = CompanyID,
                    AssemblySeq = 0,
                    OpCode = item.OpCode,
                    OpComplete = false,
                    OprSeq = 10,
                    ProdStandard = 0,
                    QtyCompleted = 0,
                    StdFormat = "HP",
                    LaborEntryMethod = "T",
                    JobNum = jobhead.JobNum,
                    SchedRelation = "FS",
                    Plant = "MfgSys",
                    AutoReceive = true,
                    FinalOpr = true,
                };

                JobOperations.Add(op1);

                //int mtlseq = 10;

                var camtl = new JobMaterial
                {
                    Company = jobhead.Company,
                    JobNum = jobhead.JobNum,
                    //Plant = jobhead.Plant,
                    IssuedComplete = true,
                    PartNum = "CostAdjustment",
                    Description = "CostAdjustment",
                    //MaterialMtlCost = 0,
                    MtlSeq = 5,
                    QtyPer = 1,
                    AssemblySeq = 0,
                    FixedQty = true,
                    RelatedOperation = 10
                    //RequiredQty = 1
                };

                JobMaterials.Add(camtl);
            }
        }
    }
}