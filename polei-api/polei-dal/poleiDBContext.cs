using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace poleiDAL
{
    #region Database context interface


    public interface IPoleiDB : IDisposable
{
    DbSet<Category> Categories { get; set; } // Category
    DbSet<Item> Items { get; set; } // Item
    DbSet<NotificationSent> NotificationSents { get; set; } // NotificationSent
    DbSet<ScrapedItem> ScrapedItems { get; set; } // ScrapedItem
    DbSet<Site> Sites { get; set; } // Site
    DbSet<SiteItem> SiteItems { get; set; } // SiteItem
    DbSet<SubscribedItem> SubscribedItems { get; set; } // SubscribedItem
    DbSet<SubscribedUser> SubscribedUsers { get; set; } // SubscribedUser

    int SaveChanges();
    int SaveChanges(bool acceptAllChangesOnSuccess);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken));
    DatabaseFacade Database { get; }
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    string ToString();

    EntityEntry Add(object entity);
    EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;
    Task AddRangeAsync(params object[] entities);
    Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default);
    ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class;
    ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default);
    void AddRange(IEnumerable<object> entities);
    void AddRange(params object[] entities);

    EntityEntry Attach(object entity);
    EntityEntry<TEntity> Attach<TEntity>(TEntity entity) where TEntity : class;
    void AttachRange(IEnumerable<object> entities);
    void AttachRange(params object[] entities);

    EntityEntry Entry(object entity);
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

    TEntity Find<TEntity>(params object[] keyValues) where TEntity : class;
    ValueTask<TEntity> FindAsync<TEntity>(object[] keyValues, CancellationToken cancellationToken) where TEntity : class;
    ValueTask<TEntity> FindAsync<TEntity>(params object[] keyValues) where TEntity : class;
    ValueTask<object> FindAsync(Type entityType, object[] keyValues, CancellationToken cancellationToken);
    ValueTask<object> FindAsync(Type entityType, params object[] keyValues);
    object Find(Type entityType, params object[] keyValues);

    EntityEntry Remove(object entity);
    EntityEntry<TEntity> Remove<TEntity>(TEntity entity) where TEntity : class;
    void RemoveRange(IEnumerable<object> entities);
    void RemoveRange(params object[] entities);

    EntityEntry Update(object entity);
    EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class;
    void UpdateRange(IEnumerable<object> entities);
    void UpdateRange(params object[] entities);

    IQueryable<TResult> FromExpression<TResult>(Expression<Func<IQueryable<TResult>>> expression);
}

#endregion

#region Database context

public class PoleiDB : DbContext, IPoleiDB
{
    public PoleiDB()
    {
    }

    public PoleiDB(DbContextOptions<PoleiDB> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; } // Category
    public DbSet<Item> Items { get; set; } // Item
    public DbSet<NotificationSent> NotificationSents { get; set; } // NotificationSent
    public DbSet<ScrapedItem> ScrapedItems { get; set; } // ScrapedItem
    public DbSet<Site> Sites { get; set; } // Site
    public DbSet<SiteItem> SiteItems { get; set; } // SiteItem
    public DbSet<SubscribedItem> SubscribedItems { get; set; } // SubscribedItem
    public DbSet<SubscribedUser> SubscribedUsers { get; set; } // SubscribedUser

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(//insert connection string here);
        }
    }

    public bool IsSqlParameterNull(SqlParameter param)
    {
        var sqlValue = param.SqlValue;
        var nullableValue = sqlValue as INullable;
        if (nullableValue != null)
            return nullableValue.IsNull;
        return (sqlValue == null || sqlValue == DBNull.Value);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ItemConfiguration());
        modelBuilder.ApplyConfiguration(new NotificationSentConfiguration());
        modelBuilder.ApplyConfiguration(new ScrapedItemConfiguration());
        modelBuilder.ApplyConfiguration(new SiteConfiguration());
        modelBuilder.ApplyConfiguration(new SiteItemConfiguration());
        modelBuilder.ApplyConfiguration(new SubscribedItemConfiguration());
        modelBuilder.ApplyConfiguration(new SubscribedUserConfiguration());
    }

}

#endregion

#region Database context factory

public class PoleiDBFactory : IDesignTimeDbContextFactory<PoleiDB>
{
    public PoleiDB CreateDbContext(string[] args)
    {
        return new PoleiDB();
    }
}

#endregion

#region Fake Database context


public class FakePoleiDB : IPoleiDB
{
    public DbSet<Category> Categories { get; set; } // Category
    public DbSet<Item> Items { get; set; } // Item
    public DbSet<NotificationSent> NotificationSents { get; set; } // NotificationSent
    public DbSet<ScrapedItem> ScrapedItems { get; set; } // ScrapedItem
    public DbSet<Site> Sites { get; set; } // Site
    public DbSet<SiteItem> SiteItems { get; set; } // SiteItem
    public DbSet<SubscribedItem> SubscribedItems { get; set; } // SubscribedItem
    public DbSet<SubscribedUser> SubscribedUsers { get; set; } // SubscribedUser

    public FakePoleiDB()
    {
        _database = new FakeDatabaseFacade(new PoleiDB());

        Categories = new FakeDbSet<Category>("CategoryId");
        Items = new FakeDbSet<Item>("ItemId");
        NotificationSents = new FakeDbSet<NotificationSent>("NotificationSentId");
        ScrapedItems = new FakeDbSet<ScrapedItem>("ScrapedItemId");
        Sites = new FakeDbSet<Site>("SiteId");
        SiteItems = new FakeDbSet<SiteItem>("SiteItemId");
        SubscribedItems = new FakeDbSet<SubscribedItem>("SubscribedItemId");
        SubscribedUsers = new FakeDbSet<SubscribedUser>("UserId");

    }

    public int SaveChangesCount { get; private set; }
    public virtual int SaveChanges()
    {
        ++SaveChangesCount;
        return 1;
    }

    public virtual int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        return SaveChanges();
    }

    public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        ++SaveChangesCount;
        return Task<int>.Factory.StartNew(() => 1, cancellationToken);
    }
    public virtual Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken)
    {
        ++SaveChangesCount;
        return Task<int>.Factory.StartNew(x => 1, acceptAllChangesOnSuccess, cancellationToken);
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
    }

    private DatabaseFacade _database;
    public DatabaseFacade Database { get { return _database; } }

    public DbSet<TEntity> Set<TEntity>() where TEntity : class
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }

    public virtual EntityEntry Add(object entity)
    {
        throw new NotImplementedException();
    }

    public virtual EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class
    {
        throw new NotImplementedException();
    }

    public virtual Task AddRangeAsync(params object[] entities)
    {
        throw new NotImplementedException();
    }

    public virtual async Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    public virtual async ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : class
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    public virtual async ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    public virtual void AddRange(IEnumerable<object> entities)
    {
        throw new NotImplementedException();
    }

    public virtual void AddRange(params object[] entities)
    {
        throw new NotImplementedException();
    }

    public virtual EntityEntry Attach(object entity)
    {
        throw new NotImplementedException();
    }

    public virtual EntityEntry<TEntity> Attach<TEntity>(TEntity entity) where TEntity : class
    {
        throw new NotImplementedException();
    }

    public virtual void AttachRange(IEnumerable<object> entities)
    {
        throw new NotImplementedException();
    }

    public virtual void AttachRange(params object[] entities)
    {
        throw new NotImplementedException();
    }

    public virtual EntityEntry Entry(object entity)
    {
        throw new NotImplementedException();
    }

    public virtual EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
    {
        throw new NotImplementedException();
    }

    public virtual TEntity Find<TEntity>(params object[] keyValues) where TEntity : class
    {
        throw new NotImplementedException();
    }

    public virtual ValueTask<TEntity> FindAsync<TEntity>(object[] keyValues, CancellationToken cancellationToken) where TEntity : class
    {
        throw new NotImplementedException();
    }

    public virtual ValueTask<TEntity> FindAsync<TEntity>(params object[] keyValues) where TEntity : class
    {
        throw new NotImplementedException();
    }

    public virtual ValueTask<object> FindAsync(Type entityType, object[] keyValues, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public virtual ValueTask<object> FindAsync(Type entityType, params object[] keyValues)
    {
        throw new NotImplementedException();
    }

    public virtual object Find(Type entityType, params object[] keyValues)
    {
        throw new NotImplementedException();
    }

    public virtual EntityEntry Remove(object entity)
    {
        throw new NotImplementedException();
    }

    public virtual EntityEntry<TEntity> Remove<TEntity>(TEntity entity) where TEntity : class
    {
        throw new NotImplementedException();
    }

    public virtual void RemoveRange(IEnumerable<object> entities)
    {
        throw new NotImplementedException();
    }

    public virtual void RemoveRange(params object[] entities)
    {
        throw new NotImplementedException();
    }

    public virtual EntityEntry Update(object entity)
    {
        throw new NotImplementedException();
    }

    public virtual EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class
    {
        throw new NotImplementedException();
    }

    public virtual void UpdateRange(IEnumerable<object> entities)
    {
        throw new NotImplementedException();
    }

    public virtual void UpdateRange(params object[] entities)
    {
        throw new NotImplementedException();
    }

    public virtual IQueryable<TResult> FromExpression<TResult>(Expression<Func<IQueryable<TResult>>> expression)
    {
        throw new NotImplementedException();
    }

}

#endregion

#region Fake DbSet

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
        _data = new ObservableCollection<TEntity>();
        _query = _data.AsQueryable();
    }

    public FakeDbSet(params string[] primaryKeys)
    {
        _primaryKeys = typeof(TEntity).GetProperties().Where(x => primaryKeys.Contains(x.Name)).ToArray();
        _data = new ObservableCollection<TEntity>();
        _query = _data.AsQueryable();
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
        var array = entities.ToArray(); RemoveRange(array);
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
        _data = new ObservableCollection<TEntity>();
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

        return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))
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
            return (IQueryable)CreateInstance(tElement, expression);
        }

        return CreateQuery<T>(expression);
    }

    public IQueryable<TEntity> CreateQuery<TEntity>(Expression expression)
    {
        return (IQueryable<TEntity>)CreateInstance(typeof(TEntity), expression);
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
        var f = Expression.Lambda<Func<TResult>>(body ?? throw new InvalidOperationException(string.Format("{0} is null", nameof(body))), (IEnumerable<ParameterExpression>)null);
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

#endregion

#region POCO classes

// Category
public class Category
{
    public int CategoryId { get; set; } // CategoryID (Primary key)
    public int? ParentCategoryId { get; set; } // ParentCategoryID
    public string CategoryName { get; set; } // CategoryName (length: 100)
    public string CategoryDescription { get; set; } // CategoryDescription (length: 200)

    // Reverse navigation

    /// <summary>
    /// Child Categories where [Category].[ParentCategoryID] point to this entity (FK_Category_Category)
    /// </summary>
    public virtual ICollection<Category> Categories { get; set; } // Category.FK_Category_Category

    /// <summary>
    /// Child Items where [Item].[CategoryID] point to this entity (FK_Item_Category)
    /// </summary>
    public virtual ICollection<Item> Items { get; set; } // Item.FK_Item_Category

    // Foreign keys

    /// <summary>
    /// Parent Category pointed by [Category].([ParentCategoryId]) (FK_Category_Category)
    /// </summary>
    public virtual Category ParentCategory { get; set; } // FK_Category_Category

    public Category()
    {
        Categories = new List<Category>();
        Items = new List<Item>();
    }
}

// Item
public class Item
{
    public int ItemId { get; set; } // ItemID (Primary key)
    public int CategoryId { get; set; } // CategoryID
    public string ItemName { get; set; } // ItemName (length: 200)
    public string ItemPhotoUrl { get; set; } // ItemPhotoURL (length: 300)
    public string ItemDescription { get; set; } // ItemDescription
    public string ItemBarcode { get; set; } // ItemBarcode (length: 100)

    // Reverse navigation

    /// <summary>
    /// Child SiteItems where [SiteItem].[ItemID] point to this entity (FK_SiteItem_Item)
    /// </summary>
    public virtual ICollection<SiteItem> SiteItems { get; set; } // SiteItem.FK_SiteItem_Item

    /// <summary>
    /// Child SubscribedItems where [SubscribedItem].[ItemID] point to this entity (FK_SubscribedItem_Item)
    /// </summary>
    public virtual ICollection<SubscribedItem> SubscribedItems { get; set; } // SubscribedItem.FK_SubscribedItem_Item

    // Foreign keys

    /// <summary>
    /// Parent Category pointed by [Item].([CategoryId]) (FK_Item_Category)
    /// </summary>
    public virtual Category Category { get; set; } // FK_Item_Category

    public Item()
    {
        SiteItems = new List<SiteItem>();
        SubscribedItems = new List<SubscribedItem>();
    }
}

// NotificationSent
public class NotificationSent
{
    public int NotificationSentId { get; set; } // NotificationSentID (Primary key)
    public int UserId { get; set; } // UserID
    public DateTime SentDateTIme { get; set; } // SentDateTIme
    public string SentAddress { get; set; } // SentAddress (length: 100)
    public int ScrapedItemId { get; set; } // ScrapedItemID
    public decimal NotifiedPrice { get; set; } // NotifiedPrice

    // Foreign keys

    /// <summary>
    /// Parent ScrapedItem pointed by [NotificationSent].([ScrapedItemId]) (FK_NotificationSent_ScrapedItem)
    /// </summary>
    public virtual ScrapedItem ScrapedItem { get; set; } // FK_NotificationSent_ScrapedItem

    /// <summary>
    /// Parent SubscribedUser pointed by [NotificationSent].([UserId]) (FK_NotificationSent_SubscribedUser)
    /// </summary>
    public virtual SubscribedUser SubscribedUser { get; set; } // FK_NotificationSent_SubscribedUser
}

// ScrapedItem
public class ScrapedItem
{
    public int ScrapedItemId { get; set; } // ScrapedItemID (Primary key)
    public int SiteItemId { get; set; } // SiteItemID
    public decimal PriceIncVat { get; set; } // PriceIncVAT
    public decimal? PriceExVat { get; set; } // PriceExVAT
    public DateTime ScrapedDateTime { get; set; } // ScrapedDateTime

    // Reverse navigation

    /// <summary>
    /// Child NotificationSents where [NotificationSent].[ScrapedItemID] point to this entity (FK_NotificationSent_ScrapedItem)
    /// </summary>
    public virtual ICollection<NotificationSent> NotificationSents { get; set; } // NotificationSent.FK_NotificationSent_ScrapedItem

    // Foreign keys

    /// <summary>
    /// Parent SiteItem pointed by [ScrapedItem].([SiteItemId]) (FK_ScrapedItem_SiteItem)
    /// </summary>
    public virtual SiteItem SiteItem { get; set; } // FK_ScrapedItem_SiteItem

    public ScrapedItem()
    {
        NotificationSents = new List<NotificationSent>();
    }
}

// Site
public class Site
{
    public int SiteId { get; set; } // SiteID (Primary key)
    public string SiteName { get; set; } // SiteName (length: 200)
    public string SiteHomeUrl { get; set; } // SiteHomeURL (length: 200)
    public string SiteLogoUrl { get; set; } // SiteLogoURL (length: 200)

    // Reverse navigation

    /// <summary>
    /// Child SiteItems where [SiteItem].[SiteID] point to this entity (FK_SiteItem_Site)
    /// </summary>
    public virtual ICollection<SiteItem> SiteItems { get; set; } // SiteItem.FK_SiteItem_Site

    public Site()
    {
        SiteItems = new List<SiteItem>();
    }
}

// SiteItem
public class SiteItem
{
    public int SiteItemId { get; set; } // SiteItemID (Primary key)
    public int SiteId { get; set; } // SiteID
    public int ItemId { get; set; } // ItemID
    public string ItemUrl { get; set; } // ItemURL (length: 300)

    // Reverse navigation

    /// <summary>
    /// Child ScrapedItems where [ScrapedItem].[SiteItemID] point to this entity (FK_ScrapedItem_SiteItem)
    /// </summary>
    public virtual ICollection<ScrapedItem> ScrapedItems { get; set; } // ScrapedItem.FK_ScrapedItem_SiteItem

    // Foreign keys

    /// <summary>
    /// Parent Item pointed by [SiteItem].([ItemId]) (FK_SiteItem_Item)
    /// </summary>
    public virtual Item Item { get; set; } // FK_SiteItem_Item

    /// <summary>
    /// Parent Site pointed by [SiteItem].([SiteId]) (FK_SiteItem_Site)
    /// </summary>
    public virtual Site Site { get; set; } // FK_SiteItem_Site

    public SiteItem()
    {
        ScrapedItems = new List<ScrapedItem>();
    }
}

// SubscribedItem
public class SubscribedItem
{
    public int SubscribedItemId { get; set; } // SubscribedItemID (Primary key)
    public int UserId { get; set; } // UserID
    public int ItemId { get; set; } // ItemID
    public decimal NotificationThreshold { get; set; } // NotificationThreshold

    // Foreign keys

    /// <summary>
    /// Parent Item pointed by [SubscribedItem].([ItemId]) (FK_SubscribedItem_Item)
    /// </summary>
    public virtual Item Item { get; set; } // FK_SubscribedItem_Item

    /// <summary>
    /// Parent SubscribedUser pointed by [SubscribedItem].([UserId]) (FK_SubscribedItem_SubscribedUser)
    /// </summary>
    public virtual SubscribedUser SubscribedUser { get; set; } // FK_SubscribedItem_SubscribedUser
}

// SubscribedUser
public class SubscribedUser
{
    public int UserId { get; set; } // UserID (Primary key)
    public string EmailAddress { get; set; } // EmailAddress (length: 100)
    public string FirstName { get; set; } // FirstName (length: 100)
    public string Surname { get; set; } // Surname (length: 100)
    public DateTime SubscribedDate { get; set; } // SubscribedDate
    public DateTime? VerifiedDate { get; set; } // VerifiedDate
    public string CellNumber { get; set; } // CellNumber (length: 20)

    // Reverse navigation

    /// <summary>
    /// Child NotificationSents where [NotificationSent].[UserID] point to this entity (FK_NotificationSent_SubscribedUser)
    /// </summary>
    public virtual ICollection<NotificationSent> NotificationSents { get; set; } // NotificationSent.FK_NotificationSent_SubscribedUser

    /// <summary>
    /// Child SubscribedItems where [SubscribedItem].[UserID] point to this entity (FK_SubscribedItem_SubscribedUser)
    /// </summary>
    public virtual ICollection<SubscribedItem> SubscribedItems { get; set; } // SubscribedItem.FK_SubscribedItem_SubscribedUser

    public SubscribedUser()
    {
        NotificationSents = new List<NotificationSent>();
        SubscribedItems = new List<SubscribedItem>();
    }
}


#endregion

#region POCO Configuration

// Category
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Category", "dbo");
        builder.HasKey(x => x.CategoryId).HasName("PK_Category").IsClustered();

        builder.Property(x => x.CategoryId).HasColumnName(@"CategoryID").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
        builder.Property(x => x.ParentCategoryId).HasColumnName(@"ParentCategoryID").HasColumnType("int").IsRequired(false);
        builder.Property(x => x.CategoryName).HasColumnName(@"CategoryName").HasColumnType("varchar(100)").IsRequired().IsUnicode(false).HasMaxLength(100);
        builder.Property(x => x.CategoryDescription).HasColumnName(@"CategoryDescription").HasColumnType("varchar(200)").IsRequired(false).IsUnicode(false).HasMaxLength(200);

        // Foreign keys
        builder.HasOne(a => a.ParentCategory).WithMany(b => b.Categories).HasForeignKey(c => c.ParentCategoryId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Category_Category");
    }
}

// Item
public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("Item", "dbo");
        builder.HasKey(x => x.ItemId).HasName("PK_Item").IsClustered();

        builder.Property(x => x.ItemId).HasColumnName(@"ItemID").HasColumnType("int").IsRequired().ValueGeneratedNever();
        builder.Property(x => x.CategoryId).HasColumnName(@"CategoryID").HasColumnType("int").IsRequired();
        builder.Property(x => x.ItemName).HasColumnName(@"ItemName").HasColumnType("varchar(200)").IsRequired().IsUnicode(false).HasMaxLength(200);
        builder.Property(x => x.ItemPhotoUrl).HasColumnName(@"ItemPhotoURL").HasColumnType("varchar(300)").IsRequired(false).IsUnicode(false).HasMaxLength(300);
        builder.Property(x => x.ItemDescription).HasColumnName(@"ItemDescription").HasColumnType("varchar(max)").IsRequired(false).IsUnicode(false);
        builder.Property(x => x.ItemBarcode).HasColumnName(@"ItemBarcode").HasColumnType("varchar(100)").IsRequired(false).IsUnicode(false).HasMaxLength(100);

        // Foreign keys
        builder.HasOne(a => a.Category).WithMany(b => b.Items).HasForeignKey(c => c.CategoryId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Item_Category");
    }
}

// NotificationSent
public class NotificationSentConfiguration : IEntityTypeConfiguration<NotificationSent>
{
    public void Configure(EntityTypeBuilder<NotificationSent> builder)
    {
        builder.ToTable("NotificationSent", "dbo");
        builder.HasKey(x => x.NotificationSentId).HasName("PK_NotificationSent").IsClustered();

        builder.Property(x => x.NotificationSentId).HasColumnName(@"NotificationSentID").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
        builder.Property(x => x.UserId).HasColumnName(@"UserID").HasColumnType("int").IsRequired();
        builder.Property(x => x.SentDateTIme).HasColumnName(@"SentDateTIme").HasColumnType("datetime").IsRequired();
        builder.Property(x => x.SentAddress).HasColumnName(@"SentAddress").HasColumnType("varchar(100)").IsRequired().IsUnicode(false).HasMaxLength(100);
        builder.Property(x => x.ScrapedItemId).HasColumnName(@"ScrapedItemID").HasColumnType("int").IsRequired();
        builder.Property(x => x.NotifiedPrice).HasColumnName(@"NotifiedPrice").HasColumnType("numeric(10,2)").HasPrecision(10, 2).IsRequired();

        // Foreign keys
        builder.HasOne(a => a.ScrapedItem).WithMany(b => b.NotificationSents).HasForeignKey(c => c.ScrapedItemId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_NotificationSent_ScrapedItem");
        builder.HasOne(a => a.SubscribedUser).WithMany(b => b.NotificationSents).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_NotificationSent_SubscribedUser");
    }
}

// ScrapedItem
public class ScrapedItemConfiguration : IEntityTypeConfiguration<ScrapedItem>
{
    public void Configure(EntityTypeBuilder<ScrapedItem> builder)
    {
        builder.ToTable("ScrapedItem", "dbo");
        builder.HasKey(x => x.ScrapedItemId).HasName("PK_ScrapedItem").IsClustered();

        builder.Property(x => x.ScrapedItemId).HasColumnName(@"ScrapedItemID").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
        builder.Property(x => x.SiteItemId).HasColumnName(@"SiteItemID").HasColumnType("int").IsRequired();
        builder.Property(x => x.PriceIncVat).HasColumnName(@"PriceIncVAT").HasColumnType("numeric(10,2)").HasPrecision(10, 2).IsRequired();
        builder.Property(x => x.PriceExVat).HasColumnName(@"PriceExVAT").HasColumnType("numeric(10,2)").HasPrecision(10, 2).IsRequired(false);
        builder.Property(x => x.ScrapedDateTime).HasColumnName(@"ScrapedDateTime").HasColumnType("datetime").IsRequired();

        // Foreign keys
        builder.HasOne(a => a.SiteItem).WithMany(b => b.ScrapedItems).HasForeignKey(c => c.SiteItemId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ScrapedItem_SiteItem");
    }
}

// Site
public class SiteConfiguration : IEntityTypeConfiguration<Site>
{
    public void Configure(EntityTypeBuilder<Site> builder)
    {
        builder.ToTable("Site", "dbo");
        builder.HasKey(x => x.SiteId).HasName("PK_Site").IsClustered();

        builder.Property(x => x.SiteId).HasColumnName(@"SiteID").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
        builder.Property(x => x.SiteName).HasColumnName(@"SiteName").HasColumnType("varchar(200)").IsRequired().IsUnicode(false).HasMaxLength(200);
        builder.Property(x => x.SiteHomeUrl).HasColumnName(@"SiteHomeURL").HasColumnType("varchar(200)").IsRequired().IsUnicode(false).HasMaxLength(200);
        builder.Property(x => x.SiteLogoUrl).HasColumnName(@"SiteLogoURL").HasColumnType("varchar(200)").IsRequired(false).IsUnicode(false).HasMaxLength(200);
    }
}

// SiteItem
public class SiteItemConfiguration : IEntityTypeConfiguration<SiteItem>
{
    public void Configure(EntityTypeBuilder<SiteItem> builder)
    {
        builder.ToTable("SiteItem", "dbo");
        builder.HasKey(x => x.SiteItemId).HasName("PK_SiteItem").IsClustered();

        builder.Property(x => x.SiteItemId).HasColumnName(@"SiteItemID").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
        builder.Property(x => x.SiteId).HasColumnName(@"SiteID").HasColumnType("int").IsRequired();
        builder.Property(x => x.ItemId).HasColumnName(@"ItemID").HasColumnType("int").IsRequired();
        builder.Property(x => x.ItemUrl).HasColumnName(@"ItemURL").HasColumnType("varchar(300)").IsRequired().IsUnicode(false).HasMaxLength(300);

        // Foreign keys
        builder.HasOne(a => a.Item).WithMany(b => b.SiteItems).HasForeignKey(c => c.ItemId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_SiteItem_Item");
        builder.HasOne(a => a.Site).WithMany(b => b.SiteItems).HasForeignKey(c => c.SiteId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_SiteItem_Site");
    }
}

// SubscribedItem
public class SubscribedItemConfiguration : IEntityTypeConfiguration<SubscribedItem>
{
    public void Configure(EntityTypeBuilder<SubscribedItem> builder)
    {
        builder.ToTable("SubscribedItem", "dbo");
        builder.HasKey(x => x.SubscribedItemId).HasName("PK_SubscribedItem").IsClustered();

        builder.Property(x => x.SubscribedItemId).HasColumnName(@"SubscribedItemID").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
        builder.Property(x => x.UserId).HasColumnName(@"UserID").HasColumnType("int").IsRequired();
        builder.Property(x => x.ItemId).HasColumnName(@"ItemID").HasColumnType("int").IsRequired();
        builder.Property(x => x.NotificationThreshold).HasColumnName(@"NotificationThreshold").HasColumnType("numeric(10,2)").HasPrecision(10, 2).IsRequired();

        // Foreign keys
        builder.HasOne(a => a.Item).WithMany(b => b.SubscribedItems).HasForeignKey(c => c.ItemId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_SubscribedItem_Item");
        builder.HasOne(a => a.SubscribedUser).WithMany(b => b.SubscribedItems).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_SubscribedItem_SubscribedUser");
    }
}

// SubscribedUser
public class SubscribedUserConfiguration : IEntityTypeConfiguration<SubscribedUser>
{
    public void Configure(EntityTypeBuilder<SubscribedUser> builder)
    {
        builder.ToTable("SubscribedUser", "dbo");
        builder.HasKey(x => x.UserId).HasName("PK_SubscribedUser").IsClustered();

        builder.Property(x => x.UserId).HasColumnName(@"UserID").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
        builder.Property(x => x.EmailAddress).HasColumnName(@"EmailAddress").HasColumnType("varchar(100)").IsRequired().IsUnicode(false).HasMaxLength(100);
        builder.Property(x => x.FirstName).HasColumnName(@"FirstName").HasColumnType("varchar(100)").IsRequired(false).IsUnicode(false).HasMaxLength(100);
        builder.Property(x => x.Surname).HasColumnName(@"Surname").HasColumnType("varchar(100)").IsRequired(false).IsUnicode(false).HasMaxLength(100);
        builder.Property(x => x.SubscribedDate).HasColumnName(@"SubscribedDate").HasColumnType("datetime").IsRequired();
        builder.Property(x => x.VerifiedDate).HasColumnName(@"VerifiedDate").HasColumnType("datetime").IsRequired(false);
        builder.Property(x => x.CellNumber).HasColumnName(@"CellNumber").HasColumnType("varchar(20)").IsRequired(false).IsUnicode(false).HasMaxLength(20);
    }
}


#endregion

}
// </auto-generated>
