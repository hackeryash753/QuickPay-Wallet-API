using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickPay.Data;
using QuickPay.Models.DTO;
using QuickPay.Services.Interface;
using System.Security.Claims;

namespace QuickPay.Controllers
{
    [ApiController]
    [Authorize]
    public class SendMoneyController : ControllerBase
    {
        public readonly IWalletService walletService;

        public SendMoneyController(IWalletService walletService)
        {
            this.walletService = walletService;
        }

        [HttpPost("api/sendmoney")]

        public async Task<IActionResult> SendMoney(SendMoneyDto sendMoneyDto)
        {
            var senderId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await walletService.SendMoneyAsync(sendMoneyDto, senderId);

            return Ok(new ApiResponeDto<SendMoneyResponseDto>
            {
                Success = true,
                Message = "Money sent successfully",
                Data = result
            });

        }
          

    }
}
