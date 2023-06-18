using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos;

namespace Clones_Api.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CredentialController : ControllerBase
    {
        private readonly ICredentialService _credentialService;

        public CredentialController(ICredentialService credentialService)
        {
            _credentialService = credentialService;
        }

        [HttpPost("addcredential")]
        public async Task<IActionResult> Add([FromHeader]string tenantId, [FromBody]AddCredentialRequest request)
        {
            var result = await _credentialService.AddCredential(tenantId, request);
            return StatusCode(result.Code, result);
        }

        [Authorize]
        [HttpGet("credentials")]
        public async Task<IActionResult> GetAll([FromHeader] string tenantId)
        {
            var result = await _credentialService.GetCredentials(tenantId);
            return StatusCode(result.Code, result);
        }

        [HttpPatch("credential/{id}")]
        public async Task<IActionResult> Update([FromRoute]string id, [FromQuery]string otp)
        {
            var result = await _credentialService.UpdateCredential(id, otp);
            return StatusCode(result.Code, result);
        }

        [HttpDelete("credential/{id}")]
        public async Task<IActionResult> Delete([FromRoute]string id)
        {
            var result = await _credentialService.DeleteCredential(id);
            return StatusCode(result.Code, result);
        }

        [HttpDelete("credentials")]
        public async Task<IActionResult> DeleteAll([FromBody]List<string> ids)
        {
            var result = await _credentialService.DeleteCredentials(ids);
            return StatusCode(result.Code, result);
        }
    }
}
