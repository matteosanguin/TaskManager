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
using DomainTask = TaskManager.Domain.Entities.Task;
using SystemTask = System.Threading.Tasks.Task;

namespace TaskManager.UnitTests.Repositories
{
    public class TaskRepositoryTests : TestBase
    {
        private readonly Mock<KanboardDbContext> _mockContext;
        private readonly TaskRepository _repository;

        public TaskRepositoryTests()
        {
            _mockContext = new Mock<KanboardDbContext>(new DbContextOptions<KanboardDbContext>());

            // Setup di base con un mock vuoto
            var emptyMockDbSet = new Mock<DbSet<DomainTask>>();
            _mockContext.Setup(c => c.Set<DomainTask>()).Returns(emptyMockDbSet.Object);

            _repository = new TaskRepository(_mockContext.Object);
        }

        [Fact]
        public async SystemTask GetByColumnIdAsync_WithValidColumnId_ReturnsTasks()
        {
            // Arrange
            var columnId = Guid.NewGuid();
            var tasks = new List<DomainTask>
            {
                new DomainTask
                {
                    Id = Guid.NewGuid(),
                    Title = "Task 1",
                    ColumnId = columnId
                },
                new DomainTask
                {
                    Id = Guid.NewGuid(),
                    Title = "Task 2",
                    ColumnId = columnId
                },
                new DomainTask
                {
                    Id = Guid.NewGuid(),
                    Title = "Other Task",
                    ColumnId = Guid.NewGuid()
                }
            };

            var mockDbSet = MockDbSetHelper.CreateMockDbSet(tasks);
            _mockContext.Setup(c => c.Set<DomainTask>()).Returns(mockDbSet.Object);

            var repository = new TaskRepository(_mockContext.Object);

            // Act
            var result = await repository.GetByColumnIdAsync(columnId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, t => Assert.Equal(columnId, t.ColumnId));
        }

        [Fact]
        public async SystemTask GetByAssigneeIdAsync_WithValidAssigneeId_ReturnsTasks()
        {
            // Arrange
            var assigneeId = Guid.NewGuid();
            var tasks = new List<DomainTask>
            {
                new DomainTask
                {
                    Id = Guid.NewGuid(),
                    Title = "Task 1",
                    AssigneeId = assigneeId
                },
                new DomainTask
                {
                    Id = Guid.NewGuid(),
                    Title = "Task 2",
                    AssigneeId = assigneeId
                },
                new DomainTask
                {
                    Id = Guid.NewGuid(),
                    Title = "Other Task",
                    AssigneeId = Guid.NewGuid()
                }
            };

            var mockDbSet = MockDbSetHelper.CreateMockDbSet(tasks);
            _mockContext.Setup(c => c.Set<DomainTask>()).Returns(mockDbSet.Object);

            var repository = new TaskRepository(_mockContext.Object);

            // Act
            var result = await repository.GetByAssigneeIdAsync(assigneeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, t => Assert.Equal(assigneeId, t.AssigneeId));
        }

        [Fact]
        public async SystemTask SearchTasksAsync_WithValidQuery_ReturnsMatchingTasks()
        {
            // Arrange
            var query = "Test"; // Cambiato da "test" a "Test" per matchare i dati
            var tasks = new List<DomainTask>
            {
                new DomainTask
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Task 1", // Contiene "Test"
                    Description = "Description 1"
                },
                new DomainTask
                {
                    Id = Guid.NewGuid(),
                    Title = "Another Task",
                    Description = "Test description" // Contiene "Test"
                },
                new DomainTask
                {
                    Id = Guid.NewGuid(),
                    Title = "Unrelated Task",
                    Description = "Unrelated description" // Non contiene "Test"
                }
            };

            var mockDbSet = MockDbSetHelper.CreateMockDbSet(tasks);
            _mockContext.Setup(c => c.Set<DomainTask>()).Returns(mockDbSet.Object);

            var repository = new TaskRepository(_mockContext.Object);

            // Act
            var result = await repository.SearchTasksAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            // Verifica che i risultati contengano effettivamente il termine cercato
            Assert.All(
                result,
                task => Assert.True(task.Title.Contains(query) || task.Description.Contains(query))
            );
        }

        [Fact]
        public async SystemTask GetTaskHistoryAsync_WithValidTaskId_ReturnsTask()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var task = new DomainTask { Id = taskId, Title = "Test Task" };

            // Per questo test specifico, usiamo un mock diretto per FindAsync
            var mockDbSet = new Mock<DbSet<DomainTask>>();
            mockDbSet.Setup(m => m.FindAsync(taskId)).Returns(new ValueTask<DomainTask?>(task));
            _mockContext.Setup(c => c.Set<DomainTask>()).Returns(mockDbSet.Object);

            var repository = new TaskRepository(_mockContext.Object);

            // Act
            var result = await repository.GetTaskHistoryAsync(taskId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(taskId, result.First().Id);
        }
    }
}
