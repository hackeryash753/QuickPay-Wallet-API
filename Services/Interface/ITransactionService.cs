using QuickPay.Models.DTO;

namespace QuickPay.Services.Interface
{
    public interface ITransactionService
    {

        Task<List<TransactionHistoryDto>> TransactionHistoryAsync(int userId, int pageNumber, int pageSize);
    }
}
