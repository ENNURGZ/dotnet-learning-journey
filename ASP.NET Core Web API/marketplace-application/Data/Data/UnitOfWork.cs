using System.Threading.Tasks;
using Data.Interfaces;
using Data.Repositories;

namespace Data.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly TradeMarketDbContext context;

    private ICustomerRepository? customerRepository;
    private IPersonRepository? personRepository;
    private IProductRepository? productRepository;
    private ICategoryRepository? categoryRepository;
    private IReceiptRepository? receiptRepository;
    private IReceiptDetailRepository? receiptDetailRepository;

    public UnitOfWork(TradeMarketDbContext context)
    {
        this.context = context;
    }

    public ICustomerRepository CustomerRepository =>
        this.customerRepository ??= new CustomerRepository(this.context);

    public IPersonRepository PersonRepository =>
        this.personRepository ??= new PersonRepository(this.context);

    public IProductRepository ProductRepository =>
        this.productRepository ??= new ProductRepository(this.context);

    public ICategoryRepository CategoryRepository =>
        this.categoryRepository ??= new CategoryRepository(this.context);

    public IReceiptRepository ReceiptRepository =>
        this.receiptRepository ??= new ReceiptRepository(this.context);

    public IReceiptDetailRepository ReceiptDetailRepository =>
        this.receiptDetailRepository ??= new ReceiptDetailRepository(this.context);

    public async Task SaveAsync()
    {
        await this.context.SaveChangesAsync();
    }
}