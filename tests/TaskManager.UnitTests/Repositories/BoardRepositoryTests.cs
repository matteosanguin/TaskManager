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
    public class BoardRepositoryTests : TestBase
    {
        private readonly Mock<KanboardDbContext> _mockContext;
        private readonly BoardRepository _repository;

        public BoardRepositoryTests()
        {
            _mockContext = new Mock<KanboardDbContext>(new DbContextOptions<KanboardDbContext>());

            // Setup di base con un mock vuoto
            var emptyMockDbSet = new Mock<DbSet<Board>>();
            _mockContext.Setup(c => c.Set<Board>()).Returns(emptyMockDbSet.Object);

            _repository = new BoardRepository(_mockContext.Object);
        }

        [Fact]
        public async SystemTask GetByProjectIdAsync_WithValidProjectId_ReturnsBoard()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var board = new Board
            {
                Id = Guid.NewGuid(),
                Name = "Test Board",
                ProjectId = projectId
            };

            var boards = new List<Board> { board };
            var mockDbSet = MockDbSetHelper.CreateMockDbSet(boards);
            _mockContext.Setup(c => c.Set<Board>()).Returns(mockDbSet.Object);

            var repository = new BoardRepository(_mockContext.Object);

            // Act
            var result = await repository.GetByProjectIdAsync(projectId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(projectId, result.ProjectId);
            Assert.Equal("Test Board", result.Name);
        }

        [Fact]
        public async SystemTask GetWithColumnsAsync_WithValidBoardId_ReturnsBoardWithColumns()
        {
            // Arrange
            var boardId = Guid.NewGuid();
            var board = new Board { Id = boardId, Name = "Test Board" };

            var boards = new List<Board> { board };
            var mockDbSet = MockDbSetHelper.CreateMockDbSet(boards);
            _mockContext.Setup(c => c.Set<Board>()).Returns(mockDbSet.Object);

            var repository = new BoardRepository(_mockContext.Object);

            // Act
            var result = await repository.GetWithColumnsAsync(boardId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(boardId, result.Id);
            Assert.Equal("Test Board", result.Name);
        }
    }
}
