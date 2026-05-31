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
    public class AddMoneyController : ControllerBase
    {
        public readonly IWalletService walletService;
        public AddMoneyController(IWalletService walletService)
        {
           this.walletService = walletService;   
        }


        [HttpPost("AddMoney")]
        public async Task<IActionResult> AddMoney(AddMoneyDto addMoneyDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await walletService.AddMoneyAsync(addMoneyDto, userId);
            
            return Ok(new ApiResponeDto<AddMoneyResponeDto>
            { 
                 Success = true,
                 Message = "Money added successfully",
                 Data = result

            });

        }
    }
}
