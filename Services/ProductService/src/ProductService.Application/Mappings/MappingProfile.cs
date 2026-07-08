using AutoMapper;
using ProductService.Application.Features.Products.Queries.GetProductById;
using ProductService.Application.Features.Products.Queries.GetProducts;
using ProductService.Domain.Entities;

namespace ProductService.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Price,
                       opt => opt.MapFrom(src => src.Price.Amount));

        CreateMap<Product, ProductListDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Price,
                       opt => opt.MapFrom(src => src.Price.Amount));
    }
}
