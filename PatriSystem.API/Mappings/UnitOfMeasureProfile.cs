using AutoMapper;
using PatriSystem.API.DTOs.Request;
using PatriSystem.API.DTOs.Response;
using PatriSystem.Domain.Entities;

namespace PatriSystem.API.Mappings
{
    public class UnitOfMeasureProfile : Profile
    {
        public UnitOfMeasureProfile()
        {
            CreateMap<CreateUnitOfMeasureRequestDto, UnitOfMeasure>();
            CreateMap<UnitOfMeasure, UnitOfMeasureResponseDto>();
        }
    }
}