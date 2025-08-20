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
    public class BoardRepositoryTests : TestBase
    {
        private readonly Mock<KanboardDbContext> _mockContext;
        private readonly Mock<DbSet<Board>> _mockDbSet;
        private readonly BoardRepository _repository;

        public BoardRepositoryTests()
        {
            _mockContext = new Mock<KanboardDbContext>(new DbContextOptions<KanboardDbContext>());
            _mockDbSet = new Mock<DbSet<Board>>();
            _mockContext.Setup(c => c.Set<Board>()).Returns(_mockDbSet.Object);
            _mockContext.Setup(c => c.Boards).Returns(_mockDbSet.Object);
            _repository = new BoardRepository(_mockContext.Object);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetByProjectIdAsync_WithValidProjectId_ReturnsBoard()
        {
            // Arrange
            var projectId = System.Guid.NewGuid();
            var board = new Board
            {
                Id = System.Guid.NewGuid(),
                Name = "Test Board",
                ProjectId = projectId
            };

            var boards = new List<Board> { board }.AsQueryable();
            _mockDbSet.As<IQueryable<Board>>().Setup(m => m.Provider).Returns(boards.Provider);
            _mockDbSet.As<IQueryable<Board>>().Setup(m => m.Expression).Returns(boards.Expression);
            _mockDbSet
                .As<IQueryable<Board>>()
                .Setup(m => m.ElementType)
                .Returns(boards.ElementType);
            _mockDbSet
                .As<IQueryable<Board>>()
                .Setup(m => m.GetEnumerator())
                .Returns(boards.GetEnumerator());

            // Act
            var result = await _repository.GetByProjectIdAsync(projectId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(projectId, result.ProjectId);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetWithColumnsAsync_WithValidBoardId_ReturnsBoardWithColumns()
        {
            // Arrange
            var boardId = System.Guid.NewGuid();
            var board = new Board
            {
                Id = boardId,
                Name = "Test Board",
                Columns = new List<Column>
                {
                    new Column { Id = System.Guid.NewGuid(), Title = "Column 1" },
                    new Column { Id = System.Guid.NewGuid(), Title = "Column 2" }
                }
            };

            var boards = new List<Board> { board }.AsQueryable();
            var mockSet = new Mock<DbSet<Board>>();
            mockSet.As<IQueryable<Board>>().Setup(m => m.Provider).Returns(boards.Provider);
            mockSet.As<IQueryable<Board>>().Setup(m => m.Expression).Returns(boards.Expression);
            mockSet.As<IQueryable<Board>>().Setup(m => m.ElementType).Returns(boards.ElementType);
            mockSet
                .As<IQueryable<Board>>()
                .Setup(m => m.GetEnumerator())
                .Returns(boards.GetEnumerator());
            mockSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSet.Object);

            _mockContext.Setup(c => c.Boards).Returns(mockSet.Object);

            // Act
            var result = await _repository.GetWithColumnsAsync(boardId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(boardId, result.Id);
            Assert.Equal(2, result.Columns.Count);
        }
    }
}
