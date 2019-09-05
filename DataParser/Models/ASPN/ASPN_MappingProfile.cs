using AutoMapper;
using DataParser.Models.Epicor;
using DataParser.Models.ASPN;
using System;

namespace DataParser.Models
{
    public class ASPN_MappingProfile : Profile
    {
        public string Company { get; set; }

        public ASPN_MappingProfile()
        {
            if (String.IsNullOrEmpty(Company))
            {
                Company = "ASPN";
            }

            CreateMap<decimal, decimal>().ConvertUsing(x => Math.Round(x, 3));
            CreateMap<string, string>().ConvertUsing(x => x.Trim());

            //CreateMap<BOM, DMT_BillOfMaterial>()
            //    .ForMember(dest => dest.Company, opts => opts.MapFrom(src => this.Company))
            //    .ForMember(dest => dest.ECOGroupID, opts => opts.MapFrom(src => "PEM"))
            //    ;

            CreateMap<DMT_INV_Part_Combined, DMT_INV_Part_Combined_Clean>();

            CreateMap<JobOrderPlanHeader, JobHeader>()
                .ForMember(dest => dest.Company, opts => opts.MapFrom(src => this.Company))
                .ForMember(dest => dest.Plant, opts => opts.MapFrom(src => "MfgSys"))
                .ForMember(dest => dest.JobNum, opts => opts.MapFrom(src => src.JobNumber))
                .ForMember(dest => dest.PartNum, opts => opts.MapFrom(src => src.JobNumber))
                .ForMember(dest => dest.PartDescription, opts => opts.MapFrom(src => src.JobNumber))
                .ForMember(dest => dest.ReqDueDate, opts => opts.MapFrom(src => src.JobNumber))
                .ForMember(dest => dest.StartDate, opts => opts.MapFrom(src => src.JobNumber))
                .ForMember(dest => dest.ProdQty, opts => opts.MapFrom(src => src.JobNumber))
                .ForMember(dest => dest.JobFirm, opts => opts.MapFrom(src => src.JobNumber))
                .ForMember(dest => dest.JobEngineered, opts => opts.MapFrom(src => src.JobNumber))
                .ForMember(dest => dest.JobReleased, opts => opts.MapFrom(src => src.JobNumber))
                .ForMember(dest => dest.CustID, opts => opts.MapFrom(src => src.CustomerID))
                .ForMember(dest => dest.SyncReqBy, opts => opts.MapFrom(src => src.JobNumber))
                ;

            CreateMap<Part, PartMaster>()
               //.ForMember(dest => dest.Company, opts => opts.MapFrom(src => src.Company))
               //.ForMember(dest => dest.PartNum, opts => opts.MapFrom(src => src.PartNum))
               //.ForMember(dest => dest.PartDescription, opts => opts.MapFrom(src => src.PartDescription))
               //.ForMember(dest => dest.BuyToOrder, opts => opts.MapFrom(src => src.BuyToOrder))
               //.ForMember(dest => dest.ClassID, opts => opts.MapFrom(src => src.ClassID))
               //.ForMember(dest => dest.CostMethod, opts => opts.MapFrom(src => src.CostMethod))
               //.ForMember(dest => dest.Reference, opts => opts.MapFrom(src => "Initial Inventory Load"))
               //.ForMember(dest => dest.Plant, opts => opts.MapFrom(src => "MfgSys"))
               //.ForMember(dest => dest.TransDate, opts => opts.MapFrom(src => "11/30/2018"))
               ;

            CreateMap<OpenInventoryQtyAdjustmentsWhse_VER, QuantityAdjustment>()
                .ForMember(dest => dest.Company, opts => opts.MapFrom(src => "VE"))
                .ForMember(dest => dest.PartNum, opts => opts.MapFrom(src => src.ItemCode))
                .ForMember(dest => dest.WareHseCode, opts => opts.MapFrom(src => src.WarehouseCode))
                .ForMember(dest => dest.BinNum, opts => opts.MapFrom(src => src.BinLocation))
                .ForMember(dest => dest.AdjustQuantity, opts => opts.MapFrom(src => src.QuantityOnHand))
                .ForMember(dest => dest.ReasonCode, opts => opts.MapFrom(src => "10"))
                .ForMember(dest => dest.Reference, opts => opts.MapFrom(src => "Initial Inventory Load"))
                .ForMember(dest => dest.Plant, opts => opts.MapFrom(src => "MfgSys"))
                .ForMember(dest => dest.TransDate, opts => opts.MapFrom(src => "11/30/2018"))
                ;

            CreateMap<APOpenInvoices_VER, Epicor_APOpenInvoiceLoad>()
                .ForMember(dest => dest.Invoice20, opts => opts.MapFrom(src => src.InvoiceNo))
                .ForMember(dest => dest.SupplierID, opts => opts.MapFrom(src => src.VendorNo))
                .ForMember(dest => dest.Terms, opts => opts.MapFrom(src => src.TermsCode))
                .ForMember(dest => dest.InvoiceDate, opts => opts.MapFrom(src => src.InvoiceDate))
                .ForMember(dest => dest.Balance, opts => opts.MapFrom(src => src.Balance))
                .ForMember(dest => dest.ReferenceID20, opts => opts.MapFrom(src => src.Comment))
                .ForMember(dest => dest.DebitMemo, opts => opts.MapFrom(src => "False"))
                ;

            CreateMap<AROpenInvoices_VER, Epicor_AROpenInvoiceLoad>()
                .ForMember(dest => dest.Invoice20, opts => opts.MapFrom(src => src.InvoiceNo))
                .ForMember(dest => dest.CustID, opts => opts.MapFrom(src => src.CustomerNo))
                .ForMember(dest => dest.TermsCode, opts => opts.MapFrom(src => src.TermsCode))
                .ForMember(dest => dest.InvoiceDate, opts => opts.MapFrom(src => src.InvoiceDate))
                .ForMember(dest => dest.BaseBalance, opts => opts.MapFrom(src => src.Balance))
                .ForMember(dest => dest.ReferenceID20, opts => opts.MapFrom(src => src.Comment))
                .ForMember(dest => dest.CreditMemo, opts => opts.MapFrom(src => "False"))
                ;

            CreateMap<OpenARtransactions, Epicor_AROpenInvoiceLoad>()
               .ForMember(dest => dest.Invoice20, opts => opts.MapFrom(src => src.Reference))
               .ForMember(dest => dest.CustID, opts => opts.MapFrom(src => src.Account))
               //.ForMember(dest => dest.TermsCode, opts => opts.MapFrom(src => src.TermsCode))
               .ForMember(dest => dest.InvoiceDate, opts => opts.MapFrom(src => src.Date))
               .ForMember(dest => dest.BaseBalance, opts => opts.MapFrom(src => src.Value))
               .ForMember(dest => dest.ReferenceID20, opts => opts.MapFrom(src => src.Reference))
               .ForMember(dest => dest.CreditMemo, opts => opts.MapFrom(src => "False"))
               ;
        }
    }
}