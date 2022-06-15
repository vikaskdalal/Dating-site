namespace DotNetCoreAngular.DTO
{
    public class UserDto
    {
        public UserDto(string token, string name, string username, string email)
        {
            Token = token;
            Name = name;
            Username = username;
            Email = email;
        }

        public string Email { get; set; }

        public string Token { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }
    }
}
