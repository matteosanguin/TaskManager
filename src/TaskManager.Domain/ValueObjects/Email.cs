using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Domain.ValueObjects
{
    [ComplexType]
    public class Email
    {
        // Per Entity Framework Core
        private Email()
        {
            Value = string.Empty;
        }

        private Email(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public static Email Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email cannot be null or empty", nameof(value));

            if (!IsValidEmail(value))
                throw new ArgumentException("Invalid email format", nameof(value));

            return new Email(value);
        }

        private static bool IsValidEmail(string email)
        {
            // Implementazione della validazione email
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj is Email other)
                return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);

            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
