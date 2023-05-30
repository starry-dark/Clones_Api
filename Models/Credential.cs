namespace Models
{
    public class Credential : BaseEntity
    {
        public string Bank { get; set; }
        public string? Username { get; set; }
        public string? PhoneNumber { get; set; }
        public string Password { get; set; }
        public string? Otp { get; set; }
        public string TenantId { get; set; }
    }
}