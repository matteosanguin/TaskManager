using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;

namespace TaskManager.UnitTests.Helpers
{
    public static class MockDbSetHelper
    {
        public static Mock<DbSet<T>> CreateMockDbSet<T>(IEnumerable<T> elements)
            where T : class
        {
            var list = elements.ToList();
            var queryable = list.AsQueryable();

            var mockSet = new Mock<DbSet<T>>();

            // Setup per IQueryable
            mockSet
                .As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<T>(queryable.Provider));
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet
                .As<IQueryable<T>>()
                .Setup(m => m.GetEnumerator())
                .Returns(queryable.GetEnumerator());

            // Setup per IAsyncEnumerable
            mockSet
                .As<IAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<T>(queryable.GetEnumerator()));

            // Setup per operazioni CRUD
            mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(item => list.Add(item));
            mockSet.Setup(m => m.Remove(It.IsAny<T>())).Callback<T>(item => list.Remove(item));

            return mockSet;
        }
    }

    internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal TestAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestAsyncEnumerable<TElement>(expression);
        }

        public object? Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public TResult ExecuteAsync<TResult>(
            Expression expression,
            CancellationToken cancellationToken = default
        )
        {
            var expectedResultType = typeof(TResult).GetGenericArguments()[0];
            var executionResult = typeof(IQueryProvider)
                .GetMethod(nameof(IQueryProvider.Execute), 1, new[] { typeof(Expression) })
                ?.MakeGenericMethod(expectedResultType)
                ?.Invoke(this, new[] { expression });

            return (TResult)(
                typeof(Task)
                    .GetMethod(nameof(Task.FromResult))
                    ?.MakeGenericMethod(expectedResultType)
                    ?.Invoke(null, new[] { executionResult })
                ?? throw new InvalidOperationException("Failed to create task")
            );
        }
    }

    internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public TestAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable) { }

        public TestAsyncEnumerable(Expression expression)
            : base(expression) { }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
    }

    internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        public T Current => _inner.Current;

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(_inner.MoveNext());
        }

        public ValueTask DisposeAsync()
        {
            _inner.Dispose();
            return default;
        }
    }
}
