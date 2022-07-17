namespace DotNetCoreAngular.Dtos
{
    public class UserDto
    {
        public UserDto(string token, string name, string username, string email, DateTime expire)
        {
            Token = token;
            Name = name;
            Username = username;
            Email = email;
            TokenExpire = expire;
        }

        public string Email { get; set; }

        public string Token { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public DateTime TokenExpire { get; set; }
    }
}
