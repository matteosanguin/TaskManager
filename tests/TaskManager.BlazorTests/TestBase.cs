using System;
using Bunit;

namespace TaskManager.BlazorTests
{
    public abstract class TestBase : IDisposable
    {
        protected readonly TestContext Context;

        protected TestBase()
        {
            Context = new TestContext();
        }

        public virtual void Dispose()
        {
            Context?.Dispose();
        }
    }
}
