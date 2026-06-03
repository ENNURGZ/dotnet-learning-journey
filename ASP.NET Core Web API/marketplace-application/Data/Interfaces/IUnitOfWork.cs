namespace Data.Interfaces;

public interface IUnitOfWork
{
    ICustomerRepository CustomerRepository { get; }

    IPersonRepository PersonRepository { get; }

    IProductRepository ProductRepository { get; }

    ICategoryRepository CategoryRepository { get; }

    IReceiptRepository ReceiptRepository { get; }

    IReceiptDetailRepository ReceiptDetailRepository { get; }

    Task SaveAsync();
}
