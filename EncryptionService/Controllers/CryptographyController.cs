using Encryption.Filters;
using Encryption.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static Encryption.Helpers.ConversionHelper;

namespace Encryption.Controllers
{
    [ApiController]
    [Route("encryption")]
    [ServiceFilter(typeof(EncryptionServiceAuthorizationFilter))]
    public class CryptographyController : Controller
    {
        private readonly IEncryptionService encryptionService;

        public CryptographyController(IEncryptionService service)
        {
            encryptionService = service;
        }

        [HttpPost("rotate")]
        public async Task<IActionResult> RotateKey()
        {
            await encryptionService.RotateKey();
            return Ok();
        }

        [HttpPost("encrypt")]
        public async Task<IActionResult> Encrypt([FromBody] string textToEncode)
        {
            var bytes = await encryptionService.Encrypt(textToEncode);
            return Ok(BytesToBase64String(bytes));
        }

        [HttpPost("decrypt")]
        public async Task<IActionResult> Decrypt([FromBody] string base64)
        {
            var textData = await encryptionService.Decrypt(Base64StringBytes(base64));
            return Ok(textData);
        }
    }
}
