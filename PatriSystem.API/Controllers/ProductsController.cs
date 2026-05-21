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
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _productService.GetAllAsync();
            if (!response.IsSuccess)
                return BadRequest(response);

            var dto = _mapper.Map<List<ProductResponseDto>>(response.Result);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _productService.GetByIdAsync(id);
            if (!response.IsSuccess)
                return NotFound(response);

            var dto = _mapper.Map<ProductResponseDto>(response.Result);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = _mapper.Map<Product>(dto);
            var response = await _productService.CreateAsync(product);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = _mapper.Map<Product>(dto);
            var response = await _productService.UpdateAsync(id, product);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpDelete("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            var response = await _productService.DeactivateAsync(id);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }
    }
}