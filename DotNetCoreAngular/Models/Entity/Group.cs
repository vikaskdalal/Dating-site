using System.ComponentModel.DataAnnotations;

namespace DotNetCoreAngular.Models.Entity
{
    public class Group : BaseEntity
    {
        public Group()
        {
        }

        public Group(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public ICollection<Connection> Connections { get; set; } = new List<Connection>();
    }
}
