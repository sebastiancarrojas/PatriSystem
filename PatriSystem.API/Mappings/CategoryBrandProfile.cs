using AutoMapper;
using PatriSystem.API.DTOs.Request;
using PatriSystem.API.DTOs.Response;
using PatriSystem.Domain.Entities;

namespace PatriSystem.API.Mappings
{
    public class CategoryBrandProfile : Profile
    {
        public CategoryBrandProfile()
        {
            // Category mappings
            CreateMap<CreateCategoryRequestDto, Category>();
            CreateMap<Category, CategoryResponseDto>();

            // Brand mappings
            CreateMap<CreateBrandRequestDto, Brand>();
            CreateMap<Brand, BrandResponseDto>();
        }
    }
}