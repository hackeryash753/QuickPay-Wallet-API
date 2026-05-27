using Microsoft.EntityFrameworkCore;
using QuickPay.Data;
using QuickPay.Exceptions;
using QuickPay.Models.DTO;
using QuickPay.Services.Interface;
using System.Security.Cryptography.Xml;

namespace QuickPay.Services
{
    public class TransactionService : ITransactionService
    {

        public readonly QuickPayDbContext dbContext;
        public TransactionService(QuickPayDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<TransactionHistoryDto>> TransactionHistoryAsync(int userId,int pageNumber,int pageSize)
        {
            var wallet = await dbContext.Wallets
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wallet == null)
            {
                throw new NotFoundException("Wallet not found for the specified user.");
            }

            var transactions = await dbContext.Transactions
                .Where(t =>
                    t.SenderWalletId == wallet.Id ||
                    t.ReceiverWalletId == wallet.Id)
                .OrderByDescending(t => t.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TransactionHistoryDto
                {
                    Amount = t.Amount,
                    Type = t.Type,
                    Status = t.Status,
                    CreatedAt = t.CreatedAt,
                    ReceiverWalletId = t.ReceiverWalletId,
                    SenderWalletId = t.SenderWalletId
                })
                .ToListAsync();

            return transactions;
        
        }
    }
}
