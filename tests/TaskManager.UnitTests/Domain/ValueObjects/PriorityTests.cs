using TaskManager.Domain.Enums;
using TaskManager.Domain.ValueObjects;
using Xunit;

namespace TaskManager.UnitTests.Domain.ValueObjects
{
    public class PriorityTests
    {
        [Fact]
        public void Create_WithValidLevel_ShouldCreatePriority()
        {
            // Arrange
            var level = PriorityLevel.High;

            // Act
            var priority = Priority.Create(level);

            // Assert
            Assert.NotNull(priority);
            Assert.Equal(level, priority.Level);
        }

        [Fact]
        public void StaticProperties_ShouldReturnCorrectPriorityLevels()
        {
            // Act
            var low = Priority.Low;
            var normal = Priority.Normal;
            var high = Priority.High;
            var urgent = Priority.Urgent;

            // Assert
            Assert.Equal(PriorityLevel.Low, low.Level);
            Assert.Equal(PriorityLevel.Normal, normal.Level);
            Assert.Equal(PriorityLevel.High, high.Level);
            Assert.Equal(PriorityLevel.Urgent, urgent.Level);
        }

        [Fact]
        public void Equals_WithSameLevel_ShouldReturnTrue()
        {
            // Arrange
            var priority1 = Priority.Create(PriorityLevel.High);
            var priority2 = Priority.Create(PriorityLevel.High);

            // Act
            var result = priority1.Equals(priority2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_WithDifferentLevel_ShouldReturnFalse()
        {
            // Arrange
            var priority1 = Priority.Create(PriorityLevel.High);
            var priority2 = Priority.Create(PriorityLevel.Low);

            // Act
            var result = priority1.Equals(priority2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetHashCode_WithSameLevel_ShouldReturnSameHashCode()
        {
            // Arrange
            var priority1 = Priority.Create(PriorityLevel.High);
            var priority2 = Priority.Create(PriorityLevel.High);

            // Act
            var hashCode1 = priority1.GetHashCode();
            var hashCode2 = priority2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        [Fact]
        public void ToString_ShouldReturnLevelName()
        {
            // Arrange
            var priority = Priority.Create(PriorityLevel.High);

            // Act
            var result = priority.ToString();

            // Assert
            Assert.Equal("High", result);
        }
    }
}
