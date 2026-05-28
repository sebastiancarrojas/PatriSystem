using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PatriSystem.API.DTOs.Request;
using PatriSystem.API.DTOs.Response;
using PatriSystem.Domain.Entities;
using PatriSystem.Domain.Interfaces.Services;

namespace PatriSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _categoryService.GetAllAsync();
            if (!response.IsSuccess)
                return BadRequest(response);

            var dto = _mapper.Map<List<CategoryResponseDto>>(response.Result);
            return Ok(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = _mapper.Map<Category>(dto);
            var response = await _categoryService.CreateAsync(category);

            if (!response.IsSuccess)
                return BadRequest(response);

            var responseDto = _mapper.Map<CategoryResponseDto>(response.Result);
            return Ok(responseDto);
        }
    }
}