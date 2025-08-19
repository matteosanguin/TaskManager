using System;
using AutoFixture;

namespace TaskManager.UnitTests
{
    public abstract class TestBase : IDisposable
    {
        protected readonly Fixture Fixture = new Fixture();

        public virtual void Dispose()
        {
            // Cleanup comune per tutti i test
        }
    }
}
