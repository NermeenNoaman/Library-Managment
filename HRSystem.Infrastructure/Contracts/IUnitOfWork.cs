namespace HRSystem.Infrastructure.Contracts;

public interface IUnitOfWork : IDisposable
{
    ICategoryRepository Categories { get; }
    // IBookRepository Books { get; }
    // ...

    Task<int> CompleteAsync();
}