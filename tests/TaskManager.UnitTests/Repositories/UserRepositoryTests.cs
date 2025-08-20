using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManager.Domain.Entities;
using TaskManager.Domain.ValueObjects;
using TaskManager.Infrastructure.Persistence;
using TaskManager.Infrastructure.Repositories;
using TaskManager.UnitTests.Helpers;
using Xunit;
using SystemTask = System.Threading.Tasks.Task;

namespace TaskManager.UnitTests.Repositories
{
    public class UserRepositoryTests : TestBase
    {
        private readonly Mock<KanboardDbContext> _mockContext;
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            _mockContext = new Mock<KanboardDbContext>(new DbContextOptions<KanboardDbContext>());

            // Setup di base con un mock vuoto
            var emptyMockDbSet = new Mock<DbSet<User>>();
            _mockContext.Setup(c => c.Set<User>()).Returns(emptyMockDbSet.Object);

            _repository = new UserRepository(_mockContext.Object);
        }

        [Fact]
        public async SystemTask GetByUsernameAsync_WithValidUsername_ReturnsUser()
        {
            // Arrange
            var username = "testuser";
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Email = Email.Create("test@example.com")
            };

            var users = new List<User> { user };
            var mockDbSet = MockDbSetHelper.CreateMockDbSet(users);
            _mockContext.Setup(c => c.Set<User>()).Returns(mockDbSet.Object);

            var repository = new UserRepository(_mockContext.Object);

            // Act
            var result = await repository.GetByUsernameAsync(username);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(username, result.Username);
        }

        [Fact]
        public async SystemTask GetByUsernameAsync_WithInvalidUsername_ReturnsNull()
        {
            // Arrange
            var username = "nonexistent";
            var users = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Username = "otheruser",
                    Email = Email.Create("other@example.com")
                }
            };

            var mockDbSet = MockDbSetHelper.CreateMockDbSet(users);
            _mockContext.Setup(c => c.Set<User>()).Returns(mockDbSet.Object);

            var repository = new UserRepository(_mockContext.Object);

            // Act
            var result = await repository.GetByUsernameAsync(username);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async SystemTask GetByEmailAsync_WithValidEmail_ReturnsUser()
        {
            // Arrange
            var email = "test@example.com";
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Email = Email.Create(email)
            };

            var users = new List<User> { user };
            var mockDbSet = MockDbSetHelper.CreateMockDbSet(users);
            _mockContext.Setup(c => c.Set<User>()).Returns(mockDbSet.Object);

            var repository = new UserRepository(_mockContext.Object);

            // Act
            var result = await repository.GetByEmailAsync(email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(email, result.Email.Value);
        }

        [Fact]
        public async SystemTask GetActiveUsersAsync_ReturnsOnlyActiveUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Username = "active1",
                    Email = Email.Create("active1@example.com"),
                    IsActive = true
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Username = "inactive",
                    Email = Email.Create("inactive@example.com"),
                    IsActive = false
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Username = "active2",
                    Email = Email.Create("active2@example.com"),
                    IsActive = true
                }
            };

            var mockDbSet = MockDbSetHelper.CreateMockDbSet(users);
            _mockContext.Setup(c => c.Set<User>()).Returns(mockDbSet.Object);

            var repository = new UserRepository(_mockContext.Object);

            // Act
            var result = await repository.GetActiveUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, u => Assert.True(u.IsActive));
        }
    }
}
