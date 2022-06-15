using DotNetCoreAngular.Common.Enums;
using DotNetCoreAngular.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetCoreAngular.Models.Entity
{
    public class User : BaseEntity
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public Gender? Gender { get; set; }

        public DateTime LastActive { get; set; } = DateTime.Now;

        public string? Interests { get; set; }

        public string? LookingFor { get; set; }

        public string? Introduction { get; set; }

        [NotMapped]
        public int Age => DateOfBirth.CalculateAge();
    }
}
