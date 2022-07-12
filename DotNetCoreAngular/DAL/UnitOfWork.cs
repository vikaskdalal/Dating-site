using AutoMapper;
using DotNetCoreAngular.DAL.Repository;
using DotNetCoreAngular.Interfaces;
using DotNetCoreAngular.Interfaces.Repository;
using DotNetCoreAngular.Models.Entity;

namespace DotNetCoreAngular.DAL
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private bool disposed = false;

        private DatabaseContext _context;

        private readonly IMapper _mapper;

        private IUserRepository _userRepository;

        private ILikeRepository _likeRepository;

        private IMessageRepository _messageRepository;

        public UnitOfWork(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IUserRepository UserRepository => _userRepository ?? new UserRepository(_context);

        public ILikeRepository LikeRepository => _likeRepository ?? new LikeRepository(_context);
        public IMessageRepository MessageRepository => _messageRepository ?? new MessageRepository(_context, _mapper);

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
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
