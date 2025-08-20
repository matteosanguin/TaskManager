using System;
using TaskManager.Domain.ValueObjects;
using Xunit;

namespace TaskManager.UnitTests.Domain.ValueObjects
{
    public class EmailTests
    {
        [Fact]
        public void Create_WithValidEmail_ShouldCreateEmail()
        {
            // Arrange
            var emailValue = "test@example.com";

            // Act
            var email = Email.Create(emailValue);

            // Assert
            Assert.NotNull(email);
            Assert.Equal(emailValue, email.Value);
        }

        [Fact]
        public void Create_WithNullEmail_ShouldThrowArgumentException()
        {
            // Arrange
            string? emailValue = null;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Email.Create(emailValue!));
        }

        [Fact]
        public void Create_WithEmptyEmail_ShouldThrowArgumentException()
        {
            // Arrange
            var emailValue = "";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Email.Create(emailValue));
        }

        [Fact]
        public void Create_WithInvalidEmail_ShouldThrowArgumentException()
        {
            // Arrange
            var emailValue = "invalid-email";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Email.Create(emailValue));
        }

        [Fact]
        public void Equals_WithSameEmailValue_ShouldReturnTrue()
        {
            // Arrange
            var email1 = Email.Create("test@example.com");
            var email2 = Email.Create("test@example.com");

            // Act
            var result = email1.Equals(email2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_WithDifferentEmailValue_ShouldReturnFalse()
        {
            // Arrange
            var email1 = Email.Create("test1@example.com");
            var email2 = Email.Create("test2@example.com");

            // Act
            var result = email1.Equals(email2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetHashCode_WithSameEmailValue_ShouldReturnSameHashCode()
        {
            // Arrange
            var email1 = Email.Create("test@example.com");
            var email2 = Email.Create("test@example.com");

            // Act
            var hashCode1 = email1.GetHashCode();
            var hashCode2 = email2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        [Fact]
        public void ToString_ShouldReturnEmailValue()
        {
            // Arrange
            var emailValue = "test@example.com";
            var email = Email.Create(emailValue);

            // Act
            var result = email.ToString();

            // Assert
            Assert.Equal(emailValue, result);
        }
    }
}
