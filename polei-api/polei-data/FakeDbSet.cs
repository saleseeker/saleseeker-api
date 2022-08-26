using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace saleseeker_data
{
    // ************************************************************************
    // Fake DbSet
    // Implementing Find:
    //      The Find method is difficult to implement in a generic fashion. If
    //      you need to test code that makes use of the Find method it is
    //      easiest to create a test DbSet for each of the entity types that
    //      need to support find. You can then write logic to find that
    //      particular type of entity, as shown below:
    //      public class FakeBlogDbSet : FakeDbSet<Blog>
    //      {
    //          public override Blog Find(params object[] keyValues)
    //          {
    //              var id = (int) keyValues.Single();
    //              return this.SingleOrDefault(b => b.BlogId == id);
    //          }
    //      }
    //      Read more about it here: https://msdn.microsoft.com/en-us/data/dn314431.aspx
    public class FakeDbSet<TEntity> :
        DbSet<TEntity>,
        IQueryable<TEntity>,
        IAsyncEnumerable<TEntity>,
        IListSource,
        IResettableService
        where TEntity : class
    {
        private readonly PropertyInfo[] _primaryKeys;
        private ObservableCollection<TEntity> _data;
        private IQueryable _query;
        public override IEntityType EntityType { get; }

        public FakeDbSet()
        {
            _primaryKeys = null;
            _data        = new ObservableCollection<TEntity>();
            _query       = _data.AsQueryable();
        }

        public FakeDbSet(params string[] primaryKeys)
        {
            _primaryKeys = typeof(TEntity).GetProperties().Where(x => primaryKeys.Contains(x.Name)).ToArray();
            _data        = new ObservableCollection<TEntity>();
            _query       = _data.AsQueryable();
        }

        public override TEntity Find(params object[] keyValues)
        {
            if (_primaryKeys == null)
                throw new ArgumentException("No primary keys defined");
            if (keyValues.Length != _primaryKeys.Length)
                throw new ArgumentException("Incorrect number of keys passed to Find method");

            var keyQuery = this.AsQueryable();
            keyQuery = keyValues
                .Select((t, i) => i)
                .Aggregate(keyQuery,
                    (current, x) =>
                        current.Where(entity => _primaryKeys[x].GetValue(entity, null).Equals(keyValues[x])));

            return keyQuery.SingleOrDefault();
        }

        public override ValueTask<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken)
        {
            return new ValueTask<TEntity>(Task<TEntity>.Factory.StartNew(() => Find(keyValues), cancellationToken));
        }

        public override ValueTask<TEntity> FindAsync(params object[] keyValues)
        {
            return new ValueTask<TEntity>(Task<TEntity>.Factory.StartNew(() => Find(keyValues)));
        }

        public override EntityEntry<TEntity> Add(TEntity entity)
        {
            _data.Add(entity);
            return null;
        }

        public override ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return new ValueTask<EntityEntry<TEntity>>(Task<EntityEntry<TEntity>>.Factory.StartNew(() => Add(entity)));
        }

        public override void AddRange(params TEntity[] entities)
        {
            if (entities == null) throw new ArgumentNullException("entities");
            foreach (var entity in entities)
                _data.Add(entity);
        }

        public override void AddRange(IEnumerable<TEntity> entities)
        {
            if (entities == null) throw new ArgumentNullException("entities");
            foreach (var entity in entities)
                _data.Add(entity);
        }

        public override Task AddRangeAsync(params TEntity[] entities)
        {
            if (entities == null) throw new ArgumentNullException("entities");
            return Task.Factory.StartNew(() => AddRange(entities));
        }

        public override Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            if (entities == null) throw new ArgumentNullException("entities");
            return Task.Factory.StartNew(() => AddRange(entities));
        }

        public override EntityEntry<TEntity> Attach(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            return Add(entity);
        }

        public override void AttachRange(params TEntity[] entities)
        {
            if (entities == null) throw new ArgumentNullException("entities");
            AddRange(entities);
        }

        public override void AttachRange(IEnumerable<TEntity> entities)
        {
            if (entities == null) throw new ArgumentNullException("entities");
            AddRange(entities);
        }

        public override EntityEntry<TEntity> Remove(TEntity entity)
        {
            _data.Remove(entity);
            return null;
        }

        public override void RemoveRange(params TEntity[] entities)
        {
            if (entities == null) throw new ArgumentNullException("entities");
            foreach (var entity in entities.ToList())
                _data.Remove(entity);
        }

        public override void RemoveRange(IEnumerable<TEntity> entities)
        {
            RemoveRange(entities.ToArray());
        }

        public override EntityEntry<TEntity> Update(TEntity entity)
        {
            _data.Remove(entity);
            _data.Add(entity);
            return null;
        }

        public override void UpdateRange(params TEntity[] entities)
        {
            if (entities == null) throw new ArgumentNullException("entities");
            RemoveRange(entities);
            AddRange(entities);
        }

        public override void UpdateRange(IEnumerable<TEntity> entities)
        {
            if (entities == null) throw new ArgumentNullException("entities");
            var array = entities.ToArray();        RemoveRange(array);
            AddRange(array);
        }

        bool IListSource.ContainsListCollection => true;

        public IList GetList()
        {
            return _data;
        }

        IList IListSource.GetList()
        {
            return _data;
        }

        Type IQueryable.ElementType
        {
            get { return _query.ElementType; }
        }

        Expression IQueryable.Expression
        {
            get { return _query.Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new FakeDbAsyncQueryProvider<TEntity>(_data); }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public override IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new FakeDbAsyncEnumerator<TEntity>(this.AsEnumerable().GetEnumerator());
        }

        public void ResetState()
        {
            _data  = new ObservableCollection<TEntity>();
            _query = _data.AsQueryable();
        }

        public Task ResetStateAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.Factory.StartNew(() => ResetState());
        }
    }

    public class FakeDbAsyncQueryProvider<TEntity> : FakeQueryProvider<TEntity>, IAsyncEnumerable<TEntity>, IAsyncQueryProvider
    {
        public FakeDbAsyncQueryProvider(Expression expression) : base(expression)
        {
        }

        public FakeDbAsyncQueryProvider(IEnumerable<TEntity> enumerable) : base(enumerable)
        {
        }

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            var expectedResultType = typeof(TResult).GetGenericArguments()[0];
            var executionResult = typeof(IQueryProvider)
                .GetMethods()
                .First(method => method.Name == nameof(IQueryProvider.Execute) && method.IsGenericMethod)
                .MakeGenericMethod(expectedResultType)
                .Invoke(this, new object[] { expression });

            return (TResult) typeof(Task).GetMethod(nameof(Task.FromResult))
                ?.MakeGenericMethod(expectedResultType)
                .Invoke(null, new[] { executionResult });
        }

        public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new FakeDbAsyncEnumerator<TEntity>(this.AsEnumerable().GetEnumerator());
        }
    }

    public class FakeDbAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public FakeDbAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        {
        }

        public FakeDbAsyncEnumerable(Expression expression)
            : base(expression)
        {
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
        {
            return new FakeDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IAsyncEnumerator<T> IAsyncEnumerable<T>.GetAsyncEnumerator(CancellationToken cancellationToken)
        {
            return GetAsyncEnumerator(cancellationToken);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.AsEnumerable().GetEnumerator();
        }
    }

    public class FakeDbAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public FakeDbAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public T Current
        {
            get { return _inner.Current; }
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(_inner.MoveNext());
        }

        public ValueTask DisposeAsync()
        {
            _inner.Dispose();
            return new ValueTask(Task.CompletedTask);
        }
    }

    public abstract class FakeQueryProvider<T> : IOrderedQueryable<T>, IQueryProvider
    {
        private IEnumerable<T> _enumerable;

        protected FakeQueryProvider(Expression expression)
        {
            Expression = expression;
        }

        protected FakeQueryProvider(IEnumerable<T> enumerable)
        {
            _enumerable = enumerable;
            Expression = enumerable.AsQueryable().Expression;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            if (expression is MethodCallExpression m)
            {
                var resultType = m.Method.ReturnType; // it should be IQueryable<T>
                var tElement = resultType.GetGenericArguments().First();
                return (IQueryable) CreateInstance(tElement, expression);
            }

            return CreateQuery<T>(expression);
        }

        public IQueryable<TEntity> CreateQuery<TEntity>(Expression expression)
        {
            return (IQueryable<TEntity>) CreateInstance(typeof(TEntity), expression);
        }

        private object CreateInstance(Type tElement, Expression expression)
        {
            var queryType = GetType().GetGenericTypeDefinition().MakeGenericType(tElement);
            return Activator.CreateInstance(queryType, expression);
        }

        public object Execute(Expression expression)
        {
            return CompileExpressionItem<object>(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return CompileExpressionItem<TResult>(expression);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            if (_enumerable == null) _enumerable = CompileExpressionItem<IEnumerable<T>>(Expression);
            return _enumerable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (_enumerable == null) _enumerable = CompileExpressionItem<IEnumerable<T>>(Expression);
            return _enumerable.GetEnumerator();
        }

        public Type ElementType => typeof(T);

        public Expression Expression { get; }

        public IQueryProvider Provider => this;

        private static TResult CompileExpressionItem<TResult>(Expression expression)
        {
            var visitor = new FakeExpressionVisitor();
            var body = visitor.Visit(expression);
            var f = Expression.Lambda<Func<TResult>>(body ?? throw new InvalidOperationException(string.Format("{0} is null", nameof(body))), (IEnumerable<ParameterExpression>) null);
            return f.Compile()();
        }
    }

    public class FakeExpressionVisitor : ExpressionVisitor
    {
    }

    public class FakeDatabaseFacade : DatabaseFacade
    {
        public FakeDatabaseFacade(DbContext context) : base(context)
        {
        }

        public override bool EnsureCreated()
        {
            return true;
        }

        public override Task<bool> EnsureCreatedAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(EnsureCreated());
        }

        public override bool EnsureDeleted()
        {
            return true;
        }

        public override Task<bool> EnsureDeletedAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(EnsureDeleted());
        }

        public override bool CanConnect()
        {
            return true;
        }

        public override Task<bool> CanConnectAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(CanConnect());
        }

        public override IDbContextTransaction BeginTransaction()
        {
            return new FakeDbContextTransaction();
        }

        public override Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(BeginTransaction());
        }

        public override void CommitTransaction()
        {
        }

        public override Task CommitTransactionAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public override void RollbackTransaction()
        {
        }

        public override Task RollbackTransactionAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.CompletedTask;
        }

        public override IExecutionStrategy CreateExecutionStrategy()
        {
            return null;
        }

        public override string ToString()
        {
            return string.Empty;
        }
    }

    public class FakeDbContextTransaction : IDbContextTransaction
    {
        public Guid TransactionId => Guid.NewGuid();
        public void Commit() { }
        public void Rollback() { }
        public Task CommitAsync(CancellationToken cancellationToken = new CancellationToken()) => Task.CompletedTask;
        public Task RollbackAsync(CancellationToken cancellationToken = new CancellationToken()) => Task.CompletedTask;
        public void Dispose() { }
        public ValueTask DisposeAsync() => default;
    }
}
