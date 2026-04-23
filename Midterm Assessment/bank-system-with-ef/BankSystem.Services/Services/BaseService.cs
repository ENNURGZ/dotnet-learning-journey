using BankSystem.EF.Entities;

namespace BankSystem.Services.Services;

public abstract class BaseService : IDisposable
{
    private bool disposed;

    protected BaseService(BankContext context)
    {
        this.Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    protected BankContext Context { get; }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                this.Context.Dispose();
            }

            this.disposed = true;
        }
    }
}
