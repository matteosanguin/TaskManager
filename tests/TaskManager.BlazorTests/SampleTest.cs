using Bunit;
using Xunit;

namespace TaskManager.BlazorTests
{
    public class SampleTest : TestBase
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
