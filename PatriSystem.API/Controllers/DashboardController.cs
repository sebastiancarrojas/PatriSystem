using Microsoft.AspNetCore.Mvc;
using PatriSystem.Domain.Interfaces.Services;

namespace PatriSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _dashboardService.GetDashboardAsync();
            if (!response.IsSuccess)
                return BadRequest(response);
            return Ok(response.Result);
        }
    }
}