using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickPay.Data;
using QuickPay.Models.DTO;
using QuickPay.Services.Interface;

namespace QuickPay.Controllers
{
    [ApiController]
    public class TransactionHistoryController : ControllerBase
    {
        public readonly ITransactionService transactionService;
        public TransactionHistoryController(ITransactionService transactionService)
        {
           this.transactionService = transactionService;
        }

        [HttpGet("History/{userId},{pageNumber},{pageSize}")]
        public async Task<IActionResult> GetTransactionHistory(int userId,int pageNumber = 1,int pageSize = 10)
        {
            var Transactions = await transactionService.TransactionHistoryAsync(userId, pageNumber,pageSize);
            return Ok(new ApiResponeDto<List<TransactionHistoryDto>>
            {
                Success = true,
                Message = "Transaction history retrieved successfully",
                Data = Transactions
            });
        }
    }
}
