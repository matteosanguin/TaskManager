using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Persistence;
using TaskManager.Infrastructure.Repositories;
using TaskManager.UnitTests;
using Xunit;

namespace TaskManager.UnitTests.Repositories
{
    public class ProjectRepositoryTests : TestBase
    {
        private readonly Mock<KanboardDbContext> _mockContext;
        private readonly Mock<DbSet<Project>> _mockDbSet;
        private readonly ProjectRepository _repository;

        public ProjectRepositoryTests()
        {
            _mockContext = new Mock<KanboardDbContext>(new DbContextOptions<KanboardDbContext>());
            _mockDbSet = new Mock<DbSet<Project>>();
            _mockContext.Setup(c => c.Set<Project>()).Returns(_mockDbSet.Object);
            _mockContext.Setup(c => c.Projects).Returns(_mockDbSet.Object);
            _repository = new ProjectRepository(_mockContext.Object);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetByOwnerIdAsync_WithValidOwnerId_ReturnsProjects()
        {
            // Arrange
            var ownerId = System.Guid.NewGuid();
            var projects = new List<Project>
            {
                new Project
                {
                    Id = System.Guid.NewGuid(),
                    Name = "Project 1",
                    OwnerId = ownerId
                },
                new Project
                {
                    Id = System.Guid.NewGuid(),
                    Name = "Project 2",
                    OwnerId = ownerId
                }
            }.AsQueryable();

            _mockDbSet.As<IQueryable<Project>>().Setup(m => m.Provider).Returns(projects.Provider);
            _mockDbSet
                .As<IQueryable<Project>>()
                .Setup(m => m.Expression)
                .Returns(projects.Expression);
            _mockDbSet
                .As<IQueryable<Project>>()
                .Setup(m => m.ElementType)
                .Returns(projects.ElementType);
            _mockDbSet
                .As<IQueryable<Project>>()
                .Setup(m => m.GetEnumerator())
                .Returns(projects.GetEnumerator());

            // Act
            var result = await _repository.GetByOwnerIdAsync(ownerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, p => Assert.Equal(ownerId, p.OwnerId));
        }

        [Fact]
        public async System.Threading.Tasks.Task GetWithTasksAsync_WithValidProjectId_ReturnsProjectWithTasks()
        {
            // Arrange
            var projectId = System.Guid.NewGuid();
            var project = new Project
            {
                Id = projectId,
                Name = "Test Project",
                Tasks = new List<TaskManager.Domain.Entities.Task>
                {
                    new TaskManager.Domain.Entities.Task
                    {
                        Id = System.Guid.NewGuid(),
                        Title = "Task 1"
                    },
                    new TaskManager.Domain.Entities.Task
                    {
                        Id = System.Guid.NewGuid(),
                        Title = "Task 2"
                    }
                }
            };

            var projects = new List<Project> { project }.AsQueryable();
            var mockSet = new Mock<DbSet<Project>>();
            mockSet.As<IQueryable<Project>>().Setup(m => m.Provider).Returns(projects.Provider);
            mockSet.As<IQueryable<Project>>().Setup(m => m.Expression).Returns(projects.Expression);
            mockSet
                .As<IQueryable<Project>>()
                .Setup(m => m.ElementType)
                .Returns(projects.ElementType);
            mockSet
                .As<IQueryable<Project>>()
                .Setup(m => m.GetEnumerator())
                .Returns(projects.GetEnumerator());
            mockSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSet.Object);

            _mockContext.Setup(c => c.Projects).Returns(mockSet.Object);

            // Act
            var result = await _repository.GetWithTasksAsync(projectId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(projectId, result.Id);
            Assert.Equal(2, result.Tasks.Count);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetPublicProjectsAsync_ReturnsOnlyPublicProjects()
        {
            // Arrange
            var projects = new List<Project>
            {
                new Project
                {
                    Id = System.Guid.NewGuid(),
                    Name = "Public Project 1",
                    IsPrivate = false
                },
                new Project
                {
                    Id = System.Guid.NewGuid(),
                    Name = "Private Project",
                    IsPrivate = true
                },
                new Project
                {
                    Id = System.Guid.NewGuid(),
                    Name = "Public Project 2",
                    IsPrivate = false
                }
            }.AsQueryable();

            _mockDbSet.As<IQueryable<Project>>().Setup(m => m.Provider).Returns(projects.Provider);
            _mockDbSet
                .As<IQueryable<Project>>()
                .Setup(m => m.Expression)
                .Returns(projects.Expression);
            _mockDbSet
                .As<IQueryable<Project>>()
                .Setup(m => m.ElementType)
                .Returns(projects.ElementType);
            _mockDbSet
                .As<IQueryable<Project>>()
                .Setup(m => m.GetEnumerator())
                .Returns(projects.GetEnumerator());

            // Act
            var result = await _repository.GetPublicProjectsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, p => Assert.False(p.IsPrivate));
        }
    }
}
