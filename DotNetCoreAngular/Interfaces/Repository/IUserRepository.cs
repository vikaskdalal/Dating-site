﻿using DotNetCoreAngular.Dtos;
using DotNetCoreAngular.Helpers.Pagination;
using DotNetCoreAngular.Models.Entity;

namespace DotNetCoreAngular.Interfaces.Repository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByEmailAsync(string email);

        Task<User> GetByUsernameAsync(string username);

        Task<User> GetUserWithLikesAsync(int userid);

        Task<PagedList<UserDetailDto>> GetAllUsersWithPhotosAsync(UserParams userParams);
    }
}
