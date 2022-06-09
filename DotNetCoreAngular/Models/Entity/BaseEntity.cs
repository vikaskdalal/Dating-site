namespace DotNetCoreAngular.Models.Entity
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;
    }
}
