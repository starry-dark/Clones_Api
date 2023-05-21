using Models;

namespace Data.Repository
{
    public interface IRepository
    {
        Task<Credential> AddCredential(Credential credential);
        Task<IEnumerable<Credential>> GetCredentials();
        Credential? GetCredential(string id);
        Task<IEnumerable<Credential>> GetCredentialsByIds(List<string> ids);
        Task<bool> DeleteCredentials(IEnumerable<Credential> credentials);
        Task<bool> DeleteCredential(Credential credential);
        Task<bool> UpdateCredential(Credential credential);
    }
}
