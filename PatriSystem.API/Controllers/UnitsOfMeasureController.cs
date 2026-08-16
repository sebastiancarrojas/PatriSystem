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
    public class UnitsOfMeasureController : ControllerBase
    {
        private readonly IUnitOfMeasureService _unitOfMeasureService;
        private readonly IMapper _mapper;

        public UnitsOfMeasureController(IUnitOfMeasureService unitOfMeasureService, IMapper mapper)
        {
            _unitOfMeasureService = unitOfMeasureService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _unitOfMeasureService.GetAllAsync();
            if (!response.IsSuccess)
                return BadRequest(response);

            var dto = _mapper.Map<List<UnitOfMeasureResponseDto>>(response.Result);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUnitOfMeasureRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var unitOfMeasure = _mapper.Map<UnitOfMeasure>(dto);
            var response = await _unitOfMeasureService.CreateAsync(unitOfMeasure);

            if (!response.IsSuccess)
                return BadRequest(response);

            var responseDto = _mapper.Map<UnitOfMeasureResponseDto>(response.Result);
            return Ok(responseDto);
        }
    }
}