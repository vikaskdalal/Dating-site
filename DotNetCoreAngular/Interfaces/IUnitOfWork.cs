namespace DotNetCoreAngular.Interfaces
{
    public interface IUnitOfWork
    {
        void Save();

        Task SaveAsync();
    }
}
