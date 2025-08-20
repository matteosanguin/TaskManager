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
    public class TaskRepositoryTests : TestBase
    {
        private readonly Mock<KanboardDbContext> _mockContext;
        private readonly Mock<DbSet<TaskManager.Domain.Entities.Task>> _mockDbSet;
        private readonly TaskRepository _repository;

        public TaskRepositoryTests()
        {
            _mockContext = new Mock<KanboardDbContext>(new DbContextOptions<KanboardDbContext>());
            _mockDbSet = new Mock<DbSet<TaskManager.Domain.Entities.Task>>();
            _mockContext
                .Setup(c => c.Set<TaskManager.Domain.Entities.Task>())
                .Returns(_mockDbSet.Object);
            _mockContext.Setup(c => c.Tasks).Returns(_mockDbSet.Object);
            _repository = new TaskRepository(_mockContext.Object);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetByColumnIdAsync_WithValidColumnId_ReturnsTasks()
        {
            // Arrange
            var columnId = System.Guid.NewGuid();
            var tasks = new List<TaskManager.Domain.Entities.Task>
            {
                new TaskManager.Domain.Entities.Task
                {
                    Id = System.Guid.NewGuid(),
                    Title = "Task 1",
                    ColumnId = columnId
                },
                new TaskManager.Domain.Entities.Task
                {
                    Id = System.Guid.NewGuid(),
                    Title = "Task 2",
                    ColumnId = columnId
                }
            }.AsQueryable();

            _mockDbSet
                .As<IQueryable<TaskManager.Domain.Entities.Task>>()
                .Setup(m => m.Provider)
                .Returns(tasks.Provider);
            _mockDbSet
                .As<IQueryable<TaskManager.Domain.Entities.Task>>()
                .Setup(m => m.Expression)
                .Returns(tasks.Expression);
            _mockDbSet
                .As<IQueryable<TaskManager.Domain.Entities.Task>>()
                .Setup(m => m.ElementType)
                .Returns(tasks.ElementType);
            _mockDbSet
                .As<IQueryable<TaskManager.Domain.Entities.Task>>()
                .Setup(m => m.GetEnumerator())
                .Returns(tasks.GetEnumerator());

            // Act
            var result = await _repository.GetByColumnIdAsync(columnId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, t => Assert.Equal(columnId, t.ColumnId));
        }

        [Fact]
        public async System.Threading.Tasks.Task GetByAssigneeIdAsync_WithValidAssigneeId_ReturnsTasks()
        {
            // Arrange
            var assigneeId = System.Guid.NewGuid();
            var tasks = new List<TaskManager.Domain.Entities.Task>
            {
                new TaskManager.Domain.Entities.Task
                {
                    Id = System.Guid.NewGuid(),
                    Title = "Task 1",
                    AssigneeId = assigneeId
                },
                new TaskManager.Domain.Entities.Task
                {
                    Id = System.Guid.NewGuid(),
                    Title = "Task 2",
                    AssigneeId = assigneeId
                }
            }.AsQueryable();

            _mockDbSet
                .As<IQueryable<TaskManager.Domain.Entities.Task>>()
                .Setup(m => m.Provider)
                .Returns(tasks.Provider);
            _mockDbSet
                .As<IQueryable<TaskManager.Domain.Entities.Task>>()
                .Setup(m => m.Expression)
                .Returns(tasks.Expression);
            _mockDbSet
                .As<IQueryable<TaskManager.Domain.Entities.Task>>()
                .Setup(m => m.ElementType)
                .Returns(tasks.ElementType);
            _mockDbSet
                .As<IQueryable<TaskManager.Domain.Entities.Task>>()
                .Setup(m => m.GetEnumerator())
                .Returns(tasks.GetEnumerator());

            // Act
            var result = await _repository.GetByAssigneeIdAsync(assigneeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, t => Assert.Equal(assigneeId, t.AssigneeId));
        }

        [Fact]
        public async System.Threading.Tasks.Task SearchTasksAsync_WithValidQuery_ReturnsMatchingTasks()
        {
            // Arrange
            var query = "test";
            var tasks = new List<TaskManager.Domain.Entities.Task>
            {
                new TaskManager.Domain.Entities.Task
                {
                    Id = System.Guid.NewGuid(),
                    Title = "Test Task 1",
                    Description = "Description 1"
                },
                new TaskManager.Domain.Entities.Task
                {
                    Id = System.Guid.NewGuid(),
                    Title = "Another Task",
                    Description = "Test description"
                },
                new TaskManager.Domain.Entities.Task
                {
                    Id = System.Guid.NewGuid(),
                    Title = "Unrelated Task",
                    Description = "Unrelated description"
                }
            }.AsQueryable();

            _mockDbSet
                .As<IQueryable<TaskManager.Domain.Entities.Task>>()
                .Setup(m => m.Provider)
                .Returns(tasks.Provider);
            _mockDbSet
                .As<IQueryable<TaskManager.Domain.Entities.Task>>()
                .Setup(m => m.Expression)
                .Returns(tasks.Expression);
            _mockDbSet
                .As<IQueryable<TaskManager.Domain.Entities.Task>>()
                .Setup(m => m.ElementType)
                .Returns(tasks.ElementType);
            _mockDbSet
                .As<IQueryable<TaskManager.Domain.Entities.Task>>()
                .Setup(m => m.GetEnumerator())
                .Returns(tasks.GetEnumerator());

            // Act
            var result = await _repository.SearchTasksAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskHistoryAsync_WithValidTaskId_ReturnsTask()
        {
            // Arrange
            var taskId = System.Guid.NewGuid();
            var task = new TaskManager.Domain.Entities.Task { Id = taskId, Title = "Test Task" };

            _mockDbSet
                .Setup(m => m.FindAsync(taskId))
                .Returns(new ValueTask<TaskManager.Domain.Entities.Task>(task));

            // Act
            var result = await _repository.GetTaskHistoryAsync(taskId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(taskId, result.First().Id);
        }
    }
}
