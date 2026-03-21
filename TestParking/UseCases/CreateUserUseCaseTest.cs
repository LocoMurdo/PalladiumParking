using Microsoft.EntityFrameworkCore;
using Moq;
using Parking.API.scr.Core.Security.Users.UseCases;
using Parking.API.scr.Infrastructure.Persistence;
using Parking.API.scr.Shared.Interfaces;
using Parking.API.scr.Shared.Interfaces.Persistence.Repositories;
using SimpleResults;

namespace TestParking.UseCases
{
    public class CreateUserUseCaseTest
    {
        private readonly AppDbContext _context;
        private readonly Mock<IUserRepository> _userRepo;
        private readonly Mock<IPasswordHasher> _passwordHasher;
        private readonly CreatedBasicUserValidatior _validator;

        public CreateUserUseCaseTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var configurators = new List<IEntityTypeConfigurator>();
            _context = new AppDbContext(configurators, options);
            _userRepo = new Mock<IUserRepository>();
            _passwordHasher = new Mock<IPasswordHasher>();
            _validator = new CreatedBasicUserValidatior();
        }

        private CreateBasicUserUseCase CreateUseCase()
            => new(_context, _userRepo.Object, _passwordHasher.Object, _validator);

        [Fact]
        public async Task Create_ValidRequest_ShouldSucceed()
        {
            _userRepo.Setup(r => r.UserExistAsync(It.IsAny<string>())).ReturnsAsync(false);
            _passwordHasher.Setup(h => h.HashPassword(It.IsAny<string>())).Returns("hashed-password");

            var request = new CreateBasicUserRequest
            {
                UserName = "test@email.com",
                Password = "Test123!",
                Names = "John",
                LastNames = "Doe",
                CellPhone = "12345678"
            };

            var result = await CreateUseCase().ExecuteAsync(request);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Create_DuplicateUser_ShouldReturnConflict()
        {
            _userRepo.Setup(r => r.UserExistAsync(It.IsAny<string>())).ReturnsAsync(true);

            var request = new CreateBasicUserRequest
            {
                UserName = "existing@email.com",
                Password = "Test123!",
                Names = "John",
                LastNames = "Doe",
                CellPhone = "12345678"
            };

            var result = await CreateUseCase().ExecuteAsync(request);

            Assert.False(result.IsSuccess);
            Assert.Equal(ResultStatus.Conflict, result.Status);
        }

        [Fact]
        public async Task Create_EmptyEmail_ShouldReturnInvalid()
        {
            var request = new CreateBasicUserRequest
            {
                UserName = "",
                Password = "Test123!",
                Names = "John",
                LastNames = "Doe",
                CellPhone = "12345678"
            };

            var result = await CreateUseCase().ExecuteAsync(request);

            Assert.False(result.IsSuccess);
            Assert.Equal(ResultStatus.Invalid, result.Status);
        }

        [Fact]
        public async Task Create_InvalidEmail_ShouldReturnInvalid()
        {
            var request = new CreateBasicUserRequest
            {
                UserName = "not-an-email",
                Password = "Test123!",
                Names = "John",
                LastNames = "Doe",
                CellPhone = "12345678"
            };

            var result = await CreateUseCase().ExecuteAsync(request);

            Assert.False(result.IsSuccess);
            Assert.Equal(ResultStatus.Invalid, result.Status);
        }

        [Fact]
        public async Task Create_WeakPassword_ShouldReturnInvalid()
        {
            var request = new CreateBasicUserRequest
            {
                UserName = "test@email.com",
                Password = "123",
                Names = "John",
                LastNames = "Doe",
                CellPhone = "12345678"
            };

            var result = await CreateUseCase().ExecuteAsync(request);

            Assert.False(result.IsSuccess);
            Assert.Equal(ResultStatus.Invalid, result.Status);
        }

        [Fact]
        public async Task Create_EmptyName_ShouldReturnInvalid()
        {
            var request = new CreateBasicUserRequest
            {
                UserName = "test@email.com",
                Password = "Test123!",
                Names = "",
                LastNames = "Doe",
                CellPhone = "12345678"
            };

            var result = await CreateUseCase().ExecuteAsync(request);

            Assert.False(result.IsSuccess);
            Assert.Equal(ResultStatus.Invalid, result.Status);
        }
    }
}
