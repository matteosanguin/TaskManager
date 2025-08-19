using Xunit;

namespace TaskManager.UnitTests
{
    public class SampleTest
    {
        [Fact]
        public void SampleTest_True_IsTrue()
        {
            // Arrange
            var condition = true;

            // Act & Assert
            Assert.True(condition);
        }
    }
}
