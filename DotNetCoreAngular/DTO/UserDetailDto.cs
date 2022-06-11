using DotNetCoreAngular.Common.Enums;

namespace DotNetCoreAngular.DTO
{
    public class UserDetailDto
    {
        public string UserName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public Gender? Gender { get; set; }

        public DateTime LastActive { get; set; }

        public string? Interests { get; set; }

        public string? LookingFor { get; set; }
    }
}
