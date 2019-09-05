using AutoMapper;
using CsvHelper;
using DataParser.Models;
using System.Collections.Generic;
using System.IO;

namespace DataParser
{
    internal class Unused
    {
        private static IMapper mapper { get; set; }
        private static string ErrorList { get; set; }
        private static string MissingParts { get; set; }
        private static string MissingCustomers { get; set; }
        private static string MissingSuppliers { get; set; }
        private static string MissingAdjustments { get; set; }

        private static List<JobOperation> stdoperations = new List<JobOperation>();
        private static List<V_VE_PartPrimWhseBin> currentParts { get; set; }
        private static List<V_VE_Vendors> currentVendors { get; set; }
        private static List<V_VE_Customer> currentCustomers { get; set; }
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
        public static List<POHeader> POHeaderApprovals { get; set; }
        public static List<Epicor_APOpenInvoiceLoad> APOpenInvoiceLoads { get; set; }
        public static List<Epicor_AROpenInvoiceLoad> AROpenInvoiceLoads { get; set; }

        #region Write Data Files

        private static void WriteARInvoiceLoad(string fileName, List<Epicor_AROpenInvoiceLoad> data)
        {
            StreamWriter textwriter = new StreamWriter(fileName);
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
            StreamWriter textwriter = new StreamWriter(fileName);
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
            StreamWriter textwriter = new StreamWriter(fileName);
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
            StreamWriter textwriter = new StreamWriter(fileName);
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
            StreamWriter textwriter = new StreamWriter(fileName);
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
            StreamWriter textwriter = new StreamWriter(fileName);
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
            StreamWriter textwriter = new StreamWriter(fileName);
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
            StreamWriter textwriter = new StreamWriter(fileName);
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
            StreamWriter textwriter = new StreamWriter(fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<OrderRelease>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<OrderRelease>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WriteServiceCalls(string fileName, List<ServiceCallCombinedData> data)
        {
            StreamWriter textwriter = new StreamWriter(fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<ServiceCallCombinedData>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<ServiceCallCombinedData>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WriteJobHeaders(string fileName, List<JobHeader> data)
        {
            StreamWriter textwriter = new StreamWriter(fileName);
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
            StreamWriter textwriter = new StreamWriter(fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<JobOperation>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<JobOperation>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WriteJobMaterials(string fileName, List<JobMaterial> data)
        {
            StreamWriter textwriter = new StreamWriter(fileName);
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
            StreamWriter textwriter = new StreamWriter(fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<JobProd>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<JobProd>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WriteJobHeadersEngineered(string fileName, List<JobHeaders_Engineered> data)
        {
            StreamWriter textwriter = new StreamWriter(fileName);
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
            StreamWriter textwriter = new StreamWriter(fileName);
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
            StreamWriter textwriter = new StreamWriter(fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<FieldServiceJobMaterial>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<FieldServiceJobMaterial>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WriteJobMtlAdjustment(string fileName, List<JobMtlAdjustment> data)
        {
            StreamWriter textwriter = new StreamWriter(fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<JobMtlAdjustment>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<JobMtlAdjustment>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        private static void WriteWIPData(string fileName, List<JobCostUnbilledWIPv2_VER> data)
        {
            var dataType = data.GetType().Name;
            StreamWriter textwriter = new StreamWriter(fileName);
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
            StreamWriter textwriter = new StreamWriter(fileName);
            var csvout = new CsvWriter(textwriter);
            csvout.WriteHeader<JobXRef>();
            csvout.Flush();
            csvout.NextRecord();
            csvout.WriteRecords<JobXRef>(data);
            csvout.Dispose();
            textwriter.Close();
        }

        #endregion Write Data Files
    }
}