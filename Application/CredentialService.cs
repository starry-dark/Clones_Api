using Application.Interfaces;
using AutoMapper;
using Data.Repository;
using Models;
using Models.Dtos;

namespace Application
{
    public class CredentialService : ICredentialService
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public CredentialService(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<Credential>> AddCredential(string tenantId, AddCredentialRequest credentialRequest)
        {
            if(string.IsNullOrWhiteSpace(tenantId))
                return new BaseResponse<Credential>().Error("Tenant Id is required!", code:400);

            var credential = _mapper.Map<Credential>(credentialRequest);
            credential.TenantId = tenantId;
            var result = await _repository.AddCredential(credential);
            if(result != null)
                return new BaseResponse<Credential>().Success("Credential added successfully", code:201, data:result);
            return new BaseResponse<Credential>().Error("An error occurred!");
        }

        public async Task<BaseResponse<bool>> DeleteCredential(string id)
        {
            var credential = _repository.GetCredential(id);
            if(credential == null)
                return new BaseResponse<bool>().Error($"Credential with id: {id} not found!", 400);

            var result = await _repository.DeleteCredential(credential);
            if(result)
                return new BaseResponse<bool>().Success("Credential successfully deleted!");
            return new BaseResponse<bool>().Error("An error occurred!");
        }

        public async Task<BaseResponse<bool>> DeleteCredentials(List<string> ids)
        {
            var credentials = await _repository.GetCredentialsByIds(ids);
            if (!credentials?.Any() ?? true)
                return new BaseResponse<bool>().Error("No credential with the given ids found!", 400);
            var result = await _repository.DeleteCredentials(credentials!);
            if(result)
                return new BaseResponse<bool>().Success("Credentials successfully deleted!");
            return new BaseResponse<bool>().Error("An error occurred!");
        }

        public async Task<BaseResponse<IEnumerable<Credential>>> GetCredentials(string tenantId)
        {
            if (string.IsNullOrWhiteSpace(tenantId))
                return new BaseResponse<IEnumerable<Credential>>().Error("Tenant Id is required!", code: 400);

            var result = await _repository.GetCredentials(tenantId);
            return new BaseResponse<IEnumerable<Credential>>().Success("Credentials successfully retrieved!", data: result);
        }

        public async Task<BaseResponse<bool>> UpdateCredential(string id, string otp)
        {
            var credential = _repository.GetCredential(id);
            if(credential == null)
                return new BaseResponse<bool>().Error($"Credential with id: {id} not found!", 400);

            credential.Otp = otp;
            var result = await _repository.UpdateCredential(credential);

            if (result)
                return new BaseResponse<bool>().Success("Credential successfully updated!");
            return new BaseResponse<bool>().Error("An error occurred!");
        }
    }
}