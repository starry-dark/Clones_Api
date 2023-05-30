using Microsoft.AspNetCore.Identity;

namespace Models
{
    public class User : IdentityUser
    {
        public string TenantId { get; set; }
        public bool IsActive { get; set; }
    }
}
