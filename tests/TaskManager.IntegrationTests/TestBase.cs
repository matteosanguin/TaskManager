using System;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TaskManager.IntegrationTests
{
    public abstract class TestBase : IDisposable
    {
        protected readonly WebApplicationFactory<object> Factory;

        protected TestBase()
        {
            Factory = new WebApplicationFactory<object>();
        }

        public virtual void Dispose()
        {
            Factory?.Dispose();
        }
    }
}
