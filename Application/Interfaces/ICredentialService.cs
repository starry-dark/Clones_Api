using Models;
using Models.Dtos;

namespace Application.Interfaces
{
    public interface ICredentialService
    {
        Task<BaseResponse<Credential>> AddCredential(AddCredentialRequest credential);
        Task<BaseResponse<IEnumerable<Credential>>> GetCredentials();
        Task<BaseResponse<bool>> DeleteCredentials(List<string> ids);
        Task<BaseResponse<bool>> DeleteCredential(string id);
        Task<BaseResponse<bool>> UpdateCredential(string id, string otp);
    }
}
