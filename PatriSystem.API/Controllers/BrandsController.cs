using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatriSystem.API.DTOs.Request;
using PatriSystem.API.DTOs.Response;
using PatriSystem.Domain.Entities;
using PatriSystem.Domain.Interfaces.Services;
using PatriSystem.Domain.Pagination;
using PatriSystem.Domain.Services;

namespace PatriSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;
        private readonly IMapper _mapper;

        public BrandsController(IBrandService brandService, IMapper mapper)
        {
            _brandService = brandService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _brandService.GetAllAsync();
            if (!response.IsSuccess)
                return BadRequest(response);

            var dto = _mapper.Map<List<BrandResponseDto>>(response.Result);
            return Ok(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBrandRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var brand = _mapper.Map<Brand>(dto);
            var response = await _brandService.CreateAsync(brand);

            if (!response.IsSuccess)
                return BadRequest(response);

            var responseDto = _mapper.Map<BrandResponseDto>(response.Result);
            return Ok(responseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBrandRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var brand = _mapper.Map<Brand>(dto);
            var response = await _brandService.UpdateAsync(id, brand);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginated([FromQuery] BrandPaginationRequest request)
        {
            var response = await _brandService.GetPaginatedAsync(request);
            if (!response.IsSuccess)
                return BadRequest(response);

            var items = _mapper.Map<List<BrandResponseDto>>(response.Result.Items);
            var result = new
            {
                currentPage = response.Result.CurrentPage,
                totalPages = response.Result.TotalPages,
                recordsPerPage = response.Result.RecordsPerPage,
                totalCount = response.Result.TotalCount,
                hasPrevious = response.Result.HasPrevious,
                hasNext = response.Result.HasNext,
                filter = response.Result.Filter,
                items
            };

            return Ok(result);
        }
    }
}