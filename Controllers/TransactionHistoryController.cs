using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickPay.Data;
using QuickPay.Models.DTO;
using QuickPay.Services.Interface;
using System.Security.Claims;

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

        [HttpGet("History/{pageNumber},{pageSize}")]
        public async Task<IActionResult> GetTransactionHistory(int pageNumber = 1,int pageSize = 10)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
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
