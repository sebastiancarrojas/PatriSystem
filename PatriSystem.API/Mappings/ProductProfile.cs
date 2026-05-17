using AutoMapper;
using PatriSystem.API.DTOs.Request;
using PatriSystem.Domain.Entities;

namespace PatriSystem.API.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductRequestDto, Product>();
        }
    }
}