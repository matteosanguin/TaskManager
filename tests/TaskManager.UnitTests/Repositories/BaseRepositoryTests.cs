using System.Linq.Expressions;
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
    public class BaseRepositoryTests : TestBase
    {
        private readonly Mock<KanboardDbContext> _mockContext;
        private readonly Mock<DbSet<Project>> _mockDbSet;
        private readonly BaseRepository<Project> _repository;

        public BaseRepositoryTests()
        {
            _mockContext = new Mock<KanboardDbContext>(new DbContextOptions<KanboardDbContext>());
            _mockDbSet = new Mock<DbSet<Project>>();
            _mockContext.Setup(c => c.Set<Project>()).Returns(_mockDbSet.Object);
            _repository = new BaseRepository<Project>(_mockContext.Object);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetByIdAsync_WithValidId_ReturnsEntity()
        {
            // Arrange
            var projectId = System.Guid.NewGuid();
            var project = new Project { Id = projectId, Name = "Test Project" };
            _mockDbSet.Setup(m => m.FindAsync(projectId)).Returns(new ValueTask<Project>(project));

            // Act
            var result = await _repository.GetByIdAsync(projectId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(projectId, result.Id);
            Assert.Equal("Test Project", result.Name);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var projectId = System.Guid.NewGuid();
            _mockDbSet
                .Setup(m => m.FindAsync(projectId))
                .Returns(new ValueTask<Project>((Project)null));

            // Act
            var result = await _repository.GetByIdAsync(projectId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAllAsync_ReturnsAllEntities()
        {
            // Arrange
            var projects = new List<Project>
            {
                new Project { Id = System.Guid.NewGuid(), Name = "Project 1" },
                new Project { Id = System.Guid.NewGuid(), Name = "Project 2" }
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
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async System.Threading.Tasks.Task FindAsync_WithValidPredicate_ReturnsFilteredEntities()
        {
            // Arrange
            var projects = new List<Project>
            {
                new Project
                {
                    Id = System.Guid.NewGuid(),
                    Name = "Project 1",
                    IsPrivate = false
                },
                new Project
                {
                    Id = System.Guid.NewGuid(),
                    Name = "Project 2",
                    IsPrivate = true
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
            System.Linq.Expressions.Expression<Func<Project, bool>> predicate = p =>
                p.IsPrivate == false;
            var result = await _repository.FindAsync(predicate);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Project 1", result.First().Name);
        }

        [Fact]
        public async System.Threading.Tasks.Task AddAsync_AddsEntityToContext()
        {
            // Arrange
            var project = new Project { Id = System.Guid.NewGuid(), Name = "New Project" };

            // Act
            await _repository.AddAsync(project);

            // Assert
            _mockDbSet.Verify(m => m.AddAsync(project, default), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task AddRangeAsync_AddsEntitiesToContext()
        {
            // Arrange
            var projects = new List<Project>
            {
                new Project { Id = System.Guid.NewGuid(), Name = "Project 1" },
                new Project { Id = System.Guid.NewGuid(), Name = "Project 2" }
            };

            // Act
            await _repository.AddRangeAsync(projects);

            // Assert
            _mockDbSet.Verify(m => m.AddRangeAsync(projects, default), Times.Once);
        }

        [Fact]
        public void Update_UpdatesEntityInContext()
        {
            // Arrange
            var project = new Project { Id = System.Guid.NewGuid(), Name = "Updated Project" };

            // Act
            _repository.Update(project);

            // Assert
            _mockDbSet.Verify(m => m.Update(project), Times.Once);
        }

        [Fact]
        public void Remove_RemovesEntityFromContext()
        {
            // Arrange
            var project = new Project { Id = System.Guid.NewGuid(), Name = "Project to Remove" };

            // Act
            _repository.Remove(project);

            // Assert
            _mockDbSet.Verify(m => m.Remove(project), Times.Once);
        }

        [Fact]
        public void RemoveRange_RemovesEntitiesFromContext()
        {
            // Arrange
            var projects = new List<Project>
            {
                new Project { Id = System.Guid.NewGuid(), Name = "Project 1" },
                new Project { Id = System.Guid.NewGuid(), Name = "Project 2" }
            };

            // Act
            _repository.RemoveRange(projects);

            // Assert
            _mockDbSet.Verify(m => m.RemoveRange(projects), Times.Once);
        }
    }
}
