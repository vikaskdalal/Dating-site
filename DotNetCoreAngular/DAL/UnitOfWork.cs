using DotNetCoreAngular.DAL.Repository;
using DotNetCoreAngular.Interfaces;
using DotNetCoreAngular.Models.Entity;

namespace DotNetCoreAngular.DAL
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private bool disposed = false;

        private DatabaseContext _context;

        private GenericRepository<User> _userRepository;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }

        public GenericRepository<User> UserRepository => _userRepository ?? new GenericRepository<User>(_context);

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}
