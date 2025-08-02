using Microsoft.AspNetCore.Mvc;
using Reseller.Domain.Interfaces.Services;
using Reseller.Domain.Models.RevendaModels.Get;
using Reseller.Domain.Models.RevendaModels.Input;

namespace Reseller.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RevendaController : ControllerBase
    {
        private readonly ILogger<RevendaController> _logger;
        private readonly IRevendaService _service;

        public RevendaController(ILogger<RevendaController> logger, IRevendaService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateRevendaAsync([FromBody] RevendaCreateModelRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new BadRequestObjectResult(ModelState);

                await _service.CreateRevendaAsync(request);
                return new CreatedResult();
            }
            catch (Exception ex)
            {
                _logger.LogError("Falha ao criar revenda.", ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpGet()]
        public async Task<IActionResult> GetRevendaByCnpjAsync([FromQuery] RevendaGetByCnpjModelRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new BadRequestObjectResult(ModelState);

                var revenda = await _service.GetRevendaByCnpjAsync(request);
                return new OkObjectResult(revenda);
            }
            catch (Exception ex)
            {
                _logger.LogError("Falha ao buscar revenda.", ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}