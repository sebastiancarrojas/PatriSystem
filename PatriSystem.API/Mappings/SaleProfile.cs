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
            CreateMap<Sale, SaleResponseDto>();
            CreateMap<SaleDetail, SaleDetailResponseDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName));
        }
    }
}