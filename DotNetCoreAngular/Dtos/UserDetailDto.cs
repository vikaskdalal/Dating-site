using DotNetCoreAngular.Common.Enums;

namespace DotNetCoreAngular.Dtos
{
    public class UserDetailDto
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public Gender? Gender { get; set; }

        public DateTime Created { get; set; }

        public string? Interests { get; set; }

        public string? LookingFor { get; set; }

        public string? Introduction { get; set; }

        public int Age { get; set; }

        public string Username { get; set; }

        public ICollection<PhotoDto> Photos { get; set; }

        public string PhotoUrl { get; set; }
    }
}
