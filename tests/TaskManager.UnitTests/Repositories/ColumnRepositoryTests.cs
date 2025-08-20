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
    public class ColumnRepositoryTests : TestBase
    {
        private readonly Mock<KanboardDbContext> _mockContext;
        private readonly ColumnRepository _repository;

        public ColumnRepositoryTests()
        {
            _mockContext = new Mock<KanboardDbContext>(new DbContextOptions<KanboardDbContext>());

            // Setup di base con un mock vuoto
            var emptyMockDbSet = new Mock<DbSet<Column>>();
            _mockContext.Setup(c => c.Set<Column>()).Returns(emptyMockDbSet.Object);

            _repository = new ColumnRepository(_mockContext.Object);
        }

        [Fact]
        public async SystemTask GetByProjectIdAsync_WithValidProjectId_ReturnsColumns()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var columns = new List<Column>
            {
                new Column
                {
                    Id = Guid.NewGuid(),
                    Title = "Column 1",
                    ProjectId = projectId
                },
                new Column
                {
                    Id = Guid.NewGuid(),
                    Title = "Column 2",
                    ProjectId = projectId
                },
                new Column
                {
                    Id = Guid.NewGuid(),
                    Title = "Other Column",
                    ProjectId = Guid.NewGuid()
                }
            };

            var mockDbSet = MockDbSetHelper.CreateMockDbSet(columns);
            _mockContext.Setup(c => c.Set<Column>()).Returns(mockDbSet.Object);

            var repository = new ColumnRepository(_mockContext.Object);

            // Act
            var result = await repository.GetByProjectIdAsync(projectId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, c => Assert.Equal(projectId, c.ProjectId));
        }

        [Fact]
        public async SystemTask GetByProjectIdOrderedAsync_WithValidProjectId_ReturnsOrderedColumns()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var columns = new List<Column>
            {
                new Column
                {
                    Id = Guid.NewGuid(),
                    Title = "Column B",
                    ProjectId = projectId,
                    Position = 2
                },
                new Column
                {
                    Id = Guid.NewGuid(),
                    Title = "Column A",
                    ProjectId = projectId,
                    Position = 1
                },
                new Column
                {
                    Id = Guid.NewGuid(),
                    Title = "Column C",
                    ProjectId = projectId,
                    Position = 3
                },
                new Column
                {
                    Id = Guid.NewGuid(),
                    Title = "Other Column",
                    ProjectId = Guid.NewGuid(),
                    Position = 1
                }
            };

            var mockDbSet = MockDbSetHelper.CreateMockDbSet(columns);
            _mockContext.Setup(c => c.Set<Column>()).Returns(mockDbSet.Object);

            var repository = new ColumnRepository(_mockContext.Object);

            // Act
            var result = await repository.GetByProjectIdOrderedAsync(projectId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            // Verifica l'ordinamento per Position
            var resultList = result.ToList();
            Assert.Equal(1, resultList[0].Position);
            Assert.Equal(2, resultList[1].Position);
            Assert.Equal(3, resultList[2].Position);
            Assert.Equal("Column A", resultList[0].Title);
            Assert.Equal("Column B", resultList[1].Title);
            Assert.Equal("Column C", resultList[2].Title);
        }
    }
}
