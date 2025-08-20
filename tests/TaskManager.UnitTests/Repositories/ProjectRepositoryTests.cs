using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Persistence;
using TaskManager.Infrastructure.Repositories;
using TaskManager.UnitTests.Helpers;
using Xunit;
using SystemTask = System.Threading.Tasks.Task;

namespace TaskManager.UnitTests.Repositories
{
    public class ProjectRepositoryTests : TestBase
    {
        private readonly Mock<KanboardDbContext> _mockContext;
        private readonly ProjectRepository _repository;

        public ProjectRepositoryTests()
        {
            _mockContext = new Mock<KanboardDbContext>(new DbContextOptions<KanboardDbContext>());

            // Setup di base con un mock vuoto
            var emptyMockDbSet = new Mock<DbSet<Project>>();
            _mockContext.Setup(c => c.Set<Project>()).Returns(emptyMockDbSet.Object);

            _repository = new ProjectRepository(_mockContext.Object);
        }

        [Fact]
        public async SystemTask GetByOwnerIdAsync_WithValidOwnerId_ReturnsProjects()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var projects = new List<Project>
            {
                new Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Project 1",
                    OwnerId = ownerId
                },
                new Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Project 2",
                    OwnerId = ownerId
                },
                new Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Other Project",
                    OwnerId = Guid.NewGuid()
                }
            };

            var mockDbSet = MockDbSetHelper.CreateMockDbSet(projects);
            _mockContext.Setup(c => c.Set<Project>()).Returns(mockDbSet.Object);

            var repository = new ProjectRepository(_mockContext.Object);

            // Act
            var result = await repository.GetByOwnerIdAsync(ownerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, p => Assert.Equal(ownerId, p.OwnerId));
        }

        [Fact]
        public async SystemTask GetWithTasksAsync_WithValidProjectId_ReturnsProjectWithTasks()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var project = new Project { Id = projectId, Name = "Test Project" };

            var projects = new List<Project> { project };
            var mockDbSet = MockDbSetHelper.CreateMockDbSet(projects);
            _mockContext.Setup(c => c.Set<Project>()).Returns(mockDbSet.Object);

            var repository = new ProjectRepository(_mockContext.Object);

            // Act
            var result = await repository.GetWithTasksAsync(projectId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(projectId, result.Id);
            Assert.Equal("Test Project", result.Name);
        }

        [Fact]
        public async SystemTask GetPublicProjectsAsync_ReturnsOnlyPublicProjects()
        {
            // Arrange
            var projects = new List<Project>
            {
                new Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Public Project 1",
                    IsPrivate = false
                },
                new Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Private Project",
                    IsPrivate = true
                },
                new Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Public Project 2",
                    IsPrivate = false
                }
            };

            var mockDbSet = MockDbSetHelper.CreateMockDbSet(projects);
            _mockContext.Setup(c => c.Set<Project>()).Returns(mockDbSet.Object);

            var repository = new ProjectRepository(_mockContext.Object);

            // Act
            var result = await repository.GetPublicProjectsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, p => Assert.False(p.IsPrivate));
        }
    }
}
