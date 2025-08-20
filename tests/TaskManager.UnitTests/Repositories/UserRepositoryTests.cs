using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManager.Domain.Entities;
using TaskManager.Domain.ValueObjects;
using TaskManager.Infrastructure.Persistence;
using TaskManager.Infrastructure.Repositories;
using TaskManager.UnitTests;
using Xunit;

namespace TaskManager.UnitTests.Repositories
{
    public class UserRepositoryTests : TestBase
    {
        private readonly Mock<KanboardDbContext> _mockContext;
        private readonly Mock<DbSet<User>> _mockDbSet;
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            _mockContext = new Mock<KanboardDbContext>(new DbContextOptions<KanboardDbContext>());
            _mockDbSet = new Mock<DbSet<User>>();
            _mockContext.Setup(c => c.Set<User>()).Returns(_mockDbSet.Object);
            _mockContext.Setup(c => c.Users).Returns(_mockDbSet.Object);
            _repository = new UserRepository(_mockContext.Object);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetByUsernameAsync_WithValidUsername_ReturnsUser()
        {
            // Arrange
            var username = "testuser";
            var user = new User
            {
                Id = System.Guid.NewGuid(),
                Username = username,
                Email = Email.Create("test@example.com")
            };

            var users = new List<User> { user }.AsQueryable();
            _mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            _mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            _mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            _mockDbSet
                .As<IQueryable<User>>()
                .Setup(m => m.GetEnumerator())
                .Returns(users.GetEnumerator());

            // Act
            var result = await _repository.GetByUsernameAsync(username);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(username, result.Username);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetByUsernameAsync_WithInvalidUsername_ReturnsNull()
        {
            // Arrange
            var username = "nonexistent";
            var users = new List<User>().AsQueryable();
            _mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            _mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            _mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            _mockDbSet
                .As<IQueryable<User>>()
                .Setup(m => m.GetEnumerator())
                .Returns(users.GetEnumerator());

            // Act
            var result = await _repository.GetByUsernameAsync(username);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetByEmailAsync_WithValidEmail_ReturnsUser()
        {
            // Arrange
            var email = "test@example.com";
            var user = new User
            {
                Id = System.Guid.NewGuid(),
                Username = "testuser",
                Email = Email.Create(email)
            };

            var users = new List<User> { user }.AsQueryable();
            _mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            _mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            _mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            _mockDbSet
                .As<IQueryable<User>>()
                .Setup(m => m.GetEnumerator())
                .Returns(users.GetEnumerator());

            // Act
            var result = await _repository.GetByEmailAsync(email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(email, result.Email.Value);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetActiveUsersAsync_ReturnsOnlyActiveUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User
                {
                    Id = System.Guid.NewGuid(),
                    Username = "active1",
                    Email = Email.Create("active1@example.com"),
                    IsActive = true
                },
                new User
                {
                    Id = System.Guid.NewGuid(),
                    Username = "inactive",
                    Email = Email.Create("inactive@example.com"),
                    IsActive = false
                },
                new User
                {
                    Id = System.Guid.NewGuid(),
                    Username = "active2",
                    Email = Email.Create("active2@example.com"),
                    IsActive = true
                }
            }.AsQueryable();

            _mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            _mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            _mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            _mockDbSet
                .As<IQueryable<User>>()
                .Setup(m => m.GetEnumerator())
                .Returns(users.GetEnumerator());

            // Act
            var result = await _repository.GetActiveUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, u => Assert.True(u.IsActive));
        }
    }
}
