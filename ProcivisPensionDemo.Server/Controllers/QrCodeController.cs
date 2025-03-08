using Microsoft.AspNetCore.Mvc;

namespace ProcivisPensionDemo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QrCodeController : ControllerBase
    {

        private readonly ILogger<QrCodeController> _logger;

        public QrCodeController(ILogger<QrCodeController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetTestResponse")]
        public IActionResult Get()
        {
            var response = new { id = 1, message = "Response success!" }; // Returning an object
            return Ok(response);
        }
    }
}
