using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Business.Models;
using Data.Entities;

namespace Business;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        // Customer - CustomerModel mapping
        _ = this.CreateMap<Customer, CustomerModel>()
            .ForMember(cm => cm.Name, opt => opt.MapFrom(c => c.Person != null ? c.Person.Name : null))
            .ForMember(cm => cm.Surname, opt => opt.MapFrom(c => c.Person != null ? c.Person.Surname : null))
            .ForMember(cm => cm.BirthDate, opt => opt.MapFrom(c => c.Person != null ? c.Person.BirthDate : default))
            .ForMember(cm => cm.ReceiptsIds, opt => opt.MapFrom(c => c.Receipts != null ? c.Receipts.Select(r => r.Id) : new List<int>()))
            .ReverseMap()
            .ForMember(c => c.PersonId, opt => opt.MapFrom(cm => cm.Id))
            .ForMember(c => c.Person, opt => opt.MapFrom(cm => new Person
            {
                Id = cm.Id,
                Name = cm.Name ?? string.Empty,
                Surname = cm.Surname ?? string.Empty,
                BirthDate = cm.BirthDate,
            }));

        // Product - ProductModel mapping
        _ = this.CreateMap<Product, ProductModel>()
            .ForMember(pm => pm.CategoryName, opt => opt.MapFrom(p => p.Category != null ? p.Category.CategoryName : null))
            .ForMember(pm => pm.ReceiptDetailIds, opt => opt.MapFrom(p => p.ReceiptDetails != null ? p.ReceiptDetails.Select(rd => rd.Id) : new List<int>()))
            .ReverseMap()
            .ForMember(p => p.Category, opt => opt.Ignore())
            .ForMember(p => p.ReceiptDetails, opt => opt.Ignore());

        // Receipt - ReceiptModel mapping
        _ = this.CreateMap<Receipt, ReceiptModel>()
            .ForMember(rm => rm.ReceiptDetailsIds, opt => opt.MapFrom(r => r.ReceiptDetails != null ? r.ReceiptDetails.Select(rd => rd.Id) : new List<int>()))
            .ReverseMap()
            .ForMember(r => r.Customer, opt => opt.Ignore())
            .ForMember(r => r.ReceiptDetails, opt => opt.Ignore());

        // ReceiptDetail - ReceiptDetailModel mapping
        _ = this.CreateMap<ReceiptDetail, ReceiptDetailModel>().ReverseMap();

        // ProductCategory - CategoryModel mapping
        _ = this.CreateMap<ProductCategory, CategoryModel>()
            .ForMember(cm => cm.Name, opt => opt.MapFrom(pc => pc.CategoryName))
            .ForMember(cm => cm.ProductIds, opt => opt.MapFrom(pc => pc.Products != null ? pc.Products.Select(p => p.Id) : new List<int>()))
            .ReverseMap()
            .ForMember(pc => pc.CategoryName, opt => opt.MapFrom(cm => cm.Name));
    }
}
