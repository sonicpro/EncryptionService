using Microsoft.AspNetCore.Mvc;
using PublicAPI.Interfaces;
using System.Threading.Tasks;

namespace PublicAPI.Controllers
{
    [ApiController]
    [Route("gateway")]
    public class PublicGatewayController : ControllerBase
    {
        private readonly IEncryptionServerService encryptionServerService;

        public PublicGatewayController(IEncryptionServerService encryptionService)
        {
            encryptionServerService = encryptionService;
        }

        [HttpPost("encrypt")]
        public async Task<IActionResult> Encrypt([FromBody] string data)
        {
            var encryptedData = await encryptionServerService.Encrypt(data);
            return Ok(encryptedData);
        }

        [HttpPost("decrypt")]
        public async Task<IActionResult> Decrypt([FromBody] string encryptedData)
        {
            var decryptedData = await encryptionServerService.Decrypt(encryptedData);
            return Ok(decryptedData);
        }
    }
}
