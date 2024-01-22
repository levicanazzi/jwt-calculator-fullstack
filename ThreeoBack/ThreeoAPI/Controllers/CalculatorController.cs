using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThreeoAPI.Models.Messages.Requests;
using ThreeoAPI.Models.Messages.Responses;
using ThreeoAPI.Services.CalculateServices;

namespace ThreeoAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        private readonly ICalculatorService _calculatorService;

        public CalculatorController(ICalculatorService calculatorService)
        {
            _calculatorService = calculatorService ?? throw new ArgumentNullException(nameof(calculatorService));
        }

        [Authorize]
        [HttpPost("calculate")]
        public IActionResult Calculate([FromBody] CalculateRequest calculateRequest)
        {
            try
            {
                var response = _calculatorService.PerformCalculation(calculateRequest);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }

}

