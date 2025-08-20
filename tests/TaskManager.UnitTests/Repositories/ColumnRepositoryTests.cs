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
    public class ColumnRepositoryTests : TestBase
    {
        private readonly Mock<KanboardDbContext> _mockContext;
        private readonly Mock<DbSet<Column>> _mockDbSet;
        private readonly ColumnRepository _repository;

        public ColumnRepositoryTests()
        {
            _mockContext = new Mock<KanboardDbContext>(new DbContextOptions<KanboardDbContext>());
            _mockDbSet = new Mock<DbSet<Column>>();
            _mockContext.Setup(c => c.Set<Column>()).Returns(_mockDbSet.Object);
            _mockContext.Setup(c => c.Columns).Returns(_mockDbSet.Object);
            _repository = new ColumnRepository(_mockContext.Object);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetByProjectIdAsync_WithValidProjectId_ReturnsColumns()
        {
            // Arrange
            var projectId = System.Guid.NewGuid();
            var columns = new List<Column>
            {
                new Column
                {
                    Id = System.Guid.NewGuid(),
                    Title = "Column 1",
                    ProjectId = projectId
                },
                new Column
                {
                    Id = System.Guid.NewGuid(),
                    Title = "Column 2",
                    ProjectId = projectId
                }
            }.AsQueryable();

            _mockDbSet.As<IQueryable<Column>>().Setup(m => m.Provider).Returns(columns.Provider);
            _mockDbSet
                .As<IQueryable<Column>>()
                .Setup(m => m.Expression)
                .Returns(columns.Expression);
            _mockDbSet
                .As<IQueryable<Column>>()
                .Setup(m => m.ElementType)
                .Returns(columns.ElementType);
            _mockDbSet
                .As<IQueryable<Column>>()
                .Setup(m => m.GetEnumerator())
                .Returns(columns.GetEnumerator());

            // Act
            var result = await _repository.GetByProjectIdAsync(projectId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, c => Assert.Equal(projectId, c.ProjectId));
        }

        [Fact]
        public async System.Threading.Tasks.Task GetByProjectIdOrderedAsync_WithValidProjectId_ReturnsOrderedColumns()
        {
            // Arrange
            var projectId = System.Guid.NewGuid();
            var columns = new List<Column>
            {
                new Column
                {
                    Id = System.Guid.NewGuid(),
                    Title = "Column B",
                    ProjectId = projectId,
                    Position = 2
                },
                new Column
                {
                    Id = System.Guid.NewGuid(),
                    Title = "Column A",
                    ProjectId = projectId,
                    Position = 1
                },
                new Column
                {
                    Id = System.Guid.NewGuid(),
                    Title = "Column C",
                    ProjectId = projectId,
                    Position = 3
                }
            }.AsQueryable();

            _mockDbSet.As<IQueryable<Column>>().Setup(m => m.Provider).Returns(columns.Provider);
            _mockDbSet
                .As<IQueryable<Column>>()
                .Setup(m => m.Expression)
                .Returns(columns.Expression);
            _mockDbSet
                .As<IQueryable<Column>>()
                .Setup(m => m.ElementType)
                .Returns(columns.ElementType);
            _mockDbSet
                .As<IQueryable<Column>>()
                .Setup(m => m.GetEnumerator())
                .Returns(columns.GetEnumerator());

            // Act
            var result = await _repository.GetByProjectIdOrderedAsync(projectId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.Equal("Column A", result.ElementAt(0).Title);
            Assert.Equal("Column B", result.ElementAt(1).Title);
            Assert.Equal("Column C", result.ElementAt(2).Title);
        }
    }
}
