using DotNetCoreAngular.Controllers;
using DotNetCoreAngular.DAL;
using DotNetCoreAngular.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace IntegrationTests.Controllers
{
    public class AccountControllerTest : BaseTest
    {
        public AccountControllerTest()
        {
            TransactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }

        [Fact]
        public async Task ShouldCreateNewUserAsync()
        {
            var registerDto = new RegisterDto()
            {
                DateOfBirth = DateTime.Now,
                Email = "vikas.dalal@gmail.com",
                Password = "123",
                Name = "vikas"
            };
            var unitofWork = new UnitOfWork(DatabaseContext);


            var controller = new AccountController(UnitOfWork, TokenService);

            var result = await controller.RegisterAsync(registerDto);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task ShouldNotCreateNewUserWhenNameNotProvidedAsync()
        {
            var registerDto = new RegisterDto()
            {
                DateOfBirth = DateTime.Now,
                Email = "vikas.dalal@gmail.com",
                Password = "123",
            };
            var unitofWork = new UnitOfWork(DatabaseContext);


            var controller = new AccountController(UnitOfWork, TokenService);

            Assert.ThrowsAsync<DbUpdateException>(async () => await controller.RegisterAsync(registerDto));
        }

        [Fact]
        public async Task ShouldNotCreateNewUserWhenDuplicateEmailAsync()
        {
            var registerDto = new RegisterDto()
            {
                DateOfBirth = DateTime.Now,
                Email = "vikaskdalal@gmail.com",
                Password = "123",
            };

            var unitofWork = new UnitOfWork(DatabaseContext);


            var controller = new AccountController(UnitOfWork, TokenService);
            var result = await controller.RegisterAsync(registerDto);
            
            var okResult = result as BadRequestObjectResult;

            Assert.Equal(400, okResult.StatusCode);
            Assert.Equal("Email is already registered.", okResult.Value);
        }

        public override void Dispose()
        {
            TransactionScope.Dispose();
        }
    }
}
