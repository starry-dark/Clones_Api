using Data.Context;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data.Repository
{
    public class CredentialRepository : IRepository
    {
        private readonly ClonesDbContext _context;
        private readonly DbSet<Credential> _dbSet;

        public CredentialRepository(ClonesDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Credential>();
        }

        public async Task<Credential> AddCredential(Credential credential)
        {
            credential = _dbSet.Add(credential).Entity;
            await _context.SaveChangesAsync();
            return credential;
        }

        public async Task<bool> UpdateCredential(Credential credential)
        {
            _dbSet.Update(credential);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCredential(Credential credential)
        {
            _dbSet.Remove(credential);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCredentials(IEnumerable<Credential> credentials)
        {
            _dbSet.RemoveRange(credentials);
            return await _context.SaveChangesAsync() > 0;
        }

        public Credential? GetCredential(string id)
        {
            return _dbSet.SingleOrDefault(x => x.Id.Equals(id));
        }

        public async Task<IEnumerable<Credential>> GetCredentials(string tenantId)
        {
            return await _dbSet.Where(x => x.TenantId == tenantId).OrderByDescending(x => x.CreatedOn).ToListAsync();
        }

        public async Task<IEnumerable<Credential>> GetCredentialsByIds(List<string> ids)
        {
            return await _dbSet.Where(x => ids.Contains(x.Id)).ToListAsync();
        }
    }
}
