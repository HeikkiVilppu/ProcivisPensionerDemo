using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ProcivisPensionDemo.Server.Services;
using ZXing.QrCode.Internal;

namespace ProcivisPensionDemo.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QrCodeController : ControllerBase
    {

        private readonly ILogger<QrCodeController> _logger;
        private readonly IHubContext<QrCodeHub> _hubContext;
        private readonly QRCodeService _qrCodeService;

        public QrCodeController(ILogger<QrCodeController> logger, IHubContext<QrCodeHub> hubContext, QRCodeService qrCodeService)
        {
            _logger = logger;
            _hubContext = hubContext;
            _qrCodeService = qrCodeService;
        }


        [HttpGet(Name = "GetQrCode")]
        public IActionResult Get()
        {
            string text = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            if (string.IsNullOrEmpty(text))
            {
                return BadRequest("Text is required to generate a QR code.");
            }

            var qrCodeImage = _qrCodeService.GenerateQRCode(text);

            // Return the QR code image as a PNG
            return File(qrCodeImage, "image/png");
        }

        /// <summary>
        /// Not used just for testing API
        /// </summary>
        /// <returns></returns>
        [HttpGet("pension", Name = "GetPensionCredential")]
        public IActionResult GetPensionCredential()
        {
            try
            {
                var jsonContent = GetJson();

                if(jsonContent == null)
                    return NotFound(new { message = "Pension credential file not found" });

                return Content(jsonContent, "application/json; charset=utf-8"); // Ensure UTF-8 response
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading pension credential file");
                return StatusCode(500, new { message = "Error retrieving pension credentials" });
            }
        }

        [HttpPost("simulateCallBack", Name = "SimulateCallBack")]
        public async Task<IActionResult> ReceiveCallback([FromBody] object data)
        {
            var jsonContent = GetJson();

            if (jsonContent == null)
                return NotFound(new { message = "Pension credential file not found" });

            await _hubContext.Clients.All.SendAsync("QrCodeApproved", jsonContent);    
            return Ok();
        }

        private string GetJson()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(basePath, "Files", "pensioncredential.json");

            if (!System.IO.File.Exists(filePath))
                _logger.LogWarning($"Pension credential file not found: {filePath}", filePath);

            var jsonContent = System.IO.File.ReadAllText(filePath);

            return jsonContent;
        }
    }
}
