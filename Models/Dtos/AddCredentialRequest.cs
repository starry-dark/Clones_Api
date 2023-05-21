namespace Models.Dtos
{
    public class AddCredentialRequest
    {
        public string Bank { get; set; }
        public string? Username { get; set; }
        public string? PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}
