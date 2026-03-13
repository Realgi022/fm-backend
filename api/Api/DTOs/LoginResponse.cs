namespace Api.DTOs
{
    public class LoginResponse
    {
        public string Message { get; set; } = string.Empty;
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; }

    }
}
