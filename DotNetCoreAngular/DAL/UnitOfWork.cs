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

        private IGroupRepository _groupRepository;

        private IConnectionRepository _connectionRepository;

        public UnitOfWork(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IUserRepository UserRepository
        {
            get
            {
                if(_userRepository == null )
                    _userRepository = new UserRepository(_context, _mapper);

                return _userRepository;
            }
        }

        public ILikeRepository LikeRepository
        {
            get
            {
                if (_likeRepository == null)
                    _likeRepository = new LikeRepository(_context);
                
                return _likeRepository;
            }
        }

        public IMessageRepository MessageRepository
        {
            get
            {
                if(_messageRepository == null)
                    _messageRepository = new MessageRepository(_context, _mapper);

                return _messageRepository;
            }
        }
            
        public IGroupRepository GroupRepository
        {
            get
            {
                if(_groupRepository == null)
                     _groupRepository = new GroupRepository(_context);

                return _groupRepository;
            }
        }

        public IConnectionRepository ConnectionRepository
        {
            get
            {
                if(_connectionRepository == null)
                    _connectionRepository = new ConnectionRepository(_context);

                return _connectionRepository;
            }
        }

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
