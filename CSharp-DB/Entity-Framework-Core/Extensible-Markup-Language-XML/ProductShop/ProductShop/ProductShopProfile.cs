using AutoMapper;
using ProductShop.DTOs.Export;
using ProductShop.Models;

namespace ProductShop;

public class ProductShopProfile : Profile
{
    public ProductShopProfile()
    {
        this.CreateMap<Product, ExportProductDto>()
            .ForMember(d => d.Buyer,
                opt => opt.MapFrom(s => $"{s.Buyer.FirstName} {s.Buyer.LastName}"));

        this.CreateMap<User, ExportSoldProductsDto>();
        this.CreateMap<User, ExportSoldProductsDto>()
            .ForMember(d => d.SoldProducts,
                opt => opt.MapFrom(s =>
                    s.ProductsSold
                        .Select(pc => new ExportProductDto()
                        {
                            Name = pc.Name,
                            Price = pc.Price
                        })
                        .ToArray()));
    }
}
