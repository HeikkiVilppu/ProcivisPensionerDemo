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

        [HttpGet("pension", Name = "GetPensionCredential")]
        public IActionResult GetPensionCredential()
        {
            try
            {
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var filePath = Path.Combine(basePath, "Files", "pensioncredential.json");

                if (!System.IO.File.Exists(filePath))
                {
                    _logger.LogWarning("Pension credential file not found: {FilePath}", filePath);
                    return NotFound(new { message = "Pension credential file not found" });
                }

                var jsonContent = System.IO.File.ReadAllText(filePath);
                return Content(jsonContent, "application/json; charset=utf-8"); // Ensure UTF-8 response
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading pension credential file");
                return StatusCode(500, new { message = "Error retrieving pension credentials" });
            }
        }
    }
}
