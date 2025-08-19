using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Domain.Enums;

namespace TaskManager.Domain.ValueObjects
{
    [ComplexType]
    public class Priority
    {
        // Per Entity Framework Core
        private Priority()
        {
            Level = PriorityLevel.Normal;
        }

        private Priority(PriorityLevel level)
        {
            Level = level;
        }

        public PriorityLevel Level { get; private set; }

        public static Priority Create(PriorityLevel level)
        {
            return new Priority(level);
        }

        public static Priority Low => new Priority(PriorityLevel.Low);
        public static Priority Normal => new Priority(PriorityLevel.Normal);
        public static Priority High => new Priority(PriorityLevel.High);
        public static Priority Urgent => new Priority(PriorityLevel.Urgent);

        public override bool Equals(object? obj)
        {
            if (obj is Priority other)
                return Level == other.Level;

            return false;
        }

        public override int GetHashCode()
        {
            return Level.GetHashCode();
        }

        public override string ToString()
        {
            return Level.ToString();
        }
    }
}
