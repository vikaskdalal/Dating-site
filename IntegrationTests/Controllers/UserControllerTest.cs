using DotNetCoreAngular.Controllers;

namespace IntegrationTests.Controllers
{
    public class UserControllerTest : BaseTest
    {
        private UserController _controller;
        public UserControllerTest()
        {
            _controller = new UserController(UnitOfWork, Mapper, PhotoService);
        }

        [Fact]
        public async void ShouldGetUserByEmail()
        {
            var useremail = "vikaskdalal@gmail.com";

            var result = await _controller.GetByEmail(useremail);

            Assert.NotNull(result);
        }

        [Fact]
        public async void ShouldNotGetUserByEmailIfDoesNotExists()
        {
            var useremail = "123456@gmail.com";

            var result = await _controller.GetByEmail(useremail);

            Assert.Null(result);
        }

        public override void Dispose()
        {
            TransactionScope.Dispose();
        }
    }
}
