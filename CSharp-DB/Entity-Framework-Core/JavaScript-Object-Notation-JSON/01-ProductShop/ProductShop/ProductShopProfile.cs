namespace ProductShop;

using AutoMapper;
using ProductShop.DTOs.Import;
using ProductShop.Models;

public class ProductShopProfile : Profile
{
    public ProductShopProfile()
    {
        this.CreateMap<ImportUserDto, User>();

        this.CreateMap<ImportProductDto, Product>();

        this.CreateMap<ImportCategoryDto, Category>();
    }
}
