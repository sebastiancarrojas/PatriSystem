using AutoMapper;
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
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;
        private readonly IMapper _mapper;

        public SalesController(ISaleService saleService, IMapper mapper)
        {
            _saleService = saleService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _saleService.GetAllAsync();
            if (!response.IsSuccess)
                return BadRequest(response);

            var dto = _mapper.Map<List<SaleResponseDto>>(response.Result);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _saleService.GetByIdAsync(id);
            if (!response.IsSuccess)
                return NotFound(response);

            var dto = _mapper.Map<SaleResponseDto>(response.Result);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSaleRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var sale = _mapper.Map<Sale>(dto);
            var response = await _saleService.CreateAsync(sale);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginated([FromQuery] SalePaginationRequest request)
        {
            var response = await _saleService.GetPaginatedAsync(request);
            if (!response.IsSuccess)
                return BadRequest(response);

            var items = _mapper.Map<List<SaleResponseDto>>(response.Result.Items);
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