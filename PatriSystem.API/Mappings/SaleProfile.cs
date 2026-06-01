using AutoMapper;
using PatriSystem.API.DTOs.Request;
using PatriSystem.API.DTOs.Response;
using PatriSystem.Domain.Entities;

namespace PatriSystem.API.Mappings
{
    public class SaleProfile : Profile
    {
        public SaleProfile()
        {
            // Request mappings
            CreateMap<CreateSaleRequestDto, Sale>()
               .ForMember(dest => dest.SaleDetails, opt => opt.MapFrom(src => src.Details));

            CreateMap<CreateSaleDetailRequestDto, SaleDetail>();

            // Response mappings
            CreateMap<Sale, SaleResponseDto>()
                .ForMember(dest => dest.SaleNumberFormatted, opt => opt.MapFrom(src => src.SaleNumberFormatted));
            CreateMap<SaleDetail, SaleDetailResponseDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src =>
                    src.IsTemporary ? src.ProductName : src.Product != null ? src.Product.ProductName : string.Empty));
        }
    }
}