using Business.Models;

namespace Business.Interfaces;

public interface IReceiptService : IModelService<ReceiptModel>
{
    Task AddProductAsync(int productId, int receiptId, int quantity);

    Task RemoveProductAsync(int productId, int receiptId, int quantity);

    Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(int receiptId);

    Task<decimal> ToPayAsync(int receiptId);

    Task CheckOutAsync(int receiptId);

    Task<IEnumerable<ReceiptModel>> GetReceiptsByPeriodAsync(DateTime startDate, DateTime endDate);
}
